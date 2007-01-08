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
    public class TurkceKokOzelDurumBilgisi : TemelKokOzelDurumBilgisi,IKokOzelDurumBilgisi
    {

        /**
         * KokOzel durum nesneleri burada uretilir. uretim icin "Uretici" metodu kullanilir. bu metod KokOzelDurumu sinifi
         * icinde yer alan Uretim sinifindan bir nesne dondurur. Bu nesne uzerinden kok izel durumu parametreleri belirlenir
         * ve Ekle sinifi icinde kokOzelDurumu nesnesi uretilip cesitli map, dizi vs tasiyici parametrelere atanir.
         */
        private void uret()
        {
            Ekle(Uretici(TurkceKokOzelDurumTipi.SESSIZ_YUMUSAMASI, new Yumusama()).SesliEkIleOlusur(true).YapiBozucu(true));

            Ekle(Uretici(TurkceKokOzelDurumTipi.SESSIZ_YUMUSAMASI_NK, new YumusamaNk(alfabe)).SesliEkIleOlusur(true).YapiBozucu(true));

            Ekle(Uretici(TurkceKokOzelDurumTipi.ISIM_SESLI_DUSMESI, new AraSesliDusmesi()).YapiBozucu(true));

            Ekle(Uretici(TurkceKokOzelDurumTipi.CIFTLEME, new Ciftleme()).
                    SesliEkIleOlusur(true).
                    YapiBozucu(true));

            Ekle(Uretici(TurkceKokOzelDurumTipi.FIIL_ARA_SESLI_DUSMESI, new AraSesliDusmesi()).YapiBozucu(true));

            Ekle(Uretici(TurkceKokOzelDurumTipi.KUCULTME, new SonHarfDusmesi()).YapiBozucu(true));

            IDictionary<String, String> benSenDonusum = new Dictionary<String,String>();

            benSenDonusum.Add("ben", "ban");
            benSenDonusum.Add("sen", "san");
            Ekle(Uretici(TurkceKokOzelDurumTipi.TEKIL_KISI_BOZULMASI, new YeniIcerikAta(alfabe, benSenDonusum)).YapiBozucu(true));

            IDictionary<String, String> deYeDonusum = new Dictionary<String,String>();
            deYeDonusum.Add("de", "di");
            deYeDonusum.Add("ye", "yi");
            Ekle(Uretici(TurkceKokOzelDurumTipi.FIIL_KOK_BOZULMASI, new YeniIcerikAta(alfabe, deYeDonusum)).YapiBozucu(true));

            Ekle(Uretici(TurkceKokOzelDurumTipi.TERS_SESLI_EK, new SonSesliIncelt(alfabe)).
                    YapiBozucu(true).
                    HerZamanOlusur(true));

            Ekle(Uretici(TurkceKokOzelDurumTipi.SIMDIKI_ZAMAN, new SonHarfDusmesi()).YapiBozucu(true));

            Ekle(Uretici(TurkceKokOzelDurumTipi.ISIM_SON_SESLI_DUSMESI, new SonHarfDusmesi()).
                    Secimlik(true).
                    YapiBozucu(true));

            //HarfDizisi yu = new HarfDizisi("yu", Alfabe);
            //Ekle(Uretici(TurkceKokOzelDurumTipi.SU_OZEL_DURUMU, new Ulama(yu)).YapiBozucu(true));

            HarfDizisi n = new HarfDizisi("n", alfabe);
            Ekle(Uretici(TurkceKokOzelDurumTipi.ZAMIR_SESLI_OZEL, new Ulama(n)).YapiBozucu(true));

            Ekle(Uretici(TurkceKokOzelDurumTipi.ISIM_TAMLAMASI, new BosHarfDizisiIslemi()).EkKisitlayici(true));

            BosOzelDurumEkle(new IKokOzelDurumTipi[]{
                    TurkceKokOzelDurumTipi.YALIN,
                    TurkceKokOzelDurumTipi.EK_OZEL_DURUMU,
                    TurkceKokOzelDurumTipi.GENIS_ZAMAN,
                    TurkceKokOzelDurumTipi.FIIL_GECISSIZ,
                    TurkceKokOzelDurumTipi.KAYNASTIRMA_N,
                    TurkceKokOzelDurumTipi.KESMESIZ,
                    TurkceKokOzelDurumTipi.TIRE,
                    TurkceKokOzelDurumTipi.KESME,
                    TurkceKokOzelDurumTipi.OZEL_IC_KARAKTER,
                    TurkceKokOzelDurumTipi.ZAMIR_IM,
                    TurkceKokOzelDurumTipi.ZAMIR_IN,
                    TurkceKokOzelDurumTipi.KISALTMA_SON_SESLI,
                    TurkceKokOzelDurumTipi.KISALTMA_SON_SESSIZ,
                    TurkceKokOzelDurumTipi.SU_OZEL_DURUMU});
        }

        public TurkceKokOzelDurumBilgisi(IEkYonetici ekler, Alfabe alfabe):base(ekler,alfabe)
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
        if (kok.OzelDurumIceriyormu(TurkceKokOzelDurumTipi.ISIM_SESLI_DUSMESI))
            eskiSonsesliInce = hdizi.SonSesli().InceSesli;

        bool yapiBozucuOzelDurumvar = false;
        //ters sesli ozel durumu yapi bozucu ama sadece seslinin tipini degistirdiginden
        //islemeye gerek yok.
        if (kok.OzelDurumDizisi().Length == 1 && kok.OzelDurumIceriyormu(TurkceKokOzelDurumTipi.TERS_SESLI_EK))
            return new String[0];

        // kok uzerindeki ozel durumlar basta sona taranip ozel durum koke uygulaniyor.
        foreach (KokOzelDurumu _ozelDurum in kok.OzelDurumDizisi()) {
            // kucultme ozel durumunda problem var, cunku kok'te hem kucultme hem yumusama uygulaniyor.
            if (_ozelDurum == null) {
                //Console.Write("kok = " + kok);
                //Environment.Exit(-1);
                logger.Warn("null ozle durum!. Kok:" + kok);
                return new String[0];
            }
            if (!_ozelDurum.Equals(OzelDurum(TurkceKokOzelDurumTipi.KUCULTME)))
                _ozelDurum.Uygula(hdizi);
            if (_ozelDurum.YapiBozucu())
                yapiBozucuOzelDurumvar = true;
        }
        // ara sesli dusmesi durumunda dusen sesi ile dustukten sonra olusan seslinin farkli olmasi durumunda
        // kok'e bir de ters sesli ek ozel durumu ekleniyor.,
        if (kok.OzelDurumIceriyormu(TurkceKokOzelDurumTipi.ISIM_SESLI_DUSMESI)
                || kok.OzelDurumIceriyormu(TurkceKokOzelDurumTipi.FIIL_ARA_SESLI_DUSMESI))
        {
            if (!hdizi.SonSesli().InceSesli && eskiSonsesliInce)
                kok.OzelDurumEkle(ozelDurumlar[TurkceKokOzelDurumTipi.TERS_SESLI_EK]);
        }

        if (yapiBozucuOzelDurumvar)
            degismisIcerikler.Add(hdizi.ToString());

        if (kok.OzelDurumVarmi() &&
                kok.OzelDurumIceriyormu(TurkceKokOzelDurumTipi.KUCULTME) &&
                kok.OzelDurumIceriyormu(TurkceKokOzelDurumTipi.SESSIZ_YUMUSAMASI))
        {
            HarfDizisi tempDizi = new HarfDizisi(kok.Icerik, alfabe);
            OzelDurum(TurkceKokOzelDurumTipi.KUCULTME).Uygula(tempDizi);
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
                if (!kok.OzelDurumIceriyormu(TurkceKokOzelDurumTipi.FIIL_KOK_BOZULMASI))
                {
                    kok.OzelDurumEkle(ozelDurumlar[TurkceKokOzelDurumTipi.SIMDIKI_ZAMAN]);
                }
            }
        }

        public void DuzyaziOzelDurumOku(Kok kok, String okunanIcerik, String[] parcalar)
        {
            for (int i = 2; i < parcalar.Length; i++)
            {

                String _ozelDurum = parcalar[i];

                //kisaltma ozel durumunun analizi burada yapiliyor.
                if (_ozelDurum.StartsWith(TurkceKokOzelDurumTipi.KISALTMA_SON_SESLI.KisaAd))
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
                            kok.OzelDurumEkle(ozelDurumlar[TurkceKokOzelDurumTipi.KISALTMA_SON_SESSIZ]);
                        }
                        else
                            kok.OzelDurumCikar(TurkceKokOzelDurumTipi.KISALTMA_SON_SESLI);
                    }
                    else
                    {
                        char sonHarf = kok.Icerik[kok.Icerik.Length - 1];
                        if (!alfabe.Harf(sonHarf).Sesli)
                        {
                            kok.KisaltmaSonSeslisi='e';
                            kok.OzelDurumEkle(ozelDurumlar[TurkceKokOzelDurumTipi.KISALTMA_SON_SESLI]);
                        }
                    }
                    continue;
                }

                //diger ozel durumlarin elde edilmesi..
                KokOzelDurumu oz = OzelDurum(_ozelDurum);
                if (oz != null)
                {
                    kok.OzelDurumEkle(oz);
                }
                else
                {
                    logger.Warn("Hatali kok bileseni" + kok.Icerik + " Token: " + _ozelDurum);
                }
            }

            //kisaltmalari ve ozel karakter iceren kokleri asil icerik olarak ata.
            if (kok.Tip == KelimeTipi.KISALTMA || kok.OzelDurumIceriyormu(TurkceKokOzelDurumTipi.OZEL_IC_KARAKTER))
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
    }
}
