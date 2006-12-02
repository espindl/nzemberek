using System;
using System.Collections.Generic;
using System.Text;

namespace net.zemberek.araclar.turkce
{
    public class YaziBirimi
    {
        public YaziBirimiTipi tip;
        public String icerik;

        public YaziBirimi(YaziBirimiTipi tip, String icerik)
        {
            this.tip = tip;
            this.icerik = icerik;
        }

        public override String ToString()
        {
            return "{" +
                    "tip=" + tip +
                    ", icerik='" + icerik + "'" +
                    "}";
        }

    }
}



