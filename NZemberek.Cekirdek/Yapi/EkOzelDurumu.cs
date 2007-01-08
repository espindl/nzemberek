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
using System.Collections;
using System.Collections.Generic;
using NZemberek.Cekirdek.Mekanizma.Cozumleme;
using NZemberek.Cekirdek.Kolleksiyonlar;

namespace NZemberek.Cekirdek.Yapi
{
	/// <summary> 
    /// Ek ozel durumu ek'e benzer bir yapiya sahiptir. Farkli olarak bazi ozel durumlarda yer alan
	/// onek listesi de bu sinifin bir parametresidir.
	/// </summary>
    public abstract class EkOzelDurumu
    {
        private String _ad;

        public String Ad
        {
            get { return _ad; }
            set { _ad = value; }
        }

        private HashSet<Ek> onEkler = Ek.EMPTY_SET;

        public HashSet<Ek> OnEkler
        {
            get { return onEkler; }
            set { onEkler = value; }
        }
        
        private IEkUretici ekUretici;

        public IEkUretici EkUretici
        {
            get { return ekUretici; }
            set { ekUretici = value; }
        }

        private List<EkUretimBileseni> uretimBilesenleri;

        public List<EkUretimBileseni> UretimBilesenleri
        {
            get { return uretimBilesenleri; }
            set { uretimBilesenleri = value; }
        }

        public abstract HarfDizisi CozumlemeIcinUret(Kelime kelime, HarfDizisi giris, IHarfDizisiKiyaslayici kiyaslayici);

        public virtual HarfDizisi OlusumIcinUret(Kelime kelime, Ek sonrakiEk)
        {
            return ekUretici.OlusumIcinEkUret(kelime.Icerik, sonrakiEk, uretimBilesenleri);
        }
    }
}