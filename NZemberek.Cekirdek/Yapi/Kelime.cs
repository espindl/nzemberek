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
using System.Text;
using System.Collections.Generic;
using NZemberek.Cekirdek.Yapi;

namespace NZemberek.Cekirdek.Yapi
{
    public class Kelime : ICloneable
    {
        public static readonly List<Kelime> EMPTY_LIST_KELIME = new List<Kelime>();
        public static readonly Kelime[] BOS_KELIME_DIZISI = new Kelime[] { };

        private static HarfDizisi BOS_ICERIK = new HarfDizisi(0);
        private HarfDizisi _icerik = BOS_ICERIK;

        public HarfDizisi Icerik
        {
            get { return _icerik; }
            set { _icerik = value; }
        }
        private Kok _kok;

        public Kok Kok
        {
            get { return _kok; }
            set { _kok = value; }
        }
        private List<Ek> _ekler = new List<Ek>(3);

        public List<Ek> Ekler
        {
            get { return _ekler; }
        }

        private KelimeTipi _tip;

        public Kelime(){}

        public Kelime(Kok kok, Alfabe alfabe)
        {
            this._kok = kok;
            _icerik = new HarfDizisi(kok.Icerik, alfabe);
            _tip = kok.Tip;
        }

        public Kelime(Kok kok, HarfDizisi dizi)
        {
            this._kok = kok;
            this._icerik = dizi;
            _tip = kok.Tip;
        }

        public Ek[] EkDizisi()
        {
            return (Ek[])_ekler.ToArray();
        }

        public Object Clone()
        {
            Kelime kopya = new Kelime();
            //kok'u kopyalamaya gerek yok. referans esitlemesi yeter
            kopya._kok = _kok;
            kopya._icerik = new HarfDizisi(_icerik);
            kopya._ekler = new List<Ek>(_ekler);
            kopya._tip = this._tip;
            return kopya;
        }

        /**
         * Gosterim amacli bir metod. ek zincirinin anlasilir sekilde yazilmasini saglar.
         *
         * @return ek zinciri dizisi yazimi.
         */
        public String MetinBicimindeEkZinciri()
        {
            StringBuilder buf = new StringBuilder();
            foreach (Ek ek in _ekler)
            {
                buf.Append(ek.Ad).Append(", ");
            }
            if (buf.Length > 2)
                buf.Remove(buf.Length - 2,2);
            return buf.ToString();
        }

        public int EkSayisi()
        {
            return _ekler.Count;
        }

        public TurkceHarf SonHarf()
        {
            return _icerik.SonHarf();
        }

        public int Boy()
        {
            return _icerik.Boy;
        }

        public Ek SonEk()
        {
            return _ekler[_ekler.Count - 1];
        }

        public String IcerikMetni()
        {
            return _icerik.ToString();
        }

        public void EkEkle(Ek ek)
        {
            _ekler.Add(ek);
        }

        public override String ToString() {
            StringBuilder ekStr = new StringBuilder();
            foreach (Ek ek in _ekler) {
                ekStr.Append(ek.Ad).Append(" + ");
            }
            if (ekStr.Length > 3)
                ekStr.Remove(ekStr.Length - 3, 3);
            return "{Icerik: " + _icerik + " Kok: " + _kok.Icerik + " tip:" + _kok.Tip + "} " +
                    " Ekler:" + ekStr;
        }

        public void IcerikEkle(HarfDizisi eklenecek)
        {
            _icerik.Ekle(eklenecek);
        }

        /**
         * Kelime icerisinde sadece kok ya da kok tipini belirten baslangic eki var ise bu metod
         * true dondurur. Eger baska bir ek eklenmis ise true doner.
         * @return
         */
        public bool GercekEkYok()
        {
            return _ekler.Count < 2;
        }
    }
}