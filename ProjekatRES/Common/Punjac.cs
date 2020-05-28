using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Punjac
    {
        [DataMember]
        public bool NaPunjacu { get; set; }
        [DataMember]
        public bool PuniSe { get; set; }
        [DataMember]
        public ElektricniAutomobil Automobil { get; set; }

        public Punjac()
        {
            NaPunjacu = false;
            PuniSe = false;
            Automobil = null;
        }
    }
}
