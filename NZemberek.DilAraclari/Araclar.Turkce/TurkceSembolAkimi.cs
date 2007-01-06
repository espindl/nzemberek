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
using System.Collections.Generic;
using System.Text;
using log4net;

namespace NZemberek.DilAraclari.MetinOkuma
{
    /// <summary>
    /// TurkceSembolAkimi
    /// Verilen bir doayadan veya herhangi bir stream'dan Türkce kelimeleri sirayla almak için kullanilir.
    /// Ýki constructor'u vardýr, istenirse verilen bir dosyayi istenirse de herhangi bir inputstream'ý isleyebilir.
    /// Biraz optimizasyona ihtiyaci var.
    /// </summary>
    public class TurkceSembolAkimi
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static int MAX_KELIME_BOY = 256;
        public static int MAX_CUMLE_BOY = 4000;
        
        private StreamReader _streamReader = null;
        private char[] kelimeBuffer = new char[MAX_KELIME_BOY];
        private char[] cumleBuffer = new char[MAX_CUMLE_BOY];

        /// <summary>
        /// Dosyadan kelime okuyan TurkishTokenStream oluþturur
        /// </summary>
        /// <param name="fileName">fileName</param>
        /// <param name="encoding">encoding: default için null verin</param>
        public TurkceSembolAkimi(String fileName, String encoding)
        {
            try
            {
                Stream fileStream = new FileStream(fileName, FileMode.Open);
                this.setupReader(fileStream, encoding);
            }
            catch (FileNotFoundException e)
            {
                logger.Error("Dosya bulunamadý. Aranan dosya:" + fileName +  e.StackTrace);
            }
        }

        /// <summary>
        /// Herhangibir input Streaminden'den kelime okuyan TurkishTokenStream oluþturur.
        /// </summary>
        /// <param name="ins">ins</param>
        /// <param name="encoding">encoding: default için null verin</param>
        public TurkceSembolAkimi(Stream inputStream, String encoding)
        {
            this.setupReader(inputStream, encoding);
        }

        /// <summary>
        /// Doðrudan StreamReader verilen constructor *mert
        /// </summary>
        /// <param name="streamReader">streamReader</param>
        public TurkceSembolAkimi(StreamReader streamReader)
        {
            _streamReader = streamReader;
        }

        private void setupReader(Stream inputStream, String encoding)
        {
            if (encoding == null)
            { _streamReader = new StreamReader(inputStream); }
            else
            {
                try
                {
                    _streamReader = new StreamReader(inputStream, Encoding.GetEncoding(encoding));
                }
                catch (Exception e)
                {
                    logger.Error("Streamreader olusturulurken hata olustu." + e.StackTrace);
                }
            }
        }

        /// <summary>
        /// Metindeki veya stream'deki bir sonraki kelimeyi getirir
        /// - Büyük harfleri küçültür > neden@mert
        /// - Noktalama iþaretlerini yutar  > neden@mert
        /// </summary>
        public String nextWord()
        {
            char ch;
            int readChar;
            bool kelimeBasladi = false;
            int kelimeIndex = 0;
            bool hypen = false;
            try
            {
                while ((readChar = _streamReader.Read()) != -1)
                {
                    ch = (char)readChar;
                    if (ch == '-')
                    {
                        hypen = true;
                        continue;
                    }
                    if (Char.IsLetter(ch) || (kelimeBasladi == true & (ch == '\'' || ch == '-')))
                    {
                        kelimeBasladi = true;
                        hypen = false;
                        if (kelimeIndex < MAX_KELIME_BOY)
                            kelimeBuffer[kelimeIndex++] = ch;
                        else
                            System.Console.WriteLine("Maksimum kelime boyu asildi. " + ch);
                        continue;
                    }

                    if (Char.IsWhiteSpace(ch))
                    {
                        if (hypen) continue;

                        if (kelimeBasladi)
                        {
                            return new String(kelimeBuffer, 0, kelimeIndex);
                        }
                        kelimeBasladi = false;
                        kelimeIndex = 0;
                        continue;
                    }
                }

                // Tum karakterler bitti, son kalan kelime varsa onu da getir.
                if (kelimeBasladi)
                {
                    return new String(kelimeBuffer, 0, kelimeIndex);
                }
            }
            catch (IOException e)
            {
                logger.Error("Kelimeler okunurken hata olustu. " + e.StackTrace);
            }
            return null;
        }

        ///// <summary>
        ///// Metindeki veya stream'deki bir sonraki cümleyi getirir
        ///// @return Sonraki cümle, eðer kalmamýþsa null
        ///// </summary>
        //public String nextSentence()
        //{
        //    char ch;
        //    int readChar;
        //    bool cumleBasladi = false;
        //    int cumleIndex = 0;
        //    try
        //    {
        //        // TODO: bir char buffer'e toptan okuyup islemek hýz kazandirir mi? (sanmam)
        //        while ((readChar = _streamReader.Read()) != -1)
        //        {
        //            ch = (char)readChar;

        //            if (Char.IsLetter(ch))
        //            {
        //                cumleBasladi = true;
        //                switch (ch)
        //                {
        //                    case 'I':
        //                        ch = '\u0131';
        //                        break; // dotless small i
        //                    // Buraya sapkalý a vs. gibi karakter donusumlari de eklenebilir.
        //                    default:
        //                        ch = Char.ToLower(ch);
        //                        break;
        //                }
        //                if (cumleIndex < MAX_CUMLE_BOY)
        //                    cumleBuffer[cumleIndex++] = ch;
        //                else
        //                    logger.Warn("Lagim tasti " + ch);
        //                continue;
        //            }

        //            // harfimiz bir cumle sinirlayici
        //            if (isSentenceDelimiter(ch))
        //            {
        //                if (cumleBasladi)
        //                {
        //                    return new String(cumleBuffer, 0, cumleIndex);
        //                }
        //                continue;
        //            }

        //            if (cumleIndex < MAX_CUMLE_BOY)
        //                cumleBuffer[cumleIndex++] = ch;
        //            else
        //            {
        //                logger.Error("Lagim tasti " + ch);
        //                return null;
        //            }

        //        }

        //        // Tüm karakterler bitti, son kalan kelime varsa onu da getir.
        //        if (cumleBasladi)
        //        {
        //            return new String(kelimeBuffer, 0, cumleIndex);
        //        }
        //    }
        //    catch (IOException e)
        //    {
        //        logger.Error(e.StackTrace);
        //    }
        //    return null;
        //}

        //public bool isSentenceDelimiter(char ch)
        //{
        //    if (ch == '.' ||
        //            ch == ':' ||
        //            ch == '!' ||
        //            ch == '?'
        //    )
        //        return true;
        //    return false;

        //}
        
        //public char harfIsle(char chIn)
        //{
        //    char ch;
        //    switch (chIn)
        //    {
        //        case 'I':
        //            ch = '\u0131';
        //            break; // dotless small i
        //        // Buraya sapkalý a vs. gibi karakter donusumlari de eklenebilir.
        //        default:
        //            ch = Char.ToLower(chIn);
        //            break;
        //    }
        //    return ch;
        //}

        //public void setStatistics(Istatistikler statistics) {
        //    this.statistics = statistics;
        //}

    }
}




