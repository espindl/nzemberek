using System;
using System.Collections.Generic;
using System.Text;

namespace net.zemberek.yapi.kok
{
    /**
     * Bu islem sadece saat-ler turu ozel durumlarda kullanilir.
     */
    public class SonSesliIncelt : HarfDizisiIslemi
    {
        Alfabe _alfabe;

        public SonSesliIncelt(Alfabe alfabe)
        {
            this._alfabe = alfabe;
        }

        #region HarfDizisiIslemi Members

        /**
        * en son kalin sesli harfi bulup onu ince formu ile degistirir.
        * ornegin saat -> saAt haline donusur. ince a harfi icin TurkceAlfabe sinifini inceleyin
        *
        * @param dizi
        */
        public void uygula(HarfDizisi dizi)
        {
            for (int i = dizi.Length - 1; i >= 0; i--)
            {
                TurkceHarf h = dizi.harf(i);
                if (h.sesliMi() && !h.inceSesliMi())
                {
                    dizi.harfDegistir(i, _alfabe.kalinSesliIncelt(dizi.harf(i)));
                }
            }
        }

        #endregion
    }
}