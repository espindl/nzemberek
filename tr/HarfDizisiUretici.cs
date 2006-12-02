using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;

namespace net.zemberek.tr
{
public class HarfDizisiUretici {

    Alfabe alfabe;

    public HarfDizisiUretici(Alfabe alfabe) {
        this.alfabe = alfabe;
    }

    public  HarfDizisi uret(String str) {
        return new HarfDizisi(str, alfabe);
    }
}
}
