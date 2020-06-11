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
            Uredjaji uredjaji = new Uredjaji();
            uredjaji.Automobili = new List<ElektricniAutomobil>();
            uredjaji.Automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false));
            uredjaji.Baterije = new List<Baterija>();
            uredjaji.Baterije.Add(new Baterija("Bat2", 200, 300));
            uredjaji.Paneli = new List<SolarniPanel>();
            uredjaji.Paneli.Add(new SolarniPanel("Sol1", 100));
            uredjaji.Potrosaci = new List<Potrosac>();
            uredjaji.Potrosaci.Add(new Potrosac("Pot1", 100));
            yield return new TestCaseData().Returns(uredjaji);
        }

        private IRepozitorijum repozitorijum;
        private SimulatorServer simulatorServer;

        [SetUp]
        public void SetUp()
        {
            repozitorijum = new FakeRepozitorijum();
            simulatorServer = new SimulatorServer(repozitorijum);
            MainWindow.SolarniPaneli = new System.ComponentModel.BindingList<SolarniPanel>();
            MainWindow.Baterije = new System.ComponentModel.BindingList<Baterija>();
            MainWindow.Potrosaci = new System.ComponentModel.BindingList<Potrosac>();
            MainWindow.ElektricniAutomobili = new System.ComponentModel.BindingList<ElektricniAutomobil>();
            MainWindow.Punjac = new Punjac();
            MainWindow.SnagaSunca = 0;
        }

        [Test]
        [TestCase(10)]
        public void PromeniSnaguSuncaTest(int novaVrednost)
        {
            bool izvrseno = true;
            int vrednost = 0;
            try
            {
                simulatorServer.PromeniSnaguSunca(novaVrednost);
            }
            catch
            {
                izvrseno = false;
            }
            vrednost = ((FakeRepozitorijum)repozitorijum).snagaSunca;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(novaVrednost, vrednost);
        }

        /*
        [Test]
        [TestCaseSource(typeof(SimulatorServerTest), nameof(UcitajTest1))]
        public Uredjaji PreuzmiUredjajeDobarTest()
        {
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false));
            ((FakeRepozitorijum)repozitorijum).baterije.Add(new Baterija("Bat2", 200, 300));
            ((FakeRepozitorijum)repozitorijum).solarniPaneli.Add(new SolarniPanel("Sol1", 100));
            ((FakeRepozitorijum)repozitorijum).potrosaci.Add(new Potrosac("Pot1", 100));

            return simulatorServer.PreuzmiUredjaje();
        }
        */

        [Test]
        [TestCase(10)]
        public void PodesiOdnosDobarTest(int noviOdnos)
        {
            bool izvrseno = true;
            try
            {
                simulatorServer.PodesiOdnos(noviOdnos);
            }
            catch
            {
                izvrseno = false;
            }
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(noviOdnos, ((FakeRepozitorijum)repozitorijum).jednaSekundaJe);
        }

        [Test]
        [TestCase(20)]
        public void PodesavanjeCeneDobarTest(int cena)
        {
            bool izvrseno = true;
            try
            {
                simulatorServer.PodesavanjeCene(cena);
            }
            catch
            {
                izvrseno = false;
            }
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(cena, ((FakeRepozitorijum)repozitorijum).cenovnik);
        }
    }
}
