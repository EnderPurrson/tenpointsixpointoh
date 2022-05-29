using System;
using UnityEngine;

public class BackButtonFromEnterPasswordInFriensJoin : MonoBehaviour
{
	public BackButtonFromEnterPasswordInFriensJoin()
	{
	}

	private void OnClick()
	{
		if (JoinRoomFromFrends.sharedJoinRoomFromFrends != null)
		{
			JoinRoomFromFrends.sharedJoinRoomFromFrends.BackFromPasswordButton();
		}
	}

	private void Start()
	{
	}
}