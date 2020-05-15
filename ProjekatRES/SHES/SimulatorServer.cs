using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHES
{
    public class SimulatorServer : ISimulator
    {
        public void DodajSolarniPanel(SolarniPanel noviPanel)
        {
            if (!BazaPodataka.SolarniPaneli.ContainsKey(noviPanel.JedinstvenoIme))
            {
                BazaPodataka.SolarniPaneli.Add(noviPanel.JedinstvenoIme,noviPanel);
            }
        }

        public void UkloniSolarniPanel(string jedinstvenoIme)
        {
            if (BazaPodataka.SolarniPaneli.ContainsKey(jedinstvenoIme))
            {
                BazaPodataka.SolarniPaneli.Remove(jedinstvenoIme);
            }
        }
    }
}
