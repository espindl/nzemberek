using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using net.zemberek.yapi;
using net.zemberek.bilgi.kokler;

namespace net.zemberek.istatistik
{
    public class BinaryIstatistikOkuyucu
    {
        protected FileStream fis = null;

        BinaryReader dis;

        public void initialize(String fileName) 
        {
            //reader = new KaynakYukleyici("UTF-8").getReader(fileName);
            dis = new BinaryReader(new FileStream(fileName,FileMode.Open));
        }

        public void oku(Sozluk sozluk) 
        {
    	    int sayac = 0;
            try {
                while (true) 
                {
                    String kokStr = dis.Read().ToString(); //TODO çok saçmaladıııım MERT
                    int frekans = dis.Read();
                    //System.out.println("Kok : " + kokStr + " Freq : " + frekans);
                    ICollection<Kok> col = sozluk.kokBul(kokStr);
                    if (col == null) {
                	    // sonraki koke geÃ§.
                	    continue;
                    }
                    sayac++;
                    // Simdilik tüm es seslilere ayni frekansi veriyoruz.
                    foreach (Kok kok in col) {
                        kok.Frekans = frekans;
                    }
                }
            }
            catch(EndOfStreamException e)
            {
                System.Console.Write("Bitti. Frekansı yazılan kök sayısı: " + sayac);
            }
            finally {
                if (dis != null)
                    dis.Close();
            }
        }
    }
}
