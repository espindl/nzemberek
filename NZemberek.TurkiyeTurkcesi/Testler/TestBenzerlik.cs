using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NZemberek.Cekirdek.Benzerlik;

namespace NZemberek.TurkiyeTurkcesi.Testler
{
    [TestFixture]
    public class TestBenzerlik
    {
        private const double benzerlikSiniri = 0.85;
        private const string sablonBenzemiyor = "{0} ile {1},{2} sınırına göre birbirine benzemiyor. ";
        private const string sablonBenziyor = "{0} ile {1},{2} sınırına göre birbirine benziyor. ";

        [Test]
        public void testLevenstheinDogrular()
        {
            IBenzerlikSinayici lbs = new LevenstheinBenzerlikSinayici();
            string[] ilkler = { "kendisiyle", "kendisiyle", "kendisiyle", "arkasından", "arkasından", "arkasından", "ayin", "ayin", "ayin" };
            string[] ikinciler = { "kendisiyle", "kedisiyle", "gendisiyle", "arkasında", "arakasından", "parkasından", "abin", "ayı", "abi" };
            bool hepsiDogru = true;
            string mesaj = string.Empty;
            for (int i = 0; i < 6; i++)
            {
                if (!lbs.SocuklerBenzer(ilkler[i], ikinciler[i], benzerlikSiniri))
                {
                    mesaj += string.Format(sablonBenzemiyor, ilkler[i], ikinciler[i], benzerlikSiniri);
                    hepsiDogru = false;
                }
            }
            Assert.IsTrue(hepsiDogru, mesaj);
        }

        [Test]
        public void testLevenstheinYanlislar()
        {
            IBenzerlikSinayici lbs = new LevenstheinBenzerlikSinayici();
            string[] ilkler = { "kendisiyle", "kendisiyle", "kendisiyle", "arkasından", "arkasından", "arkasından", "ayin", "ayin", "ayin" };
            string[] ikinciler = { "keriziyle", "gerisiyle", "kendisinlen", "arabasından", "parkasında", "arkadaşından", "bayım", "tayin", "yarın" };
            bool hepsiYanlis = true;
            string mesaj = string.Empty;
            for (int i = 0; i < 6; i++)
            {
                if (lbs.SocuklerBenzer(ilkler[i], ikinciler[i], benzerlikSiniri))
                {
                    mesaj += string.Format(sablonBenziyor, ilkler[i], ikinciler[i], benzerlikSiniri);
                    hepsiYanlis = false;
                }
            }
            Assert.IsTrue(hepsiYanlis, mesaj);
        }

        [Test]
        public void testJaroWinklerDogrular()
        {
            IBenzerlikSinayici jwbs = new JaroWinklerBenzerlikSinayici();
            string[] ilkler = { "kendisiyle", "kendisiyle", "kendisiyle", "arkasından", "arkasından", "arkasından", "ayin", "ayin", "ayin" };
            string[] ikinciler = { "kendisiyle", "kedisiyle", "gendisiyle", "arkasında", "arakasından", "parkasından", "abin", "ayı", "abi" };
            bool hepsiDogru = true;
            string mesaj = string.Empty;
            for (int i = 0; i < 6; i++)
            {
                if (!jwbs.SocuklerBenzer(ilkler[i], ikinciler[i], benzerlikSiniri))
                {
                    mesaj += string.Format(sablonBenzemiyor, ilkler[i], ikinciler[i], benzerlikSiniri);
                    hepsiDogru = false;
                }
            }
            Assert.IsTrue(hepsiDogru, mesaj);
        }

        [Test]
        public void testJaroWinklerYanlislar()
        {
            IBenzerlikSinayici jwbs = new JaroWinklerBenzerlikSinayici();
            string[] ilkler = { "kendisiyle", "kendisiyle", "kendisiyle", "arkasından", "arkasından", "arkasından", "ayin", "ayin", "ayin" };
            string[] ikinciler = { "gerdiziyle", "gerisiyle", "bebisinle", "arabasıylan", "barakasında", "parkasıyla", "bayım", "tayin", "yarın" };
            bool hepsiYanlis = true;
            string mesaj = string.Empty;
            for (int i = 0; i < 6; i++)
            {
                if (jwbs.SocuklerBenzer(ilkler[i], ikinciler[i], benzerlikSiniri))
                {
                    mesaj += string.Format(sablonBenziyor, ilkler[i], ikinciler[i], benzerlikSiniri);
                    hepsiYanlis = false;
                }
            }
            Assert.IsTrue(hepsiYanlis, mesaj);
        }

        [Test]
        public void testJaroWinkler()
        {
            IBenzerlikSinayici jwbs = new JaroWinklerBenzerlikSinayici();
            double actual = jwbs.SozcuklerNeKadarBenzer("merhaba", "merhaa");
            double expectedMin = 0.97;
            Assert.IsTrue(actual >= expectedMin, string.Format("Benzerlik {0}dan büyük bekleniyordu ama {1}.", expectedMin, actual));
        }

        [Test]
        public void testBenzerlikaraciLevenstheinDogrular()
        {
            string[] ilkler = { "elma", "elma", "elma", "elma", "elma", "elma", "elma", "armutlar" };
            string[] ikinciler = { "elma", "ekma", "ema", "elmas", "lma", "emas", "el", "armutlr" };
            int[] mesafeler = { 1, 1, 1, 1, 1, 2, 2, 1 };
            bool hepsiDogru = true;
            string mesaj = string.Empty;
            for (int i = 0; i < ilkler.Length; i++)
            {
                if (!BenzerlikAraci.DuzeltmeMesafesiIcinde(ilkler[i], ikinciler[i], mesafeler[i]))
                {
                    mesaj += string.Format("{0} ile {1}, {2} enazbenzerligine gore benzer değil.", ilkler[i], ikinciler[i], mesafeler[i]);
                    hepsiDogru = false;
                }
            }
            Assert.IsTrue(hepsiDogru, mesaj);
        }

        [Test]
        public void testBenzerlikaraciLevenstheinYanlislar()
        {
            string[] ilkler = {  "elmalar", "elma", "armutu" };
            string[] ikinciler = { "gelmeler", "eksa", "mahmutu" };
            int[] mesafeler = { 1, 1, 1 };
            bool hepsiYanlis = true;
            string mesaj = string.Empty;
            for (int i = 0; i < ilkler.Length; i++)
            {
                if (BenzerlikAraci.DuzeltmeMesafesiIcinde(ilkler[i], ikinciler[i], mesafeler[i]))
                {
                    mesaj += string.Format("{0} ile {1}, {2} enazbenzerligine gore benzer.", ilkler[i], ikinciler[i], mesafeler[i]);
                    hepsiYanlis = false;
                }
            }
            Assert.IsTrue(hepsiYanlis, mesaj);
        }
    }
}
