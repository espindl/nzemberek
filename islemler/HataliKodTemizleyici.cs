using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using log4net;
using net.zemberek.bilgi;


namespace net.zemberek.islemler
{
    public class HataliKodTemizleyici
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        System.Collections.Generic.IDictionary<char,IList> donusumler = new Dictionary<char,IList>();

        public void initialize() 
        {
            //kodlama karsiliklari dosyasini oku
            StreamReader reader = new KaynakYukleyici().getReader("kaynaklar/tr/bilgi/kodlama-donusum.txt");
            String s;
            while ((s = reader.ReadLine()) != null) {
                // bos ve # isaretli satirlari atla
                if (s.Length == 0 || s[0] == '#')
                    continue;
                s = toNative(s);
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

        /**
         * Biraz balta bir metodla kodlara karsilik dusulen
         *
         * @param giris
         * @return String'in temizlenmis hali.
         */
        public String temizle(String giris) {
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

        /**
         * odun bir yontemle "uxxxx" seklindeki string'ler karakter karsiliklarina
         *
         * @param str
         * @return unicode karsilik.
         */
        private String toNative(String str) {
            bool end = false;
            StringBuilder yeni = new StringBuilder();
            while (true) {
                if (str.StartsWith("\\u")) {
                    char c = (char) int.Parse(str.Substring(2, 6),System.Globalization.NumberStyles.AllowHexSpecifier);
                    yeni.Append(c);
                    str = str.Substring(6);
                } else {
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






