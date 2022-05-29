using Rilisoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class DailyQuestsBannerController : BannerWindow
{
	public UISprite blockingFon;

	public DailyQuestItem[] DailyQuests;

	public GameObject noQuestsLabel;

	public UITable questsTable;

	public UILabel skipHint;

	public bool inBannerSystem = true;

	public static DailyQuestsBannerController Instance;

	private IDisposable _backSubscription;

	public DailyQuestsBannerController()
	{
	}

	private void Awake()
	{
		QuestSystem.Instance.Updated += new EventHandler(this.HandleQuestSystemUpdate);
		DailyQuestsBannerController.Instance = this;
	}

	private void HandleLevelUpShown()
	{
		if (!this.inBannerSystem)
		{
			base.gameObject.SetActive(false);
		}
		else if (BannerWindowController.SharedController != null)
		{
			BannerWindowController.SharedController.HideBannerWindowNoShowNext();
		}
	}

	private void HandleQuestSystemUpdate(object sender, EventArgs e)
	{
		if (Defs.IsDeveloperBuild)
		{
			Debug.Log("Refreshing after quest system update.");
		}
		this.UpdateItems();
	}

	public new void Hide()
	{
		if (!this.inBannerSystem)
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			BannerWindowController.SharedController.HideBannerWindow();
		}
	}

	private void OnDestroy()
	{
		QuestSystem.Instance.Updated -= new EventHandler(this.HandleQuestSystemUpdate);
	}

	private void OnDisable()
	{
		ExpController.LevelUpShown -= new Action(this.HandleLevelUpShown);
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	private void OnEnable()
	{
		ExpController.LevelUpShown += new Action(this.HandleLevelUpShown);
		this.UpdateItems();
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.Hide), "Quest Banner");
	}

	public new void Show()
	{
		if (!this.inBannerSystem || !(BannerWindowController.SharedController != null))
		{
			base.gameObject.SetActive(true);
		}
		else
		{
			BannerWindowController.SharedController.RegisterWindow(this, BannerWindowType.DailyQuests);
			BannerWindowController.SharedController.ForceShowBanner(BannerWindowType.DailyQuests);
		}
	}

	private void Update()
	{
		if (this.blockingFon != null)
		{
			this.blockingFon.depth = (!(ExpController.Instance != null) || !ExpController.Instance.WaitingForLevelUpView ? 100 : 100000);
		}
	}

	public void UpdateItems()
	{
		QuestProgress questProgress = QuestSystem.Instance.QuestProgress;
		bool flag = (!TrainingController.TrainingCompleted || questProgress == null ? false : questProgress.GetActiveQuests().Values.Count<QuestBase>((QuestBase q) => (q == null ? false : !q.Rewarded)) > 0);
		bool flag1 = false;
		for (int i = 0; i < (int)this.DailyQuests.Length; i++)
		{
			DailyQuestItem dailyQuests = this.DailyQuests[i];
			if (flag)
			{
				if (!dailyQuests.gameObject.activeSelf)
				{
					dailyQuests.gameObject.SetActive(true);
				}
				dailyQuests.FillData(i);
			}
			else if (dailyQuests.gameObject.activeSelf)
			{
				dailyQuests.gameObject.SetActive(false);
			}
			flag1 = (flag1 ? true : dailyQuests.CanSkip);
		}
		if (this.skipHint != null)
		{
			this.skipHint.gameObject.SetActive(flag1);
		}
		QuestSystem.Instance.SaveQuestProgressIfDirty();
		if (!flag)
		{
			this.noQuestsLabel.SetActive(true);
		}
		else
		{
			this.noQuestsLabel.SetActive(false);
			this.questsTable.Reposition();
		}
	}
}