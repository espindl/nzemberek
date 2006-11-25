using System;
using System.Collections.Generic;
using System.Text;

namespace net.zemberek.yapi.kok
{
    /**
     * Harf dizisine olusum sirasinda parametre olarak gelen kelimeyi ular..
     */
    public class Ulama : HarfDizisiIslemi
    {
        private readonly HarfDizisi ulanacak;

        public Ulama(HarfDizisi ulanacak) 
        {
            this.ulanacak = ulanacak;
        }
        
        #region HarfDizisiIslemi Members

        public void uygula(HarfDizisi dizi)
        {
            dizi.ekle(ulanacak);
        }

        #endregion
    }
}
