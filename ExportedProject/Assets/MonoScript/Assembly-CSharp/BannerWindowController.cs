using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Linq;
using UnityEngine;

public class BannerWindowController : MonoBehaviour
{
	private const float StartBannerShowDelay = 3f;

	public BannerWindow[] bannerWindows;

	public static bool firstScreen;

	[NonSerialized]
	public AdvertisementController advertiseController;

	private readonly int BannerWindowCount = (int)Enum.GetNames(typeof(BannerWindowType)).Length;

	private Queue<BannerWindow> _bannerQueue;

	private BannerWindow _currentBanner;

	private bool[] _bannerShowed;

	private bool[] _needShowBanner;

	private bool _someBannerShown;

	private float _lastCheckTime;

	private float _whenStart;

	private bool _isBlockShowForNewPlayer;

	internal bool IsAnyBannerShown
	{
		get
		{
			return this._currentBanner != null;
		}
	}

	public static BannerWindowController SharedController
	{
		get;
		private set;
	}

	static BannerWindowController()
	{
		BannerWindowController.firstScreen = true;
	}

	private BannerWindowController()
	{
		this._bannerShowed = new bool[this.BannerWindowCount];
		this._needShowBanner = new bool[this.BannerWindowCount];
	}

	public void AddBannersTimeout(float seconds)
	{
		this._lastCheckTime = Time.realtimeSinceStartup + seconds;
	}

	public void AdmobBannerApplyClick()
	{
		if (AdmobPerelivWindow.Context != null)
		{
			Dictionary<string, string> levelAndTierParameters = FlurryPluginWrapper.LevelAndTierParameters;
			levelAndTierParameters.Add("Context", AdmobPerelivWindow.Context);
			FlurryPluginWrapper.LogEventAndDublicateToConsole("Replace Admob Pereliv Opened", levelAndTierParameters, true);
		}
		Application.OpenURL(AdmobPerelivWindow.admobUrl);
	}

	public void AdmobBannerExitClick()
	{
		ButtonClickSound.Instance.PlayClick();
		this.HideBannerWindow();
		this._bannerShowed[2] = false;
		this._needShowBanner[2] = false;
		this.ResetStateBannerShowed(BannerWindowType.Admob);
	}

	public void AdvertBannerApplyClick()
	{
		ButtonClickSound.Instance.PlayClick();
		this.advertiseController.Close();
		this.UpdateAdvertShownCount();
		Application.OpenURL(PromoActionsManager.Advert.adUrl);
		this.HideBannerWindow();
	}

	public void AdvertBannerExitClick()
	{
		ButtonClickSound.Instance.PlayClick();
		this.advertiseController.Close();
		this.UpdateAdvertShownCount();
		this.HideBannerWindow();
	}

	private void Awake()
	{
		BannerWindowController.SharedController = this;
	}

	private void CheckBannersShowConditions()
	{
		if (PromoActionsManager.sharedManager == null)
		{
			return;
		}
		if (AdmobPerelivWindow.admobTexture != null && !string.IsNullOrEmpty(AdmobPerelivWindow.admobUrl) && !this._bannerShowed[2])
		{
			this._bannerShowed[2] = true;
			this._needShowBanner[2] = true;
		}
		this.CheckDownloadAdvertisement();
		if (this.IsAdvertisementDownloading())
		{
			return;
		}
		if (PromoActionsManager.Advert.enabled && this.advertiseController.CurrentState == AdvertisementController.State.Complete && !this._bannerShowed[5])
		{
			this._bannerShowed[5] = true;
			this._needShowBanner[5] = true;
		}
		if (!BannerWindowController.firstScreen && PromoActionsManager.sharedManager.IsDayOfValorEventActive && TrainingController.TrainingCompleted && PlayerPrefs.GetInt("DaysOfValorShownCount", 1) > 0 && !this._bannerShowed[6])
		{
			this._bannerShowed[6] = true;
			this._needShowBanner[6] = true;
		}
		if (ConnectSceneNGUIController.sharedController != null && ConnectSceneNGUIController.isReturnFromGame && !ReviewController.IsNeedActive && !ReviewHUDWindow.isShow && PromoActionsManager.sharedManager.IsEventX3Active && TrainingController.TrainingCompleted && PlayerPrefs.GetInt(Defs.EventX3WindowShownCount, 1) > 0 && !this._bannerShowed[7])
		{
			this._bannerShowed[7] = true;
			this._needShowBanner[7] = true;
		}
		if (GlobalGameController.NewVersionAvailable && PlayerPrefs.GetInt(Defs.UpdateAvailableShownTimesSN, 3) > 0 && !this._bannerShowed[9])
		{
			this._bannerShowed[9] = true;
			this._needShowBanner[9] = true;
		}
		if (!BannerWindowController.firstScreen && StarterPackController.Get.IsNeedShowEventWindow() && !this._bannerShowed[10])
		{
			this._bannerShowed[10] = true;
			this._needShowBanner[10] = true;
		}
	}

