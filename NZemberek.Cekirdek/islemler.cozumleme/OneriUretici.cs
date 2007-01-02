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
    /// <summary>
    /// Hatali kelimeler icin kullanilan kelime uretme mekanizmasini icerir.
    /// Su anda kokte bir ekte bir mesafeye kadar olmak uzere Levenshtein duzeltme mesafesine 
    /// uyan tum onerileri deasciifier'den donus degeri olarak gelen onerileri ve kelimenin 
    /// ayrik iki kelimeden olusmasi durumu icin onerileri bulabilmekte 
    /// </summary>
    public class OneriUretici
    {
        private KelimeCozumleyici cozumleyici;
        private KelimeCozumleyici asciiToleransliCozumleyici;
        private ToleransliCozumleyici toleransliCozumleyici;
        private CozumlemeYardimcisi yardimci;
        private ZemberekAyarlari ayarlar;

        public OneriUretici(CozumlemeYardimcisi yardimci, KelimeCozumleyici cozumleyici,
                            KelimeCozumleyici asciiToleransliCozumleyici, ToleransliCozumleyici toleransliCozumleyici,
                            ZemberekAyarlari ayarlar)
        {
            this.yardimci = yardimci;
            this.toleransliCozumleyici = toleransliCozumleyici;
            this.cozumleyici = cozumleyici;
            this.asciiToleransliCozumleyici = asciiToleransliCozumleyici;
            this.ayarlar = ayarlar;
        }

        /// <summary>
        /// Verilen kelime icin oneri uretir. Yapilan oneriler simdilik sunlardir.
        /// -Kokte ekte mesafeye kadar olmak uzere Levenshtein duzeltme mesafesine uyan tum oneriler 
        /// -Deasciifier'den donus degeri olarak gelen oneriler 
        /// -Kelimenin ayrik iki kelimeden olusmasi durumu icin oneriler 
        /// </summary>
        /// <param name="kelime">oneri uretilecek kelime</param>
        /// <returns>String[] oneriler. Oneri yoksa sifir uzunluklu dizi.</returns>
        public String[] oner(String kelime)
        {
            Kelime[] oneriler = toleransliCozumleyici.cozumle(kelime);        // Once hatali kelime icin tek kelimelik onerileri bulmaya calisalim
            Kelime[] asciiTurkceOneriler = asciiDonusumuOnerileriBul(kelime); // Deasciifierden gelebilecek onerilere bakalim
            Set<String> ayriYazimOnerileri = ayriYazimOnerileriniBul(kelime); // Kelime yanlislikla bitisik yazilmis iki kelimeden mi olusmus?
            
            if (oneriler.Length == 0 && ayriYazimOnerileri.Count == 0 && asciiTurkceOneriler.Length == 0)
            { return new String[0]; /*Oneri olusmadiysa donelim*/ }

            List<String> sonucListesi = onerileriToparla(oneriler, asciiTurkceOneriler); // Onerileri puanlandirmak icin bir listeye koyalim
            List<String> rafineListe = onerileriRafineEt(sonucListesi); // Cift sonuclari liste sirasini bozmadan iptal edelim
            this.ayriYazimOnerileriniEkle(ayriYazimOnerileri, rafineListe); // Son olarak yer kalmissa ayri yazilim onerilerini ekle 

            return rafineListe.ToArray();
        }

        private void ayriYazimOnerileriniEkle(Set<String> ayriYazimOnerileri, List<String> rafineListe)
        {
            foreach (String oneri in ayriYazimOnerileri)
            {
                if (rafineListe.Count < ayarlar.getOneriMax())
                    rafineListe.Add(oneri);
                else
                    break;
            }
        }

        private List<String> onerileriRafineEt(List<String> girisListesi)
        {
            Dictionary<string, int> rafineListe = new Dictionary<string, int>();
            List<string> sonListe = new List<string>();
            foreach (String str in girisListesi)
            {
                if (!rafineListe.ContainsKey(str) && sonListe.Count < ayarlar.getOneriMax())
                {
                    rafineListe.Add(str, 0);
                    sonListe.Add(str);
                }
            }
            return sonListe;
        }

        private static List<String> onerileriToparla(Kelime[] oneriler, Kelime[] asciiTurkceOneriler)
        {
            List<Kelime> oneriList = new List<Kelime>();
            oneriList.AddRange(oneriler);
            oneriList.AddRange(asciiTurkceOneriler);

            oneriList.Sort(new KelimeKokFrekansKiyaslayici()); // Frekansa gore siralayalim

            List<String> sonucListesi = new List<String>(); // Donus icin yeni bir liste olusturalim
            foreach (Kelime anOneriList in oneriList)
            {
                sonucListesi.Add(anOneriList.icerik().ToString());
            }
            return sonucListesi;
        }

        private Kelime[] asciiDonusumuOnerileriBul(String kelime)
        {
            Kelime[] asciiTurkceOneriler = new Kelime[0];
            if (ayarlar.oneriDeasciifierKullan())
            {
                asciiTurkceOneriler = asciiToleransliCozumleyici.cozumle(kelime);
            }
            return asciiTurkceOneriler;
        }

        private Set<String> ayriYazimOnerileriniBul(String kelime)
        {
            Set<String> ayriYazimOnerileri = Collections.EMPTY_SET_STRING;
            if (ayarlar.oneriBilesikKelimeKullan())
            {
                for (int i = 1; i < kelime.Length; i++)
                {
                    String s1 = kelime.Substring(0, i);
                    String s2 = kelime.Substring(i, kelime.Length - i);
                    if (cozumleyici.denetle(s1) && cozumleyici.denetle(s2))
                    {
                        if (ayriYazimOnerileri.Count == 0)
                        {  ayriYazimOnerileri = new HashedSet<String>(); }
                        ayriYazimOnerileri.AddAll(this.ayriYazimlariOlustur(s1, s2));
                    }
                }
            }
            return ayriYazimOnerileri;
        }

        private Set<String> ayriYazimlariOlustur(String s1, String s2)
        {
            Set<String> ayriYazimOnerileri = new HashedSet<String>();
            
            Set<String> set1 = parcayiCozumle(s1);
            Set<String> set2 = parcayiCozumle(s2);
            
            foreach (String str1 in set1)
            {
                foreach (String str2 in set2)
                {
                    ayriYazimOnerileri.Add(str1 + " " + str2);
                }
            }
            return ayriYazimOnerileri;
        }

        private Set<String> parcayiCozumle(String s)
        {
            Set<String> set = new HashedSet<String>();
            Kelime[] kelimeler = cozumleyici.cozumle(s);
            foreach (Kelime kelime in kelimeler)
            {
                yardimci.kelimeBicimlendir(kelime);
                set.Add(kelime.icerik().ToString());
            }
            return set;
        }
    }
}
