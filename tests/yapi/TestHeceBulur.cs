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
