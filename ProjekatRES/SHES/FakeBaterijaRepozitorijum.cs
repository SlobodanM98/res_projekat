using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace SHES
{
    public class FakeBaterijaRepozitorijum : IBaterijaRepozitorijum
    {
        public List<Baterija> baterije = new List<Baterija>();

        public void DodajBateriju(Baterija novaBaterija, bool jesteAutomobil, string AutomobilJedinstvenoIme)
        {
            baterije.Add(novaBaterija);
        }

        public void UkloniBateriju(Baterija b)
        {
            int i = 0;
            foreach (Baterija baterija in baterije)
            {
                if (baterija.JedinstvenoIme == b.JedinstvenoIme)
                {
                    baterije.RemoveAt(i);
                    break;
                }
                i++;
            }
        }
    }
}
