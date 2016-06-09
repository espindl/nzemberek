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
 * The Original Code is Zemberek Do�al Dil ��leme K�t�phanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Ak�n, Mehmet D. Ak�n.
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
using System.Xml;
using NZemberek.Cekirdek.Kolleksiyonlar;
using System.Reflection;

namespace NZemberek.Cekirdek.Yapi
{
    /// <summary>
    /// xml ek dosyasindan ek bilgilerini okur ve ekleri olusturur
    /// </summary>
    public class XmlEkOkuyucu
    {
        private IDictionary<String, Kolleksiyonlar.HashSet<Ek>> ekKumeleri = new Dictionary<String, Kolleksiyonlar.HashSet<Ek>>();
        private IDictionary<String, Ek> ekler = new Dictionary<String, Ek>();

        public IDictionary<String, Ek> Ekler
        {
            get { return ekler; }
            set { ekler = value; }
        }

        private readonly String xmlEkDosyasi;
        private readonly IEkUretici ekUretici;
        private readonly EkKuralKelimesiCozumleyici kuralKelimesiCozumleyici;

        private readonly IEkOzelDurumUretici ekOzelDurumUretici;

        public XmlEkOkuyucu(String xmlEkDosyasi, IEkUretici ekUretici,
                            IEkOzelDurumUretici ekOzelDurumUretici, EkKuralKelimesiCozumleyici ekKuralKelimesiCozumleyici)
        {
            this.xmlEkDosyasi = xmlEkDosyasi;
            this.ekUretici = ekUretici;
            this.ekOzelDurumUretici = ekOzelDurumUretici;
            this.kuralKelimesiCozumleyici = ekKuralKelimesiCozumleyici;
        }

        public void XmlOku(Assembly assembly)
        {
            XmlDocument document = new XmlDocument();
            document.Load(assembly.GetManifestResourceStream(xmlEkDosyasi));

            // kok elemente ulas.
            XmlElement kokElement = document.DocumentElement;

            IlkEkleriOlustur((XmlElement)kokElement.SelectNodes("ekler")[0]);
            EkKumeleriniOlustur((XmlElement)kokElement.SelectNodes("ek-kumeleri")[0]);
            EkleriOlustur((XmlElement)kokElement.SelectNodes("ekler")[0]);
        }

        /// <summary>
        /// xml dosyadan sadece eklerin adlarini okuyup Ek nesnelerin ilk hallerinin olusturulmasini saglar.
        /// </summary>
        /// <param name="eklerElement"></param>
        private void IlkEkleriOlustur(XmlElement eklerElement)
        {
            XmlNodeList tumEkler = eklerElement.SelectNodes("ek");// XmlYardimcisi.elemanlar(eklerElement, "ek");
            // tum ekleri bos haliyle Uret.
            foreach (XmlElement ekElement in tumEkler)
            {
                String ekadi = ekElement.GetAttribute("ad");
                if (ekler.ContainsKey(ekadi))
                    throw new EkKonfigurasyonHatasi("Ek tekrari! " + ekadi);
                ekler.Add(ekadi, new Ek(ekadi));
            }
        }

        /// <summary>
        /// xml dosyadan ek kumelerini ayiklar. sonuclar ekKumeleri Map'ina atilir.
        /// </summary>
        /// <param name="ekKumeleriElement"></param>
        private void EkKumeleriniOlustur(XmlElement ekKumeleriElement)
        {
            XmlNodeList xmlKumeler = ekKumeleriElement.SelectNodes("ek-kumesi");
            foreach (XmlElement ekKumeEl in xmlKumeler)
            {
                String kumeAdi = ekKumeEl.GetAttribute("ad");
                Kolleksiyonlar.HashSet<Ek> kumeEkleri = new Kolleksiyonlar.HashSet<Ek>();
                XmlNodeList xmlKumeEkleri = ekKumeEl.SelectNodes("ek");
                foreach (XmlElement ekEl in xmlKumeEkleri)
                {
                    String ekAdi = ekEl.InnerText;//???:GetTextContext
                    Ek ek = this.ekler[ekAdi];
                    if (ek == null) throw new EkKonfigurasyonHatasi("kume eki bulunamiyor!" + ekAdi);
                    kumeEkleri.Add(ek);
                }
                ekKumeleri.Add(kumeAdi, kumeEkleri);
            }
        }

