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

        public ElektricniAutomobil(string jedinstvenoIme, double maksimalnaSnaga, int kapacitet, int brojBaterije)
        {
            JedinstvenoIme = jedinstvenoIme;
            Baterija baterija = new Baterija("BaterijaAuta" + brojBaterije.ToString(), maksimalnaSnaga, kapacitet);
            BaterijaAuta = baterija;
            NaPunjacu = false;
            PuniSe = false;
        }

        public ElektricniAutomobil(Baterija baterija, string jedinstvenoIme, bool naPunjacu, bool puniSe)
        {
            JedinstvenoIme = jedinstvenoIme;
            BaterijaAuta = baterija;
            NaPunjacu = naPunjacu;
            PuniSe = puniSe;
        }

    }
}
