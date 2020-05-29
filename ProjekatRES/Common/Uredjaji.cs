using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Uredjaji
    {
        [DataMember]
        public List<Baterija> Baterije { get; set; }
        [DataMember]
        public List<ElektricniAutomobil> Automobili { get; set; }
        [DataMember]
        public List<Potrosac> Potrosaci { get; set; }
        [DataMember]
        public List<SolarniPanel> Paneli { get; set; }
    }
}
