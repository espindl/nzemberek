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
    public class StandartCozumleyici : IKelimeCozumleyici
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IKokBulucu kokBulucu;
        private IHarfDizisiKiyaslayici harfDizisiKiyaslayici;
        private IEkYonetici ekYonetici;
        private IKokOzelDurumYonetici kokOzelDurumYonetici;
        private Alfabe alfabe;
        private ICozumlemeYardimcisi yardimci;

        public StandartCozumleyici(IKokBulucu kokBulucu,
                                   IHarfDizisiKiyaslayici kiyaslayci,
                                   Alfabe alfabe,
                                   IEkYonetici ekYonetici,
                                   IKokOzelDurumYonetici kokOzelDurumYonetici,
                                   ICozumlemeYardimcisi yardimci)
        {
            this.kokBulucu = kokBulucu;
            this.harfDizisiKiyaslayici = kiyaslayci;
            this.ekYonetici = ekYonetici;
            this.kokOzelDurumYonetici = kokOzelDurumYonetici;
            this.alfabe = alfabe;
            this.yardimci = yardimci;
        }


        public bool Cozumlenebilir(String strGiris)
        {
            return Cozumle(strGiris, CozumlemeSeviyesi.TEK_KOK).Length == 1;
        }

        /// <summary>
        /// eger hepsiniCozumle=true ise dogru olabilecek tum kok ve ek kombinasyonlarinidondurur.
        /// Eger flag false ise ilk dogru cozumu tek elemanli dizi seklinde
        /// dondurur.bu yontem hiz gerektiren denetleme islemi icin kullanilir.
        /// </summary>
        /// <param name="strGiris"></param>
        /// <param name="seviye">cozumnleme isleminin ne zaman sona erecegi bu bilesenin degerine gore anlasilir.</param>
        /// <returns>tek ya da coklu kelime dizisi</returns>
        public Kelime[] Cozumle(String strGiris, CozumlemeSeviyesi seviye)
        {
            //on islemler
            String strIslenmis = alfabe.Ayikla(strGiris);
            if (!alfabe.CozumlemeyeUygunMu(strIslenmis) || strIslenmis.Length == 0)
                return Kelime.BOS_KELIME_DIZISI;

            //kok adaylarinin bulunmasi.
            List<Kok> kokler = kokBulucu.AdayKokleriGetir(strIslenmis);

#if log
            if (logger.IsInfoEnabled) logger.Info("Giris: " + strGiris + ", Adaylar: " + kokler);
#endif
            HarfDizisi girisDizi = new HarfDizisi(strIslenmis, alfabe);
            bool icerikDegisti = false;

            //cozumlerin bulunmasi
            List<Kelime> cozumler = new List<Kelime>(2);
            for (int i = kokler.Count - 1; i >= 0; i--)
            {
                if (icerikDegisti)
                {
                    girisDizi = new HarfDizisi(strIslenmis, alfabe);
                    icerikDegisti = false;
                }
                Kok kok = kokler[i];
#if log
                if (logger.IsInfoEnabled) logger.Info("Aday:" + kok.Icerik);
#endif
                HarfDizisi kokDizi = new HarfDizisi(kok.Icerik, alfabe);

                //eger giris koke dogrudan esitse cozmeden kelimeyi olustur.
                if (harfDizisiKiyaslayici.Kiyasla(kokDizi, girisDizi))
                {
                    Kelime kelime = KelimeUret(kok, kokDizi);
                    if (yardimci.KelimeBicimiDenetle(kelime, strGiris))
                    {
                        if (seviye == CozumlemeSeviyesi.TEK_KOK)
                        {
                            return new Kelime[] { kelime };
                        }
                        else
                            cozumler.Add(kelime);
                    }
                }
                else
                {
                    icerikDegisti = yardimci.KokGirisDegismiVarsaUygula(kok, kokDizi, girisDizi);
                    List<Kelime> sonuclar = Coz(kok, kokDizi, girisDizi, seviye);
                    foreach (Kelime sonuc in sonuclar)
                    {
                        if (yardimci.KelimeBicimiDenetle(sonuc, strGiris))
                        {
                            if (seviye == CozumlemeSeviyesi.TEK_KOK)
                            {
                                return new Kelime[] { sonuc };
                            }
                            cozumler.Add(sonuc);
                        }
                    }
                }
            }
            return cozumler.ToArray();
        }

        private Kelime KelimeUret(Kok kok, HarfDizisi dizi)
        {
            Kelime kelime = new Kelime(kok, dizi);
            kelime.EkEkle(ekYonetici.IlkEkBelirle(kelime.Kok));
            return kelime;
        }

        private List<Kelime> Coz(Kok kok, HarfDizisi kokDizi, HarfDizisi giris, CozumlemeSeviyesi seviye)
        {
            Kelime kelime = KelimeUret(kok, kokDizi);
            BasitKelimeYigini kelimeYigini = new BasitKelimeYigini();
            Ek bulunanEk = kelime.SonEk();
            int ardisilEkSirasi = 0;
            List<Kelime> uygunSonuclar = Kelime.EMPTY_LIST_KELIME;
            TurkceHarf ilkEkHarfi = giris.Harf(kelime.Boy());
            while (true)
            {
                //bulunan son ekten sonra gelebilecek eklerden siradakini Al.
                Ek incelenenEk = bulunanEk.ArdisilEkGetir(ardisilEkSirasi++);
                //siradaki ek yoksa incelenen ek yanlis demektir.
                // yigindan kelimenin onceki durumunu cek.
                if (incelenenEk == null)
                {
                    //yigin bos ise basarisizlik.
                    if (kelimeYigini.Bos())
                        return uygunSonuclar;

                    //kelimeyi ve bulunan eki onceki formuna donustur.
                    BasitKelimeYigini.YiginKelime yiginKelime = kelimeYigini.Al();
                    kelime = yiginKelime.Kelime;
                    bulunanEk = kelime.SonEk();
                    ardisilEkSirasi = yiginKelime.EkSirasi;
                    ilkEkHarfi = giris.Harf(kelime.Boy());
                    continue;
                }

                if (kelime.GercekEkYok() && kelime.Kok.OzelDurumVarmi())
                {
                    if (!OzelDurumUygula(kelime, giris, incelenenEk))
                    {
                        continue;
                    }
                    else
                        ilkEkHarfi = giris.Harf(kelime.Boy());
                }

                if (!incelenenEk.IlkHarfDenetle(ilkEkHarfi))
                    continue;

                HarfDizisi olusanEkIcerigi = incelenenEk.CozumlemeIcinUret(kelime, giris, harfDizisiKiyaslayici);
                if (olusanEkIcerigi == null || olusanEkIcerigi.Boy == 0)
                {
                    continue;
                }

                if (harfDizisiKiyaslayici.AradanKiyasla(giris,
                        olusanEkIcerigi,
                        kelime.Boy()))
                {
                    // ek dongusu testi
                    //if (kelime.ekDongusuVarmi(incelenenEk)) continue;
                    kelimeYigini.Koy((Kelime)kelime.Clone(), ardisilEkSirasi);
                    ardisilEkSirasi = 0;
                    kelime.EkEkle(incelenenEk);
                    kelime.IcerikEkle(olusanEkIcerigi);
                    ilkEkHarfi = giris.Harf(kelime.Boy());
#if log
                    if (logger.IsInfoEnabled) logger.Info("ekleme sonrasi olusan kelime: " + kelime.Icerik);
#endif
                    bulunanEk = incelenenEk;

                    if (harfDizisiKiyaslayici.Kiyasla(kelime.Icerik, giris) && !incelenenEk.SonEkOlamaz)
                    {
                        if (seviye != CozumlemeSeviyesi.TUM_KOK_VE_EKLER)
                        {
                            uygunSonuclar = new List<Kelime>(1);
                            uygunSonuclar.Add(kelime);
                            return uygunSonuclar;
                        }
                        if (uygunSonuclar.Count == 0)
                            uygunSonuclar = new List<Kelime>(2);
                        uygunSonuclar.Add((Kelime)kelime.Clone());
                    }
                }
            }
        }

        private bool OzelDurumUygula(Kelime kelime, HarfDizisi giris, Ek ek)
        {
            if (!kelime.Kok.YapiBozucuOzelDurumVar)
                return true;
            HarfDizisi testKokIcerigi = kokOzelDurumYonetici.OzelDurumUygula(kelime.Kok, ek);
            if (testKokIcerigi == null) return false;
#if log
            if (logger.IsInfoEnabled) logger.Info("Ozel durum sonrasi:" + testKokIcerigi + "  ek:" + ek.Ad);
#endif
            kelime.Icerik = testKokIcerigi;
            return harfDizisiKiyaslayici.BastanKiyasla(giris, testKokIcerigi);
        }
    }
}
