using Common;
using LiveCharts;
using LiveCharts.Wpf;
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

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        public static BindingList<Datum> Datumi { get; set; }

        public static List<Baterija> autoBaterije { get; set; }

        public static Punjac Punjac;

        public static int jednaSekundaJe;
        public static DateTime trenutnoVreme;

        public static Elektrodistribucija distribucija;

        public static double cenovnik; 

        public MainWindow()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SHES.Properties.Settings.BazaPodatakaConnectionString"].ConnectionString;
            Baterije = new BindingList<Baterija>();
            Potrosaci = new BindingList<Potrosac>();
            SolarniPaneli = new BindingList<SolarniPanel>();
            ElektricniAutomobili = new BindingList<ElektricniAutomobil>();
            autoBaterije = new List<Baterija>();
            Datumi = new BindingList<Datum>();
            SnagaSunca = 0;
            cenovnik = 50;
            Punjac = new Punjac();
            jednaSekundaJe = int.Parse(ConfigurationManager.AppSettings["jednaSekundaJe"]);
            distribucija = new Elektrodistribucija();

            UcitajUredjaje();

            InitializeComponent();

            Labels = new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24" };
            Formatter = value => value.ToString("N");

            UcitajDatume();
            labelCenovnik.Content = cenovnik + " $";
            labelSnagaSunca.Content = SnagaSunca.ToString();
            PodesiSnaguSunca(SnagaSunca);
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
                double baterije = 0;
                double distribucijaSat = 0;
                double potrosaci = 0;
                double solarniPaneli = 0;
                string staraCena = "";

                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    int.TryParse(labelSnagaSunca.Content.ToString(), out StaraSnagaSunca);
                    double.TryParse(labelSnagaRazmene.Content.ToString(), out StaraSnagaRazmene);
                    double.TryParse(labelCena.Content.ToString(), out StaraCena);
                    puniSE = labelPuniSe.Content.ToString();
                    staroIme = labelNaPunjacu.Content.ToString();
                    staraCena = labelCenovnik.Content.ToString();
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

                if(staraCena != cenovnik.ToString() + " $")
                {
                    PodesiCenovnik(cenovnik);
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
                        if(b.TrenutniKapacitet <= b.Kapacitet)
                        {
                            baterije += PunjenjeBaterije(b, false);
                            potrosnja = baterije;
                        }
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
                        if(b.TrenutniKapacitet >= 0)
                        {
                            baterije -= PraznjenjeBaterije(b);
                            potrosnja = baterije;
                        }
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
                        potrosaci += p.Potrosnja / 3600;
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
                    solarniPaneli -= ((s.MaksimalnaSnaga / 3600) / 100) * SnagaSunca;
                    potrosnja -= ((s.MaksimalnaSnaga / 3600) / 100) * SnagaSunca;
                }

                if(trenutnoVreme.Hour == 0 && trenutnoVreme.Minute == 0 && trenutnoVreme.Second == 1)
                {
                    UcitajDatume();
                }

                distribucija.Cena += (cenovnik / 3600 * (potrosnja * (-1)));

                distribucija.SnagaRazmene += (potrosnja * (-1));
                distribucijaSat += potrosnja;

                pozoviBaterije++;

                ZapamtiDatumVreme();

                ZapamtiZaGraf(baterije * (-1), solarniPaneli * (-1), distribucijaSat * (-1), potrosaci * (-1));

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

        public void PodesiCenovnik(double cena)
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                labelCenovnik.Content = cena + " $";
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
            if (baterija.TrenutniKapacitet >= 0)
            {
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

        public void ZapamtiDatumVreme()
        {
            string query = "DELETE FROM Vreme WHERE Id = '" + "DatumVreme" + "'";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
            }

            query = $"INSERT INTO Vreme VALUES (@Id, @TrenutnoVreme, NULL)";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@Id", "DatumVreme");
                command.Parameters.AddWithValue("@TrenutnoVreme", trenutnoVreme);
                command.ExecuteNonQuery();
            }
        }

        public void ZapamtiZaGraf(double baterije, double paneli, double distribucija, double potrosaci)
        {
            DataTable table = new DataTable();
            string queryDatumVreme = $"SELECT * FROM Graf WHERE  Datum = @Datum and Sat = {trenutnoVreme.Hour + 1}";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(queryDatumVreme, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.Parameters.AddWithValue("@Datum", trenutnoVreme.Date);

                adapter.Fill(table);
            

                if (table.Rows.Count == 0)
                {
                    string query = $"INSERT INTO Graf VALUES (@Datum2, @Sat, 'Baterije', {baterije})";

                    using (connection = new SqlConnection(connectionString))
                    using (SqlCommand command1 = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command1.Parameters.AddWithValue("@Datum2", trenutnoVreme.Date);
                        command1.Parameters.AddWithValue("@Sat", trenutnoVreme.Hour + 1);
                        command1.ExecuteNonQuery();
                    }

                    query = $"INSERT INTO Graf VALUES (@Datum3, {trenutnoVreme.Hour + 1}, 'Distribucija', {distribucija})";

                    using (connection = new SqlConnection(connectionString))
                    using (SqlCommand command2 = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command2.Parameters.AddWithValue("@Datum3", trenutnoVreme.Date);
                        command2.ExecuteNonQuery();
                    }

                    query = $"INSERT INTO Graf VALUES (@Datum4, {trenutnoVreme.Hour + 1}, 'SolarniPaneli', {paneli})";

                    using (connection = new SqlConnection(connectionString))
                    using (SqlCommand command3 = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command3.Parameters.AddWithValue("@Datum4", trenutnoVreme.Date);
                        command3.ExecuteNonQuery();
                    }

                    query = $"INSERT INTO Graf VALUES (@Datum5, {trenutnoVreme.Hour + 1}, 'Potrosaci', {potrosaci})";

                    using (connection = new SqlConnection(connectionString))
                    using (SqlCommand command4 = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command4.Parameters.AddWithValue("@Datum5", trenutnoVreme.Date);
                        command4.ExecuteNonQuery();
                    }
                }
                else
                {
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        DateTime datum = DateTime.Parse(table.Rows[i]["Datum"].ToString());
                        int sat = int.Parse(table.Rows[i]["Sat"].ToString());
                        string uredjaj = table.Rows[i]["Uredjaj"].ToString();
                        double vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());

                        if (uredjaj == "Baterije")
                        {
                            vrednost += baterije;
                        }
                        else if (uredjaj == "Distribucija")
                        {
                            vrednost += distribucija;
                        }
                        else if (uredjaj == "SolarniPaneli")
                        {
                            vrednost += paneli;
                        }
                        else if (uredjaj == "Potrosaci")
                        {
                            vrednost += potrosaci;
                        }

                        string query = "UPDATE Graf SET Vrednost=@up  WHERE Datum = '" + datum.Date + "'" + " and " + "Sat = " + sat + "and Uredjaj = '" + uredjaj + "'";

                        using (connection = new SqlConnection(connectionString))
                        using (SqlCommand command5 = new SqlCommand(query, connection))
                        {
                            command5.Parameters.Add("@up", SqlDbType.Float).Value = vrednost;
                            connection.Open();
                            command5.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        void UcitajUredjaje()
        {
            string queryDatumVreme = "SELECT * FROM Vreme WHERE SnagaSunca IS NULL";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(queryDatumVreme, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);

                if(table.Rows.Count == 0)
                {
                    trenutnoVreme = DateTime.Now;
                }
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string id = table.Rows[i]["Id"].ToString();
                    trenutnoVreme = DateTime.Parse(table.Rows[i]["DatumVreme"].ToString());
                }
            }

            string querySnagaSunca = "SELECT * FROM Vreme WHERE DatumVreme IS NULL";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(querySnagaSunca, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);

                if (table.Rows.Count == 0)
                {
                    SnagaSunca = 0;
                    vreme.Kind = MaterialDesignThemes.Wpf.PackIconKind.MoonAndStars;
                }
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string id = table.Rows[i]["Id"].ToString();
                    SnagaSunca = int.Parse(table.Rows[i]["SnagaSunca"].ToString());
                }
            }

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

        public void UcitajDatume()
        {
            this.Dispatcher.Invoke(() =>
            {
                Datumi.Clear();
                DataTable table = new DataTable();
                string queryDatumVreme = $"SELECT DISTINCT Datum FROM Graf";
                using (connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(queryDatumVreme, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.Parameters.AddWithValue("@Datum", trenutnoVreme.Date);
                    adapter.Fill(table);

                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        Datumi.Add(new Datum(DateTime.Parse(table.Rows[i]["Datum"].ToString()).ToString("d/M/yyyy")));
                    }
                }
            });
        }

        private void ButtonDodaj_Click(object sender, RoutedEventArgs e)
        {
            string datum;
            try
            {
                datum = ((Datum)(cmbDatum.SelectedValue)).DatumBaza;
                DateTime date = DateTime.ParseExact(datum, "d/M/yyyy", null);
                DataTable table = new DataTable();
                Dictionary<int, double> vBaterije = new Dictionary<int, double>();
                Dictionary<int, double> vPotrosaca = new Dictionary<int, double>();
                Dictionary<int, double> vPanela = new Dictionary<int, double>();
                Dictionary<int, double> vDistribucije = new Dictionary<int, double>();
                double ukupnoBaterije = 0;
                double ukupnoPaneli = 0;
                double ukupnoPotrosaci = 0;
                double ukupnoDistribucija = 0;
                double ukupno = 0;
                for (int i = 1; i < 25; i++)
                {
                    vBaterije.Add(i, 0);
                    vPotrosaca.Add(i, 0);
                    vDistribucije.Add(i, 0);
                    vPanela.Add(i, 0);
                }
                string queryDatumVreme = $"SELECT * FROM Graf WHERE Datum = @Datum";
                using (connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(queryDatumVreme, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.Parameters.AddWithValue("@Datum", date.Date);
                    adapter.Fill(table);

                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        int sat = 0;
                        double vrednost = 0;
                        switch (int.Parse(table.Rows[i]["Sat"].ToString()))
                        {
                            case 1:
                                sat = 1;
                                vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());
                                break;
                            case 2:
                                sat = 2;
                                vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());
                                break;
                            case 3:
                                sat = 3;
                                vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());
                                break;
                            case 4:
                                sat = 4;
                                vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());
                                break;
                            case 5:
                                sat = 5;
                                vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());
                                break;
                            case 6:
                                sat = 6;
                                vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());
                                break;
                            case 7:
                                sat = 7;
                                vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());
                                break;
                            case 8:
                                sat = 8;
                                vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());
                                break;
                            case 9:
                                sat = 9;
                                vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());
                                break;
                            case 10:
                                sat = 10;
                                vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());
                                break;
                            case 11:
                                sat = 11;
                                vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());
                                break;
                            case 12:
                                sat = 12;
                                vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());
                                break;
                            case 13:
                                sat = 13;
                                vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());
                                break;
                            case 14:
                                sat = 14;
                                vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());
                                break;
                            case 15:
                                sat = 15;
                                vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());
                                break;
                            case 16:
                                sat = 16;
                                vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());
                                break;
                            case 17:
                                sat = 17;
                                vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());
                                break;
                            case 18:
                                sat = 18;
                                vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());
                                break;
                            case 19:
                                sat = 19;
                                vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());
                                break;
                            case 20:
                                sat = 20;
                                vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());
                                break;
                            case 21:
                                sat = 21;
                                vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());
                                break;
                            case 22:
                                sat = 22;
                                vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());
                                break;
                            case 23:
                                sat = 23;
                                vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());
                                break;
                            case 24:
                                sat = 24;
                                vrednost = double.Parse(table.Rows[i]["Vrednost"].ToString());
                                break;
                            default:
                                break;
                        }
                        switch (table.Rows[i]["Uredjaj"].ToString())
                        {
                            case "Baterije":
                                vBaterije[sat] = vrednost;
                                ukupnoBaterije += vrednost;
                                break;
                            case "Distribucija":
                                vDistribucije[sat] = vrednost;
                                ukupnoDistribucija += vrednost;
                                break;
                            case "SolarniPaneli":
                                vPanela[sat] = vrednost;
                                ukupnoPaneli += vrednost;
                                break;
                            case "Potrosaci":
                                vPotrosaca[sat] = vrednost;
                                ukupnoPotrosaci += vrednost;
                                break;
                            default:
                                break;
                        }
                    }
                }

                SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Baterije",
                    Values = new ChartValues<double> { vBaterije[1], vBaterije[2], vBaterije[3], vBaterije[4], vBaterije[5], vBaterije[6], vBaterije[7], vBaterije[8],
                     vBaterije[9], vBaterije[10], vBaterije[11], vBaterije[12], vBaterije[13], vBaterije[14], vBaterije[15], vBaterije[16],
                     vBaterije[17], vBaterije[18], vBaterije[19], vBaterije[20], vBaterije[21], vBaterije[22], vBaterije[23], vBaterije[24] }
                }
            };

                //adding series will update and animate the chart automatically
                SeriesCollection.Add(new ColumnSeries
                {
                    Title = "Distribucija",
                    Values = new ChartValues<double> { vDistribucije[1], vDistribucije[2], vDistribucije[3], vDistribucije[4], vDistribucije[5], vDistribucije[6], vDistribucije[7],
                 vDistribucije[8], vDistribucije[9], vDistribucije[10], vDistribucije[11], vDistribucije[12], vDistribucije[13], vDistribucije[14], vDistribucije[15],
                 vDistribucije[16], vDistribucije[17], vDistribucije[18], vDistribucije[19], vDistribucije[20], vDistribucije[21], vDistribucije[22], vDistribucije[23], vDistribucije[24] }
                });

                SeriesCollection.Add(new ColumnSeries
                {
                    Title = "Paneli",
                    Values = new ChartValues<double> { vPanela[1], vPanela[2], vPanela[3], vPanela[4], vPanela[5], vPanela[6], vPanela[7], vPanela[8], vPanela[9], vPanela[10],
                vPanela[11], vPanela[12],vPanela[13], vPanela[14],vPanela[15], vPanela[16],vPanela[17], vPanela[18],vPanela[19], vPanela[20],vPanela[21], vPanela[22],
                vPanela[23], vPanela[24] }
                });

                SeriesCollection.Add(new ColumnSeries
                {
                    Title = "Potrosaci",
                    Values = new ChartValues<double> { vPotrosaca[1], vPotrosaca[2], vPotrosaca[3], vPotrosaca[4], vPotrosaca[5], vPotrosaca[6], vPotrosaca[7], vPotrosaca[8],
                vPotrosaca[9], vPotrosaca[10],vPotrosaca[11], vPotrosaca[12],vPotrosaca[13], vPotrosaca[14],vPotrosaca[15], vPotrosaca[16],vPotrosaca[17], vPotrosaca[18],
                vPotrosaca[19], vPotrosaca[20],vPotrosaca[21], vPotrosaca[22],vPotrosaca[23], vPotrosaca[24] }
                });

                garf1.Series = SeriesCollection;

                ukupno = ukupnoBaterije + ukupnoDistribucija + ukupnoPaneli + ukupnoPotrosaci;

                labelTrosakDatum.Content = (cenovnik * ukupno).ToString("N2") + " $";
            }
            catch
            {
                datum = "";
            }
                
        }
            
        

        private void ButtonObrisi_Click(object sender, RoutedEventArgs e)
        {
            string datum = ((Datum)(cmbDatum.SelectedValue)).DatumBaza;
            DateTime date = DateTime.ParseExact(datum, "d/M/yyyy", null);

            string query = "DELETE FROM Graf WHERE Datum = @Datum";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Datum", date.Date);
                connection.Open();
                command.ExecuteNonQuery();
            }

            UcitajDatume();
        }
    }
}
