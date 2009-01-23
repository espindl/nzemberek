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
 * The Original Code is Zemberek Doðal Dil Ýþleme Kütüphanesi.
 *
 * The Initial Developer of the Original Code is
 * Ahmet A. Akýn, Mehmet D. Akýn.
 * Portions created by the Initial Developer are Copyright (C) 2006
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Mert Derman
 *   Tankut Tekeli
 * ***** END LICENSE BLOCK ***** */

using System;
using NZemberek.Cekirdek.Yapi;

namespace NZemberek.Cekirdek.Yapi
{
	public class Kok
	{
        private static readonly KokOzelDurumu[] BOS_OZEL_DURUM_DIZISI = new KokOzelDurumu[0];

        private int indeks;
        /// <summary>
        /// Eger bir kok icinde Alfabe disi karakter barindiriyorsa (nokta, tire gibi) orjinal hali bu
        /// String icinde yer alir. Aksi halde null.
        /// </summary>
        private String asil_Renamed_Field;
        /// <summary>
        /// Bazi kisaltmalara ek eklenebilmesi icin kisaltmanin asil halinin son seslisine ihtiyac duyulur.
        /// </summary>
        private char kisaltmaSonSeslisi = char.MinValue;
        /// <summary>
        /// Kok'un ozel karakterlerden temizlenmis hali. Genelde kok icerigi olarak bu String kullanilir.
        /// </summary>
        private System.String _icerik;

        private bool yapiBozucuOzelDurumVar = false;
        private KelimeTipi _tip = KelimeTipi.YOK;
        private string[] kokOzelDurumlari = new string[] { };
        private int frekans;

        #region Properties
        public KelimeTipi Tip
		{
            get
            {
                return _tip;
            }
			set
			{
				this._tip = value;
			}
			
		}
		public String Icerik
		{
            get
            {
                return _icerik;
            }
			set
			{
				this._icerik = value;
			}
			
		}
		public int Indeks
		{
			get
			{
				return indeks;
			}
			
			set
			{
				this.indeks = value;
			}
			
		}
		public int Frekans
		{
			get
			{
				return frekans;
			}
			
			set
			{
				this.frekans = value;
			}
			
		}
		public String Asil
		{
            get
            {
                return asil_Renamed_Field;
            }
			set
			{
				this.asil_Renamed_Field = value;
			}
			
		}
		public char KisaltmaSonSeslisi
		{
			get
			{
				return kisaltmaSonSeslisi;
			}
			
			set
			{
				this.kisaltmaSonSeslisi = value;
			}

        }
        public virtual bool YapiBozucuOzelDurumVar
        {
            get { return yapiBozucuOzelDurumVar; }
            set { yapiBozucuOzelDurumVar = value; }
        }
        #endregion

        public Kok(String icerik)
        {
            this._icerik = icerik;
        }
        public Kok(String icerik, KelimeTipi tip)
        {
            this._icerik = icerik;
            this._tip = tip;
        }

        /// <summary>
        /// Kokun ozel durumu olup olmadigini doner
        /// </summary>
        /// <returns></returns>
        public bool OzelDurumVarmi()
		{
            return kokOzelDurumlari.Length > 0;
		}

        /// <summary>
        /// Tum ozel durumlari doner.
        /// </summary>
        /// <returns></returns>
        public string[] KokOzelDurumlariGetir()
        {
            return kokOzelDurumlari;
        }
		
        /// <summary>
        /// Kokun ozel durumu icerip icermedigini doner
        /// </summary>
        /// <param name="ad">sorulan ozel durum</param>
        /// <returns></returns>
		public bool OzelDurumIceriyormu(string ad)
		{            
            foreach (string durum in kokOzelDurumlari)
            {
                if (durum == ad)
                    return true;
            }
			return false;
		}

        /// <summary> 
        /// Koke ozel durum ekler. burada dizi kullaniminda kaynak konusunda cimrilik ettigimizden
		/// her yeni ozel durum icin dizi boyutunu bir buyuttuk. ayrica tekrar olmamasini da sagliyoruz.
		/// Normalde bu islem Set icin cok daha kolay bir yapida olabilirdi set.add() ancak Set'in kaynak tuketimi
		/// diziden daha fazla.
		/// </summary>
		/// <param name="OzelDurum">
		/// </param>
		public virtual void  OzelDurumEkle(string ozelDurum)
		{
			if (kokOzelDurumlari.Length == 0)
			{
                kokOzelDurumlari = new string[1];
                kokOzelDurumlari[0] = ozelDurum;
			}
			else
			{
				if (OzelDurumIceriyormu(ozelDurum))
					return ;
                string[] yeni = new string[kokOzelDurumlari.Length + 1];
                for (int i = 0; i < kokOzelDurumlari.Length; i++)
				{
                    yeni[i] = kokOzelDurumlari[i];
				}
                yeni[kokOzelDurumlari.Length] = ozelDurum;
                this.kokOzelDurumlari = yeni;
			}
		}
		
		/// <summary> 
        /// Sadece ilk acilista kullanilan bir metod
		/// </summary>
		/// <param name="Tip"></param>
		public virtual void  OzelDurumCikar(string ozelDurum)
		{
			if (!OzelDurumIceriyormu(ozelDurum))
				return ;
            string[] yeni = new string[kokOzelDurumlari.Length - 1];
			int j = 0;
			foreach(string durum in kokOzelDurumlari)
			{
				if (durum != ozelDurum)
					yeni[j++] = durum;
			}
			this.kokOzelDurumlari = yeni;
		}

        protected bool TipVarmi()
        {
            return (_tip != KelimeTipi.YOK);
        }

		public override String ToString()
		{
			System.String strOzel = "";
            foreach (string ozelDurum in kokOzelDurumlari)
			{
				if (ozelDurum != null)
					strOzel += (ozelDurum + " ");
			}
			return _icerik + " " + _tip + " " + strOzel;
		}

		public override bool Equals(System.Object o)
		{
			if (this == o)
				return true;
			if (o == null || GetType() != o.GetType())
				return false;
			
			Kok kok = (Kok) o;
			
			if (_icerik != null?!_icerik.Equals(kok._icerik):kok._icerik != null)
				return false;
            if (kokOzelDurumlari != null ? !kokOzelDurumlari.Equals(kok.kokOzelDurumlari) : kok.kokOzelDurumlari != null)
				return false;
            if (TipVarmi() ? !_tip.Equals(kok._tip) : kok.TipVarmi())
				return false;
			
			return true;
		}
		
		public override int GetHashCode()
		{
			int result;
			result = (_icerik != null?_icerik.GetHashCode():0);
            result = 29 * result + (TipVarmi() ? _tip.GetHashCode() : 0);
            result = 29 * result + (kokOzelDurumlari != null ? kokOzelDurumlari.GetHashCode() : 0);
			return result;
		}
	}
}