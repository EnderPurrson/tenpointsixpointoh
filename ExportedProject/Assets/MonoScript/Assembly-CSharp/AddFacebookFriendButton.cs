using System;
using System.Collections.Generic;
using UnityEngine;

internal sealed class AddFacebookFriendButton : MonoBehaviour
{
	public AddFacebookFriendButton()
	{
	}

	private void OnClick()
	{
		FriendPreview component = base.transform.parent.GetComponent<FriendPreview>();
		ButtonClickSound.Instance.PlayClick();
		string str = component.id;
		if (str != null)
		{
			if (!component.ClanInvite)
			{
				Dictionary<string, object> strs = new Dictionary<string, object>()
				{
					{ "Added Friends", "Find Friends: Facebook" },
					{ "Deleted Friends", "Add" },
					{ "Search Friends", "Add" }
				};
				FriendsController.sharedController.SendInvitation(str, strs);
			}
			else
			{
				FriendsController.SendPlayerInviteToClan(str, null);
				FriendsController.sharedController.clanSentInvitesLocal.Add(str);
			}
		}
		if (!component.ClanInvite)
		{
			component.DisableButtons();
		}
	}
}