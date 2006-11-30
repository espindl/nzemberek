using System;
using System.Collections.Generic;
using System.Text;

namespace net.zemberek.islemler
{
    public interface DenetlemeCebi
    {
        bool kontrol(String str);

        void ekle(String s);

        void sil(String s);
    }
}
