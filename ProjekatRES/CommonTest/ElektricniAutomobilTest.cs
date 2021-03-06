﻿using Common;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTest
{
    [TestFixture]
    public class ElektricniAutomobilTest
    {

        private static IEnumerable<TestCaseData> UcitajTestove()
        {
            yield return new TestCaseData(new Baterija("Bat1", 100, 200), "Auto1", true, true);
        }

        private static IEnumerable<TestCaseData> UcitajLoseTestove()
        {
            List<TestCaseData> lista = new List<TestCaseData>();
            lista.Add(new TestCaseData(new Baterija("Bat1", 100, 200), "", true, true));
            //lista.Add(new TestCaseData(new Baterija("Bat1", -100, 200), "Auto1", true, true));
            //lista.Add(new TestCaseData(new Baterija("Bat1", -100, 300), "Auto2", true, true));
            return lista;
        }

        [Test]
        [TestCase("auto1", 100, 200, 0)]
        [TestCase("auto2", 200, 300, 1)]
        [TestCase("auto3", 300, 400, 2)]
        public void ElektricniAutomobilKonstruktor1DobriParametri(string jedinstvenoIme, double maksimalnaSnaga, double kapacitet, int brojBaterije)
        {
            ElektricniAutomobil elektricniAutomobil = new ElektricniAutomobil(jedinstvenoIme, maksimalnaSnaga, kapacitet, brojBaterije);
            Assert.AreEqual(elektricniAutomobil.JedinstvenoIme, jedinstvenoIme);
            Assert.AreEqual(elektricniAutomobil.BaterijaAuta.MaksimalnaSnaga, maksimalnaSnaga);
            Assert.AreEqual(elektricniAutomobil.BaterijaAuta.Kapacitet, kapacitet);
            Assert.AreEqual(elektricniAutomobil.BaterijaAuta.JedinstvenoIme, "BaterijaAuta" + brojBaterije.ToString());
            Assert.AreEqual(false, elektricniAutomobil.NaPunjacu);
            Assert.AreEqual(false, elektricniAutomobil.PuniSe);
        }

        [Test]
        [TestCaseSource(typeof(ElektricniAutomobilTest), nameof(UcitajTestove))]
        public void ElektricniAutomobilKonstruktor2DobriParametri(Baterija baterija, string jedinstvenoIme, bool naPunjacu, bool puniSe)
        {
            ElektricniAutomobil elektricniAutomobil = new ElektricniAutomobil(baterija, jedinstvenoIme, naPunjacu, puniSe);
            Assert.AreEqual(elektricniAutomobil.JedinstvenoIme, jedinstvenoIme);
            Assert.AreEqual(elektricniAutomobil.BaterijaAuta, baterija);
            Assert.AreEqual(elektricniAutomobil.NaPunjacu, naPunjacu);
            Assert.AreEqual(elektricniAutomobil.PuniSe, puniSe);
        }

        [Test]
        [TestCase("", 100, 200, 0)]
        [TestCase("auto2", -200, 300, 1)]
        [TestCase("auto3", 300, -400, 2)]
        public void ElektricniAutomobilKonstruktor1LosiParametri(string jedinstvenoIme, double maksimalnaSnaga, double kapacitet, int brojBaterije)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                ElektricniAutomobil automobil = new ElektricniAutomobil(jedinstvenoIme, maksimalnaSnaga, kapacitet, brojBaterije);
            }
           );
        }

        [Test]
        [TestCaseSource(typeof(ElektricniAutomobilTest), nameof(UcitajLoseTestove))]
        public void ElektricniAutomobilKonstruktor2LosiParametri(Baterija baterija, string jedinstvenoIme, bool naPunjacu, bool puniSe)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                ElektricniAutomobil automobil = new ElektricniAutomobil(baterija, jedinstvenoIme, naPunjacu, puniSe);
            }
            );
        }
    }
}
