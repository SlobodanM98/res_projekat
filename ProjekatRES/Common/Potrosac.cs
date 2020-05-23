﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Potrosac
    {
        [DataMember]
        public string JedinstvenoIme { get; set; }
        [DataMember]
        public double Potrosnja { get; set; }
        [DataMember]
        public bool Upaljen { get; set; }
        [DataMember]
        public MaterialDesignThemes.Wpf.PackIconKind Slika { get; set; }

        public Potrosac(string jedinstvenoIme, double potrosnja)
        {
            JedinstvenoIme = jedinstvenoIme;
            Potrosnja = potrosnja;
            Upaljen = false;
            Slika = MaterialDesignThemes.Wpf.PackIconKind.PowerPlugOffOutline;
        }
    }
}
