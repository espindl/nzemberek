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
    public class AsciiToleransliHDKiyaslayici : IHarfDizisiKiyaslayici
    {
        #region IHarfDizisiKiyaslayici Members

        public bool  Kiyasla(HarfDizisi h1, HarfDizisi h2)
        {
            return h1.AsciiToleransliKiyasla(h2);
        }

        public bool  BastanKiyasla(HarfDizisi h1, HarfDizisi h2)
        {
            return h1.AsciiToleransliBastanKiyasla(h2);
        }

        public bool  AradanKiyasla(HarfDizisi h1, HarfDizisi h2, int baslangic)
        {
            return h1.AsciiToleransliAradanKiyasla(baslangic, h2);
        }

        #endregion
    }
}
