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
    public class PodaciZaGrafTest
    {
        private static IEnumerable<TestCaseData> UcitajTestove()
        {
            yield return new TestCaseData(3, DateTime.Parse("10/05/99"), 300, 400, 500, -600);
        }

        private static IEnumerable<TestCaseData> UcitajLoseTestove()
        {
            yield return new TestCaseData(3, DateTime.Parse("10/05/99"), 300, 400, 500, 600);
        }

        [Test]
        [TestCaseSource(typeof(PodaciZaGrafTest), nameof(UcitajTestove))]
        public void PodaciZaGrafKonstruktorDobriParametri(int sat, DateTime datum, double baterije, double distribucija, double solarniPaneli, double potrosaci)
        {
            PodaciZaGraf podaci = new PodaciZaGraf(sat, datum, baterije, distribucija, solarniPaneli, potrosaci);
            Assert.AreEqual(podaci.Sat, sat);
            Assert.AreEqual(podaci.Datum, datum);
            Assert.AreEqual(podaci.Baterije, baterije);
            Assert.AreEqual(podaci.Distribucija, distribucija);
            Assert.AreEqual(podaci.SolarniPaneli, solarniPaneli);
            Assert.AreEqual(podaci.Potrosaci, potrosaci);
        }

        [Test]
        [TestCaseSource(typeof(PodaciZaGrafTest), nameof(UcitajLoseTestove))]
        public void PodaciZaGrafKonstruktorLosiParametri(int sat, DateTime datum, double baterije, double distribucija, double solarniPaneli, double potrosaci)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                PodaciZaGraf podaci = new PodaciZaGraf(sat, datum, baterije, distribucija, solarniPaneli, potrosaci);
            }
            );
        }
    }
}
