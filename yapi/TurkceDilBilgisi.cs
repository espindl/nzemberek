using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using log4net;
using System.Reflection;
using System.Reflection.Emit;
using net.zemberek.yapi;
using net.zemberek.yapi.ek;
using net.zemberek.yapi.kok;
using net.zemberek.bilgi;
using net.zemberek.bilgi.kokler;
using net.zemberek.islemler;
//using net.zemberek.istatistik;
using net.zemberek.islemler.cozumleme;
using net.zemberek.bilgi.araclar;

namespace net.zemberek.yapi
{
    public class TurkceDilBilgisi : DilBilgisi
    {
        private DilAyarlari dilAyarlari;
        private String dilAdi;

        private Alfabe _alfabe;
        private Sozluk sozluk;
        private DenetlemeCebi _cep;
        private CozumlemeYardimcisi yardimci;
        private EkYonetici ekYonetici;
        private KokOzelDurumBilgisi ozelDurumBilgisi;
        private HeceBulucu heceleyici;

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);		

        private String bilgiDizini;

        private String alfabeDosyaAdi;
        private String ekDosyaAdi;
        private String kokDosyaAdi;
        private String cepDosyaAdi;
        private String kokIstatistikDosyaAdi;

        private bool cepKullan=true;

        /**
         * istenilen dilayarlari nesnesine gore cesitli parametreleri (bilgi dizin adi, kaynak dosyalarin locale
         * uyumlu adlari gibi) olusturur. bilgi dosyalari
         * kaynaklar/<locale>/bilgi/ ana dizini altinda yer almak zorundadir.
         *
         * @param dilAyarlari
         */
        public TurkceDilBilgisi(DilAyarlari dilAyarlari) {

            this.dilAyarlari = dilAyarlari;
            this.dilAdi=dilAyarlari.ad();
            char c = '\\';//File.separatorChar; TODO genellestir
            //TODO MERT buradaki Ietlanguagetag > yerinde  getLanguage() vardı
            bilgiDizini = "kaynaklar" + c + dilAyarlari.locale().TwoLetterISOLanguageName + c + "bilgi" + c;
            alfabeDosyaAdi = dosyaAdiUret("harf", "txt");
            ekDosyaAdi = dosyaAdiUret("ek", "xml");
            kokDosyaAdi = dosyaAdiUret("kokler", "bin");
            cepDosyaAdi = dosyaAdiUret("kelime_cebi", "txt");
            kokIstatistikDosyaAdi = dosyaAdiUret("kok_istatistik", "bin");
        }

        public TurkceDilBilgisi(DilAyarlari inpDilAyarlari, ZemberekAyarlari zemberekAyarlari):this(inpDilAyarlari)
        {
            this.cepKullan = zemberekAyarlari.cepKullan();
        }

        /**
         * kok_<locale>.uzanti dosya adini uretir.
         *
         * @param kok
         * @param uzanti
         * @return olusan kaynak dosyasi adi.
         */
        private String dosyaAdiUret(String kok, String uzanti) {
            return bilgiDizini + kok + '_' + dilAyarlari.locale().TwoLetterISOLanguageName + '.' + uzanti;
        }

        public Alfabe alfabe() {
            if (_alfabe != null) {
                return _alfabe;
            } else
                try {
                    logger.Info("Alfabe uretiliyor:" + dilAdi);
                    Type clazz = dilAyarlari.alfabeSinifi();
                    ConstructorInfo ci = clazz.GetConstructor(new Type[] { typeof(String), typeof(String) });
                    _alfabe = (Alfabe)ci.Invoke(new object[] { alfabeDosyaAdi, dilAyarlari.locale().TwoLetterISOLanguageName });
                } catch (Exception e) {
                    logger.Fatal("Alfabe uretilemiyor. muhtemel dosya erisim hatasi.");
                    e.StackTrace.ToString();
                }
            return _alfabe;
        }

        public EkYonetici ekler() {
            if (ekYonetici != null) {
                return ekYonetici;
            } else {
                alfabe();
                try {
                    logger.Info("Ek yonetici uretiliyor:" + dilAdi);
                    Type clazz = dilAyarlari.ekYoneticiSinifi();
                    ConstructorInfo ci = clazz.GetConstructor(new Type[]{
                        typeof(Alfabe),typeof(String),
                        typeof(EkUretici),typeof(EkOzelDurumUretici),typeof(IDictionary<KelimeTipi,String>)});
                    ekYonetici = (EkYonetici)ci.Invoke(new object[]{
                            _alfabe,
                            ekDosyaAdi,
                            dilAyarlari.ekUretici(_alfabe),
                            dilAyarlari.ekOzelDurumUretici(_alfabe),
                            dilAyarlari.baslangiEkAdlari()});
                } catch (Exception e) {
                    logger.Fatal("ek yonetici sinif uretilemiyor.");
                    e.StackTrace.ToString();
                }
            }
            return ekYonetici;
        }

