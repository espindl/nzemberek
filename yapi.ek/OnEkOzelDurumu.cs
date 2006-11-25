using System;
using net.zemberek.islemler.cozumleme;

namespace net.zemberek.yapi.ek
{
    public class OnEkOzelDurumu : EkOzelDurumu
    {
        public override HarfDizisi cozumlemeIcinUret(Kelime kelime, HarfDizisi giris, HarfDizisiKiyaslayici kiyaslayici)
        {
            if (this.onEkler.Contains(kelime.sonEk()))
                return ekUretici.cozumlemeIcinEkUret(kelime.icerik(), giris, _uretimBilesenleri);
            else
                return null;
        }

        public HarfDizisi olusumIcinUret(Kelime kelime, Ek sonrakiEk)
        {
            return cozumlemeIcinUret(kelime, null, null);
        }
    }
}
