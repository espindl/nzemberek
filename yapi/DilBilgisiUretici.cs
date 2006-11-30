using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace net.zemberek.yapi
{
    public class DilBilgisiUretici
    {
        public static  String TR_SINIF = "net.zemberek.tr.yapi.TurkiyeTurkcesi";
    public static  String TM_SINIF = "net.zemberek.tm.yapi.Turkmence";

    public static void main(String[] args) {
        if (args.Length > 0) {
            String dilAdi = args[0].ToLower().Trim();
            uret(dilAdi);
            Environment.Exit(0);//System.exit(0);

        } else {
            System.Console.WriteLine("Dil adi girmelisiniz (tr,tm,az gibi)");
            Environment.Exit(1);//System.exit(1);
        }
    }

    public static void uret(String dilAdi) {
        DilAyarlari dilAyari = null;

        if (dilAdi.Equals("tr"))
            dilAyari = dilAyarUret(TR_SINIF);
        else if (dilAdi.Equals("tm"))
            dilAyari = dilAyarUret(TM_SINIF);
        else {
            System.Console.WriteLine("Dil sinifi bulunamiyor:" + dilAdi);
            Environment.Exit(1);//System.exit(1);
        }

        new TurkceDilBilgisi(dilAyari).ikiliKokDosyasiUret();


    }

    public static DilAyarlari dilAyarUret(String sinifadi) {
        Type c = null;
        try {
            c = Type.GetType(sinifadi);
        } catch (ReflectionTypeLoadException e) {
            System.Console.WriteLine("Sinif bulunamadi!:" + sinifadi);
            Environment.Exit(1);//System.exit(1);
        }
        try {
            //TODO buraya dikkat
            return (DilAyarlari)c.GetConstructor(new Type[] { }).Invoke(new object[] { });
        } catch (Exception e) {
            System.Console.Write(e.StackTrace.ToString());
            Environment.Exit(1);//System.exit(1);
        }
        return null;
    }
    }
}
