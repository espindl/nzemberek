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
 * The Original Code is Zemberek Doğal Dil İşleme Kütüphanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Akın, Mehmet D. Akın.
 * Portions created by the Initial Developer are Copyright (C) 2006
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Mert Derman
 *   Tankut Tekeli
 * ***** END LICENSE BLOCK ***** */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NZemberek.Cekirdek.Benzerlik;

namespace NZemberek.TurkiyeTurkcesi.Test
{
    [TestClass]
    public class TestBenzerlik
    {
        private const double benzerlikSiniri = 0.85;
        private const string sablonBenzemiyor = "{0} ile {1},{2} sınırına göre birbirine benzemiyor. ";
        private const string sablonBenziyor = "{0} ile {1},{2} sınırına göre birbirine benziyor. ";

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void testJaroWinkler()
        {
            IBenzerlikSinayici jwbs = new JaroWinklerBenzerlikSinayici();
            double actual = jwbs.SozcuklerNeKadarBenzer("merhaba", "merhaa");
            double expectedMin = 0.97;
            Assert.IsTrue(actual >= expectedMin, string.Format("Benzerlik {0}dan büyük bekleniyordu ama {1}.", expectedMin, actual));
        }

        [TestMethod]
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

        [TestMethod]
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
