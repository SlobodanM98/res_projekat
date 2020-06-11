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
    class PotrosacRepozitorijum : IPotrosacRepozitorijum
    {
        SqlConnection connection;
        string connectionString = ConfigurationManager.ConnectionStrings["SHES.Properties.Settings.BazaPodatakaConnectionString"].ConnectionString;

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

        public void UgasiPotrosac(Potrosac p)
        {
            p.Upaljen = false;
            p.Slika = MaterialDesignThemes.Wpf.PackIconKind.PowerPlugOffOutline;
            string query = "UPDATE Potrosaci SET Upaljen=@down  WHERE JedinstvenoIme = '" + p.JedinstvenoIme + "'";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@down", SqlDbType.Bit).Value = false;
                connection.Open();
                command.ExecuteNonQuery();
            }
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



        public void UpaliPotrosac(Potrosac p)
        {
            p.Upaljen = true;
            p.Slika = MaterialDesignThemes.Wpf.PackIconKind.PowerPlugOutline;
            string query = "UPDATE Potrosaci SET Upaljen=@up  WHERE JedinstvenoIme = '" + p.JedinstvenoIme + "'";

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
