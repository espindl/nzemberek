using System;
using System.Collections.Generic;
using System.Text;

namespace net.zemberek.araclar.turkce
{
    public class YaziBirimiTipi 
    {
        public static readonly YaziBirimiTipi KELIME = new YaziBirimiTipi("KELIME");
        public static readonly YaziBirimiTipi NOKTALAMA = new YaziBirimiTipi("NOKTALAMA");
        public static readonly YaziBirimiTipi BOSLUK = new YaziBirimiTipi("BOSLUK");
        public static readonly YaziBirimiTipi CUMLE = new YaziBirimiTipi("CUMLE");
        public static readonly YaziBirimiTipi PARAGRAF = new YaziBirimiTipi("PARAGRAF");
        public static readonly YaziBirimiTipi DIGER = new YaziBirimiTipi("DIGER");

        private readonly String myName; // for debug only

        private YaziBirimiTipi(String name) {
            myName = name;
        }

        public override String ToString()
        {
            return myName;
        }
    }
}


