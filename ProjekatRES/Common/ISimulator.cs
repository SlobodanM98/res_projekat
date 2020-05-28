using System;
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
        void DodajPotrosaca(Potrosac potrosac);

        [OperationContract]
        void UkloniPotrosaca(string jedinstvenoIme);

        [OperationContract]
        void DodajBateriju(Baterija novaBaterija, bool jesteAutomobil, string AutomobilJedinstvenoIme);

        [OperationContract]
        void UkloniBateriju(string jedinstvenoIme);

        [OperationContract]
        void DodajElektricniAutomobil(ElektricniAutomobil automobil);

        [OperationContract]
        void UkloniElektricniAutomobil(string jedinstvenoIme);

        [OperationContract]
        void PromeniSnaguSunca(int novaVrednost);
    }
}
