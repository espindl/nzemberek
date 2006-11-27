using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;


namespace net.zemberek.bilgi.kokler
{
    interface Sozluk
    {
        /**
     * str seklinde yazilan tum kelime koklerini dondurur. str kokun istisna hali de olabilir.
     *
     * @param str
     * @return kok listesi.
     */
        List<Kok> kokBul(String str);


        Kok kokBul(String str, KelimeTipi tip);

        /**
         * sozluk icindeki normal ya da kok ozel durumu seklindeki tum kok iceriklerini bir
         * Koleksiyon nesnesi olarak dondurur.
         *
         * @return tum kokleri iceren Collection nesnesi
         */
        ICollection<Kok> tumKokler();

        /**
         * sozluge kok ekler.
         *
         * @param kok
         */
        void ekle(Kok kok);

        /**
         * Bu metod kÃ¶kbulucu fabrikasÄ± elde etmek iÃ§in kullanÄ±lÄ±r. GerÃ§ekleyen sÃ¶zlÃ¼k sÄ±nÄ±flarÄ± bu
         * metodda kendi KÃ¶k bulucu fabrikasÄ± gerÃ§eklemelerinin bir instancesini geri dÃ¶ndÃ¼rmelidirler.
         *
         * @return SÃ¶zlÃ¼k
         * @see AgacSozluk
         */
        KokBulucuUretici getKokBulucuFactory();

        KokAgaci getAgac();
    }
}
