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
	
	/// <summary> Ek ozel durumu ek'e benzer bir yapiya sahiptir. Farkli olarak bazi ozel durumlarda yer alan
	/// onek listesi de bu sinifin bir parametresidir.
	/// User: ahmet
	/// Date: Aug 24, 2005
	/// </summary>
    public abstract class EkOzelDurumu
    {
        protected String _ad;
        protected HashSet<Ek> onEkler = Ek.EMPTY_SET;
        protected EkUretici ekUretici;
        protected List<EkUretimBileseni> _uretimBilesenleri;

        public abstract HarfDizisi cozumlemeIcinUret(Kelime kelime, HarfDizisi giris, HarfDizisiKiyaslayici kiyaslayici);

        public virtual HarfDizisi olusumIcinUret(Kelime kelime, Ek sonrakiEk)
        {
            return ekUretici.olusumIcinEkUret(kelime.icerik(), sonrakiEk, _uretimBilesenleri);
        }

        public String ad()
        {
            return _ad;
        }

        public void setAd(String ad)
        {
            this._ad = ad;
        }

        public HashSet<Ek> getOnEkler()
        {
            return onEkler;
        }

        public void setOnEkler(HashSet<Ek> onEkler)
        {
            this.onEkler = onEkler;
        }

        public void setEkKuralCozumleyici(EkUretici ekUretici)
        {
            this.ekUretici = ekUretici;
        }

        public void setUretimBilesenleri(List<EkUretimBileseni> uretimBilesenleri)
        {
            this._uretimBilesenleri = uretimBilesenleri;
        }

        public List<EkUretimBileseni> uretimBilesenleri()
        {
            return _uretimBilesenleri;
        }
    }
}