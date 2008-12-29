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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using log4net;
using NZemberek.Cekirdek.Kolleksiyonlar;

namespace NZemberek.Cekirdek.Yapi
{
    /// <summary>
    /// xml ek dosyasindan ek bilgilerini okur ve ekleri olusturur
    /// </summary>
    public class XmlEkOkuyucu
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IDictionary<String, HashSet<Ek>> ekKumeleri = new Dictionary<String, HashSet<Ek>>();
        private IDictionary<String, Ek> ekler = new Dictionary<String, Ek>();

        public IDictionary<String, Ek> Ekler
        {
            get { return ekler; }
            set { ekler = value; }
        }

        private readonly String xmlEkDosyasi;
        private readonly IEkUretici ekUretici;
        private readonly Alfabe alfabe;

        private readonly IEkOzelDurumUretici ekOzelDurumUretici;

        public XmlEkOkuyucu(String xmlEkDosyasi, IEkUretici ekUretici,
                            IEkOzelDurumUretici ekOzelDurumUretici, Alfabe alfabe)
        {
            this.xmlEkDosyasi = xmlEkDosyasi;
            this.ekUretici = ekUretici;
            this.ekOzelDurumUretici = ekOzelDurumUretici;
            this.alfabe = alfabe;
        }

        public void XmlOku()
        {
            XmlDocument document = new XmlDocument();
            document.Load(xmlEkDosyasi);

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
                    Exit("Ek tekrari! " + ekadi);
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
                HashSet<Ek> kumeEkleri = new HashSet<Ek>();
                XmlNodeList xmlKumeEkleri = ekKumeEl.SelectNodes("ek");
                foreach (XmlElement ekEl in xmlKumeEkleri)
                {
                    String ekAdi = ekEl.InnerText;//???:GetTextContext
                    Ek ek = this.ekler[ekAdi];
                    if (ek == null) Exit("kume eki bulunamiyor!" + ekAdi);
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
                    Exit("ek uretim kural kelimesi yok!" + ekAdi);

                ek.ArdisilEkler = ArdisilEkleriOlustur(ek, ekElement);
                ek.EkUretici = ekUretici;
                List<EkUretimBileseni> bilesenler = EkUretimKelimesiCozumle(uretimKurali.Value);
                ek.UretimBilesenleri = bilesenler;
                List<EkOzelDurumu> ozelDurumlar = OzelDurumlariOku(ekElement);
                ek.OzelDurumlar = ozelDurumlar;

                EkOzellikleriBelirle(ek, ekElement);
                XmlDisiEkOzellikleriBelirle(ek, bilesenler);
                ek.BaslangicHarfleriEkle(ekUretici.OlasiBaslangicHarfleri(bilesenler));
                foreach (EkOzelDurumu oz in ozelDurumlar)
                {
                    ek.BaslangicHarfleriEkle(ekUretici.OlasiBaslangicHarfleri(oz.UretimBilesenleri));
                }
            }
#if log
            logger.Debug("ek olusumu sonlandi.");
#endif
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
                    oz.UretimBilesenleri = EkUretimKelimesiCozumle(uretimKurali.Value);
                }

