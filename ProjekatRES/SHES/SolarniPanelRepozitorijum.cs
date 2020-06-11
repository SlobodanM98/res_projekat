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
    class SolarniPanelRepozitorijum : ISolarniPanelRepozitorijum
    {
        SqlConnection connection;
        string connectionString = ConfigurationManager.ConnectionStrings["SHES.Properties.Settings.BazaPodatakaConnectionString"].ConnectionString;

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
    }
}
