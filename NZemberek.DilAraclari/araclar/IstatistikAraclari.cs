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

namespace net.zemberek.araclar
{
    /**
 * Bazi basit  yuzde hesaplamalarï¿½nda kullannilan fonksiyonlar. 
 * TODO: istatistik paketine alinmasi dusunulebilir.
 * @author MDA
 */
    class IstatistikAraclari
    {
        //public static DecimalFormat df = new DecimalFormat("#0.000");
        //public static DecimalFormat df2 = new DecimalFormat("#0.00000");
        public static string df = "#0.000";
        public static string df2 = "#0.00000";

        /**
         * Verilen girisin toplamın yüzde kaçını oluşturduğunu döndürür.
         * @param input
         * @param total
         * @return input, toplamin %kaci ise.
         * Eğer total 0 ise -1 
         * 
         */
        public static double yuzdeHesapla(long input, long total)
        {
            if (total == 0) return -1;
            return (double)(input * 100) / total;
        }

        /**
         * Yuzde hesaplamasının aynısı, sadece formatlı String olarak döndürür.
         * @param input : giriş 
         * @param total : toplam
         * @return
         */
        public static String yuzdeHesaplaStr(long input, long total)
        {
            if (total == 0) return "0";
            return string.Format(df,(double)(input * 100) / total);
        }

        /**
         * Gene yuzde hesabı. ama bu sefer virgülden sonra 5 basamak hassasiyet
         * @return
         */
        public static String onbindeHesaplaStr(long input, long total)
        {
            if (total == 0) return "0";
            return string.Format(df2,(double)(input * 100) / total);
        }
    }
}
