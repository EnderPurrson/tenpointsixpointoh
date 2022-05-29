using Facebook.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rilisoft
{
	public sealed class LevelCompleteScript : MonoBehaviour
	{
		private const string AllStarsForBoxRewardWindowIsShownNameBase = "AllStarsForBoxRewardWindowIsShown_";

		private const string AllSecretsForBoxRewardWindowIsShownNameBase = "AllSecretsForBoxRewardWindowIsShown_";

		public static LevelCompleteScript sharedScript;

		public RewardWindowBase rewardWindow;

		public RewardWindowBase rewardWindowSurvival;

		public CampaignLevelCompleteRewardSettings rewardSettings;

		public ArenaRewardWindowSettings survivalRewardWindowSettings;

		public GameObject mainInterface;

		public GameObject premium;

		public Transform RentWindowPoint;

		public GameObject mainPanel;

		public GameObject loadingPanel;

		public GameObject quitButton;

		public GameObject menuButton;

		public GameObject retryButton;

		public GameObject nextButton;

		public GameObject shopButton;

		public GameObject brightStarPrototypeSprite;

		public GameObject darkStarPrototypeSprite;

		public GameObject award1coinSprite;

		public GameObject checkboxSpritePrototype;

		public AudioClip[] starClips;

		public AudioClip shopButtonSound;

		public AudioClip awardClip;

		public GameObject survivalResults;

		public GameObject facebookButton;

		public GameObject twitterButton;

		public GameObject backgroundTexture;

		public GameObject backgroundSurvivalTexture;

		public GameObject[] statisticLabels;

		public GameObject gameOverSprite;

		public UICamera uiCamera;

		private static LevelCompleteScript _instance;

		private int _numOfRewardWindowsShown;

		private bool _hasAwardForMission;

		private bool _shouldBlinkCoinsIndicatorAfterRewardWindow;

		private bool _shouldBlinkGemsIndicatorAfterRewardWindow;

		private bool _shouldShowAllStarsCollectedRewardWindow;

		private bool _shouldShowAllSecretsCollectedRewardWindow;

		private IDisposable _backSubscription;

		private static Dictionary<string, string> boxNamesTwitter;

		private bool _awardConferred;

		private AudioSource _awardAudioSource;

		private ExperienceController _experienceController;

		private int _oldStarCount;

		private int _starCount;

		private ShopNGUIController _shopInstance;

		private string _nextSceneName = string.Empty;

		private bool _isLastLevel;

		private int? _boxCompletionExperienceAward;

		private bool completedFirstTime;

		private bool _gameOver;

		private bool _shouldShowFacebookButton;

		private bool _shouldShowTwitterButton;

		public bool DisplayLevelResultIsRunning
		{
			get;
			set;
		}

		public bool DisplaySurvivalResultIsRunning
		{
			get;
			set;
		}

		public static bool IsInterfaceBusy
		{
			get
			{
				bool flag;
				if (LevelCompleteScript.sharedScript == null)
				{
					flag = false;
				}
				else
				{
					flag = (LevelCompleteScript.IsShowRewardWindow() || LevelCompleteScript.sharedScript.DisplayLevelResultIsRunning ? true : LevelCompleteScript.sharedScript.DisplaySurvivalResultIsRunning);
				}
				return flag;
			}
		}

		static LevelCompleteScript()
		{
			LevelCompleteScript._instance = null;
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ "Real", "PIXELATED WORLD" },
				{ "minecraft", "BLOCK WORLD" },
				{ "Crossed", "CROSSED WORLDS" }
			};
			LevelCompleteScript.boxNamesTwitter = strs;
		}

		public LevelCompleteScript()
		{
		}

		private string _SocialMessage()
		{
			if (Defs.IsSurvival)
			{
				string str = string.Format(LocalizationStore.GetByDefault("Key_1382"), PlayerPrefs.GetInt(Defs.WavesSurvivedS, 0), PlayerPrefs.GetInt(Defs.KilledZombiesSett, 0), GlobalGameController.Score);
				UnityEngine.Debug.Log(str);
				return str;
			}
			SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(CurrentCampaignGame.levelSceneName);
			if (infoScene == null)
			{
				return "error map";
			}
			if (this._gameOver)
			{
				string str1 = string.Format(LocalizationStore.GetByDefault("Key_1380"), infoScene.TranslateName);
				UnityEngine.Debug.Log(str1);
				return str1;
			}
			string str2 = string.Format(LocalizationStore.GetByDefault("Key_1382"), infoScene.TranslateName, this._starCount);
			UnityEngine.Debug.Log(str2);
			return str2;
		}

		private bool AllSecretsForBoxRewardWindowIsShown(string boXName)
		{
			return PlayerPrefs.GetInt(string.Concat("AllSecretsForBoxRewardWindowIsShown_", boXName), 0) == 1;
		}

		private bool AllStarsForBoxRewardWindowIsShown(string boXName)
		{
			return PlayerPrefs.GetInt(string.Concat("AllStarsForBoxRewardWindowIsShown_", boXName), 0) == 1;
		}

		private void Awake()
		{
			RewardWindowBase.Shown += new Action(this.HandleRewardWindowShown);
			LevelCompleteScript.sharedScript = this;
			EventDelegate.Add(this.rewardWindow.hideButton.onClick, new EventDelegate(this, "HideRewardWindow"));
			EventDelegate.Add(this.rewardWindow.continueButton.onClick, new EventDelegate(this, "HideRewardWindow"));
			EventDelegate.Add(this.rewardWindow.collect.onClick, new EventDelegate(this, "HideRewardWindow"));
			EventDelegate.Add(this.rewardWindow.collectAndShare.onClick, new EventDelegate(this, "HideRewardWindow"));
			EventDelegate.Add(this.rewardWindow.continueAndShare.onClick, new EventDelegate(this, "HideRewardWindow"));
			EventDelegate.Add(this.rewardWindowSurvival.hideButton.onClick, new EventDelegate(this, "HideRewardWindowSurvival"));
			EventDelegate.Add(this.rewardWindowSurvival.continueButton.onClick, new EventDelegate(this, "HideRewardWindowSurvival"));
			EventDelegate.Add(this.rewardWindowSurvival.collect.onClick, new EventDelegate(this, "HideRewardWindowSurvival"));
			EventDelegate.Add(this.rewardWindowSurvival.continueAndShare.onClick, new EventDelegate(this, "HideRewardWindowSurvival"));
			EventDelegate.Add(this.rewardWindowSurvival.collectAndShare.onClick, new EventDelegate(this, "HideRewardWindowSurvival"));
			FacebookController.StoryPriority storyPriority = FacebookController.StoryPriority.Red;
			this.rewardWindowSurvival.priority = storyPriority;
			this.rewardWindowSurvival.twitterPriority = FacebookController.StoryPriority.ArenaLimit;
			this.rewardWindowSurvival.shareAction = () => FacebookController.PostOpenGraphStory("complete", "fight", storyPriority, new Dictionary<string, string>()
			{
				{ "map", Defs.SurvivalMaps[Defs.CurrentSurvMapIndex] }
			});
			this.rewardWindowSurvival.HasReward = false;
			this.rewardWindowSurvival.twitterStatus = () => "I've beaten ARENA score in @PixelGun3D! Can you beat my record? #pixelgun3d #pixelgun #pg3d #mobile #fps #shooter http://goo.gl/8fzL9u";
			this.rewardWindowSurvival.EventTitle = "Arena Survival";
		}

		private static void BindButtonHandler(GameObject button, EventHandler handler)
		{
			if (button != null)
			{
				ButtonHandler component = button.GetComponent<ButtonHandler>();
				if (component != null)
				{
					component.Clicked += handler;
				}
			}
		}

		private static string BoxNameForTwitter()
		{
			return LevelCompleteScript.boxNamesTwitter[CurrentCampaignGame.boXName];
		}

		private static int CalculateExperienceAward(int score)
		{
			if (ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel == ExperienceController.maxLevel)
			{
				return 0;
			}
			int num = (!Application.isEditor ? 1 : 100);
			if (score < 15000 / num)
			{
				return 0;
			}
			if (score < 50000 / num)
			{
				return 10;
			}
			if (score < 100000 / num)
			{
				return 35;
			}
			if (score < 150000 / num)
			{
				return 50;
			}
			return 75;
		}

		private int CoinsToAddForBox()
		{
			int item = 0;
			if (LevelCompleteScript.IsBox1Completed())
			{
				item = LevelBox.campaignBoxes[0].coins;
			}
			else if (LevelCompleteScript.IsBox2Completed())
			{
				item = LevelBox.campaignBoxes[1].coins;
			}
			else if (LevelCompleteScript.IsBox3Completed())
			{
				item = LevelBox.campaignBoxes[2].coins;
			}
			return item;
		}

		[DebuggerHidden]
		private IEnumerator DisplayLevelResult()
		{
			LevelCompleteScript.u003cDisplayLevelResultu003ec__Iterator16A variable = null;
			return variable;
		}

		private void DisplaySurvivalResult()
		{
			try
			{
				this.DisplaySurvivalResultIsRunning = true;
				this.menuButton.SetActive(false);
				this.retryButton.SetActive(false);
				this.nextButton.SetActive(false);
				this.shopButton.SetActive(false);
				this.quitButton.SetActive(false);
				this.survivalResults.SetActive(true);
				this.retryButton.SetActive(true);
				this.shopButton.SetActive(true);
				this.quitButton.SetActive(true);
				base.StartCoroutine(this.TryToShowExpiredBanner());
			}
			finally
			{
				this.DisplaySurvivalResultIsRunning = false;
			}
		}

		private static string EnglishNameForCompletedLevel(out CampaignLevel campaignLevel)
		{
			campaignLevel = LevelBox.GetLevelBySceneName(CurrentCampaignGame.levelSceneName);
			if (LevelCompleteScript.IsBox3Completed())
			{
				return "???";
			}
			return ((campaignLevel == null || campaignLevel.localizeKeyForLevelMap == null ? "FARM" : LocalizationStore.GetByDefault(campaignLevel.localizeKeyForLevelMap) ?? "FARM")).Replace("\n", " ");
		}

		private int GemsToAddForBox()
		{
			int item = 0;
			if (LevelCompleteScript.IsBox1Completed())
			{
				item = LevelBox.campaignBoxes[0].gems;
			}
			else if (LevelCompleteScript.IsBox2Completed())
			{
				item = LevelBox.campaignBoxes[1].gems;
			}
			else if (LevelCompleteScript.IsBox3Completed())
			{
				item = LevelBox.campaignBoxes[2].gems;
			}
			return item;
		}

		private void GiveAwardForCampaign()
		{
			int num = 0;
			int addForBox = 0;
			if (this._awardConferred || this._hasAwardForMission)
			{
				num = Mathf.Min(LevelCompleteScript.InitializeCoinIndexBound(), this._starCount);
				if (num > 0)
				{
					FlurryEvents.LogCoinsGained("Campaign", num);
				}
				for (int i = 0; i < num; i++)
				{
					FlurryPluginWrapper.LogCoinEarned();
				}
				num = num * (PremiumAccountController.Instance == null ? 1 : PremiumAccountController.Instance.RewardCoeff);
				addForBox = 0;
				if (this._awardConferred)
				{
					addForBox = this.GemsToAddForBox();
					FlurryEvents.LogGemsGained("Campaign", addForBox);
					int addForBox1 = this.CoinsToAddForBox();
					num += addForBox1;
					FlurryEvents.LogCoinsGained("Campaign", addForBox1);
					LevelCompleteScript.PostBoxCompletedAchievement();
				}
				if (num > 0)
				{
					BankController.AddCoins(num, true, AnalyticsConstants.AccrualType.Earned);
				}
				if (addForBox > 0)
				{
					BankController.AddGems(addForBox, true, AnalyticsConstants.AccrualType.Earned);
				}
			}
			int value = 0;
			if (this._starCount == 3 && this._oldStarCount < 3 && ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel < ExperienceController.maxLevel)
			{
				value += 5;
			}
			if (this._boxCompletionExperienceAward.HasValue && ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel < ExperienceController.maxLevel)
			{
				value += this._boxCompletionExperienceAward.Value;
			}
			value = value * (PremiumAccountController.Instance == null ? 1 : PremiumAccountController.Instance.RewardCoeff);
			if (value != 0)
			{
				this._experienceController.addExperience(value);
			}
			if (num > 0 || addForBox > 0)
			{
				base.StartCoroutine(this.PlayGetCoinsClip());
				if (addForBox > 0)
				{
					this._shouldBlinkGemsIndicatorAfterRewardWindow = true;
				}
				if (num > 0)
				{
					this._shouldBlinkCoinsIndicatorAfterRewardWindow = true;
				}
			}
			bool flag = (!this._awardConferred ? false : LevelCompleteScript.IsBox3Completed());
			CampaignLevel campaignLevel = null;
			string str2 = LevelCompleteScript.EnglishNameForCompletedLevel(out campaignLevel);
			string str3 = string.Format("All enemies {0} {1} are defeated in @PixelGun3D! Join my adventures now! #pixelgun3d #pixelgun #pg3d http://goo.gl/8fzL9u", campaignLevel.predlog, str2);
			string str4 = "Level Complete";
			if (this._isLastLevel)
			{
				if (LevelCompleteScript.IsBox1Completed())
				{
					str3 = "I’ve defeated the RIDER in @PixelGun3D! Join my adventures now! #pixelgun3d #pixelgun #3d #pg3d #mobile #fps http://goo.gl/8fzL9u";
					str4 = "Box 1 Complete";
				}
				else if (LevelCompleteScript.IsBox2Completed())
				{
					str3 = "I’ve defeated the DRAGON in @PixelGun3D! Join my adventures now! #pixelgun3d #pixelgun #3d #pg3d #mobile #fps http://goo.gl/8fzL9u";
					str4 = "Box 2 Complete";
				}
				else if (LevelCompleteScript.IsBox3Completed())
				{
					str3 = "I’ve defeated the EVIL BUG in @PixelGun3D! Join my adventures now! #pixelgun3d #pixelgun #3d #pg3d #mobile #fps http://goo.gl/8fzL9u";
					str4 = "Box 3 Complete";
				}
			}
			FacebookController.StoryPriority storyPriority1 = (!this._isLastLevel ? FacebookController.StoryPriority.Red : FacebookController.StoryPriority.Green);
			this.rewardWindow.priority = storyPriority1;
			this.rewardWindow.twitterStatus = () => str3;
			this.rewardWindow.EventTitle = str4;
			this.rewardWindow.HasReward = true;
			this.rewardWindow.shareAction = () => {
				Dictionary<string, string> strs;
				string str;
				string str1;
				Dictionary<string, string> strs1;
				str = (!this._isLastLevel ? "complete" : "finish");
				str1 = (!this._isLastLevel ? "mission" : "chapter");
				FacebookController.StoryPriority storyPriority = storyPriority1;
				if (!this._isLastLevel)
				{
					strs = new Dictionary<string, string>()
					{
						{ "mission", CurrentCampaignGame.levelSceneName }
					};
					strs1 = strs;
				}
				else
				{
					strs = new Dictionary<string, string>()
					{
						{ "chapter", CurrentCampaignGame.boXName }
					};
					strs1 = strs;
				}
				FacebookController.PostOpenGraphStory(str, str1, storyPriority, strs1);
			};
			this.rewardSettings.normalBackground.SetActive((PremiumAccountController.Instance == null ? true : !PremiumAccountController.Instance.isAccountActive));
			this.rewardSettings.premiumBackground.SetActive((PremiumAccountController.Instance == null ? false : PremiumAccountController.Instance.isAccountActive));
			foreach (UILabel rewardSetting in this.rewardSettings.bossDefeatedHeader)
			{
				rewardSetting.gameObject.SetActive(this._awardConferred);
				if (!this._awardConferred)
				{
					continue;
				}
				if (LevelCompleteScript.IsBox1Completed())
				{
					rewardSetting.text = LocalizationStore.Get("Key_1546");
				}
				else if (!LevelCompleteScript.IsBox2Completed())
				{
					if (!LevelCompleteScript.IsBox3Completed())
					{
						continue;
					}
					rewardSetting.text = LocalizationStore.Get("Key_1548");
				}
				else
				{
					rewardSetting.text = LocalizationStore.Get("Key_1547");
				}
			}
			foreach (UILabel boxCompletedLabel in this.rewardSettings.boxCompletedLabels)
			{
				boxCompletedLabel.gameObject.SetActive(this._awardConferred);
				if (!this._awardConferred)
				{
					continue;
				}
				if (LevelCompleteScript.IsBox1Completed())
				{
					boxCompletedLabel.text = LocalizationStore.Get("Key_1549");
				}
				else if (!LevelCompleteScript.IsBox2Completed())
				{
					if (!LevelCompleteScript.IsBox3Completed())
					{
						continue;
					}
					boxCompletedLabel.text = LocalizationStore.Get("Key_1551");
				}
				else
				{
					boxCompletedLabel.text = LocalizationStore.Get("Key_1550");
				}
			}
			foreach (UILabel uILabel in this.rewardSettings.missionHeader)
			{
				uILabel.gameObject.SetActive(!this._awardConferred);
			}
			float single = (!flag ? 1f : 0.8f);
			this.rewardSettings.coinsReward.gameObject.SetActive(num > 0);
			this.rewardSettings.coinsReward.localScale = new Vector3(single, single, single);
			foreach (UILabel coinsRewardLabel in this.rewardSettings.coinsRewardLabels)
			{
				coinsRewardLabel.text = string.Concat("+", num.ToString(), " ", LocalizationStore.Get("Key_0275"));
			}
			this.rewardSettings.coinsMultiplierContainer.SetActive(((PremiumAccountController.Instance == null ? 1 : PremiumAccountController.Instance.RewardCoeff) <= 1 ? false : !this._awardConferred));
			foreach (UILabel coinsMultiplierLabel in this.rewardSettings.coinsMultiplierLabels)
			{
				UILabel uILabel1 = coinsMultiplierLabel;
				int num1 = (PremiumAccountController.Instance == null ? 1 : PremiumAccountController.Instance.RewardCoeff);
				uILabel1.text = string.Concat("x", num1.ToString());
			}
			this.rewardSettings.gemsReward.gameObject.SetActive(addForBox > 0);
			this.rewardSettings.gemsReward.localScale = new Vector3(single, single, single);
			foreach (UILabel gemsRewrdLabel in this.rewardSettings.gemsRewrdLabels)
			{
				gemsRewrdLabel.text = string.Concat("+", addForBox.ToString(), " ", LocalizationStore.Get("Key_0951"));
			}
			this.rewardSettings.gemsMultyplierContainer.SetActive(((PremiumAccountController.Instance == null ? 1 : PremiumAccountController.Instance.RewardCoeff) <= 1 ? false : !this._awardConferred));
			foreach (UILabel gemsMultiplierLabel in this.rewardSettings.gemsMultiplierLabels)
			{
				UILabel uILabel2 = gemsMultiplierLabel;
				int num2 = (PremiumAccountController.Instance == null ? 1 : PremiumAccountController.Instance.RewardCoeff);
				uILabel2.text = string.Concat("x", num2.ToString());
			}
			this.rewardSettings.experienceReward.gameObject.SetActive(value > 0);
			this.rewardSettings.experienceReward.localScale = new Vector3(single, single, single);
			foreach (UILabel experienceRewardLabel in this.rewardSettings.experienceRewardLabels)
			{
				experienceRewardLabel.text = string.Concat("+", value.ToString(), " ", LocalizationStore.Get("Key_0204"));
			}
			this.rewardSettings.expMultiplier.SetActive((PremiumAccountController.Instance == null ? 1 : PremiumAccountController.Instance.RewardCoeff) > 1);
			foreach (UILabel expMultiplierLabel in this.rewardSettings.expMultiplierLabels)
			{
				UILabel uILabel3 = expMultiplierLabel;
				int num3 = (PremiumAccountController.Instance == null ? 1 : PremiumAccountController.Instance.RewardCoeff);
				uILabel3.text = string.Concat("x", num3.ToString());
			}
			this.rewardSettings.badcode.gameObject.SetActive(flag);
			this.rewardSettings.badcode.localScale = new Vector3(single, single, single);
			this.rewardSettings.grid.Reposition();
		}

		[Obfuscation(Exclude=true)]
		private void GoToChooseLevel()
		{
			Singleton<SceneLoader>.Instance.LoadScene("ChooseLevel", LoadSceneMode.Single);
		}

		private void HandleFacebookButton(object sender, EventArgs args)
		{
			if (this._shopInstance != null)
			{
				return;
			}
			FlurryPluginWrapper.LogFacebook();
			FacebookController.ShowPostDialog();
		}

		private void HandleMenuButton(object sender, EventArgs args)
		{
			if (this._shopInstance != null)
			{
				return;
			}
			if (Defs.IsSurvival)
			{
				FlurryPluginWrapper.LogEvent("Back to Main Menu");
			}
			Singleton<SceneLoader>.Instance.LoadScene((!Defs.IsSurvival ? "ChooseLevel" : Defs.MainMenuScene), LoadSceneMode.Single);
		}

		private void HandleNextButton(object sender, EventArgs args)
		{
			if (this._shopInstance != null)
			{
				return;
			}
			if (this._isLastLevel)
			{
				LevelArt.endOfBox = true;
				Singleton<SceneLoader>.Instance.LoadScene((!LevelArt.ShouldShowArts ? "ChooseLevel" : "LevelArt"), LoadSceneMode.Single);
			}
			else
			{
				CurrentCampaignGame.levelSceneName = this._nextSceneName;
				LevelCompleteScript.SetInitialAmmoForAllGuns();
				LevelArt.endOfBox = false;
				Singleton<SceneLoader>.Instance.LoadScene((!LevelArt.ShouldShowArts ? "CampaignLoading" : "LevelArt"), LoadSceneMode.Single);
			}
		}

		private void HandleQuitButton(object sender, EventArgs args)
		{
			ActivityIndicator.IsActiveIndicator = true;
			this.loadingPanel.SetActive(true);
			this.mainPanel.SetActive(false);
			ExperienceController.sharedController.isShowRanks = false;
			base.Invoke("QuitLevel", 0.1f);
		}

		private void HandleResumeFromShop()
		{
			if (this._shopInstance == null)
			{
				return;
			}
			ShopNGUIController.GuiActive = false;
			this._shopInstance.resumeAction = () => {
			};
			this._shopInstance = null;
			if (coinsPlashka.thisScript != null)
			{
				coinsPlashka.thisScript.enabled = false;
			}
			this.quitButton.SetActive(Defs.IsSurvival);
			if (this._experienceController != null)
			{
				this._experienceController.isShowRanks = true;
			}
			if (!Defs.IsSurvival)
			{
				this.backgroundTexture.SetActive(true);
			}
			else
			{
				this.backgroundSurvivalTexture.SetActive(true);
			}
		}

		private void HandleRetryButton(object sender, EventArgs args)
		{
			if (this._shopInstance != null)
			{
				return;
			}
			if (Defs.IsSurvival)
			{
				WeaponManager.sharedManager.Reset(0);
			}
			else
			{
				LevelCompleteScript.SetInitialAmmoForAllGuns();
			}
			GlobalGameController.Score = 0;
			if (Defs.IsSurvival)
			{
				Defs.CurrentSurvMapIndex = UnityEngine.Random.Range(0, (int)Defs.SurvivalMaps.Length);
			}
			Singleton<SceneLoader>.Instance.LoadScene("CampaignLoading", LoadSceneMode.Single);
		}

		private void HandleRewardWindowShown()
		{
			this._numOfRewardWindowsShown++;
		}

		private void HandleShopButton(object sender, EventArgs args)
		{
			if (this._shopInstance != null)
			{
				return;
			}
			this._shopInstance = ShopNGUIController.sharedShop;
			if (this._shopInstance != null)
			{
				this._shopInstance.SetInGame(false);
				ShopNGUIController.GuiActive = true;
				if (this.shopButtonSound != null && Defs.isSoundFX)
				{
					NGUITools.PlaySound(this.shopButtonSound);
				}
				if (!Defs.IsSurvival)
				{
					this.backgroundTexture.SetActive(false);
				}
				else
				{
					this.backgroundSurvivalTexture.SetActive(false);
				}
				this._shopInstance.resumeAction = new Action(this.HandleResumeFromShop);
			}
			this.quitButton.SetActive(false);
		}

		private void HandleTwitterButton(object sender, EventArgs args)
		{
			if (Application.isEditor)
			{
				UnityEngine.Debug.Log(string.Concat("Send Twitter: ", this._SocialMessage()));
				return;
			}
			if (this._shopInstance != null)
			{
				return;
			}
			FlurryPluginWrapper.LogTwitter();
			if (TwitterController.Instance != null)
			{
				TwitterController.Instance.PostStatusUpdate(this._SocialMessage(), null);
			}
		}

		public void HideRewardWindow()
		{
			ButtonClickSound.TryPlayClick();
			this.mainInterface.SetActive(true);
			this.rewardWindow.gameObject.SetActive(false);
			if (!Defs.IsSurvival && this.brightStarPrototypeSprite != null && this.darkStarPrototypeSprite != null)
			{
				base.StartCoroutine(this.DisplayLevelResult());
			}
		}

		public void HideRewardWindowSurvival()
		{
			ButtonClickSound.TryPlayClick();
			this.mainInterface.SetActive(true);
			this.rewardWindowSurvival.gameObject.SetActive(false);
			this.DisplaySurvivalResult();
		}

		private bool InitializeAwardConferred()
		{
			return (!this._isLastLevel ? false : this.completedFirstTime);
		}

		private static int InitializeCoinIndexBound()
		{
			return Defs.diffGame + 1;
		}

		private static ExperienceController InitializeExperienceController()
		{
			ExperienceController component = null;
			GameObject gameObject = GameObject.FindGameObjectWithTag("ExperienceController");
			if (gameObject != null)
			{
				component = gameObject.GetComponent<ExperienceController>();
			}
			if (component != null)
			{
				component.posRanks = new Vector2(21f * Defs.Coef, 21f * Defs.Coef);
				component.isShowRanks = true;
				if (ExpController.Instance != null)
				{
					ExpController.Instance.InterfaceEnabled = true;
				}
			}
			else
			{
				UnityEngine.Debug.LogError("Cannot find experience controller.");
			}
			return component;
		}

		private static int InitializeStarCount()
		{
			int num = 1;
			if (CurrentCampaignGame.completeInTime)
			{
				num++;
			}
			if (CurrentCampaignGame.withoutHits)
			{
				num++;
			}
			return num;
		}

		private static bool IsBox1Completed()
		{
			return CurrentCampaignGame.levelSceneName.Equals("School");
		}

		private static bool IsBox2Completed()
		{
			return CurrentCampaignGame.levelSceneName.StartsWith("Gluk");
		}

		private static bool IsBox3Completed()
		{
			return CurrentCampaignGame.levelSceneName.Equals("Code_campaign3");
		}

		public static bool IsShowRewardWindow()
		{
			if (LevelCompleteScript.sharedScript == null)
			{
				return false;
			}
			bool flag = (!(LevelCompleteScript.sharedScript.rewardWindowSurvival != null) || !(LevelCompleteScript.sharedScript.rewardWindowSurvival.gameObject != null) ? false : LevelCompleteScript.sharedScript.rewardWindowSurvival.gameObject.activeInHierarchy);
			return (flag ? true : (!(LevelCompleteScript.sharedScript.rewardWindow != null) || !(LevelCompleteScript.sharedScript.rewardWindow.gameObject != null) ? false : LevelCompleteScript.sharedScript.rewardWindow.gameObject.activeInHierarchy));
		}

		private void OnDestroy()
		{
			LevelCompleteScript._instance = null;
			if (this._experienceController != null)
			{
				this._experienceController.isShowRanks = false;
			}
			PlayerPrefs.Save();
			RewardWindowBase.Shown -= new Action(this.HandleRewardWindowShown);
			LevelCompleteScript.sharedScript = null;
		}

		private void OnDisable()
		{
			if (this._backSubscription != null)
			{
				this._backSubscription.Dispose();
				this._backSubscription = null;
			}
		}

		private void OnEnable()
		{
			if (this._backSubscription != null)
			{
				this._backSubscription.Dispose();
			}
			this._backSubscription = BackSystem.Instance.Register(() => this.HandleMenuButton(this, EventArgs.Empty), "Level Complete");
		}

		[DebuggerHidden]
		private IEnumerator PlayGetCoinsClip()
		{
			LevelCompleteScript.u003cPlayGetCoinsClipu003ec__Iterator169 variable = null;
			return variable;
		}

		private static void PostBoxCompletedAchievement()
		{
			string empty = string.Empty;
			string str = string.Empty;
			bool flag = LevelCompleteScript.IsBox1Completed();
			bool flag1 = LevelCompleteScript.IsBox2Completed();
			if (flag)
			{
				empty = (BuildSettings.BuildTargetPlatform != RuntimePlatform.IPhonePlayer ? "CgkIr8rGkPIJEAIQCA" : "block_world_id");
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					empty = "Block_Survivor_id";
				}
				str = "Block World Survivor";
			}
			else if (flag1)
			{
				empty = "CgkIr8rGkPIJEAIQCQ";
				str = "Dragon Slayer";
			}
			if (string.IsNullOrEmpty(empty))
			{
				UnityEngine.Debug.LogWarning(string.Concat("Achievement Box Completed: id is null. Scene: ", CurrentCampaignGame.levelSceneName));
			}
			else if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
				Social.ReportProgress(empty, 100, (bool success) => UnityEngine.Debug.Log(string.Format("Achievement {0} completed: {1}", str, success)));
			}
			else
			{
				AGSAchievementsClient.UpdateAchievementProgress(empty, 100f, 0);
				UnityEngine.Debug.Log(string.Format("Achievement {0} completed.", str));
			}
		}

		[Obfuscation(Exclude=true)]
		private void QuitLevel()
		{
			FlurryPluginWrapper.LogEvent("Back to Main Menu");
			Singleton<SceneLoader>.Instance.LoadSceneAsync(Defs.MainMenuScene, LoadSceneMode.Single);
		}

		private static void SetInitialAmmoForAllGuns()
		{
			IEnumerator enumerator = WeaponManager.sharedManager.allAvailablePlayerWeapons.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Weapon current = (Weapon)enumerator.Current;
					WeaponSounds component = current.weaponPrefab.GetComponent<WeaponSounds>();
					if (current.currentAmmoInClip + current.currentAmmoInBackpack >= component.InitialAmmoWithEffectsApplied + component.ammoInClip)
					{
						if (current.currentAmmoInClip >= component.ammoInClip)
						{
							continue;
						}
						int num = Mathf.Min(component.ammoInClip - current.currentAmmoInClip, current.currentAmmoInBackpack);
						Weapon weapon = current;
						weapon.currentAmmoInClip = weapon.currentAmmoInClip + num;
						Weapon weapon1 = current;
						weapon1.currentAmmoInBackpack = weapon1.currentAmmoInBackpack - num;
					}
					else
					{
						current.currentAmmoInClip = component.ammoInClip;
						current.currentAmmoInBackpack = component.InitialAmmoWithEffectsApplied;
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable == null)
				{
				}
				disposable.Dispose();
			}
		}

		public static void SetInputEnabled(bool enabled)
		{
			if (LevelCompleteScript._instance != null)
			{
				LevelCompleteScript._instance.uiCamera.enabled = enabled;
			}
		}

		private void Start()
		{
			CampaignLevel campaignLevel;
			bool flag;
			if (Application.isEditor)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
			QuestSystem.Instance.SaveQuestProgressIfDirty();
			if (!Defs.IsSurvival)
			{
				this.backgroundTexture.SetActive(true);
			}
			else
			{
				this.backgroundSurvivalTexture.SetActive(true);
			}
			ActivityIndicator.IsActiveIndicator = false;
			if (PlayerPrefs.GetInt("IsGameOver", 0) == 1)
			{
				this._gameOver = true;
				PlayerPrefs.SetInt("IsGameOver", 0);
			}
			if (!this._gameOver && !Defs.IsSurvival)
			{
				StoreKitEventListener.State.PurchaseKey = "Level Completed";
				StoreKitEventListener.State.Parameters["Level"] = string.Concat(CurrentCampaignGame.levelSceneName, " Level Completed");
			}
			else if (this._gameOver && !Defs.IsSurvival)
			{
				StoreKitEventListener.State.PurchaseKey = "Level Failed";
				StoreKitEventListener.State.Parameters["Level"] = string.Concat(CurrentCampaignGame.levelSceneName, " Level Failed");
			}
			else if (!this._gameOver && Defs.IsSurvival)
			{
				StoreKitEventListener.State.PurchaseKey = "Player quit";
				StoreKitEventListener.State.Parameters["Waves"] = string.Concat(StoreKitEventListener.State.Parameters["Waves"].Substring(0, StoreKitEventListener.State.Parameters["Waves"].IndexOf(" In game")), " Player quit");
			}
			else if (this._gameOver && Defs.IsSurvival)
			{
				StoreKitEventListener.State.PurchaseKey = "Game over";
				StoreKitEventListener.State.Parameters["Waves"] = string.Concat(StoreKitEventListener.State.Parameters["Waves"].Substring(0, StoreKitEventListener.State.Parameters["Waves"].IndexOf(" In game")), " Game over");
			}
			this._shouldShowFacebookButton = FacebookController.FacebookSupported;
			this._shouldShowTwitterButton = TwitterController.TwitterSupported;
			this._experienceController = LevelCompleteScript.InitializeExperienceController();
			LevelCompleteScript.BindButtonHandler(this.menuButton, new EventHandler(this.HandleMenuButton));
			LevelCompleteScript.BindButtonHandler(this.retryButton, new EventHandler(this.HandleRetryButton));
			LevelCompleteScript.BindButtonHandler(this.nextButton, new EventHandler(this.HandleNextButton));
			LevelCompleteScript.BindButtonHandler(this.shopButton, new EventHandler(this.HandleShopButton));
			LevelCompleteScript.BindButtonHandler(this.quitButton, new EventHandler(this.HandleQuitButton));
			LevelCompleteScript.BindButtonHandler(this.facebookButton, new EventHandler(this.HandleFacebookButton));
			LevelCompleteScript.BindButtonHandler(this.twitterButton, new EventHandler(this.HandleTwitterButton));
			if (!Defs.IsSurvival)
			{
				int num = -1;
				LevelBox levelBox = null;
				foreach (LevelBox campaignBox in LevelBox.campaignBoxes)
				{
					if (!campaignBox.name.Equals(CurrentCampaignGame.boXName))
					{
						continue;
					}
					levelBox = campaignBox;
					int num1 = 0;
					while (num1 != campaignBox.levels.Count)
					{
						if (!campaignBox.levels[num1].sceneName.Equals(CurrentCampaignGame.levelSceneName))
						{
							num1++;
						}
						else
						{
							num = num1;
							break;
						}
					}
					break;
				}
				if (levelBox == null)
				{
					UnityEngine.Debug.LogError("Current box not found in the list of boxes!");
					this._isLastLevel = true;
					this._nextSceneName = Application.loadedLevelName;
				}
				else
				{
					this._isLastLevel = num >= levelBox.levels.Count - 1;
					this._nextSceneName = levelBox.levels[(!this._isLastLevel ? num + 1 : num)].sceneName;
				}
				this._oldStarCount = 0;
				this._starCount = LevelCompleteScript.InitializeStarCount();
				if (!this._gameOver)
				{
					if (WeaponManager.sharedManager != null)
					{
						WeaponManager.sharedManager.DecreaseTryGunsMatchesCount();
					}
					Dictionary<string, int> item = CampaignProgress.boxesLevelsAndStars[CurrentCampaignGame.boXName];
					if (item.ContainsKey(CurrentCampaignGame.levelSceneName))
					{
						this._oldStarCount = item[CurrentCampaignGame.levelSceneName];
						item[CurrentCampaignGame.levelSceneName] = Math.Max(this._oldStarCount, this._starCount);
						CampaignProgress.SaveCampaignProgress();
					}
					else
					{
						this.completedFirstTime = true;
						if (this._isLastLevel)
						{
							this._boxCompletionExperienceAward = new int?(levelBox.CompletionExperienceAward);
						}
						item.Add(CurrentCampaignGame.levelSceneName, this._starCount);
						CampaignProgress.SaveCampaignProgress();
						FlurryPluginWrapper.LogEventWithParameterAndValue("LevelReached", "Level_Name", CurrentCampaignGame.levelSceneName);
						try
						{
							string str = null;
							if (this._isLastLevel)
							{
								if (!LevelCompleteScript.IsBox1Completed())
								{
									str = (!LevelCompleteScript.IsBox2Completed() ? "Box 3" : "Box 2");
								}
								else
								{
									str = "Box 1";
								}
							}
							AnalyticsStuff.LogCampaign(LevelCompleteScript.EnglishNameForCompletedLevel(out campaignLevel), str);
						}
						catch (Exception exception)
						{
							UnityEngine.Debug.LogError(string.Concat("Exception in LogCampaign(LevelCompleteScript): ", exception));
						}
					}
					FlurryPluginWrapper.LogEventWithParameterAndValue("Campaign Progress", "Level Completed", CurrentCampaignGame.levelSceneName);
					CampaignProgress.OpenNewBoxIfPossible();
					var dictionary = WeaponManager.sharedManager.allAvailablePlayerWeapons.OfType<Weapon>().ToDictionary((Weapon w) => w.weaponPrefab.nameNoClone(), (Weapon w) => new { AmmoInClip = w.currentAmmoInClip, AmmoInBackpack = w.currentAmmoInBackpack });
					Action action = () => {
						IEnumerable<Weapon> weapons = WeaponManager.sharedManager.allAvailablePlayerWeapons.OfType<Weapon>();
						foreach (var keyValuePair in dictionary)
						{
							Weapon ammoInClip = weapons.FirstOrDefault<Weapon>((Weapon w) => w.weaponPrefab.nameNoClone() == keyValuePair.Key);
							if (ammoInClip == null)
							{
								continue;
							}
							ammoInClip.currentAmmoInClip = keyValuePair.Value.AmmoInClip;
							ammoInClip.currentAmmoInBackpack = keyValuePair.Value.AmmoInBackpack;
						}
					};
					if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
					{
						if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
						{
							ProgressSynchronizer.Instance.AuthenticateAndSynchronize(() => {
								WeaponManager.sharedManager.Reset(WeaponManager.sharedManager.CurrentFilterMap);
								action();
							}, true);
							CampaignProgressSynchronizer.Instance.Sync();
						}
						else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
						{
							ProgressSynchronizer.Instance.SynchronizeAmazonProgress();
							WeaponManager.sharedManager.Reset(0);
							action();
						}
					}
					if (Application.platform == RuntimePlatform.IPhonePlayer)
					{
						ProgressSynchronizer.Instance.SynchronizeIosProgress();
						WeaponManager.sharedManager.Reset(0);
						CampaignProgressSynchronizer.Instance.Sync();
						action();
					}
					try
					{
						if (!this.AllStarsForBoxRewardWindowIsShown(CurrentCampaignGame.boXName))
						{
							int num2 = item.Values.ToList<int>().Sum();
							this._shouldShowAllStarsCollectedRewardWindow = num2 == LevelBox.campaignBoxes.Find((LevelBox lb) => lb.name == CurrentCampaignGame.boXName).levels.Count * 3;
						}
						if (!this.AllSecretsForBoxRewardWindowIsShown(CurrentCampaignGame.boXName))
						{
							List<string> list = (
								from level in LevelBox.campaignBoxes.Find((LevelBox lb) => lb.name == CurrentCampaignGame.boXName).levels
								select level.sceneName).ToList<string>();
							HashSet<string> strs = new HashSet<string>(CoinBonus.GetLevelsWhereGotBonus(VirtualCurrencyBonusType.Coin));
							HashSet<string> strs1 = new HashSet<string>(CoinBonus.GetLevelsWhereGotBonus(VirtualCurrencyBonusType.Gem));
							this._shouldShowAllSecretsCollectedRewardWindow = list.All<string>((string l) => (!strs.Contains(l) ? false : strs1.Contains(l)));
						}
					}
					catch (Exception exception1)
					{
						UnityEngine.Debug.LogException(exception1);
					}
					this._hasAwardForMission = (this._starCount <= this._oldStarCount ? false : LevelCompleteScript.InitializeCoinIndexBound() > this._oldStarCount);
				}
				this._awardConferred = this.InitializeAwardConferred();
			}
			this.survivalResults.SetActive(false);
			this.quitButton.SetActive(false);
			if (this._gameOver)
			{
				this.award1coinSprite.SetActive(false);
				this.nextButton.SetActive(false);
				this.checkboxSpritePrototype.SetActive(false);
				if (!Defs.IsSurvival && this.gameOverSprite != null)
				{
					this.gameOverSprite.SetActive(true);
				}
				if (Defs.IsSurvival)
				{
					int num3 = PlayerPrefs.GetInt(Defs.WavesSurvivedS, 0);
					FlurryPluginWrapper.LogEventWithParameterAndValue("Survival Finished", "Game Over", num3.ToString());
				}
				GameObject[] gameObjectArray = this.statisticLabels;
				for (int i = 0; i < (int)gameObjectArray.Length; i++)
				{
					gameObjectArray[i].SetActive(Defs.IsSurvival);
				}
				if (!Defs.IsSurvival)
				{
					float single = this.retryButton.transform.position.x;
					Vector3 vector3 = this.menuButton.transform.position;
					float single1 = (single - vector3.x) / 2f;
					Vector3 vector31 = new Vector3(single1, 0f, 0f);
					this.menuButton.transform.position = this.retryButton.transform.position - vector31;
					Transform transforms = this.retryButton.transform;
					transforms.position = transforms.position + vector31;
					FlurryPluginWrapper.LogEventWithParameterAndValue("Campaign Progress", "Game Over", CurrentCampaignGame.levelSceneName);
				}
				this.menuButton.SetActive(!Defs.IsSurvival);
				if (!Defs.IsSurvival)
				{
					base.StartCoroutine(this.TryToShowExpiredBanner());
				}
			}
			else
			{
				flag = (PremiumAccountController.Instance == null ? false : PremiumAccountController.Instance.isAccountActive);
				this.premium.SetActive(flag);
				this.award1coinSprite.SetActive(true);
				GameObject[] gameObjectArray1 = this.statisticLabels;
				for (int j = 0; j < (int)gameObjectArray1.Length; j++)
				{
					gameObjectArray1[j].SetActive(Defs.IsSurvival);
				}
				if (Defs.IsSurvival)
				{
					int num4 = PlayerPrefs.GetInt(Defs.WavesSurvivedS, 0);
					FlurryPluginWrapper.LogEventWithParameterAndValue("Survival Finished", "Quit", num4.ToString());
				}
				if (this._starCount > this._oldStarCount)
				{
					CoinsMessage.FireCoinsAddedEvent(false, 2);
				}
			}
			if (Defs.IsSurvival)
			{
				if (WeaponManager.sharedManager != null && PlayerPrefs.GetInt(Defs.WavesSurvivedS, 0) > 0)
				{
					WeaponManager.sharedManager.DecreaseTryGunsMatchesCount();
				}
				WeaponManager.sharedManager.Reset(0);
			}
			LevelCompleteScript._instance = this;
			if (!Defs.IsSurvival && (this._awardConferred || this._hasAwardForMission) || this._starCount == 3 && this._oldStarCount < 3 && !this._gameOver && ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel < ExperienceController.maxLevel)
			{
				this.mainInterface.SetActive(false);
				this.rewardWindow.gameObject.SetActive(true);
				this.GiveAwardForCampaign();
			}
			else if (!Defs.IsSurvival)
			{
				this.mainInterface.SetActive(true);
				this.rewardWindow.gameObject.SetActive(false);
				if (!this._gameOver && this.brightStarPrototypeSprite != null && this.darkStarPrototypeSprite != null)
				{
					base.StartCoroutine(this.DisplayLevelResult());
				}
			}
			else if (Defs.IsSurvival)
			{
				int num5 = LevelCompleteScript.CalculateExperienceAward(GlobalGameController.Score);
				if (num5 > 0)
				{
					this._experienceController.addExperience(num5 * (PremiumAccountController.Instance == null ? 1 : PremiumAccountController.Instance.RewardCoeff));
				}
				if (!GlobalGameController.HasSurvivalRecord)
				{
					this.DisplaySurvivalResult();
				}
				else
				{
					GlobalGameController.HasSurvivalRecord = false;
					if ((FacebookController.FacebookSupported || TwitterController.TwitterSupported) && !Device.isPixelGunLow)
					{
						this.mainInterface.SetActive(false);
						this.rewardWindowSurvival.gameObject.SetActive(true);
						foreach (UILabel scoreLabel in this.survivalRewardWindowSettings.scoreLabels)
						{
							scoreLabel.text = string.Format(LocalizationStore.Get("Key_1553"), GlobalGameController.Score);
						}
					}
					else
					{
						this.DisplaySurvivalResult();
					}
				}
			}
		}

		[DebuggerHidden]
		private IEnumerator TryToShowExpiredBanner()
		{
			LevelCompleteScript.u003cTryToShowExpiredBanneru003ec__Iterator16B variable = null;
			return variable;
		}

		private void Update()
		{
			bool flag;
			if (this._experienceController != null && BankController.Instance != null && !BankController.Instance.InterfaceEnabled && !ShopNGUIController.GuiActive)
			{
				this._experienceController.isShowRanks = (this.RentWindowPoint.childCount != 0 || this.loadingPanel.activeSelf ? false : (LevelCompleteScript.sharedScript == null ? 0 : (int)LevelCompleteScript.IsShowRewardWindow()) == 0);
			}
			flag = (!FacebookController.FacebookSupported || !this._shouldShowFacebookButton || !FacebookController.FacebookPost_Old_Supported ? false : FB.IsLoggedIn);
			this.facebookButton.SetActive(flag);
			this.twitterButton.SetActive((!TwitterController.TwitterSupported || !this._shouldShowTwitterButton || !TwitterController.TwitterSupported_OldPosts ? false : TwitterController.IsLoggedIn));
		}
	}
}