using System;
using System.Text;
using System.Collections.Generic;
using net.zemberek.yapi;
using net.zemberek.yapi.ek;

namespace net.zemberek.yapi
{

    public class Kelime : System.ICloneable
    {
        private static HarfDizisi BOS_ICERIK = new HarfDizisi(0);
        private HarfDizisi _icerik = BOS_ICERIK;
        private Kok _kok;
        private List<Ek> _ekler = new List<Ek>(3);
        private KelimeTipi _tip;

        public Kelime()
        {
        }

        public Ek[] ekDizisi()
        {
            return (Ek[])_ekler.ToArray();
        }

        public System.Object Clone()
        {
            Kelime kopya = new Kelime();
            //kok'u kopyalamaya gerek yok. referans esitlemesi yeter
            kopya._kok = _kok;
            kopya._icerik = new HarfDizisi(_icerik);
            kopya._ekler = new List<Ek>(_ekler);
            kopya._tip = this._tip;
            return kopya;
        }

        public List<Ek> ekler()
        {
            return _ekler;
        }

        /**
         * Gosterim amacli bir metod. ek zincirinin anlasilir sekilde yazilmasini saglar.
         *
         * @return ek zinciri dizisi yazimi.
         */
        public String ekZinciriStr()
        {
            StringBuilder buf = new StringBuilder();
            foreach (Ek ek in _ekler)
            {
                buf.Append(ek.ad()).Append(", ");
            }
            if (buf.Length > 2)
                buf.Remove(buf.Length - 2, buf.Length);
            return buf.ToString();
        }

        public Kelime(Kok kok, Alfabe alfabe)
        {
            this._kok = kok;
            _icerik = new HarfDizisi(kok.icerik(), alfabe);
            _tip = kok.tip();
        }

        public Kelime(Kok kok, HarfDizisi dizi)
        {
            this._kok = kok;
            this._icerik = dizi;
            _tip = kok.tip();
        }

        public void setIcerik(HarfDizisi icerik)
        {
            this._icerik = icerik;
        }

        public int ekSayisi()
        {
            return _ekler.Count;
        }

        public TurkceHarf sonHarf()
        {
            return _icerik.sonHarf();
        }

        public HarfDizisi icerik()
        {
            return _icerik;
        }

        public int boy()
        {
            return _icerik.Length;
        }

        public Ek sonEk()
        {
            return _ekler[_ekler.Count - 1];
        }

        public String icerikStr()
        {
            return _icerik.ToString();
        }

        public void ekEkle(Ek ek)
        {
            _ekler.Add(ek);
        }

        public Kok kok()
        {
            return _kok;
        }

        public override String ToString() {
            StringBuilder ekStr = new StringBuilder();
            foreach (Ek ek in _ekler) {
                ekStr.Append(ek.ad()).Append(" + ");
            }
            if (ekStr.Length > 3)
                ekStr.Remove(ekStr.Length - 3, ekStr.Length);
            return "{Icerik: " + _icerik + " Kok: " + _kok.icerik() + " tip:" + _kok.tip() + "} " +
                    " Ekler:" + ekStr;
        }

        public void icerikEkle(HarfDizisi eklenecek)
        {
            _icerik.ekle(eklenecek);
        }

        /**
         * Kelime icerisinde sadece kok ya da kok tipini belirten baslangic eki var ise bu metod
         * true dondurur. Eger baska bir ek eklenmis ise true doner.
         * @return
         */
        public bool gercekEkYok()
        {
            return _ekler.Count < 2;
        }
    }
}