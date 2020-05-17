﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface ISimulator
    {
        [OperationContract]
        void DodajSolarniPanel(SolarniPanel noviPanel);

        [OperationContract]
        void UkloniSolarniPanel(string jedinstvenoIme);

        [OperationContract]
        void DodajPotrosac(Potrosac potrosac);

        [OperationContract]
        void UkloniPotrosac(string jedinstvenoIme);

        [OperationContract]
        void DodajBateriju(Baterija novaBaterija);

        [OperationContract]
        void UkloniBateriju(string jedinstvenoIme);
    }
}
