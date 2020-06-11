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
    public class SolarniPanel : INotifyPropertyChanged
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
            set
            {
                maksimalnaSnaga = value;
                OnPropertyChanged("MaksimalnaSnaga");
            }
        }

        private MaterialDesignThemes.Wpf.PackIconKind slika;
        [DataMember]
        public MaterialDesignThemes.Wpf.PackIconKind Slika
        {
            get { return slika; }
            set
            {
                slika = value;
                OnPropertyChanged("Slika");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public SolarniPanel(string jedinstvenoIme, double maksimalnaSnaga)
        {
            JedinstvenoIme = jedinstvenoIme;
            MaksimalnaSnaga = maksimalnaSnaga;
            Slika = MaterialDesignThemes.Wpf.PackIconKind.Waze;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /*public override bool Equals(object obj)
        {
            if (JedinstvenoIme != ((SolarniPanel)obj).JedinstvenoIme)
            {
                return false;
            }
            if (MaksimalnaSnaga != ((SolarniPanel)obj).MaksimalnaSnaga)
            {
                return false;
            }
            return true;
        }*/
    }
}
