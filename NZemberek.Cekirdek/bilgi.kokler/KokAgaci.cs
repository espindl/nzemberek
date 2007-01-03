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


        private KokDugumu IcerikDugumuBul(string icerik)
        {
            KokDugumu parent = baslangicDugumu;
            KokDugumu child = null;
            while (parent.Level < icerik.Length)
            {
                child = parent.altDugumGetir(icerik[parent.Level]);
                if (child != null)
                {
                    parent = child;
                }
                else
                {
                    break;
                }
            }
            return parent;
        }


        public void ekle(string icerik, Kok kok)
        {
            // Ýçerik için mevcut aðaçta inilebilecek en derin düðümü bul.
            KokDugumu dugum = this.IcerikDugumuBul(icerik);
            while (true)
            {
                // Aðaç üzerinde ilerlerken kelimemizin sonuna kadar gitmiþsek
                if (dugum.Level == icerik.Length)
                {
                    //kelimemizi bu düðüme ekleriz.
                    //örnek : içerik="istif ISIM", düðüm = f(level:5).
                    SonHarfDugumuneEkle(icerik, kok, dugum);
                    return;
                }
                // Kelimenin sonuna kadar gitmedik...
                else
                {
                    // bu düðüme baðlý bir kök yoksa 
                    if (dugum.getKok() == null)
                    {
                        //bu kök için bir düðüm oluþturup ekleriz.
                        dugum.DugumEkle(icerik, kok);
                        return;
                    }
                    // bu düðüme baðlý bir kök var...
                    else
                    {
                        // bu düðümün içeriði eldeki içerik ile ayný ise o zaman bir eþsesli bulduk.
                        if (dugum.Kelime.Equals(icerik))
                        {
                            // eþsesliyi ekleriz. (Eþsesli eklemede sadece kök ekleniyor, içerik deðiþtirilmiyor.
                            //örnek : içerik="yüz SAYI", düðüm = ü(level:2,kök="yüz FIIL").
                            dugum.kokEkle(kok);
                            return;
                        }
                        // eþsesli deðil...
                        else
                        {
                            //düðüm bir kýsayol deðilse 
                            if (!dugum.KisayolDugumu)
                            {
                                //yeni içeriði alt düðüm olarak ekleriz.
                                //örnek : içerik="istifra ISIM", düðüm = f(level:5,kök="istif ISIM").
                                dugum.DugumEkle(icerik, kok);
                                return;
                            }
                            //kýsayol düðümü
                            else
                            {
                                //öncelikle düðümdeki kökü bir ilerletiriz.
                                KokDugumu yenidugum = dugum.KokuDallandir();
                                if (yenidugum.Harf == icerik[dugum.Level])
                                {
                                    dugum = yenidugum;
                                    continue;
                                }
                                else
                                {
                                    dugum.DugumEkle(icerik, kok);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        

        /// <summary>
        /// Verilen kökü ve içeriði düðümleri oluþturarak aðaca ekler.
        /// </summary>
        /// <param name="icerik"></param>
        /// <param name="kok"></param>
        //public void ekle(String icerik, Kok kok)
        //{
        //    KokDugumu node = baslangicDugumu;
        //    KokDugumu oncekiDugum = null;
        //    int level = 0;
        //    // null alt düðüm bulana dek veya kelimenin sonuna dek alt düðümlerde ilerle
        //    while (level < icerik.Length)
        //    {
        //        oncekiDugum = node;
        //        node = node.altDugumGetir(icerik[level]);
        //        if (node == null) break;
        //        level++;
        //    }
        //    /**
        //     * Aðaç üzerinde ilerlerken kelimemizin sonuna kadar gitmiþiz.
        //     * kelimemizi bu düðüme ekleriz.
        //     */
        //    if (level == icerik.Length)
        //    {
        //        SonHarfDugumuneEkle(icerik, kok, node);
        //        return;
        //    }


        //    /**
        //     * Kaldýðýmýz düðüme baðlý bir kök yoksa bu kök için bir düðüm oluþturup ekleriz.
        //     */
        //    if (oncekiDugum.getKok() == null && level < icerik.Length)
        //    {
        //        oncekiDugum.DugumEkle(icerik, kok);
        //        return;
        //    }

        //    if (oncekiDugum.Kelime.Equals(icerik))
        //    {
        //        oncekiDugum.kokEkle(kok);
        //        return;
        //    }

        //    /**
        //     * Düðümde duran "istimlak" ve gelen kök = "istimbot" için,
        //     * i-s-t-i-m
        //     * e kadar ilerler. daha sonra "istimlak" için "l" düðümünü oluþturup kökü baðlar
        //     * i-s-t-i-m-l-->istimlak
        //     * sonra da diðer düðüm için "b" düðümünü oluþturup gene "m" düðümüne baðlar
        //     * i-s-t-i-m-l-->istimlak
        //     *         |-b-->istimbot
        //     *
        //     * Eðer istimlak düðümü baðlanmýþsa ve "istim" düðümü eklenmek üzere 
        //     * elimize gelmiþe
        //     * i-s-t-i-m-l-->istimlak
        //     * tan sonra istim, "m" düðümüne doðrudan baðlanýr.
        //     * i-s-t-i-m-l-->istimlak
        //     *         |-->istim
        //     *
        //     */
        //    string oncekiDugumIcerigi = oncekiDugum.Kelime;
        //    KokDugumu newNode = oncekiDugum;

        //    if (level == oncekiDugumIcerigi.Length)
        //    {
        //        newNode.DugumEkle(icerik, kok);
        //        return;
        //    }

        //    if (oncekiDugumIcerigi.Length <= icerik.Length)
        //    {
        //        while (level < oncekiDugumIcerigi.Length && oncekiDugumIcerigi[level] == icerik[level])
        //        {
        //            newNode = newNode.DugumEkle(oncekiDugumIcerigi[level]);
        //            level++;
        //        }

        //        // Kisa dugumun eklenmesi.
        //        if (level < oncekiDugumIcerigi.Length)
        //        {
        //            //newNode.KokuDallandir();
        //            KokDugumu temp = newNode.DugumEkle(oncekiDugumIcerigi[level]);
        //            temp.kopyala(oncekiDugum);
        //        }
        //        else
        //        {
        //            newNode.kopyala(oncekiDugum);
        //        }

        //        // Uzun olan dugumun (yeni gelen) eklenmesi, es anlamlilari kotar
        //        newNode.DugumEkle(icerik, kok);
        //        oncekiDugum.temizle();
        //    }

        //    /**
        //     *
        //     * Eðer köke önce "istimlak" ve sonra "istifa" gelirse
        //     * i-s-t-i-m-l-->istimlak
        //     * daha sonra gene son ortak harf olan "i" ye "f" karakterli düðümü
        //     * oluþturup istifayý baðlar
        //     * istimlak ta "m" düðümüne baðlý kalýr.
        //     * i-s-t-i-m-->istimlak
        //     *       |-f-->istifa
        //     *
        //     */

        //    else
        //    {
        //        while (level < icerik.Length && icerik[level] == oncekiDugumIcerigi[level])
        //        {
        //            newNode = newNode.DugumEkle(icerik[level]);
        //            level++;
        //        }
        //        // Kisa dugumun eklenmesi.
        //        if (level < icerik.Length)
        //        {
        //            newNode.DugumEkle(icerik, kok);
        //        }
        //        else
        //        {
        //            newNode.kokEkle(kok);
        //            newNode.Kelime = icerik;
        //        }

        //        // Uzun olan dugumun (yeni gelen) eklenmesi.
        //        newNode = newNode.DugumEkle(oncekiDugumIcerigi[level]);
        //        newNode.kopyala(oncekiDugum);
        //        // Es seslileri tasi.
        //        oncekiDugum.temizle();
        //    }
        //}


        /**
         * Aranan bir kök düðümünü bulur.
         *
         * @param str
         * @return Aranan kök ve eþ seslilerini taþýyan liste, bulunamazsa null.
         */


        public List<Kok> bul(String str)
        {
            int girisIndex = 0;
            // Basit bir tree traverse algoritmasý ile kelime bulunur.
            KokDugumu node = baslangicDugumu;
            while (node != null && girisIndex < str.Length)
            {
                if (node.Kelime != null && node.Kelime.Equals(str))
                {
                    break;
                }
                node = node.altDugumGetir(str[girisIndex++]);
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
        private static void SonHarfDugumuneEkle(String icerik, Kok kok, KokDugumu node)
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



