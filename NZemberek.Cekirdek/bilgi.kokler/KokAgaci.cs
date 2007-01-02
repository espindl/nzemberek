/* ***** BEGIN LICENSE BLOCK *****
 * Version: MPL 1.1
 *
 * The contents of this file are subject to the Mozilla Public License Version
 * 1.1 (the "License"); you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * The Original Code is Zemberek Doðal Dil Ýþleme Kütüphanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Akýn, Mehmet D. Akýn.
 * Portions created by the Initial Developer are Copyright (C) 2006
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Mert Derman
 *   Tankut Tekeli
 * ***** END LICENSE BLOCK ***** */

//V 0.1

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
            KokDugumu node = baslangicDugumu;
            KokDugumu oncekiDugum = null;
            int idx = 0;
            // null alt düðüm bulana dek veya kelimenin sonuna dek alt düðümlerde ilerle
            while (idx < icerik.Length)
            {
                oncekiDugum = node;
                node = node.altDugumGetir(icerik[idx]);
                if (node == null) break;
                idx++;
            }
            /**
             * Aðaç üzerinde ilerlerken kelimemizin sonuna kadar gitmiþiz.
             * kelimemizi bu düðüme ekleriz.
             */
            if (idx == icerik.Length)
            {
                SonHarfDugumuneEkle(icerik, kok, node, idx);
                return;
            }


            /**
             * Kaldýðýmýz düðüme baðlý bir kök yoksa bu kök için bir düðüm oluþturup ekleriz.
             */
            if (oncekiDugum.getKok() == null && idx < icerik.Length)
            {
                oncekiDugum.DugumEkle(icerik[idx], icerik, kok);
                return;
            }

            if (oncekiDugum.Kelime.Equals(icerik))
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
            string oncekiDugumIcerigi = oncekiDugum.Kelime;
            KokDugumu newNode = oncekiDugum;

            if (idx == oncekiDugumIcerigi.Length)
            {
                newNode.DugumEkle(icerik[idx], icerik, kok);
                return;
            }

            if (oncekiDugumIcerigi.Length <= icerik.Length)
            {
                while (idx < oncekiDugumIcerigi.Length && oncekiDugumIcerigi[idx] == icerik[idx])
                {
                    newNode = newNode.DugumEkle(oncekiDugumIcerigi[idx]);
                    idx++;
                }

                // Kisa dugumun eklenmesi.
                if (idx < oncekiDugumIcerigi.Length)
                {
                    //newNode.KokuDallandir();
                    KokDugumu temp = newNode.DugumEkle(oncekiDugumIcerigi[idx]);
                    temp.kopyala(oncekiDugum);
                }
                else
                {
                    newNode.kopyala(oncekiDugum);
                }

                // Uzun olan dugumun (yeni gelen) eklenmesi, es anlamlilari kotar
                newNode.DugumEkle(icerik[idx], icerik, kok);
                oncekiDugum.temizle();
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
                while (idx < icerik.Length && icerik[idx] == oncekiDugumIcerigi[idx])
                {
                    newNode = newNode.DugumEkle(icerik[idx]);
                    idx++;
                }
                // Kisa dugumun eklenmesi.
                if (idx < icerik.Length)
                {
                    newNode.DugumEkle(icerik[idx], icerik, kok);
                }
                else
                {
                    newNode.kokEkle(kok);
                    newNode.Kelime = icerik;
                }

                // Uzun olan dugumun (yeni gelen) eklenmesi.
                newNode = newNode.DugumEkle(oncekiDugumIcerigi[idx]);
                newNode.kopyala(oncekiDugum);
                // Es seslileri tasi.
                oncekiDugum.temizle();
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
                if (node.Kelime != null && node.Kelime.Equals(str))
                {
                    break;
                }
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


        /// <summary>
        /// Aðaç üzerinde ilerlerken kelimemizin sonuna kadar gitmiþiz.
        /// Kelimemizi bu düðüme ekleriz.
        /// Örneðin
        /// i-s-t-->istim þeklindeki dala "is" kelimesini eklemek gibi.
        /// i-s-t-->istim
        ///   |-->is
        /// 
        /// veya
        /// 
        /// i-s-->istim e is gelirse de
        /// i-s-t-->istim
        ///   |-->is
        /// 
        /// i-s-->is  e "is" gelirse
        /// i-s-->is(2) olmalý.
        /// </summary>
        /// <param name="icerik"></param>
        /// <param name="kok"></param>
        /// <param name="node"></param>
        private static void SonHarfDugumuneEkle(String icerik, Kok kok, KokDugumu node, int idx)
        {
            if (node.altDugumVarMi())
            {
                node.kokEkle(kok);
                node.Kelime = icerik;
            }
            // Eþ sesli!
            else if (node.Kelime.Equals(icerik))
            {
                node.kokEkle(kok);
            }
            else if (node.getKok() != null)
            {
                node.KokuDallandir();
                node.kokEkle(kok);
                node.Kelime = icerik;
            }
        }

    }

}



