using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Simulator
{
    class Program
    {
        private static Dictionary<string, bool> uredjaji = new Dictionary<string, bool>();
        private static ISimulator proxy;

        static void Main(string[] args)
        {
            ISimulator proxy = new ChannelFactory<ISimulator>("ServisSimulator").CreateChannel();

            while (true)
            {
                Console.WriteLine("Meni :\n1.Solarni panel\n2.Potrosac\n");
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

                switch(unos)
                {
                    case 1:
                        Console.WriteLine("Solarni panel :\n1.Dodaj solarni panel\n2.Obrisi solarni panel\n");
                        string tekstSolarniPaneli = Console.ReadLine();
                        int unosSolarniPaneli = 0;
                        try
                        {
                            unosSolarniPaneli = int.Parse(tekstSolarniPaneli);
                        }
                        catch
                        {
                            Console.WriteLine("Mora se uneti broj !");
                        }

                        switch (unosSolarniPaneli)
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

                                if (maksimalnaSnaga > 0)
                                {
                                    new Thread(() => PokreniSolarnuPanelu(jedinstvenoIme, maksimalnaSnaga)).Start();
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
                        break;

                    case 2:
                        Console.WriteLine("Potrosac :\n1.Dodaj potrosac\n2.Obrisi potrosac\n");
                        string tekstPotrosaci = Console.ReadLine();
                        int unosPotrosaci = 0;
                        try
                        {
                            unosPotrosaci = int.Parse(tekstPotrosaci);
                        }
                        catch
                        {
                            Console.WriteLine("Mora se uneti broj !");
                        }

                        switch (unosPotrosaci)
                        {
                            case 1:
                                Console.WriteLine("Unesi jedinstveno ime potrosaca : ");
                                string jedinstvenoIme = Console.ReadLine();
                                Console.WriteLine("Unesi potrosnju potrosaca : ");
                                tekst = Console.ReadLine();
                                double potrosnja = -1;
                                try
                                {
                                    potrosnja = double.Parse(tekst);
                                }
                                catch
                                {
                                    Console.WriteLine("Potrosnja mora biti broj !");
                                }

                                if (potrosnja > 0)
                                {
                                    proxy.DodajPotrosac(new Potrosac(jedinstvenoIme, potrosnja));
                                }
                                else
                                {
                                    Console.WriteLine("Potrosnja mora biti broj veci od 0 !");
                                }

                                break;
                            case 2:
                                Console.WriteLine("Unesi jedinstveno ime potrosaca : ");
                                string imeBrisanje = Console.ReadLine();
                                proxy.UkloniSolarniPanel(imeBrisanje);
                                break;
                            default:
                                Console.WriteLine("Greska pri unosu !");
                                break;
                        }
                        break;  
                        
                    default:
                        Console.WriteLine("Greska pri unosu !");
                        break;
                }

            }
        }

        public static void PokreniSolarnuPanelu(string jedinstvenoIme, double maksimalnaSnaga)
        {

            bool jestePokrenut = false;
            proxy = new ChannelFactory<ISimulator>("ServisSimulator").CreateChannel();

            do
            {
                Thread.Sleep(100);
                if(uredjaji.ContainsKey(jedinstvenoIme))
                {
                    jestePokrenut = uredjaji[jedinstvenoIme];
                }
                if (!jestePokrenut)
                {
                    jestePokrenut = true;
                    uredjaji.Add(jedinstvenoIme, jestePokrenut);
                    proxy.DodajSolarniPanel(new SolarniPanel(jedinstvenoIme, maksimalnaSnaga));
                }
            } while (jestePokrenut);
            
            proxy.UkloniSolarniPanel(jedinstvenoIme);
        }
    }
}
