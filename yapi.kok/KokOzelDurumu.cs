using System;
using net.zemberek.yapi;
using net.zemberek.yapi.ek;
using Iesi.Collections.Generic;
using Iesi.Collections;

namespace net.zemberek.yapi.kok
{
	
	/// <summary> Koke ilskin ozel durumu ifade eder. kok ozel durumlarinin farkli ozlelikleri
	/// bu sinifta belirtilir. Dogrudan uretilmez, once Uretici ic sinifi olusturulmasi gerekir.
	/// </summary>
    public class KokOzelDurumu
    {
        /**
          * tip  bilgisi bu ozel duruma iliskin cesitli kimlik bilgilerini tasir.
          * onegin ozel durumun adi, indeksi, ek dizisi gibi.
          * dile gore farkli tip gerceklemeleri mevcttur.
          */
        private KokOzelDurumTipi _tip;

        /**
         * Kok ozel durumu bir kelime uzerinde ne tur islem yapacak bu nesne ile belirlenir.
         */
        private HarfDizisiIslemi _islem;

        /**
         * bazi ozel durumlar sadece bazi eklerin koke eklenmesi ile olusur. Bu listede
         * bu ekler yer alir.
         */
        private Set<Ek> _gelebilecekEkler = new HashedSet<Ek>();

        /**
         * Eger ozel durum kokun yapisini bozuyorsa true.
         */
        private bool _yapiBozucu = false;

        /**
         * bazi ozel durumlarin olusmasi mevburi degildir. ornegin turkiye turkceisnde
         * "icerlerde" ve "icerilerde" kelimeleri dogru kabul edilir. Eger ozel durum
         * bu sekilde ise secimlik=true olur.
         */
        private bool _secimlik = false;

        /**
         * Ek kisitlayici ozel durumlar genellikle kokun yapisini bozmaktansa kendisinden sonra
         * bazei eklerin gelmesini engeller.
         */
        private bool _ekKisitlayici = false;

        /**
         * Cogu kok bozucu ozel durum sadece kendisinden sonra sesli ile baslayan
         * bir ekin glmesi ile olusur. but bayrak bu tyur ozel durumlar icin true olur.
         */
        private bool _sesliEkIleOlusur = false;

        /**
         * Cogu kok bozucu ozel durum sadece kendisinden sonra sesli ile baslayan
         * bir ekin glmesi ile olusur. but bayrak bu tyur ozel durumlar icin true olur.
         */
        private bool _herZamanOlusur = false;


        /**
         * bu sinif KokOzelDurumu uretimi icin kullanilir. Bu sinif sayesinde
         * hem verilere dogrudan setter erisimi kisitlanir hem de esnek ilklnedirme saglanir.
         */
        public class Uretici
        {

            internal Set<Ek> _gelebilecekEkler = new HashedSet<Ek>();
            internal bool _sesliEkIleOlusur = false;
            internal bool _yapiBozucu = false;
            internal bool _secimlik = false;
            internal bool _ekKisitlayici = false;
            internal HarfDizisiIslemi _islem;
            internal KokOzelDurumTipi _tip;
            internal bool _herZamanOlusur = false;

            public Uretici(KokOzelDurumTipi tip, HarfDizisiIslemi islem)
            {
                this._tip = tip;
                this._islem = islem;
            }

            public Uretici gelebilecekEkler(Set<Ek> ekler)
            {
                this._gelebilecekEkler = ekler;
                return this;
            }

            public Uretici sesliEkIleOlusur(bool deger)
            {
                this._sesliEkIleOlusur = deger;
                return this;
            }

            public Uretici yapiBozucu(bool yapiBozucu)
            {
                this._yapiBozucu = yapiBozucu;
                return this;
            }

            public Uretici secimlik(bool secimlik)
            {
                this._secimlik = secimlik;
                return this;
            }

            public Uretici ekKisitlayici(bool ekKisitlayici)
            {
                this._ekKisitlayici = ekKisitlayici;
                return this;
            }

            public Uretici herZamanOlusur(bool herZamanOlusur)
            {
                this._herZamanOlusur = herZamanOlusur;
                return this;
            }


            public Uretici parametre(KokOzelDurumTipi tip)
            {
                this._tip = tip;
                return this;
            }

            public KokOzelDurumu uret()
            {
                return new KokOzelDurumu(this);
            }
        }

        /**
         * KokOzelDurumu uretici nesnesi uzerinden uretilir. dogrudan erisim yoktur.
         *
         * @param uretici
         */
        private KokOzelDurumu(Uretici uretici)
        {
            this._gelebilecekEkler = uretici._gelebilecekEkler;
            this._sesliEkIleOlusur = uretici._sesliEkIleOlusur;
            this._yapiBozucu = uretici._yapiBozucu;
            this._secimlik = uretici._secimlik;
            this._ekKisitlayici = uretici._ekKisitlayici;
            this._tip = uretici._tip;
            this._islem = uretici._islem;
            this._herZamanOlusur = uretici._herZamanOlusur;
        }

