using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace SHES
{
    public class FakePotrosacRepozitorijum : IPotrosacRepozitorijum
    {
        public List<Potrosac> potrosaci = new List<Potrosac>();

        public void DodajPotrosaca(Potrosac potrosac)
        {
            potrosaci.Add(potrosac);
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
    }
}
