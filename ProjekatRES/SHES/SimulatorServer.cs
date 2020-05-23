using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
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

        public void DodajBateriju(Baterija novaBaterija)
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
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    MainWindow.Baterije.Add(novaBaterija);
                });

                string query = $"INSERT INTO Baterije VALUES (@jedinstvenoIme, {novaBaterija.MaksimalnaSnaga}, {novaBaterija.Kapacitet})";

                using (connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@jedinstvenoIme", novaBaterija.JedinstvenoIme);
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
            if (!BazaPodataka.Automobili.ContainsKey(automobil.JedinstvenoIme))
            {
                BazaPodataka.Automobili.Add(automobil.JedinstvenoIme, automobil);
            }
        }

        public void UkloniElektricniAutomobil(string jedinstvenoIme)
        {
            if (BazaPodataka.Automobili.ContainsKey(jedinstvenoIme))
            {
                BazaPodataka.Automobili.Remove(jedinstvenoIme);
            }
        }

        public void PromeniSnaguSunca(int novaVrednost)
        {
            MainWindow.SnagaSunca = novaVrednost;
        }
    }
}
