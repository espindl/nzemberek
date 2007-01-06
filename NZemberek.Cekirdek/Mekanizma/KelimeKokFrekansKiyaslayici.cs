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

namespace NZemberek.Cekirdek.Mekanizma
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
