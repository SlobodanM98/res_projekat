using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHES
{
    public class BaterijaServer : IBaterija
    {
        IBaterijaRepozitorijum repozitorijum;

        public BaterijaServer() { }

        public BaterijaServer(IBaterijaRepozitorijum repo)
        {
            repozitorijum = repo;
        }

        public void DodajBateriju(Baterija novaBaterija, bool jesteAutomobil, string AutomobilJedinstvenoIme)
        {
            bool sadrzi = false;
            foreach (Baterija b in MainWindow.Baterije)
            {
                if (b.JedinstvenoIme == novaBaterija.JedinstvenoIme)
                {
                    sadrzi = true;
                    break;
                }
            }

            if (!sadrzi)
            {
                if (repozitorijum == null)
                {
                    repozitorijum = new BaterijaRepozitorijum();
                }
                repozitorijum.DodajBateriju(novaBaterija, jesteAutomobil, AutomobilJedinstvenoIme);
            }
        }

        public void UkloniBateriju(string jedinstvenoIme)
        {
            foreach (Baterija b in MainWindow.Baterije)
            {
                if (b.JedinstvenoIme == jedinstvenoIme)
                {
                    if (repozitorijum == null)
                    {
                        repozitorijum = new BaterijaRepozitorijum();
                    }
                    repozitorijum.UkloniBateriju(b);
                    break;
                }
            }
        }
    }
}
