using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;

namespace net.zemberek.bilgi.araclar
{
    public interface KokOkuyucu
    {
        List<Kok> hepsiniOku() ;

        Kok oku();
    }
}
