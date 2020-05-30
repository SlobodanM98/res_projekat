﻿using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SHES
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection connection;
        string connectionString;

        public static int SnagaSunca;
        public static MaterialDesignThemes.Wpf.PackIconKind SlikaSnageSunca { get; set; }
        //public static BindingList<MaterialDesignThemes.Wpf.PackIconKind> SlikaSnageSunca { get; set; }

        public static BindingList<Baterija> Baterije { get; set; }
        public static BindingList<Potrosac> Potrosaci { get; set; }
        public static BindingList<SolarniPanel> SolarniPaneli { get; set; }
        public static BindingList<ElektricniAutomobil> ElektricniAutomobili { get; set; }

        public static List<Baterija> autoBaterije { get; set; }

        public static Punjac Punjac;

        public static int jednaSekundaJe;
        public static DateTime trenutnoVreme;

        public static Elektrodistribucija distribucija;

        public MainWindow()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SHES.Properties.Settings.BazaPodatakaConnectionString"].ConnectionString;
            Baterije = new BindingList<Baterija>();
            Potrosaci = new BindingList<Potrosac>();
            SolarniPaneli = new BindingList<SolarniPanel>();
            ElektricniAutomobili = new BindingList<ElektricniAutomobil>();
            autoBaterije = new List<Baterija>();
            SnagaSunca = 0;
            Punjac = new Punjac();
            jednaSekundaJe = int.Parse(ConfigurationManager.AppSettings["jednaSekundaJe"]);
            trenutnoVreme = DateTime.Now;
            distribucija = new Elektrodistribucija();

            UcitajUredjaje();

            InitializeComponent();

            vreme.Kind = MaterialDesignThemes.Wpf.PackIconKind.MoonAndStars;

            labelSnagaSunca.Content = SnagaSunca.ToString();
            labelSnagaSunca.Foreground = Brushes.Blue;
            labelSnagaRazmene.Content = distribucija.SnagaRazmene.ToString();
            labelCena.Content = distribucija.Cena.ToString();

            Thread pokreniServer = new Thread(() => PokreniServer());
            pokreniServer.IsBackground = true;
            pokreniServer.Start();
            Thread azuriranje = new Thread(() => Azuriranje());
            azuriranje.IsBackground = true;
            azuriranje.Start();

            DataContext = this;
        }

        void PokreniServer()
        {
            bool jestePokrenut = false;

            using (ServiceHost host = new ServiceHost(typeof(SimulatorServer)))
            {
                while (true)
                {
                    Thread.Sleep(100);
                    if (!jestePokrenut)
                    {
                        jestePokrenut = true;
                        host.Open();
                    }
                }
            }
        }

        void Azuriranje()
        {
            while (true)
            {
                trenutnoVreme = trenutnoVreme.AddSeconds(1);
                PodesiTrenutnoVreme();
                int StaraSnagaSunca = 0;
                double StaraSnagaRazmene = 0;
                double StaraCena = 0;
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    int.TryParse(labelSnagaSunca.Content.ToString(), out StaraSnagaSunca);
                    double.TryParse(labelSnagaRazmene.Content.ToString(), out StaraSnagaRazmene);
                    double.TryParse(labelCena.Content.ToString(), out StaraCena);
                });
                if (StaraSnagaSunca != SnagaSunca)
                {
                    PodesiSnaguSunca(SnagaSunca);
                }
                if(StaraSnagaRazmene != distribucija.SnagaRazmene)
                {
                    PodesiSnaguRazmene(distribucija.SnagaRazmene);
                }
                if(StaraCena != distribucija.Cena)
                {
                    PodesiCenu(distribucija.Cena);
                }
                Thread.Sleep(1000 / jednaSekundaJe);
            }
        }

        public void PodesiSnaguSunca(int novaVrednost)
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                labelSnagaSunca.Content = SnagaSunca.ToString();
                if(SnagaSunca == 0)
                {
                    labelSnagaSunca.Foreground = Brushes.Black;
                    vreme.Kind = MaterialDesignThemes.Wpf.PackIconKind.MoonAndStars;
                }

                else if (SnagaSunca > 0 && SnagaSunca < 20)
                {
                    labelSnagaSunca.Foreground = Brushes.Blue;
                    vreme.Kind = MaterialDesignThemes.Wpf.PackIconKind.WeatherCloudy;
                }
                else if (SnagaSunca >= 20 && SnagaSunca < 50)
                {
                    labelSnagaSunca.Foreground = Brushes.Green;
                    vreme.Kind = MaterialDesignThemes.Wpf.PackIconKind.WeatherPartlyCloudy;
                }
                else if (SnagaSunca >= 50 && SnagaSunca < 80)
                {
                    labelSnagaSunca.Foreground = Brushes.Orange;
                    vreme.Kind = MaterialDesignThemes.Wpf.PackIconKind.WeatherSunny;
                }
                else
                {
                    labelSnagaSunca.Foreground = Brushes.Red;
                    vreme.Kind = MaterialDesignThemes.Wpf.PackIconKind.WeatherSunnyAlert;
                }
            });
        }

        public void PodesiSnaguRazmene(double novaSnaga)
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                labelSnagaRazmene.Content = novaSnaga.ToString();
            });
        }

        public void PodesiCenu(double novaCena)
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                labelCena.Content = novaCena.ToString();
            });
        }

        public void PodesiTrenutnoVreme()
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                labelTrenutnoVreme.Content = trenutnoVreme.ToLongTimeString();
            });
        }

        void UcitajUredjaje()
        {
            string queryBaterije = "SELECT * FROM Baterije WHERE AutomobilJedinstvenoIme IS NULL";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(queryBaterije, connection))
                using(SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);

                for(int i = 0; i < table.Rows.Count; i++)
                {
                    string jedinstvenoIme = table.Rows[i]["JedinstvenoIme"].ToString();
                    double maksimalnaSnaga = double.Parse(table.Rows[i]["MaksimalnaSnaga"].ToString());
                    int kapacitet = int.Parse(table.Rows[i]["Kapacitet"].ToString());
                    Baterije.Add(new Baterija(jedinstvenoIme, maksimalnaSnaga, kapacitet));
                }
            }

            queryBaterije = "SELECT * FROM Baterije WHERE AutomobilJedinstvenoIme IS NOT NULL";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(queryBaterije, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string jedinstvenoIme = table.Rows[i]["JedinstvenoIme"].ToString();
                    double maksimalnaSnaga = double.Parse(table.Rows[i]["MaksimalnaSnaga"].ToString());
                    int kapacitet = int.Parse(table.Rows[i]["Kapacitet"].ToString());
                    string autoJedinstvenoIme = table.Rows[i]["AutomobilJedinstvenoIme"].ToString();
                    Baterija novaBaterija = new Baterija(jedinstvenoIme, maksimalnaSnaga, kapacitet);
                    novaBaterija.AutomobilJedinstvenoIme = autoJedinstvenoIme;
                    autoBaterije.Add(novaBaterija);
                }
            }

            string queryPotrosaci = "SELECT * FROM Potrosaci";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(queryPotrosaci, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string jedinstvenoIme = table.Rows[i]["JedinstvenoIme"].ToString();
                    double potrosnja = double.Parse(table.Rows[i]["Potrosnja"].ToString());
                    bool stanje = bool.Parse(table.Rows[i]["Upaljen"].ToString());
                    Potrosac novi = new Potrosac(jedinstvenoIme, potrosnja);
                    novi.Upaljen = stanje;
                    if(stanje)
                    {
                        novi.Slika = MaterialDesignThemes.Wpf.PackIconKind.PowerPlugOutline;
                    }
                    Potrosaci.Add(novi);
                }
            }

            string querySolarniPaneli = "SELECT * FROM SolarnePanele";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(querySolarniPaneli, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string jedinstvenoIme = table.Rows[i]["JedinstvenoIme"].ToString();
                    double maksimalnaSnaga = double.Parse(table.Rows[i]["MaksimalnaSnaga"].ToString());
                    SolarniPanel novi = new SolarniPanel(jedinstvenoIme, maksimalnaSnaga);
                    SolarniPaneli.Add(novi);
                }
            }

            string queryAutomobil = "SELECT * FROM Automobili";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(queryAutomobil, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string jedinstvenoIme = table.Rows[i]["JedinstvenoIme"].ToString();
                    bool naPunjacu = bool.Parse(table.Rows[i]["NaPunjacu"].ToString());
                    bool puniSe = bool.Parse(table.Rows[i]["Punise"].ToString());
                    Baterija baterija = autoBaterije.Find(b => b.AutomobilJedinstvenoIme.Equals(jedinstvenoIme));
                    ElektricniAutomobil noviAuto = new ElektricniAutomobil(baterija, jedinstvenoIme, naPunjacu, puniSe);
                    ElektricniAutomobili.Add(noviAuto);
                }
            }
        }
    }
}
