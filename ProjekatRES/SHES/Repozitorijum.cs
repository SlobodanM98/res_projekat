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

        public void PodesavanjeCene(int cena)
        {
            MainWindow.cenovnik = cena;
        }

        public void PodesiOdnos(int noviOdnos)
        {
            MainWindow.jednaSekundaJe = noviOdnos;
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
    }
}