	private void CheckDownloadAdvertisement()
	{
		if (BuildSettings.BuildTargetPlatform != RuntimePlatform.IPhonePlayer || ExperienceController.sharedController == null)
		{
			return;
		}
		int num = ExperienceController.sharedController.currentLevel;
		PromoActionsManager.AdvertInfo advert = PromoActionsManager.Advert;
		bool flag = (advert.minLevel == -1 ? true : num >= advert.minLevel);
		bool flag1 = (advert.maxLevel == -1 ? true : num <= advert.maxLevel);
		bool flag2 = (!advert.showAlways ? PlayerPrefs.GetInt(Defs.AdvertWindowShownCount, 3) > 0 : true);
		if ((!advert.enabled ? false : this.advertiseController.CurrentState == AdvertisementController.State.Idle) && flag && flag1 && flag2)
		{
			this.advertiseController.Run();
		}
	}

	public void ClearBannerStates()
	{
		this._bannerShowed = new bool[this.BannerWindowCount];
		this._needShowBanner = new bool[this.BannerWindowCount];
	}

	private static void ClearNewVersionShownCount()
	{
		PlayerPrefs.SetInt(Defs.UpdateAvailableShownTimesSN, 0);
		PlayerPrefs.SetString(Defs.LastTimeUpdateAvailableShownSN, DateTimeOffset.Now.ToString("s"));
		PlayerPrefs.Save();
	}

	public void EventX3ApplyClick()
	{
		this.EventX3ExitClick();
		if (MainMenuController.sharedController != null)
		{
			MainMenuController.sharedController.ShowBankWindow();
			return;
		}
		if (ConnectSceneNGUIController.sharedController == null)
		{
			return;
		}
		ConnectSceneNGUIController.sharedController.ShowBankWindow();
	}

	public void EventX3ExitClick()
	{
		ButtonClickSound.TryPlayClick();
		this.UpdateEventX3ShownCount();
		this.HideBannerWindow();
	}

	public void EverydayRewardApplyClick()
	{
		ButtonClickSound.TryPlayClick();
		this.TakeEverydayRewardForPlayer();
		this.HideBannerWindow();
	}

	public void ForceShowBanner(BannerWindowType windowType)
	{
		if (this._currentBanner == null)
		{
			this.ShowBannerWindow(windowType);
			return;
		}
		if (this._currentBanner.type != windowType)
		{
			this.HideBannerWindow();
			this.ShowBannerWindow(windowType);
		}
	}

	public void HideBannerWindow()
	{
		BuySmileBannerController.openedFromPromoActions = false;
		this.HideBannerWindowNoShowNext();
		if (this._bannerQueue.Count > 0)
		{
			BannerWindow bannerWindow = this._bannerQueue.Dequeue();
			this._currentBanner = bannerWindow;
			bannerWindow.Show();
		}
	}

	public void HideBannerWindowNoShowNext()
	{
		if (this._currentBanner != null)
		{
			this._currentBanner.Hide();
			this._currentBanner = null;
		}
	}

	private bool IsAdvertisementDownloading()
	{
		if (this.advertiseController == null)
		{
			return false;
		}
		AdvertisementController.State currentState = this.advertiseController.CurrentState;
		return (currentState == AdvertisementController.State.Idle || currentState == AdvertisementController.State.Complete ? false : currentState != AdvertisementController.State.Error);
	}

	private bool IsBannersCanShowAfterNewInstall()
	{
		DateTime dateTime;
		if (string.IsNullOrEmpty(Defs.StartTimeShowBannersString))
		{
			return true;
		}
		if (!DateTime.TryParse(Defs.StartTimeShowBannersString, out dateTime))
		{
			return true;
		}
		return ((DateTime.UtcNow - dateTime).TotalMinutes >= 1440 ? true : Defs.countReturnInConnectScene >= 4);
	}

