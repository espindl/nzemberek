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
using NZemberek.Cekirdek.Yapi;



namespace NZemberek.Cekirdek.Mekanizma.Cozumleme
{
    public class BasitKelimeYigini
    {
        private LinkedList<YiginKelime> yigin = new LinkedList<YiginKelime>();

        public YiginKelime al() 
        {
            YiginKelime ret = yigin.First.Value;
            yigin.RemoveFirst();
            return ret;
        }

        public bool bosMu() {
            return (yigin.Count==0);
        }

        public void temizle() {
            yigin.Clear();
        }

        public void koy(Kelime kelime, int ardisilEkSirasi) {
            yigin.AddFirst(new YiginKelime(kelime, ardisilEkSirasi));
        }

        public sealed class YiginKelime 
        {

            private readonly Kelime kelime;
            private readonly int ekSirasi;

            public YiginKelime(Kelime kel, int index) {
                this.kelime = kel;
                this.ekSirasi = index;
            }

            public Kelime getKelime() {
                return kelime;
            }

            public int getEkSirasi() {
                return ekSirasi;
            }

            public override String ToString() {
                return " olusan: " + kelime.icerikStr()
                        + " sonEk: " + kelime.sonEk().ToString()
                        + " ekSira: " + ekSirasi;
            }
        }
    }
}


public class BasitKelimeYigini 
{

}
