using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;
using net.zemberek.araclar;

namespace net.zemberek.bilgi.kokler
{
    /**
 * Hata toleranslÄ± kÃ¶k bulucu hatalÄ± giriÅŸler iÃ§in Ã¶neri Ã¼retmekte kullanÄ±lÄ±r.
 * <p/>
 * AÄŸacÄ±n "e" harfi ile baÅŸlayan kolu aÅŸaÄŸÄ±daki gibi olsun:
 * <p/>
 * <pre>
 * e
 * |---l(el)
 * |  |---a(ela)
 * |  |  |--s-(elastik)
 * |  |
 * |  |---b
 * |  |  |--i-(elbise)
 * |  |
 * |  |---m
 * |  |  |--a(elma)
 * |  |  |  |--c-(elmacÄ±k)
 * |  |  |  |--s-(elmas)
 * |  | ...
 * | ...
 * ...
 * </pre>
 * <p/>
 * "elm" giriÅŸi iÃ§in aÄŸaÃ§ Ã¼zerinde ilerlerken hata mesafesi 1 olduÄŸu mÃ¼ddetÃ§e 
 * ilerlenir. bu sÄ±rada "el, ela, elma" kÃ¶kleri toplanÄ±r.
 *  @author MDA
 */
    public class ToleransliKokBulucu : KokBulucu
    {
        private KokAgaci agac = null;
        private int tolerans = 0;
        private int distanceCalculationCount = 0;

        public int getDistanceCalculationCount()
        {
            return distanceCalculationCount;
        }

        public ToleransliKokBulucu(KokAgaci agac, int tolerans)
        {
            this.agac = agac;
            this.tolerans = tolerans;
        }

        public List<Kok> getAdayKokler(String giris)
        {
            return benzerKokleriBul(giris);
        }

        private String giris = null;
        private List<Kok> adaylar = null;

        private List<Kok> benzerKokleriBul(String giris)
        {
            this.giris = giris;
            adaylar = new List<Kok>();
            yuru(agac.getKokDugumu(), "");
            return adaylar;
        }

        private void yuru(KokDugumu dugum, String olusan) {
        String tester = olusan;
        tester += dugum.getHarf();
        if (dugum.getKok() != null) {
            distanceCalculationCount++;
            if (MetinAraclari.isInSubstringEditDistance((String) dugum.getKelime(), giris, tolerans)) {
            	// Aday kÃ¶k bulundu
                adaylar.Add(dugum.getKok());
            } else {
                // Mesafe sÄ±nÄ±rÄ± aÅŸÄ±ldÄ±.
                return;
            }
        } else {
            if (!MetinAraclari.isInSubstringEditDistance(tester.Trim(), giris, tolerans)) {
            	// Ara stringde mesafe sÄ±nÄ±rÄ± aÅŸÄ±ldÄ±
                return;
            }
        }

        foreach (KokDugumu altDugum in dugum.altDugumDizisiGetir()) {
            if (altDugum != null) {
                this.yuru(altDugum, tester);
            }
        }
    }
    }
}
