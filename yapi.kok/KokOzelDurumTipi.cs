using System;
namespace net.zemberek.yapi.kok
{
	public interface KokOzelDurumTipi
	{
		
		/// <summary> ozel durumun tam adini temsil eder.</summary>
		/// <returns> ad
		/// </returns>
		System.String ad();
		
		/// <summary> ozel durumun duz yazi dosyasindaki ifade edilen halini temsil eder.</summary>
		/// <returns> kisaAd
		/// </returns>
		System.String kisaAd();
		
		/// <summary> ozel durum indeksi enum ordinal() bilgisi ile uretilir.
		/// (yani enum bilesninin sira indeksi)
		/// bu bilgi ikili kok olusumu sirasinda ozel durumu temsil etmek icin dosyaya yazilir.
		/// okuma sirasinda bu indeks ile ozel duruma erisilir. Eger bir sekilde ozel durumlarin
		/// enum iceirsindeki sirasi gelistirme asamasinda degistirilirse ikili kok dosyasinin
		/// tekrar uretilmesi gerekir..
		/// </summary>
		/// <returns> ozel durum indeksi.
		/// </returns>
		int indeks();
		
		/// <summary> bu ozel durumun olusamasina neden olacak eklerin adlari.</summary>
		/// <returns> 0 yada n uzunluklu dizi.
		/// </returns>
		System.String[] ekAdlari();
	}
}