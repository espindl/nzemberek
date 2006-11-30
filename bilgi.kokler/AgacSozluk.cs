using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;
using net.zemberek.yapi.kok;
using net.zemberek.bilgi.araclar;
using net.zemberek.bilgi.kokler;
using Iesi.Collections.Generic;

namespace net.zemberek.bilgi.kokler
{
    public class AgacSozluk : Sozluk
    {
        private KokAgaci agac = null;
    private AgacKokBulucuUretici agacKokBulucuFactory = null;
    private KokOzelDurumBilgisi ozelDurumlar;
    private int indeks = 0;

    /**
     * constructor.
     *
     * @param okuyucu: SÃ¶zlÃ¼kler mutlaka bir sÃ¶zlÃ¼k okuyucu ile ilklendirilir.
     * @param alfabe : KullanÄ±lan TÃ¼rk dili alfabesi
     * @param ozelDurumlar : Dile ait kÃ¶k Ã¶zel durumlarÄ±nÄ± taÅŸÄ±yan nesne
     */
    public AgacSozluk(KokOkuyucu okuyucu, Alfabe alfabe, KokOzelDurumBilgisi ozelDurumlar) {
        this.ozelDurumlar = ozelDurumlar;
        agac = new KokAgaci(new KokDugumu(), alfabe);
        Kok kok;
        while ((kok = okuyucu.oku()) != null) {
            ekle(kok);
        }
        agacKokBulucuFactory = new AgacKokBulucuUretici(this.agac);
    }

    /**
     * Constructor.
     * @param kokler
     * @param alfabe
     * @param ozelDurumlar
     */
    public AgacSozluk(List<Kok> kokler, Alfabe alfabe,
    		KokOzelDurumBilgisi ozelDurumlar) {
        agac = new KokAgaci(new KokDugumu(), alfabe);
        this.ozelDurumlar = ozelDurumlar;
        foreach(Kok kok in kokler){
            ekle(kok);
        }
        agacKokBulucuFactory = new AgacKokBulucuUretici(this.agac);
    }

    /**
     * Verilen bir kÃ¶kÃ¼ sÃ¶zlÃ¼kte arar.
     *
     * @param str: Aranan kÃ¶k
     * @return EÄŸer aranan kÃ¶k varsa, eÅŸ seslileri ile beraber kÃ¶k nesnesini de
     * taÅŸÄ±yan bir List<Kok>, aranan kÃ¶k yoksa null;
     */
    public List<Kok> kokBul(String str) {
        return agac.bul(str);
    }

    public Kok kokBul(String str, KelimeTipi tip) {
        List<Kok> kokler = agac.bul(str);
        foreach (Kok kok in kokler) {
            if(kok.tip()==tip) return kok;
        }
        return null;
    }

    public ICollection<Kok> tumKokler() {
        HashedSet<Kok> set = new HashedSet<Kok>();
        KokAgaciYuruyucu yuruyucu = new KokAgaciYuruyucu(this, set);
        yuruyucu.agaciTara();
        return set;
    }

    /**
     * Verilen kÃ¶kÃ¼ sÃ¶zlÃ¼ÄŸe ekler. Eklemeden once koke ait ozel durumlar varsa bunlar denetlenir.
     * Eger kok ozel durumlari kok yapisini bozacak sekilde ise ozel durumlarin koke uyarlanmis halleride
     * agaca eklenir. bu sekilde bozulmus kok formlarini iceren kelimeler icin kok bulma
     * islemi basari ile gerceklestirilebilir.
     *
     * @param kok: SÃ¶zlÃ¼ÄŸe eklenecek olan kÃ¶k nesnesi.
     */
    public void ekle(Kok kok) {
        kok.Indeks = indeks++;
        agac.ekle(kok.icerik(), kok);
        String[] degismisIcerikler = ozelDurumlar.ozelDurumUygula(kok);
        if (degismisIcerikler.Length > 0) {
            foreach (String degismisIcerik in degismisIcerikler) {
                agac.ekle(degismisIcerik, kok);
            }
        }
    }

    /**
     * @return Returns the agac.
     */
    public KokAgaci getAgac() {
        return agac;
    }

    /**
     * KÃ¶k seÃ§iciler, sÃ¶zlÃ¼kten alÄ±nan bir fabrika ile elde edilirler.
     * Ã–rneÄŸin:
     * <pre>
     * KokBulucu kokSecici = kokler.getKokBulucuFactory().getKesinKokBulucu();
     * </pre>
     */
    public KokBulucuUretici getKokBulucuFactory() {
        return agacKokBulucuFactory;
    }

    /**
     * AÄŸaÃ§ sÃ¶zlÃ¼k iÃ§in fabrika gerÃ§eklemesi
     *
     * @author MDA
     */
    class AgacKokBulucuUretici : KokBulucuUretici {

        KokAgaci agac = null;

        public AgacKokBulucuUretici(KokAgaci agac) {
            this.agac = agac;
        }

        public KokBulucu getKesinKokBulucu() {
            return new KesinKokBulucu(this.agac);
        }

        public KokBulucu getToleransliKokBulucu(int tolerans) {
            return new ToleransliKokBulucu(this.agac, tolerans);
        }

        public KokBulucu getAsciiKokBulucu() {
            return new AsciiKokBulucu(this.agac);
        }
    }
    }
}
