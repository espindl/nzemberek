using System;
using System.Collections.Generic;
using System.Text;

namespace net.zemberek.yapi.kok
{
    /**
     * kelimeye sesli harf eklendiginde son sessizin tekrarlanmasina neden olur.
     * hak-> hakka, red->reddi gibi.
     */
    public class Ciftleme : HarfDizisiIslemi
    {
        #region HarfDizisiIslemi Members

        public void uygula(HarfDizisi dizi)
        {
            if (dizi.Length > 0)
                dizi.ekle(dizi.harf(dizi.Length - 1));
        }

        #endregion
    }
}
