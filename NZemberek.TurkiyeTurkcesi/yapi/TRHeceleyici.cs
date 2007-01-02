using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;

namespace NZemberek.TurkiyeTurkcesi.yapi
{
    internal class TRHeceleyici : IHeceleyici
    {
        private Alfabe _alfabe;
        public TRHeceleyici(Alfabe alfabe)
        {
            _alfabe = alfabe;
        }

        #region IHeceleyici Members

        public int[] heceIndeksleriniBul(string giris)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        
        public bool hecelenebilirmi(string giris)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string[] hecele(string giris)
        {
            HarfDizisi kelime = new HarfDizisi(giris, _alfabe);
            ArrayList heceler = new ArrayList();
            List<int> heceyapicilar = heceYapiciIndeksleriniBul(kelime);

            int sonNokta = kelime.Length;
            for (int i = kelime.Length - 1; i > -1; i--)
            {
                if (heceyapicilar.Contains(i))
                {
                    heceyapicilar.Remove(i);
                    if (heceyapicilar.Count == 0 && kelime.subSequence(0, kelime.Length - (kelime.Length - i)).sesliSayisi() == 0)
                    {
                        heceler.Add(kelime.subSequence(0, kelime.Length - (kelime.Length - sonNokta)).ToString());
                        sonNokta = 0;
                    }
                    else
                    {
                        heceler.Add(kelime.subSequence(i, kelime.Length - (kelime.Length - sonNokta)).ToString());
                        sonNokta = i;
                    }
                }
            }
            if (sonNokta != 0)
            {
                heceler.Add(kelime.subSequence(0, kelime.Length -  (kelime.Length-sonNokta)).ToString());
            }
            heceler.Reverse();
            return (string[])heceler.ToArray(typeof(string));
        }

        #endregion

        private static List<int> heceYapiciIndeksleriniBul(HarfDizisi kelime)
        {
            List<int> heceyapicilar = new List<int>();
            for (int i = 0; i < kelime.Length - 1; i++)
            {
                if ((!kelime.harf(i).Sesli && kelime.harf(i + 1).Sesli) || (kelime.harf(i).Sesli && i > 0 && kelime.harf(i - 1).Sesli))
                {
                    heceyapicilar.Add(i);
                }
            }
            return heceyapicilar;
        }
    }
}
