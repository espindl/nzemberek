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
