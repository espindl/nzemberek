using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using log4net;
using System.Collections.Specialized;
using System.Configuration;


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
            try 
            {
                //konfigurasyon = new KaynakYukleyici().konfigurasyonYukle("zemberek_" + dilKisaAdi + ".properties");
                konfigurasyonOku();
            } 
            catch (System.IO.IOException e) 
            {
                logger.Warn("Konfigurasyon kayıtlarına erisilemiyor! varsayilan degerler kullanilacak. Hata : "+e.Message);
            }
        }


        /**
         * Isaret edilen Properties dosyasindan verilere erisilmesye calisilir.
         *
         * @param ayarlar
         */
        public ZemberekAyarlari() {
            konfigurasyonOku();
        }

        private void konfigurasyonOku() 
        {
            try 
            {
                _oneriDeasciifierKullan = boolOku("oneri.deasciifierKullan");
                _oneriKokFrekansKullan = boolOku("oneri.kokFrekansKullan");
                _oneriBilesikKelimeKullan = boolOku("oneri.bilesikKelimeKullan");
                oneriMax = Int32.Parse(ConfigurationManager.AppSettings["oneri.max"]);
                _disKaynakErisimi = boolOku("bilgi.disKaynakErisimi");
                _cepKullan = boolOku("denetleme.cepKullan");
                //TODO loglama seviyeleri log4netin configi ile yapılacak (@tankut)
                //Kayitci.genelKayitSeviyesiAyarla(_kayitSeviyesi);
                if (_disKaynakErisimi) 
                {
                    //TODO gerekebilir : if(Directory.Exists(ConfigurationManager.AppSettings["bilgi.dizin")))
                    // ve buralar gözden gecemeli
                    string dizin = ConfigurationManager.AppSettings["bilgi.dizin"];
                    bilgiDizini = new Uri(dizin);
                    bilgiEk = new Uri(dizin+"/"+ConfigurationManager.AppSettings["bilgi.ekler"]);
                    File.OpenRead(bilgiEk.ToString());
                    bilgiKokler = new Uri(dizin + "/" + ConfigurationManager.AppSettings["bilgi.kokler"]);
                    File.OpenRead(bilgiKokler.ToString());
                    bilgiAlfabe = new Uri(dizin + "/" + ConfigurationManager.AppSettings["bilgi.harf"]);
                    File.OpenRead(bilgiAlfabe.ToString());
                    bilgiCep = new Uri(dizin + "/" + ConfigurationManager.AppSettings["bilgi.harf"]);
                    File.OpenRead(bilgiCep.ToString());
                }
            } 
            catch (FormatException e) 
            {
                logger.Error("property erisim hatasi!! Muhtemel tip donusum problemi.. varsayilan parametreler kullanilacak "+e.Message);
            } 
            catch (Exception e) 
            {
                logger.Error("property erisim hatasi!! propety yer almiyor, ya da adi yanlis yazilmis olabilir. varsayilan konfigurasyon kullanilacak."+e.Message);
            }
        }

        private bool boolOku(String anahtar)
        {
            return bool.Parse(ConfigurationManager.AppSettings[anahtar]);
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

        public bool disKaynakErisimi() 
        {
            return _disKaynakErisimi;
        }

        public bool cepKullan() 
        {
            return _cepKullan;
        }
    }
}
