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

// V 0.1
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;

using log4net;

using NZemberek.Cekirdek.Yapi;
using NZemberek.Cekirdek.KokSozlugu;
using NZemberek.Cekirdek.Araclar;


namespace NZemberek.DilAraclari.KokSozlugu
{
    /**
 * BinarySozlukOkuyucu sınıfı düzyazı olarak düzenlenmiş sözlüğü okur.
 * Aşağıdaki kod parçası bir binary sözlükteki tüm kökleri okur.
 * <pre>
 * ...
 * KokOkuyucu okuyucu = new DuzYaziKokOkuyucu();
 * okuyucu.Baslat("kaynaklar/kb/duzyazi-kokler.txt");
 * Kok kok = null;
 * while((kok = sozlukOkuyucu.oku())!= null){
 *      Ekle(kok); // Elde edilen kök nesnesi ile ne gerekiyorsa yap.
 * }
 * ...
 * </pre>
 *
 * @author MDA
 */
    public class DuzYaziKokOkuyucu : IKokOkuyucu
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);		
        private Alfabe alfabe;
        private IKokOzelDurumBilgisi ozelDurumlar;
        protected StreamReader reader;
        //private static Pattern AYIRICI_PATTERN = Pattern.compile("[ ]+");
        private char[] AYIRICI_PATTERN = new char[] { ' ' };
        private IDictionary<String, KelimeTipi> _kokTipAdlari = new Dictionary<String,KelimeTipi>();

        string dosyaAdi = string.Empty;


        // Eger farkli turk dillerine ait kok dosyalarinda farkli turden tip adlari 
        // kullanildiysa bu isimleri KelimeITplerine esleyen bir Map olusturulup bu
        // constructor kullanilabilir. Map icin ornek diger constructor icerisinde 
        // yer almaktadir.
        public DuzYaziKokOkuyucu(String pDosyaAdi, IKokOzelDurumBilgisi ozelDurumlar, Alfabe alfabe, IDictionary<String, KelimeTipi> kokTipAdlari)
        {
            dosyaAdi = pDosyaAdi;
            this.ozelDurumlar = ozelDurumlar;
            this.alfabe = alfabe;
            this._kokTipAdlari = kokTipAdlari;
        }


        public List<Kok> HepsiniOku()
        {
            List<Kok> list = new List<Kok>();
            Kok kok;
            while ((kok = Oku()) != null) {
                list.Add(kok);
            }
            if(reader!=null)
                reader.Close();
            return list;
        }

        public Kok Oku() {
            String line;
            while (!reader.EndOfStream ) 
            {
                line = reader.ReadLine().Trim();
                if (line.StartsWith("#") || line.Length == 0) 
                    continue;

                String[] tokens = line.Split(AYIRICI_PATTERN);
                if (tokens == null || tokens.Length < 2) {
                    logger.Warn("Eksik bilgi!" + line);
                    continue;
                }
                String icerik = tokens[0];
                Kok kok = new Kok(icerik);

                // Ayikla() ile kok icerigi kucuk harfe donusturuluyor ve '- vs 
                // isaretler siliniyor.
                kok.Icerik = alfabe.Ayikla(kok.Icerik);

                // kelime tipini belirle. ilk parca mutlaka kok tipini belirler
                if (_kokTipAdlari.ContainsKey(tokens[1])) {
                    KelimeTipi tip = (KelimeTipi) _kokTipAdlari[tokens[1]];
                    kok.Tip = tip;
                    ozelDurumlar.KokIcerigiIsle(kok, tip, icerik);

                } else
                    logger.Warn("Kok tipi bulunamadi!" + line);

                // kok ozelliklerini Ekle.
                ozelDurumlar.DuzyaziOzelDurumOku(kok, icerik, tokens);

                // bazi ozel durumlar ana dosyada yer almaz, algoritma ile uretilir.
                // bu ozel durumlari koke Ekle.
                ozelDurumlar.OzelDurumBelirle(kok);

                return kok;
            }
            this.Kapat();
            return null;
        }

        public void Kapat()
        {
            reader.Close();
            reader=null;
        }

        public void Ac()
        {
            //Reader açıksa hata
            if (reader != null)
            {
                throw new ApplicationException("Kök dosyası zaten açık! : " + dosyaAdi);
            }
            //Dosya yoksa hata
            if (!File.Exists(dosyaAdi))
            {
                throw new ApplicationException("Kök dosyası yok! : " + dosyaAdi);
            }
            reader = new KaynakYukleyici("UTF-8").OkuyucuGetir(dosyaAdi);
            //Dosya boşsa hata 
            if (reader.EndOfStream)
            {
                reader.Close();
                reader = null;
                throw new ApplicationException("Kök dosyası boş! : " + dosyaAdi);
            }
        }
    }
}
