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
using System.Collections.Generic;
using System.Text;
using log4net;

using NZemberek.Cekirdek.Yapi;
using NZemberek.Cekirdek.Kolleksiyonlar;


namespace NZemberek.Cekirdek.Yapi
{
    /**
    * User: ahmet
    * Date: Aug 29, 2006
    */ 
    public class TemelKokOzelDurumYonetici
    {
        protected static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected IEkYonetici ekYonetici;
        protected Alfabe alfabe;
        protected IDictionary<String, KokOzelDurumu> ozelDurumlar = new Dictionary<String, KokOzelDurumu>();

        public static readonly int MAX_OZEL_DURUM_SAYISI = 30;
        protected KokOzelDurumu[] ozelDurumDizisi = new KokOzelDurumu[MAX_OZEL_DURUM_SAYISI];

        public TemelKokOzelDurumYonetici(IEkYonetici ekYonetici, Alfabe alfabe) {
            this.ekYonetici = ekYonetici;
            this.alfabe = alfabe;
        }

        public KokOzelDurumu OzelDurum(int indeks) {
            if (indeks < 0 || indeks >= ozelDurumDizisi.Length)
                throw new IndexOutOfRangeException("istenilen indeksli ozel durum Mevcut degil:" + indeks);
            return ozelDurumDizisi[indeks];
        }

        public KokOzelDurumu KisaAdlaOzelDurum(String ozelDurumKisaAdi) {
            return ozelDurumlar[ozelDurumKisaAdi];
        }

        protected KokOzelDurumu.Uretici Uretici(int indeks, string ad, IHarfDizisiIslemi islem)
        {
            return this.Uretici(indeks, ad, new string[] { }, islem);
        }

        protected KokOzelDurumu.Uretici Uretici(int indeks, string ad, string[] ekAdlari, IHarfDizisiIslemi islem) 
        {

            // bir adet kok ozel durumu Uretici olustur.
            KokOzelDurumu.Uretici uretici = new KokOzelDurumu.Uretici(indeks, ad, islem);

            // eger varsa kok adlarini kullanarak iliskili ekleri Bul ve bir Set'e ata.
            if (ekAdlari.Length > 0) {
                HashSet<Ek> set = new HashSet<Ek>();
                foreach (String s in ekAdlari) {
                    Ek ek = ekYonetici.EkVer(s);
                    if (ek != null) {
                        set.Add(ek);
                    }
#if log
                    else 
                    {
                        logger.Warn(s + " eki bulunamadigindan kok ozel durumuna eklenemedi!");
                    }
#endif
                }
                // ureticiye seti ata.
                uretici.GelebilecekEkler(set);
            }
            return uretici;
        }

        protected void Ekle(KokOzelDurumu.Uretici uretici) {
            //tum
            KokOzelDurumu ozelDurum = uretici.Uret();
            ozelDurumDizisi[ozelDurum.Indeks] = ozelDurum;
            ozelDurumlar.Add(ozelDurum.Ad, ozelDurum);
        }

        protected void BosOzelDurumEkle(int indeks, string[] durumlar) 
        {
            foreach (string ad in durumlar) 
            {
                Ekle(Uretici(indeks++, ad,new BosHarfDizisiIslemi()));
            }
        }

        public KokOzelDurumu OzelDurum(String pAd) 
        {
            return ozelDurumlar[pAd];
        }
    }
}