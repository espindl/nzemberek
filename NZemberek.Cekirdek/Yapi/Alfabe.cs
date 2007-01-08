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
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using log4net;
using System.Xml.Serialization;
using System.IO;
using NZemberek.Cekirdek.Araclar;

namespace NZemberek.Cekirdek.Yapi
{
    public class Alfabe
    {
        // Turkce ozel
        public static char CHAR_CC = '\u00c7'; // Kuyruklu buyuk c (ch)
        public static char CHAR_cc = '\u00e7'; // Kuyruklu kucuk c (ch)
        public static char CHAR_GG = '\u011e'; // Buyuk yumusak g
        public static char CHAR_gg = '\u011f'; // Kucuk yumusak g
        public static char CHAR_ii = '\u0131'; // Noktassiz kucuk i
        public static char CHAR_II = '\u0130'; // Noktali buyuk i
        public static char CHAR_OO = '\u00d6'; // Noktali buyuk o
        public static char CHAR_oo = '\u00f6'; // Noktali kucuk o
        public static char CHAR_SS = '\u015e'; // Kuyruklu buyuk s (sh)
        public static char CHAR_ss = '\u015f'; // Kuyruklu kucuk s (sh)
        public static char CHAR_UU = '\u00dc'; // Noktali buyuk u
        public static char CHAR_uu = '\u00fc'; // Noktali kucuk u

        // Azeri ozel
        public static char CHAR_ee = '\u0259'; // ?
        public static char CHAR_EE = '\u018f'; // ?

        // Turkmen ozel
        public static char CHAR_AA = '\u00c4'; // Noktali buyuk A
        public static char CHAR_aa = '\u00e4'; // Noktali kucuk a
        public static char CHAR_NN = '\u0147'; // Kuyruklu buyuk N
        public static char CHAR_nn = '\u0148'; // Kuyruklu kucuk n
        public static char CHAR_YY = '\u00dd'; // Kuyruklu buyuk Y
        public static char CHAR_yy = '\u00fd'; // Kuyruklu kucuk y
        public static char CHAR_JJ = '\u017d'; // J harfi
        public static char CHAR_jj = '\u017e'; //j harfi

        // Sapkali
        public static char CHAR_SAPKALI_A = '\u00c2'; // sapkali buyuk A
        public static char CHAR_SAPKALI_a = '\u00e2'; // sapkali kucuk A
        public static char CHAR_SAPKALI_I = '\u00ce'; // sapkali buyuk noktasiz i
        public static char CHAR_SAPKALI_i = '\u00ee'; // sapkali kucuk noktasiz i
        public static char CHAR_SAPKALI_U = '\u00db'; // sapkali buyuk U
        public static char CHAR_SAPKALI_u = '\u00fb'; // sapkali kucuk u

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static TurkceHarf TANIMSIZ_HARF = new TurkceHarf('#', 0);

        public const char ALFABE_DISI_KARAKTER = '#';
        protected internal const int TURKISH_CHAR_MAP_SIZE = 610;
        protected internal const int TURKISH_HARF_MAP_SIZE = 50;
        protected internal char[] temizlemeDizisi = new char[TURKISH_CHAR_MAP_SIZE];
        protected internal char[] asciifierDizisi = new char[TURKISH_CHAR_MAP_SIZE];
        protected internal TurkceHarf[] turkceHarfDizisi = new TurkceHarf[TURKISH_CHAR_MAP_SIZE];
        protected internal TurkceHarf[] kucukHarflerDizi = new TurkceHarf[TURKISH_HARF_MAP_SIZE];
        protected internal TurkceHarf[] buyukHarflerDizi = new TurkceHarf[TURKISH_HARF_MAP_SIZE];
        protected internal bool[] turkceMap = new bool[TURKISH_CHAR_MAP_SIZE];
        protected internal sbyte[] alfabetikSiralar = new sbyte[TURKISH_CHAR_MAP_SIZE];

        protected IDictionary<Char, TurkceHarf> harfler = new Dictionary<Char, TurkceHarf>();
        private char[] asciiDisi = new char[20];

