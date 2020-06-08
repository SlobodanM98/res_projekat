using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHES
{
    public class FakeMetodeRepozitorijum : IMetode
    {
        public List<Baterija> baterije = new List<Baterija>();
        public List<Baterija> baterijeUcitavanje = new List<Baterija>();
        public List<Baterija> autoBaterije = new List<Baterija>();
        public List<Baterija> autoBaterijeUcitavanje = new List<Baterija>();
        public List<Potrosac> potrosaci = new List<Potrosac>();
        public List<Potrosac> potrosaciUcitavanje = new List<Potrosac>();
        public List<SolarniPanel> solarniPaneli = new List<SolarniPanel>();
        public List<SolarniPanel> solarniPaneliUcitavanje = new List<SolarniPanel>();
        public List<ElektricniAutomobil> automobili = new List<ElektricniAutomobil>();
        public List<ElektricniAutomobil> automobiliUcitavanje = new List<ElektricniAutomobil>();
        public List<Datum> datumi = new List<Datum>();
        public List<Datum> datumiUcitavanje = new List<Datum>();
        public DateTime vreme;
        public DateTime vremeUcitavanje;
        public int snagaSunca;
        public int snagaSuncaUcitavanje;
        public Elektrodistribucija elektro = new Elektrodistribucija();
        public double baterijeGraf;
        public double paneliGraf;
        public double distribucijaGraf;
        public double potrosaciGraf;

        public double PraznjenjeBaterije(Baterija baterija)
        {
            baterija.PrazniSe = true;
            foreach(Baterija b in baterije)
            {
                if(b.JedinstvenoIme == baterija.JedinstvenoIme)
                {
                    b.PrazniSe = true;
                    break;
                }
            }
            if(baterija.TrenutniKapacitet >= 0)
            {
                return baterija.MaksimalnaSnaga / 3600;
            }
            else
            {
                return 0;
            }
        }

        public double PunjenjeBaterije(Baterija baterija, bool baterijaAuta)
        {
            baterija.PuniSe = true;
            foreach (Baterija b in baterije)
            {
                if (b.JedinstvenoIme == baterija.JedinstvenoIme)
                {
                    b.PuniSe = true;
                    break;
                }
            }
            if (baterija.TrenutniKapacitet >= 0)
            {
                return baterija.MaksimalnaSnaga / 3600;
            }
            else
            {
                return 0;
            }
        }

        public void ResetBaterije(Baterija baterija)
        {
            foreach(Baterija b in baterije)
            {
                if (b.JedinstvenoIme == baterija.JedinstvenoIme)
                {
                    b.PuniSe = false;
                    b.PrazniSe = false;
                    break;
                }
            }
        }

        public void UcitajDatume()
        {
            foreach(Datum d in datumiUcitavanje)
            {
                datumi.Add(d);
            }
        }

        public void UcitajPoslednjiSat()
        {
            foreach (Datum d in datumiUcitavanje)
            {
                datumi.Add(d);
            }
        }

        public void UcitajUredjaje()
        {
            vreme = vremeUcitavanje;
            snagaSunca = snagaSuncaUcitavanje;
            foreach(Baterija b in baterijeUcitavanje)
            {
                baterije.Add(b);
            }
            foreach (Baterija b in autoBaterijeUcitavanje)
            {
                autoBaterije.Add(b);
            }
            foreach(Potrosac p in potrosaciUcitavanje)
            {
                potrosaci.Add(p);
            }
            foreach(SolarniPanel s in solarniPaneliUcitavanje)
            {
                solarniPaneli.Add(s);
            }
            foreach(ElektricniAutomobil e in automobiliUcitavanje)
            {
                automobili.Add(e);
            }
        }

        public void ZapamtiVreme()
        {
            vreme = DateTime.Parse("1/1/11");
        }

        public void ZapamtiZaGraf(double baterije, double paneli, double distribucija, double potrosaci)
        {
            baterijeGraf = baterije;
            paneliGraf = paneli;
            distribucijaGraf = distribucija;
            potrosaciGraf = potrosaci;
        }
    }
}
