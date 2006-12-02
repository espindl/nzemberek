using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;
using net.zemberek.yapi.kok;

namespace net.zemberek.tr.yapi.kok
{
    public class YumusamaNk : HarfDizisiIslemi
    {
        
    private HarfDizisi NK;
    private Alfabe alfabe;


    public YumusamaNk(Alfabe alfabe) {
        this.alfabe = alfabe;
        NK = new HarfDizisi("nk", alfabe);
    }

    public void uygula(HarfDizisi dizi) {
        if (dizi.aradanKiyasla(dizi.Length - 2, NK))
            dizi.harfDegistir(dizi.Length - 1, alfabe.harf('g'));
    }
    }
}
