using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHES
{
    public class BaterijaRepozitorijum : IBaterijaRepozitorijum
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
    }
}
