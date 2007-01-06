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
using System.Collections.Generic;


namespace NZemberek.Cekirdek.Yapi
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