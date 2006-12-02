using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using net.zemberek.yapi;

namespace net.zemberek.tests
{
    [TestFixture]
    public class TemelTest
    {
        internal DilBilgisi dilBilgisi;
        internal DilAyarlari dilAyarlari;
        internal Alfabe alfabe;

        public void setUp() 
        {
            //TODO Bu satır açılacak ama belki de mock ile
            //dilAyarlari = new TurkiyeTurkcesi();
            dilBilgisi = new TurkceDilBilgisi(dilAyarlari);
            alfabe = dilBilgisi.alfabe();
        }

        public HarfDizisi hd(String s) 
        {
            return new HarfDizisi(s, alfabe);
        }
    }
}
