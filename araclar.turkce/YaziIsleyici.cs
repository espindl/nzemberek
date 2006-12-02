using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
/*
import java.io.*;
import java.text.BreakIterator;
import java.util.ArrayList;
import java.util.IList;
import java.util.Locale;
*/

namespace net.zemberek.araclar.turkce
{
    /**
     * Metinler uzerinde ayristirma islemlerini kolaylastirmak icin yazilmis bir sinif. 
     * Static methodlari kullanarak metin icerisindeki kelimelere, cumlelere ulasim sagliyor.
     */
    public class YaziIsleyici
    {
        //TODO : BreakIterator yerine string.Split kullanýyoruz, hiç yoktan iyidir ama iyileþtirme gerekir.
        //public static BreakIterator kelimeIt = BreakIterator.getWordInstance(new Locale("tr"));
        //public static BreakIterator cumleIt = BreakIterator.getSentenceInstance(new Locale("tr"));
        private static readonly char[] kelimeAyrac = new char[] { ' ', '.', ',', '?', '!' };
        private static readonly char[] cumleAyrac = new char[] { '.', '?', '!' };

        /**
         * Verilen metni {@link java.text.BreakIterator} kullanarak kelimelerine ayirir. Noktalama isaretleri filtrelenir
         *
         * @param target
         * @return metin kelimeleri liste icerisinde String olarak dondurulur.
         */
        public static IList kelimeAyikla(String target) 
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

        /**
         * @param target
         * @return
         */
        /*TODO: Please review if this method needs to be here
        */

        public static IList analizIcinKelimeAyikla(String target) {
            IList cumleler = cumleAyikla(target);
            IList tumKelimeler = new ArrayList();
            for (int i = 0; i < cumleler.Count; i++) {
                String cumle = (String) cumleler[i];
                IList kelimeler = kelimeAyikla(cumle);
                for (int j = 0; j < kelimeler.Count; j++)
                {
                    String kelime = (String) kelimeler[j];
                    if (Char.IsLower(kelime[0])) {
                        char[] chrs = kelime.ToCharArray();
                        bool nokta = false;
                        for (int k = 0; k < chrs.Length; k++) {
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

        public static IList cumleAyikla(String target) 
        {
            string[] splits = target.Split();
            IList cumleList = new ArrayList();
            foreach (string s in splits)
            {
                cumleList.Add(s);
            }
            return cumleList;
        }

        /**
         * Verilen metnin icinde gecen kelimeler {@link YaziBirimi} listesi  halinde dondururlur.
         *
         * @param target
         * @return
         */
        public static IList<YaziBirimi> analizDizisiOlustur(String target) 
        {
            string[] splits = target.Split();
            IList kelimeList = new ArrayList();
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

        public static String yaziOkuyucu(String fileName) 
        {
            StringBuilder sb = new StringBuilder();
            Stream fis = new FileStream(fileName, FileMode.Open);
            StreamReader bis = new StreamReader(fis, Encoding.GetEncoding("ISO-8859-9"));
            String s;
            while ((s = bis.ReadLine()) != null) {
                sb.Append(s);
                sb.Append("\n");
            }
            bis.Close();
            return sb.ToString();
        }
        }
}



