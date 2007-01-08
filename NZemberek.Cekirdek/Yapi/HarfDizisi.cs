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
using System.Text;
using System.Collections.Generic;
using NZemberek.Cekirdek.Kolleksiyonlar;

namespace NZemberek.Cekirdek.Yapi
{
	/// <summary>
	/// Bu sinif Dil islemleri sirasinda Turkceye ozel islemler gerektiginden String-StringBuffer yerine kullanilir.
	/// String gibi genel bir tasiyici degil ara islem nesnesi olarak kullanilmasi onerilir.
	/// String'den farkli olarak "degistirilebilir" bir yapidadir ve Thread-safe degildir.
	/// </summary>
	public class HarfDizisi
	{
		private TurkceHarf[] dizi;
        //TODO : Bu Boy deðerini Harf arrayinin Boyut deðeri üzerinden saðlamak daha akýllýca.
        //Tamamlanýnca deneyip Javacýlara da önermeliyiz.
        private int boy = 0;
		
		/// <summary> default constructor. 7 boyutlu bir TurkceHarf referans dizisi olusturur.</summary>
		public HarfDizisi()
		{
			dizi = new TurkceHarf[7];
		}
		
		/// <summary> 
		/// 'kapasite' boyutlu 'TurkceHarf' dizisine sahip nesne olusturur.
		/// </summary>
		/// <param name="kapasite">baslangic olusan TurkceHarf[] boyu
		/// </param>
		public HarfDizisi(int kapasite)
		{
			dizi = new TurkceHarf[kapasite];
		}
		
		/// <summary> 'kapasite' boyutlu 'TurkceHarf' dizisine sahip nesne olusturur. daha sonra
		/// girisi String'i icindeki karakterleri TurkceHarf seklinde TurkceHarf dizisine aktarir.
		/// Eger String boyu kapasiteden buyukse kapasite'yi Boy'a esitler.
		/// Eger String icindeki karakter Alfabe'de yar almiyorsa "TANIMSIZ_HARF" harfi olarak eklenir.
		/// </summary>
		/// <param name="str">ornek alincak String </param>
		/// <param name="kapasite">baslangic olusan TurkceHarf[] boyu </param>
		/// <param name="Alfabe">ilgili Alfabe </param>
		public HarfDizisi(System.String str, Alfabe alfabe, int kapasite)
		{
			if (kapasite < str.Length)
				kapasite = str.Length;
			dizi = new TurkceHarf[kapasite];
			boy = str.Length;
			for (int i = 0; i < boy; i++)
				dizi[i] = alfabe.Harf(str[i]);
		}
		
		/// <summary> 
        /// Belirlenen Alfabe ile String icerigini Harflere donusturur.
		/// </summary>
		/// <param name="str">ornek alincak String </param>
		/// <param name="Alfabe">ilgili Alfabe </param>
		public HarfDizisi(System.String str, Alfabe alfabe)
		{
			boy = str.Length;
			dizi = new TurkceHarf[boy];
			for (int i = 0; i < boy; i++)
				dizi[i] = alfabe.Harf(str[i]);
		}
		
		/// <summary> 
        /// Copy-Constructor. gelen Harf dizisi ile ayni icerige sahip olacak sekilde TurkceHarf dizisi olusturur.
		/// </summary>
		/// <param name="hdizi">ornek alinacak HarfDizisi </param>
		public HarfDizisi(HarfDizisi hdizi)
		{
			boy = hdizi.Boy;
			dizi = new TurkceHarf[boy];
			Array.Copy(hdizi.dizi, 0, dizi, 0, boy);
		}
		
		/// <summary> 
		/// gelen TurkceHarf dizisini icerige kopyalar.
		/// </summary>
        /// <param name="aDizi">kopyalancak TurkceHarf dizisi. </param>
        private HarfDizisi(TurkceHarf[] aDizi)
		{
            boy = aDizi.Length;
			dizi = new TurkceHarf[boy];
            Array.Copy(aDizi, 0, dizi, 0, boy);
		}
		
		/// <summary> 
        /// bu metod Harf referansi dizisini serbest birakmaz, sadece boyu sifira indirir.
		/// </summary>
		public virtual void  Sil()
		{
			boy = 0;
		}
		
