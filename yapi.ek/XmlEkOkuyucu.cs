using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Iesi.Collections.Generic;
using log4net;
using Iesi.Collections;

using net.zemberek.javaporttemp;



namespace net.zemberek.yapi.ek
{
    /**
     * xml ek dosyasindan ek bilgilerini okur ve ekleri olusturur.
     * User: ahmet
     * Date: Aug 15, 2005
     */
    public class XmlEkOkuyucu
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IDictionary<String, Set<Ek>> ekKumeleri = new Dictionary<String, Set<Ek>>();
        private IDictionary<String, Ek> ekler = new Dictionary<String, Ek>();

        private readonly String xmlEkDosyasi;
        private readonly EkUretici ekUretici;
        private readonly Alfabe alfabe;

        private readonly EkOzelDurumUretici ekOzelDurumUretici;

        public XmlEkOkuyucu(String xmlEkDosyasi,
                            EkUretici ekUretici,
                            EkOzelDurumUretici ekOzelDurumUretici,
                            Alfabe alfabe)
        {
            this.xmlEkDosyasi = xmlEkDosyasi;
            this.ekUretici = ekUretici;
            this.ekOzelDurumUretici = ekOzelDurumUretici;
            this.alfabe = alfabe;
        }

        public IDictionary<String, Ek> getEkler()
        {
            return ekler;
        }

        public void xmlOku()
        {
            XmlDocument document = new XmlDocument();
            document.Load(xmlEkDosyasi);

            // kok elemente ulas.
            XmlElement kokElement = document.DocumentElement;

            ilkEkleriOlustur((XmlElement)kokElement.SelectNodes("ekler")[0]);
            ekKumeleriniOlustur((XmlElement)kokElement.SelectNodes("ek-kumeleri")[0]);
            ekleriOlustur((XmlElement)kokElement.SelectNodes("ekler")[0]);
        }

        /**
         * xml dosyadan sadece eklerin adlarini okuyup Ek nesnelerin ilk hallerinin
         * olusturulmasini saglar.
         *
         * @param eklerElement
         */
        private void ilkEkleriOlustur(XmlElement eklerElement)
        {
            XmlNodeList tumEkler = eklerElement.SelectNodes("ek");// XmlYardimcisi.elemanlar(eklerElement, "ek");
            // tum ekleri bos haliyle uret.
            foreach (XmlElement ekElement in tumEkler)
            {
                String ekadi = ekElement.GetAttribute("ad");
                if (ekler.ContainsKey(ekadi))
                    exit("Ek tekrari! " + ekadi);
                ekler.Add(ekadi, new Ek(ekadi));
            }
        }

        /**
         * xml dosyadan ek kumelerini ayiklar. sonuclar ekKumeleri Map'ina atilir.
         *
         * @param ekKumeleriElement
         */
        private void ekKumeleriniOlustur(XmlElement ekKumeleriElement)
        {
            XmlNodeList xmlKumeler = ekKumeleriElement.SelectNodes("ek-kumesi");
            foreach (XmlElement ekKumeEl in xmlKumeler)
            {
                String kumeAdi = ekKumeEl.GetAttribute("ad");
                Set<Ek> kumeEkleri = new HashedSet<Ek>();
                XmlNodeList xmlKumeEkleri = ekKumeEl.SelectNodes("ek");
                foreach (XmlElement ekEl in xmlKumeEkleri)
                {
                    String ekAdi = ekEl.InnerText;//???:GetTextContext
                    Ek ek = this.ekler[ekAdi];
                    if (ek == null) exit("kume eki bulunamiyor!" + ekAdi);
                    kumeEkleri.Add(ek);
                }
                ekKumeleri.Add(kumeAdi, kumeEkleri);
            }
        }

        /**
         * asil ek nesnelerinin olusturulma islemi burada olur.
         * @param eklerElement
         */
        private void ekleriOlustur(XmlElement eklerElement)
        {
            XmlNodeList tumEkler = eklerElement.SelectNodes("ek");
            foreach (XmlElement ekElement in tumEkler)
            {
                String ekAdi = ekElement.GetAttribute("ad");
                Ek ek = this.ekler[ekAdi];
                // uretim kuralini oku ve ekleri uret.
                XmlAttribute uretimKurali = ekElement.GetAttributeNode("uretim");
                if (uretimKurali == null)
                    exit("ek uretim kural kelimesi yok!" + ekAdi);

                ek.setArdisilEkler(ardisilEkleriOlustur(ek, ekElement));
                ek.setEkKuralCozumleyici(ekUretici);
                List<EkUretimBileseni> bilesenler = ekUretimKelimesiCozumle(uretimKurali.Value);
                ek.setUretimBilesenleri(bilesenler);
                List<EkOzelDurumu> ozelDurumlar = ozelDurumlariOku(ekElement);
                ek.setOzelDurumlar(ozelDurumlar);

                ekOzellikleriBelirle(ek, ekElement);
                xmlDisiEkOzellikleriBelirle(ek, bilesenler);
                ek.baslangicHarfleriEkle(ekUretici.olasiBaslangicHarfleri(bilesenler));
                foreach (EkOzelDurumu oz in ozelDurumlar)
                {
                    ek.baslangicHarfleriEkle(ekUretici.olasiBaslangicHarfleri(oz.uretimBilesenleri()));
                }
            }
            logger.Debug("ek olusumu sonlandi.");
        }

