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
    public class SolarniPanelServerTest
    {
        ISolarniPanelRepozitorijum repozitorijum;
        SolarniPanelServer solarniPanelServer;

        private static IEnumerable<TestCaseData> UcitajTest1()
        {
            yield return new TestCaseData(new SolarniPanel("Sol1", 100));
        }

        [SetUp]
        public void SetUp()
        {
            repozitorijum = new FakeSolarniPanelRepozitorijum();
            solarniPanelServer = new SolarniPanelServer(repozitorijum);
            MainWindow.SolarniPaneli = new System.ComponentModel.BindingList<SolarniPanel>();
        }

        [Test]
        [TestCaseSource(typeof(SolarniPanelServerTest), nameof(UcitajTest1))]
        public void DodajSolarniPanelDobarTest(SolarniPanel solarniPanel)
        {
            bool izvrseno = true;
            int count = -1;
            try
            {
                solarniPanelServer.DodajSolarniPanel(solarniPanel);
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakeSolarniPanelRepozitorijum)repozitorijum).solarniPaneli.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(1, count);
        }

        [Test]
        [TestCaseSource(typeof(SolarniPanelServerTest), nameof(UcitajTest1))]
        public void DodajSolarniPanelLosTest(SolarniPanel solarniPanel)
        {
            MainWindow.SolarniPaneli.Add(solarniPanel);
            ((FakeSolarniPanelRepozitorijum)repozitorijum).solarniPaneli.Add(solarniPanel);
            bool izvrseno = true;
            int count = -1;
            try
            {
                solarniPanelServer.DodajSolarniPanel(solarniPanel);
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakeSolarniPanelRepozitorijum)repozitorijum).solarniPaneli.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(1, count);
        }

        [Test]
        [TestCase("Sol1")]
        public void UkloniSolarniPanelDobarTest(string jedinstvenoIme)
        {
            bool izvrseno = true;
            int count = -1;
            ((FakeSolarniPanelRepozitorijum)repozitorijum).solarniPaneli.Add(new SolarniPanel(jedinstvenoIme, 100));
            MainWindow.SolarniPaneli.Add(new SolarniPanel(jedinstvenoIme, 100));
            try
            {
                solarniPanelServer.UkloniSolarniPanel(jedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakeSolarniPanelRepozitorijum)repozitorijum).solarniPaneli.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(0, count);
        }

        [Test]
        [TestCase("Sol1")]
        public void UkloniSolarniPanelLosTest(string jedinstvenoIme)
        {
            bool izvrseno = true;
            int count = -1;
            ((FakeSolarniPanelRepozitorijum)repozitorijum).solarniPaneli.Add(new SolarniPanel("Sol2", 100));
            MainWindow.SolarniPaneli.Add(new SolarniPanel("Sol2", 100));
            try
            {
                solarniPanelServer.UkloniSolarniPanel(jedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakeSolarniPanelRepozitorijum)repozitorijum).solarniPaneli.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(1, count);
        }
    }
}
