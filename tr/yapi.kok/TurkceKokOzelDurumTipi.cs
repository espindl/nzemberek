using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi.kok;
using net.zemberek.tr.yapi.ek;

namespace net.zemberek.tr.yapi.kok
{
    public class TurkceKokOzelDurumTipi : KokOzelDurumTipi
    {
        private string _kisaAd;
        private string _ad = string.Empty;
        private string[] _ekAdlari = new string[0];
        private int _index;

        private TurkceKokOzelDurumTipi(int index, string kisaAd, string ad, string[] ekAdlari)
        {
            this._kisaAd = kisaAd;
            this._ad = ad;
            this._ekAdlari = ekAdlari;
            this._index = index;
        }

        private TurkceKokOzelDurumTipi(int index, string kisaAd, string ad)
        {
            this._kisaAd = kisaAd;
            this._ad = ad;
            this._index = index;
        }

        private TurkceKokOzelDurumTipi(int index, string kisaAd, string[] ekAdlari)
        {
            this._kisaAd = kisaAd;
            this._ekAdlari = ekAdlari;
            this._index = index;
        }

        private TurkceKokOzelDurumTipi(int index, string kisaAd)
        {
            this._kisaAd = kisaAd;
            this._index = index;
        }

        public string KisaAd
        {
            get { return _kisaAd; }
        }

        public string Ad
        {
            get { return _ad; }
        }

        public string[] EkAdlari
        {
            get { return _ekAdlari; }
        }

        public int Index
        {
            get { return _index; }
        }

        public readonly static TurkceKokOzelDurumTipi SESSIZ_YUMUSAMASI = new TurkceKokOzelDurumTipi(0,"YUM");
        public readonly static TurkceKokOzelDurumTipi SESSIZ_YUMUSAMASI_NK = new TurkceKokOzelDurumTipi(1,"YUM_NK");
        public readonly static TurkceKokOzelDurumTipi ISIM_SESLI_DUSMESI = new TurkceKokOzelDurumTipi(2,"DUS", new string[]{
                                                                                    TurkceEkAdlari.ISIM_YONELME_E,
                                                                                    TurkceEkAdlari.ISIM_BELIRTME_I,
                                                                                    TurkceEkAdlari.ISIM_SAHIPLIK_BEN_IM,
                                                                                    TurkceEkAdlari.ISIM_SAHIPLIK_SEN_IN,
                                                                                    TurkceEkAdlari.ISIM_SAHIPLIK_SIZ_INIZ,
                                                                                    TurkceEkAdlari.ISIM_SAHIPLIK_O_I,
                                                                                    TurkceEkAdlari.ISIM_SAHIPLIK_BIZ_IMIZ,
                                                                                    TurkceEkAdlari.ISIM_TAMLAMA_IN});
        public readonly static TurkceKokOzelDurumTipi CIFTLEME = new TurkceKokOzelDurumTipi(3,"CIFT");
        public readonly static TurkceKokOzelDurumTipi FIIL_ARA_SESLI_DUSMESI = new TurkceKokOzelDurumTipi(4,"DUS_FI", new string[] { TurkceEkAdlari.FIIL_EDILGEN_IL });    
        public readonly static TurkceKokOzelDurumTipi KUCULTME = new TurkceKokOzelDurumTipi(5,"KUCULTME",new string[]{TurkceEkAdlari.ISIM_KUCULTME_CIK});
        public readonly static TurkceKokOzelDurumTipi TEKIL_KISI_BOZULMASI = new TurkceKokOzelDurumTipi(6,"TEKIL_KISI_BOZULMASI",new string[]{TurkceEkAdlari.ISIM_YONELME_E});
        public readonly static TurkceKokOzelDurumTipi FIIL_KOK_BOZULMASI = new TurkceKokOzelDurumTipi(7,"FIIL_KOK_BOZULMASI",new string[]{
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
                                                TurkceEkAdlari.FIIL_EMIR_SIZRESMI_INIZ});
        public readonly static TurkceKokOzelDurumTipi TERS_SESLI_EK = new TurkceKokOzelDurumTipi(8,"TERS");
        public readonly static TurkceKokOzelDurumTipi SIMDIKI_ZAMAN = new TurkceKokOzelDurumTipi(9,"SDK_ZAMAN",new string[]{TurkceEkAdlari.FIIL_SIMDIKIZAMAN_IYOR});
        public readonly static TurkceKokOzelDurumTipi ISIM_SON_SESLI_DUSMESI = new TurkceKokOzelDurumTipi(10,"DUS_SON",new string[]{
                                                TurkceEkAdlari.ISIM_KALMA_DE,
                                                TurkceEkAdlari.ISIM_CIKMA_DEN,
                                                TurkceEkAdlari.ISIM_COGUL_LER});
        
