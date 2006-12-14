// V 0.1
using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using Iesi.Collections.Generic;
using net.zemberek.yapi;
using net.zemberek.yapi.ek;

namespace net.zemberek.tr.yapi.ek
{
 public class EkUreticiTr : EkUretici {

    private TurkceSesliUretici sesliUretici;
     public TurkceHarf HARF_a;
     public TurkceHarf HARF_e;
     public TurkceHarf HARF_i;
     public TurkceHarf HARF_ii;
     public TurkceHarf HARF_u;
     public TurkceHarf HARF_uu;

    public EkUreticiTr(Alfabe alfabe) {
         this.sesliUretici = new TurkceSesliUretici(alfabe);
         HARF_a = alfabe.harf('a');
         HARF_e = alfabe.harf('e');
         HARF_i = alfabe.harf('i');
         HARF_ii = alfabe.harf(Alfabe.CHAR_ii);
         HARF_u = alfabe.harf('u');
         HARF_uu = alfabe.harf(Alfabe.CHAR_uu);
    }

     public HarfDizisi cozumlemeIcinEkUret(HarfDizisi ulanacak, HarfDizisi giris, List<EkUretimBileseni> bilesenler)
     {
        HarfDizisi sonuc = new HarfDizisi(4);
        TurkceHarf sonSesli = ulanacak.sonSesli();
        for (int i = 0; i < bilesenler.Count; i++) {
            EkUretimBileseni ekUretimBileseni = bilesenler[i];
            TurkceHarf harf = ekUretimBileseni.harf();
            switch (ekUretimBileseni.kural()) {
                case UretimKurali.HARF:
                    sonuc.ekle(harf);
                    break;
                case UretimKurali.KAYNASTIR:
                    if (ulanacak.sonHarf().sesliMi())
                        sonuc.ekle(harf);
                    break;
                case UretimKurali.SERTLESTIR:
                    if (ulanacak.sonHarf().sertMi())
                        sonuc.ekle(harf.sertDonusum());
                   else
                        sonuc.ekle(harf);
                    break;
                case UretimKurali.SESLI_AE:
                    if (i == 0 && ulanacak.sonHarf().sesliMi())
                        break;
                    else {
                        sonSesli = sesliUretici.sesliBelirleAE(sonSesli);
                        sonuc.ekle(sonSesli);
                    }
                    break;
                case UretimKurali.SESLI_IU:
                    if (i == 0 && ulanacak.sonHarf().sesliMi())
                        break;
                    else {
                        sonSesli = sesliUretici.sesliBelirleIU(sonSesli);
                        sonuc.ekle(sonSesli);
                    }
                    break;
            }
        }
        return sonuc;
    }

     public HarfDizisi olusumIcinEkUret(HarfDizisi ulanacak, Ek sonrakiEk, List<EkUretimBileseni> bilesenler)
     {
        //TODO: gecici olarak bu sekilde
        return cozumlemeIcinEkUret(ulanacak, null, bilesenler);
    }

    public Set<TurkceHarf> olasiBaslangicHarfleri(List<EkUretimBileseni> bilesenler) {
        Set<TurkceHarf> kume = new HashedSet<TurkceHarf>();//TOREMEMBER 4
        for (int i=0; i< bilesenler.Count; i++) {
            EkUretimBileseni bilesen = bilesenler[i];
            TurkceHarf harf = bilesen.harf();
            switch (bilesen.kural()) {
                case UretimKurali.HARF:
                    kume.Add(harf);
                    return kume;
                case UretimKurali.KAYNASTIR:
                    kume.Add(harf);
                    break;
                case UretimKurali.SERTLESTIR:
                    kume.Add(harf);
                    kume.Add(harf.sertDonusum());
                    return kume;
                case UretimKurali.SESLI_AE:
                      kume.Add(HARF_a);
                      kume.Add(HARF_e);
                      if (i > 0)
                          return kume;
                      else
                          break;
                case UretimKurali.SESLI_IU:
                    kume.Add(HARF_i);
                    kume.Add(HARF_u);
                      kume.Add(HARF_ii);
                      kume.Add(HARF_uu);
                      if (i > 0)
                          return kume;
                      else
                          break;                
            }
        }
        return kume;
    }


}
}
