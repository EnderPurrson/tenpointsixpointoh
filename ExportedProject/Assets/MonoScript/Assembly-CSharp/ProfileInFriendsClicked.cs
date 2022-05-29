using System;
using UnityEngine;

public class ProfileInFriendsClicked : MonoBehaviour
{
	public ProfileInFriendsClicked()
	{
	}

	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
	}
}