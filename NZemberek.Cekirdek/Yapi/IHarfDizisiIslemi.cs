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

using System;
using NZemberek.Cekirdek.Yapi;

namespace NZemberek.Cekirdek.Yapi
{
	/// <summary> Bir Harf dizisi uzerinde yapilabilecek islemi ifade eder. Bu arayuz genellikle
	/// kok yapisi uzerinde degisiklige nedenn olan ozel durumlarin tanimlanmasinda kullanilir.
	/// </summary>
	public interface IHarfDizisiIslemi
	{
		
		/// <summary> dizi uzerinde degisiklik yapacak metod.</summary>
		/// <param name="dizi">
		/// </param>
		void  Uygula(HarfDizisi dizi);
	}
}