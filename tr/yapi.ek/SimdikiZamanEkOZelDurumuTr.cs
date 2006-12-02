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
 * Ekler icin simdiki zaman ozel durumu. Simdiki zaman eki sesli ile biten bir koke (ya da eke)
 * eklendiginde son sesliyi dusurur.
 * <p/>
 * User: ahmet
 * Date: Aug 15, 2005
 */
public class SimdikiZamanEkOzelDurumuTr : EkOzelDurumu {

    private Alfabe alfabe;
    private TurkceSesliUretici sesliUretci;

    public SimdikiZamanEkOzelDurumuTr(Alfabe alfabe) {
        this.alfabe = alfabe;
        sesliUretci = new TurkceSesliUretici(alfabe);
    }

    public override  HarfDizisi cozumlemeIcinUret(Kelime kelime, HarfDizisi giris, HarfDizisiKiyaslayici kiyaslayici) {
        if (kiyaslayici == null) return null;
        // eki olustur.
        HarfDizisi ek = ekUretici.cozumlemeIcinEkUret(kelime.icerik(), giris, uretimBilesenleri());
        TurkceHarf ekHarfi = sesliUretci.sesliBelirleIU(kelime.icerik());
        HarfDizisi olusum = new HarfDizisi("yor", alfabe);
        olusum.ekle(0, ekHarfi);
        int harfPozisyonu = kelime.boy() + ek.Length;
        if (kiyaslayici.aradanKiyasla(giris, olusum, harfPozisyonu))
            return ek;
        return null;
    }

    public HarfDizisi olusumIcinUret(Kelime kelime, Ek sonrakiEk){
        if(sonrakiEk.ad().Equals(TurkceEkAdlari.FIIL_SIMDIKIZAMAN_IYOR))
          return ekUretici.olusumIcinEkUret(kelime.icerik(),sonrakiEk, uretimBilesenleri());
        return null;
    }

}
}
