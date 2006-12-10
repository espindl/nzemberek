using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using net.zemberek.bilgi;
using net.zemberek.yapi;
using net.zemberek.yapi.kok;

namespace net.zemberek.bilgi.araclar
{
    public class IkiliKokOkuyucu : KokOkuyucu
    {
        private KokOzelDurumBilgisi ozelDurumlar;


        public IkiliKokOkuyucu(String pDosyaAdi, KokOzelDurumBilgisi ozelDurumlar) 
        {
            dosyaAdi = pDosyaAdi;
            this.ozelDurumlar = ozelDurumlar;
        }

        public List<Kok> hepsiniOku() {
            List<Kok> list = new List<Kok>();
            Kok kok;
            while ((kok = oku()) != null) 
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
        public Kok oku() 
        {
            String icerik=string.Empty;
            //kok icerigini oku. eger dosya sonuna gelinmisse (EndOfStreamException) null dondur.
            try
            {
                int len = binReader.ReadByte() * 255 + binReader.ReadByte();
                icerik = Encoding.UTF8.GetString(binReader.ReadBytes(len));
            }
            catch (EndOfStreamException e)
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
