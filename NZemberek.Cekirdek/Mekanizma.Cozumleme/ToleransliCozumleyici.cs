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
 * The Original Code is Zemberek Doðal Dil Ýþleme Kütüphanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Akýn, Mehmet D. Akýn.
 * Portions created by the Initial Developer are Copyright (C) 2006
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Mert Derman
 *   Tankut Tekeli
 * ***** END LICENSE BLOCK ***** */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NZemberek.Cekirdek.KokSozlugu;
using NZemberek.Cekirdek.Yapi;
using NZemberek.Cekirdek.Benzerlik;
using NZemberek.Cekirdek.Kolleksiyonlar;

namespace NZemberek.Cekirdek.Mekanizma.Cozumleme
{
    public class ToleransliCozumleyici
    {
        public static readonly int TOLERANS = 1;
        private IKokBulucu kokBulucu;
        private IEkYonetici ekYonetici;
        private IKokOzelDurumYonetici kokOzelDurumYonetici;
        private Alfabe alfabe;
        private ICozumlemeYardimcisi yardimci;

        public ToleransliCozumleyici(IKokBulucu kokBulucu, IEkYonetici yonetici, IKokOzelDurumYonetici kokOzelDurumYonetici, Alfabe alfabe, ICozumlemeYardimcisi yardimci)
        {
            this.kokBulucu = kokBulucu;
            this.ekYonetici = yonetici;
            this.alfabe = alfabe;
            this.yardimci = yardimci;
            this.kokOzelDurumYonetici = kokOzelDurumYonetici;
        }

        public Kelime[] Cozumle(String strGiris)
        {
            String strIslenmis = alfabe.Ayikla(strGiris);
            if (strIslenmis.Length == 0)
                return Kelime.BOS_KELIME_DIZISI;
            List<Kok> kokler = kokBulucu.AdayKokleriGetir(strIslenmis);
            List<Kelime> cozumler = new List<Kelime>();

            HarfDizisi girisDizi = new HarfDizisi(strIslenmis, alfabe);
            bool icerikDegisti = false;
            for (int i = kokler.Count - 1; i >= 0; i--)
            {
                Kok kok = kokler[i];
                HarfDizisi kokDizi = new HarfDizisi(kok.Icerik, alfabe);
                if (icerikDegisti)
                {
                    girisDizi = new HarfDizisi(strIslenmis, alfabe);
                }
                //int kokHatasi=MetinAraclari.DuzeltmeMesafesi(kok.icerik(), strGiris.substring(0,kok.icerik().length()));
                int kokHatasi = 0;

                icerikDegisti = yardimci.KokGirisDegismiVarsaUygula(kok, kokDizi, girisDizi);
                if (BenzerlikAraci.DuzeltmeMesafesiIcinde(kok.Icerik, strIslenmis, TOLERANS))
                    cozumler.Add(new Kelime(kok, alfabe));
                IList<Kelime> sonuclar;
                if (TOLERANS > kokHatasi)
                    sonuclar = Coz(kok, kokDizi, girisDizi, TOLERANS - kokHatasi);
                else
                    sonuclar = Coz(kok, kokDizi, girisDizi, 0);
                cozumler.AddRange(sonuclar);
            }
            foreach (Kelime kel in cozumler)
            {
                yardimci.KelimeBicimlendir(kel);
                if (Char.IsUpper(strGiris[0]))
                    kel.Icerik.HarfDegistir(0, alfabe.BuyukHarf(kel.Icerik.IlkHarf()));
            }
            return cozumler.ToArray();
        }

