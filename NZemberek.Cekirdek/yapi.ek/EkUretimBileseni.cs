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
using net.zemberek.yapi;

namespace net.zemberek.yapi.ek
{
	
	/// <summary> uretim bilesen sinifi, uretim kural kelimesindeki bilesenleri temsil eder.
	/// degistirilemez, thread safe.
	/// </summary>
	public class EkUretimBileseni
	{
    private readonly UretimKurali _kural=UretimKurali.YOK;
    private readonly TurkceHarf _harf;

    public EkUretimBileseni(UretimKurali kural, TurkceHarf harf) {
        this._kural = kural;
        this._harf = harf;
    }

    public UretimKurali kural() {
        return _kural;
    }

    public TurkceHarf harf() {
        return _harf;
    }

        public override String ToString()
        {
        return "kural=" + _kural + ", harf=" + (_harf == null ? "" : "" + _harf.charDeger());
    }

        public override bool Equals(Object o)
        {
        if (this == o) return true;
        if (o == null || GetType() != o.GetType()) return false;

        EkUretimBileseni that = (EkUretimBileseni) o;

        if (_harf != null ? !_harf.Equals(that._harf) : that._harf != null) return false;
        if (_kural != that._kural) return false;

        return true;
    }

    public override int GetHashCode() {
        int result;
        result = (_kural != UretimKurali.YOK ? _kural.GetHashCode() : 0);
        result = 29 * result + (_harf != null ? _harf.GetHashCode() : 0);
        return result;
    }
    
    }

}