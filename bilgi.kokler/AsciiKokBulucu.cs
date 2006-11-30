using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;

namespace net.zemberek.bilgi.kokler
{
    public class AsciiKokBulucu : KokBulucu
    {
    //    private static Logger log = Logger.getLogger(AsciiKokBulucu.class.getName());
        KokAgaci agac = null;
    //private int walkCount = 0;
    //private String giris;
    //private String asciiGiris;
    //private List<Kok> adaylar = null;

        public AsciiKokBulucu(KokAgaci agac)
        {
            this.agac = agac;
        }

    //public int getYurumeSayisi() {
    //    return walkCount;
    //}

        public List<Kok> getAdayKokler(String giris)
        {
            //this.giris = giris;
            //asciiGiris = agac.getAlfabe().asciifyString(giris);
            //adaylar = new ArrayList<Kok>(4);
            //yuru(agac.getKokDugumu(), "");
            //return adaylar;
            return null;
        }

    ///**
    // * Verilen iki string'in asciified versiyonlarÄ±nÄ± karÅŸÄ±laÅŸtÄ±rÄ±r.
    // *
    // * @param aday
    // * @param giris
    // * @return aday ve giris degerlerinin ascii karsiliklari aynÄ±ysa true, 
    // * 	       deÄŸilse false. Ã–rneÄŸin:
    // * <pre>
    // * asciiTolaransliKarsilastir("siraci", "ÅŸÄ±racÄ±") --> true 
    // * </pre>
    // */
    //public boolean asciiTolaransliKarsilastir(String aday, String giris) {
    //    if (aday.length() > giris.length()) return false;
    //    String clean = agac.getAlfabe().asciifyString(aday);
    //    return asciiGiris.startsWith(clean);
    //}

    ///**
    // * AÄŸaÃ§ Ã¼zerinde  yÃ¼rÃ¼yerek ASCII toleranslÄ± karÅŸÄ±laÅŸtÄ±rma ile
    // * kÃ¶k adaylarÄ±nÄ± bulur. RekÃ¼rsiftir.
    // *
    // * @param dugum  : baÅŸlangÄ±Ã§ dÃ¼ÄŸÃ¼mÃ¼
    // * @param olusan : YÃ¼rÃ¼me sÄ±rasÄ±nda oluÅŸan kelime (dÃ¼ÄŸÃ¼mlerin karakter deÄŸerlerinden)
    // */
    //public void yuru(KokDugumu dugum, String olusan) {
    //    String tester = (olusan + dugum.getHarf()).trim();
    //    walkCount++;
    //    if (dugum.getKok() != null) {
    //        if (log.isLoggable(Level.FINEST)) log.finest("Kok : " + dugum.getKelime());
    //        if (asciiTolaransliKarsilastir((String) dugum.getKelime(), giris)) {
    //            // Aday kok bulundu.
    //            dugum.tumKokleriEkle(adaylar);
    //        } else {
    //            return;
    //        }
    //    } else {
    //        if (!asciiTolaransliKarsilastir(tester, giris)) {
    //            return;
    //        }
    //    }

    //   int seviye = tester.length();
    //   if(seviye == giris.length()) return;
    //   // Uygun tÃ¼m alt dallarda yÃ¼rÃ¼
    //   for (KokDugumu altDugum : dugum.altDugumDizisiGetir()) {
    //       if (altDugum != null) {
    //           if (agac.getAlfabe().asciiToleransliKiyasla(altDugum.getHarf(), giris.charAt(seviye)))
    //               this.yuru(altDugum, tester);
    //       }
    //   }
    //}
    }
}
