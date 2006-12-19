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
using Iesi.Collections;
using Iesi.Collections.Generic;
using net.zemberek.yapi.ek;


namespace net.zemberek.yapi.kok
{
    /**
    * User: ahmet
    * Date: Aug 29, 2006
    */ 
    public class TemelKokOzelDurumBilgisi
    {

        internal static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected EkYonetici ekYonetici;
        protected Alfabe alfabe;
        protected IDictionary<KokOzelDurumTipi, KokOzelDurumu> ozelDurumlar = new Dictionary<KokOzelDurumTipi, KokOzelDurumu>();
        protected IDictionary<String, KokOzelDurumu> kisaAdOzelDurumlar = new Dictionary<String, KokOzelDurumu>();

        public static readonly int MAX_OZEL_DURUM_SAYISI = 30;
        protected KokOzelDurumu[] ozelDurumDizisi = new KokOzelDurumu[MAX_OZEL_DURUM_SAYISI];

        public TemelKokOzelDurumBilgisi(EkYonetici ekYonetici, Alfabe alfabe) {
            this.ekYonetici = ekYonetici;
            this.alfabe = alfabe;
        }

        public KokOzelDurumu ozelDurum(int indeks) {
            if (indeks < 0 || indeks >= ozelDurumDizisi.Length)
                throw new IndexOutOfRangeException("istenilen indeksli ozel durum mevcut degil:" + indeks);
            return ozelDurumDizisi[indeks];
        }

        public KokOzelDurumu kisaAdIleOzelDurum(String ozelDurumKisaAdi) {
            return kisaAdOzelDurumlar[ozelDurumKisaAdi];
        }


        protected KokOzelDurumu.Uretici uretici(KokOzelDurumTipi tip, HarfDizisiIslemi islem) 
        {

            // bir adet kok ozel durumu uretici olustur.
            KokOzelDurumu.Uretici uretici = new KokOzelDurumu.Uretici(tip, islem);

            // eger varsa kok adlarini kullanarak iliskili ekleri bul ve bir Set'e ata.
            String[] ekAdlari = tip.EkAdlari;
            if (ekAdlari.Length > 0) {
                Set<Ek> set = new HashedSet<Ek>();
                foreach (String s in ekAdlari) {
                    Ek ek = ekYonetici.ek(s);
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

        protected void ekle(KokOzelDurumu.Uretici uretici) {
            //tum
            KokOzelDurumu ozelDurum = uretici.uret();
            ozelDurumlar.Add(ozelDurum.tip(), ozelDurum);
            ozelDurumDizisi[ozelDurum.indeks()] = ozelDurum;

            kisaAdOzelDurumlar.Add(ozelDurum.kisaAd(), ozelDurum);
        }

        protected void bosOzelDurumEkle(KokOzelDurumTipi[] args) 
        {
            foreach (KokOzelDurumTipi tip in args) 
            {
                ekle(uretici(tip,new BosHarfDizisiIslemi()));
            }
        }

        public KokOzelDurumu ozelDurum(String kisaAd) {
            return kisaAdOzelDurumlar[kisaAd];
        }

        public KokOzelDurumu ozelDurum(KokOzelDurumTipi tip) {
            return ozelDurumlar[tip];
        }
    }
}