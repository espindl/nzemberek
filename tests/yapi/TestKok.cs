/* ***** BEGIN LICENSE BLOCK *****
 * Version: MPL 1.1
 *
 * The contents of this file are subject to the Mozilla Public License Version
 * 1.1 (the "License"); you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * The Original Code is Zemberek Doğal Dil İşleme Kütüphanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Akın, Mehmet D. Akın.
 * Portions created by the Initial Developer are Copyright (C) 2006
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Mert Derman
 *   Tankut Tekeli
 * ***** END LICENSE BLOCK ***** */

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using net.zemberek.yapi;
using net.zemberek.yapi.kok;
using net.zemberek.tr.yapi.kok;

namespace net.zemberek.tests.yapi
{
    [TestFixture]
public class TestKok : TemelTest {


    KokOzelDurumBilgisi koz;

    [SetUp]
    public override void once() {
        base.once();
        koz = dilBilgisi.kokOzelDurumlari();
    }

    [Test]
    public void testDegismisIcerikOlustur() 
    {
        Kok kok = new Kok("ara", KelimeTipi.FIIL);
        kok.ozelDurumEkle(koz.ozelDurum(TurkceKokOzelDurumTipi.SIMDIKI_ZAMAN));//bu satır java tarafında yok, ama bu olmazsa test cakar
        Assert.IsTrue(koz.ozelDurumUygula(kok).Length > 0);
        Assert.AreEqual((koz.ozelDurumUygula(kok))[0], "ar");
        Assert.IsTrue(kok.ozelDurumIceriyormu(TurkceKokOzelDurumTipi.SIMDIKI_ZAMAN));
        kok = new Kok("kitap", KelimeTipi.ISIM);
        kok.ozelDurumEkle(koz.ozelDurum(TurkceKokOzelDurumTipi.SESSIZ_YUMUSAMASI));
        Assert.IsTrue(koz.ozelDurumUygula(kok).Length > 0);
        Assert.AreEqual((koz.ozelDurumUygula(kok))[0], "kitab");

        String str = "al" + Alfabe.CHAR_ii + "n";
        kok = new Kok(str, KelimeTipi.ISIM);
        kok.ozelDurumEkle(koz.ozelDurum(TurkceKokOzelDurumTipi.ISIM_SESLI_DUSMESI));
        Assert.IsTrue(koz.ozelDurumUygula(kok).Length > 0);
        Assert.AreEqual((koz.ozelDurumUygula(kok))[0], "aln");

        kok = new Kok("nakit", KelimeTipi.ISIM);
        kok.ozelDurumEkle(koz.ozelDurum(TurkceKokOzelDurumTipi.ISIM_SESLI_DUSMESI));
        kok.ozelDurumEkle(koz.ozelDurum(TurkceKokOzelDurumTipi.SESSIZ_YUMUSAMASI));
        Assert.IsTrue(koz.ozelDurumUygula(kok).Length > 0);
        Assert.AreEqual((koz.ozelDurumUygula(kok))[0], "nakd");

        kok = new Kok("ben", KelimeTipi.ZAMIR);
        kok.ozelDurumEkle(koz.ozelDurum(TurkceKokOzelDurumTipi.TEKIL_KISI_BOZULMASI));
        Assert.IsTrue(koz.ozelDurumUygula(kok).Length > 0);
        Assert.AreEqual((koz.ozelDurumUygula(kok))[0], "ban");

        kok = new Kok("sen", KelimeTipi.ZAMIR);
        kok.ozelDurumEkle(koz.ozelDurum(TurkceKokOzelDurumTipi.TEKIL_KISI_BOZULMASI));
        Assert.IsTrue(koz.ozelDurumUygula(kok).Length > 0);
        Assert.AreEqual((koz.ozelDurumUygula(kok))[0], "san");

        kok = new Kok("de", KelimeTipi.FIIL);
        kok.ozelDurumEkle(koz.ozelDurum(TurkceKokOzelDurumTipi.FIIL_KOK_BOZULMASI));
        Assert.IsTrue(koz.ozelDurumUygula(kok).Length > 0);
        Assert.AreEqual((koz.ozelDurumUygula(kok))[0], "di");

        kok = new Kok("ye", KelimeTipi.FIIL);
        kok.ozelDurumEkle(koz.ozelDurum(TurkceKokOzelDurumTipi.FIIL_KOK_BOZULMASI));
        Assert.IsTrue(koz.ozelDurumUygula(kok).Length > 0);
        Assert.AreEqual((koz.ozelDurumUygula(kok))[0], "yi");
    }

    //java tarafından kalan metod, buna gerek yok
    //private void ozelDurumTest(Kok kok, String beklenen) {
    //    String[] results = koz.ozelDurumUygula(kok);
    //    Assert.IsTrue(results.Length > 0);
    //    String sonuc = results[0];
    //    Assert.AreEqual(sonuc, beklenen);
    //}

    [Test]
    public void testEqual() {
        Kok kok1 = new Kok("kitap", KelimeTipi.ISIM);
        Kok kok2 = new Kok("kitap", KelimeTipi.ISIM);
        Kok kok3 = new Kok("kitab", KelimeTipi.ISIM);
        Assert.IsTrue(kok1.Equals(kok2));
        Assert.IsTrue(kok1.Equals(kok3) == false);
    }

    public void testilkEkBelirle() 
    {
        /* 
        Kok kok = new Kok("kedi", KelimeTipi.ISIM);
        assertEquals(TurkceEkYonetici.ref().ilkEkBelirle(), Ekler.ISIM_YALIN);
        kok.setOzelDurumlar(new HashSet());
        kok.ozelDurumlar().add(TurkceKokOzelDurumlari.YALIN);
        assertEquals(kok.ilkEkBelirle(), Ekler.GENEL_YALIN);
        kok = new Kok("almak", KelimeTipi.FIIL);
        assertEquals(kok.ilkEkBelirle(), Ekler.FIIL_YALIN);
        kok = new Kok("on", KelimeTipi.SAYI);
        assertEquals(kok.ilkEkBelirle(), Ekler.SAYI_YALIN);
        */
    }
}

}
