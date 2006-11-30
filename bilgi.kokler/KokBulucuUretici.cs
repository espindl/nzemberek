using System;
using System.Collections.Generic;
using System.Text;

namespace net.zemberek.bilgi.kokler
{
    public interface KokBulucuUretici
    {
        KokBulucu getKesinKokBulucu();

        KokBulucu getToleransliKokBulucu(int tolerans);

        KokBulucu getAsciiKokBulucu();
    }
}
