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

using NZemberek.Cekirdek.KokSozlugu;
using NZemberek.Cekirdek.Mekanizma;
using NZemberek.Cekirdek.Mekanizma.Cozumleme;
using NZemberek.Cekirdek.Yapi;
using log4net;
using System.Reflection;


namespace NZemberek
{

    /// <summary>
    /// EN : This is a facade for accessing the high level functions of the Zemberek library.
    /// This class should be created only once per language.
    /// 
    /// TR : Zemberek projesine ust seviye erisim icin kullanilan sinif.
    /// Ýlklendirilmesi köklerin okunmasý ve agac olusumu nedeniyle pahalidir.
    /// Bu nedenle bu sinifin sadece bir defa olusturulmasi onerilir.
    /// </summary>
    public class Zemberek
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IKelimeCozumleyici _cozumleyici;
        private KelimeUretici _kelimeUretici;
        private IKelimeCozumleyici _asciiToleransliCozumleyici;
        private HataliKodlamaTemizleyici _temizleyici;
        private TurkceYaziTesti _turkceTest;
        private OneriUretici _oneriUretici;
        private AsciiDonusturucu _asciiDonusturucu;
        private IHeceleyici _heceleyici;
        private ZemberekAyarlari _ayarlar;
        private IDilFabrikasi _dilFabrikasi;
        private IDenetlemeCebi _denetlemeCebi;

        /**
         * Default constructor.
         * @param dilayarlari
         */
        public Zemberek()
        {
            _ayarlar = new ZemberekAyarlari();
            Assembly dilpaketi = Assembly.Load(_ayarlar.DilAyarlari[0]);
            this._dilFabrikasi = (IDilFabrikasi)dilpaketi.CreateInstance(_ayarlar.DilAyarlari[1]);
            this._dilFabrikasi.CepKullan = _ayarlar.CepKullan;
            Baslat();
        }

        private void Baslat()
        {
            //Sozluk hazirla.
            ISozluk kokler = _dilFabrikasi.SozlukVer();
            //Normal denetleyici-cozumleyici olusumu
            IKokBulucu kokBulucu = kokler.KesinKokBulucuGetir();
            _cozumleyici = new StandartCozumleyici(
                    kokBulucu,
                    new KesinHDKiyaslayici(),
                    _dilFabrikasi.AlfabeVer(),
                    _dilFabrikasi.EkYoneticisiver(),
                    _dilFabrikasi.KokOzelDurumYoneticiVer(),
                    _dilFabrikasi.CozumlemeYardimcisiVer());

            // ASCII-Turkce donusturucu icin tukce toleransli cozumleyici olusumu.
            IKokBulucu turkceToleransliKokBulucu = kokler.AsciiKokBulucuGetir();
            _asciiToleransliCozumleyici = new StandartCozumleyici(
                    turkceToleransliKokBulucu,
                    new AsciiToleransliHDKiyaslayici(),
                    _dilFabrikasi.AlfabeVer(),
                    _dilFabrikasi.EkYoneticisiver(),
                    _dilFabrikasi.KokOzelDurumYoneticiVer(),
                    _dilFabrikasi.CozumlemeYardimcisiVer());

            IKokBulucu toleransliBulucu = kokler.ToleransliKokBulucuGetir(1);
            ToleransliCozumleyici toleransliCozumleyici = new ToleransliCozumleyici(
                    toleransliBulucu,
                    _dilFabrikasi.EkYoneticisiver(),
                    _dilFabrikasi.KokOzelDurumYoneticiVer(),
                    _dilFabrikasi.AlfabeVer(),
                    _dilFabrikasi.CozumlemeYardimcisiVer());

            _oneriUretici = new OneriUretici(
                    _dilFabrikasi.CozumlemeYardimcisiVer(),
                    _cozumleyici,
                    _asciiToleransliCozumleyici,
                    toleransliCozumleyici,
                    _ayarlar);

            _turkceTest = new TurkceYaziTesti(_cozumleyici, _asciiToleransliCozumleyici);

            _asciiDonusturucu = new AsciiDonusturucu(_dilFabrikasi.AlfabeVer());
            _heceleyici = _dilFabrikasi.HeceleyiciVer(); // new Heceleyici(_dilFabrikasi.Alfabe(), _dilFabrikasi.heceBulucu());

            _kelimeUretici = new KelimeUretici(_dilFabrikasi.AlfabeVer(), _dilFabrikasi.CozumlemeYardimcisiVer(), _dilFabrikasi.KokOzelDurumYoneticiVer());
            _denetlemeCebi = _dilFabrikasi.DenetlemeCebiVer();
        }

