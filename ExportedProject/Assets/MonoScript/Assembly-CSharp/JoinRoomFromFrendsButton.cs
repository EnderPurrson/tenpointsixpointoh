using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class JoinRoomFromFrendsButton : MonoBehaviour
{
	public JoinRoomFromFrends joinRoomFromFrends;

	public JoinRoomFromFrendsButton()
	{
	}

	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		string component = base.transform.parent.GetComponent<FriendPreview>().id;
		if (FriendsController.sharedController.onlineInfo.ContainsKey(component))
		{
			int num = int.Parse(FriendsController.sharedController.onlineInfo[component]["game_mode"]);
			string item = FriendsController.sharedController.onlineInfo[component]["room_name"];
			string str = FriendsController.sharedController.onlineInfo[component]["map"];
			if (this.joinRoomFromFrends == null)
			{
				this.joinRoomFromFrends = JoinRoomFromFrends.sharedJoinRoomFromFrends;
			}
			if (SceneInfoController.instance.GetInfoScene(int.Parse(str)) != null)
			{
				this.joinRoomFromFrends.ConnectToRoom(num, item, str);
			}
		}
	}
}