        public bool yapiBozucumu()
        {
            return _yapiBozucu;
        }

        public bool secimlikmi()
        {
            return _secimlik;
        }

        public bool seslikEkleolusurmu()
        {
            return _sesliEkIleOlusur;
        }

        public Set<Ek> geleibilecekEkler()
        {
            return _gelebilecekEkler;
        }


        public bool ekKisitlayiciMi()
        {
            return _ekKisitlayici;
        }

        public int indeks()
        {
            return _tip.Index;
        }

        public String kisaAd()
        {
            return _tip.KisaAd;
        }

        public KokOzelDurumTipi tip()
        {
            return _tip;
        }
        
        /**
         * giris ile gelen [dizi] harf dizisine ozel durumu uygular.
         * basit ziyaretci deseni (visitor pattern).
         *
         * @param dizi
         */
        public void uygula(HarfDizisi dizi)
        {
            _islem.uygula(dizi);
        }

        /**
         * Ozel durum giris parametresi olan ek'in bu ozel durumun olusmasina
         * izin verip vermeyeegi belirlenir.
         *
         * @param ek
         * @return gelen ek ile bu ozel durum olusabilirse true
         */
        public bool olusabilirMi(Ek ek)
        {
            if (_herZamanOlusur)
                return true;
            if (_sesliEkIleOlusur && ek.sesliIleBaslayabilirMi())
                return true;
            return _gelebilecekEkler.Contains(ek);
        }

        /**
         * esitlik kiyaslamasi sadece tip indexi ve tip adina gore yapilir.
         *
         * @param o
         * @return ayni ise true.
         */
        public bool equals(Object o) {
        if (this == o) return true;
        if (o == null || GetType() != o.GetType()) return false;

        KokOzelDurumu that = (KokOzelDurumu) o;
        if ((_tip.Index == that._tip.Index) && _tip.Ad.Equals(that._tip.Ad))
            return true;
        return false;

    }

