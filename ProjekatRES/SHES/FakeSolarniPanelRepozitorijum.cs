using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace SHES
{
    public class FakeSolarniPanelRepozitorijum : ISolarniPanelRepozitorijum
    {
        public List<SolarniPanel> solarniPaneli = new List<SolarniPanel>();

        public void DodajSolarniPanel(SolarniPanel noviPanel)
        {
            solarniPaneli.Add(noviPanel);
        }

        public void UkloniSolarniPanel(SolarniPanel sp)
        {
            int i = 0;
            foreach (SolarniPanel solarniPanel in solarniPaneli)
            {
                if (solarniPanel.JedinstvenoIme == sp.JedinstvenoIme)
                {
                    solarniPaneli.RemoveAt(i);
                    break;
                }
                i++;
            }
        }
    }
}
