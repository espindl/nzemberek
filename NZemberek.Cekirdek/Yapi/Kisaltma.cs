using System;
using System.Collections.Generic;
using System.Text;

namespace NZemberek.Cekirdek.Yapi
{
    public class Kisaltma : Kok
    {
        // bazi kisaltmalara ek eklenebilmesi icin kisaltmanin asil halinin son seslisine ihtiyac duyulur.
        private char kisaltmaSonSeslisi;

        public Kisaltma(String icerik):base(icerik)
        {
            this.Tip = KelimeTipi.KISALTMA;
        }

        public char getKisaltmaSonSeslisi()
        {
            return kisaltmaSonSeslisi;
        }

        public void setKisaltmaSonSeslisi(char kisaltmaSonSeslisi)
        {
            this.kisaltmaSonSeslisi = kisaltmaSonSeslisi;
        }
    }
}
