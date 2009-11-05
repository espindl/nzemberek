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
using NZemberek.Cekirdek.Yapi;
using NZemberek.Cekirdek.Kolleksiyonlar;

namespace NZemberek.TrTurkcesi.Yapi
{
 public class EkUreticiTr : TemelEkUretici, IEkUretici {

    private TurkceSesliUretici sesliUretici;
     public TurkceHarf HARF_a;
     public TurkceHarf HARF_e;
     public TurkceHarf HARF_i;
     public TurkceHarf HARF_ii;
     public TurkceHarf HARF_u;
     public TurkceHarf HARF_uu;

    public EkUreticiTr(Alfabe alfabe) {
         this.sesliUretici = new TurkceSesliUretici(alfabe);
         HARF_a = alfabe.Harf('a');
         HARF_e = alfabe.Harf('e');
         HARF_i = alfabe.Harf('i');
         HARF_ii = alfabe.Harf(Alfabe.CHAR_ii);
         HARF_u = alfabe.Harf('u');
         HARF_uu = alfabe.Harf(Alfabe.CHAR_uu);
    }

     public override HarfDizisi CozumlemeIcinEkUret(HarfDizisi ulanacak, HarfDizisi giris, List<EkUretimBileseni> bilesenler)
     {
        HarfDizisi sonuc = new HarfDizisi(4);
        TurkceHarf sonSesli = ulanacak.SonSesli();
        for (int i = 0; i < bilesenler.Count; i++) {
            EkUretimBileseni ekUretimBileseni = bilesenler[i];
            TurkceHarf harf = ekUretimBileseni.Harf;
            switch (ekUretimBileseni.Kural.ToString()) {
                case "HARF":
                    sonuc.Ekle(harf);
                    break;
                case "KAYNASTIR":
                    if (ulanacak.SonHarf().Sesli)
                        sonuc.Ekle(harf);
                    break;
                case "SERTLESTIR":
                    if (ulanacak.SonHarf().Sert)
                        sonuc.Ekle(harf.SertDonusum);
                   else
                        sonuc.Ekle(harf);
                    break;
                case "YUMUSAT":
                    if (giris.Harf(ulanacak.Boy + sonuc.Boy + 1).Sesli)
                        sonuc.Ekle(harf.Yumusama);
                    else
                        sonuc.Ekle(harf);
                    break;
                case "SESLI_AE":
                    if (i == 0 && ulanacak.SonHarf().Sesli)
                        break;
                    else {
                        sonSesli = sesliUretici.sesliBelirleAE(sonSesli);
                        sonuc.Ekle(sonSesli);
                    }
                    break;
                case "SESLI_IU":
                    if (i == 0 && ulanacak.SonHarf().Sesli)
                        break;
                    else {
                        sonSesli = sesliUretici.sesliBelirleIU(sonSesli);
                        sonuc.Ekle(sonSesli);
                    }
                    break;
            }
        }
        return sonuc;
    }

     public new HarfDizisi OlusumIcinEkUret(HarfDizisi ulanacak, Ek sonrakiEk, List<EkUretimBileseni> bilesenler)
     {
        //TODO: gecici olarak bu sekilde
        return CozumlemeIcinEkUret(ulanacak, null, bilesenler);
    }

    public new HashSet<TurkceHarf> OlasiBaslangicHarfleri(List<EkUretimBileseni> bilesenler) {
        HashSet<TurkceHarf> kume = new HashSet<TurkceHarf>();//TOREMEMBER 4
        for (int i=0; i< bilesenler.Count; i++) {
            EkUretimBileseni bilesen = bilesenler[i];
            TurkceHarf harf = bilesen.Harf;
            switch (bilesen.Kural.ToString()) {
                case "HARF":
                    kume.Add(harf);
                    return kume;
                case "KAYNASTIR":
                    kume.Add(harf);
                    break;
                case "SERTLESTIR":
                    kume.Add(harf);
                    kume.Add(harf.SertDonusum);
                    return kume;
                case "SESLI_AE":
                      kume.Add(HARF_a);
                      kume.Add(HARF_e);
                      if (i > 0)
                          return kume;
                      else
                          break;
                case "SESLI_IU":
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
