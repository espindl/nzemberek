using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;
using net.zemberek.yapi.ek;

namespace net.zemberek.tr.yapi
{
public class TurkceSesliUretici : SesliUretici {

    public TurkceHarf HARF_a;
    public TurkceHarf HARF_e;
    public TurkceHarf HARF_i;
    public TurkceHarf HARF_ii;
    public TurkceHarf HARF_u;
    public TurkceHarf HARF_uu;

    public TurkceSesliUretici(Alfabe alfabe) {
        HARF_a = alfabe.harf('a');
        HARF_e = alfabe.harf('e');
        HARF_i = alfabe.harf('i');
        HARF_ii = alfabe.harf(Alfabe.CHAR_ii);
        HARF_u = alfabe.harf('u');
        HARF_uu = alfabe.harf(Alfabe.CHAR_uu);
    }

    public TurkceHarf sesliBelirleIU(HarfDizisi dizi) {
        TurkceHarf sonSesli = dizi.sonSesli();
        return sesliBelirleIU(sonSesli);
    }

    public TurkceHarf sesliBelirleIU(TurkceHarf sonSesli) {
        if (sonSesli.inceSesliMi() && sonSesli.duzSesliMi())
            return HARF_i;
        if (!sonSesli.inceSesliMi() && sonSesli.duzSesliMi())
            return HARF_ii;
        if (!sonSesli.inceSesliMi() && sonSesli.yuvarlakSesliMi())
            return HARF_u;
        if (sonSesli.inceSesliMi() && sonSesli.yuvarlakSesliMi())
            return HARF_uu;
        return Alfabe.TANIMSIZ_HARF;
    }

    public TurkceHarf sesliBelirleAE(HarfDizisi dizi) {
        return sesliBelirleAE(dizi.sonSesli());
    }

    public  TurkceHarf sesliBelirleAE(TurkceHarf sonSesli) {
        if (sonSesli.inceSesliMi())
            return HARF_e;
        else
            return HARF_a;
    }    
}
}
