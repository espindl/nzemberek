using System;
using System.Collections.Generic;
using System.IO;

namespace NZemberek.TurkiyeTurkcesi.Test
{
    class TestYardimcisi
    {
        public static List<String> satirlariOku(String dosya)
        {
            List<String> satirlar = new List<String>();
            String s;
            using (TextReader streamReader = new StreamReader(dosya))
            {
                while ((s = streamReader.ReadLine()) != null)
                {
                    if (!s.StartsWith("#") && s.Trim().Length != 0)
                        satirlar.Add(s.Trim());
                }
                streamReader.Close();
            }
            return satirlar;
        }
    }
}
