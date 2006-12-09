using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.islemler.cozumleme;

namespace net.zemberek.yapi.ek
{
    public class ZamanKiOzelDurumu : EkOzelDurumu
    {
        public override HarfDizisi cozumlemeIcinUret(Kelime kelime, HarfDizisi giris, HarfDizisiKiyaslayici kiyaslayici)
        {
            TurkceHarf sonSesli = kelime.icerik().sonSesli();
            if (sonSesli.charDeger() == 'u' || sonSesli.charDeger() == Alfabe.CHAR_uu)
                return ekUretici.cozumlemeIcinEkUret(kelime.icerik(), giris, _uretimBilesenleri);
            else
                return null;
        }

        public override HarfDizisi olusumIcinUret(Kelime kelime, Ek sonrakiEk)
        {
            return cozumlemeIcinUret(kelime, null, null);
        }
    }
}
