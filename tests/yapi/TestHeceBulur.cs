using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using net.zemberek.yapi;
using net.zemberek.tr.yapi;

namespace net.zemberek.tests.yapi
{
    /**
     * User: ahmet
     * Date: Sep 10, 2005
     */
    [TestFixture]
    public class TestHeceBulur : TemelTest {

        [SetUp]
        public override void once()
        {
            base.once();
        }

        [Test]
        public void testSonHece() {
            HeceBulucu heceBulur = new TurkceHeceBulucu();
            String[] strs = {"turk", "ara", "sarta", "siir", "siiir", "kanat", "kanaat",
                    "yaptirt", "artti", "arttir", "arttirt", "sirret", "siirt", "teleskop"};
            int[] sonuclar = {4, 2, 2, 2, 2, 3, 2,
                    4, 2, 3, 4, 3, 3, 3};
            HarfDizisi[] girisler = new HarfDizisi[strs.Length];
            for (int i = 0; i < strs.Length; i++)
                girisler[i] = hd(strs[i]);
            for (int i = 0; i < girisler.Length; i++) {
                HarfDizisi harfDizisi = girisler[i];
                Assert.AreEqual(heceBulur.sonHeceHarfSayisi(harfDizisi), sonuclar[i],harfDizisi.ToString());
            }
        }
    }
}
