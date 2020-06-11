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
    class ElektricniAutomobilRepozitorijum : IElektricniAutomobilRepozitorijum
    {
        SqlConnection connection;
        string connectionString = ConfigurationManager.ConnectionStrings["SHES.Properties.Settings.BazaPodatakaConnectionString"].ConnectionString;

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

        public void IskljuciSaPunjaca(ElektricniAutomobil a)
        {
            string query = "UPDATE Automobili SET NaPunjacu=@up  WHERE JedinstvenoIme = '" + a.JedinstvenoIme + "'";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@up", SqlDbType.Bit).Value = false;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void PostavljanjeKapacitetaAuta(ElektricniAutomobil e, int trenutniKapacitet)
        {
            e.BaterijaAuta.TrenutniKapacitet = trenutniKapacitet;
        }

        public void UkljuciNaPunjac(ElektricniAutomobil a)
        {
            MainWindow.Punjac.NaPunjacu = true;
            MainWindow.Punjac.Automobil = a;
            a.NaPunjacu = true;
            string query = "UPDATE Automobili SET NaPunjacu=@up  WHERE JedinstvenoIme = '" + a.JedinstvenoIme + "'";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@up", SqlDbType.Bit).Value = true;
                connection.Open();
                command.ExecuteNonQuery();
            }
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
    }
}
