using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IBaterija
    {
        [OperationContract]
        void DodajBateriju(Baterija novaBaterija, bool jesteAutomobil, string AutomobilJedinstvenoIme);

        [OperationContract]
        void UkloniBateriju(string jedinstvenoIme);
    }
}
