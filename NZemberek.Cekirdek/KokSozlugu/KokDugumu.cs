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

using System;
using System.Collections.Generic;
using System.Text;
using NZemberek.Cekirdek.Yapi;
using NZemberek.Cekirdek.Kolleksiyonlar;



namespace NZemberek.Cekirdek.KokSozlugu
{
    /*
    * Created on 24.Eki.2004
    * 
    * Kök düðümü sýnýfý Kök aðacýnýn yapýtaþýdýr. Her düðüm, kökler, eþseli kökler,
    * deðiþmiþ halleri ifade eden bir string ve uygun þekilde bellek kullanýmý için 
    * hazýrlanmýþ özel bir alt düðüm listesi nesnesi taþýr.
    * <p/>
    * çeþitli nedenlerle deðiþikliðe uðrayabilecek olan kökler aðaca eklenirken
    * deðiþmiþ halleri ile beraber eklenirler. Örneðin kitap kökü hem kitab hem de
    * kitap hali ile sözlüðe eklenir, ancak bu iki kelime için oluþan düðüm de
    * ayný kökü gösterirler. Böylece Kitabýna gibi kelimeler için kök adaylarý
    * aranýrken kitap köküne eriþilmiþ olur.
    * <p/>
    * Eþ sesli olan kökler ayný düðüme baðlanýrlar. Aðacýn oluþumu sýrasýnda ilk 
    * gelen kök düðümdeki kök deðiþkenne, sonradan gelenler de esSesliler listesine 
    * eklenirler. Arama sýrasýnda bu kök te aday olarak döndürülür.
    *
    * @author MDA
    */
    public class KokDugumu
    {
        //Düðümün aðaçtaki seviyesi
        private int level;

        private AltDugumListesi altDugumler = null;
        // Her düðüm bir harfle ifade edilir.
        private char harf;
        // eþ seslileri taþýyan liste (Kok nesneleri taþýr)
        private List<Kok> esSesliler = null;

        public List<Kok> EsSesliler
        {
            get { return esSesliler; }
        }
        // Düðümün taþýdýðý kök
        private Kok kok = null;

        public Kok Kok
        {
            get { return kok; }
        }
        // Kökün deðiþmiþ halini tutan string
        private string kelime = null;

        public KokDugumu(int pLevel)
        {
            level = pLevel;
        }

        public KokDugumu(int pLevel, char harf):this(pLevel)
        {
            this.harf = harf;
        }

