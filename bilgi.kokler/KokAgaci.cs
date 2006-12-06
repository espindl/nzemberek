using System;
using System.Collections.Generic;
using System.Text;

using net.zemberek.yapi;
using log4net;


namespace net.zemberek.bilgi.kokler
{
    /*
    * Created on 28.Eyl.2004
    * 
    * Kök aðacý zemberek sisteminin temel veri taþýyýcýlarýndan biridir. Kök 
    * sözlüðünden okunan tüm kökler bu aðaca yerleþtirilirler. Aðacýn oluþumundan 
    * AgacSozluk sýnýfý sorumludur.
    * Kök aðacý kompakt DAWG (Directed Acyclic Word Graph) ve Patricia tree benzeri
    * bir yapýya sahiptir. Aðaca eklenen her kök harflerine göre bir aðaç oluþturacak
    * þekilde yerleþtirilir. Bir kökü bulmak için aðacýn baþýndan itibaren kökü 
    * oluþturan harfleri temsil eden düðümleri izlemek yeterlidir. 
    * Eðer bir kökü ararken eriþmek istediðimiz harfe ait bir alt düðüme
    * gidemiyorsak kök aðaçta yok demektir.
    * <p/>
    * Aðacýn bir özelliði de boþuna düðüm oluþturmamasýdýr. Eðer bir kökün altýnda
    * baþka bir kök olmayacaksa tüm harfleri için ayrý ayrý deðil, sadece gerektiði
    * kadar düðüm oluþturulur.
    * <p/>
    * Kod içerisinde hangi durumda nasýl düðüm oluþturulduðu detaylarýyla 
    * anlatýlmýþtýr.
    *
    * @author MDA
    */
    public class KokAgaci
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private KokDugumu baslangicDugumu = null;
        private int nodeCount = 0;
        private Alfabe alfabe;

        public KokAgaci(KokDugumu baslangicDugumu, Alfabe alfabe)
        {
            this.baslangicDugumu = baslangicDugumu;
            this.alfabe = alfabe;
        }

        public KokDugumu getKokDugumu()
        {
            return baslangicDugumu;
        }

        public Alfabe getAlfabe()
        {
            return alfabe;
        }

        public int getNodeCount()
        {
            return nodeCount;
        }

