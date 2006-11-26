using System;
using System.Text;
using net.zemberek.javaporttemp;

namespace net.zemberek.yapi
{
	/// <summary>
	/// Bu sinif Dil islemleri sirasinda Turkceye ozel islemler gerektiginden String-StringBuffer yerine kullanilir.
	/// String gibi genel bir tasiyici degil ara islem nesnesi olarak kullanilmasi onerilir.
	/// String'den farkli olarak "degistirilebilir" bir yapidadir ve Thread-safe degildir.
	/// </summary>
	public class HarfDizisi : CharSequence
	{
		private TurkceHarf[] dizi;
		private int boy = 0;
		
		/// <summary> default constructor. 7 boyutlu bir TurkceHarf referans dizisi olusturur.</summary>
		public HarfDizisi()
		{
			dizi = new TurkceHarf[7];
		}
		
		/// <summary> 'kapasite' boyutlu 'TurkceHarf' dizisine sahip nesne olusturur.
		/// 
		/// </summary>
		/// <param name="kapasite">baslangic olusan TurkceHarf[] boyu
		/// </param>
		public HarfDizisi(int kapasite)
		{
			dizi = new TurkceHarf[kapasite];
		}
		
		/// <summary> 'kapasite' boyutlu 'TurkceHarf' dizisine sahip nesne olusturur. daha sonra
		/// girisi String'i icindeki karakterleri TurkceHarf seklinde TurkceHarf dizisine aktarir.
		/// Eger String boyu kapasiteden buyukse kapasite'yi boy'a esitler.
		/// Eger String icindeki karakter Alfabe'de yar almiyorsa "TANIMSIZ_HARF" harfi olarak eklenir.
	
		/// </summary>
		/// <param name="str">ornek alincak String
		/// </param>
		/// <param name="kapasite">baslangic olusan TurkceHarf[] boyu
		/// </param>
		/// <param name="alfabe">ilgili alfabe
		/// </param>
		public HarfDizisi(System.String str, Alfabe alfabe, int kapasite)
		{
			if (kapasite < str.Length)
				kapasite = str.Length;
			dizi = new TurkceHarf[kapasite];
			boy = str.Length;
			for (int i = 0; i < boy; i++)
				dizi[i] = alfabe.harf(str[i]);
		}
		
		/// <summary> 
        /// Belirlenen alfabe ile String icerigini Harflere donusturur.
		/// </summary>
		/// <param name="str">ornek alincak String
		/// </param>
		/// <param name="alfabe">ilgili alfabe
		/// </param>
		public HarfDizisi(System.String str, Alfabe alfabe)
		{
			boy = str.Length;
			dizi = new TurkceHarf[boy];
			for (int i = 0; i < boy; i++)
				dizi[i] = alfabe.harf(str[i]);
		}
		
		/// <summary> 
        /// Copy-Constructor. gelen harf dizisi ile ayni icerige sahip olacak sekilde TurkceHarf dizisi olusturur.
		/// </summary>
		/// <param name="hdizi">ornek alinacak HarfDizisi
		/// </param>
		public HarfDizisi(HarfDizisi hdizi)
		{
			boy = hdizi.length();
			dizi = new TurkceHarf[boy];
			Array.Copy(hdizi.dizi, 0, dizi, 0, boy);
		}
		
		/// <summary> 
		/// gelen TurkceHarf dizisini icerige kopyalar.
		/// </summary>
		/// <param name="dizi">kopyalancak TurkceHarf dizisi.
		/// </param>
		private HarfDizisi(TurkceHarf[] dizi)
		{
			boy = dizi.Length;
			dizi = new TurkceHarf[boy];
			Array.Copy(dizi, 0, dizi, 0, boy);
		}
		
		/// <summary> 
        /// bu metod harf referansi dizisini serbest birakmaz, sadece boyu sifira indirir.
		/// </summary>
		public virtual void  sil()
		{
			boy = 0;
		}
		
		/// <summary> 
		/// Dizinin son harfini dondurur.
		/// </summary>
		/// <returns> varsa son harf, Yoksa TANIMSIZ_HARF.
		/// </returns>
		public TurkceHarf sonHarf()
		{
			if (boy > 0)
				return dizi[boy - 1];
			else
				return Alfabe.TANIMSIZ_HARF;
		}
		
		/// <summary> 
		/// dizideki son sesliyi dondurur. eger dizi boyu 0 ise ya da sesli harf yoksa TANIMSIZ_HARF doner.
		/// </summary>
		/// <returns> varsa son sesli yoksa TANIMSIZ_HARF
		/// </returns>
		public TurkceHarf sonSesli()
		{
			for (int i = boy - 1; i >= 0; i--)
			{
				if (dizi[i].sesliMi())
					return dizi[i];
			}
			return Alfabe.TANIMSIZ_HARF;
		}
		
