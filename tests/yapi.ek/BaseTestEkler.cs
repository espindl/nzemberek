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
    [TestFixture]
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
