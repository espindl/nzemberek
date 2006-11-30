using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;

namespace net.zemberek.bilgi.kokler
{
    public interface KokBulucu
    {
        List<Kok> getAdayKokler(String giris);
    }
}
