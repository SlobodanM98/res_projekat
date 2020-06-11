using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IPotrosac
    {
        [OperationContract]
        void DodajPotrosaca(Potrosac potrosac);

        [OperationContract]
        void UkloniPotrosaca(string jedinstvenoIme);

        [OperationContract]
        void UpaliPotrosac(string jedinstvenoIme);

        [OperationContract]
        void UgasiPotrosac(string jedinstvenoIme);
    }
}
