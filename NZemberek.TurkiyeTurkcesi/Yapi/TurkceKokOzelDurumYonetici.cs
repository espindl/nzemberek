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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NZemberek.Cekirdek.Yapi;

namespace NZemberek.TrTurkcesi.Yapi
{
    public class TurkceKokOzelDurumYonetici : TemelKokOzelDurumYonetici,IKokOzelDurumYonetici
    {
        public static readonly string SESSIZ_YUMUSAMASI = "YUM";
        public static readonly string SESSIZ_YUMUSAMASI_NK = "YUM_NK";
        public static readonly string ISIM_SESLI_DUSMESI = "DUS";
        public static readonly string CIFTLEME = "CIFT";
        public static readonly string FIIL_ARA_SESLI_DUSMESI = "DUS_FI";
        public static readonly string KUCULTME = "KUCULTME";
        public static readonly string TEKIL_KISI_BOZULMASI = "TEKIL_KISI_BOZULMASI";
        public static readonly string FIIL_KOK_BOZULMASI = "FIIL_KOK_BOZULMASI";
        public static readonly string TERS_SESLI_EK = "TERS";
        public static readonly string SIMDIKI_ZAMAN = "SDK_ZAMAN";
        public static readonly string ISIM_SON_SESLI_DUSMESI = "DUS_SON";
        public static readonly string ZAMIR_SESLI_OZEL = "ZAMIR_SESLI_OZEL";
        public static readonly string ISIM_TAMLAMASI = "IS_TAM";
        public static readonly string YALIN = "YAL";
        public static readonly string GENIS_ZAMAN = "GEN";
        public static readonly string EK_OZEL_DURUMU = "EK";
        public static readonly string FIIL_GECISSIZ = "GEC";
        public static readonly string KAYNASTIRMA_N = "KAYNASTIRMA_N";
        public static readonly string KESMESIZ = "KESMESIZ";
        public static readonly string TIRE = "TIRE";
        public static readonly string KESME = "KESME";
        public static readonly string OZEL_IC_KARAKTER = "OZEL_IC_KARAKTER";
        public static readonly string ZAMIR_IM = "ZAMIR_IM";
        public static readonly string ZAMIR_IN = "ZAMIR_IN";
        public static readonly string KISALTMA_SON_SESLI = "SON";
        public static readonly string KISALTMA_SON_SESSIZ = "SON_N";
        public static readonly string SU_OZEL_DURUMU = "FARKLI_KAYNASTIRMA";


