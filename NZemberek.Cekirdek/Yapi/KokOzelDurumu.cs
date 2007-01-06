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
using Iesi.Collections.Generic;
using Iesi.Collections;

namespace NZemberek.Cekirdek.Yapi
{
	
	/// <summary> Koke ilskin ozel durumu ifade eder. kok ozel durumlarinin farkli ozlelikleri
	/// bu sinifta belirtilir. Dogrudan uretilmez, once Uretici ic sinifi olusturulmasi gerekir.
	/// </summary>
    public class KokOzelDurumu
    {
        /**
          * tip  bilgisi bu ozel duruma iliskin cesitli kimlik bilgilerini tasir.
          * onegin ozel durumun adi, indeksi, ek dizisi gibi.
          * dile gore farkli tip gerceklemeleri mevcttur.
          */
        private KokOzelDurumTipi _tip;

        /**
         * Kok ozel durumu bir kelime uzerinde ne tur islem yapacak bu nesne ile belirlenir.
         */
        private HarfDizisiIslemi _islem;

        /**
         * bazi ozel durumlar sadece bazi eklerin koke eklenmesi ile olusur. Bu listede
         * bu ekler yer alir.
         */
        private Set<Ek> _gelebilecekEkler = new HashedSet<Ek>();

        /**
         * Eger ozel durum kokun yapisini bozuyorsa true.
         */
        private bool _yapiBozucu = false;

        /**
         * bazi ozel durumlarin olusmasi mevburi degildir. ornegin turkiye turkceisnde
         * "icerlerde" ve "icerilerde" kelimeleri dogru kabul edilir. Eger ozel durum
         * bu sekilde ise secimlik=true olur.
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

            internal Set<Ek> _gelebilecekEkler = new HashedSet<Ek>();
            internal bool _sesliEkIleOlusur = false;
            internal bool _yapiBozucu = false;
            internal bool _secimlik = false;
            internal bool _ekKisitlayici = false;
            internal HarfDizisiIslemi _islem;
            internal KokOzelDurumTipi _tip;
            internal bool _herZamanOlusur = false;

            public Uretici(KokOzelDurumTipi tip, HarfDizisiIslemi islem)
            {
                this._tip = tip;
                this._islem = islem;
            }

            public Uretici gelebilecekEkler(Set<Ek> ekler)
            {
                this._gelebilecekEkler = ekler;
                return this;
            }

            public Uretici sesliEkIleOlusur(bool deger)
            {
                this._sesliEkIleOlusur = deger;
                return this;
            }

            public Uretici yapiBozucu(bool yapiBozucu)
            {
                this._yapiBozucu = yapiBozucu;
                return this;
            }

            public Uretici secimlik(bool secimlik)
            {
                this._secimlik = secimlik;
                return this;
            }

            public Uretici ekKisitlayici(bool ekKisitlayici)
            {
                this._ekKisitlayici = ekKisitlayici;
                return this;
            }

            public Uretici herZamanOlusur(bool herZamanOlusur)
            {
                this._herZamanOlusur = herZamanOlusur;
                return this;
            }


            public Uretici parametre(KokOzelDurumTipi tip)
            {
                this._tip = tip;
                return this;
            }

            public KokOzelDurumu uret()
            {
                return new KokOzelDurumu(this);
            }
        }

        /**
         * KokOzelDurumu uretici nesnesi uzerinden uretilir. dogrudan erisim yoktur.
         *
         * @param uretici
         */
        private KokOzelDurumu(Uretici uretici)
        {
            this._gelebilecekEkler = uretici._gelebilecekEkler;
            this._sesliEkIleOlusur = uretici._sesliEkIleOlusur;
            this._yapiBozucu = uretici._yapiBozucu;
            this._secimlik = uretici._secimlik;
            this._ekKisitlayici = uretici._ekKisitlayici;
            this._tip = uretici._tip;
            this._islem = uretici._islem;
            this._herZamanOlusur = uretici._herZamanOlusur;
        }

        public bool yapiBozucumu()
        {
            return _yapiBozucu;
        }

        public bool secimlikmi()
        {
            return _secimlik;
        }

        public bool seslikEkleolusurmu()
        {
            return _sesliEkIleOlusur;
        }

        public Set<Ek> geleibilecekEkler()
        {
            return _gelebilecekEkler;
        }


        public bool ekKisitlayiciMi()
        {
            return _ekKisitlayici;
        }

        public int indeks()
        {
            return _tip.Index;
        }

        public String kisaAd()
        {
            return _tip.KisaAd;
        }

        public KokOzelDurumTipi tip()
        {
            return _tip;
        }
        
        /**
         * giris ile gelen [dizi] harf dizisine ozel durumu uygular.
         * basit ziyaretci deseni (visitor pattern).
         *
         * @param dizi
         */
        public void uygula(HarfDizisi dizi)
        {
            _islem.uygula(dizi);
        }

        /**
         * Ozel durum giris parametresi olan ek'in bu ozel durumun olusmasina
         * izin verip vermeyeegi belirlenir.
         *
         * @param ek
         * @return gelen ek ile bu ozel durum olusabilirse true
         */
        public bool olusabilirMi(Ek ek)
        {
            if (_herZamanOlusur)
                return true;
            if (_sesliEkIleOlusur && ek.sesliIleBaslayabilirMi())
                return true;
            return _gelebilecekEkler.Contains(ek);
        }

        /**
         * esitlik kiyaslamasi sadece tip indexi ve tip adina gore yapilir.
         *
         * @param o
         * @return ayni ise true.
         */
        public override bool Equals(Object o) {
        if (this == o) return true;
        if (o == null || GetType() != o.GetType()) return false;

        KokOzelDurumu that = (KokOzelDurumu) o;
        if ((_tip.Index == that._tip.Index) && _tip.Ad.Equals(that._tip.Ad))
            return true;
        return false;

    }

        /**
         * sadece tip adi ve indeksine gore belirlenir.
         *
         * @return hash code.
         */
        public override int GetHashCode()
        {
            return _tip.Ad.GetHashCode() * _tip.Index;
        }
    }
}