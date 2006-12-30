/* ***** BEGIN LICENSE BLOCK *****
 * Version: MPL 1.1
 *
 * The contents of this file are subject to the Mozilla Public License Version
 * 1.1 (the "License"); you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * The Original Code is Zemberek Doðal Dil Ýþleme Kütüphanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Akýn, Mehmet D. Akýn.
 * Portions created by the Initial Developer are Copyright (C) 2006
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Mert Derman
 *   Tankut Tekeli
 * ***** END LICENSE BLOCK ***** */

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
