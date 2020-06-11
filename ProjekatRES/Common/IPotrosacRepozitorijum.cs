using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IPotrosacRepozitorijum
    {
        [OperationContract]
        void DodajPotrosaca(Potrosac potrosac);

        [OperationContract]
        void UkloniPotrosaca(Potrosac p);

        [OperationContract]
        void UpaliPotrosac(Potrosac p);

        [OperationContract]
        void UgasiPotrosac(Potrosac p);
    }
}
