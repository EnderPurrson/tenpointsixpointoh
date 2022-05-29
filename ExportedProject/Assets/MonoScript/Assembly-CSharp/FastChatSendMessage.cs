using System;
using UnityEngine;

public class FastChatSendMessage : MonoBehaviour
{
	public string message = "-=GO!=-";

	public UISprite iconSprite;

	public FastChatSendMessage()
	{
	}

	private void Awake()
	{
	}

	private void OnClick()
	{
		if (InGameGUI.sharedInGameGUI.playerMoveC != null)
		{
			InGameGUI.sharedInGameGUI.playerMoveC.SendChat(this.message, false, string.Empty);
			if (ChatViewrController.sharedController)
			{
				ChatViewrController.sharedController.CloseChat(false);
			}
		}
	}
}