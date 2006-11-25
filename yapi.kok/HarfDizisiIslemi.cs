using System;
using net.zemberek.yapi;

namespace net.zemberek.yapi.kok
{
	/// <summary> Bir harf dizisi uzerinde yapilabilecek islemi ifade eder. Bu arayuz genellikle
	/// kok yapisi uzerinde degisiklige nedenn olan ozel durumlarin tanimlanmasinda kullanilir.
	/// </summary>
	public interface HarfDizisiIslemi
	{
		
		/// <summary> dizi uzerinde degisiklik yapacak metod.</summary>
		/// <param name="dizi">
		/// </param>
		void  uygula(HarfDizisi dizi);
	}
}