        public KokDugumu(int pLevel, char harf, string icerik, Kok kok):this(pLevel,harf)
        {
            this.kok = kok;
            if (!icerik.Equals(kok.Icerik)) this.kelime = icerik;
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        /**
         * Verilen karakteri taþýyan alt düðümü getirir.
         *
         * @param in
         * @return Eðer verilen karakteri taþýyan bir alt düðüm varsa
         * o düðümü, yoksa null.
         */
        public KokDugumu AltDugumGetir(char cin)
        {
            if (altDugumler == null)
                return null;
            else
                return altDugumler.AltDugumGetir(cin);
        }

        /**
         * Verilen düðümü bu düðüme alt düðüm olarak ekler.
         * Dönüþ deðeri eklenen düðümdür
         *
         * @param dugum
         * @return Eklenen düðüm
         */
        private KokDugumu DugumEkle(KokDugumu dugum)
        {
            if (altDugumler == null)
            {
                altDugumler = new AltDugumListesi();
            }
            altDugumler.Ekle(dugum);
            return dugum;
        }

        public KokDugumu DugumEkle(char harf)
        {
            KokDugumu yeniDugum = new KokDugumu(this.Level + 1, harf);
            return this.DugumEkle(yeniDugum);
        }

        public KokDugumu DugumEkle(string icerik, Kok kok)
        {
            KokDugumu yeniDugum = new KokDugumu(this.Level + 1, icerik[this.Level], icerik, kok);
            return this.DugumEkle(yeniDugum);
        }
        /**
         * @return tum alt dugumler. dizi olarak.
         */
        public KokDugumu[] AltDugumDizisiGetir()
        {
            if (altDugumler == null)
            {
                return new KokDugumu[0];
            }
            return altDugumler.AltDugumlerDizisiGetir();
        }

        public bool AltDugumVar()
        {
            return !(altDugumler == null || altDugumler.Boyut() == 0);
        }
        /**
         * Eðer Düðüme baðlý bir kök zaten varsa esSesli olarak Ekle, 
         * yoksa sadece kok'e yaz.
         *
         * @param kok
         */
        public void KokEkle(Kok kok)
        {
            if (this.kok != null)
            {
                if (esSesliler == null) esSesliler = new List<Kok>(1);
                esSesliler.Add(kok);
            }
            else
            {
                this.kok = kok;
            }
        }

        public string Kelime
        {
            get
            {
                if (kelime != null) return kelime;
                if (kok != null) return kok.Icerik;
                return null;
            }
            set { kelime = value; }
        }


        /**
         * @return düðüme baðlý kök ve eþ seslilerin hepsini bir listeye 
         * koyarak geri döndürür.
         */
        public List<Kok> TumKokleriGetir()
        {
            if (kok != null)
            {
                List<Kok> kokler = new List<Kok>();
                kokler.Add(kok);
                if (esSesliler != null)
                {
                    kokler.AddRange(esSesliler);
                }
                return kokler;
            }
            return null;
        }

        /**
         * Verilen collectiona düðüme baðlý tüm kökleri ekler. 
         *
         * @param kokler
         */
        public void TumKokleriEkle(List<Kok> kokler)
        {
            if (kok != null && !kokler.Contains(kok))
            {
                kokler.Add(kok);
                if (esSesliler != null)
                {
                    kokler.AddRange(esSesliler);
                }
            }
        }

        public void Temizle()
        {
            this.kok = null;
            this.kelime = null;
            this.esSesliler = null;
        }

        public void Kopyala(KokDugumu kaynak)
        {
            this.kok = kaynak.Kok;
            this.kelime = kaynak.Kelime;
            this.esSesliler = kaynak.EsSesliler;
        }

        /// <summary>
        /// Düðüme baðlý kelimeyi bir seviye ileri düðüm oluþturarak ona aktarýr.
        /// </summary>
        /// <returns></returns>
        public KokDugumu KokuDallandir()
        {
            if (!this.KisayolDugumu)
            {
                throw new ApplicationException("Düðüm içeriði ilerletilmeye uygun deðil.");
            }
            KokDugumu aNewNode = this.DugumEkle(this.Kelime[this.Level]);
            aNewNode.Kopyala(this);
            this.Temizle();
            return aNewNode;
        }

        public char Harf
        {
            get { return harf; }
//            set { Harf = value; }
        }

        /// <summary>
        /// Eðer düðüm seviyesinden uzun bir köke iþaret ediyor ise true döner.
        /// Eðer tam kendi içeriðine iþaret ediyorsa veya herhangi bir köke iþaret etmiyorsa false döner.
        /// Bu özellik aðacýn oluþturulmasý sýrasýnda düðümün iþaret ettiði kökün alt düðümlere ilerletilmesi gerektiðinde kullanýlýr.
        /// </summary>
        public bool KisayolDugumu
        {
            get
            {
                return (this.Kelime != null && level < this.Kelime.Length);
            }
        }

        /**
         * Düðümün ve alt düðümlerinin aðaç yapýsý þeklinde string gösterimini döndürür.
         * sadece debug amaçlýdýr.
         *
         * @param level
         * @return dugumun string halini dondurur.
         */
        public String MetinBicimindeVer(int level)
        {
            char[] indentChars = new char[level * 2];
            for (int i = 0; i < indentChars.Length; i++)
                indentChars[i] = ' ';
            String indent = new String(indentChars);
            String str = indent + " Harf: " + harf;
            if (kelime != null)
            {
                str += " [Kelime: " + kelime + "] ";
            }
            if (kok != null)
            {
                str += " [Kok: " + kok + "] ";
            }

            if (esSesliler != null)
            {
                str += " [Es sesli: ";
                foreach (Kok anEsSesliler in esSesliler)
                {
                    str += (anEsSesliler) + " ";
                }
                str += " ]";
            }

            KokDugumu[] subNodes = AltDugumDizisiGetir();
            if (subNodes != null)
            {
                str += "\n " + indent + " Alt dugumler:\n";
                foreach (KokDugumu subNode in subNodes)
                {
                    if (subNode != null)
                    {
                        str += subNode.MetinBicimindeVer(level + 1) + "\n";
                    }
                }
            }
            return str;
        }

        public override String ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("Harf:").Append(harf);
            if (altDugumler != null)
                buf.Append(" alt dugum sayisi:").Append(altDugumler.Boyut());
            return buf.ToString();
        }

