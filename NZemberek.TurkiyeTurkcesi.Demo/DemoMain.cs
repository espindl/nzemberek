using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NZemberek.TurkiyeTurkcesi;
using NZemberek.DilAraclari.KokSozlugu;
using NZemberek.Cekirdek.Yapi;
using System.Reflection;

namespace NZemberek.TurkiyeTurkcesi.Demo
{
    public partial class DemoMain : Form
    {
        public DemoMain()
        {
            InitializeComponent();
        }

        private void DemoMain_Load(object sender, EventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ZemberekAyarlari _ayarlar = new ZemberekAyarlari();
            Assembly dilpaketi = Assembly.Load(_ayarlar.DilAyarlari[0]);
            IDilFabrikasi _dilFabrikasi = (IDilFabrikasi)dilpaketi.CreateInstance(_ayarlar.DilAyarlari[1]);
            _dilFabrikasi.CepKullan = _ayarlar.CepKullan;

            List<Kok> tumKokler = new List<Kok>();
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(@"D:\temp\Kaynaklar");
            IKokOzelDurumYonetici kokOzelDurumYoneticiVer = _dilFabrikasi.KokOzelDurumYoneticiVer();
            Alfabe alfabe = _dilFabrikasi.AlfabeVer();
            foreach (System.IO.FileInfo dosya in di.GetFiles())
            {
                DuzYaziKokOkuyucu okuyucu = new DuzYaziKokOkuyucu(
                    dosya.FullName,
                    kokOzelDurumYoneticiVer,
                    alfabe,
                    KelimeTipleriUtil.KelimeTipleri);
                List<Kok> list = okuyucu.HepsiniOku();
                tumKokler.AddRange(list);
            }


            IkiliKokYazici ozelYazici = new IkiliKokYazici(@"D:\temp\mertDeneSozluk.bin");
            ozelYazici.yaz(tumKokler);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Zemberek zem = new Zemberek();
            bool a = zem.KelimeDenetle("suyuyla");
            MessageBox.Show(a.ToString());
        }
    }
}
