using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;


namespace net.zemberek.islemler.cozumleme
{
    public interface KelimeCozumleyici
    {
        Kelime[] cozumle(String strGiris);

        bool denetle(String strGiris);
    }
}
