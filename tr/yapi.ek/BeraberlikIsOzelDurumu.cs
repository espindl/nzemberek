using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;
using net.zemberek.yapi.ek;
using net.zemberek.islemler.cozumleme;

namespace net.zemberek.tr.yapi.ek
{
/**
 * -is fiil eki tek heceli koklere eklendiginde "-yis" cok heceli koke eklendiginde ise
 * "-is" seklinde olusur. "tak-IS-mak" "ye-yis-mek"
 * User: ahmet
 * Date: Sep 11, 2005
 */
public class BeraberlikIsOzelDurumu : EkOzelDurumu {

    public override HarfDizisi cozumlemeIcinUret(Kelime kelime, HarfDizisi giris, HarfDizisiKiyaslayici kiyaslayici) {
        if(kelime.icerik().sesliSayisi()<2)
          return ekUretici.cozumlemeIcinEkUret(kelime.icerik(), giris, uretimBilesenleri());
        else
          return null;
    }

    public override HarfDizisi olusumIcinUret(Kelime kelime, Ek sonrakiEk)
    {
        return cozumlemeIcinUret(kelime, null, null);
    }
}
}
