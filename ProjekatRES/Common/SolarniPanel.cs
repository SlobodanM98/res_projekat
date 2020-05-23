using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class SolarniPanel
    {
        [DataMember]
        public string JedinstvenoIme { get; set; }
        [DataMember]
        public double MaksimalnaSnaga { get; set; }
        [DataMember]
        public MaterialDesignThemes.Wpf.PackIconKind Slika { get; set; }

        public SolarniPanel(string jedinstvenoIme, double maksimalnaSnaga)
        {
            JedinstvenoIme = jedinstvenoIme;
            MaksimalnaSnaga = maksimalnaSnaga;
            Slika = MaterialDesignThemes.Wpf.PackIconKind.Waze;
        }
    }
}
