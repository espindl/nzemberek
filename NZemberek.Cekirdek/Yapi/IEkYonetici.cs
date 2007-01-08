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
 * The Original Code is Zemberek Doğal Dil İşleme Kütüphanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Akın, Mehmet D. Akın.
 * Portions created by the Initial Developer are Copyright (C) 2006
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Mert Derman
 *   Tankut Tekeli
 * ***** END LICENSE BLOCK ***** */

using System;

using NZemberek.Cekirdek.Yapi;

namespace NZemberek.Cekirdek.Yapi
{
    public interface IEkYonetici
    {
        /// <summary>
        /// istenilen isimli Ek'i dondurur
        /// </summary>
        /// <param name="ad"></param>
        /// <returns>ek</returns>
        Ek EkVer(String ad);

        /// <summary>
        /// Kok nesnesinin tipine gore gelebilecek ilk ek'i dondurur.
        /// Baslangic ekleri bilgisi dil tarafindan belirlenir.
        /// </summary>
        /// <param name="kok"></param>
        /// <returns>ilk Ek, eger kok tipi baslangic ekleri baslangicEkleri
        /// haritasinda belirtilmemisse BOS_EK doner.</returns>
        Ek IlkEkBelirle(Kok kok);
    }
}
