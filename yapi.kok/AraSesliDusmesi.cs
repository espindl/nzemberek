using System;
using System.Collections.Generic;
using System.Text;

namespace net.zemberek.yapi.kok
{
    /**
     * Basitce harf dizisinin sondan bir onceki harfini siler.
     * User: ahmet
     * Date: Aug 28, 2005
     */
    public class AraSesliDusmesi : HarfDizisiIslemi 
    {
        public void uygula(HarfDizisi dizi) 
        {
            if (dizi.length() >= 2)
                dizi.harfSil(dizi.length() - 2);
        }
    }
}