        /**
         * Verilen kök icerigini aðaca ekler.
         *
         * @param icerik
         * @param kok
         */
        public void ekle(String icerik, Kok kok)
        {
            //System.out.println("Kelime: " + icerik);
            char[] hd = icerik.ToCharArray();
            KokDugumu node = baslangicDugumu;
            KokDugumu oncekiDugum = null;
            int idx = 0;
            // null alt düðüm bulana dek veya kelimenin sonuna dek alt düðümlerde ilerle
            while (idx < hd.Length)
            {
                oncekiDugum = node;
                node = node.altDugumGetir(hd[idx]);
                if (node == null) break;
                idx++;
            }
            /**
             * Aðaç üzerinde ilerlerken kelimemizin sonuna kadar gitmiþiz.
             * kelimemizi bu düðüme ekleriz.
             * Örneðin
             * i-s-t-->istim þeklindeki dala "is" kelimesini eklemek gibi.
             * i-s-t-->istim
             *   |-->is
             *
             * veya
             *
             * i-s-->istim e is gelirse de
             * i-s-t-->istim
             *   |-->is
             *
             * i-s-->is  e "is" gelirse
             * i-s-->is(2) olmalý.
             *
             */
            if (idx == hd.Length)
            {
                if (node.altDugumVarMi())
                {
                    node.kokEkle(kok);
                    node.setKelime((IEnumerable<char>)icerik);
                }
                // Eþ sesli!
                else if (node.getKelime().Equals(icerik))
                {
                    node.kokEkle(kok);
                    return;
                }
                else if (node.getKok() != null)
                {
                    //TODO : Burada charenumerable'dan son chari oldukça kazma bir yöntemle alýyoruz. Bunu incelemek lazým.
                    KokDugumu aNewNode = node.addNode(new KokDugumu(node.getKelime().ToString().ToCharArray()[idx]));
                    aNewNode.kopyala(node);
                    node.temizle();
                    node.kokEkle(kok);
                    node.setKelime(icerik);
                }
                return;

                
            }

            /**
             * Kaldýðýmýz düðüme baðlý bir kök yoksa bu kök için bir düðüm oluþturup ekleriz.
             */
            if (oncekiDugum.getKok() == null && idx < hd.Length)
            {
                oncekiDugum.addNode(new KokDugumu(hd[idx], icerik, kok));
                return;
            }

            if (oncekiDugum.getKelime().Equals(icerik))
            {
                oncekiDugum.kokEkle(kok);
                return;
            }

            /**
             * Düðümde duran "istimlak" ve gelen kök = "istimbot" için,
             * i-s-t-i-m
             * e kadar ilerler. daha sonra "istimlak" için "l" düðümünü oluþturup kökü baðlar
             * i-s-t-i-m-l-->istimlak
             * sonra da diðer düðüm için "b" düðümünü oluþturup gene "m" düðümüne baðlar
             * i-s-t-i-m-l-->istimlak
             *         |-b-->istimbot
             *
             * Eðer istimlak düðümü baðlanmýþsa ve "istim" düðümü eklenmek üzere 
             * elimize gelmiþe
             * i-s-t-i-m-l-->istimlak
             * tan sonra istim, "m" düðümüne doðrudan baðlanýr.
             * i-s-t-i-m-l-->istimlak
             *         |-->istim
             *
             */
            char[] nodeHd = ((String)oncekiDugum.getKelime()).ToCharArray();
            //char[] nodeChars = ((String) oncekiDugum.getKelime()).toCharArray();
            KokDugumu newNode = oncekiDugum;

            if (idx == nodeHd.Length)
            {
                newNode.addNode(new KokDugumu(hd[idx], icerik, kok));
                return;
            }

            //TODO : Ayný kazmalýk, kelimenin boyunu alýrken, hepsi CharSequence yüzünden
            if (oncekiDugum.getKelime().ToString().Length == idx)
            {
                newNode.addNode(new KokDugumu(hd[idx], icerik, kok));
                return;
            }

            if (nodeHd.Length <= hd.Length)
            {
                while (idx < nodeHd.Length && nodeHd[idx] == hd[idx])
                {
                    newNode = newNode.addNode(new KokDugumu(nodeHd[idx]));
                    idx++;
                }

                // Kisa dugumun eklenmesi.
                if (idx < nodeHd.Length)
                {
                    KokDugumu temp = newNode.addNode(new KokDugumu(nodeHd[idx]));
                    temp.kopyala(oncekiDugum);
                }
                else
                {
                    newNode.kopyala(oncekiDugum);
                }

                // Uzun olan dugumun (yeni gelen) eklenmesi, es anlamlilari kotar
                newNode.addNode(new KokDugumu(hd[idx], icerik, kok));
                oncekiDugum.temizle();
                return;
            }

            /**
             *
             * Eðer köke önce "istimlak" ve sonra "istifa" gelirse
             * i-s-t-i-m-l-->istimlak
             * daha sonra gene son ortak harf olan "i" ye "f" karakterli düðümü
             * oluþturup istifayý baðlar
             * istimlak ta "m" düðümüne baðlý kalýr.
             * i-s-t-i-m-->istimlak
             *       |-f-->istifa
             *
             */

            else
            {
                while (idx < hd.Length && hd[idx] == nodeHd[idx])
                {
                    newNode = newNode.addNode(new KokDugumu(hd[idx]));
                    idx++;
                }
                // Kisa dugumun eklenmesi.
                if (idx < hd.Length)
                {
                    newNode.addNode(new KokDugumu(hd[idx], icerik, kok));
                }
                else
                {
                    newNode.kokEkle(kok);
                    newNode.setKelime(icerik);
                }

                // Uzun olan dugumun (yeni gelen) eklenmesi.
                newNode = newNode.addNode(new KokDugumu(nodeHd[idx]));
                newNode.kopyala(oncekiDugum);
                // Es seslileri tasi.
                oncekiDugum.temizle();
                return;
            }
        }

        /**
         * Aranan bir kök düðümünü bulur.
         *
         * @param str
         * @return Aranan kök ve eþ seslilerini taþýyan liste, bulunamazsa null.
         */
        public List<Kok> bul(String str)
        {
            char[] girisChars = str.ToCharArray();
            int girisIndex = 0;
            // Basit bir tree traverse algoritmasý ile kelime bulunur.
            KokDugumu node = baslangicDugumu;
            while (node != null && girisIndex < girisChars.Length)
            {
                if (node.getKelime() != null && node.getKelime().Equals(str))
                {
                    break;
                }
                if (logger.IsInfoEnabled)
                    logger.Info("Harf: " + node.getHarf() + " Taranan Kelime: " + node.getKelime());
                node = node.altDugumGetir(girisChars[girisIndex++]);
            }
            if (node != null)
            {
                return node.tumKokleriGetir();
            }
            return null;
        }

        public override String ToString()
        {
            return baslangicDugumu.ToString();
        }

    }

}



