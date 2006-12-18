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
