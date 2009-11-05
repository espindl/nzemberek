using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using NZemberek.Cekirdek.Yapi;
using NZemberek.Cekirdek.KokSozlugu;
using NZemberek.Cekirdek.Mekanizma;
using NZemberek.Cekirdek.Araclar;
using NZemberek.Cekirdek.Mekanizma.Cozumleme;
using NZemberek.TrTurkcesi.Mekanizma;

namespace NZemberek.TrTurkcesi.Yapi
{
    class TRDilFabrikasi : IDilFabrikasi
    {
        private String dilAdi = "TURKIYE TURKCESI";

        private Alfabe _alfabe;
        private ISozluk sozluk;
        private IDenetlemeCebi _denetlemeCebi;
        private ICozumlemeYardimcisi yardimci;
        private IEkYonetici ekYonetici;
        private IKokOzelDurumYonetici ozelDurumYonetici;
        private IHeceleyici _heceleyici;
        private IEkKuralBilgisi _ekKuralBilgisi;

        private String kaynakDizini;

        private String alfabeDosyaAdi;
        private String ekDosyaAdi;
        private String kokDosyaAdi;
        private String cepDosyaAdi;
        private String kokIstatistikDosyaAdi;

        private bool cepKullan = true;

        public TRDilFabrikasi()
        {
            kaynakDizini = "Kaynaklar";
            alfabeDosyaAdi = kaynakAdresi("harf.txt");
            ekDosyaAdi = kaynakAdresi("ek.xml");
            kokDosyaAdi = kaynakAdresi("kokler.bin");
            cepDosyaAdi = kaynakAdresi("kelime_cebi.txt");
            kokIstatistikDosyaAdi = kaynakAdresi("kok_istatistik.bin");
        }

        private string dosyaAdresi(string dosyaAdi)
        {
            return String.Format("{0}{1}{2}", kaynakDizini, System.IO.Path.DirectorySeparatorChar, dosyaAdi);
        }

