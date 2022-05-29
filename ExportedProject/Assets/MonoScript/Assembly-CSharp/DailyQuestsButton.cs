using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Linq;
using UnityEngine;

internal sealed class DailyQuestsButton : MonoBehaviour
{
	public bool inBannerSystem = true;

	[SerializeField]
	private DailyQuestsBannerController controller;

	public GameObject rewardIndicator;

	[SerializeField]
	private GameObject dailyQuestsParent;

	private Action OnClickAction;

	public DailyQuestsButton()
	{
	}

	private void Awake()
	{
		if (this.inBannerSystem)
		{
			QuestSystem.Instance.Updated += new EventHandler(this.HandleQuestSystemUpdate);
		}
		else if (Defs.isDaterRegim)
		{
			base.gameObject.SetActive(false);
		}
		if (QuestSystem.Instance.QuestProgress != null)
		{
			this.SetUI();
		}
	}

	private void CheckUnrewardedEvent(object sender, EventArgs e)
	{
		this.SetUI();
	}

	private void HandleQuestSystemUpdate(object sender, EventArgs e)
	{
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log("Refreshing after quest system update.");
		}
		this.SetUI();
	}

	private void OnClick()
	{
		ButtonClickSound.TryPlayClick();
		if (BankController.Instance != null && BankController.Instance.InterfaceEnabled)
		{
			return;
		}
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		if (this.OnClickAction != null)
		{
			this.OnClickAction();
			return;
		}
		if (!this.inBannerSystem)
		{
			if (LoadingInAfterGame.isShowLoading)
			{
				return;
			}
			if ((this.controller == null || this.controller.gameObject == null) && this.dailyQuestsParent != null)
			{
				this.dailyQuestsParent.transform.DestroyChildren();
				DailyQuestsBannerController dailyQuestsBannerController = UnityEngine.Object.Instantiate<DailyQuestsBannerController>(Resources.Load<DailyQuestsBannerController>("Windows/DailyQuests-Window"));
				if (dailyQuestsBannerController != null)
				{
					dailyQuestsBannerController.transform.parent = this.dailyQuestsParent.transform;
					dailyQuestsBannerController.transform.localPosition = Vector3.zero;
					dailyQuestsBannerController.transform.localRotation = Quaternion.identity;
					dailyQuestsBannerController.transform.localScale = Vector3.one;
					int num = base.gameObject.layer;
					dailyQuestsBannerController.gameObject.layer = num;
					IEnumerator<GameObject> enumerator = dailyQuestsBannerController.gameObject.Descendants().GetEnumerator();
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
					dailyQuestsBannerController.inBannerSystem = this.inBannerSystem;
				}
				this.controller = dailyQuestsBannerController;
			}
			if (this.controller != null)
			{
				this.controller.Show();
			}
		}
		else
		{
			BannerWindowController sharedController = BannerWindowController.SharedController;
			if (sharedController == null)
			{
				return;
			}
			sharedController.ForceShowBanner(BannerWindowType.DailyQuests);
		}
	}

	private void OnDestroy()
	{
		if (this.inBannerSystem)
		{
			QuestSystem.Instance.Updated -= new EventHandler(this.HandleQuestSystemUpdate);
		}
	}

	private void OnDisable()
	{
		this.controller = null;
		if (this.dailyQuestsParent != null)
		{
			this.dailyQuestsParent.transform.DestroyChildren();
		}
	}

	private void OnEnable()
	{
		this.SetUI();
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.UpdateUI());
		}
	}

	public void SetUI()
	{
		bool flag = (QuestSystem.Instance.QuestProgress == null ? false : QuestSystem.Instance.AnyActiveQuest);
		if (flag != base.gameObject.activeInHierarchy)
		{
			base.gameObject.SetActive(flag);
		}
		if (this.rewardIndicator != null && QuestSystem.Instance.QuestProgress != null)
		{
			bool flag1 = QuestSystem.Instance.QuestProgress.HasUnrewaredAccumQuests();
			this.rewardIndicator.SetActive(flag1);
		}
	}

	[DebuggerHidden]
	private IEnumerator UpdateUI()
	{
		DailyQuestsButton.u003cUpdateUIu003ec__Iterator181 variable = null;
		return variable;
	}

	public event Action OnClickAction
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.OnClickAction += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.OnClickAction -= value;
		}
	}
}