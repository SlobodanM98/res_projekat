using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Elektrodistribucija
    {
        [DataMember]
        public double SnagaRazmene { get; set; }
        [DataMember]
        public double Cena { get; set; }

        public Elektrodistribucija()
        {
            SnagaRazmene = 0;
            Cena = 1;
        }
    }
}
