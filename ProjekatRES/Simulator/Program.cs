using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    class Program
    {
        static void Main(string[] args)
        {
            ISimulator proxy = new ChannelFactory<ISimulator>("ServisSimulator").CreateChannel();

            while (true)
            {
                Console.WriteLine("Meni :\n1.Dodaj solarni panel\n2.Ukloni solarni panel\n");
                string tekst = Console.ReadLine();
                int unos = 0;
                try
                {
                    unos = int.Parse(tekst);
                }
                catch
                {
                    Console.WriteLine("Mora se uneti broj !");
                }

                switch (unos)
                {
                    case 1:
                        Console.WriteLine("Unesi jedinstveno ime solarnog panela : ");
                        string jedinstvenoIme = Console.ReadLine();
                        Console.WriteLine("Unesi maksimalnu snagu solarnog panela : ");
                        tekst = Console.ReadLine();
                        double maksimalnaSnaga = -1;
                        try
                        {
                            maksimalnaSnaga = double.Parse(tekst);
                        }
                        catch
                        {
                            Console.WriteLine("Maksimalna snaga mora biti broj !");
                        }

                        if(maksimalnaSnaga > 0)
                        {
                            proxy.DodajSolarniPanel(new SolarniPanel(jedinstvenoIme, maksimalnaSnaga));
                        }
                        else
                        {
                            Console.WriteLine("Maksimalna snaga mora biti broj veci od 0 !");
                        }

                        break;
                    case 2:
                        Console.WriteLine("Unesi jedinstveno ime solarnog panela : ");
                        string imeBrisanje = Console.ReadLine();
                        proxy.UkloniSolarniPanel(imeBrisanje);
                        break;
                    default:
                        Console.WriteLine("Greska pri unosu !");
                        break;
                }
            }
        }
    }
}
