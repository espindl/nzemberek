using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using net.zemberek.araclar;
using net.zemberek.bilgi;
using net.zemberek.bilgi.kokler;
using net.zemberek.islemler;
using net.zemberek.islemler.cozumleme;
using net.zemberek.yapi;
using net.zemberek.yapi.ek;
using log4net;


namespace net.zemberek.erisim
{
    /**
     * <b>ENin</b>This is a facade for accessing the high level functions of the Zemberek library.
     * This class should be created only once per language.
     *
     * <b>TRin</b>Zemberek projesine ust seviye erisim icin kullanilan sinif.
     * Ilk olsum sirasinda kokler okuma ve agac olusumu nedeniyle belli bir miktar gecikme
     * yasanabilir. Bu sinifin her dil icin sadece bir defa olusturulmasi onerilir.
     */
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
    private DilBilgisi _dilBilgisi;

    /**
     * Default constructor.
     * @param dilayarlari
     */
    public Zemberek(DilAyarlari dilayarlari) {
        _ayarlar = new ZemberekAyarlari(dilayarlari.locale().Name);
        this._dilBilgisi = new TurkceDilBilgisi(dilayarlari, _ayarlar);
        initialize();
    }

    /**
     * Dosya sisteminden zemberek properties dosyasini yukleyip ZemberekAyarlari nesnesine atar.
     *
     * @param disKonfigurasyon
     * @return
     */
    public static ZemberekAyarlari ayarOlustur(String disKonfigurasyon)
    {
        return new ZemberekAyarlari(new KaynakYukleyici().konfigurasyonYukle(disKonfigurasyon));
    }

    private void initialize() {
        //Sozluk hazirla.
        Sozluk kokler = _dilBilgisi.kokler();
        //Normal denetleyici-cozumleyici olusumu
        KokBulucu kokBulucu = kokler.getKokBulucuFactory().getKesinKokBulucu();
        _cozumleyici = new StandartCozumleyici(
                kokBulucu,
                new KesinHDKiyaslayici(),
                _dilBilgisi.alfabe(),
                _dilBilgisi.ekler(),
                _dilBilgisi.cozumlemeYardimcisi());

        // ASCII-Turkce donusturucu icin tukce toleransli cozumleyici olusumu.
        KokBulucu turkceToleransliKokBulucu = kokler.getKokBulucuFactory().getAsciiKokBulucu();
        _asciiToleransliCozumleyici = new StandartCozumleyici(
                turkceToleransliKokBulucu,
                new AsciiToleransliHDKiyaslayici(),
                _dilBilgisi.alfabe(),
                _dilBilgisi.ekler(),
                _dilBilgisi.cozumlemeYardimcisi());

        KokBulucu toleransliBulucu = kokler.getKokBulucuFactory().getToleransliKokBulucu(1);
        ToleransliCozumleyici toleransliCozumleyici = new ToleransliCozumleyici(
                toleransliBulucu,
                _dilBilgisi.ekler(),
                _dilBilgisi.alfabe(),
                _dilBilgisi.cozumlemeYardimcisi());

        _oneriUretici = new OneriUretici(
                _dilBilgisi.cozumlemeYardimcisi(),
                _cozumleyici,
                _asciiToleransliCozumleyici,
                toleransliCozumleyici,
                _ayarlar);

        _turkceTest = new TurkceYaziTesti(_cozumleyici, _asciiToleransliCozumleyici);

        _asciiDonusturucu = new AsciiDonusturucu(_dilBilgisi.alfabe());
        _heceleyici = new Heceleyici(_dilBilgisi.alfabe(), _dilBilgisi.heceBulucu());

        _kelimeUretici = new KelimeUretici(_dilBilgisi.alfabe(), _dilBilgisi.cozumlemeYardimcisi());
    }

    /**
     * return the word parser.
     *
     * @return cozumleyici
     */
    public KelimeCozumleyici cozumleyici() {
        return _cozumleyici;
    }

    public KelimeUretici kelimeUretici() {
        return _kelimeUretici;
    }

    public KelimeCozumleyici asciiToleransliCozumleyici() {
        return _asciiToleransliCozumleyici;
    }

