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