        /**
         * Sozluk, daha dogrusu Kokleri tasiyan agac ve iliskili kok secicileri tasiyan nesneyi uretir
         * Proje gelistirime asamasinda, eger ikili kok-sozluk dosyasi (kokler_xx.bin) dosyasi mevcut
         * degilse once onu uretmeye calisir, daha sonra asil sozluk uretim islemini yapar.
         * Normal kosullarda dagitim jar icerisinde bu dosya yer alacagindan bu islem (bin dosya uretimi) atlanir.
         *
         * @return Sozluk
         */
        public Sozluk kokler() 
        {
            if (sozluk != null) 
            {
                return sozluk;
            }

            if (!KaynakYukleyici.kaynakMevcutmu(kokDosyaAdi)) 
            {
                logger.Error("Kök dosyası bulunamadı, sozluk uretilemiyor.");
                throw new ApplicationException("Kök dosyası bulunamadı.");
/*                logger.Info("binary kok dosyasi bulunamadi. proje icerisinden calisildigi varsayilarak \n" +
                        "calisilan dizine goreceli olarak '" + kokDosyaAdi + "' dosyasi uretilmeye calisacak.\n" +
                        "eger duz yazki kok bilgilerine erisim saglanamazsa sistem kok bilgisine uretemeycektir. ");
                try {
                    ikiliKokDosyasiUret();
                } catch (System.IO.IOException e) {
                    logger.Fatal("kok bilgilerine erisim saglanamadigindan uygulama calismaya devam edemez. Hata : "+e.Message);
                    Environment.Exit(-1);
                }*/
            }
            kokOzelDurumlari();
            logger.Info("Ikili okuyucu uretiliyor:");
            try
            {
                KokOkuyucu okuyucu = new IkiliKokOkuyucu(kokDosyaAdi, ozelDurumBilgisi);
                logger.Info("Sozluk ve agac uretiliyor:" + dilAdi);
                okuyucu.Ac();
                sozluk = new AgacSozluk(okuyucu, _alfabe, ozelDurumBilgisi);
            }
            catch (Exception e)
            {
                logger.Error("sozluk uretilemiyor. Hata : "+e.Message);
                throw new ApplicationException("sozluk uretilemiyor. Hata : "+e.Message);
            }
            return sozluk;
        }


        public KokOzelDurumBilgisi kokOzelDurumlari() {
            if (ozelDurumBilgisi != null) {
                return ozelDurumBilgisi;
            } else {
                ekler();
                try {
                    Type clazz = dilAyarlari.kokOzelDurumBilgisiSinifi();
                    ConstructorInfo ci = clazz.GetConstructor(new Type[] { typeof(EkYonetici), typeof(Alfabe) });
                    ozelDurumBilgisi = (KokOzelDurumBilgisi)ci.Invoke(new object[] { this.ekler(), this.alfabe() });
                } catch (Exception e) {
                    logger.Fatal("kok ozel durum bilgi nesnesi uretilemiyor.");
                    e.StackTrace.ToString();
                }
            }
            return ozelDurumBilgisi;
        }

        public DenetlemeCebi cep() {
            if(!cepKullan) {
                logger.Info("cep kullanilmayacak.");
                return null;
            }

            if (_cep != null) {
                return _cep;
            } else {
                try {
                    _cep = new BasitDenetlemeCebi(cepDosyaAdi);
                } catch (IOException e) 
                {
                    logger.Warn("cep dosyasina (" + cepDosyaAdi + ") erisilemiyor. sistem cep kullanmayacak. Hata : "+e.Message);
                    _cep = null;
                }
            }
            return _cep;
        }

