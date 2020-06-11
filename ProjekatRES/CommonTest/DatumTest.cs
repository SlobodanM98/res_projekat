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
    public class DatumTest
    {
        [Test]
        [TestCase("10/6/2020")]
        public void DobarKonstruktorDatum(string datumBaza)
        {
            Datum datum = new Datum(datumBaza);
            Assert.AreEqual(datum.DatumBaza, datumBaza.Trim());
        }

        [Test]
        [TestCase("50/10/2020")]
        [TestCase("0/10/2020")]
        [TestCase("/10/2020")]
        public void LosKonstruktorDatum(string datumBaza)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Datum datum = new Datum(datumBaza);
            }
            );
        }
    }
}
