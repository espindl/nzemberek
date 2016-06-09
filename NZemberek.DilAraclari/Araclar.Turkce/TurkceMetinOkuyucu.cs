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
 * The Original Code is Zemberek Do�al Dil ��leme K�t�phanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Ak�n, Mehmet D. Ak�n.
 * Portions created by the Initial Developer are Copyright (C) 2006
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Mert Derman
 *   Tankut Tekeli
 * ***** END LICENSE BLOCK ***** */

// V0.1
using System;
using System.Collections;
using System.Text;
using System.IO;

namespace NZemberek.DilAraclari.MetinOkuma
{
    public class TurkceMetinOkuyucu
    {
        public String[] MetinOku(String path)
        {
            String[] kelimeler;
            TurkceSembolAkimi stream = new TurkceSembolAkimi(path, "ISO-8859-9");
            ArrayList list = new ArrayList();
            while (true)
            {
                String str = stream.nextWord();
                if (str == null) break;
                list.Add(str);
            }
            kelimeler = new String[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                kelimeler[i] = (String)list[i];
            }
            return kelimeler;
        }

        public String[] MetinOku(Stream sr)
        {
            String[] kelimeler;
            TurkceSembolAkimi stream = new TurkceSembolAkimi(new StreamReader(sr,System.Text.Encoding.GetEncoding("ISO-8859-9")));
            ArrayList list = new ArrayList();
            while (true)
            {
                String str = stream.nextWord();
                if (str == null) break;
                list.Add(str);
            }
            kelimeler = new String[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                kelimeler[i] = (String)list[i];
            }
            return kelimeler;
        }
    }
}

