using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;
using net.zemberek.javaporttemp;
using Iesi.Collections.Generic;



namespace net.zemberek.bilgi.kokler
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
        private AltDugumListesi altDugumler = null;
        // Her düðüm bir harfle ifade edilir.
        private char harf;
        // eþ seslileri taþýyan liste (Kok nesneleri taþýr)
        private List<Kok> esSesliler = null;
        // Düðümðn taþýdýðý kök
        private Kok kok = null;
        // Kökün deðiþmiþ halini tutan string
        private IEnumerable<char> kelime = null;

        public KokDugumu()
        {
        }

        public KokDugumu(char harf)
        {
            this.harf = harf;
        }

        public KokDugumu(char harf, IEnumerable<char> icerik, Kok kok)
        {
            this.harf = harf;
            this.kok = kok;
            if (!icerik.Equals(kok.icerik())) this.kelime = icerik;
        }

        /**
         * Verilen karakteri taþýyan alt düðümü getirir.
         *
         * @param in
         * @return Eðer verilen karakteri taþýyan bir alt düðüm varsa
         * o düðümü, yoksa null.
         */
        public KokDugumu altDugumGetir(char cin)
        {
            if (altDugumler == null)
                return null;
            else
                return altDugumler.altDugumGetir(cin);
        }

        /**
         * Verilen düðümü bu düðüme alt düðüm olarak ekler.
         * Dönüþ deðeri eklenen düðümdür
         *
         * @param dugum
         * @return Eklenen düðüm
         */
        public KokDugumu addNode(KokDugumu dugum)
        {
            if (altDugumler == null)
            {
                altDugumler = new AltDugumListesi();
            }
            altDugumler.ekle(dugum);
            return dugum;
        }

        /**
         * @return tum alt dugumler. dizi olarak.
         */
        public KokDugumu[] altDugumDizisiGetir()
        {
            if (altDugumler == null)
            {
                return new KokDugumu[0];
            }
            return altDugumler.altDugumlerDizisiGetir();
        }

        public bool altDugumVarMi()
        {
            return !(altDugumler == null || altDugumler.size() == 0);
        }
        /**
         * Eðer Düðüme baðlý bir kök zaten varsa esSesli olarak ekle, 
         * yoksa sadece kok'e yaz.
         *
         * @param kok
         */
        public void kokEkle(Kok kok)
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

        public Kok getKok()
        {
            return this.kok;
        }

        public List<Kok> getEsSesliler()
        {
            return esSesliler;
        }

        public IEnumerable<char> getKelime()
        {
            if (kelime != null) return kelime;
            if (kok != null) return kok.icerik();
            return null;
        }

        public void setKelime(IEnumerable<char> kelime)
        {
            this.kelime = kelime;
        }

        /**
         * @return düðüme baðlý kök ve eþ seslilerin hepsini bir listeye 
         * koyarak geri döndürür.
         */
        public List<Kok> tumKokleriGetir()
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
        public void tumKokleriEkle(List<Kok> kokler)
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

        public void temizle()
        {
            this.kok = null;
            this.kelime = null;
            this.esSesliler = null;
        }

        public void kopyala(KokDugumu kaynak)
        {
            this.kok = kaynak.getKok();
            this.kelime = kaynak.getKelime();
            this.esSesliler = kaynak.getEsSesliler();
        }

        public char getHarf()
        {
            return harf;
        }

        public void setHarf(char harf)
        {
            this.harf = harf;
        }

        /**
         * Düðümün ve alt düðümlerinin aðaç yapýsý þeklinde string gösterimini döndürür.
         * sadece debug amaçlýdýr.
         *
         * @param level
         * @return dugumun string halini dondurur.
         */
        public String getStringRep(int level)
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

            KokDugumu[] subNodes = altDugumDizisiGetir();
            if (subNodes != null)
            {
                str += "\n " + indent + " Alt dugumler:\n";
                foreach (KokDugumu subNode in subNodes)
                {
                    if (subNode != null)
                    {
                        str += subNode.getStringRep(level + 1) + "\n";
                    }
                }
            }
            return str;
        }

        public String toString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("harf:").Append(harf);
            if (altDugumler != null)
                buf.Append(" alt dugum sayisi:").Append(altDugumler.size());
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
            public void ekle(KokDugumu dugum)
            {
                if (index == CEP_BUYUKLUGU)
                {
                    if (tumDugumler == null)
                    {
                        tumDugumler = new Dictionary<Char, KokDugumu>(CEP_BUYUKLUGU + 2);
                        for (int i = 0; i < CEP_BUYUKLUGU; i++)
                        {
                            tumDugumler.Add(dugumler[i].getHarf(), dugumler[i]);
                        }
                        dugumler = null;
                    }
                    tumDugumler.Add(dugum.getHarf(), dugum);
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
            public KokDugumu altDugumGetir(char giris)
            {
                if (dugumler != null)
                {
                    for (int i = 0; i < index; i++)
                    {
                        if (dugumler[i].getHarf() == giris)
                        {
                            return dugumler[i];
                        }
                    }
                    return null;
                }
                else
                {
                    return tumDugumler[giris];
                }
            }

            /**
             * Alt düðümleri dizi olarak döndürür.
             * @return KokDugumu[] cinsinden alt düðümler dizisi
             */
            public KokDugumu[] altDugumlerDizisiGetir()
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

            public int size()
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
