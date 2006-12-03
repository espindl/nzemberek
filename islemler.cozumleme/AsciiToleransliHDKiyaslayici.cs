using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;


namespace net.zemberek.islemler.cozumleme
{
    public class AsciiToleransliHDKiyaslayici : HarfDizisiKiyaslayici
    {

        #region HarfDizisiKiyaslayici Members

        public bool  kiyasla(HarfDizisi h1, HarfDizisi h2)
        {
            return h1.asciiToleransliKiyasla(h2);
        }

        public bool  bastanKiyasla(HarfDizisi h1, HarfDizisi h2)
        {
            return h1.asciiToleransliBastanKiyasla(h2);
        }

        public bool  aradanKiyasla(HarfDizisi h1, HarfDizisi h2, int baslangic)
        {
            return h1.asciiToleransliAradanKiyasla(baslangic, h2);
        }

        #endregion
    }
}
