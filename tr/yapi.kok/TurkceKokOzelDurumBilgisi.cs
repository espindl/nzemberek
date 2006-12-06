using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;
using net.zemberek.yapi.ek;
using net.zemberek.yapi.kok;

namespace net.zemberek.tr.yapi.kok
{
    public class TurkceKokOzelDurumBilgisi : TemelKokOzelDurumBilgisi,KokOzelDurumBilgisi
    {

        /**
         * KokOzel durum nesneleri burada uretilir. uretim icin "uretici" metodu kullanilir. bu metod KokOzelDurumu sinifi
         * icinde yer alan Uretim sinifindan bir nesne dondurur. Bu nesne uzerinden kok izel durumu parametreleri belirlenir
         * ve ekle sinifi icinde kokOzelDurumu nesnesi uretilip cesitli map, dizi vs tasiyici parametrelere atanir.
         */
        private void uret()
        {
            ekle(uretici(TurkceKokOzelDurumTipi.SESSIZ_YUMUSAMASI, new Yumusama()).sesliEkIleOlusur(true).yapiBozucu(true));

            ekle(uretici(TurkceKokOzelDurumTipi.SESSIZ_YUMUSAMASI_NK, new YumusamaNk(alfabe)).sesliEkIleOlusur(true).yapiBozucu(true));

            ekle(uretici(TurkceKokOzelDurumTipi.ISIM_SESLI_DUSMESI, new AraSesliDusmesi()).yapiBozucu(true));

            ekle(uretici(TurkceKokOzelDurumTipi.CIFTLEME, new Ciftleme()).
                    sesliEkIleOlusur(true).
                    yapiBozucu(true));

            ekle(uretici(TurkceKokOzelDurumTipi.FIIL_ARA_SESLI_DUSMESI, new AraSesliDusmesi()).yapiBozucu(true));

            ekle(uretici(TurkceKokOzelDurumTipi.KUCULTME, new SonHarfDusmesi()).yapiBozucu(true));

            IDictionary<String, String> benSenDonusum = new Dictionary<String,String>();

            benSenDonusum.Add("ben", "ban");
            benSenDonusum.Add("sen", "san");
            ekle(uretici(TurkceKokOzelDurumTipi.TEKIL_KISI_BOZULMASI, new YeniIcerikAta(alfabe, benSenDonusum)).yapiBozucu(true));

            IDictionary<String, String> deYeDonusum = new Dictionary<String,String>();
            deYeDonusum.Add("de", "di");
            deYeDonusum.Add("ye", "yi");
            ekle(uretici(TurkceKokOzelDurumTipi.FIIL_KOK_BOZULMASI, new YeniIcerikAta(alfabe, deYeDonusum)).yapiBozucu(true));

            ekle(uretici(TurkceKokOzelDurumTipi.TERS_SESLI_EK, new SonSesliIncelt(alfabe)).
                    yapiBozucu(true).
                    herZamanOlusur(true));

            ekle(uretici(TurkceKokOzelDurumTipi.SIMDIKI_ZAMAN, new SonHarfDusmesi()).yapiBozucu(true));

            ekle(uretici(TurkceKokOzelDurumTipi.ISIM_SON_SESLI_DUSMESI, new SonHarfDusmesi()).
                    secimlik(true).
                    yapiBozucu(true));

            HarfDizisi yu = new HarfDizisi("yu", alfabe);
            ekle(uretici(TurkceKokOzelDurumTipi.SU_OZEL_DURUMU, new Ulama(yu)).yapiBozucu(true));

            HarfDizisi n = new HarfDizisi("n", alfabe);
            ekle(uretici(TurkceKokOzelDurumTipi.ZAMIR_SESLI_OZEL, new Ulama(n)).yapiBozucu(true));

            ekle(uretici(TurkceKokOzelDurumTipi.ISIM_TAMLAMASI, new BosHarfDizisiIslemi()).ekKisitlayici(true));

            bosOzelDurumEkle(new KokOzelDurumTipi[]{TurkceKokOzelDurumTipi.YALIN,
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
                    TurkceKokOzelDurumTipi.KISALTMA_SON_SESSIZ});
        }

        public TurkceKokOzelDurumBilgisi(EkYonetici ekler, Alfabe alfabe):base(ekler,alfabe)
        {
            uret();
        }

