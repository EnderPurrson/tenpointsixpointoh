using System;
using UnityEngine;

public class SendSmileButtonHundler : MonoBehaviour
{
	private string smileName = string.Empty;

	public SendSmileButtonHundler()
	{
	}

	private void Awake()
	{
		this.smileName = base.GetComponent<UISprite>().spriteName;
	}

	private void OnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		if (PrivateChatController.sharedController != null)
		{
			PrivateChatController.sharedController.SendSmile(this.smileName);
		}
		if (ChatViewrController.sharedController != null && InGameGUI.sharedInGameGUI.playerMoveC != null)
		{
			InGameGUI.sharedInGameGUI.playerMoveC.SendChat(string.Empty, false, this.smileName);
			ChatViewrController.sharedController.HideSmilePannelOnClick();
		}
	}
}