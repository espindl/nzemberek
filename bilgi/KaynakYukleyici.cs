using System;
using System.IO;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Configuration;
using log4net;

namespace net.zemberek.bilgi
{
    public class KaynakYukleyici
    {
        /**
     * Default constructor. okuma sirasinda sistemde varsayilan kodlama kullanilir.
     */
        public KaynakYukleyici()
        {
            this.encoding = Encoding.Default;
            logger.Info("Kaynak yukleyici olusturuluyor. varsayilan karakter seti:" + encoding);
        }

        public KaynakYukleyici(string enc)
        {
            this.encoding = Encoding.GetEncoding(enc);
            logger.Info("Kaynak yukleyici olusturuluyor. varsayilan karakter seti:" + encoding);
        }

        /**
         * kaynak erisim islemleri verilen encoding ile gerceklestirilir.
         *
         * @param encoding
         */
        public KaynakYukleyici(Encoding encoding)
        {
            this.encoding = encoding;
            logger.Info("Kaynak yukleyici olusturuluyor. varsayilan karakter seti:" + encoding);
        }

        private Encoding encoding;
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);		

        /**
     * UTF metinlerin en basinda BOM adi verilen bir isaret bilgisi yer alabiliyor (UTF-8'de yer
     * almasi gerekmiyor aslinda). Bu bilgi Java tarafindan UTF-8 icin goz ardi edilmiyor.
     * Windows altinda olusturulan duz metinlerde bu bilgi koyuldugu icin Java'da okumada probleme yol acabiliyor.
     * Bu nedenleutf-8 icin BOM bilgisinin yer alip almadiginin denetlenmesi gerekiyor. asagidaki byte dizisi
     * UTF-8 icerisinde yer alan BOM bilgisini ifade ediyor.
     */
    private static byte[] bomBytes = new byte[]{(byte) 0xef, (byte) 0xbb, (byte) 0xbf};

