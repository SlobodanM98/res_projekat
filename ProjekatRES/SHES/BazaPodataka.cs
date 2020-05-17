using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHES
{
    public class BazaPodataka
    {
        public static Dictionary<string, SolarniPanel> SolarniPaneli = new Dictionary<string, SolarniPanel>();

        public static Dictionary<string, Potrosac> Potrosaci = new Dictionary<string, Potrosac>();

        public static Dictionary<string, Baterija> Baterije = new Dictionary<string, Baterija>();

        public static Dictionary<string, PunjacElektricnogAutomobila> Punjaci = new Dictionary<string, PunjacElektricnogAutomobila>();
    }
}
