using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHES
{
    public class SolarniPanelServer : ISolarniPanel
    {
        ISolarniPanelRepozitorijum repozitorijum;

        public SolarniPanelServer() { }

        public SolarniPanelServer(ISolarniPanelRepozitorijum repo)
        {
            repozitorijum = repo;
        }

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
                if (repozitorijum == null)
                {
                    repozitorijum = new SolarniPanelRepozitorijum();
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
                        repozitorijum = new SolarniPanelRepozitorijum();
                    }
                    repozitorijum.UkloniSolarniPanel(sp);
                    break;
                }
            }
        }
    }
}