        /**
         * sadece tip adi ve indeksine gore belirlenir.
         *
         * @return hash code.
         */
        public int hashCode()
        {
            return _tip.Ad.GetHashCode() * _tip.Index;
        }
    }
/*		
		/// <summary> tip  bilgisi bu ozel duruma iliskin cesitli kimlik bilgilerini tasir.
		/// onegin ozel durumun adi, indeksi, ek dizisi gibi.
		/// dile gore farkli tip gerceklemeleri mevcttur.
		/// </summary>
		private KokOzelDurumTipi tip_Renamed_Field;
		
		/// <summary> Kok ozel durumu bir kelime uzerinde ne tur islem yapacak bu nesne ile belirlenir.</summary>
		private HarfDizisiIslemi islem;
		
		/// <summary> bazi ozel durumlar sadece bazi eklerin koke eklenmesi ile olusur. Bu listede
		/// bu ekler yer alir.
		/// </summary>
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private Set < Ek > gelebilecekEkler = new HashSet < Ek >();
		
		/// <summary> Eger ozel durum kokun yapisini bozuyorsa true.</summary>
		private bool yapiBozucu = false;
		
		/// <summary> bazi ozel durumlarin olusmasi mevburi degildir. ornegin turkiye turkceisnde
		/// "icerlerde" ve "icerilerde" kelimeleri dogru kabul edilir. Eger ozel durum
		/// bu sekilde ise secimlik=true olur.
		/// </summary>
		private bool secimlik = false;
		
		/// <summary> Ek kisitlayici ozel durumlar genellikle kokun yapisini bozmaktansa kendisinden sonra
		/// bazei eklerin gelmesini engeller.
		/// </summary>
		private bool ekKisitlayici = false;
		
		/// <summary> Cogu kok bozucu ozel durum sadece kendisinden sonra sesli ile baslayan
		/// bir ekin glmesi ile olusur. but bayrak bu tyur ozel durumlar icin true olur.
		/// </summary>
		private bool sesliEkIleOlusur = false;
		
		/// <summary> Cogu kok bozucu ozel durum sadece kendisinden sonra sesli ile baslayan
		/// bir ekin glmesi ile olusur. but bayrak bu tyur ozel durumlar icin true olur.
		/// </summary>
		private bool herZamanOlusur = false;
		
		
		/// <summary> bu sinif KokOzelDurumu uretimi icin kullanilir. Bu sinif sayesinde
		/// hem verilere dogrudan setter erisimi kisitlanir hem de esnek ilklnedirme saglanir.
		/// </summary>
		public class Uretici
		{
			private void  InitBlock()
			{
				this.gelebilecekEkler = ekler;
				return this;
			}
			
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			private Set < Ek > gelebilecekEkler = new HashSet < Ek >();
			private bool sesliEkIleOlusur_Renamed_Field = false;
			private bool yapiBozucu_Renamed_Field = false;
			private bool secimlik_Renamed_Field = false;
			private bool ekKisitlayici_Renamed_Field = false;
			private HarfDizisiIslemi islem;
			private KokOzelDurumTipi tip;
			private bool herZamanOlusur_Renamed_Field = false;
			
			public Uretici(KokOzelDurumTipi tip, HarfDizisiIslemi islem)
			{
				this.tip = tip;
				this.islem = islem;
			}
			
			public Uretici gelebilecekEkler;
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			(Set < Ek > ekler)
			
			public virtual Uretici sesliEkIleOlusur(bool deger)
			{
				this.sesliEkIleOlusur_Renamed_Field = deger;
				return this;
			}
			
			public virtual Uretici yapiBozucu(bool yapiBozucu)
			{
				this.yapiBozucu_Renamed_Field = yapiBozucu;
				return this;
			}
			
			public virtual Uretici secimlik(bool secimlik)
			{
				this.secimlik_Renamed_Field = secimlik;
				return this;
			}
			
			public virtual Uretici ekKisitlayici(bool ekKisitlayici)
			{
				this.ekKisitlayici_Renamed_Field = ekKisitlayici;
				return this;
			}
			
			public virtual Uretici herZamanOlusur(bool herZamanOlusur)
			{
				this.herZamanOlusur_Renamed_Field = herZamanOlusur;
				return this;
			}
			
			
			public virtual Uretici parametre(KokOzelDurumTipi tip)
			{
				this.tip = tip;
				return this;
			}
			
			public virtual KokOzelDurumu uret()
			{
				return new KokOzelDurumu(this);
			}
		}
		
		/// <summary> KokOzelDurumu uretici nesnesi uzerinden uretilir. dogrudan erisim yoktur.
		/// 
		/// </summary>
		/// <param name="uretici">
		/// </param>
		private KokOzelDurumu(Uretici uretici)
		{
			this.gelebilecekEkler = uretici.gelebilecekEkler;
			this.sesliEkIleOlusur = uretici.sesliEkIleOlusur_Renamed_Field;
			this.yapiBozucu = uretici.yapiBozucu_Renamed_Field;
			this.secimlik = uretici.secimlik_Renamed_Field;
			this.ekKisitlayici = uretici.ekKisitlayici_Renamed_Field;
			this.tip_Renamed_Field = uretici.tip;
			this.islem = uretici.islem;
			this.herZamanOlusur = uretici.herZamanOlusur_Renamed_Field;
		}
		
		public virtual bool yapiBozucumu()
		{
			return yapiBozucu;
		}
		
		public virtual bool secimlikmi()
		{
			return secimlik;
		}
		
		public virtual bool seslikEkleolusurmu()
		{
			return sesliEkIleOlusur;
		}
		
		public virtual SupportClass.SetSupport geleibilecekEkler()
		{
			return gelebilecekEkler;
		}
		
		
		public virtual bool ekKisitlayiciMi()
		{
			return ekKisitlayici;
		}
		
		public virtual int indeks()
		{
			return tip_Renamed_Field.indeks();
		}
		
		public virtual System.String kisaAd()
		{
			return tip_Renamed_Field.kisaAd();
		}
		
		public virtual KokOzelDurumTipi tip()
		{
			return tip_Renamed_Field;
		}
		
		/// <summary> giris ile gelen [dizi] harf dizisine ozel durumu uygular.
		/// basit ziyaretci deseni (visitor pattern).
		/// 
		/// </summary>
		/// <param name="dizi">
		/// </param>
		public virtual void  uygula(HarfDizisi dizi)
		{
			islem.uygula(dizi);
		}
		
		/// <summary> Ozel durum giris parametresi olan ek'in bu ozel durumun olusmasina
		/// izin verip vermeyeegi belirlenir.
		/// 
		/// </summary>
		/// <param name="ek">
		/// </param>
		/// <returns> gelen ek ile bu ozel durum olusabilirse true
		/// </returns>
		public virtual bool olusabilirMi(Ek ek)
		{
			if (herZamanOlusur)
				return true;
			if (sesliEkIleOlusur && ek.sesliIleBaslayabilirMi())
				return true;
			return gelebilecekEkler.contains(ek);
		}
		
		/// <summary> esitlik kiyaslamasi sadece tip indexi ve tip adina gore yapilir.
		/// 
		/// </summary>
		/// <param name="o">
		/// </param>
		/// <returns> ayni ise true.
		/// </returns>
		public  override bool Equals(System.Object o)
		{
			if (this == o)
				return true;
			if (o == null || GetType() != o.GetType())
				return false;
			
			//UPGRADE_NOTE: Final was removed from the declaration of 'that '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			KokOzelDurumu that = (KokOzelDurumu) o;
			if ((tip_Renamed_Field.indeks() == that.tip_Renamed_Field.indeks()) && tip_Renamed_Field.ad().Equals(that.tip_Renamed_Field.ad()))
				return true;
			return false;
		}
		
		/// <summary> sadece tip adi ve indeksine gore belirlenir.
		/// 
		/// </summary>
		/// <returns> hash code.
		/// </returns>
		public override int GetHashCode()
		{
			return tip_Renamed_Field.ad().GetHashCode() * tip_Renamed_Field.indeks();
		}
	}
*/
}