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

using System;
using System.Collections.Generic;
using System.Text;

namespace NZemberek.Cekirdek.Yapi
{
    public class TemelEkOzelDurumUretici : IEkOzelDurumUretici  
    {
        protected Alfabe alfabe;

        public virtual EkOzelDurumu Uret(String ad) 
        {
            if (!Mevcut(TemelEkOzelDurumuTipi.AllValues,ad))
                return null;
            else
                switch (ad) 
                {
                    case "SON_HARF_YUMUSAMA":
                        return new SonHarfYumusamaOzelDurumu();
                    case "OLDURGAN":
                        return new OldurganEkOzelDurumu(alfabe);
                    case "ON_EK":
                        return new OnEkOzelDurumu();
                    case "ZAMAN_KI":
                        return new ZamanKiOzelDurumu();
                    default:
                        return null;
                }
        }

        /**
         * efektif olmayan bir Tip denetimi.
         *
         * @param tipler
         * @param ad
         * @return eger KisaAd ile belirtilen Tip var ise true.
         */
        protected bool Mevcut(IEkOzelDurumTipi[] tipler, String ad) {
            foreach (IEkOzelDurumTipi tip in tipler) {
                if (tip.Ad.Equals(ad))
                    return true;
            }
            return false;
        }

    }

    public class TemelEkOzelDurumuTipi : IEkOzelDurumTipi 
    {
        protected string _ad = string.Empty;
        protected int _index;

        #region IEkOzelDurumTipi Members

        public string  Ad
        {
            get { return _ad; }
        }

        public int  Index
        {
            get { return _index; }
        }

        #endregion


        protected TemelEkOzelDurumuTipi(int index, string ad)
        {
            this._ad = ad;
            this._index = index;
        }

        public readonly static TemelEkOzelDurumuTipi SON_HARF_YUMUSAMA = new TemelEkOzelDurumuTipi(0, "SON_HARF_YUMUSAMA");
        public readonly static TemelEkOzelDurumuTipi OLDURGAN = new TemelEkOzelDurumuTipi(1, "OLDURGAN");
        public readonly static TemelEkOzelDurumuTipi ON_EK = new TemelEkOzelDurumuTipi(2, "ON_EK");
        public readonly static TemelEkOzelDurumuTipi ZAMAN_KI = new TemelEkOzelDurumuTipi(3, "ZAMAN_KI");

        public static IEkOzelDurumTipi[] AllValues = new IEkOzelDurumTipi[] { SON_HARF_YUMUSAMA, OLDURGAN, ON_EK, ZAMAN_KI };
        public static int Length = 4;

    }
}






