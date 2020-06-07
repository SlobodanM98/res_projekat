using Common;
using NUnit.Framework;
using SHES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHESTest
{
    [TestFixture]
    public class SimulatorServerTest
    {
        private static IEnumerable<TestCaseData> UcitajTest1()
        {
            yield return new TestCaseData(new SolarniPanel("Sol1", 100));
        }
        private static IEnumerable<TestCaseData> UcitajTest2()
        {
            yield return new TestCaseData(new SolarniPanel("Sol1", 100));
        }

        [Test]
        [TestCaseSource(typeof(SimulatorServerTest), nameof(UcitajTest1))]
        public void DodajSolarniPanelTest(SolarniPanel solarniPanel)
        {
            IRepozitorijum repozitorijum = new FakeRepozitorijum();
            SimulatorServer simulatorServer = new SimulatorServer(repozitorijum);
            MainWindow.SolarniPaneli = new System.ComponentModel.BindingList<SolarniPanel>();
            bool izvrseno = true;
            int count = 0;
            try
            {
                simulatorServer.DodajSolarniPanel(solarniPanel);
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakeRepozitorijum)repozitorijum).solarniPaneli.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(1, count);
        }

        [Test]
        [TestCase("Sol1")]
        public void UkloniSolarniPanelTest(string jedinstvenoIme)
        {
            IRepozitorijum repozitorijum = new FakeRepozitorijum();
            SimulatorServer simulatorServer = new SimulatorServer(repozitorijum);
            MainWindow.SolarniPaneli = new System.ComponentModel.BindingList<SolarniPanel>();
            MainWindow.SolarniPaneli.Add(new SolarniPanel(jedinstvenoIme, 100));
            bool izvrseno = true;
            int count = -1;
            try
            {
                simulatorServer.UkloniSolarniPanel(jedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakeRepozitorijum)repozitorijum).solarniPaneli.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(0, count);
        }
    }
}
