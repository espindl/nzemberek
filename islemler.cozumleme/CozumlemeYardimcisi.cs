using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;

namespace net.zemberek.islemler.cozumleme
{
    interface CozumlemeYardimcisi
    {
        /**
 * kelimenin icindeki olusumu kok'un orjinal haline gore ve gerekli noktalama isaretlerine gore
 * bicimlendirir. Ornegin Turkiye turkcesinde "ankaraya" -> "Ankara'ya" ve "bbceye"->"BBC'ye" seklinde.
 * Bu metod ozellikle oneri mekanizmasinda kullaniliyor.
 * @param kelime : cozumleme sonrasi olusan kelime.
 */
        void kelimeBicimlendir(Kelime kelime);

        /**
         * eger kok ozel karaterler iceriyorsa bunun giris ile olan uygunlugunu denetler.
         * @param kelime
         * @param giris
         * @return eger kok orjinal icereigi ve kurallari girise uygunsa true.
         */
        bool kelimeBicimiDenetle(Kelime kelime, String giris);

        /**
         * Asagidaki aciklamalar Turkiye Turkcesi icindir.
         * Kisaltmalarin cozumlenmesi sirasinda karsilasilan bir sorun bazi durumlarda
         * kokun hic sesli icermemesi, ya da kisaltmanin okunusunun son sessizin okunusuna bagli olmasidir.
         * Bu durumda eklenecek ekin belirlenmesi son harfin
         * okunusu ile belirlenir. Bu durumun cozumleme islemine uygulanabilmesi icin
         * hem giris hem de kok dizisinde degisiklik yapilamsi gerekebiliyor. Bu metod sesli icermeyen
         * kok ve girise gecici sesli eklenmesi ya da baska gerekli bir ozel durum varsa uygulanmasini
         * saglar.
         * @param kokDizi
         * @param girisDizi
         */
        bool kokGirisDegismiVarsaUygula(Kok kok, HarfDizisi kokDizi, HarfDizisi girisDizi);

        /**
         * Eger dil icin denetleme cebi olusturulmussa cepte arama islemi yapilir.
         * @param str
         * @return true eger String cepte yer aliyorsa.
         */
        bool cepteAra(String str);


    }
}
