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
using System.Text;
using System.Collections;
using System.Collections.Generic;

using net.zemberek.islemler;
using net.zemberek.tr.yapi;
using net.zemberek.yapi;
using net.zemberek.yapi.ek;


namespace net.zemberek.tests.kullanim
{
    public class RastgeleKelimeUretici
    {
        private static Random random = new Random();
        ArrayList isimler = new ArrayList(100);
        ArrayList sifatlar = new ArrayList(100);
        ArrayList fiiller = new ArrayList(100);
        EkYonetici ekYonetici;
        KelimeUretici kelimeUretici;
        Alfabe alfabe;


        public RastgeleKelimeUretici() {

            DilBilgisi db = new TurkceDilBilgisi(new TurkiyeTurkcesi());
            alfabe = db.alfabe();
            ekYonetici = db.ekler();
            kelimeUretici = new KelimeUretici(alfabe, db.cozumlemeYardimcisi());

            foreach (Kok kok in db.kokler().tumKokler()) {
                if (kok.tip() == KelimeTipi.ISIM) {
                    isimler.Add(kok);
                } else if (kok.tip() == KelimeTipi.FIIL) {
                    fiiller.Add(kok);
                } else if (kok.tip() == KelimeTipi.SIFAT) {
                    sifatlar.Add(kok);
                }
            }
        }

        public Kok isimSec() {
            int r = random.Next(isimler.Count);
            return (Kok) isimler[r];
        }

        public Kok fiilSec() {
            int r = random.Next(fiiller.Count);
            return (Kok) fiiller[r];
        }

        public Kok sifatSec() {
            int r = random.Next(sifatlar.Count);
            return (Kok) sifatlar[r];
        }

        public IList<Ek> rastgeleEkListesiGetir(IList<Ek> ekler, int limit) {
            if (ekler.Count == limit) {
                return ekler;
            }
            Ek sonEk = (Ek) ekler[ekler.Count - 1];
            IList olasiArdisilEkler = sonEk.ardisilEkler();
            if (olasiArdisilEkler == null || olasiArdisilEkler.Count == 0) {
                return ekler;
            }
            Ek rastgeleEk = (Ek) olasiArdisilEkler[random.Next(olasiArdisilEkler.Count)];
            ekler.Add(rastgeleEk);
            return rastgeleEkListesiGetir(ekler, limit);
        }

        public String rastgeleKelimeOlustur(Kok kok, int maxEkSayisi) {
            Kelime kelime = kelimeUret(kok);
            IList<Ek> girisEkListesi = new List<Ek>();
            girisEkListesi.Add(kelime.sonEk());
            IList<Ek> rastgeleEkler = rastgeleEkListesiGetir(girisEkListesi, maxEkSayisi);
            return kelimeUretici.kelimeUret(kok, rastgeleEkler);
        }

        private Kelime kelimeUret(Kok kok) {
            Kelime kelime = new Kelime(kok, alfabe);
            kelime.ekEkle(ekYonetici.ilkEkBelirle(kelime.kok()));
            return kelime;
        }

        public static void main(String[] args)
        {
            RastgeleKelimeUretici r = new RastgeleKelimeUretici();
            for (int i = 0; i < 30; i++) {
                System.Console.Write(r.rastgeleKelimeOlustur(r.sifatSec(), 1) + " ");
                System.Console.Write(r.rastgeleKelimeOlustur(r.isimSec(), random.Next(3) + 1) + " ");
                System.Console.Write(r.rastgeleKelimeOlustur(r.fiilSec(), random.Next(3) + 1) + " ");
                System.Console.WriteLine("");
            }

        }
    }
}