        /// <summary>
        /// Verilen kelime dizisini kelimenin 'ToString' metodu ile string dizisine çevirir.
        /// Yani çözümleme sonuçlarýný döner.
        /// </summary>
        /// <param name="kelimeler"></param>
        /// <returns></returns>
        private String[] KelimeleriMetneDonustur(Kelime[] kelimeler)
        {
            String[] retStrings = new String[kelimeler.Length];
            for(int i=0;i<kelimeler.Length;i++)
            {
                retStrings[i] = kelimeler[i].ToString();
            }
            return retStrings;
        }

        /// <summary>
        /// Verilen kelime dizisini kelime içerikleri ile string dizisine çevirir.
        /// Ýçeriði ayný olan çözümlerden sadece birinin içeriðini yani ayrýk sonuçlarý döner.
        /// </summary>
        /// <param name="kelimeler"></param>
        /// <returns></returns>
        private String[] IcerikleriMetneCevir(Kelime[] kelimeler)
        {
            ArrayList olusumlar = new ArrayList(kelimeler.Length);
            foreach (Kelime kelime in kelimeler)
            {
                String olusum = kelime.IcerikMetni();
                if (!olusumlar.Contains(olusum))
                {
                    olusumlar.Add(olusum);
                }
            }
            //kumeyi tekrar diziye donustur.
            string[] retStrings=new string[olusumlar.Count];
            olusumlar.CopyTo(retStrings);
            return retStrings;
        }


        public bool KelimeDenetle(String giris)
        {
            if (_denetlemeCebi != null)
            {
                return _denetlemeCebi.Kontrol(giris) || _cozumleyici.Cozumlenebilir(giris);
            }
            else
            {
                return _cozumleyici.Cozumlenebilir(giris);
            }
        }

        /**
         * performs morphological parsing of the word. Returns the possible solutions as an Kelime array.
         * Kelime object contains the root and a suffix list. Kok method can be used for accessing the
         * root. EkYoneticisiver() can be used for accessing the Ek object List.
         *
         *
         * giris kelimesinin olasi tum (kok+EkYoneticisiver) cozumlemelerini dondurur.
         *
         * @param giris giris kelimesi
         * @return Kelime sinifi cinsinden dizi. Eger dizinin boyu 0 ise kelime cozumlenemedi demektir.
         *         Kelime kokune erisim icin Kok, eklere erisim icin Ek cinsinden nesne listesi donduren
         *         EkYoneticisiver() metodu kullanilir.
         * @see NZemberek.Cekirdek.Yapi.Kelime
         */
        public String[] KelimeCozumle(String giris)
        {
            return KelimeleriMetneDonustur(_cozumleyici.Cozumle(giris, CozumlemeSeviyesi.TUM_KOK_VE_EKLER));
        }

        /**
         * giris kelimesinin ascii karakter toleransli olarak cozumleyip
         * Kelime cinsinden(kok+EkYoneticisiver) cozumlemelerini dondurur.
         * Birden cok cozumun oldugu durumda simdilik donen adaylarin
         * hangisinin gercekten yazidaki kelime olup olmadigi belirlenmiyor. ancak donen sonuclar
         * basitce kok kullanim frekansina gore dizilir. Yani ilk kelime buyuk ihtimalle kastedilen kelimedir.
         *
         * @param giris giris kelimesi
         * @return Kelime sinifi cinsinden dizi. Eger dizinin boyu 0 ise kelime cozumlenemedi demektir.
         *         Kelime kokune erisim icin Kok, eklere erisim icin Ek cinsinden nesne listesi donduren
         *         EkYoneticisiver() metodu kullanilir.  Kelimenin String cinsinden ifadesi icin icerik().ToString()
         *         metodu kullanilabilir.
         * @see NZemberek.Cekirdek.Yapi.Kelime
         */
        public String[] AsciiToleransliCozumle(String giris)
        {
            Kelime[] sonuclar = _asciiToleransliCozumleyici.Cozumle(giris, CozumlemeSeviyesi.TUM_KOK_VE_EKLER);
            Array.Sort(sonuclar, new KelimeKokFrekansKiyaslayici());
            return KelimeleriMetneDonustur(sonuclar);
        }

        public String[] AsciiTolerasnliCozumle(String giris, CozumlemeSeviyesi seviye)
         {
	        Kelime[] sonuclar = _asciiToleransliCozumleyici.Cozumle(giris, seviye);
            Array.Sort(sonuclar, new KelimeKokFrekansKiyaslayici());
            return KelimeleriMetneDonustur(sonuclar);
	    }

        /// <summary>
        /// Brings the most probable tukish equivalents of a string that uses ascii look alikes of
        /// those characters. Returns a distinct result set.
        /// AsciiToleransliCozumle ile benzer bir yapidadir. Farki donus degerlerinin tekil olmasidir, 
        /// yani yazimi ayni iki kelimeden biri döner.
        /// </summary>
        /// <param name="giris"></param>
        /// <returns>yazilan kelimenin olasi turkce karakter iceren halleri.</returns>
        public String[] TurkceKarakterlereDonustur(String giris)
        {
            Kelime[] kelimeler = _asciiToleransliCozumleyici.Cozumle(giris, CozumlemeSeviyesi.TUM_KOKLER); ;
            return IcerikleriMetneCevir(kelimeler);
        }

