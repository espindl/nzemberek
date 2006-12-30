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
using net.zemberek.bilgi.kokler;
using net.zemberek.islemler;
using net.zemberek.islemler.cozumleme;
using net.zemberek.yapi;
using log4net;
using System.Reflection;


namespace net.zemberek.erisim
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
        private Heceleyici _heceleyici;
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
            Sozluk kokler = _dilFabrikasi.kokler();
            //Normal denetleyici-cozumleyici olusumu
            KokBulucu kokBulucu = kokler.getKokBulucuFactory().getKesinKokBulucu();
            _cozumleyici = new StandartCozumleyici(
                    kokBulucu,
                    new KesinHDKiyaslayici(),
                    _dilFabrikasi.alfabe(),
                    _dilFabrikasi.ekler(),
                    _dilFabrikasi.cozumlemeYardimcisi());

            // ASCII-Turkce donusturucu icin tukce toleransli cozumleyici olusumu.
            KokBulucu turkceToleransliKokBulucu = kokler.getKokBulucuFactory().getAsciiKokBulucu();
            _asciiToleransliCozumleyici = new StandartCozumleyici(
                    turkceToleransliKokBulucu,
                    new AsciiToleransliHDKiyaslayici(),
                    _dilFabrikasi.alfabe(),
                    _dilFabrikasi.ekler(),
                    _dilFabrikasi.cozumlemeYardimcisi());

            KokBulucu toleransliBulucu = kokler.getKokBulucuFactory().getToleransliKokBulucu(1);
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
            _heceleyici = new Heceleyici(_dilFabrikasi.alfabe(), _dilFabrikasi.heceBulucu());

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


        //TODO Bunlarý dýþarý sunmaktan vazgeçmeliydik vazgeçtik
        ///**
        // * Dosya sisteminden zemberek properties dosyasini yukleyip ZemberekAyarlari nesnesine atar.
        // *
        // * @param disKonfigurasyon
        // * @return
        // */
        //public static ZemberekAyarlari ayarOlustur(String disKonfigurasyon)
        //{
        //    return new ZemberekAyarlari();
        //}
        ///**
        // * Istenilen kok ve ek listesi ile kelime uretir.
        // *
        // * @param kok kok nesnesi
        // * @param ekler ek listesi
        // *
        // * @return String olarak uretilen kelime.
        // */
        //public String kelimeUret(Kok kok, IList<Ek> ekler)
        //{
        //    return _kelimeUretici.kelimeUret(kok, ekler);
        //}
        ///**
        // * Zemberek konfigurasyon parametrelerini dondurur.
        // *
        // * @return ayarlar.
        // */
        //public ZemberekAyarlari ayarlar()
        //{
        //    return _ayarlar;
        //}
        ///**
        //* nesnenin olusumu sirasinda kullanilan DilBilgisi arabirimine sahip dili dondurur.
        //* Eger nesne hic parametre kullanilmadan olusturulmussa bir adet TurkiyeTurkcesi nesnesi doner.
        //*
        //* @return bu nesneyi olustururken kullanilan DilBilgisi arayuzune sahip nesne.
        //*/
        //public DilBilgisi dilBilgisi()
        //{
        //    return _dilBilgisi;
        //}
        ///**
        // * return the word parser.
        // *
        // * @return cozumleyici
        // */
        //public KelimeCozumleyici cozumleyici() {
        //    return _cozumleyici;
        //}

        //public KelimeUretici kelimeUretici() {
        //    return _kelimeUretici;
        //}

        //public KelimeCozumleyici asciiToleransliCozumleyici() {
        //    return _asciiToleransliCozumleyici;
        //}

        ///**
        // * Accessor for the word suggestion producer.
        // * @return oneri uretici.
        // */
        //public OneriUretici oneriUretici() {
        //    return _oneriUretici;
        //}

        ///**
        // * Accessor for the syllable extractor.
        // *
        // * @return  heceleyici
        // */
        //public Heceleyici heceleyici() {
        //    return _heceleyici;
        //}

        /**
         * performs spell checking
         *
         * girisin imla denetimini yapar.
         *
         * @param giris giris kelimesi 
         * @return ENin trueinspell checking successfull, false otherwise.
         *         TRin trueinimla denetimi basarili. falsein Denetim basarisiz.
         */

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
         * @see net.zemberek.yapi.Kelime
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
         * @see net.zemberek.yapi.Kelime
         */
        public String[] asciiCozumle(String giris)
        {
            Kelime[] sonuclar = _asciiToleransliCozumleyici.cozumle(giris);
            Array.Sort(sonuclar, new KelimeKokFrekansKiyaslayici());
            return CozumStringeCevir(sonuclar);
        }

        /**
         * Brings the most probable tukish equivalents of a string that uses ascii look alikes of
         * those characters.
         *
         * asciiCozumle ile benzer bir yapidadir. Farki String[] dizisi donmesi ve
         * donus degerlerinin tekil olmasidir, yani ayni kelime tekrari olmaz.
         *
         * @param giris giris kelimesi
         * @return ENinpossible turkish equivalents of the ascii turkish string in a String array.
         *         TRinyazilan kelimenin olasi turkce karakter iceren halleri. String[] seklinde.
         */
        //TODO Burayý düzelteceðiz
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
         * @see net.zemberek.yapi.Kelime
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






