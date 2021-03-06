﻿using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHES
{
    public class ElektricniAutomobilServer : IElektricniAutomobil
    {
        IElektricniAutomobilRepozitorijum repozitorijum;
        IBaterijaRepozitorijum baterijaRepozitorijum;

        public ElektricniAutomobilServer() { }

        public ElektricniAutomobilServer(IElektricniAutomobilRepozitorijum repo, IBaterijaRepozitorijum batRepo)
        {
            repozitorijum = repo;
            baterijaRepozitorijum = batRepo;
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
                    repozitorijum = new ElektricniAutomobilRepozitorijum();
                }
                if(baterijaRepozitorijum == null)
                {
                    baterijaRepozitorijum = new BaterijaRepozitorijum();
                }
                repozitorijum.DodajElektricniAutomobil(automobil);
                //DodajBateriju(automobil.BaterijaAuta, true, automobil.JedinstvenoIme);
                BaterijaServer bs = new BaterijaServer(baterijaRepozitorijum);
                bs.DodajBateriju(automobil.BaterijaAuta, true, automobil.JedinstvenoIme);
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
                        repozitorijum = new ElektricniAutomobilRepozitorijum();
                    }
                    ZaustaviPunjenje(jedinstvenoIme);
                    IskljuciSaPunjaca(jedinstvenoIme);
                    repozitorijum.UkloniElektricniAutomobil(a);
                    break;
                }
            }
        }

        public bool UkljuciNaPunjac(string jedinstvenoIme)
        {
            if (MainWindow.Punjac.NaPunjacu)
            {
                return false;
            }

            foreach (ElektricniAutomobil e in MainWindow.ElektricniAutomobili)
            {
                if (e.JedinstvenoIme == jedinstvenoIme)
                {
                    if (repozitorijum == null)
                    {
                        repozitorijum = new ElektricniAutomobilRepozitorijum();
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
                        repozitorijum = new ElektricniAutomobilRepozitorijum();
                    }
                    repozitorijum.IskljuciSaPunjaca(e);
                    break;
                }
            }
            return true;
        }

        public bool PokreniPunjenje(string jedinstvenoIme)
        {
            if (!MainWindow.Punjac.NaPunjacu || MainWindow.Punjac.PuniSe)
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
                            repozitorijum = new ElektricniAutomobilRepozitorijum();
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
                    if (e.JedinstvenoIme == MainWindow.Punjac.Automobil.JedinstvenoIme)
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
                            repozitorijum = new ElektricniAutomobilRepozitorijum();
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
                    if (repozitorijum == null)
                    {
                        repozitorijum = new ElektricniAutomobilRepozitorijum();
                    }

                    repozitorijum.PostavljanjeKapacitetaAuta(e, trenutniKapacitet);

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

                    return true;
                }
            }
            return false;
        }

    }
}