		/// <summary> 
		/// Dizinin son harfini dondurur.
		/// </summary>
		/// <returns> varsa son Harf, Yoksa TANIMSIZ_HARF.
		/// </returns>
		public TurkceHarf SonHarf()
		{
			if (boy > 0)
				return dizi[boy - 1];
			else
				return Alfabe.TANIMSIZ_HARF;
		}
		
		/// <summary> 
		/// dizideki son sesliyi dondurur. eger dizi boyu 0 ise ya da sesli Harf yoksa TANIMSIZ_HARF doner.
		/// </summary>
		/// <returns> varsa son sesli yoksa TANIMSIZ_HARF
		/// </returns>
		public TurkceHarf SonSesli()
		{
			for (int i = boy - 1; i >= 0; i--)
			{
				if (dizi[i].Sesli)
					return dizi[i];
			}
			return Alfabe.TANIMSIZ_HARF;
		}
		
		/// <summary> 
        /// ic metod. Harf dizisinin boyutu yetersiz geldiginde "ek" miktarinda daha
		/// fazla yere sahip yeni dizi olusturulup icerik yeni diziye kopyalanir.
		/// </summary>
		/// <param name="ek">eklenecek HarfDizisi miktari.
		/// </param>
		private void  KapasiteAyarla(int ek)
		{
			TurkceHarf[] yeniDizi = new TurkceHarf[dizi.Length + ek];
			Array.Copy(dizi, 0, yeniDizi, 0, dizi.Length);
			dizi = yeniDizi;
		}
		
		/// <summary> otomatik kapasite ayarlama. dizi boyu iki katina cikarilir.</summary>
		private void  KapasiteAyarla()
		{
			TurkceHarf[] yeniDizi = new TurkceHarf[dizi.Length * 2];
			Array.Copy(dizi, 0, yeniDizi, 0, dizi.Length);
			dizi = yeniDizi;
		}
		
		/// <summary> 
        /// kelimenin sonuna Harf ekler.
		/// </summary>
		/// <param name="Harf">eklenecek Harf </param>
		/// <returns> this </returns>
		public virtual HarfDizisi Ekle(TurkceHarf harf)
		{
			if (boy == dizi.Length)
				KapasiteAyarla(3);
			dizi[boy++] = harf;
			return this;
		}
		
		/// <summary> 
        /// girilen pozisyona herf ekler, bu noktadan sonraki harfler otelenir.
		/// "armut" icin (2, a) "aramut" uretir.
		/// </summary>
		/// <param name="index">eklenecek pozisyon </param>
		/// <param name="Harf">eklenecek Harf. </param>
		/// <throws>  ArrayIndexOutOfBoundsException </throws>
		public virtual void  Ekle(int index, TurkceHarf harf)
		{
			if (index < 0 || index > boy)
				throw new System.IndexOutOfRangeException("index degeri:" + index + " fakat Harf dizi boyu:" + boy);
			
			if (boy == dizi.Length)
				KapasiteAyarla();
			
			for (int i = boy - 1; i >= index; i--)
				dizi[i + 1] = dizi[i];
			dizi[index] = harf;
			boy++;
		}
		
		/// <summary> 
		/// Diziye baska bir Harf dizisinin icerigini ular.
		/// </summary>
		/// <param name="hdizi">ulanacak Harf dizisi. </param>
		/// <returns> this.
		public virtual HarfDizisi Ekle(HarfDizisi hdizi)
		{
			int hboy = hdizi.Boy;
			if (boy + hboy > dizi.Length)
				KapasiteAyarla(hboy);
			
			Array.Copy(hdizi.dizi, 0, dizi, boy, hboy);
			boy += hdizi.Boy;
			return this;
		}
		
