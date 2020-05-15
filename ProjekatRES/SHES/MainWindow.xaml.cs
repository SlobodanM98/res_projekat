using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();

            new Thread(() => PokreniServer()).Start();
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
    }
}
