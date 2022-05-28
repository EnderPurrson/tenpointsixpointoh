using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
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

	[CompilerGenerated]
	private static Func<QuestBase, bool> _003C_003Ef__am_0024cache8;

	private void Awake()
	{
		QuestSystem.Instance.Updated += HandleQuestSystemUpdate;
		Instance = this;
	}

	private void OnDestroy()
	{
		QuestSystem.Instance.Updated -= HandleQuestSystemUpdate;
	}

	private void HandleQuestSystemUpdate(object sender, EventArgs e)
	{
		if (Defs.IsDeveloperBuild)
		{
			Debug.Log("Refreshing after quest system update.");
		}
		UpdateItems();
	}

	public new void Show()
	{
		if (inBannerSystem && BannerWindowController.SharedController != null)
		{
			BannerWindowController.SharedController.RegisterWindow(this, BannerWindowType.DailyQuests);
			BannerWindowController.SharedController.ForceShowBanner(BannerWindowType.DailyQuests);
		}
		else
		{
			base.gameObject.SetActive(true);
		}
	}

	public new void Hide()
	{
		if (inBannerSystem)
		{
			BannerWindowController.SharedController.HideBannerWindow();
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	private void OnEnable()
	{
		ExpController.LevelUpShown += HandleLevelUpShown;
		UpdateItems();
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
		_backSubscription = BackSystem.Instance.Register(Hide, "Quest Banner");
	}

	private void OnDisable()
	{
		ExpController.LevelUpShown -= HandleLevelUpShown;
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
	}

	private void Update()
	{
		if (blockingFon != null)
		{
			blockingFon.depth = ((!(ExpController.Instance != null) || !ExpController.Instance.WaitingForLevelUpView) ? 100 : 100000);
		}
	}

	public void UpdateItems()
	{
		QuestProgress questProgress = QuestSystem.Instance.QuestProgress;
		int num;
		if (TrainingController.TrainingCompleted && questProgress != null)
		{
			ICollection<QuestBase> values = questProgress.GetActiveQuests().Values;
			if (_003C_003Ef__am_0024cache8 == null)
			{
				_003C_003Ef__am_0024cache8 = _003CUpdateItems_003Em__3BC;
			}
			num = ((values.Count(_003C_003Ef__am_0024cache8) > 0) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		bool flag2 = false;
		for (int i = 0; i < DailyQuests.Length; i++)
		{
			DailyQuestItem dailyQuestItem = DailyQuests[i];
			if (flag)
			{
				if (!dailyQuestItem.gameObject.activeSelf)
				{
					dailyQuestItem.gameObject.SetActive(true);
				}
				dailyQuestItem.FillData(i);
			}
			else if (dailyQuestItem.gameObject.activeSelf)
			{
				dailyQuestItem.gameObject.SetActive(false);
			}
			flag2 = flag2 || dailyQuestItem.CanSkip;
		}
		if (skipHint != null)
		{
			skipHint.gameObject.SetActive(flag2);
		}
		QuestSystem.Instance.SaveQuestProgressIfDirty();
		if (flag)
		{
			noQuestsLabel.SetActive(false);
			questsTable.Reposition();
		}
		else
		{
			noQuestsLabel.SetActive(true);
		}
	}

	private void HandleLevelUpShown()
	{
		if (inBannerSystem)
		{
			if (BannerWindowController.SharedController != null)
			{
				BannerWindowController.SharedController.HideBannerWindowNoShowNext();
			}
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	[CompilerGenerated]
	private static bool _003CUpdateItems_003Em__3BC(QuestBase q)
	{
		return q != null && !q.Rewarded;
	}
}
