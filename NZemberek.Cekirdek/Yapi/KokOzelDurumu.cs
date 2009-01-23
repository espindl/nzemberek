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
using NZemberek.Cekirdek.Yapi;
using NZemberek.Cekirdek.Kolleksiyonlar;

namespace NZemberek.Cekirdek.Yapi
{
	/// <summary> 
    /// Koke ilskin ozel durumu ifade eder. Kok ozel durumlarinin farkli ozlelikleri
	/// bu sinifta belirtilir. Dogrudan uretilmez, once Uretici ic sinifi olusturulmasi gerekir.
	/// </summary>
    public class KokOzelDurumu
    {
        private int indeks;
        private string ad;

        /// <summary>
        /// Kok ozel durumu bir kelime uzerinde ne tur islem yapacak bu nesne ile belirlenir.
        /// </summary>
        private IHarfDizisiIslemi _islem;

        /// <summary>
        /// Bazi ozel durumlar sadece bazi eklerin koke eklenmesi ile olusur. Bu listede bu ekler yer alir.
        /// </summary>
        private HashSet<Ek> _gelebilecekEkler = new HashSet<Ek>();

        /// <summary>
        /// Eger ozel durum kokun yapisini bozuyorsa true.
        /// </summary>
        private bool _yapiBozucu = false;

        /// <summary>
        /// Bazi ozel durumlarin olusmasi mecburi degildir. Ornegin Turkiye Turkcesinde
        /// "icerlerde" ve "icerilerde" kelimeleri dogru kabul edilir. Eger ozel durum
        /// bu sekilde ise Secimlik=true olur.
        /// </summary>
        private bool _secimlik = false;

        /// <summary>
        /// Ek kisitlayici ozel durumlar genellikle kokun yapisini bozmaktansa kendisinden sonra
        /// bazei eklerin gelmesini engeller.
        /// </summary>
        private bool _ekKisitlayici = false;

        /// <summary>
        /// Cogu kok bozucu ozel durum sadece kendisinden sonra sesli ile baslayan
        /// bir ekin glmesi ile olusur. but bayrak bu tyur ozel durumlar icin true olur.
        /// </summary>
        private bool _sesliEkIleOlusur = false;

        /// <summary>
        /// Cogu kok bozucu ozel durum sadece kendisinden sonra sesli ile baslayan
        /// bir ekin glmesi ile olusur. but bayrak bu tyur ozel durumlar icin true olur.
        /// </summary>
        private bool _herZamanOlusur = false;

        public string Ad
        {
            get { return ad; }
            set { ad = value; }
        }

        public int Indeks
        {
            get { return indeks; }
            set { indeks = value; }
        }

        /// <summary>
        /// KokOzelDurumu Uretici nesnesi uzerinden uretilir. Dogrudan erisim yoktur.
        /// </summary>
        /// <param name="uretici"></param>
        private KokOzelDurumu(Uretici uretici)
        {
            this._gelebilecekEkler = uretici._gelebilecekEkler;
            this._sesliEkIleOlusur = uretici._sesliEkIleOlusur;
            this._yapiBozucu = uretici._yapiBozucu;
            this._secimlik = uretici._secimlik;
            this._ekKisitlayici = uretici._ekKisitlayici;
            this.ad = uretici._ad;
            this.indeks = uretici._indeks;
            this._islem = uretici._islem;
            this._herZamanOlusur = uretici._herZamanOlusur;
        }

        public bool YapiBozucu()
        {
            return _yapiBozucu;
        }

        public bool Secimlik()
        {
            return _secimlik;
        }

        public bool SeslikEkleolusur()
        {
            return _sesliEkIleOlusur;
        }

        public HashSet<Ek> GelebilecekEkleriVer()
        {
            return _gelebilecekEkler;
        }

        public bool EkKisitlayici()
        {
            return _ekKisitlayici;
        }

        /// <summary>
        /// Giris ile gelen [dizi] Harf dizisine ozel durumu uygular. Basit ziyaretci deseni (visitor pattern).
        /// </summary>
        /// <param name="dizi"></param>
        public void Uygula(HarfDizisi dizi)
        {
            _islem.Uygula(dizi);
        }
        
        /// <summary>
        /// Ozel durum giris parametresi olan ek'in bu ozel durumun olusmasina izin verip vermeyeegi belirlenir.
        /// </summary>
        /// <param name="ek"></param>
        /// <returns></returns>
        public bool Olusabilir(Ek ek)
        {
            if (_herZamanOlusur)
                return true;
            if (_sesliEkIleOlusur && ek.SesliIleBaslayabilir)
                return true;
            return _gelebilecekEkler.Contains(ek);
        }

        /// <summary>
        /// Esitlik kiyaslamasi sadece Tip indexi ve Tip adina gore yapilir.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(Object o) {
        if (this == o) return true;
        if (o == null || GetType() != o.GetType()) return false;

        KokOzelDurumu that = (KokOzelDurumu) o;
        if ((indeks == that.Indeks) && ad.Equals(that.Ad))
            return true;
        return false;

    }

        /// <summary>
        /// Sadece Tip adi ve indeksine gore belirlenir.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ad.GetHashCode() * indeks;
        }

        /// <summary>
        /// Bu sinif KokOzelDurumu uretimi icin kullanilir. Bu sinif sayesinde
        /// hem verilere dogrudan setter erisimi kisitlanir hem de esnek ilklendirme saglanir.
        /// </summary>
        public class Uretici
        {
            internal HashSet<Ek> _gelebilecekEkler = new HashSet<Ek>();
            internal bool _sesliEkIleOlusur = false;
            internal bool _yapiBozucu = false;
            internal bool _secimlik = false;
            internal bool _ekKisitlayici = false;
            internal IHarfDizisiIslemi _islem;
            internal int _indeks;
            internal string _ad;
            internal bool _herZamanOlusur = false;

            public Uretici(int indeks, string ad, IHarfDizisiIslemi islem)
            {
                _indeks = indeks;
                _ad = ad;
                _islem = islem;
            }

            public Uretici GelebilecekEkler(HashSet<Ek> ekler)
            {
                this._gelebilecekEkler = ekler;
                return this;
            }

            public Uretici SesliEkIleOlusur(bool deger)
            {
                this._sesliEkIleOlusur = deger;
                return this;
            }

            public Uretici YapiBozucu(bool yapiBozucu)
            {
                this._yapiBozucu = yapiBozucu;
                return this;
            }

            public Uretici Secimlik(bool secimlik)
            {
                this._secimlik = secimlik;
                return this;
            }

            public Uretici EkKisitlayici(bool ekKisitlayici)
            {
                this._ekKisitlayici = ekKisitlayici;
                return this;
            }

            public Uretici HerZamanOlusur(bool herZamanOlusur)
            {
                this._herZamanOlusur = herZamanOlusur;
                return this;
            }

            public KokOzelDurumu Uret()
            {
                return new KokOzelDurumu(this);
            }
        }
    }
}