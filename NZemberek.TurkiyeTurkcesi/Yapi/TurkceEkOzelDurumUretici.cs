﻿/* ***** BEGIN LICENSE BLOCK *****
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

// V 0.1
using System;
using System.Collections.Generic;
using System.Text;
using NZemberek.Cekirdek.Yapi;

namespace NZemberek.TrTurkcesi.Yapi
{
    public class TurkceEkOzelDurumUretici : TemelEkOzelDurumUretici {

        public TurkceEkOzelDurumUretici(Alfabe alfabe) {
            this.alfabe = alfabe;
        }

        public override EkOzelDurumu Uret(String ad) 
        {
            EkOzelDurumu oz = base.Uret(ad);
            if (oz != null)
                return oz;

            if (!Mevcut(TurkceEkOzelDurumTipi.AllValues, ad)) 
            {
                return null;
            }

            switch (ad) 
            {
                case "BERABERLIK_IS":
                    return new BeraberlikIsOzelDurumu();
                case "EDILGEN":
                    return new EdilgenOzelDurumu(alfabe);
                case "GENIS_ZAMAN":
                    return new GenisZamanEkOzelDurumuTr();
                case "SIMDIKI_ZAMAN":
                    return new SimdikiZamanEkOzelDurumuTr(alfabe);
                case "SU":
                    return new SuOzelDurumu();
/*                case "ZAMAN_KI":
                    return new ZamanKiOzelDurumu();*/
            }
            return oz;
        }
    }

    public class TurkceEkOzelDurumTipi : TemelEkOzelDurumuTipi
    {
        public readonly static TemelEkOzelDurumuTipi BERABERLIK_IS = new TurkceEkOzelDurumTipi(TemelEkOzelDurumuTipi.Length, "BERABERLIK_IS");
        public readonly static TemelEkOzelDurumuTipi EDILGEN = new TurkceEkOzelDurumTipi(TemelEkOzelDurumuTipi.Length + 1, "EDILGEN");
        public readonly static TemelEkOzelDurumuTipi GENIS_ZAMAN = new TurkceEkOzelDurumTipi(TemelEkOzelDurumuTipi.Length + 2, "GENIS_ZAMAN");
        public readonly static TemelEkOzelDurumuTipi SIMDIKI_ZAMAN = new TurkceEkOzelDurumTipi(TemelEkOzelDurumuTipi.Length + 3, "SIMDIKI_ZAMAN");
        public readonly static TemelEkOzelDurumuTipi SU = new TurkceEkOzelDurumTipi(TemelEkOzelDurumuTipi.Length + 4, "SU");
//        public readonly static TemelEkOzelDurumuTipi ZAMAN_KI = new TemelEkOzelDurumuTipi(TemelEkOzelDurumuTipi.Boy + 4, "ZAMAN_KI");

        internal TurkceEkOzelDurumTipi(int index, string ad) : base(index, ad)
        {}

        public static new IEkOzelDurumTipi[] AllValues;

        static TurkceEkOzelDurumTipi()
        {
            AllValues = new IEkOzelDurumTipi[TemelEkOzelDurumuTipi.Length + 5];
            Array.Copy(TemelEkOzelDurumuTipi.AllValues,AllValues,TemelEkOzelDurumuTipi.Length);
            AllValues[TemelEkOzelDurumuTipi.Length]=BERABERLIK_IS;
            AllValues[TemelEkOzelDurumuTipi.Length+1]=EDILGEN;
            AllValues[TemelEkOzelDurumuTipi.Length+2]=GENIS_ZAMAN;
            AllValues[TemelEkOzelDurumuTipi.Length+3]=SIMDIKI_ZAMAN;
            AllValues[TemelEkOzelDurumuTipi.Length+4] = SU;
//            AllValues[TemelEkOzelDurumuTipi.Boy+4]= ZAMAN_KI;
        }
    }
}

