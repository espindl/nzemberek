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
            return adaylar;;
        }

        /**
         * Verilen iki string'in asciified versiyonlarÄ±nÄ± karÅŸÄ±laÅŸtÄ±rÄ±r.
         *
         * @param aday
         * @param giris
         * @return aday ve giris degerlerinin ascii karsiliklari aynÄ±ysa true, 
         * 	       deÄŸilse false. Ã–rneÄŸin:
         * <pre>
         * asciiTolaransliKarsilastir("siraci", "ÅŸÄ±racÄ±") --> true 
         * </pre>
         */
        public bool asciiTolaransliKarsilastir(String aday, String giris)
        {
            if (aday.Length > giris.Length) return false;
            String clean = agac.getAlfabe().asciifyString(aday);
            return asciiGiris.StartsWith(clean);
        }

        /**
         * AÄŸaÃ§ Ã¼zerinde  yÃ¼rÃ¼yerek ASCII toleranslÄ± karÅŸÄ±laÅŸtÄ±rma ile
         * kÃ¶k adaylarÄ±nÄ± bulur. RekÃ¼rsiftir.
         *
         * @param dugum  : baÅŸlangÄ±Ã§ dÃ¼ÄŸÃ¼mÃ¼
         * @param olusan : YÃ¼rÃ¼me sÄ±rasÄ±nda oluÅŸan kelime (dÃ¼ÄŸÃ¼mlerin karakter deÄŸerlerinden)
         */
        public void yuru(KokDugumu dugum, String olusan) 
        {
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
            if (asciiTolaransliKarsilastir(tester, giris)) {//TODO böölemi ! idi
                return;
            }
        }

        int seviye = tester.Length - 1; //TODO böölemi -1 yoktu
       if(seviye == giris.Length) return;
       // Uygun tÃ¼m alt dallarda yÃ¼rÃ¼
       foreach (KokDugumu altDugum in dugum.altDugumDizisiGetir()) {
           if (altDugum != null) {
               if (agac.getAlfabe().asciiToleransliKiyasla(altDugum.getHarf(), giris[seviye]))
                   this.yuru(altDugum, tester);
           }
       }
    }
    }
}
