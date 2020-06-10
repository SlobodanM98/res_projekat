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

        public override bool Equals(object obj)
        {
            if (Baterije.Count != ((Uredjaji)obj).Baterije.Count)
            {
                return false;
            }

            for(int i = 0; i < Baterije.Count; i++)
            {
                if(!Baterije[i].Equals(((Uredjaji)obj).Baterije[i]))
                {
                    return false;   
                }
            }

            if(Automobili.Count != ((Uredjaji)obj).Automobili.Count)
            {
                return false;
            }

            for (int i = 0; i < Automobili.Count; i++)
            {
                if (!Automobili[i].Equals(((Uredjaji)obj).Automobili[i]))
                {
                    return false;
                }
            }

            if (Potrosaci.Count != ((Uredjaji)obj).Potrosaci.Count)
            {
                return false;
            }

            for (int i = 0; i < Potrosaci.Count; i++)
            {
                if (!Potrosaci[i].Equals(((Uredjaji)obj).Potrosaci[i]))
                {
                    return false;
                }
            }

            if (Paneli.Count != ((Uredjaji)obj).Paneli.Count)
            {
                return false;
            }

            for (int i = 0; i < Paneli.Count; i++)
            {
                if (!Paneli[i].Equals(((Uredjaji)obj).Paneli[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
