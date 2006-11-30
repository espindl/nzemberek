using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi.ek;
using net.zemberek.yapi.kok;
using net.zemberek.islemler.cozumleme;
using net.zemberek.bilgi.kokler;

namespace net.zemberek.yapi
{
    interface DilBilgisi
    {
        /**
 * Dile ozel alfabe nesnesini dondurur.
 * @return alfabe.
 */
        Alfabe alfabe();

        /**
         * Dile ozgu ek oynetici nesnesini dondurur.
         * @return ekyonetici
         */
        EkYonetici ekler();

        /**
         * Kok bilgilerine ve kok secicilere erisimi saglayan
         * dile ozel Sozluk nesnesini dondurur.
         * @return sozluk
         */
        Sozluk kokler();

        /**
         * Dile ozgu kok ozel durumu bilgilerini tasiyan nesneyi dondurur.
         * @return ozeldurumbilgisi
         */
        KokOzelDurumBilgisi kokOzelDurumlari();

        /**
         * eger varsa dile ozgu hece bulma nesnesi.
         * @return hecebulma nesnesi
         */
        HeceBulucu heceBulucu();

        /**
         * dile ozgu cozumleme yardimcisi nesnesi. bu nesne cozumleme sirasinda kullanilan
         * cesitli on ve art isleme, cep mekanizmalarini tasir.
         * @return cozumleme yardimcisi
         */
        CozumlemeYardimcisi cozumlemeYardimcisi();
    }
}
