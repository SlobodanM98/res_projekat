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
    public class ElektricniAutomobil : INotifyPropertyChanged
    {
        private string jedinstvenoIme;
        [DataMember]
        public string JedinstvenoIme
        {
            get { return jedinstvenoIme; }
            set { jedinstvenoIme = value;
                  OnPropertyChanged("JedinstvenoIme");
            }
        }

        private bool naPunjacu;
        [DataMember]
        public bool NaPunjacu
        {
            get { return naPunjacu; }
            set { naPunjacu = value;
                OnPropertyChanged("NaPunjacu");
            }
        }

        private bool puniSe;
        [DataMember]
        public bool PuniSe
        {
            get { return puniSe; }
            set { puniSe = value;
                OnPropertyChanged("PuniSe");
            }
        }

        private Baterija baterijaAuta;
        [DataMember]
        public Baterija BaterijaAuta
        {
            get { return baterijaAuta; }
            set { baterijaAuta = value;
                OnPropertyChanged("BaterijaAuta");
            }
        }

        private MaterialDesignThemes.Wpf.PackIconKind slika;
        [DataMember]
        public MaterialDesignThemes.Wpf.PackIconKind Slika
        {
            get { return slika; }
            set { slika = value;
                OnPropertyChanged("Slika");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ElektricniAutomobil(string jedinstvenoIme, double maksimalnaSnaga, double kapacitet, int brojBaterije)
        {
            JedinstvenoIme = jedinstvenoIme;
            Baterija baterija = new Baterija("BaterijaAuta" + brojBaterije.ToString(), maksimalnaSnaga, kapacitet);
            BaterijaAuta = baterija;
            NaPunjacu = false;
            PuniSe = false;
            Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery0;
        }

        public ElektricniAutomobil(Baterija baterija, string jedinstvenoIme, bool naPunjacu, bool puniSe)
        {
            JedinstvenoIme = jedinstvenoIme;
            BaterijaAuta = baterija;
            NaPunjacu = naPunjacu;
            PuniSe = puniSe;
            Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery0;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public override bool Equals(object obj)
        {
            if (JedinstvenoIme != ((ElektricniAutomobil)obj).JedinstvenoIme)
            {
                return false;
            }
            if (!BaterijaAuta.Equals(((ElektricniAutomobil)obj).BaterijaAuta))
            {
                return false;
            }
            if (NaPunjacu != ((ElektricniAutomobil)obj).NaPunjacu)
            {
                return false;
            }
            if (PuniSe != ((ElektricniAutomobil)obj).PuniSe)
            {
                return false;
            }
            return true;
        }
    }
}
