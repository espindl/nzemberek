using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;
using net.zemberek.yapi;
namespace net.zemberek.yapi.ek
{

    /// <summary> ek uretim kuralinin islenmesinde kullanilan sinif icin ortak arayuz.
    /// Her lehce kendi uretim sinifini kullanir.
    /// User: ahmet
    /// Date: Aug 22, 2005
    /// </summary>
    public interface EkUretici
    {
        /**
         * Kelime Cozumleme islemi icin ek uretimi.
         * @param ulanacak
         * @param giris
         * @param bilesenler
         * @return uretilen ek, HarfDizisi cinsinden.
         */
        HarfDizisi cozumlemeIcinEkUret(HarfDizisi ulanacak,
                                       HarfDizisi giris,
                                       List<EkUretimBileseni> bilesenler);

        /**
         * Kelime uretimi icin ek uretimi.
         * @param ulanacak
         * @param sonrakiEk
         * @param bilesenler
         * @return uretilen ek, HarfDizisi cinsinden.
         */
        HarfDizisi olusumIcinEkUret(HarfDizisi ulanacak,
                                    Ek sonrakiEk,
                                    List<EkUretimBileseni> bilesenler);

        /**
         * Ek bilesenlerini kullarak bir ekin hangi harflerle baslayacagini kestirip sonuclari
         * bir set icerisinde dondurur.
         * @param bilesenler
         * @return olasi baslangic harfleri bir Set icerisinde.
         */
        Set<TurkceHarf> olasiBaslangicHarfleri(List<EkUretimBileseni> bilesenler);
    }


    /**
 * Turk dilleri icin cesitli uretim kurallarini belirler. Bazi kurallar sadece belli dillerde
 * kullanilir.
 */
    public enum EkUretimKurali
    {
        SESLI_AE,
        SESLI_AA,
        SESLI_IU,
        SESSIZ_Y,
        SERTLESTIR,
        KAYNASTIR,
        HARF
    }

}