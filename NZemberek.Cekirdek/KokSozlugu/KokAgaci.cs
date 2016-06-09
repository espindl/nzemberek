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
using NZemberek.Cekirdek.Yapi;


namespace NZemberek.Cekirdek.KokSozlugu
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
        private KokDugumu baslangicDugumu = null;

        private Alfabe alfabe;

        public Alfabe Alfabe
        {
            get { return alfabe; }
            set { alfabe = value; }
        }

        public KokAgaci(KokDugumu baslangicDugumu, Alfabe alfabe)
        {
            this.baslangicDugumu = baslangicDugumu;
            this.alfabe = alfabe;
        }

        public KokDugumu BaslangicDugumu
        {
            get { return baslangicDugumu; }
        }

        private KokDugumu IcerikDugumuBul(string icerik)
        {
            KokDugumu parent = baslangicDugumu;
            KokDugumu child = null;
            while (parent.Level < icerik.Length)
            {
                child = parent.AltDugumGetir(icerik[parent.Level]);
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


        /// <summary>
        /// Verilen kökü ve içeriði düðümleri oluþturarak aðaca ekler.
        /// </summary>
        /// <param name="icerik">kökün içeriði(deðiþime uðramýþ ise)</param>
        /// <param name="kok">eklenecek kök</param>
        public void Ekle(string icerik, Kok kok)
        {
            // Ýçerik için Mevcut aðaçta inilebilecek en derin düðümü Bul.
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
                    if (dugum.Kok == null)
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
                            dugum.KokEkle(kok);
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
            if (node.AltDugumVar())
            {
                node.KokEkle(kok);
                node.Kelime = icerik;
            }
            // Eþ sesli!
            else if (node.Kelime.Equals(icerik))
            {
                node.KokEkle(kok);
            }
            else if (node.Kok != null)
            {
                node.KokuDallandir();
                node.KokEkle(kok);
                node.Kelime = icerik;
            }
        }
    }

}



