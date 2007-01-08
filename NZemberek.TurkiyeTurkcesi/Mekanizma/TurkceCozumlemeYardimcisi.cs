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

using System;
using System.Collections.Generic;
using System.Text;
using NZemberek.Cekirdek.Yapi;
using NZemberek.Cekirdek.Mekanizma;
using NZemberek.Cekirdek.Mekanizma.Cozumleme;
using NZemberek.TrTurkcesi.Yapi;


namespace NZemberek.TrTurkcesi.Mekanizma
{
/**
 * Bu sinif Turkiye Turkcesine ozel cesitli islemleri icerir.
 * User: ahmet
 * Date: Sep 11, 2005
 */
public class TurkceCozumlemeYardimcisi : ICozumlemeYardimcisi {

    private Alfabe alfabe;
  //  private IEkYonetici ekYonetici;
    private IDenetlemeCebi cep;


    public TurkceCozumlemeYardimcisi(Alfabe alfabe,
                                     IDenetlemeCebi cep) {
        this.alfabe = alfabe;
        this.cep = cep;
    }

    public void KelimeBicimlendir(Kelime kelime) {
        Kok kok = kelime.Kok;
        HarfDizisi olusan = kelime.Icerik;
        if (kok.Tip.Equals(KelimeTipi.KISALTMA)) {
            //cozumleme sirasinda eklenmis Harf varsa onlari Sil.
            int silinecek = kok.Icerik.Length;
            if (kok.OzelDurumIceriyormu(TurkceKokOzelDurumYonetici.KISALTMA_SON_SESSIZ))
                silinecek += 2;
            if (kok.OzelDurumIceriyormu(TurkceKokOzelDurumYonetici.KISALTMA_SON_SESLI))
                silinecek++;
            //kelimenin olusan kismindan kokun icereigini Sil.
            olusan.HarfSil(0, silinecek);
            //simdi kokun orjinal halini Ekle.
            olusan.Ekle(0, new HarfDizisi(kok.Asil, alfabe));

            if (olusan.Boy == kok.Asil.Length)
                return;
            //eger gerekiyorsa kesme isareti Koy.
            if (!olusan.Harf(kok.Asil.Length - 1).Equals(alfabe.Harf('.')))
                olusan.Ekle(kok.Asil.Length, alfabe.Harf('\''));

        } else if (kok.Tip == KelimeTipi.OZEL) {
            olusan.HarfDegistir(0, alfabe.BuyukHarf(olusan.IlkHarf()));
            if (kok.OzelDurumIceriyormu(TurkceKokOzelDurumYonetici.KESMESIZ))
                return;
            List<Ek> ekler = kelime.Ekler;
            if (ekler.Count > 1) {
                Ek ek = (Ek) ekler[1];
                if (ek.IyelikEki ||ek.HalEki) {
                    int kesmePozisyonu = kok.Icerik.Length;
                    olusan.Ekle(kesmePozisyonu,alfabe.Harf('\''));
                }
            }
        }
        // ozel ic karakter iceren kokler icin bicimleme
/*        if (kok.ozelDurumlar().contains(TurkceKokOzelDurumlari.OZEL_IC_KARAKTER)) {
            //olusan ksimdan koku Sil
            int silinecek = kok.icerik().length();
            olusan.HarfSil(0, silinecek);
            //simdi kokun orjinal halini Ekle.
            olusan.Ekle(0, new HarfDizisi(kok.asil()));
        }*/
    }

    public bool KelimeBicimiDenetle(Kelime kelime, String giris) {
        if (giris.Length == 0) return false;
        Kok kok = kelime.Kok;
        if (kok.Tip.Equals(KelimeTipi.KISALTMA)) {
            // eger giriskokun orjinal hali ile baslamiyorsa hatali demektir.
            String _asil = kok.Asil;
            if (!giris.StartsWith(_asil))
                return false;
            if (giris.Equals(_asil))
                return true;
            //burada farkli kisaltma turleri icin kesme ve nokta isaretlerinin
            // dogru olup olmadigina bakiliyor.
            String kalan = giris.Substring(_asil.Length);
            if (_asil[_asil.Length - 1] == '.') {
                return kalan[0] != '\'';
            }
            return kalan[0] == '\'';
        } else if (kelime.Kok.Tip == KelimeTipi.OZEL) {
            if (Char.IsLower(giris[0]))
                return false;
            if (kelime.Kok.OzelDurumIceriyormu(TurkceKokOzelDurumYonetici.KESMESIZ))
                return true;
            List<Ek> ekler = kelime.Ekler;
            if (ekler.Count > 1) {
                Ek ek = (Ek) ekler[1];
                if (ek.IyelikEki || ek.HalEki) {
                    int kesmePozisyonu = kelime.Kok.Icerik.Length;
                    return kesmePozisyonu <= giris.Length && giris[kesmePozisyonu] == '\'';
                }
            }
        }
        // ozel ic karakter iceren kokler icin bicimleme
/*        if (kok.ozelDurumlar().contains(TurkceKokOzelDurumlari.OZEL_IC_KARAKTER)) {&
            //olusan ksimdan koku Sil
            String _asil = kok.asil();
            if (!giris.startsWith(_asil))
              return false;
        }*/
        return true;
    }

    public bool KokGirisDegismiVarsaUygula(Kok kok, HarfDizisi kokDizi, HarfDizisi girisDizi) {
        //turkce'de sadece kisaltmalarda bu metoda ihtiyacimiz var.
        char c = kok.KisaltmaSonSeslisi;
        if (girisDizi.Boy == 0) return false;
        if (kok.Tip.Equals(KelimeTipi.KISALTMA) && c != 0) {
            TurkceHarf h = alfabe.Harf(c);
            //toleransli cozumleyicide kok giristen daha uzun olabiliyor.
            // o nedenle asagidaki kontrolun yapilmasi gerekiyor.
            int kokBoyu = kok.Icerik.Length;
            if (kokBoyu <= girisDizi.Boy)
                girisDizi.Ekle(kokBoyu, h);
            else
                girisDizi.Ekle(h);
            kokDizi.Ekle(h);
            if (kok.OzelDurumIceriyormu(TurkceKokOzelDurumYonetici.KISALTMA_SON_SESSIZ)) {
                //gene toleransli cozumleyicinin hata vermemesi icin asagidaki kontrole ihtiyacimiz var
                if (kokBoyu < girisDizi.Boy)
                    girisDizi.Ekle(kokBoyu + 1, alfabe.Harf('b'));
                else
                    girisDizi.Ekle( alfabe.Harf('b'));
                kokDizi.Ekle( alfabe.Harf('b'));
            }
            return true;
        }
        return false;
    }

    public bool CepteAra(String str) {
        return false;
       // return cep != null && cep.Kontrol(str);
    }
}

}
