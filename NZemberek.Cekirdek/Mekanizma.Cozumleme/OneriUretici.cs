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
using NZemberek.Cekirdek.Mekanizma;
using NZemberek.Cekirdek.Yapi;
using NZemberek.Cekirdek.Kolleksiyonlar;

namespace NZemberek.Cekirdek.Mekanizma.Cozumleme
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
        private ICozumlemeYardimcisi yardimci;
        private ZemberekAyarlari ayarlar;

        public OneriUretici(ICozumlemeYardimcisi yardimci, KelimeCozumleyici cozumleyici,
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
        public String[] Oner(String kelime)
        {
            Kelime[] oneriler = toleransliCozumleyici.Cozumle(kelime);        // Once hatali kelime icin tek kelimelik onerileri bulmaya calisalim
            Kelime[] asciiTurkceOneriler = AsciiDonusumuOnerileriBul(kelime); // Deasciifierden gelebilecek onerilere bakalim
            HashSet<String> ayriYazimOnerileri = AyriYazimOnerileriniBul(kelime); // Kelime yanlislikla bitisik yazilmis iki kelimeden mi olusmus?
            
            if (oneriler.Length == 0 && ayriYazimOnerileri.Count == 0 && asciiTurkceOneriler.Length == 0)
            { return new String[0]; /*Oneri olusmadiysa donelim*/ }

            List<String> sonucListesi = OnerileriToparla(oneriler, asciiTurkceOneriler); // Onerileri puanlandirmak icin bir listeye koyalim
            List<String> rafineListe = OnerileriRafineEt(sonucListesi); // Cift sonuclari liste sirasini bozmadan iptal edelim
            this.AyriYazimOnerileriniEkle(ayriYazimOnerileri, rafineListe); // Son olarak yer kalmissa ayri yazilim onerilerini Ekle 

            return rafineListe.ToArray();
        }

        private void AyriYazimOnerileriniEkle(HashSet<String> ayriYazimOnerileri, List<String> rafineListe)
        {
            foreach (String oneri in ayriYazimOnerileri)
            {
                if (rafineListe.Count < ayarlar.getOneriMax())
                    rafineListe.Add(oneri);
                else
                    break;
            }
        }

        private List<String> OnerileriRafineEt(List<String> girisListesi)
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

        private static List<String> OnerileriToparla(Kelime[] oneriler, Kelime[] asciiTurkceOneriler)
        {
            List<Kelime> oneriList = new List<Kelime>();
            oneriList.AddRange(oneriler);
            oneriList.AddRange(asciiTurkceOneriler);

            oneriList.Sort(new KelimeKokFrekansKiyaslayici()); // Frekansa gore siralayalim

            List<String> sonucListesi = new List<String>(); // Donus icin yeni bir liste olusturalim
            foreach (Kelime anOneriList in oneriList)
            {
                sonucListesi.Add(anOneriList.Icerik.ToString());
            }
            return sonucListesi;
        }

        private Kelime[] AsciiDonusumuOnerileriBul(String kelime)
        {
            Kelime[] asciiTurkceOneriler = new Kelime[0];
            if (ayarlar.oneriDeasciifierKullan())
            {
                asciiTurkceOneriler = asciiToleransliCozumleyici.Cozumle(kelime);
            }
            return asciiTurkceOneriler;
        }

        private HashSet<String> AyriYazimOnerileriniBul(String kelime)
        {
            HashSet<String> ayriYazimOnerileri = HashSet<String>.EMPTY_SET_STRING;
            if (ayarlar.oneriBilesikKelimeKullan())
            {
                for (int i = 1; i < kelime.Length; i++)
                {
                    String s1 = kelime.Substring(0, i);
                    String s2 = kelime.Substring(i, kelime.Length - i);
                    if (cozumleyici.Denetle(s1) && cozumleyici.Denetle(s2))
                    {
                        if (ayriYazimOnerileri.Count == 0)
                        {  ayriYazimOnerileri = new HashSet<String>(); }
                        ayriYazimOnerileri.AddAll(this.AyriYazimlariOlustur(s1, s2));
                    }
                }
            }
            return ayriYazimOnerileri;
        }

        private HashSet<String> AyriYazimlariOlustur(String s1, String s2)
        {
            HashSet<String> ayriYazimOnerileri = new HashSet<String>();
            
            HashSet<String> set1 = ParcayiCozumle(s1);
            HashSet<String> set2 = ParcayiCozumle(s2);
            
            foreach (String str1 in set1)
            {
                foreach (String str2 in set2)
                {
                    ayriYazimOnerileri.Add(str1 + " " + str2);
                }
            }
            return ayriYazimOnerileri;
        }

        private HashSet<String> ParcayiCozumle(String s)
        {
            HashSet<String> set = new HashSet<String>();
            Kelime[] kelimeler = cozumleyici.Cozumle(s);
            foreach (Kelime kelime in kelimeler)
            {
                yardimci.KelimeBicimlendir(kelime);
                set.Add(kelime.Icerik.ToString());
            }
            return set;
        }
    }
}
