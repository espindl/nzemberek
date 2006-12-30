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
using System.IO;
using net.zemberek.yapi;
using net.zemberek.yapi.kok;

namespace net.zemberek.bilgi.araclar
{
    public class IkiliKokYazici: KokYazici
    {
      BinaryWriter   dos;

    public IkiliKokYazici(String dosyaAdi) {
        FileStream fos = new FileStream(dosyaAdi,FileMode.Create); //TODO mı Append mi
        dos = new BinaryWriter(fos);
    }

    public void yaz(List<Kok> kokler) {

        foreach (Kok kok in kokler) {
            // Kök içerigi
            dos.Write(kok.icerik());

            // asil icerik ozel karakterler barindiran koklerde olur. yoksa bos string yaz.
            if (kok.asil() != null) {
                // Kök asil içerigi
                dos.Write(kok.asil());
            } else
                dos.Write("");

            // Kök tipi
            dos.Write(kok.tip().ToString());

            dos.Write(kok.KisaltmaSonSeslisi);

            KokOzelDurumu[] ozd = kok.ozelDurumDizisi();
            dos.Write(ozd.Length);
            foreach (KokOzelDurumu ozelDurum in ozd) {
                dos.Write(ozelDurum.indeks());
            }
            // kullanim frekansi
            dos.Write(kok.Frekans);

        }
        dos.Close();
    }
    }
}
