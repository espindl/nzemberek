using System;
using System.Collections.Generic;
using System.Text;

namespace NZemberek.Cekirdek.YaziElemanlari
{
    public class Sessiz
    {
        public bool Sert;

        /// <summary>
        /// Sert bir Harf i�in benzeri yumu�ak harfi temsil eden karakter.
        /// Harf sert de�ilse Alfabedeki tan�ms�z karaktere e�ittir.
        /// </summary>
        public char YumusakBenzeri;

        /// <summary>
        /// Yumu�ak bir Harf i�in benzeri sert harfi temsil eden karakter.
        /// Harf yumu�ak de�ilse Alfabedeki tan�ms�z karaktere e�ittir.
        /// </summary>
        public char SertBenzeri;
    }
}
