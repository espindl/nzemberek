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
using net.zemberek.bilgi;

namespace net.zemberek.tests.yapi
{
    [TestFixture]
    public class TestKaynakYukleyici
    {
        [Test]
        public void testKodlamaliKaynakYuklyici() {
            IDictionary<String, String> harfler = new KaynakYukleyici().kodlamaliOzellikDosyasiOku(@"kaynaklar\tr\test\test_harf_tr.txt");
            String test = "e,i,ö,ü";
            Assert.AreEqual(test, harfler["ince-sesli"]);
        }

            

        //[Test]
        //public void testPropertiesURI() {
        //    // herhangi bir adresten (ya da dizinden) dosyayi yuklemeye calisir.
        //    URI uri = new File("test/net/zemberek/bilgi/test.properties").toURI();
        //    Properties props = new KaynakYukleyici().konfigurasyonYukle(uri);
        //    String test = props.getProperty("test");
        //    assertEquals(test, "test 1 2 3");
        //}

        //[Test]
        //public void testPropertiesClasspath() {
        //    // verilen dosyayi classpath icinden yuklemeye calisir.
        //    Properties props = new KaynakYukleyici().konfigurasyonYukle("net//zemberek//bilgi//test.properties");
        //    String test = props.getProperty("test");
        //    assertEquals(test, "test 1 2 3");
        //}
    }
}
