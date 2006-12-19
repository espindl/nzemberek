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
using net.zemberek.yapi.ek;
using net.zemberek.islemler.cozumleme;

namespace net.zemberek.tests.yapi.ek
{
    /**
     * User: aakin
     * Date: Feb 24, 2004
     */
    public class BaseTestEkler : TemelTest {

        protected Kelime[] kelimeler;



        protected void olusanEkKontrol(String[] strs, String[] gercek, Ek ek) {
            String[] olusanEkler;
            kelimeleriOlustur(strs);
            olusanEkler = ekleriOlustur(ek);
            for (int i = 0; i < gercek.Length; i++) {
                Assert.AreEqual(olusanEkler[i], gercek[i], "Hatali olusum:" + olusanEkler[i]);
            }
        }

        protected void kelimeleriOlustur(String[] strs) {
            kelimeler = new Kelime[strs.Length];
            for (int i = 0; i < strs.Length; i++) {
                kelimeler[i] = new Kelime(new Kok(strs[i], KelimeTipi.FIIL), alfabe);
            }
        }

        protected String[] ekleriOlustur(Ek ek) {
            String[] olusan = new String[kelimeler.Length];
            for (int i = 0; i < kelimeler.Length; i++) {
                olusan[i] = ek.cozumlemeIcinUret(kelimeler[i], kelimeler[i].icerik(),
                        new KesinHDKiyaslayici()).ToString();
            }
            return olusan;
        }

        public void testEmpty() {
            Assert.IsTrue(true);
        }
    }
}
