using System;
using UnityEngine;

namespace I2.Loc
{
	[AddComponentMenu("I2/Localization/SetLanguage")]
	public class SetLanguage : MonoBehaviour
	{
		public string _Language;

		public SetLanguage()
		{
		}

		public void ApplyLanguage()
		{
			if (LocalizationManager.HasLanguage(this._Language, true))
			{
				LocalizationManager.CurrentLanguage = this._Language;
			}
		}

		private void OnClick()
		{
			this.ApplyLanguage();
		}
	}
}