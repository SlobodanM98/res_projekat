using Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTest
{
    [TestFixture]
    public class BaterijaTest
    {
        [Test]
        [TestCase("Bat1", 100, 200)]
        [TestCase("Bat2", 200, 300)]
        [TestCase("Bat3", 300, 400)]
        public void BaterijaKonstruktorDobriParametri(string jedinstvenoIme, double maksimalnaSnaga, double kapacitet)
        {
            Baterija baterija = new Baterija(jedinstvenoIme, maksimalnaSnaga, kapacitet);

            Assert.AreEqual(baterija.JedinstvenoIme, jedinstvenoIme);
            Assert.AreEqual(baterija.MaksimalnaSnaga, maksimalnaSnaga);
            Assert.AreEqual(baterija.Kapacitet, kapacitet);
            Assert.AreEqual(0, baterija.TrenutniKapacitet);
            Assert.AreEqual(null, baterija.AutomobilJedinstvenoIme);
            Assert.AreEqual(false, baterija.PuniSe);
            Assert.AreEqual(false, baterija.PrazniSe);
        }
    }
}
