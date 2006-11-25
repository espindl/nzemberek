using System;

using net.zemberek.yapi;

namespace net.zemberek.yapi.ek
{
    public interface EkYonetici
    {
        /**
     * istenilen isimli ek'i dondurur
     */
        Ek ek(String ad);

        /**
         * Kok nesnesinin tipine gore gelebilecek ilk ek'i dondurur.
         * Baslangic ekleri bilgisi dil tarafindan belirlenir.
         * @return ilk Ek, eger kok tipi baslangic ekleri <baslangicEkleri>
         *         haritasinda belirtilmemisse BOS_EK doner.
         */
        Ek ilkEkBelirle(Kok kok);
    }
}
