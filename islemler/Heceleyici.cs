using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;

namespace net.zemberek.islemler
{
/**
 * Heceleyici
 * User: aakin Date: Mar 6, 2004
 */
public class Heceleyici {

    private Alfabe alfabe;
    private HeceBulucu heceBulucu;

    public Heceleyici(Alfabe alfabe, HeceBulucu heceBulucu) {
        this.alfabe = alfabe;
        this.heceBulucu = heceBulucu;
    }

    /**
     * Gelen String'i turkce heceleme kurallarina gore hecelerine ayirir. Sonucta
     * heceleri bir liste icinde dondurur. Eger heceleme yapilamazsa bos liste doner.
     *
     * @param giris
     * @return sonHeceHarfSayisi String dizisi
     */
    public String[] hecele(String giris) {
        giris = alfabe.ayikla(giris);
        HarfDizisi kelime = new HarfDizisi(giris, alfabe);
        ArrayList list = new ArrayList(); //reverse kullanmak icin generics kullanmadim...
        while (kelime.Length > 0) {
            int index = heceBulucu.sonHeceHarfSayisi(kelime);
            if (index < 0) {
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
    public bool hecelenebilirmi(String giris) {
        HarfDizisi kelime = new HarfDizisi(giris, alfabe);
        while (kelime.Length > 0) {
            int index = heceBulucu.sonHeceHarfSayisi(kelime);
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
    public int[] heceIndeksleriniBul(String giris) {
        giris = alfabe.ayikla(giris);
        HarfDizisi kelime = new HarfDizisi(giris, alfabe);
        int[] tmpHeceIndeksleri = new int[50];
        int heceIndeks = 0;
        while (kelime.Length > 0) {
            int index = heceBulucu.sonHeceHarfSayisi(kelime);
            if (index < 0) {
                return null;
            }
            int basla = kelime.Length - index;
            tmpHeceIndeksleri[heceIndeks++] = basla;
            if (heceIndeks > 50) return null;
            kelime.kirp(basla);
        }
        int[] heceIndeksleri = new int[heceIndeks];
        for (int i = 0; i < heceIndeks; i++) {
            heceIndeksleri[i] = tmpHeceIndeksleri[heceIndeks - i - 1];
        }
        return heceIndeksleri;
    }
}
}
