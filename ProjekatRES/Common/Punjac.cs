using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Punjac : INotifyPropertyChanged
    {
        private bool naPunjacu;
        [DataMember]
        public bool NaPunjacu
        {
            get { return naPunjacu; }
            set
            {
                naPunjacu = value;
                OnPropertyChanged("NaPunjacu");
            }
        }

        private bool puniSe;
        [DataMember]
        public bool PuniSe
        {
            get { return puniSe; }
            set
            {
                puniSe = value;
                OnPropertyChanged("PuniSe");
            }
        }

        private ElektricniAutomobil automobil;
        [DataMember]
        public ElektricniAutomobil Automobil
        {
            get { return automobil; }
            set { automobil = value;
                OnPropertyChanged("Automobil");
            }
        }

        /*[DataMember]
        public bool NaPunjacu { get; set; }
        [DataMember]
        public bool PuniSe { get; set; }
        [DataMember]
        public ElektricniAutomobil Automobil { get; set; }*/

        public event PropertyChangedEventHandler PropertyChanged;

        public Punjac()
        {
            NaPunjacu = false;
            PuniSe = false;
            Automobil = null;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
