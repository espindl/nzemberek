using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;
using NUnit.Framework;


namespace net.zemberek.tests.yapi
{
    [TestFixture]
    public class TestHarfDizisi
    {
        //TODO :  testler alfabe yüklemeyi tamamlayýnca çalýþacak.
        Alfabe alfabe = new Alfabe(@"..\..\tests\yapi\hartf_tr.txt", "tr");
        String str;
        HarfDizisi dizi1;

        public HarfDizisi HarfDizisiYap(String s)
        {
            return new HarfDizisi(s, alfabe);
        }

        [SetUp]
        public void setUp()
        {
            //base.setUp();
            str = "kalem";
            dizi1 = new HarfDizisi(str, alfabe);
        }

        [Test]
        public void testSonSesli()
        {
            Assert.AreEqual(dizi1.sonSesli(), alfabe.harf('e'), "Yanlis sonsesli " + dizi1.sonSesli().charDeger());
        }

        [Test]
        public void testSonHarf()
        {
            Assert.AreEqual(dizi1.sonHarf(), alfabe.harf('m'), "harf ekleme problemi");
        }

        [Test]
        public void testHarfEkle()
        {
            dizi1.ekle(alfabe.harf('s'));
            dizi1.ekle(alfabe.harf('i'));
            Assert.AreEqual(alfabe.harf('i'), dizi1.sonHarf(), "harf ekleme problemi");
        }

        [Test]
        public void testDiziEkle()
        {
            HarfDizisi dizi2 = new HarfDizisi("ler", alfabe);
            dizi1.ekle(dizi2);
            Assert.AreEqual(dizi1. ToString(), "kalemler", "dizi1 ekleme problemi " + dizi1.ToString());
        }

        [Test]
        public void testAradanEkle()
        {
            HarfDizisi dizi = HarfDizisiYap("kale");
            HarfDizisi ek = HarfDizisiYap("ku");
            dizi.ekle(2, ek);
            Assert.AreEqual(dizi.ToString(), "kakule");
            ek = HarfDizisiYap("kara");
            dizi.ekle(0, ek);
            Assert.AreEqual(dizi.ToString(), "karakakule");
            try
            {
                dizi.ekle(20, ek);
                TestCase.Fail("Exception olmai gerekirdi");
                dizi.ekle(-1, ek);
            }
            catch (IndexOutOfRangeException expected)
            {
                Assert.IsTrue(true);
            }
        }

        [Test]
        public void testAradanSil()
        {
            HarfDizisi dizi = HarfDizisiYap("abcdefg");
            Assert.AreEqual(dizi.harfSil(2, 3).ToString(), "abfg");
            dizi = HarfDizisiYap("abcdefg");
            Assert.AreEqual(dizi.harfSil(4, 7).ToString(), "abcd");
            dizi = HarfDizisiYap("abcdefg");
            Assert.AreEqual(dizi.harfSil(0, 4).ToString(), "efg");
            dizi = HarfDizisiYap("abcdefg");
            Assert.AreEqual(dizi.harfSil(1, 4).ToString(), "afg");

            try
            {
                dizi.harfSil(20, 1);
                dizi.harfSil(-1, 2);
                TestCase.Fail("Exception olmasi gerekirdi");
            }
            catch (IndexOutOfRangeException expected)
            {
                Assert.IsTrue(true);
            }
        }

        [Test]
        public void testAradanKarsilastir()
        {
            HarfDizisi kelime = new HarfDizisi("kedicikler", alfabe);
            HarfDizisi o1 = new HarfDizisi("cik", alfabe);
            HarfDizisi o2 = new HarfDizisi("er", alfabe);
            HarfDizisi o3 = new HarfDizisi("ere", alfabe);
            Assert.IsTrue(kelime.aradanKiyasla(4, o1));
            Assert.IsTrue(kelime.aradanKiyasla(5, o1) == false);
            Assert.IsTrue(kelime.aradanKiyasla(11, o1) == false);
            Assert.IsTrue(kelime.aradanKiyasla(8, o2));
            Assert.IsTrue(kelime.aradanKiyasla(8, o3) == false);
        }

        [Test]
        public void testHarfDegistir()
        {
            HarfDizisi kelime = new HarfDizisi("kedicikler", alfabe);
            kelime.harfDegistir(0, alfabe.harf('c'));
            kelime.harfDegistir(3, alfabe.harf('a'));
            Assert.AreEqual(kelime.ToString(), "cedacikler");
        }