		/// <summary> 
        /// Diziye baska bir Harf dizisinin icerigini index ile belirtilen harften itibaren ekler.
		/// "armut" icin (2, hede) "arhedemut" uretir.
		/// </summary>
		/// <param name="index">eklencek pozisyon
		/// </param>
		/// <param name="hdizi">eklenecek Harf dizisi
		/// </param>
		/// <returns> this.
		/// </returns>
		/// <throws>  ArrayIndexOutOfBoundsException </throws>
		public virtual HarfDizisi Ekle(int index, HarfDizisi hdizi)
		{
			if (index < 0 || index > boy)
				throw new System.IndexOutOfRangeException("Indeks degeri:" + index + " fakat Harf dizi boyu:" + boy);
			
			//dizi kapasitesini ayarla
			int hboy = hdizi.Boy;
			if (boy + hboy > dizi.Length)
				KapasiteAyarla(hboy);
			
			//sondan baslayarak this.dizinin index'ten sonraki kismini dizinin sonuna tasi
			for (int i = hboy + boy - 1; i >= hboy; i--)
				dizi[i] = dizi[i - hboy];
			
			//gelen diziyi Kopyala ve boyutu degistir.
			Array.Copy(hdizi.dizi, 0, dizi, index, hboy);
			boy += hdizi.Boy;
			return this;
		}
		
		
		/// <summary> 
        /// verilen pozisyondaki harfi dondurur. icerigi "kedi" olan HarfDizisi icin Harf(1) e dondurur.
		/// </summary>
		/// <param name="i">istenilen pozisyondaki Harf.
		/// </param>
		/// <returns> girilen pozisyondaki Harf, yoksa TANIMSIZ_HARF
		/// </returns>
		public TurkceHarf Harf(int i)
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
		public virtual TurkceHarf IlkSesli(int basla)
		{
			for (int i = basla; i < boy; i++)
			{
				if (dizi[i].Sesli)
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
				if (this.dizi[i].CharDeger != harfDizisi.dizi[i].CharDeger)
					return false;
			}
			return true;
		}
		
		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}
		
		/// <summary> 
		/// ascii benzer Harf toleransli esitlik kiyaslamasi.
		/// </summary>
		/// <param name="harfDizisi">kiyaslanacak harfDizisi </param>
		/// <returns> true eger esitse. </returns>
		public bool AsciiToleransliKiyasla(HarfDizisi harfDizisi)
		{
			if (harfDizisi == null)
				return false;
			if (this == harfDizisi)
				return true;
			if (boy != harfDizisi.boy)
				return false;
			for (int i = 0; i < boy; i++)
			{
				if (!dizi[i].AsciiToleransliKiyasla(harfDizisi.dizi[i]))
					return false;
			}
			return true;
		}
		
		public bool AsciiToleransliAradanKiyasla(int baslangic, HarfDizisi kelime)
		{
			if (kelime == null)
				return false;
			if (boy < baslangic + kelime.Boy)
				return false;
			for (int i = 0; i < kelime.Boy; i++)
				if (!dizi[baslangic + i].AsciiToleransliKiyasla(kelime.Harf(i)))
					return false;
			return true;
		}
		
		public bool AsciiToleransliBastanKiyasla(HarfDizisi giris)
		{
			if (giris == null)
				return false;
			if (giris.Boy > this.boy)
				return false;
			for (int i = 0; i < giris.Boy; i++)
				if (!dizi[i].AsciiToleransliKiyasla(giris.Harf(i)))
					return false;
			return true;
		}
		
		public bool AradanKiyasla(int baslangic, HarfDizisi kelime)
		{
			if (kelime == null)
				return false;
			if (boy < baslangic + kelime.Boy)
				return false;
			for (int i = 0; i < kelime.Boy; i++)
				if (dizi[baslangic + i].CharDeger != kelime.Harf(i).CharDeger)
					return false;
			return true;
		}
		
		public bool BastanKiyasla(HarfDizisi giris)
		{
			if (giris == null)
				return false;
			if (giris.Boy > this.boy)
				return false;
			for (int i = 0; i < giris.Boy; i++)
				if (dizi[i].CharDeger != giris.Harf(i).CharDeger)
					return false;
			return true;
		}
		
		/// <summary> 
		/// istenen noktadaki harfi giris parametresi olan TurkceHarf ile degistirir.
		/// </summary>
		/// <param name="index">degistirilecek Indeks. </param>
		/// <param name="Harf">kullanilacak Harf </param>
		/// <throws>  ArrayIndexOutOfBoundsException </throws>
		public void  HarfDegistir(int index, TurkceHarf harf)
		{
			if (index < 0 || index >= boy)
				throw new System.IndexOutOfRangeException("Indeks degeri:" + index + " fakat Harf dizi boyu:" + boy);
			dizi[index] = harf;
		}
		
