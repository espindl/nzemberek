/*
 * Created on 27.May.2005
 * MDA
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using net.zemberek.bilgi;
using net.zemberek.islemler;
using net.zemberek.yapi;
using Iesi.Collections;
using Iesi.Collections.Generic;
using net.zemberek.javaporttemp;


namespace net.zemberek.islemler.cozumleme
{
    public class OneriUretici
    {
        private KelimeCozumleyici cozumleyici, asciiToleransliCozumleyici;
        private ToleransliCozumleyici toleransliCozumleyici;
        private CozumlemeYardimcisi yardimci;
        private ZemberekAyarlari ayarlar;


        public OneriUretici(CozumlemeYardimcisi yardimci,
                            KelimeCozumleyici cozumleyici,
                            KelimeCozumleyici asciiToleransliCozumleyici,
                            ToleransliCozumleyici toleransliCozumleyici,
                            ZemberekAyarlari ayarlar)
        {
            this.yardimci = yardimci;
            this.toleransliCozumleyici = toleransliCozumleyici;
            this.cozumleyici = cozumleyici;
            this.asciiToleransliCozumleyici = asciiToleransliCozumleyici;
            this.ayarlar = ayarlar;
        }

        /**
         * Verilen kelime için öneri üretir.
         * Yapýlan öneriler þu þekildedir:
         * - Kökte 1, ekte 1 mesafeye kadar olmak üzere Levenshtein düzeltme mesafesine uyan tüm öneriler
         * - Deasciifier'den dönüþ deðeri olarak gelen öneriler
         * - Kelimenin ayrýk iki kelimeden oluþmasý durumu için öneriler
         *
         * @param kelime : Öneri yapýlmasý istenen giriþ kelimesi
         * @return String[] olarak öneriler
         *         Eðer öneri yoksa sifir uzunluklu dizi.
         */
        public String[] oner(String kelime) {
        // Once hatalý kelime için tek kelimelik önerileri bulmaya çalýþ
        Kelime[] oneriler = toleransliCozumleyici.cozumle(kelime);
        
        //Deasciifierden bir þey var mý?
        Kelime[] asciiTurkceOneriler = new Kelime[0];
        if (ayarlar.oneriDeasciifierKullan())
            asciiTurkceOneriler = asciiToleransliCozumleyici.cozumle(kelime);

        Set<String> ayriYazimOnerileri = Collections.EMPTY_SET_STRING;

        // Kelime yanlislikla bitisik yazilmis iki kelimeden mi olusmus?
        if (ayarlar.oneriBilesikKelimeKullan()) {
            for (int i = 1; i < kelime.Length; i++) {
                String s1 = kelime.Substring(0, i);
                String s2 = kelime.Substring(i, kelime.Length-i);
                if (cozumleyici.denetle(s1) && cozumleyici.denetle(s2)) {

                    Set<String> set1 = new HashedSet<String>();
                    Kelime[] kelimeler1 = cozumleyici.cozumle(s1);
                    foreach (Kelime kelime1 in kelimeler1) {
                        yardimci.kelimeBicimlendir(kelime1);
                        set1.Add(kelime1.icerik().ToString());
                    }

                    Set<String> set2 = new HashedSet<String>();
                    Kelime[] kelimeler2 = cozumleyici.cozumle(s2);
                    foreach (Kelime kelime1 in kelimeler2) {
                        yardimci.kelimeBicimlendir(kelime1);
                        set2.Add(kelime1.icerik().ToString());
                    }

                    if (ayriYazimOnerileri.Count == 0) {
                        ayriYazimOnerileri = new HashedSet<String>();
                    }

                    foreach (String str1 in set1) {
                        foreach (String str2 in set2) {
                            ayriYazimOnerileri.Add(str1 + " " + str2);
                        }
                    }
                }
            }
        }

        // erken donus..
        if (oneriler.Length == 0 && ayriYazimOnerileri.Count == 0 && asciiTurkceOneriler.Length == 0) {
            return new String[0];
        }

        // Onerileri puanlandýrmak için bir listeye koy
        List<Kelime> oneriList = new List<Kelime>();
        oneriList.AddRange(new List<Kelime>(oneriler));
        oneriList.AddRange(new List<Kelime>(asciiTurkceOneriler));

        // Frekansa göre sýrala
        oneriList.Sort(new KelimeKokFrekansKiyaslayici());

        // Dönüþ listesi string olacak, Yeni bir liste oluþtur. 
        List<String> sonucListesi = new List<String>();
        foreach (Kelime anOneriList in oneriList) {
            sonucListesi.Add(anOneriList.icerik().ToString());
        }

        //Çift sonuçlarý liste sirasýný bozmadan iptal et.
        List<String> rafineListe = new List<String>();
        foreach (String aday in sonucListesi) {
            bool aynisiVar = false;
            foreach (String aRafineListe in rafineListe) {
                if (aday.Equals(aRafineListe)) {
                    aynisiVar = true;
                    break;
                }
            }
            if (!aynisiVar && rafineListe.Count < ayarlar.getOneriMax()) {
                rafineListe.Add(aday);
            }
        }
        	
        // Son olarak yer kalmýþsa ayrý yazýlým önerilerini ekle
        foreach (String oneri in ayriYazimOnerileri) {
            if (rafineListe.Count < ayarlar.getOneriMax())
                rafineListe.Add(oneri);
            else
                break;
        }

        return rafineListe.ToArray();
    }
    }
}
