using net.zemberek.yapi;

namespace net.zemberek.islemler.cozumleme
{
    public interface HarfDizisiKiyaslayici
    {
         bool kiyasla(HarfDizisi h1, HarfDizisi h2);

         bool bastanKiyasla(HarfDizisi h1, HarfDizisi h2);

         bool aradanKiyasla(HarfDizisi h1, HarfDizisi h2, int baslangic);
    }
}