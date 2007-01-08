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
    /// <summary>
    /// Ek sinifi icerisinde eke ozel bilgiler, o ekten sonra gelebilecek eklerin listesi ve o eke ozel ozel durumlar yer alir
    /// </summary>
    [Serializable]
    public class Ek
    {
        public Ek(String name)
        {
            this._ad = name;
        }

        public static readonly HashSet<Ek> EMPTY_SET = new HashSet<Ek>();

        //bu ekten sonra elebilecek eklerin listesi.
        private List<Ek> _ardisilEkler = new List<Ek>();

        public List<Ek> ArdisilEkler
        {
            get { return _ardisilEkler; }
            set { _ardisilEkler = value; }
        }

        private String _ad;

        public String Ad
        {
            get { return _ad; }
            set { _ad = value; }
        }

        //ekin sesli ile baslayip baslayamayacagini belirler. bu bilgi otomatik olarak ek olusum kurallarina
        // gore baslangicta belirlenir.
        private bool sesliIleBaslayabilir = false;

        public bool SesliIleBaslayabilir
        {
            get { return sesliIleBaslayabilir; }
            set { sesliIleBaslayabilir = value; }
        }

        //kurallara gore ek olusumunu beliler. dile gore farkli gerceklemeleri olabilir.
        private IEkUretici ekUretici;

        public IEkUretici EkUretici
        {
            set { ekUretici = value; }
        }

        //bu ekin uretim kullarinin listesi.
        private List<EkUretimBileseni> uretimBilesenleri;

        public List<EkUretimBileseni> UretimBilesenleri
        {
            set { uretimBilesenleri = value; }
        }

        //bu eke iliskin ozel durumlar.
        private List<EkOzelDurumu> ozelDurumlar = new List<EkOzelDurumu>(1);

        public List<EkOzelDurumu> OzelDurumlar
        {
            get { return ozelDurumlar; }
            set { ozelDurumlar = value; }
        }

        private bool sonEkOlamaz = false;

        public bool SonEkOlamaz
        {
            get { return sonEkOlamaz; }
            set { sonEkOlamaz = value; }
        }

        private bool halEki = false;

        public bool HalEki
        {
            get { return halEki; }
            set { halEki = value; }
        }

        private bool iyelikEki = false;

        public bool IyelikEki
        {
            get { return iyelikEki; }
            set { iyelikEki = value; }
        }

        private HashSet<TurkceHarf> baslangicHarfleri;

        /// <summary>
        /// ilk harfler kumesine gelen kumeyi ekler
        /// </summary>
        /// <param name="harfler"></param>
        public void BaslangicHarfleriEkle(HashSet<TurkceHarf> harfler)
        {
            if (harfler == null)
                return;
            if (baslangicHarfleri == null)
                baslangicHarfleri = new HashSet<TurkceHarf>(); //TODO (5) idi
            this.baslangicHarfleri.AddAll(harfler);
        }

        public HarfDizisi CozumlemeIcinUret(Kelime kelime,HarfDizisi giris,IHarfDizisiKiyaslayici kiyaslayici)
        {
            foreach (EkOzelDurumu ozelDurum in ozelDurumlar)
            {
                HarfDizisi ozelDurumSonucu = ozelDurum.CozumlemeIcinUret(kelime, giris, kiyaslayici);
                if (ozelDurumSonucu != null)
                    return ozelDurumSonucu;
            }
            return ekUretici.CozumlemeIcinEkUret(kelime.Icerik, giris, uretimBilesenleri);
        }

        public HarfDizisi OlusumIcinUret(Kelime kelime, Ek sonrakiEk)
        {
            foreach (EkOzelDurumu ozelDurum in ozelDurumlar)
            {
                HarfDizisi ozelDurumSonucu = ozelDurum.OlusumIcinUret(kelime, sonrakiEk);
                if (ozelDurumSonucu != null)
                    return ozelDurumSonucu;
            }
            return ekUretici.OlusumIcinEkUret(kelime.Icerik, sonrakiEk, uretimBilesenleri);
        }

        public bool ArdindanGelebilir(Ek ek)
        {
            return _ardisilEkler.Contains(ek);
        }

        public override bool Equals(Object o)
        {
            if (this == o) return true;
            if (o == null || this.GetType() != o.GetType()) return false;

            Ek ek = (Ek)o;

            return !(_ad != null ? !_ad.Equals(ek._ad) : ek._ad != null);

        }

        public override int GetHashCode()
        {
            return (_ad != null ? _ad.GetHashCode() : 0);
        }

        public Ek ArdisilEkGetir(int ardisilEkSirasi)
        {
            if (ardisilEkSirasi < _ardisilEkler.Count)
                return _ardisilEkler[ardisilEkSirasi];
            return null;
        }

        public bool OzelEkOlustur(Kelime ozelKelime)
        {
            return false;
        }

        /// <summary>
        /// Eger baslangic harfleri kumsei var ise gelen harfin bu kumede olup olmadigina bakar.
        /// </summary>
        /// <param name="IlkHarf"></param>
        /// <returns>eger kume tanimlanmamis ise bu ek icin ilk Harf denetimi yapilmiyor demektir, true doner.
        /// eger kume Mevcut ise (null disi) ve Harf kumede mevcutsa true doner. aksi halde false</returns>
        public bool IlkHarfDenetle(TurkceHarf ilkHarf)
        {
            return baslangicHarfleri == null || baslangicHarfleri.Contains(ilkHarf);
        }
    }
}