                XmlNodeList oneklerElements = element.SelectNodes("on-ek");
                if (oneklerElements != null)
                {
                    HashSet<Ek> onekler = new HashSet<Ek>();
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

            HashSet<Ek> ardisilEkSet = new HashSet<Ek>();
            XmlElement ardisilEklerEl = (XmlElement)ekElement.SelectNodes("ardisil-ekler")[0];
            if (ardisilEklerEl == null) return new List<Ek>();

            // tek ekleri Ekle.
            XmlNodeList tekArdisilEkler = ardisilEklerEl.SelectNodes("aek");
            foreach (XmlElement element in tekArdisilEkler)
            {
                String ekAdi = element.InnerText;
                Ek ek = this.ekler[ekAdi];
                if (ek == null)
                    Exit(anaEk.Ad + " icin ardisil ek bulunamiyor! " + ekAdi);
                ardisilEkSet.Add(ek);
            }

            // kume eklerini Ekle.
            XmlNodeList kumeEkler = ardisilEklerEl.SelectNodes("kume");
            foreach (XmlElement element in kumeEkler)
            {
                String kumeAdi = element.InnerText;
                HashSet<Ek> kumeEkleri = ekKumeleri[kumeAdi];
                if (kumeEkleri == null)
                    Exit("kume bulunamiyor..." + kumeAdi);
                ardisilEkSet.AddAll(kumeEkleri);
            }

            //varsa baska bir ekin ardisil eklerini Kopyala.
            XmlAttribute attr = ardisilEklerEl.GetAttributeNode("kopya-ek");
            if (attr != null)
            {
                String kopyaEkadi = attr.Value;
                Ek ek = this.ekler[kopyaEkadi];
                if (ek == null)
                    Exit(anaEk.Ad + " icin kopyalanacak ek bulunamiyor! " + kopyaEkadi);
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
                    if (ek == null) Exit(anaEk.Ad + " icin oncelikli ek bulunamiyor! " + ekAdi);
                    if (ardisilEkSet.Contains(ek))
                    {
                        ardisilEkler.Add(ek);
                        ardisilEkSet.Remove(ek);
                    }
#if log
                    else logger.Warn(anaEk.Ad + "icin oncelikli ek:" + ekAdi + " bu ekin ardisil eki degil!");
#endif
                }
            }

