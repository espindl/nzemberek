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
using System.IO;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Configuration;
using log4net;

namespace NZemberek.Cekirdek.Araclar
{
    public class KaynakYukleyici
    {
        /// <summary>
        /// Default constructor. Okuma sirasinda sistemde varsayilan kodlama kullanilir.
        /// </summary>
        public KaynakYukleyici()
        {
            this.encoding = Encoding.Default;
            logger.Info("Kaynak yukleyici olusturuluyor. varsayilan karakter seti:" + encoding);
        }

        public KaynakYukleyici(string enc)
        {
            this.encoding = Encoding.GetEncoding(enc);
            logger.Info("Kaynak yukleyici olusturuluyor. varsayilan karakter seti:" + encoding);
        }

        public KaynakYukleyici(Encoding encoding)
        {
            this.encoding = encoding;
            logger.Info("Kaynak yukleyici olusturuluyor. varsayilan karakter seti:" + encoding);
        }

        private Encoding encoding;
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);		

        /// <summary>
        /// properties formatina benzer yapidaki dosyayi kodlamali olarak okur. Normal properties dosyalari ASCII
        /// okundugundan turkce karakterlere uygun degil. Dosya icindeki satirlarin
        /// anahtar=deger seklindeki satirlardan olusmasi gerekir. dosya icindeki yorumlar
        /// # yorum seklinde ifade edilir.
        /// </summary>
        /// <param name="dosyaAdi"></param>
        /// <returns></returns>
        public IDictionary<String, String> KodlamaliOzellikDosyasiOku(String dosyaAdi)
        {
            StreamReader reader  = null;
            IDictionary<String, String> ozellikler;
            try
            {
                reader = new StreamReader(dosyaAdi, this.encoding);
                ozellikler = new Dictionary<String, String>();
                while (!reader.EndOfStream)
                {
                    String line = reader.ReadLine().Trim();
                    if (line.Length == 0 || line.StartsWith("#"))
                        continue;
                    int esitlik = line.IndexOf('=');
                    if (esitlik == -1)
                        throw new ArgumentException("Ozellik satirinda '=' simgesi bekleniyordu");
                    String key = line.Substring(0, esitlik).Trim();
                    if (line.Length > esitlik - 1)
                        ozellikler.Add(key, line.Substring(esitlik+1).Trim());
                    else
                        ozellikler.Add(key, "");
                }
                return ozellikler;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        /// <summary>
        /// istenilen kaynaga erisimin mumkun olup olmadigina bakar. Bazi secimlik kaynaklarin erisiminde
        /// bu metoddan yararlanilabilir.
        /// </summary>
        /// <param name="kaynakAdi"></param>
        /// <returns>true-> kaynak erisiminde hata olusmadi false-> kaynak erisiminde hata olustu ya da kaynak=null</returns>
        public static bool KaynakMevcut(String kaynakAdi)
        {
            return File.Exists(Environment.CurrentDirectory + System.IO.Path.DirectorySeparatorChar + kaynakAdi);
        }

        /// <summary>
        /// Girilen kaynaga once class path disindan erismeye calisir. Eger dosya bulunamazsa
        /// bu defa ayni dosyaya classpath icerisinden erismeye calisir.
        /// </summary>
        /// <param name="kaynakAdi"></param>
        /// <returns>kaynak erisimi icin Buffered reader</returns>
        public StreamReader OkuyucuGetir(String kaynakAdi)
        {
            StreamReader sr = new StreamReader(AkimGetir(kaynakAdi));
            if (sr == null)
                throw new IOException(kaynakAdi + " erisimi saglanamadi. Elde edilen Stream degeri null!");
            return sr;
        }

        /// <summary>
        /// Kaynak adi ile stream doner
        /// </summary>
        /// <param name="kaynakAdi"></param>
        /// <returns></returns>
        public Stream AkimGetir(String kaynakAdi)
        {
            FileStream stream = new FileStream(kaynakAdi, FileMode.Open);
            return stream;
        }
    }
}
