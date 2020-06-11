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
        void PromeniSnaguSunca(int novaVrednost);

        [OperationContract]
        Uredjaji PreuzmiUredjaje();

        [OperationContract]
        void PodesiOdnos(int noviOdnos);

        [OperationContract]
        void PodesavanjeCene(int cena);
    }
}
