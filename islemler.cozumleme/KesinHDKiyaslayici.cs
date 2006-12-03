using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;


namespace net.zemberek.islemler.cozumleme
{
    public class KesinHDKiyaslayici : HarfDizisiKiyaslayici
    {
        #region HarfDizisiKiyaslayici Members

        public bool  kiyasla(HarfDizisi h1, HarfDizisi h2)
        {
            return h1.Equals(h2);
        }

        public bool  bastanKiyasla(HarfDizisi h1, HarfDizisi h2)
        {
            return h1.bastanKiyasla(h2);
        }

        public bool  aradanKiyasla(HarfDizisi h1, HarfDizisi h2, int baslangic)
        {
            return h1.aradanKiyasla(baslangic, h2);
        }

        #endregion
    }
}


