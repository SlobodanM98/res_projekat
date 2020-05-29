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
    public class Potrosac : INotifyPropertyChanged
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

        private double potrosnja;
        [DataMember]
        public double Potrosnja
        {
            get { return potrosnja; }
            set { potrosnja = value;
                OnPropertyChanged("Potrosnja");
            }
        }

        private bool upaljen;
        [DataMember]
        public bool Upaljen
        {
            get { return upaljen; }
            set { upaljen = value;
                OnPropertyChanged("Upaljen");
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

        public Potrosac(string jedinstvenoIme, double potrosnja)
        {
            JedinstvenoIme = jedinstvenoIme;
            Potrosnja = potrosnja;
            Upaljen = false;
            Slika = MaterialDesignThemes.Wpf.PackIconKind.PowerPlugOffOutline;
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
