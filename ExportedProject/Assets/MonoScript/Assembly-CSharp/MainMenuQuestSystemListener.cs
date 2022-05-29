using System;
using UnityEngine;

internal sealed class MainMenuQuestSystemListener : MonoBehaviour
{
	public DailyQuestItem dailyQuestItem;

	private static MainMenuQuestSystemListener _instance;

	public MainMenuQuestSystemListener()
	{
	}

	private void Awake()
	{
		MainMenuQuestSystemListener._instance = this;
	}

	private void HandleQuestSystemUpdate(object sender, EventArgs e)
	{
		if (this.dailyQuestItem != null)
		{
			this.dailyQuestItem.Refresh();
		}
	}

	private void OnDestroy()
	{
		QuestSystem.Instance.Updated -= new EventHandler(this.HandleQuestSystemUpdate);
	}

	public static void Refresh()
	{
		if (MainMenuQuestSystemListener._instance == null)
		{
			return;
		}
		if (MainMenuQuestSystemListener._instance.dailyQuestItem == null)
		{
			return;
		}
		MainMenuQuestSystemListener._instance.dailyQuestItem.Refresh();
	}

	private void Start()
	{
		QuestSystem.Instance.Updated += new EventHandler(this.HandleQuestSystemUpdate);
	}
}