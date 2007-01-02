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
using log4net;
using net.zemberek.bilgi.kokler;
using net.zemberek.yapi;
using net.zemberek.yapi.ek;
using net.zemberek.javaporttemp;
using net.zemberek.araclar;


namespace net.zemberek.islemler.cozumleme
{
    public class ToleransliCozumleyici : KelimeCozumleyici
    {
    public static readonly int TOLERANS = 1;
    private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private KokBulucu kokBulucu;
    private EkYonetici ekYonetici;
    private Alfabe alfabe;
    private CozumlemeYardimcisi yardimci;

    public ToleransliCozumleyici(KokBulucu kokBulucu,
                                 EkYonetici yonetici,
                                 Alfabe alfabe,
                                 CozumlemeYardimcisi yardimci) {
        this.kokBulucu = kokBulucu;
        this.ekYonetici = yonetici;
        this.alfabe = alfabe;
        this.yardimci = yardimci;
    }

    public bool denetle(String strGiris) {
        return false;
    }

    public Kelime[] cozumle(String strGiris) {
        String strIslenmis = alfabe.ayikla(strGiris);
        if (strIslenmis.Length == 0)
            return Collections.BOS_KELIME_DIZISI;
        List<Kok> kokler = kokBulucu.getAdayKokler(strIslenmis);
        List<Kelime> cozumler = new List<Kelime>();
        if (logger.IsInfoEnabled) logger.Info("Giris: " + strIslenmis + ", Adaylar: " + kokler);

        HarfDizisi girisDizi = new HarfDizisi(strIslenmis, alfabe);
        bool icerikDegisti = false;
        for (int i = kokler.Count - 1; i >= 0; i--) {
            Kok kok = kokler[i];
            HarfDizisi kokDizi = new HarfDizisi(kok.icerik(), alfabe);
            if (icerikDegisti) {
                girisDizi = new HarfDizisi(strIslenmis, alfabe);
            }
            //int kokHatasi=MetinAraclari.editDistance(kok.icerik(), strGiris.substring(0,kok.icerik().length()));
            int kokHatasi = 0;

            icerikDegisti = yardimci.kokGirisDegismiVarsaUygula(kok, kokDizi, girisDizi);
            if (logger.IsInfoEnabled) logger.Info("Aday:" + kok.icerik() + " tolerans:" + kokHatasi);
            if (MetinAraclari.inEditDistance(kok.icerik(), strIslenmis, TOLERANS))
                cozumler.Add(new Kelime(kok, alfabe));
            IList<Kelime> sonuclar;
            if (TOLERANS > kokHatasi)
                sonuclar = coz(kok, kokDizi, girisDizi, TOLERANS - kokHatasi);
            else
                sonuclar = coz(kok, kokDizi, girisDizi, 0);
            cozumler.AddRange(sonuclar);
        }
        foreach (Kelime kel in cozumler) {
            yardimci.kelimeBicimlendir(kel);
            if (Char.IsUpper(strGiris[0]))
                kel.icerik().harfDegistir(0, alfabe.buyukHarf(kel.icerik().ilkHarf()));
        }
        return cozumler.ToArray();
    }

