using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class PunjacElektricnogAutomobila
    {
        [DataMember]
        public string JedinstvenoIme { get; set; }
        [DataMember]
        public double SnagaElektricnogAutomobila { get; set; }
        [DataMember]
        public bool AutoNaPunjacu { get; set; }
        [DataMember]
        public bool AutoSePuni { get; set; }

        public PunjacElektricnogAutomobila(string jedinstvenoIme)
        {
            this.JedinstvenoIme = jedinstvenoIme;
            this.AutoNaPunjacu = false;
            this.AutoSePuni = false;
        }
    }
}
