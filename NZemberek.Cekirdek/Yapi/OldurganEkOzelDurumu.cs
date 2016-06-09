﻿/* ***** BEGIN LICENSE BLOCK *****
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
using NZemberek.Cekirdek.Mekanizma.Cozumleme;

namespace NZemberek.Cekirdek.Yapi
{
    public class OldurganEkOzelDurumu : EkOzelDurumu    {

    private HarfDizisi T;

    public OldurganEkOzelDurumu(Alfabe alfabe) {
        T = new HarfDizisi("t",alfabe);
    }

    public override HarfDizisi CozumlemeIcinUret(Kelime kelime, HarfDizisi giris, IHarfDizisiKiyaslayici kiyaslayici) {
        TurkceHarf son = kelime.SonHarf();
        if (son.Sesli || ((son.CharDeger=='r') || son.CharDeger==('l'))
                && kelime.Icerik.SesliSayisi() > 1) {
            return T;
        }
        return null;
    }

        public override HarfDizisi OlusumIcinUret(Kelime kelime, Ek sonrakiEk)
        {
        return CozumlemeIcinUret(kelime, null, null);
    }
}

}
