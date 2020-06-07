using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IRepozitorijum
    {
        [OperationContract]
        void DodajSolarniPanel(SolarniPanel noviPanel);

        [OperationContract]
        void UkloniSolarniPanel(SolarniPanel sp);

        [OperationContract]
        void DodajPotrosaca(Potrosac potrosac);

        [OperationContract]
        void UkloniPotrosaca(Potrosac p);

        [OperationContract]
        void UpaliPotrosac(string jedinstvenoIme);

        [OperationContract]
        void UgasiPotrosac(string jedinstvenoIme);

        [OperationContract]
        void DodajBateriju(Baterija novaBaterija, bool jesteAutomobil, string AutomobilJedinstvenoIme);

        [OperationContract]
        void UkloniBateriju(Baterija b);

        [OperationContract]
        void DodajElektricniAutomobil(ElektricniAutomobil automobil);

        [OperationContract]
        void UkloniElektricniAutomobil(ElektricniAutomobil a);

        [OperationContract]
        void PromeniSnaguSunca(int novaVrednost);

        [OperationContract]
        void UkljuciNaPunjac(string jedinstvenoIme);

        [OperationContract]
        void IskljuciSaPunjaca(string jedinstvenoIme);

        [OperationContract]
        void PokreniPunjenje(ElektricniAutomobil e);

        [OperationContract]
        void ZaustaviPunjenje(ElektricniAutomobil e);
    }
}
