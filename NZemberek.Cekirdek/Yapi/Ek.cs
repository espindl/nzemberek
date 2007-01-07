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
 * The Original Code is Zemberek Doğal Dil İşleme Kütüphanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Akın, Mehmet D. Akın.
 * Portions created by the Initial Developer are Copyright (C) 2006
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Mert Derman
 *   Tankut Tekeli
 * ***** END LICENSE BLOCK ***** */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

using NZemberek.Cekirdek.Yapi;
using NZemberek.Cekirdek.Kolleksiyonlar;
using NZemberek.Cekirdek.Mekanizma.Cozumleme;


namespace NZemberek.Cekirdek.Yapi
{
    /**
 * Ek sinifi icerisinde eke ozel bilgiler, o ekten sonra gelebilecek eklerin listesi
 * ve o eke ozel ozel durumlar yer alir.
 * User: aakin
 * Date: Feb 15, 2004
 */
    [Serializable]
    public class Ek     
    {
        public static readonly HashSet<Ek> EMPTY_SET = new HashSet<Ek>();

        //bu ekten sonra elebilecek eklerin listesi.
        private List<Ek> _ardisilEkler = new List<Ek>();

        private String _ad;

        //ekin sesli ile baslayip baslayamayacagini belirler. bu bilgi otomatik olarak ek olusum kurallarina
        // gore baslangicta belirlenir.
        private bool sesliIleBaslayabilir = false;

        //kurallara gore ek olusumunu beliler. dile gore farkli gerceklemeleri olabilir.
        private EkUretici ekUretici;

        //bu ekin uretim kullarinin listesi.
        private List<EkUretimBileseni> uretimBilesenleri;

        //bu eke iliskin ozel durumlar.
        private List<EkOzelDurumu> ozelDurumlar = new List<EkOzelDurumu>(1);

        private bool sonEkOlamaz = false;

        private bool halEki = false;

        private bool iyelikEki = false;

        private HashSet<TurkceHarf> baslangicHarfleri;

        /**
         * ilk harfler kumesine gelen kumeyi ekler.
         * @param harfler
         */
        public void baslangicHarfleriEkle(HashSet<TurkceHarf> harfler) {
            if(harfler==null)
              return;        
            if(baslangicHarfleri==null)
              baslangicHarfleri = new HashSet<TurkceHarf>(); //TODO (5) idi
            this.baslangicHarfleri.AddAll(harfler);
        }

        public void setHalEki(bool halEki) {
            this.halEki = halEki;
        }

        public void setIyelikEki(bool iyelikEki) {
            this.iyelikEki = iyelikEki;
        }


        public bool halEkiMi() {
            return halEki;
        }

        public bool iyelikEkiMi() {
            return iyelikEki;
        }

        public HarfDizisi cozumlemeIcinUret(
            Kelime kelime,
            HarfDizisi giris,
            HarfDizisiKiyaslayici kiyaslayici) {

        foreach (EkOzelDurumu ozelDurum in ozelDurumlar) {
            HarfDizisi ozelDurumSonucu = ozelDurum.cozumlemeIcinUret(kelime, giris, kiyaslayici);
            if (ozelDurumSonucu != null)
                return ozelDurumSonucu;
        }
        return ekUretici.cozumlemeIcinEkUret(kelime.icerik(), giris, uretimBilesenleri);
    }

        public HarfDizisi olusumIcinUret(
                Kelime kelime,
                Ek sonrakiEk) {
        foreach (EkOzelDurumu ozelDurum in ozelDurumlar) {
            HarfDizisi ozelDurumSonucu = ozelDurum.olusumIcinUret(kelime, sonrakiEk);
            if (ozelDurumSonucu != null)
                return ozelDurumSonucu;
        }
        return ekUretici.olusumIcinEkUret(kelime.icerik(), sonrakiEk, uretimBilesenleri);
    }

        public void setOzelDurumlar(List<EkOzelDurumu> ozelDurumlar)
        {
            this.ozelDurumlar = ozelDurumlar;
        }

        public bool ardindanGelebilirMi(Ek ek)
        {
            return _ardisilEkler.Contains(ek);
        }

        public override bool Equals(Object o) 
        {
            if (this == o) return true;
            if (o == null || this.GetType() != o.GetType()) return false;

            Ek ek = (Ek) o;

            return !(_ad != null ? !_ad.Equals(ek._ad) : ek._ad != null);

        }

        public override int GetHashCode()
        {
            return (_ad != null ? _ad.GetHashCode() : 0);
        }

        public bool sesliIleBaslayabilirMi()
        {
            return sesliIleBaslayabilir;
        }

        public override String ToString()
        {
            return _ad;
        }

        public Ek(String name)
        {
            this._ad = name;
        }

        public String ad()
        {
            return _ad;
        }

        public Ek getArdisilEk(int ardisilEkSirasi)
        {
            if (ardisilEkSirasi < _ardisilEkler.Count)
                return _ardisilEkler[ardisilEkSirasi];
            return null;
        }

        public bool OzelEkOlustur(Kelime ozelKelime)
        {
            return false;
        }

        public List<Ek> ardisilEkler()
        {
            return _ardisilEkler;
        }

        public void setArdisilEkler(List<Ek> ardisilEkler)
        {
            this._ardisilEkler = ardisilEkler;
        }

        public void setSesliIleBaslayabilir(bool sesliIleBaslayabilir)
        {
            this.sesliIleBaslayabilir = sesliIleBaslayabilir;
        }

        public void setEkKuralCozumleyici(EkUretici ekUretici)
        {
            this.ekUretici = ekUretici;
        }

        public void setUretimBilesenleri(List<EkUretimBileseni> uretimBilesenleri)
        {
            this.uretimBilesenleri = uretimBilesenleri;
        }

        public bool sonEkOlamazMi()
        {
            return sonEkOlamaz;
        }

        public void setSonEkOlamaz(bool sonEkOlamaz)
        {
            this.sonEkOlamaz = sonEkOlamaz;
        }

        /**
         * Eger baslangic harfleri kumsei var ise gelen harfin bu kumede olup olmadigina bakar.
         * @param ilkHarf
         * @return eger kume tanimlanmamis ise bu ek icin ilk harf denetimi yapilmiyor demektir, true doner.
         *   eger kume mevcut ise (null disi) ve harf kumede mevcutsa true doner. aksi halde false.
         */
        public bool ilkHarfDenetle(TurkceHarf ilkHarf)
        {
            return baslangicHarfleri == null || baslangicHarfleri.Contains(ilkHarf);
        }
    }
}
