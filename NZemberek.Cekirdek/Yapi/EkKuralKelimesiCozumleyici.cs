using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NZemberek.Cekirdek.Yapi
{
    public class EkKuralKelimesiCozumleyici
    {
        private Alfabe alfabe;
        //Ek kural bilgisi nesnesi dile ozel ek kural kelime enum sinifindan elde edilir.
        private IEkKuralBilgisi ekKuralBilgisi;

		public EkKuralKelimesiCozumleyici(Alfabe alfabe, IEkKuralBilgisi ekKuralBilgisi)
		{
			this.alfabe = alfabe;
			this.ekKuralBilgisi = ekKuralBilgisi;
		}

        public List<EkUretimBileseni> cozumle(String uretimKelimesi)
        {
            if (uretimKelimesi == null || uretimKelimesi.Length == 0)
            {
                return new List<EkUretimBileseni>();
            }
            char[] uretimKelimesiCharacters = uretimKelimesi.Trim().Replace("[ ]", "").ToCharArray();
            List<EkUretimBileseni> bilesenler = new List<EkUretimBileseni>();
            foreach (EkUretimBileseni ekUretimBileseni in BilesenEnumerator(uretimKelimesiCharacters))
            {
                bilesenler.Add(ekUretimBileseni);
            }
            return bilesenler;
        }

        public IEnumerable<EkUretimBileseni> BilesenEnumerator(char[] uretimKelimesiCharacters)
        {
            for(int i=0;i<uretimKelimesiCharacters.Length;i++)
            {
                char c = uretimKelimesiCharacters[i];
                if (ekKuralBilgisi.harfKuralKarakterleri().Contains(c))
                {
                    i += 1;
                    if (i == uretimKelimesiCharacters.Length)
                    {
                        throw new ArgumentException(c + " kuralindan sonra normal harf bekleniyordu!");
                    }
                    char h = uretimKelimesiCharacters[i];
                    if (ekKuralBilgisi.sesliKuralKarakterleri().Contains(h))
                    {
                        throw new ArgumentException(c + " kuralindan sonra sesli uretim kurali gelemez:" + h);
                    }
                    yield return new EkUretimBileseni(ekKuralBilgisi.karakterKuralTablosu()[c], alfabe.Harf(h));
                }
                else if (ekKuralBilgisi.sesliKuralKarakterleri().Contains(c))
                {
                    yield return new EkUretimBileseni(ekKuralBilgisi.karakterKuralTablosu()[c], Alfabe.TANIMSIZ_HARF);
                }
                else if (alfabe.Harf(c) != null && Char.IsLower(c))
                {
                    yield return new EkUretimBileseni(ekKuralBilgisi.harfEklemeKurali(), alfabe.Harf(c));
                }
                else
                {
                    throw new ArgumentException(c + "  simgesi cozumlenemiyor.. kelime:" + uretimKelimesiCharacters);
                }
            }            
        }
    }
}
