using System;

namespace net.zemberek.yapi
{
    public interface IHeceleyici
    {
        int[] heceIndeksleriniBul(string giris);
        string[] hecele(string giris);
        bool hecelenebilirmi(string giris);
    }
}
