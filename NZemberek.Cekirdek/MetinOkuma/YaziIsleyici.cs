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
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NZemberek.Cekirdek.MetinOkuma
{
    /// <summary>
    /// Metinler uzerinde ayristirma islemlerini kolaylastirmak icin yazilmis bir sinif. 
    /// Static methodlari kullanarak metin icerisindeki kelimelere, cumlelere ulasim sagliyor.
    /// </summary>
    public class YaziIsleyici
    {
        //TODO : BreakIterator yerine string.Split kullanýyoruz, hiç yoktan iyidir ama iyileþtirme gerekir.
        //public static BreakIterator kelimeIt = BreakIterator.getWordInstance(new Locale("tr"));
        //public static BreakIterator cumleIt = BreakIterator.getSentenceInstance(new Locale("tr"));
        private static readonly char[] kelimeAyrac = new char[] { ' ', '.', ',', '?', '!' };
        private static readonly char[] cumleAyrac = new char[] { '.', '?', '!' };

        /// <summary>
        /// Verilen metni {@link java.text.BreakIterator} kullanarak kelimelerine ayirir. Noktalama isaretleri filtrelenir
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static IList KelimeAyikla(String target)
        {
            string[] splits = target.Split();
            IList kelimeList = new ArrayList();
            foreach (string s in splits)
            {
                if (Char.IsLetterOrDigit(s[0]))
                {
                    kelimeList.Add(s);
                }
            }
            return kelimeList;
        }

        // TODO: Please review if this method needs to be here
        public static IList AnalizIcinKelimeAyikla(String target)
        {
            IList cumleler = CumleAyikla(target);
            IList tumKelimeler = new ArrayList();
            for (int i = 0; i < cumleler.Count; i++)
            {
                String cumle = (String)cumleler[i];
                IList kelimeler = KelimeAyikla(cumle);
                for (int j = 0; j < kelimeler.Count; j++)
                {
                    String kelime = (String)kelimeler[j];
                    if (Char.IsLower(kelime[0]))
                    {
                        char[] chrs = kelime.ToCharArray();
                        bool nokta = false;
                        for (int k = 0; k < chrs.Length; k++)
                        {
                            if (chrs[k] == '.' || chrs[k] == '-')
                                nokta = true;
                        }
                        if (!nokta)
                            tumKelimeler.Add(kelime);
                    }
                }
            }
            return tumKelimeler;
        }

        public static IList CumleAyikla(String target)
        {
            string[] splits = target.Split();
            IList cumleList = new ArrayList();
            foreach (string s in splits)
            {
                cumleList.Add(s);
            }
            return cumleList;
        }

        /// <summary>
        /// Verilen metnin icinde gecen kelimeler {@link YaziBirimi} listesi  halinde dondururlur.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static IList<YaziBirimi> AnalizDizisiOlustur(String target)
        {
            string[] splits = target.Split();
            IList<YaziBirimi> yaziBirimleri = new System.Collections.Generic.List<YaziBirimi>();
            foreach (string s in splits)
            {
                if (Char.IsLetterOrDigit(s[0]))
                {
                    yaziBirimleri.Add(new YaziBirimi(YaziBirimiTipi.KELIME, s));
                }
                yaziBirimleri.Add(new YaziBirimi(YaziBirimiTipi.DIGER, s));
            }

            return yaziBirimleri;
        }

        public static String YaziOkuyucu(String fileName)
        {
            StringBuilder sb = new StringBuilder();
            Stream fis = new FileStream(fileName, FileMode.Open);
            StreamReader bis = new StreamReader(fis, Encoding.GetEncoding("ISO-8859-9"));
            String s;
            while ((s = bis.ReadLine()) != null)
            {
                sb.Append(s);
                sb.Append("\n");
            }
            bis.Close();
            return sb.ToString();
        }
    }
}