        /**
         * HAL ve IYELIK eki ozellikleri burada belirlenir. ek iceriisne farkli ozellikler
         * eklenecekse burasi ona gore degistirilmeli.
         * @param ek
         * @param ekElement
         */
        private void ekOzellikleriBelirle(Ek ek, XmlElement ekElement)
        {
            XmlNodeList ozellikler = ekElement.SelectNodes("ozellik");
            foreach (XmlElement element in ozellikler)
            {
                String ozellik = element.InnerText.Trim();
                if (ozellik.Equals("HAL"))
                    ek.setHalEki(true);
                else if (ozellik.Equals("IYELIK"))
                    ek.setIyelikEki(true);
            }
        }

        private List<EkOzelDurumu> ozelDurumlariOku(XmlElement ekElement)
        {
            List<EkOzelDurumu> ozelDurumlar = new List<EkOzelDurumu>();
            //xml ozel durumlarini al.
            XmlNodeList ozelDurumlarXml = ekElement.SelectNodes("ozel-durum");
            if (ozelDurumlarXml == null) return new List<EkOzelDurumu>();//??? : return Collections.EmptyList

            foreach (XmlElement element in ozelDurumlarXml)
            {
                String ozelDurumAdi = element.GetAttribute("ad");
                EkOzelDurumu oz = ekOzelDurumUretici.uret(ozelDurumAdi);
                XmlAttribute uretimKurali = element.GetAttributeNode("uretim");

                if (uretimKurali != null)
                {
                    oz.setEkKuralCozumleyici(ekUretici);
                    oz.setUretimBilesenleri(ekUretimKelimesiCozumle(uretimKurali.Value));
                }

                XmlNodeList oneklerElements = element.SelectNodes("on-ek");
                if (oneklerElements != null)
                {
                    Set<Ek> onekler = new HashedSet<Ek>();
                    foreach (XmlElement onekEl in oneklerElements)
                    {
                        String onekAdi = onekEl.InnerText;
                        onekler.Add(ekler[onekAdi]);
                    }
                    oz.setOnEkler(onekler);
                }
                ozelDurumlar.Add(oz);
            }
            return ozelDurumlar;
        }

        /**
         * Bir eke iliskin ardisil ekler belirlenir. ardisil ekler
         * a) ek kumelerinden
         * b) normal tek olarak
         * c) dogrudan baska bir ekin ardisil eklerinden kopyalanarak
         * elde edilir.
         * Ayrica eger oncelikli ekler belirtilmis ise bu ekler ardisil ek listeisnin en basina koyulur.
         *
         * @param ekElement :  ek xml bileseni..
         * @return Ek referans Listesi.
         * @param anaEk ardisil ekler eklenecek asil ek
         */
        private List<Ek> ardisilEkleriOlustur(Ek anaEk, XmlElement ekElement)
        {

            Set<Ek> ardisilEkSet = new HashedSet<Ek>();
            XmlElement ardisilEklerEl = (XmlElement)ekElement.SelectNodes("ardisil-ekler")[0];
            if (ardisilEklerEl == null) return new List<Ek>();

            // tek ekleri ekle.
            XmlNodeList tekArdisilEkler = ardisilEklerEl.SelectNodes("aek");
            foreach (XmlElement element in tekArdisilEkler)
            {
                String ekAdi = element.InnerText;
                Ek ek = this.ekler[ekAdi];
                if (ek == null)
                    exit(anaEk.ad() + " icin ardisil ek bulunamiyor! " + ekAdi);
                ardisilEkSet.Add(ek);
            }

            // kume eklerini ekle.
            XmlNodeList kumeEkler = ardisilEklerEl.SelectNodes("kume");
            foreach (XmlElement element in kumeEkler)
            {
                String kumeAdi = element.InnerText;
                Set<Ek> kumeEkleri = ekKumeleri[kumeAdi];
                if (kumeEkleri == null)
                    exit("kume bulunamiyor..." + kumeAdi);
                ardisilEkSet.AddAll(kumeEkleri);
            }

            //varsa baska bir ekin ardisil eklerini kopyala.
            XmlAttribute attr = ardisilEklerEl.GetAttributeNode("kopya-ek");
            if (attr != null)
            {
                String kopyaEkadi = attr.Value;
                Ek ek = this.ekler[kopyaEkadi];
                if (ek == null)
                    exit(anaEk.ad() + " icin kopyalanacak ek bulunamiyor! " + kopyaEkadi);
                ardisilEkSet.AddAll(ek.ardisilEkler());
            }

            List<Ek> ardisilEkler = new List<Ek>(ardisilEkSet.Count);

            //varsa oncelikli ekleri oku ve ardisil ekler listesinin ilk basina koy.
            // bu tamamen performans ile iliskili bir islemdir.
            XmlElement oncelikliEklerEl = (XmlElement)ekElement.SelectNodes("oncelikli-ekler")[0];
            if (oncelikliEklerEl != null)
            {
                XmlNodeList oncelikliEkler = oncelikliEklerEl.SelectNodes("oek");
                foreach (XmlElement element in oncelikliEkler)
                {
                    String ekAdi = element.InnerText;
                    Ek ek = this.ekler[ekAdi];
                    if (ek == null) exit(anaEk.ad() + " icin oncelikli ek bulunamiyor! " + ekAdi);
                    if (ardisilEkSet.Contains(ek))
                    {
                        ardisilEkler.Add(ek);
                        ardisilEkSet.Remove(ek);
                    }
                    else logger.Warn(anaEk.ad() + "icin oncelikli ek:" + ekAdi + " bu ekin ardisil eki degil!");
                }
            }

            ardisilEkler.AddRange(ardisilEkSet);
            return ardisilEkler;
        }

