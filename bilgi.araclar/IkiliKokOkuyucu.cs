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
        //TODO MERT java classından bakarak buraları duzelt
        private  Stream dis;
        private KokOzelDurumBilgisi ozelDurumlar;

    public IkiliKokOkuyucu(Stream ist, KokOzelDurumBilgisi ozelDurumlar) {
        dis = new BufferedStream(ist);
        this.ozelDurumlar = ozelDurumlar;
    }

    public IkiliKokOkuyucu(String dosyaAdi, KokOzelDurumBilgisi ozelDurumlar) {
        //TODO MERT
        //Stream fis = new KaynakYukleyici("UTF-8").getStream(dosyaAdi);
        //dis = new BufferedStream(fis);
        //this.ozelDurumlar = ozelDurumlar;
    }

    /**
     * SÃ¶zlÃ¼kteki TÃ¼m kÃ¶kleri okur ve bir ArrayList olarak dÃ¶ndÃ¼rÃ¼r.
     */
    public List<Kok> hepsiniOku() {
        List<Kok> list = new List<Kok>();
        Kok kok;
        while ((kok = oku()) != null) {
            list.Add(kok);
        }
        dis.Close();
        return list;
    }

    /**
     * Ä°kili (Binary) sÃ¶zlÃ¼kten bir kÃ¶k okur. Ã§aÄŸrÄ±ldÄ±kÃ§a bir sonraki kÃ¶kÃ¼ alÄ±r.
     *
     * @return bir sonraki kÃ¶k. EÄŸer okunacak kÃ¶k kalmamÄ±ÅŸsa null
     */
    public Kok oku() {
        //TODO MERT
        //String icerik;
        ////kok icerigini oku. eger dosya sonuna gelinmisse (EOFException) null dondur.
        //try {
        //    icerik = dis.readUTF();
        //} catch (EndOfStreamException e) {
        //    dis.Close();
        //    return null;
        //}
        //String asil = dis.readUTF();

        //// Tip bilgisini oku (1 byte)
        //KelimeTipi tip = KelimeTipi.Tip(dis.Read());
        //Kok kok = new Kok(icerik, tip);

        //if (asil.Length != 0)
        //    kok.Asil =asil;

        //kok.KisaltmaSonSeslisi=dis.readChar();

        //// Ã–zel durum sayÄ±sÄ±nÄ± (1 byte) ve ozel durumlari oku.
        //int ozelDurumSayisi = dis.Read();
        //for (int i = 0; i < ozelDurumSayisi; i++) {
        //    int ozelDurum = dis.Read();
        //    KokOzelDurumu oz = ozelDurumlar.ozelDurum(ozelDurum);
        //    kok.ozelDurumEkle(oz);
        //}
        //int frekans = dis.Read();
        //if (frekans != 0) {
        //    kok.Frekans = frekans ;
        //}
        //return kok;
        return null;
    }
    }
}
