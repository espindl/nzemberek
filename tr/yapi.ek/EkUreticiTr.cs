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

     internal static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private TurkceSesliUretici sesliUretici;
    private Alfabe alfabe;

    public EkUreticiTr(Alfabe alfabe) {
        this.sesliUretici = new TurkceSesliUretici(alfabe);
        this.alfabe = alfabe;
    }

    public HarfDizisi cozumlemeIcinEkUret(HarfDizisi ulanacak, HarfDizisi giris, List<EkUretimBileseni> bilesenler) {
        HarfDizisi sonuc = new HarfDizisi();
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

    public HarfDizisi olusumIcinEkUret(HarfDizisi ulanacak, Ek sonrakiEk, List<EkUretimBileseni> bilesenler) {
        //TODO: gecici olarak bu sekilde
        return cozumlemeIcinEkUret(ulanacak, null, bilesenler);
    }

    public Set<TurkceHarf> olasiBaslangicHarfleri(List<EkUretimBileseni> bilesenler) {
        Set<TurkceHarf> kume = new HashedSet<TurkceHarf>();//TOREMEMBER 4
        for (int i=0; i< bilesenler.Count; i++) {
            EkUretimBileseni bilesen = bilesenler[i];
            switch (bilesen.kural()) {
                case UretimKurali.HARF:
                    kume.Add(bilesen.harf());
                    return kume;
                case UretimKurali.KAYNASTIR:
                    kume.Add(bilesen.harf());
                    break;
                case UretimKurali.SERTLESTIR:
                    kume.Add(bilesen.harf());
                    kume.Add(bilesen.harf().sertDonusum());
                    return kume;
                case UretimKurali.SESLI_AE:
                      kume.Add(alfabe.harf('a'));
                      kume.Add(alfabe.harf('e'));
                      if (i > 0)
                          return kume;
                      else
                          break;
                case UretimKurali.SESLI_IU:
                      kume.Add(alfabe.harf('i'));
                      kume.Add(alfabe.harf('u'));
                      kume.Add(alfabe.harf(Alfabe.CHAR_ii));
                      kume.Add(alfabe.harf(Alfabe.CHAR_uu));
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
