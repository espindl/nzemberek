/*
 * Created on 06.Nis.2004
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;
using net.zemberek.yapi.kok;
using log4net;



namespace net.zemberek.bilgi.araclar
{
    /**
     * Verilen bir sözlüðün düzyazý olarak yazýlmasýný saðlar.
     * @author MDA
     */
    public class DuzYaziKokYazici : KokYazici
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        StreamWriter writer;

        public DuzYaziKokYazici(String dosyaAdi)
        {
            Stream fos = new FileStream(dosyaAdi, FileMode.Create);
            writer = new StreamWriter(fos);
        }

        #region KokYazici Members

        public void yaz(List<Kok> kokler) 
        {
            writer.Write("#-------------------------\n");
            writer.Write("# TSPELL DUZ METIN SOZLUK \n");
            writer.Write("#-------------------------\n");
            writer.Write("#v0.1\n");
            foreach (Kok kok in kokler) 
            {
                writer.Write(getDuzMetinSozlukForm(kok));
                writer.Write(writer.NewLine);
            }
            writer.Close();
        }

        #endregion

        private String getDuzMetinSozlukForm(Kok kok)
        {

        //icerik olarak icerigin varsa asil halini yoksa normal kok icerigini al.
        String icerik = kok.icerik();
        if (kok.asil() != null)
            icerik = kok.asil();

        StringBuilder res = new StringBuilder(icerik);
        res.Append(" ");
        // Tipi ekleyelim.
        if (kok.tip() == KelimeTipi.YOK) 
        {
            logger.Warn("tipsiz kok:" + kok);
            return res.ToString();
        }

        res.Append(kok.tip().ToString());
        res.Append(" ");
        res.Append(getOzellikString(kok.ozelDurumDizisi()));
        return res.ToString();
    }

        private String getOzellikString(KokOzelDurumu[] ozelDurumlar) 
        {
            String oz = "";
            foreach (KokOzelDurumu ozelDurum in ozelDurumlar) 
            {
                oz = oz + ozelDurum.kisaAd() + " ";
            }
            return oz;
        }

    }
}





