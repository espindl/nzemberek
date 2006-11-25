using System;
using net.zemberek.islemler.cozumleme;

namespace net.zemberek.yapi.ek
{
    public class OldurganEkOzelDurumu : EkOzelDurumu    {

    private HarfDizisi T;

    public OldurganEkOzelDurumu(Alfabe alfabe) {
        T = new HarfDizisi("t",alfabe);
    }

    public override HarfDizisi cozumlemeIcinUret(Kelime kelime, HarfDizisi giris, HarfDizisiKiyaslayici kiyaslayici) {
        TurkceHarf son = kelime.sonHarf();
        if (son.sesliMi() || ((son.charDeger()=='r') || son.charDeger()==('l'))
                && kelime.icerik().sesliSayisi() > 1) {
            return T;
        }
        return null;
    }

    public HarfDizisi olusumIcinUret(Kelime kelime, Ek sonrakiEk) {
        return cozumlemeIcinUret(kelime, null, null);
    }
}

}
