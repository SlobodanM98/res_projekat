using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SHES
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection connection;
        string connectionString;

        public static BindingList<Baterija> Baterije { get; set; }

        public MainWindow()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SHES.Properties.Settings.BazaPodatakaConnectionString"].ConnectionString;
            Baterije = new BindingList<Baterija>();

            UcitajUredjaje();

            InitializeComponent();

            new Thread(() => PokreniServer()).Start();

            DataContext = this;
        }

        void PokreniServer()
        {
            bool jestePokrenut = false;
            ServiceHost host = new ServiceHost(typeof(SimulatorServer));

            while (true)
            {
                Thread.Sleep(100);
                if (!jestePokrenut)
                {
                    jestePokrenut = true;
                    host.Open();
                }
            }
            host.Close();
        }

        void UcitajUredjaje()
        {
            string query = "SELECT * FROM Baterije";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
                using(SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);

                for(int i = 0; i < table.Rows.Count; i++)
                {
                    string jedinstvenoIme = table.Rows[i]["JedinstvenoIme"].ToString();
                    double maksimalnaSnaga = double.Parse(table.Rows[i]["MaksimalnaSnaga"].ToString());
                    int kapacitet = int.Parse(table.Rows[i]["Kapacitet"].ToString());
                    Baterije.Add(new Baterija(jedinstvenoIme, maksimalnaSnaga, kapacitet));
                }
            }
        }
    }
}
