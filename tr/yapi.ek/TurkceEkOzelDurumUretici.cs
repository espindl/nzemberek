using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using net.zemberek.yapi;
using net.zemberek.yapi.ek;

namespace net.zemberek.tr.yapi.ek
{
public class TurkceEkOzelDurumUretici : TemelEkOzelDurumUretici {

    private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);		

    public TurkceEkOzelDurumUretici(Alfabe alfabe) {
        this.alfabe = alfabe;
    }

    class TurkceEkOzelDurumTipi : EkOzelDurumTipi 
    {
        private string _ad = string.Empty;
        private int _index;
        public string Ad
        {
          get { return _ad; }
        }
        private TurkceEkOzelDurumTipi(int index,string ad)
        {
            this._index = index;
            this._ad = ad;
        }

        public readonly static TurkceEkOzelDurumTipi BERABERLIK_IS = new TurkceEkOzelDurumTipi(0,"BERABERLIK_IS");
        public readonly static TurkceEkOzelDurumTipi EDILGEN = new TurkceEkOzelDurumTipi(1,"EDILGEN");
        public readonly static TurkceEkOzelDurumTipi GENIS_ZAMAN = new TurkceEkOzelDurumTipi(2,"GENIS_ZAMAN");
        public readonly static TurkceEkOzelDurumTipi SIMDIKI_ZAMAN = new TurkceEkOzelDurumTipi(3,"SIMDIKI_ZAMAN");
        public readonly static TurkceEkOzelDurumTipi ZAMAN_KI = new TurkceEkOzelDurumTipi(4,"ZAMAN_KI");

        public static EkOzelDurumTipi[] values()
        {
            //TODO Mert böyle olmaz bu aman ha
            return new EkOzelDurumTipi[]{BERABERLIK_IS,EDILGEN,GENIS_ZAMAN,SIMDIKI_ZAMAN,ZAMAN_KI};
        }

        public int Index
        {
            get { return _index; }
        }
    }

    public EkOzelDurumu uret(String ad) {
        EkOzelDurumu oz = base.uret(ad);
        if (oz != null)
            return oz;

        if (!mevcut(TurkceEkOzelDurumTipi.values(), ad)) {
            logger.Fatal("Ozel durum adina karsilik dusen ek ozel durum tipi bulunamadi:" + ad);
            return null;
        }

        switch (ad) {
            case "BERABERLIK_IS":
                return new BeraberlikIsOzelDurumu();
            case "EDILGEN":
                return new EdilgenOzelDurumu(alfabe);
            case "GENIS_ZAMAN":
                return new GenisZamanEkOzelDurumuTr();
            case "SIMDIKI_ZAMAN":
                return new SimdikiZamanEkOzelDurumuTr(alfabe);
            case "ZAMAN_KI":
                return new ZamanKiOzelDurumu();
        }
        return oz;
    }
}
}
