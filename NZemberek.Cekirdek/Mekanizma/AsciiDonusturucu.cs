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
using NZemberek.Cekirdek.Yapi;


namespace NZemberek.Cekirdek.Mekanizma
{
    public class AsciiDonusturucu
    {
        Alfabe alfabe;

        public AsciiDonusturucu(Alfabe alfabe)
        {
            this.alfabe = alfabe;
        }

        public String AsciiyeDonustur(String giris)
        {
            char[] chars = giris.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                TurkceHarf harf = alfabe.Harf(chars[i]);
                if (harf != null && harf != Alfabe.TANIMSIZ_HARF)
                    if (harf.AsciiDonusum != null)
                        chars[i] = harf.AsciiDonusum.CharDeger;
            }
            return new String(chars);
        }
    }
}

