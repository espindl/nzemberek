using System;
using System.Collections.Generic;
using System.Text;

namespace NZemberek.Cekirdek.Benzerlik
{
    /*
     * Bu gerceklemedeki kodlar yazilirken SimMetrics projesinin .Net gerçeklemesinden
     * faydalanildi.
     * 
     * SimMetrics - SimMetrics is a java library of Similarity or Distance
     * Metrics, e.g. Levenshtein Distance, that provide float based similarity
     * measures between string Data. All metrics return consistant measures
     * rather than unbounded similarity scores.
     * Copyright (C) 2005 Sam Chapman - Open Source Release v1.1
     * www: http://www.dcs.shef.ac.uk/~sam/stringmetrics.html
     */
    public class JaroWinklerBenzerlikSinayici : IBenzerlikSinayici
    {
        const double varsayilanBenzemezlikSkoru = 0.0;
        const int enAzBaslangicOrtakSayisi = 4;

        private static StringBuilder OrtakKarakterler(string sozcuk1, string sozcuk2, int uzaklikAdimi)
        {
            if ((sozcuk1 != null) && (sozcuk2 != null))
            {
                StringBuilder ortaklar = new StringBuilder();
                StringBuilder kopya = new StringBuilder(sozcuk2);
                for (int i = 0; i < sozcuk1.Length; i++)
                {
                    char ch = sozcuk1[i];
                    bool bulundu = false;
                    for (int j = Math.Max(0, i - uzaklikAdimi);
                         !bulundu && j < Math.Min(i + uzaklikAdimi, sozcuk2.Length);
                         j++)
                    {
                        if (kopya[j] == ch)
                        {
                            bulundu = true;
                            ortaklar.Append(ch);
                            kopya[j] = '#';
                        }
                    }
                }
                return ortaklar;
            }
            return null;
        }
        
        private static double JaroBenzerlikSkoru(string sozcuk1, string sozcuk2)
        {
            if ((sozcuk1 != null) && (sozcuk2 != null))
            {
                int yariUzunluk = Math.Min(sozcuk1.Length, sozcuk2.Length) / 2 + 1;
                //ortak karakterleri bul
                StringBuilder ortaklar1 = OrtakKarakterler(sozcuk1, sozcuk2, yariUzunluk);
                int ortakSayisi = ortaklar1.Length;
                //ortak karakter sayisi sifir mi
                if (ortakSayisi == 0)
                {
                    return varsayilanBenzemezlikSkoru;
                }
                StringBuilder ortaklar2 = OrtakKarakterler(sozcuk2, sozcuk1, yariUzunluk);
                if (ortakSayisi != ortaklar2.Length)
                {
                    return varsayilanBenzemezlikSkoru;
                }
                //transpoziyon sayisini bul
                int transpoziyonSayisi = 0;
                for (int i = 0; i < ortakSayisi; i++)
                {
                    if (ortaklar1[i] != ortaklar2[i])
                    {
                        transpoziyonSayisi++;
                    }
                }

                //jaro metrigini hesapla
                transpoziyonSayisi /= 2;
                double gecici;
                gecici = ortakSayisi / (3.0 * sozcuk1.Length) + ortakSayisi / (3.0 * sozcuk2.Length) +
                       (ortakSayisi - transpoziyonSayisi) / (3.0 * ortakSayisi);
                return gecici;
            }
            return varsayilanBenzemezlikSkoru;
        }

        static int BaslangicOrtakSayisi(string sozcuk1, string sozcuk2)
        {
            if ((sozcuk1 != null) && (sozcuk2 != null))
            {
                int n = Math.Min(Math.Min(enAzBaslangicOrtakSayisi, sozcuk1.Length), sozcuk2.Length);
                for (int i = 0; i < n; i++)
                {
                    if (sozcuk1[i] != sozcuk2[i])
                    {
                        return i;
                    }
                }
                return n;
            }
            return enAzBaslangicOrtakSayisi;
        }

        #region IBenzerlikSinayici Members

        public double SozcuklerNeKadarBenzer(string sozcuk1, string sozcuk2)
        {
            if ((sozcuk1 != null) && (sozcuk2 != null))
            {
                double jaroBenzerligi = JaroBenzerlikSkoru(sozcuk1, sozcuk2);
                int baslangicOrtakSayisi = BaslangicOrtakSayisi(sozcuk1, sozcuk2);
                return jaroBenzerligi + baslangicOrtakSayisi * 0.1F * (1.0 - jaroBenzerligi);
            }
            return 0.0;
        }

        public bool SocuklerBenzer(string sozcuk1, string sozcuk2, double benzerlikLimiti)
        {
            return this.SozcuklerNeKadarBenzer(sozcuk1, sozcuk2) >= benzerlikLimiti;
        }

        #endregion
    }
}
