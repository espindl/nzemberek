using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NZemberek.Cekirdek.Araclar;

namespace NZemberek.TurkiyeTurkcesi.Testler
{
    [TestFixture]
    public class TestMetinAraclari
    {
        [Test]
        public void levenstheinTest()
        {
            string s1 = "çekostlavakya";
            string s2 = "cekoslavakia";
            int expected = 3;
            int actual = MetinAraclari.editDistance(s1, s2);
            Assert.AreEqual(expected, actual, string.Format("{0} beklenirken {1} geldi.", expected, actual));
        }

        [Test]
        public void testEditDistance()
        {
            Assert.AreEqual(0, MetinAraclari.editDistance("elma", "elma"),string.Format("{0} bekleniyordu.", 0));
            Assert.AreEqual(1, MetinAraclari.editDistance("elma", "elmax"),string.Format("{0} bekleniyordu.", 1));
            Assert.AreEqual(1, MetinAraclari.editDistance("elma", "lma"),string.Format("{0} bekleniyordu.", 1));
            Assert.AreEqual(2, MetinAraclari.editDistance("elma", "ma"),string.Format("{0} bekleniyordu.", 2));
            Assert.AreEqual(2, MetinAraclari.editDistance("elma", "frma"),string.Format("{0} bekleniyordu.", 2));
            Assert.AreEqual(3, MetinAraclari.editDistance("elma", "a"),string.Format("{0} bekleniyordu.", 3));
            Assert.AreEqual(3, MetinAraclari.editDistance("elma", "elmalar"),string.Format("{0} bekleniyordu.", 3));
            Assert.AreEqual(4, MetinAraclari.editDistance("elma", "elmalara"),string.Format("{0} bekleniyordu.", 4));
            Assert.AreEqual(4, MetinAraclari.editDistance("elma", "amel"),string.Format("{0} bekleniyordu.", 4));
            Assert.AreEqual(5, MetinAraclari.editDistance("elma", "frtyu"),string.Format("{0} bekleniyordu.", 5));
            // ************ TRANSPOZISYON *********************
            Assert.AreEqual(1, MetinAraclari.editDistance("elma", "emla"),string.Format("{0} bekleniyordu.", 1));
            Assert.AreEqual(1, MetinAraclari.editDistance("elma", "elam"),string.Format("{0} bekleniyordu.", 1));
            Assert.AreEqual(1, MetinAraclari.editDistance("elma", "lema"),string.Format("{0} bekleniyordu.", 1));
            Assert.AreEqual(1, MetinAraclari.editDistance("varil", "varli"),string.Format("{0} bekleniyordu.", 1));
            Assert.AreEqual(1, MetinAraclari.editDistance("varil", "vrail"),string.Format("{0} bekleniyordu.", 1));
            Assert.AreEqual(1, MetinAraclari.editDistance("varil", "vairl"),string.Format("{0} bekleniyordu.", 1));
            Assert.AreEqual(1, MetinAraclari.editDistance("varil", "avril"),string.Format("{0} bekleniyordu.", 1));
        }

        [Test]
        public void testInModifiedLevenshteinDistance()
        {
            Assert.IsTrue(MetinAraclari.inEditDistance("elma", "elma", 1));
            Assert.IsTrue(MetinAraclari.inEditDistance("elma", "ekma", 1));
            Assert.IsTrue(MetinAraclari.inEditDistance("elma", "ema", 1));
            Assert.IsTrue(MetinAraclari.inEditDistance("elma", "elmas", 1));
            Assert.IsTrue(MetinAraclari.inEditDistance("elma", "lma", 1));
            Assert.IsTrue(MetinAraclari.inEditDistance("elma", "emas", 2));
            Assert.IsTrue(MetinAraclari.inEditDistance("elma", "el", 2));
            Assert.IsFalse(MetinAraclari.inEditDistance("elma", "el", 1));
            Assert.IsFalse(MetinAraclari.inEditDistance("elma", "eksa", 1));
            Assert.IsFalse(MetinAraclari.inEditDistance("armutu", "armutlr", 1));
            Assert.IsTrue(MetinAraclari.inEditDistance("armutlar", "armutlr", 1));
        }

        [Test]
        public void testIsInSubStringEditDistance()
        {
            Assert.IsTrue(MetinAraclari.isInSubstringEditDistance("elma", "elma", 1));
            Assert.IsTrue(MetinAraclari.isInSubstringEditDistance("elma", "elmalar", 1));
            Assert.IsTrue(MetinAraclari.isInSubstringEditDistance("elma", "ekmalar", 1));
            Assert.IsTrue(MetinAraclari.isInSubstringEditDistance("elma", "emaciklar", 1));
            Assert.IsTrue(MetinAraclari.isInSubstringEditDistance("sefil", "sfil", 1));
        }

        [Test]
        public void testJaroWinklerBenzerlik()
        {
            Assert.IsTrue(MetinAraclari.sozcukBenzerlikOrani("elma", "elm") > 0.9d);
            Assert.IsTrue(MetinAraclari.sozcukBenzerlikOrani("elma", "elam") > 0.9d);
            Assert.IsTrue(MetinAraclari.sozcukBenzerlikOrani("elma", "elfa") > 0.85d);
            Assert.IsTrue(MetinAraclari.sozcukBenzerlikOrani("elma", "elmar") > 0.9d);
        }
    }
}
