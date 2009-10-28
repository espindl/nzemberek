using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using NZemberek.Cekirdek.Yapi;
using NZemberek.DilAraclari.KokSozlugu;
using NZemberek.Cekirdek.KokSozlugu;

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
            FileInfo duzyazi = new FileInfo(@"D:\NZemberek.1.0.x\NZemberek.TurkiyeTurkcesi\Kaynaklar\duzyazi-kilavuz.txt");
            FileInfo isimler = new FileInfo(@"D:\NZemberek.1.0.x\NZemberek.TurkiyeTurkcesi\Kaynaklar\kisi-adlari.txt");
            FileInfo kisaltma = new FileInfo(@"D:\NZemberek.1.0.x\NZemberek.TurkiyeTurkcesi\Kaynaklar\kisaltmalar.txt");
            FileInfo bilisim = new FileInfo(@"D:\NZemberek.1.0.x\NZemberek.TurkiyeTurkcesi\Kaynaklar\bilisim.txt");

           // FileInfo[] files = new FileInfo[] { duzyazi, isimler, bilisim, kisaltma};
            FileInfo[] files = new FileInfo[] { duzyazi };
            IKokOzelDurumYonetici kokOzelDurumYoneticiVer = _dilFabrikasi.KokOzelDurumYoneticiVer();
            Alfabe alfabe = _dilFabrikasi.AlfabeVer();
            foreach (FileInfo dosya in files)
            {
                DuzYaziKokOkuyucu okuyucu = new DuzYaziKokOkuyucu(
                    dosya.FullName,
                    kokOzelDurumYoneticiVer,
                    alfabe,
                    KelimeTipleriUtil.KelimeTipleri);
                List<Kok> list = okuyucu.HepsiniOku();
                tumKokler.AddRange(list);
            }

            AgacSozluk sozluk = new AgacSozluk(alfabe, kokOzelDurumYoneticiVer,tumKokler);

            IkiliKokYazici ozelYazici = new IkiliKokYazici(@"D:\temp\mertSozluk.bin");
            ozelYazici.yaz(tumKokler);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Zemberek zemberek = new Zemberek();
            string[] a = zemberek.TurkceKarakterlereDonustur("dugumsuzlukmus");
            MessageBox.Show(a[0]);
        }
    }
}
