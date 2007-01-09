using System;
using System.Collections.Generic;
using System.Text;

namespace NZemberek.Cekirdek.Benzerlik
{
    public interface IBenzerlikSinayici
    {
        /// <summary>
        /// Verilen iki sozcugun benzerliklerini 0-1 skalasinda doner
        /// </summary>
        /// <param name="sozcuk1">benzerligi sinanacak sozcuklerden biri</param>
        /// <param name="sozcuk2">benzerligi sinanacak sozcuklerden digeri</param>
        /// <returns>0-1 skalasında deger</returns>
        double SozcuklerNeKadarBenzer(String sozcuk1, String sozcuk2);
        
        /// <summary>
        ///  Verilen iki sozcugun yine verilen benzerlik limiti dahilinde olup olmadigini doner
        /// </summary>
        /// <param name="sozcuk1">benzerligi sinanacak sozcuklerden biri</param>
        /// <param name="sozcuk2">benzerligi sinanacak sozcuklerden digeri</param>
        /// <param name="benzerlikLimiti">benzer diyebilmek icin uzerinde olunmasi gereken limit deger</param>
        /// <returns>limit degerden yuksekse dogru degilse yanlis(SozcuklerBenzer ya da SozcuklerBenzerd degil)</returns>
        bool SocuklerBenzer(String sozcuk1, String sozcuk2, double benzerlikLimiti);
    }
}
