using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NZemberek.Cekirdek.Yapi
{
    public class TemelEkUretimKurali : IEkUretimKurali
    {
        public static readonly TemelEkUretimKurali SESLI_AA = new TemelEkUretimKurali("SESLI_AA",true);
        public static readonly TemelEkUretimKurali SESLI_AE = new TemelEkUretimKurali("SESLI_AE", true);
        public static readonly TemelEkUretimKurali SESLI_IU = new TemelEkUretimKurali("SESLI_IU", true);
        public static readonly TemelEkUretimKurali HARF = new TemelEkUretimKurali("HARF",false);
        public static readonly TemelEkUretimKurali KAYNASTIR = new TemelEkUretimKurali("KAYNASTIR", false);
        public static readonly TemelEkUretimKurali SERTLESTIR = new TemelEkUretimKurali("SERTLESTIR", false);
        public static readonly TemelEkUretimKurali SESSIZ_Y = new TemelEkUretimKurali("SESSIZ_Y", false);
        public static readonly TemelEkUretimKurali YUMUSAT = new TemelEkUretimKurali("YUMUSAT", false);
        public static readonly TemelEkUretimKurali YOK = new TemelEkUretimKurali("YOK", false);

        private readonly String myName; // for debug only
        private readonly bool sesli; // for debug only

        private TemelEkUretimKurali(String name, bool sesliMi)
        {
            myName = name;
            sesli = sesliMi;
        }

        public override String ToString()
        {
            return myName;
        }

        public bool isSesliUretimKurali()
        {
            return sesli;
        }

        public class TemelKuralBilgisi : IEkKuralBilgisi
        {

            public List<char> sesliKuralKarakterleri()
            {
                return new List<char> { 'A', 'I', 'E', 'Y' };
            }

            public List<char> harfKuralKarakterleri()
            {
                return new List<char> { '+', '>', '~' };
            }

            Dictionary<char, IEkUretimKurali> IEkKuralBilgisi.karakterKuralTablosu()
            {
                Dictionary<char, IEkUretimKurali> kuralTablosu = new Dictionary<char, IEkUretimKurali>();
                kuralTablosu.Add('A', TemelEkUretimKurali.SESLI_AE);
                kuralTablosu.Add('I', TemelEkUretimKurali.SESLI_IU);
                kuralTablosu.Add('E', TemelEkUretimKurali.SESLI_AA);
                kuralTablosu.Add('Y', TemelEkUretimKurali.SESSIZ_Y);
                kuralTablosu.Add('+', TemelEkUretimKurali.KAYNASTIR);
                kuralTablosu.Add('>', TemelEkUretimKurali.SERTLESTIR);
                kuralTablosu.Add('~', TemelEkUretimKurali.YUMUSAT);
                return kuralTablosu;

            }

            IEkUretimKurali IEkKuralBilgisi.harfEklemeKurali()
            {
                return TemelEkUretimKurali.HARF;
            }
        }
    }

    
}
