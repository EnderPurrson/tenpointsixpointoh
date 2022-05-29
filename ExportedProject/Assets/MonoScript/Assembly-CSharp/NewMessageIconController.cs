using System;
using System.Collections.Generic;
using UnityEngine;

public class NewMessageIconController : MonoBehaviour
{
	public bool privateMessageFriends;

	public bool inviteToFriends;

	public bool inviteToClan;

	public bool clanMessages;

	public bool privateMessageClan;

	public GameObject newMessageSprite;

	public UILabel countLabel;

	public NewMessageIconController()
	{
	}

	private void Start()
	{
		this.UpdateStateNewMessage();
	}

	private void Update()
	{
		this.UpdateStateNewMessage();
	}

	private void UpdateStateNewMessage()
	{
		bool flag = false;
		int num = 0;
		if (this.privateMessageFriends && ChatController.countNewPrivateMessage > 0)
		{
			flag = true;
			num += ChatController.countNewPrivateMessage;
		}
		if (this.inviteToFriends && FriendsController.sharedController.invitesToUs.Count > 0)
		{
			for (int i = 0; i < FriendsController.sharedController.invitesToUs.Count; i++)
			{
				string item = FriendsController.sharedController.invitesToUs[i];
				if (FriendsController.sharedController.friendsInfo.ContainsKey(item))
				{
					flag = true;
					num++;
				}
				else if (FriendsController.sharedController.clanFriendsInfo.ContainsKey(item))
				{
					flag = true;
					num++;
				}
				else if (FriendsController.sharedController.profileInfo.ContainsKey(item))
				{
					flag = true;
					num++;
				}
			}
		}
		if (this.newMessageSprite != null && flag != this.newMessageSprite.activeSelf)
		{
			this.newMessageSprite.SetActive(flag);
		}
		if (this.countLabel != null)
		{
			this.countLabel.text = num.ToString();
		}
	}
}