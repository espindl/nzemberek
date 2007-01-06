using System;

namespace NZemberek.Cekirdek.Yapi
{
    public interface IHeceleyici
    {
        int[] heceIndeksleriniBul(string giris);
        string[] hecele(string giris);
        bool hecelenebilirmi(string giris);
    }
}
