using System;
using net.zemberek.yapi;

namespace net.zemberek.yapi.ek
{
	
	/// <summary> uretim bilesen sinifi, uretim kural kelimesindeki bilesenleri temsil eder.
	/// degistirilemez, thread safe.
	/// </summary>
	public class EkUretimBileseni
	{
    private readonly UretimKurali _kural;
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

    public String toString() {
        return "kural=" + _kural + ", harf=" + (_harf == null ? "" : "" + _harf.charDeger());
    }

    public bool equals(Object o) {
        if (this == o) return true;
        if (o == null || GetType() != o.GetType()) return false;

        EkUretimBileseni that = (EkUretimBileseni) o;

        if (_harf != null ? !_harf.Equals(that._harf) : that._harf != null) return false;
        if (_kural != that._kural) return false;

        return true;
    }

    public int hashCode() {
        int result;
        result = (_kural != null ? _kural.GetHashCode() : 0);
        result = 29 * result + (_harf != null ? _harf.GetHashCode() : 0);
        return result;
    }
    
    }

}