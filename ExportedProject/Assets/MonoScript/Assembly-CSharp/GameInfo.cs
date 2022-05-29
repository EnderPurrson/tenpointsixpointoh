using System;
using UnityEngine;

public class GameInfo : MonoBehaviour
{
	public GameObject openSprite;

	public GameObject closeSprite;

	public UILabel countPlayersLabel;

	public UILabel serverNameLabel;

	public UILabel mapNameLabel;

	public RoomInfo roomInfo;

	public LANBroadcastService.ReceivedMessage roomInfoLocal;

	public int index;

	public GameInfo()
	{
	}

	private void OnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		if (ConnectSceneNGUIController.sharedController != null)
		{
			if (!Defs.isInet)
			{
				ConnectSceneNGUIController.sharedController.JoinToLocalRoom(this.roomInfoLocal);
			}
			else
			{
				ConnectSceneNGUIController.sharedController.JoinToRoomPhoton(this.roomInfo);
			}
		}
	}

	private void Start()
	{
	}
}