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

        public void DodajBateriju(Baterija novaBaterija, bool jesteAutomobil, string AutomobilJedinstvenoIme)
        {
            throw new NotImplementedException();
        }

        public void DodajElektricniAutomobil(ElektricniAutomobil automobil)
        {
            throw new NotImplementedException();
        }

        public void DodajPotrosaca(Potrosac potrosac)
        {
            throw new NotImplementedException();
        }

        public void DodajSolarniPanel(SolarniPanel noviPanel)
        {
            solarniPaneli.Add(noviPanel);
        }

        public void IskljuciSaPunjaca(string jedinstvenoIme)
        {
            throw new NotImplementedException();
        }

        public void PokreniPunjenje(ElektricniAutomobil e)
        {
            throw new NotImplementedException();
        }

        public void PromeniSnaguSunca(int novaVrednost)
        {
            throw new NotImplementedException();
        }

        public void UgasiPotrosac(string jedinstvenoIme)
        {
            throw new NotImplementedException();
        }

        public void UkljuciNaPunjac(string jedinstvenoIme)
        {
            throw new NotImplementedException();
        }

        public void UkloniBateriju(Baterija b)
        {
            throw new NotImplementedException();
        }

        public void UkloniElektricniAutomobil(ElektricniAutomobil a)
        {
            throw new NotImplementedException();
        }

        public void UkloniPotrosaca(Potrosac p)
        {
            throw new NotImplementedException();
        }

        public void UkloniSolarniPanel(SolarniPanel sp)
        {
            solarniPaneli.Remove(sp);
        }

        public void UpaliPotrosac(string jedinstvenoIme)
        {
            throw new NotImplementedException();
        }

        public void ZaustaviPunjenje(ElektricniAutomobil e)
        {
            throw new NotImplementedException();
        }
    }
}
