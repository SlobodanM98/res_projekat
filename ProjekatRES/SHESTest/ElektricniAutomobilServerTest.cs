using Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHES;

namespace SHESTest
{
    [TestFixture]
    public class ElektricniAutomobilServerTest
    {
        IElektricniAutomobilRepozitorijum repozitorijum;
        ElektricniAutomobilServer elektricniAutomobilServer;

        private static IEnumerable<TestCaseData> UcitajTest1()
        {
            List<TestCaseData> izlaz = new List<TestCaseData>();
            izlaz.Add(new TestCaseData(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false)));
            izlaz.Add(new TestCaseData(new ElektricniAutomobil(new Baterija("Bat2", 100, 200), "Auto2", true, false)));
            izlaz.Add(new TestCaseData(new ElektricniAutomobil(new Baterija("Bat3", 100, 200), "Auto3", true, true)));
            return izlaz;
        }

        [SetUp]
        public void SetUp()
        {
            repozitorijum = new FakeElektricniAutomobilRepozitorijum();
            elektricniAutomobilServer = new ElektricniAutomobilServer(repozitorijum);
            MainWindow.ElektricniAutomobili = new System.ComponentModel.BindingList<ElektricniAutomobil>();
            MainWindow.Punjac = new Punjac();
        }

        [Test]
        [TestCaseSource(typeof(ElektricniAutomobilServerTest), nameof(UcitajTest1))]
        public void DodajElektricniAutomobilDobarTest(ElektricniAutomobil automobil)
        {
            bool izvrseno = true;
            int count = -1;
            try
            {
                elektricniAutomobilServer.DodajElektricniAutomobil(automobil);
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(1, count);
        }

        [Test]
        [TestCaseSource(typeof(ElektricniAutomobilServerTest), nameof(UcitajTest1))]
        public void DodajElektricniAutomobilLosTest(ElektricniAutomobil automobil)
        {
            MainWindow.ElektricniAutomobili.Add(automobil);
            ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Add(automobil);
            bool izvrseno = true;
            int count = -1;
            try
            {
                elektricniAutomobilServer.DodajElektricniAutomobil(automobil);
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(1, count);
        }

        [Test]
        [TestCase("Auto1")]
        public void UkloniElektricniAutomobilDobarTest(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));

            bool izvrseno = true;
            int count = -1;
            try
            {
                elektricniAutomobilServer.UkloniElektricniAutomobil(jedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(0, count);
        }

        [Test]
        [TestCase("Auto1")]
        public void UkloniElektricniAutomobilLosTest(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto2", false, false));
            ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto2", false, false));

            bool izvrseno = true;
            int count = -1;
            try
            {
                elektricniAutomobilServer.UkloniElektricniAutomobil(jedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(1, count);
        }

        [Test]
        [TestCase("Auto1", Result = true)]
        public bool UkljuciNaPunjacDobarTest(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));

            bool naPunjacu = false;
            bool izlaz = elektricniAutomobilServer.UkljuciNaPunjac(jedinstvenoIme);
            foreach (ElektricniAutomobil e in ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili)
            {
                if (e.JedinstvenoIme == jedinstvenoIme && e.NaPunjacu)
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
            ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            MainWindow.Punjac.NaPunjacu = true;

            return elektricniAutomobilServer.UkljuciNaPunjac(jedinstvenoIme);
        }

        [Test]
        [TestCase("Auto1", Result = false)]
        public bool UkljuciNaPunjacLosTest2(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto2", false, false));
            ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto2", false, false));

            return elektricniAutomobilServer.UkljuciNaPunjac(jedinstvenoIme);
        }

        [Test]
        [TestCase("Auto1", Result = true)]
        public bool IskljuciSaPunjacaDobarTest(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            MainWindow.Punjac.NaPunjacu = true;
            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false);

            bool izlaz = elektricniAutomobilServer.IskljuciSaPunjaca(jedinstvenoIme);
            bool naPunjacu = true;
            bool puniSe = true;

            foreach (ElektricniAutomobil e in ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili)
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
            ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            MainWindow.Punjac.NaPunjacu = false;

            return elektricniAutomobilServer.IskljuciSaPunjaca(jedinstvenoIme);
        }

        [Test]
        [TestCase("Auto1", Result = false)]
        public bool IskljuciSaPunjacaLosTest2(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto2", false, false));
            ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto2", false, false));

            return elektricniAutomobilServer.IskljuciSaPunjaca(jedinstvenoIme);
        }

        [Test]
        [TestCase("Auto1", Result = true)]
        public bool PokreniPunjenjeDobarTest(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));

            MainWindow.Punjac.NaPunjacu = true;
            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false);

