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
 * The Original Code is Zemberek Do�al Dil ��leme K�t�phanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Ak�n, Mehmet D. Ak�n.
 * Portions created by the Initial Developer are Copyright (C) 2006
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Mert Derman
 *   Tankut Tekeli
 * ***** END LICENSE BLOCK ***** */

using System;
using NZemberek.Cekirdek.Yapi;

namespace NZemberek.Cekirdek.Yapi
{
	/// <summary> uretim bilesen sinifi, uretim kural kelimesindeki bilesenleri temsil eder.
	/// degistirilemez, thread safe.
	/// </summary>
    public class EkUretimBileseni
    {
        private readonly IEkUretimKurali _kural = TemelEkUretimKurali.YOK;

        public IEkUretimKurali Kural
        {
            get { return _kural; }
        }

        private readonly TurkceHarf _harf;

        public TurkceHarf Harf
        {
            get { return _harf; }
        } 

        public EkUretimBileseni(IEkUretimKurali kural, TurkceHarf harf)
        {
            this._kural = kural;
            this._harf = harf;
        }

        public override String ToString()
        {
            return "kural=" + _kural + ", Harf=" + (_harf == null ? "" : "" + _harf.CharDeger);
        }

        public override bool Equals(Object o)
        {
            if (this == o) return true;
            if (o == null || GetType() != o.GetType()) return false;

            EkUretimBileseni that = (EkUretimBileseni)o;

            if (_harf != null ? !_harf.Equals(that._harf) : that._harf != null) return false;
            if (_kural != that._kural) return false;

            return true;
        }

        public override int GetHashCode()
        {
            int result;
            result = (_kural != TemelEkUretimKurali.YOK ? _kural.GetHashCode() : 0);
            result = 29 * result + (_harf != null ? _harf.GetHashCode() : 0);
            return result;
        }
    }
}