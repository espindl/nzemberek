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
 * The Original Code is Zemberek Do�al Dil ��leme K�t�phanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Ak�n, Mehmet D. Ak�n.
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
    * K�k d���m� s�n�f� K�k a�ac�n�n yap�ta��d�r. Her d���m, k�kler, e�seli k�kler,
    * de�i�mi� halleri ifade eden bir string ve uygun �ekilde bellek kullan�m� i�in 
    * haz�rlanm�� �zel bir alt d���m listesi nesnesi ta��r.
    * <p/>
    * �e�itli nedenlerle de�i�ikli�e u�rayabilecek olan k�kler a�aca eklenirken
    * de�i�mi� halleri ile beraber eklenirler. �rne�in kitap k�k� hem kitab hem de
    * kitap hali ile s�zl��e eklenir, ancak bu iki kelime i�in olu�an d���m de
    * ayn� k�k� g�sterirler. B�ylece Kitab�na gibi kelimeler i�in k�k adaylar�
    * aran�rken kitap k�k�ne eri�ilmi� olur.
    * <p/>
    * E� sesli olan k�kler ayn� d���me ba�lan�rlar. A�ac�n olu�umu s�ras�nda ilk 
    * gelen k�k d���mdeki k�k de�i�kenne, sonradan gelenler de esSesliler listesine 
    * eklenirler. Arama s�ras�nda bu k�k te aday olarak d�nd�r�l�r.
    *
    * @author MDA
    */
    public class KokDugumu
    {
        //D���m�n a�a�taki seviyesi
        private byte level;

        private AltDugumListesi altDugumler = null;
        // Her d���m bir harfle ifade edilir.
        private char harf;
        // e� seslileri ta��yan liste (Kok nesneleri ta��r)
        private List<Kok> esSesliler = null;
        // K�k�n de�i�mi� halini tutan string
        private string kelime = null;
        // D���m�n ta��d��� k�k
        private Kok kok = null;


        public List<Kok> EsSesliler
        {
            get { return esSesliler; }
        }

        public Kok Kok
        {
            get { return kok; }
        }

        public byte Level
        {
            get { return level; }
            set { level = value; }
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

        public char Harf
        {
            get { return harf; }
            //            set { Harf = value; }
        }

        /// <summary>
        /// E�er d���m seviyesinden uzun bir k�ke i�aret ediyor ise true d�ner.
        /// E�er tam kendi i�eri�ine i�aret ediyorsa veya herhangi bir k�ke i�aret etmiyorsa false d�ner.
        /// Bu �zellik a�ac�n olu�turulmas� s�ras�nda d���m�n i�aret etti�i k�k�n alt d���mlere ilerletilmesi gerekti�inde kullan�l�r.
        /// </summary>
        public bool KisayolDugumu
        {
            get
            {
                return (this.Kelime != null && level < this.Kelime.Length);
            }
        }
        
        
        public KokDugumu(byte pLevel)
        {
            level = pLevel;
        }

        public KokDugumu(byte pLevel, char harf):this(pLevel)
        {
            this.harf = harf;
        }

        public KokDugumu(byte pLevel, char harf, string icerik, Kok kok)
            : this(pLevel, harf)
        {
            this.kok = kok;
            if (!icerik.Equals(kok.Icerik)) this.kelime = icerik;
        }

        /**
         * Verilen d���m� bu d���me alt d���m olarak ekler.
         * D�n�� de�eri eklenen d���md�r
         *
         * @param dugum
         * @return Eklenen d���m
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

        private KokDugumu DugumEkle(char harf)
        {
            KokDugumu yeniDugum = new KokDugumu((byte)(this.Level + 1), harf);
            return this.DugumEkle(yeniDugum);
        }

        /**
         * @return d���me ba�l� k�k ve e� seslilerin hepsini bir listeye 
         * koyarak geri d�nd�r�r.
         */
        private List<Kok> TumKokleriGetir()
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

        private void Temizle()
        {
            this.kok = null;
            this.kelime = null;
            this.esSesliler = null;
        }

        private void Kopyala(KokDugumu kaynak)
        {
            this.kok = kaynak.Kok;
            this.kelime = kaynak.Kelime;
            this.esSesliler = kaynak.EsSesliler;
        }

        /**
         * Verilen karakteri ta��yan alt d���m� getirir.
         *
         * @param in
         * @return E�er verilen karakteri ta��yan bir alt d���m varsa
         * o d���m�, yoksa null.
         */
        public KokDugumu AltDugumGetir(char cin)
        {
            if (altDugumler == null)
                return null;
            else
                return altDugumler.AltDugumGetir(cin);
        }

        public KokDugumu DugumEkle(string icerik, Kok kok)
        {
            KokDugumu yeniDugum = new KokDugumu((byte)(this.Level + 1), icerik[this.Level], icerik, kok);
            return this.DugumEkle(yeniDugum);
        }

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
         * E�er D���me ba�l� bir k�k zaten varsa esSesli olarak Ekle, 
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

        /**
         * Verilen collectiona d���me ba�l� t�m k�kleri ekler. 
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

        /// <summary>
        /// D���me ba�l� kelimeyi bir seviye ileri d���m olu�turarak ona aktar�r.
        /// </summary>
        /// <returns></returns>
        public KokDugumu KokuDallandir()
        {
            if (!this.KisayolDugumu)
            {
                throw new ApplicationException("D���m i�eri�i ilerletilmeye uygun de�il.");
            }
            KokDugumu aNewNode = this.DugumEkle(this.Kelime[this.Level]);
            aNewNode.Kopyala(this);
            this.Temizle();
            return aNewNode;
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
         * K�k agacindaki d���mlerin alt d���mleri i�in bu sinifi kullanirlar.
         * �zellikle bellek kullaniminin �nemli oldugu Zemberek-Pardus ve OOo 
         * eklentisi gibi uygulamalarda bu yapinin kullanilmasi bellek kazanci 
         * getirmektedir. 
         * Asagidaki sinifta alt dugum sayisi CEP_BUYUKLUGU degerinden
         * az ise sadece CEP_BUYUKLUGU elemanli bir dizi acar. Bu dizi �zerinde 
         * Arama yapmak biraz daha yavas olsa da ortalama CEP_BUYUKLUGU/2 aramada 
         * sonuca eri�ildi�i i�in verilen ceza minimumda kalir. 
         *
         */
        private static readonly int CEP_BUYUKLUGU = 3;

        private sealed class AltDugumListesi
        {
            KokDugumu[] dugumler = new KokDugumu[CEP_BUYUKLUGU];
            int index = 0;
            IDictionary<Char, KokDugumu> tumDugumler = null;

            /**
             * Verilen d���m� alt d���m olarak ekler. eger alt d���mlerinin sayisi
             * CEP_BUYUKLUGU degerini asmissa bir HashMap olu�turur
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
             * Verilen karaktere sahip alt d���m� d�nd�r�r.
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
                    //TODO : Burada KEyNotFound hatas� imkans�z m�d�r? (@tankut)
                    if (tumDugumler.ContainsKey(giris))
                        return tumDugumler[giris];
                    return null;
                }
            }

            /**
             * Alt d���mleri dizi olarak d�nd�r�r.
             * @return KokDugumu[] cinsinden alt d���mler dizisi
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
