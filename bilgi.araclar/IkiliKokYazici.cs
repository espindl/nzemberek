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
            // KÃ¶k iÃ§erigi
            dos.Write(kok.icerik());

            // asil icerik ozel karakterler barindiran koklerde olur. yoksa bos string yaz.
            if (kok.asil() != null) {
                // KÃ¶k asil iÃ§erigi
                dos.Write(kok.asil());
            } else
                dos.Write("");

            // KÃ¶k tipi
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
