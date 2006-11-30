using System;
using Iesi.Collections.Generic;
using System.Text;
using System.IO;
using net.zemberek.bilgi;

namespace net.zemberek.islemler
{
    public class BasitDenetlemeCebi : DenetlemeCebi
    {
            private Set<String> cep;

    public BasitDenetlemeCebi(String dosyaAdi) {
        StreamReader rd = new KaynakYukleyici("UTF-8").getReader(dosyaAdi);
        //TODO cep = new HashedSet<String>(2500);
        cep = new HashedSet<String>();
        while (!rd.EndOfStream) {
            ekle(rd.ReadLine());
        }
        rd.Close();
    }

    public bool kontrol(String str) {
        return cep.Contains(str);
    }

        //TODO synchronized
    public  void ekle(String s) {
        cep.Add(s);
    }
         //TODO synchronized
    public  void sil(String s) {
        cep.Remove(s);
    }
    }
}
