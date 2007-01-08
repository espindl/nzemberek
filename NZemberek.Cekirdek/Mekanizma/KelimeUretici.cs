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
using NZemberek.Cekirdek.Yapi;
using NZemberek.Cekirdek.Mekanizma.Cozumleme;

namespace NZemberek.Cekirdek.Mekanizma
{
    public class KelimeUretici
    {
        private Alfabe alfabe;
        private ICozumlemeYardimcisi yardimci;

        public KelimeUretici(Alfabe alfabe, ICozumlemeYardimcisi yardimci)
        {
            this.alfabe = alfabe;
            this.yardimci = yardimci;
        }

        /// <summary>
        /// Kok ve Ek listesi tasiyan bir kelimeyi String listesi seklinde parcalara ayirir.
        /// Kelime {kok={kitap, ISIM} ekler = {ISIM_SAHIPLIK_BEN, ISIM_YONELME_E}} icin {kitap,Im,a} dizisi doner.
        /// </summary>
        /// <param name="kelime"></param>
        /// <returns>kok ve ek icerikleri (String[]) cinsinden dizi. Eger ek listesi bos ise ya da
        /// sadece yalin ek var ise sadece kok icerigi doner. Kokun ozel durum ile bozulmus hali degilorjinal icerigini iceren dizi doner.
        /// TODO: simdilik ozel adlarda bas Harf kucuk olarak donuyor. Ayrica ozel yazimli koklerin orjinali
        /// degil ayiklanmis hali doner.</returns>
        public IList<String> Ayristir(Kelime kelime)
        {
            UretimNesnesi ure = UretimNesnesiUret(kelime.Kok, kelime.Ekler);
            return ure.olusumlar;
        }

        private UretimNesnesi UretimNesnesiUret(Kok kok, IList<Ek> ekler)
        {
            UretimNesnesi ure = new UretimNesnesi(kok.Icerik);
            Kelime kelime = new Kelime(kok, alfabe);

            if (ekler.Count > 1)
            {
                HarfDizisi ozelDurumSonrasi = kok.OzelDurumUygula(alfabe, ekler[1]);
                if (ozelDurumSonrasi != null)
                    kelime.Icerik = ozelDurumSonrasi;
                else
                    return ure;
            }
            else
            {
                return ure;
            }

            for (int i = 0; i < ekler.Count; i++)
            {
                Ek ek = ekler[i];

                // eger incelenen ek onceki ekten sonra gelemezse cik.
                if (i > 0)
                {
                    Ek oncekiEk = ekler[i - 1];
                    if (!oncekiEk.ArdindanGelebilir(ek))
                    {
                        return ure;
                    }
                }

                //olusum icin kural belirle ve eki olustur.
                HarfDizisi ekOlusumu;
                if (i < ekler.Count - 1)
                    ekOlusumu = new HarfDizisi(ek.OlusumIcinUret(kelime, ekler[i + 1]));
                else
                    ekOlusumu = new HarfDizisi(ek.OlusumIcinUret(kelime, TemelEkYonetici.BOS_EK));

                //TODO: asagidaki bolum dil ozel. muhtemelen olusumIcinURet metodu duzletilirse gerek kalmaz.
                // ek son Harf yumusatmayi kendimiz hallediyoruz (eger yalin ek ise bu islemi pas geciyoruz.)
                if (i > 1)
                {
                    if (kelime.SonHarf().Sert && ekOlusumu.IlkHarf().Sesli)
                        kelime.Icerik.SonHarfYumusat();
                }

                //eki kelimeye ve ek olusumlarina Ekle.
                kelime.IcerikEkle(ekOlusumu);
                if (ekOlusumu.Boy > 0)
                    ure.olusumlar.Add(ekOlusumu.ToString());
                kelime.Ekler.Add(ek);
            }

            //son duzeltmeleri Uygula.
            yardimci.KelimeBicimlendir(kelime);
            ure.olusum = kelime.IcerikMetni();
            return ure;
        }

        internal class UretimNesnesi
        {
            internal String olusum = "";
            internal IList<String> olusumlar = new List<String>(4);

            public UretimNesnesi(String ilkolusum)
            {
                this.olusum = ilkolusum;
                olusumlar.Add(ilkolusum);
            }
        }
    }
}
