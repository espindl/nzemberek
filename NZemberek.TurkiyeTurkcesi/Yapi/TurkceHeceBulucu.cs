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
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text;
//using Iesi.Collections.Generic;
//using NZemberek.Cekirdek.Yapi;

//namespace NZemberek.TrTurkcesi.Yapi
//{
//    public class TurkceHeceBulucu : IHeceleyici
//    {
//        private Alfabe Alfabe;
//        public TurkceHeceBulucu(Alfabe alf)
//        {
//            Alfabe = alf;
//        }
//        /**
//         * Giren Harf dizisinin sonunda mantikli olarak yer alan hecenin Harf
//         * sayisini dondurur.
//         * Sistem, -trak ve benzeri Harf dizilimine sahip kelimeleri hecelemiyor.
//         *
//         * @param kelime: turkce Harf dizisi.
//         * @return int, 1,2,3 ya da 4 donerse giris dizisinin dizinin sondan o
//         *         kadarharfi heceyi temsil eder -1 donerse hecenin bulunamadigi
//         *         anlamina gelir. sistem yabanci Harf ya da isaretlerin oldugu ya
//         *         da kural disi kelimeleri heceleyemez. (ornegin, three, what vs.)
//         *         TODO: sistem su anda basta bulunan iki Harf sessiz oldugu
//         *         durumlari kabul etmekte ama buna kisitlama getirilmesi iyi olur.
//         *         sadece "tr", "st", "kr" gibi girislere izin verilmeli
//         */
//        private int sonHeceHarfSayisi(HarfDizisi kelime)
//        {

//            int Boy = kelime.Boy;
//            TurkceHarf Harf = kelime.Harf(Boy - 1);
//            TurkceHarf oncekiHarf = kelime.Harf(Boy - 2);

//            if (Boy == 0)
//                return -1;

//            if (Harf.Sesli)
//            {
//                //kelime sadece sesli.
//                if (Boy == 1)
//                    return 1;
//                //onceki Harf sesli kelime="saa" ise son ek "a"
//                if (oncekiHarf.Sesli)
//                    return 1;
//                //onceki Harf sessiz ise ve kelime sadece 2 Harf ise hece tum kelime. "ya"
//                if (Boy == 2)
//                    return 2;

//                TurkceHarf ikiOncekiHarf = kelime.Harf(Boy - 3);

//                // steteskp > ste
//                if (!ikiOncekiHarf.Sesli && Boy == 3)
//                {
//                    return 3;
//                }
//                return 2;
//            }
//            else
//            {

//                // tek sessiz ile hece olmaz.
//                if (Boy == 1)
//                    return -1;

//                TurkceHarf ikiOncekiHarf = kelime.Harf(Boy - 3);
//                if (oncekiHarf.Sesli)
//                {

//                    //kelime iki harfli (el, Al) ya da iki onceki Harf sesli (saat)
//                    if (Boy == 2 || ikiOncekiHarf.Sesli)
//                        return 2;

//                    TurkceHarf ucOncekiHarf = kelime.Harf(Boy - 4);
//                    // kelime uc harfli (kal, sel) ya da uc onceki Harf sesli (kanat),
//                    if (Boy == 3 || ucOncekiHarf.Sesli)
//                        return 3;

//                    //kelime dort harfli ise yukaridaki kurallari gecmesi nedeniyle hecelenemez sayiyoruz.
//                    // ornegin tren strateji krank angstrom kelimelerii hecelenemez sayiyoruz.
//                    if (Boy == 4)
//                        return -1;

//                    TurkceHarf dortOncekiHarf = kelime.Harf(Boy - 5);
//                    if (!dortOncekiHarf.Sesli)
//                        return 3;
//                    return 3;
//                }
//                else
//                {


//                        if (Boy == 2 || !ikiOncekiHarf.Sesli)
//                            return -1;
//                        TurkceHarf ucOncekiHarf = kelime.Harf(Boy - 4);
//                        if (Boy > 3 && !ucOncekiHarf.Sesli)
//                            return 4;
//                        return 3;

//                }
//            }

//        }

//        #region IHeceleyici Members

//        /**
//     * Gelen String'i turkce heceleme kurallarina gore hecelerine ayirir. Sonucta
//     * heceleri bir liste icinde dondurur. Eger heceleme yapilamazsa bos liste doner.
//     *
//     * @param giris
//     * @return sonHeceHarfSayisi String dizisi
//     */
//        public String[] Hecele(String giris)
//        {
//            giris = Alfabe.Ayikla(giris);
//            HarfDizisi kelime = new HarfDizisi(giris, Alfabe);
//            ArrayList list = new ArrayList(); //reverse kullanmak icin generics kullanmadim...
//            while (kelime.Boy > 0)
//            {
//                int index = this.sonHeceHarfSayisi(kelime);
//                if (index < 0)
//                {
//                    list.Clear();
//                    return new String[0];
//                }
//                int basla = kelime.Boy - index;
//                list.Add(kelime.ToString(basla));
//                kelime.Kirp(basla);
//            }
//            list.Reverse();
//            String[] retArr = new String[list.Count];
//            list.CopyTo(retArr, 0);
//            return retArr;
//        }

//        /**
//         * girisin hecelenebir olup olmadigini bulur.
//         *
//         * @param giris
//         * @return hecelenebilirse true, aksi halde false.
//         */
//        public bool Hecelenebilir(String giris)
//        {
//            HarfDizisi kelime = new HarfDizisi(giris, Alfabe);
//            while (kelime.Boy > 0)
//            {
//                int index = this.sonHeceHarfSayisi(kelime);
//                if (index < 0)
//                    return false;
//                int basla = kelime.Boy - index;
//                kelime.Kirp(basla);
//            }
//            return true;
//        }

//        /**
//         * Verilen kelime için sonHeceHarfSayisi indekslerini bir dizi içinde döndürür
//         *
//         * @param giris : Hece indeksleri belirlenecek
//         * @return Hece indekslerini tutan bir int[]
//         *         Ã–rnek: "merhaba" kelimesi için 0,3,5
//         *         "türklerin" kelimesi için 0,4,6
//         */
//        public int[] HeceIndeksleriniBul(String giris)
//        {
//            giris = Alfabe.Ayikla(giris);
//            HarfDizisi kelime = new HarfDizisi(giris, Alfabe);
//            int[] tmpHeceIndeksleri = new int[50];
//            int heceIndeks = 0;
//            while (kelime.Boy > 0)
//            {
//                int index = this.sonHeceHarfSayisi(kelime);
//                if (index < 0)
//                {
//                    return null;
//                }
//                int basla = kelime.Boy - index;
//                tmpHeceIndeksleri[heceIndeks++] = basla;
//                if (heceIndeks > 50) return null;
//                kelime.Kirp(basla);
//            }
//            int[] heceIndeksleri = new int[heceIndeks];
//            for (int i = 0; i < heceIndeks; i++)
//            {
//                heceIndeksleri[i] = tmpHeceIndeksleri[heceIndeks - i - 1];
//            }
//            return heceIndeksleri;
//        }

//        #endregion
//    }
//}
