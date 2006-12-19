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
using System.Reflection;
using log4net;


namespace net.zemberek.yapi
{
    public class DilBilgisiUretici
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static String TR_SINIF = "net.zemberek.tr.yapi.TurkiyeTurkcesi";
        public static  String TM_SINIF = "net.zemberek.tm.yapi.Turkmence";


        //TODO : Dosya üretim mekanizmalarını ayırmakta fayda var. (@tankut)
        //public static void main(String[] args) 
        //{
        //    if (args.Length > 0) 
        //    {
        //        String dilAdi = args[0].ToLower().Trim();
        //        uret(dilAdi);
        //        Environment.Exit(0);
        //    } 
        //    else 
        //    {
        //        logger.Fatal("Dil adi girmelisiniz (tr,tm,az gibi)");
        //        Environment.Exit(1);
        //    }
        //}

        //public static void uret(String dilAdi) {
        //    DilAyarlari dilAyari = null;

        //    if (dilAdi.Equals("tr"))
        //        dilAyari = dilAyarUret(TR_SINIF);
        //    else if (dilAdi.Equals("tm"))
        //        dilAyari = dilAyarUret(TM_SINIF);
        //    else 
        //    {
        //        logger.Fatal("Dil sinifi bulunamiyor : " + dilAdi);
        //        Environment.Exit(1);
        //    }

        //    new TurkceDilBilgisi(dilAyari).ikiliKokDosyasiUret();


        //}

        public static DilAyarlari dilAyarUret(String sinifadi) {
            Type c = null;
            try 
            {
                c = Type.GetType(sinifadi);
            } 
            catch (ReflectionTypeLoadException e) 
            {
                logger.Fatal("Dil ayarı sinifi yüklenemiyor : " + sinifadi +" Hata : "+e.Message);
                Environment.Exit(1);
            }
            try 
            {
                //TODO buraya dikkat
                return (DilAyarlari)c.GetConstructor(new Type[] { }).Invoke(new object[] { });
            } 
            catch (Exception e) 
            {
                logger.Fatal("Dil ayarı sinifi yüklenemiyor : " + sinifadi +" Hata : "+e.Message);
                Environment.Exit(1);
            }
            return null;
        }
    }
}
