using System;
using System.Collections.Generic;
using System.Text;
using net.zemberek.yapi;

namespace net.zemberek.bilgi.araclar
{
    /// <summary>
    /// Kök ağacını oluşturan kök nesnelerini bir kaynaktan sırayla veya topluca okur.
    /// Once 'Ac', sonra tek tek 'Oku' veya 'hepsiniOku'.
    /// Eğer sonuna kadar okunmuşsa okuyucu kendiliğinden kapanır, yoksa 'Kapat'mak gerekir.
    /// </summary>
    public interface KokOkuyucu
    {
        /// <summary>
        /// Sözlükteki Tüm kökleri okur ve bir ArrayList olarak döndürür.
        /// </summary>
        List<Kok> hepsiniOku();

        /// <summary>
        /// Kaynaktan bir kök okur, çağrıldıkça bir sonraki kökü alır.
        /// Dosyanın sonuna gelirse dosyayı kapatır.
        /// </summary>
        /// <returns>bir sonraki kök. Eğer okunacak kök kalmamışsa null</returns>
        Kok oku();

        /// <summary>
        /// Kaynağı okumak için açar.
        /// </summary>
        void Ac();

        /// <summary>
        /// Okunan kaynağı kapatır.
        /// </summary>
        void Kapat();
    }
}
