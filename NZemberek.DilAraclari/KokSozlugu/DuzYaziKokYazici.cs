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
 * The Original Code is Zemberek Doðal Dil Ýþleme Kütüphanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Akýn, Mehmet D. Akýn.
 * Portions created by the Initial Developer are Copyright (C) 2006
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Mert Derman
 *   Tankut Tekeli
 * ***** END LICENSE BLOCK ***** */

/*
 * Created on 06.Nis.2004
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using NZemberek.Cekirdek.Yapi;
using log4net;



namespace NZemberek.DilAraclari.KokSozlugu
{
    /**
     * Verilen bir sözlüðün düzyazý olarak yazýlmasýný saðlar.
     * @author MDA
     */
    public class DuzYaziKokYazici : IKokYazici
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        StreamWriter writer;

        public DuzYaziKokYazici(String dosyaAdi)
        {
            Stream fos = new FileStream(dosyaAdi, FileMode.Create);
            writer = new StreamWriter(fos);
        }

        #region KokYazici Members

        public void yaz(List<Kok> kokler) 
        {
            writer.Write("#-------------------------\n");
            writer.Write("# TSPELL DUZ METIN SOZLUK \n");
            writer.Write("#-------------------------\n");
            writer.Write("#v0.1\n");
            foreach (Kok kok in kokler) 
            {
                writer.Write(getDuzMetinSozlukForm(kok));
                writer.Write(writer.NewLine);
            }
            writer.Close();
        }

        #endregion

        private String getDuzMetinSozlukForm(Kok kok)
        {

        //icerik olarak icerigin varsa asil halini yoksa normal kok icerigini Al.
        String icerik = kok.Icerik;
        if (kok.Asil != null)
            icerik = kok.Asil;

        StringBuilder res = new StringBuilder(icerik);
        res.Append(" ");
        // Tipi ekleyelim.
        if (kok.Tip == KelimeTipi.YOK) 
        {
#if log
            logger.Warn("tipsiz kok:" + kok);
#endif
            return res.ToString();
        }

        res.Append(kok.Tip.ToString());
        res.Append(" ");
        res.Append(getOzellikString(kok.KokOzelDurumlariGetir()));
        return res.ToString();
    }

        private String getOzellikString(string[] ozelDurumlar) 
        {
            String oz = "";
            foreach (string ozelDurum in ozelDurumlar) 
            {
                oz = oz + ozelDurum + " ";
            }
            return oz;
        }

    }
}





