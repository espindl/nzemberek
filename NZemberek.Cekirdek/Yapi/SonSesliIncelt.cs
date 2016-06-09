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

namespace NZemberek.Cekirdek.Yapi
{
    /**
     * Bu islem sadece saat-ler turu ozel durumlarda kullanilir.
     */
    public class SonSesliIncelt : IHarfDizisiIslemi
    {
        Alfabe _alfabe;

        public SonSesliIncelt(Alfabe alfabe)
        {
            this._alfabe = alfabe;
        }

        /**
        * en son kalin sesli harfi bulup onu ince formu ile degistirir.
        * ornegin saat -> saAt haline donusur. ince a harfi icin TurkceAlfabe sinifini inceleyin
        *
        * @param dizi
        */
        public void Uygula(HarfDizisi dizi)
        {
            for (int i = dizi.Boy - 1; i >= 0; i--)
            {
                TurkceHarf h = dizi.Harf(i);
                if (h.Sesli && !h.InceSesli)
                {
                    dizi.HarfDegistir(i, _alfabe.KalinSesliIncelt(dizi.Harf(i)));
                }
            }
        }

    }
}