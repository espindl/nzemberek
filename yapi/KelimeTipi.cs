using System;
using System.Collections.Generic;


namespace net.zemberek.yapi
{
    public enum KelimeTipi
	{
        //.Net'te enum Null olamadýðý için bu elemaný ekledim. (@tankut)
        ISIM = 0,
        FIIL = 1,
        SIFAT = 2,
        SAYI = 3,
        YANKI = 4,
        ZAMIR = 5,
        SORU = 6,
        IMEK = 7,
        ZAMAN = 8,
        EDAT = 9,
        BAGLAC = 10,
        OZEL = 11,
        UNLEM = 12,
        KISALTMA = 13,
        HATALI = 14,
        YOK = 15,
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