using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace SHES
{
    public class FakeElektricniAutomobilRepozitorijum : IElektricniAutomobilRepozitorijum
    {
        public List<ElektricniAutomobil> automobili = new List<ElektricniAutomobil>();
        public bool puniSePunjac = false;

        public void DodajElektricniAutomobil(ElektricniAutomobil automobil)
        {
            automobili.Add(automobil);
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

        public void UkloniElektricniAutomobil(ElektricniAutomobil a)
        {
            int i = 0;
            foreach (ElektricniAutomobil automobil in automobili)
            {
                if (automobil.JedinstvenoIme == a.JedinstvenoIme)
                {
                    automobili.RemoveAt(i);
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
