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

// V 0.1
using System;
using System.Collections.Generic;
using System.Text;
using Iesi.Collections.Generic;
using net.zemberek.yapi;
using net.zemberek.yapi.ek;

namespace net.zemberek.javaporttemp
{
    //TODO Bu tamamen yokolmalı....
    public abstract class Collections
    {
        public static readonly List<object> EMPTY_LIST = new List<object>();
        public static readonly List<Kelime> EMPTY_LIST_KELIME = new List<Kelime>();
        public static readonly Set<Ek> EMPTY_SET = new HashedSet<Ek>();// (System.Collections.IList)System.Collections.ArrayList.ReadOnly(new System.Collections.ArrayList());
        public static readonly Set<String> EMPTY_SET_STRING = new HashedSet<String>();
        public static readonly Kelime[] BOS_KELIME_DIZISI = new Kelime[0];
    }
}
