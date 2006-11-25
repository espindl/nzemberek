using System;
using System.Collections.Generic;
using System.Text;

namespace net.zemberek.yapi.kok
{
    /**
     * son harfi yumusatir. normal sert harf yumusama kurali gecerlidir.
     */
    public class Yumusama : HarfDizisiIslemi
    {
        #region HarfDizisiIslemi Members

        public void uygula(HarfDizisi dizi)
        {
            dizi.sonHarfYumusat();
        }

        #endregion
    }
}