        [Test]
        public void testHarfSil()
        {
            HarfDizisi kelime = new HarfDizisi("kedicikler", alfabe);
            kelime.harfSil(9);
            Assert.AreEqual(kelime.ToString(), "kedicikle");
            kelime.harfSil(0);
            Assert.AreEqual(kelime.ToString(), "edicikle");
            kelime.harfSil(3);
            Assert.AreEqual(kelime.ToString(), "ediikle");
        }

        [Test]
        public void testIlkSesli()
        {
            HarfDizisi kelime = new HarfDizisi("saatte", alfabe);
            Assert.AreEqual(kelime.ilkSesli(0), alfabe.harf('a'));
            Assert.AreEqual(kelime.ilkSesli(1), alfabe.harf('a'));
            Assert.AreEqual(kelime.ilkSesli(3), alfabe.harf('e'));
            Assert.AreEqual(kelime.ilkSesli(6), Alfabe.TANIMSIZ_HARF);
        }

        [Test]
        public void testSonHarfYumusat()
        {
            HarfDizisi kelime = new HarfDizisi("kitap", alfabe);
            kelime.sonHarfYumusat();
            Assert.AreEqual(kelime.sonHarf(), alfabe.harf('b'));
            kelime = new HarfDizisi("armut", alfabe);
            kelime.sonHarfYumusat();
            Assert.AreEqual(kelime.sonHarf(), alfabe.harf('d'));
            kelime = new HarfDizisi("kulak", alfabe);
            kelime.sonHarfYumusat();
            Assert.AreEqual(kelime.sonHarf(), alfabe.harf(Alfabe.CHAR_gg));
        }

        [Test]
        public void testKirp()
        {
            HarfDizisi dizi = new HarfDizisi("merhaba", alfabe);
            dizi.kirp(5);
            Assert.AreEqual("merha", dizi.ToString());
            dizi.kirp(5);
            Assert.AreEqual("merha", dizi.ToString());
            dizi.kirp(0);
            Assert.IsTrue(dizi.length() == 0);
        }

        [Test]
        public void testtoStringIndex()
        {
            HarfDizisi dizi = new HarfDizisi("merhaba", alfabe);
            Assert.AreEqual(dizi.ToString(4), "aba");
            Assert.AreEqual(dizi.ToString(0), "merhaba");
            Assert.AreEqual(dizi.ToString(7), "");
            Assert.AreEqual(dizi.ToString(-1), "");
            Assert.AreEqual(dizi.ToString(6), "a");
        }

        [Test]
        public void testBastanKarsilastir()
        {
            HarfDizisi dizi = new HarfDizisi("merhaba", alfabe);
            Assert.IsTrue(dizi.bastanKiyasla(new HarfDizisi("m", alfabe)));
            Assert.IsTrue(dizi.bastanKiyasla(new HarfDizisi("merha", alfabe)));
            Assert.IsTrue(dizi.bastanKiyasla(new HarfDizisi("merhaba", alfabe)));
            Assert.IsTrue(dizi.bastanKiyasla(new HarfDizisi("merhabal", alfabe)) == false);
        }

        [Test]
        public void testEquals()
        {
            HarfDizisi dizi = new HarfDizisi("merhaba", alfabe);
            Assert.IsTrue(dizi.Equals(new HarfDizisi("merhaba", alfabe)));
            Assert.IsTrue(dizi.Equals(new HarfDizisi("merha", alfabe)) == false);
            Assert.IsTrue(dizi.Equals(new HarfDizisi("merhabalar", alfabe)) == false);
            HarfDizisi ydizi = new HarfDizisi("merhaba", alfabe, 20);
            Assert.IsTrue(dizi.Equals(new HarfDizisi("merhaba", alfabe)));
            Assert.IsTrue(dizi.Equals(new HarfDizisi("merhaba", alfabe, 15)));
            Assert.IsTrue(dizi.Equals(new HarfDizisi("merhabalar", alfabe, 15)) == false);
        }