        /**
         * Kök agacindaki düðümlerin alt düðümleri için bu sinifi kullanirlar.
         * Özellikle bellek kullaniminin önemli oldugu Zemberek-Pardus ve OOo 
         * eklentisi gibi uygulamalarda bu yapinin kullanilmasi bellek kazanci 
         * getirmektedir. 
         * Asagidaki sinifta alt dugum sayisi CEP_BUYUKLUGU degerinden
         * az ise sadece CEP_BUYUKLUGU elemanli bir dizi acar. Bu dizi üzerinde 
         * Arama yapmak biraz daha yavas olsa da ortalama CEP_BUYUKLUGU/2 aramada 
         * sonuca eriþildiði için verilen ceza minimumda kalir. 
         *
         */
        private static readonly int CEP_BUYUKLUGU = 3;

        private sealed class AltDugumListesi
        {
            KokDugumu[] dugumler = new KokDugumu[CEP_BUYUKLUGU];
            int index = 0;
            IDictionary<Char, KokDugumu> tumDugumler = null;

            /**
             * Verilen düðümü alt düðüm olarak ekler. eger alt düðümlerinin sayisi
             * CEP_BUYUKLUGU degerini asmissa bir HashMap oluþturur
             * @param dugum
             */
            public void Ekle(KokDugumu dugum)
            {
                if (index == CEP_BUYUKLUGU)
                {
                    if (tumDugumler == null)
                    {
                        tumDugumler = new Dictionary<Char, KokDugumu>(CEP_BUYUKLUGU + 2);
                        for (int i = 0; i < CEP_BUYUKLUGU; i++)
                        {
                            tumDugumler.Add(dugumler[i].Harf, dugumler[i]);
                        }
                        dugumler = null;
                    }
                    tumDugumler.Add(dugum.Harf, dugum);
                }
                else
                {
                    dugumler[index++] = dugum;
                }
            }

            /**
             * Verilen karaktere sahip alt düðümü döndürür.
             * @param giris
             * @return ilgili KokDugumu
             */
            public KokDugumu AltDugumGetir(char giris)
            {
                if (dugumler != null)
                {
                    for (int i = 0; i < index; i++)
                    {
                        if (dugumler[i].Harf == giris)
                        {
                            return dugumler[i];
                        }
                    }
                    return null;
                }
                else
                {
                    //TODO : Burada KEyNotFound hatasý imkansýz mýdýr? (@tankut)
                    if (tumDugumler.ContainsKey(giris))
                        return tumDugumler[giris];
                    return null;
                }
            }

            /**
             * Alt düðümleri dizi olarak döndürür.
             * @return KokDugumu[] cinsinden alt düðümler dizisi
             */
            public KokDugumu[] AltDugumlerDizisiGetir()
            {
                if (dugumler != null)
                {
                    return dugumler;
                }
                else
                {
                    KokDugumu[] ret = new KokDugumu[tumDugumler.Values.Count];
                    tumDugumler.Values.CopyTo(ret, 0);
                    return ret;
                }
            }

            public int Boyut()
            {
                if (dugumler != null)
                {
                    return index;
                }
                else
                {
                    return tumDugumler.Count;
                }
            }

        }
    }
}
