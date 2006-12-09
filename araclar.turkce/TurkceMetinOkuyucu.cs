using System;
using System.Collections;
using System.Text;
using net.zemberek.istatistik;
using log4net;


namespace net.zemberek.araclar.turkce
{
    public class TurkceMetinOkuyucu
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);		
        //private Istatistikler istatistikler = null;

        public String[] MetinOku(String path) {
        String[] kelimeler;
        TurkishTokenStream stream = new TurkishTokenStream(path, "ISO-8859-9");
        //if (istatistikler != null) {
        //    stream.setStatistics(istatistikler);
        //}
        ArrayList list = new ArrayList();
        while (true) {
            String str = stream.nextWord();
            if (str == null) break;
            list.Add(str);
        }
        if (logger.IsInfoEnabled)
            logger.Info(" Metin kelime sayisi : " + list.Count);
        kelimeler = new String[list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            kelimeler[i] = (String) list[i];
        }
        return kelimeler;
    }

        //public void setStatistics(Istatistikler istatistikler)
        //{
        //    this.istatistikler = istatistikler;
        //}
    }
}

