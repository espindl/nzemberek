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
 * The Original Code is Zemberek Do�al Dil ��leme K�t�phanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Ak�n, Mehmet D. Ak�n.
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

namespace NZemberek.Cekirdek.MetinOkuma
{
    public class YaziBirimiTipi 
    {
        public static readonly YaziBirimiTipi KELIME = new YaziBirimiTipi("KELIME");
        public static readonly YaziBirimiTipi NOKTALAMA = new YaziBirimiTipi("NOKTALAMA");
        public static readonly YaziBirimiTipi BOSLUK = new YaziBirimiTipi("BOSLUK");
        public static readonly YaziBirimiTipi CUMLE = new YaziBirimiTipi("CUMLE");
        public static readonly YaziBirimiTipi PARAGRAF = new YaziBirimiTipi("PARAGRAF");
        public static readonly YaziBirimiTipi DIGER = new YaziBirimiTipi("DIGER");

        private readonly String myName; // for debug only

        private YaziBirimiTipi(String name) {
            myName = name;
        }

        public override String ToString()
        {
            return myName;
        }
    }
}