		/// <summary> 
        /// ic metod. harf dizisinin boyutu yetersiz geldiginde "ek" miktarinda daha
		/// fazla yere sahip yeni dizi olusturulup icerik yeni diziye kopyalanir.
		/// </summary>
		/// <param name="ek">eklenecek HarfDizisi miktari.
		/// </param>
		private void  kapasiteAyarla(int ek)
		{
			TurkceHarf[] yeniDizi = new TurkceHarf[dizi.Length + ek];
			Array.Copy(dizi, 0, yeniDizi, 0, dizi.Length);
			dizi = yeniDizi;
		}
		
		/// <summary> otomatik kapasite ayarlama. dizi boyu iki katina cikarilir.</summary>
		private void  kapasiteAyarla()
		{
			TurkceHarf[] yeniDizi = new TurkceHarf[dizi.Length * 2];
			Array.Copy(dizi, 0, yeniDizi, 0, dizi.Length);
			dizi = yeniDizi;
		}
		
		/// <summary> 
		/// kelimenin sonuna harf ekler.
		/// </summary>
		/// <param name="harf">eklenecek harf
		/// </param>
		/// <returns> this
		/// </returns>
		public virtual HarfDizisi ekle(TurkceHarf harf)
		{
			if (boy == dizi.Length)
				kapasiteAyarla(3);
			dizi[boy++] = harf;
			return this;
		}
		
		/// <summary> 
        /// girilen pozisyona herf ekler, bu noktadan sonraki harfler otelenir.
		/// "armut" icin (2, a) "aramut" uretir.
		/// </summary>
		/// <param name="index">eklenecek pozisyon
		/// </param>
		/// <param name="harf">eklenecek harf.
		/// </param>
		/// <throws>  ArrayIndexOutOfBoundsException </throws>
		public virtual void  ekle(int index, TurkceHarf harf)
		{
			if (index < 0 || index > boy)
				throw new System.IndexOutOfRangeException("index degeri:" + index + " fakat harf dizi boyu:" + boy);
			
			if (boy == dizi.Length)
				kapasiteAyarla();
			
			for (int i = boy - 1; i >= index; i--)
				dizi[i + 1] = dizi[i];
			dizi[index] = harf;
			boy++;
		}
		
		/// <summary> 
		/// Diziye baska bir harf dizisinin icerigini ular.
		/// </summary>
		/// <param name="hdizi">ulanacak harf dizisi.
		/// </param>
		/// <returns> this.
		/// </returns>
		public virtual HarfDizisi ekle(HarfDizisi hdizi)
		{
			int hboy = hdizi.length();
			if (boy + hboy > dizi.Length)
				kapasiteAyarla(hboy);
			
			Array.Copy(hdizi.dizi, 0, dizi, boy, hboy);
			boy += hdizi.length();
			return this;
		}
		
		/// <summary> 
        /// Diziye baska bir harf dizisinin icerigini index ile belirtilen harften itibaren ekler.
		/// "armut" icin (2, hede) "arhedemut" uretir.
		/// </summary>
		/// <param name="index">eklencek pozisyon
		/// </param>
		/// <param name="hdizi">eklenecek harf dizisi
		/// </param>
		/// <returns> this.
		/// </returns>
		/// <throws>  ArrayIndexOutOfBoundsException </throws>
		public virtual HarfDizisi ekle(int index, HarfDizisi hdizi)
		{
			if (index < 0 || index > boy)
				throw new System.IndexOutOfRangeException("indeks degeri:" + index + " fakat harf dizi boyu:" + boy);
			
			//dizi kapasitesini ayarla
			int hboy = hdizi.length();
			if (boy + hboy > dizi.Length)
				kapasiteAyarla(hboy);
			
			//sondan baslayarak this.dizinin index'ten sonraki kismini dizinin sonuna tasi
			for (int i = hboy + boy - 1; i >= hboy; i--)
				dizi[i] = dizi[i - hboy];
			
			//gelen diziyi kopyala ve boyutu degistir.
			Array.Copy(hdizi.dizi, 0, dizi, index, hboy);
			boy += hdizi.length();
			return this;
		}
		
		
		/// <summary> 
        /// verilen pozisyondaki harfi dondurur. icerigi "kedi" olan HarfDizisi icin harf(1) e dondurur.
		/// </summary>
		/// <param name="i">istenilen pozisyondaki harf.
		/// </param>
		/// <returns> girilen pozisyondaki harf, yoksa TANIMSIZ_HARF
		/// </returns>
		public TurkceHarf harf(int i)
		{
			if (i < 0)
				return Alfabe.TANIMSIZ_HARF;
			if (i < boy)
				return dizi[i];
			return Alfabe.TANIMSIZ_HARF;
		}
		
