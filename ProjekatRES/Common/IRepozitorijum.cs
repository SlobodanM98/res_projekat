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
        void UpaliPotrosac(Potrosac p);

        [OperationContract]
        void UgasiPotrosac(Potrosac p);

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
        void UkljuciNaPunjac(ElektricniAutomobil a);

        [OperationContract]
        void IskljuciSaPunjaca(ElektricniAutomobil a);

        [OperationContract]
        void PokreniPunjenje(ElektricniAutomobil e);

        [OperationContract]
        void ZaustaviPunjenje(ElektricniAutomobil e);

        [OperationContract]
        Uredjaji PreuzmiUredjaje();

        [OperationContract]
        void PodesiOdnos(int noviOdnos);

        [OperationContract]
        void PodesavanjeCene(int cena);

        [OperationContract]
        void PostavljanjeKapacitetaAuta(ElektricniAutomobil e, int trenutniKapacitet);
    }
}
