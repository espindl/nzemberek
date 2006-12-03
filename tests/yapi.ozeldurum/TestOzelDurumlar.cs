using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using net.zemberek.yapi;
using net.zemberek.yapi.kok;

/*
import net.zemberek.TemelTest;
import net.zemberek.yapi.HarfDizisi;
import net.zemberek.yapi.KelimeTipi;
import net.zemberek.yapi.Kok;
import net.zemberek.yapi.kok.KokOzelDurumBilgisi;
import static org.junit.Assert.Assert.AreEqual;
import static org.junit.Assert.assertTrue;
import org.junit.Before;

import java.io.IOException;
*/

namespace net.zemberek.tests.yapi.ozeldurum
{
    [TestFixture]
    public class TestOzelDurumlar : TemelTest
    {
       KokOzelDurumBilgisi koz ;

        [SetUp]
        public void once()
        {
            base.once();
            koz = dilBilgisi.kokOzelDurumlari();
        }

        [Test]
        public void testYumusama()
        {
            HarfDizisi dizi = hd("kitap");
            dilBilgisi.kokOzelDurumlari().ozelDurum("YUM").uygula(dizi);
            Assert.AreEqual("kitab", dizi.ToString());
        }

        [Test]
        public void testCiftleme()
        {
            HarfDizisi dizi = hd("hat");
            dilBilgisi.kokOzelDurumlari().ozelDurum("CIFT").uygula(dizi);
            Assert.AreEqual("hatt", dizi.ToString());
        }

        [Test]
        public void testAraSesliDusmesi()
        {
            HarfDizisi dizi = hd("burun");
             dilBilgisi.kokOzelDurumlari().ozelDurum("DUS").uygula(dizi);
            Assert.AreEqual("burn", dizi.ToString());
        }

        [Test]
        public void testKokDegisimleri() {
            Kok kok= new Kok("bahset", KelimeTipi.FIIL);
            kok.ozelDurumEkle(koz.ozelDurum("GEN"));
            kok.ozelDurumEkle(koz.ozelDurum("YUM"));
            String[] sonuclar = koz.ozelDurumUygula(kok);
            Assert.AreEqual(sonuclar.Length,1);
            Assert.AreEqual(sonuclar[0], "bahsed");
        }
    }

}