        /**
         * KokOzel durum nesneleri burada uretilir. uretim icin "Uretici" metodu kullanilir. bu metod KokOzelDurumu sinifi
         * icinde yer alan Uretim sinifindan bir nesne dondurur. Bu nesne uzerinden kok izel durumu parametreleri belirlenir
         * ve Ekle sinifi icinde kokOzelDurumu nesnesi uretilip cesitli map, dizi vs tasiyici parametrelere atanir.
         */
        private void uret()
        {
            int ind = 0;
            Ekle(Uretici(ind++,TurkceKokOzelDurumYonetici.SESSIZ_YUMUSAMASI, new Yumusama()).SesliEkIleOlusur(true).YapiBozucu(true));

            Ekle(Uretici(ind++,TurkceKokOzelDurumYonetici.SESSIZ_YUMUSAMASI_NK, new YumusamaNk(alfabe)).SesliEkIleOlusur(true).YapiBozucu(true));

            Ekle(Uretici(ind++,TurkceKokOzelDurumYonetici.ISIM_SESLI_DUSMESI, 
                                            new string[]{
                                                TurkceEkAdlari.ISIM_YONELME_E,
                                                TurkceEkAdlari.ISIM_BELIRTME_I,
                                                TurkceEkAdlari.ISIM_SAHIPLIK_BEN_IM,
                                                TurkceEkAdlari.ISIM_SAHIPLIK_SEN_IN,
                                                TurkceEkAdlari.ISIM_SAHIPLIK_SIZ_INIZ,
                                                TurkceEkAdlari.ISIM_SAHIPLIK_O_I,
                                                TurkceEkAdlari.ISIM_SAHIPLIK_BIZ_IMIZ,
                                                TurkceEkAdlari.ISIM_TAMLAMA_IN},
                                            new AraSesliDusmesi()).YapiBozucu(true));

            Ekle(Uretici(ind++,TurkceKokOzelDurumYonetici.CIFTLEME, new Ciftleme()).
                    SesliEkIleOlusur(true).
                    YapiBozucu(true));

            Ekle(Uretici(ind++,TurkceKokOzelDurumYonetici.FIIL_ARA_SESLI_DUSMESI, new AraSesliDusmesi()).YapiBozucu(true));

            Ekle(Uretici(ind++,TurkceKokOzelDurumYonetici.KUCULTME, new SonHarfDusmesi()).YapiBozucu(true));

            IDictionary<String, String> benSenDonusum = new Dictionary<String,String>();

            benSenDonusum.Add("ben", "ban");
            benSenDonusum.Add("sen", "san");
            Ekle(Uretici(ind++,TurkceKokOzelDurumYonetici.TEKIL_KISI_BOZULMASI, new YeniIcerikAta(alfabe, benSenDonusum)).YapiBozucu(true));

            IDictionary<String, String> deYeDonusum = new Dictionary<String,String>();
            deYeDonusum.Add("de", "di");
            deYeDonusum.Add("ye", "yi");
            Ekle(Uretici(ind++,TurkceKokOzelDurumYonetici.FIIL_KOK_BOZULMASI, 
                                            new string[]{
                                                TurkceEkAdlari.FIIL_EDILGEN_IL,
                                                TurkceEkAdlari.FIIL_SIMDIKIZAMAN_IYOR,
                                                TurkceEkAdlari.FIIL_GELECEKZAMAN_ECEK,
                                                TurkceEkAdlari.FIIL_DONUSUM_ECEK,
                                                TurkceEkAdlari.FIIL_DONUSUM_EN,
                                                TurkceEkAdlari.FIIL_DONUSUM_IS,
                                                TurkceEkAdlari.FIIL_ISTEK_E,
                                                TurkceEkAdlari.FIIL_SUREKLILIK_EREK,
                                                TurkceEkAdlari.FIIL_SURERLIK_EDUR,
                                                TurkceEkAdlari.FIIL_SURERLIK_EGEL,
                                                TurkceEkAdlari.FIIL_SURERLIK_EKAL,
                                                TurkceEkAdlari.FIIL_YAKLASMA_AYAZ,
                                                TurkceEkAdlari.FIIL_YETENEK_EBIL,
                                                TurkceEkAdlari.FIIL_BERABERLIK_IS,
                                                TurkceEkAdlari.FIIL_EMIR_SIZ_IN,
                                                TurkceEkAdlari.FIIL_EMIR_SIZRESMI_INIZ},
                                            new YeniIcerikAta(alfabe, deYeDonusum)).YapiBozucu(true));

            Ekle(Uretici(ind++,TurkceKokOzelDurumYonetici.TERS_SESLI_EK, new SonSesliIncelt(alfabe)).
                    YapiBozucu(true).
                    HerZamanOlusur(true));

            Ekle(Uretici(ind++,TurkceKokOzelDurumYonetici.SIMDIKI_ZAMAN, new SonHarfDusmesi()).YapiBozucu(true));

            Ekle(Uretici(ind++,TurkceKokOzelDurumYonetici.ISIM_SON_SESLI_DUSMESI, 
                                            new string[]{
                                                TurkceEkAdlari.ISIM_KALMA_DE,
                                                TurkceEkAdlari.ISIM_CIKMA_DEN,
                                                TurkceEkAdlari.ISIM_COGUL_LER}, 
                                            new SonHarfDusmesi()).Secimlik(true).YapiBozucu(true));

            HarfDizisi n = new HarfDizisi("n", alfabe);
            Ekle(Uretici(ind++,TurkceKokOzelDurumYonetici.ZAMIR_SESLI_OZEL, 
                                            new string[]{
                                                TurkceEkAdlari.ISIM_YONELME_E,
                                                TurkceEkAdlari.ISIM_KALMA_DE,
                                                TurkceEkAdlari.ISIM_CIKMA_DEN,
                                                TurkceEkAdlari.ISIM_BELIRTME_I,
                                                TurkceEkAdlari.ISIM_TAMLAMA_IN,
                                                TurkceEkAdlari.ISIM_YOKLUK_SIZ,
                                                TurkceEkAdlari.ISIM_COGUL_LER,
                                                TurkceEkAdlari.ISIM_TARAFINDAN_CE,
                                                TurkceEkAdlari.ISIM_DURUM_LIK,
                                                TurkceEkAdlari.ISIM_KUCULTME_CEGIZ}, 
                                            new Ulama(n)).YapiBozucu(true));

            Ekle(Uretici(ind++,TurkceKokOzelDurumYonetici.ISIM_TAMLAMASI,
                                            new string[]{
                                                TurkceEkAdlari.ISIM_SAHIPLIK_BEN_IM,
                                                TurkceEkAdlari.ISIM_SAHIPLIK_SEN_IN,
                                                TurkceEkAdlari.ISIM_SAHIPLIK_O_I,
                                                TurkceEkAdlari.ISIM_SAHIPLIK_BIZ_IMIZ,
                                                TurkceEkAdlari.ISIM_SAHIPLIK_SIZ_INIZ,
                                                TurkceEkAdlari.ISIM_SAHIPLIK_ONLAR_LERI,
                                                TurkceEkAdlari.ISIM_TAMLAMA_I,
                                                TurkceEkAdlari.ISIM_BULUNMA_LIK,
                                                TurkceEkAdlari.ISIM_BULUNMA_LI,
                                                TurkceEkAdlari.ISIM_COGUL_LER,
                                                TurkceEkAdlari.ISIM_YOKLUK_SIZ,
                                                TurkceEkAdlari.ISIM_TARAFINDAN_CE,
                                                TurkceEkAdlari.ISIM_DURUM_LIK,
                                                TurkceEkAdlari.ISIM_KUCULTME_CEGIZ,
                                                TurkceEkAdlari.ISIM_ILGI_CI},
                                            new BosHarfDizisiIslemi()).EkKisitlayici(true));

            BosOzelDurumEkle(ind, new string[]{
                    TurkceKokOzelDurumYonetici.YALIN,
                    TurkceKokOzelDurumYonetici.GENIS_ZAMAN,
                    TurkceKokOzelDurumYonetici.EK_OZEL_DURUMU,
                    TurkceKokOzelDurumYonetici.FIIL_GECISSIZ,
                    TurkceKokOzelDurumYonetici.KAYNASTIRMA_N,
                    TurkceKokOzelDurumYonetici.KESMESIZ,
                    TurkceKokOzelDurumYonetici.TIRE,
                    TurkceKokOzelDurumYonetici.KESME,
                    TurkceKokOzelDurumYonetici.OZEL_IC_KARAKTER,
                    TurkceKokOzelDurumYonetici.ZAMIR_IM,
                    TurkceKokOzelDurumYonetici.ZAMIR_IN,
                    TurkceKokOzelDurumYonetici.KISALTMA_SON_SESLI,
                    TurkceKokOzelDurumYonetici.KISALTMA_SON_SESSIZ,
                    TurkceKokOzelDurumYonetici.SU_OZEL_DURUMU});

        }