    /**
     * Accessor for the word suggestion producer.
     * @return oneri uretici.
     */
    public OneriUretici oneriUretici() {
        return _oneriUretici;
    }

    /**
     * Accessor for the syllable extractor.
     *
     * @return  heceleyici
     */
    public Heceleyici heceleyici() {
        return _heceleyici;
    }

    /**
     * performs spell checking
     *
     * girisin imla denetimini yapar.
     *
     * @param giris giris kelimesi 
     * @return ENin trueinspell checking successfull, false otherwise.
     *         TRin trueinimla denetimi basarili. falsein Denetim basarisiz.
     */
    public bool kelimeDenetle(String giris) {
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
    public Kelime[] kelimeCozumle(String giris) {
        return _cozumleyici.cozumle(giris);
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
     *         ekler() metodu kullanilir.  Kelimenin String cinsinden ifadesi icin icerik().toString()
     *         metodu kullanilabilir.
     * @see net.zemberek.yapi.Kelime
     */
    public Kelime[] asciiCozumle(String giris) {
        Kelime[] sonuclar = _asciiToleransliCozumleyici.cozumle(giris);
        Array.Sort(sonuclar, new KelimeKokFrekansKiyaslayici());
        return sonuclar;
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
    public String[] asciidenTurkceye(String giris) {
        Kelime[] kelimeler = asciiCozumle(giris);
        // cift olusumlari temizle.
        ArrayList olusumlar = new ArrayList(kelimeler.Length);
        foreach(Kelime kelime in kelimeler) {
            String olusum = kelime.icerikStr();
            if (!olusumlar.Contains(olusum))
                olusumlar.Add(olusum);
        }
        //kumeyi tekrar diziye donustur.
        return (string[])olusumlar.ToArray(typeof(string));
    }

    /**
     * kelime icindeki dile ozel karakterleri ASCII benzer formalarina dondurur.
     *
     * @param giris giris kelimesi
     * @return turkce karakter tasimayan String.
     */
    public String asciiyeDonustur(String giris) {
        return _asciiDonusturucu.toAscii(giris);
    }

    /**
     * girilen kelimeyi heceler.
     *
     * @param giris giris kelimesi
     * @return String dizisi. Eger dizi boyu 0 ise kelime hecelenememis demektir.
     */
    public String[] hecele(String giris) {
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
    public String[] oner(String giris) {
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
    public String temizle(String giris) {
        if (_temizleyici == null) {
            _temizleyici = new HataliKodlamaTemizleyici();
            try {
                _temizleyici.initialize();
            } catch (System.IO.IOException e) 
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
    public int dilTesti(String giris) {
        return _turkceTest.turkceTest(giris);
    }

    /**
     * nesnenin olusumu sirasinda kullanilan DilBilgisi arabirimine sahip dili dondurur.
     * Eger nesne hic parametre kullanilmadan olusturulmussa bir adet TurkiyeTurkcesi nesnesi doner.
     *
     * @return bu nesneyi olustururken kullanilan DilBilgisi arayuzune sahip nesne.
     */
    public DilBilgisi dilBilgisi() {
        return _dilBilgisi;
    }

    /**
     * Istenilen kok ve ek listesi ile kelime uretir.
     *
     * @param kok kok nesnesi
     * @param ekler ek listesi
     *
     * @return String olarak uretilen kelime.
     */
    public String kelimeUret(Kok kok, IList<Ek> ekler) {
        return _kelimeUretici.kelimeUret(kok, ekler);
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
    public IList<IList<String>> kelimeAyristir(String kelime) {
        IList<IList<String>> sonuclar = new List<IList<String>>();
        Kelime[] cozumler = _cozumleyici.cozumle(kelime);
        foreach(Kelime kel in cozumler) {
            sonuclar.Add(_kelimeUretici.ayristir(kel));
        }
        return sonuclar;
    }

    /**
     * Zemberek konfigurasyon parametrelerini dondurur.
     *
     * @return ayarlar.
     */
    public ZemberekAyarlari ayarlar() {
        return _ayarlar;
    }

    }
}






