using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace CommonTest
{
    [TestFixture]
    public class ElektrodistribucijaTest
    {
        [Test]
        public void ElektrodistribucijaKonstruktorDobriParametri()
        {
            Elektrodistribucija distribucija = new Elektrodistribucija();
            Assert.AreEqual(distribucija.Cena, 1);
            Assert.AreEqual(distribucija.SnagaRazmene, 0);
        }
    }
}
