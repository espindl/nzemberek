/* ***** BEGIN LICENSE BLOCK *****
 * Version: MPL 1.1
 *
 * The contents of this file are subject to the Mozilla Public License Version
 * 1.1 (the "License"); you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * The Original Code is Zemberek Do�al Dil ��leme K�t�phanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Ak�n, Mehmet D. Ak�n.
 * Portions created by the Initial Developer are Copyright (C) 2006
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Mert Derman
 *   Tankut Tekeli
 * ***** END LICENSE BLOCK ***** */

// V 0.1
using System;
using System.Collections.Generic;
using System.Text;

namespace NZemberek.DilAraclari.Araclar
{
    /**
     * Hassas kronometre ihtiya�lar� i�in tasarlanm��t�r.
     * <p/>
     * Kullanmak i�in timeTracker.startClock(isim) dedikten sonra
     * TimeTracker.stopClock(isim)'un d�nd�rd��� String'i ge�en s�reyi g�stermek 
     * i�in kullanabilirsiniz. Stop'tan �nce ara ad�mlar� izlemek istiyorsan�z 
     * TimeTracker.getElapsedTimeString(isim) veya getElapsedTimeStringAsMillis
     * metodlarini kullanabilirsiniz. Start ile ba�latt���n�z saatleri isiniz 
     * bittigindemutlaka stop ile durdurman�z gerekiyor, ��nk� ancak stop ile register
     * olmu� bir saat nesnesini unregister edebilirsiniz.
     * <p/>
     * Olusan saatler globaldir, yani programin icinde istediginiz her yerde
     * kullanabilirsiniz.
     *
     * @author M.D.A
     */
    public sealed class TimeTracker
    {
        public static int MAX_TIMETRACKER_USERS = 500;
        //public static readonly long BOLUCU = 1000000000L;
        private static IDictionary<String, TimerElement> users = new Dictionary<String, TimerElement>();

        /**
         * Yeni bir saat olu�turur ve listeye register eder.
         * @param name : saat ad�
         */
        public static void startClock(String name)
        {
            if (users.Count > MAX_TIMETRACKER_USERS)
            {
                return;
            }
            if (users[name] != null)
            {
                return;
            }
            TimerElement timer = new TimerElement(name);
            users.Add(name, timer);
        }

        /**
         * ismi verilen saat i�in ba�lang��tan bu yana bu yana ne kadar zaman 
         * ge�ti�ini milisaniye cinsinden d�nd�r�r.
         *
         * @param name : saatin ad�
         * @return :Bir �nceki tick'ten bu yana ge�en s�re (milisaniye cinsinden)
         */
        public static long getElapsedTime(String name)
        {
            TimerElement timer = (TimerElement)users[name];
            if (timer == null)
                return -1;
            timer.refresh();
            return timer.getElapsedTime();
        }

        /**
         * ismi verilen saatin en son kontrol�nden bu yana ne kadar zaman ge�ti�ini
         * milisaniye cinsinden d�nd�r�r.
         *
         * @param name :  saatin ad�
         * @return :Bir �nceki tick'ten bu yana ge�en s�re (milisaniye cinsinden)
         */
        public static long getTimeDelta(String name)
        {
            TimerElement timer = (TimerElement)users[name];
            if (timer == null)
                return -1;
            timer.refresh();
            return timer.getDiff();
        }

        /**
         * ismi verilen saatin en son kontrolunden (baslangic veya bir onceki tick) 
         * bu yana ne kadar zaman gecti�ini ve ba�lang��tan bu yana ge�en s�reyi 
         * virg�lden sonra 3 basamakl� saniyeyi ifade eden String cinsinden d�nd�r�r.
         *
         * @param name : saatin ad�
         * @return : Bir �nceki tick'ten bu yana ge�en s�re (Binde bir hassasiyetli saniye cinsinden cinsinden)
         */
        public static String getElapsedTimeString(String name)
        {
            TimerElement timer = (TimerElement)users[name];
            if (timer == null)
                return "Ge�ersiz Kronometre: " + name;
            timer.refresh();
            return "Delta: " + (float)timer.getDiff() / 1000 + " s. Elapsed: " + (float)timer.getElapsedTime() / 1000 + " s.";
        }

        /**
         * @param name : saatin ad�
         * @return : Bir �nceki tick'ten bu yana ge�en s�re (milisaniye cinsinden)
         */
        public static String getElapsedTimeStringAsMillis(String name)
        {
            TimerElement timer = (TimerElement)users[name];
            if (timer == null)
                return "Ge�ersiz Kronometre: " + name;
            timer.refresh();
            return "Delta: " + timer.getDiff() + "ms. Elapsed: " + timer.getElapsedTime() + "ms.";
        }

        /**
         * @param name      : saatin ad�
         * @param itemCount : sure zarf�nda islenen nesne sayisi
         * @return : baslangictan bu yana islenen saniyedeki eleman sayisi
         */
        public static long getItemsPerSecond(String name, long itemCount)
        {
            TimerElement timer = (TimerElement)users[name];
            if (timer == null)
                return -1;
            timer.refresh();
            long items = 0;
            if (timer.getElapsedTime() > 0)
                items = (1000 * itemCount) / timer.getElapsedTime();
            return items;
        }

        /**
         * Saati durdurur ve ba�lang��tan bu yana ge�en s�reyi saniye ve ms 
         * cinsinden d�nd�r�r. Ayr�ca saati listeden siler. 
         *
         * @param name Saat ismi
         * @return ba�lang��tan bu yana ge�en s�re
         */
        public static String stopClock(String name)
        {
            TimerElement timer = (TimerElement)users[name];
            if (timer == null)
                return name + " : Ge�ersiz Kronometre";
            timer.refresh();
            users.Remove(name);
            return "" + (float)timer.getElapsedTime() / 1000 + "sn."
                   + "(" + timer.getElapsedTime() + " ms.)";
        }

        /**
        * isimlendirilmi� Zaman bilgisi ta��y�c�.
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
                //TODO : Eger DateTime.Now.Ticks yeterince hassas de�ilse 
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
    




