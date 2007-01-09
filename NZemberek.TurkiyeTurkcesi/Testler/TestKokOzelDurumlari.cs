using System;
using System.Collections.Generic;
using System.Text;
using NZemberek.TrTurkcesi.Yapi;
using NZemberek.Cekirdek.Yapi;

using NUnit.Framework;


namespace NZemberek.TurkiyeTurkcesi.Testler
{
    [TestFixture]
    public class TestKokOzelDurumlari
    {
        [Test]
        public void testIlklendirme()
        {
            TRDilFabrikasi fabrika = new TRDilFabrikasi();
            IKokOzelDurumYonetici kokYon = fabrika.KokOzelDurumYoneticiVer();

            int indeks = 0;

            KokOzelDurumu oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.SESSIZ_YUMUSAMASI, oz.Ad);
            Assert.AreEqual(0, oz.GelebilecekEkleriVer().Count);
            Assert.IsTrue(oz.SeslikEkleolusur());
            Assert.IsTrue(oz.YapiBozucu());

            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.SESSIZ_YUMUSAMASI_NK, oz.Ad);
            Assert.AreEqual(0, oz.GelebilecekEkleriVer().Count);
            Assert.IsTrue(oz.SeslikEkleolusur());
            Assert.IsTrue(oz.YapiBozucu());

            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.ISIM_SESLI_DUSMESI, oz.Ad);
            Assert.AreEqual(8, oz.GelebilecekEkleriVer().Count);
            Assert.IsTrue(oz.YapiBozucu());

            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.CIFTLEME, oz.Ad);
            Assert.AreEqual(0, oz.GelebilecekEkleriVer().Count);
            Assert.IsTrue(oz.SeslikEkleolusur());
            Assert.IsTrue(oz.YapiBozucu());

            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.FIIL_ARA_SESLI_DUSMESI, oz.Ad);
            Assert.AreEqual(1, oz.GelebilecekEkleriVer().Count);
            Assert.IsTrue(oz.YapiBozucu());

            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.KUCULTME, oz.Ad);
            Assert.AreEqual(1, oz.GelebilecekEkleriVer().Count);
            Assert.IsTrue(oz.YapiBozucu());

            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.TEKIL_KISI_BOZULMASI, oz.Ad);
            Assert.AreEqual(1, oz.GelebilecekEkleriVer().Count);
            Assert.IsTrue(oz.YapiBozucu());

            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.FIIL_KOK_BOZULMASI, oz.Ad);
            Assert.AreEqual(16, oz.GelebilecekEkleriVer().Count);
            Assert.IsTrue(oz.YapiBozucu());


            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.TERS_SESLI_EK, oz.Ad);
            Assert.AreEqual(0, oz.GelebilecekEkleriVer().Count);
            Assert.IsTrue(oz.Olusabilir(null));//@Tankut : HerZamanOluþur'u kontrol için null ek gönder. 
            Assert.IsTrue(oz.YapiBozucu());

            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.SIMDIKI_ZAMAN, oz.Ad);
            Assert.AreEqual(1, oz.GelebilecekEkleriVer().Count);
            Assert.IsTrue(oz.YapiBozucu());

            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.ISIM_SON_SESLI_DUSMESI, oz.Ad);
            Assert.AreEqual(3, oz.GelebilecekEkleriVer().Count);
            Assert.IsTrue(oz.Secimlik());
            Assert.IsTrue(oz.YapiBozucu());

            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.ZAMIR_SESLI_OZEL, oz.Ad);
            Assert.AreEqual(10, oz.GelebilecekEkleriVer().Count);
            Assert.IsTrue(oz.YapiBozucu());

            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.ISIM_TAMLAMASI, oz.Ad);
            Assert.AreEqual(15, oz.GelebilecekEkleriVer().Count);
            Assert.IsTrue(oz.EkKisitlayici());

            //Boþ özel durumlar
            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.YALIN, oz.Ad);
            Assert.AreEqual(0, oz.GelebilecekEkleriVer().Count);
            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.GENIS_ZAMAN, oz.Ad);
            Assert.AreEqual(0, oz.GelebilecekEkleriVer().Count);
            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.EK_OZEL_DURUMU, oz.Ad);
            Assert.AreEqual(0, oz.GelebilecekEkleriVer().Count);

            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.FIIL_GECISSIZ, oz.Ad);
            Assert.AreEqual(0, oz.GelebilecekEkleriVer().Count);
            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.KAYNASTIRMA_N, oz.Ad);
            Assert.AreEqual(0, oz.GelebilecekEkleriVer().Count);
            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.KESMESIZ, oz.Ad);
            Assert.AreEqual(0, oz.GelebilecekEkleriVer().Count);

            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.TIRE, oz.Ad);
            Assert.AreEqual(0, oz.GelebilecekEkleriVer().Count);
            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.KESME, oz.Ad);
            Assert.AreEqual(0, oz.GelebilecekEkleriVer().Count);
            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.OZEL_IC_KARAKTER, oz.Ad);
            Assert.AreEqual(0, oz.GelebilecekEkleriVer().Count);

            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.ZAMIR_IM, oz.Ad);
            Assert.AreEqual(0, oz.GelebilecekEkleriVer().Count);
            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.ZAMIR_IN, oz.Ad);
            Assert.AreEqual(0, oz.GelebilecekEkleriVer().Count);
            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.KISALTMA_SON_SESLI, oz.Ad);
            Assert.AreEqual(0, oz.GelebilecekEkleriVer().Count);

            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.KISALTMA_SON_SESSIZ, oz.Ad);
            Assert.AreEqual(0, oz.GelebilecekEkleriVer().Count);
            oz = kokYon.OzelDurum(indeks++);
            Assert.AreEqual(TurkceKokOzelDurumYonetici.SU_OZEL_DURUMU, oz.Ad);
            Assert.AreEqual(0, oz.GelebilecekEkleriVer().Count);

        }
    }
}