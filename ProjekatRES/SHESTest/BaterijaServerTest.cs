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
    public class BaterijaServerTest
    {
        IBaterijaRepozitorijum repozitorijum;
        BaterijaServer baterijaServer;

        private static IEnumerable<TestCaseData> UcitajTest1()
        {
            List<TestCaseData> izlaz = new List<TestCaseData>();
            izlaz.Add(new TestCaseData(new Baterija("Bat1", 100, 200), false, ""));
            izlaz.Add(new TestCaseData(new Baterija("Bat2", 200, 300), true, "Auto1"));
            return izlaz;
        }

        [SetUp]
        public void SetUp()
        {
            repozitorijum = new FakeBaterijaRepozitorijum();
            baterijaServer = new BaterijaServer(repozitorijum);
            MainWindow.Baterije = new System.ComponentModel.BindingList<Baterija>();
        }

        [Test]
        [TestCaseSource(typeof(BaterijaServerTest), nameof(UcitajTest1))]
        public void DodajBaterijuDobarTest(Baterija novaBaterija, bool jesteAutomobil, string AutomobilJedinstvenoIme)
        {
            bool izvrseno = true;
            int count = -1;
            try
            {
                baterijaServer.DodajBateriju(novaBaterija, jesteAutomobil, AutomobilJedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakeBaterijaRepozitorijum)repozitorijum).baterije.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(1, count);
        }

        [Test]
        [TestCaseSource(typeof(BaterijaServerTest), nameof(UcitajTest1))]
        public void DodajBaterijuLosTest(Baterija novaBaterija, bool jesteAutomobil, string AutomobilJedinstvenoIme)
        {
            MainWindow.Baterije.Add(novaBaterija);
            ((FakeBaterijaRepozitorijum)repozitorijum).baterije.Add(novaBaterija);
            bool izvrseno = true;
            int count = -1;
            try
            {
                baterijaServer.DodajBateriju(novaBaterija, jesteAutomobil, AutomobilJedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakeBaterijaRepozitorijum)repozitorijum).baterije.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(1, count);
        }

        [Test]
        [TestCase("Bat1")]
        public void UkloniBaterijuDobarTest(string jedinstvenoIme)
        {
            MainWindow.Baterije.Add(new Baterija(jedinstvenoIme, 100, 200));
            ((FakeBaterijaRepozitorijum)repozitorijum).baterije.Add(new Baterija(jedinstvenoIme, 100, 200));
            bool izvrseno = true;
            int count = -1;
            try
            {
                baterijaServer.UkloniBateriju(jedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakeBaterijaRepozitorijum)repozitorijum).baterije.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(0, count);
        }

        [Test]
        [TestCase("Bat1")]
        public void UkloniBaterijuLosTest(string jedinstvenoIme)
        {
            MainWindow.Baterije.Add(new Baterija("Bat2", 100, 200));
            ((FakeBaterijaRepozitorijum)repozitorijum).baterije.Add(new Baterija("Bat2", 100, 200));
            bool izvrseno = true;
            int count = -1;
            try
            {
                baterijaServer.UkloniBateriju(jedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakeBaterijaRepozitorijum)repozitorijum).baterije.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(1, count);
        }
    }
}
