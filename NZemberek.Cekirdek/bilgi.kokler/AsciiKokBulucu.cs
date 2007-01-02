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
 * The Original Code is Zemberek Doğal Dil İşleme Kütüphanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Akın, Mehmet D. Akın.
 * Portions created by the Initial Developer are Copyright (C) 2006
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Mert Derman
 *   Tankut Tekeli
 * ***** END LICENSE BLOCK ***** */

using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;
using log4net;

    
namespace net.zemberek.bilgi.kokler
{
    public class AsciiKokBulucu : KokBulucu
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);		

        KokAgaci agac = null;
        private int walkCount = 0;
        private String giris;
        private String asciiGiris;
        private List<Kok> adaylar = null;

        public AsciiKokBulucu(KokAgaci agac)
        {
            this.agac = agac;
        }

        public int getYurumeSayisi() 
        {
            return walkCount;
        }

        public List<Kok> getAdayKokler(String giris)
        {
            this.giris = giris;
            asciiGiris = agac.getAlfabe().asciifyString(giris);
            adaylar = new List<Kok>(4);
            yuru(agac.getKokDugumu(), "");
            return adaylar;;
        }

        /**
         * Verilen iki string'in asciified versiyonlarını karşılaştırır.
         *
         * @param aday
         * @param giris
         * @return aday ve giris degerlerinin ascii karsiliklari aynıysa true, 
         * 	       değilse false. Ã–rneğin:
         * <pre>
         * asciiTolaransliKarsilastir("siraci", "şıracı") --> true 
         * </pre>
         */
        public bool asciiTolaransliKarsilastir(String aday, String giris)
        {
            if (aday.Length > giris.Length) return false;
            String clean = agac.getAlfabe().asciifyString(aday);
            return asciiGiris.StartsWith(clean);
        }

        /**
         * Ağaç üzerinde  yürüyerek ASCII toleranslı karşılaştırma ile
         * kök adaylarını bulur. Rekürsiftir.
         *
         * @param dugum  : başlangıç düğümü
         * @param olusan : Yürüme sırasında oluşan kelime (düğümlerin karakter değerlerinden)
         */
        public void yuru(KokDugumu dugum, String olusan) 
        {
            String tester = (olusan + dugum.Harf).Trim();
            walkCount++;
            if (dugum.getKok() != null) 
            {
                if (logger.IsInfoEnabled) logger.Info("Kok : " + dugum.Kelime);
                if (asciiTolaransliKarsilastir((String) dugum.Kelime, giris)) 
                {
                    // Aday kok bulundu.
                    dugum.tumKokleriEkle(adaylar);
                }
                else 
                {
                    return;
                }
            }
            else 
            {
                if (asciiTolaransliKarsilastir(tester, giris)) 
                {//TODO böölemi ! idi
                    return;
                }
            }

            int seviye = tester.Length - 1; //TODO böölemi -1 yoktu
            if(seviye == giris.Length) return;
            // Uygun tüm alt dallarda yürü
            foreach (KokDugumu altDugum in dugum.altDugumDizisiGetir()) 
            {
               if (altDugum != null) 
               {
                   if (agac.getAlfabe().asciiToleransliKiyasla(altDugum.Harf, giris[seviye]))
                       this.yuru(altDugum, tester);
               }
            }
        }
    }
}
