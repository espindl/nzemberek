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
using System.Collections.Generic;
using System.Text;

namespace NZemberek.DilAraclari.MetinOkuma
{
    /// <summary>
    /// TurkceSembolAkimi
    /// Verilen bir doayadan veya herhangi bir stream'dan T�rkce kelimeleri sirayla almak i�in kullanilir.
    /// �ki constructor'u vard�r, istenirse verilen bir dosyayi istenirse de herhangi bir inputstream'� isleyebilir.
    /// Biraz optimizasyona ihtiyaci var.
    /// </summary>
    public class TurkceSembolAkimi
    {
        private static int MAX_KELIME_BOY = 256;
        public static int MAX_CUMLE_BOY = 4000;
        
        private StreamReader _streamReader = null;
        private char[] kelimeBuffer = new char[MAX_KELIME_BOY];
        private char[] cumleBuffer = new char[MAX_CUMLE_BOY];

        /// <summary>
        /// Dosyadan kelime okuyan TurkishTokenStream olu�turur
        /// </summary>
        /// <param name="fileName">fileName</param>
        /// <param name="encoding">encoding: default i�in null verin</param>
        public TurkceSembolAkimi(String fileName, String encoding)
        {
            try
            {
                Stream fileStream = new FileStream(fileName, FileMode.Open);
                this.setupReader(fileStream, encoding);
            }
            catch (FileNotFoundException e)
            {
            }
        }

        /// <summary>
        /// Herhangibir input Streaminden'den kelime okuyan TurkishTokenStream olu�turur.
        /// </summary>
        /// <param name="ins">ins</param>
        /// <param name="encoding">encoding: default i�in null verin</param>
        public TurkceSembolAkimi(Stream inputStream, String encoding)
        {
            this.setupReader(inputStream, encoding);
        }

        /// <summary>
        /// Do�rudan StreamReader verilen constructor *mert
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
                }
            }
        }

        /// <summary>
        /// Metindeki veya stream'deki bir sonraki kelimeyi getirir
        /// - B�y�k harfleri k���lt�r > neden@mert
        /// - Noktalama i�aretlerini yutar  > neden@mert
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
            }
            return null;
        }
    }
}




