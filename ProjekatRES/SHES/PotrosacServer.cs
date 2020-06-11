using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHES
{
    public class PotrosacServer : IPotrosac
    {
        IPotrosacRepozitorijum repozitorijum;

        public PotrosacServer() { }

        public PotrosacServer(IPotrosacRepozitorijum repo)
        {
            repozitorijum = repo;
        }

        public void DodajPotrosaca(Potrosac potrosac)
        {
            bool sadrzi = false;
            foreach (Potrosac p in MainWindow.Potrosaci)
            {
                if (p.JedinstvenoIme == potrosac.JedinstvenoIme)
                {
                    sadrzi = true;
                    break;
                }
            }
            if (!sadrzi)
            {
                if (repozitorijum == null)
                {
                    repozitorijum = new PotrosacRepozitorijum();
                }
                repozitorijum.DodajPotrosaca(potrosac);
            }
        }

        public void UkloniPotrosaca(string jedinstvenoIme)
        {
            foreach (Potrosac p in MainWindow.Potrosaci)
            {
                if (p.JedinstvenoIme == jedinstvenoIme)
                {
                    if (repozitorijum == null)
                    {
                        repozitorijum = new PotrosacRepozitorijum();
                    }
                    repozitorijum.UkloniPotrosaca(p);
                    break;
                }
            }
        }

        public void UpaliPotrosac(string jedinstvenoIme)
        {
            foreach (Potrosac p in MainWindow.Potrosaci)
            {
                if (p.JedinstvenoIme == jedinstvenoIme)
                {
                    if (repozitorijum == null)
                    {
                        repozitorijum = new PotrosacRepozitorijum();
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
                        repozitorijum = new PotrosacRepozitorijum();
                    }
                    repozitorijum.UgasiPotrosac(p);
                    break;
                }
            }
        }
    }
}
