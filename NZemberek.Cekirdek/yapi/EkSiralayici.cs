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
using System.Collections;
using System.Collections.Generic;
using net.zemberek.yapi.ek;

namespace net.zemberek.yapi
{
    /// <summary> 
    /// Bu sinif rasgele bir sirada gelen ek ve kok bilgisini kullanarak olasi dogru ek dizilimlerini uretir
    /// </summary>
    public class EkSiralayici
    {
        public static readonly IList EMPTY_LISTX = (System.Collections.IList)System.Collections.ArrayList.ReadOnly(new System.Collections.ArrayList());
        public static readonly List<List<Ek>> EMPTY_LIST = new List<List<Ek>>();

        private List<List<Ek>> tumOlusumlar;
        private List<Ek> ekler;
        private Ek baslangicEki;
        private int tum;

        public EkSiralayici(List<Ek> ekler, Ek baslangicEki)
        {
            this.ekler = ekler;
            this.baslangicEki = baslangicEki;
            this.tum = ekler.Count;
        }

        /**
         * olasi dogru ek dizilimlerini bulur.
         *
         * @return List<List<Ek>> Her bir dogru ek dizilimi bir listede tutulur. Donus ise tum dogru
         *         ek dizilim listelerini tutan baska bir liste. Eger liste bos ise dogru dizilim bulunamamis demektir.
         */
        public List<List<Ek>> olasiEkDizilimleriniBul()
        {
            if (ekler == null)
                return EMPTY_LIST;
            tumOlusumlar = new List<List<Ek>>();
            List<Ek> kopya = new List<Ek>(ekler);
            yuru(new List<Ek>(), baslangicEki, kopya);
            return tumOlusumlar;
        }


        /**
         * Bu metod rasgele girilen bir ek dizisinin olasi dogru siralamalarini bulur.
         * Metod kendi kendini cagiran bir yapidadir (recursive). Sonucta dogru olabilecek dizilimler
         * nesne icindeki "tumOlusumlar" dizisine atilir.
         *
         * @param olusan:       ic calisma sirasinda dogru olusan dizilimi tasiyan ArrayList.
         * @param incelenenEk:  Kok'un tipine gore gereken baslangic eki. FIIL ise FIIL_YALIN vs.
         * @param rasgeleEkler: rasgele sirali ekleri tasiyan liste.
         */
        private void yuru(List<Ek> olusan, Ek incelenenEk, List<Ek> rasgeleEkler)
        {
            for (int i = 0; i < rasgeleEkler.Count; i++)
            {
                Ek ek = rasgeleEkler[i];
                if (incelenenEk.ardindanGelebilirMi(ek))
                {
                    List<Ek> newList = new List<Ek>(rasgeleEkler);
                    newList.Remove(ek);
                    olusan.Add(ek);
                    if (newList.Count != 0)
                        yuru(olusan, ek, newList);
                }
            }
            if (olusan.Count == tum)
            {
                if (!this.tumOlusumlar.Contains(olusan))
                    this.tumOlusumlar.Add(olusan);
                olusan = new List<Ek>();
            }
            else
            {
                rasgeleEkler.Add(incelenenEk);
                if (olusan.Count > 0)
                    olusan.Remove(olusan[olusan.Count - 1]);
            }
        }
    }
}