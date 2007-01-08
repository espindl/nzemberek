using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NZemberek.Cekirdek.Yapi;

namespace NZemberek.TrTurkcesi.Yapi
{
    internal class TRHeceleyici : IHeceleyici
    {
        private Alfabe _alfabe;
        public TRHeceleyici(Alfabe alfabe)
        {
            _alfabe = alfabe;
        }

        #region IHeceleyici Members

        public int[] HeceIndeksleriniBul(string giris)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        
        public bool Hecelenebilir(string giris)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Turkcede kelime icinde iki unlu arasindaki unsuz kendinden sonraki unluyle hece kurar.
        /// Ayrica iki unlu yanyana ise bunlarda hece yapicidir. 
        /// Tanimlarindan yola cikar.Once heceyapici harflerin yerlerini bulur ve bunlara gore 
        /// hecelere ayırır.
        /// </summary>
        /// <param name="giris"></param>
        /// <returns></returns>
        public string[] Hecele(string giris)
        {
            HarfDizisi kelime = new HarfDizisi(giris, _alfabe);
            ArrayList heceler = new ArrayList();
            List<int> heceyapicilar = heceYapiciIndeksleriniBul(kelime);

            int sonNokta = kelime.Boy;
            for (int i = kelime.Boy - 1; i > -1; i--)
            {
                if (heceyapicilar.Contains(i))
                {
                    heceyapicilar.Remove(i);
                    if (heceyapicilar.Count == 0 && kelime.SubSequence(0, kelime.Boy - (kelime.Boy - i)).SesliSayisi() == 0)
                    {
                        heceler.Add(kelime.SubSequence(0, kelime.Boy - (kelime.Boy - sonNokta)).ToString());
                        sonNokta = 0;
                    }
                    else
                    {
                        heceler.Add(kelime.SubSequence(i, kelime.Boy - (kelime.Boy - sonNokta)).ToString());
                        sonNokta = i;
                    }
                }
            }
            if (sonNokta != 0)
            {
                heceler.Add(kelime.SubSequence(0, kelime.Boy -  (kelime.Boy-sonNokta)).ToString());
            }
            heceler.Reverse();
            return (string[])heceler.ToArray(typeof(string));
        }

        #endregion

        private static List<int> heceYapiciIndeksleriniBul(HarfDizisi kelime)
        {
            List<int> heceyapicilar = new List<int>();
            for (int i = 0; i < kelime.Boy - 1; i++)
            {
                if ((!kelime.Harf(i).Sesli && kelime.Harf(i + 1).Sesli) || (kelime.Harf(i).Sesli && i > 0 && kelime.Harf(i - 1).Sesli))
                {
                    heceyapicilar.Add(i);
                }
            }
            return heceyapicilar;
        }
    }
}
