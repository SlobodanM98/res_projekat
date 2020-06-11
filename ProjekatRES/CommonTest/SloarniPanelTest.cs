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
    public class SloarniPanelTest
    {
        [Test]
        [TestCase("Sol1", 100)]
        [TestCase("Sol2", 200)]
        [TestCase("Sol3", 300)]
        public void SolarniPanelKonstruktorDobriParametri(string jedinstvenoIme, double maksimalnaSnaga)
        {
            SolarniPanel solarniPanel = new SolarniPanel(jedinstvenoIme, maksimalnaSnaga);
            Assert.AreEqual(solarniPanel.JedinstvenoIme, jedinstvenoIme);
            Assert.AreEqual(solarniPanel.MaksimalnaSnaga, maksimalnaSnaga);
        }

        [Test]
        [TestCase("", 100)]
        [TestCase("Sol2", -200)]
        [TestCase(" ", 300)]
        public void SolarniPanelKonstruktorLosiParametri(string jedinstvenoIme, double maksimalnaSnaga)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                SolarniPanel solarniPanel = new SolarniPanel(jedinstvenoIme, maksimalnaSnaga);
            }
           );
        }
    }
}
