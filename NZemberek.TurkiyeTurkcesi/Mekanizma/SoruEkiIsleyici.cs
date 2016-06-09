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
using System.Collections.Generic;
using System.Text;
using NZemberek.Cekirdek.Yapi;
using NZemberek.TrTurkcesi.Yapi;

namespace NZemberek.TrTurkcesi.Mekanizma
{
    public class SoruEkiIsleyici
    {

        IEkYonetici ekYonetici;


        public SoruEkiIsleyici(IEkYonetici yonetici)
        {
            this.ekYonetici = yonetici;
        }

        /**
         * Gelen kelimler icinde soru "koku" bulursa bunu onceki kelimeye ek olarak ekler.
         * aslnda eklenip eklenemeyeceginin testinin yapilmasi gerekir ama duzgun yazilmis cumlelerde
         * isleyecegini saniyourm
         *
         * @param cumleKelimeleri
         * @return yeni kelime dizisi. soru kokleri eke donustugunden yeni kelime dizisinde bu kokler yer almaz.
         */
        public Kelime[] soruEkiVarsaBirlestir(Kelime[] cumleKelimeleri)
        {
            //soru koku cumleden silineceginden yeni bir diziye gerek var..
            Kelime[] yeniKelimeler = new Kelime[cumleKelimeleri.Length];
            int j = 0;
            //cumle kelimelerini tarayalim bastan sona.
            for (int i = 0; i < cumleKelimeleri.Length; i++)
            {
                Kelime kelime = cumleKelimeleri[i];
                // ilk kelime degilse ve kelime aslinda soru eki ise..
                if (i > 0 && kelime.Kok.Tip.Equals(KelimeTipi.SORU))
                {
                    // onceki kelimeyi Al ve sonuna soru eki Ekle.
                    // daha sonra soru "kokunden" sonra gelen tum ekleri de Ekle.
                    Kelime oncekiKelime = cumleKelimeleri[i - 1];
                    oncekiKelime.Ekler.Add(ekYonetici.EkVer(TurkceEkAdlari.FIIL_SORU_MI));
                    if (kelime.Ekler.Count > 1)
                    {   
                        List<Ek> tempList = kelime.Ekler;
                        tempList.RemoveAt(0);
                        oncekiKelime.Ekler.AddRange(tempList);
                        //oncekiKelime.ekler().addAll(kelime.EkYoneticisiver().subList(1, kelime.EkYoneticisiver().Count));
                    }
                }
                else
                    yeniKelimeler[j++] = kelime;
            }
            return yeniKelimeler;
        }



    }
}
