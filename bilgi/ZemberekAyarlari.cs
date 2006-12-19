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
 * The Original Code is Zemberek Doğal Dil İşleme Kütüphanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Akın, Mehmet D. Akın.
 * Portions created by the Initial Developer are Copyright (C) 2006
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Mert Derman
 *   Tankut Tekeli
 * ***** END LICENSE BLOCK ***** */

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
                NameValueCollection settings = GetSettings();
                _oneriDeasciifierKullan = boolOku("oneri.deasciifierKullan");
                _oneriKokFrekansKullan = boolOku("oneri.kokFrekansKullan");
                _oneriBilesikKelimeKullan = boolOku("oneri.bilesikKelimeKullan");
                oneriMax = Int32.Parse(settings["oneri.max"]);
                _disKaynakErisimi = boolOku("bilgi.disKaynakErisimi");
                _cepKullan = boolOku("denetleme.cepKullan");
                //TODO loglama seviyeleri log4netin configi ile yapılacak (@tankut)
                //Kayitci.genelKayitSeviyesiAyarla(_kayitSeviyesi);
                if (_disKaynakErisimi) 
                {
                    //TODO gerekebilir : if(Directory.Exists(ConfigurationManager.AppSettings["bilgi.dizin")))
                    // ve buralar gözden gecemeli
                    string dizin = settings["bilgi.dizin"];
                    bilgiDizini = new Uri(dizin);
                    bilgiEk = new Uri(dizin + "/" + settings["bilgi.ekler"]);
                    File.OpenRead(bilgiEk.ToString());
                    bilgiKokler = new Uri(dizin + "/" + settings["bilgi.kokler"]);
                    File.OpenRead(bilgiKokler.ToString());
                    bilgiAlfabe = new Uri(dizin + "/" + settings["bilgi.harf"]);
                    File.OpenRead(bilgiAlfabe.ToString());
                    bilgiCep = new Uri(dizin + "/" + settings["bilgi.harf"]);
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

        private NameValueCollection GetSettings()
        {
#if mono
            NameValueCollection settings = ConfigurationSettings.AppSettings;
#endif
#if net
            NameValueCollection settings = ConfigurationManager.AppSettings;
#endif
            return settings;
        }
        private bool boolOku(String anahtar)
        {
            return bool.Parse(GetSettings()[anahtar]);
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

        //private static NameValueCollection GetSettingsForRuntime()
        //{
        //    Version mono = new Version(1,2,2);
        //    Version net20 = new Version(2,0);
        //    Version cur = Environment.Version;
        //    Type type = null;
        //    if (cur < net20)
        //    {
        //        type = typeof(ConfigurationSettings);
        //    }
        //    else
        //    {
        //        type = typeof(ConfigurationManager);
        //    }
        //    System.Reflection.PropertyInfo prop = type.GetProperty("AppSettings", System.Reflection.BindingFlags.Static);
        //    NameValueCollection settings = (NameValueCollection)prop.GetValue(null, null);
        //    return settings;
        //}
    }
}