	public bool IsBannerShow(BannerWindowType bannerType)
	{
		if (this._currentBanner == null)
		{
			return false;
		}
		return this._currentBanner.type == bannerType;
	}

	public void NewVersionBannerApplyClick()
	{
		ButtonClickSound.Instance.PlayClick();
		BannerWindowController.ClearNewVersionShownCount();
		Application.OpenURL(MainMenu.RateUsURL);
		this.HideBannerWindow();
	}

	public void NewVersionBannerExitClick()
	{
		ButtonClickSound.Instance.PlayClick();
		BannerWindowController.UpdateNewVersionShownCount();
		this.HideBannerWindow();
	}

	[DebuggerHidden]
	private IEnumerator OnApplicationPause(bool pause)
	{
		BannerWindowController.u003cOnApplicationPauseu003ec__Iterator10A variable = null;
		return variable;
	}

	private void OnDestroy()
	{
		BannerWindowController.SharedController = null;
		this._bannerQueue = null;
		this.advertiseController = null;
		BannerWindowController.firstScreen = false;
	}

	public void RegisterWindow(BannerWindow window, BannerWindowType windowType)
	{
		int num = (int)windowType;
		if ((int)this.bannerWindows.Length < num + 1)
		{
			List<BannerWindow> list = this.bannerWindows.ToList<BannerWindow>();
			while (list.Count<BannerWindow>() < num + 1)
			{
				list.Add(null);
			}
			this.bannerWindows = list.ToArray();
		}
		this.bannerWindows[num] = window;
		int layer = LayerMask.NameToLayer("Banners");
		window.gameObject.Descendants().ForEach<GameObject>((GameObject go) => go.layer = layer);
	}

	public void ResetStateBannerShowed(BannerWindowType windowType)
	{
		int num = (int)windowType;
		if ((int)this.bannerWindows.Length < 0 || num > (int)this.bannerWindows.Length - 1)
		{
			return;
		}
		this._bannerShowed[num] = false;
		this._someBannerShown = false;
	}

	private void ShowAdmobBanner()
	{
		if (AdmobPerelivWindow.admobTexture == null || string.IsNullOrEmpty(AdmobPerelivWindow.admobUrl))
		{
			return;
		}
		this.ShowBannerWindow(BannerWindowType.Admob);
	}

	private void ShowAdvertisementBanner(AdvertisementController advertisementController)
	{
		if (advertisementController.AdvertisementTexture == null)
		{
			return;
		}
		this.advertiseController = advertisementController;
		BannerWindow bannerWindow = this.ShowBannerWindow(BannerWindowType.Advertisement);
		if (bannerWindow == null)
		{
			return;
		}
		bannerWindow.SetBackgroundImage(advertisementController.AdvertisementTexture);
		bannerWindow.SetEnableExitButton(PromoActionsManager.Advert.btnClose);
	}

	private BannerWindow ShowBannerWindow(BannerWindowType windowType)
	{
		int num = (int)windowType;
		if ((int)this.bannerWindows.Length < 0 || num > (int)this.bannerWindows.Length - 1)
		{
			return null;
		}
		if (this.bannerWindows[num] == null)
		{
			return null;
		}
		if (this.bannerWindows[num].gameObject.activeSelf)
		{
			return null;
		}
		BannerWindow bannerWindow = this.bannerWindows[num];
		if (this._currentBanner != null)
		{
			this._bannerQueue.Enqueue(bannerWindow);
		}
		else
		{
			this._currentBanner = bannerWindow;
			this._currentBanner.type = windowType;
			bannerWindow.Show();
		}
		return bannerWindow;
	}

	public void SorryBannerExitButtonClick()
	{
		MainMenuController.sharedController.stubLoading.SetActive(false);
		this.HideBannerWindow();
	}

	private void Start()
	{
		this._currentBanner = null;
		this._bannerQueue = new Queue<BannerWindow>();
		this._someBannerShown = false;
		this._whenStart = Time.realtimeSinceStartup + 3f;
		if (StarterPackController.Get != null)
		{
			StarterPackController.Get.CheckShowStarterPack();
			StarterPackController.Get.UpdateCountShownWindowByTimeCondition();
		}
		PromoActionsManager.UpdateDaysOfValorShownCondition();
		this._isBlockShowForNewPlayer = !this.IsBannersCanShowAfterNewInstall();
	}

