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
using System.Text;


namespace NZemberek.Cekirdek.Araclar
{
    /*
    Copied from Zemberek2
    which has formerly copied and modified from SecondString project.
    Copyright (c) 2003 Carnegie Mellon University
    All rights reserved.
    Developed by: 		Center for Automated Learning and Discovery
                      	    Carnegie Mellon University
                            http://www.cald.cs.cmu.edu

         The design and implementation of this software was supported in
         part by National Science Foundation Grant No. EIA-0131884 to the
         National Institute of Statistical Sciences, and by a contract
         from the Army Research Office to the Center for Computer and
         Communications Security with Carnegie Mellon University.


    Permission is hereby granted, free of charge, to any person obtaining
    a copy of this software and associated documentation files (the
    "Software"), to deal with the Software without restriction, including
    without limitation the rights to use, copy, modify, merge, publish,
    distribute, sublicense, and/or sell copies of the Software, and to
    permit persons to whom the Software is furnished to do so, subject to
    the following conditions:

    Redistributions of source code must retain the above copyright notice,
    this list of conditions and the following disclaimers.
    Redistributions in binary form must reproduce the above copyright
    notice, this list of conditions and the following disclaimers in the
    documentation and/or other materials provided with the distribution.
    Neither the names of the Center for Automated Learning and Discovery,
    or Carnegie Mellon University, nor the names of its contributors may
    be used to endorse or promote products derived from this Software
    without specific prior written permission.  THE SOFTWARE IS PROVIDED
    "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
    BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
    PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    CONTRIBUTORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
    OTHER DEALINGS WITH THE SOFTWARE.

    [This is an instance of the University of Illinois/NCSA Open Source
    agreement, obtained from http://www.opensource.org/licenses/UoI-NCSA.php]*/


    /**
     * 
     * Winkler's reweighting scheme for distance metrics.  In the
     * literature, this was applied to the Jaro metric ('An Application of
     * the Fellegi-Sunter Model of Record Linkage to the 1990
     * U.S. Decennial Census' by William E. Winkler and Yves Thibaudeau.)
     */
    public class JaroWinkler
    {
        /**
         * Verilen iki String'in benzerlik oranýný Jaro-Winkler algortimasý kullanarak hesaplar.
         * Oluþan sonuç 0-1 arasýndadýr, 1'e yaklaþtýkça benzerlik artar, ayný stringler için "1" dir
         * @param source
         * @param target
         * @return Stringlerin benzerlik oraný. stringler ayný ise 1
         */
        public double BenzerlikOrani(String source, String target)
        {
            double dist = JaroBenzerlikOrani(source, target);
            if (dist < 0 || dist > 1)
                throw new ArgumentOutOfRangeException("innerDistance should produce scores between 0 and 1");
            int prefLength = OrtakBaslangicUzunlugu(4, source, target);
            dist = dist + prefLength * 0.1 * (1 - dist);
            return dist;
        }

        private double JaroBenzerlikOrani(String source, String target)
        {
            int halflen = KisaOlaninYariUzunlugu(source, target);
            String common1 = OrtakKarakterler(source, target, halflen);
            String common2 = OrtakKarakterler(target, source, halflen);
            if (common1.Length != common2.Length) return 0;
            if (common1.Length == 0 || common2.Length == 0) return 0;

            int transpositions = GetTranspositions(common1, common2);
            double dist =
                    (common1.Length / ((double)source.Length) +
                    common2.Length / ((double)target.Length) +
                    (common1.Length - transpositions) / ((double)common1.Length)) / 3.0;
            return dist;
        }

        private int KisaOlaninYariUzunlugu(String source, String target)
        {
            return (source.Length > target.Length) ? target.Length / 2 + 1 : source.Length / 2 + 1;
        }

        private String OrtakKarakterler(String s, String t, int halflen)
        {
            StringBuilder common = new StringBuilder();
            StringBuilder copy = new StringBuilder(t);
            for (int i = 0; i < s.Length; i++)
            {
                char ch = s[i];
                bool foundIt = false;
                for (int j = Math.Max(0, i - halflen); !foundIt && j < Math.Min(i + halflen, t.Length); j++)
                {
                    if (copy[j] == ch)
                    {
                        foundIt = true;
                        common.Append(ch);
                        copy[j] = '*';
                    }
                }
            }
            return common.ToString();
        }

        private int GetTranspositions(String common1, String common2)
        {
            int retVal = 0;
            for (int i = 0; i < common1.Length; i++)
            {
                if (common1[i] != common2[i])
                    retVal++;
            }
            retVal /= 2;
            return retVal;
        }

        private static int OrtakBaslangicUzunlugu(int maxLength, String common1, String common2)
        {
            int n = Math.Min(maxLength, Math.Min(common1.Length, common2.Length));
            for (int i = 0; i < n; i++)
            {
                if (common1[i] != common2[i]) return i;
            }
            return n; // ilk n karakter ayni
        }

    }
}
