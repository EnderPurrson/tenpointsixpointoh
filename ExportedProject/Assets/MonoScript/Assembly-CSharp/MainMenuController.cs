using Facebook.Unity;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using I2.Loc;
using Rilisoft;
using Rilisoft.NullExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

internal sealed class MainMenuController : ControlsSettingsBase
{
	internal const string TrafficForwardedKey = "TrafficForwarded";

	private readonly static TaskCompletionSource<bool> _syncPromise;

	public GameObject questButton;

	public GameObject facebookLoginContainer;

	public GameObject twitterLoginContainer;

	public GameObject facebookConnectedSettings;

	public GameObject facebookDisconnectedSettings;

	public GameObject facebookConnectSettings;

	public GameObject twitterConnectedSettings;

	public GameObject twitterDisconnectedSettings;

	public GameObject twitterConnectSettings;

	[Header("subwindows")]
	[SerializeField]
	private GameObject _subwindowsHandler;

	[SerializeField]
	private PrefabHandler _socialBannerPrefab;

	private LazyObject<SocialGunBannerView> _socialBanner;

	[SerializeField]
	private PrefabHandler _freePanelPrefab;

	private LazyObject<MainMenuFreePanel> _freePanel;

	[Header("MainMenuController properties")]
	public Transform topLeftAnchor;

	public GameObject buySmileButton;

	public UIButton premiumButton;

	public GameObject premium;

	public GameObject daysOfValor;

	public GameObject adventureButton;

	public GameObject achievementsButton;

	public GameObject clansButton;

	public GameObject leadersButton;

	public UILabel battleNowLabel;

	public UILabel trainingNowLabel;

	public GameObject friendsGUI;

	public UILabel premiumTime;

	public GameObject premiumUpPlashka;

	public GameObject premiumbottomPlashka;

	public List<GameObject> premiumLevels = new List<GameObject>();

	public GameObject starParticleStarterPackGaemObject;

	public Transform RentExpiredPoint;

	public Transform pers;

	public GameObject completeTraining;

	public GameObject stubLoading;

	public UITexture stubTexture;

	public MainMenuHeroCamera rotateCamera;

	public static MainMenuController sharedController;

	public GameObject campaignButton;

	public GameObject survivalButton;

	public GameObject multiplayerButton;

	public GameObject skinsMakerButton;

	public GameObject friendsButton;

	public GameObject profileButton;

	public GameObject freeButton;

	public GameObject gameCenterButton;

	public GameObject shopButton;

	public GameObject settingsButton;

	public GameObject coinsShopButton;

	public GameObject diclineButton;

	public GameObject agreeButton;

	public GameObject UserAgreementPanel;

	public UIButton signOutButton;

	public GameObject mainPanel;

	public GameObject newsIndicator;

	[Header("FeedBack")]
	[SerializeField]
	private ButtonHandler _openFeedBackBtn;

	[SerializeField]
	private PrefabHandler _feedbackPrefab;

	[Header("News")]
	[SerializeField]
	private ButtonHandler _openNewsBtn;

	[SerializeField]
	private PrefabHandler _newsPrefab;

	[Header("Leaderboards")]
	public UIPanel leaderboardsPanel;

	[Header("Misc")]
	public GameObject PromoActionsPanel;

	public UIToggle notShowAgain;

	public UILabel coinsLabel;

	public GameObject award800to810;

	public UIButton awardOk;

	public GameObject bannerContainer;

	public GameObject nicknameLabel;

	public UIButton developerConsole;

	public UICamera uiCamera;

	public NickLabelController persNickLabel;

	public GameObject eventX3Window;

	public UILabel[] eventX3RemainTime;

	public UIButton trafficForwardingButton;

	public static bool trafficForwardActive;

	private float _eventX3RemainTimeLastUpdateTime;

	private readonly Lazy<UISprite> _newClanIncomingInvitesSprite;

	private AdvertisementController _advertisementController;

	protected ShopNGUIController _shopInstance;

	private bool isMultyPress;

	private bool isFriendsPress;

	private List<GameObject> saveOpenPanel = new List<GameObject>();

	public static bool canRotationLobbyPlayer;

	private LazyObject<NewsLobbyController> _newsPanel;

	private LazyObject<FeedbackMenuController> _feedbackPanel;

	private readonly List<EventHandler> _backSubscribers = new List<EventHandler>();

	private bool loadReplaceAdmobPerelivRunning;

	private bool loadAdmobRunning;

	private float _lastTimeInterstitialShown;

	private static bool _drawLoadingProgress;

	[NonSerialized]
	public GameObject freeAwardChestObj;

	public static bool SingleModeOnStart;

	public static bool friendsOnStart;

	private static bool _socialNetworkingInitilized;

	private Rect campaignRect;

	private Rect survivalRect;

	private Rect shopRect;

	public TweenColor colorBlinkForX3;

	private string _localizeSaleLabel;

	private float _timePremiumTimeUpdated;

	private readonly Lazy<bool> _timeTamperingDetected = new Lazy<bool>(new Func<bool>(() => {
		bool flag = FreeAwardController.Instance.TimeTamperingDetected();
		!flag;
		return flag;
	}));

	private IDisposable _backSubscription;

	private float lastTime;

	private float idleTimerLastTime;

	private float _bankEnteredTime;

	private MenuLeaderboardsController _menuLeaderboardsController;

	public UIPanel starterPackPanel;

	public UILabel starterPackTimer;

	public UILabel socialGunEventTimer;

	public UITexture buttonBackground;

	private bool _starterPackEnabled;

	private string _trafficForwardingUrl = "http://pixelgun3d.com/";

	private readonly Lazy<UIButton[]> _leaderboardsButton;

	private readonly Lazy<LeaderboardScript> _leaderboardScript;

	public UIWidget dayOfValorContainer;

	public UILabel dayOfValorTimer;

	private bool _dayOfValorEnabled;

	public GameObject singleModePanel;

	public UILabel singleModeBestScores;

	public UILabel singleModeStarsProgress;

	private Transform _parentBankPanel;

	private bool inAdventureScreen;

	[Header("Social panel settings")]
	public UIButton socialButton;

	public bool FreePanelIsActive
	{
		get
		{
			return this._freePanel.ObjectIsActive;
		}
	}

	public bool InAdventureScreen
	{
		get
		{
			return this.inAdventureScreen;
		}
		private set
		{
			this.inAdventureScreen = value;
		}
	}

	public UIPanel LeaderboardsPanel
	{
		get
		{
			return this._leaderboardScript.Value.Panel;
		}
	}

