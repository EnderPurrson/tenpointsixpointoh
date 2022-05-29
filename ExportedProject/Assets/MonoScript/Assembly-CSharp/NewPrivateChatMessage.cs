using System;
using UnityEngine;

public class NewPrivateChatMessage : MonoBehaviour
{
	private GameObject newMessageSprite;

	public NewPrivateChatMessage()
	{
	}

	private void Start()
	{
		this.newMessageSprite = base.gameObject.transform.GetChild(0).gameObject;
		this.UpdateStateNewMessage();
	}

	private void Update()
	{
		this.UpdateStateNewMessage();
	}

	private void UpdateStateNewMessage()
	{
		if (this.newMessageSprite.activeSelf != ChatController.countNewPrivateMessage > 0)
		{
			this.newMessageSprite.SetActive(ChatController.countNewPrivateMessage > 0);
		}
	}
}