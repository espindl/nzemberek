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
using System.Text;
using System.IO;

using NZemberek.Cekirdek.Araclar;
using NZemberek.Cekirdek.Kolleksiyonlar;


namespace NZemberek.Cekirdek.Mekanizma
{
    public class BasitDenetlemeCebi : IDenetlemeCebi
    {
        private HashSet<String> cep;

        public BasitDenetlemeCebi(String dosyaAdi) 
        {
            StreamReader rd = new KaynakYukleyici("UTF-8").OkuyucuGetir(dosyaAdi);
            try
            {
                cep = new HashSet<String>();
                while (!rd.EndOfStream)
                {
                    Ekle(rd.ReadLine());
                }
            }
            finally
            {
                rd.Close();
            }
        }

        public bool Kontrol(String str) 
        {
            return cep.Contains(str);
        }

        public void Ekle(String s) 
        {
            lock (this)
            {
                cep.Add(s);
            }
        }
        
        public void Sil(String s) 
        {
            lock (this)
            {
                cep.Remove(s);
            }
        }
    }
}
