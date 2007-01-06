using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;



namespace NZemberek.Cekirdek.Agac
{
    public class SozAgaci
    {
        private Dugum[] altDugumler;
    }

    public class Dugum
    {
        private Char harf;
        private IDictionary<char, Dugum> altDugumler;
        private Kok kok;

        public int Frekans
        {
            get { }
            set { }
        }

    }

    public class Kok
    {
        private string tip;
        private string icerik;
        private string[] ozelDurumlar;
        

    }
}
