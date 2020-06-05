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

        public static List<PodaciZaGraf> podaciZaGraf;

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

        public static double baterije = 0;
        public static double distribucijaSat = 0;
        public static double potrosaci = 0;
        public static double solarniPaneli = 0;

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
            podaciZaGraf = new List<PodaciZaGraf>();

            InitializeComponent();

            UcitajUredjaje();

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
            int vremeZaAzuriranje = trenutnoVreme.Minute * 60 + trenutnoVreme.Second;
            DateTime staraSekunda = DateTime.Now;
            int stariSat = trenutnoVreme.Hour;
            int brSekunda = 0;
            bool prosaoDan = false;
            DateTime stariDatum = trenutnoVreme.Date;

            //da se pozove ucitavanje
            //UcitajPoslednjiSat();
            while (true)
            {
                trenutnoVreme = trenutnoVreme.AddSeconds(1);
                brSekunda++;
                PodesiTrenutnoVreme();
                vremeZaAzuriranje++;

                double potrosnja = 0;
                int StaraSnagaSunca = 0;
                double StaraSnagaRazmene = 0;
                double StaraCena = 0;
                string puniSE = "";
                string staroIme = "";
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
                        if (b.TrenutniKapacitet < b.Kapacitet)
                        {
                            double povratnaBaterije = PunjenjeBaterije(b, false);
                            baterije += povratnaBaterije;
                            potrosnja += povratnaBaterije;
                        }
                        if (pozoviBaterije == 0 && b.TrenutniKapacitet <= b.Kapacitet)
                        {
                            if (b.TrenutniKapacitet + (b.MaksimalnaSnaga / 60) <= b.Kapacitet)
                            {
                                b.TrenutniKapacitet += b.MaksimalnaSnaga / 60;
                                b.TrenutniKapacitet = Math.Round(b.TrenutniKapacitet, 4);
                            }
                            else
                            {
                                b.TrenutniKapacitet = b.Kapacitet;
                            }
                        }
                    }

                }else if(trenutnoVreme.Hour >= 14 && trenutnoVreme.Hour < 17)
                {
                    foreach (Baterija b in Baterije)
                    {
                        if (b.TrenutniKapacitet > 0)
                        {
                            double povratnaBaterije = PraznjenjeBaterije(b);
                            baterije -= povratnaBaterije;
                            potrosnja -= povratnaBaterije;
                        }
                        if (pozoviBaterije == 0 && b.TrenutniKapacitet >= 0)
                        {
                            if (b.TrenutniKapacitet - (b.MaksimalnaSnaga / 60) >= 0)
                            {
                                b.TrenutniKapacitet -= b.MaksimalnaSnaga / 60;
                                b.TrenutniKapacitet = Math.Round(b.TrenutniKapacitet, 4);
                            }
                            else
                            {
                                b.TrenutniKapacitet = 0;
                            }
                        }
                    }
                }
                else if((trenutnoVreme.Hour == 6 && trenutnoVreme.Minute == 0 && trenutnoVreme.Second == 0) || (trenutnoVreme.Hour == 17 && trenutnoVreme.Minute == 0 && trenutnoVreme.Second == 0))
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
                    if(Punjac.Automobil.BaterijaAuta.TrenutniKapacitet < Punjac.Automobil.BaterijaAuta.Kapacitet)
                    {
                        potrosnja += PunjenjeBaterije(Punjac.Automobil.BaterijaAuta, true);
                    }
                    if(pozoviBaterije == 0 && Punjac.Automobil.BaterijaAuta.TrenutniKapacitet <= Punjac.Automobil.BaterijaAuta.Kapacitet)
                    {
                        if(Punjac.Automobil.BaterijaAuta.TrenutniKapacitet + (Punjac.Automobil.BaterijaAuta.MaksimalnaSnaga / 60) <= Punjac.Automobil.BaterijaAuta.Kapacitet)
                        {
                            Punjac.Automobil.BaterijaAuta.TrenutniKapacitet += Punjac.Automobil.BaterijaAuta.MaksimalnaSnaga / 60;
                            Punjac.Automobil.BaterijaAuta.TrenutniKapacitet = Math.Round(Punjac.Automobil.BaterijaAuta.TrenutniKapacitet, 4);
                        }
                        else
                        {
                            Punjac.Automobil.BaterijaAuta.TrenutniKapacitet = Punjac.Automobil.BaterijaAuta.Kapacitet;
                        }
                    }
                }

                foreach(SolarniPanel s in SolarniPaneli)
                {
                    solarniPaneli -= ((s.MaksimalnaSnaga / 3600) / 100) * SnagaSunca;
                    potrosnja -= ((s.MaksimalnaSnaga / 3600) / 100) * SnagaSunca;
                }

                distribucija.Cena += (cenovnik * (potrosnja * (-1)));

                distribucija.SnagaRazmene += (potrosnja * (-1));
                distribucijaSat += potrosnja;

                pozoviBaterije++;

                if (podaciZaGraf.Count != 0)
                {
                    foreach (PodaciZaGraf pzg in podaciZaGraf.ToList())
                    {
                        if (pzg.Datum.Date == trenutnoVreme.Date)
                        {
                            if (pzg.Sat == trenutnoVreme.Hour)
                            {
                                pzg.Baterije = baterije * (-1);
                                pzg.Distribucija = distribucijaSat * (-1);
                                pzg.SolarniPaneli = solarniPaneli * (-1);
                                pzg.Potrosaci = potrosaci * (-1);
                            }
                            else
                            {
                                podaciZaGraf.Add(new PodaciZaGraf(trenutnoVreme.Hour, trenutnoVreme.Date, baterije * (-1), distribucijaSat * (-1), solarniPaneli * (-1), potrosaci * (-1)));
                            }
                        }
                        else
                        {
                            podaciZaGraf.Add(new PodaciZaGraf(trenutnoVreme.Hour, trenutnoVreme.Date, baterije * (-1), distribucijaSat * (-1), solarniPaneli * (-1), potrosaci * (-1)));
                        }
                    }
                }
                else
                {
                    podaciZaGraf.Add(new PodaciZaGraf(trenutnoVreme.Hour, trenutnoVreme.Date, baterije * (-1), distribucijaSat * (-1), solarniPaneli * (-1), potrosaci * (-1)));
                    if (stariDatum.Date != trenutnoVreme.Date)
                    {
                        stariDatum = trenutnoVreme.Date;
                        prosaoDan = true;
                    }
                }

                //ZapamtiVreme();
                if (staraSekunda.Second != DateTime.Now.Second)
                {
                    brSekunda = 0;
                    ZapamtiZaGraf(baterije * (-1), solarniPaneli * (-1), distribucijaSat * (-1), potrosaci * (-1));
                    if(prosaoDan == true)
                    {
                        UcitajDatume();
                        prosaoDan = false;
                    }
                    staraSekunda = DateTime.Now;
                }

                if (stariSat != trenutnoVreme.Hour)
                {
                    stariSat = trenutnoVreme.Hour;
                    baterije = 0;
                    solarniPaneli = 0;
                    distribucijaSat = 0;
                    potrosaci = 0;
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

        public void ZapamtiZaGraf(double baterije, double paneli, double distribucija, double potrosaci)
        {
            DataTable table = new DataTable();
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach (PodaciZaGraf pzg in podaciZaGraf)
                {
                    string queryDVreme = $"SELECT * FROM Graf WHERE  Datum = @Datum and Sat = @Sat2";
                    using (SqlCommand command = new SqlCommand(queryDVreme, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            command.Parameters.AddWithValue("@Datum", pzg.Datum.Date);
                            command.Parameters.AddWithValue("@Sat2", pzg.Sat + 1);

                            adapter.Fill(table);


                            if (table.Rows.Count == 0)
                            {
                                command.CommandText = $"INSERT INTO Graf VALUES (@Datum2, @Sat, {pzg.Baterije}, {pzg.Distribucija}, {pzg.SolarniPaneli}, {pzg.Potrosaci})";

                                //using (SqlCommand command1 = new SqlCommand(query, connection))
                                //{
                                //connection.Open();
                                command.Parameters.AddWithValue("@Datum2", pzg.Datum.Date);
                                command.Parameters.AddWithValue("@Sat", pzg.Sat + 1);
                                int i = command.ExecuteNonQuery();
                                //}
                            }
                            else
                            {
                                for (int i = 0; i < table.Rows.Count; i++)
                                {
                                    DateTime datum = DateTime.Parse(table.Rows[i]["Datum"].ToString());
                                    int sat = int.Parse(table.Rows[i]["Sat"].ToString());
                                    /*double vrednostBaterije = double.Parse(table.Rows[i]["Baterije"].ToString()) + baterije;
                                    double vrednostDistribucija = double.Parse(table.Rows[i]["Distribucija"].ToString()) + distribucija;
                                    double vrednostPaneli = double.Parse(table.Rows[i]["SolarniPaneli"].ToString()) + paneli;
                                    double vrednostPotrosaci = double.Parse(table.Rows[i]["Potrosaci"].ToString()) + potrosaci;*/

                                    command.CommandText = "UPDATE Graf SET Baterije=@vb, Distribucija=@vd, SolarniPaneli=@vsp, Potrosaci=@vp WHERE Datum = @Datum2" + " and " + "Sat = @Sat";


                                    //using (SqlCommand command5 = new SqlCommand(query, connection))
                                    //{
                                    command.Parameters.AddWithValue("@Datum2", pzg.Datum.Date);
                                    command.Parameters.AddWithValue("@Sat", pzg.Sat + 1);
                                    command.Parameters.Add("@vb", SqlDbType.Float).Value = pzg.Baterije;
                                    command.Parameters.Add("@vd", SqlDbType.Float).Value = pzg.Distribucija;
                                    command.Parameters.Add("@vsp", SqlDbType.Float).Value = pzg.SolarniPaneli;
                                    command.Parameters.Add("@vp", SqlDbType.Float).Value = pzg.Potrosaci;
                                    //connection.Open();
                                    int iss = command.ExecuteNonQuery();
                                    
                                    // }
                                }
                            }
                        }
                    }
                }
                string queryDatumVreme  = "UPDATE Vreme SET DatumVreme=@dv WHERE Id = 'DatumVreme'";
                using (SqlCommand command = new SqlCommand(queryDatumVreme, connection))
                {
                    command.Parameters.AddWithValue("@dv", trenutnoVreme);
                    command.ExecuteNonQuery();
                }
            }
            podaciZaGraf.Clear();
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

            string queryDatum = "SELECT * FROM Graf WHERE Datum = @Datum and Sat = @Sat";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(queryDatum, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable table = new DataTable();
                command.Parameters.AddWithValue("@Datum", trenutnoVreme.Date);
                command.Parameters.AddWithValue("@Sat", trenutnoVreme.Hour + 1);
                adapter.Fill(table);

                baterije = double.Parse(table.Rows[0]["Baterije"].ToString()) * (-1);
                distribucijaSat = double.Parse(table.Rows[0]["Distribucija"].ToString()) * (-1);
                solarniPaneli = double.Parse(table.Rows[0]["SolarniPaneli"].ToString()) * (-1);
                potrosaci = double.Parse(table.Rows[0]["Potrosaci"].ToString()) * (-1);

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
                    //command.Parameters.AddWithValue("@Datum", trenutnoVreme.Date);
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
                        int sat = int.Parse(table.Rows[i]["Sat"].ToString());
                        double vrednostBaterije = double.Parse(table.Rows[i]["Baterije"].ToString());
                        double vrednostDistribucija = double.Parse(table.Rows[i]["Distribucija"].ToString());
                        double vrednostPaneli = double.Parse(table.Rows[i]["SolarniPaneli"].ToString());
                        double vrednostPotrosaci = double.Parse(table.Rows[i]["Potrosaci"].ToString());

                        vBaterije[sat] = vrednostBaterije;
                        ukupnoBaterije += vrednostBaterije; 

                        vDistribucije[sat] = vrednostDistribucija;
                        ukupnoDistribucija += vrednostDistribucija;

                        vPanela[sat] = vrednostPaneli;
                        ukupnoPaneli += vrednostPaneli;

                        vPotrosaca[sat] = vrednostPotrosaci;
                        ukupnoPotrosaci += vrednostPotrosaci;
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

        //ovo da se implementira
        private void UcitajPoslednjiSat()
        {
            DataTable table = new DataTable();

            string queryDatumVreme = $"SELECT * FROM Graf WHERE Datum = @Datum AND Sat = @Sat";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(queryDatumVreme, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.Parameters.AddWithValue("@Datum", trenutnoVreme.Date);
                command.Parameters.AddWithValue("@Sat", trenutnoVreme.Hour + 1);
                adapter.Fill(table);
                for(int i = 0; i < table.Rows.Count; i++)
                {
                    baterije = double.Parse(table.Rows[i]["Baterije"].ToString());
                    distribucijaSat = double.Parse(table.Rows[i]["Distribucija"].ToString());
                    solarniPaneli = double.Parse(table.Rows[i]["SolarniPaneli"].ToString());
                    potrosaci = double.Parse(table.Rows[i]["Potrosaci"].ToString());
                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            ZapamtiZaGraf(baterije * (-1), solarniPaneli * (-1), distribucijaSat * (-1), potrosaci * (-1));

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

        public void ZapamtiVreme()
        {
            string query = "UPDATE Vreme SET DatumVreme=@dv WHERE Id = 'DatumVreme'";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@dv", trenutnoVreme);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
