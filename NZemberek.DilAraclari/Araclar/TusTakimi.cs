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

/*
 * Created on 30.Tem.2005
 * 
 * V0.1
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace NZemberek.DilAraclari.Araclar
{
    public class TusTakimi
    {

        // Standart Türkçe Q Klavye haritasý.
        public static char[][] qKlavyeHaritasi = new char[][]{
        new char[]{'1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '*', '-'},
        new char[]{'q', 'w', 'e', 'r', 't', 'y', 'u', '\u0131', 'o', 'p', '\u011f', '\u00fc'},
        new char[]{'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', '\u015f', 'i', ',', '#'},
        new char[]{'z', 'x', 'c', 'v', 'b', 'n', 'm', '\u00f6', '\u00e7', '.', '.','.'}};

        
        // Standart Türkçe F Klavye haritasý.
        public static char[][] fKlavyeHaritasi = new char[][]{
        new char[]{'1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '*', '-'},
        new char[]{'f', 'g', '\u011f', '\u0131', 'o', 'd', 'r', 'n', 'h', 'p', 'q', 'w'},
        new char[]{'u', 'i', 'e', 'a', '\u00fc', 't', 'k', 'm', 'l', 'y','\u015f', 'x'},
        new char[]{'j', '\u00f6', 'v', 'c', '\u00e7', 'z', 's', 'b', '.', ','}};


        private char[][] klavyeHaritasi = null;
        private IDictionary<Char, KarakterKoordinati> koordinatlar = new Dictionary<Char, KarakterKoordinati>();

        private static TusTakimi q = new TusTakimi(qKlavyeHaritasi);
        private static TusTakimi f = new TusTakimi(fKlavyeHaritasi);

        public TusTakimi(char[][] klavyeHaritasi)
        {
            this.klavyeHaritasi = klavyeHaritasi;
            for (int i = 0; i < klavyeHaritasi.Length; i++)
            {
                for (int j = 0; j < klavyeHaritasi[i].Length; j++)
                {
                    char c = klavyeHaritasi[i][j];
                    koordinatlar.Add(c, new KarakterKoordinati(c, i, j));
                }
            }
        }

        public static TusTakimi trQ()
        {
            return q;
        }

        public static TusTakimi trF()
        {
            return f;
        }

        public KarakterKoordinati koordinat(char c)
        {
            return koordinatlar[c];
        }

        public KarakterKoordinati koordinat(int i, int j)
        {
            return koordinatlar[klavyeHaritasi[i][j]];
        }


        /**
         * Verilen iki karakter arasýndaki klavye mesafesini getirir.      
         * Mesafenin ölçümünde sadece karakterlerin koordinatlarý arasýndaki mesafe 
         * sqrt((y2-y1)^2 + (x2-x1)^2))*10 þeklinde hesaplanýyor. Yani a-s arasýndaki mesafe 
         * 10 iken s-q arasýndaki mesafe 14, a-e arasýnda ise 22 olur. Karakterlerin 
         * klavyedeki yerleri arasýndaki  mesafe arttýkça rakam büyür.
         * 
         * @param c1 birinci karakter
         * @param c2 ikinci karakter
         * @return  Q Klavye üzerinde verilen karakterler arasýndaki fiziksel mesafenin 10 katý.
         * Eðer karakterlerden herhangi biri klavye haritasýnda yoksa -1 döner.
         *  
         */
        public int mesafeHesapla(char c1, char c2)
        {
            KarakterKoordinati k1 = (KarakterKoordinati)koordinatlar[c1];
            KarakterKoordinati k2 = (KarakterKoordinati)koordinatlar[c2];
            if (k1 == null || k2 == null)
            {
                return -1;
            }
            double mesafeD = Math.Sqrt((double)(k1.x - k2.x) * (k1.x - k2.x) + (k1.y - k2.y) * (k1.y - k2.y));
            int normalizeMesafe = (int)(mesafeD * 10);
            return normalizeMesafe;
        }

        public String ToStirng()
        {
            return koordinatlar.ToString();
        }


        public class KarakterKoordinati
        {
            char c;
            internal int x, y;

            public KarakterKoordinati(char c, int x, int y)
            {
                this.c = c;
                this.x = x;
                this.y = y;
            }

            public override String ToString()
            {
                return "(" + x + "," + y + ") " + c;
            }
        }

    }
}