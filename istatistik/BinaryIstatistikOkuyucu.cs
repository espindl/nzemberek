/* ***** BEGIN LICENSE BLOCK *****
 * Version: MPL 1.1
 *
 * The contents of this file are subject to the Mozilla Public License Version
 * 1.1 (the "License"); you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * The Original Code is Zemberek Doğal Dil İşleme Kütüphanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Akın, Mehmet D. Akın.
 * Portions created by the Initial Developer are Copyright (C) 2006
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Mert Derman
 *   Tankut Tekeli
 * ***** END LICENSE BLOCK ***** */

/*using System;
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
                	    // sonraki koke geç.
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
}*/
