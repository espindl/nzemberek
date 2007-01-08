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
	
	/// <summary> Koke ilskin ozel durumu ifade eder. kok ozel durumlarinin farkli ozlelikleri
	/// bu sinifta belirtilir. Dogrudan uretilmez, once Uretici ic sinifi olusturulmasi gerekir.
	/// </summary>
    public class KokOzelDurumu
    {
        /// <summary>
        /// Tip  bilgisi bu ozel duruma iliskin cesitli kimlik bilgilerini tasir.
        /// onegin ozel durumun adi, indeksi, ek dizisi gibi.
        /// dile gore farkli Tip gerceklemeleri mevcttur.
        /// </summary>
//        private IKokOzelDurumTipi _tip;



        private int indeks;
        private string ad;

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
        /// Kok ozel durumu bir kelime uzerinde ne tur islem yapacak bu nesne ile belirlenir.
        /// </summary>
        private IHarfDizisiIslemi _islem;

        /**
         * bazi ozel durumlar sadece bazi eklerin koke eklenmesi ile olusur. Bu listede
         * bu ekler yer alir.
         */
        private HashSet<Ek> _gelebilecekEkler = new HashSet<Ek>();

        /**
         * Eger ozel durum kokun yapisini bozuyorsa true.
         */
        private bool _yapiBozucu = false;

        /**
         * bazi ozel durumlarin olusmasi mevburi degildir. ornegin turkiye turkceisnde
         * "icerlerde" ve "icerilerde" kelimeleri dogru kabul edilir. Eger ozel durum
         * bu sekilde ise Secimlik=true olur.
         */
        private bool _secimlik = false;

        /**
         * Ek kisitlayici ozel durumlar genellikle kokun yapisini bozmaktansa kendisinden sonra
         * bazei eklerin gelmesini engeller.
         */
        private bool _ekKisitlayici = false;

        /**
         * Cogu kok bozucu ozel durum sadece kendisinden sonra sesli ile baslayan
         * bir ekin glmesi ile olusur. but bayrak bu tyur ozel durumlar icin true olur.
         */
        private bool _sesliEkIleOlusur = false;

        /**
         * Cogu kok bozucu ozel durum sadece kendisinden sonra sesli ile baslayan
         * bir ekin glmesi ile olusur. but bayrak bu tyur ozel durumlar icin true olur.
         */
        private bool _herZamanOlusur = false;


        /**
         * bu sinif KokOzelDurumu uretimi icin kullanilir. Bu sinif sayesinde
         * hem verilere dogrudan setter erisimi kisitlanir hem de esnek ilklnedirme saglanir.
         */
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

        /**
         * KokOzelDurumu Uretici nesnesi uzerinden uretilir. dogrudan erisim yoktur.
         *
         * @param Uretici
         */
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

        public HashSet<Ek> GeleibilecekEkler()
        {
            return _gelebilecekEkler;
        }


        public bool EkKisitlayici()
        {
            return _ekKisitlayici;
        }

        /**
         * giris ile gelen [dizi] Harf dizisine ozel durumu uygular.
         * basit ziyaretci deseni (visitor pattern).
         *
         * @param dizi
         */
        public void Uygula(HarfDizisi dizi)
        {
            _islem.Uygula(dizi);
        }
        
        /**
         * Ozel durum giris parametresi olan ek'in bu ozel durumun olusmasina
         * izin verip vermeyeegi belirlenir.
         *
         * @param ek
         * @return gelen ek ile bu ozel durum olusabilirse true
         */
        public bool Olusabilir(Ek ek)
        {
            if (_herZamanOlusur)
                return true;
            if (_sesliEkIleOlusur && ek.SesliIleBaslayabilir)
                return true;
            return _gelebilecekEkler.Contains(ek);
        }

        /**
         * esitlik kiyaslamasi sadece Tip indexi ve Tip adina gore yapilir.
         *
         * @param o
         * @return ayni ise true.
         */
        public override bool Equals(Object o) {
        if (this == o) return true;
        if (o == null || GetType() != o.GetType()) return false;

        KokOzelDurumu that = (KokOzelDurumu) o;
        if ((indeks == that.Indeks) && ad.Equals(that.Ad))
            return true;
        return false;

    }

        /**
         * sadece Tip adi ve indeksine gore belirlenir.
         *
         * @return hash code.
         */
        public override int GetHashCode()
        {
            return ad.GetHashCode() * indeks;
        }
    }
}