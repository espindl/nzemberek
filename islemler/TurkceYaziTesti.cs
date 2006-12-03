using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using log4net;
using net.zemberek.islemler.cozumleme;
using net.zemberek.araclar.turkce;

namespace net.zemberek.islemler
{
public class TurkceYaziTesti {
    private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);		
    KelimeCozumleyici cozumleyici, asciiCozumleyici;
    public static int HIC = 0;
    public static int AZ = 1;
    public static int ORTA = 2;
    public static int YUKSEK = 3;
    public static int KESIN = 4;

    public TurkceYaziTesti(KelimeCozumleyici cozumleyici, KelimeCozumleyici asciiCozumleyici) {
        this.cozumleyici = cozumleyici;
        this.asciiCozumleyici = asciiCozumleyici;
    }

    private double turkceOranla(String yazi) {
        int cozulenler = 0, asciiCozulenler = 0, cozulemeyenler = 0;
        IList<YaziBirimi> analizDizisi = YaziIsleyici.analizDizisiOlustur(yazi);
        for (int i = 0; i < analizDizisi.Count; i++) {
            YaziBirimi birim = (YaziBirimi) analizDizisi[i];
            if (birim.tip == YaziBirimiTipi.KELIME) {
                if (cozumleyici.denetle(birim.icerik))
                    cozulenler++;
                else if (asciiCozumleyici.denetle(birim.icerik))
                    asciiCozulenler++;
                else
                    cozulemeyenler++;
            }
        }
        int toplam = cozulenler + asciiCozulenler;
        if (toplam == 0 || (toplam + cozulemeyenler) == 0)
            return 0.0d;
        if (cozulemeyenler == 0)
            return 1.0d;
        double sonuc = 1.0d - (double) cozulemeyenler / (double) (toplam + cozulemeyenler);
        if (logger.IsInfoEnabled) logger.Info("cozulenler:" + cozulenler + "  ascii Cozulenler:" + asciiCozulenler
                    + "cozulemeyenler:" + cozulemeyenler + "  oran:" + sonuc);
        return sonuc;
    }

    public int turkceTest(String giris) {
        double sonuc = turkceOranla(giris);
        if (sonuc <= 0.1d)
            return HIC;
        if (sonuc > 0.1d && sonuc <= 0.35d)
            return AZ;
        if (sonuc > 0.35d && sonuc <= 0.65d)
            return ORTA;
        if (sonuc > 0.65d && sonuc <= 0.95d)
            return YUKSEK;
        return KESIN;
    }
}
}
