// V 0.1
using System;
using System.Collections.Generic;
using System.Text;
using Iesi.Collections.Generic;
using net.zemberek.yapi;

namespace net.zemberek.tr.yapi
{
    public class TurkceHeceBulucu :HeceBulucu
    {
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
        public int sonHeceHarfSayisi(HarfDizisi kelime)
        {

            int boy = kelime.Length;
            TurkceHarf harf = kelime.harf(boy - 1);
            TurkceHarf oncekiHarf = kelime.harf(boy - 2);

            if (boy == 0)
                return -1;

            if (harf.sesliMi())
            {
                //kelime sadece sesli.
                if (boy == 1)
                    return 1;
                //onceki harf sesli kelime="saa" ise son ek "a"
                if (oncekiHarf.sesliMi())
                    return 1;
                //onceki harf sessiz ise ve kelime sadece 2 harf ise hece tum kelime. "ya"
                if (boy == 2)
                    return 2;

                TurkceHarf ikiOncekiHarf = kelime.harf(boy - 3);

                // steteskp > ste
                if (!ikiOncekiHarf.sesliMi() && boy == 3)
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
                if (oncekiHarf.sesliMi())
                {

                    //kelime iki harfli (el, al) ya da iki onceki harf sesli (saat)
                    if (boy == 2 || ikiOncekiHarf.sesliMi())
                        return 2;

                    TurkceHarf ucOncekiHarf = kelime.harf(boy - 4);
                    // kelime uc harfli (kal, sel) ya da uc onceki harf sesli (kanat),
                    if (boy == 3 || ucOncekiHarf.sesliMi())
                        return 3;

                    //kelime dort harfli ise yukaridaki kurallari gecmesi nedeniyle hecelenemez sayiyoruz.
                    // ornegin tren strateji krank angstrom kelimelerii hecelenemez sayiyoruz.
                    if (boy == 4)
                        return -1;

                    TurkceHarf dortOncekiHarf = kelime.harf(boy - 5);
                    if (!dortOncekiHarf.sesliMi())
                        return 3;
                    return 3;
                }
                else
                {


                        if (boy == 2 || !ikiOncekiHarf.sesliMi())
                            return -1;
                        TurkceHarf ucOncekiHarf = kelime.harf(boy - 4);
                        if (boy > 3 && !ucOncekiHarf.sesliMi())
                            return 4;
                        return 3;

                }
            }

        }
    }
}
