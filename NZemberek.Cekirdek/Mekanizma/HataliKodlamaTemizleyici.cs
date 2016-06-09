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
 * The Original Code is Zemberek Do�al Dil ��leme K�t�phanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Ak�n, Mehmet D. Ak�n.
 * Portions created by the Initial Developer are Copyright (C) 2006
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Mert Derman
 *   Tankut Tekeli
 * ***** END LICENSE BLOCK ***** */

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NZemberek.Cekirdek.Araclar;

namespace NZemberek.Cekirdek.Mekanizma
{
    public class HataliKodlamaTemizleyici
    {
        System.Collections.Generic.IDictionary<char,IList> donusumler = new Dictionary<char,IList>();

        public void Baslat() 
        {
            //kodlama karsiliklari dosyasini oku
            StreamReader reader = new KaynakYukleyici().OkuyucuGetir("kaynaklar/tr/bilgi/kodlama-donusum.txt");
            String s;
            while ((s = reader.ReadLine()) != null) {
                // bos ve # isaretli satirlari atla
                if (s.Length == 0 || s[0] == '#')
                    continue;
                s = YereleDonustur(s);
                Char c = s[0];

                //satirdan turkce karaktere karsilik duzen kod cekiliyor. ve bu kod map'a yerletiriliyor
                String kod = s.Substring(2);
                if (donusumler.ContainsKey(c)) {
                    IList a = donusumler[c];
                    a.Add(kod);
                } else {
                    IList yeni = new ArrayList();
                    yeni.Add(kod);
                    donusumler.Add(c, yeni);
                }
            }
        }

        /// <summary>
        /// Biraz balta bir metodla kodlara karsilik dusulen
        /// </summary>
        /// <param name="giris"></param>
        /// <returns>String'in temizlenmis hali</returns>
        public String Temizle(String giris) {
            //keys icerisinde turkce karakterler yer aliyor
            ICollection<char> keys = donusumler.Keys;
            foreach (char c in keys) 
            {
                //values icinde "karakter"e karsilik dusen karakter dizileri bulunuyor
                IList values = donusumler[c];
                for (int i = 0; i < values.Count; i++) {
                    String kod = (String) values[i];
                    //asagidaki kod efektif degil ama is gorur. caktirmadan regexp kullaniyor.
                    giris = giris.Replace(kod, c.ToString());
                }
            }
            return giris;
        }

        /// <summary>
        /// Odun bir yontemle "uxxxx" seklindeki string'ler karakter karsiliklarina
        /// </summary>
        /// <param name="str"></param>
        /// <returns>unicode karsilik</returns>
        private String YereleDonustur(String str) {
            StringBuilder yeni = new StringBuilder();
            while (true) 
            {
                if (str.StartsWith("\\u")) {
                    char c = (char) int.Parse(str.Substring(2, 4),System.Globalization.NumberStyles.AllowHexSpecifier);
                    yeni.Append(c);
                    str = str.Substring(6);
                } else 
                {
                    if (str.Length == 0)
                        break;
                    yeni.Append(str[0]);
                    str = str.Substring(1);
                }
            }
            return yeni.ToString();
        }
    }
}






