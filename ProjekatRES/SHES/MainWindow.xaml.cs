using Common;
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

        double cenovnik = 1000;

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
            labelSnagaSunca.Foreground = Brushes.Black;
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
            int pozoviBaterije = 0;
            bool puni = false;
            bool puni2 = false;
            while (true)
            {
                trenutnoVreme = trenutnoVreme.AddSeconds(1);
                PodesiTrenutnoVreme();

                double potrosnja = 0;
                int StaraSnagaSunca = 0;
                double StaraSnagaRazmene = 0;
                double StaraCena = 0;
                string puniSE = "";
                string staroIme = "";

                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    int.TryParse(labelSnagaSunca.Content.ToString(), out StaraSnagaSunca);
                    double.TryParse(labelSnagaRazmene.Content.ToString(), out StaraSnagaRazmene);
                    double.TryParse(labelCena.Content.ToString(), out StaraCena);
                    puniSE = labelPuniSe.Content.ToString();
                    staroIme = labelNaPunjacu.Content.ToString();
                });
                if(Punjac.Automobil != null)
                {
                    if (puni2 != Punjac.NaPunjacu)
                    {
                        if (Punjac.NaPunjacu == false)
                        {
                            PodesiPuniSe("");
                        }
                        else
                        {
                            PodesiPunjac(Punjac.Automobil.JedinstvenoIme);
                        }
                        puni2 = Punjac.NaPunjacu;
                    }
                    if(puni != Punjac.PuniSe)
                    {
                        if (Punjac.PuniSe == false)
                        {
                            PodesiPuniSe("");
                        }
                        else
                        {
                            PodesiPuniSe(Punjac.Automobil.JedinstvenoIme);
                        }
                        puni = Punjac.PuniSe;
                    }
                }
                else
                {
                    PodesiPunjac("");
                    puni = false;
                    PodesiPuniSe("");
                    puni2 = false;
                }

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

                if (pozoviBaterije == 60)
                {
                    pozoviBaterije = 0;
                }
                if (trenutnoVreme.Hour >= 3 && trenutnoVreme.Hour < 6)
                {
                    foreach (Baterija b in Baterije)
                    {
                        if(pozoviBaterije == 0 && b.TrenutniKapacitet <= b.Kapacitet)
                        {
                            if (b.TrenutniKapacitet + (b.MaksimalnaSnaga / 60) <= b.Kapacitet)
                            {
                                b.TrenutniKapacitet += b.MaksimalnaSnaga / 60;
                            }
                            else
                            {
                                b.TrenutniKapacitet = b.Kapacitet;
                            }
                        }
                        potrosnja += PunjenjeBaterije(b, false);
                    }

                }else if(trenutnoVreme.Hour >= 14 && trenutnoVreme.Hour < 17)
                {
                    foreach (Baterija b in Baterije)
                    {
                        if(pozoviBaterije == 0 && b.TrenutniKapacitet >= 0)
                        {
                            if (b.TrenutniKapacitet - (b.MaksimalnaSnaga / 60) >= 0)
                            {
                                b.TrenutniKapacitet -= b.MaksimalnaSnaga / 60;
                            }
                            else
                            {
                                b.TrenutniKapacitet = 0;
                            }
                        }
                        potrosnja -= PraznjenjeBaterije(b);
                    }
                }
                else
                {
                    foreach(Baterija b in Baterije)
                    {
                        ResetBaterije(b);
                    }
                }

                foreach(Potrosac p in Potrosaci)
                {
                    if (p.Upaljen)
                    {
                        potrosnja += p.Potrosnja / 3600;
                    }
                }

                if (Punjac.PuniSe)
                {
                    if(pozoviBaterije == 0 && Punjac.Automobil.BaterijaAuta.TrenutniKapacitet <= Punjac.Automobil.BaterijaAuta.Kapacitet)
                    {
                        if(Punjac.Automobil.BaterijaAuta.TrenutniKapacitet + (Punjac.Automobil.BaterijaAuta.MaksimalnaSnaga / 60) <= Punjac.Automobil.BaterijaAuta.Kapacitet)
                        {
                            Punjac.Automobil.BaterijaAuta.TrenutniKapacitet += Punjac.Automobil.BaterijaAuta.MaksimalnaSnaga / 60;
                        }
                        else
                        {
                            Punjac.Automobil.BaterijaAuta.TrenutniKapacitet = Punjac.Automobil.BaterijaAuta.Kapacitet;
                        }
                    }
                    potrosnja += PunjenjeBaterije(Punjac.Automobil.BaterijaAuta, true);
                }

                foreach(SolarniPanel s in SolarniPaneli)
                {
                    potrosnja -= ((s.MaksimalnaSnaga / 3600) / 100) * SnagaSunca;
                }

                distribucija.Cena += (cenovnik / 3600 * (potrosnja * (-1)));

                distribucija.SnagaRazmene += (potrosnja * (-1));

                pozoviBaterije++;

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

        public void PodesiPunjac(string ime)
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                labelNaPunjacu.Content = ime;
            });
        }

        public void PodesiPuniSe(string ime)
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                labelPuniSe.Content = ime;
            });
        }

        public void PodesiSnaguRazmene(double novaSnaga)
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                labelSnagaRazmene.Content = novaSnaga.ToString("N4") + "kw";
            });
        }

        public void PodesiCenu(double novaCena)
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                labelCena.Content =  novaCena.ToString("N4") + "$";
            });
        }

        public void PodesiTrenutnoVreme()
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                //labelTrenutnoVreme.Content = trenutnoVreme.ToLongTimeString();
                labelTrenutnoVreme.Content = trenutnoVreme.ToString("HH:mm:ss");
            });
        }

        public double PunjenjeBaterije(Baterija baterija, bool baterijaAuta)
        {
            string query;
            if (!baterija.PuniSe)
            {
                baterija.PuniSe = true;
                query = "UPDATE Baterije SET PuniSe=@up  WHERE JedinstvenoIme = '" + baterija.JedinstvenoIme + "'";

                using (connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@up", SqlDbType.Bit).Value = true;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            //if(baterija.Kapacitet != 180)
              //  baterija.Kapacitet++;

            if (baterijaAuta == true)
            {
                if (baterija.TrenutniKapacitet == 0)
                {
                    Punjac.Automobil.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery0;
                }
                else if (baterija.TrenutniKapacitet > 0 && baterija.TrenutniKapacitet <= baterija.Kapacitet * 20 / 100)
                {
                    Punjac.Automobil.Slika = MaterialDesignThemes.Wpf.PackIconKind.BatteryCharging10;
                }
                else if (baterija.TrenutniKapacitet > baterija.Kapacitet * 20 / 100 && baterija.TrenutniKapacitet <= baterija.Kapacitet * 40 / 100)
                {
                    Punjac.Automobil.Slika = MaterialDesignThemes.Wpf.PackIconKind.BatteryCharging30;
                }
                else if (baterija.TrenutniKapacitet > baterija.Kapacitet * 40 / 100 && baterija.TrenutniKapacitet <= baterija.Kapacitet * 60 / 100)
                {
                    Punjac.Automobil.Slika = MaterialDesignThemes.Wpf.PackIconKind.BatteryCharging50;
                }
                else if (baterija.TrenutniKapacitet > baterija.Kapacitet * 60 / 100 && baterija.TrenutniKapacitet <= baterija.Kapacitet * 80 / 100)
                {
                    Punjac.Automobil.Slika = MaterialDesignThemes.Wpf.PackIconKind.BatteryCharging70;
                }
                else if (baterija.TrenutniKapacitet > baterija.Kapacitet * 80 / 100 && baterija.TrenutniKapacitet <= baterija.Kapacitet * 95 / 100)
                {
                    Punjac.Automobil.Slika = MaterialDesignThemes.Wpf.PackIconKind.BatteryCharging90;
                }
                else
                {
                    Punjac.Automobil.Slika = MaterialDesignThemes.Wpf.PackIconKind.BatteryCharging100;
                }
            }
            if (baterija.TrenutniKapacitet == 0)
            {
                baterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery0;
            }
            else if (baterija.TrenutniKapacitet > 0 && baterija.TrenutniKapacitet <= baterija.Kapacitet * 20 / 100)
            {
                baterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.BatteryCharging10;
            }
            else if (baterija.TrenutniKapacitet > baterija.Kapacitet * 20 / 100 && baterija.TrenutniKapacitet <= baterija.Kapacitet * 40 / 100)
            {
                baterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.BatteryCharging30;
            }
            else if (baterija.TrenutniKapacitet > baterija.Kapacitet * 40 / 100 && baterija.TrenutniKapacitet <= baterija.Kapacitet * 60 / 100)
            {
                baterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.BatteryCharging50;
            }
            else if (baterija.TrenutniKapacitet > baterija.Kapacitet * 60 / 100 && baterija.TrenutniKapacitet <= baterija.Kapacitet * 80 / 100)
            {
                baterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.BatteryCharging70;
            }
            else if (baterija.TrenutniKapacitet > baterija.Kapacitet * 80 / 100 && baterija.TrenutniKapacitet <= baterija.Kapacitet * 95 / 100)
            {
                baterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.BatteryCharging90;
            }
            else
            {
                baterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.BatteryCharging100;
            }

            query = $"UPDATE Baterije SET TrenutniKapacitet={baterija.TrenutniKapacitet}  WHERE JedinstvenoIme = '" + baterija.JedinstvenoIme + "'";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            return baterija.MaksimalnaSnaga / 3600;
        }

        public double PraznjenjeBaterije(Baterija baterija)
        {
            string query;
            if (!baterija.PrazniSe)
            {
                baterija.PrazniSe = true;
                query = "UPDATE Baterije SET PrazniSe=@up  WHERE JedinstvenoIme = '" + baterija.JedinstvenoIme + "'";

                using (connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@up", SqlDbType.Bit).Value = true;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            if (baterija.TrenutniKapacitet >= 0)//---------------------------------------------------
            {
                //baterija.Kapacitet--;
                if (baterija.TrenutniKapacitet == 0)
                {
                    baterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery0;
                }
                else if (baterija.TrenutniKapacitet > 0 && baterija.TrenutniKapacitet <= baterija.Kapacitet * 20 / 100)
                {
                    baterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery10;
                }
                else if (baterija.TrenutniKapacitet > baterija.Kapacitet * 20 / 100 && baterija.TrenutniKapacitet <= baterija.Kapacitet * 40 / 100)
                {
                    baterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery30;
                }
                else if (baterija.TrenutniKapacitet > baterija.Kapacitet * 40 / 100 && baterija.TrenutniKapacitet <= baterija.Kapacitet * 60 / 100)
                {
                    baterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery50;
                }
                else if (baterija.TrenutniKapacitet > baterija.Kapacitet * 60 / 100 && baterija.TrenutniKapacitet <= baterija.Kapacitet * 80 / 100)
                {
                    baterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery70;
                }
                else if (baterija.TrenutniKapacitet > baterija.Kapacitet * 80 / 100 && baterija.TrenutniKapacitet <= baterija.Kapacitet * 95 / 100)
                {
                    baterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery90;
                }
                else
                {
                    baterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery100;
                }

                query = $"UPDATE Baterije SET TrenutniKapacitet={baterija.TrenutniKapacitet}  WHERE JedinstvenoIme = '" + baterija.JedinstvenoIme + "'";

                using (connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                return baterija.MaksimalnaSnaga / 3600;
            }
            else
            {
                return 0;
            }
        }

        public void ResetBaterije(Baterija baterija)
        {
            if (baterija.PuniSe)
            {
                baterija.PuniSe = false;
            }
            if (baterija.PrazniSe)
            {
                baterija.PrazniSe = false;
            }
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
                    double kapacitet = double.Parse(table.Rows[i]["Kapacitet"].ToString());
                    bool puniSe = bool.Parse(table.Rows[i]["PuniSe"].ToString());
                    bool prazniSe = bool.Parse(table.Rows[i]["PrazniSe"].ToString());
                    double trenutniKapacitet = double.Parse(table.Rows[i]["TrenutniKapacitet"].ToString());
                    Baterija novaBaterija = new Baterija(jedinstvenoIme, maksimalnaSnaga, kapacitet);
                    novaBaterija.PuniSe = puniSe;
                    novaBaterija.PrazniSe = prazniSe;
                    novaBaterija.TrenutniKapacitet = trenutniKapacitet;
                    if(novaBaterija.TrenutniKapacitet == 0)
                    {
                        novaBaterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery0;
                    }
                    else if(novaBaterija.TrenutniKapacitet > 0 && novaBaterija.TrenutniKapacitet <= novaBaterija.Kapacitet * 20/100)
                    {
                        novaBaterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery10;
                    }
                    else if(novaBaterija.TrenutniKapacitet > novaBaterija.Kapacitet * 20 / 100 && novaBaterija.TrenutniKapacitet <= novaBaterija.Kapacitet * 40 / 100)
                    {
                        novaBaterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery30;
                    }
                    else if(novaBaterija.TrenutniKapacitet > novaBaterija.Kapacitet * 40 / 100 && novaBaterija.TrenutniKapacitet <= novaBaterija.Kapacitet * 60 / 100)
                    {
                        novaBaterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery50;
                    }
                    else if(novaBaterija.TrenutniKapacitet > novaBaterija.Kapacitet * 60 / 100 && novaBaterija.TrenutniKapacitet <= novaBaterija.Kapacitet * 80 / 100)
                    {
                        novaBaterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery70;
                    }
                    else if(novaBaterija.TrenutniKapacitet > novaBaterija.Kapacitet * 80 / 100 && novaBaterija.TrenutniKapacitet <= novaBaterija.Kapacitet * 95 / 100)
                    {
                        novaBaterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery90;
                    }
                    else
                    {
                        novaBaterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery100;
                    }
                    Baterije.Add(novaBaterija);
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
                    double kapacitet = double.Parse(table.Rows[i]["Kapacitet"].ToString());
                    string autoJedinstvenoIme = table.Rows[i]["AutomobilJedinstvenoIme"].ToString();
                    bool puniSe = bool.Parse(table.Rows[i]["PuniSe"].ToString());
                    bool prazniSe = bool.Parse(table.Rows[i]["PrazniSe"].ToString());
                    double trenutniKapacitet = double.Parse(table.Rows[i]["TrenutniKapacitet"].ToString());
                    Baterija novaBaterija = new Baterija(jedinstvenoIme, maksimalnaSnaga, kapacitet);
                    novaBaterija.PuniSe = puniSe;
                    novaBaterija.PrazniSe = prazniSe;
                    novaBaterija.AutomobilJedinstvenoIme = autoJedinstvenoIme;
                    novaBaterija.TrenutniKapacitet = trenutniKapacitet;
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

                    if (naPunjacu == true)
                    {
                        Punjac.Automobil = noviAuto;
                        Punjac.NaPunjacu = true;
                        if(puniSe == true)
                        {
                            Punjac.PuniSe = true;
                        }
                    }

                    if (noviAuto.BaterijaAuta.TrenutniKapacitet == 0)
                    {
                        noviAuto.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery0;
                    }
                    else if (noviAuto.BaterijaAuta.TrenutniKapacitet > 0 && noviAuto.BaterijaAuta.TrenutniKapacitet <= noviAuto.BaterijaAuta.Kapacitet * 20 / 100)
                    {
                        noviAuto.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery10;
                    }
                    else if (noviAuto.BaterijaAuta.TrenutniKapacitet > noviAuto.BaterijaAuta.Kapacitet * 20 / 100 && noviAuto.BaterijaAuta.TrenutniKapacitet <= noviAuto.BaterijaAuta.Kapacitet *40 / 100)
                    {
                        noviAuto.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery30;
                    }
                    else if (noviAuto.BaterijaAuta.TrenutniKapacitet > noviAuto.BaterijaAuta.Kapacitet * 40 / 100 && noviAuto.BaterijaAuta.TrenutniKapacitet <= noviAuto.BaterijaAuta.Kapacitet * 60 / 100)
                    {
                        noviAuto.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery50;
                    }
                    else if (noviAuto.BaterijaAuta.TrenutniKapacitet > noviAuto.BaterijaAuta.Kapacitet * 60 / 100 && noviAuto.BaterijaAuta.TrenutniKapacitet <= noviAuto.BaterijaAuta.Kapacitet * 80 / 100)
                    {
                        noviAuto.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery70;
                    }
                    else if (noviAuto.BaterijaAuta.TrenutniKapacitet > noviAuto.BaterijaAuta.Kapacitet * 80 / 100 && noviAuto.BaterijaAuta.TrenutniKapacitet <= noviAuto.BaterijaAuta.Kapacitet * 95 / 100)
                    {
                        noviAuto.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery90;
                    }
                    else
                    {
                        noviAuto.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery100;
                    }
                    ElektricniAutomobili.Add(noviAuto);
                }
            }
        }
    }
}
