using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using net.zemberek.yapi.ek;

namespace net.zemberek.yapi
{
    interface DilAyarlari
    {
        CultureInfo locale();
        
        Type alfabeSinifi();

        Type ekYoneticiSinifi();

        Type heceBulucuSinifi();

        Type kokOzelDurumBilgisiSinifi();

        Type cozumlemeYardimcisiSinifi();

        String[] duzYaziKokDosyalari();

        /**
         * Duz yazi ile belirtilen kok dosyalarinda kokun tipinin hangi kelime ile ifade
         * edilecegi bir Map icerisinde belirtilir.
         * @return kisaAd-tip ikililerini tasiyan Map
         */
        IDictionary<String, KelimeTipi> kokTipiAdlari();

        EkUretici ekUretici(Alfabe alfabe);

        EkOzelDurumUretici ekOzelDurumUretici(Alfabe alfabe);

        IDictionary<KelimeTipi, String> baslangiEkAdlari();

        String ad();
    }
}
