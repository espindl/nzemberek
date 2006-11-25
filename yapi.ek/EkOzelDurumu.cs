using System;
using System.Collections;
using System.Collections.Generic;
using Iesi.Collections;
using Iesi.Collections.Generic;
using net.zemberek.islemler.cozumleme;

namespace net.zemberek.yapi.ek
{
	
	/// <summary> Ek ozel durumu ek'e benzer bir yapiya sahiptir. Farkli olarak bazi ozel durumlarda yer alan
	/// onek listesi de bu sinifin bir parametresidir.
	/// User: ahmet
	/// Date: Aug 24, 2005
	/// </summary>
    public abstract class EkOzelDurumu
    {
        public static readonly Set EMPTY_SET = new HashedSet();// (System.Collections.IList)System.Collections.ArrayList.ReadOnly(new System.Collections.ArrayList());

        protected String _ad;
        protected Set onEkler = EMPTY_SET;
        protected EkUretici ekUretici;
        protected List<EkUretimBileseni> _uretimBilesenleri;

        public abstract HarfDizisi cozumlemeIcinUret(Kelime kelime, HarfDizisi giris, HarfDizisiKiyaslayici kiyaslayici);

        public HarfDizisi olusumIcinUret(Kelime kelime, Ek sonrakiEk)
        {
            return ekUretici.olusumIcinEkUret(kelime.icerik(), sonrakiEk, _uretimBilesenleri);
        }

        public String ad()
        {
            return _ad;
        }

        public void setAd(String ad)
        {
            this._ad = ad;
        }

        public Set getOnEkler()
        {
            return onEkler;
        }

        public void setOnEkler(Set onEkler)
        {
            this.onEkler = onEkler;
        }

        public void setEkKuralCozumleyici(EkUretici ekUretici)
        {
            this.ekUretici = ekUretici;
        }

        public void setUretimBilesenleri(List<EkUretimBileseni> uretimBilesenleri)
        {
            this._uretimBilesenleri = uretimBilesenleri;
        }

        public List<EkUretimBileseni> uretimBilesenleri()
        {
            return _uretimBilesenleri;
        }
    }
}