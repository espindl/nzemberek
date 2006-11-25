using System;
using System.Collections.Generic;
using System.Text;


namespace net.zemberek.yapi.kok
{
    /**
     * kuralsiz kok bozulmasi ozel durumlarinda kullanilir.
     * uretim parametresi olarak gelen Map icerisinde hangi kelimenin hangi kelimeye
     * donusecegi belirtilir.
     * ornegin
     * demek->diyen icin de->ye donusumu, ben->bana icin ben->ban donusumu.
     */
    public class YeniIcerikAta : HarfDizisiIslemi
    {
        private IDictionary<String, String> kokDonusum;
        private Alfabe alfabe;

        public YeniIcerikAta(Alfabe alfabe, IDictionary<String, String> kokDonusum) 
        {
            this.kokDonusum = kokDonusum;
            this.alfabe = alfabe;
        }

        #region HarfDizisiIslemi Members

        public void uygula(HarfDizisi dizi)
        {
            String kelime = kokDonusum[dizi.ToString()];
            if (kelime != null)
            {
                dizi.sil();
                dizi.ekle(new HarfDizisi(kelime, alfabe));
            }
        }

        #endregion
    }
}
