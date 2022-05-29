using System;
using UnityEngine;

namespace I2.Loc
{
	public class LanguagePopup : MonoBehaviour
	{
		public LanguageSource Source;

		public LanguagePopup()
		{
		}

		public void OnValueChange()
		{
			LocalizationStore.CurrentLanguage = UIPopupList.current.@value;
		}

		private void Start()
		{
			UIPopupList component = base.GetComponent<UIPopupList>();
			component.items = this.Source.GetLanguages();
			EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.OnValueChange));
			component.@value = LocalizationManager.CurrentLanguage;
		}
	}
}