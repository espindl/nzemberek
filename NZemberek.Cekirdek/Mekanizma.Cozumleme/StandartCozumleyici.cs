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
using log4net;
using NZemberek.Cekirdek.Yapi;
using NZemberek.Cekirdek.KokSozlugu;
using NZemberek.Cekirdek.Kolleksiyonlar;


namespace NZemberek.Cekirdek.Mekanizma.Cozumleme
{
/**
 * User: ahmet
 * Date: Jan 9, 2005
 */
public class StandartCozumleyici : KelimeCozumleyici {

    private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);		
    private IKokBulucu kokBulucu;
    private HarfDizisiKiyaslayici harfDizisiKiyaslayici;
    private EkYonetici ekYonetici;
    private Alfabe alfabe;
    private CozumlemeYardimcisi yardimci;

    public StandartCozumleyici(IKokBulucu kokBulucu,
                               HarfDizisiKiyaslayici kiyaslayci,
                               Alfabe alfabe,
                               EkYonetici ekYonetici,
                               CozumlemeYardimcisi yardimci) {
        this.kokBulucu = kokBulucu;
        this.harfDizisiKiyaslayici = kiyaslayci;
        this.ekYonetici = ekYonetici;
        this.alfabe = alfabe;
        this.yardimci = yardimci;
    }


    public bool denetle(String strGiris) {
        return yardimci.cepteAra(strGiris) || (cozumle(strGiris, false).Length == 1);
    }

    public Kelime[] cozumle(String strGiris) {
        return cozumle(strGiris, true);
    }

    /**
     * eger hepsiniCozumle=true ise dogru olabilecek tum kok ve ek kombinasyonlarini
     * dondurur.
     * Eger flag false ise ilk dogru cozumu tek elemanli dizi seklinde
     * dondurur.bu yontem hiz gerektiren denetleme islemi icin kullanilir.
     *
     * @param strGiris
     * @param hepsiniCozumle
     * @return tek ya da coklu kelime dizisi.
     */
    public Kelime[] cozumle(String strGiris, bool hepsiniCozumle) {

        //on islemler
        String strIslenmis = alfabe.ayikla(strGiris);
        if (!alfabe.cozumlemeyeUygunMu(strIslenmis) || strIslenmis.Length == 0)
            return Kelime.BOS_KELIME_DIZISI;

        //kok adaylarinin bulunmasi.
        List<Kok> kokler = kokBulucu.AdayKokleriGetir(strIslenmis);

        if (logger.IsInfoEnabled) logger.Info("Giris: " + strGiris + ", Adaylar: " + kokler);
        HarfDizisi girisDizi = new HarfDizisi(strIslenmis, alfabe);
        bool icerikDegisti = false;

        //cozumlerin bulunmasi
        List<Kelime> cozumler = new List<Kelime>(2);
        for (int i = kokler.Count - 1; i >= 0; i--) {
            if (icerikDegisti) {
                girisDizi = new HarfDizisi(strIslenmis, alfabe);
                icerikDegisti = false;
            }
            Kok kok = kokler[i];
            if (logger.IsInfoEnabled) logger.Info("Aday:" + kok.icerik());
            HarfDizisi kokDizi = new HarfDizisi(kok.icerik(), alfabe);

            //eger giris koke dogrudan esitse cozmeden kelimeyi olustur.
            if (harfDizisiKiyaslayici.kiyasla(kokDizi, girisDizi)) {
                Kelime kelime = kelimeUret(kok, kokDizi);
                if (yardimci.kelimeBicimiDenetle(kelime, strGiris)) {
                    if (!hepsiniCozumle) {
                        return new Kelime[] {kelime};
                    } else
                        cozumler.Add(kelime);
                }
            } else {
                icerikDegisti = yardimci.kokGirisDegismiVarsaUygula(kok, kokDizi, girisDizi);
                List<Kelime> sonuclar = coz(kok, kokDizi, girisDizi, hepsiniCozumle);
                foreach (Kelime sonuc in sonuclar) {
                    if (yardimci.kelimeBicimiDenetle(sonuc, strGiris)) {
                        if (!hepsiniCozumle) {
                            return new Kelime[] {sonuc};
                        }
                        cozumler.Add(sonuc);
                    }
                }
            }
        }
        return cozumler.ToArray();
    }