	internal void SubmitCurrentBanner()
	{
		if (this._currentBanner == null)
		{
			return;
		}
		this._currentBanner.Submit();
	}

	private void TakeEverydayRewardForPlayer()
	{
		NotificationController.isGetEveryDayMoney = false;
		if (MainMenu.sharedMenu != null)
		{
			MainMenu.sharedMenu.isShowAvard = false;
		}
		BankController.GiveInitialNumOfCoins();
		int num = Storager.getInt("Coins", false);
		Storager.setInt("Coins", num + 3, false);
		AnalyticsFacade.CurrencyAccrual(3, "Coins", AnalyticsConstants.AccrualType.Earned);
		FlurryEvents.LogCoinsGained("Main Menu", 3);
		CoinsMessage.FireCoinsAddedEvent(false, 2);
		AudioClip audioClip = Resources.Load("coin_get") as AudioClip;
		if (audioClip != null && Defs.isSoundFX)
		{
			NGUITools.PlaySound(audioClip);
		}
	}

	private void Update()
	{
		if (this._isBlockShowForNewPlayer)
		{
			return;
		}
		if (Time.realtimeSinceStartup < this._whenStart)
		{
			return;
		}
		if (Time.realtimeSinceStartup - this._lastCheckTime >= 1f)
		{
			this.CheckBannersShowConditions();
			for (int i = 0; i < (int)this._needShowBanner.Length; i++)
			{
				if ((!this._someBannerShown || i == 2) && this._needShowBanner[i] && !ActivityIndicator.IsActiveIndicator)
				{
					if (MainMenuController.IsShowRentExpiredPoint() || MainMenuController.sharedController != null && (MainMenuController.sharedController.FreePanelIsActive || MainMenuController.sharedController.singleModePanel.activeSelf))
					{
						break;
					}
					else if (BankController.Instance != null && BankController.Instance.InterfaceEnabled)
					{
						break;
					}
					else if (!FreeAwardController.FreeAwardChestIsInIdleState)
					{
						break;
					}
					else if (MainMenuController.SavedShwonLobbyLevelIsLessThanActual())
					{
						break;
					}
					else if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
					{
						break;
					}
					else if (i != 6 || !SceneLoader.ActiveSceneName.Equals(Defs.MainMenuScene) || Storager.getInt(Defs.ShownLobbyLevelSN, false) >= 3)
					{
						if (i != 1 || SceneLoader.ActiveSceneName.Equals(Defs.MainMenuScene))
						{
							if (i != 0 || SceneLoader.ActiveSceneName.Equals(Defs.MainMenuScene))
							{
								this._needShowBanner[i] = false;
								if (i == 2)
								{
									this.ShowAdmobBanner();
								}
								else if (i != 5)
								{
									this.ShowBannerWindow((BannerWindowType)i);
								}
								else
								{
									this.ShowAdvertisementBanner(this.advertiseController);
								}
								this._someBannerShown = true;
								break;
							}
						}
					}
				}
			}
			this._lastCheckTime = Time.realtimeSinceStartup;
		}
	}

	private void UpdateAdvertShownCount()
	{
		if (PromoActionsManager.Advert.showAlways)
		{
			return;
		}
		PlayerPrefs.SetInt(Defs.AdvertWindowShownCount, PlayerPrefs.GetInt(Defs.AdvertWindowShownCount, 3) - 1);
		PlayerPrefs.SetString(Defs.AdvertWindowShownLastTime, PromoActionsManager.CurrentUnixTime.ToString());
		PlayerPrefs.Save();
	}

	private void UpdateEventX3ShownCount()
	{
		PlayerPrefs.SetInt(Defs.EventX3WindowShownCount, PlayerPrefs.GetInt(Defs.EventX3WindowShownCount, 1) - 1);
		PlayerPrefs.Save();
	}

	private static void UpdateNewVersionShownCount()
	{
		PlayerPrefs.SetInt(Defs.UpdateAvailableShownTimesSN, PlayerPrefs.GetInt(Defs.UpdateAvailableShownTimesSN, 3) - 1);
		PlayerPrefs.SetString(Defs.LastTimeUpdateAvailableShownSN, DateTimeOffset.Now.ToString("s"));
		PlayerPrefs.Save();
	}
}