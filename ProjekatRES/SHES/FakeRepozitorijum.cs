using Common;
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

        public void PodesavanjeCene(int cena)
        {
            cenovnik = cena;
        }

        public void PodesiOdnos(int noviOdnos)
        {
            jednaSekundaJe = noviOdnos;
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
    }
}
