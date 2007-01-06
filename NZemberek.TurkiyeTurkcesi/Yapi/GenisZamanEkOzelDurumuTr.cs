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
using NZemberek.TrTurkcesi.Yapi;

namespace NZemberek.TrTurkcesi.Yapi
{
/**
 * Genis zaman ozel durumu. Normalde fiillere genis zaman eki eklenedeginde "ir, ur, Ur, Ir"
 * olusur. Ancak
 * a)fiil tek heceli (bunun da istisnalari var)
 * b)fiil tek heceli fiil eklenmesi ile olusmus bilesik fiilse (addetmek gibi)
 * "ar er" olusur
 * <p/>
 * User: ahmet
 * Date: Aug 15, 2005
 */
public class GenisZamanEkOzelDurumuTr : EkOzelDurumu {

    public GenisZamanEkOzelDurumuTr() {
    }

    public override HarfDizisi cozumlemeIcinUret(Kelime kelime, HarfDizisi giris, HarfDizisiKiyaslayici kiyaslayici) {
        if (kelime.sonEk().ad().Equals(TurkceEkAdlari.FIIL_KOK)
             && kelime.kok().ozelDurumIceriyormu(TurkceKokOzelDurumTipi.GENIS_ZAMAN))
            return ekUretici.cozumlemeIcinEkUret(kelime.icerik(), giris, uretimBilesenleri());
        else
            return null;
    }

    public override HarfDizisi olusumIcinUret(Kelime kelime, Ek sonrakiEk)
    {
        return cozumlemeIcinUret(kelime, null, null);
    }
}
}
