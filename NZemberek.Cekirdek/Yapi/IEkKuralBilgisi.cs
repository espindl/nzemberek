using System;
using System.Collections.Generic;
using System.Text;

namespace NZemberek.Cekirdek.Yapi
{
    public interface IEkKuralBilgisi
    {
        List<char> sesliKuralKarakterleri();

        List<char> harfKuralKarakterleri();

        Dictionary<char, IEkUretimKurali> karakterKuralTablosu();

        IEkUretimKurali harfEklemeKurali();
    }


    public class EkKonfigurasyonHatasi : ApplicationException {
	
	    public EkKonfigurasyonHatasi(String message): base(message)
        {
	       
	    }
	}
}
