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

        public bool OneriDeasciifierKullan
        {
            get { return _oneriDeasciifierKullan; }
        }
        private int oneriMax = 12;

        public int OneriMax
        {
            get { return oneriMax; }
        }
        private bool _oneriKokFrekansKullan = true;

        public bool OneriKokFrekansKullan
        {
            get { return _oneriKokFrekansKullan; }
        }
        private bool _oneriBilesikKelimeKullan = true;

        public bool OneriBilesikKelimeKullan
        {
            get { return _oneriBilesikKelimeKullan; }
        }
        private bool _cepKullan = true;

        public bool CepKullan
        {
            get { return _cepKullan; }
        }

        private string[] dilAyarlari = new string[3];

        public string[] DilAyarlari
        {
            get { return dilAyarlari; }
        }
        

        public ZemberekAyarlari() {
            try 
            {
                KonfigurasyonOku();
            } 
            catch (System.IO.IOException e) 
            {
#if log
                logger.Warn("Konfigurasyon kayıtlarına erisilemiyor! varsayilan degerler kullanilacak. Hata : "+e.Message);
#endif
            }
        }


        private void KonfigurasyonOku() 
        {
            try 
            {
                NameValueCollection settings = AyarlariAl();
                _oneriDeasciifierKullan = DogruYanlisOku("oneri.deasciifierKullan");
                _oneriKokFrekansKullan = DogruYanlisOku("oneri.kokFrekansKullan");
                _oneriBilesikKelimeKullan = DogruYanlisOku("oneri.bilesikKelimeKullan");
                oneriMax = Int32.Parse(settings["oneri.max"]);
                _cepKullan = DogruYanlisOku("denetleme.CepKullan");
                dilAyarlari[0] = settings["dilAyarlari.KutuphaneAdi"];
                dilAyarlari[1] = settings["dilAyarlari.SinifAdi"];
                dilAyarlari[2] = settings["dilAyarlari.KaynakDizini"];
            } 
            catch (FormatException e) 
            {
#if log
                logger.Error("property erisim hatasi!! Muhtemel Tip donusum problemi.. varsayilan parametreler kullanilacak "+e.Message);
#endif
            } 
            catch (Exception e) 
            {
#if log
                logger.Error("property erisim hatasi!! propety yer almiyor, ya da adi yanlis yazilmis olabilir. varsayilan konfigurasyon kullanilacak."+e.Message);
#endif
            }
        }

        private NameValueCollection AyarlariAl()
        {
            //#if mono
            //            NameValueCollection settings = ConfigurationSettings.AppSettings;
            //#endif
            //#if DotNet
            NameValueCollection settings = ConfigurationManager.AppSettings;
            //#endif
            return settings;
        }

        private bool DogruYanlisOku(String anahtar)
        {
            return bool.Parse(AyarlariAl()[anahtar]);
        }
    }
}
