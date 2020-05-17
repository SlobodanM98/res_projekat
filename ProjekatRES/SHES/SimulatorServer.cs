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

        public void DodajPotrosac(Potrosac potrosac)
        {
            if(!BazaPodataka.Potrosaci.ContainsKey(potrosac.JedinstvenoIme))
            {
                BazaPodataka.Potrosaci.Add(potrosac.JedinstvenoIme, potrosac);
            }
        }

        public void UkloniPotrosac(string jedinstvenoIme)
        {
            if (BazaPodataka.Potrosaci.ContainsKey(jedinstvenoIme))
            {
                BazaPodataka.Potrosaci.Remove(jedinstvenoIme);
            }
        }

        public void DodajBateriju(Baterija novaBaterija)
        {
            if (!BazaPodataka.Baterije.ContainsKey(novaBaterija.JedinstvenoIme))
            {
                BazaPodataka.Baterije.Add(novaBaterija.JedinstvenoIme, novaBaterija);
            }
        }

        public void UkloniBateriju(string jedinstvenoIme)
        {
            if (BazaPodataka.Baterije.ContainsKey(jedinstvenoIme))
            {
                BazaPodataka.Baterije.Remove(jedinstvenoIme);
            }
        }
    }
}
