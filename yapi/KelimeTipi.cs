using System;
using System.Collections.Generic;


namespace net.zemberek.yapi
{
    public enum KelimeTipi
	{
        YOK, //.Net'te enum Null olamadýðý için bu elemaný ekledim. (@tankut)
	    ISIM,
	    FIIL, 
	    SIFAT, 
	    SAYI, 
	    YANKI, 
	    ZAMIR, 
	    SORU, 
	    IMEK, 
	    ZAMAN, 
	    EDAT, 
	    BAGLAC, 
	    OZEL, 
	    UNLEM, 
	    KISALTMA, 
	    HATALI,
    }

    public class KelimeTipleriUtil
    {
        public static readonly IDictionary<String, KelimeTipi> KelimeTipleri = new Dictionary<String,KelimeTipi>();
        static KelimeTipleriUtil()
        {
            KelimeTipleri = new Dictionary<String,KelimeTipi>();
            KelimeTipleri.Add("IS"    ,KelimeTipi.ISIM);
	        KelimeTipleri.Add("FI"    ,KelimeTipi.FIIL); 
	        KelimeTipleri.Add("SI"   ,KelimeTipi.SIFAT);  
	        KelimeTipleri.Add("SA"    ,KelimeTipi.SAYI);  
	        KelimeTipleri.Add("YA"   ,KelimeTipi.YANKI);  
	        KelimeTipleri.Add("ZA"   ,KelimeTipi.ZAMIR);  
	        KelimeTipleri.Add("SO"    ,KelimeTipi.SORU);  
	        KelimeTipleri.Add("IM"    ,KelimeTipi.IMEK);  
	        KelimeTipleri.Add("ZAMAN"   ,KelimeTipi.ZAMAN);  
	        KelimeTipleri.Add("EDAT"    ,KelimeTipi.EDAT);  
	        KelimeTipleri.Add("BAGLAC"  ,KelimeTipi.BAGLAC);  
	        KelimeTipleri.Add("OZ"    ,KelimeTipi.OZEL);  
	        KelimeTipleri.Add("UN"   ,KelimeTipi.UNLEM);  
	        KelimeTipleri.Add("KI",KelimeTipi.KISALTMA);
            KelimeTipleri.Add("HA"  ,KelimeTipi.HATALI);
        }  
    }     
}