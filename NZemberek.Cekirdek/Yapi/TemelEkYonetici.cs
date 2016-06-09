﻿/* ***** BEGIN LICENSE BLOCK *****
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
using System.Reflection;

namespace NZemberek.Cekirdek.Yapi
{
    public class TemelEkYonetici : IEkYonetici
    {
        public static Ek BOS_EK = new Ek("BOS_EK");
        protected IDictionary<String, Ek> ekler;
        protected IDictionary<KelimeTipi, Ek> baslangicEkleri = new Dictionary<KelimeTipi, Ek>();


        public TemelEkYonetici(Dictionary<KelimeTipi, String> baslangicEkMap,
	                           XmlEkOkuyucu okuyucu) {
            DateTime start = System.DateTime.Now;
            okuyucu.XmlOku(Assembly.GetCallingAssembly());
            ekler = okuyucu.Ekler;
            foreach (KelimeTipi tip in baslangicEkMap.Keys) {
                Ek ek = ekler[baslangicEkMap[tip]];
                if (ek != null)
                    baslangicEkleri.Add(tip, ek);
            }
        }

        /**
         * adi verilen Ek nesnesini bulur. Eger ek yok ise null doner.
         *
         * @param ekId - ek adi
         * @return istenen Ek nesnesi.
         */
        public Ek EkVer(String ekId) {
            Ek ek = ekler[ekId];
            return ekler[ekId];
        }

        /**
         * Kok nesnesinin tipine gore gelebilecek ilk ek'i dondurur.
         * Baslangic ekleri bilgisi dil tarafindan belirlenir.
         *
         * @param kok
         * @return ilk Ek, eger kok tipi baslangic ekleri <baslangicEkleri>
         *         haritasinda belirtilmemisse BOS_EK doner.
         */
        public Ek IlkEkBelirle(Kok kok) {
            Ek baslangicEki = baslangicEkleri[kok.Tip];
            if (baslangicEki != null)
                return baslangicEki;
            else
                return BOS_EK;
        }
    }
}
