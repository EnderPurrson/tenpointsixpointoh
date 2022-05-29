using Rilisoft;
using System;
using UnityEngine;

internal sealed class FriendProfileExample : MonoBehaviour
{
	public FriendProfileView friendProfileView;

	public FriendProfileExample()
	{
	}

	private void Start()
	{
		if (this.friendProfileView != null)
		{
			this.friendProfileView.Reset();
			this.friendProfileView.IsCanConnectToFriend = true;
			this.friendProfileView.FriendLocation = "Deathmatch/Bridge";
			this.friendProfileView.FriendCount = 42;
			this.friendProfileView.FriendName = "Дуэйн «Rock» Джонсон";
			this.friendProfileView.Online = OnlineState.playing;
			this.friendProfileView.Rank = 4;
			this.friendProfileView.SurvivalScore = 4376;
			this.friendProfileView.Username = "John Doe";
			this.friendProfileView.WinCount = 13;
			this.friendProfileView.SetBoots("boots_blue");
			this.friendProfileView.SetHat("hat_KingsCrown");
			this.friendProfileView.SetStockCape("cape_BloodyDemon");
		}
	}
}