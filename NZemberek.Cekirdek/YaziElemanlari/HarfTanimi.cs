using System;
using System.Collections.Generic;
using System.Text;

namespace NZemberek.Cekirdek.YaziElemanlari
{
    public class HarfTanimi
    {
        /// <summary>
        /// Büyük harf karþýlýðý karakter.
        /// </summary>
        public char BuyukKarakter;

        /// <summary>
        /// Küçük harf karþýlýðý karakter.
        /// </summary>
        public char KucukKarakter;

        /// <summary>
        /// Ascii dýþý bir harf için Ascii benzeri olan harfi temsil eden karakter.
        /// Harf ascii dýþý deðilse Alfabedeki tanýmsýz karaktere eþittir.
        /// </summary>
        public char AsciiBenzeri;

        /// <summary>
        /// Ascii bir harf için Ascii dýþý benzeri olan harfi temsil eden karakter.
        /// Harf ascii deðilse Alfabedeki tanýmsýz karaktere eþittir.
        /// </summary>
        public char AsciiDisiBenzeri;
    }
}
