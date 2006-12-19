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
