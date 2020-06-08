using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHES
{
    public class Metode : IMetode
    {
        IMetode repozitorijum;

        public Metode(IMetode repo)
        {
            repozitorijum = repo;
        }

        public double PraznjenjeBaterije(Baterija baterija)
        {
            return repozitorijum.PraznjenjeBaterije(baterija);
        }

        public double PunjenjeBaterije(Baterija baterija, bool baterijaAuta)
        {
            return repozitorijum.PunjenjeBaterije(baterija, baterijaAuta);
        }

        public void ResetBaterije(Baterija baterija)
        {
            repozitorijum.ResetBaterije(baterija);
        }

        public void UcitajDatume()
        {
            repozitorijum.UcitajDatume();
        }

        public void UcitajPoslednjiSat()
        {
            repozitorijum.UcitajPoslednjiSat();
        }

        public void UcitajUredjaje()
        {
            repozitorijum.UcitajUredjaje();
        }

        public void ZapamtiVreme()
        {
            repozitorijum.ZapamtiVreme();
        }

        public void ZapamtiZaGraf(double baterije, double paneli, double distribucija, double potrosaci)
        {
            repozitorijum.ZapamtiZaGraf(baterije, paneli, distribucija, potrosaci);
        }
    }
}