        private IDictionary<TurkceHarf, TurkceHarf> ozelInceSesliler = new Dictionary<TurkceHarf, TurkceHarf>();

        public Alfabe(String dosyaAdi, String localeStr)
        {
            IDictionary<String, String> harfOzellikleri;
            harfOzellikleri = new KaynakYukleyici().KodlamaliOzellikDosyasiOku(dosyaAdi);
            this.locale = new System.Globalization.CultureInfo(localeStr);
            DiziBaslat();
            HarfBilgisiOlustur(harfOzellikleri);
        }

        private void DiziBaslat()
        {

            for (int i = 0; i < TURKISH_CHAR_MAP_SIZE; i++)
            {
                temizlemeDizisi[i] = ALFABE_DISI_KARAKTER;
                turkceMap[i] = false;
                asciifierDizisi[i] = (char)i;
            }
        }

        /// <summary>
        /// char olarak girilen harfin TurkceHarf karsiligini dondurur.
        /// Bu sekilde harfin Turkce'ye has ozelliklerine erisilebilir. sesli, sert vs.
        /// </summary>
        /// <param name="Harf"></param>
        /// <returns>char harfin turkeceHarf karsiligi. Eger yoksa TANIMSIZ_HARF doner</returns>
        public TurkceHarf Harf(char harf)
        {
            if (harf > TURKISH_CHAR_MAP_SIZE) return TANIMSIZ_HARF;
            return turkceHarfDizisi[harf];
        }

        /// <summary>
        /// girilen stringi kucuk harfe donusturup icindeki uyumsuz karakterleri siler
        /// "Wah'met-@" -> "ahmet"
        /// </summary>
        /// <param name="Harf"></param>
        /// <returns>girisin ayiklanmis hali</returns>
        public String Ayikla(String giris)
        {
            StringBuilder buf = new StringBuilder(giris.Length);
            for (int i = 0; i < giris.Length; i++)
            {
                if (giris[i] >= TURKISH_CHAR_MAP_SIZE)
                    continue;
                char temiz = temizlemeDizisi[giris[i]];
                if (temiz != ALFABE_DISI_KARAKTER)
                    buf.Append(temiz);
            }
            return buf.ToString();
        }

        public bool CozumlemeyeUygunMu(String giris)
        {
            for (int i = 0; i < giris.Length; i++)
            {
                if (!turkceMap[giris[i]] || giris[i] > TURKISH_CHAR_MAP_SIZE)
                { return false; }
            }
            return true;
        }

        public String AsciiyeDonustur(String inp)
        {
            char[] buffer = inp.ToCharArray();
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = asciifierDizisi[buffer[i]];
            }
            return new String(buffer);
        }

        public TurkceHarf BuyukHarf(TurkceHarf harf)
        {
            TurkceHarf buyuk = buyukHarflerDizi[harf.AlfabetikSira - 1];
            if (buyuk != null) return buyuk;
            return TANIMSIZ_HARF;
        }

        public TurkceHarf BuyukHarf(char c)
        {
            TurkceHarf buyuk = buyukHarflerDizi[Harf(c).AlfabetikSira - 1];
            if (buyuk != null) return buyuk;
            return TANIMSIZ_HARF;
        }

        public bool AsciiToleransliKiyasla(char harf1, char harf2)
        {
            if (harf1 > TURKISH_CHAR_MAP_SIZE || harf2 > TURKISH_CHAR_MAP_SIZE) return false;
            return (asciifierDizisi[harf1] == asciifierDizisi[harf2]);
        }

        public char[] AsciiDisiHarfler()
        {
            return asciiDisi;
        }

        /// <summary>
        /// istenilen kalin seslinin inceltilmis kopya halini dondurur. sadece ters sesli
        /// ozel durumu isleminde kullanilmaslidir.
        /// </summary>
        /// <param name="kalinSesli"></param>
        /// <returns>eger varsa karsilik dusen kalin sesli. yoksa seslinin kendisi</returns>
        public TurkceHarf KalinSesliIncelt(TurkceHarf kalinSesli)
        {
            if (ozelInceSesliler.ContainsKey(kalinSesli))
                return ozelInceSesliler[kalinSesli];
            else return kalinSesli;
        }