            ardisilEkler.AddRange(ardisilEkSet);
            return ardisilEkler;
        }

        /// <summary>
        /// ciddi hata durumunda sistmein mesaj vererek yazilimdan cikmasi saglanir.
        /// </summary>
        /// <param name="mesaj"></param>
        private void Exit(String mesaj)
        {
#if log
            logger.Fatal("Ek dosyasi okuma sorunu:" + mesaj);
#endif
            Environment.Exit(1);
        }


        /// <summary>
        /// bazi ek ozellikleri konfigurasyon dosyasinda yer almaz, ekler okunduktan sonra
        /// bilesenlere gore otomatik olarak belirlenir.
        /// </summary>
        /// <param name="ek"></param>
        /// <param name="bilesenler"></param>
        public void XmlDisiEkOzellikleriBelirle(Ek ek, List<EkUretimBileseni> bilesenler)
        {
            for (int i = 0; i < bilesenler.Count; i++)
            {
                EkUretimBileseni uretimBileseni = bilesenler[i];
                TurkceHarf harf = uretimBileseni.Harf;
                if (i == 0 || (i == 1 && bilesenler[0].Kural == EkUretimKurali.KAYNASTIR))
                {
                    if (harf.Sesli)
                        ek.SesliIleBaslayabilir = true;
                    switch (uretimBileseni.Kural)
                    {
                        case EkUretimKurali.SESLI_AA :
                        case EkUretimKurali.SESLI_AE :
                        case EkUretimKurali.SESLI_IU :
                            ek.SesliIleBaslayabilir = true;
                            break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        // ek uretim kural kelimesinde kullanilan parcalarin dilbilgisi kurali karsiliklarini tutan tablo.
        private static readonly IDictionary<Char, EkUretimKurali> kuralTablosu = new Dictionary<Char, EkUretimKurali>();

        static XmlEkOkuyucu()
        {
            kuralTablosu.Add('A', EkUretimKurali.SESLI_AE);
            kuralTablosu.Add('I', EkUretimKurali.SESLI_IU);
            kuralTablosu.Add('E', EkUretimKurali.SESLI_AA);
            kuralTablosu.Add('Y', EkUretimKurali.SESSIZ_Y);
            kuralTablosu.Add('+', EkUretimKurali.KAYNASTIR);
            kuralTablosu.Add('>', EkUretimKurali.SERTLESTIR);
        }

        private readonly HashSet<Char> sesliKurallari = new HashSet<Char>(new Char[]{ 'A', 'I', 'E', 'Y' });
        private readonly HashSet<Char> harfKurallari = new HashSet<Char>(new Char[] { '+', '>' });


        private List<EkUretimBileseni> EkUretimKelimesiCozumle(String uretimKelimesi)
        {
            if (uretimKelimesi == null || uretimKelimesi.Length == 0)
                return new List<EkUretimBileseni>();
            List<EkUretimBileseni> bilesenler = new List<EkUretimBileseni>();
            foreach (EkUretimBileseni bilesen in new EkKuralCozumleyici(uretimKelimesi, this))
            {
                bilesenler.Add(bilesen);
            }
            return bilesenler;
        }

        /**
         * Basit bir tokenizer. Iterable yapidadir, yani kural kelimesine gore
         * her iterasyonda eger varsa yeni bir EkUretimBileseni uretir.
         */
        class EkKuralCozumleyici : IEnumerable<EkUretimBileseni>
        {
            private readonly String uretimKelimesi;
            private XmlEkOkuyucu _okuyucu;

            public EkKuralCozumleyici(String uretimKelimesi, XmlEkOkuyucu okuyucu)
            {
                _okuyucu = okuyucu;
                this.uretimKelimesi = uretimKelimesi.Trim().Replace("[ ]", "");
            }

            #region IEnumerable<EkUretimBileseni> Members

            public IEnumerator<EkUretimBileseni> GetEnumerator()
            {
                return new BilesenIterator(this);
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return new BilesenIterator(this);
            }

            #endregion

            class BilesenIterator : IEnumerator<EkUretimBileseni>
            {
                private int pointer=-1;
                private EkUretimBileseni current;
                EkKuralCozumleyici _enumerable;

                public BilesenIterator(EkKuralCozumleyici enumerable)
                {
                    _enumerable = enumerable;
                }

                #region IEnumerator<EkUretimBileseni> Members

                public EkUretimBileseni Current
                {

                    get
                    {
                        return current;
                    }
                }

                #endregion

                #region IDisposable Members

                public void Dispose()
                {
                    //TODO : Dispose için yapmak gereken biþey var mý?
                }

                #endregion

                #region IEnumerator Members

                object System.Collections.IEnumerator.Current
                {
                    get { return current; }
                }

                public bool MoveNext()
                {
                    pointer++;
                    if (_enumerable.uretimKelimesi == null || pointer >= _enumerable.uretimKelimesi.Length)
                    {
                        current = null;
                        return false;
                    }
                    else
                    {
                        char p = _enumerable.uretimKelimesi[pointer];
                        //ardisil Harf ile iliskili kuralmi
                        if (_enumerable._okuyucu.harfKurallari.Contains(p))
                        {
                            pointer++;
                            if (pointer == _enumerable.uretimKelimesi.Length)
                                throw new ArgumentException(p + " kuralindan sonra normal Harf bekleniyordu!");
                            char h = _enumerable.uretimKelimesi[pointer];
                            if (_enumerable._okuyucu.sesliKurallari.Contains(h))
                                throw new ArgumentException(p + " kuralindan sonra sesli uretim kurali gelemez:" + h);
                            current = new EkUretimBileseni(kuralTablosu[p], _enumerable._okuyucu.alfabe.Harf(h));
                        }
                        else if (_enumerable._okuyucu.sesliKurallari.Contains(p))
                        {
                            current = new EkUretimBileseni(kuralTablosu[p], Alfabe.TANIMSIZ_HARF);
                        }
                        else if (_enumerable._okuyucu.alfabe.Harf(p) != null && Char.IsLower(p))
                        {
                            current = new EkUretimBileseni(EkUretimKurali.HARF, _enumerable._okuyucu.alfabe.Harf(p));
                        }
                        else
                        {
                            throw new ArgumentException(p + "  simgesi cozumlenemiyor.. kelime:" + _enumerable.uretimKelimesi);
                        }
                        return true;
                    }
                }

                public void Reset()
                {
                    pointer = -1;
                    this.MoveNext();
                }

                #endregion
            }

        }
    }
}




