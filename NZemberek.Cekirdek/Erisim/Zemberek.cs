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

        private KelimeCozumleyici _cozumleyici;
        private KelimeUretici _kelimeUretici;
        private KelimeCozumleyici _asciiToleransliCozumleyici;
        private HataliKodlamaTemizleyici _temizleyici;
        private TurkceYaziTesti _turkceTest;
        private OneriUretici _oneriUretici;
        private AsciiDonusturucu _asciiDonusturucu;
        private IHeceleyici _heceleyici;
        private ZemberekAyarlari _ayarlar;
        private IDilFabrikasi _dilFabrikasi;

        /**
         * Default constructor.
         * @param dilayarlari
         */
        public Zemberek()
        {
            _ayarlar = new ZemberekAyarlari();
            Assembly dilpaketi = Assembly.Load(_ayarlar.getDilAyarlari()[0]);
            this._dilFabrikasi = (IDilFabrikasi)dilpaketi.CreateInstance(_ayarlar.getDilAyarlari()[1]);
            this._dilFabrikasi.CepKullan = _ayarlar.cepKullan();
            initialize();
        }

        private void initialize()
        {
            //Sozluk hazirla.
            ISozluk kokler = _dilFabrikasi.kokler();
            //Normal denetleyici-cozumleyici olusumu
            IKokBulucu kokBulucu = kokler.KesinKokBulucuGetir();
            _cozumleyici = new StandartCozumleyici(
                    kokBulucu,
                    new KesinHDKiyaslayici(),
                    _dilFabrikasi.alfabe(),
                    _dilFabrikasi.ekler(),
                    _dilFabrikasi.cozumlemeYardimcisi());

            // ASCII-Turkce donusturucu icin tukce toleransli cozumleyici olusumu.
            IKokBulucu turkceToleransliKokBulucu = kokler.AsciiKokBulucuGetir();
            _asciiToleransliCozumleyici = new StandartCozumleyici(
                    turkceToleransliKokBulucu,
                    new AsciiToleransliHDKiyaslayici(),
                    _dilFabrikasi.alfabe(),
                    _dilFabrikasi.ekler(),
                    _dilFabrikasi.cozumlemeYardimcisi());

            IKokBulucu toleransliBulucu = kokler.ToleransliKokBulucuGetir(1);
            ToleransliCozumleyici toleransliCozumleyici = new ToleransliCozumleyici(
                    toleransliBulucu,
                    _dilFabrikasi.ekler(),
                    _dilFabrikasi.alfabe(),
                    _dilFabrikasi.cozumlemeYardimcisi());

            _oneriUretici = new OneriUretici(
                    _dilFabrikasi.cozumlemeYardimcisi(),
                    _cozumleyici,
                    _asciiToleransliCozumleyici,
                    toleransliCozumleyici,
                    _ayarlar);

            _turkceTest = new TurkceYaziTesti(_cozumleyici, _asciiToleransliCozumleyici);

            _asciiDonusturucu = new AsciiDonusturucu(_dilFabrikasi.alfabe());
            _heceleyici = _dilFabrikasi.heceleyici(); // new Heceleyici(_dilFabrikasi.alfabe(), _dilFabrikasi.heceBulucu());

            _kelimeUretici = new KelimeUretici(_dilFabrikasi.alfabe(), _dilFabrikasi.cozumlemeYardimcisi());
        }

        /// <summary>
        /// Verilen kelime dizisini kelimenin 'ToString' metodu ile string dizisine çevirir.
        /// Yani çözümleme sonuçlarýný döner.
        /// </summary>
        /// <param name="kelimeler"></param>
        /// <returns></returns>
        private String[] CozumStringeCevir(Kelime[] kelimeler)
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
        private String[] IcerikStringeCevir(Kelime[] kelimeler)
        {
            ArrayList olusumlar = new ArrayList(kelimeler.Length);
            foreach (Kelime kelime in kelimeler)
            {
                String olusum = kelime.icerikStr();
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


        public bool kelimeDenetle(String giris)
        {
            return _cozumleyici.denetle(giris);
        }

        /**
         * performs morphological parsing of the word. Returns the possible solutions as an Kelime array.
         * Kelime object contains the root and a suffix list. kok() method can be used for accessing the
         * root. ekler() can be used for accessing the Ek object List.
         *
         *
         * giris kelimesinin olasi tum (kok+ekler) cozumlemelerini dondurur.
         *
         * @param giris giris kelimesi
         * @return Kelime sinifi cinsinden dizi. Eger dizinin boyu 0 ise kelime cozumlenemedi demektir.
         *         Kelime kokune erisim icin kok(), eklere erisim icin Ek cinsinden nesne listesi donduren
         *         ekler() metodu kullanilir.
         * @see NZemberek.Cekirdek.Yapi.Kelime
         */
        public String[] kelimeCozumle(String giris)
        {
            return CozumStringeCevir(_cozumleyici.cozumle(giris));
        }

        /**
         * giris kelimesinin ascii karakter toleransli olarak cozumleyip
         * Kelime cinsinden(kok+ekler) cozumlemelerini dondurur.
         * Birden cok cozumun oldugu durumda simdilik donen adaylarin
         * hangisinin gercekten yazidaki kelime olup olmadigi belirlenmiyor. ancak donen sonuclar
         * basitce kok kullanim frekansina gore dizilir. Yani ilk kelime buyuk ihtimalle kastedilen kelimedir.
         *
         * @param giris giris kelimesi
         * @return Kelime sinifi cinsinden dizi. Eger dizinin boyu 0 ise kelime cozumlenemedi demektir.
         *         Kelime kokune erisim icin kok(), eklere erisim icin Ek cinsinden nesne listesi donduren
         *         ekler() metodu kullanilir.  Kelimenin String cinsinden ifadesi icin icerik().ToString()
         *         metodu kullanilabilir.
         * @see NZemberek.Cekirdek.Yapi.Kelime
         */
        public String[] asciiCozumle(String giris)
        {
            Kelime[] sonuclar = _asciiToleransliCozumleyici.cozumle(giris);
            Array.Sort(sonuclar, new KelimeKokFrekansKiyaslayici());
            return CozumStringeCevir(sonuclar);
        }

        /// <summary>
        /// Brings the most probable tukish equivalents of a string that uses ascii look alikes of
        /// those characters. Returns a distinct result set.
        /// asciiCozumle ile benzer bir yapidadir. Farki donus degerlerinin tekil olmasidir, 
        /// yani yazimi ayni iki kelimeden biri döner.
        /// </summary>
        /// <param name="giris"></param>
        /// <returns>yazilan kelimenin olasi turkce karakter iceren halleri.</returns>
        public String[] asciidenTurkceye(String giris)
        {
            Kelime[] kelimeler = _asciiToleransliCozumleyici.cozumle(giris); ;
            return IcerikStringeCevir(kelimeler);
        }

        /**
         * kelime icindeki dile ozel karakterleri ASCII benzer formalarina dondurur.
         *
         * @param giris giris kelimesi
         * @return turkce karakter tasimayan String.
         */
        public String asciiyeDonustur(String giris)
        {
            return _asciiDonusturucu.toAscii(giris);
        }

        /**
         * girilen kelimeyi heceler.
         *
         * @param giris giris kelimesi
         * @return String dizisi. Eger dizi boyu 0 ise kelime hecelenememis demektir.
         */
        public String[] hecele(String giris)
        {
            return _heceleyici.hecele(giris);
        }

        /**
         * giris kelimesine yakin Stringleri dondurur. Yani eger kelime bozuk ise bu kelimeye
         * benzeyen dogru kelime olasiliklarini dondurur. simdilik
         * - 1 harf eksikligi
         * - 1 harf fazlaligi
         * - 1 yanlis harf kullanimi
         * - yanyana yeri yanlis harf kullanimi.
         * hatalarini giderecek sekilde cozumleri donduruyor. Bu metod dogru kelimeler icin de
         * isler, yani giris "kedi" ise donus listesinde kedi ve kedi'ye benzesen kelimeler de doner.
         * Ornegin "kedim", "yedi" .. gibi.
         *
         * @param giris giris kelimesi
         *
         * @return String sinifi cinsinden dizi. Eger dizinin boyu 0 ise kelime cozumlenemedi demektir.
         * @see NZemberek.Cekirdek.Yapi.Kelime
         */
        public String[] oner(String giris)
        {
            return _oneriUretici.oner(giris);
        }

        /**
         * giris kelime ya da yazisi icindeki cesitli kodlama hatalarindan kaynkalanan
         * bozulmalari onarir. Bu metod kelime ya da kelime dizisi icin calisir
         * Bazi bozulmalar henuz duzeltilemiyor olabilir.
         *
         * @param giris giris kelimesi
         * @return girisin temizlenmis hali.
         */
        public String temizle(String giris)
        {
            if (_temizleyici == null)
            {
                _temizleyici = new HataliKodlamaTemizleyici();
                try
                {
                    _temizleyici.initialize();
                }
                catch (System.IO.IOException e)
                {
                    logger.Error(e.Message);
                }
            }
            if (_temizleyici == null) return null;
            return _temizleyici.temizle(giris);
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
        public int dilTesti(String giris)
        {
            return _turkceTest.turkceTest(giris);
        }

        /**
         * Istenilen kelimenin olasi String acilimlarini bulur.
         * Ornegin, "alayim" icin
         * "al-a-yim" ve "ala-yim" cozumleri String dizileri seklinde uretilir.
         * sonucta olusan diziler bir Listeye eklenir.
         *
         * @param kelime giris kelimesi
         *
         * @return Kok ve ek olusumlarini ifade eden String dizilerini tasiyan List.
         *         List<List<String>>
         *         Eger kelime ayristirilamiyorsa sifir uzunluklu String dizisi tasiyan tek elemanli
         *         liste doner. .
         */
        public IList<IList<String>> kelimeAyristir(String kelime)
        {
            IList<IList<String>> sonuclar = new List<IList<String>>();
            Kelime[] cozumler = _cozumleyici.cozumle(kelime);
            foreach (Kelime kel in cozumler)
            {
                sonuclar.Add(_kelimeUretici.ayristir(kel));
            }
            return sonuclar;
        }
    }
}






