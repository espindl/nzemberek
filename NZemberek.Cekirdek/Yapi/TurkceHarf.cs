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
using System.Xml.Serialization;


namespace NZemberek.Cekirdek.Yapi
{
	/// <summary> Turkce Harf bilgilerini tasiyan sinif</summary>
    public sealed class TurkceHarf : System.ICloneable
    {
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

        public TurkceHarf()
        { }

        public override String ToString()
        {
            return "harf:" + _charDeger;
        }

        public bool asciiToleransliKiyasla(TurkceHarf ha)
        {

            //eger harf icerikleri ayni degilse turkce-ascii donusumleri kiyaslanir.
            if (_charDeger != ha.CharDeger)
            {
                if (_asciiDonusum != null && _asciiDonusum.CharDeger == ha.CharDeger)
                    return true;
                return _turkceDonusum != null && _turkceDonusum.CharDeger == ha.CharDeger;
            }
            return true;
        }

        public System.Object Clone()
        {
            TurkceHarf kopya = new TurkceHarf(_charDeger, _alfabetikSira);
            kopya.sert = sert;
            kopya.sesli = sesli;
            kopya.InceSesli = inceSesli;
            kopya.buyukHarf = buyukHarf;
            kopya.asciiDisi = asciiDisi;
            kopya.DuzSesli = duzSesli;
            kopya.yuvarlakSesli = yuvarlakSesli;
            kopya._yumusama = _yumusama;
            kopya._sertDonusum = _sertDonusum;
            kopya._turkceDonusum = _turkceDonusum;
            kopya._asciiDonusum = _asciiDonusum;
            return kopya;
        }

        public char CharDeger
        {
            get { return _charDeger; }
            set { _charDeger = value; }
        }

        public int AlfabetikSira
        {
            get { return _alfabetikSira; }
            set { _alfabetikSira = value; }

        }

        public bool Sesli
        {
            get { return sesli; }
            set { sesli = value; }
        }

        public bool Sert
        {
            get { return sert; }
            set { sert = value; }
        }

        public bool InceSesli
        {
            get {return inceSesli;}
            set { inceSesli = value; }
        }

        public bool YuvarlakSesli
        {
            get { return yuvarlakSesli; }
            set { yuvarlakSesli = value; }
        }

        public bool DuzSesli
        {
            get { return duzSesli; }
            set { duzSesli = value; }
        }

        public bool BuyukHarf
        {
            get { return buyukHarf; }
            set { buyukHarf = value; }
        }

        public bool AsciiDisi
        {
            get { return asciiDisi; }
            set { asciiDisi = value; }
        }


        [XmlIgnore]
        public TurkceHarf Yumusama
        {
            get { return _yumusama; }
            set { _yumusama = value; }
        }

        [XmlIgnore]
        public TurkceHarf SertDonusum
        {
            get { return _sertDonusum; }
            set { _sertDonusum = value; }
        }

        [XmlIgnore]
        public TurkceHarf TurkceDonusum
        {
            get { return _turkceDonusum; }
            set { _turkceDonusum = value; }
        }

        [XmlIgnore]
        public TurkceHarf AsciiDonusum
        {
            get { return _asciiDonusum; }
            set { _asciiDonusum = value; }
        }
    }
}