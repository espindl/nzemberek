using System;
using net.zemberek.islemler.cozumleme;

namespace net.zemberek.yapi.ek
{
    public class SonHarfYumusamaOzelDurumu : EkOzelDurumu
    {
        public override HarfDizisi cozumlemeIcinUret(Kelime kelime, HarfDizisi giris, HarfDizisiKiyaslayici kiyaslayici)
        {
            HarfDizisi ek = ekUretici.cozumlemeIcinEkUret(kelime.icerik(), giris, _uretimBilesenleri);
            // XXXX gibi Turkce harf tasimayan stringler icin koruma.
            // TODO: Daha dogru bir yontem bulunmali.
            if (ek == null)
            {
                return null;
            }
            int harfPozisyonu = kelime.boy() + ek.length();
            if (giris.harf(harfPozisyonu).sesliMi())
                return ek;
            return null;
        }

        public HarfDizisi olusumIcinUret(Kelime kelime, Ek sonrakiEk)
        {
            return null;
        }
    }
}
