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
using System.IO;
using System.Text;
using NZemberek.Cekirdek.Yapi;

namespace NZemberek.Cekirdek.KokSozlugu
{
    public class IkiliKokOkuyucu : IKokOkuyucu
    {
        private KokOzelDurumBilgisi ozelDurumlar;


        public IkiliKokOkuyucu(String pDosyaAdi, KokOzelDurumBilgisi ozelDurumlar) 
        {
            dosyaAdi = pDosyaAdi;
            this.ozelDurumlar = ozelDurumlar;
        }

        public List<Kok> HepsiniOku() {
            List<Kok> list = new List<Kok>();
            Kok kok;
            while ((kok = Oku()) != null) 
            {
                list.Add(kok);
            }
            return list;
        }

        /**
         * İkili (Binary) sözlükten bir kök okur. çağrıldıkça bir sonraki kökü alır.
         *
         * @return bir sonraki kök. Eğer okunacak kök kalmamışsa null
         */
        public Kok Oku() 
        {
            String icerik=string.Empty;
            //kok icerigini oku. eger dosya sonuna gelinmisse (EndOfStreamException) null dondur.
            try
            {
                int len = binReader.ReadByte() * 255 + binReader.ReadByte();
                icerik = Encoding.UTF8.GetString(binReader.ReadBytes(len));
            }
            catch (EndOfStreamException)
            {
                this.Kapat();
                return null;
            }

            int len1 = binReader.ReadByte() * 255 + binReader.ReadByte();
            String asil = Encoding.UTF8.GetString(binReader.ReadBytes(len1));

            // Tip bilgisini oku (1 byte)
            string tipstr = binReader.ReadByte().ToString();
            KelimeTipi tip = (KelimeTipi)Enum.Parse(typeof(KelimeTipi), tipstr);
            Kok kok = new Kok(icerik, tip);

            if (asil.Length != 0)
                kok.Asil = asil;

            char c = Encoding.UTF8.GetChars(binReader.ReadBytes(2))[0];
            if (char.IsLetter(c))
                kok.KisaltmaSonSeslisi = c;

            // Özel durum sayısını (1 byte) ve ozel durumlari oku.
            int ozelDurumSayisi = binReader.ReadByte();
            for (int i = 0; i < ozelDurumSayisi; i++)
            {
                int ozelDurum = binReader.ReadByte();
                KokOzelDurumu oz = ozelDurumlar.ozelDurum(ozelDurum);
                kok.ozelDurumEkle(oz);
            }
            int frekans = binReader.ReadByte() * 255 * 255 * 255 + binReader.ReadByte() * 255 * 255
                        + binReader.ReadByte() * 255 + binReader.ReadByte();
            if (frekans != 0)
            {
                kok.Frekans = frekans;
            }
            return kok;
        }

        
        BinaryReader binReader = null;
        string dosyaAdi = string.Empty;


        public void Ac()
        {
            //Reader açıksa hata
            if (binReader != null)
            {
                throw new ApplicationException("Kök dosyası zaten açık! : " + dosyaAdi);
            }
            //Dosya yoksa hata
            if (!File.Exists(dosyaAdi))
            {
                throw new ApplicationException("Kök dosyası yok! : " + dosyaAdi);
            }
            binReader = new BinaryReader(File.Open(dosyaAdi, FileMode.Open),Encoding.UTF8);
            //Dosya boşsa hata 
            if (binReader.PeekChar() == -1)
            {
                binReader.Close();
                binReader = null;
                throw new ApplicationException("Kök dosyası boş! : " + dosyaAdi);
            }
        }
        

        public void Kapat()
        {
            try
            {
                binReader.Close();
            }
            finally
            {
                binReader = null;
            }

        }


        public void Dispose()
        {
            if (binReader != null)
            {
                binReader.Close();
                binReader = null;
            }
        }
    }
}
