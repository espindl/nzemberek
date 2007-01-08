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
    public class TemelKokOzelDurumBilgisi
    {

        protected static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected IEkYonetici ekYonetici;
        protected Alfabe alfabe;
        protected IDictionary<IKokOzelDurumTipi, KokOzelDurumu> ozelDurumlar = new Dictionary<IKokOzelDurumTipi, KokOzelDurumu>();
        protected IDictionary<String, KokOzelDurumu> kisaAdOzelDurumlar = new Dictionary<String, KokOzelDurumu>();

        public static readonly int MAX_OZEL_DURUM_SAYISI = 30;
        protected KokOzelDurumu[] ozelDurumDizisi = new KokOzelDurumu[MAX_OZEL_DURUM_SAYISI];

        public TemelKokOzelDurumBilgisi(IEkYonetici ekYonetici, Alfabe alfabe) {
            this.ekYonetici = ekYonetici;
            this.alfabe = alfabe;
        }

        public KokOzelDurumu OzelDurum(int indeks) {
            if (indeks < 0 || indeks >= ozelDurumDizisi.Length)
                throw new IndexOutOfRangeException("istenilen indeksli ozel durum Mevcut degil:" + indeks);
            return ozelDurumDizisi[indeks];
        }

        public KokOzelDurumu KisaAdlaOzelDurum(String ozelDurumKisaAdi) {
            return kisaAdOzelDurumlar[ozelDurumKisaAdi];
        }


        protected KokOzelDurumu.Uretici Uretici(IKokOzelDurumTipi tip, IHarfDizisiIslemi islem) 
        {

            // bir adet kok ozel durumu Uretici olustur.
            KokOzelDurumu.Uretici uretici = new KokOzelDurumu.Uretici(tip, islem);

            // eger varsa kok adlarini kullanarak iliskili ekleri Bul ve bir Set'e ata.
            String[] ekAdlari = tip.EkAdlari;
            if (ekAdlari.Length > 0) {
                HashSet<Ek> set = new HashSet<Ek>();
                foreach (String s in ekAdlari) {
                    Ek ek = ekYonetici.EkVer(s);
                    if (ek != null) {
                        set.Add(ek);
                    } else {
                        logger.Warn(s + " eki bulunamadigindan kok ozel durumuna eklenemedi!");
                    }
                }
                // ureticiye seti ata.
                uretici.gelebilecekEkler(set);
            }
            return uretici;
        }

        protected void Ekle(KokOzelDurumu.Uretici uretici) {
            //tum
            KokOzelDurumu ozelDurum = uretici.uret();
            ozelDurumlar.Add(ozelDurum.tip(), ozelDurum);
            ozelDurumDizisi[ozelDurum.indeks()] = ozelDurum;

            kisaAdOzelDurumlar.Add(ozelDurum.kisaAd(), ozelDurum);
        }

        protected void BosOzelDurumEkle(IKokOzelDurumTipi[] args) 
        {
            foreach (IKokOzelDurumTipi tip in args) 
            {
                Ekle(Uretici(tip,new BosHarfDizisiIslemi()));
            }
        }

        public KokOzelDurumu OzelDurum(String kisaAd) {
            return kisaAdOzelDurumlar[kisaAd];
        }

        public KokOzelDurumu OzelDurum(IKokOzelDurumTipi tip) {
            return ozelDurumlar[tip];
        }
    }
}