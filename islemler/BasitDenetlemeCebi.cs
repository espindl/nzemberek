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

        public BasitDenetlemeCebi(String dosyaAdi) 
        {
            StreamReader rd = new KaynakYukleyici("UTF-8").getReader(dosyaAdi);
            try
            {
                cep = new HashedSet<String>();
                while (!rd.EndOfStream)
                {
                    ekle(rd.ReadLine());
                }
            }
            finally
            {
                rd.Close();
            }
        }

        public bool kontrol(String str) 
        {
            return cep.Contains(str);
        }

        public void ekle(String s) 
        {
            lock (this)
            {
                cep.Add(s);
            }
        }
        
        public void sil(String s) 
        {
            lock (this)
            {
                cep.Remove(s);
            }
        }
    }
}
