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
using System.Reflection;

namespace NZemberek.Cekirdek.KokSozlugu
{
    public class IkiliKokOkuyucu : IKokOkuyucu
    {
        private IKokOzelDurumYonetici ozelDurumlar;


        public IkiliKokOkuyucu(String pKaynakAdi, IKokOzelDurumYonetici ozelDurumlar) 
        {
            kaynakAdi = pKaynakAdi;
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
                icerik = binReader.ReadString();
            }
            catch (EndOfStreamException)
            {
                this.Kapat();
                return null;
            }

            String asil = binReader.ReadString();

            // Tip bilgisini oku (1 byte)
            int tipstr = binReader.ReadInt32();
            KelimeTipi tip = (KelimeTipi)tipstr;
            Kok kok = new Kok(icerik, tip);

            if (asil.Length != 0)
                kok.Asil = asil;

            char c = binReader.ReadChar(); // Bytes(2))[0];
            if (char.IsLetter(c))
                kok.KisaltmaSonSeslisi = c;

            // Özel durum sayısını (1 byte) ve ozel durumlari oku.
            int ozelDurumSayisi = binReader.ReadInt32();
            bool yapibozucu = false;
            for (int i = 0; i < ozelDurumSayisi; i++)
            {
                int ozelDurum = binReader.ReadInt32();
                KokOzelDurumu oz = ozelDurumlar.OzelDurum(ozelDurum);
                kok.OzelDurumEkle(oz);
                if (!yapibozucu  && oz.YapiBozucu())
                    yapibozucu = true;
            }
            kok.YapiBozucuOzelDurumVar = yapibozucu;

            int frekans = binReader.ReadInt32();
            if (frekans != 0)
            {
                kok.Frekans = frekans;
            }
            return kok;
        }

        BinaryReader binReader = null;
        string kaynakAdi = string.Empty;


        public void Ac()
        {
            //Reader açıksa hata
            if (binReader != null)
            {
                throw new ApplicationException("Kök dosyası zaten açık! : " + kaynakAdi);
            }
            //Dosya yoksa hata
            if (!KaynakMevcut(Assembly.GetCallingAssembly()))
            {
                throw new ApplicationException("Kök dosyası yok! : " + kaynakAdi);
            }
            Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream(kaynakAdi);
            binReader = new BinaryReader(stream,Encoding.UTF8);
            //binReader = new BinaryReader(File.Open(dosyaAdi, FileMode.Open),Encoding.UTF8);
            //Dosya boşsa hata 
            if (binReader.PeekChar() == -1)
            {
                binReader.Close();
                binReader = null;
                throw new ApplicationException("Kök dosyası boş! : " + kaynakAdi);
            }
        }

        private bool KaynakMevcut(Assembly assembly)
        {
            foreach (string s in assembly.GetManifestResourceNames())
            {
                if (s == kaynakAdi)
                {
                    return true;
                }
            }
            return false;
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