        //bu degerler Alfabe bilgisinin dosyadan okunmasi sirasinda kullanilir
        protected internal System.Globalization.CultureInfo locale;
        private Regex virgulReg = new Regex("[,]");
        private Regex tireReg = new Regex("[-]");

        public static String HARFLER = "harfler";
        public static String SESLI = "sesli";
        public static String INCE_SESLI = "ince-sesli";
        public static String DUZ_SESLI = "duz-sesli";
        public static String YUVARLAK_SESLI = "yuvarlak-sesli";
        public static String SERT = "sert";
        public static String ASCII_DISI = "ascii-disi";
        public static String ASCII_TURKCE = "ascii-turkce";
        public static String TURKCE_ASCII = "turkce-ascii";
        public static String YUMUSAMA_DONUSUM = "yumusama-donusum";
        public static String SERT_DONUSUM = "sert-donusum";
        public static String AYIKLAMA = "ayiklama";
        public static String AYIKLAMA_DONUSUM = "ayiklama-donusum";
        public static String OZEL_INCE_SESLI = "ozel-ince-sesli";

        #region Harf ve Dönüşümleri Oluşturma Rutinleri
        /**
         * Harf dosyasindan Harf bilgilerini okur ve TurkceHarf, ve Alfabe sinifi icin gerekli
         * gerekli Harf iliskili veri yapilarinin olusturur.
         */
        private void HarfBilgisiOlustur(IDictionary<String, String> bilgi)
        {
            HarfDizileriOlustur(bilgi);

            TemizlemeDizisiOlustur(bilgi);

            SertYumusakDonusumleriTanimla(bilgi);

            AsciiTurkceDonusumleriTanimla(bilgi);

            OzelInceSeslileriOlustur(bilgi);
        }

        private void OzelInceSeslileriOlustur(IDictionary<String, String> bilgi)
        {
            // bazi turkcelerde yabanci dillerden gelen bazi kokler normal kalin-kalin sesli
            // uretimini bozar. Eger Harf ozelliklerinde belirtilmisse burada ince ozelligine sahip kalin
            // sesli kopyalari uretilir. bu harfler normal sesli listesnde yer almaz. kiyaslama sirasinda
            // kalin hali ile ayni kabul edilir.
            if (bilgi.ContainsKey(OZEL_INCE_SESLI))
            {
                foreach (char c in HarfAyristir(Ozellik(bilgi, OZEL_INCE_SESLI)))
                {
                    TurkceHarf inceltilmisKalinSesli = (TurkceHarf)Harf(c).Clone();
                    inceltilmisKalinSesli.InceSesli = true;
                    ozelInceSesliler.Add(Harf(c), inceltilmisKalinSesli);
                }
            }
        }

        private void AsciiTurkceDonusumleriTanimla(IDictionary<String, String> bilgi)
        {
            List<HarfCifti> asciiDonusum = HarfCiftiAyristir(Ozellik(bilgi, TURKCE_ASCII));
            foreach (HarfCifti cift in asciiDonusum)
            {
                asciifierDizisi[cift.h1] = cift.h2;
                harfler[cift.h1].AsciiDonusum = harfler[cift.h2];
            }

            // eger ascii-turkce donusum ikilileri Harf dosyasinda belirtilimisse okunur.
            // yoksa turkce-ascii ikililerinin tersi kullanilarak harflerin turkceDonusum ozellikleri belirlenir.
            if (bilgi.ContainsKey(ASCII_TURKCE))
            {
                foreach (HarfCifti cift in HarfCiftiAyristir(Ozellik(bilgi, ASCII_TURKCE)))
                    harfler[cift.h1].TurkceDonusum = harfler[cift.h2];
            }
            else
            {
                foreach (HarfCifti cift in asciiDonusum)
                    Harf(cift.h2).TurkceDonusum = Harf(cift.h1);
            }
        }

