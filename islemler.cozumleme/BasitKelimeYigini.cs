using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;



namespace net.zemberek.islemler.cozumleme
{
    public class BasitKelimeYigini
    {
        private LinkedList<YiginKelime> yigin = new LinkedList<YiginKelime>();

        public YiginKelime al() 
        {
            YiginKelime ret = yigin.First.Value;
            yigin.RemoveFirst();
            return ret;
        }

        public bool bosMu() {
            return (yigin.Count==0);
        }

        public void temizle() {
            yigin.Clear();
        }

        public void koy(Kelime kelime, int ardisilEkSirasi) {
            yigin.AddFirst(new YiginKelime(kelime, ardisilEkSirasi));
        }

        public sealed class YiginKelime 
        {

            private readonly Kelime kelime;
            private readonly int ekSirasi;

            public YiginKelime(Kelime kel, int index) {
                this.kelime = kel;
                this.ekSirasi = index;
            }

            public Kelime getKelime() {
                return kelime;
            }

            public int getEkSirasi() {
                return ekSirasi;
            }

            public override String ToString() {
                return " olusan: " + kelime.icerikStr()
                        + " sonEk: " + kelime.sonEk().ToString()
                        + " ekSira: " + ekSirasi;
            }
        }
    }
}


public class BasitKelimeYigini 
{

}
