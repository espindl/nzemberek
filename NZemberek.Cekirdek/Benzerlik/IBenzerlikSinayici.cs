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
 * The Original Code is NZemberek Doğal Dil İşleme Kütüphanesi.
 *
 * The Initial Developer of the Original Code is
 * Mert Derman, Tankut Tekeli.
 * Portions created by the Initial Developer are Copyright (C) 2006
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 * 
 * ***** END LICENSE BLOCK ***** */

using System;
using System.Collections.Generic;
using System.Text;

namespace NZemberek.Cekirdek.Benzerlik
{
    public interface IBenzerlikSinayici
    {
        /// <summary>
        /// Verilen iki sozcugun benzerliklerini 0-1 skalasinda doner
        /// </summary>
        /// <param name="sozcuk1">benzerligi sinanacak sozcuklerden biri</param>
        /// <param name="sozcuk2">benzerligi sinanacak sozcuklerden digeri</param>
        /// <returns>0-1 skalasında deger</returns>
        double SozcuklerNeKadarBenzer(String sozcuk1, String sozcuk2);
        
        /// <summary>
        ///  Verilen iki sozcugun yine verilen benzerlik limiti dahilinde olup olmadigini doner
        /// </summary>
        /// <param name="sozcuk1">benzerligi sinanacak sozcuklerden biri</param>
        /// <param name="sozcuk2">benzerligi sinanacak sozcuklerden digeri</param>
        /// <param name="benzerlikLimiti">benzer diyebilmek icin uzerinde olunmasi gereken limit deger</param>
        /// <returns>limit degerden yuksekse dogru degilse yanlis(SozcuklerBenzer ya da SozcuklerBenzerd degil)</returns>
        bool SocuklerBenzer(String sozcuk1, String sozcuk2, double benzerlikLimiti);
    }
}
