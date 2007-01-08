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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using log4net;
using NZemberek.Cekirdek.Mekanizma.Cozumleme;
using NZemberek.Cekirdek.MetinOkuma;

namespace NZemberek.Cekirdek.Mekanizma
{
    public class TurkceYaziTesti
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IKelimeCozumleyici cozumleyici, asciiCozumleyici;
        public static int HIC = 0;
        public static int AZ = 1;
        public static int ORTA = 2;
        public static int YUKSEK = 3;
        public static int KESIN = 4;

        public TurkceYaziTesti(IKelimeCozumleyici cozumleyici, IKelimeCozumleyici asciiCozumleyici)
        {
            this.cozumleyici = cozumleyici;
            this.asciiCozumleyici = asciiCozumleyici;
        }

        private double TurkceOranla(String yazi)
        {
            int cozulenler = 0, asciiCozulenler = 0, cozulemeyenler = 0;
            IList<YaziBirimi> analizDizisi = YaziIsleyici.AnalizDizisiOlustur(yazi);
            for (int i = 0; i < analizDizisi.Count; i++)
            {
                YaziBirimi birim = (YaziBirimi)analizDizisi[i];
                if (birim.tip == YaziBirimiTipi.KELIME)
                {
                    if (cozumleyici.Denetle(birim.icerik))
                        cozulenler++;
                    else if (asciiCozumleyici.Denetle(birim.icerik))
                        asciiCozulenler++;
                    else
                        cozulemeyenler++;
                }
            }
            int toplam = cozulenler + asciiCozulenler;
            if (toplam == 0 || (toplam + cozulemeyenler) == 0)
                return 0.0d;
            if (cozulemeyenler == 0)
                return 1.0d;
            double sonuc = 1.0d - (double)cozulemeyenler / (double)(toplam + cozulemeyenler);
            if (logger.IsInfoEnabled) logger.Info("cozulenler:" + cozulenler + "  ascii Cozulenler:" + asciiCozulenler
                        + "cozulemeyenler:" + cozulemeyenler + "  oran:" + sonuc);
            return sonuc;
        }

        public int TurkceTest(String giris)
        {
            double sonuc = TurkceOranla(giris);
            if (sonuc <= 0.1d)
                return HIC;
            if (sonuc > 0.1d && sonuc <= 0.35d)
                return AZ;
            if (sonuc > 0.35d && sonuc <= 0.65d)
                return ORTA;
            if (sonuc > 0.65d && sonuc <= 0.95d)
                return YUKSEK;
            return KESIN;
        }
    }
}