        public readonly static TurkceKokOzelDurumTipi SU_OZEL_DURUMU = new TurkceKokOzelDurumTipi(11,"FARKLI_KAYNASTIRMA",new string[]{
                                                TurkceEkAdlari.ISIM_SAHIPLIK_BEN_IM,
                                                TurkceEkAdlari.ISIM_SAHIPLIK_SEN_IN,
                                                TurkceEkAdlari.ISIM_SAHIPLIK_O_I,
                                                TurkceEkAdlari.ISIM_SAHIPLIK_BIZ_IMIZ,
                                                TurkceEkAdlari.ISIM_SAHIPLIK_SIZ_INIZ});
        
        public readonly static TurkceKokOzelDurumTipi ZAMIR_SESLI_OZEL = new TurkceKokOzelDurumTipi(12,"ZAMIR_SESLI_OZEL",new string[]{
                                                TurkceEkAdlari.ISIM_YONELME_E,
                                                TurkceEkAdlari.ISIM_KALMA_DE,
                                                TurkceEkAdlari.ISIM_CIKMA_DEN,
                                                TurkceEkAdlari.ISIM_BELIRTME_I,
                                                TurkceEkAdlari.ISIM_TAMLAMA_IN,
                                                TurkceEkAdlari.ISIM_YOKLUK_SIZ,
                                                TurkceEkAdlari.ISIM_COGUL_LER,
                                                TurkceEkAdlari.ISIM_TARAFINDAN_CE,
                                                TurkceEkAdlari.ISIM_DURUM_LIK,
                                                TurkceEkAdlari.ISIM_KUCULTME_CEGIZ});
        
        /*
         * bazi kokler aslinda saf kok degil, icinde isim tamlamasi iceriyor
         * mesela, zeytinyagi, acemborusu gibi. bu koklere bazi ekler eklendiginde kok bozuluyor
         * mesela: acemborusu -> acemborulari seklinde. bu kokler sistemde ozel sekilde saklaniyor.
         * acemborusu -> acemboru IS_TAM seklinde tanimlanmistir. ayni sekilde zeytinyag IS_TAM gibi
         */    
        public readonly static TurkceKokOzelDurumTipi ISIM_TAMLAMASI = new TurkceKokOzelDurumTipi(13,"IS_TAM",new string[]{
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
                                                TurkceEkAdlari.ISIM_ILGI_CI});        
        public readonly static TurkceKokOzelDurumTipi YALIN = new TurkceKokOzelDurumTipi(14,"YAL");
        public readonly static TurkceKokOzelDurumTipi GENIS_ZAMAN = new TurkceKokOzelDurumTipi(15,"GEN");
        public readonly static TurkceKokOzelDurumTipi EK_OZEL_DURUMU = new TurkceKokOzelDurumTipi(16,"EK");
        public readonly static TurkceKokOzelDurumTipi FIIL_GECISSIZ = new TurkceKokOzelDurumTipi(17,"GEC");
        public readonly static TurkceKokOzelDurumTipi KAYNASTIRMA_N = new TurkceKokOzelDurumTipi(18,"KAYNASTIRMA_N");
        public readonly static TurkceKokOzelDurumTipi KESMESIZ = new TurkceKokOzelDurumTipi(19,"TERS");
        public readonly static TurkceKokOzelDurumTipi TIRE = new TurkceKokOzelDurumTipi(20,"TIRE");
        public readonly static TurkceKokOzelDurumTipi KESME = new TurkceKokOzelDurumTipi(21,"KESME");
        public readonly static TurkceKokOzelDurumTipi OZEL_IC_KARAKTER = new TurkceKokOzelDurumTipi(22,"OZEL_IC_KARAKTER");
        public readonly static TurkceKokOzelDurumTipi ZAMIR_IM = new TurkceKokOzelDurumTipi(23,"ZAMIR_IM");
        public readonly static TurkceKokOzelDurumTipi ZAMIR_IN = new TurkceKokOzelDurumTipi(24,"ZAMIR_IN");
        public readonly static TurkceKokOzelDurumTipi KISALTMA_SON_SESLI = new TurkceKokOzelDurumTipi(25,"SON");
        public readonly static TurkceKokOzelDurumTipi KISALTMA_SON_SESSIZ = new TurkceKokOzelDurumTipi(26,"SON_N");
    }
}
