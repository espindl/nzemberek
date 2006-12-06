//Created on 15.Mar.2004
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using net.zemberek.istatistik;
using log4net;


namespace net.zemberek.araclar.turkce
{
    /**
     * TurkishTokenStream
     * Verilen bir doayadan veya herhangi bir stream'dan Türkce kelimeleri
     * sirayla almak için kullanilir. Ýki constructor'u vardýr, istenirse verilen bir
     * dosyayi istenirse de herhangi bir inputstream'ý isleyebilir.
     * Biraz optimizasyona ihtiyaci var ,ama corpus.txt deki tüm kelimeleri tek tek
     * nextWord() ile cekmek yaklasik 0.8 saniye aldi. (Athlon 900)
     *
     * @author MDA & GBA
     */
    public class TurkishTokenStream
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        StreamReader bis = null;
        Istatistikler statistics = null;
        char[] buffer = new char[1000];

        /**
         * Dosyadan kelime okuyan TurkishTokenStream oluþturur
         *
         * @param fileName
         * @param encoding: default için null verin
         */
        public TurkishTokenStream(String fileName, String encoding)
        {
            try 
            {
                Stream fis = new FileStream(fileName, FileMode.Open);
                setupReader(fis, encoding);
            } catch (FileNotFoundException e)
            {
                logger.Error(e.StackTrace);
            }
        }

        /**
         * Herhangibir input Streaminden'den kelime okuyan TurkishTokenStream oluþturur.
         *
         * @param is
         * @param encoding : default için null verin
         */
        public TurkishTokenStream(Stream ins, String encoding)
        {
            setupReader(ins, encoding);
        }

        private void setupReader(Stream ins, String encoding)
        {
            if (encoding == null)
            {
                bis = new StreamReader(ins);
            }
            else
            {
                try
                {
                    bis = new StreamReader(ins, Encoding.GetEncoding(encoding));
                }
                catch (Exception e)
                {
                    logger.Error(e.StackTrace);
                }
            }
        }

        public static int MAX_KELIME_BOY = 256;
        private char[] kelimeBuffer = new char[MAX_KELIME_BOY];

        /**
         * Metindeki veya stream'deki bir sonraki kelimeyi getirir
         * - Büyük harfleri küçültür
         * - Noktalama iþaretlerini yutar.
         *
         * @return Sonraki kelime, eðer kelime kalmamýþsa null
         */
        public String nextWord() {
            char ch;
            int readChar;
            bool kelimeBasladi = false;
            int kelimeIndex = 0;
            bool hypen = false;
            try {
                // TODO: bir char buffer'e toptan okuyup islemek hýz kazandirir mi?
                while ((readChar = bis.Read()) != -1) {
                    ch = (char) readChar;
                    if (statistics != null) {
                        statistics.processChar(ch);
                    }
                    if (ch == '-') {
                        hypen = true;
                        continue;
                    }
                    if (Char.IsLetter(ch)) {
                        kelimeBasladi = true;
                        hypen = false;
                        //cumleBasladi = true;
                        switch (ch) {
                            case 'I':
                                ch = '\u0131';
                                break; // dotless small i
                                // Buraya sapkalý a vs. gibi karakter donusumlari de eklenebilir.
                            default  :
                                ch = Char.ToLower(ch);
                                break;
                        }
                        if (kelimeIndex < MAX_KELIME_BOY)
                            kelimeBuffer[kelimeIndex++] = ch;
                        else
                            logger.Warn("Lagim tasti " + ch);
                        continue;
                    }

                    if (Char.IsWhiteSpace(ch)) {
                        if (hypen) continue;

                        if (kelimeBasladi) {
                            return new String(kelimeBuffer, 0, kelimeIndex);
                        }
                        kelimeBasladi = false;
                        kelimeIndex = 0;
                        continue;
                    } 
                    
                    // harfimiz bir cumle sinirlayici
                    if (isSentenceDelimiter(ch)) {
                        /* if (cumleBasladi)
                         {
                             // kelimeyi cumleye ekle.
                             cumleTamamlandi = true;
                         }
                         continue;*/
                    }

                }
                
                // Tüm karakterler bitti, son kalan kelime varsa onu da getir.
                if (kelimeBasladi) {
                    return new String(kelimeBuffer, 0, kelimeIndex);
                }
            } catch (IOException e) 
            {
                logger.Error(e.StackTrace);
            }
            return null;
        }


        public static int MAX_CUMLE_BOY = 4000;
        private char[] cumleBuffer = new char[MAX_CUMLE_BOY];

        /**
         * Metindeki veya stream'deki bir sonraki cümleyi getirir
         * @return Sonraki cümle, eðer kalmamýþsa null
         */
        public String nextSentence() {
            char ch;
            int readChar;
            bool cumleBasladi = false;
            int cumleIndex = 0;
            try {
                // TODO: bir char buffer'e toptan okuyup islemek hýz kazandirir mi? (sanmam)
                while ((readChar = bis.Read()) != -1) {
                    ch = (char) readChar;

                    if (Char.IsLetter(ch)) {
                        cumleBasladi = true;
                        switch (ch) {
                            case 'I':
                                ch = '\u0131';
                                break; // dotless small i
                                // Buraya sapkalý a vs. gibi karakter donusumlari de eklenebilir.
                            default  :
                                ch = Char.ToLower(ch);
                                break;
                        }
                        if (cumleIndex < MAX_CUMLE_BOY)
                            cumleBuffer[cumleIndex++] = ch;
                        else
                            logger.Warn("Lagim tasti " + ch);
                        continue;
                    }

                    // harfimiz bir cumle sinirlayici
                    if (isSentenceDelimiter(ch)) {
                        if (cumleBasladi) {
                            return new String(cumleBuffer, 0, cumleIndex);
                        }
                        continue;
                    }

                    if (cumleIndex < MAX_CUMLE_BOY)
                        cumleBuffer[cumleIndex++] = ch;
                    else 
                    {
                        logger.Error("Lagim tasti " + ch);
                        return null;
                    }

                }
                
                // Tüm karakterler bitti, son kalan kelime varsa onu da getir.
                if (cumleBasladi) {
                    return new String(kelimeBuffer, 0, cumleIndex);
                }
            } catch (IOException e) 
            {
                logger.Error(e.StackTrace);
            }
            return null;
        }

        public char harfIsle(char chIn) {
            char ch;
            switch (chIn) {
                case 'I':
                    ch = '\u0131';
                    break; // dotless small i
                    // Buraya sapkalý a vs. gibi karakter donusumlari de eklenebilir.
                default  :
                    ch = Char.ToLower(chIn);
                    break;
            }
            return ch;
        }

        public bool isSentenceDelimiter(char ch) {
            if (ch == '.' ||
                    ch == ':' ||
                    ch == '!' ||
                    ch == '?'
            )
                return true;
            return false;
        }
        
        public void setStatistics(Istatistikler statistics) {
            this.statistics = statistics;
        }

    }
}




