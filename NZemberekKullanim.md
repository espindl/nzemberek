# NZemberek Kullanım #

NZemberek Kullanımı

1. http://code.google.com/p/nzemberek/downloads/list adresinden NZemberek-0.1a.zip isimli
dosyayı indirin. Arşiv dosyasını dilediğiniz bir klasöre açın.
2. Yeni bir konsol uygulaması projesi yaratın.
3. Proje referanslarına indirdiğiniz arşiv dosyasını açtığınız klasördeki NZemberek.dll
kütüphanesini ekleyin.
4. Konsol uygulaması projenizdeki static void Main(string[.md](.md) args) imzalı metodu bulun ve
içine aşağıdaki kodu kopyalayın. (DenemeIslemleri() metodunu az sonra yaratacağız.)

```
Zemberek zemberek = new Zemberek(new TurkiyeTurkcesi());
ZemberekDeneme.DenemeIslemleri(zemberek);
```

5. Main metodunun bulunduğu dosyanın en başına using belirteçleri altına şu belirteçleri
ekleyin:

```
using net.zemberek.erisim;
using net.zemberek.yapi;
using net.zemberek.tr.yapi;
```

6. Main metodundan sonra aşağıdaki kodu kopyalayın. DenemeIslemleri() metodu zemberek
kütüphanesinin gerçekleştirdiği işlevlerin tümünü görebileceğiniz kodlar içeriyor.
```
        public static void DenemeIslemleri(Zemberek zemberek)
        {
            String giris = "kedilerim";
            System.Console.WriteLine("Giris:" + giris);

            // denetleme
            if (!zemberek.kelimeDenetle(giris))
            {
                System.Console.WriteLine(giris + " kelimesi dogru yazilmamis");
                Environment.Exit(1);
            }
            System.Console.WriteLine(giris + " kelimesi dogru yazilmis.\n");

            // cozumleme
            Kelime[] cozumler = zemberek.kelimeCozumle(giris);
            System.Console.WriteLine("cozumlemeler:");
            foreach (Kelime kelime_ in cozumler)
                System.Console.WriteLine(kelime_);

            //ayristirma
            System.Console.WriteLine("\nayristirma sonuclari:");
            IList<IList<String>> ayrisimlar = zemberek.kelimeAyristir(giris);
            foreach (IList<String> sonuc in ayrisimlar)
            {
                System.Console.Write("[");
                foreach (String str in sonuc)
                {
                    System.Console.Write(str + "-");
                }
                System.Console.WriteLine("]");
            }

            //kelime uretme.5
            //kedi'yi koyun ile degistirelim.. koyun kokunu Kok kok = new Kok("koyun",
KelimeTipi.ISIM);
            //seklinde olusturabilirdik, ama sistemden almak daha dogru
            Kelime kelime = cozumler[0];
            Kok kok = (Kok)zemberek.dilBilgisi().kokler().kokBul("koyun")[0];
            String yeni = zemberek.kelimeUret(kok, kelime.ekler());
            System.Console.WriteLine("\nkok degisimi sonrasi yeni kelime: " + yeni);

            //ascii donusum cozumleme
            String asciiGiris = "koyun";
            System.Console.WriteLine('\n' + asciiGiris + " icin ascii ayristirma sonuclari:");
            Kelime[] asciiCozumler = zemberek.asciiCozumle(asciiGiris);
            foreach (Kelime kelime1 in asciiCozumler)
                System.Console.WriteLine("olasi cozum: " + kelime1);

            //ascii donusum islemini dogrudan String[] donecek sekilde de kullanabiliriz.
            System.Console.WriteLine("\n 'koyun' icin ascii donusum sonuclari:");
            String[] sonuclar = zemberek.asciidenTurkceye("koyun");
            foreach (String s in sonuclar)
                System.Console.WriteLine("olasi cozum: " + s);

            //heceleme.
            String[] heceler = zemberek.hecele(giris);
            Array.ForEach<String>(heceler, HeceYazAction);


            giris = "kirli";
            System.Console.WriteLine("\n" + giris + " icin asciiden geri donusum
olasiliklari:");
            String[] olasiliklar = zemberek.asciidenTurkceye(giris);
            foreach (String s in olasiliklar)
                System.Console.WriteLine("olasi cozum: " + s);

            System.Console.Read();
        }

        private static void HeceYazAction(string str)
        {
            System.Console.WriteLine("\nheceleme sonucu:" + str);
        }
```

7. Şu haliyle kodu build ettiğinizde hata almıyor olmanız gerekiyor. Ama henüz işimiz
bitmedi. İndirdiğiniz arşiv dosyasını açtığınız klasörden  app.config dosyasını ve kaynaklar
dizininin tümünü kopyalayıp proje dizini altındaki bin\debug (Build konfigürasyonunuza
göre bu bin\release'de olabilir) dizini altına kopyalayın.
8. Şimdi projenizi build ettiğinizde oluşan exe dosyasını çift tıklayıp çalıştırabilirsiniz.


Takıldığınız bir nokta ya da farkettiğiniz bir hata durumunda lütfen
#NZemberek Gelistirici Grubu (http://groups.google.com/group/nzemberek_dev) ve
#NZemberek Hata Bildirim Grubu (http://groups.google.com/group/nzemberek_issues) e-posta
gruplarını kullanın