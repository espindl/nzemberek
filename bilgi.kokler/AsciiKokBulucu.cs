using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;
using log4net;

    
namespace net.zemberek.bilgi.kokler
{
    public class AsciiKokBulucu : KokBulucu
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        KokAgaci agac = null;
        private int walkCount = 0;
        private String giris;
        private String asciiGiris;
        private List<Kok> adaylar = null;

        public AsciiKokBulucu(KokAgaci agac)
        {
            this.agac = agac;
        }

        public int getYurumeSayisi() {
            return walkCount;
        }

        public List<Kok> getAdayKokler(String giris)
        {
            this.giris = giris;
            asciiGiris = agac.getAlfabe().asciifyString(giris);
            adaylar = new List<Kok>(4);
            yuru(agac.getKokDugumu(), "");
            return adaylar;
        }

    /**
     * Verilen iki string'in asciified versiyonlarını karşılaştırır.
     *
     * @param aday
     * @param giris
     * @return aday ve giris degerlerinin ascii karsiliklari aynıysa true, 
     * 	       değilse false. Örneğin:
     * <pre>
     * asciiTolaransliKarsilastir("siraci", "şıracı") --> true 
     * </pre>
     */
    public bool asciiTolaransliKarsilastir(String aday, String giris) {
        if (aday.Length > giris.Length) return false;
        String clean = agac.getAlfabe().asciifyString(aday);
        return asciiGiris.StartsWith(clean);
    }

    /**
     * Ağaç üzerinde  yürüyerek ASCII toleranslı karşılaştırma ile
     * kÃ¶k adaylarını bulur. Rekürsiftir.
     *
     * @param dugum  : başlangıç düğümü
     * @param olusan : Yürüme sırasında oluşan kelime (düğümlerin karakter değerlerinden)
     */
    public void yuru(KokDugumu dugum, String olusan) {
        String tester = (olusan + dugum.getHarf()).Trim();
        walkCount++;
        if (dugum.getKok() != null) {
            if (logger.IsInfoEnabled) logger.Info("Kok : " + dugum.getKelime());
            if (asciiTolaransliKarsilastir((String) dugum.getKelime(), giris)) {
                // Aday kok bulundu.
                dugum.tumKokleriEkle(adaylar);
            } else {
                return;
            }
        } else {
            if (!asciiTolaransliKarsilastir(tester, giris)) {
                return;
            }
        }

       int seviye = tester.Length;
       if(seviye == giris.Length) return;
       // Uygun tüm alt dallarda yürü
       foreach(KokDugumu altDugum in dugum.altDugumDizisiGetir()) 
       {
           if (altDugum != null) {
               if (agac.getAlfabe().asciiToleransliKiyasla(altDugum.getHarf(), giris[seviye]))
                   this.yuru(altDugum, tester);
           }
       }
    }
    }
}