        private string kaynakAdresi(string kaynakAdi)
        {
            return String.Format("{0}.{1}.{2}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,kaynakDizini, kaynakAdi);
        }

        #region DilBilgisi Members

        public bool CepKullan
        {
            set { cepKullan = value; }

        }
        
        public Alfabe AlfabeVer()
        {
            if (_alfabe != null) 
            {
                return _alfabe;
            } 
            else
            {
                _alfabe = new Alfabe(alfabeDosyaAdi, "tr");
                return _alfabe;
            }
        }

        public IEkYonetici EkYoneticisiver()
        {
            if (ekYonetici != null) 
            {
                return ekYonetici;
            } 
            else
            {
                EkKuralKelimesiCozumleyici kuralCozumleyici = new EkKuralKelimesiCozumleyici(AlfabeVer(), EkKuralBilgisiVer());
                XmlEkOkuyucu ekOkuyucu = new XmlEkOkuyucu(ekDosyaAdi, new EkUreticiTr(AlfabeVer()), new TurkceEkOzelDurumUretici(AlfabeVer()), kuralCozumleyici);
                ekYonetici = new TemelEkYonetici(baslangiEkAdlari(), ekOkuyucu);
                return ekYonetici;
            } 
        }


        public IEkKuralBilgisi EkKuralBilgisiVer()
        {
            if (_ekKuralBilgisi != null)
            {
                return _ekKuralBilgisi;
            }
            else
            {
                _ekKuralBilgisi = new TemelEkUretimKurali.TemelKuralBilgisi();
                return _ekKuralBilgisi;
            }
        }

        public ISozluk SozlukVer()
        {
            if (sozluk != null)
            {
                return sozluk;
            }

            if (!KaynakYukleyici.KaynakMevcut(Assembly.GetExecutingAssembly(), kokDosyaAdi))
            {
                throw new ApplicationException("Kök dosyası bulunamadı.");
            }
            KokOzelDurumYoneticiVer();
            try
            {
                IKokOkuyucu okuyucu = new IkiliKokOkuyucu(kokDosyaAdi, ozelDurumYonetici);
                okuyucu.Ac();
                sozluk = new AgacSozluk(AlfabeVer(), ozelDurumYonetici, okuyucu);
            }
            catch (Exception e)
            {
                throw new ApplicationException("sozluk uretilemiyor. Hata : " + e.Message);
            }
            return sozluk;
        }

        public IKokOzelDurumYonetici KokOzelDurumYoneticiVer()
        {
            if (ozelDurumYonetici != null)
            {
                return ozelDurumYonetici;
            }
            else
            {
                ozelDurumYonetici = new TurkceKokOzelDurumYonetici(EkYoneticisiver(), AlfabeVer());
                return ozelDurumYonetici;
            }
        }

        public IHeceleyici HeceleyiciVer()
        {
            if (_heceleyici != null)
            {
                return _heceleyici;
            }
            else
            {
                //_heceleyici = new TurkceHeceBulucu(Alfabe());
                _heceleyici = new TRHeceleyici(AlfabeVer());
                return _heceleyici;
            }
        }

        public ICozumlemeYardimcisi CozumlemeYardimcisiVer()
        {
            if (yardimci != null)
            {
                return yardimci;
            }
            else
            {
                yardimci = new TurkceCozumlemeYardimcisi(AlfabeVer());
                return yardimci;
            }
        }

        public IDenetlemeCebi DenetlemeCebiVer()
        {
            if (!cepKullan)
            {
                return null;
            }

            if (_denetlemeCebi != null)
            {
                return _denetlemeCebi;
            }
            else
            {
                try
                {
                    _denetlemeCebi = new BasitDenetlemeCebi(cepDosyaAdi);
                }
                catch (System.IO.IOException e)
                {
                    _denetlemeCebi = null;
                }
            }
            return _denetlemeCebi;
        }

        #endregion

        private Dictionary<KelimeTipi, String> baslangiEkAdlari()
        {
            Dictionary<KelimeTipi, String> baslangicEkAdlari = new Dictionary<KelimeTipi, String>();
            baslangicEkAdlari.Add(KelimeTipi.ISIM, TurkceEkAdlari.ISIM_KOK);
            baslangicEkAdlari.Add(KelimeTipi.SIFAT, TurkceEkAdlari.ISIM_KOK);
            baslangicEkAdlari.Add(KelimeTipi.FIIL, TurkceEkAdlari.FIIL_KOK);
            baslangicEkAdlari.Add(KelimeTipi.ZAMAN, TurkceEkAdlari.ZAMAN_KOK);
            baslangicEkAdlari.Add(KelimeTipi.ZAMIR, TurkceEkAdlari.ZAMIR_KOK);
            baslangicEkAdlari.Add(KelimeTipi.SAYI, TurkceEkAdlari.SAYI_KOK);
            baslangicEkAdlari.Add(KelimeTipi.SORU, TurkceEkAdlari.SORU_KOK);
            baslangicEkAdlari.Add(KelimeTipi.UNLEM, TurkceEkAdlari.UNLEM_KOK);
            baslangicEkAdlari.Add(KelimeTipi.EDAT, TurkceEkAdlari.EDAT_KOK);
            baslangicEkAdlari.Add(KelimeTipi.BAGLAC, TurkceEkAdlari.BAGLAC_KOK);
            baslangicEkAdlari.Add(KelimeTipi.OZEL, TurkceEkAdlari.OZEL_KOK);
            baslangicEkAdlari.Add(KelimeTipi.IMEK, TurkceEkAdlari.IMEK_KOK);
            baslangicEkAdlari.Add(KelimeTipi.YANKI, TurkceEkAdlari.YANKI_KOK);
            baslangicEkAdlari.Add(KelimeTipi.KISALTMA, TurkceEkAdlari.ISIM_KOK);
            return baslangicEkAdlari;
        }
    }
}