        /**
         * kelime icindeki dile ozel karakterleri ASCII benzer formalarina dondurur.
         *
         * @param giris giris kelimesi
         * @return turkce karakter tasimayan String.
         */
        public String AsciiKarakterlereDonustur(String giris)
        {
            return _asciiDonusturucu.AsciiyeDonustur(giris);
        }

        /**
         * girilen kelimeyi heceler.
         *
         * @param giris giris kelimesi
         * @return String dizisi. Eger dizi boyu 0 ise kelime hecelenememis demektir.
         */
        public String[] Hecele(String giris)
        {
            return _heceleyici.Hecele(giris);
        }

        /**
         * giris kelimesine yakin Stringleri dondurur. Yani eger kelime bozuk ise bu kelimeye
         * benzeyen dogru kelime olasiliklarini dondurur. simdilik
         * - 1 Harf eksikligi
         * - 1 Harf fazlaligi
         * - 1 yanlis Harf kullanimi
         * - yanyana yeri yanlis Harf kullanimi.
         * hatalarini giderecek sekilde cozumleri donduruyor. Bu metod dogru kelimeler icin de
         * isler, yani giris "kedi" ise donus listesinde kedi ve kedi'ye benzesen kelimeler de doner.
         * Ornegin "kedim", "yedi" .. gibi.
         *
         * @param giris giris kelimesi
         *
         * @return String sinifi cinsinden dizi. Eger dizinin boyu 0 ise kelime cozumlenemedi demektir.
         * @see NZemberek.Cekirdek.Yapi.Kelime
         */
        public String[] Oner(String giris)
        {
            return _oneriUretici.Oner(giris);
        }

        /**
         * giris kelime ya da yazisi icindeki cesitli kodlama hatalarindan kaynkalanan
         * bozulmalari onarir. Bu metod kelime ya da kelime dizisi icin calisir
         * Bazi bozulmalar henuz duzeltilemiyor olabilir.
         *
         * @param giris giris kelimesi
         * @return girisin temizlenmis hali.
         */
        public String Temizle(String giris)
        {
            if (_temizleyici == null)
            {
                _temizleyici = new HataliKodlamaTemizleyici();
                try
                {
                    _temizleyici.Baslat();
                }
                catch (System.IO.IOException e)
                {
#if log
                    logger.Error(e.Message);
#endif
                }
            }
            if (_temizleyici == null) return null;
            return _temizleyici.Temizle(giris);
        }

        /**
         * Basit sekilde giris kelime ya da kelime dizisinin Zemberek olusturulrken kullanilan dil ile
         * benzerligi kestirir. Girilen kelime sayisinin azligi soznucun basarimini dusurur.
         * donus farkli seviyelerde olabilir.
         *
         * @param giris giris string
         * @return Donus integer 0-4 arasi deger alabilir. nesne olusturulurken kullanilan dil D ise
         *         0 yazinin D dili olmadigi 4 ise kesin D oldugunu belirtir.
         *         ara degerler
         *         1- yazi icinde D olabilecek kelimeler var, ama genel D degil.
         *         2- yazi D, cok fazla yabanci ya da bozuk kelime var.
         *         3- yazi D, yabanci ve bozuk kelimeler iceriyor.
         */
        public int TurkceMi(String giris)
        {
            return _turkceTest.TurkceTest(giris);
        }

        /**
         * Istenilen kelimenin olasi String acilimlarini bulur.
         * Ornegin, "alayim" icin
         * "Al-a-yim" ve "ala-yim" cozumleri String dizileri seklinde uretilir.
         * sonucta olusan diziler bir Listeye eklenir.
         *
         * @param kelime giris kelimesi
         *
         * @return Kok ve ek olusumlarini ifade eden String dizilerini tasiyan List.
         *         List<List<String>>
         *         Eger kelime ayristirilamiyorsa sifir uzunluklu String dizisi tasiyan tek elemanli
         *         liste doner. .
         */
        public IList<IList<String>> OlasiAcilimlariBul(String kelime)
        {
            IList<IList<String>> sonuclar = new List<IList<String>>();
            Kelime[] cozumler = _cozumleyici.Cozumle(kelime, CozumlemeSeviyesi.TUM_KOK_VE_EKLER);
            foreach (Kelime kel in cozumler)
            {
                sonuclar.Add(_kelimeUretici.Ayristir(kel));
            }
            return sonuclar;
        }
    }
}






