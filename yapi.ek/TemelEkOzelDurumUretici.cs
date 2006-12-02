using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace net.zemberek.yapi.ek
{
    public class TemelEkOzelDurumUretici : EkOzelDurumUretici  
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);		

        protected Alfabe alfabe;

        public EkOzelDurumu uret(String ad) {
            if (!mevcut((EkOzelDurumTipi[])System.Enum.GetValues(typeof(TemelEkOzelDurumuTipi)), ad))
                return null;
            else
                switch (ad) {
                    case "SON_HARF_YUMUSAMA":
                        return new SonHarfYumusamaOzelDurumu();
                    case "OLDURGAN":
                        return new OldurganEkOzelDurumu(alfabe);
                    case "ON_EK":
                        return new OnEkOzelDurumu();
                    case "ZAMAN_KI":
                        return new ZamanKiOzelDurumu();
                    default:
                        return null;
                }
        }

        /**
         * efektif olmayan bir tip denetimi.
         *
         * @param tipler
         * @param ad
         * @return eger kisaAd ile belirtilen tip var ise true.
         */
        internal bool mevcut(EkOzelDurumTipi[] tipler, String ad) {
            foreach (EkOzelDurumTipi tip in tipler) {
                if (tip.Ad.Equals(ad))
                    return true;
            }
            return false;
        }

    }

    public enum TemelEkOzelDurumuTipi //: EkOzelDurumTipi {
    {
        SON_HARF_YUMUSAMA,
        OLDURGAN,
        ON_EK,
        ZAMAN_KI,

    }
}