        private IList<Kelime> Coz(Kok kok, HarfDizisi kokDizi, HarfDizisi girisDizi, int tolerans)
        {
            Kelime kelime = new Kelime(kok, kokDizi);
            kelime.EkEkle(ekYonetici.IlkEkBelirle(kelime.Kok));
            BasitKelimeYigini kelimeYigini = new BasitKelimeYigini();

            List<Kelime> uygunSonuclar = new List<Kelime>();

            //analiz kelimesini kokler kokunden olustur.
            kelimeYigini.Temizle();
            Ek bulunanEk = kelime.SonEk();

            int ardisilEkSirasi = 0;
            while (true)
            {
                //bulunan son ekten sonra gelebilecek eklerden siradakini Al.
                Ek incelenenEk = bulunanEk.ArdisilEkGetir(ardisilEkSirasi++);

                //siradaki ek yoksa incelenen ek yanlis demektir.
                // yigindan kelimenin onceki durumunu cek.
                if (incelenenEk == null)
                {
                    //yigin bos ise sonuclar dondur.
                    if (kelimeYigini.Bos())
                        return uygunSonuclar;

                    //kelimeyi ve bulunan eki onceki formuna donustur.
                    BasitKelimeYigini.YiginKelime yiginKelime = kelimeYigini.Al();
                    kelime = yiginKelime.Kelime;
                    bulunanEk = kelime.SonEk();
                    ardisilEkSirasi = yiginKelime.EkSirasi;
                    continue;
                }

                //eger daha olusan kelime kok asamasinda ise (yani sadece YALIN eki eklenmisse)
                // ve kokun (kelime ile kok ayni ozel durumlara sahip) icinde bir ozel durum var ise
                // ozel durum denetlenir, yani kokun girilen ek ile degisip degismedigine bakilir.
                if (kelime.Ekler.Count == 1 && kelime.Kok.OzelDurumVarmi())
                {
                    if (!OzelDurumDenetle(kelime, girisDizi, incelenenEk, tolerans))
                    {
                        continue;
                    }
                }

                // bazi eklerin olusumu, giris kelimesinin yapisina gore degisebilir.
                // ornegin giris "geleceGim" oldugu durumda gelecek zaman ekinin son harfinin yumusamasi bilgisi 
                // ancak girise bakarak anlasilabilir. bu nedenle ek olusturma sirasinda giris kullanilir

                HarfDizisi olusanEk = incelenenEk.CozumlemeIcinUret(kelime, girisDizi, null);
                if (olusanEk == null || olusanEk.Boy == 0)
                {
                    continue;
                }
                //Toleransli kiyaslama islemi burada yapiliyor. once gecici bir sekilde olusan kelimeye
                // olusan ek ekleniyor, ve giris ile toleransli kiyaslama yapiliyor. Eger kiyaslama
                // sonunda esik tolerans degeri asilmazsa dogru kabul edilip devam ediliyor.
                HarfDizisi olusum = new HarfDizisi(kelime.Icerik);
                olusum.Ekle(olusanEk);
                String olusumStr = olusum.ToString();
                if (BenzerlikAraci.ParcasiDuzeltmeMesafesiIcinde(olusumStr, girisDizi.ToString(), tolerans) ||
                        BenzerlikAraci.DuzeltmeMesafesiIcinde(olusumStr, girisDizi.ToString(), tolerans))
                {
                    kelimeYigini.Koy((Kelime)kelime.Clone(), ardisilEkSirasi);
                    ardisilEkSirasi = 0;
                    // ek ekleneceginde yumusama yapilip yapilmayacagi belirleniyor.. aci
                    if (olusanEk.Harf(0).Sesli
                            && kelime.SonHarf().Sert
                            && kelime.Ekler.Count > 1
                            && olusanEk.IlkHarf().SertDonusum != null)
                    {
                        kelime.Icerik.SonHarfYumusat();
                    }
                    kelime.IcerikEkle(olusanEk);
                    kelime.EkEkle(incelenenEk);
                    olusumStr = kelime.IcerikMetni();
                    bulunanEk = incelenenEk;

                    if (BenzerlikAraci.DuzeltmeMesafesiIcinde(olusumStr, girisDizi.ToString(), tolerans))
                    {
                        uygunSonuclar.Add((Kelime)kelime.Clone());
                    }
                    /*
                     * TurkceHarf ekIlkHarf = giris.Harf(kelime.Boy());
                     * if (ekIlkHarf == TurkceAlfabe.TANIMSIZ_HARF)
                     *     return false;
                     */
                }
            }
        }

        private bool OzelDurumDenetle(Kelime kelime, HarfDizisi girisDizi, Ek ek, int tolerans)
        {
            if (!kelime.Kok.YapiBozucuOzelDurumVar)
                return true;
            HarfDizisi testKokIcerigi = kokOzelDurumYonetici.OzelDurumUygula(kelime.Kok, ek);
            if (testKokIcerigi == null)
                return false;
            if (BenzerlikAraci.ParcasiDuzeltmeMesafesiIcinde(testKokIcerigi.ToString(), girisDizi.ToString(), tolerans))
            {
                kelime.Icerik = new HarfDizisi(testKokIcerigi);
                return true;
            }
            else
                kelime.Icerik = new HarfDizisi(kelime.Kok.Icerik, alfabe);
            return false;
        }
    }
}