        /// <summary>
        /// asil ek nesnelerinin olusturulma islemi burada olur.
        /// </summary>
        /// <param name="eklerElement"></param>
        private void EkleriOlustur(XmlElement eklerElement)
        {
            XmlNodeList tumEkler = eklerElement.SelectNodes("ek");
            foreach (XmlElement ekElement in tumEkler)
            {
                String ekAdi = ekElement.GetAttribute("ad");
                Ek ek = this.ekler[ekAdi];
                // uretim kuralini oku ve ekleri Uret.
                XmlAttribute uretimKurali = ekElement.GetAttributeNode("uretim");
                if (uretimKurali == null)
                    throw new EkKonfigurasyonHatasi("ek uretim kural kelimesi yok!" + ekAdi);

                ek.ArdisilEkler = ArdisilEkleriOlustur(ek, ekElement);
                ek.EkUretici = ekUretici;
                List<EkUretimBileseni> bilesenler = kuralKelimesiCozumleyici.cozumle(uretimKurali.Value);
                ek.UretimBilesenleri = bilesenler;
                List<EkOzelDurumu> ozelDurumlar = OzelDurumlariOku(ekElement);
                ek.OzelDurumlar = ozelDurumlar;

                EkOzellikleriBelirle(ek, ekElement);
                ek.SesliIleBaslayabilir = ekUretici.SesliIleBaslayabilir(bilesenler);
                ek.BaslangicHarfleriEkle(ekUretici.OlasiBaslangicHarfleri(bilesenler));
                foreach (EkOzelDurumu oz in ozelDurumlar)
                {
                    ek.BaslangicHarfleriEkle(ekUretici.OlasiBaslangicHarfleri(oz.UretimBilesenleri));
                }
            }
        }

        /// <summary>
        /// HAL ve IYELIK eki ozellikleri burada belirlenir. ek iceriisne farkli ozellikler
        /// eklenecekse burasi ona gore degistirilmeli.
        /// </summary>
        /// <param name="ek"></param>
        /// <param name="ekElement"></param>
        private void EkOzellikleriBelirle(Ek ek, XmlElement ekElement)
        {
            XmlNodeList ozellikler = ekElement.SelectNodes("Ozellik");
            foreach (XmlElement element in ozellikler)
            {
                String ozellik = element.InnerText.Trim();
                if (ozellik.Equals("HAL"))
                    ek.HalEki =true;
                else if (ozellik.Equals("IYELIK"))
                    ek.IyelikEki=true;
            }
        }

        private List<EkOzelDurumu> OzelDurumlariOku(XmlElement ekElement)
        {
            List<EkOzelDurumu> ozelDurumlar = new List<EkOzelDurumu>();
            //xml ozel durumlarini Al.
            XmlNodeList ozelDurumlarXml = ekElement.SelectNodes("ozel-durum");
            if (ozelDurumlarXml == null) return new List<EkOzelDurumu>();//??? : return Collections.EmptyList

            foreach (XmlElement element in ozelDurumlarXml)
            {
                String ozelDurumAdi = element.GetAttribute("ad");
                EkOzelDurumu oz = ekOzelDurumUretici.Uret(ozelDurumAdi);
                XmlAttribute uretimKurali = element.GetAttributeNode("uretim");

                if (uretimKurali != null)
                {
                    oz.EkUretici = ekUretici;
                    oz.UretimBilesenleri = kuralKelimesiCozumleyici.cozumle(uretimKurali.Value);
                }

                XmlNodeList oneklerElements = element.SelectNodes("on-ek");
                if (oneklerElements != null)
                {
                    Kolleksiyonlar.HashSet<Ek> onekler = new Kolleksiyonlar.HashSet<Ek>();
                    foreach (XmlElement onekEl in oneklerElements)
                    {
                        String onekAdi = onekEl.InnerText;
                        onekler.Add(ekler[onekAdi]);
                    }
                    oz.OnEkler = onekler;
                }
                ozelDurumlar.Add(oz);
            }
            return ozelDurumlar;
        }

