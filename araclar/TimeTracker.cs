using System;
using System.Collections.Generic;
using System.Text;
using Iesi.Collections.Generic;
using log4net;


namespace net.zemberek.araclar
{
    /**
     * Hassas kronometre ihtiyaçlarý için tasarlanmýþtýr.
     * <p/>
     * Kullanmak için timeTracker.startClock(isim) dedikten sonra
     * TimeTracker.stopClock(isim)'un döndürdüðü String'i geçen süreyi göstermek 
     * için kullanabilirsiniz. Stop'tan önce ara adýmlarý izlemek istiyorsanýz 
     * TimeTracker.getElapsedTimeString(isim) veya getElapsedTimeStringAsMillis
     * metodlarini kullanabilirsiniz. Start ile baþlattýðýnýz saatleri isiniz 
     * bittigindemutlaka stop ile durdurmanýz gerekiyor, Çünkü ancak stop ile register
     * olmuþ bir saat nesnesini unregister edebilirsiniz.
     * <p/>
     * Olusan saatler globaldir, yani programin icinde istediginiz her yerde
     * kullanabilirsiniz.
     *
     * @author M.D.A
     */
    public sealed class TimeTracker
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static int MAX_TIMETRACKER_USERS = 500;
        public static readonly long BOLUCU = 1000000000L;
        private static IDictionary<String, TimerElement> users = new Dictionary<String, TimerElement>();

        /**
         * Yeni bir saat oluþturur ve listeye register eder.
         * @param name : saat adý
         */
        public static void startClock(String name)
        {
            if (users.Count > MAX_TIMETRACKER_USERS)
            {
                logger.Error("Max Saat izleyici sayýsý aþýldý. (" + MAX_TIMETRACKER_USERS + ")");
                return;
            }
            if (users[name] != null)
            {
                logger.Error(name + " isminde bir zaman izleyici zaten var.");
                return;
            }
            TimerElement timer = new TimerElement(name);
            users.Add(name, timer);
        }

        /**
         * ismi verilen saat için baþlangýçtan bu yana bu yana ne kadar zaman 
         * geçtiðini milisaniye cinsinden döndürür.
         *
         * @param name : saatin adý
         * @return :Bir önceki tick'ten bu yana geçen süre (milisaniye cinsinden)
         */
        public static long getElapsedTime(String name)
        {
            TimerElement timer = users[name];
            if (timer == null)
                return -1;
            timer.refresh();
            return timer.getElapsedTime();
        }

        /**
         * ismi verilen saatin en son kontrolünden bu yana ne kadar zaman geçtiðini
         * milisaniye cinsinden döndürür.
         *
         * @param name :  saatin adý
         * @return :Bir önceki tick'ten bu yana geçen süre (milisaniye cinsinden)
         */
        public static long getTimeDelta(String name)
        {
            TimerElement timer = users[name];
            if (timer == null)
                return -1;
            timer.refresh();
            return timer.getDiff();
        }

        /**
         * ismi verilen saatin en son kontrolunden (baslangic veya bir onceki tick) 
         * bu yana ne kadar zaman gectiðini ve baþlangýçtan bu yana geçen süreyi 
         * virgülden sonra 3 basamaklý saniyeyi ifade eden String cinsinden döndürür.
         *
         * @param name : saatin adý
         * @return : Bir önceki tick'ten bu yana geçen süre (Binde bir hassasiyetli saniye cinsinden cinsinden)
         */
        public static String getElapsedTimeString(String name)
        {
            TimerElement timer = users[name];
            if (timer == null)
                return "Geçersiz Kronometre: " + name;
            timer.refresh();
            return "Delta: " + (double)timer.getDiff() / BOLUCU + " s. Elapsed: " + (double)timer.getElapsedTime() / BOLUCU + " s.";
        }

        /**
         * @param name : saatin adý
         * @return : Bir önceki tick'ten bu yana geçen süre (milisaniye cinsinden)
         */
        public static String getElapsedTimeStringAsMillis(String name)
        {
            TimerElement timer = users[name];
            if (timer == null)
                return "Geçersiz Kronometre: " + name;
            timer.refresh();
            return "Delta: " + timer.getDiff() + "ms. Elapsed: " + timer.getElapsedTime() + "ms.";
        }

        /**
         * @param name      : saatin adý
         * @param itemCount : sure zarfýnda islenen nesne sayisi
         * @return : baslangictan bu yana islenen saniyedeki eleman sayisi
         */
        public static long getItemsPerSecond(String name, long itemCount)
        {
            TimerElement timer = users[name];
            if (timer == null)
                return -1;
            timer.refresh();
            long items = 0;
            if (timer.getElapsedTime() > 0)
                items = (BOLUCU * itemCount) / timer.getElapsedTime();
            return items;
        }

        /**
         * Saati durdurur ve baþlangýçtan bu yana geçen süreyi saniye ve ms 
         * cinsinden döndürür. Ayrýca saati listeden siler. 
         *
         * @param name Saat ismi
         * @return baþlangýçtan bu yana geçen süre
         */
        public static String stopClock(String name)
        {
            TimerElement timer = users[name];
            if (timer == null)
                return name + " : Geçersiz Kronometre";
            timer.refresh();
            users.Remove(name);
            return "" + (float)timer.getElapsedTime() / BOLUCU + "sn."
                   + "(" + timer.getElapsedTime() + " ms.)";
        }

        /**
        * isimlendirilmiþ Zaman bilgisi taþýyýcý.
        *
        * @author MDA
        */
        class TimerElement
        {
            String name;
            long startTime = 0;
            long stopTime = 0;
            long lastTime = 0;
            long creationTime = 0;
            long elapsedTime = 0;
            long diff = 0;

            public TimerElement(String name)
            {
                //TODO : Eger DateTime.Now.Ticks yeterince hassas deðilse 
                //bunu implement edebiliriz : http://www.codeproject.com/csharp/highperformancetimercshar.asp
                creationTime = DateTime.Now.Ticks;
                startTime = creationTime;
                lastTime = creationTime;
                this.name = name;
            }

            public void refresh()
            {
                diff = DateTime.Now.Ticks - lastTime;
                lastTime = DateTime.Now.Ticks;
                elapsedTime = lastTime - startTime;
            }

            public long getDiff()
            {
                return diff;
            }

            public long getElapsedTime()
            {
                return elapsedTime;
            }

            public long getLastTime()
            {
                return lastTime;
            }

            public String getName()
            {
                return name;
            }

            public long getStartTime()
            {
                return startTime;
            }

            public long getStopTime()
            {
                return stopTime;
            }
        }
    }
}
    




