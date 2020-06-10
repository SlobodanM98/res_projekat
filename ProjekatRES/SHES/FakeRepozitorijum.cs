﻿using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHES
{
    public class FakeRepozitorijum : IRepozitorijum
    {
        public List<SolarniPanel> solarniPaneli = new List<SolarniPanel>();
        public List<Potrosac> potrosaci = new List<Potrosac>();
        public List<Baterija> baterije = new List<Baterija>();
        public List<ElektricniAutomobil> automobili = new List<ElektricniAutomobil>();
        public int snagaSunca = 0;
        public bool puniSePunjac = false;
        public int jednaSekundaJe = 0;
        public int cenovnik = 0;

        public void DodajBateriju(Baterija novaBaterija, bool jesteAutomobil, string AutomobilJedinstvenoIme)
        {
            baterije.Add(novaBaterija);
        }

        public void DodajElektricniAutomobil(ElektricniAutomobil automobil)
        {
            automobili.Add(automobil);
        }

        public void DodajPotrosaca(Potrosac potrosac)
        {
            potrosaci.Add(potrosac);
        }

        public void DodajSolarniPanel(SolarniPanel noviPanel)
        {
            solarniPaneli.Add(noviPanel);
        }

        public void IskljuciSaPunjaca(ElektricniAutomobil e)
        {
            int i = 0;
            foreach (ElektricniAutomobil automobil in automobili)
            {
                if (automobil.JedinstvenoIme == e.JedinstvenoIme)
                {
                    automobil.NaPunjacu = false;
                    automobil.PuniSe = false;
                    break;
                }
                i++;
            }
        }

        public void PodesavanjeCene(int cena)
        {
            cenovnik = cena;
        }

        public void PodesiOdnos(int noviOdnos)
        {
            jednaSekundaJe = noviOdnos;
        }

        public void PokreniPunjenje(ElektricniAutomobil e)
        {
            int i = 0;
            puniSePunjac = true;
            foreach (ElektricniAutomobil automobil in automobili)
            {
                if (automobil.JedinstvenoIme == e.JedinstvenoIme)
                {
                    automobil.PuniSe = true;
                    break;
                }
                i++;
            }
            
        }

        public void PostavljanjeKapacitetaAuta(ElektricniAutomobil e, int trenutniKapacitet)
        {
            int i = 0;
            foreach (ElektricniAutomobil automobil in automobili)
            {
                if (automobil.JedinstvenoIme == e.JedinstvenoIme)
                {
                    automobil.BaterijaAuta.TrenutniKapacitet = trenutniKapacitet;
                    break;
                }
                i++;
            }
        }

        public Uredjaji PreuzmiUredjaje()
        {
            Uredjaji uredjaji = new Uredjaji();
            uredjaji.Automobili = new List<ElektricniAutomobil>();
            foreach(ElektricniAutomobil a in automobili)
            {
                uredjaji.Automobili.Add(a);
            }
            uredjaji.Baterije = new List<Baterija>();
            foreach(Baterija b in baterije)
            {
                uredjaji.Baterije.Add(b);
            }
            uredjaji.Potrosaci = new List<Potrosac>();
            foreach(Potrosac p in potrosaci)
            {
                uredjaji.Potrosaci.Add(p);
            }
            uredjaji.Paneli = new List<SolarniPanel>();
            foreach(SolarniPanel s in solarniPaneli)
            {
                uredjaji.Paneli.Add(s);
            }

            return uredjaji;
        }

        public void PromeniSnaguSunca(int novaVrednost)
        {
            snagaSunca = novaVrednost;
        }

        public void UgasiPotrosac(Potrosac p)
        {
            int i = 0;
            foreach (Potrosac potrosac in potrosaci)
            {
                if (potrosac.JedinstvenoIme == p.JedinstvenoIme)
                {
                    potrosac.Upaljen = false;
                    break;
                }
                i++;
            }
        }

        public void UkljuciNaPunjac(ElektricniAutomobil e)
        {
            int i = 0;
            foreach (ElektricniAutomobil automobil in automobili)
            {
                if (automobil.JedinstvenoIme == e.JedinstvenoIme)
                {
                    automobil.NaPunjacu = true;
                    break;
                }
                i++;
            }
        }

        public void UkloniBateriju(Baterija b)
        {
            int i = 0;
            foreach(Baterija baterija in baterije)
            {
                if(baterija.JedinstvenoIme == b.JedinstvenoIme)
                {
                    baterije.RemoveAt(i);
                    break;
                }
                i++;
            }
        }

        public void UkloniElektricniAutomobil(ElektricniAutomobil a)
        {
            int i = 0;
            foreach(ElektricniAutomobil automobil in automobili)
            {
                if(automobil.JedinstvenoIme == a.JedinstvenoIme)
                {
                    automobili.RemoveAt(i);
                    break;
                }
                i++;
            }
        }

        public void UkloniPotrosaca(Potrosac p)
        {
            int i = 0;
            foreach (Potrosac potrosac in potrosaci)
            {
                if (potrosac.JedinstvenoIme == p.JedinstvenoIme)
                {
                    potrosaci.RemoveAt(i);
                    break;
                }
                i++;
            }
        }

        public void UkloniSolarniPanel(SolarniPanel sp)
        {
            int i = 0;
            foreach(SolarniPanel solarniPanel in solarniPaneli)
            {
                if(solarniPanel.JedinstvenoIme == sp.JedinstvenoIme)
                {
                    solarniPaneli.RemoveAt(i);
                    break;
                }
                i++;
            }
        }

        public void UpaliPotrosac(Potrosac p)
        {
            int i = 0;
            foreach (Potrosac potrosac in potrosaci)
            {
                if (potrosac.JedinstvenoIme == p.JedinstvenoIme)
                {
                    potrosac.Upaljen = true;
                    break;
                }
                i++;
            }
        }

        public void ZaustaviPunjenje(ElektricniAutomobil e)
        {
            int i = 0;
            puniSePunjac = false;
            foreach (ElektricniAutomobil automobil in automobili)
            {
                if (automobil.JedinstvenoIme == e.JedinstvenoIme)
                {
                    automobil.PuniSe = false;
                    break;
                }
                i++;
            }
        }
    }
}
