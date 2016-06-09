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
using System.Collections.Generic;
using System.Text;
using NZemberek.Cekirdek.Yapi;

namespace NZemberek.Cekirdek.KokSozlugu
{
    /// <summary>
    /// Kök ağacını oluşturan kök nesnelerini bir kaynaktan sırayla veya topluca okur.
    /// Once 'Ac', sonra tek tek 'Oku' veya 'hepsiniOku'.
    /// Eğer sonuna kadar okunmuşsa okuyucu kendiliğinden kapanır, yoksa 'Kapat'mak gerekir.
    /// </summary>
    public interface IKokOkuyucu
    {
        /// <summary>
        /// Sözlükteki Tüm kökleri okur ve bir ArrayList olarak döndürür.
        /// </summary>
        List<Kok> HepsiniOku();

        /// <summary>
        /// Kaynaktan bir kök okur, çağrıldıkça bir sonraki kökü alır.
        /// Dosyanın sonuna gelirse dosyayı kapatır.
        /// </summary>
        /// <returns>bir sonraki kök. Eğer okunacak kök kalmamışsa null</returns>
        Kok Oku();

        /// <summary>
        /// Kaynağı okumak için açar.
        /// </summary>
        void Ac();

        /// <summary>
        /// Okunan kaynağı kapatır.
        /// </summary>
        void Kapat();
    }
}
