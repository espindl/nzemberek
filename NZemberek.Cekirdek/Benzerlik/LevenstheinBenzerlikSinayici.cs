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
    /*
     * Bu gerceklemedeki kodlar yazilirken SimMetrics projesinin .Net gerçeklemesinden
     * faydalanildi.
     * 
     * SimMetrics - SimMetrics is a java library of Similarity or Distance
     * Metrics, e.g. Levenshtein Distance, that provide float based similarity
     * measures between string Data. All metrics return consistant measures
     * rather than unbounded similarity scores.
     * Copyright (C) 2005 Sam Chapman - Open Source Release v1.1
     * www: http://www.dcs.shef.ac.uk/~sam/stringmetrics.html
     */
    public class LevenstheinBenzerlikSinayici : IBenzerlikSinayici
    {
        const double varsayilanTamBenzerlik = 1.0;
        const double varsayilanBenzemezlikSkoru = 0.0;

        private static double DuzeltmeMesafesi(string s, string t)
        {
            int n = s.Length; //length of s
            int m = t.Length; //length of t
            double[,] d = new double[n + 1, m + 1]; // matrix
            double cost; // cost
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
            return d[n, m];
        }

        private static double TranspozisyonlariKontrolEt(string s, string t, int i, int j, double[,] d)
        {
            if (i > 1 && j > 1)
            {
                double trans = d[i - 2, j - 2] + 1;
                if (s[i - 2] != t[j - 1])
                    trans++;
                if (s[i - 1] != t[j - 2])
                    trans++;
                if (d[i, j] > trans) d[i, j] = trans;
            }
            return d[i, j];
        }

        #region IBenzerlikSinayici Members

        public double SozcuklerNeKadarBenzer(string sozcuk1, string sozcuk2)
        {
            if ((sozcuk1 != null) && (sozcuk2 != null))
            {
                double duzeltmeMesafesi = DuzeltmeMesafesi(sozcuk1, sozcuk2);
                double enuzun = sozcuk1.Length;
                if (enuzun < sozcuk2.Length)
                {
                    enuzun = sozcuk2.Length;
                }
                if (enuzun == varsayilanBenzemezlikSkoru)
                {
                    return varsayilanTamBenzerlik;
                }
                else
                {
                    return varsayilanTamBenzerlik - duzeltmeMesafesi / enuzun;
                }
            }
            return varsayilanBenzemezlikSkoru;
        }

        public bool SocuklerBenzer(string sozcuk1, string sozcuk2, double benzerlikLimiti)
        {
            return this.SozcuklerNeKadarBenzer(sozcuk1, sozcuk2) >= benzerlikLimiti;
        }

        #endregion
    }
}
