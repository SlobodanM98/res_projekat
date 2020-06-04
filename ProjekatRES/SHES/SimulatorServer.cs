using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SHES
{
    public class SimulatorServer : ISimulator
    {
        SqlConnection connection;
        string connectionString = ConfigurationManager.ConnectionStrings["SHES.Properties.Settings.BazaPodatakaConnectionString"].ConnectionString;

        public void DodajSolarniPanel(SolarniPanel noviPanel)
        {
            bool sadrzi = false;
            foreach (SolarniPanel sp in MainWindow.SolarniPaneli)
            {
                if (sp.JedinstvenoIme == noviPanel.JedinstvenoIme)
                {
                    sadrzi = true;
                    break;
                }
            }

            if (!sadrzi)
            {
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    MainWindow.SolarniPaneli.Add(noviPanel);
                });
          
                string query = $"INSERT INTO SolarnePanele VALUES (@jedinstvenoIme, {noviPanel.MaksimalnaSnaga})";

                using (connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@jedinstvenoIme", noviPanel.JedinstvenoIme);
                    command.ExecuteNonQuery();
                }

            }
        }

        public void UkloniSolarniPanel(string jedinstvenoIme)
        {
            bool sadrzi = false;
            foreach (SolarniPanel sp in MainWindow.SolarniPaneli)
            {
                if (sp.JedinstvenoIme == jedinstvenoIme)
                {
                    sadrzi = true;
                    App.Current.Dispatcher.Invoke((System.Action)delegate
                    {
                        MainWindow.SolarniPaneli.Remove(sp);
                    });
                    break;
                }
            }

            if (sadrzi)
            {
                string query = "DELETE FROM SolarnePanele WHERE JedinstvenoIme = '" + jedinstvenoIme + "'";

                using (connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DodajPotrosaca(Potrosac potrosac)
        {
            bool sadrzi = false;
            foreach(Potrosac p in MainWindow.Potrosaci)
            {
                if(p.JedinstvenoIme == potrosac.JedinstvenoIme)
                {
                    sadrzi = true;
                    break;
                }
            }

            if (!sadrzi)
            {
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    MainWindow.Potrosaci.Add(potrosac);
                });

                int vrednost = 0;
                if (potrosac.Upaljen)
                    vrednost = 1;
                string query = $"INSERT INTO Potrosaci VALUES (@jedinstvenoIme, {potrosac.Potrosnja}, {vrednost})";

                using (connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@jedinstvenoIme", potrosac.JedinstvenoIme);
                    command.ExecuteNonQuery();
                }

            }
        }

        public void UkloniPotrosaca(string jedinstvenoIme)
        {
            bool sadrzi = false;
            foreach(Potrosac p in MainWindow.Potrosaci)
            {
                if(p.JedinstvenoIme == jedinstvenoIme)
                {
                    sadrzi = true;
                    App.Current.Dispatcher.Invoke((System.Action)delegate
                    {
                        MainWindow.Potrosaci.Remove(p);
                    });
                    break;
                }
            }

            if (sadrzi)
            {
                string query = "DELETE FROM Potrosaci WHERE JedinstvenoIme = '" + jedinstvenoIme + "'";

                using (connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpaliPotrosac(string jedinstvenoIme)
        {
            bool sadrzi = false;
            foreach(Potrosac p in MainWindow.Potrosaci)
            {
                if(p.JedinstvenoIme == jedinstvenoIme)
                {
                    sadrzi = true;
                    p.Upaljen = true;
                    p.Slika = MaterialDesignThemes.Wpf.PackIconKind.PowerPlugOutline;
                    break;
                }
            }

            if(sadrzi)
            {
                string query = "UPDATE Potrosaci SET Upaljen=@up  WHERE JedinstvenoIme = '" + jedinstvenoIme + "'";

                using (connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@up", SqlDbType.Bit).Value = true;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UgasiPotrosac(string jedinstvenoIme)
        {
            bool sadrzi = false;
            foreach (Potrosac p in MainWindow.Potrosaci)
            {
                if (p.JedinstvenoIme == jedinstvenoIme)
                {
                    sadrzi = true;
                    p.Upaljen = false;
                    p.Slika = MaterialDesignThemes.Wpf.PackIconKind.PowerPlugOffOutline;
                    break;
                }
            }

            if (sadrzi)
            {
                string query = "UPDATE Potrosaci SET Upaljen=@down  WHERE JedinstvenoIme = '" + jedinstvenoIme + "'";

                using (connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@down", SqlDbType.Bit).Value = false;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DodajBateriju(Baterija novaBaterija, bool jesteAutomobil, string AutomobilJedinstvenoIme)
        {
            bool sadrzi = false;
            foreach(Baterija b in MainWindow.Baterije)
            {
                if(b.JedinstvenoIme == novaBaterija.JedinstvenoIme)
                {
                    sadrzi = true;
                    break;
                }
            }

            if (!sadrzi)
            {
                string query;
                int puniSe = 0;
                int prazniSe = 0;
                if (novaBaterija.PuniSe)
                {
                    puniSe = 1;
                }
                if (novaBaterija.PrazniSe)
                {
                    prazniSe = 1;
                }

                if (jesteAutomobil)
                {
                    query = $"INSERT INTO Baterije VALUES (@jedinstvenoIme, {novaBaterija.MaksimalnaSnaga}, {novaBaterija.Kapacitet}, @automobilJedinstvenoIme, {puniSe}, {prazniSe}, {novaBaterija.TrenutniKapacitet})";
                }
                else
                {
                    App.Current.Dispatcher.Invoke((System.Action)delegate
                    {
                        MainWindow.Baterije.Add(novaBaterija);
                    });
                    query = $"INSERT INTO Baterije VALUES (@jedinstvenoIme, {novaBaterija.MaksimalnaSnaga}, {novaBaterija.Kapacitet}, NULL, {puniSe}, {prazniSe}, {novaBaterija.TrenutniKapacitet})";
                }

                using (connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@jedinstvenoIme", novaBaterija.JedinstvenoIme);
                    if (jesteAutomobil)
                    {
                        command.Parameters.AddWithValue("@automobilJedinstvenoIme", AutomobilJedinstvenoIme);
                    }
                    
                    command.ExecuteNonQuery();
                }
            }

        }

        public void UkloniBateriju(string jedinstvenoIme)
        {
            bool sadrzi = false;

            foreach(Baterija b in MainWindow.Baterije)
            {
                if(b.JedinstvenoIme == jedinstvenoIme)
                {
                    sadrzi = true;
                    App.Current.Dispatcher.Invoke((System.Action)delegate
                    {
                        MainWindow.Baterije.Remove(b);
                    });
                    break;
                }
            }

            if (sadrzi)
            {
                string query = "DELETE FROM Baterije WHERE JedinstvenoIme = '" + jedinstvenoIme + "'";

                using (connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DodajElektricniAutomobil(ElektricniAutomobil automobil)
        {
            bool sadrzi = false;
            foreach (ElektricniAutomobil e in MainWindow.ElektricniAutomobili)
            {
                if (e.JedinstvenoIme == automobil.JedinstvenoIme)
                {
                    sadrzi = true;
                    break;
                }
            }

            if (!sadrzi)
            {
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    MainWindow.ElektricniAutomobili.Add(automobil);
                });

                int naPunjacu = 0;
                if (automobil.NaPunjacu)
                    naPunjacu = 1;
                int puniSe = 0;
                if (automobil.PuniSe)
                    puniSe = 1;
                string query = $"INSERT INTO Automobili VALUES (@jedinstvenoIme, {naPunjacu}, {puniSe})";

                using (connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@jedinstvenoIme", automobil.JedinstvenoIme);
                    command.ExecuteNonQuery();
                }
                DodajBateriju(automobil.BaterijaAuta, true, automobil.JedinstvenoIme);
            }
        }

        public void UkloniElektricniAutomobil(string jedinstvenoIme)
        {
            bool sadrzi = false;

            foreach (ElektricniAutomobil a in MainWindow.ElektricniAutomobili)
            {
                if (a.JedinstvenoIme == jedinstvenoIme)
                {
                    sadrzi = true;
                    ZaustaviPunjenje(jedinstvenoIme);
                    IskljuciSaPunjaca(jedinstvenoIme);
                    App.Current.Dispatcher.Invoke((System.Action)delegate
                    {
                        MainWindow.ElektricniAutomobili.Remove(a);
                        MainWindow.autoBaterije.Remove(a.BaterijaAuta);
                    });
                    break;
                }
            }

            if (sadrzi)
            {
                string query = "DELETE FROM Automobili WHERE JedinstvenoIme = '" + jedinstvenoIme + "'";

                using (connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                query = "DELETE FROM Baterije WHERE AutomobilJedinstvenoIme = '" + jedinstvenoIme + "'";

                using (connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void PromeniSnaguSunca(int novaVrednost)
        {
            MainWindow.SnagaSunca = novaVrednost;

            string query = "DELETE FROM Vreme WHERE Id = '" + "SnagaSunca" + "'";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
            }

            query = $"INSERT INTO Vreme VALUES (@Id, NULL, {novaVrednost})";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@Id", "SnagaSunca");
                command.ExecuteNonQuery();
            }
        }

        public bool UkljuciNaPunjac(string jedinstvenoIme)
        {
            if (MainWindow.Punjac.NaPunjacu)
            {
                return false;
            }
            MainWindow.Punjac.NaPunjacu = true;
            foreach(ElektricniAutomobil e in MainWindow.ElektricniAutomobili)
            {
                if(e.JedinstvenoIme == jedinstvenoIme)
                {
                    MainWindow.Punjac.Automobil = e;
                    e.NaPunjacu = true;

                    string query = "UPDATE Automobili SET NaPunjacu=@up  WHERE JedinstvenoIme = '" + jedinstvenoIme + "'";

                    using (connection = new SqlConnection(connectionString))
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@up", SqlDbType.Bit).Value = true;
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    break;
                }
            }

            return true;
        }

        public bool IskljuciSaPunjaca(string jedinstvenoIme)
        {
            if (!MainWindow.Punjac.NaPunjacu)
            {
                return false;
            }
            MainWindow.Punjac.NaPunjacu = false;
            MainWindow.Punjac.PuniSe = false;
            foreach (ElektricniAutomobil e in MainWindow.ElektricniAutomobili)
            {
                if (e.JedinstvenoIme == jedinstvenoIme)
                {
                    MainWindow.Punjac.Automobil = null;
                    e.NaPunjacu = false;
                    e.PuniSe = false;

                    if (e.BaterijaAuta.TrenutniKapacitet == 0)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery0;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > 0 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 20 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery10;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 20 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 40 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery30;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 40 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 60 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery50;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 60 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 80 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery70;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 80 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 95 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery90;
                    }
                    else
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery100;
                    }

                    string query = "UPDATE Automobili SET NaPunjacu=@up  WHERE JedinstvenoIme = '" + jedinstvenoIme + "'";

                    using (connection = new SqlConnection(connectionString))
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@up", SqlDbType.Bit).Value = false;
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    break;
                }
            }
            return true;
        }

        public bool PokreniPunjenje()
        {
            if(!MainWindow.Punjac.NaPunjacu || MainWindow.Punjac.PuniSe)
            {
                return false;
            }
            MainWindow.Punjac.PuniSe = true;
            foreach (ElektricniAutomobil e in MainWindow.ElektricniAutomobili)
            {
                if (e.JedinstvenoIme == MainWindow.Punjac.Automobil.JedinstvenoIme)
                {
                    e.PuniSe = true;
                    string query = "UPDATE Automobili SET Punise=@up  WHERE JedinstvenoIme = '" + e.JedinstvenoIme + "'";

                    using (connection = new SqlConnection(connectionString))
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@up", SqlDbType.Bit).Value = true;
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    break;
                }
            }
            return true;
        }

        public bool ZaustaviPunjenje()
        {
            if(!MainWindow.Punjac.NaPunjacu || !MainWindow.Punjac.PuniSe)
            {
                return false;
            }
            MainWindow.Punjac.PuniSe = false;
            foreach (ElektricniAutomobil e in MainWindow.ElektricniAutomobili)
            {
                if (e.JedinstvenoIme == MainWindow.Punjac.Automobil.JedinstvenoIme)
                {
                    e.PuniSe = false;
                    if (e.BaterijaAuta.TrenutniKapacitet == 0)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery0;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > 0 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 20 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.BatteryCharging10;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 20 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 40 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.BatteryCharging30;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 40 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 60 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.BatteryCharging50;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 60 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 80 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.BatteryCharging70;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 80 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 95 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.BatteryCharging90;
                    }
                    else
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.BatteryCharging100;
                    }

                    string query = "UPDATE Automobili SET Punise=@up  WHERE JedinstvenoIme = '" + e.JedinstvenoIme + "'";

                    using (connection = new SqlConnection(connectionString))
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@up", SqlDbType.Bit).Value = false;
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    break;
                }
            }
            return true;
        }

        public bool ZaustaviPunjenje(string jedinstvenoIme)
        {
            if (!MainWindow.Punjac.NaPunjacu || !MainWindow.Punjac.PuniSe)
            {
                return false;
            }
            foreach (ElektricniAutomobil e in MainWindow.ElektricniAutomobili)
            {
                if (e.JedinstvenoIme == jedinstvenoIme)
                {
                    if(e.JedinstvenoIme == MainWindow.Punjac.Automobil.JedinstvenoIme)
                    {
                        MainWindow.Punjac.PuniSe = false;
                        e.PuniSe = false;
                        if (e.BaterijaAuta.TrenutniKapacitet == 0)
                        {
                            e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery0;
                        }
                        else if (e.BaterijaAuta.TrenutniKapacitet > 0 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 20 / 100)
                        {
                            e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery10;
                        }
                        else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 20 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 40 / 100)
                        {
                            e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery30;
                        }
                        else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 40 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 60 / 100)
                        {
                            e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery50;
                        }
                        else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 60 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 80 / 100)
                        {
                            e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery70;
                        }
                        else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 80 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 95 / 100)
                        {
                            e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery90;
                        }
                        else
                        {
                            e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery100;
                        }

                        string query = "UPDATE Automobili SET Punise=@up  WHERE JedinstvenoIme = '" + e.JedinstvenoIme + "'";

                        using (connection = new SqlConnection(connectionString))
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.Add("@up", SqlDbType.Bit).Value = false;
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                        return true;
                        //break;
                    }
                }
            }
            return false;
        }

        public bool PostavljanjeKapacitetaAuta(int trenutniKapacitet)
        {
            if (!MainWindow.Punjac.NaPunjacu || !MainWindow.Punjac.PuniSe)
            {
                return false;
            }
            foreach (ElektricniAutomobil e in MainWindow.ElektricniAutomobili)
            {
                if (e.JedinstvenoIme == MainWindow.Punjac.Automobil.JedinstvenoIme)
                {
                    e.BaterijaAuta.TrenutniKapacitet = trenutniKapacitet;
                    if (e.BaterijaAuta.TrenutniKapacitet == 0)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery0;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > 0 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 20 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery10;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 20 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 40 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery30;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 40 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 60 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery50;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 60 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 80 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery70;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 80 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 95 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery90;
                    }
                    else
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery100;
                    }

                    break;
                }
            }
            return true;
        }

        public Uredjaji PreuzmiUredjaje()
        {
            Uredjaji uredjaji = new Uredjaji();
            uredjaji.Automobili = MainWindow.ElektricniAutomobili.ToList();
            uredjaji.Baterije = MainWindow.Baterije.ToList();
            uredjaji.Potrosaci = MainWindow.Potrosaci.ToList();
            uredjaji.Paneli = MainWindow.SolarniPaneli.ToList();

            return uredjaji;
        }

        public void PodesiOdnos(int noviOdnos)
        {
            MainWindow.jednaSekundaJe = noviOdnos;
        }

        public void PodesavanjeCene(int cena)
        {
            MainWindow.cenovnik = cena;
        }
    }
}
