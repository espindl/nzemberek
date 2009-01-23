using System;
using System.Collections.Generic;
using System.Text;
using NZemberek.Cekirdek.Kolleksiyonlar;

namespace NZemberek.Cekirdek.Yapi
{
    public abstract class  TemelEkUretici : IEkUretici
    {
        public bool SesliIleBaslayabilir(List<EkUretimBileseni> bilesenler) {
        foreach (EkUretimBileseni bilesen in bilesenler) {
            if (bilesen.Kural == TemelEkUretimKurali.KAYNASTIR) continue;
            return bilesen.Harf.Sesli || bilesen.Kural.isSesliUretimKurali();
        }
        return false;
    }

        public HarfDizisi OlusumIcinEkUret(HarfDizisi ulanacak, Ek sonrakiEk, List<EkUretimBileseni> bilesenler)
        {
            //TODO: gecici olarak bu sekilde
            return CozumlemeIcinEkUret(ulanacak, null, bilesenler);
        }

        public HashSet<TurkceHarf> OlasiBaslangicHarfleri(List<EkUretimBileseni> bilesenler)
        {
            return null;
        }

        public abstract HarfDizisi CozumlemeIcinEkUret(HarfDizisi ulanacak, HarfDizisi giris, List<EkUretimBileseni> bilesenler);
    }
}
