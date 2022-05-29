using System;
using System.Collections.Generic;
using UnityEngine;

public class FriendPrevInChatItem : MonoBehaviour
{
	public UILabel nickLabel;

	public UITexture previewTexture;

	public UISprite rank;

	public string playerID;

	public GameObject newMessageObj;

	public UILabel countNewMessageLabel;

	private int contNewMessage;

	public int myWrapIndex;

	public FriendPrevInChatItem()
	{
	}

	public void SetActivePlayer()
	{
		if (PrivateChatController.sharedController.selectedPlayerID == this.playerID)
		{
			return;
		}
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		PrivateChatController.sharedController.SetSelectedPlayer(this.playerID, false);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void UpdateCountNewMessage()
	{
		int num = 0;
		if (ChatController.privateMessages.ContainsKey(this.playerID))
		{
			List<ChatController.PrivateMessage> item = ChatController.privateMessages[this.playerID];
			for (int i = 0; i < item.Count; i++)
			{
				if (!item[i].isRead)
				{
					num++;
				}
			}
		}
		this.contNewMessage = num;
		if (this.contNewMessage != 0)
		{
			this.newMessageObj.SetActive(true);
			this.countNewMessageLabel.text = this.contNewMessage.ToString();
		}
		else
		{
			this.newMessageObj.SetActive(false);
		}
	}
}