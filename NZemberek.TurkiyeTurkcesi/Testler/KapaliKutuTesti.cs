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
using net.zemberek.erisim;
using NUnit.Framework;


namespace NZemberek.TurkiyeTurkcesi.Testler
{
    [TestFixture]
    public class KapaliKutuTesti
    {
        private static Zemberek zemberek;

        [SetUp]
        public void baslangic()
        {
            zemberek = new Zemberek();
        }

        [Test]
        public void testCozumle1()
        {
            string str = "kedi";
            Assert.IsTrue(zemberek.kelimeDenetle(str));
            string[] sonuc = zemberek.kelimeCozumle(str);
            Assert.AreEqual(1, sonuc.Length);
            Assert.AreEqual("{Icerik: kedi Kok: kedi tip:ISIM}  Ekler:ISIM_KOK", sonuc[0]);
        }
    }
}
