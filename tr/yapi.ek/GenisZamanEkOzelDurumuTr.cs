using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;
using net.zemberek.yapi.ek;
using net.zemberek.islemler.cozumleme;
using net.zemberek.tr.yapi.kok;

namespace net.zemberek.tr.yapi.ek
{
/**
 * Genis zaman ozel durumu. Normalde fiillere genis zaman eki eklenedeginde "ir, ur, Ur, Ir"
 * olusur. Ancak
 * a)fiil tek heceli (bunun da istisnalari var)
 * b)fiil tek heceli fiil eklenmesi ile olusmus bilesik fiilse (addetmek gibi)
 * "ar er" olusur
 * <p/>
 * User: ahmet
 * Date: Aug 15, 2005
 */
public class GenisZamanEkOzelDurumuTr : EkOzelDurumu {

    public GenisZamanEkOzelDurumuTr() {
    }

    public override HarfDizisi cozumlemeIcinUret(Kelime kelime, HarfDizisi giris, HarfDizisiKiyaslayici kiyaslayici) {
        if (kelime.sonEk().ad().Equals(TurkceEkAdlari.FIIL_KOK)
             && kelime.kok().ozelDurumIceriyormu(TurkceKokOzelDurumTipi.GENIS_ZAMAN))
            return ekUretici.cozumlemeIcinEkUret(kelime.icerik(), giris, uretimBilesenleri());
        else
            return null;
    }

    public HarfDizisi olusumIcinUret(Kelime kelime, Ek sonrakiEk) {
        return cozumlemeIcinUret(kelime, null, null);
    }
}
}
