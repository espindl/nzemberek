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
using net.zemberek.yapi;
using NUnit.Framework;

namespace net.zemberek.tests.yapi.alfabe
{
    [TestFixture]
    public class TestAlfabe
    {
    
        private Alfabe alfabe;

        [SetUp]
        public void setUp() {
            alfabe = new Alfabe(@"kaynaklar\tr\test\test_harf_tr.txt", "tr");
        }

        [Test]
        public void testHarfErisim() {
            TurkceHarf harf = new TurkceHarf('a', 1);
            harf.setSesli(true);

            TurkceHarf okunan = alfabe.harf('a');
            Assert.AreEqual(harf.charDeger(), okunan.charDeger());
            Assert.IsTrue(harf.sesliMi());

        }

        [Test]
        public void testAyikla() {
            String kel = "a'ghh-";
            Assert.AreEqual(alfabe.ayikla(kel), "aghh");
        }
        
        [Test]
        public void testTurkceMi() {
            String kel = "wws$$dgsdashj";
            Assert.IsTrue(!alfabe.cozumlemeyeUygunMu(kel));
            kel = "merhaba";
            Assert.IsTrue(alfabe.cozumlemeyeUygunMu(kel));
        }

        [Test]
        public void testLowerUpperCase() 
        {
            TurkceHarf kucukI = alfabe.harf(Alfabe.CHAR_ii);
            TurkceHarf harfI = alfabe.buyukHarf(kucukI);
            Assert.AreEqual(harfI.charDeger(),'I');
            TurkceHarf i = alfabe.harf('i');
            TurkceHarf harfBuyuki = alfabe.buyukHarf(i);
            Assert.AreEqual(harfBuyuki.charDeger(), 'İ');
        }
    }
}
