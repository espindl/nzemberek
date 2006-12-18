using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;

namespace net.zemberek.bilgi.kokler
{
    /**
     * Çözümleyicinin verilen bir kelime için aday kökleri bulması için kullanılır.
     * Giriş kelimesinin ilk harfinden başlanarak ağaçta ilerlenir.
     * İlerlenecek yer kalmayana veya kelime bitene dek ağaçta yürünür, 
     * ve rastlanan tüm kökler aday olarak toplanır.
     * 
     * Bu seçici, Balerinler kelimesi için "bal, bale ve balerin" köklerini taşıyan
     * bir liste döndürür.
     *
     * @author MDA
     */
    public class KesinKokBulucu : KokBulucu
    {
        KokAgaci agac = null;

        public KesinKokBulucu(KokAgaci agac) 
        {
            this.agac = agac;
        }

        public List<Kok> getAdayKokler( String giris) 
        {
            List<Kok> adaylar = new List<Kok>(3);
            int girisIndex = 0;
            KokDugumu node = agac.getKokDugumu();

            while (girisIndex < giris.Length) 
            {
                node = node.altDugumGetir(giris[girisIndex]);
                if (node == null) break;
                if (node.getKok() != null) 
                {
                    // buradaki kodu daha basit ama biraz yavas hale getirdim.
                    if (giris.StartsWith((String) node.getKelime())) 
                    {
                        node.tumKokleriEkle(adaylar);
                    }
                }
                girisIndex++;
            }
            return adaylar;
        }
    }
}
