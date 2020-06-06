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
    public class PotrosacTest
    {
        [Test]
        [TestCase("Pot1", 100)]
        [TestCase("Pot2", 200)]
        [TestCase("Pot3", 300)]
        public void PotrosacKonstruktorDobriParametri(string jedinstvenoIme, double potrosnja)
        {
            Potrosac potrosac = new Potrosac(jedinstvenoIme, potrosnja);
            Assert.AreEqual(potrosac.JedinstvenoIme, jedinstvenoIme);
            Assert.AreEqual(potrosac.Potrosnja, potrosnja);
        }
    }
}
