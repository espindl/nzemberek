/* ***** BEGIN LICENSE BLOCK *****
 * Version: MPL 1.1
 *
 * The contents of this file are subject to the Mozilla Public License Version
 * 1.1 (the "License"); you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * The Original Code is Zemberek Doğal Dil İşleme Kütüphanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Akın, Mehmet D. Akın.
 * Portions created by the Initial Developer are Copyright (C) 2006
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Mert Derman
 *   Tankut Tekeli
 * ***** END LICENSE BLOCK ***** */

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
                    if (ulanacak.sonHarf().Sesli)
                        sonuc.ekle(harf);
                    break;
                case UretimKurali.SERTLESTIR:
                    if (ulanacak.sonHarf().Sert)
                        sonuc.ekle(harf.SertDonusum);
                   else
                        sonuc.ekle(harf);
                    break;
                case UretimKurali.SESLI_AE:
                    if (i == 0 && ulanacak.sonHarf().Sesli)
                        break;
                    else {
                        sonSesli = sesliUretici.sesliBelirleAE(sonSesli);
                        sonuc.ekle(sonSesli);
                    }
                    break;
                case UretimKurali.SESLI_IU:
                    if (i == 0 && ulanacak.sonHarf().Sesli)
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
                    kume.Add(harf.SertDonusum);
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
