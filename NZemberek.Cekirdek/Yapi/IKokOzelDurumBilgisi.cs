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

namespace NZemberek.Cekirdek.Yapi
{
    public interface IKokOzelDurumBilgisi
    {
        KokOzelDurumu OzelDurum(IKokOzelDurumTipi tip);

        /// <summary>
        /// KisaAd ile belirtilen ozel durumu dondurur.
        /// </summary>
        /// <param name="KisaAd"></param>
        /// <returns>OzelDurum ya da null</returns>
        KokOzelDurumu OzelDurum(String kisaAd);

        KokOzelDurumu OzelDurum(int indeks);

        String[] OzelDurumUygula(Kok kok);

        /// <summary>
        /// Bazi ozel durumlar dogrudan kaynak kok dosyasinda yer almaz. bu ozel durumlari bu metod
        /// tespit eder ve koke ekler.
        /// </summary>
        /// <param name="kok"></param>
        void OzelDurumBelirle(Kok kok);

        /// <summary>
        /// Duz yazi kok listesinden okunan dile ozel ozel durumlarin kok'e atanmasi islemi burada yapilir.
        /// </summary>
        /// <param name="kok"></param>
        /// <param name="okunanIcerik"></param>
        /// <param name="parcalar"></param>
        void DuzyaziOzelDurumOku(Kok kok, String okunanIcerik, String[] parcalar);

        /// <summary>
        /// Ozellikle duz yazi dosyadan kok okumada kok icerigi Tip ve dile gore on islemeden gecirilebilir
        /// Ornegin turkiye turkcesinde eger kok icinde "mek" mastar eki bulunuyorsa bu silinir.
        /// </summary>
        /// <param name="kok"></param>
        /// <param name="Tip"></param>
        /// <param name="icerik"></param>
        void KokIcerigiIsle(Kok kok, KelimeTipi tip, String icerik);
    }
}