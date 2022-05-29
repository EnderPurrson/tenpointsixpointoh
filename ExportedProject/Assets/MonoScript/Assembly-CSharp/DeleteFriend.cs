using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class DeleteFriend : MonoBehaviour
{
	public DeleteFriend()
	{
	}

	private bool IsDeletePlayerFromClan(string clanId, string leaderId, string friendId)
	{
		FriendsController friendsController = FriendsController.sharedController;
		if (string.IsNullOrEmpty(friendsController.id))
		{
			return false;
		}
		bool flag = (string.IsNullOrEmpty(clanId) ? false : !string.IsNullOrEmpty(leaderId));
		bool flag1 = friendsController.id.Equals(friendsController.clanLeaderID);
		if (!flag || !flag1 || (friendId == null ? true : !friendsController.playersInfo.ContainsKey(friendId)))
		{
			return false;
		}
		Dictionary<string, object> item = friendsController.playersInfo[friendId];
		if (item == null)
		{
			return false;
		}
		Dictionary<string, object> strs = item["player"] as Dictionary<string, object>;
		bool flag2 = false;
		if (strs != null && strs["clan_creator_id"] != null)
		{
			string str = Convert.ToString(strs["clan_creator_id"]);
			if (str != null)
			{
				flag2 = str.Equals(friendsController.clanLeaderID);
			}
		}
		return flag2;
	}

	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		if (base.transform.parent.GetComponent<FriendPreview>().ClanMember)
		{
			FriendsController.sharedController.clanDeletedLocal.Add(base.transform.parent.GetComponent<FriendPreview>().id);
			FriendsController.sharedController.DeleteClanMember(base.transform.parent.GetComponent<FriendPreview>().id);
		}
		else
		{
			string clanID = FriendsController.sharedController.ClanID;
			string str = FriendsController.sharedController.clanLeaderID;
			FriendPreview component = base.transform.parent.GetComponent<FriendPreview>();
			if (!string.IsNullOrEmpty(clanID) && !string.IsNullOrEmpty(str) && component.id != null && FriendsController.sharedController.clanLeaderID.Equals(component.id))
			{
				FriendsController.sharedController.RejectInvite(component.id, null);
				component.DisableButtons();
				FriendsController.sharedController.ExitClan(null);
			}
			else if (!this.IsDeletePlayerFromClan(clanID, str, component.id))
			{
				FriendsController.sharedController.RejectInvite(component.id, null);
				component.DisableButtons();
			}
			else
			{
				FriendsController.sharedController.RejectInvite(component.id, (bool ok) => {
					if (!ok)
					{
						FriendsController.sharedController.friendsDeletedLocal.Remove(component.id);
					}
					else
					{
						FriendsController.sharedController.friendsDeletedLocal.Add(component.id);
					}
				});
				component.DisableButtons();
				FriendsController.sharedController.ExitClan(component.id);
			}
		}
	}
}