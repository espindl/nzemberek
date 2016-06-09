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
using NZemberek.Cekirdek.Mekanizma.Cozumleme;

namespace NZemberek.TrTurkcesi.Yapi
{
/**
 * -is fiil eki tek heceli koklere eklendiginde "-yis" cok heceli koke eklendiginde ise
 * "-is" seklinde olusur. "tak-IS-mak" "ye-yis-mek"
 * User: ahmet
 * Date: Sep 11, 2005
 */
public class BeraberlikIsOzelDurumu : EkOzelDurumu {

    public override HarfDizisi CozumlemeIcinUret(Kelime kelime, HarfDizisi giris, IHarfDizisiKiyaslayici kiyaslayici) {
        if(kelime.Icerik.SesliSayisi()<2)
          return EkUretici.CozumlemeIcinEkUret(kelime.Icerik, giris, UretimBilesenleri);
        else
          return null;
    }

    public override HarfDizisi OlusumIcinUret(Kelime kelime, Ek sonrakiEk)
    {
        return CozumlemeIcinUret(kelime, null, null);
    }
}
}
