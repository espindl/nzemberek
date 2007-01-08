using System;
using System.Collections.Generic;
using System.Text;
using log4net;
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
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private String dilAdi = "TURKIYE TURKCESI";

        private Alfabe _alfabe;
        private ISozluk sozluk;
        private IDenetlemeCebi _cep;
        private ICozumlemeYardimcisi yardimci;
        private IEkYonetici ekYonetici;
        private IKokOzelDurumBilgisi ozelDurumBilgisi;
        private IHeceleyici _heceleyici;

        private String bilgiDizini;

        private String alfabeDosyaAdi;
        private String ekDosyaAdi;
        private String kokDosyaAdi;
        private String cepDosyaAdi;
        private String kokIstatistikDosyaAdi;

        private bool cepKullan = true;

        public TRDilFabrikasi()
        {
            bilgiDizini = "kaynaklar";
            alfabeDosyaAdi = dosyaAdresi("harf.txt");
            ekDosyaAdi = dosyaAdresi("ek.xml");
            kokDosyaAdi = dosyaAdresi("kokler.bin");
            cepDosyaAdi = dosyaAdresi("kelime_cebi.txt");
            kokIstatistikDosyaAdi = dosyaAdresi("kok_istatistik.bin");
        }

        private string dosyaAdresi(string dosyaAdi)
        {
            return String.Format("{0}{1}{2}", bilgiDizini, System.IO.Path.DirectorySeparatorChar, dosyaAdi);
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
                ekYonetici = new TemelEkYonetici(AlfabeVer(), ekDosyaAdi, new EkUreticiTr(AlfabeVer()), new TurkceEkOzelDurumUretici(AlfabeVer()), baslangiEkAdlari());
                return ekYonetici;
            } 
        }

        public ISozluk SozlukVer()
        {
            if (sozluk != null)
            {
                return sozluk;
            }

            if (!KaynakYukleyici.KaynakMevcut(kokDosyaAdi))
            {
                logger.Error("Kök dosyası bulunamadı, sozluk uretilemiyor.");
                throw new ApplicationException("Kök dosyası bulunamadı.");
            }
            KokOzelDurumBilgisiVer();
            logger.Info("Ikili okuyucu uretiliyor:");
            try
            {
                IKokOkuyucu okuyucu = new IkiliKokOkuyucu(kokDosyaAdi, ozelDurumBilgisi);
                logger.Info("Sozluk ve agac uretiliyor:" + dilAdi);
                okuyucu.Ac();
                sozluk = new AgacSozluk(AlfabeVer(), ozelDurumBilgisi, okuyucu);
            }
            catch (Exception e)
            {
                logger.Error("sozluk uretilemiyor. Hata : " + e.Message);
                throw new ApplicationException("sozluk uretilemiyor. Hata : " + e.Message);
            }
            return sozluk;
        }

        public IKokOzelDurumBilgisi KokOzelDurumBilgisiVer()
        {
            if (ozelDurumBilgisi != null)
            {
                return ozelDurumBilgisi;
            }
            else
            {
                ozelDurumBilgisi = new TurkceKokOzelDurumBilgisi(EkYoneticisiver(), AlfabeVer());
                return ozelDurumBilgisi;
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
                yardimci = new TurkceCozumlemeYardimcisi(AlfabeVer(), _cep);
                return yardimci;
            }
        }

        #endregion

        public IDenetlemeCebi cep()
        {
            if (!cepKullan)
            {
                logger.Info("cep kullanilmayacak.");
                return null;
            }

            if (_cep != null)
            {
                return _cep;
            }
            else
            {
                try
                {
                    _cep = new BasitDenetlemeCebi(cepDosyaAdi);
                }
                catch (System.IO.IOException e)
                {
                    logger.Warn("cep dosyasina (" + cepDosyaAdi + ") erisilemiyor. sistem cep kullanmayacak. Hata : " + e.Message);
                    _cep = null;
                }
            }
            return _cep;
        }

        private IDictionary<KelimeTipi, String> baslangiEkAdlari()
        {
            IDictionary<KelimeTipi, String> baslangicEkAdlari = new Dictionary<KelimeTipi, String>();
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
