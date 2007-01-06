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


namespace NZemberek.TrTurkcesi.Yapi
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
        if (sonSesli.InceSesli && sonSesli.DuzSesli)
            return HARF_i;
        if (!sonSesli.InceSesli && sonSesli.DuzSesli)
            return HARF_ii;
        if (!sonSesli.InceSesli && sonSesli.YuvarlakSesli)
            return HARF_u;
        if (sonSesli.InceSesli && sonSesli.YuvarlakSesli)
            return HARF_uu;
        return Alfabe.TANIMSIZ_HARF;
    }

    public TurkceHarf sesliBelirleAE(HarfDizisi dizi) {
        return sesliBelirleAE(dizi.sonSesli());
    }

    public  TurkceHarf sesliBelirleAE(TurkceHarf sonSesli) {
        if (sonSesli.InceSesli)
            return HARF_e;
        else
            return HARF_a;
    }    
}
}
