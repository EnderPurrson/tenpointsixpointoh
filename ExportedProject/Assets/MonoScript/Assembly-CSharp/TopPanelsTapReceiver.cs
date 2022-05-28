using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TopPanelsTapReceiver : MonoBehaviour
{
	[CompilerGenerated]
	private static Action _003C_003Ef__am_0024cache1;

	public static event Action OnClicked;

	static TopPanelsTapReceiver()
	{
		if (_003C_003Ef__am_0024cache1 == null)
		{
			_003C_003Ef__am_0024cache1 = _003COnClicked_003Em__58D;
		}
		TopPanelsTapReceiver.OnClicked = _003C_003Ef__am_0024cache1;
	}

	private void Start()
	{
		base.gameObject.SetActive(Defs.isMulti);
	}

	private void OnClick()
	{
		ButtonClickSound.TryPlayClick();
		TopPanelsTapReceiver.OnClicked();
	}

	[CompilerGenerated]
	private static void _003COnClicked_003Em__58D()
	{
	}
}