            bool puniSe = false;
            bool izlaz = elektricniAutomobilServer.PokreniPunjenje(jedinstvenoIme);

            foreach (ElektricniAutomobil e in ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili)
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
            ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            MainWindow.Punjac.NaPunjacu = false;
            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false);

            return elektricniAutomobilServer.PokreniPunjenje(jedinstvenoIme);

        }

        [Test]
        [TestCase("Auto1", Result = false)]
        public bool PokreniPunjenjeLosTest2(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto2", false, false));
            ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            MainWindow.Punjac.NaPunjacu = true;
            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto2", false, false);

            return elektricniAutomobilServer.PokreniPunjenje(jedinstvenoIme);
        }

        [Test]
        [TestCase("Auto1", Result = false)]
        public bool PokreniPunjenjeLosTest3(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            MainWindow.Punjac.PuniSe = true;
            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false);

            return elektricniAutomobilServer.PokreniPunjenje(jedinstvenoIme);
        }

        [Test]
        [TestCase("Auto1", Result = true)]
        public bool ZaustaviPunjenjeDobarTest(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));

            MainWindow.Punjac.NaPunjacu = true;
            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false);
            MainWindow.Punjac.PuniSe = true;

            bool puniSe = true;
            bool izlaz = elektricniAutomobilServer.ZaustaviPunjenje(jedinstvenoIme);

            foreach (ElektricniAutomobil e in ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili)
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
            ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));

            MainWindow.Punjac.NaPunjacu = false;
            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false);
            MainWindow.Punjac.PuniSe = true;

            return elektricniAutomobilServer.ZaustaviPunjenje(jedinstvenoIme);
        }

        [Test]
        [TestCase("Auto1", Result = false)]
        public bool ZaustaviPunjenjeLosTest2(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));
            ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));

            MainWindow.Punjac.NaPunjacu = true;
            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false);
            MainWindow.Punjac.PuniSe = false;

            return elektricniAutomobilServer.ZaustaviPunjenje(jedinstvenoIme);
        }

        [Test]
        [TestCase("Auto1", Result = false)]
        public bool ZaustaviPunjenjeLosTest3(string jedinstvenoIme)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto2", false, false));
            ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), jedinstvenoIme, false, false));

            MainWindow.Punjac.NaPunjacu = true;
            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto2", false, false);
            MainWindow.Punjac.PuniSe = true;

            return elektricniAutomobilServer.ZaustaviPunjenje(jedinstvenoIme);
        }

        [Test]
        [TestCase(10, Result = true)]
        public bool PostavljanjeKapacitetaAutaDobarTest(int trenutniKapacitet)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false));
            ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false));

            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false);
            MainWindow.Punjac.NaPunjacu = true;
            MainWindow.Punjac.PuniSe = true;

            return elektricniAutomobilServer.PostavljanjeKapacitetaAuta(trenutniKapacitet);
        }

        [Test]
        [TestCase(10, Result = false)]
        public bool PostavljanjeKapacitetaAutaLosTest1(int trenutniKapacitet)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false));
            ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false));

            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false);
            MainWindow.Punjac.NaPunjacu = false;
            MainWindow.Punjac.PuniSe = true;

            return elektricniAutomobilServer.PostavljanjeKapacitetaAuta(trenutniKapacitet);
        }

        [Test]
        [TestCase(10, Result = false)]
        public bool PostavljanjeKapacitetaAutaLosTest2(int trenutniKapacitet)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false));
            ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false));

            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false);
            MainWindow.Punjac.NaPunjacu = true;
            MainWindow.Punjac.PuniSe = false;

            return elektricniAutomobilServer.PostavljanjeKapacitetaAuta(trenutniKapacitet);
        }

        [Test]
        [TestCase(10, Result = false)]
        public bool PostavljanjeKapacitetaAutaLosTest3(int trenutniKapacitet)
        {
            MainWindow.ElektricniAutomobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false));
            ((FakeElektricniAutomobilRepozitorijum)repozitorijum).automobili.Add(new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto1", false, false));

            MainWindow.Punjac.Automobil = new ElektricniAutomobil(new Baterija("Bat1", 100, 200), "Auto2", false, false);
            MainWindow.Punjac.NaPunjacu = true;
            MainWindow.Punjac.PuniSe = true;

            return elektricniAutomobilServer.PostavljanjeKapacitetaAuta(trenutniKapacitet);
        }
    }
}
