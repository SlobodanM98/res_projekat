using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IMetode
    {
        [OperationContract]
        double PunjenjeBaterije(Baterija baterija, bool baterijaAuta);
        [OperationContract]
        double PraznjenjeBaterije(Baterija baterija);
        [OperationContract]
        void ResetBaterije(Baterija baterija);
        [OperationContract]
        void ZapamtiZaGraf(double baterije, double paneli, double distribucija, double potrosaci);
        [OperationContract]
        void UcitajUredjaje();
        [OperationContract]
        void UcitajDatume();
        [OperationContract]
        void UcitajPoslednjiSat();
        [OperationContract]
        void ZapamtiVreme();
    }
}
