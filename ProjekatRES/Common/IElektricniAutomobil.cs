using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IElektricniAutomobil
    {
        [OperationContract]
        void DodajElektricniAutomobil(ElektricniAutomobil automobil);

        [OperationContract]
        void UkloniElektricniAutomobil(string jedinstvenoIme);

        [OperationContract]
        bool UkljuciNaPunjac(string jedinstvenoIme);

        [OperationContract]
        bool IskljuciSaPunjaca(string jedinstvenoIme);

        [OperationContract]
        bool PokreniPunjenje(string jedinstvenoIme);

        [OperationContract]
        bool ZaustaviPunjenje(string jedinstvenoIme);

        [OperationContract]
        bool PostavljanjeKapacitetaAuta(int kapacitet);
    }
}
