using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Linq;
using UnityEngine;

public class DailyQuestBannerHandler : MonoBehaviour
{
	public bool inBannerSystem = true;

	[SerializeField]
	private DailyQuestsButton questsButton;

	[SerializeField]
	private GameObject _windowRoot;

	[SerializeField]
	private PrefabHandler _prefab;

	[SerializeField]
	private LazyObject<DailyQuestsBannerController> _controller;

	public static DailyQuestBannerHandler Instance
	{
		get;
		private set;
	}

	public DailyQuestBannerHandler()
	{
	}

	private void Awake()
	{
		DailyQuestBannerHandler.Instance = this;
		this._controller = new LazyObject<DailyQuestsBannerController>(this._prefab.ResourcePath, this._windowRoot);
		ExpController.LevelUpShown += new Action(this.HandleLevelUpShown);
		if (this.questsButton != null)
		{
			this.questsButton.OnClickAction += new Action(this.ShowUI);
		}
	}

	private void HandleLevelUpShown()
	{
		if (this._controller.ObjectIsLoaded)
		{
			this._controller.Value.Hide();
		}
	}

	public void HideUI()
	{
		this._controller.Value.Hide();
	}

	private void OnDetroy()
	{
		ExpController.LevelUpShown -= new Action(this.HandleLevelUpShown);
	}

	public void ShowUI()
	{
		if (!this._controller.ObjectIsLoaded)
		{
			DailyQuestsBannerController value = this._controller.Value;
			int num = this._windowRoot.layer;
			value.gameObject.layer = num;
			IEnumerator<GameObject> enumerator = value.gameObject.Descendants().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.layer = num;
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
			this._controller.Value.inBannerSystem = this.inBannerSystem;
		}
		this._controller.Value.Show();
		if (this.questsButton != null)
		{
			this.questsButton.SetUI();
		}
	}
}