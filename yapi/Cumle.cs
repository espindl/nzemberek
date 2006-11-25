using System;
using System.Collections;
using System.Collections.Generic;

namespace net.zemberek.yapi
{
    public class Cumle
    {
        private List<Kelime> kelimeler = new List<Kelime>();

        public void addKelime(Kelime kelime)
        {
            kelimeler.Add(kelime);
        }

        public List<Kelime> getKelimeler()
        {
            return kelimeler;
        }

        public String toString() 
        {
            String str = "";
            foreach (Kelime kelime in kelimeler) 
            {
                str += kelime.icerik() + " ";
            }
            return str;
        }
    }
}