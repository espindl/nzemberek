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
 * The Original Code is Zemberek Doğal Dil İşleme Kütüphanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Akın, Mehmet D. Akın.
 * Portions created by the Initial Developer are Copyright (C) 2006
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Mert Derman
 *   Tankut Tekeli
 * ***** END LICENSE BLOCK ***** */

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using net.zemberek.yapi;
using net.zemberek.tr.yapi;

namespace net.zemberek.tests
{
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
