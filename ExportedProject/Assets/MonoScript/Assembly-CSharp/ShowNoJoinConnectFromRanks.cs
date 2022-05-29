using System;
using UnityEngine;

public class ShowNoJoinConnectFromRanks : MonoBehaviour
{
	public float showTimer;

	public UILabel label;

	public GameObject panelMessage;

	public static ShowNoJoinConnectFromRanks sharedController;

	public ShowNoJoinConnectFromRanks()
	{
	}

	private void OnDestroy()
	{
		ShowNoJoinConnectFromRanks.sharedController = null;
	}

	public void resetShow(int rank)
	{
		this.label.text = string.Concat("Reach rank ", rank, "  to play this mode!");
		this.panelMessage.SetActive(true);
		this.showTimer = 3f;
	}

	private void Start()
	{
		ShowNoJoinConnectFromRanks.sharedController = this;
	}

	private void Update()
	{
		if (this.showTimer > 0f)
		{
			this.showTimer -= Time.deltaTime;
			if (this.showTimer <= 0f)
			{
				this.panelMessage.SetActive(false);
			}
		}
	}
}