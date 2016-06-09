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
 * The Original Code is NZemberek Doğal Dil İşleme Kütüphanesi.
 *
 * The Initial Developer of the Original Code is
 * Mert Derman, Tankut Tekeli.
 * Portions created by the Initial Developer are Copyright (C) 2006
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 * 
 * ***** END LICENSE BLOCK ***** */

using System;
using System.Collections.Generic;
using System.Text;

namespace NZemberek.Cekirdek.Benzerlik
{
    public class BenzerlikAraci
    {
        private static IBenzerlikSinayici benzerlikSinayici = new LevenstheinBenzerlikSinayici();
        //private static IBenzerlikSinayici benzerlikSinayici = new JaroWinklerBenzerlikSinayici();

        private static double NormalizeBenzerlikSkoru(int enuzun, int benzerlik)
        {
            return 1.0 - (Convert.ToDouble(benzerlik) / Convert.ToDouble(enuzun));
        }

        /// <summary>
        /// Iki sozcugun en az verilen benzerlik skoru kadar benzer olup olmadigini denetler
        /// </summary>
        /// <param name="sozcuk1">benzerligi sinanacak sozcuklerden biri</param>
        /// <param name="sozcuk2">benzerligi sinanacak sozcuklerden digeri</param>
        /// <returns>eger istenilen mesafede ise true</returns>
        /// TODO : adı değişmeli ama sonra MERT
        public static bool DuzeltmeMesafesiIcinde(string sozcuk1, string sozcuk2, int enAzBenzerlik)
        {
            double normalizeBenzerlik = NormalizeBenzerlikSkoru(Math.Max(sozcuk1.Length, sozcuk2.Length), enAzBenzerlik);
            return benzerlikSinayici.SozcuklerNeKadarBenzer(sozcuk1, sozcuk2) >= normalizeBenzerlik;
        }

        /// <summary>
        /// sozccuk1'in, sozcuk2'nin parcalarindan birine enAzBenzerlik kadar benzeyip benzemedigini kontrol eder
        /// </summary>
        /// <param name="sozcuk1">sozcuk2 icinde enAzBenzerlik kadar bennzedigi parca varmi diye bakilan sozcuk</param>
        /// <param name="sozcuk2">icinde herhangi bir parcasi sozcuk1'e enAzBenzerlik kadar bener bir parca aranan sozcuk</param>
        /// <param name="distance">duzeltme mesafesi</param>
        /// <returns></returns>
        /// TODO : adı değişmeli ama sonra MERT
        public static bool ParcasiDuzeltmeMesafesiIcinde(String sozcuk1, String sozcuk2, int enAzBenzerlik)
        {
            if (sozcuk2.Length < (sozcuk1.Length - enAzBenzerlik))
                return false;

            if (sozcuk2.Length >= sozcuk1.Length)
            {
                String test = sozcuk2.Substring(0, sozcuk1.Length);
                if (DuzeltmeMesafesiIcinde(sozcuk1, test, enAzBenzerlik)) return true;
                test = sozcuk2.Substring(0, sozcuk1.Length - 1);
                if (DuzeltmeMesafesiIcinde(sozcuk1, test, enAzBenzerlik)) return true;
                if (sozcuk2.Length >= sozcuk1.Length + 1)
                {
                    test = sozcuk2.Substring(0, sozcuk1.Length + 1);
                    if (DuzeltmeMesafesiIcinde(sozcuk1, test, enAzBenzerlik)) return true;
                }
            }
            else
            {
                if (DuzeltmeMesafesiIcinde(sozcuk1, sozcuk2, enAzBenzerlik)) return true;
            }
            return false;
        }
    }
}