        public String[] ozelDurumUygula(Kok kok) {
        //kok icinde ozel durum yok ise cik..
        if (!kok.ozelDurumVarmi())
            return new String[0];

        HarfDizisi hdizi = new HarfDizisi(kok.icerik(), alfabe);

        IList degismisIcerikler = new ArrayList(1);

        //ara sesli dusmesi nedeniyle bazen yapay oarak kok'e ters sesli etkisi ozel durumunun eklenmesi gerekir.
        // nakit -> nakde seklinde. normal kosullarda "nakda" olusmasi gerekirdi.
        bool eskiSonsesliInce = false;
        if (kok.ozelDurumIceriyormu(TurkceKokOzelDurumTipi.ISIM_SESLI_DUSMESI))
            eskiSonsesliInce = hdizi.sonSesli().inceSesliMi();

        bool yapiBozucuOzelDurumvar = false;
        // kok uzerindeki ozel durumlar basta sona taranip ozel durum koke uygulaniyor.
        foreach (KokOzelDurumu _ozelDurum in kok.ozelDurumDizisi()) {
            // kucultme ozel durumunda problem var, cunku kok'te hem kucultme hem yumusama uygulaniyor.
            if (_ozelDurum == null) {
                Console.Write("kok = " + kok);
                Environment.Exit(-1);
            }
            if (!_ozelDurum.Equals(ozelDurum(TurkceKokOzelDurumTipi.KUCULTME)))
                _ozelDurum.uygula(hdizi);
            if (_ozelDurum.yapiBozucumu())
                yapiBozucuOzelDurumvar = true;
        }
        // ara sesli dusmesi durumunda dusen sesi ile dustukten sonra olusan seslinin farkli olmasi durumunda
        // kok'e bir de ters sesli ek ozel durumu ekleniyor.,
        if (kok.ozelDurumIceriyormu(TurkceKokOzelDurumTipi.ISIM_SESLI_DUSMESI)
                || kok.ozelDurumIceriyormu(TurkceKokOzelDurumTipi.FIIL_ARA_SESLI_DUSMESI))
        {
            if (!hdizi.sonSesli().inceSesliMi() && eskiSonsesliInce)
                kok.ozelDurumEkle(ozelDurumlar[TurkceKokOzelDurumTipi.TERS_SESLI_EK]);
        }

        if (yapiBozucuOzelDurumvar)
            degismisIcerikler.Add(hdizi.ToString());

        if (kok.ozelDurumVarmi() &&
                kok.ozelDurumIceriyormu(TurkceKokOzelDurumTipi.KUCULTME) &&
                kok.ozelDurumIceriyormu(TurkceKokOzelDurumTipi.SESSIZ_YUMUSAMASI))
        {
            HarfDizisi tempDizi = new HarfDizisi(kok.icerik(), alfabe);
            ozelDurum(TurkceKokOzelDurumTipi.KUCULTME).uygula(tempDizi);
            degismisIcerikler.Add(tempDizi.ToString());
        }
        // yani ozel durumlar eklenmis olabileceginden koke ods'u tekrar koke esle.
        String[] tempArr = new String[degismisIcerikler.Count];
        degismisIcerikler.CopyTo(tempArr,0);
        return tempArr;
    }

        public void ozelDurumBelirle(Kok kok)
        {
            // eger bir fiilin son harfi sesli ise bu dogrudan simdiki zaman ozel durumu olarak ele alinir.
            // bu ozel durum bilgi tabaninda ayrica belirtilmedigi icin burada kok'e eklenir.  aramak -> ar(a)Iyor
            char sonChar = kok.icerik()[kok.icerik().Length - 1];
            if (kok.tip() == KelimeTipi.FIIL && alfabe.harf(sonChar).sesliMi())
            {
                //demek-yemek fiilleri icin bu uygulama yapilamaz.
                if (!kok.ozelDurumIceriyormu(TurkceKokOzelDurumTipi.FIIL_KOK_BOZULMASI))
                {
                    kok.ozelDurumEkle(ozelDurumlar[TurkceKokOzelDurumTipi.SIMDIKI_ZAMAN]);
                }
            }
        }

        public void duzyaziOzelDurumOku(Kok kok, String okunanIcerik, String[] parcalar)
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
                        if (!alfabe.harf(sonSesli).sesliMi())
                            logger.Warn("Hatali kisaltma harfi.. Sesli bekleniyordu." + _ozelDurum);
                        kok.KisaltmaSonSeslisi = sonSesli;
                        if (parca.Length > 1)
                        {
                            kok.ozelDurumEkle(ozelDurumlar[TurkceKokOzelDurumTipi.KISALTMA_SON_SESSIZ]);
                        }
                        else
                            kok.ozelDurumCikar(TurkceKokOzelDurumTipi.KISALTMA_SON_SESLI);
                    }
                    else
                    {
                        char sonHarf = kok.icerik()[kok.icerik().Length - 1];
                        if (!alfabe.harf(sonHarf).sesliMi())
                        {
                            kok.KisaltmaSonSeslisi='e';
                            kok.ozelDurumEkle(ozelDurumlar[TurkceKokOzelDurumTipi.KISALTMA_SON_SESLI]);
                        }
                    }
                    continue;
                }

                //diger ozel durumlarin elde edilmesi..
                KokOzelDurumu oz = ozelDurum(_ozelDurum);
                if (oz != null)
                {
                    kok.ozelDurumEkle(oz);
                }
                else
                {
                    logger.Warn("Hatali kok bileseni" + kok.icerik() + " Token: " + _ozelDurum);
                }
            }

            //kisaltmalari ve ozel karakter iceren kokleri asil icerik olarak ata.
            if (kok.tip() == KelimeTipi.KISALTMA || kok.ozelDurumIceriyormu(TurkceKokOzelDurumTipi.OZEL_IC_KARAKTER))
                kok.Asil=okunanIcerik;
        }

        public void kokIcerikIsle(Kok kok, KelimeTipi tip, String icerik)
        {
            //tip kisaltma ise ya da icerik ozel karakterler iceriyorsa bunu kok'un asil haline ata.
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