        public TurkceKokOzelDurumYonetici(IEkYonetici ekler, Alfabe alfabe):base(ekler,alfabe)
        {
            uret();
        }

        public String[] OzelDurumUygula(Kok kok) {
        //kok icinde ozel durum yok ise cik..
        if (!kok.OzelDurumVarmi())
            return new String[0];

        HarfDizisi hdizi = new HarfDizisi(kok.Icerik, alfabe);

        IList degismisIcerikler = new ArrayList(1);

        //ara sesli dusmesi nedeniyle bazen yapay oarak kok'e ters sesli etkisi ozel durumunun eklenmesi gerekir.
        // nakit -> nakde seklinde. normal kosullarda "nakda" olusmasi gerekirdi.
        bool eskiSonsesliInce = false;
        if (kok.OzelDurumIceriyormu(TurkceKokOzelDurumYonetici.ISIM_SESLI_DUSMESI))
            eskiSonsesliInce = hdizi.SonSesli().InceSesli;

        bool yapiBozucuOzelDurumvar = false;
        //ters sesli ozel durumu yapi bozucu ama sadece seslinin tipini degistirdiginden
        //islemeye gerek yok.
        if (kok.KokOzelDurumlariGetir().Length == 1 && kok.OzelDurumIceriyormu(TurkceKokOzelDurumYonetici.TERS_SESLI_EK))
            return new String[0];

        // kok uzerindeki ozel durumlar basta sona taranip ozel durum koke uygulaniyor.
        foreach (string ad in kok.KokOzelDurumlariGetir())
        {
            KokOzelDurumu _ozelDurum = ozelDurumlar[ad];
            // kucultme ozel durumunda problem var, cunku kok'te hem kucultme hem yumusama uygulaniyor.
            if (_ozelDurum == null) {
                //Console.Write("kok = " + kok);
                //Environment.Exit(-1);
                logger.Warn("null ozle durum!. Kok:" + kok);
                return new String[0];
            }
            if (!_ozelDurum.Equals(OzelDurum(TurkceKokOzelDurumYonetici.KUCULTME)))
                _ozelDurum.Uygula(hdizi);
            if (_ozelDurum.YapiBozucu())
                yapiBozucuOzelDurumvar = true;
        }
        // ara sesli dusmesi durumunda dusen sesi ile dustukten sonra olusan seslinin farkli olmasi durumunda
        // kok'e bir de ters sesli ek ozel durumu ekleniyor.,
        if (kok.OzelDurumIceriyormu(TurkceKokOzelDurumYonetici.ISIM_SESLI_DUSMESI)
                || kok.OzelDurumIceriyormu(TurkceKokOzelDurumYonetici.FIIL_ARA_SESLI_DUSMESI))
        {
            if (!hdizi.SonSesli().InceSesli && eskiSonsesliInce)
                kok.OzelDurumEkle(TurkceKokOzelDurumYonetici.TERS_SESLI_EK);
        }

        if (yapiBozucuOzelDurumvar)
            degismisIcerikler.Add(hdizi.ToString());

        if (kok.OzelDurumVarmi() &&
                kok.OzelDurumIceriyormu(TurkceKokOzelDurumYonetici.KUCULTME) &&
                kok.OzelDurumIceriyormu(TurkceKokOzelDurumYonetici.SESSIZ_YUMUSAMASI))
        {
            HarfDizisi tempDizi = new HarfDizisi(kok.Icerik, alfabe);
            OzelDurum(TurkceKokOzelDurumYonetici.KUCULTME).Uygula(tempDizi);
            degismisIcerikler.Add(tempDizi.ToString());
        }
        // yani ozel durumlar eklenmis olabileceginden koke ods'u tekrar koke esle.
        String[] tempArr = new String[degismisIcerikler.Count];
        degismisIcerikler.CopyTo(tempArr,0);
        return tempArr;
    }