    private IList<Kelime> coz(Kok kok, HarfDizisi kokDizi, HarfDizisi girisDizi, int tolerans) {

        Kelime kelime = new Kelime(kok, kokDizi);
        kelime.ekEkle(ekYonetici.ilkEkBelirle(kelime.kok()));
        BasitKelimeYigini kelimeYigini = new BasitKelimeYigini();

        List<Kelime> uygunSonuclar = new List<Kelime>();

        //analiz kelimesini kokler kokunden olustur.
        kelimeYigini.temizle();
        Ek bulunanEk = kelime.sonEk();

        int ardisilEkSirasi = 0;
        while (true) {
            //bulunan son ekten sonra gelebilecek eklerden siradakini al.
            Ek incelenenEk = bulunanEk.getArdisilEk(ardisilEkSirasi++);

            //siradaki ek yoksa incelenen ek yanlis demektir.
            // yigindan kelimenin onceki durumunu cek.
            if (incelenenEk == null) {
                //yigin bos ise sonuclar dondur.
                if (kelimeYigini.bosMu())
                    return uygunSonuclar;

                //kelimeyi ve bulunan eki onceki formuna donustur.
                BasitKelimeYigini.YiginKelime yiginKelime = kelimeYigini.al();
                kelime = yiginKelime.getKelime();
                bulunanEk = kelime.sonEk();
                ardisilEkSirasi = yiginKelime.getEkSirasi();
                continue;
            }

            //eger daha olusan kelime kok asamasinda ise (yani sadece YALIN eki eklenmisse)
            // ve kokun (kelime ile kok ayni ozel durumlara sahip) icinde bir ozel durum var ise
            // ozel durum denetlenir, yani kokun girilen ek ile degisip degismedigine bakilir.
            if (kelime.ekler().Count == 1 && kelime.kok().ozelDurumVarmi()) {
                if (!ozelDurumDenetle(kelime, girisDizi, incelenenEk, tolerans)) 
                {
                    if (logger.IsInfoEnabled) logger.Info("Ozel durum yanlis, ek:" + incelenenEk);
                    continue;
                }
            }

            //bazi eklerin olusumu, giris kelimesinin yapisina gore degisebilir.
            // ornegin giris "geleceGim" oldugu durumda gelecek zaman ekinin son harfinin
            // yumusamasi bilgisi ancak girise bakarak anlasilabilir. bu nedenle ek olusturma sirasinda giris
            // kullanilir

            HarfDizisi olusanEk = incelenenEk.cozumlemeIcinUret(kelime, girisDizi, null);
            //log.info("ek:" + incelenenEk + " olusum:" + olusanEk);
            if (olusanEk == null || olusanEk.Length == 0) {
                //log.info("bos ek.. " + incelenenEk);
                continue;
            }


            if (logger.IsInfoEnabled) logger.Info("Kok ve Olusan Ek:" + kelime.icerik() + " " + olusanEk);

            //Toleransli kiyaslama islemi burada yapiliyor. once gecici bir sekilde olusan kelimeye
            // olusan ek ekleniyor, ve giris ile toleransli kiyaslama yapiliyor. Eger kiyaslama
            // sonunda esik tolerans degeri asilmazsa dogru kabul edilip devam ediliyor.
            HarfDizisi olusum = new HarfDizisi(kelime.icerik());
            olusum.ekle(olusanEk);
            String olusumStr = olusum.ToString();
            if (logger.IsInfoEnabled) logger.Info("olusum:" + olusum);

            if (MetinAraclari.isInSubstringEditDistance(olusumStr, girisDizi.ToString(), tolerans) ||
                    MetinAraclari.inEditDistance(olusumStr, girisDizi.ToString(), tolerans)) {
                        kelimeYigini.koy((Kelime)kelime.Clone(), ardisilEkSirasi);
                ardisilEkSirasi = 0;
                // ek ekleneceginde yumusama yapilip yapilmayacagi belirleniyor.. aci
                if (olusanEk.harf(0).Sesli
                        && kelime.sonHarf().Sert
                        && kelime.ekler().Count > 1
                        && olusanEk.ilkHarf().SertDonusum!=null) 
                {
                    kelime.icerik().sonHarfYumusat();
                }
                kelime.icerikEkle(olusanEk);
                kelime.ekEkle(incelenenEk);
                olusumStr = kelime.icerikStr();
                if (logger.IsInfoEnabled) logger.Info("ekleme sonrasi olusan kelime: " + kelime.icerik());

                bulunanEk = incelenenEk;

                if (MetinAraclari.inEditDistance(olusumStr, girisDizi.ToString(), tolerans)) {
                    uygunSonuclar.Add((Kelime)kelime.Clone());
                    if (logger.IsInfoEnabled) logger.Info("uygun kelime:" + kelime.icerik());
                }
/*
                        TurkceHarf ekIlkHarf = giris.harf(kelime.boy());
                        if (ekIlkHarf == TurkceAlfabe.TANIMSIZ_HARF)
                            return false;*/

            }
        }
    }

    private bool ozelDurumDenetle(Kelime kelime, HarfDizisi girisDizi, Ek ek, int tolerans) {
        if (!kelime.kok().yapiBozucuOzelDurumVarmi())
            return true;
        HarfDizisi testKokIcerigi = kelime.kok().ozelDurumUygula(alfabe, ek);
        //if (log.isTraceEnabled()) log.trace("Ozel durum sonrasi:" + testKokIcerigi + "  ek:" + ek.getIsim());
        if (testKokIcerigi == null)
            return false;
        if (MetinAraclari.isInSubstringEditDistance(testKokIcerigi.ToString(), girisDizi.ToString(), tolerans)) {
            kelime.setIcerik(new HarfDizisi(testKokIcerigi));
            //if (log.isTraceEnabled()) log.trace("basari, kelime:" + kelime.icerik());
            return true;
        } else
            kelime.setIcerik(new HarfDizisi(kelime.kok().icerik(), alfabe));
        //if (log.isTraceEnabled()) log.trace("kelime:" + kelime.icerik());
        return false;
    }
    }
}


