using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NUnit.Framework;
using net.zemberek.yapi.ek;
using net.zemberek.tr.yapi.ek;
using net.zemberek.bilgi;

namespace net.zemberek.tests.yapi.ek
{
    /**
     * Fonksiyonel test. ek uretimlerinin dogrulugunu denetler.
     * User: ahmet
     * Date: Aug 23, 2005
     */
    [TestFixture]
    public class TestEkUreticiTr : TemelTest 
    {
        [Test]
        public void testUretici() {
            EkUretici uretici = new EkUreticiTr(alfabe);

            StreamReader reader = new KaynakYukleyici().getReader("kaynaklar/tr/test/ek_olusum.txt");
            String s;
            while ((s = reader.ReadLine()) != null) {
                if (s.StartsWith("#") || s.Trim().Length == 0) continue;
                String kuralKelimesi = s.Substring(0, s.IndexOf(':'));
                String[] olusumlar = s.Substring(s.IndexOf(':') + 1).Trim().Split(' ');
                //System.Console.WriteLine("okunan kelime:" + kuralKelimesi + " olusumlar:" + olusumlar.ToString());
                olusumTesti(kuralKelimesi, olusumlar);
            }
            reader.Close();
        }
        [Test]
        public void testKuralCozumleyici() {
             
        }

        public void olusumTesti(String kural, String[] olusumlar)
        {
    /*        Set beklenen = new HashSet();
            for (String s : olusumlar)
                beklenen.add(new HarfDizisi(s, TurkceAlfabe.ref()));
            Set gelen = new HashSet(uretici.ekOlusumlariniUret(kural).values());
            assertEquals("beklenmedik ya da eksik olusum!", gelen, beklenen);*/
        }
    }
}