		/// <summary> son harfi yumusatir. Eger harfin yumusamis formu yoksa Harf degismez.</summary>
		public virtual void SonHarfYumusat()
		{
			if (boy == 0)
				return ;
			TurkceHarf yum = dizi[boy - 1].Yumusama;
			if (yum != null)
				dizi[boy - 1] = dizi[boy - 1].Yumusama;
		}
		
		/// <summary> son harfi siler. eger Harf yoksa hicbir etki yapmaz.</summary>
		public virtual void  SonHarfSil()
		{
			if (boy > 0)
				boy--;
		}
		
		/// <summary> verilen pozisyondaki harfi siler. kelimenin kalan kismi otelenir.
		/// eger verilen pozisyon yanlis ise  ArrayIndexOutOfBoundsException firlatir.
		/// "kedi" icin (2) "kei" olusturur.
		/// </summary>
		/// <param name="index">silinecek Harf pozisyonu </param>
		/// <returns> dizinin kendisi. </returns>
		/// <throws>  ArrayIndexOutOfBoundsException </throws>
		public virtual HarfDizisi HarfSil(int index)
		{
			if (index < 0 || index >= boy)
				throw new System.IndexOutOfRangeException("Indeks degeri:" + index + " fakat Harf dizi boyu:" + boy);
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
		/// <param name="index">silinmeye baslanacak pozisyon</param>
		/// <param name="harfSayisi">silinecek Harf miktari</param>
		/// <returns> dizinin kendisi</returns>
		public virtual HarfDizisi HarfSil(int index, int harfSayisi)
		{
			if (index < 0 || index >= boy)
				throw new System.IndexOutOfRangeException("Indeks degeri:" + index + " fakat Harf dizi boyu:" + boy);
			if (index + harfSayisi > boy)
				harfSayisi = boy - index;
			for (int i = index + harfSayisi; i < boy; i++)
				dizi[i - harfSayisi] = dizi[i];
			boy -= harfSayisi;
			return this;
		}

        public HarfDizisi SubSequence(int start, int end)
        {
            if (end < start)
                return null;
            TurkceHarf[] yeniHarfler = new TurkceHarf[end - start];
            Array.Copy(dizi, start, yeniHarfler, 0, end - start);
            return new HarfDizisi(yeniHarfler);
        }

		/// <summary> 
		/// ilk harfi dondurur. eger Harf yoksa TANIMSIZ_HARF doner.
		/// </summary>
		/// <returns> ilk TurkceHarf</returns>
		public TurkceHarf IlkHarf()
		{
			if (boy == 0)
				return Alfabe.TANIMSIZ_HARF;
			else
				return dizi[0];
		}
		
		/// <summary> 
        /// "index" numarali harften itibaren siler.
		/// "kedi" icin (1) "k" olusturur.
		/// </summary>
		/// <param name="index">kirpilmaya baslanacak pozisyon</param>
		public void  Kirp(int index)
		{
			if (index <= boy && index >= 0)
				boy = index;
		}
		
		/// <summary> sadece belirli bir bolumunu String'e donusturur.</summary>
		/// <param name="index">String'e donusum baslangic noktasi.
		/// </param>
		/// <returns> olusan String.
		/// </returns>
		public virtual String ToString(int index)
		{
			if (index < 0 || index >= boy)
				return "";
			StringBuilder s = new StringBuilder(boy - index);
			for (int i = index; i < boy; i++)
				s.Append(dizi[i].CharDeger);
			return s.ToString();
		}

        public override String ToString()
        {
            StringBuilder str = new StringBuilder();
            for(int i=0;i < this.boy;i++)
            {
                str.Append(dizi[i].CharDeger);
            }
            return str.ToString(); ;
        }
		
		/* ------------------------- ozel metodlar ------------------------------- */
		
		/// <summary> Genellikle kelimedeki hece sayisini bulmak icin kullanilir.
		/// 
		/// </summary>
		/// <returns> inte, sesli Harf sayisi.
		/// </returns>
		public virtual int SesliSayisi()
		{
			int sonuc = 0;
			for (int i = 0; i < boy; i++)
			{
				if (dizi[i].Sesli)
					sonuc++;
			}
			return sonuc;
		}
		
        public int Boy
        {
            get { return boy; }
        }
    }
}