        /**
         * ciddi hata durumunda sistmein mesaj vererek yazilimdan cikmasi saglanir.
         *
         * @param mesaj
         */
        private void exit(String mesaj)
        {
            logger.Fatal("Ek dosyasi okuma sorunu:" + mesaj);
            Environment.Exit(1);
        }


        /**
         * bazi ek ozellikleri konfigurasyon dosyasinda yer almaz, ekler okunduktan sonra
         * bilesenlere gore otomatik olarak belirlenir.
         *
         * @param ek
         * @param bilesenler
         */
        public void xmlDisiEkOzellikleriBelirle(Ek ek, List<EkUretimBileseni> bilesenler)
        {
            for (int i = 0; i < bilesenler.Count; i++)
            {
                EkUretimBileseni uretimBileseni = bilesenler[i];
                TurkceHarf harf = uretimBileseni.harf();
                if (i == 0 || (i == 1 && bilesenler[0].kural() == UretimKurali.KAYNASTIR))
                {
                    if (harf.sesliMi())
                        ek.setSesliIleBaslayabilir(true);
                    switch (uretimBileseni.kural())
                    {
                        case UretimKurali.SESLI_AA :
                        case UretimKurali.SESLI_AE :
                        case UretimKurali.SESLI_IU :
                            ek.setSesliIleBaslayabilir(true);
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
        private static readonly IDictionary<Char, UretimKurali> kuralTablosu = new Dictionary<Char, UretimKurali>();

        static XmlEkOkuyucu()
        {
            kuralTablosu.Add('A', UretimKurali.SESLI_AE);
            kuralTablosu.Add('I', UretimKurali.SESLI_IU);
            kuralTablosu.Add('E', UretimKurali.SESLI_AA);
            kuralTablosu.Add('Y', UretimKurali.SESSIZ_Y);
            kuralTablosu.Add('+', UretimKurali.KAYNASTIR);
            kuralTablosu.Add('>', UretimKurali.SERTLESTIR);
        }

        private readonly Set<Char> sesliKurallari = new HashedSet<Char>(new Char[]{ 'A', 'I', 'E', 'Y' });
        private readonly Set<Char> harfKurallari = new HashedSet<Char>(new Char[] { '+', '>' });


        private List<EkUretimBileseni> ekUretimKelimesiCozumle(String uretimKelimesi)
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
                        //ardisil harf ile iliskili kuralmi
                        if (_enumerable._okuyucu.harfKurallari.Contains(p))
                        {
                            pointer++;
                            if (pointer == _enumerable.uretimKelimesi.Length)
                                throw new ArgumentException(p + " kuralindan sonra normal harf bekleniyordu!");
                            char h = _enumerable.uretimKelimesi[pointer];
                            if (_enumerable._okuyucu.sesliKurallari.Contains(h))
                                throw new ArgumentException(p + " kuralindan sonra sesli uretim kurali gelemez:" + h);
                            current = new EkUretimBileseni(kuralTablosu[p], _enumerable._okuyucu.alfabe.harf(h));
                        }
                        else if (_enumerable._okuyucu.sesliKurallari.Contains(p))
                        {
                            current = new EkUretimBileseni(kuralTablosu[p], Alfabe.TANIMSIZ_HARF);
                        }
                        else if (_enumerable._okuyucu.alfabe.harf(p) != null && Char.IsLower(p))
                        {
                            current = new EkUretimBileseni(UretimKurali.HARF, _enumerable._okuyucu.alfabe.harf(p));
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




