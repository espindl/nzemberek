using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;
using net.zemberek.yapi.ek;
using net.zemberek.tr.yapi.kok;
using net.zemberek.islemler;
using net.zemberek.islemler.cozumleme;

namespace net.zemberek.tr.islemler
{
/**
 * Bu sinif Turkiye Turkcesine ozel cesitli islemleri icerir.
 * User: ahmet
 * Date: Sep 11, 2005
 */
public class TurkceCozumlemeYardimcisi : CozumlemeYardimcisi {

    private Alfabe alfabe;
  //  private EkYonetici ekYonetici;
    private DenetlemeCebi cep;


    public TurkceCozumlemeYardimcisi(Alfabe alfabe,
                                     DenetlemeCebi cep) {
        this.alfabe = alfabe;
        this.cep = cep;
    }

    public void kelimeBicimlendir(Kelime kelime) {
        Kok kok = kelime.kok();
        HarfDizisi olusan = kelime.icerik();
        if (kok.tip().Equals(KelimeTipi.KISALTMA)) {
            //cozumleme sirasinda eklenmis harf varsa onlari sil.
            int silinecek = kok.icerik().Length;
            if (kok.ozelDurumIceriyormu(TurkceKokOzelDurumTipi.KISALTMA_SON_SESSIZ))
                silinecek += 2;
            if (kok.ozelDurumIceriyormu(TurkceKokOzelDurumTipi.KISALTMA_SON_SESLI))
                silinecek++;
            //kelimenin olusan kismindan kokun icereigini sil.
            olusan.harfSil(0, silinecek);
            //simdi kokun orjinal halini ekle.
            olusan.ekle(0, new HarfDizisi(kok.asil(), alfabe));

            if (olusan.Length == kok.asil().Length)
                return;
            //eger gerekiyorsa kesme isareti koy.
            if (!olusan.harf(kok.asil().Length - 1).Equals(alfabe.harf('.')))
                olusan.ekle(kok.asil().Length, alfabe.harf('\''));

        } else if (kok.tip() == KelimeTipi.OZEL) {
            olusan.harfDegistir(0, alfabe.buyukHarf(olusan.ilkHarf()));
            if (kok.ozelDurumIceriyormu(TurkceKokOzelDurumTipi.KESMESIZ))
                return;
            List<Ek> ekler = kelime.ekler();
            if (ekler.Count > 1) {
                Ek ek = (Ek) ekler[1];
                if (ek.iyelikEkiMi() ||ek.halEkiMi()) {
                    int kesmePozisyonu = kok.icerik().Length;
                    olusan.ekle(kesmePozisyonu,alfabe.harf('\''));
                }
            }
        }
        // ozel ic karakter iceren kokler icin bicimleme
/*        if (kok.ozelDurumlar().contains(TurkceKokOzelDurumlari.OZEL_IC_KARAKTER)) {
            //olusan ksimdan koku sil
            int silinecek = kok.icerik().length();
            olusan.harfSil(0, silinecek);
            //simdi kokun orjinal halini ekle.
            olusan.ekle(0, new HarfDizisi(kok.asil()));
        }*/
    }

    public bool kelimeBicimiDenetle(Kelime kelime, String giris) {
        if (giris.Length == 0) return false;
        Kok kok = kelime.kok();
        if (kok.tip().Equals(KelimeTipi.KISALTMA)) {
            // eger giriskokun orjinal hali ile baslamiyorsa hatali demektir.
            String _asil = kok.asil();
            if (!giris.StartsWith(_asil))
                return false;
            if (giris.Equals(_asil))
                return true;
            //burada farkli kisaltma turleri icin kesme ve nokta isaretlerinin
            // dogru olup olmadigina bakiliyor.
            String kalan = giris.Substring(_asil.Length);
            if (_asil[_asil.Length - 1] == '.') {
                return kalan[0] != '\'';
            }
            return kalan[0] == '\'';
        } else if (kelime.kok().tip() == KelimeTipi.OZEL) {
            if (Char.IsLower(giris[0]))
                return false;
            if (kelime.kok().ozelDurumIceriyormu(TurkceKokOzelDurumTipi.KESMESIZ))
                return true;
            List<Ek> ekler = kelime.ekler();
            if (ekler.Count > 1) {
                Ek ek = (Ek) ekler[1];
                if (ek.iyelikEkiMi() || ek.halEkiMi()) {
                    int kesmePozisyonu = kelime.kok().icerik().Length;
                    return kesmePozisyonu <= giris.Length && giris[kesmePozisyonu] == '\'';
                }
            }
        }
        // ozel ic karakter iceren kokler icin bicimleme
/*        if (kok.ozelDurumlar().contains(TurkceKokOzelDurumlari.OZEL_IC_KARAKTER)) {&
            //olusan ksimdan koku sil
            String _asil = kok.asil();
            if (!giris.startsWith(_asil))
              return false;
        }*/
        return true;
    }

    public bool kokGirisDegismiVarsaUygula(Kok kok, HarfDizisi kokDizi, HarfDizisi girisDizi) {
        //turkce'de sadece kisaltmalarda bu metoda ihtiyacimiz var.
        char c = kok.KisaltmaSonSeslisi;
        if (girisDizi.Length == 0) return false;
        if (kok.tip().Equals(KelimeTipi.KISALTMA) && c != 0) {
            TurkceHarf h = alfabe.harf(c);
            //toleransli cozumleyicide kok giristen daha uzun olabiliyor.
            // o nedenle asagidaki kontrolun yapilmasi gerekiyor.
            int kokBoyu = kok.icerik().Length;
            if (kokBoyu <= girisDizi.Length)
                girisDizi.ekle(kokBoyu, h);
            else
                girisDizi.ekle(h);
            kokDizi.ekle(h);
            if (kok.ozelDurumIceriyormu(TurkceKokOzelDurumTipi.KISALTMA_SON_SESSIZ)) {
                //gene toleransli cozumleyicinin hata vermemesi icin asagidaki kontrole ihtiyacimiz var
                if (kokBoyu < girisDizi.Length)
                    girisDizi.ekle(kokBoyu + 1, alfabe.harf('b'));
                else
                    girisDizi.ekle( alfabe.harf('b'));
                kokDizi.ekle( alfabe.harf('b'));
            }
            return true;
        }
        return false;
    }

    public bool cepteAra(String str) {
        return false;
       // return cep != null && cep.kontrol(str);
    }
}

}
