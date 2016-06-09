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
 * The Original Code is Zemberek Do�al Dil ��leme K�t�phanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Ak�n, Mehmet D. Ak�n.
 * Portions created by the Initial Developer are Copyright (C) 2006
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Mert Derman
 *   Tankut Tekeli
 * ***** END LICENSE BLOCK ***** */

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace NZemberek.TurkiyeTurkcesi.Test
{
    [TestClass]
    public class KapaliKutuTesti
    {
        private static Zemberek zemberek;

        [TestInitialize]
        public void baslangic()
        {
            zemberek = new Zemberek();
        }

        [TestMethod]
        public void testCozumle_Kedi()
        {
            string str = "kedi";
            Assert.IsTrue(zemberek.KelimeDenetle(str));
            string[] sonuc = zemberek.KelimeCozumle(str);
            Assert.AreEqual(1, sonuc.Length);
            Assert.AreEqual("{Icerik: kedi Kok: kedi Tip:ISIM}  Ekler:ISIM_KOK", sonuc[0]);
        }

        [TestMethod]
        public void testCozumle_Kediciklerin()
        {
            string str = "kediciklerin";
            Assert.IsTrue(zemberek.KelimeDenetle(str));
            string[] sonuc = zemberek.KelimeCozumle(str);
            Assert.AreEqual(2, sonuc.Length);
            Assert.AreEqual("{Icerik: kediciklerin Kok: kedi Tip:ISIM} "+
                            " Ekler:ISIM_KOK + ISIM_KUCULTME_CIK + ISIM_COGUL_LER + ISIM_TAMLAMA_IN", sonuc[0]);
            Assert.AreEqual("{Icerik: kediciklerin Kok: kedi Tip:ISIM} "+
                            " Ekler:ISIM_KOK + ISIM_KUCULTME_CIK + ISIM_COGUL_LER + ISIM_SAHIPLIK_SEN_IN", sonuc[1]);
        }

        [TestMethod]
        public void testCozumle_Getirttirebilirsiniz()
        {
            string str = "getirttirebilirsiniz";
            Assert.IsTrue(zemberek.KelimeDenetle(str));
            string[] sonuc = zemberek.KelimeCozumle(str);
            Assert.AreEqual(1, sonuc.Length);
            Assert.AreEqual("{Icerik: getirttirebilirsiniz Kok: getir Tip:FIIL} "+
                            " Ekler:FIIL_KOK + FIIL_OLDURGAN_T + FIIL_ETTIRGEN_TIR +"+
                            " FIIL_YETENEK_EBIL + FIIL_GENISZAMAN_IR + FIIL_KISI_SIZ", sonuc[0]);
        }

        [TestMethod]
        public void testCozumle_Suyuyla()
        {
            string str = "suyuyla";
            Assert.IsTrue(zemberek.KelimeDenetle(str));
            string[] sonuc = zemberek.KelimeCozumle(str);
            Assert.AreEqual(2, sonuc.Length);
            Assert.AreEqual("{Icerik: suyuyla Kok: su Tip:ISIM} "+
                            " Ekler:ISIM_KOK + ISIM_TAMLAMA_I + ISIM_BIRLIKTELIK_LE", sonuc[0]);
            Assert.AreEqual("{Icerik: suyuyla Kok: su Tip:ISIM} "+
                            " Ekler:ISIM_KOK + ISIM_SAHIPLIK_O_I + ISIM_BIRLIKTELIK_LE", sonuc[1]);
        }

        [TestMethod]
        public void testCozumle_Sembol�()
        {
            string str = "sembol�";
            Assert.IsTrue(zemberek.KelimeDenetle(str));
            string[] sonuc = zemberek.KelimeCozumle(str);
            Assert.AreEqual(3, sonuc.Length);
            Assert.AreEqual("{Icerik: sembol� Kok: sembol Tip:ISIM} " +
                            " Ekler:ISIM_KOK + ISIM_TAMLAMA_I", sonuc[0]);
            Assert.AreEqual("{Icerik: sembol� Kok: sembol Tip:ISIM} " +
                            " Ekler:ISIM_KOK + ISIM_BELIRTME_I", sonuc[1]);
            Assert.AreEqual("{Icerik: sembol� Kok: sembol Tip:ISIM} " +
                            " Ekler:ISIM_KOK + ISIM_SAHIPLIK_O_I", sonuc[2]);
        }

        [TestMethod]
        public void testAsciiYap_Sebek()
        {
            string actual = zemberek.AsciiKarakterlereDonustur("�ebek");
            string expected = "sebek";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void testAsciiYap_Sasirtmis()
        {
            string actual = zemberek.AsciiKarakterlereDonustur("�a��rtm��");
            string expected = "sasirtmis";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void testAsciiYap_Dugumsuzlukmus()
        {
            string actual = zemberek.AsciiKarakterlereDonustur("d���ms�zl�km��");
            string expected = "dugumsuzlukmus";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void testAsciiCozumle_Dugumsuzlukmus()
        {
            string[] actual = zemberek.TurkceKarakterlereDonustur("dugumsuzlukmus");
            string expected = "d���ms�zl�km��";
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(expected, actual[0]);
        }

        [TestMethod]
        public void testAsciiCozumle_Sasirtmis()
        {
            string[] actual = zemberek.TurkceKarakterlereDonustur("sasirtmis");
            string expected = "�a��rtm��";
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(expected, actual[0]);
        }

        [TestMethod]
        public void testAsciiCozumle_Sebek()
        {
            string[] actual = zemberek.TurkceKarakterlereDonustur("sebek");
            string expected = "�ebek";
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(expected, actual[0]);
        }

        [TestMethod]
        public void testBelirsizAsciiCozumle_Siraci()
        {
            string[] actual = zemberek.TurkceKarakterlereDonustur("siraci");
            string expected1 = "s�rac�";
            string expected2 = "��rac�";
            Assert.AreEqual(2, actual.Length);
            CollectionAssert.Contains(actual,expected1);
            CollectionAssert.Contains(actual, expected2);
        }

        [TestMethod]
        public void testBelirsizAsciiCozumle_Olmus()
        {
            string[] actual = zemberek.TurkceKarakterlereDonustur("olmus");
            string expected1 = "olmu�";
            string expected2 = "�lm��";
            Assert.AreEqual(2, actual.Length);
            CollectionAssert.Contains(actual, expected1);
            CollectionAssert.Contains(actual, expected2);
        }

        [TestMethod]
        public void testOner_Gidiktler()
        {
            string[] actual = zemberek.Oner("gidiktler");
            string[] expected = new string[] { "gidikteler", "gidikler", "gidikeler", "gidiktiler" };
            Assert.AreEqual(actual.Length, expected.Length);
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[1], actual[1]);
            Assert.AreEqual(expected[2], actual[2]);
            Assert.AreEqual(expected[3], actual[3]);
        }

        [TestMethod]
        public void testOner_Merhaa()
        {
            string[] actual = zemberek.Oner("merhaa");
            string expected = "merhaba";
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(expected, actual[0]);
        }

        [TestMethod]
        public void testOner_Teknolokiler()
        {
            string[] actual = zemberek.Oner("teknolokiler");
            string expected = "teknolojiler";
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(expected, actual[0]);
        }

        [TestMethod]
        public void testOner_OneriYok()
        {
            string[] actual = zemberek.Oner("assscd");
            Assert.AreEqual(0, actual.Length);
        }

        [TestMethod]
        public void testOner_AyriYazim()
        {
            string[] actual = zemberek.Oner("evegittik");
            string expected = "eve gittik";
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(expected, actual[0]);
        }

        [TestMethod]
        public void testHecele_Bisuru()
        {
            String[] strs = {"turk", "ara", "sarta", "siir", "kanat", "kanaat",
                    "yaptirt", "artti", "arttir", "arttirt", "sirret", "siirt", "teleskop","pratik",
                    "krank","str�kt�r","steteskop","turkcan"};
            int[] hecesayilari = { 1, 2, 2, 2, 2, 3, 2, 2, 2, 2, 2, 2, 3, 2, 1, 2, 3, 2};
            String[][] heceler = {new String[]{"turk"},new String[]{"a","ra"},new String[]{"sar","ta"},new String[]{"si","ir"},new String[]{"ka","nat"},
                             new String[]{"ka","na","at"},new String[]{"yap","tirt"},new String[]{"art","ti"},new String[]{"art","tir"},
                             new String[]{"art","tirt"},new String[]{"sir","ret"},new String[]{"si","irt"},new String[]{"te","les","kop"},
                             new String[]{"pra","tik"},new String[]{"krank"},new String[]{"str�k","t�r"},new String[]{"ste","tes","kop"},
                             new String[]{"turk","can"}};
            string[] sonuc;
            for(int i=0;i<strs.Length;i++)
            {
                sonuc = zemberek.Hecele(strs[i]);
                Assert.AreEqual(sonuc.Length, hecesayilari[i]);
                for (int j = 0; j < sonuc.Length; j++)
                {
                    Assert.AreEqual(sonuc[j], heceler[i][j]);
                }
            }
        }

        [TestMethod]
        public void testDenetle_HepsiDogru()
        {
            List<String> dogrular = TestYardimcisi.satirlariOku("hepsi-dogru.txt");
            string result = string.Empty;
            foreach (String s in dogrular)
            {
                if (zemberek.KelimeDenetle(s) == false)
                {
                    result += s + ", ";
                }
            }
            Assert.AreEqual(string.Empty,result,"Denetleme ba�ar�s�z. Yanl�� negatif kelimeler : \n"+result);
        }

        [TestMethod]
        public void testDenetle_HepsiYanlis()
        {
            List<String> yanlislar = TestYardimcisi.satirlariOku("hepsi-yanlis.txt");
            string result = string.Empty;
            foreach (String s in yanlislar)
            {
                if (zemberek.KelimeDenetle(s) == true)
                {
                    result += s + ", ";
                }
            }
            Assert.AreEqual(string.Empty, result, "Denetleme hatali. Yanl�� pozitif kelimeler : \n" + result);
        }
    }
}
