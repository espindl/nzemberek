using System;

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
}