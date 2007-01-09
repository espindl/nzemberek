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
 * 
 ***** END LICENSE BLOCK ***** */
using System;
using System.Collections.Generic;
using System.Text;

namespace NZemberek.Cekirdek.Araclar
{
    public class MetinAraclari
    {
        private static JaroWinkler jaroWinkler = new JaroWinkler();

        /// <summary>
        /// Degistirilmis Levenshtein Edit Dist. algoritması. transpozisyonları da 1 düzeltme mesafesi olarak hesaplar.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns>iki kelime arasindaki mesafe, tamsayi cinsinden. kucuk rakamlar daha buyuk benzerligi gosterir</returns>
        public static int DuzeltmeMesafesi(String source, String target)
        {
            int maxDif = Math.Max(source.Length, target.Length);
            return DuzeltmeMesafesi(source, target, maxDif);
        }

        /// <summary>
        /// Degistirilmis Levenshtein Edit Dist. algoritması. transpozisyonları da 1 düzeltme mesafesi olarak hesaplar.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns>eger istenilen mesafede ise true</returns>
        public static bool DuzeltmeMesafesiIcinde(string source, string target, int dist)
        {
            return (DuzeltmeMesafesi(source, target, dist) <= dist);
        }

        /// <summary>
        /// Verilen s1 stringinin verilen distance duzeltme mesafesi cercevesinde 
        /// s2 stringinin alt stringi olup olmadigini dondurur. Ornegin:
        ///   isInSubStringLevenshteinDistance("elma","ekmalar",1) -> true
        ///   isInSubStringLevenshteinDistance("elma","emalar",1) -> true
        ///   isInSubStringLevenshteinDistance("elma","eksalar",1) -> false (substring min dist=2)
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2">s1'i distance duzeltme mesafesi icinde kapsayıp kapsamadigi arastirilan String</param>
        /// <param name="distance">duzeltme mesafesi</param>
        /// <returns></returns>
        public static bool ParcasiDuzeltmeMesafesiIcinde(String s1, String s2, int distance)
        {
            if (s2.Length < (s1.Length - distance))
                return false;

            if (s2.Length >= s1.Length)
            {
                String test = s2.Substring(0, s1.Length);
                if (DuzeltmeMesafesiIcinde(s1, test, distance)) return true;
                test = s2.Substring(0, s1.Length - 1);
                if (DuzeltmeMesafesiIcinde(s1, test, distance)) return true;
                if (s2.Length >= s1.Length + 1)
                {
                    test = s2.Substring(0, s1.Length + 1);
                    if (DuzeltmeMesafesiIcinde(s1, test, distance)) return true;
                }
            }
            else
            {
                if (DuzeltmeMesafesiIcinde(s1, s2, distance)) return true;
            }
            return false;
        }

        /// <summary>
        /// s1 ile s2'nin benzerlik oranini hesaplar.
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns>0-1.0 arasi bir deger. Buyuk rakamlar kelimelerin daha benzer oldugunu gosterir</returns>
        public static double SozcukBenzerlikOrani(String s1, String s2)
        {
            return jaroWinkler.BenzerlikOrani(s1, s2);
        }

        /// <summary>
        /// s1 ile s2'nin enazBenzerlik degeri kadar ya da daha benzer olup olmadigini test eder.
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns>eger benzerlik orani enazBenzerlik'na es ya da buyukse true</returns>
        public static bool SozcuklerBenzer(String s1, String s2, double enazBenzerlik)
        {
            return (jaroWinkler.BenzerlikOrani(s1, s2) >= enazBenzerlik);
        }

        /// <summary>
        /// Degistirilmis Levenshtein Edit Distance Algoritmasi. 
        /// Transpozisyonları da 1 düzeltme mesafesi olarak hesapliyor.
        /// </summary>
        /// <param name="s">kaynak</param>
        /// <param name="t">hedef</param>
        /// <param name="limit">uzaklik</param>
        /// <returns></returns>
        private static int DuzeltmeMesafesi(string s, string t, int limit)
        {
            int n = s.Length; //length of s
            int m = t.Length; //length of t
            int[,] d = new int[n + 1, m + 1]; // matrix
            int cost; // cost
            // Step 1
            if (n == 0) return m;
            if (m == 0) return n;
            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 0; j <= m; d[0, j] = j++) ;
            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    cost = (t.Substring(j - 1, 1) == s.Substring(i - 1, 1) ? 0 : 1);
                    // Step 6
                    d[i, j] = System.Math.Min(System.Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                    // Step 6A
                    d[i, j] = TranspozisyonlariKontrolEt(s, t, i, j, d);
                }
            }
            // Step 7
            return d[n, m] > limit ? limit + 1 : d[n, m];
        }

        private static int TranspozisyonlariKontrolEt(string s, string t, int i, int j, int[,] d)
        {
            if (i > 1 && j > 1)
            {
                int trans = d[i - 2, j - 2] + 1;
                if (s[i - 2] != t[j - 1])
                    trans++;
                if (s[i - 1] != t[j - 2])
                    trans++;
                if (d[i, j] > trans) d[i, j] = trans;
            }
            return d[i, j];
        }
    }
}