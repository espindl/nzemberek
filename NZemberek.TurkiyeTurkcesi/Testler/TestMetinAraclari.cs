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
            int actual = MetinAraclari.DuzeltmeMesafesi(s1, s2);
            Assert.AreEqual(expected, actual, string.Format("{0} beklenirken {1} geldi.", expected, actual));
        }

        [Test]
        public void testEditDistance()
        {
            Assert.AreEqual(0, MetinAraclari.DuzeltmeMesafesi("elma", "elma"),string.Format("{0} bekleniyordu.", 0));
            Assert.AreEqual(1, MetinAraclari.DuzeltmeMesafesi("elma", "elmax"),string.Format("{0} bekleniyordu.", 1));
            Assert.AreEqual(1, MetinAraclari.DuzeltmeMesafesi("elma", "lma"),string.Format("{0} bekleniyordu.", 1));
            Assert.AreEqual(2, MetinAraclari.DuzeltmeMesafesi("elma", "ma"),string.Format("{0} bekleniyordu.", 2));
            Assert.AreEqual(2, MetinAraclari.DuzeltmeMesafesi("elma", "frma"),string.Format("{0} bekleniyordu.", 2));
            Assert.AreEqual(3, MetinAraclari.DuzeltmeMesafesi("elma", "a"),string.Format("{0} bekleniyordu.", 3));
            Assert.AreEqual(3, MetinAraclari.DuzeltmeMesafesi("elma", "elmalar"),string.Format("{0} bekleniyordu.", 3));
            Assert.AreEqual(4, MetinAraclari.DuzeltmeMesafesi("elma", "elmalara"),string.Format("{0} bekleniyordu.", 4));
            Assert.AreEqual(4, MetinAraclari.DuzeltmeMesafesi("elma", "amel"),string.Format("{0} bekleniyordu.", 4));
            Assert.AreEqual(5, MetinAraclari.DuzeltmeMesafesi("elma", "frtyu"),string.Format("{0} bekleniyordu.", 5));
            // ************ TRANSPOZISYON *********************
            Assert.AreEqual(1, MetinAraclari.DuzeltmeMesafesi("elma", "emla"),string.Format("{0} bekleniyordu.", 1));
            Assert.AreEqual(1, MetinAraclari.DuzeltmeMesafesi("elma", "elam"),string.Format("{0} bekleniyordu.", 1));
            Assert.AreEqual(1, MetinAraclari.DuzeltmeMesafesi("elma", "lema"),string.Format("{0} bekleniyordu.", 1));
            Assert.AreEqual(1, MetinAraclari.DuzeltmeMesafesi("varil", "varli"),string.Format("{0} bekleniyordu.", 1));
            Assert.AreEqual(1, MetinAraclari.DuzeltmeMesafesi("varil", "vrail"),string.Format("{0} bekleniyordu.", 1));
            Assert.AreEqual(1, MetinAraclari.DuzeltmeMesafesi("varil", "vairl"),string.Format("{0} bekleniyordu.", 1));
            Assert.AreEqual(1, MetinAraclari.DuzeltmeMesafesi("varil", "avril"),string.Format("{0} bekleniyordu.", 1));
        }

        [Test]
        public void testInModifiedLevenshteinDistance()
        {
            Assert.IsTrue(MetinAraclari.DuzeltmeMesafesiIcinde("elma", "elma", 1));
            Assert.IsTrue(MetinAraclari.DuzeltmeMesafesiIcinde("elma", "ekma", 1));
            Assert.IsTrue(MetinAraclari.DuzeltmeMesafesiIcinde("elma", "ema", 1));
            Assert.IsTrue(MetinAraclari.DuzeltmeMesafesiIcinde("elma", "elmas", 1));
            Assert.IsTrue(MetinAraclari.DuzeltmeMesafesiIcinde("elma", "lma", 1));
            Assert.IsTrue(MetinAraclari.DuzeltmeMesafesiIcinde("elma", "emas", 2));
            Assert.IsTrue(MetinAraclari.DuzeltmeMesafesiIcinde("elma", "el", 2));
            Assert.IsFalse(MetinAraclari.DuzeltmeMesafesiIcinde("elma", "el", 1));
            Assert.IsFalse(MetinAraclari.DuzeltmeMesafesiIcinde("elma", "eksa", 1));
            Assert.IsFalse(MetinAraclari.DuzeltmeMesafesiIcinde("armutu", "armutlr", 1));
            Assert.IsTrue(MetinAraclari.DuzeltmeMesafesiIcinde("armutlar", "armutlr", 1));
        }

        [Test]
        public void testIsInSubStringEditDistance()
        {
            Assert.IsTrue(MetinAraclari.ParcasiDuzeltmeMesafesiIcinde("elma", "elma", 1));
            Assert.IsTrue(MetinAraclari.ParcasiDuzeltmeMesafesiIcinde("elma", "elmalar", 1));
            Assert.IsTrue(MetinAraclari.ParcasiDuzeltmeMesafesiIcinde("elma", "ekmalar", 1));
            Assert.IsTrue(MetinAraclari.ParcasiDuzeltmeMesafesiIcinde("elma", "emaciklar", 1));
            Assert.IsTrue(MetinAraclari.ParcasiDuzeltmeMesafesiIcinde("sefil", "sfil", 1));
        }
    }
}