        public HeceBulucu heceBulucu()
        {
            if (heceleyici != null)
            {
                return heceleyici;
            }
            else
            {
                Type clazz = null;
                alfabe();
                try
                {
                    clazz = dilAyarlari.heceBulucuSinifi();
                    ConstructorInfo ci = clazz.GetConstructor(new Type[] { typeof(Alfabe) });
                    if (ci == null)
                    {
                        ci = clazz.GetConstructor(new Type[] { });
                        heceleyici = (HeceBulucu)ci.Invoke(new object[] { });
                    }
                    else
                    {
                        heceleyici = (HeceBulucu)ci.Invoke(new object[] { _alfabe });
                    }
                }
                catch (Exception e)
                {
                    logger.Warn("heceleyici nesnesi uretilemiyor. heceleme islemi basarisiz olacak.Hata : " + e.Message);
                }
            }
            return heceleyici;
        }


        public CozumlemeYardimcisi cozumlemeYardimcisi() {
            if (yardimci != null) {
                return yardimci;
            } else {
                alfabe();
                cep();
                try {
                    Type clazz = dilAyarlari.cozumlemeYardimcisiSinifi();
                    ConstructorInfo ci = clazz.GetConstructor(new Type[] { typeof(Alfabe), typeof(DenetlemeCebi) });
                    yardimci = (CozumlemeYardimcisi)ci.Invoke(new object[] { _alfabe, _cep });
                } catch (Exception e) {
                    logger.Fatal("cozumleme yardimcisi nesnesi uretilemiyor.");
                    e.StackTrace.ToString();
                }
            }
            return yardimci;
        }

        //TODO : Dosya üretim mekanizmalarını ayırmakta fayda var. (@tankut)
        ///**
        // * Bu metod ile ikili kok bilgisi dosyasi (kokler_xx.bin uretilir.)
        // * Eger uretim sirasinda istatistik bilgisi mevcutsa bu da kullanilir.
        // *
        // * @throws IOException
        // */
        //public void ikiliKokDosyasiUret() {
        //    alfabe();
        //    ekler();
        //    kokOzelDurumlari();
        //    logger.Info("Ikili sozluk dosyasi olusturuluyor...");

        //    //kokleri duz yazi dosyalardan oku
        //    List<Kok> tumKokler = new List<Kok>();
        //    foreach (String dosyaAdi in dilAyarlari.duzYaziKokDosyalari()) 
        //    {
        //        KokOkuyucu okuyucu = new DuzYaziKokOkuyucu(dosyaAdi,ozelDurumBilgisi,_alfabe,dilAyarlari.kokTipiAdlari());
        //        okuyucu.Ac();
        //        List<Kok> list = okuyucu.hepsiniOku();
        //        logger.Info("Okunan kok sayisi: " + list.Count);
        //        foreach (Kok kok in list)
        //        {
        //            tumKokler.Add(kok);
        //        }
        //    }
        //    logger.Info("Toplam kok sayisi:" + tumKokler.Count);

        //    AgacSozluk sozluk = new AgacSozluk(tumKokler, _alfabe, ozelDurumBilgisi);

        //    if (File.Exists(kokIstatistikDosyaAdi)) {
        //        // istatistikleri koklere bagla.
        //        BinaryIstatistikOkuyucu istatistikOkuyucu = new BinaryIstatistikOkuyucu();
        //        istatistikOkuyucu.initialize(kokIstatistikDosyaAdi);
        //        istatistikOkuyucu.oku(sozluk);
        //    } else {
        //        logger.Warn("istatistik dosyasina erisilemedi, kok dosyasi istatistik bilgisi icermeyecek." + kokIstatistikDosyaAdi);
        //    }
        //    // kokleri ikili olarak kaydet.
        //    IkiliKokYazici ozelYazici = new IkiliKokYazici(kokDosyaAdi);
        //    ozelYazici.yaz(tumKokler);
        //}

        ///**
        // * Ana sinif calistiginda ikiliKokDosyasiUret uret sinifini calistirir. Eger parametre olarak
        // * dil ayar sinifi adi gonderilirse iliskili dil icin uretim yapar. aksi halde Turkiye Turkcesi icin
        // * ikili kok-sozluk dosyasini olusturur.
        // *
        // * @param args
        // * @throws Exception
        // */
        //public static void main(String[] args) {

        //    Type c = Type.GetType("net.zemberek.tr.yapi.TurkiyeTurkcesi");
        //    if (args.Length > 0) {
        //        String dilAyarSinifi = args[0];
        //        c = Type.GetType(dilAyarSinifi);
        //    }
        //    new TurkceDilBilgisi((DilAyarlari)Assembly.GetAssembly(Type.GetType("net.zemberek.tr.yapi")).CreateInstance("net.zemberek.tr.yapi.TurkiyeTurkcesi"));
        //}
    }
}
