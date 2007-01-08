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
    /**
     * Çözümleyicinin verilen bir kelime için aday kökleri bulması için kullanılır.
     * Giriş kelimesinin ilk harfinden başlanarak ağaçta ilerlenir.
     * İlerlenecek yer kalmayana veya kelime bitene dek ağaçta yürünür, 
     * ve rastlanan tüm kökler aday olarak toplanır.
     * 
     * Bu seçici, Balerinler kelimesi için "bal, bale ve balerin" köklerini taşıyan
     * bir liste döndürür.
     *
     * @author MDA
     */
    public class KesinKokBulucu : IKokBulucu
    {
        KokAgaci agac = null;

        public KesinKokBulucu(KokAgaci agac) 
        {
            this.agac = agac;
        }

        public List<Kok> AdayKokleriGetir( String giris) 
        {
            List<Kok> adaylar = new List<Kok>(3);
            int girisIndex = 0;
            KokDugumu node = agac.BaslangicDugumu();

            while (girisIndex < giris.Length) 
            {
                node = node.altDugumGetir(giris[girisIndex]);
                if (node == null) break;
                if (node.getKok() != null) 
                {
                    // buradaki kodu daha basit ama biraz yavas hale getirdim.
                    if (giris.StartsWith(node.Kelime)) 
                    {
                        node.tumKokleriEkle(adaylar);
                    }
                }
                girisIndex++;
            }
            return adaylar;
        }
    }
}
