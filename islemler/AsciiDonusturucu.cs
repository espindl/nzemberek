using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;


namespace net.zemberek.islemler
{
    public class AsciiDonusturucu
    {
        Alfabe alfabe;

        public AsciiDonusturucu(Alfabe alfabe)
        {
            this.alfabe = alfabe;
        }

        public String toAscii(String giris)
        {
            char[] chars = giris.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                TurkceHarf harf = alfabe.harf(chars[i]);
                if (harf != null && harf != Alfabe.TANIMSIZ_HARF)
                    if (harf.asciiDonusum() != null)
                        chars[i] = harf.asciiDonusum().charDeger();
            }
            return new String(chars);
        }
    }
}