        /// <summary>
        /// Bir eke iliskin ardisil ekler belirlenir. ardisil EkYoneticisiver
        /// a) ek kumelerinden
        /// b) normal tek olarakc) dogrudan baska bir ekin ardisil eklerinden kopyalanarakelde edilir.
        /// Ayrica eger oncelikli ekler belirtilmis ise bu EkYoneticisiver ardisil ek listeisnin en basina koyulur.
        /// </summary>
        /// <param name="anaEk">rdisil ekler eklenecek asil ek</param>
        /// <param name="ekElement">ek xml bileseni</param>
        /// <returns>Ek referans Listesi</returns>
        private List<Ek> ArdisilEkleriOlustur(Ek anaEk, XmlElement ekElement)
        {

            Kolleksiyonlar.HashSet<Ek> ardisilEkSet = new Kolleksiyonlar.HashSet<Ek>();
            XmlElement ardisilEklerEl = (XmlElement)ekElement.SelectNodes("ardisil-ekler")[0];
            if (ardisilEklerEl == null) return new List<Ek>();

            // tek ekleri Ekle.
            XmlNodeList tekArdisilEkler = ardisilEklerEl.SelectNodes("aek");
            foreach (XmlElement element in tekArdisilEkler)
            {
                String ekAdi = element.InnerText;
                Ek ek = this.ekler[ekAdi];
                if (ek == null)
                    throw new EkKonfigurasyonHatasi(anaEk.Ad + " icin ardisil ek bulunamiyor! " + ekAdi);
                ardisilEkSet.Add(ek);
            }

            // kume eklerini Ekle.
            XmlNodeList kumeEkler = ardisilEklerEl.SelectNodes("kume");
            foreach (XmlElement element in kumeEkler)
            {
                String kumeAdi = element.InnerText;
                Kolleksiyonlar.HashSet<Ek> kumeEkleri = ekKumeleri[kumeAdi];
                if (kumeEkleri == null)
                    throw new EkKonfigurasyonHatasi("kume bulunamiyor..." + kumeAdi);
                ardisilEkSet.AddAll(kumeEkleri);
            }

            //varsa baska bir ekin ardisil eklerini Kopyala.
            XmlAttribute attr = ardisilEklerEl.GetAttributeNode("kopya-ek");
            if (attr != null)
            {
                String kopyaEkadi = attr.Value;
                Ek ek = this.ekler[kopyaEkadi];
                if (ek == null)
                    throw new EkKonfigurasyonHatasi(anaEk.Ad + " icin kopyalanacak ek bulunamiyor! " + kopyaEkadi);
                ardisilEkSet.AddAll(ek.ArdisilEkler);
            }

            List<Ek> ardisilEkler = new List<Ek>(ardisilEkSet.Count);

            //varsa oncelikli ekleri oku ve ardisil ekler listesinin ilk basina Koy.
            // bu tamamen performans ile iliskili bir islemdir.
            XmlElement oncelikliEklerEl = (XmlElement)ekElement.SelectNodes("oncelikli-ekler")[0];
            if (oncelikliEklerEl != null)
            {
                XmlNodeList oncelikliEkler = oncelikliEklerEl.SelectNodes("oek");
                foreach (XmlElement element in oncelikliEkler)
                {
                    String ekAdi = element.InnerText;
                    Ek ek = this.ekler[ekAdi];
                    if (ek == null) throw new EkKonfigurasyonHatasi(anaEk.Ad + " icin oncelikli ek bulunamiyor! " + ekAdi);
                    if (ardisilEkSet.Contains(ek))
                    {
                        ardisilEkler.Add(ek);
                        ardisilEkSet.Remove(ek);
                    }
                }
            }

            ardisilEkler.AddRange(ardisilEkSet);
            return ardisilEkler;
        }




    }
}




