using System;
using System.Reflection;
using Iesi.Collections.Generic;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;
using net.zemberek.araclar;
using net.zemberek.bilgi.araclar;
using log4net;


namespace net.zemberek.bilgi.kokler
{
    public class KokAgaciYuruyucu
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);		
        int walkCount = 0;
        int distanceCalculationCount = 0;
        private Sozluk sozluk;

        int dugumSayisi = 0;
        int kokTasiyanDugumSayisi = 0;
        int esSesliTasiyanDugumSayisi = 0;
        int ucDugumSayisi  = 0;
        int[] dugumSayilari = new int[50];
        Set<Kok> set;

        /**
         *
         * @param sozluk Taranacak sozluk
         * @param set Yurume sırasında bulunan düğümler toplanmak istiyorsa buraya bir set gönderilir.
         * istenmiyorsa null verilir.
         */
        public KokAgaciYuruyucu(Sozluk sozluk, Set<Kok> set)
        {
            this.sozluk =sozluk;
            this.set = set;
        }

        public int getWalkCount()
        {
            return walkCount;
        }

        public void agaciTara(){
            walk(sozluk.getAgac().getKokDugumu(), "");
        }

        public void walk(KokDugumu dugum, String olusan)
        {
            String tester = (olusan + dugum.getHarf()).Trim();
            walkCount++;
            if (dugum != null){
                dugumSayisi++;
                if(dugum.getKok() != null){
    /*            	if (dugum.getKelime() != null &&
            			    !dugum.getKelime().equals(dugum.getKok().icerik())){
            		    System.out.println("!!!!! " + dugum.getKelime() + " - " + dugum.getKok().icerik());
            	    }*/
                    kokTasiyanDugumSayisi++;
                    if(set != null){
                        set.Add(dugum.getKok());
                    }
                }
                if(dugum.getEsSesliler() != null){
                    esSesliTasiyanDugumSayisi++;
                    if(set!= null){
                	    set.AddAll(dugum.getEsSesliler());
                    }
                }
                if(!dugum.altDugumVarMi()){
                    ucDugumSayisi++;
                }else{
                    KokDugumu[] altDugumler = dugum.altDugumDizisiGetir();
                    int top = 0;
                    foreach (KokDugumu altDugum in altDugumler) {
                        if(altDugum != null) top++;
                    }
                    dugumSayilari[top]++;
                }
            }
            KokDugumu[] altDugumlerX = dugum.altDugumDizisiGetir();
            if (altDugumlerX != null){
        	    foreach (KokDugumu altDugum in altDugumlerX){
        		    if (altDugum != null){
        			    this.walk(altDugum, tester);
        		    }
        	    }
            }
        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Toplam yurume sayisi (walks) " + walkCount + "\n");
            sb.Append("Toplam Dugum Sayisi: " + dugumSayisi + "\n");
            sb.Append("Kok tasiyan dugum sayisi: " + kokTasiyanDugumSayisi + "\n");
            sb.Append("Es sesli tasiyan dugum sayisi: " + esSesliTasiyanDugumSayisi + "\n");
            sb.Append("Alt dugumu olan dugum sayisi:" + (dugumSayisi - ucDugumSayisi) + "\n");
            sb.Append("Alt dugumu olmayan (yaprak) dugum sayisi: " + ucDugumSayisi + "\n");
            sb.Append("AltDugumu olanların dökümü: \n");
            int araToplam = 0;
            for(int i=1; i<30; i++){
                araToplam += dugumSayilari[i];
                sb.Append(i + " alt dugumu olanlar: " + dugumSayilari[i]
                            + " Ara Toplam: " + araToplam + " Yuzdesi: %"
                            + IstatistikAraclari.yuzdeHesaplaStr(dugumSayilari[i], (dugumSayisi - ucDugumSayisi))
                            + " - Kaplam: "
                            + IstatistikAraclari.yuzdeHesaplaStr(araToplam, (dugumSayisi - ucDugumSayisi))
                            + "\n");
            }
            sb.Append("\n");
            return sb.ToString();
        }
    
        public static void main(String[] args) 
        {
            Type c = Type.GetType("net.zemberek.tr.yapi.TurkiyeTurkcesi");
            
            DilBilgisi dilBilgisi = new TurkceDilBilgisi((DilAyarlari)Assembly.GetAssembly(Type.GetType("net.zemberek.tr.yapi")).CreateInstance("net.zemberek.tr.yapi.TurkiyeTurkcesi"));
            Alfabe alfabe = dilBilgisi.alfabe();
            KokOkuyucu okuyucu = new IkiliKokOkuyucu("kaynaklar/tr/bilgi/binary-sozluk.bin", dilBilgisi.kokOzelDurumlari());
            AgacSozluk sozluk = new AgacSozluk(okuyucu, alfabe, dilBilgisi.kokOzelDurumlari());
    	    KokAgaciYuruyucu yuruyucu = new KokAgaciYuruyucu(sozluk, new HashedSet<Kok>());
    	    yuruyucu.agaciTara();
            logger.Info(yuruyucu);
	    }
    }
}
