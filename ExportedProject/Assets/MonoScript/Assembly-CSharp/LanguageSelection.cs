using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Language Selection")]
[RequireComponent(typeof(UIPopupList))]
public class LanguageSelection : MonoBehaviour
{
	private UIPopupList mList;

	public LanguageSelection()
	{
	}

	private void Awake()
	{
		this.mList = base.GetComponent<UIPopupList>();
		this.Refresh();
	}

	public void Refresh()
	{
		if (this.mList != null && Localization.knownLanguages != null)
		{
			this.mList.Clear();
			int num = 0;
			int length = (int)Localization.knownLanguages.Length;
			while (num < length)
			{
				this.mList.items.Add(Localization.knownLanguages[num]);
				num++;
			}
			this.mList.@value = Localization.language;
		}
	}

	private void Start()
	{
		EventDelegate.Add(this.mList.onChange, () => Localization.language = UIPopupList.current.@value);
	}
}