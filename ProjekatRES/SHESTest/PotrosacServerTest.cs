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
    public class PotrosacServerTest
    {
        IPotrosacRepozitorijum repozitorijum;
        PotrosacServer potrosacServer;

        private static IEnumerable<TestCaseData> UcitajTest2()
        {
            yield return new TestCaseData(new Potrosac("Pot1", 100));
        }

        [SetUp]
        public void SetUp()
        {
            repozitorijum = new FakePotrosacRepozitorijum();
            potrosacServer = new PotrosacServer(repozitorijum);
            MainWindow.Potrosaci = new System.ComponentModel.BindingList<Potrosac>();
        }

        [Test]
        [TestCaseSource(typeof(PotrosacServerTest), nameof(UcitajTest2))]
        public void DodajPotrosacaDobarTest(Potrosac potrosac)
        {
            bool izvrseno = true;
            int count = -1;
            try
            {
                potrosacServer.DodajPotrosaca(potrosac);
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakePotrosacRepozitorijum)repozitorijum).potrosaci.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(1, count);
        }

        [Test]
        [TestCaseSource(typeof(PotrosacServerTest), nameof(UcitajTest2))]
        public void DodajPotrosacaLosTest(Potrosac potrosac)
        {
            MainWindow.Potrosaci.Add(potrosac);
            ((FakePotrosacRepozitorijum)repozitorijum).potrosaci.Add(potrosac);
            bool izvrseno = true;
            int count = -1;
            try
            {
                potrosacServer.DodajPotrosaca(potrosac);
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakePotrosacRepozitorijum)repozitorijum).potrosaci.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(1, count);
        }

        [Test]
        [TestCase("Pot1")]
        public void UkloniPotrosacaDobarTest(string jedinstvenoIme)
        {
            ((FakePotrosacRepozitorijum)repozitorijum).potrosaci.Add(new Potrosac(jedinstvenoIme, 100));
            MainWindow.Potrosaci.Add(new Potrosac(jedinstvenoIme, 100));
            bool izvrseno = true;
            int count = -1;
            try
            {
                potrosacServer.UkloniPotrosaca(jedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakePotrosacRepozitorijum)repozitorijum).potrosaci.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(0, count);
        }

        [Test]
        [TestCase("Pot1")]
        public void UkloniPotrosacaLosTest(string jedinstvenoIme)
        {
            ((FakePotrosacRepozitorijum)repozitorijum).potrosaci.Add(new Potrosac("Pot2", 100));
            MainWindow.Potrosaci.Add(new Potrosac("Pot2", 100));
            bool izvrseno = true;
            int count = -1;
            try
            {
                potrosacServer.UkloniPotrosaca(jedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            count = ((FakePotrosacRepozitorijum)repozitorijum).potrosaci.Count;
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(1, count);
        }

        [Test]
        [TestCase("Pot1")]
        public void UpaliPotrosacDobarTest(string jedinstvenoIme)
        {
            ((FakePotrosacRepozitorijum)repozitorijum).potrosaci.Add(new Potrosac(jedinstvenoIme, 100));
            MainWindow.Potrosaci.Add(new Potrosac(jedinstvenoIme, 100));
            bool izvrseno = true;
            bool upaljen = false;
            try
            {
                potrosacServer.UpaliPotrosac(jedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            foreach (Potrosac p in ((FakePotrosacRepozitorijum)repozitorijum).potrosaci)
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
            ((FakePotrosacRepozitorijum)repozitorijum).potrosaci.Add(new Potrosac("Pot2", 100));
            MainWindow.Potrosaci.Add(new Potrosac("Pot2", 100));
            bool izvrseno = true;
            bool upaljen = false;
            try
            {
                potrosacServer.UpaliPotrosac(jedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            foreach (Potrosac p in ((FakePotrosacRepozitorijum)repozitorijum).potrosaci)
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
            ((FakePotrosacRepozitorijum)repozitorijum).potrosaci.Add(new Potrosac(jedinstvenoIme, 100));
            MainWindow.Potrosaci.Add(new Potrosac(jedinstvenoIme, 100));
            bool izvrseno = true;
            bool upaljen = true;
            try
            {
                potrosacServer.UgasiPotrosac(jedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            foreach (Potrosac p in ((FakePotrosacRepozitorijum)repozitorijum).potrosaci)
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
            ((FakePotrosacRepozitorijum)repozitorijum).potrosaci.Add(new Potrosac("Pot2", 100));
            MainWindow.Potrosaci.Add(new Potrosac("Pot2", 100));
            bool izvrseno = true;
            bool upaljen = true;
            try
            {
                potrosacServer.UgasiPotrosac(jedinstvenoIme);
            }
            catch
            {
                izvrseno = false;
            }
            foreach (Potrosac p in ((FakePotrosacRepozitorijum)repozitorijum).potrosaci)
            {
                if (p.JedinstvenoIme == jedinstvenoIme)
                {
                    upaljen = p.Upaljen;
                }
            }
            Assert.AreEqual(true, izvrseno);
            Assert.AreEqual(true, upaljen);
        }
    }
}
