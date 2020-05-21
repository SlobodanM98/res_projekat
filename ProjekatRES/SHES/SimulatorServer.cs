﻿using Common;
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
            if (!BazaPodataka.SolarniPaneli.ContainsKey(noviPanel.JedinstvenoIme))
            {
                BazaPodataka.SolarniPaneli.Add(noviPanel.JedinstvenoIme,noviPanel);
            }
        }

        public void UkloniSolarniPanel(string jedinstvenoIme)
        {
            if (BazaPodataka.SolarniPaneli.ContainsKey(jedinstvenoIme))
            {
                BazaPodataka.SolarniPaneli.Remove(jedinstvenoIme);
            }
        }

        public void DodajPotrosaca(Potrosac potrosac)
        {
            if(!BazaPodataka.Potrosaci.ContainsKey(potrosac.JedinstvenoIme))
            {
                BazaPodataka.Potrosaci.Add(potrosac.JedinstvenoIme, potrosac);
            }
        }

        public void UkloniPotrosaca(string jedinstvenoIme)
        {
            if (BazaPodataka.Potrosaci.ContainsKey(jedinstvenoIme))
            {
                BazaPodataka.Potrosaci.Remove(jedinstvenoIme);
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

        public void DodajPunjacElektricnogAutomobila(PunjacElektricnogAutomobila punjac)
        {
            if(!BazaPodataka.Punjaci.ContainsKey(punjac.JedinstvenoIme))
            {
                BazaPodataka.Punjaci.Add(punjac.JedinstvenoIme, punjac);
            }
        }

        public void UkoloniPunjacElektricnogAutomobila(string jedinstvenoIme)
        {
            if(BazaPodataka.Punjaci.ContainsKey(jedinstvenoIme))
            {
                BazaPodataka.Punjaci.Remove(jedinstvenoIme);
            }
        }
    }
}
