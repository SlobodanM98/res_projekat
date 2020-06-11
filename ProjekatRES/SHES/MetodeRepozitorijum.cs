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
    public class MetodeRepozitorijum : IMetode
    {
        SqlConnection connection;
        string connectionString = ConfigurationManager.ConnectionStrings["SHES.Properties.Settings.BazaPodatakaConnectionString"].ConnectionString;

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

            if (baterijaAuta == true)
            {
                if (baterija.TrenutniKapacitet == 0)
                {
                    MainWindow.Punjac.Automobil.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery0;
                }
                else if (baterija.TrenutniKapacitet > 0 && baterija.TrenutniKapacitet <= baterija.Kapacitet * 20 / 100)
                {
                    MainWindow.Punjac.Automobil.Slika = MaterialDesignThemes.Wpf.PackIconKind.BatteryCharging10;
                }
                else if (baterija.TrenutniKapacitet > baterija.Kapacitet * 20 / 100 && baterija.TrenutniKapacitet <= baterija.Kapacitet * 40 / 100)
                {
                    MainWindow.Punjac.Automobil.Slika = MaterialDesignThemes.Wpf.PackIconKind.BatteryCharging30;
                }
                else if (baterija.TrenutniKapacitet > baterija.Kapacitet * 40 / 100 && baterija.TrenutniKapacitet <= baterija.Kapacitet * 60 / 100)
                {
                    MainWindow.Punjac.Automobil.Slika = MaterialDesignThemes.Wpf.PackIconKind.BatteryCharging50;
                }
                else if (baterija.TrenutniKapacitet > baterija.Kapacitet * 60 / 100 && baterija.TrenutniKapacitet <= baterija.Kapacitet * 80 / 100)
                {
                    MainWindow.Punjac.Automobil.Slika = MaterialDesignThemes.Wpf.PackIconKind.BatteryCharging70;
                }
                else if (baterija.TrenutniKapacitet > baterija.Kapacitet * 80 / 100 && baterija.TrenutniKapacitet <= baterija.Kapacitet * 95 / 100)
                {
                    MainWindow.Punjac.Automobil.Slika = MaterialDesignThemes.Wpf.PackIconKind.BatteryCharging90;
                }
                else
                {
                    MainWindow.Punjac.Automobil.Slika = MaterialDesignThemes.Wpf.PackIconKind.BatteryCharging100;
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
                foreach (PodaciZaGraf pzg in MainWindow.podaciZaGraf)
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
                                command.Parameters.AddWithValue("@Datum2", pzg.Datum.Date);
                                command.Parameters.AddWithValue("@Sat", pzg.Sat + 1);
                                int i = command.ExecuteNonQuery();
                            }
                            else
                            {
                                for (int i = 0; i < table.Rows.Count; i++)
                                {
                                    DateTime datum = DateTime.Parse(table.Rows[i]["Datum"].ToString());
                                    int sat = int.Parse(table.Rows[i]["Sat"].ToString());

                                    command.CommandText = "UPDATE Graf SET Baterije=@vb, Distribucija=@vd, SolarniPaneli=@vsp, Potrosaci=@vp WHERE Datum = @Datum2" + " and " + "Sat = @Sat";

                                    command.Parameters.AddWithValue("@Datum2", pzg.Datum.Date);
                                    command.Parameters.AddWithValue("@Sat", pzg.Sat + 1);
                                    command.Parameters.Add("@vb", SqlDbType.Float).Value = pzg.Baterije;
                                    command.Parameters.Add("@vd", SqlDbType.Float).Value = pzg.Distribucija;
                                    command.Parameters.Add("@vsp", SqlDbType.Float).Value = pzg.SolarniPaneli;
                                    command.Parameters.Add("@vp", SqlDbType.Float).Value = pzg.Potrosaci;

                                    int iss = command.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
                string queryDatumVreme = "UPDATE Vreme SET DatumVreme=@dv WHERE Id = 'DatumVreme'";
                using (SqlCommand command = new SqlCommand(queryDatumVreme, connection))
                {
                    command.Parameters.AddWithValue("@dv", MainWindow.trenutnoVreme);
                    command.ExecuteNonQuery();
                }
            }
            MainWindow.podaciZaGraf.Clear();
        }

        public void UcitajUredjaje()
        {
            string queryDatumVreme = "SELECT * FROM Vreme WHERE SnagaSunca IS NULL";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(queryDatumVreme, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);

                if (table.Rows.Count == 0)
                {
                    MainWindow.trenutnoVreme = DateTime.Now;
                    connection.Open();
                    command.CommandText = $"INSERT INTO Vreme Values(@id, @vreme, NULL)";
                    command.Parameters.AddWithValue("@id", "DatumVreme");
                    command.Parameters.AddWithValue("vreme", MainWindow.trenutnoVreme);
                    command.ExecuteNonQuery();
                }
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string id = table.Rows[i]["Id"].ToString();
                    MainWindow.trenutnoVreme = DateTime.Parse(table.Rows[i]["DatumVreme"].ToString());
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
                    MainWindow.SnagaSunca = 0;
                }
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string id = table.Rows[i]["Id"].ToString();
                    MainWindow.SnagaSunca = int.Parse(table.Rows[i]["SnagaSunca"].ToString());
                }
            }

            string queryDatum = "SELECT * FROM Graf WHERE Datum = @Datum and Sat = @Sat";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(queryDatum, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable table = new DataTable();
                command.Parameters.AddWithValue("@Datum", MainWindow.trenutnoVreme.Date);
                command.Parameters.AddWithValue("@Sat", MainWindow.trenutnoVreme.Hour + 1);
                adapter.Fill(table);

                if (table.Rows.Count > 0)
                {
                    MainWindow.baterije = double.Parse(table.Rows[0]["Baterije"].ToString()) * (-1);
                    MainWindow.distribucijaSat = double.Parse(table.Rows[0]["Distribucija"].ToString()) * (-1);
                    MainWindow.solarniPaneli = double.Parse(table.Rows[0]["SolarniPaneli"].ToString()) * (-1);
                    MainWindow.potrosaci = double.Parse(table.Rows[0]["Potrosaci"].ToString()) * (-1);
                }
                else
                {
                    MainWindow.baterije = 0;
                    MainWindow.distribucijaSat = 0;
                    MainWindow.solarniPaneli = 0;
                    MainWindow.potrosaci = 0;
                }
            }

            string queryBaterije = "SELECT * FROM Baterije WHERE AutomobilJedinstvenoIme IS NULL";

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
                    bool puniSe = bool.Parse(table.Rows[i]["PuniSe"].ToString());
                    bool prazniSe = bool.Parse(table.Rows[i]["PrazniSe"].ToString());
                    double trenutniKapacitet = double.Parse(table.Rows[i]["TrenutniKapacitet"].ToString());
                    Baterija novaBaterija = new Baterija(jedinstvenoIme, maksimalnaSnaga, kapacitet);
                    novaBaterija.PuniSe = puniSe;
                    novaBaterija.PrazniSe = prazniSe;
                    novaBaterija.TrenutniKapacitet = trenutniKapacitet;
                    if (novaBaterija.TrenutniKapacitet == 0)
                    {
                        novaBaterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery0;
                    }
                    else if (novaBaterija.TrenutniKapacitet > 0 && novaBaterija.TrenutniKapacitet <= novaBaterija.Kapacitet * 20 / 100)
                    {
                        novaBaterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery10;
                    }
                    else if (novaBaterija.TrenutniKapacitet > novaBaterija.Kapacitet * 20 / 100 && novaBaterija.TrenutniKapacitet <= novaBaterija.Kapacitet * 40 / 100)
                    {
                        novaBaterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery30;
                    }
                    else if (novaBaterija.TrenutniKapacitet > novaBaterija.Kapacitet * 40 / 100 && novaBaterija.TrenutniKapacitet <= novaBaterija.Kapacitet * 60 / 100)
                    {
                        novaBaterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery50;
                    }
                    else if (novaBaterija.TrenutniKapacitet > novaBaterija.Kapacitet * 60 / 100 && novaBaterija.TrenutniKapacitet <= novaBaterija.Kapacitet * 80 / 100)
                    {
                        novaBaterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery70;
                    }
                    else if (novaBaterija.TrenutniKapacitet > novaBaterija.Kapacitet * 80 / 100 && novaBaterija.TrenutniKapacitet <= novaBaterija.Kapacitet * 95 / 100)
                    {
                        novaBaterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery90;
                    }
                    else
                    {
                        novaBaterija.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery100;
                    }
                    MainWindow.Baterije.Add(novaBaterija);
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
                    MainWindow.autoBaterije.Add(novaBaterija);
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
                    if (stanje)
                    {
                        novi.Slika = MaterialDesignThemes.Wpf.PackIconKind.PowerPlugOutline;
                    }
                    MainWindow.Potrosaci.Add(novi);
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
                    MainWindow.SolarniPaneli.Add(novi);
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
                    Baterija baterija = MainWindow.autoBaterije.Find(b => b.AutomobilJedinstvenoIme.Equals(jedinstvenoIme));
                    ElektricniAutomobil noviAuto = new ElektricniAutomobil(baterija, jedinstvenoIme, naPunjacu, puniSe);

                    if (naPunjacu == true)
                    {
                        MainWindow.Punjac.Automobil = noviAuto;
                        MainWindow.Punjac.NaPunjacu = true;
                        if (puniSe == true)
                        {
                            MainWindow.Punjac.PuniSe = true;
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
                    else if (noviAuto.BaterijaAuta.TrenutniKapacitet > noviAuto.BaterijaAuta.Kapacitet * 20 / 100 && noviAuto.BaterijaAuta.TrenutniKapacitet <= noviAuto.BaterijaAuta.Kapacitet * 40 / 100)
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
                    MainWindow.ElektricniAutomobili.Add(noviAuto);
                }
            }
        }

        public void UcitajDatume()
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                MainWindow.Datumi.Clear();
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
                        MainWindow.Datumi.Add(new Datum(DateTime.Parse(table.Rows[i]["Datum"].ToString()).ToString("d/M/yyyy")));
                    }
                }
            });
        }

        public void UcitajPoslednjiSat()
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                MainWindow.Datumi.Clear();
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
                        MainWindow.Datumi.Add(new Datum(DateTime.Parse(table.Rows[i]["Datum"].ToString()).ToString("d/M/yyyy")));
                    }
                }
            });
        }

        public void ZapamtiVreme()
        {
            string query = "UPDATE Vreme SET DatumVreme=@dv WHERE Id = 'DatumVreme'";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@dv", MainWindow.trenutnoVreme);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
