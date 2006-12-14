// V 0.1
using System;
using net.zemberek.yapi.ek;
using net.zemberek.yapi.kok;

namespace net.zemberek.yapi
{
	public class Kok
	{
		virtual public KelimeTipi Tip
		{
			set
			{
				this._tip = value;
			}
			
		}
		virtual public System.String Icerik
		{
			set
			{
				this._icerik = value;
			}
			
		}
		virtual public int Indeks
		{
			get
			{
				return indeks;
			}
			
			set
			{
				this.indeks = value;
			}
			
		}
		virtual public int Frekans
		{
			get
			{
				return frekans;
			}
			
			set
			{
				this.frekans = value;
			}
			
		}
		virtual public System.String Asil
		{
			set
			{
				this.asil_Renamed_Field = value;
			}
			
		}
		virtual public char KisaltmaSonSeslisi
		{
			get
			{
				return kisaltmaSonSeslisi;
			}
			
			set
			{
				this.kisaltmaSonSeslisi = value;
			}
			
		}
		
		private static readonly KokOzelDurumu[] BOS_OZEL_DURUM_DIZISI = new KokOzelDurumu[0];
		
		private int indeks;
		// Eger bir kok icinde alfabe disi karakter barindiriyorsa (nokta, tire gibi) orjinal hali bu
		// String icinde yer alir. Aksi halde null.
		private System.String asil_Renamed_Field;
		// bazi kisaltmalara ek eklenebilmesi icin kisaltmanin asil halinin son seslisine ihtiyac duyulur.
		private char kisaltmaSonSeslisi=char.MinValue;
		// Kok'un ozel karakterlerden tmeizlenmis hali. Genel olarak kok icerigi olarak bu String kullanilir.
		private System.String _icerik;
		private KelimeTipi _tip=KelimeTipi.YOK;
		//performans ve kaynak tuketimini nedeniyle icin ozel durumlari Set yerine diziye koyduk.
		private KokOzelDurumu[] ozelDurumlar = BOS_OZEL_DURUM_DIZISI;
		
		private int frekans;
		
		public virtual bool ozelDurumVarmi()
		{
			return ozelDurumlar.Length > 0;
		}
		
		public virtual KokOzelDurumu[] ozelDurumDizisi()
		{
			return ozelDurumlar;
		}
		
		public virtual bool ozelDurumIceriyormu(KokOzelDurumTipi tip)
		{            
            foreach (KokOzelDurumu oz in ozelDurumlar)
            {
                if (oz.indeks() == tip.Index) // TODO Buraya oz!=null yazmazsam hata alýyordu bakalým
                    return true;
            }
			return false;
		}
		
		/// <summary> koke ozel durum ekler. burada dizi kullaniminda kaynak konusunda cimrilik ettigimizden
		/// her yeni ozel durum icin dizi boyutunu bir buyuttuk. ayrica tekrar olmamasini da sagliyoruz.
		/// Normalde bu islem Set icin cok daha kolay bir yapida olabilirdi set.add() ancak Set'in kaynak tuketimi
		/// diziden daha fazla.
		/// 
		/// </summary>
		/// <param name="ozelDurum">
		/// </param>
		public virtual void  ozelDurumEkle(KokOzelDurumu ozelDurum)
		{
			if (ozelDurumlar.Length == 0)
			{
				ozelDurumlar = new KokOzelDurumu[1];
				ozelDurumlar[0] = ozelDurum;
			}
			else
			{
				if (ozelDurumIceriyormu(ozelDurum.tip()))
					return ;
				KokOzelDurumu[] yeni = new KokOzelDurumu[ozelDurumlar.Length + 1];
				for (int i = 0; i < ozelDurumlar.Length; i++)
				{
					yeni[i] = ozelDurumlar[i];
				}
				yeni[ozelDurumlar.Length] = ozelDurum;
				this.ozelDurumlar = yeni;
			}
		}
		
		/// <summary> sadece ilk acilista kullanilan bir metod
		/// 
		/// </summary>
		/// <param name="tip">
		/// </param>
		public virtual void  ozelDurumCikar(KokOzelDurumTipi tip)
		{
			if (!ozelDurumIceriyormu(tip))
				return ;
			KokOzelDurumu[] yeni = new KokOzelDurumu[ozelDurumlar.Length - 1];
			int j = 0;
			foreach(KokOzelDurumu oz in ozelDurumlar)
			{
				if (!oz.tip().Equals(tip))
					yeni[j++] = oz;
			}
			this.ozelDurumlar = yeni;
		}
		
		public Kok(System.String icerik)
		{
			this._icerik = icerik;
		}
		
		public Kok(System.String icerik, KelimeTipi tip)
		{
			this._icerik = icerik;
			this._tip = tip;
		}
		
		public override System.String ToString()
		{
			System.String strOzel = "";
			foreach(KokOzelDurumu ozelDurum in ozelDurumlar)
			{
				if (ozelDurum != null)
					strOzel += (ozelDurum.kisaAd() + " ");
			}
			return _icerik + " " + _tip + " " + strOzel;
		}
		
		public virtual HarfDizisi ozelDurumUygula(Alfabe alfabe, Ek ek)
		{
			HarfDizisi dizi = new HarfDizisi(this._icerik, alfabe);
			foreach(KokOzelDurumu ozelDurum in ozelDurumlar)
			{
				if (ozelDurum.yapiBozucumu() && ozelDurum.olusabilirMi(ek))
					ozelDurum.uygula(dizi);
				if (!ozelDurum.olusabilirMi(ek) && ozelDurum.ekKisitlayiciMi())
					return null;
			}
			return dizi;
		}
		
		public virtual bool yapiBozucuOzelDurumVarmi()
		{
			if (ozelDurumlar.Length == 0)
				return false;
			foreach(KokOzelDurumu ozelDurum in ozelDurumlar)
			{
				if (ozelDurum.yapiBozucumu()) //TODO Buraya da != null eklemek zorunda kaldým mert (bakacaðým anlamadým)
					return true;
			}
			return false;
		}
		
		public KelimeTipi tip()
		{
			return _tip;
		}
		
		public System.String icerik()
		{
			return _icerik;
		}
		
		public  override bool Equals(System.Object o)
		{
			if (this == o)
				return true;
			if (o == null || GetType() != o.GetType())
				return false;
			
			Kok kok = (Kok) o;
			
			if (_icerik != null?!_icerik.Equals(kok._icerik):kok._icerik != null)
				return false;
			if (ozelDurumlar != null?!ozelDurumlar.Equals(kok.ozelDurumlar):kok.ozelDurumlar != null)
				return false;
            if (TipVarmi() ? !_tip.Equals(kok._tip) : kok.TipVarmi())
				return false;
			
			return true;
		}
		
		public override int GetHashCode()
		{
			int result;
			result = (_icerik != null?_icerik.GetHashCode():0);
            result = 29 * result + (TipVarmi() ? _tip.GetHashCode() : 0);
			result = 29 * result + (ozelDurumlar != null?ozelDurumlar.GetHashCode():0);
			return result;
		}
		
		public virtual System.String asil()
		{
			return asil_Renamed_Field;
		}

        protected bool TipVarmi()
        {
            return (_tip != KelimeTipi.YOK);
        }
	}
}