        public NameValueCollection konfigurasyonYukle(String s)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            return appSettings;
        }

        /**
         * eger encoding UTF-8 ise dosyanin icerisinde BOM bilgisinin olup olmadigina bakar.
         * Gelen stream PushBackInputStream'a donusturulur.
         * Eger BOM mevcut degil ise Stream icerisindeki okunan 3 karakter geri gidilir. BOM
         * mevcut ise dogrudan olusturulan stream dondurulur.
         */
        private StreamReader utf8BomDenetle(StreamReader inpStr)
        {

            //if (inpStr == null)
            //    throw new IOException("inputStream is null. throwing exception");
            //if (encoding != null && !encoding.Equals("UTF-8"))
            //    return inpStr;
            //PushbackInputStream pis = new PushbackInputStream(inpStr, bomBytes.Length);
            //byte[] okunanBom = new byte[bomBytes.Length];
            //if (pis.read(okunanBom, 0, bomBytes.length) == -1)
            //{
            //    return inpStr;
            //}
            //if (!Arrays.equals(okunanBom, bomBytes))
            //{
            //    pis.unread(okunanBom);
            //}
            //return pis;
            return null;
        }


     /**
     * properties formatina benzer yapidaki dosyayi kodlamali olarak okur.
     * Normal properties dosyalari ASCII
     * okundugundan turkce karakterlere uygun degil. Dosya icindeki satirlarin
     * anahtar=deger seklindeki satirlardan olusmasi gerekir. dosya icindeki yorumlar
     * # yorum seklinde ifade edilir.
     */
        public IDictionary<String, String> kodlamaliOzellikDosyasiOku(String dosyaAdi)
        {
            StreamReader reader  = null;
            IDictionary<String, String> ozellikler;
            try
            {
                reader = new StreamReader(dosyaAdi, this.encoding);
                ozellikler = new Dictionary<String, String>();
                while (!reader.EndOfStream)
                {
                    String line = reader.ReadLine().Trim();
                    if (line.Length == 0 || line.StartsWith("#"))
                        continue;
                    int esitlik = line.IndexOf('=');
                    if (esitlik == -1)
                        throw new ArgumentException("Ozellik satirinda '=' simgesi bekleniyordu");
                    String key = line.Substring(0, esitlik).Trim();
                    if (line.Length > esitlik - 1)
                        ozellikler.Add(key, line.Substring(esitlik + 1).Trim());
                    else
                        ozellikler.Add(key, "");
                }
                return ozellikler;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        /**
         * istenilen kaynaga erisimin mumkun olup olmadigina bakar. Bazi secimlik kaynaklarin erisiminde
         * bu metoddan yararlanilabilir.
         *
         * @param kaynakAdi
         * @return true-> kaynak erisiminde hata olusmadi false-> kaynak erisiminde hata olustu ya da kaynak=null
         */
        public bool kaynakMevcutmu(String kaynakAdi)
        {
            if (File.Exists(kaynakAdi) )//|| this.GetType().getResource("/" + kaynakAdi) != null)
                return true;
            else
            return false;
        }

        /**
         * Girilen kaynaga once class path disindan erismeye calisir. Eger dosya bulunamazsa
         * bu defa ayni dosyaya classpath icerisinden erismeye calisir
         * (ozellikle jar icinden okumada kullanilir.).
         *
         * @param kaynakAdi
         * @return kaynak risimi icin Buffered reader.
         */
        public StreamReader getReader(String kaynakAdi)
        {
            StreamReader sr = new StreamReader(getStream(kaynakAdi));
            if (sr == null)
                throw new IOException(kaynakAdi + " erisimi saglanamadi, Elde edilen Stream degeri null!");
            return sr; //TODO encoding vardı burda
            //return null;
        }

        /**
         * belirtilen kaynagi Stream olarak once classpath kokunden (jar ise jar icinden) yuklemeye calisir.
         * Eger kaynak bulunamazsa dosya sisteminden yuklemeye calisir (calisilan dizine goreceli olarak.)
         * Onceligi classpath erisimine vermek mantikli cunku dagitimda kaynak erisimi buyuk ihtimalle
         * classpath icerisinden gerceklestirilir.
         */
        public Stream getStream(String kaynakAdi)
        {
            //StreamReader stream = null;
            //try
            //{
            //    // classpath icinden yuklemeye calis.
            //    stream = utf8BomDenetle(this.getClass().getResourceAsStream("/" + kaynakAdi));
            //    log.info("classpath kaynak erisimi saglandi:" + kaynakAdi + " kodlama:" + encoding);
            //}
            //catch (IOException e)
            //{
            //    // proje ici kaynak erisimi yapmaya calis.
            //    stream = utf8BomDenetle(new FileInputStream(kaynakAdi));
            //    if (stream == null)
            //        throw new IOException("Kaynak erisim hatasi: " + kaynakAdi);
            //    log.info("Proje ici kaynak erisimi saglandi:" + kaynakAdi + " kodlama:" + encoding);
            //}
            //return stream;
            return null;
        }

        ///**
        // * Properties dosyasi yukler.
        // */
        //public NameValueCollection konfigurasyonYukle(Uri uri)
        //{
        //    string[] names = ConfigurationManager.AppSettings.g
        //    NameValueCollection appStgs = ConfigurationManager.AppSettings.get;
        //    Properties props = new Properties();
        //    props.load(getStream(uri.toURL().getPath()));
        //    log.info("Dis properties stream erisimi saglandi:" + uri + " kodlama:" + encoding);
        //    return props;
        //}

        //public Properties konfigurasyonYukle(String dosya)
        //{
        //    Properties props = new Properties();
        //    props.load(getStream(dosya));
        //    log.info("properties kaynak erisimi saglandi:" + dosya + " kodlama:" + encoding);
        //    return props;
        //}
    }
}
