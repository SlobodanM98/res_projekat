using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IElektricniAutomobilRepozitorijum
    {
        [OperationContract]
        void DodajElektricniAutomobil(ElektricniAutomobil automobil);

        [OperationContract]
        void UkloniElektricniAutomobil(ElektricniAutomobil a);

        [OperationContract]
        void UkljuciNaPunjac(ElektricniAutomobil a);

        [OperationContract]
        void IskljuciSaPunjaca(ElektricniAutomobil a);

        [OperationContract]
        void PokreniPunjenje(ElektricniAutomobil e);

        [OperationContract]
        void ZaustaviPunjenje(ElektricniAutomobil e);

        [OperationContract]
        void PostavljanjeKapacitetaAuta(ElektricniAutomobil e, int trenutniKapacitet);
    }
}