        private void SertYumusakDonusumleriTanimla(IDictionary<String, String> bilgi)
        {
            List<HarfCifti> yumusamaDonusum = HarfCiftiAyristir(Ozellik(bilgi, YUMUSAMA_DONUSUM));
            foreach (HarfCifti cift in yumusamaDonusum)
            {
                Harf(cift.h1).Yumusama = Harf(cift.h2);
                BuyukHarf(cift.h1).Yumusama = BuyukHarf(cift.h2);
            }

            // eger sert donusum bilgisi Harf ozelliklerinde yar almazsa
            // yumusama donusumun tersi olarak uygulanir.
            if (bilgi.ContainsKey(SERT_DONUSUM))
            {
                foreach (HarfCifti cift in HarfCiftiAyristir(Ozellik(bilgi, SERT_DONUSUM)))
                {
                    Harf(cift.h1).SertDonusum = Harf(cift.h2);
                    BuyukHarf(cift.h1).SertDonusum = BuyukHarf(cift.h2);
                }
            }
            else
            {
                foreach (HarfCifti cift in yumusamaDonusum)
                {
                    Harf(cift.h2).SertDonusum = Harf(cift.h1);
                    BuyukHarf(cift.h2).SertDonusum = BuyukHarf(cift.h1);
                }
            }
        }

        private void TemizlemeDizisiOlustur(IDictionary<String, String> bilgi)
        {
            foreach (char c in HarfAyristir(Ozellik(bilgi, AYIKLAMA)))
            {
                temizlemeDizisi[c] = ALFABE_DISI_KARAKTER;
                temizlemeDizisi[BuyukHarf(c).CharDeger] = ALFABE_DISI_KARAKTER;
            }

            foreach (HarfCifti cift in HarfCiftiAyristir(Ozellik(bilgi, AYIKLAMA_DONUSUM)))
            {
                temizlemeDizisi[cift.h1] = cift.h2;
                temizlemeDizisi[BuyukHarf(cift.h1).CharDeger] = BuyukHarf(cift.h2).CharDeger;
            }
        }

        private void HarfDizileriOlustur(IDictionary<String, String> bilgi)
        {

            String tumKucukler = Ozellik(bilgi, HARFLER);
            String tumBuyukler = tumKucukler.ToUpper(locale);
            char[] kucukler = HarfAyristir(tumKucukler);
            char[] buyukler = HarfAyristir(tumBuyukler);

            //TurkceHarfleri olustur.
            for (int i = 0; i < kucukler.Length; i++)
            {
                char c = kucukler[i];
                TurkceHarf harf = new TurkceHarf(c);
                harf.AlfabetikSira = i + 1;
                harfler.Add(c, harf);
                turkceHarfDizisi[c] = harf;
                kucukHarflerDizi[i] = harf;
                temizlemeDizisi[c] = c;
                turkceMap[c] = true;
                asciifierDizisi[c] = c;
            }

            for (int i = 0; i < buyukler.Length; i++)
            {
                char c = buyukler[i];
                TurkceHarf harf = new TurkceHarf(c);
                if (Char.IsLetter(c))
                    harf.BuyukHarf = true;
                harf.AlfabetikSira = i + 1;
                // TODO Bu if'i ekledim nokta karakteri sorun çıkarıyordu
                if (!harfler.ContainsKey(c))
                    harfler.Add(c, harf);
                buyukHarflerDizi[i] = harf;
                temizlemeDizisi[c] = kucukHarflerDizi[i].CharDeger;
                turkceHarfDizisi[c] = harf;
            }

            foreach (char c in HarfAyristir(Ozellik(bilgi, SESLI)))
            {
                Harf(c).Sesli = true;
                BuyukHarf(harfler[c]).Sesli = true;
            }

            foreach (char c in HarfAyristir(Ozellik(bilgi, INCE_SESLI)))
            {
                Harf(c).InceSesli = true;
                BuyukHarf(c).InceSesli = true;
            }

            foreach (char c in HarfAyristir(Ozellik(bilgi, DUZ_SESLI)))
            {
                Harf(c).DuzSesli = true;
                BuyukHarf(c).DuzSesli = true;
            }

            foreach (char c in HarfAyristir(Ozellik(bilgi, YUVARLAK_SESLI)))
            {
                Harf(c).YuvarlakSesli = true;
                BuyukHarf(c).YuvarlakSesli = true;
            }

            foreach (char c in HarfAyristir(Ozellik(bilgi, SERT)))
            {
                Harf(c).Sert = true;
                BuyukHarf(c).Sert = true;
            }

            asciiDisi = HarfAyristir(Ozellik(bilgi, ASCII_DISI));
            foreach (char c in asciiDisi)
            {
                Harf(c).AsciiDisi = true;
            }
        }

