using System;

namespace net.zemberek.yapi
{
	public interface HeceBulucu
	{
		/// <summary> 
        /// Giren harf dizisinin sonunda mantikli olarak yer alan hecenin harf sayisini dondurur.
		/// </summary>
		/// <param name="dizi:">harf dizisi.
		/// </param>
		/// <returns> int, 1,2,3 ya da 4 donerse giris dizisinin dizinin sondan o
		/// kadarharfi heceyi temsil eder -1 donerse hecenin bulunamadigi
		/// anlamina gelir.
		/// </returns>
		int sonHeceHarfSayisi(HarfDizisi dizi);
	}
}