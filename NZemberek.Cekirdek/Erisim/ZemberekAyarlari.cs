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


namespace NZemberek
{
    /// <summary>
    /// Uygulama konfigürasyon dosyasından zemberek ile ilgili ayarları okur.
    /// </summary>
    public class ZemberekAyarlari
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);		

        private bool _oneriDeasciifierKullan = true;
        private int oneriMax = 12;
        private bool _oneriKokFrekansKullan = true;
        private bool _oneriBilesikKelimeKullan = true;
        private bool _cepKullan = true;

        private string[] dilAyarlari = new string[3];
        

        public ZemberekAyarlari() {
            try 
            {
                konfigurasyonOku();
            } 
            catch (System.IO.IOException e) 
            {
                logger.Warn("Konfigurasyon kayıtlarına erisilemiyor! varsayilan degerler kullanilacak. Hata : "+e.Message);
            }
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
                _cepKullan = boolOku("denetleme.cepKullan");
                dilAyarlari[0] = settings["dilAyarlari.KutuphaneAdi"];
                dilAyarlari[1] = settings["dilAyarlari.SinifAdi"];
                dilAyarlari[2] = settings["dilAyarlari.KaynakDizini"];
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

        public bool cepKullan() 
        {
            return _cepKullan;
        }

        public string[] getDilAyarlari()
        {
            return dilAyarlari;
        }
    }
}
