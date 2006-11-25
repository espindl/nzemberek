using System;

namespace net.zemberek.yapi
{
	/// <summary> Turkce Harf bilgilerini tasiyan sinif</summary>
    public sealed class TurkceHarf : System.ICloneable
    {
        ////public char CharDeger
        ////{
        ////    set
        ////    {
        ////        this.charDeger_Renamed_Field = value;
        ////    }
        ////}
        // Harfin bilgisayar karsiligi.
        private char _charDeger;
        // Harfin kullanilan dil alfabesindeki sirasi
        private int _alfabetikSira;
        private bool sesli = false;
        private bool sert = false;
        private bool inceSesli = false;
        private bool buyukHarf = false;
        private bool asciiDisi = false;
        private bool duzSesli = false;
        private bool yuvarlakSesli = false;

        // harf bir sekilde yumusuyorsa hangi harfe donusuyor?
        private TurkceHarf _yumusama = null;
        // eger bu harf bir sekilde sertlesiyorsa hangi harf?
        private TurkceHarf _sertDonusum = null;
        // Harf ASCII kumesinde yer aliyorsa ve turkce'ye ozel bir karaktere benzesiyorsa, benzesen harf.
        // ornegin s icin sh, i -> noktasiz i
        private TurkceHarf _turkceDonusum = null;
        // eger harf ASCII icinde yer almiyorsa ve benzesen bir ASCII karsiligi varsa bu harf.
        private TurkceHarf _asciiDonusum = null;

        public TurkceHarf(char charValue, int sira)
        {
            this._charDeger = charValue;
            this._alfabetikSira = sira;
            if (Char.IsUpper(charValue))
                this.buyukHarf = true;
        }

        public TurkceHarf(char charValue)
        {
            this._charDeger = charValue;
            if (Char.IsUpper(charValue))
                this.buyukHarf = true;
        }

        public String toString()
        {
            return "harf:" + _charDeger;
        }

        public bool asciiToleransliKiyasla(TurkceHarf ha)
        {

            //eger harf icerikleri ayni degilse turkce-ascii donusumleri kiyaslanir.
            if (_charDeger != ha.charDeger())
            {
                if (_asciiDonusum != null && _asciiDonusum.charDeger() == ha.charDeger())
                    return true;
                return _turkceDonusum != null && _turkceDonusum.charDeger() == ha.charDeger();
            }
            return true;
        }

        public System.Object Clone()
        {
            TurkceHarf kopya = new TurkceHarf(_charDeger, _alfabetikSira);
            kopya.sert = sert;
            kopya.sesli = sesli;
            kopya.inceSesli = inceSesli;
            kopya.buyukHarf = buyukHarf;
            kopya.asciiDisi = asciiDisi;
            kopya.duzSesli = duzSesli;
            kopya._yumusama = _yumusama;
            kopya._sertDonusum = _sertDonusum;
            kopya._turkceDonusum = _turkceDonusum;
            kopya._asciiDonusum = _asciiDonusum;
            return kopya;
        }

        public char charDeger()
        {
            return _charDeger;
        }

        public void setCharDeger(char charDeger)
        {
            this._charDeger = charDeger;
        }

        public int alfabetikSira()
        {
            return _alfabetikSira;
        }

        public void setAlfabetikSira(int alfabetikSira)
        {
            this._alfabetikSira = alfabetikSira;
        }

        public bool sesliMi()
        {
            return sesli;
        }

        public void setSesli(bool sesli)
        {
            this.sesli = sesli;
        }

        public bool sertMi()
        {
            return sert;
        }

        public void setSert(bool sert)
        {
            this.sert = sert;
        }

        public bool inceSesliMi()
        {
            return inceSesli;
        }

        public void setInceSesli(bool inceSesli)
        {
            this.inceSesli = inceSesli;
        }

        public bool buyukHarfMi()
        {
            return buyukHarf;
        }

        public void setBuyukHarf(bool buyukHarf)
        {
            this.buyukHarf = buyukHarf;
        }

        public bool asciiDisindaMi()
        {
            return asciiDisi;
        }

        public void setAsciiDisi(bool asciiDisi)
        {
            this.asciiDisi = asciiDisi;
        }

        public bool duzSesliMi()
        {
            return duzSesli;
        }

        public void setDuzSesli(bool duzSesli)
        {
            this.duzSesli = duzSesli;
        }

        public bool yuvarlakSesliMi()
        {
            return yuvarlakSesli;
        }

        public void setYuvarlakSesli(bool yuvarlakSesli)
        {
            this.yuvarlakSesli = yuvarlakSesli;
        }

        public TurkceHarf yumusama()
        {
            return _yumusama;
        }

        public void setYumusama(TurkceHarf yumusama)
        {
            this._yumusama = yumusama;
        }

        public TurkceHarf sertDonusum()
        {
            return _sertDonusum;
        }

        public void setSertDonusum(TurkceHarf sertDonusum)
        {
            this._sertDonusum = sertDonusum;
        }

        public TurkceHarf turkceDonusum()
        {
            return _turkceDonusum;
        }

        public void setTurkceDonusum(TurkceHarf turkceDonusum)
        {
            this._turkceDonusum = turkceDonusum;
        }

        public TurkceHarf asciiDonusum()
        {
            return _asciiDonusum;
        }

        public void setAsciiDonusum(TurkceHarf asciiDonusum)
        {
            this._asciiDonusum = asciiDonusum;
        }
    }
}