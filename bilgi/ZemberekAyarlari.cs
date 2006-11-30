using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using log4net;
using System.Collections.Specialized;
namespace net.zemberek.bilgi
{
    public class ZemberekAyarlari
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);		

    private  NameValueCollection konfigurasyon;
    private bool _oneriDeasciifierKullan = true;
    private int oneriMax = 12;
    private bool _oneriKokFrekansKullan = true;
    private bool _disKaynakErisimi = false;
    private bool _oneriBilesikKelimeKullan = true;
    private bool _cepKullan = true;
    private String _kayitSeviyesi = "WARNING";

        private Uri bilgiEk;
        private Uri bilgiKokler;
        private Uri bilgiAlfabe;
        private Uri bilgiDizini;
        private Uri bilgiCep;
    

    /**
     * classpath kokunden zemberek_'locale_str'.properties dosyasina erismeye calisir.
     * Bu dosyanin normalde projede yer almis olmasi gerekir. Eger bulunamazsa sistem
     * varsayilan degerleri kullanir.
     * @param dilKisaAdi kisa ad (tr,az vs)
     */
    public ZemberekAyarlari(String dilKisaAdi) {
        try {
            konfigurasyon = new KaynakYukleyici().konfigurasyonYukle("zemberek_" + dilKisaAdi + ".properties");
        } catch (System.IO.IOException e) {
            logger.Warn("Konfigurasyon dosyasina erisilemiyor! varsayilan degerler kullanilacak");
        }
        konfigurasyonOku(konfigurasyon);
    }


    /**
     * Isaret edilen Properties dosyasindan verilere erisilmesye calisilir.
     *
     * @param ayarlar
     */
    public ZemberekAyarlari(NameValueCollection ayarlar) {
        this.konfigurasyon = ayarlar;
        konfigurasyonOku(ayarlar);
    }

    private void konfigurasyonOku(NameValueCollection ayarlar) {
        try {
            _oneriDeasciifierKullan = boolOku(ayarlar, "oneri.deasciifierKullan");
            _oneriKokFrekansKullan = boolOku(ayarlar, "oneri.kokFrekansKullan");
            _oneriBilesikKelimeKullan = boolOku(ayarlar, "oneri.bilesikKelimeKullan");
            oneriMax = Int32.Parse(ayarlar.Get("oneri.max"));
            _disKaynakErisimi = boolOku(ayarlar, "bilgi.disKaynakErisimi");
            _cepKullan = boolOku(ayarlar, "denetleme.cepKullan");
            _kayitSeviyesi = ayarlar.Get("genel.kayitSeviyesi");
            //TODO sadece commentledim
            //Kayitci.genelKayitSeviyesiAyarla(_kayitSeviyesi);
            if (_disKaynakErisimi) {
                //TODO gerekebilir : if(Directory.Exists(ayarlar.Get("bilgi.dizin")))
                // ve buralar gözden gecemeli
                string dizin = ayarlar.Get("bilgi.dizin");
                bilgiDizini = new Uri(dizin);
                bilgiEk = new Uri(dizin+"/"+ayarlar.Get("bilgi.ekler"));
                File.OpenRead(bilgiEk.ToString());
                bilgiKokler = new Uri(dizin + "/" + ayarlar.Get("bilgi.kokler"));
                File.OpenRead(bilgiKokler.ToString());
                bilgiAlfabe = new Uri(dizin + "/" + ayarlar.Get("bilgi.harf"));
                File.OpenRead(bilgiAlfabe.ToString());
                bilgiCep = new Uri(dizin + "/" + ayarlar.Get("bilgi.harf"));
                File.OpenRead(bilgiCep.ToString());
            }
        } catch (FormatException e) {
            logger.Error("property erisim hatasi!! Muhtemel tip donusum problemi.. varsayilan parametreler kullanilacak ");
                   } catch (Exception e) {
            logger.Error("property erisim hatasi!! propety yer almiyor, ya da adi yanlis yazilmis olabilir. varsayilan konfigurasyon kullanilacak.");
        }
    }

    private bool boolOku(NameValueCollection ayarlar, String anahtar) {
        return bool.Parse(ayarlar.Get(anahtar));
    }

    public NameValueCollection getKonfigurasyon() {
        return konfigurasyon;
    }

    public bool oneriDeasciifierKullan() {
        return _oneriDeasciifierKullan;
    }

    public bool oneriBilesikKelimeKullan() {
        return _oneriBilesikKelimeKullan;
    }

    public int getOneriMax() {
        return oneriMax;
    }

    public bool oneriKokFrekansKullan() {
        return _oneriKokFrekansKullan;
    }

    public Uri getBilgiEk() {
        return bilgiEk;
    }

        public Uri getBilgiKokler()
        {
        return bilgiKokler;
    }

        public Uri getBilgiDizini()
        {
        return bilgiDizini;
    }

        public Uri getBilgiAlfabe()
        {
        return bilgiAlfabe;
    }

        public Uri getBilgiCep()
        {
        return bilgiCep;
    }

    public bool disKaynakErisimi() {
        return _disKaynakErisimi;
    }

    public bool cepKullan() {
        return _cepKullan;
    }


    public String kayitSeviyesi() {
        return _kayitSeviyesi;
    }
    }
}
