using System;
using UnityEngine;

public class SendInvitationsButton : MonoBehaviour
{
	public SendInvitationsButton()
	{
	}

	private void OnClick()
	{
		if (FacebookController.FacebookSupported)
		{
			FacebookController.sharedController.InvitePlayer(null);
		}
		ButtonClickSound.Instance.PlayClick();
	}
}