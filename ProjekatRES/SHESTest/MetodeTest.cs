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
    public class MetodeTest
    {
        Metode metode;
        IMetode repozitorijum;

        private static IEnumerable<TestCaseData> UcitajTest1()
        {
            yield return new TestCaseData(new Baterija("Bat1", 100, 200)).Returns((double)100 / (double)3600);
        }
        private static IEnumerable<TestCaseData> UcitajTest2()
        {
            yield return new TestCaseData(new Baterija("Bat1", 100, 200)).Returns(0);
        }
        private static IEnumerable<TestCaseData> UcitajTest3()
        {
            List<TestCaseData> izlaz = new List<TestCaseData>();
            izlaz.Add(new TestCaseData(new Baterija("Bat1", 100, 200), false).Returns((double)100 / (double)3600));
            izlaz.Add(new TestCaseData(new Baterija("Bat2", 200, 300), true).Returns((double)200 / (double)3600));
            return izlaz;
        }
        private static IEnumerable<TestCaseData> UcitajTest4()
        {
            List<TestCaseData> izlaz = new List<TestCaseData>();
            izlaz.Add(new TestCaseData(new Baterija("Bat1", 100, 200), false).Returns(0));
            izlaz.Add(new TestCaseData(new Baterija("Bat2", 200, 300), true).Returns(0));
            return izlaz;
        }
        private static IEnumerable<TestCaseData> UcitajTest5()
        {
            yield return new TestCaseData(new Baterija("Bat1", 100, 200));
        }

        [SetUp]
        public void SetUp()
        {
            repozitorijum = new FakeMetodeRepozitorijum();
            metode = new Metode(repozitorijum);
        }

        [Test]
        [TestCaseSource(typeof(MetodeTest), nameof(UcitajTest1))]
        public double PraznjenjeBaterijeDobarTest1(Baterija baterija)
        {
            ((FakeMetodeRepozitorijum)repozitorijum).baterije.Add(baterija);

            return metode.PraznjenjeBaterije(baterija);
        }

        [Test]
        [TestCaseSource(typeof(MetodeTest), nameof(UcitajTest2))]
        public double PraznjenjeBaterijeDobarTest2(Baterija baterija)
        {
            baterija.TrenutniKapacitet = -1;
            ((FakeMetodeRepozitorijum)repozitorijum).baterije.Add(baterija);

            return metode.PraznjenjeBaterije(baterija);
        }

        [Test]
        [TestCaseSource(typeof(MetodeTest), nameof(UcitajTest3))]
        public double PunjenjeBaterijeDobarTest1(Baterija baterija, bool baterijaAuta)
        {
            ((FakeMetodeRepozitorijum)repozitorijum).baterije.Add(baterija);

            return metode.PunjenjeBaterije(baterija, baterijaAuta);

        }

        [Test]
        [TestCaseSource(typeof(MetodeTest), nameof(UcitajTest4))]
        public double PunjenjeBaterijeDobarTest2(Baterija baterija, bool baterijaAuta)
        {
            baterija.TrenutniKapacitet = -1;
            ((FakeMetodeRepozitorijum)repozitorijum).baterije.Add(baterija);

            return metode.PunjenjeBaterije(baterija, baterijaAuta);
        }

        [Test]
        [TestCaseSource(typeof(MetodeTest), nameof(UcitajTest5))]
        public void ResetBaterijeDobarTest(Baterija baterija)
        {
            ((FakeMetodeRepozitorijum)repozitorijum).baterije.Add(baterija);
            bool izvrseno = true;
            bool izlaz = true;
            try
            {
                metode.ResetBaterije(baterija);
            }
            catch
            {
                izvrseno = false;
            }

            foreach (Baterija b in ((FakeMetodeRepozitorijum)repozitorijum).baterije)
            {
                if (b.PrazniSe != false)
                {
                    izlaz = false;
                    break;
                }
                if(b.PuniSe != false)
                {
                    izlaz = false;
                    break;
                }
            }

            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(true, izlaz);
        }

        [Test]
        public void UcitajDatumeDobarTest()
        {
            ((FakeMetodeRepozitorijum)repozitorijum).datumiUcitavanje.Add(new Datum("1/1/11"));
            ((FakeMetodeRepozitorijum)repozitorijum).datumiUcitavanje.Add(new Datum("2/1/11"));
            bool izvrseno = true;
            int count = -1;
            try
            {
                metode.UcitajDatume();
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakeMetodeRepozitorijum)repozitorijum).datumi.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(2, count);
        }

        [Test]
        public void UcitajPoslednjiSatDobarTest()
        {
            ((FakeMetodeRepozitorijum)repozitorijum).datumiUcitavanje.Add(new Datum("1/1/11"));
            ((FakeMetodeRepozitorijum)repozitorijum).datumiUcitavanje.Add(new Datum("2/1/11"));
            bool izvrseno = true;
            int count = -1;
            try
            {
                metode.UcitajPoslednjiSat();
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakeMetodeRepozitorijum)repozitorijum).datumi.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(2, count);
        }

        [Test]
        public void UcitajUredjajeDobarTest()
        {
            ((FakeMetodeRepozitorijum)repozitorijum).snagaSuncaUcitavanje = 10;
            DateTime datum = DateTime.Now;
            ((FakeMetodeRepozitorijum)repozitorijum).vremeUcitavanje = datum;
            ((FakeMetodeRepozitorijum)repozitorijum).baterijeUcitavanje.Add(new Baterija("Bat1", 100, 200));
            ((FakeMetodeRepozitorijum)repozitorijum).baterijeUcitavanje.Add(new Baterija("Bat2", 200, 300));
            ((FakeMetodeRepozitorijum)repozitorijum).potrosaciUcitavanje.Add(new Potrosac("Pot1", 100));
            ((FakeMetodeRepozitorijum)repozitorijum).potrosaciUcitavanje.Add(new Potrosac("Pot2", 200));
            ((FakeMetodeRepozitorijum)repozitorijum).solarniPaneliUcitavanje.Add(new SolarniPanel("Sol1", 100));
            ((FakeMetodeRepozitorijum)repozitorijum).solarniPaneliUcitavanje.Add(new SolarniPanel("Sol2", 200));
            ((FakeMetodeRepozitorijum)repozitorijum).automobiliUcitavanje.Add(new ElektricniAutomobil(new Baterija("BatAuto1", 100, 200), "Auto1", false, false));
            ((FakeMetodeRepozitorijum)repozitorijum).automobiliUcitavanje.Add(new ElektricniAutomobil(new Baterija("BatAuto2", 200, 300), "Auto2", false, false));
            bool izvrseno = true;
            int countBaterije = -1;
            int countPotrosaci = -1;
            int countSolarni = -1;
            int countAuto = -1;
            try
            {
                metode.UcitajUredjaje();
            }
            catch
            {
                izvrseno = false;
            }
            countBaterije = ((FakeMetodeRepozitorijum)repozitorijum).baterije.Count;
            countPotrosaci = ((FakeMetodeRepozitorijum)repozitorijum).potrosaci.Count;
            countSolarni = ((FakeMetodeRepozitorijum)repozitorijum).solarniPaneli.Count;
            countAuto = ((FakeMetodeRepozitorijum)repozitorijum).automobili.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(2, countBaterije);
            Assert.AreEqual(2, countPotrosaci);
            Assert.AreEqual(2, countSolarni);
            Assert.AreEqual(2, countAuto);
            Assert.AreEqual(10, ((FakeMetodeRepozitorijum)repozitorijum).snagaSunca);
            Assert.AreEqual(datum, ((FakeMetodeRepozitorijum)repozitorijum).vreme);
        }

        [Test]
        public void ZapamtiVremeDobarTest()
        {
            bool izvrseno = true;
            try
            {
                metode.ZapamtiVreme();
            }
            catch
            {
                izvrseno = false;
            }
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(DateTime.Parse("1/1/11"), ((FakeMetodeRepozitorijum)repozitorijum).vreme);
        }

        [Test]
        [TestCase(10, 20, 30, 40)]
        public void ZapamtiZaGrafDobarTest(double baterije, double paneli, double distribucija, double potrosaci)
        {
            bool izvrseno = true;
            try
            {
                metode.ZapamtiZaGraf(baterije, paneli, distribucija, potrosaci);
            }
            catch
            {
                izvrseno = false;
            }
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(baterije, ((FakeMetodeRepozitorijum)repozitorijum).baterijeGraf);
            Assert.AreEqual(paneli, ((FakeMetodeRepozitorijum)repozitorijum).paneliGraf);
            Assert.AreEqual(distribucija, ((FakeMetodeRepozitorijum)repozitorijum).distribucijaGraf);
            Assert.AreEqual(potrosaci, ((FakeMetodeRepozitorijum)repozitorijum).potrosaciGraf);
        }
    }
}
