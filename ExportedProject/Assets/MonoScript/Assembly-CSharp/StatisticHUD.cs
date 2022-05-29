using System;
using System.Collections.Generic;
using UnityEngine;

public class StatisticHUD : MonoBehaviour
{
	[Header("Вкладки")]
	public StatisticHUD.TypeOpenTab curOpenTab;

	public GameObject tabMultiplayer;

	public GameObject tabSingleplayer;

	public GameObject tabLeaguesOld;

	public GameObject tabLeagues;

	[Header("Кнопки по вкладкам")]
	public UIButton btnMultiplayer;

	public UIButton btnSingleplayer;

	public UIButton btnLeagues;

	[Header("добавить в порядке заполнения")]
	[Header("КУБКИ")]
	public List<CupHUD> listAllCup = new List<CupHUD>();

	public StatisticHUD()
	{
	}

	[ContextMenu("Add all cup")]
	private void AddAllCup()
	{
		this.listAllCup.Clear();
		this.listAllCup.AddRange(base.GetComponentsInChildren<CupHUD>(true));
	}

	private void HideAllTab()
	{
		this.tabMultiplayer.SetActive(false);
		this.tabSingleplayer.SetActive(false);
		this.tabLeaguesOld.SetActive(false);
		this.tabLeagues.SetActive(false);
		this.btnMultiplayer.enabled = true;
		this.btnSingleplayer.enabled = true;
		this.btnLeagues.enabled = true;
		this.btnMultiplayer.SetState(UIButtonColor.State.Normal, true);
		this.btnSingleplayer.SetState(UIButtonColor.State.Normal, true);
		this.btnLeagues.SetState(UIButtonColor.State.Normal, true);
	}

	private void OnEnable()
	{
		this.OpenActiveTab(false);
	}

	private void OnOpenLeagues()
	{
		for (int i = 0; i < this.listAllCup.Count; i++)
		{
			this.listAllCup[i].UpdateByOrder(i);
		}
		this.HideAllTab();
		this.btnLeagues.enabled = false;
		this.btnLeagues.SetState(UIButtonColor.State.Pressed, true);
		if (!FriendsController.isUseRatingSystem)
		{
			this.tabLeagues.SetActive(false);
			this.tabLeaguesOld.SetActive(true);
		}
		else
		{
			this.tabLeagues.SetActive(true);
			this.tabLeaguesOld.SetActive(false);
		}
	}

	private void OnOpenMultiplayer()
	{
		this.HideAllTab();
		this.btnMultiplayer.enabled = false;
		this.btnMultiplayer.SetState(UIButtonColor.State.Pressed, true);
		this.tabMultiplayer.SetActive(true);
	}

	private void OnOpenSingleplayer()
	{
		this.HideAllTab();
		this.btnSingleplayer.enabled = false;
		this.btnSingleplayer.SetState(UIButtonColor.State.Pressed, true);
		this.tabSingleplayer.SetActive(true);
	}

	private void OpenActiveTab(bool playSound = false)
	{
		if (playSound)
		{
			ButtonClickSound.TryPlayClick();
		}
		switch (this.curOpenTab)
		{
			case StatisticHUD.TypeOpenTab.multiplayer:
			{
				this.OnOpenMultiplayer();
				break;
			}
			case StatisticHUD.TypeOpenTab.singleplayer:
			{
				this.OnOpenSingleplayer();
				break;
			}
			case StatisticHUD.TypeOpenTab.leagues:
			{
				this.OnOpenLeagues();
				break;
			}
		}
	}

	public void OpenLeagues()
	{
		this.curOpenTab = StatisticHUD.TypeOpenTab.leagues;
		this.OpenActiveTab(true);
	}

	public void OpenMultiplayer()
	{
		this.curOpenTab = StatisticHUD.TypeOpenTab.multiplayer;
		this.OpenActiveTab(true);
	}

	public void OpenSingleplayer()
	{
		this.curOpenTab = StatisticHUD.TypeOpenTab.singleplayer;
		this.OpenActiveTab(true);
	}

	public void OpenTab(StatisticHUD.TypeOpenTab tab)
	{
		this.curOpenTab = tab;
		this.OpenActiveTab(true);
	}

	public enum TypeOpenTab
	{
		multiplayer,
		singleplayer,
		leagues
	}
}