        #endregion

        protected String Ozellik(IDictionary<String, String> harfOzellikleri, String anahtar)
        {
            if (harfOzellikleri.ContainsKey(anahtar))
                return harfOzellikleri[anahtar];
            else
            {
                logger.Warn("Harf ozelligi bulunamiyor: " + anahtar);
                return "";
            }
        }

        /**
         * "a,b,c,d" seklindeki bir Stringi bosluklardan temizleyip {'a','b','c','d'} char dizisine donusturur.
         *
         * @param tum
         * @return virgul ile ayrilmis karater dizisi.
         */
        protected char[] HarfAyristir(String tum)
        {
            tum = tum.Replace("[ \t]", "");
            String[] charStrDizi = virgulReg.Split(tum);
            char[] cDizi = new char[charStrDizi.Length];
            for (int i = 0; i < charStrDizi.Length; i++)
            {
                if (charStrDizi[i].Length != 1)
                    logger.Warn(tum + "ayristirilirken tek Harf bekleniyordu. " + charStrDizi + " uygun degil");
                cDizi[i] = charStrDizi[i].ToCharArray()[0];
            }
            return cDizi;
        }

        /**
         * "a-b,c-d,e-f" seklindeki Stringi Harf cifti listesine donusturur.
         *
         * @return TurkceHarf cifti tasiyan HarfCifti listesi
         */
        protected List<HarfCifti> HarfCiftiAyristir(String tum)
        {
            tum = tum.Replace("[ \t]", "");
            String[] charStrDizi = virgulReg.Split(tum);
            List<HarfCifti> ciftler = new List<HarfCifti>(charStrDizi.Length);
            foreach (String s in charStrDizi)
            {
                String[] cift = tireReg.Split(s);
                if (cift.Length != 2)
                    logger.Warn(tum + "ayristirilirken Harf cifti  bekleniyordu. " + s + " uygun degil.");
                if (cift[0].Length != 1 || cift[1].Length != 1)
                    logger.Warn(tum + "ayristirilirken tek Harf bekleniyordu. " + charStrDizi + " uygun degil");
                char h1 = cift[0][0];
                char h2 = cift[1][0];
                ciftler.Add(new HarfCifti(h1, h2));
            }
            return ciftler;
        }

        protected class HarfCifti
        {

            internal char h1;
            internal char h2;

            public HarfCifti(char h1, char h2)
            {
                this.h1 = h1;
                this.h2 = h2;
            }
        }
/*
        private void serialize()
        {
            XmlSerializer x = new XmlSerializer(typeof(TurkceHarf[]));
            TextWriter writer = new StreamWriter("harfler.xml");
            x.Serialize(writer, turkceHarfDizisi);
            writer.Close();
        }

        private TurkceHarf[] deserialize(string kaynakdosya)
        {
            XmlSerializer x = new XmlSerializer(typeof(TurkceHarf[]));
            TextReader reader = new StreamReader("harfler.xml");
            try
            {
                TurkceHarf[] ret = (TurkceHarf[])x.Deserialize(reader);
                return ret;
            }
            catch
            {
                throw new ApplicationException("Dosya okuma başarısız : " + kaynakdosya);
            }
            finally
            {
                reader.Close();
            }
        }
*/    
    }
}
