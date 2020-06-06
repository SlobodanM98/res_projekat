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
    public class PunjacTest
    {
        [Test]
        public void PunjacKonstruktorDobriParametri()
        {
            Punjac punjac = new Punjac();
            Assert.AreEqual(punjac.Automobil, null);
            Assert.AreEqual(punjac.NaPunjacu, false);
            Assert.AreEqual(punjac.PuniSe, false);
        }
    }
}
