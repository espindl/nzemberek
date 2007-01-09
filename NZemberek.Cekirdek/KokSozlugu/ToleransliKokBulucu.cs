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
using NZemberek.Cekirdek.Benzerlik;

namespace NZemberek.Cekirdek.KokSozlugu
{
    /**
 * Hata toleranslı kök bulucu hatalı girişler için öneri üretmekte kullanılır.
 * <p/>
 * Ağacın "e" harfi ile başlayan kolu aşağıdaki gibi olsun:
 * <p/>
 * <pre>
 * e
 * |---l(el)
 * |  |---a(ela)
 * |  |  |--s-(elastik)
 * |  |
 * |  |---b
 * |  |  |--i-(elbise)
 * |  |
 * |  |---m
 * |  |  |--a(elma)
 * |  |  |  |--c-(elmacık)
 * |  |  |  |--s-(elmas)
 * |  | ...
 * | ...
 * ...
 * </pre>
 * <p/>
 * "elm" girişi için ağaç üzerinde ilerlerken hata mesafesi 1 olduğu müddetçe 
 * ilerlenir. bu sırada "el, ela, elma" kökleri toplanır.
 *  @author MDA
 */
    public class ToleransliKokBulucu : IKokBulucu
    {
        private KokAgaci agac = null;
        private int tolerans = 0;
        private int distanceCalculationCount = 0;

        public int DistanceCalculationCount
        {
            get { return distanceCalculationCount; }
        }

        public ToleransliKokBulucu(KokAgaci agac, int tolerans)
        {
            this.agac = agac;
            this.tolerans = tolerans;
        }

        public List<Kok> AdayKokleriGetir(String giris)
        {
            return BenzerKokleriBul(giris);
        }

        private String giris = null;
        private List<Kok> adaylar = null;

        private List<Kok> BenzerKokleriBul(String giris)
        {
            this.giris = giris;
            adaylar = new List<Kok>();
            Yuru(agac.BaslangicDugumu(), "");
            return adaylar;
        }

        private void Yuru(KokDugumu dugum, String olusan)
        {
            String tester = olusan;
            if (dugum.Harf != '\0')
                tester += dugum.Harf;
            if (dugum.Kok != null)
            {
                distanceCalculationCount++;
                if (BenzerlikAraci.ParcasiDuzeltmeMesafesiIcinde(dugum.Kelime, giris, tolerans))
                {
                    // Aday kök bulundu
                    adaylar.Add(dugum.Kok);
                }
                else
                {
                    // Mesafe sınırı aşıldı.
                    return;
                }
            }
            else
            {
                if (!BenzerlikAraci.ParcasiDuzeltmeMesafesiIcinde(tester.Trim(), giris, tolerans))
                {
                    // Ara stringde mesafe sınırı aşıldı
                    return;
                }
            }

            foreach (KokDugumu altDugum in dugum.AltDugumDizisiGetir())
            {
                if (altDugum != null)
                {
                    this.Yuru(altDugum, tester);
                }
            }
        }
    }
}
