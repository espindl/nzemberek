using System;
using System.Collections.Generic;
using System.Text;

namespace NZemberek.Cekirdek.YaziElemanlari
{
    public class Sessiz
    {
        public bool Sert;

        /// <summary>
        /// Sert bir harf için benzeri yumuþak harfi temsil eden karakter.
        /// Harf sert deðilse Alfabedeki tanýmsýz karaktere eþittir.
        /// </summary>
        public char YumusakBenzeri;

        /// <summary>
        /// Yumuþak bir harf için benzeri sert harfi temsil eden karakter.
        /// Harf yumuþak deðilse Alfabedeki tanýmsýz karaktere eþittir.
        /// </summary>
        public char SertBenzeri;
    }
}
