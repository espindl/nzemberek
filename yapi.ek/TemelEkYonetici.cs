using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace net.zemberek.yapi.ek
{
    public class TemelEkYonetici : EkYonetici
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);		

        protected Alfabe alfabe;

        public static Ek BOS_EK = new Ek("BOS_EK");
        protected IDictionary<String, Ek> ekler;
        protected IDictionary<KelimeTipi, Ek> baslangicEkleri = new Dictionary<KelimeTipi, Ek>();


        public TemelEkYonetici(Alfabe alfabe,
                               String dosya,
                               EkUretici ekUretici,
                               EkOzelDurumUretici ozelDurumUretici,
                               IDictionary<KelimeTipi, String> baslangicEkMap) {
            this.alfabe = alfabe;
            DateTime start = System.DateTime.Now;// currentTimeMillis();

            XmlEkOkuyucu okuyucu = new XmlEkOkuyucu(
                    dosya,
                    ekUretici,
                    ozelDurumUretici,
                    alfabe);
            okuyucu.xmlOku();
            ekler = okuyucu.getEkler();
            foreach (KelimeTipi tip in baslangicEkMap.Keys) {
                Ek ek = ekler[baslangicEkMap[tip]];
                if (ek != null)
                    baslangicEkleri.Add(tip, ek);
                else
                    logger.Warn(tip + " tipi icin baslangic eki " + baslangicEkMap[tip] + " bulunamiyor!");
            }
            DateTime end = System.DateTime.Now;
            TimeSpan ts = end.Subtract(start);
            logger.Info("ek okuma ve olusum suresii: " + ts.Milliseconds + "ms.");
        }

        /**
         * adi verilen Ek nesnesini bulur. Eger ek yok ise null doner.
         *
         * @param ekId - ek adi
         * @return istenen Ek nesnesi.
         */
        public Ek ek(String ekId) {
            Ek ek = ekler[ekId];
            if (ek == null)
                logger.Error("Ek bulunamiyor!" + ekId);
            return ekler[ekId];
        }

        /**
         * Kok nesnesinin tipine gore gelebilecek ilk ek'i dondurur.
         * Baslangic ekleri bilgisi dil tarafindan belirlenir.
         *
         * @param kok
         * @return ilk Ek, eger kok tipi baslangic ekleri <baslangicEkleri>
         *         haritasinda belirtilmemisse BOS_EK doner.
         */
        public Ek ilkEkBelirle(Kok kok) {
            Ek baslangicEki = baslangicEkleri[kok.tip()];
            if (baslangicEki != null)
                return baslangicEki;
            else
                return BOS_EK;
        }
    }
}
