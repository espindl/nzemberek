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

namespace net.zemberek.yapi
{
    public class Cumle
    {
        private List<Kelime> kelimeler = new List<Kelime>();

        public void addKelime(Kelime kelime)
        {
            kelimeler.Add(kelime);
        }

        public List<Kelime> getKelimeler()
        {
            return kelimeler;
        }

        public override String ToString() 
        {
            String str = "";
            foreach (Kelime kelime in kelimeler) 
            {
                str += kelime.icerik() + " ";
            }
            return str;
        }
    }
}