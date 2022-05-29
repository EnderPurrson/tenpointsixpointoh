using System;
using System.Collections.Generic;
using UnityEngine;

public class CancelClanInvitaionPressed : MonoBehaviour
{
	public CancelClanInvitaionPressed()
	{
	}

	private void OnClick()
	{
		FriendsController.sharedController.clanCancelledInvitesLocal.Add(base.transform.parent.GetComponent<FriendPreview>().id);
		FriendsController.sharedController.RejectClanInvite(FriendsController.sharedController.ClanID, base.transform.parent.GetComponent<FriendPreview>().id);
	}
}