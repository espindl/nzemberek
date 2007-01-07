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
using NZemberek.Cekirdek.KokSozlugu;


namespace NZemberek.Cekirdek.KokSozlugu
{
    public class AgacSozluk : ISozluk
    {
        private KokAgaci agac = null;
        private KokOzelDurumBilgisi ozelDurumlar;
        private int indeks = 0;

        private AgacSozluk(Alfabe alfabe, KokOzelDurumBilgisi ozelDurumlar)
        {
            agac = new KokAgaci(new KokDugumu(0), alfabe);
            this.ozelDurumlar = ozelDurumlar;
        }

        public AgacSozluk(Alfabe alfabe, KokOzelDurumBilgisi ozelDurumlar, IKokOkuyucu okuyucu):this(alfabe, ozelDurumlar)
        {
            Kok kok;
            while ((kok = okuyucu.Oku()) != null)
            {
                KokEkle(kok);
            }
        }

        public AgacSozluk(Alfabe alfabe, KokOzelDurumBilgisi ozelDurumlar, List<Kok> kokler):this(alfabe, ozelDurumlar)
        {
            foreach (Kok kok in kokler)
            {
                KokEkle(kok);
            }
        }

        /// <summary>
        /// Verilen kökü sözlüğe ekler. Eklemeden once koke ait ozel durumlar varsa bunlar denetlenir.
        /// Eger kok ozel durumlari kok yapisini bozacak sekilde ise ozel durumlarin koke uyarlanmis halleride
        /// agaca eklenir. bu sekilde bozulmus kok formlarini iceren kelimeler icin kok bulma
        /// islemi basari ile gerceklestirilebilir.
        /// </summary>
        /// <param name="kok">Sözlüğe eklenecek olan kök nesnesi.</param>
        private void KokEkle(Kok kok)
        {
            kok.Indeks = indeks++;
            agac.ekle(kok.icerik(), kok);
            String[] degismisIcerikler = ozelDurumlar.ozelDurumUygula(kok);
            if (degismisIcerikler.Length > 0)
            {
                foreach (String degismisIcerik in degismisIcerikler)
                {
                    agac.ekle(degismisIcerik, kok);
                }
            }
        }

        public IKokBulucu KesinKokBulucuGetir()
        {
            return new KesinKokBulucu(agac);
        }

        public IKokBulucu ToleransliKokBulucuGetir(int tolerans)
        {
            return new ToleransliKokBulucu(agac, tolerans);
        }

        public IKokBulucu AsciiKokBulucuGetir()
        {
            return new AsciiKokBulucu(agac);
        }
    }
}
