using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChatTapReceiver : MonoBehaviour
{
	public ChatTapReceiver()
	{
	}

	private void HandleChatSettUpdated()
	{
		base.gameObject.SetActive((!Defs.isMulti ? false : Defs.IsChatOn));
	}

	private void OnClick()
	{
		ButtonClickSound.TryPlayClick();
		if (ChatTapReceiver.ChatClicked != null)
		{
			ChatTapReceiver.ChatClicked();
		}
	}

	private void OnDestroy()
	{
		PauseNGUIController.ChatSettUpdated -= new Action(this.HandleChatSettUpdated);
	}

	private void Start()
	{
		this.HandleChatSettUpdated();
		PauseNGUIController.ChatSettUpdated += new Action(this.HandleChatSettUpdated);
	}

	public static event Action ChatClicked;
}