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
        private static IEnumerable<TestCaseData> UcitajTest5()
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
        [TestCaseSource(typeof(SimulatorServerTest), nameof(UcitajTest1))]
        public void DodajSolarniPanelDobarTest(SolarniPanel solarniPanel)
        {
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
        [TestCaseSource(typeof(SimulatorServerTest), nameof(UcitajTest1))]
        public void DodajSolarniPanelLosTest(SolarniPanel solarniPanel)
        {
            MainWindow.SolarniPaneli.Add(solarniPanel);
            ((FakeRepozitorijum)repozitorijum).solarniPaneli.Add(solarniPanel);
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
        public void UkloniSolarniPanelDobarTest(string jedinstvenoIme)
        {
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
        [TestCase("Sol1")]
        public void UkloniSolarniPanelLosTest(string jedinstvenoIme)
        {
            bool izvrseno = true;
            int count = -1;
            ((FakeRepozitorijum)repozitorijum).solarniPaneli.Add(new SolarniPanel("Sol2", 100));
            MainWindow.SolarniPaneli.Add(new SolarniPanel("Sol2", 100));
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
            Assert.AreEqual(1, count);
        }

        [Test]
        [TestCaseSource(typeof(SimulatorServerTest), nameof(UcitajTest2))]
        public void DodajPotrosacaDobarTest(Potrosac potrosac)
        {
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
        [TestCaseSource(typeof(SimulatorServerTest), nameof(UcitajTest2))]
        public void DodajPotrosacaLosTest(Potrosac potrosac)
        {
            MainWindow.Potrosaci.Add(potrosac);
            ((FakeRepozitorijum)repozitorijum).potrosaci.Add(potrosac);
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
        public void UkloniPotrosacaDobarTest(string jedinstvenoIme)
        {
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
        public void UkloniPotrosacaLosTest(string jedinstvenoIme)
        {
            ((FakeRepozitorijum)repozitorijum).potrosaci.Add(new Potrosac("Pot2", 100));
            MainWindow.Potrosaci.Add(new Potrosac("Pot2", 100));
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
            Assert.AreEqual(1, count);
        }

        [Test]
        [TestCase("Pot1")]
        public void UpaliPotrosacDobarTest(string jedinstvenoIme)
        {
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
        public void UpaliPotrosacLosTest(string jedinstvenoIme)
        {
            ((FakeRepozitorijum)repozitorijum).potrosaci.Add(new Potrosac("Pot2", 100));
            MainWindow.Potrosaci.Add(new Potrosac("Pot2", 100));
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
            Assert.AreEqual(false, upaljen);
        }

        [Test]
        [TestCase("Pot1")]
        public void UgasiPotrosacDobarTest(string jedinstvenoIme)
        {
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
        [TestCase("Pot1")]
        public void UgasiPotrosacLosTest(string jedinstvenoIme)
        {
            ((FakeRepozitorijum)repozitorijum).potrosaci.Add(new Potrosac("Pot2", 100));
            MainWindow.Potrosaci.Add(new Potrosac("Pot2", 100));
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
            Assert.AreEqual(true, upaljen);
        }

        [Test]
        [TestCaseSource(typeof(SimulatorServerTest), nameof(UcitajTest3))]
        public void DodajBaterijuDobarTest(Baterija novaBaterija, bool jesteAutomobil, string AutomobilJedinstvenoIme)
        {
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
        [TestCaseSource(typeof(SimulatorServerTest), nameof(UcitajTest3))]
        public void DodajBaterijuLosTest(Baterija novaBaterija, bool jesteAutomobil, string AutomobilJedinstvenoIme)
        {
            MainWindow.Baterije.Add(novaBaterija);
            ((FakeRepozitorijum)repozitorijum).baterije.Add(novaBaterija);
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
        public void UkloniBaterijuDobarTest(string jedinstvenoIme)
        {
            MainWindow.Baterije.Add(new Baterija(jedinstvenoIme, 100, 200));
            ((FakeRepozitorijum)repozitorijum).baterije.Add(new Baterija(jedinstvenoIme, 100, 200));
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
        [TestCase("Bat1")]
        public void UkloniBaterijuLosTest(string jedinstvenoIme)
        {
            MainWindow.Baterije.Add(new Baterija("Bat2", 100, 200));
            ((FakeRepozitorijum)repozitorijum).baterije.Add(new Baterija("Bat2", 100, 200));
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
            Assert.AreEqual(1, count);
        }

        [Test]
        [TestCaseSource(typeof(SimulatorServerTest), nameof(UcitajTest4))]
        public void DodajElektricniAutomobilDobarTest(ElektricniAutomobil automobil)
        {
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
        [TestCaseSource(typeof(SimulatorServerTest), nameof(UcitajTest4))]
        public void DodajElektricniAutomobilLosTest(ElektricniAutomobil automobil)
        {
            MainWindow.ElektricniAutomobili.Add(automobil);
            ((FakeRepozitorijum)repozitorijum).automobili.Add(automobil);
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
        public void UkloniElektricniAutomobilDobarTest(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));

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
        [TestCase("Auto1")]
        public void UkloniElektricniAutomobilLosTest(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto2", false, false));
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto2", false, false));

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
            Assert.AreEqual(1, count);
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

        [Test]
        [TestCase("Auto1",Result = true)]
        public bool UkljuciNaPunjacDobarTest(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));

            bool naPunjacu = false;
            bool izlaz = simulatorServer.UkljuciNaPunjac(jedinstvenoIme);
            foreach (ElektricniAutomobil e in ((FakeRepozitorijum)repozitorijum).automobili)
            {
                if(e.JedinstvenoIme == jedinstvenoIme && e.NaPunjacu)
                {
                    naPunjacu = true;
                }
            }

            Assert.AreEqual(true, naPunjacu);
            return izlaz;
        }

        [Test]
        [TestCase("Auto1", Result = false)]
        public bool UkljuciNaPunjacLosTest1(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            MainWindow.Punjac.NaPunjacu = true;

            return simulatorServer.UkljuciNaPunjac(jedinstvenoIme);
        }

        [Test]
        [TestCase("Auto1", Result = false)]
        public bool UkljuciNaPunjacLosTest2(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto2", false, false));
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto2", false, false));

            return simulatorServer.UkljuciNaPunjac(jedinstvenoIme);
        }

        [Test]
        [TestCase("Auto1", Result = true)]
        public bool IskljuciSaPunjacaDobarTest(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            MainWindow.Punjac.NaPunjacu = true;
            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false);

            bool izlaz = simulatorServer.IskljuciSaPunjaca(jedinstvenoIme);
            bool naPunjacu = true;
            bool puniSe = true;

            foreach (ElektricniAutomobil e in ((FakeRepozitorijum)repozitorijum).automobili)
            {
                if (e.JedinstvenoIme == jedinstvenoIme && !e.NaPunjacu && !e.PuniSe)
                {
                    naPunjacu = false;
                    puniSe = false;
                }
            }

            Assert.AreEqual(false, naPunjacu);
            Assert.AreEqual(false, puniSe);
            return izlaz;
        }

        [Test]
        [TestCase("Auto1", Result = false)]
        public bool IskljuciSaPunjacaLosTest1(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            MainWindow.Punjac.NaPunjacu = false;

            return simulatorServer.IskljuciSaPunjaca(jedinstvenoIme);
        }

        [Test]
        [TestCase("Auto1", Result = false)]
        public bool IskljuciSaPunjacaLosTest2(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto2", false, false));
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto2", false, false));

            return simulatorServer.IskljuciSaPunjaca(jedinstvenoIme);
        }

        [Test]
        [TestCase("Auto1", Result = true)]
        public bool PokreniPunjenjeDobarTest(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));

            MainWindow.Punjac.NaPunjacu = true;
            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false);

            bool puniSe = false;
            bool izlaz = simulatorServer.PokreniPunjenje(jedinstvenoIme);

            foreach (ElektricniAutomobil e in ((FakeRepozitorijum)repozitorijum).automobili)
            {
                if (e.JedinstvenoIme == jedinstvenoIme && e.PuniSe)
                {
                    puniSe = true;
                }
            }

            Assert.AreEqual(true, puniSe);
            return izlaz;

        }

        [Test]
        [TestCase("Auto1", Result = false)]
        public bool PokreniPunjenjeLosTest1(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            MainWindow.Punjac.NaPunjacu = false;
            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false);

            return simulatorServer.PokreniPunjenje(jedinstvenoIme);

        }

        [Test]
        [TestCase("Auto1", Result = false)]
        public bool PokreniPunjenjeLosTest2(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto2", false, false));
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            MainWindow.Punjac.NaPunjacu = true;
            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto2", false, false);

            return simulatorServer.PokreniPunjenje(jedinstvenoIme);
        }

        [Test]
        [TestCase("Auto1", Result = false)]
        public bool PokreniPunjenjeLosTest3(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            MainWindow.Punjac.PuniSe = true;
            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false);

            return simulatorServer.PokreniPunjenje(jedinstvenoIme);
        }

        [Test]
        [TestCase("Auto1", Result = true)]
        public bool ZaustaviPunjenjeDobarTest(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));

            MainWindow.Punjac.NaPunjacu = true;
            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false);
            MainWindow.Punjac.PuniSe = true;

            bool puniSe = true;
            bool izlaz = simulatorServer.ZaustaviPunjenje(jedinstvenoIme);

            foreach (ElektricniAutomobil e in ((FakeRepozitorijum)repozitorijum).automobili)
            {
                if (e.JedinstvenoIme == jedinstvenoIme && !e.PuniSe)
                {
                    puniSe = false;
                }
            }

            Assert.AreEqual(false, puniSe);
            return izlaz;
        }

        [Test]
        [TestCase("Auto1", Result = false)]
        public bool ZaustaviPunjenjeLosTest1(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));

            MainWindow.Punjac.NaPunjacu = false;
            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false);
            MainWindow.Punjac.PuniSe = true;

            return simulatorServer.ZaustaviPunjenje(jedinstvenoIme);
        }

        [Test]
        [TestCase("Auto1", Result = false)]
        public bool ZaustaviPunjenjeLosTest2(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));

            MainWindow.Punjac.NaPunjacu = true;
            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false);
            MainWindow.Punjac.PuniSe = false;

            return simulatorServer.ZaustaviPunjenje(jedinstvenoIme);
        }

        [Test]
        [TestCase("Auto1", Result = false)]
        public bool ZaustaviPunjenjeLosTest3(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto2", false, false));
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));

            MainWindow.Punjac.NaPunjacu = true;
            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto2", false, false);
            MainWindow.Punjac.PuniSe = true;

            return simulatorServer.ZaustaviPunjenje(jedinstvenoIme);
        }

        [Test]
        [TestCase(10, Result = true)]
        public bool PostavljanjeKapacitetaAutaDobarTest(int trenutniKapacitet)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false));
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false));

            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false);
            MainWindow.Punjac.NaPunjacu = true;
            MainWindow.Punjac.PuniSe = true;

            return simulatorServer.PostavljanjeKapacitetaAuta(trenutniKapacitet);
        }

        [Test]
        [TestCase(10, Result = false)]
        public bool PostavljanjeKapacitetaAutaLosTest1(int trenutniKapacitet)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false));
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false));

            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false);
            MainWindow.Punjac.NaPunjacu = false;
            MainWindow.Punjac.PuniSe = true;

            return simulatorServer.PostavljanjeKapacitetaAuta(trenutniKapacitet);
        }

        [Test]
        [TestCase(10, Result = false)]
        public bool PostavljanjeKapacitetaAutaLosTest2(int trenutniKapacitet)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false));
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false));

            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false);
            MainWindow.Punjac.NaPunjacu = true;
            MainWindow.Punjac.PuniSe = false;

            return simulatorServer.PostavljanjeKapacitetaAuta(trenutniKapacitet);
        }

        [Test]
        [TestCase(10, Result = false)]
        public bool PostavljanjeKapacitetaAutaLosTest3(int trenutniKapacitet)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false));
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false));

            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto2", false, false);
            MainWindow.Punjac.NaPunjacu = true;
            MainWindow.Punjac.PuniSe = true;

            return simulatorServer.PostavljanjeKapacitetaAuta(trenutniKapacitet);
        }

        [Test]
        [TestCaseSource(typeof(SimulatorServerTest), nameof(UcitajTest5))]
        public Uredjaji PreuzmiUredjajeDobarTest()
        {
            ((FakeRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false));
            ((FakeRepozitorijum)repozitorijum).baterije.Add(new Baterija("Bat2", 200, 300));
            ((FakeRepozitorijum)repozitorijum).solarniPaneli.Add(new SolarniPanel("Sol1", 100));
            ((FakeRepozitorijum)repozitorijum).potrosaci.Add(new Potrosac("Pot1", 100));

            return simulatorServer.PreuzmiUredjaje();
        }

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
