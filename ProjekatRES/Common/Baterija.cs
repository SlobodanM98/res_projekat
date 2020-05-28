using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Baterija
    {
        [DataMember]
        public string JedinstvenoIme { get; set; }
        [DataMember]
        public double MaksimalnaSnaga { get; set; }
        [DataMember]
        public int Kapacitet { get; set; }
        [DataMember]
        public MaterialDesignThemes.Wpf.PackIconKind Slika { get; set; }
        [DataMember]
        public string AutomobilJedinstvenoIme { get; set; }

        public Baterija(string jedinstvenoIme, double maksimalnaSnaga, int kapacitet)
        {
            JedinstvenoIme = jedinstvenoIme;
            MaksimalnaSnaga = maksimalnaSnaga;
            Kapacitet = kapacitet;
            Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery0;
            AutomobilJedinstvenoIme = null;
        }
    }
}
