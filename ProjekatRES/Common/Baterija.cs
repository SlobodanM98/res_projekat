﻿using System;
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

        private int kapacitet;
        [DataMember]
        public int Kapacitet
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

        public event PropertyChangedEventHandler PropertyChanged;

        public Baterija(string jedinstvenoIme, double maksimalnaSnaga, int kapacitet)
        {
            JedinstvenoIme = jedinstvenoIme;
            MaksimalnaSnaga = maksimalnaSnaga;
            Kapacitet = kapacitet;
            Slika = MaterialDesignThemes.Wpf.PackIconKind.Battery0;
            AutomobilJedinstvenoIme = null;
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
