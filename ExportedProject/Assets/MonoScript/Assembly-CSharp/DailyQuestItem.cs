using Rilisoft;
using Rilisoft.NullExtensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DailyQuestItem : MonoBehaviour
{
	public UISprite progressBar;

	public GameObject coinsObject;

	public GameObject gemObject;

	public GameObject expObject;

	public UILabel coinsCount;

	public UILabel gemsCount;

	public UILabel expCount;

	public UILabel progressLabel;

	public UILabel questDescription;

	public UITable awardTable;

	public GameObject getRewardButton;

	public GameObject toBattleButton;

	public GameObject completedObject;

	public GameObject questInProgress;

	public UILabel viewAllLabel;

	public bool itemInLobby;

	public UIButton skipButton;

	public UILabel questsButtonLabel;

	public UILabel rewardButtonLabel;

	[Header("Animations for quest frame")]
	public GameObject questSkipFrame;

	public GameObject oldQuest;

	public TweenPosition skipAnimPosition;

	public TweenScale rewardAnim;

	public UISprite modeColor;

	public UISprite modeIcon;

	private QuestInfo _questInfo;

	public bool CanSkip
	{
		get
		{
			if (this._questInfo == null)
			{
				return false;
			}
			return this._questInfo.CanSkip;
		}
	}

	private AccumulativeQuestBase Quest
	{
		get
		{
			return this._questInfo.Map<QuestInfo, AccumulativeQuestBase>((QuestInfo qi) => qi.Quest as AccumulativeQuestBase);
		}
	}

	public DailyQuestItem()
	{
	}

	public void FillData(int slot)
	{
		bool flag;
		if (!TrainingController.TrainingCompleted)
		{
			base.gameObject.SetActive(false);
			return;
		}
		QuestProgress questProgress = QuestSystem.Instance.QuestProgress;
		if (questProgress == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this._questInfo = (slot != -1 ? questProgress.GetActiveQuestInfoBySlot(slot + 1) : questProgress.GetRandomQuestInfo());
		if (this.Quest == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (this.Quest.SetActive())
		{
			Dictionary<string, object> strs = new Dictionary<string, object>()
			{
				{ "Total", "Get" }
			};
			AnalyticsFacade.SendCustomEvent("Daily Quests", strs);
			string str = string.Format(CultureInfo.InvariantCulture, "{0} / {1}", new object[] { this.Quest.Id, QuestConstants.GetDifficultyKey(this.Quest.Difficulty) });
			strs = new Dictionary<string, object>()
			{
				{ "Get", str }
			};
			AnalyticsFacade.SendCustomEvent("Daily Quests", strs);
		}
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		this.questDescription.text = QuestConstants.GetAccumulativeQuestDescriptionByType(this.Quest);
		this.progressLabel.text = string.Format("{0}/{1}", this.Quest.CurrentCount, this.Quest.RequiredCount);
		this.progressBar.fillAmount = (float)this.Quest.CalculateProgress();
		if (Defs.IsDeveloperBuild && base.GetComponent<UISprite>() != null)
		{
			this.oldQuest.SetActive(this.Quest.Day < questProgress.Day);
		}
		if (!this.itemInLobby)
		{
			if (this.Quest.CalculateProgress() < new decimal(1))
			{
				this.getRewardButton.SetActive(false);
				this.completedObject.SetActive(false);
				this.toBattleButton.SetActive(false);
				this.questInProgress.SetActive(true);
				this.questSkipFrame.SetActive(false);
			}
			else
			{
				this.getRewardButton.SetActive(!this.Quest.Rewarded);
				this.completedObject.SetActive(this.Quest.Rewarded);
				this.toBattleButton.SetActive(false);
				this.questInProgress.SetActive(false);
				this.questSkipFrame.SetActive(false);
			}
			if (SceneManager.GetActiveScene().name != Defs.MainMenuScene)
			{
				this.toBattleButton.SetActive(false);
			}
			if (this.modeColor != null)
			{
				UISprite color = this.modeColor;
				QuestImage instance = QuestImage.Instance;
				color.color = instance.GetColor(this._questInfo.Quest);
			}
			if (this.modeIcon != null)
			{
				UISprite spriteName = this.modeIcon;
				QuestImage questImage = QuestImage.Instance;
				spriteName.spriteName = questImage.GetSpriteName(this._questInfo.Quest);
			}
		}
		else if (this._questInfo.Quest != null)
		{
			bool flag1 = (this._questInfo.Quest.CalculateProgress() < new decimal(1) ? false : !this._questInfo.Quest.Rewarded);
			if (this.questsButtonLabel != null && this.rewardButtonLabel != null)
			{
				this.questsButtonLabel.gameObject.SetActive(!flag1);
				this.rewardButtonLabel.gameObject.SetActive(flag1);
				if (flag1)
				{
					DailyQuestsButton component = this.questsButtonLabel.parent.GetComponent<DailyQuestsButton>();
					if (component != null)
					{
						component.SetUI();
					}
				}
			}
			if (this.modeColor != null)
			{
				UISprite uISprite = this.modeColor;
				QuestImage instance1 = QuestImage.Instance;
				uISprite.color = instance1.GetColor(this._questInfo.Quest);
			}
			if (this.modeIcon != null)
			{
				UISprite spriteName1 = this.modeIcon;
				QuestImage questImage1 = QuestImage.Instance;
				spriteName1.spriteName = questImage1.GetSpriteName(this._questInfo.Quest);
			}
		}
		this.coinsCount.text = this.Quest.Reward.Coins.ToString();
		this.gemsCount.text = this.Quest.Reward.Gems.ToString();
		this.expCount.text = this.Quest.Reward.Experience.ToString();
		this.coinsObject.SetActive(this.Quest.Reward.Coins > 0);
		this.gemObject.SetActive(this.Quest.Reward.Gems > 0);
		GameObject gameObject = this.expObject;
		if (this.Quest.Reward.Experience <= 0)
		{
			flag = false;
		}
		else if (ExperienceController.sharedController.currentLevel != ExperienceController.maxLevel)
		{
			flag = true;
		}
		else
		{
			flag = (this.Quest.Reward.Coins != 0 ? false : this.Quest.Reward.Gems == 0);
		}
		gameObject.SetActive(flag);
		this.awardTable.repositionNow = true;
		if (this.skipButton != null)
		{
			this.skipButton.gameObject.SetActive(this._questInfo.CanSkip);
		}
	}

	private string GetQuestMap(AccumulativeQuestBase quest)
	{
		MapAccumulativeQuest mapAccumulativeQuest = quest as MapAccumulativeQuest;
		if (mapAccumulativeQuest == null)
		{
			return string.Empty;
		}
		return mapAccumulativeQuest.Map;
	}

	private int GetQuestMode(AccumulativeQuestBase quest)
	{
		ModeAccumulativeQuest modeAccumulativeQuest = quest as ModeAccumulativeQuest;
		if (modeAccumulativeQuest == null)
		{
			return 0;
		}
		return (int)modeAccumulativeQuest.Mode;
	}

	private void HandleSkip()
	{
		if (this._questInfo == null)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogError("QuestInfo is null.");
			}
			return;
		}
		if (this._questInfo.CanSkip)
		{
			this._questInfo.Skip();
			Dictionary<string, object> strs = new Dictionary<string, object>()
			{
				{ "Total", "Skipped" }
			};
			AnalyticsFacade.SendCustomEvent("Daily Quests", strs);
			QuestBase quest = this._questInfo.Quest;
			string str = string.Format(CultureInfo.InvariantCulture, "{0} / {1}", new object[] { quest.Id, QuestConstants.GetDifficultyKey(quest.Difficulty) });
			strs = new Dictionary<string, object>()
			{
				{ "Skip", str }
			};
			AnalyticsFacade.SendCustomEvent("Daily Quests", strs);
			DailyQuestsBannerController.Instance.Do<DailyQuestsBannerController>((DailyQuestsBannerController c) => c.UpdateItems());
			if (this.questSkipFrame != null)
			{
				this.questSkipFrame.SetActive(true);
			}
			if (this.skipAnimPosition != null)
			{
				this.skipAnimPosition.enabled = true;
			}
		}
		else if (Defs.IsDeveloperBuild)
		{
			Debug.LogError("Cannot skip!");
		}
		MainMenuQuestSystemListener.Refresh();
	}

	private void OnEnable()
	{
		if (this.questSkipFrame != null)
		{
			this.questSkipFrame.SetActive(false);
		}
		if (this.rewardAnim != null)
		{
			this.rewardAnim.enabled = false;
		}
		if (this.skipAnimPosition != null)
		{
			this.skipAnimPosition.enabled = false;
			base.transform.localScale = Vector3.one;
			base.transform.localPosition = this.skipAnimPosition.@from;
		}
		if (this.itemInLobby)
		{
			this.FillData(-1);
		}
	}

	public void OnGetRewardButtonClick()
	{
		if (QuestSystem.Instance.QuestProgress == null)
		{
			return;
		}
		QuestSystem.Instance.QuestProgress.FilterFulfilledTutorialQuests();
		if (this.Quest.CalculateProgress() >= new decimal(1) && !this.Quest.Rewarded)
		{
			this.Quest.SetRewarded();
			Dictionary<string, object> strs = new Dictionary<string, object>()
			{
				{ "Total", "Rewarded" }
			};
			AnalyticsFacade.SendCustomEvent("Daily Quests", strs);
			string str = string.Format(CultureInfo.InvariantCulture, "{0} / {1}", new object[] { this.Quest.Id, QuestConstants.GetDifficultyKey(this.Quest.Difficulty) });
			strs = new Dictionary<string, object>()
			{
				{ "Quests", str }
			};
			AnalyticsFacade.SendCustomEvent("Daily Quests", strs);
			QuestSystem.Instance.QuestProgress.TryRemoveTutorialQuest(this.Quest.Id);
			QuestSystem.Instance.SaveQuestProgressIfDirty();
			this.getRewardButton.SetActive(false);
			this.completedObject.SetActive(true);
			this.rewardAnim.enabled = true;
			Reward reward = this.Quest.Reward;
			if (reward.Coins > 0)
			{
				BankController.AddCoins(reward.Coins, true, AnalyticsConstants.AccrualType.Earned);
			}
			if (reward.Gems > 0)
			{
				BankController.AddGems(reward.Gems, true, AnalyticsConstants.AccrualType.Earned);
			}
			if (reward.Experience > 0)
			{
				ExperienceController.sharedController.addExperience(reward.Experience);
			}
		}
		DailyQuestsBannerController.Instance.UpdateItems();
		MainMenuQuestSystemListener.Refresh();
	}

	public void OnSkipInGameClick()
	{
		this.HandleSkip();
	}

	public void OnSkipInMainMenuClick()
	{
		this.HandleSkip();
	}

	public void OnToBattleButtonClick()
	{
		int num;
		if (this.Quest == null)
		{
			Debug.LogError("Quest is null.");
			return;
		}
		string id = this.Quest.Id;
		if (id != null)
		{
			if (DailyQuestItem.u003cu003ef__switchu0024mapD == null)
			{
				Dictionary<string, int> strs = new Dictionary<string, int>(15)
				{
					{ "winInMode", 0 },
					{ "killInMode", 0 },
					{ "killFlagCarriers", 1 },
					{ "captureFlags", 1 },
					{ "capturePoints", 2 },
					{ "killNpcWithWeapon", 3 },
					{ "winInMap", 4 },
					{ "killWithWeapon", 5 },
					{ "killViaHeadshot", 5 },
					{ "killWithGrenade", 5 },
					{ "revenge", 5 },
					{ "breakSeries", 5 },
					{ "makeSeries", 5 },
					{ "surviveWavesInArena", 6 },
					{ "killInCampaign", 7 }
				};
				DailyQuestItem.u003cu003ef__switchu0024mapD = strs;
			}
			if (DailyQuestItem.u003cu003ef__switchu0024mapD.TryGetValue(id, out num))
			{
				switch (num)
				{
					case 0:
					{
						this.OpenConnectScene(this.GetQuestMode(this.Quest));
						break;
					}
					case 1:
					{
						this.OpenConnectScene(4);
						break;
					}
					case 2:
					{
						this.OpenConnectScene(5);
						break;
					}
					case 3:
					{
						this.OpenConnectScene(1);
						break;
					}
					case 4:
					{
						ConnectSceneNGUIController.selectedMap = this.GetQuestMap(this.Quest);
						MainMenuController.sharedController.OnClickMultiplyerButton();
						break;
					}
					case 5:
					{
						MainMenuController.sharedController.OnClickMultiplyerButton();
						break;
					}
					case 6:
					{
						MainMenuController.sharedController.StartSurvivalButton();
						break;
					}
					case 7:
					{
						MainMenuController.sharedController.StartCampaingButton();
						break;
					}
				}
			}
		}
	}

	public void OnViewAllButtonClick()
	{
		BannerWindowController sharedController = BannerWindowController.SharedController;
		if (sharedController == null)
		{
			return;
		}
		sharedController.ForceShowBanner(BannerWindowType.DailyQuests);
		base.gameObject.SetActive(false);
	}

	private void OpenConnectScene(int mode)
	{
		PlayerPrefs.SetInt("RegimMulty", mode);
		ConnectSceneNGUIController.directedFromQuests = true;
		MainMenuController.sharedController.OnClickMultiplyerButton();
	}

	public void Refresh()
	{
		if (this.itemInLobby)
		{
			this.FillData(-1);
			return;
		}
		if (this._questInfo == null)
		{
			return;
		}
		if (this._questInfo.Quest != null)
		{
			this.FillData(this.Quest.Slot);
		}
	}
}