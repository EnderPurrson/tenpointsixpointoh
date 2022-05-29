using System;
using UnityEngine;

public class EnterPasswordInFriensJoinButton : MonoBehaviour
{
	public UILabel passwordLabel;

	public JoinRoomFromFrends joinRoomFromFrends;

	public EnterPasswordInFriensJoinButton()
	{
	}

	private void OnClick()
	{
		if (this.joinRoomFromFrends == null)
		{
			this.joinRoomFromFrends = JoinRoomFromFrends.sharedJoinRoomFromFrends;
		}
		if (this.joinRoomFromFrends != null)
		{
			this.joinRoomFromFrends.EnterPassword(this.passwordLabel.text);
		}
	}

	private void Start()
	{
	}
}