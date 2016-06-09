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
using System.Collections.Generic;
using System.IO;
using NZemberek.Cekirdek.Yapi;


namespace NZemberek.DilAraclari.KokSozlugu
{
    public class IkiliKokYazici : IKokYazici
    {
        BinaryWriter binaryWriter;

        public IkiliKokYazici(String dosyaAdi)
        {
            FileStream fileStream = new FileStream(dosyaAdi, FileMode.Create); //TODO mı Append mi
            binaryWriter = new BinaryWriter(fileStream,System.Text.Encoding.UTF8);
        }

        //kok.içerik
        //kok.asil(yoksa "")
        //kok.tip (int)
        //kok.KisaltmaSonSeslisi (kok tipi kisaltmaysa)
        //ozeldurum sayısı
        //ozeldurumindeksleri(sırayla)
        //frekans
        public void yaz(List<Kok> kokler)
        {
            foreach (Kok kok in kokler)
            {
                // Kök içerigi
                binaryWriter.Write(kok.Icerik);

                // asil icerik ozel karakterler barindiran koklerde olur. yoksa bos string yaz.
                if (kok.Asil != null)
                {
                    // Kök asil içerigi
                    binaryWriter.Write(kok.Asil);
                }
                else
                {
                    binaryWriter.Write("");
                }

                // Kök tipi
                binaryWriter.Write((int)kok.Tip);

                if (kok.GetType() == typeof(Kisaltma) )
                {
                    binaryWriter.Write(((Kisaltma)kok).KisaltmaSonSeslisi); 
                }
                else
                {
                   binaryWriter.Write('#'); 
                }

                List<KokOzelDurumu> ozd = kok.KokOzelDurumlariGetir();
                binaryWriter.Write(ozd.Count);
                foreach (KokOzelDurumu s in ozd)
                {
                    //KokOzelDurumu ozelDurum = null;
                    binaryWriter.Write(s.Indeks);
                }
                // kullanim frekansi
                binaryWriter.Write(kok.Frekans);

            }
            binaryWriter.Close();
        }
    }
}