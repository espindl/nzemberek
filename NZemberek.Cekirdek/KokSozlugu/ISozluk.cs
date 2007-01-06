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
    public interface ISozluk
    {
        /**
     * str seklinde yazilan tum kelime koklerini dondurur. str kokun istisna hali de olabilir.
     *
     * @param str
     * @return kok listesi.
     */
        List<Kok> KokBul(String str);


        Kok KokBul(String str, KelimeTipi tip);

        //TODO Gereksiz gibi
        ///**
        // * sozluk icindeki normal ya da kok ozel durumu seklindeki tum kok iceriklerini bir
        // * Koleksiyon nesnesi olarak dondurur.
        // *
        // * @return tum kokleri iceren Collection nesnesi
        // */
        //ICollection<Kok> tumKokler();

        /// <summary>
        /// Verilen kökü sözlüğe ekler.
        /// </summary>
        /// <param name="kok">Sözlüğe eklenecek olan kök nesnesi.</param>
        void KokEkle(Kok kok);

        /**
         * Bu metod kökbulucu fabrikası elde etmek için kullanılır. Gerçekleyen sözlük sınıfları bu
         * metodda kendi Kök bulucu fabrikası gerçeklemelerinin bir instancesini geri döndürmelidirler.
         *
         * @return Sözlük
         * @see AgacSozluk
         */
        KokBulucuUretici KokBulucuFabrikasiGetir();

        //TODO TANKUT kılım dedi
        //KokAgaci getAgac();
    }
}
