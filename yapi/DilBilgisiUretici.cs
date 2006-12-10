using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using log4net;


namespace net.zemberek.yapi
{
    public class DilBilgisiUretici
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static String TR_SINIF = "net.zemberek.tr.yapi.TurkiyeTurkcesi";
        public static  String TM_SINIF = "net.zemberek.tm.yapi.Turkmence";


        //TODO : Dosya üretim mekanizmalarını ayırmakta fayda var. (@tankut)
        //public static void main(String[] args) 
        //{
        //    if (args.Length > 0) 
        //    {
        //        String dilAdi = args[0].ToLower().Trim();
        //        uret(dilAdi);
        //        Environment.Exit(0);
        //    } 
        //    else 
        //    {
        //        logger.Fatal("Dil adi girmelisiniz (tr,tm,az gibi)");
        //        Environment.Exit(1);
        //    }
        //}

        //public static void uret(String dilAdi) {
        //    DilAyarlari dilAyari = null;

        //    if (dilAdi.Equals("tr"))
        //        dilAyari = dilAyarUret(TR_SINIF);
        //    else if (dilAdi.Equals("tm"))
        //        dilAyari = dilAyarUret(TM_SINIF);
        //    else 
        //    {
        //        logger.Fatal("Dil sinifi bulunamiyor : " + dilAdi);
        //        Environment.Exit(1);
        //    }

        //    new TurkceDilBilgisi(dilAyari).ikiliKokDosyasiUret();


        //}

        public static DilAyarlari dilAyarUret(String sinifadi) {
            Type c = null;
            try 
            {
                c = Type.GetType(sinifadi);
            } 
            catch (ReflectionTypeLoadException e) 
            {
                logger.Fatal("Dil ayarı sinifi yüklenemiyor : " + sinifadi +" Hata : "+e.Message);
                Environment.Exit(1);
            }
            try 
            {
                //TODO buraya dikkat
                return (DilAyarlari)c.GetConstructor(new Type[] { }).Invoke(new object[] { });
            } 
            catch (Exception e) 
            {
                logger.Fatal("Dil ayarı sinifi yüklenemiyor : " + sinifadi +" Hata : "+e.Message);
                Environment.Exit(1);
            }
            return null;
        }
    }
}
