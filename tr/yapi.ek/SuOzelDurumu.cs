// V 0.1
using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi.ek;
using net.zemberek.yapi;
using net.zemberek.islemler.cozumleme;
using net.zemberek.tr.yapi.kok;

namespace net.zemberek.tr.yapi.ek
{

/**
 * 'su' kokune kaynastirma iceren cesitli ekler eklendiginde normal kaynastirma harfi yerine
 * 'y' harfi kullanilir.
 */
public class SuOzelDurumu : EkOzelDurumu {

    public override HarfDizisi cozumlemeIcinUret(Kelime kelime, HarfDizisi giris, HarfDizisiKiyaslayici kiyaslayici) {
        if(kelime.gercekEkYok() && kelime.kok().ozelDurumIceriyormu(TurkceKokOzelDurumTipi.SU_OZEL_DURUMU))
           return ekUretici.cozumlemeIcinEkUret(kelime.icerik(), giris, uretimBilesenleri());
        return null;
    }

    public override HarfDizisi olusumIcinUret(Kelime kelime, Ek sonrakiEk)
    {
        return cozumlemeIcinUret(kelime, null, null);
    }
}

}
