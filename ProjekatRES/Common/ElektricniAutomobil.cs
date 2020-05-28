using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class ElektricniAutomobil
    {
        [DataMember]
        public string JedinstvenoIme { get; set; }
        [DataMember]
        public bool NaPunjacu { get; set; }
        [DataMember]
        public bool PuniSe { get; set; }
        [DataMember]
        public Baterija BaterijaAuta { get; set; }
        [DataMember]
        private static int brojBaterije = 0;

        public ElektricniAutomobil(string jedinstvenoIme, double maksimalnaSnaga, int kapacitet)
        {
            JedinstvenoIme = jedinstvenoIme;
            brojBaterije++;
            Baterija baterija = new Baterija("BaterijaAuta" + brojBaterije.ToString(), maksimalnaSnaga, kapacitet);
            BaterijaAuta = baterija;
            NaPunjacu = false;
            PuniSe = false;
        }
    }
}
