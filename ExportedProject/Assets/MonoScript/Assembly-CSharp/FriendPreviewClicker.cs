using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FriendPreviewClicker : MonoBehaviour
{
	public FriendPreviewClicker()
	{
	}

	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		if (FriendPreviewClicker.FriendPreviewClicked != null)
		{
			string component = base.transform.parent.GetComponent<FriendPreview>().id;
			FriendPreviewClicker.FriendPreviewClicked(component);
		}
	}

	public static event Action<string> FriendPreviewClicked;
}