using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(UIPopupList))]
[AddComponentMenu("NGUI/Interaction/Language Selection")]
public class LanguageSelection : MonoBehaviour
{
	private UIPopupList mList;

	[CompilerGenerated]
	private static EventDelegate.Callback _003C_003Ef__am_0024cache1;

	private void Awake()
	{
		mList = GetComponent<UIPopupList>();
		Refresh();
	}

	private void Start()
	{
		List<EventDelegate> onChange = mList.onChange;
		if (_003C_003Ef__am_0024cache1 == null)
		{
			_003C_003Ef__am_0024cache1 = _003CStart_003Em__1CE;
		}
		EventDelegate.Add(onChange, _003C_003Ef__am_0024cache1);
	}

	public void Refresh()
	{
		if (mList != null && Localization.knownLanguages != null)
		{
			mList.Clear();
			int i = 0;
			for (int num = Localization.knownLanguages.Length; i < num; i++)
			{
				mList.items.Add(Localization.knownLanguages[i]);
			}
			mList.value = Localization.language;
		}
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__1CE()
	{
		Localization.language = UIPopupList.current.value;
	}
}
