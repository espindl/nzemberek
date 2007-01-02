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

// V 0.1
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Iesi.Collections.Generic;
using net.zemberek.yapi;

namespace net.zemberek.tr.yapi
{
    public class TurkceHeceBulucu : IHeceleyici
    {
        private Alfabe alfabe;
        public TurkceHeceBulucu(Alfabe alf)
        {
            alfabe = alf;
        }
        /**
         * Giren harf dizisinin sonunda mantikli olarak yer alan hecenin harf
         * sayisini dondurur.
         * Sistem, -trak ve benzeri harf dizilimine sahip kelimeleri hecelemiyor.
         *
         * @param kelime: turkce harf dizisi.
         * @return int, 1,2,3 ya da 4 donerse giris dizisinin dizinin sondan o
         *         kadarharfi heceyi temsil eder -1 donerse hecenin bulunamadigi
         *         anlamina gelir. sistem yabanci harf ya da isaretlerin oldugu ya
         *         da kural disi kelimeleri heceleyemez. (ornegin, three, what vs.)
         *         TODO: sistem su anda basta bulunan iki harf sessiz oldugu
         *         durumlari kabul etmekte ama buna kisitlama getirilmesi iyi olur.
         *         sadece "tr", "st", "kr" gibi girislere izin verilmeli
         */
        private int sonHeceHarfSayisi(HarfDizisi kelime)
        {

            int boy = kelime.Length;
            TurkceHarf harf = kelime.harf(boy - 1);
            TurkceHarf oncekiHarf = kelime.harf(boy - 2);

            if (boy == 0)
                return -1;

            if (harf.Sesli)
            {
                //kelime sadece sesli.
                if (boy == 1)
                    return 1;
                //onceki harf sesli kelime="saa" ise son ek "a"
                if (oncekiHarf.Sesli)
                    return 1;
                //onceki harf sessiz ise ve kelime sadece 2 harf ise hece tum kelime. "ya"
                if (boy == 2)
                    return 2;

                TurkceHarf ikiOncekiHarf = kelime.harf(boy - 3);

                // steteskp > ste
                if (!ikiOncekiHarf.Sesli && boy == 3)
                {
                    return 3;
                }
                return 2;
            }
            else
            {

                // tek sessiz ile hece olmaz.
                if (boy == 1)
                    return -1;

                TurkceHarf ikiOncekiHarf = kelime.harf(boy - 3);
                if (oncekiHarf.Sesli)
                {

                    //kelime iki harfli (el, al) ya da iki onceki harf sesli (saat)
                    if (boy == 2 || ikiOncekiHarf.Sesli)
                        return 2;

                    TurkceHarf ucOncekiHarf = kelime.harf(boy - 4);
                    // kelime uc harfli (kal, sel) ya da uc onceki harf sesli (kanat),
                    if (boy == 3 || ucOncekiHarf.Sesli)
                        return 3;

                    //kelime dort harfli ise yukaridaki kurallari gecmesi nedeniyle hecelenemez sayiyoruz.
                    // ornegin tren strateji krank angstrom kelimelerii hecelenemez sayiyoruz.
                    if (boy == 4)
                        return -1;

                    TurkceHarf dortOncekiHarf = kelime.harf(boy - 5);
                    if (!dortOncekiHarf.Sesli)
                        return 3;
                    return 3;
                }
                else
                {


                        if (boy == 2 || !ikiOncekiHarf.Sesli)
                            return -1;
                        TurkceHarf ucOncekiHarf = kelime.harf(boy - 4);
                        if (boy > 3 && !ucOncekiHarf.Sesli)
                            return 4;
                        return 3;

                }
            }

        }

        #region IHeceleyici Members

        /**
     * Gelen String'i turkce heceleme kurallarina gore hecelerine ayirir. Sonucta
     * heceleri bir liste icinde dondurur. Eger heceleme yapilamazsa bos liste doner.
     *
     * @param giris
     * @return sonHeceHarfSayisi String dizisi
     */
        public String[] hecele(String giris)
        {
            giris = alfabe.ayikla(giris);
            HarfDizisi kelime = new HarfDizisi(giris, alfabe);
            ArrayList list = new ArrayList(); //reverse kullanmak icin generics kullanmadim...
            while (kelime.Length > 0)
            {
                int index = this.sonHeceHarfSayisi(kelime);
                if (index < 0)
                {
                    list.Clear();
                    return new String[0];
                }
                int basla = kelime.Length - index;
                list.Add(kelime.ToString(basla));
                kelime.kirp(basla);
            }
            list.Reverse();
            String[] retArr = new String[list.Count];
            list.CopyTo(retArr, 0);
            return retArr;
        }

        /**
         * girisin hecelenebir olup olmadigini bulur.
         *
         * @param giris
         * @return hecelenebilirse true, aksi halde false.
         */
        public bool hecelenebilirmi(String giris)
        {
            HarfDizisi kelime = new HarfDizisi(giris, alfabe);
            while (kelime.Length > 0)
            {
                int index = this.sonHeceHarfSayisi(kelime);
                if (index < 0)
                    return false;
                int basla = kelime.Length - index;
                kelime.kirp(basla);
            }
            return true;
        }

        /**
         * Verilen kelime için sonHeceHarfSayisi indekslerini bir dizi içinde döndürür
         *
         * @param giris : Hece indeksleri belirlenecek
         * @return Hece indekslerini tutan bir int[]
         *         Ã–rnek: "merhaba" kelimesi için 0,3,5
         *         "türklerin" kelimesi için 0,4,6
         */
        public int[] heceIndeksleriniBul(String giris)
        {
            giris = alfabe.ayikla(giris);
            HarfDizisi kelime = new HarfDizisi(giris, alfabe);
            int[] tmpHeceIndeksleri = new int[50];
            int heceIndeks = 0;
            while (kelime.Length > 0)
            {
                int index = this.sonHeceHarfSayisi(kelime);
                if (index < 0)
                {
                    return null;
                }
                int basla = kelime.Length - index;
                tmpHeceIndeksleri[heceIndeks++] = basla;
                if (heceIndeks > 50) return null;
                kelime.kirp(basla);
            }
            int[] heceIndeksleri = new int[heceIndeks];
            for (int i = 0; i < heceIndeks; i++)
            {
                heceIndeksleri[i] = tmpHeceIndeksleri[heceIndeks - i - 1];
            }
            return heceIndeksleri;
        }

        #endregion
    }
}