	public static string RateUsURL
	{
		get
		{
			string applicationUrl = Defs2.ApplicationUrl;
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				applicationUrl = "https://play.google.com/store/apps/details?id=com.pixel.gun3d&hl=en";
			}
			return applicationUrl;
		}
	}

	public static bool ShopOpened
	{
		get
		{
			return (MainMenuController.sharedController == null ? false : MainMenuController.sharedController._shopInstance != null);
		}
	}

	internal Task SyncFuture
	{
		get
		{
			return MainMenuController._syncPromise.Task;
		}
	}

	public static bool trainingCompleted
	{
		get;
		set;
	}

	static MainMenuController()
	{
		MainMenuController.canRotationLobbyPlayer = true;
		MainMenuController._drawLoadingProgress = true;
		MainMenuController._syncPromise = new TaskCompletionSource<bool>();
	}

	public MainMenuController()
	{
		this._newClanIncomingInvitesSprite = new Lazy<UISprite>(() => this.clansButton.Map<GameObject, UISprite>((GameObject c) => ((IEnumerable<UISprite>)c.GetComponentsInChildren<UISprite>(true)).FirstOrDefault<UISprite>((UISprite s) => "NewMessages".Equals(s.name))));
		this._leaderboardsButton = new Lazy<UIButton[]>(() => this.leadersButton.GetComponentsInChildren<UIButton>(true));
		this._leaderboardScript = new Lazy<LeaderboardScript>(new Func<LeaderboardScript>(UnityEngine.Object.FindObjectOfType<LeaderboardScript>));
	}

	private void Awake()
	{
		this._socialBanner = new LazyObject<SocialGunBannerView>(this._socialBannerPrefab.ResourcePath, this._subwindowsHandler);
		this._freePanel = new LazyObject<MainMenuFreePanel>(this._freePanelPrefab.ResourcePath, this._subwindowsHandler);
		this._newsPanel = new LazyObject<NewsLobbyController>(this._newsPrefab.ResourcePath, this._subwindowsHandler);
		this._feedbackPanel = new LazyObject<FeedbackMenuController>(this._feedbackPrefab.ResourcePath, this._subwindowsHandler);
	}

	private void CalcBtnRects()
	{
		Transform root = NGUITools.GetRoot(base.gameObject).transform;
		Transform component = root.GetChild(0).GetComponent<Camera>().transform;
		float single = 768f;
		float single1 = single * ((float)Screen.width / (float)Screen.height);
		Bounds vector3 = NGUIMath.CalculateRelativeWidgetBounds(component, this.shopButton.GetComponent<UIButton>().tweenTarget.transform, true, true);
		vector3.center = vector3.center + new Vector3(single1 * 0.5f, single * 0.5f, 0f);
		Vector3 vector31 = vector3.center;
		float coef = (vector31.x - 105.5f) * Defs.Coef;
		Vector3 vector32 = vector3.center;
		this.shopRect = new Rect(coef, (vector32.y - 57f) * Defs.Coef, 211f * Defs.Coef, 114f * Defs.Coef);
		Bounds bound = NGUIMath.CalculateRelativeWidgetBounds(component, this.survivalButton.GetComponent<UIButton>().tweenTarget.transform, true, true);
		bound.center = bound.center + new Vector3(single1 * 0.5f, single * 0.5f, 0f);
		Vector3 vector33 = bound.center;
		float coef1 = (vector33.x - 107f) * Defs.Coef;
		Vector3 vector34 = bound.center;
		this.survivalRect = new Rect(coef1, (vector34.y - 35f) * Defs.Coef, 214f * Defs.Coef, 70f * Defs.Coef);
		Bounds bound1 = NGUIMath.CalculateRelativeWidgetBounds(component, this.campaignButton.GetComponent<UIButton>().tweenTarget.transform, true, true);
		bound1.center = bound1.center + new Vector3(single1 * 0.5f, single * 0.5f, 0f);
		Vector3 vector35 = bound1.center;
		float coef2 = (vector35.x - 107f) * Defs.Coef;
		Vector3 vector36 = bound1.center;
		this.campaignRect = new Rect(coef2, (vector36.y - 35f) * Defs.Coef, 214f * Defs.Coef, 70f * Defs.Coef);
	}

	private void ChangeLocalizeLabel()
	{
		this._localizeSaleLabel = LocalizationStore.Get("Key_0419");
	}

	private void CheckIfPendingAward()
	{
		Dictionary<string, string> strs;
		if (Storager.hasKey("PendingFreeAward"))
		{
			int num = Storager.getInt("PendingFreeAward", false);
			if (num > 0)
			{
				FreeAwardController.Instance.GiveAwardAndIncrementCount();
				Storager.setInt("PendingInterstitial", 0, false);
				strs = new Dictionary<string, string>()
				{
					{ "Context", "FreeAwardVideo" }
				};
				Dictionary<string, string> strs1 = strs;
				strs1.Add("Device", SystemInfo.deviceModel);
				strs1.Add("Provider", num.ToString());
				if (ExperienceController.sharedController != null)
				{
					strs1.Add("Level", ExperienceController.sharedController.currentLevel.ToString());
				}
				if (ExpController.Instance != null)
				{
					strs1.Add("Tier", ExpController.Instance.OurTier.ToString());
				}
				FlurryPluginWrapper.LogEventAndDublicateToConsole("Crash on advertising", strs1, true);
			}
		}
		if (Storager.hasKey("PendingInterstitial"))
		{
			int num1 = Storager.getInt("PendingInterstitial", false);
			if (num1 > 0)
			{
				Storager.setInt("PendingInterstitial", 0, false);
				strs = new Dictionary<string, string>()
				{
					{ "Context", "Interstitial" }
				};
				Dictionary<string, string> strs2 = strs;
				strs2.Add("Device", SystemInfo.deviceModel);
				strs2.Add("Provider", num1.ToString());
				if (ExperienceController.sharedController != null)
				{
					strs2.Add("Level", ExperienceController.sharedController.currentLevel.ToString());
				}
				if (ExpController.Instance != null)
				{
					strs2.Add("Tier", ExpController.Instance.OurTier.ToString());
				}
				FlurryPluginWrapper.LogEventAndDublicateToConsole("Crash on advertising", strs2, true);
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator ContinueWithCoroutine(Task task, Action<Task> continuation)
	{
		MainMenuController.u003cContinueWithCoroutineu003ec__IteratorB6 variable = null;
		return variable;
	}

	public static void DoMemoryConsumingTaskInEmptyScene(Action action, Action onSeparateSceneCaseAction = null)
	{
		if (Device.IsLoweMemoryDevice)
		{
			CleanUpAndDoAction.action = onSeparateSceneCaseAction ?? action;
			SceneManager.LoadScene("LoadAnotherApp");
		}
		else if (action != null)
		{
			action();
		}
	}

	internal static int FindMaxLevel(IEnumerable<string> itemsToBeSaved)
	{
		int num = 0;
		IEnumerator<string> enumerator = itemsToBeSaved.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				if (current.StartsWith("currentLevel"))
				{
					string[] strArrays = current.Split(new string[] { "currentLevel" }, StringSplitOptions.RemoveEmptyEntries);
					if ((int)strArrays.Length > 0)
					{
						string str = strArrays[(int)strArrays.Length - 1];
						if (!string.IsNullOrEmpty(str))
						{
							int num1 = Convert.ToInt32(str);
							if (num1 <= num)
							{
								continue;
							}
							num = num1;
						}
					}
				}
			}
		}
		finally
		{
			if (enumerator == null)
			{
			}
			enumerator.Dispose();
		}
		return num;
	}

	private static string GetAbuseKey_f1a4329e(uint pad)
	{
		return (894321575 ^ pad).ToString("x");
	}

	private void GoClans()
	{
		MenuBackgroundMusic.keepPlaying = true;
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.sceneToLoad = "Clans";
		LoadConnectScene.noteToShow = null;
		Singleton<SceneLoader>.Instance.LoadScene(Defs.PromSceneName, LoadSceneMode.Single);
	}

	private void GoFriens()
	{
		MenuBackgroundMusic.keepPlaying = true;
		if (FriendsWindowGUI.Instance == null)
		{
			UnityEngine.Debug.LogWarning("FriendsWindowController.Instance == null");
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		FriendsController.sharedController.GetFriendsData(true);
		ButtonClickSound.Instance.PlayClick();
		GameObject gameObject = null;
		if (NickLabelStack.sharedStack != null)
		{
			NickLabelStack.sharedStack.gameObject.SetActive(false);
		}
		if (!MainMenuController.friendsOnStart)
		{
			base.StartCoroutine(this.HideMenuInterfaceCoroutine(gameObject));
		}
		FriendsWindowGUI.Instance.ShowInterface(() => {
			if (this.persNickLabel != null)
			{
				NickLabelStack.sharedStack.gameObject.SetActive(true);
				if (this.persNickLabel != null)
				{
					this.persNickLabel.UpdateNickInLobby();
					this.persNickLabel.UpdateInfo();
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogWarning("nickLabelController == null");
				}
			}
			this.rotateCamera.gameObject.SetActive(true);
			if (this.mainPanel != null)
			{
				this.mainPanel.transform.root.gameObject.SetActive(true);
			}
		});
		FriendsController.sharedController.DownloadDataAboutPossibleFriends();
	}

	private void GoMulty()
	{
		Defs.isFlag = false;
		Defs.isCOOP = false;
		Defs.isMulti = true;
		Defs.isHunger = false;
		Defs.isCompany = false;
		Defs.IsSurvival = false;
		Defs.isFlag = false;
		Defs.isCapturePoints = false;
		FlurryPluginWrapper.LogDeathmatchModePress();
		MenuBackgroundMusic.keepPlaying = true;
		LoadConnectScene.textureToShow = Resources.Load<Texture>(ConnectSceneNGUIController.MainLoadingTexture());
		LoadConnectScene.sceneToLoad = "ConnectScene";
		FlurryPluginWrapper.LogEvent("Launch_Multiplayer");
		LoadConnectScene.noteToShow = null;
		Application.LoadLevel(Defs.PromSceneName);
	}

	public void GoSandBox()
	{
		ButtonClickSound.Instance.PlayClick();
		Defs.isFlag = false;
		Defs.isCOOP = false;
		Defs.isMulti = true;
		Defs.isHunger = false;
		Defs.isCompany = false;
		Defs.IsSurvival = false;
		Defs.isFlag = false;
		Defs.isCapturePoints = false;
		FlurryPluginWrapper.LogDeathmatchModePress();
		MenuBackgroundMusic.keepPlaying = true;
		LoadConnectScene.textureToShow = Resources.Load<Texture>(ConnectSceneNGUIController.MainLoadingTexture());
		LoadConnectScene.sceneToLoad = "ConnectSceneSandbox";
		FlurryPluginWrapper.LogEvent("Launch_Sandbox");
		LoadConnectScene.noteToShow = null;
		Application.LoadLevel(Defs.PromSceneName);
	}

	public void GoToProfile()
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (FriendsWindowGUI.Instance.InterfaceEnabled)
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining("Profile", TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		FlurryPluginWrapper.LogEvent("Profile");
		PlayerPrefs.SetInt(Defs.ProfileEnteredFromMenu, 0);
		if (ProfileController.Instance == null)
		{
			UnityEngine.Debug.LogWarning("ProfileController.Instance == null");
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		ButtonClickSound.Instance.PlayClick();
		if (NickLabelStack.sharedStack.gameObject != null)
		{
			NickLabelStack.sharedStack.gameObject.SetActive(false);
		}
		if (this.mainPanel != null)
		{
			this.mainPanel.transform.root.gameObject.SetActive(false);
		}
		ProfileController.Instance.ShowInterface(new Action[] { new Action(() => {
			if (NickLabelStack.sharedStack.gameObject != null)
			{
				NickLabelStack.sharedStack.gameObject.SetActive(true);
				if (this.persNickLabel != null)
				{
					this.persNickLabel.UpdateNickInLobby();
					this.persNickLabel.UpdateInfo();
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogWarning("nickLabelController == null");
				}
			}
			if (this.mainPanel != null)
			{
				this.mainPanel.transform.root.gameObject.SetActive(true);
			}
		}) });
	}

	private void HandleAgreeClicked(object sender, EventArgs e)
	{
		Defs.isShowUserAgrement = false;
		this.UserAgreementPanel.SetActive(false);
		if (this.notShowAgain.@value)
		{
			PlayerPrefs.SetInt("UserAgreement", 1);
		}
		if (this.isMultyPress)
		{
			this.GoMulty();
		}
		if (this.isFriendsPress)
		{
			this.GoFriens();
		}
	}

	private void HandleBackFromBankClicked(object sender, EventArgs e)
	{
		if (this._shopInstance != null)
		{
			UnityEngine.Debug.LogWarning("_shopInstance != null");
			return;
		}
		if (BankController.Instance == null)
		{
			UnityEngine.Debug.LogWarning("bankController == null");
			return;
		}
		if (BankController.Instance.InterfaceEnabledCoroutineLocked)
		{
			UnityEngine.Debug.LogWarning("InterfaceEnabledCoroutineLocked");
			return;
		}
		BankController.Instance.BackRequested -= new EventHandler(this.HandleBackFromBankClicked);
		BankController.Instance.InterfaceEnabled = false;
		if (this.nicknameLabel != null)
		{
			this.nicknameLabel.transform.root.gameObject.SetActive(true);
		}
		if (this.mainPanel != null)
		{
			this.mainPanel.transform.root.gameObject.SetActive(true);
		}
		if (this.singleModePanel != null && this.singleModePanel.activeSelf)
		{
			ExperienceController.SetEnable(true);
		}
	}

	private void HandleBankClicked(object sender, EventArgs e)
	{
		this.ShowBankWindow();
	}

	private void HandleCampaingClicked(object sender, EventArgs e)
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		this.StartCampaingButton();
	}

	public void HandleClansClicked()
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining("Clans", TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		new Action(() => {
			if (ProtocolListGetter.currentVersionIsSupported)
			{
				this.GoClans();
			}
			else
			{
				BannerWindowController sharedController = BannerWindowController.SharedController;
				if (sharedController != null)
				{
					sharedController.ForceShowBanner(BannerWindowType.NewVersion);
				}
			}
		})();
	}

	public void HandleDeveloperConsoleClicked()
	{
	}

	private void HandleDiclineClicked(object sender, EventArgs e)
	{
		Defs.isShowUserAgrement = false;
		this.UserAgreementPanel.SetActive(false);
	}

	private void HandleEscape()
	{
		if (this._backSubscribers.Count <= 0)
		{
			this._isCancellationRequested = true;
		}
		else
		{
			this.InvokeLastBackHandler();
		}
	}

	public void HandleFacebookLoginButton()
	{
		ButtonClickSound.TryPlayClick();
		if (!FB.IsLoggedIn)
		{
			MainMenuController.DoMemoryConsumingTaskInEmptyScene(() => FacebookController.Login(null, null, "Options", null), null);
		}
		else
		{
			FB.LogOut();
		}
	}

	private void HandleFreeClicked(object sender, EventArgs e)
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining("Free Coins", TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		this.settingsPanel.SetActive(false);
		this._freePanel.Value.SetVisible(true);
	}

	private void HandleFriendsClicked(object sender, EventArgs e)
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining("Friends", TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		new Action(() => {
			if (ProtocolListGetter.currentVersionIsSupported)
			{
				this.GoFriens();
			}
			else
			{
				BannerWindowController sharedController = BannerWindowController.SharedController;
				if (sharedController != null)
				{
					sharedController.ForceShowBanner(BannerWindowType.NewVersion);
				}
			}
		})();
	}

	private void HandleGameServicesClicked(object sender, EventArgs e)
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining("Game Services", TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		SettingsController component = this.settingsPanel.GetComponent<SettingsController>();
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log("[Sign in] pressed.");
			if (component != null)
			{
				component.RefreshSignOutButton();
			}
			return;
		}
		switch (BuildSettings.BuildTargetPlatform)
		{
			case RuntimePlatform.IPhonePlayer:
			{
				FlurryPluginWrapper.LogGamecenter();
				Application.isEditor;
				break;
			}
			case RuntimePlatform.PS3:
			case RuntimePlatform.XBOX360:
			{
				break;
			}
			case RuntimePlatform.Android:
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					if (!GpgFacade.Instance.IsAuthenticated())
					{
						GpgFacade instance = GpgFacade.Instance;
						instance.Authenticate((bool succeeded) => {
							PlayerPrefs.SetInt("GoogleSignInDenied", Convert.ToInt32(!succeeded));
							if (!succeeded)
							{
								UnityEngine.Debug.LogWarning("Authentication failed.");
							}
							else
							{
								Social.ShowAchievementsUI();
							}
							if (component != null)
							{
								component.RefreshSignOutButton();
							}
						}, false);
					}
					else
					{
						Social.ShowAchievementsUI();
					}
				}
				else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					AGSAchievementsClient.ShowAchievementsOverlay();
				}
				break;
			}
			default:
			{
				goto case RuntimePlatform.XBOX360;
			}
		}
	}

	public void HandleLeaderboardsClicked()
	{
		base.StartCoroutine(this.HandleLeaderboardsClickedCoroutine());
	}

	[DebuggerHidden]
	private IEnumerator HandleLeaderboardsClickedCoroutine()
	{
		MainMenuController.u003cHandleLeaderboardsClickedCoroutineu003ec__IteratorB7 variable = null;
		return variable;
	}

	public void HandleMultiPlayerClicked(object sender, EventArgs e)
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		this.OnClickMultiplyerButton();
	}

	private void HandleNewsButtonClicked(object sender, EventArgs e)
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (FriendsWindowGUI.Instance.InterfaceEnabled)
		{
			return;
		}
		this._newsPanel.Value.gameObject.SetActive(true);
		this.mainPanel.SetActive(false);
	}

	public void HandlePremiumClicked()
	{
		ShopNGUIController.ShowPremimAccountExpiredIfPossible(this.RentExpiredPoint, "NGUI", string.Empty, false);
	}

	private void HandleProfileClicked(object sender, EventArgs e)
	{
		this.GoToProfile();
	}

	public void HandlePromoActionClicked(string tg)
	{
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining("Promoactions", TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		AnalyticsStuff.LogSpecialOffersPanel("Efficiency", "View", null, null);
		if (tg != null && tg == "StickersPromoActionsPanelKey")
		{
			ButtonClickSound.Instance.PlayClick();
			BuySmileBannerController.openedFromPromoActions = true;
			this.OnBuySmilesClick();
			return;
		}
		int component = -1;
		ItemRecord byTag = ItemDb.GetByTag(tg);
		if (byTag != null)
		{
			string prefabName = byTag.PrefabName;
			if (prefabName != null)
			{
				UnityEngine.Object[] objArray = WeaponManager.sharedManager.weaponsInGame;
				int num = 0;
				while (num < (int)objArray.Length)
				{
					GameObject gameObject = (GameObject)objArray[num];
					if (gameObject.name != prefabName)
					{
						num++;
					}
					else
					{
						component = gameObject.GetComponent<WeaponSounds>().categoryNabor - 1;
						break;
					}
				}
			}
		}
		if (component == -1)
		{
			bool flag = false;
			foreach (KeyValuePair<ShopNGUIController.CategoryNames, List<List<string>>> keyValuePair in Wear.wear)
			{
				foreach (List<string> value in keyValuePair.Value)
				{
					if (!value.Contains(tg))
					{
						continue;
					}
					flag = true;
					component = (int)keyValuePair.Key;
					break;
				}
				if (!flag)
				{
					continue;
				}
				break;
			}
		}
		if (component == -1 && (SkinsController.skinsNamesForPers.ContainsKey(tg) || tg.Equals("CustomSkinID")))
		{
			component = 8;
		}
		if (ShopNGUIController.sharedShop != null)
		{
			ShopNGUIController.sharedShop.SetOfferID(tg);
			ShopNGUIController.sharedShop.IsInShopFromPromoPanel(true, tg);
			ShopNGUIController.sharedShop.offerCategory = (ShopNGUIController.CategoryNames)component;
		}
		this.HandleShopClicked(null, EventArgs.Empty);
	}

	private void HandleResumeFromShop()
	{
		if (this._shopInstance != null)
		{
			ShopNGUIController.GuiActive = false;
			this._shopInstance.resumeAction = () => {
			};
			this._shopInstance = null;
			if (NickLabelStack.sharedStack != null)
			{
				NickLabelStack.sharedStack.gameObject.SetActive(true);
				if (this.persNickLabel != null)
				{
					this.persNickLabel.UpdateNickInLobby();
					this.persNickLabel.UpdateInfo();
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogWarning("nickLabelController == null");
				}
			}
			if (StarterPackController.Get != null && StarterPackController.Get.isEventActive)
			{
				StarterPackController.Get.CheckShowStarterPack();
			}
			base.StartCoroutine(MainMenuController.ShowRanks());
		}
	}

	public void HandleSandboxClicked()
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining("Sandbox", TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		if (ProtocolListGetter.currentVersionIsSupported)
		{
			this.GoSandBox();
		}
		else
		{
			BannerWindowController sharedController = BannerWindowController.SharedController;
			if (sharedController != null)
			{
				sharedController.ForceShowBanner(BannerWindowType.NewVersion);
			}
		}
	}

	protected override void HandleSavePosJoystikClicked(object sender, EventArgs e)
	{
		base.HandleSavePosJoystikClicked(sender, e);
		ExperienceController.sharedController.isShowRanks = true;
		ExpController.Instance.InterfaceEnabled = true;
	}

	private void HandleSettingsClicked(object sender, EventArgs e)
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining("Settings", TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		this.rotateCamera.OnMainMenuOpenOptions();
		ButtonClickSound.Instance.PlayClick();
		base.StartCoroutine(this.OpenSettingPanelWithDelay());
	}

	public void HandleShopClicked(object sender, EventArgs e)
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining("Shop", TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		if (PlayerPrefs.GetInt(Defs.ShouldEnableShopSN, 0) != 1)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		FlurryPluginWrapper.LogEvent("Shop");
		this._shopInstance = ShopNGUIController.sharedShop;
		if (this._shopInstance == null)
		{
			UnityEngine.Debug.LogWarning("sharedShop == null");
		}
		else
		{
			MainMenuController.UnequipSniperRifleAndArmryArmoIfNeeded();
			this._shopInstance.SetInGame(false);
			ShopNGUIController.GuiActive = true;
			this._shopInstance.resumeAction = new Action(this.HandleResumeFromShop);
		}
	}

	public void HandleSignOutButton()
	{
		ButtonClickSound.TryPlayClick();
		PlayerPrefs.SetInt("GoogleSignInDenied", 1);
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log("[Sign Out] pressed.");
		}
		else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
		{
			GpgFacade.Instance.SignOut();
		}
		InfoWindowController.ShowInfoBox(LocalizationStore.Get("Key_2103") ?? "Signed out.");
		if (this.signOutButton != null)
		{
			this.signOutButton.gameObject.SetActive(false);
		}
	}

	private void HandleSkinsMakerClicked(object sender, EventArgs e)
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining("Skins Maker", TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		PlayerPrefs.SetInt(Defs.SkinEditorMode, 0);
		FlurryPluginWrapper.LogSkinsMakerModePress();
		FlurryPluginWrapper.LogSkinsMakerEnteredEvent();
		GlobalGameController.EditingCape = 0;
		GlobalGameController.EditingLogo = 0;
		FlurryPluginWrapper.LogEvent("Launch_Skins Maker");
		Singleton<SceneLoader>.Instance.LoadScene("SkinEditor", LoadSceneMode.Single);
	}

	private void HandleSocialButton(object sender, EventArgs e)
	{
		if (FriendsWindowGUI.Instance.InterfaceEnabled)
		{
			return;
		}
		this.rotateCamera.OnMainMenuOpenOptions();
		ButtonClickSound.Instance.PlayClick();
		this._freePanel.Value.SetVisible(true);
		this.mainPanel.SetActive(false);
	}

	private void HandleSocialGunViewLoginCompleted(bool success)
	{
		if (this.mainPanel == null)
		{
			return;
		}
		GameObject vector3 = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>(string.Concat("NguiWindows/", (!success ? "PanelAuthFailed" : "PanelAuthSucces"))));
		vector3.transform.parent = (!this._freePanel.ObjectIsActive ? this.mainPanel.transform : this._freePanel.Value.gameObject.transform);
		Player_move_c.SetLayerRecursively(vector3, this.mainPanel.layer);
		vector3.transform.localPosition = new Vector3(0f, 0f, -130f);
		vector3.transform.localRotation = Quaternion.identity;
		vector3.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	private void HandleSupportButtonClicked(object sender, EventArgs e)
	{
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining("Support", TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		this.settingsPanel.SetActive(false);
		this._feedbackPanel.Value.gameObject.SetActive(true);
	}

	public void HandleSurvivalClicked()
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		this.StartSurvivalButton();
	}

	public void HandleTrafficForwardingClicked()
	{
		if (string.IsNullOrEmpty(this._trafficForwardingUrl))
		{
			UnityEngine.Debug.LogError("HandleTrafficForwardingClicked() called while trafficForwardingUrl is empty.");
			return;
		}
		try
		{
			int num = PlayerPrefs.GetInt("TrafficForwarded", 0);
			PlayerPrefs.SetInt("TrafficForwarded", num + 1);
			AnalyticsStuff.LogTrafficForwarding(AnalyticsStuff.LogTrafficForwardingMode.Press);
			FriendsController.LogPromoTrafficForwarding(FriendsController.TypeTrafficForwardingLog.click);
		}
		finally
		{
			TrafficForwardingScript trafficForwardingScript = FriendsController.sharedController.Map<FriendsController, TrafficForwardingScript>((FriendsController fc) => fc.GetComponent<TrafficForwardingScript>());
			if (trafficForwardingScript == null)
			{
				this.RefreshTrafficForwardingButton(this, TrafficForwardingInfo.DisabledInstance);
			}
			else
			{
				Task<TrafficForwardingInfo> trafficForwardingInfo = trafficForwardingScript.GetTrafficForwardingInfo();
				this.RefreshTrafficForwardingButton(this, (!trafficForwardingInfo.IsCompleted || trafficForwardingInfo.IsCanceled || trafficForwardingInfo.IsFaulted ? TrafficForwardingInfo.DisabledInstance : trafficForwardingInfo.Result));
			}
		}
		Application.OpenURL(this._trafficForwardingUrl);
	}

	public void HandleTwitterLoginButton()
	{
		ButtonClickSound.TryPlayClick();
		if (!TwitterController.IsLoggedIn || !(TwitterController.Instance != null))
		{
			MainMenuController.DoMemoryConsumingTaskInEmptyScene(() => {
				if (TwitterController.Instance != null)
				{
					TwitterController.Instance.Login(null, null, "Options");
				}
			}, null);
		}
		else
		{
			TwitterController.Instance.Logout();
		}
	}

	[DebuggerHidden]
	private IEnumerator HideMenuInterfaceCoroutine(GameObject nickLabelObj)
	{
		MainMenuController.u003cHideMenuInterfaceCoroutineu003ec__IteratorB3 variable = null;
		return variable;
	}

	private void InitializeBannerWindow()
	{
		this._advertisementController = base.gameObject.GetComponent<AdvertisementController>();
		if (this._advertisementController == null)
		{
			this._advertisementController = base.gameObject.AddComponent<AdvertisementController>();
		}
		BannerWindowController.SharedController.advertiseController = this._advertisementController;
	}

	private void InvokeLastBackHandler()
	{
		if (this._backSubscribers.Count == 0)
		{
			return;
		}
		EventHandler item = this._backSubscribers[this._backSubscribers.Count - 1];
		item.Do<EventHandler>((EventHandler lastHandler) => lastHandler(this, EventArgs.Empty));
	}

	public static bool IsLevelUpOrBannerShown()
	{
		bool flag = (ExperienceController.sharedController == null ? false : ExperienceController.sharedController.isShowNextPlashka);
		return (flag ? true : (BannerWindowController.SharedController == null ? false : BannerWindowController.SharedController.IsAnyBannerShown));
	}

	public static bool IsShowRentExpiredPoint()
	{
		if (MainMenuController.sharedController == null)
		{
			return false;
		}
		Transform rentExpiredPoint = MainMenuController.sharedController.RentExpiredPoint;
		if (rentExpiredPoint == null)
		{
			return false;
		}
		return rentExpiredPoint.childCount > 0;
	}

	internal static bool LevelAlreadySaved(int level)
	{
		string str = string.Concat("currentLevel", level);
		return (!Storager.hasKey(str) ? false : Storager.getInt(str, false) > 0);
	}

	[DebuggerHidden]
	private IEnumerator LoadAndShowReplaceAdmobPereliv(string context)
	{
		MainMenuController.u003cLoadAndShowReplaceAdmobPerelivu003ec__IteratorAF variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator OnApplicationPause(bool pausing)
	{
		MainMenuController.u003cOnApplicationPauseu003ec__IteratorAE variable = null;
		return variable;
	}

	public void OnBuySmilesClick()
	{
		if (SkinEditorController.sharedController != null)
		{
			return;
		}
		BannerWindowController sharedController = BannerWindowController.SharedController;
		if (sharedController == null)
		{
			return;
		}
		sharedController.ForceShowBanner(BannerWindowType.buySmiles);
	}

	public void OnClickBackSingleModeButton()
	{
		base.StartCoroutine(MainMenuController.ShowRanks());
		base.StartCoroutine(this.SetActiveSinglePanel(false));
		this.rotateCamera.OnCloseSingleModePanel();
		this.coinsShopButton.transform.parent = this._parentBankPanel;
	}

	public void OnClickMultiplyerButton()
	{
		ButtonClickSound.Instance.PlayClick();
		new Action(() => {
			if (ProtocolListGetter.currentVersionIsSupported)
			{
				this.GoMulty();
			}
			else
			{
				BannerWindowController sharedController = BannerWindowController.SharedController;
				if (sharedController != null)
				{
					sharedController.ForceShowBanner(BannerWindowType.NewVersion);
				}
			}
		})();
	}

	public void OnClickSingleModeButton()
	{
		if (!ProtocolListGetter.currentVersionIsSupported)
		{
			BannerWindowController sharedController = BannerWindowController.SharedController;
			if (sharedController != null)
			{
				sharedController.ForceShowBanner(BannerWindowType.NewVersion);
			}
			return;
		}
		Defs.isDaterRegim = false;
		base.StartCoroutine(this.SetActiveSinglePanel(true));
		this.rotateCamera.OnOpenSingleModePanel();
		this._parentBankPanel = this.coinsShopButton.transform.parent;
		this.coinsShopButton.transform.parent = this.singleModePanel.transform;
		int value = 0;
		foreach (KeyValuePair<string, Dictionary<string, int>> boxesLevelsAndStar in CampaignProgress.boxesLevelsAndStars)
		{
			foreach (KeyValuePair<string, int> keyValuePair in boxesLevelsAndStar.Value)
			{
				value += keyValuePair.Value;
			}
		}
		this.singleModeStarsProgress.text = string.Format("{0}: {1}", LocalizationStore.Get("Key_1262"), string.Concat(value, "/60"));
		UILabel uILabel = this.singleModeBestScores;
		string str = LocalizationStore.Get("Key_0234");
		int num = PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0);
		uILabel.text = string.Format("{0} {1}", str, num.ToString());
	}

	private static void OnCompletedQuest(object sender, QuestCompletedEventArgs e)
	{
		AccumulativeQuestBase quest = e.Quest as AccumulativeQuestBase;
		if (quest == null)
		{
			return;
		}
		InfoWindowController.ShowAchievementBox(string.Empty, QuestConstants.GetAccumulativeQuestDescriptionByType(quest));
	}

	public void OnDayOfValorButtonClick()
	{
		BannerWindowController sharedController = BannerWindowController.SharedController;
		if (sharedController == null)
		{
			return;
		}
		sharedController.ForceShowBanner(BannerWindowType.DaysOfValor);
	}

	private void OnDayOfValorContainerShow(bool enable)
	{
		this.dayOfValorContainer.gameObject.SetActive(enable);
		this._dayOfValorEnabled = enable;
		this.dayOfValorTimer.text = PromoActionsManager.sharedManager.GetTimeToEndDaysOfValor();
	}

	private void OnDestroy()
	{
		if (NickLabelStack.sharedStack != null && NickLabelStack.sharedStack.gameObject != null)
		{
			NickLabelStack.sharedStack.gameObject.SetActive(true);
		}
		if (FriendsController.sharedController != null)
		{
			FriendsController.sharedController.GetComponent<TrafficForwardingScript>().Do<TrafficForwardingScript>((TrafficForwardingScript tf) => tf.Updated -= new EventHandler<TrafficForwardingInfo>(this.RefreshTrafficForwardingButton));
		}
		SocialGunBannerView.SocialGunBannerViewLoginCompletedWithResult -= new Action<bool>(this.HandleSocialGunViewLoginCompleted);
		PromoActionsManager.EventX3Updated -= new Action(this.OnEventX3Updated);
		StarterPackController.OnStarterPackEnable -= new StarterPackController.OnStarterPackEnableDelegate(this.OnStarterPackContainerShow);
		PromoActionsManager.OnDayOfValorEnable -= new PromoActionsManager.OnDayOfValorEnableDelegate(this.OnDayOfValorContainerShow);
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.ChangeLocalizeLabel));
		PromoActionClick.Click -= new Action<string>(this.HandlePromoActionClicked);
		SettingsController.ControlsClicked -= new Action(this.HandleControlsClicked);
		MainMenuController.sharedController = null;
		if (FreeAwardController.Instance != null)
		{
			FreeAwardController.Instance.transform.root.Map<Transform, GameObject>((Transform t) => t.gameObject).Do<GameObject>(new Action<GameObject>(UnityEngine.Object.Destroy));
		}
		if (!TrainingController.TrainingCompleted)
		{
			AskNameManager.onComplete -= new Action(HintController.instance.ShowCurrentHintObjectLabel);
		}
	}

	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
		if (MainMenuController.onActiveMainMenu != null)
		{
			MainMenuController.onActiveMainMenu(false);
		}
	}

	private new void OnEnable()
	{
		base.OnEnable();
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "Main Menu Controller");
		RewardedLikeButton[] componentsInChildren = base.GetComponentsInChildren<RewardedLikeButton>(true);
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Refresh();
		}
		if (ExperienceController.sharedController != null && !ShopNGUIController.GuiActive)
		{
			ExperienceController.sharedController.isShowRanks = true;
		}
		if (ExpController.Instance != null)
		{
			ExpController.Instance.InterfaceEnabled = true;
		}
		if (MainMenuController.onActiveMainMenu != null)
		{
			MainMenuController.onActiveMainMenu(true);
		}
		if (MainMenuController.onEnableMenuForAskname != null)
		{
			MainMenuController.onEnableMenuForAskname();
		}
	}

	private void OnEventX3Updated()
	{
		this.eventX3RemainTime[0].gameObject.SetActive(PromoActionsManager.sharedManager.IsEventX3Active);
	}

	private void OnGUI()
	{
		if (Launcher.UsingNewLauncher || !MainMenuController._drawLoadingProgress)
		{
			return;
		}
		ActivityIndicator.LoadingProgress = 1f;
	}

	public void OnShowBannerGift()
	{
		BannerWindowController sharedController = BannerWindowController.SharedController;
		if (sharedController == null)
		{
			return;
		}
		sharedController.ForceShowBanner(BannerWindowType.GiftBonuse);
	}

	public void OnSocialGunEventButtonClick()
	{
		if (SkinEditorController.sharedController != null)
		{
			return;
		}
		if (BannerWindowController.SharedController == null)
		{
			return;
		}
		this._socialBanner.Value.Show();
	}

	public void OnStarterPackButtonClick()
	{
		if (SkinEditorController.sharedController != null)
		{
			return;
		}
		BannerWindowController sharedController = BannerWindowController.SharedController;
		if (sharedController == null)
		{
			return;
		}
		sharedController.ForceShowBanner(BannerWindowType.StarterPack);
	}

	private void OnStarterPackContainerShow(bool enable)
	{
		Task<TrafficForwardingInfo> task = FriendsController.sharedController.Map<FriendsController, TrafficForwardingScript>((FriendsController f) => f.GetComponent<TrafficForwardingScript>()).Map<TrafficForwardingScript, Task<TrafficForwardingInfo>>((TrafficForwardingScript t) => t.GetTrafficForwardingInfo()).Filter<Task<TrafficForwardingInfo>>((Task<TrafficForwardingInfo> t) => (!t.IsCompleted || t.IsCanceled ? false : !t.IsFaulted));
		bool flag = ((task == null ? false : this.TrafficForwardingEnabled(task.Result)) ? false : enable);
		this.starterPackPanel.gameObject.SetActive(flag);
		if (flag)
		{
			this.buttonBackground.mainTexture = StarterPackController.Get.GetCurrentPackImage();
		}
		this._starterPackEnabled = flag;
		this.starterPackTimer.text = StarterPackController.Get.GetTimeToEndEvent();
	}

	[DebuggerHidden]
	private IEnumerator OpenSettingPanelWithDelay()
	{
		MainMenuController.u003cOpenSettingPanelWithDelayu003ec__IteratorB5 variable = null;
		return variable;
	}

	public bool PromoOffersPanelShouldBeShown()
	{
		return (this._shopInstance != null ? false : !MainMenuController.ShowBannerOrLevelup());
	}

	private void RefreshGui()
	{
		PlayerPrefs.DeleteKey("PendingGooglePlayGamesSync");
		if (WeaponManager.sharedManager != null)
		{
			CoroutineRunner.Instance.StartCoroutine(WeaponManager.sharedManager.ResetCoroutine(WeaponManager.sharedManager.CurrentFilterMap));
		}
		MainMenuController._syncPromise.TrySetResult(true);
	}

	private void RefreshSettingsButton()
	{
		if (this.settingsButton == null)
		{
			return;
		}
		ButtonHandler component = this.settingsButton.GetComponent<ButtonHandler>();
		if (component != null)
		{
			component.Clicked += new EventHandler(this.HandleSettingsClicked);
		}
		UIButton trainingCompleted = this.settingsButton.GetComponent<UIButton>();
		if (trainingCompleted != null)
		{
			trainingCompleted.isEnabled = TrainingController.TrainingCompleted;
		}
	}

	private void RefreshTrafficForwardingButton(object sender, TrafficForwardingInfo e)
	{
		if (e == null)
		{
			UnityEngine.Debug.LogError("Null TrafficForwardingInfo passed.");
			e = TrafficForwardingInfo.DisabledInstance;
		}
		this._trafficForwardingUrl = e.Url;
		bool flag = false;
		try
		{
			if (this != null)
			{
				flag = this.TrafficForwardingEnabled(e);
				if (flag && PlayerPrefs.GetInt(Defs.TrafficForwardingShowAnalyticsSent, 0) == 0)
				{
					AnalyticsStuff.LogTrafficForwarding(AnalyticsStuff.LogTrafficForwardingMode.Show);
					PlayerPrefs.SetInt(Defs.TrafficForwardingShowAnalyticsSent, 1);
					FriendsController.LogPromoTrafficForwarding(FriendsController.TypeTrafficForwardingLog.newView);
				}
				else if (flag)
				{
					FriendsController.LogPromoTrafficForwarding(FriendsController.TypeTrafficForwardingLog.view);
				}
				MainMenuController.trafficForwardActive = flag;
				ButtonBannerHUD.OnUpdateBanners();
				this.trafficForwardingButton.Do<UIButton>((UIButton tf) => tf.gameObject.SetActive(flag));
			}
		}
		finally
		{
			this.OnStarterPackContainerShow((flag ? false : StarterPackController.Get.isEventActive));
		}
	}

	private static void ReloadFacebookFriends()
	{
		if (FacebookController.FacebookSupported && FacebookController.sharedController != null && FB.IsLoggedIn)
		{
			FacebookController.sharedController.InputFacebookFriends(null, true);
		}
	}

	private void ReturnPersTonNormState()
	{
		HOTween.Kill(this.pers);
		Vector3 vector3 = new Vector3(-0.33f, 28f, -0.28f);
		this.idleTimerLastTime = Time.realtimeSinceStartup;
		HOTween.To(this.pers, 0.5f, (new TweenParms()).Prop("localRotation", new PlugQuaternion(vector3)).Ease(EaseType.Linear).OnComplete(() => this.idleTimerLastTime = Time.realtimeSinceStartup));
	}

	public static bool SavedShwonLobbyLevelIsLessThanActual()
	{
		return Storager.getInt(Defs.ShownLobbyLevelSN, false) < ExpController.LobbyLevel;
	}

	[DebuggerHidden]
	internal static IEnumerator SaveItemsToStorager(Action callback)
	{
		MainMenuController.u003cSaveItemsToStorageru003ec__IteratorB1 variable = null;
		return variable;
	}

	public void SaveShowPanelAndClose()
	{
		if (this.mainPanel != null)
		{
			this.saveOpenPanel.Clear();
			for (int i = 0; i < this.mainPanel.transform.childCount; i++)
			{
				GameObject child = this.mainPanel.transform.GetChild(i).gameObject;
				if (child.GetComponent<UICamera>() == null)
				{
					if (child.activeSelf)
					{
						this.saveOpenPanel.Add(child);
						child.SetActive(false);
					}
				}
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator SetActiveSinglePanel(bool isActive)
	{
		MainMenuController.u003cSetActiveSinglePanelu003ec__IteratorB8 variable = null;
		return variable;
	}

	public static void SetInputEnabled(bool enabled)
	{
		if (MainMenuController.sharedController != null)
		{
			MainMenuController.sharedController.uiCamera.enabled = enabled;
		}
	}

	public void ShowBankWindow()
	{
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining("Bank", TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		if (this._shopInstance != null)
		{
			UnityEngine.Debug.LogWarning("_shopInstance != null");
			return;
		}
		if (BankController.Instance == null)
		{
			UnityEngine.Debug.LogWarning("bankController == null");
			return;
		}
		if (BankController.Instance.InterfaceEnabledCoroutineLocked)
		{
			UnityEngine.Debug.LogWarning("InterfaceEnabledCoroutineLocked");
			return;
		}
		BankController.Instance.BackRequested += new EventHandler(this.HandleBackFromBankClicked);
		if ((GiftBannerWindow.instance == null || !GiftBannerWindow.instance.IsShow) && MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		this._bankEnteredTime = Time.realtimeSinceStartup;
		ButtonClickSound.Instance.PlayClick();
		if (this.mainPanel != null)
		{
			this.mainPanel.transform.root.gameObject.SetActive(false);
		}
		if (this.nicknameLabel != null)
		{
			this.nicknameLabel.transform.root.gameObject.SetActive(false);
		}
		BankController.Instance.InterfaceEnabled = true;
	}

	public static bool ShowBannerOrLevelup()
	{
		return (MainMenuController.IsLevelUpOrBannerShown() || FriendsWindowGUI.Instance.InterfaceEnabled || MainMenu.BlockInterface ? true : Defs.isShowUserAgrement);
	}

	[DebuggerHidden]
	public static IEnumerator ShowRanks()
	{
		return new MainMenuController.u003cShowRanksu003ec__IteratorB4();
	}

	public void ShowSavePanel(bool needClear = true)
	{
		for (int i = 0; i < this.saveOpenPanel.Count; i++)
		{
			GameObject item = this.saveOpenPanel[i];
			if (item != null)
			{
				item.SetActive(true);
			}
		}
		if (needClear)
		{
			this.saveOpenPanel.Clear();
		}
	}

	[DebuggerHidden]
	private new IEnumerator Start()
	{
		MainMenuController.u003cStartu003ec__IteratorB0 variable = null;
		return variable;
	}

	public void StartCampaingButton()
	{
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining("Campaign", TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		new Action(() => {
			Defs.isFlag = false;
			Defs.isCOOP = false;
			Defs.isMulti = false;
			Defs.isHunger = false;
			Defs.isCompany = false;
			Defs.IsSurvival = false;
			Defs.isCapturePoints = false;
			GlobalGameController.Score = 0;
			WeaponManager.sharedManager.Reset(0);
			FlurryPluginWrapper.LogCampaignModePress();
			StoreKitEventListener.State.Mode = "Campaign";
			StoreKitEventListener.State.PurchaseKey = "In game";
			StoreKitEventListener.State.Parameters.Clear();
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ Defs.RankParameterKey, ExperienceController.sharedController.currentLevel.ToString() },
				{ Defs.MultiplayerModesKey, StoreKitEventListener.State.Mode }
			};
			FlurryPluginWrapper.LogEvent(Defs.GameModesEventKey, strs, true);
			MenuBackgroundMusic.keepPlaying = true;
			LoadConnectScene.textureToShow = null;
			LoadConnectScene.sceneToLoad = "CampaignChooseBox";
			LoadConnectScene.noteToShow = null;
			Application.LoadLevel(Defs.PromSceneName);
		})();
	}

	public void StartSurvivalButton()
	{
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining("Survival", TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		new Action(() => {
			Defs.isFlag = false;
			Defs.isCOOP = false;
			Defs.isMulti = false;
			Defs.isHunger = false;
			Defs.isCompany = false;
			Defs.isCapturePoints = false;
			Defs.IsSurvival = true;
			CurrentCampaignGame.levelSceneName = string.Empty;
			GlobalGameController.Score = 0;
			WeaponManager.sharedManager.Reset(0);
			FlurryPluginWrapper.LogTrueSurvivalModePress();
			FlurryPluginWrapper.LogEvent("Launch_Survival");
			StoreKitEventListener.State.Mode = "Survival";
			StoreKitEventListener.State.PurchaseKey = "In game";
			StoreKitEventListener.State.Parameters.Clear();
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ Defs.RankParameterKey, ExperienceController.sharedController.currentLevel.ToString() },
				{ Defs.MultiplayerModesKey, StoreKitEventListener.State.Mode }
			};
			FlurryPluginWrapper.LogEvent(Defs.GameModesEventKey, strs, true);
			Defs.CurrentSurvMapIndex = UnityEngine.Random.Range(0, (int)Defs.SurvivalMaps.Length);
			Singleton<SceneLoader>.Instance.LoadScene("CampaignLoading", LoadSceneMode.Single);
		})();
	}

	[DebuggerHidden]
	private IEnumerator SynchronizeAmazonCoroutine(Action tryUpdateNickname, GameServicesController gameServicesController)
	{
		MainMenuController.u003cSynchronizeAmazonCoroutineu003ec__IteratorAC variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator SynchronizeEditorCoroutine()
	{
		MainMenuController.u003cSynchronizeEditorCoroutineu003ec__IteratorAA variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator SynchronizeGoogleCoroutine(Action tryUpdateNickname, GameServicesController gameServicesController)
	{
		MainMenuController.u003cSynchronizeGoogleCoroutineu003ec__IteratorAB variable = null;
		return variable;
	}

	private bool TrafficForwardingEnabled(TrafficForwardingInfo e)
	{
		return (PlayerPrefs.GetInt("TrafficForwarded", 0) >= 1 || MainMenuController.SavedShwonLobbyLevelIsLessThanActual() || !TrainingController.TrainingCompleted || !e.Enabled || ExperienceController.sharedController.currentLevel < e.MinLevel ? false : ExperienceController.sharedController.currentLevel <= e.MaxLevel);
	}

	[DebuggerHidden]
	private IEnumerator TryToShowExpiredBanner()
	{
		MainMenuController.u003cTryToShowExpiredBanneru003ec__IteratorB2 variable = null;
		return variable;
	}

	private static void UnequipSniperRifleAndArmryArmoIfNeeded()
	{
		try
		{
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
			{
				int trainingStep = AnalyticsStuff.TrainingStep;
				if (Storager.getInt("Training.NoviceArmorUsedKey", false) != 1 && trainingStep < 11 && Storager.getString(Defs.ArmorNewEquppedSN, false) != Defs.ArmorNewNoneEqupped)
				{
					ShopNGUIController.UnequipCurrentWearInCategory(ShopNGUIController.CategoryNames.ArmorCategory, false);
				}
				if (trainingStep < 9 && WeaponManager.sharedManager != null && WeaponManager.sharedManager.playerWeapons != null)
				{
					if ((
						from w in WeaponManager.sharedManager.playerWeapons.OfType<Weapon>()
						select w.weaponPrefab.GetComponent<WeaponSounds>()).FirstOrDefault<WeaponSounds>((WeaponSounds ws) => ws.categoryNabor - 1 == 4) != null)
					{
						WeaponManager.sharedManager.SaveWeaponSet(Defs.MultiplayerWSSN, string.Empty, 4);
						WeaponManager.sharedManager.Reset(0);
					}
				}
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in UnequipSniperRifleAndArmryArmoIfNeeded: ", exception));
		}
	}

	private void Update()
	{
		Rect rect;
		bool flag;
		if (this.InAdventureScreen && (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled) && ExperienceController.sharedController != null)
		{
			ExperienceController.sharedController.isShowRanks = true;
		}
		if (this.settingsPanel.activeInHierarchy)
		{
			if (this.facebookConnectedSettings.activeSelf != (!FacebookController.FacebookSupported ? false : FB.IsLoggedIn))
			{
				this.facebookConnectedSettings.SetActive((!FacebookController.FacebookSupported ? false : FB.IsLoggedIn));
			}
			if (this.facebookDisconnectedSettings.activeSelf != (!FacebookController.FacebookSupported || FB.IsLoggedIn ? false : Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 1))
			{
				this.facebookDisconnectedSettings.SetActive((!FacebookController.FacebookSupported || FB.IsLoggedIn ? false : Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 1));
			}
			if (this.facebookConnectSettings.activeSelf != (!FacebookController.FacebookSupported || FB.IsLoggedIn ? false : Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 0))
			{
				this.facebookConnectSettings.SetActive((!FacebookController.FacebookSupported || FB.IsLoggedIn ? false : Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 0));
			}
			if (this.twitterConnectedSettings.activeSelf != (!TwitterController.TwitterSupported ? false : TwitterController.IsLoggedIn))
			{
				this.twitterConnectedSettings.SetActive((!TwitterController.TwitterSupported ? false : TwitterController.IsLoggedIn));
			}
			if (this.twitterDisconnectedSettings.activeSelf != (!TwitterController.TwitterSupported || TwitterController.IsLoggedIn ? false : Storager.getInt(Defs.IsTwitterLoginRewardaGained, true) == 1))
			{
				this.twitterDisconnectedSettings.SetActive((!TwitterController.TwitterSupported || TwitterController.IsLoggedIn ? false : Storager.getInt(Defs.IsTwitterLoginRewardaGained, true) == 1));
			}
			if (this.twitterConnectSettings.activeSelf != (!TwitterController.TwitterSupported || TwitterController.IsLoggedIn ? false : Storager.getInt(Defs.IsTwitterLoginRewardaGained, true) == 0))
			{
				this.twitterConnectSettings.SetActive((!TwitterController.TwitterSupported || TwitterController.IsLoggedIn ? false : Storager.getInt(Defs.IsTwitterLoginRewardaGained, true) == 0));
			}
			if (this.facebookLoginContainer != null)
			{
				this.facebookLoginContainer.SetActive(FacebookController.FacebookSupported);
			}
			if (this.twitterLoginContainer != null)
			{
				this.twitterLoginContainer.SetActive(TwitterController.TwitterSupported);
			}
		}
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer && this.gameCenterButton.activeSelf != Social.localUser.authenticated)
		{
			this.gameCenterButton.SetActive(Social.localUser.authenticated);
		}
		flag = (Storager.getInt(Defs.PremiumEnabledFromServer, false) == 1 ? true : PremiumAccountController.Instance.isAccountActive);
		this.premium.SetActive((!flag || !(ExperienceController.sharedController != null) ? false : ExperienceController.sharedController.currentLevel >= 3));
		this.premiumButton.isEnabled = Storager.getInt(Defs.PremiumEnabledFromServer, false) == 1;
		if (this.premiumUpPlashka.activeSelf != ((PremiumAccountController.Instance == null ? 0 : (int)PremiumAccountController.Instance.isAccountActive) == 0))
		{
			this.premiumUpPlashka.SetActive((PremiumAccountController.Instance == null ? 0 : (int)PremiumAccountController.Instance.isAccountActive) == 0);
		}
		if (this.premiumbottomPlashka.activeSelf != (PremiumAccountController.Instance == null ? false : PremiumAccountController.Instance.isAccountActive))
		{
			this.premiumbottomPlashka.SetActive((PremiumAccountController.Instance == null ? false : PremiumAccountController.Instance.isAccountActive));
		}
		if (PremiumAccountController.Instance != null)
		{
			long daysToEndAllAccounts = (long)PremiumAccountController.Instance.GetDaysToEndAllAccounts();
			for (int i = 0; i < this.premiumLevels.Count; i++)
			{
				bool flag1 = false;
				if (daysToEndAllAccounts > (long)0 && daysToEndAllAccounts < (long)3 && i == 0)
				{
					flag1 = true;
				}
				if (daysToEndAllAccounts >= (long)3 && daysToEndAllAccounts < (long)7 && i == 1)
				{
					flag1 = true;
				}
				if (daysToEndAllAccounts >= (long)7 && daysToEndAllAccounts < (long)30 && i == 2)
				{
					flag1 = true;
				}
				if (daysToEndAllAccounts >= (long)30 && i == 3)
				{
					flag1 = true;
				}
				if (this.premiumLevels[i].activeSelf != flag1)
				{
					this.premiumLevels[i].SetActive(flag1);
				}
			}
			if (Time.realtimeSinceStartup - this._timePremiumTimeUpdated >= 1f)
			{
				this.premiumTime.text = PremiumAccountController.Instance.GetTimeToEndAllAccounts();
				this._timePremiumTimeUpdated = Time.realtimeSinceStartup;
			}
		}
		bool flag2 = (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled ? !ShopNGUIController.GuiActive : false);
		if (this.starParticleStarterPackGaemObject != null && this.starParticleStarterPackGaemObject.activeInHierarchy != flag2)
		{
			this.starParticleStarterPackGaemObject.SetActive(flag2);
		}
		if (Time.realtimeSinceStartup - this._eventX3RemainTimeLastUpdateTime >= 0.5f)
		{
			this._eventX3RemainTimeLastUpdateTime = Time.realtimeSinceStartup;
			this.UpdateEventX3RemainedTime();
			if (this._dayOfValorEnabled)
			{
				this.dayOfValorTimer.text = PromoActionsManager.sharedManager.GetTimeToEndDaysOfValor();
			}
		}
		if (this._isCancellationRequested)
		{
			MainMenuController mainMenuController = MainMenuController.sharedController;
			if (this.SettingsJoysticksPanel.activeSelf)
			{
				this.SettingsJoysticksPanel.SetActive(false);
				this.settingsPanel.SetActive(true);
			}
			else if (this._freePanel.ObjectIsActive)
			{
				if (this._shopInstance == null && !MainMenuController.ShowBannerOrLevelup())
				{
					this.mainPanel.SetActive(true);
					if (this._freePanel.ObjectIsLoaded)
					{
						this._freePanel.Value.SetVisible(false);
					}
					this.rotateCamera.OnMainMenuCloseOptions();
				}
			}
			else if (this._newsPanel.ObjectIsActive)
			{
				this._newsPanel.Value.gameObject.SetActive(false);
				this.mainPanel.SetActive(true);
			}
			else if (BannerWindowController.SharedController != null && BannerWindowController.SharedController.IsAnyBannerShown)
			{
				BannerWindowController.SharedController.HideBannerWindow();
			}
			else if (!(this.settingsPanel != null) || !this.settingsPanel.activeInHierarchy)
			{
				if (!this._freePanel.ObjectIsLoaded || !this._freePanel.Value.gameObject.activeInHierarchy)
				{
					if (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled)
					{
						if (!ShopNGUIController.GuiActive)
						{
							if (!(ProfileController.Instance != null) || !ProfileController.Instance.InterfaceEnabled)
							{
								if (PremiumAccountScreenController.Instance != null)
								{
									PremiumAccountScreenController.Instance.Hide();
								}
								else if (!(mainMenuController != null) || !mainMenuController.singleModePanel.activeSelf)
								{
									PlayerPrefs.Save();
									Application.Quit();
								}
								else
								{
									mainMenuController.OnClickBackSingleModeButton();
								}
							}
						}
					}
				}
			}
			this._isCancellationRequested = false;
		}
		if (!(GiftBannerWindow.instance != null) || !GiftBannerWindow.instance.IsShow)
		{
			this.PromoActionsPanel.SetActive((!FriendsController.SpecialOffersEnabled || !this.PromoOffersPanelShouldBeShown() ? false : Storager.getInt(Defs.ShownLobbyLevelSN, false) > 2));
		}
		else
		{
			this.PromoActionsPanel.SetActive(false);
		}
		if (this.rotateCamera != null && !this.rotateCamera.IsAnimPlaying)
		{
			float single = -120f;
			single = single * (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android ? 0.5f : 2f);
			if (!this.settingsPanel.activeInHierarchy)
			{
				if (this.campaignRect.width.Equals(0f))
				{
					this.CalcBtnRects();
				}
				rect = (!(MenuLeaderboardsController.sharedController != null) || !MenuLeaderboardsController.sharedController.IsOpened ? new Rect(0.2f * (float)Screen.width, 0.25f * (float)Screen.height, 1.4f * (float)Screen.width, 0.65f * (float)Screen.height) : new Rect(0.38f * (float)Screen.width, 0.25f * (float)Screen.height, 1.4f * (float)Screen.width, 0.65f * (float)Screen.height));
			}
			else
			{
				rect = new Rect(0f, 0.1f * (float)Screen.height, 0.5f * (float)Screen.width, 0.8f * (float)Screen.height);
			}
			if (MainMenuController.canRotationLobbyPlayer)
			{
				if (Input.touchCount > 0 && !ShopNGUIController.GuiActive)
				{
					Touch touch = Input.GetTouch(0);
					if (touch.phase == TouchPhase.Moved && rect.Contains(touch.position))
					{
						this.idleTimerLastTime = Time.realtimeSinceStartup;
						Transform transforms = this.pers;
						Vector3 vector3 = Vector3.up;
						Vector2 vector2 = touch.deltaPosition;
						transforms.Rotate(vector3, vector2.x * single * 0.5f * (Time.realtimeSinceStartup - this.lastTime));
					}
				}
				if (Application.isEditor && !ShopNGUIController.GuiActive)
				{
					float axis = Input.GetAxis("Mouse ScrollWheel") * 3f * single * (Time.realtimeSinceStartup - this.lastTime);
					this.pers.Rotate(Vector3.up, axis);
					if (axis != 0f)
					{
						this.idleTimerLastTime = Time.realtimeSinceStartup;
					}
				}
			}
			this.lastTime = Time.realtimeSinceStartup;
		}
		if (Time.realtimeSinceStartup - this.idleTimerLastTime > ShopNGUIController.IdleTimeoutPers)
		{
			this.ReturnPersTonNormState();
		}
		if (this._starterPackEnabled)
		{
			this.starterPackTimer.text = StarterPackController.Get.GetTimeToEndEvent();
		}
		if (!MainMenuController.sharedController.stubLoading.activeInHierarchy)
		{
			if (!(ShopNGUIController.sharedShop != null) || !ShopNGUIController.GuiActive)
			{
				if (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled)
				{
					if (!(BannerWindowController.SharedController != null) || !BannerWindowController.SharedController.IsAnyBannerShown)
					{
						if (!this.singleModePanel.gameObject.activeSelf)
						{
							if (!TrainingController.TrainingCompleted)
							{
								return;
							}
							if (true)
							{
								if (MobileAdManager.AdIsApplicable(MobileAdManager.Type.Video))
								{
									if (!this._timeTamperingDetected.Value)
									{
										if (FreeAwardController.Instance.IsInState<FreeAwardController.IdleState>())
										{
											if (FreeAwardController.Instance.AdvertCountLessThanLimit())
											{
												this.freeAwardChestObj.SetActive(true);
											}
											else if (this.freeAwardChestObj.GetActive())
											{
												FreeAwardShowHandler.Instance.HideChestWithAnimation();
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		if (this._newClanIncomingInvitesSprite.Value != null)
		{
			if (ClanIncomingInvitesController.CurrentRequest == null || !ClanIncomingInvitesController.CurrentRequest.IsCompleted)
			{
				this._newClanIncomingInvitesSprite.Value.gameObject.SetActive(false);
			}
			else if (ClanIncomingInvitesController.CurrentRequest.IsCanceled || ClanIncomingInvitesController.CurrentRequest.IsFaulted)
			{
				this._newClanIncomingInvitesSprite.Value.gameObject.SetActive(false);
			}
			else
			{
				this._newClanIncomingInvitesSprite.Value.gameObject.SetActive(ClanIncomingInvitesController.CurrentRequest.Result.Count > 0);
			}
		}
	}

	private void UpdateEventX3RemainedTime()
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds((double)PromoActionsManager.sharedManager.EventX3RemainedTime);
		string empty = string.Empty;
		if (timeSpan.Days <= 0)
		{
			empty = string.Format("{0}: {1:00}:{2:00}:{3:00}", new object[] { this._localizeSaleLabel, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds });
		}
		else
		{
			object[] days = new object[] { this._localizeSaleLabel, timeSpan.Days, null, null, null, null };
			days[2] = (timeSpan.Days != 1 ? "Days" : "Day");
			days[3] = timeSpan.Hours;
			days[4] = timeSpan.Minutes;
			days[5] = timeSpan.Seconds;
			empty = string.Format("{0}: {1} {2} {3:00}:{4:00}:{5:00}", days);
		}
		if (this.eventX3RemainTime != null)
		{
			for (int i = 0; i < (int)this.eventX3RemainTime.Length; i++)
			{
				this.eventX3RemainTime[i].text = empty;
			}
		}
		if (this.colorBlinkForX3 != null && timeSpan.TotalHours < (double)Defs.HoursToEndX3ForIndicate && !this.colorBlinkForX3.enabled)
		{
			this.colorBlinkForX3.enabled = true;
		}
	}

	[DebuggerHidden]
	private IEnumerator WaitReturnToMainMenuAndShowRestorePanel(Action refreshCallback)
	{
		MainMenuController.u003cWaitReturnToMainMenuAndShowRestorePanelu003ec__IteratorAD variable = null;
		return variable;
	}

	public event EventHandler BackPressed
	{
		add
		{
			this._backSubscribers.Add(value);
		}
		remove
		{
			this._backSubscribers.Remove(value);
		}
	}

	public static event Action<bool> onActiveMainMenu;

	public static event Action onEnableMenuForAskname;

	public static event Action onLoadMenu;
}