		/// <summary> 
        /// ilk sesliyi dondurur. eger sesli yoksa TANIMSIZ_HARF doner. aramaya belirtilen indeksten baslar.
		/// </summary>
		/// <param name="basla">baslangic indeksi.
		/// </param>
		/// <returns> varsa ilk sesli, yoksa TANIMSIZ_HARF
		/// </returns>
		public virtual TurkceHarf ilkSesli(int basla)
		{
			for (int i = basla; i < boy; i++)
			{
				if (dizi[i].sesliMi())
					return dizi[i];
			}
			return Alfabe.TANIMSIZ_HARF;
		}
		
		/// <summary> Tam esitlik kiyaslamasi. kiyaslama nesne tipi, ardindan da TurkceHarf dizisi icindeki
		/// harflerin char iceriklerine gore yapilir.
		/// 
		/// </summary>
		/// <param name="o">kiyaslanacak nesne
		/// </param>
		/// <returns> true eger esitse.
		/// </returns>
		public  override bool Equals(System.Object o)
		{
			if (o == null)
				return false;
			if (this == o)
				return true;
			if (!(o is HarfDizisi))
				return false;
			
			HarfDizisi harfDizisi = (HarfDizisi) o;
			if (boy != harfDizisi.boy)
				return false;
			for (int i = 0; i < boy; i++)
			{
				if (this.dizi[i].charDeger() != harfDizisi.dizi[i].charDeger())
					return false;
			}
			return true;
		}
		
		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}
		
		/// <summary> ascii benzer harf toleransli esitlik kiyaslamasi.
		/// 
		/// </summary>
		/// <param name="harfDizisi">kiyaslanacak harfDizisi
		/// </param>
		/// <returns> true eger esitse.
		/// </returns>
		public bool asciiToleransliKiyasla(HarfDizisi harfDizisi)
		{
			if (harfDizisi == null)
				return false;
			if (this == harfDizisi)
				return true;
			if (boy != harfDizisi.boy)
				return false;
			for (int i = 0; i < boy; i++)
			{
				if (!dizi[i].asciiToleransliKiyasla(harfDizisi.dizi[i]))
					return false;
			}
			return true;
		}
		
		public bool asciiToleransliAradanKiyasla(int baslangic, HarfDizisi kelime)
		{
			if (kelime == null)
				return false;
			if (boy < baslangic + kelime.length())
				return false;
			for (int i = 0; i < kelime.length(); i++)
				if (!dizi[baslangic + i].asciiToleransliKiyasla(kelime.harf(i)))
					return false;
			return true;
		}
		
		public bool asciiToleransliBastanKiyasla(HarfDizisi giris)
		{
			if (giris == null)
				return false;
			if (giris.length() > this.boy)
				return false;
			for (int i = 0; i < giris.length(); i++)
				if (!dizi[i].asciiToleransliKiyasla(giris.harf(i)))
					return false;
			return true;
		}
		
		public bool aradanKiyasla(int baslangic, HarfDizisi kelime)
		{
			if (kelime == null)
				return false;
			if (boy < baslangic + kelime.length())
				return false;
			for (int i = 0; i < kelime.length(); i++)
				if (dizi[baslangic + i].charDeger() != kelime.harf(i).charDeger())
					return false;
			return true;
		}
		
		public bool bastanKiyasla(HarfDizisi giris)
		{
			if (giris == null)
				return false;
			if (giris.length() > this.boy)
				return false;
			for (int i = 0; i < giris.length(); i++)
				if (dizi[i].charDeger() != giris.harf(i).charDeger())
					return false;
			return true;
		}
		
		/// <summary> istenen noktadaki harfi giris parametresi olan TurkceHarf ile degistirir.
		/// 
		/// </summary>
		/// <param name="index">degistirilecek indeks.
		/// </param>
		/// <param name="harf">kullanilacak harf
		/// </param>
		/// <throws>  ArrayIndexOutOfBoundsException </throws>
		public void  harfDegistir(int index, TurkceHarf harf)
		{
			if (index < 0 || index >= boy)
				throw new System.IndexOutOfRangeException("indeks degeri:" + index + " fakat harf dizi boyu:" + boy);
			dizi[index] = harf;
		}
		
		/// <summary> son harfi yumusatir. Eger harfin yumusamis formu yoksa harf degismez.</summary>
		public virtual void  sonHarfYumusat()
		{
			if (boy == 0)
				return ;
			TurkceHarf yum = dizi[boy - 1].yumusama();
			if (yum != null)
				dizi[boy - 1] = dizi[boy - 1].yumusama();
		}
		
		/// <summary> son harfi siler. eger harf yoksa hicbir etki yapmaz.</summary>
		public virtual void  sonHarfSil()
		{
			if (boy > 0)
				boy--;
		}
		
