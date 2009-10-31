using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using NZemberek.Cekirdek.KokSozlugu;
using NZemberek.Cekirdek.Yapi;
using NZemberek.DilAraclari.KokSozlugu;

namespace NZemberek.TurkiyeTurkcesi.Demo
{
    public partial class DemoMain : Form
    {
        public DemoMain()
        {
            InitializeComponent();
        }

        private Zemberek zemberek;
        private void DemoMain_Load(object sender, EventArgs e)
        {
            zemberek = new Zemberek();
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

        private void ShowResults(string[] results)
        {
            lstResults.ForeColor = System.Drawing.Color.Black;
            lstResults.Items.Clear();
            if (results.Length > 0)
            {
                foreach (string s in results)
                {
                    lstResults.Items.Add(s);
                }
            }
            else
            {
                lstResults.Items.Add("Sonuç yok...");
            }
        }

        private void ShowResult(string result)
        {
            lstResults.ForeColor = System.Drawing.Color.Black;
            lstResults.Items.Clear();
            lstResults.Items.Add(result);
        }

        private void ShowResult(string result, bool resultTrue)
        {
            if (resultTrue)
            {
                lstResults.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lstResults.ForeColor = System.Drawing.Color.Red;
            }
            ShowResult(result);
        }

        private bool IsInputValid()
        {
            if (txtEntry.Text.Trim() == "" || txtEntry.Text.Length > 100 || txtEntry.Text.Split(' ').Length > 1)
            {
                MessageBox.Show("Lütfen anlamlı bir giriş yapın.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }


        private void btnConvertToTurkishChars_Click(object sender, EventArgs e)
        {
            if (IsInputValid())
            {
                string[] results = zemberek.TurkceKarakterlereDonustur(txtEntry.Text);
                ShowResults(results);
            }
        }

        private void btnToAscii_Click(object sender, EventArgs e)
        {
            if (IsInputValid())
            {
                ShowResult(zemberek.AsciiKarakterlereDonustur(txtEntry.Text));
            }
        }

        private void btnAnalyse_Click(object sender, EventArgs e)
        {
            if (IsInputValid())
            {
                ShowResults(zemberek.KelimeCozumle(txtEntry.Text));
            }
        }

        private void btnControl_Click(object sender, EventArgs e)
        {
            if (IsInputValid())
            {
                bool result = zemberek.KelimeDenetle(txtEntry.Text);
                ShowResult(result == true ? "geçerli" : "geçersiz", result);
            }
        }

        private void btnAnalyseByAsciiTolerance_Click(object sender, EventArgs e)
        {
            if (IsInputValid())
            {
                ShowResults(zemberek.AsciiToleransliCozumle(txtEntry.Text));
            }
        }

        private void btnFindPossibilities_Click(object sender, EventArgs e)
        {
           
        }

        private void btnSuggest_Click(object sender, EventArgs e)
        {
            if (IsInputValid())
            {
                ShowResults(zemberek.Oner(txtEntry.Text));
            }
        }

        private void btnIsThisTurkish_Click(object sender, EventArgs e)
        {
        }
    }
}
