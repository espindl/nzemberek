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
 * The Original Code is Zemberek Doðal Dil Ýþleme Kütüphanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Akýn, Mehmet D. Akýn.
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
        public override void once()
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