    private Kelime kelimeUret(Kok kok, HarfDizisi dizi) {
        Kelime kelime = new Kelime(kok, dizi);
        kelime.ekEkle(ekYonetici.ilkEkBelirle(kelime.kok()));
        return kelime;
    }

    private List<Kelime> coz(Kok kok, HarfDizisi kokDizi, HarfDizisi giris, bool tumunuCozumle) {

        Kelime kelime = kelimeUret(kok, kokDizi);
        BasitKelimeYigini kelimeYigini = new BasitKelimeYigini();
        Ek bulunanEk = kelime.sonEk();
        int ardisilEkSirasi = 0;
        List<Kelime> uygunSonuclar = Kelime.EMPTY_LIST_KELIME;
        TurkceHarf ilkEkHarfi= giris.harf(kelime.boy());
        while (true) {
            //bulunan son ekten sonra gelebilecek eklerden siradakini al.
            Ek incelenenEk = bulunanEk.getArdisilEk(ardisilEkSirasi++);
            //siradaki ek yoksa incelenen ek yanlis demektir.
            // yigindan kelimenin onceki durumunu cek.
            if (incelenenEk == null) {
                //yigin bos ise basarisizlik.
                if (kelimeYigini.bosMu())
                    return uygunSonuclar;

                //kelimeyi ve bulunan eki onceki formuna donustur.
                BasitKelimeYigini.YiginKelime yiginKelime = kelimeYigini.al();
                kelime = yiginKelime.getKelime();
                bulunanEk = kelime.sonEk();
                ardisilEkSirasi = yiginKelime.getEkSirasi();
                ilkEkHarfi= giris.harf(kelime.boy());
                continue;
            }

            if (kelime.gercekEkYok() && kelime.kok().ozelDurumVarmi()) {
                if (!ozelDurumUygula(kelime, giris, incelenenEk)) {
                    continue;
                } else
                   ilkEkHarfi = giris.harf(kelime.boy());
            }

            if(!incelenenEk.ilkHarfDenetle(ilkEkHarfi))
               continue;
            
            HarfDizisi olusanEkIcerigi = incelenenEk.cozumlemeIcinUret(kelime, giris, harfDizisiKiyaslayici);
            if (olusanEkIcerigi == null || olusanEkIcerigi.Length == 0) {
                continue;
            }

            if (harfDizisiKiyaslayici.aradanKiyasla(giris,
                    olusanEkIcerigi,
                    kelime.boy())) {
                // ek dongusu testi
                //if (kelime.ekDongusuVarmi(incelenenEk)) continue;
                kelimeYigini.koy((Kelime)kelime.Clone(), ardisilEkSirasi);
                ardisilEkSirasi = 0;
                kelime.ekEkle(incelenenEk);
                kelime.icerikEkle(olusanEkIcerigi);
                ilkEkHarfi = giris.harf(kelime.boy());
                if (logger.IsInfoEnabled) logger.Info("ekleme sonrasi olusan kelime: " + kelime.icerik());

                bulunanEk = incelenenEk;

                if (harfDizisiKiyaslayici.kiyasla(kelime.icerik(), giris) && !incelenenEk.sonEkOlamazMi()) {
                    if (!tumunuCozumle) {
                        uygunSonuclar = new List<Kelime>(1);
                        uygunSonuclar.Add(kelime);
                        return uygunSonuclar;
                    }
                    if (uygunSonuclar.Count==0)
                        uygunSonuclar = new List<Kelime>(2);
                    uygunSonuclar.Add((Kelime)kelime.Clone());
                }
            }
        }
    }

    private bool ozelDurumUygula(Kelime kelime, HarfDizisi giris, Ek ek) {
        if (!kelime.kok().yapiBozucuOzelDurumVarmi())
            return true;
        HarfDizisi testKokIcerigi = kelime.kok().ozelDurumUygula(alfabe, ek);
        if (testKokIcerigi == null) return false;
        if (logger.IsInfoEnabled) logger.Info("Ozel durum sonrasi:" + testKokIcerigi + "  ek:" + ek.ad());
        kelime.setIcerik(testKokIcerigi);
        return harfDizisiKiyaslayici.bastanKiyasla(giris, testKokIcerigi);
    }
}
}