        [Test]
        public void testSesliSayisi()
        {
            HarfDizisi dizi = new HarfDizisi("merhaba", alfabe);
            Assert.IsTrue(dizi.sesliSayisi() == 3);
            dizi = new HarfDizisi("aarteetytye", alfabe);
            Assert.IsTrue(dizi.sesliSayisi() == 5);
            dizi = new HarfDizisi("art", alfabe);
            Assert.IsTrue(dizi.sesliSayisi() == 1);
            dizi = new HarfDizisi("rrt", alfabe);
            Assert.IsTrue(dizi.sesliSayisi() == 0);
        }

        /*    
        public void testSesliBelirleIU()
        {
            String[] girisler = {"ev", "av",
                                 "armut", "varil",
                                 "bidon", "but",
                                 "t", "o"};
            TurkceHarf[] harfler = {TurkceAlfabe.HARF_i, TurkceAlfabe.HARF_ii,
                                    TurkceAlfabe.HARF_u, TurkceAlfabe.HARF_i,
                                    TurkceAlfabe.HARF_u, TurkceAlfabe.HARF_u,
                                    TurkceAlfabe.TANIMSIZ_HARF, TurkceAlfabe.HARF_u};
            for (int i = 0; i < girisler.length; i++)
            {
                HarfDizisi dizi = new HarfDizisi(girisler[i]);
                TurkceHarf sonuc = dizi.sesliBelirleIU(false);
                Assert.AreEqual("Yanlis Harf"+sonuc, harfler[i], sonuc);
            }
        }
        */

        [Test]
        public void testTurkceToleransliKiyasla()
        {
            HarfDizisi hd1 = new HarfDizisi("\u00c7\u0131k\u0131s", alfabe);
            HarfDizisi hd2 = new HarfDizisi("Ciki\u015f", alfabe);
            HarfDizisi hdkisa = new HarfDizisi("Ci", alfabe);
            HarfDizisi hdtkisaturkce = new HarfDizisi("\u00c7\u0131", alfabe);
            Assert.IsTrue(hd1.asciiToleransliKiyasla(hd2));
            Assert.IsTrue(hd2.asciiToleransliKiyasla(hd1));
            Assert.IsTrue(hd1.asciiToleransliBastanKiyasla(hdkisa));
            Assert.IsTrue(hd2.asciiToleransliBastanKiyasla(hdkisa));
            Assert.IsTrue(hd2.asciiToleransliBastanKiyasla(hdtkisaturkce));
        }

        [Test]
        public void testHarfEkleHarf()
        {
            HarfDizisi dizi = new HarfDizisi("armut", alfabe);
            dizi.ekle(2, alfabe.harf('n'));
            Assert.AreEqual(dizi.ToString(), "arnmut");
            dizi.ekle(1, alfabe.harf('c'));
            Assert.AreEqual(dizi.ToString(), "acrnmut");
            dizi.ekle(0, alfabe.harf('s'));
            Assert.AreEqual(dizi.ToString(), "sacrnmut");
            dizi.ekle(8, alfabe.harf('a'));
            Assert.AreEqual(dizi.ToString(), "sacrnmuta");
        }

        [Test]
        public void testCharSequenceMethods()
        {
            HarfDizisi dizi = new HarfDizisi("armut", alfabe);
            Assert.AreEqual(dizi.length(), 5);
            Assert.IsNull(dizi.subSequence(3, 1));
            Assert.AreEqual(new HarfDizisi("arm", alfabe), dizi.subSequence(0, 3));
            Assert.AreEqual(new HarfDizisi("rm", alfabe), dizi.subSequence(1, 3));
            Assert.AreEqual(new HarfDizisi("", alfabe), dizi.subSequence(3, 3));
            Assert.AreEqual(dizi.charAt(0), 'a');
            Assert.AreEqual(dizi.charAt(4), 't');
        }

        [Test]
        public void testHepsiBuyukHarfmi() 
        {
            String[] strs = {"AA", "AA" + Alfabe.CHAR_SAPKALI_A, "AWAQ", ""};
            foreach (String s in strs) {
                HarfDizisi d = HarfDizisiYap(s);
                Assert.IsTrue(d.hepsiBuyukHarfmi(), "olmadi" + s);
            }
            String[] ss = {"aaa", "Aa", "AA"+Alfabe.CHAR_SAPKALI_a, "AwAQ", "..A", "-"};
            foreach (String s in ss)
            {
                HarfDizisi d = HarfDizisiYap(s);
                Assert.IsFalse(d.hepsiBuyukHarfmi(), "olmadi" + s);
            }
        }
    }
}
