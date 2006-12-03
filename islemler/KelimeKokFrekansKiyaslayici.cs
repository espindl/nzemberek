using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;

namespace net.zemberek.islemler
{
/**
 * iki kelimeyi kok kullanim frekansina gore kiyaslar. Sonucta o1 frekansi yuksek ise NEGATIF
 * aksi halde pozitif doner. azalan siralamada kullanilir.
 * User: ahmet
 * Date: Dec 10, 2005
 */
public class KelimeKokFrekansKiyaslayici : Comparer<Kelime> {

    public override int Compare(Kelime o1, Kelime o2) {
        if (o1 == null || o2 == null) return -1;
        Kok k1 = o1.kok();
        Kok k2 = o2.kok();
        return k2.Frekans - k1.Frekans;
    }
}
}
