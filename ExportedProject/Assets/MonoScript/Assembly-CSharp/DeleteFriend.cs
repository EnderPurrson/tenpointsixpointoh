using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class DeleteFriend : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003COnClick_003Ec__AnonStorey1EE
	{
		internal FriendPreview fp;

		internal void _003C_003Em__3E(bool ok)
		{
			if (ok)
			{
				FriendsController.sharedController.friendsDeletedLocal.Add(fp.id);
			}
			else
			{
				FriendsController.sharedController.friendsDeletedLocal.Remove(fp.id);
			}
		}
	}

	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		if (!base.transform.parent.GetComponent<FriendPreview>().ClanMember)
		{
			_003COnClick_003Ec__AnonStorey1EE _003COnClick_003Ec__AnonStorey1EE = new _003COnClick_003Ec__AnonStorey1EE();
			string clanID = FriendsController.sharedController.ClanID;
			string clanLeaderID = FriendsController.sharedController.clanLeaderID;
			_003COnClick_003Ec__AnonStorey1EE.fp = base.transform.parent.GetComponent<FriendPreview>();
			if (!string.IsNullOrEmpty(clanID) && !string.IsNullOrEmpty(clanLeaderID) && _003COnClick_003Ec__AnonStorey1EE.fp.id != null && FriendsController.sharedController.clanLeaderID.Equals(_003COnClick_003Ec__AnonStorey1EE.fp.id))
			{
				FriendsController.sharedController.RejectInvite(_003COnClick_003Ec__AnonStorey1EE.fp.id);
				_003COnClick_003Ec__AnonStorey1EE.fp.DisableButtons();
				FriendsController.sharedController.ExitClan();
			}
			else if (IsDeletePlayerFromClan(clanID, clanLeaderID, _003COnClick_003Ec__AnonStorey1EE.fp.id))
			{
				FriendsController.sharedController.RejectInvite(_003COnClick_003Ec__AnonStorey1EE.fp.id, _003COnClick_003Ec__AnonStorey1EE._003C_003Em__3E);
				_003COnClick_003Ec__AnonStorey1EE.fp.DisableButtons();
				FriendsController.sharedController.ExitClan(_003COnClick_003Ec__AnonStorey1EE.fp.id);
			}
			else
			{
				FriendsController.sharedController.RejectInvite(_003COnClick_003Ec__AnonStorey1EE.fp.id);
				_003COnClick_003Ec__AnonStorey1EE.fp.DisableButtons();
			}
		}
		else
		{
			FriendsController.sharedController.clanDeletedLocal.Add(base.transform.parent.GetComponent<FriendPreview>().id);
			FriendsController.sharedController.DeleteClanMember(base.transform.parent.GetComponent<FriendPreview>().id);
		}
	}

	private bool IsDeletePlayerFromClan(string clanId, string leaderId, string friendId)
	{
		FriendsController sharedController = FriendsController.sharedController;
		if (string.IsNullOrEmpty(sharedController.id))
		{
			return false;
		}
		bool flag = !string.IsNullOrEmpty(clanId) && !string.IsNullOrEmpty(leaderId);
		bool flag2 = sharedController.id.Equals(sharedController.clanLeaderID);
		bool flag3 = friendId != null && sharedController.playersInfo.ContainsKey(friendId);
		if (!flag || !flag2 || !flag3)
		{
			return false;
		}
		Dictionary<string, object> dictionary = sharedController.playersInfo[friendId];
		if (dictionary == null)
		{
			return false;
		}
		Dictionary<string, object> dictionary2 = dictionary["player"] as Dictionary<string, object>;
		bool result = false;
		if (dictionary2 != null && dictionary2["clan_creator_id"] != null)
		{
			string text = Convert.ToString(dictionary2["clan_creator_id"]);
			if (text != null)
			{
				result = text.Equals(sharedController.clanLeaderID);
			}
		}
		return result;
	}
}