		/// <summary> verilen pozisyondaki harfi siler. kelimenin kalan kismi otelenir.
		/// eger verilen pozisyon yanlis ise  ArrayIndexOutOfBoundsException firlatir.
		/// 
		/// "kedi" icin (2) "kei" olusturur.
		/// 
		/// </summary>
		/// <param name="index">silinecek harf pozisyonu
		/// </param>
		/// <returns> dizinin kendisi.
		/// </returns>
		/// <throws>  ArrayIndexOutOfBoundsException </throws>
		public virtual HarfDizisi harfSil(int index)
		{
			if (index < 0 || index >= boy)
				throw new System.IndexOutOfRangeException("indeks degeri:" + index + " fakat harf dizi boyu:" + boy);
			if (index == boy - 1)
			{
				boy--;
			}
			else
			{
				Array.Copy(dizi, index + 1, dizi, index, boy - index - 1);
				boy--;
			}
			return this;
		}
		
		/// <summary> verilen pozisyondan belli miktar harfi siler.
		/// "kediler" icin (2,2) "keler" olusturur.
		/// </summary>
		/// <param name="index">silinmeye baslanacak pozisyon
		/// </param>
		/// <param name="harfSayisi">silinecek harf miktari
		/// </param>
		/// <returns> dizinin kendisi
		/// </returns>
		public virtual HarfDizisi harfSil(int index, int harfSayisi)
		{
			if (index < 0 || index >= boy)
				throw new System.IndexOutOfRangeException("indeks degeri:" + index + " fakat harf dizi boyu:" + boy);
			if (index + harfSayisi > boy)
				harfSayisi = boy - index;
			for (int i = index + harfSayisi; i < boy; i++)
				dizi[i - harfSayisi] = dizi[i];
			boy -= harfSayisi;
			return this;
		}
		
		/// <summary> ilk harfi dondurur. eger harf yoksa TANIMSIZ_HARF doner.
		/// 
		/// </summary>
		/// <returns> ilk TurkceHarf.
		/// </returns>
		public TurkceHarf ilkHarf()
		{
			if (boy == 0)
				return Alfabe.TANIMSIZ_HARF;
			else
				return dizi[0];
		}
		
		/// <summary> "index" numarali harften itibaren siler.
		/// "kedi" icin (1) "k" olusturur.
		/// </summary>
		/// <param name="index">kirpilmaya baslanacak pozisyon
		/// </param>
		public void  kirp(int index)
		{
			if (index <= boy && index >= 0)
				boy = index;
		}
		
		/// <summary> sadece belirli bir bolumunu String'e donusturur.</summary>
		/// <param name="index">String'e donusum baslangic noktasi.
		/// </param>
		/// <returns> olusan String.
		/// </returns>
		public virtual System.String ToString(int index)
		{
			if (index < 0 || index >= boy)
				return "";
			StringBuilder s = new StringBuilder(boy - index);
			for (int i = index; i < boy; i++)
				s.Append(charAt(i));
			return s.ToString();
		}

        public override System.String ToString()
        {
            StringBuilder str = new StringBuilder();
            for(int i=0;i < this.boy;i++)
            {
                str.Append(this.charAt(i));
            }
            return str.ToString(); ;
        }
        //TODO: StringBuilder javada olan Charsequence alýyor icine ama biz charsequence'i kendimiz yazdik
        //public override System.String ToString()
        //{
        //    return new StringBuilder(this).ToString();
        //}
		
		/* ------------------------- ozel metodlar ------------------------------- */
		
		/// <summary> Genellikle kelimedeki hece sayisini bulmak icin kullanilir.
		/// 
		/// </summary>
		/// <returns> inte, sesli harf sayisi.
		/// </returns>
		public virtual int sesliSayisi()
		{
			int sonuc = 0;
			for (int i = 0; i < boy; i++)
			{
				if (dizi[i].sesliMi())
					sonuc++;
			}
			return sonuc;
		}
		
		/// <returns> hepsi buyuk harf ise true, boy=0 dahil.
		/// </returns>
		public virtual bool hepsiBuyukHarfmi()
		{
			for (int i = 0; i < boy; i++)
			{
				if (!dizi[i].buyukHarfMi())
					return false;
			}
			return true;
		}
		
		//--------- asagidaki metodlar CharSequence arayuzu icin hazirlandi. -----
		
		public int length()
		{
			return boy;
		}
		
		public char charAt(int index)
		{
			if (index < 0 || index >= boy)
				throw new System.ArgumentOutOfRangeException(System.String.Empty, (System.Object) index, System.String.Empty);
			return dizi[index].charDeger();
		}
		
		public virtual CharSequence subSequence(int start, int end)
		{
			if (end < start)
				return null;
			TurkceHarf[] yeniHarfler = new TurkceHarf[end - start];
		    Array.Copy(dizi, start, yeniHarfler, 0, end - start);
			return new HarfDizisi(yeniHarfler);
		}
	}
}