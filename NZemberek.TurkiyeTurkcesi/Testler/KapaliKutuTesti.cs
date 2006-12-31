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
        public void testCozumle_Kedi()
        {
            string str = "kedi";
            Assert.IsTrue(zemberek.kelimeDenetle(str));
            string[] sonuc = zemberek.kelimeCozumle(str);
            Assert.AreEqual(1, sonuc.Length);
            Assert.AreEqual("{Icerik: kedi Kok: kedi tip:ISIM}  Ekler:ISIM_KOK", sonuc[0]);
        }

        [Test]
        public void testCozumle_Kediciklerin()
        {
            string str = "kediciklerin";
            Assert.IsTrue(zemberek.kelimeDenetle(str));
            string[] sonuc = zemberek.kelimeCozumle(str);
            Assert.AreEqual(2, sonuc.Length);
            Assert.AreEqual("{Icerik: kediciklerin Kok: kedi tip:ISIM} "+
                            " Ekler:ISIM_KOK + ISIM_KUCULTME_CIK + ISIM_COGUL_LER + ISIM_TAMLAMA_IN", sonuc[0]);
            Assert.AreEqual("{Icerik: kediciklerin Kok: kedi tip:ISIM} "+
                            " Ekler:ISIM_KOK + ISIM_KUCULTME_CIK + ISIM_COGUL_LER + ISIM_SAHIPLIK_SEN_IN", sonuc[1]);
        }

        [Test]
        public void testCozumle_Getirttirebilirsiniz()
        {
            string str = "getirttirebilirsiniz";
            Assert.IsTrue(zemberek.kelimeDenetle(str));
            string[] sonuc = zemberek.kelimeCozumle(str);
            Assert.AreEqual(1, sonuc.Length);
            Assert.AreEqual("{Icerik: getirttirebilirsiniz Kok: getir tip:FIIL} "+
                            " Ekler:FIIL_KOK + FIIL_OLDURGAN_T + FIIL_ETTIRGEN_TIR +"+
                            " FIIL_YETENEK_EBIL + FIIL_GENISZAMAN_IR + FIIL_KISI_SIZ", sonuc[0]);
        }

        [Test]
        public void testCozumle_Suyuyla()
        {
            string str = "suyuyla";
            Assert.IsTrue(zemberek.kelimeDenetle(str));
            string[] sonuc = zemberek.kelimeCozumle(str);
            Assert.AreEqual(2, sonuc.Length);
            Assert.AreEqual("{Icerik: suyuyla Kok: su tip:ISIM} "+
                            " Ekler:ISIM_KOK + ISIM_TAMLAMA_I + ISIM_BIRLIKTELIK_LE", sonuc[0]);
            Assert.AreEqual("{Icerik: suyuyla Kok: su tip:ISIM} "+
                            " Ekler:ISIM_KOK + ISIM_SAHIPLIK_O_I + ISIM_BIRLIKTELIK_LE", sonuc[1]);
        }

        [Test]
        public void testCozumle_Sembolü()
        {
            string str = "sembolü";
            Assert.IsTrue(zemberek.kelimeDenetle(str));
            string[] sonuc = zemberek.kelimeCozumle(str);
            Assert.AreEqual(3, sonuc.Length);
            Assert.AreEqual("{Icerik: sembolü Kok: sembol tip:ISIM} " +
                            " Ekler:ISIM_KOK + ISIM_TAMLAMA_I", sonuc[0]);
            Assert.AreEqual("{Icerik: sembolü Kok: sembol tip:ISIM} " +
                            " Ekler:ISIM_KOK + ISIM_BELIRTME_I", sonuc[1]);
            Assert.AreEqual("{Icerik: sembolü Kok: sembol tip:ISIM} " +
                            " Ekler:ISIM_KOK + ISIM_SAHIPLIK_O_I", sonuc[2]);
        }

        [Test]
        public void testAsciiYap_Sebek()
        {
            string actual = zemberek.asciiyeDonustur("þebek");
            string expected = "sebek";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void testAsciiYap_Sasirtmis()
        {
            string actual = zemberek.asciiyeDonustur("þaþýrtmýþ");
            string expected = "sasirtmis";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void testAsciiYap_Dugumsuzlukmus()
        {
            string actual = zemberek.asciiyeDonustur("düðümsüzlükmüþ");
            string expected = "dugumsuzlukmus";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void testAsciiCozumle_Dugumsuzlukmus()
        {
            string[] actual = zemberek.asciidenTurkceye("dugumsuzlukmus");
            string expected = "düðümsüzlükmüþ";
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(expected, actual[0]);
        }

        [Test]
        public void testAsciiCozumle_Sasirtmis()
        {
            string[] actual = zemberek.asciidenTurkceye("sasirtmis");
            string expected = "þaþýrtmýþ";
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(expected, actual[0]);
        }

        [Test]
        public void testAsciiCozumle_Sebek()
        {
            string[] actual = zemberek.asciidenTurkceye("sebek");
            string expected = "þebek";
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(expected, actual[0]);
        }

        [Test]
        public void testBelirsizAsciiCozumle_Siraci()
        {
            string[] actual = zemberek.asciidenTurkceye("siraci");
            string expected1 = "sýracý";
            string expected2 = "þýracý";
            Assert.AreEqual(2, actual.Length);
            Assert.Contains(expected1, actual);
            Assert.Contains(expected2, actual);
        }

        [Test]
        public void testBelirsizAsciiCozumle_Olmus()
        {
            string[] actual = zemberek.asciidenTurkceye("olmus");
            string expected1 = "olmuþ";
            string expected2 = "ölmüþ";
            Assert.AreEqual(2, actual.Length);
            Assert.Contains(expected1, actual);
            Assert.Contains(expected2, actual);
        }
        
    }
}
