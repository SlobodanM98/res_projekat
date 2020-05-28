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
        private static Dictionary<string, bool> solarniPaneli = new Dictionary<string, bool>();
        private static Dictionary<string, bool> potrosaci = new Dictionary<string, bool>();
        private static Dictionary<string, bool> baterije = new Dictionary<string, bool>();
        private static Dictionary<string, bool> automobili = new Dictionary<string, bool>();
        private static ISimulator proxy = new ChannelFactory<ISimulator>("ServisSimulator").CreateChannel();
        private static int brojBaterije = 0;

        private static string zaUkljucivanje = "";
        private static string zaIskljucivanje = "";
        private static string zaPokretanjePunjenja = "";
        private static string zaZaustavljanjePunjenja = "";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Meni :\n1.Solarni panel\n2.Potrosac\n3.Baterija\n4.Elektricni automobil\n5.Snaga sunca\n6.Punjac");
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

                                if (maksimalnaSnaga > 0 && !solarniPaneli.ContainsKey(jedinstvenoIme))
                                {
                                    new Thread(() => PokreniSolarnuPanelu(jedinstvenoIme, maksimalnaSnaga)).Start();
                                }
                                else
                                {
                                    Console.WriteLine("Maksimalna snaga mora biti broj veci od 0 i ime mora biti jedistveno!");
                                }

                                break;
                            case 2:
                                Console.WriteLine("Unesi jedinstveno ime solarnog panela : ");
                                string imeBrisanje = Console.ReadLine();
                                if (solarniPaneli.ContainsKey(imeBrisanje))
                                {
                                    solarniPaneli[imeBrisanje] = false;
                                }
                                else
                                {
                                    Console.WriteLine("Ne postoji solarni panel sa unetim imenom !");
                                }
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

                                if (potrosnja > 0 && !potrosaci.ContainsKey(jedinstvenoIme))
                                {
                                    new Thread(() => PokreniPotrosac(jedinstvenoIme, potrosnja)).Start();
                                }
                                else
                                {
                                    Console.WriteLine("Potrosnja mora biti broj veci od 0 i ime mora biti jedistveno!");
                                }

                                break;
                            case 2:
                                Console.WriteLine("Unesi jedinstveno ime potrosaca : ");
                                string imeBrisanje = Console.ReadLine();
                                if (potrosaci.ContainsKey(imeBrisanje))
                                {
                                    potrosaci[imeBrisanje] = false;
                                }
                                else
                                {
                                    Console.WriteLine("Ne postoji potrosac sa unetim imenom !");
                                }
                                break;
                            default:
                                Console.WriteLine("Greska pri unosu !");
                                break;
                        }
                        break;

                    case 3:
                        Console.WriteLine("Baterija :\n1.Dodaj bateriju\n2.Obrisi bateriju\n");
                        string tekstBaterija = Console.ReadLine();
                        int unosBaterija = 0;
                        try
                        {
                            unosBaterija = int.Parse(tekstBaterija);
                        }
                        catch
                        {
                            Console.WriteLine("Mora se uneti broj !");
                        }

                        switch (unosBaterija)
                        {
                            case 1:
                                Console.WriteLine("Unesi jedinstveno ime baterije : ");
                                string jedinstvenoIme = Console.ReadLine();
                                Console.WriteLine("Unesi maksimalnu snagu baterije : ");
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

                                if (maksimalnaSnaga > 0 && !baterije.ContainsKey(jedinstvenoIme))
                                {
                                    new Thread(() => PokreniBateriju(jedinstvenoIme, maksimalnaSnaga)).Start();
                                }
                                else
                                {
                                    Console.WriteLine("Maksimalna snaga mora biti broj veci od 0 i ime mora biti jedistveno!");
                                }
                                break;
                            case 2:
                                Console.WriteLine("Unesi jedinstveno ime baterije : ");
                                string imeBrisanje = Console.ReadLine();
                                if (baterije.ContainsKey(imeBrisanje))
                                {
                                    baterije[imeBrisanje] = false;
                                }
                                else
                                {
                                    Console.WriteLine("Ne postoji baterija sa unetim imenom !");
                                }
                                break;
                            default:
                                Console.WriteLine("Greska pri unosu !");
                                break;
                        }
                        break;

                    case 4:
                        Console.WriteLine("Elektricni automobil :\n1.Dodaj elektricni automobila\n2.Obrisi elektricni automobil\n");
                        string tekstAuto = Console.ReadLine();
                        int unosAuto = 0;
                        try
                        {
                            unosAuto = int.Parse(tekstAuto);
                        }
                        catch
                        {
                            Console.WriteLine("Mora se uneti broj !");
                        }

                        switch (unosAuto)
                        {
                            case 1:
                                Console.WriteLine("Unesi jedinstveno ime elektricnog automobila : ");
                                string jedinstvenoIme = Console.ReadLine();

                                Console.WriteLine("Unesi snagu baterije : ");
                                string snaga = Console.ReadLine();
                                double snagaBaterije = 0;
                                
                                try
                                {
                                    snagaBaterije = double.Parse(snaga);
                                }
                                catch
                                {
                                    Console.WriteLine("Mora se uneti broj !");
                                }

                                Console.WriteLine("Unesi kapacitet baterije : ");
                                string kapacitet = Console.ReadLine();
                                int kapacitetBaterije = 0;

                                try
                                {
                                    kapacitetBaterije = int.Parse(kapacitet);
                                }
                                catch
                                {
                                    Console.WriteLine("Mora se uneti broj !");
                                }

                                if (!automobili.ContainsKey(jedinstvenoIme))
                                {
                                    new Thread(() => PokreniElektricniAutomobil(jedinstvenoIme, snagaBaterije, kapacitetBaterije)).Start();
                                }
                                else
                                {
                                    Console.WriteLine("Ime mora biti jedinstveno !");
                                }
                                
                                break;
                            case 2:
                                Console.WriteLine("Unesi jedinstveno ime punjaca elektricnog automobila : ");
                                string imeBrisanje = Console.ReadLine();
                                if (automobili.ContainsKey(imeBrisanje))
                                {
                                    automobili[imeBrisanje] = false;
                                }
                                else
                                {
                                    Console.WriteLine("Ne postoji punjac elektricnog automobila sa unetim imenom !");
                                }
                                break;
                            default:
                                Console.WriteLine("Greska pri unosu !");
                                break;
                        }
                        break;
                    case 5:
                        Console.WriteLine("Unesi novu snagu sunca : ");
                        string unetaSnagaSunca = Console.ReadLine();
                        int snagaSunca = -1;
                        try
                        {
                            snagaSunca = int.Parse(unetaSnagaSunca);
                        }
                        catch
                        {
                            Console.WriteLine("Mora se uneti broj !");
                        }

                        if(snagaSunca >= 0 && snagaSunca <= 100)
                        {
                            proxy.PromeniSnaguSunca(snagaSunca);
                        }
                        else
                        {
                            Console.WriteLine("Snaga sunca mora biti broj u intervalu izmedju 0 i 100 !");
                        }
                        break;
                    case 6:
                        Console.WriteLine("1.Ukljuci na punjac\n2.Iskljuci sa punjaca\n3.Pokreni punjenje\n4.Zaustavi punjenje");
                        string tekstPunjac = Console.ReadLine();
                        int unosPunjac = 0;
                        try
                        {
                            unosPunjac = int.Parse(tekstPunjac);
                        }
                        catch
                        {
                            Console.WriteLine("Mora se uneti broj !");
                        }
                        switch (unosPunjac)
                        {
                            case 1:
                                Console.WriteLine("Unesi jedinstveno ime automobila : ");
                                string imeAuto = Console.ReadLine();
                                if (automobili.ContainsKey(imeAuto))
                                {
                                    zaUkljucivanje = imeAuto;
                                }
                                else
                                {
                                    Console.WriteLine("Ne postoji automobil sa unetim imenom!");
                                }
                                break;
                            case 2:
                                Console.WriteLine("Unesi jedinstveno ime automobila : ");
                                imeAuto = Console.ReadLine();
                                if (automobili.ContainsKey(imeAuto))
                                {
                                    zaIskljucivanje = imeAuto;
                                }
                                else
                                {
                                    Console.WriteLine("Ne postoji automobil sa unetim imenom!");
                                }
                                break;
                            case 3:
                                Console.WriteLine("Unesi jedinstveno ime automobila : ");
                                imeAuto = Console.ReadLine();
                                if (automobili.ContainsKey(imeAuto))
                                {
                                    zaPokretanjePunjenja = imeAuto;
                                }
                                else
                                {
                                    Console.WriteLine("Ne postoji automobil sa unetim imenom!");
                                }
                                break;
                            case 4:
                                Console.WriteLine("Unesi jedinstveno ime automobila : ");
                                imeAuto = Console.ReadLine();
                                if (automobili.ContainsKey(imeAuto))
                                {
                                    zaZaustavljanjePunjenja = imeAuto;
                                }
                                else
                                {
                                    Console.WriteLine("Ne postoji automobil sa unetim imenom!");
                                }
                                break;
                            default:
                                Console.WriteLine("Greska pri unosu!");
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

            bool jestePokrenut = true;
            solarniPaneli.Add(jedinstvenoIme, jestePokrenut);
            proxy.DodajSolarniPanel(new SolarniPanel(jedinstvenoIme, maksimalnaSnaga));

            do
            {
                Thread.Sleep(100);
                jestePokrenut = solarniPaneli[jedinstvenoIme];
            } while (jestePokrenut);
            
            proxy.UkloniSolarniPanel(jedinstvenoIme);
            solarniPaneli.Remove(jedinstvenoIme);
        }

        public static void PokreniPotrosac(string jedinstvenoIme, double potrosnja)
        {
            bool jestePokrenut = true;
            potrosaci.Add(jedinstvenoIme, jestePokrenut);
            proxy.DodajPotrosaca(new Potrosac(jedinstvenoIme, potrosnja));

            do
            {
                Thread.Sleep(100);
                jestePokrenut = potrosaci[jedinstvenoIme];
            } while (jestePokrenut);

            proxy.UkloniPotrosaca(jedinstvenoIme);
            potrosaci.Remove(jedinstvenoIme);
        }

        public static void PokreniBateriju(string jedinstvenoIme, double maksimalnaSnaga)
        {
            bool jestePokrenut = true;
            baterije.Add(jedinstvenoIme, jestePokrenut);
            Console.WriteLine("Dodata baterija !");
            proxy.DodajBateriju(new Baterija(jedinstvenoIme, maksimalnaSnaga, 0), false, "");

            do
            {
                Thread.Sleep(100);
                jestePokrenut = baterije[jedinstvenoIme];
            } while (jestePokrenut);

            proxy.UkloniBateriju(jedinstvenoIme);
            baterije.Remove(jedinstvenoIme);
            Console.WriteLine("Obrisana baterija !");
        }

        public static void PokreniElektricniAutomobil(string jedinstvenoIme, double snaga, int kapacitet)
        {
            bool jestePokrenut = true;
            automobili.Add(jedinstvenoIme, jestePokrenut);
            proxy.DodajElektricniAutomobil(new ElektricniAutomobil(jedinstvenoIme, snaga, kapacitet, brojBaterije));
            brojBaterije++;

            do
            {
                Thread.Sleep(100);
                if(zaUkljucivanje == jedinstvenoIme)
                {
                    proxy.UkljuciNaPunjac(jedinstvenoIme);
                    zaUkljucivanje = "";
                }
                if(zaIskljucivanje == jedinstvenoIme)
                {
                    proxy.IskljuciSaPunjaca(jedinstvenoIme);
                    zaIskljucivanje = "";
                }
                if (zaPokretanjePunjenja == jedinstvenoIme)
                {
                    proxy.PokreniPunjenje();
                    zaPokretanjePunjenja = "";
                }
                if (zaZaustavljanjePunjenja == jedinstvenoIme)
                {
                    proxy.ZaustaviPunjenje();
                    zaZaustavljanjePunjenja = "";
                }
                jestePokrenut = automobili[jedinstvenoIme];
            } while (jestePokrenut);

            proxy.UkloniElektricniAutomobil(jedinstvenoIme);
            automobili.Remove(jedinstvenoIme);
        }
    }
}