        public void OzelDurumBelirle(Kok kok)
        {
            // eger bir fiilin son harfi sesli ise bu dogrudan simdiki zaman ozel durumu olarak ele alinir.
            // bu ozel durum bilgi tabaninda ayrica belirtilmedigi icin burada kok'e eklenir.  aramak -> ar(a)Iyor
            char sonChar = kok.Icerik[kok.Icerik.Length - 1];
            if (kok.Tip == KelimeTipi.FIIL && alfabe.Harf(sonChar).Sesli)
            {
                //demek-yemek fiilleri icin bu uygulama yapilamaz.
                if (!kok.OzelDurumIceriyormu(TurkceKokOzelDurumYonetici.FIIL_KOK_BOZULMASI))
                {
                    kok.OzelDurumEkle(TurkceKokOzelDurumYonetici.SIMDIKI_ZAMAN);
                }
            }
        }

        public void DuzyaziOzelDurumOku(Kok kok, String okunanIcerik, String[] parcalar)
        {
            for (int i = 2; i < parcalar.Length; i++)
            {

                String _ozelDurum = parcalar[i];

                //kisaltma ozel durumunun analizi burada yapiliyor.
                if (_ozelDurum.StartsWith(TurkceKokOzelDurumYonetici.KISALTMA_SON_SESLI))
                {
                    int loc = _ozelDurum.IndexOf(':');
                    if (loc > 0)
                    {
                        String parca = _ozelDurum.Substring(loc + 1);
                        char sonSesli = parca[0];
                        if (!alfabe.Harf(sonSesli).Sesli)
                            logger.Warn("Hatali kisaltma harfi.. Sesli bekleniyordu." + _ozelDurum);
                        kok.KisaltmaSonSeslisi = sonSesli;
                        if (parca.Length > 1)
                        {
                            kok.OzelDurumEkle(TurkceKokOzelDurumYonetici.KISALTMA_SON_SESSIZ);
                        }
                        else
                            kok.OzelDurumCikar(TurkceKokOzelDurumYonetici.KISALTMA_SON_SESLI);
                    }
                    else
                    {
                        char sonHarf = kok.Icerik[kok.Icerik.Length - 1];
                        if (!alfabe.Harf(sonHarf).Sesli)
                        {
                            kok.KisaltmaSonSeslisi='e';
                            kok.OzelDurumEkle(TurkceKokOzelDurumYonetici.KISALTMA_SON_SESLI);
                        }
                    }
                    continue;
                }

                //diger ozel durumlarin elde edilmesi..
                KokOzelDurumu oz = OzelDurum(_ozelDurum);
                if (oz != null)
                {
                    kok.OzelDurumEkle(oz.Ad);
                }
                else
                {
                    logger.Warn("Hatali kok bileseni" + kok.Icerik + " Token: " + _ozelDurum);
                }
            }

            //kisaltmalari ve ozel karakter iceren kokleri asil icerik olarak ata.
            if (kok.Tip == KelimeTipi.KISALTMA || kok.OzelDurumIceriyormu(TurkceKokOzelDurumYonetici.OZEL_IC_KARAKTER))
                kok.Asil=okunanIcerik;
        }

        public void KokIcerigiIsle(Kok kok, KelimeTipi tip, String icerik)
        {
            //Tip kisaltma ise ya da icerik ozel karakterler iceriyorsa bunu kok'un asil haline ata.
            if (tip.Equals(KelimeTipi.KISALTMA))
                kok.Asil =icerik;
            if (tip.Equals(KelimeTipi.FIIL) && (icerik.EndsWith("mek") || icerik.EndsWith("mak")))
            {
                icerik = icerik.Substring(0, icerik.Length - 3);
                kok.Icerik=icerik;
            }
        }

        public HarfDizisi OzelDurumUygula(Kok kok, Ek ek)
        {
            HarfDizisi dizi = new HarfDizisi(kok.Icerik, alfabe);
            foreach (string ozelDurumAdi in kok.KokOzelDurumlariGetir())
            {
                KokOzelDurumu ozelDurum = ozelDurumlar[ozelDurumAdi];
                if (ozelDurum.YapiBozucu() && ozelDurum.Olusabilir(ek))
                    ozelDurum.Uygula(dizi);
                if (!ozelDurum.Olusabilir(ek) && ozelDurum.EkKisitlayici())
                    return null;
            }
            return dizi;
        }

    }
}
