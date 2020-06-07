using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHES
{
    public class Repozitorijum : IRepozitorijum
    {
        SqlConnection connection;
        string connectionString = ConfigurationManager.ConnectionStrings["SHES.Properties.Settings.BazaPodatakaConnectionString"].ConnectionString;

        public void DodajBateriju(Baterija novaBaterija, bool jesteAutomobil, string AutomobilJedinstvenoIme)
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

        public void DodajElektricniAutomobil(ElektricniAutomobil automobil)
        {
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
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                MainWindow.ElektricniAutomobili.Add(automobil);
            });
        }

        public void DodajPotrosaca(Potrosac potrosac)
        {
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
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                MainWindow.Potrosaci.Add(potrosac);
            });
        }

        public void DodajSolarniPanel(SolarniPanel noviPanel)
        {
            string query = $"INSERT INTO SolarnePanele VALUES (@jedinstvenoIme, {noviPanel.MaksimalnaSnaga})";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@jedinstvenoIme", noviPanel.JedinstvenoIme);
                command.ExecuteNonQuery();
            }

            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                MainWindow.SolarniPaneli.Add(noviPanel);
            });
        }

        public void IskljuciSaPunjaca(string jedinstvenoIme)
        {
            string query = "UPDATE Automobili SET NaPunjacu=@up  WHERE JedinstvenoIme = '" + jedinstvenoIme + "'";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@up", SqlDbType.Bit).Value = false;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void PokreniPunjenje(ElektricniAutomobil e)
        {
            string query = "UPDATE Automobili SET Punise=@up  WHERE JedinstvenoIme = '" + e.JedinstvenoIme + "'";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@up", SqlDbType.Bit).Value = true;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void PromeniSnaguSunca(int novaVrednost)
        {
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

        public void UgasiPotrosac(string jedinstvenoIme)
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

        public void UkljuciNaPunjac(string jedinstvenoIme)
        {
            string query = "UPDATE Automobili SET NaPunjacu=@up  WHERE JedinstvenoIme = '" + jedinstvenoIme + "'";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@up", SqlDbType.Bit).Value = true;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UkloniBateriju(Baterija b)
        {
            string query = "DELETE FROM Baterije WHERE JedinstvenoIme = '" + b.JedinstvenoIme + "'";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                MainWindow.Baterije.Remove(b);
            });
        }

        public void UkloniElektricniAutomobil(ElektricniAutomobil a)
        {
            string query = "DELETE FROM Automobili WHERE JedinstvenoIme = '" + a.JedinstvenoIme + "'";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
            }

            query = "DELETE FROM Baterije WHERE AutomobilJedinstvenoIme = '" + a.JedinstvenoIme + "'";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
            }

            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                MainWindow.ElektricniAutomobili.Remove(a);
                MainWindow.autoBaterije.Remove(a.BaterijaAuta);
            });
        }

        public void UkloniPotrosaca(Potrosac p)
        {
            string query = "DELETE FROM Potrosaci WHERE JedinstvenoIme = '" + p.JedinstvenoIme + "'";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                MainWindow.Potrosaci.Remove(p);
            });
        }

        public void UkloniSolarniPanel(SolarniPanel sp)
        {
            string query = "DELETE FROM SolarnePanele WHERE JedinstvenoIme = '" + sp.JedinstvenoIme + "'";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                MainWindow.SolarniPaneli.Remove(sp);
            });
        }

        public void UpaliPotrosac(string jedinstvenoIme)
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

        public void ZaustaviPunjenje(ElektricniAutomobil e)
        {
            string query = "UPDATE Automobili SET Punise=@up  WHERE JedinstvenoIme = '" + e.JedinstvenoIme + "'";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@up", SqlDbType.Bit).Value = false;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
