using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using net.zemberek.yapi;
using net.zemberek.tr.yapi;

namespace net.zemberek.tests
{
    [TestFixture]
    public class TemelTest
    {
        internal DilBilgisi dilBilgisi;
        internal DilAyarlari dilAyarlari;
        internal Alfabe alfabe;

        [SetUp]
        public virtual void once() 
        {
            dilAyarlari = new TurkiyeTurkcesi();
            dilBilgisi = new TurkceDilBilgisi(dilAyarlari);
            alfabe = dilBilgisi.alfabe();
        }

        public HarfDizisi hd(String s) 
        {
            return new HarfDizisi(s, alfabe);
        }
    }
}
