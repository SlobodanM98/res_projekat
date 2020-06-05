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
            Sat = sat;
            Datum = datum;
            Baterije = baterije;
            Distribucija = distribucija;
            SolarniPaneli = solarniPaneli;
            Potrosaci = potrosaci;
        }
    }
}
