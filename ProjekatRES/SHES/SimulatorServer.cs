using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SHES
{
    public class SimulatorServer : ISimulator
    {
        private IRepozitorijum repozitorijum;

        public SimulatorServer(IRepozitorijum repo)
        {
            repozitorijum = repo;
        }

        public SimulatorServer() { }

        public void DodajSolarniPanel(SolarniPanel noviPanel)
        {
            bool sadrzi = false;
            foreach (SolarniPanel sp in MainWindow.SolarniPaneli)
            {
                if (sp.JedinstvenoIme == noviPanel.JedinstvenoIme)
                {
                    sadrzi = true;
                    break;
                }
            }

            if (!sadrzi)
            {
                if(repozitorijum == null)
                {
                    repozitorijum = new Repozitorijum();
                }
                repozitorijum.DodajSolarniPanel(noviPanel);
            }
        }

        public void UkloniSolarniPanel(string jedinstvenoIme)
        {
            foreach (SolarniPanel sp in MainWindow.SolarniPaneli)
            {
                if (sp.JedinstvenoIme == jedinstvenoIme)
                {
                    if (repozitorijum == null)
                    {
                        repozitorijum = new Repozitorijum();
                    }
                    repozitorijum.UkloniSolarniPanel(sp);
                    break;
                }
            }
        }

        public void DodajPotrosaca(Potrosac potrosac)
        {
            bool sadrzi = false;
            foreach(Potrosac p in MainWindow.Potrosaci)
            {
                if(p.JedinstvenoIme == potrosac.JedinstvenoIme)
                {
                    sadrzi = true;
                    break;
                }
            }
            if (!sadrzi)
            {
                if (repozitorijum == null)
                {
                    repozitorijum = new Repozitorijum(); 
                }
                repozitorijum.DodajPotrosaca(potrosac);
            }
        }

        public void UkloniPotrosaca(string jedinstvenoIme)
        {
            foreach(Potrosac p in MainWindow.Potrosaci)
            {
                if(p.JedinstvenoIme == jedinstvenoIme)
                {
                    if (repozitorijum == null)
                    {
                        repozitorijum = new Repozitorijum();
                    }
                    repozitorijum.UkloniPotrosaca(p);
                    break;
                }
            }
        }

        public void UpaliPotrosac(string jedinstvenoIme)
        {
            foreach(Potrosac p in MainWindow.Potrosaci)
            {
                if(p.JedinstvenoIme == jedinstvenoIme)
                {
                    if (repozitorijum == null)
                    {
                        repozitorijum = new Repozitorijum();
                    }
                    repozitorijum.UpaliPotrosac(p);
                    break;
                }
            }
        }

        public void UgasiPotrosac(string jedinstvenoIme)
        {
            foreach (Potrosac p in MainWindow.Potrosaci)
            {
                if (p.JedinstvenoIme == jedinstvenoIme)
                {
                    if (repozitorijum == null)
                    {
                        repozitorijum = new Repozitorijum();
                    }
                    repozitorijum.UgasiPotrosac(p);
                    break;
                }
            }
        }

        public void DodajBateriju(Baterija novaBaterija, bool jesteAutomobil, string AutomobilJedinstvenoIme)
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
                if (repozitorijum == null)
                {
                    repozitorijum = new Repozitorijum();
                }
                repozitorijum.DodajBateriju(novaBaterija, jesteAutomobil, AutomobilJedinstvenoIme);
            }
        }

        public void UkloniBateriju(string jedinstvenoIme)
        {
            foreach(Baterija b in MainWindow.Baterije)
            {
                if(b.JedinstvenoIme == jedinstvenoIme)
                {
                    if (repozitorijum == null)
                    {
                        repozitorijum = new Repozitorijum();
                    }
                    repozitorijum.UkloniBateriju(b);
                    break;
                }
            }
        }

        public void DodajElektricniAutomobil(ElektricniAutomobil automobil)
        {
            bool sadrzi = false;
            foreach (ElektricniAutomobil e in MainWindow.ElektricniAutomobili)
            {
                if (e.JedinstvenoIme == automobil.JedinstvenoIme)
                {
                    sadrzi = true;
                    break;
                }
            }
            if (!sadrzi)
            {
                if (repozitorijum == null)
                {
                    repozitorijum = new Repozitorijum();
                }
                repozitorijum.DodajElektricniAutomobil(automobil);
                DodajBateriju(automobil.BaterijaAuta, true, automobil.JedinstvenoIme);
            }
        }

        public void UkloniElektricniAutomobil(string jedinstvenoIme)
        {
            foreach (ElektricniAutomobil a in MainWindow.ElektricniAutomobili)
            {
                if (a.JedinstvenoIme == jedinstvenoIme)
                {
                    if (repozitorijum == null)
                    {
                        repozitorijum = new Repozitorijum();
                    }
                    ZaustaviPunjenje(jedinstvenoIme);
                    IskljuciSaPunjaca(jedinstvenoIme);
                    repozitorijum.UkloniElektricniAutomobil(a);
                    break;
                }
            }
        }

        public void PromeniSnaguSunca(int novaVrednost)
        {
            MainWindow.SnagaSunca = novaVrednost;
            if (repozitorijum == null)
            {
                repozitorijum = new Repozitorijum();
            }
            repozitorijum.PromeniSnaguSunca(novaVrednost);
        }

        public bool UkljuciNaPunjac(string jedinstvenoIme)
        {
            if (MainWindow.Punjac.NaPunjacu)
            {
                return false;
            }
            
            foreach(ElektricniAutomobil e in MainWindow.ElektricniAutomobili)
            {
                if(e.JedinstvenoIme == jedinstvenoIme)
                {
                    if (repozitorijum == null)
                    {
                        repozitorijum = new Repozitorijum();
                    }
                    repozitorijum.UkljuciNaPunjac(e);
                    return true;
                }
            }

            return false;
        }

        public bool IskljuciSaPunjaca(string jedinstvenoIme)
        {
            if (!MainWindow.Punjac.NaPunjacu)
            {
                return false;
            }
            foreach (ElektricniAutomobil e in MainWindow.ElektricniAutomobili)
            {
                if (e.JedinstvenoIme == jedinstvenoIme)
                {
                    MainWindow.Punjac.Automobil = null;
                    e.NaPunjacu = false;
                    e.PuniSe = false;
                    MainWindow.Punjac.NaPunjacu = false;
                    MainWindow.Punjac.PuniSe = false;

                    if (e.BaterijaAuta.TrenutniKapacitet == 0)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery0;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > 0 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 20 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery10;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 20 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 40 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery30;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 40 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 60 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery50;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 60 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 80 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery70;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 80 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 95 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery90;
                    }
                    else
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery100;
                    }
                    if (repozitorijum == null)
                    {
                        repozitorijum = new Repozitorijum();
                    }
                    repozitorijum.IskljuciSaPunjaca(e);
                    break;
                }
            }
            return true;
        }

        public bool PokreniPunjenje(string jedinstvenoIme)
        {
            if(!MainWindow.Punjac.NaPunjacu || MainWindow.Punjac.PuniSe)
            {
                return false;
            }
            if (MainWindow.Punjac.Automobil.JedinstvenoIme == jedinstvenoIme)
            {
                
                foreach (ElektricniAutomobil e in MainWindow.ElektricniAutomobili)
                {
                    if (e.JedinstvenoIme == jedinstvenoIme)
                    {
                        e.PuniSe = true;
                        MainWindow.Punjac.PuniSe = true;
                        if (repozitorijum == null)
                        {
                            repozitorijum = new Repozitorijum();
                        }
                        repozitorijum.PokreniPunjenje(e);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ZaustaviPunjenje(string jedinstvenoIme)
        {
            if (!MainWindow.Punjac.NaPunjacu || !MainWindow.Punjac.PuniSe)
            {
                return false;
            }
            foreach (ElektricniAutomobil e in MainWindow.ElektricniAutomobili)
            {
                if (e.JedinstvenoIme == jedinstvenoIme)
                {
                    if(e.JedinstvenoIme == MainWindow.Punjac.Automobil.JedinstvenoIme)
                    {
                        MainWindow.Punjac.PuniSe = false;
                        e.PuniSe = false;
                        if (e.BaterijaAuta.TrenutniKapacitet == 0)
                        {
                            e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery0;
                        }
                        else if (e.BaterijaAuta.TrenutniKapacitet > 0 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 20 / 100)
                        {
                            e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery10;
                        }
                        else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 20 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 40 / 100)
                        {
                            e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery30;
                        }
                        else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 40 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 60 / 100)
                        {
                            e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery50;
                        }
                        else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 60 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 80 / 100)
                        {
                            e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery70;
                        }
                        else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 80 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 95 / 100)
                        {
                            e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery90;
                        }
                        else
                        {
                            e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery100;
                        }
                        if (repozitorijum == null)
                        {
                            repozitorijum = new Repozitorijum();
                        }
                        repozitorijum.ZaustaviPunjenje(e);
                        return true;
                        //break;
                    }
                }
            }
            return false;
        }

        public bool PostavljanjeKapacitetaAuta(int trenutniKapacitet)
        {
            if (!MainWindow.Punjac.NaPunjacu || !MainWindow.Punjac.PuniSe)
            {
                return false;
            }
            foreach (ElektricniAutomobil e in MainWindow.ElektricniAutomobili)
            {
                if (e.JedinstvenoIme == MainWindow.Punjac.Automobil.JedinstvenoIme)
                {
                    e.BaterijaAuta.TrenutniKapacitet = trenutniKapacitet;
                    if (e.BaterijaAuta.TrenutniKapacitet == 0)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery0;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > 0 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 20 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery10;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 20 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 40 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery30;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 40 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 60 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery50;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 60 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 80 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery70;
                    }
                    else if (e.BaterijaAuta.TrenutniKapacitet > e.BaterijaAuta.Kapacitet * 80 / 100 && e.BaterijaAuta.TrenutniKapacitet <= e.BaterijaAuta.Kapacitet * 95 / 100)
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery90;
                    }
                    else
                    {
                        e.Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery100;
                    }

                    break;
                }
            }
            return true;
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

        public void PodesiOdnos(int noviOdnos)
        {
            MainWindow.jednaSekundaJe = noviOdnos;
        }

        public void PodesavanjeCene(int cena)
        {
            MainWindow.cenovnik = cena;
        }
    }
}
