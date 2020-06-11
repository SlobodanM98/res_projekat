using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SHES
{
    public class SimulatorServer : ISimulator
    {
        private IRepozitorijum repozitorijum;

        public SimulatorServer(IRepozitorijum repo)
        {
            repozitorijum = repo;
        }

        public SimulatorServer() { }

        public void PromeniSnaguSunca(int novaVrednost)
        {
            MainWindow.SnagaSunca = novaVrednost;
            if (repozitorijum == null)
            {
                repozitorijum = new Repozitorijum();
            }
            repozitorijum.PromeniSnaguSunca(novaVrednost);
        }

        public Uredjaji PreuzmiUredjaje()
        {
            if(repozitorijum == null)
            {
                repozitorijum = new Repozitorijum();
            }
            return repozitorijum.PreuzmiUredjaje();
        }

        public void PodesiOdnos(int noviOdnos)
        {
            if (repozitorijum == null)
            {
                repozitorijum = new Repozitorijum();
            }
            repozitorijum.PodesiOdnos(noviOdnos);
        }

        public void PodesavanjeCene(int cena)
        {
            if (repozitorijum == null)
            {
                repozitorijum = new Repozitorijum();
            }
            repozitorijum.PodesavanjeCene(cena);
        }
    }
}
