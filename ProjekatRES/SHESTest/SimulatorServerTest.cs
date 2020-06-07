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
            yield return new TestCaseData(new Potrosac("Pot1", 100));
        }
        private static IEnumerable<TestCaseData> UcitajTest3()
        {
            List<TestCaseData> izlaz = new List<TestCaseData>();
            izlaz.Add(new TestCaseData(new Baterija("Bat1", 100, 200), false, ""));
            izlaz.Add(new TestCaseData(new Baterija("Bat2", 200, 300), true, "Auto1"));
            return izlaz;
        }
        private static IEnumerable<TestCaseData> UcitajTest4()
        {
            List<TestCaseData> izlaz = new List<TestCaseData>();
            izlaz.Add(new TestCaseData(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false)));
            izlaz.Add(new TestCaseData(new ElektricniAutomobil(new Baterija("Bat2", 100, 200), "Auto2", true, false)));
            izlaz.Add(new TestCaseData(new ElektricniAutomobil(new Baterija("Bat3", 100, 200), "Auto3", true, true)));
            return izlaz;
        }

        [Test]
        [TestCaseSource(typeof(SimulatorServerTest), nameof(UcitajTest1))]
        public void DodajSolarniPanelTest(SolarniPanel solarniPanel)
        {
            IRepozitorijum repozitorijum = new FakeRepozitorijum();
            SimulatorServer simulatorServer = new SimulatorServer(repozitorijum);
            MainWindow.SolarniPaneli = new System.ComponentModel.BindingList<SolarniPanel>();
            bool izvrseno = true;
            int count = -1;
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
            bool izvrseno = true;
            int count = -1;
            ((FakeRepozitorijum)repozitorijum).solarniPaneli.Add(new SolarniPanel(jedinstvenoIme, 100));
            MainWindow.SolarniPaneli.Add(new SolarniPanel(jedinstvenoIme, 100));
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

        [Test]
        [TestCaseSource(typeof(SimulatorServerTest), nameof(UcitajTest2))]
        public void DodajPotrosacaTest(Potrosac potrosac)
        {
            IRepozitorijum repozitorijum = new FakeRepozitorijum();
            SimulatorServer simulatorServer = new SimulatorServer(repozitorijum);
            MainWindow.Potrosaci = new System.ComponentModel.BindingList<Potrosac>();
            bool izvrseno = true;
            int count = -1;
            try
            {
                simulatorServer.DodajPotrosaca(potrosac);
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakeRepozitorijum)repozitorijum).potrosaci.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(1, count);
        }

        [Test]
        [TestCase("Pot1")]
        public void UkloniPotrosacaTest(string jedinstvenoIme)
        {
            IRepozitorijum repozitorijum = new FakeRepozitorijum();
            SimulatorServer simulatorServer = new SimulatorServer(repozitorijum);
            MainWindow.Potrosaci = new System.ComponentModel.BindingList<Potrosac>();
            ((FakeRepozitorijum)repozitorijum).potrosaci.Add(new Potrosac(jedinstvenoIme, 100));
            MainWindow.Potrosaci.Add(new Potrosac(jedinstvenoIme, 100));
            bool izvrseno = true;
            int count = -1;
            try
            {
                simulatorServer.UkloniPotrosaca(jedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakeRepozitorijum)repozitorijum).potrosaci.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(0, count);
        }

        [Test]
        [TestCase("Pot1")]
        public void UpaliPotrosacTest(string jedinstvenoIme)
        {
            IRepozitorijum repozitorijum = new FakeRepozitorijum();
            SimulatorServer simulatorServer = new SimulatorServer(repozitorijum);
            MainWindow.Potrosaci = new System.ComponentModel.BindingList<Potrosac>();
            ((FakeRepozitorijum)repozitorijum).potrosaci.Add(new Potrosac(jedinstvenoIme, 100));
            MainWindow.Potrosaci.Add(new Potrosac(jedinstvenoIme, 100));
            bool izvrseno = true;
            bool upaljen = false;
            try
            {
                simulatorServer.UpaliPotrosac(jedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            foreach (Potrosac p in ((FakeRepozitorijum)repozitorijum).potrosaci)
            {
                if (p.JedinstvenoIme == jedinstvenoIme)
                {
                    upaljen = p.Upaljen;
                }
            }
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(true, upaljen);
        }

        [Test]
        [TestCase("Pot1")]
        public void UgasiPotrosacTest(string jedinstvenoIme)
        {
            IRepozitorijum repozitorijum = new FakeRepozitorijum();
            SimulatorServer simulatorServer = new SimulatorServer(repozitorijum);
            MainWindow.Potrosaci = new System.ComponentModel.BindingList<Potrosac>();
            ((FakeRepozitorijum)repozitorijum).potrosaci.Add(new Potrosac(jedinstvenoIme, 100));
            MainWindow.Potrosaci.Add(new Potrosac(jedinstvenoIme, 100));
            bool izvrseno = true;
            bool upaljen = true;
            try
            {
                simulatorServer.UgasiPotrosac(jedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            foreach (Potrosac p in ((FakeRepozitorijum)repozitorijum).potrosaci)
            {
                if (p.JedinstvenoIme == jedinstvenoIme)
                {
                    upaljen = p.Upaljen;
                }
            }
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(false, upaljen);
        }

        [Test]
        [TestCaseSource(typeof(SimulatorServerTest), nameof(UcitajTest3))]
        public void DodajBaterijuTest(Baterija novaBaterija, bool jesteAutomobil, string AutomobilJedinstvenoIme)
        {
            IRepozitorijum repozitorijum = new FakeRepozitorijum();
            SimulatorServer simulatorServer = new SimulatorServer(repozitorijum);
            MainWindow.Baterije = new System.ComponentModel.BindingList<Baterija>();
            bool izvrseno = true;
            int count = -1;
            try
            {
                simulatorServer.DodajBateriju(novaBaterija, jesteAutomobil, AutomobilJedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakeRepozitorijum)repozitorijum).baterije.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(1, count);
        }

        [Test]
        [TestCase("Bat1")]
        public void UkloniBaterijuTest(string jedinstvenoIme)
        {
            IRepozitorijum repozitorijum = new FakeRepozitorijum();
            SimulatorServer simulatorServer = new SimulatorServer(repozitorijum);
            MainWindow.Baterije = new System.ComponentModel.BindingList<Baterija>();
            MainWindow.Baterije.Add(new Baterija(jedinstvenoIme, 100, 200));
            bool izvrseno = true;
            int count = -1;
            try
            {
                simulatorServer.UkloniBateriju(jedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakeRepozitorijum)repozitorijum).baterije.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(0, count);
        }

        [Test]
        [TestCaseSource(typeof(SimulatorServerTest), nameof(UcitajTest4))]
        public void DodajElektricniAutomobilTest(ElektricniAutomobil automobil)
        {
            IRepozitorijum repozitorijum = new FakeRepozitorijum();
            SimulatorServer simulatorServer = new SimulatorServer(repozitorijum);
            MainWindow.ElektricniAutomobili = new System.ComponentModel.BindingList<ElektricniAutomobil>();
            MainWindow.Baterije = new System.ComponentModel.BindingList<Baterija>();
            bool izvrseno = true;
            int count = -1;
            try
            {
                simulatorServer.DodajElektricniAutomobil(automobil);
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakeRepozitorijum)repozitorijum).automobili.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(1, count);
        }

        [Test]
        [TestCase("Auto1")]
        public void UkloniElektricniAutomobil(string jedinstvenoIme)
        {
            IRepozitorijum repozitorijum = new FakeRepozitorijum();
            SimulatorServer simulatorServer = new SimulatorServer(repozitorijum);
            MainWindow.ElektricniAutomobili = new System.ComponentModel.BindingList<ElektricniAutomobil>();
            MainWindow.Baterije = new System.ComponentModel.BindingList<Baterija>();
            MainWindow.Punjac = new Punjac();
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            simulatorServer.DodajElektricniAutomobil(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));

            bool izvrseno = true;
            int count = -1;
            try
            {
                simulatorServer.UkloniElektricniAutomobil(jedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakeRepozitorijum)repozitorijum).automobili.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(0, count);
        }

        [Test]
        [TestCase(10)]
        public void PromeniSnaguSunca(int novaVrednost)
        {
            IRepozitorijum repozitorijum = new FakeRepozitorijum();
            SimulatorServer simulatorServer = new SimulatorServer(repozitorijum);
            MainWindow.SnagaSunca = 0;
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

        [Test]
        [TestCase("Auto1")]
        public void UkljuciNaPunjac(string jedinstvenoIme)
        {
            IRepozitorijum repozitorijum = new FakeRepozitorijum();
            SimulatorServer simulatorServer = new SimulatorServer(repozitorijum);
            MainWindow.ElektricniAutomobili = new System.ComponentModel.BindingList<ElektricniAutomobil>();
            MainWindow.Baterije = new System.ComponentModel.BindingList<Baterija>();
            MainWindow.Punjac = new Punjac();
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));

            bool izvrseno = true;
            bool ukljucen = false;
            try
            {
                simulatorServer.UkljuciNaPunjac(jedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            foreach(ElektricniAutomobil a in ((FakeRepozitorijum)repozitorijum).automobili)
            {
                if(a.JedinstvenoIme == jedinstvenoIme)
                {
                    ukljucen = a.NaPunjacu;
                    break;
                }
            }
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(true, ukljucen);
        }

        [Test]
        [TestCase("Auto1")]
        public void IskljuciSaPunjaca(string jedinstvenoIme)
        {
            IRepozitorijum repozitorijum = new FakeRepozitorijum();
            SimulatorServer simulatorServer = new SimulatorServer(repozitorijum);
            MainWindow.ElektricniAutomobili = new System.ComponentModel.BindingList<ElektricniAutomobil>();
            MainWindow.Baterije = new System.ComponentModel.BindingList<Baterija>();
            MainWindow.Punjac = new Punjac();
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));

            bool izvrseno = true;
            bool ukljucen = true;
            try
            {
                simulatorServer.IskljuciSaPunjaca(jedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            foreach (ElektricniAutomobil a in ((FakeRepozitorijum)repozitorijum).automobili)
            {
                if (a.JedinstvenoIme == jedinstvenoIme)
                {
                    ukljucen = a.NaPunjacu;
                    break;
                }
            }
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(false, ukljucen);
        }

        [Test]
        [TestCase("Auto1")]
        public void PokreniPunjenje(string jedinstvenoIme)
        {
            IRepozitorijum repozitorijum = new FakeRepozitorijum();
            SimulatorServer simulatorServer = new SimulatorServer(repozitorijum);
            MainWindow.ElektricniAutomobili = new System.ComponentModel.BindingList<ElektricniAutomobil>();
            MainWindow.Baterije = new System.ComponentModel.BindingList<Baterija>();
            MainWindow.Punjac = new Punjac();
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));

            bool izvrseno = true;
            bool puniSe = false;
            bool puniSePunjac = false;
            MainWindow.Punjac.NaPunjacu = true;
            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false);
            try
            {
                simulatorServer.PokreniPunjenje(jedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            foreach (ElektricniAutomobil a in ((FakeRepozitorijum)repozitorijum).automobili)
            {
                if (a.JedinstvenoIme == jedinstvenoIme)
                {
                    puniSe = a.PuniSe;
                    break;
                }
            }
            puniSePunjac = ((FakeRepozitorijum)repozitorijum).puniSePunjac;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(true, puniSe);
            Assert.AreEqual(true, puniSePunjac);
        }

        [Test]
        [TestCase("Auto1")]
        public void ZaustaviPunjenje(string jedinstvenoIme)
        {
            IRepozitorijum repozitorijum = new FakeRepozitorijum();
            SimulatorServer simulatorServer = new SimulatorServer(repozitorijum);
            MainWindow.ElektricniAutomobili = new System.ComponentModel.BindingList<ElektricniAutomobil>();
            MainWindow.Baterije = new System.ComponentModel.BindingList<Baterija>();
            MainWindow.Punjac = new Punjac();
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));

            bool izvrseno = true;
            bool puniSe = true;
            bool puniSePunjac = true;
            MainWindow.Punjac.NaPunjacu = true;
            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false);
            MainWindow.Punjac.PuniSe = true;
            try
            {
                simulatorServer.ZaustaviPunjenje(jedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            foreach (ElektricniAutomobil a in ((FakeRepozitorijum)repozitorijum).automobili)
            {
                if (a.JedinstvenoIme == jedinstvenoIme)
                {
                    puniSe = a.PuniSe;
                    break;
                }
            }
            puniSePunjac = ((FakeRepozitorijum)repozitorijum).puniSePunjac;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(false, puniSe);
            Assert.AreEqual(false, puniSePunjac);
        }
    }
}
