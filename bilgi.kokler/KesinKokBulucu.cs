using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;

namespace net.zemberek.bilgi.kokler
{
    /**
 * Ã‡Ã¶zÃ¼mleyicinin verilen bir kelime iÃ§in aday kÃ¶kleri bulmasÄ± iÃ§in kullanÄ±lÄ±r.
 * GiriÅŸ kelimesinin ilk harfinden baÅŸlanarak aÄŸaÃ§ta ilerlenir. ï¿½lerleyecek
 * Ä°lerlenecek yer kalmayana veya kelime bitene dek aÄŸaÃ§ta yÃ¼rÃ¼nÃ¼r, 
 * ve rastlanan tÃ¼m kÃ¶kler aday olarak toplanÄ±r.
 * 
 * Bu seÃ§ici, Balerinler kelimesi iÃ§in "bal, bale ve balerin" kÃ¶klerini taÅŸÄ±yan
 * bir liste dÃ¶ndÃ¼rÃ¼r.
 *
 * @author MDA
 */
    public class KesinKokBulucu : KokBulucu
    {
            KokAgaci agac = null;

    public KesinKokBulucu(KokAgaci agac) {
        this.agac = agac;
    }

    public List<Kok> getAdayKokler( String giris) {
        List<Kok> adaylar = new List<Kok>(3);
        int girisIndex = 0;
        KokDugumu node = agac.getKokDugumu();

        while (girisIndex < giris.Length) {
            node = node.altDugumGetir(giris[girisIndex]);
            if (node == null) break;
            if (node.getKok() != null) {
                // buradaki kodu daha basit ama biraz yavas hale getirdim.
                if (giris.StartsWith((String) node.getKelime())) {
                    node.tumKokleriEkle(adaylar);
                }
            }
            girisIndex++;
        }
        return adaylar;
    }
    }
}
