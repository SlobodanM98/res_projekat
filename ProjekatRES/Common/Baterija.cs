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
    public class Baterija : INotifyPropertyChanged
    {
        private string jedinstvenoIme;
        [DataMember]
        public string JedinstvenoIme
        {
            get { return jedinstvenoIme; }
            set
            {
                jedinstvenoIme = value;
                OnPropertyChanged("JedinstvenoIme");
            }
        }

        private double maksimalnaSnaga;
        [DataMember]
        public double MaksimalnaSnaga
        {
            get { return maksimalnaSnaga; }
            set { maksimalnaSnaga = value;
                OnPropertyChanged("MaksimalnaSnaga");
            }
        }

        private double kapacitet;
        [DataMember]
        public double Kapacitet
        {
            get { return kapacitet; }
            set { kapacitet = value;
                OnPropertyChanged("Kapacitet");
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

        private string automobilJedinstvenoIme;
        [DataMember]
        public string AutomobilJedinstvenoIme
        {
            get { return automobilJedinstvenoIme; }
            set { automobilJedinstvenoIme = value;
                OnPropertyChanged("AutomobilJedinstvenoIme");
            }
        }

        private double trenutniKapacitet;
        [DataMember]
        public double TrenutniKapacitet
        {
            get { return trenutniKapacitet; }
            set { trenutniKapacitet = value;
                OnPropertyChanged("TrenutniKapacitet");
            }
        }


        public bool PuniSe { get; set; }

        public bool PrazniSe { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public Baterija(string jedinstvenoIme, double maksimalnaSnaga, double kapacitet)
        {
            JedinstvenoIme = jedinstvenoIme;
            MaksimalnaSnaga = maksimalnaSnaga;
            Kapacitet = kapacitet;
            TrenutniKapacitet = 0;
            Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery0;
            AutomobilJedinstvenoIme = null;
            PuniSe = false;
            PrazniSe = false;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public override bool Equals(object obj)
        {
            if(JedinstvenoIme != ((Baterija)obj).JedinstvenoIme)
            {
                return false;
            }
            if (MaksimalnaSnaga != ((Baterija)obj).MaksimalnaSnaga)
            {
                return false;
            }
            if (Kapacitet != ((Baterija)obj).Kapacitet)
            {
                return false;
            }
            if (TrenutniKapacitet != ((Baterija)obj).TrenutniKapacitet)
            {
                return false;
            }
            if (AutomobilJedinstvenoIme != ((Baterija)obj).AutomobilJedinstvenoIme)
            {
                return false;
            }
            if (PuniSe != ((Baterija)obj).PuniSe)
            {
                return false;
            }
            if (PrazniSe != ((Baterija)obj).PrazniSe)
            {
                return false;
            }
            return true;
        }
    }
}
