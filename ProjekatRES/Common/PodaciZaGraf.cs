using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class PodaciZaGraf
    {
        public DateTime Datum { get; set; }
        public int Sat { get; set; }
        public double Baterije { get; set; }
        public double Distribucija { get; set; }
        public double SolarniPaneli { get; set; }
        public double Potrosaci { get; set; }

        public PodaciZaGraf(int sat, DateTime datum, double baterije, double distribucija, double solarniPaneli, double potrosaci)
        {
            if(!(sat >= 0 && sat <= 23))
            {
                throw new ArgumentException("Sat mora biti izmedju 1 i 24.");
            }
            if(solarniPaneli < 0)
            {
                throw new ArgumentException("Elektricna energija solarnih panela mora biti pozitivna.");
            }
            if(potrosaci > 0)
            {
                throw new ArgumentException("Potrosnja potrosaca mora biti negativna.");
            }
            Sat = sat;
            Datum = datum;
            Baterije = baterije;
            Distribucija = distribucija;
            SolarniPaneli = solarniPaneli;
            Potrosaci = potrosaci;
        }
    }
}
