using ExitGames.Client.Photon;
using FyberPlugin;
using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectSceneNGUIController : MonoBehaviour
{
	public const string PendingInterstitialKey = "PendingInterstitial";

	public static ConnectSceneNGUIController.PlatformConnect myPlatformConnect;

	private string rulesDeadmatch;

	private string rulesDater;

	private string rulesTeamFight;

	private string rulesTimeBattle;

	private string rulesDeadlyGames;

	private string rulesFlagCapture;

	private string rulesCapturePoint;

	public GameObject armoryButton;

	public int myLevelGame;

	public UILabel rulesLabel;

	public static int gameTier;

	public readonly static IDictionary<string, string> gameModesLocalizeKey;

	public List<ConnectSceneNGUIController.infoServer> servers = new List<ConnectSceneNGUIController.infoServer>();

	private float posNumberOffPlayersX = -139f;

	private string goMapName;

	public static TypeModeGame curSelectMode;

	private Dictionary<string, Texture> mapPreview = new Dictionary<string, Texture>();

	public UILabel priceRegimLabel;

	public UILabel priceMapLabel;

	public UILabel priceMapLabelInCreate;

	public GameObject mapPreviewTexture;

	public GameObject grid;

	public MyCenterOnChild centerScript;

	public Transform ScrollTransform;

	public Transform selectMapPanelTransform;

	public MapPreviewController selectMap;

	public float widthCell;

	public int countMap;

	public UIButton createRoomUIBtn;

	public UISprite fonMapPreview;

	public UIPanel mapPreviewPanel;

	public GameObject mainPanel;

	public GameObject localBtn;

	public GameObject customBtn;

	public GameObject randomBtn;

	public GameObject goBtn;

	public GameObject backBtn;

	public GameObject unlockBtn;

	public GameObject unlockMapBtnInCreate;

	public GameObject unlockMapBtn;

	public GameObject cancelFromConnectToPhotonBtn;

	public GameObject connectToPhotonPanel;

	public GameObject failInternetLabel;

	public GameObject customPanel;

	public GameObject gameInfoItemPrefab;

	public GameObject loadingMapPanel;

	public GameObject searchPanel;

	public GameObject clearBtn;

	public GameObject searchBtn;

	public GameObject showSearchPanelBtn;

	public GameObject selectMapPanel;

	public GameObject createPanel;

	public GameObject goToCreateRoomBtn;

	public GameObject createRoomBtn;

	public GameObject setPasswordBtn;

	public GameObject clearInSetPasswordBtn;

	public GameObject okInsetPasswordBtn;

	public GameObject setPasswordPanel;

	public GameObject passONSprite;

	public GameObject enterPasswordPanel;

	public GameObject joinRoomFromEnterPasswordBtn;

	public GameObject connectToWiFIInCreateLabel;

	public GameObject connectToWiFIInCustomLabel;

	public Transform scrollViewSelectMapTransform;

	public PlusMinusController numberOfPlayer;

	public PlusMinusController killToWin;

	public TeamNumberOfPlayer teamCountPlayer;

	public UIGrid gridGames;

	public UIInput searchInput;

	public UIInput nameServerInput;

	public UIInput setPasswordInput;

	public UIInput enterPasswordInput;

	public Transform gridGamesTransform;

	public UITexture loadingToDraw;

	public UILabel conditionLabel;

	private static ConnectSceneNGUIController.RegimGame _regim;

	public static bool isReturnFromGame;

	public int nRegim;

	private bool isSetUseMap;

	public string gameNameFilter;

	public List<GameObject> gamesInfo = new List<GameObject>();

	public DisableObjectFromTimer gameIsfullLabel;

	public DisableObjectFromTimer incorrectPasswordLabel;

	public DisableObjectFromTimer serverIsNotAvalible;

	public DisableObjectFromTimer accountBlockedLabel;

	public DisableObjectFromTimer nameAlreadyUsedLabel;

	private float timerShowBan = -1f;

	private bool isConnectingToPhoton;

	private bool isCancelConnectingToPhoton;

	private int pressButton;

	private List<RoomInfo> filteredRoomList = new List<RoomInfo>();

	private int countNoteCaptureDeadmatch = 5;

	private int countNoteCaptureCOOP = 5;

	private int countNoteCaptureHunger = 5;

	private int countNoteCaptureFlag = 5;

	private int countNoteCaptureCompany = 5;

	public static ConnectSceneNGUIController sharedController;

	private string password = string.Empty;

	public LANBroadcastService lanScan;

	private RoomInfo joinRoomInfoFromCustom;

	private bool firstConnectToPhoton;

	private bool isGoInPhotonGame;

	private bool isMainPanelActiv = true;

	public GameObject ChooseMapLabelSmall;

	private AdvertisementController _advertisementController;

	public UIToggle deathmatchToggle;

	public UIToggle teamFightToogle;

	public UIToggle timeBattleToogle;

	public UIToggle deadlyGamesToogle;

	public UIToggle flagCaptureToogle;

	public UIToggle capturePointsToogle;

	public bool isStartShowAdvert;

	private Action actAfterConnectToPhoton;

	private GameInfo[] roomFields;

	public UIWrapContent wrapGames;

	public UIScrollView scrollGames;

	public static Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> mapStatistics;

	public static string selectedMap;

	public static bool directedFromQuests;

	public GameObject modeAnimObj;

	public GameObject fingerAnimObj;

	public UIButton[] modeButtonByLevel;

	public UILabel[] modeUnlockLabelByLevel;

	private bool fingerStopped;

	private bool animationStarted;

	private bool loadReplaceAdmobPerelivRunning;

	private bool loadAdmobRunning;

	private int _countOfLoopsRequestAdThisTime;

	private float _lastTimeInterstitialShown;

	public static bool NeedShowReviewInConnectScene;

	public readonly static string mapProperty;

	public readonly static string passwordProperty;

	public readonly static string platformProperty;

	public readonly static string endingProperty;

	public readonly static string maxKillProperty;

	public readonly static string ABTestProperty;

	public readonly static string ABTestEnum;

	private bool abTestConnect = (Defs.isActivABTestBuffSystem ? true : Defs.isActivABTestRatingSystem);

	private int joinNewRoundTries;

	private int tryJoinRoundMap;

	private IDisposable _someWindowSubscription;

	private int _tempMinValue = 3;

	private int _tempMaxValue = 7;

	private int _tempStep = 2;

	private int daterStep = 5;

	private int daterMinValue = 5;

	private int daterMaxValue = 10;

	private IDisposable _backSubscription;

	private float startPosX;

	private LoadingNGUIController _loadingNGUIController;

	private LANBroadcastService.ReceivedMessage[] _copy;

	internal static bool InterstitialRequest
	{
		get;
		set;
	}

	public static bool isTeamRegim
	{
		get
		{
			bool flag;
			if (!Defs.isMulti)
			{
				flag = false;
			}
			else
			{
				flag = (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture ? true : ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints);
			}
			return flag;
		}
	}

	public static ConnectSceneNGUIController.RegimGame regim
	{
		get
		{
			return ConnectSceneNGUIController._regim;
		}
		set
		{
			ConnectSceneNGUIController._regim = value;
			ConnectSceneNGUIController.UpdateUseMasMaps();
		}
	}

	internal static bool ReplaceAdmobWithPerelivRequest
	{
		get;
		set;
	}

	static ConnectSceneNGUIController()
	{
		ConnectSceneNGUIController.myPlatformConnect = ConnectSceneNGUIController.PlatformConnect.android;
		ConnectSceneNGUIController.gameTier = 1;
		Dictionary<string, string> strs = new Dictionary<string, string>()
		{
			{ 0.ToString(), "Key_0104" },
			{ 1.ToString(), "Key_0135" },
			{ 2.ToString(), "Key_0130" },
			{ 3.ToString(), "Key_0121" },
			{ 4.ToString(), "Key_0113" },
			{ 5.ToString(), "Key_1263" },
			{ 6.ToString(), "Key_1465" },
			{ 7.ToString(), "Key_1466" }
		};
		ConnectSceneNGUIController.gameModesLocalizeKey = strs;
		ConnectSceneNGUIController._regim = ConnectSceneNGUIController.RegimGame.Deathmatch;
		ConnectSceneNGUIController.mapStatistics = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>();
		ConnectSceneNGUIController.selectedMap = string.Empty;
		ConnectSceneNGUIController.directedFromQuests = false;
		ConnectSceneNGUIController.NeedShowReviewInConnectScene = false;
		ConnectSceneNGUIController.mapProperty = "C0";
		ConnectSceneNGUIController.passwordProperty = "C1";
		ConnectSceneNGUIController.platformProperty = "C2";
		ConnectSceneNGUIController.endingProperty = "C3";
		ConnectSceneNGUIController.maxKillProperty = "C4";
		ConnectSceneNGUIController.ABTestProperty = "C5";
		ConnectSceneNGUIController.ABTestEnum = "C6";
	}

	public ConnectSceneNGUIController()
	{
	}

	[DebuggerHidden]
	private IEnumerator AnimateModeOpen()
	{
		ConnectSceneNGUIController.u003cAnimateModeOpenu003ec__Iterator1A variable = null;
		return variable;
	}

	private void Awake()
	{
		if (ConnectSceneNGUIController.isReturnFromGame)
		{
			Defs.countReturnInConnectScene = Defs.countReturnInConnectScene + 1;
		}
		PhotonObjectCacher.AddObject(base.gameObject);
		this.setPasswordInput.onSubmit.Add(new EventDelegate(() => this.OnPaswordSelected()));
	}

	private void BackFromSetPasswordPanel()
	{
		this.createPanel.SetActive(true);
		this.selectMapPanel.SetActive(true);
		this.passONSprite.SetActive(!string.IsNullOrEmpty(this.password));
		this.setPasswordPanel.SetActive(false);
	}

	private bool CheckLocalAvailability()
	{
		if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
		{
			return true;
		}
		return false;
	}

	[Obfuscation(Exclude=true)]
	private void ConnectToPhoton()
	{
		string str;
		if (FriendsController.sharedController != null && FriendsController.sharedController.Banned == 1)
		{
			return;
		}
		if (PhotonNetwork.connectionState == ConnectionState.Connecting || PhotonNetwork.connectionState == ConnectionState.Connected)
		{
			UnityEngine.Debug.Log("ConnectToPhoton return");
			return;
		}
		UnityEngine.Debug.Log("ConnectToPhoton");
		if (FriendsController.sharedController != null && FriendsController.sharedController.Banned == 1)
		{
			this.timerShowBan = 3f;
			return;
		}
		this.isConnectingToPhoton = true;
		this.isCancelConnectingToPhoton = false;
		ConnectSceneNGUIController.gameTier = (ExpController.Instance == null ? 1 : ExpController.Instance.OurTier);
		if (Defs.useSqlLobby)
		{
			PhotonNetwork.lobby = new TypedLobby("PixelGun3D", LobbyType.SqlLobby);
		}
		string[] separator = new string[] { Initializer.Separator, ConnectSceneNGUIController.regim.ToString(), null, null, null };
		if (!Defs.isDaterRegim)
		{
			str = (!Defs.isHunger ? ConnectSceneNGUIController.gameTier.ToString() : "0");
		}
		else
		{
			str = "Dater";
		}
		separator[2] = str;
		separator[3] = "v";
		separator[4] = GlobalGameController.MultiplayerProtocolVersion;
		PhotonNetwork.ConnectUsingSettings(string.Concat(separator));
	}

	public static void CreateGameRoom(string roomName, int playerLimit, int mapIndex, int MaxKill, string password, ConnectSceneNGUIController.RegimGame gameMode)
	{
		int num;
		string[] aBTestProperty = new string[] { ConnectSceneNGUIController.mapProperty, ConnectSceneNGUIController.passwordProperty, ConnectSceneNGUIController.platformProperty, ConnectSceneNGUIController.endingProperty, ConnectSceneNGUIController.maxKillProperty, "TimeMatchEnd", "tier", ConnectSceneNGUIController.ABTestProperty, ConnectSceneNGUIController.ABTestEnum, "SpecialBonus" };
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[ConnectSceneNGUIController.mapProperty] = mapIndex;
		hashtable[ConnectSceneNGUIController.passwordProperty] = password;
		ExitGames.Client.Photon.Hashtable hashtable1 = hashtable;
		string str = ConnectSceneNGUIController.platformProperty;
		if (!string.IsNullOrEmpty(password))
		{
			num = 3;
		}
		else
		{
			num = (int)ConnectSceneNGUIController.myPlatformConnect;
		}
		hashtable1[str] = num;
		hashtable[ConnectSceneNGUIController.endingProperty] = 0;
		hashtable[ConnectSceneNGUIController.maxKillProperty] = MaxKill;
		hashtable["TimeMatchEnd"] = PhotonNetwork.time;
		hashtable["tier"] = ExpController.Instance.OurTier;
		if (ExpController.Instance.OurTier == 0)
		{
			hashtable[ConnectSceneNGUIController.ABTestProperty] = (!Defs.isABTestBalansCohortActual ? 0 : 1);
		}
		if (Defs.isActivABTestRatingSystem)
		{
			hashtable[ConnectSceneNGUIController.ABTestEnum] = (!FriendsController.isUseRatingSystem ? 1 : 2);
		}
		else if (Defs.isActivABTestBuffSystem)
		{
			hashtable[ConnectSceneNGUIController.ABTestEnum] = (!FriendsController.useBuffSystem ? 1 : 3);
		}
		hashtable["SpecialBonus"] = 0;
		ConnectSceneNGUIController.PhotonCreateRoom(roomName, true, true, (playerLimit <= 10 ? playerLimit : 10), hashtable, aBTestProperty);
	}

	private void CustomBtnAct()
	{
		this.gameNameFilter = string.Empty;
		if (Defs.isInet)
		{
			base.Invoke("UpdateFilteredRoomListInvoke", 0.03f);
		}
		this.showSearchPanelBtn.SetActive(Defs.isInet);
		this.mainPanel.SetActive(false);
		this.selectMapPanel.SetActive(false);
		this.customPanel.SetActive(true);
		this.password = string.Empty;
		this.incorrectPasswordLabel.timer = -1f;
		this.incorrectPasswordLabel.gameObject.SetActive(false);
		this.gameIsfullLabel.timer = -1f;
		this.gameIsfullLabel.gameObject.SetActive(false);
		this.wrapGames.SortAlphabetically();
		this.scrollGames.ResetPosition();
	}

	private void EnterPassInput()
	{
		this.HandleJoinRoomFromEnterPasswordBtnClicked(null, null);
	}

	private void EnterPassInputSubmit()
	{
		this.enterPasswordInput.RemoveFocus();
		this.enterPasswordInput.isSelected = false;
		base.Invoke("EnterPassInput", 0.1f);
	}

	private void GetMapName(string _mapName, bool isAddCountRun)
	{
		bool flag;
		Texture texture;
		UnityEngine.Debug.Log(string.Concat("setFonLoading ", _mapName));
		if (Defs.isCOOP)
		{
			int num = PlayerPrefs.GetInt("CountRunCoop", 0);
			flag = num < 5;
			if (isAddCountRun)
			{
				PlayerPrefs.SetInt("CountRunCoop", PlayerPrefs.GetInt("CountRunCoop", 0) + 1);
			}
			texture = Resources.Load(string.Concat("NoteLoadings/note_Time_Survival_", num % this.countNoteCaptureCOOP)) as Texture;
		}
		else if (Defs.isCompany)
		{
			int num1 = PlayerPrefs.GetInt("CountRunCompany", 0);
			flag = num1 < 5;
			texture = Resources.Load(string.Concat("NoteLoadings/note_Team_Battle_", num1 % this.countNoteCaptureCompany)) as Texture;
			if (isAddCountRun)
			{
				PlayerPrefs.SetInt("CountRunCompany", PlayerPrefs.GetInt("CountRunCompany", 0) + 1);
			}
		}
		else if (Defs.isHunger)
		{
			int num2 = PlayerPrefs.GetInt("CountRunHunger", 0);
			flag = num2 < 5;
			texture = Resources.Load(string.Concat("NoteLoadings/note_Deadly_Games_", num2 % this.countNoteCaptureHunger)) as Texture;
			if (isAddCountRun)
			{
				PlayerPrefs.SetInt("CountRunHunger", PlayerPrefs.GetInt("CountRunHunger", 0) + 1);
			}
		}
		else if (!Defs.isFlag)
		{
			int num3 = PlayerPrefs.GetInt("CountRunDeadmath", 0);
			flag = num3 < 5;
			texture = Resources.Load(string.Concat("NoteLoadings/note_Deathmatch_", num3 % this.countNoteCaptureDeadmatch)) as Texture;
			if (isAddCountRun)
			{
				PlayerPrefs.SetInt("CountRunDeadmath", PlayerPrefs.GetInt("CountRunDeadmath", 0) + 1);
			}
		}
		else
		{
			int num4 = PlayerPrefs.GetInt("CountRunFlag", 0);
			flag = num4 < 5;
			texture = Resources.Load(string.Concat("NoteLoadings/note_Flag_Capture_", num4 % this.countNoteCaptureFlag)) as Texture;
			if (isAddCountRun)
			{
				PlayerPrefs.SetInt("CountRunFlag", PlayerPrefs.GetInt("CountRunFlag", 0) + 1);
			}
		}
		LoadConnectScene.textureToShow = Resources.Load(string.Concat("LevelLoadings", (!Device.isRetinaAndStrong ? string.Empty : "/Hi"), "/Loading_", _mapName)) as Texture2D;
		LoadingInAfterGame.loadingTexture = LoadConnectScene.textureToShow;
		LoadConnectScene.sceneToLoad = _mapName;
		LoadConnectScene.noteToShow = null;
		this.loadingToDraw.gameObject.SetActive(false);
	}

	private int GetRandomMapIndex()
	{
		SceneInfo item;
		bool flag;
		bool flag1 = true;
		AllScenesForMode listScenesForMode = SceneInfoController.instance.GetListScenesForMode(ConnectSceneNGUIController.curSelectMode);
		if (listScenesForMode == null)
		{
			return -1;
		}
		int count = listScenesForMode.avaliableScenes.Count;
		int num = UnityEngine.Random.Range(0, count);
		int num1 = 0;
		do
		{
			if (num1 > count)
			{
				return -1;
			}
			item = listScenesForMode.avaliableScenes[num];
			if (item != null)
			{
				num++;
				num1++;
				if (num >= count)
				{
					num = 0;
				}
				if (!item.isPremium)
				{
					flag = false;
				}
				else
				{
					flag = (Storager.getInt(string.Concat(item.NameScene, "Key"), true) != 0 ? false : !PremiumAccountController.MapAvailableDueToPremiumAccount(item.NameScene));
				}
				flag1 = flag;
			}
		}
		while (flag1);
		return item.indexMap;
	}

	private void GoBtnAct()
	{
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(this.selectMap.mapID);
		if (infoScene == null)
		{
			return;
		}
		bool flag = infoScene.isPremium;
		if (!flag || flag && (Storager.getInt(string.Concat(infoScene.NameScene, "Key"), true) == 1 || PremiumAccountController.MapAvailableDueToPremiumAccount(infoScene.NameScene)))
		{
			this.JoinRandomRoom(infoScene.indexMap, ConnectSceneNGUIController.regim);
		}
		else
		{
			PhotonNetwork.Disconnect();
		}
	}

	[Obfuscation(Exclude=true)]
	private void goGame()
	{
		WeaponManager.sharedManager.Reset((!Defs.filterMaps.ContainsKey(PlayerPrefs.GetString("MapName")) ? 0 : Defs.filterMaps[PlayerPrefs.GetString("MapName")]));
		Singleton<SceneLoader>.Instance.LoadScene(PlayerPrefs.GetString("MapName"), LoadSceneMode.Single);
	}

	public static void GoToClans()
	{
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.sceneToLoad = "Clans";
		LoadConnectScene.noteToShow = null;
		SceneManager.LoadScene(Defs.PromSceneName);
	}

	public static void GoToFriends()
	{
		FriendsController friendsController = FriendsController.sharedController;
		if (friendsController != null)
		{
			friendsController.GetFriendsData(false);
		}
		MainMenuController.friendsOnStart = true;
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.sceneToLoad = Defs.MainMenuScene;
		LoadConnectScene.noteToShow = null;
		SceneManager.LoadScene(Defs.PromSceneName);
		Defs.isDaterRegim = false;
	}

	public static void GoToProfile()
	{
		PlayerPrefs.SetInt(Defs.SkinEditorMode, 1);
		GlobalGameController.EditingLogo = 0;
		GlobalGameController.EditingCape = 0;
		SceneManager.LoadScene("SkinEditor");
	}

	private void HandleBackBtnClicked(object sender, EventArgs e)
	{
		if (this.mainPanel != null && this.mainPanel.activeSelf)
		{
			if (FriendsController.sharedController != null)
			{
				FriendsController.sharedController.GetFriendsData(false);
			}
			FlurryPluginWrapper.LogEvent("Back to Main Menu");
			MenuBackgroundMusic.keepPlaying = true;
			LoadConnectScene.textureToShow = null;
			LoadConnectScene.sceneToLoad = Defs.MainMenuScene;
			LoadConnectScene.noteToShow = null;
			Application.LoadLevel(Defs.PromSceneName);
			this.isGoInPhotonGame = false;
		}
		if (this.customPanel != null && this.customPanel.activeSelf)
		{
			this.connectToWiFIInCreateLabel.SetActive(false);
			this.connectToWiFIInCustomLabel.SetActive(false);
			this.createRoomUIBtn.isEnabled = true;
			Defs.isInet = true;
			this.customPanel.SetActive(false);
			this.mainPanel.SetActive(true);
			this.selectMapPanel.SetActive(true);
			PhotonNetwork.Disconnect();
		}
		if (this.searchPanel != null && this.searchPanel.activeSelf)
		{
			this.searchInput.@value = this.gameNameFilter;
			this.searchPanel.SetActive(false);
			this.customPanel.SetActive(true);
		}
		if (this.createPanel != null && this.createPanel.activeSelf)
		{
			PlayerPrefs.SetString("TypeGame", "client");
			this.SetPosSelectMapPanelInMainMenu();
			this.createPanel.SetActive(false);
			this.selectMapPanel.SetActive(false);
			this.customPanel.SetActive(true);
		}
		if (this.setPasswordPanel != null && this.setPasswordPanel.activeSelf)
		{
			this.BackFromSetPasswordPanel();
		}
		if (this.enterPasswordPanel != null && this.enterPasswordPanel.activeSelf)
		{
			this.enterPasswordPanel.SetActive(false);
			this.customPanel.SetActive(true);
			ExperienceController.sharedController.isShowRanks = true;
		}
	}

	private void HandleCancelFromConnectToPhotonBtnClicked()
	{
		if (this._someWindowSubscription != null)
		{
			this._someWindowSubscription.Dispose();
		}
		if (this.failInternetLabel != null)
		{
			this.failInternetLabel.SetActive(false);
		}
		if (this.connectToPhotonPanel != null)
		{
			this.connectToPhotonPanel.SetActive(false);
		}
		if (this.actAfterConnectToPhoton == null)
		{
			PhotonNetwork.Disconnect();
		}
		else
		{
			this.actAfterConnectToPhoton = null;
		}
	}

	private void HandleCancelFromConnectToPhotonBtnClicked(object sender, EventArgs e)
	{
		this.HandleCancelFromConnectToPhotonBtnClicked();
	}

	private void HandleClearBtnClicked(object sender, EventArgs e)
	{
		if (this.searchInput != null)
		{
			this.searchInput.@value = string.Empty;
		}
	}

	private void HandleClearInSetPasswordBtnClicked(object sender, EventArgs e)
	{
		this.setPasswordInput.@value = string.Empty;
	}

	private void HandleCoinsShopClicked(object sender, EventArgs e)
	{
		this.ShowBankWindow();
	}

	private void HandleCreateRoomBtnClicked(object sender, EventArgs e)
	{
		int value;
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(this.selectMap.mapID);
		if (infoScene == null)
		{
			return;
		}
		string str = infoScene.gameObject.name;
		if (infoScene.isPremium && Storager.getInt(string.Concat(str, "Key"), true) == 0 && !PremiumAccountController.MapAvailableDueToPremiumAccount(str))
		{
			PhotonNetwork.Disconnect();
			return;
		}
		string str1 = FilterBadWorld.FilterString(this.nameServerInput.@value);
		bool flag = false;
		if (Defs.isInet)
		{
			RoomInfo[] roomList = PhotonNetwork.GetRoomList();
			int num = 0;
			while (num < (int)roomList.Length)
			{
				if (!roomList[num].name.Equals(str1))
				{
					num++;
				}
				else
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			this.nameAlreadyUsedLabel.timer = 3f;
			this.nameAlreadyUsedLabel.gameObject.SetActive(true);
			return;
		}
		this.goMapName = str;
		PlayerPrefs.SetString("MapName", this.goMapName);
		if (this.killToWin.@value.Value > this.killToWin.maxValue.Value)
		{
			this.killToWin.@value = this.killToWin.maxValue;
		}
		if (this.killToWin.@value.Value < this.killToWin.minValue.Value)
		{
			this.killToWin.@value = this.killToWin.minValue;
		}
		int value1 = this.killToWin.@value.Value;
		PlayerPrefs.SetString("MaxKill", value1.ToString());
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset((!Defs.filterMaps.ContainsKey(str) ? 0 : Defs.filterMaps[str]));
		}
		base.StartCoroutine(this.SetFonLoadingWaitForReset(this.goMapName, false));
		this.loadingMapPanel.SetActive(true);
		int num1 = (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Deathmatch || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.DeadlyGames ? this.numberOfPlayer.@value.Value : this.teamCountPlayer.@value);
		if (Defs.isDaterRegim)
		{
			value = this.killToWin.@value.Value;
		}
		else if (ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.DeadlyGames)
		{
			value = (!Defs.isDaterRegim ? 4 : 5);
		}
		else
		{
			value = 10;
		}
		int num2 = value;
		if (!Defs.isInet)
		{
			bool flag1 = Network.HavePublicAddress();
			Network.InitializeServer(num1 - 1, 25002, flag1);
			PlayerPrefs.SetString("ServerName", str1);
			PlayerPrefs.SetString("PlayersLimits", num1.ToString());
			Singleton<SceneLoader>.Instance.LoadSceneAsync("PromScene", LoadSceneMode.Single);
		}
		else
		{
			this.loadingMapPanel.SetActive(true);
			ActivityIndicator.IsActiveIndicator = true;
			ConnectSceneNGUIController.CreateGameRoom(str1, num1, infoScene.indexMap, num2, this.password, ConnectSceneNGUIController.regim);
		}
	}

	private void HandleCustomBtnClicked(object sender, EventArgs e)
	{
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining(string.Concat(ConnectSceneNGUIController.regim, ".Custom"), TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		this.actAfterConnectToPhoton = new Action(this.CustomBtnAct);
		PhotonNetwork.autoJoinLobby = true;
		this.ShowConnectToPhotonPanel();
	}

	public void HandleGoBtnClicked(object sender, EventArgs e)
	{
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining(string.Concat(ConnectSceneNGUIController.regim, ".Go"), TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		this.actAfterConnectToPhoton = new Action(this.GoBtnAct);
		PhotonNetwork.autoJoinLobby = false;
		this.ShowConnectToPhotonPanel();
	}

	private void HandleGoToCreateRoomBtnClicked(object sender, EventArgs e)
	{
		PlayerPrefs.SetString("TypeGame", "server");
		this.password = string.Empty;
		this.passONSprite.SetActive(false);
		this.SetPosSelectMapPanelInCreatePanel();
		this.createPanel.SetActive(true);
		this.setPasswordBtn.SetActive(Defs.isInet);
		this.selectMapPanel.SetActive(true);
		this.customPanel.SetActive(false);
		this.nameAlreadyUsedLabel.timer = -1f;
		this.nameAlreadyUsedLabel.gameObject.SetActive(false);
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Deathmatch)
		{
			this.teamCountPlayer.gameObject.SetActive(false);
			this.numberOfPlayer.gameObject.SetActive(false);
			this.numberOfPlayer.minValue.Value = 2;
			this.numberOfPlayer.maxValue.Value = 10;
			this.numberOfPlayer.@value.Value = 10;
			if (!Defs.isDaterRegim)
			{
				this.ShowKillToWinPanel(false);
				if (ExperienceController.sharedController != null)
				{
					if (ExperienceController.sharedController.currentLevel > 2)
					{
						this.killToWin.minValue.Value = 3;
						this.killToWin.maxValue.Value = 7;
						this.killToWin.@value.Value = 3;
						this.killToWin.stepValue = 2;
					}
					else
					{
						this.killToWin.minValue.Value = 3;
						this.killToWin.maxValue.Value = 7;
						this.killToWin.@value.Value = 3;
						this.killToWin.stepValue = 2;
					}
				}
			}
			else
			{
				this.ShowKillToWinPanel(true);
				this.killToWin.minValue.Value = this.daterMinValue;
				this.killToWin.maxValue.Value = this.daterMaxValue;
				this.killToWin.@value.Value = this.daterMinValue;
				this.killToWin.stepValue = this.daterStep;
			}
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle)
		{
			this.teamCountPlayer.gameObject.SetActive(false);
			this.numberOfPlayer.gameObject.SetActive(false);
			this.numberOfPlayer.minValue.Value = 2;
			this.numberOfPlayer.maxValue.Value = 4;
			this.numberOfPlayer.@value.Value = 4;
			this.ShowKillToWinPanel(false);
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight)
		{
			this.teamCountPlayer.gameObject.SetActive(false);
			this.teamCountPlayer.SetValue(10);
			this.numberOfPlayer.gameObject.SetActive(false);
			this.ShowKillToWinPanel(false);
			this.killToWin.stepValue = 2;
			if (ExperienceController.sharedController != null)
			{
				if (ExperienceController.sharedController.currentLevel > 2)
				{
					this.killToWin.minValue.Value = 3;
					this.killToWin.maxValue.Value = 7;
					this.killToWin.@value.Value = 3;
				}
				else
				{
					this.killToWin.minValue.Value = 3;
					this.killToWin.maxValue.Value = 7;
					this.killToWin.@value.Value = 3;
				}
			}
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
		{
			this.teamCountPlayer.gameObject.SetActive(false);
			this.teamCountPlayer.SetValue(10);
			this.numberOfPlayer.gameObject.SetActive(false);
			this.ShowKillToWinPanel(false);
			this.killToWin.stepValue = 2;
			if (ExperienceController.sharedController != null)
			{
				if (ExperienceController.sharedController.currentLevel > 2)
				{
					this.killToWin.minValue.Value = 3;
					this.killToWin.maxValue.Value = 7;
					this.killToWin.@value.Value = 3;
				}
				else
				{
					this.killToWin.minValue.Value = 3;
					this.killToWin.maxValue.Value = 7;
					this.killToWin.@value.Value = 3;
				}
			}
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
		{
			this.teamCountPlayer.gameObject.SetActive(false);
			this.teamCountPlayer.SetValue(10);
			this.numberOfPlayer.gameObject.SetActive(false);
			this.ShowKillToWinPanel(false);
			this.killToWin.stepValue = 2;
			if (ExperienceController.sharedController != null)
			{
				if (ExperienceController.sharedController.currentLevel > 2)
				{
					this.killToWin.minValue.Value = 3;
					this.killToWin.maxValue.Value = 7;
					this.killToWin.@value.Value = 3;
				}
				else
				{
					this.killToWin.minValue.Value = 3;
					this.killToWin.maxValue.Value = 7;
					this.killToWin.@value.Value = 3;
				}
			}
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.DeadlyGames)
		{
			this.teamCountPlayer.gameObject.SetActive(false);
			this.numberOfPlayer.gameObject.SetActive(false);
			this.numberOfPlayer.minValue.Value = 3;
			this.numberOfPlayer.maxValue.Value = 8;
			this.numberOfPlayer.@value.Value = 6;
			this.ShowKillToWinPanel(false);
			this.killToWin.stepValue = 5;
			if (ExperienceController.sharedController != null)
			{
				if (ExperienceController.sharedController.currentLevel > 2)
				{
					this.killToWin.minValue.Value = 5;
					this.killToWin.maxValue.Value = 10;
					this.killToWin.@value.Value = 10;
				}
				else
				{
					this.killToWin.minValue.Value = 5;
					this.killToWin.maxValue.Value = 10;
					this.killToWin.@value.Value = 10;
				}
			}
		}
	}

	private void HandleJoinRoomFromEnterPasswordBtnClicked(object sender, EventArgs e)
	{
		if (!this.enterPasswordInput.@value.Equals(this.joinRoomInfoFromCustom.customProperties[ConnectSceneNGUIController.passwordProperty].ToString()))
		{
			this.enterPasswordPanel.SetActive(false);
			ExperienceController.sharedController.isShowRanks = true;
			this.customPanel.SetActive(true);
			base.Invoke("UpdateFilteredRoomListInvoke", 0.03f);
		}
		else
		{
			this.JoinToRoomPhotonAfterCheck();
		}
	}

	private void HandleLocalBtnClicked(object sender, EventArgs e)
	{
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining(string.Concat(ConnectSceneNGUIController.regim, ".Local"), TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		Defs.isInet = false;
		this.UpdateLocalServersList();
		this.CustomBtnAct();
		this.wrapGames.SortAlphabetically();
		this.scrollGames.enabled = true;
		this.scrollGames.ResetPosition();
	}

	private void HandleRandomBtnClicked(object sender, EventArgs e)
	{
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining(string.Concat(ConnectSceneNGUIController.regim, ".Random"), TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		this.actAfterConnectToPhoton = new Action(this.RandomBtnAct);
		PhotonNetwork.autoJoinLobby = false;
		this.ShowConnectToPhotonPanel();
	}

	public void HandleResumeFromShop()
	{
		ShopNGUIController.GuiActive = false;
		ShopNGUIController.sharedShop.resumeAction = () => {
		};
		base.StartCoroutine(MainMenuController.ShowRanks());
	}

	private void HandleSearchBtnClicked(object sender, EventArgs e)
	{
		this.customPanel.SetActive(true);
		if (this.searchInput != null)
		{
			this.gameNameFilter = this.searchInput.@value;
		}
		this.updateFilteredRoomList(this.gameNameFilter);
		this.searchPanel.SetActive(false);
		this.wrapGames.SortAlphabetically();
		this.scrollGames.ResetPosition();
	}

	private void HandleSetPasswordBtnClicked(object sender, EventArgs e)
	{
		this.createPanel.SetActive(false);
		this.selectMapPanel.SetActive(false);
		this.setPasswordInput.@value = this.password;
		this.setPasswordPanel.SetActive(true);
	}

	public void HandleShopClicked()
	{
		if (ShopNGUIController.GuiActive)
		{
			return;
		}
		if (MainMenuController.IsLevelUpOrBannerShown() || this.connectToPhotonPanel != null && this.connectToPhotonPanel.activeInHierarchy)
		{
			return;
		}
		ShopNGUIController.sharedShop.SetInGame(false);
		ShopNGUIController.GuiActive = true;
		ShopNGUIController.sharedShop.resumeAction = new Action(this.HandleResumeFromShop);
	}

	private void HandleShowSearchPanelBtnClicked(object sender, EventArgs e)
	{
		this.customPanel.SetActive(false);
		if (this.searchInput != null)
		{
			this.searchInput.@value = this.gameNameFilter;
		}
		this.searchPanel.SetActive(true);
	}

	private void HandleUnlockBtnClicked(object sender, EventArgs e)
	{
		int captureFlagPrice = 0;
		string empty = string.Empty;
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
		{
			captureFlagPrice = Defs.CaptureFlagPrice;
			empty = Defs.CaptureFlagPurchasedKey;
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.DeadlyGames)
		{
			captureFlagPrice = Defs.HungerGamesPrice;
			empty = Defs.hungerGamesPurchasedKey;
		}
		new Action(() => {
			coinsShop.thisScript.notEnoughCurrency = null;
			coinsShop.thisScript.onReturnAction = null;
			int num = Storager.getInt("Coins", false) - captureFlagPrice;
			if (num < 0)
			{
				FlurryPluginWrapper.LogEvent("Try_Enable_CaptureFlag");
				StoreKitEventListener.State.PurchaseKey = "Mode opened";
				if (BankController.Instance == null)
				{
					UnityEngine.Debug.LogWarning("BankController.Instance == null");
				}
				else
				{
					EventHandler instance = null;
					instance = (object sender_, EventArgs e_) => {
						BankController.Instance.BackRequested -= this.handleBackFromBank;
						this.mainPanel.transform.root.gameObject.SetActive(true);
						coinsShop.thisScript.notEnoughCurrency = null;
						BankController.Instance.InterfaceEnabled = false;
					};
					BankController.Instance.BackRequested += instance;
					this.mainPanel.transform.root.gameObject.SetActive(false);
					coinsShop.thisScript.notEnoughCurrency = "Coins";
					BankController.Instance.InterfaceEnabled = true;
				}
			}
			else
			{
				FlurryPluginWrapper.LogEvent("Enable_Flags");
				Storager.setInt(empty, 1, true);
				Storager.setInt("Coins", num, false);
				ShopNGUIController.SpendBoughtCurrency("Coins", captureFlagPrice);
				if (Application.platform != RuntimePlatform.IPhonePlayer)
				{
					PlayerPrefs.Save();
				}
				ShopNGUIController.SynchronizeAndroidPurchases("Mode enabled");
				if (coinsPlashka.thisScript != null)
				{
					coinsPlashka.thisScript.enabled = false;
				}
				if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
				{
					this.SetUnLockedButton(this.flagCaptureToogle);
				}
				if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.DeadlyGames)
				{
					this.SetUnLockedButton(this.deadlyGamesToogle);
				}
				this.unlockBtn.SetActive(false);
				this.customBtn.SetActive(true);
				this.randomBtn.SetActive(true);
				this.conditionLabel.gameObject.SetActive(false);
				this.goBtn.SetActive(true);
			}
		})();
	}

	private void HandleUnlockMapBtnClicked(object sender, EventArgs e)
	{
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(this.selectMap.mapID);
		if (infoScene == null)
		{
			return;
		}
		int item = Defs.PremiumMaps[infoScene.NameScene];
		new Action(() => {
			coinsShop.thisScript.notEnoughCurrency = null;
			coinsShop.thisScript.onReturnAction = null;
			int num = Storager.getInt("Coins", false) - item;
			string nameScene = infoScene.NameScene;
			if (num < 0)
			{
				StoreKitEventListener.State.PurchaseKey = "In map selection";
				FlurryPluginWrapper.LogEvent(string.Concat("Try_Enable ", nameScene, " map"));
				if (BankController.Instance == null)
				{
					UnityEngine.Debug.LogWarning("BankController.Instance == null");
				}
				else
				{
					EventHandler instance = null;
					instance = (object sender_, EventArgs e_) => {
						BankController.Instance.BackRequested -= this.handleBackFromBank;
						this.mainPanel.transform.root.gameObject.SetActive(true);
						coinsShop.thisScript.notEnoughCurrency = null;
						BankController.Instance.InterfaceEnabled = false;
					};
					BankController.Instance.BackRequested += instance;
					this.mainPanel.transform.root.gameObject.SetActive(false);
					coinsShop.thisScript.notEnoughCurrency = "Coins";
					BankController.Instance.InterfaceEnabled = true;
				}
			}
			else
			{
				this.LogBuyMap(nameScene);
				AnalyticsFacade.InAppPurchase(nameScene, "Premium Maps", 1, item, "Coins");
				Storager.setInt(string.Concat(nameScene, "Key"), 1, true);
				this.selectMap.mapPreviewTexture.mainTexture = this.mapPreview[nameScene];
				Storager.setInt("Coins", num, false);
				ShopNGUIController.SpendBoughtCurrency("Coins", item);
				if (Application.platform != RuntimePlatform.IPhonePlayer)
				{
					PlayerPrefs.Save();
				}
				ShopNGUIController.SynchronizeAndroidPurchases(string.Concat("Map unlocked from connect scene: ", nameScene));
				if (coinsPlashka.thisScript != null)
				{
					coinsPlashka.thisScript.enabled = false;
				}
			}
		})();
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

	private bool IsUseMap(int indMap)
	{
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(ConnectSceneNGUIController.curSelectMode, indMap);
		if (infoScene == null)
		{
			return false;
		}
		return (!infoScene.isPremium || Storager.getInt(string.Concat(infoScene.NameScene, "Key"), true) != 0 ? true : PremiumAccountController.MapAvailableDueToPremiumAccount(infoScene.NameScene));
	}

	public static void JoinRandomGameRoom(int mapIndex, ConnectSceneNGUIController.RegimGame gameMode, int joinToNewRound, bool abTestSeparate = false)
	{
		string str;
		string empty = string.Empty;
		if (Defs.useSqlLobby)
		{
			if (mapIndex != -1)
			{
				empty = string.Concat(ConnectSceneNGUIController.mapProperty, " = ", mapIndex);
			}
			else
			{
				TypeModeGame typeModeGame = (TypeModeGame)((int)Enum.Parse(typeof(TypeModeGame), gameMode.ToString()));
				int[] array = (
					from m in SceneInfoController.instance.GetListScenesForMode(typeModeGame).avaliableScenes
					select m.indexMap).ToArray<int>();
				empty = string.Concat(empty, "( ");
				for (int i = 0; i < (int)array.Length; i++)
				{
					str = empty;
					empty = string.Concat(new object[] { str, ConnectSceneNGUIController.mapProperty, " = ", array[i] });
					if (i + 1 < (int)array.Length)
					{
						empty = string.Concat(empty, " OR ");
					}
				}
				empty = string.Concat(empty, " )");
			}
			empty = string.Concat(empty, " AND ", ConnectSceneNGUIController.passwordProperty, " = \"\"");
			if (!Defs.isDaterRegim)
			{
				str = empty;
				empty = string.Concat(new string[] { str, " AND ", ConnectSceneNGUIController.platformProperty, " = ", ConnectSceneNGUIController.myPlatformConnect.ToString() });
			}
			int num = joinToNewRound;
			if (num == 0)
			{
				empty = string.Concat(empty, " AND ", ConnectSceneNGUIController.endingProperty, " = 0");
			}
			else if (num == 1)
			{
				empty = string.Concat(empty, " AND ", ConnectSceneNGUIController.endingProperty, " = 2");
			}
			if (ExpController.Instance.OurTier == 0)
			{
				empty = (!Defs.isABTestBalansCohortActual ? string.Concat(empty, " AND ", ConnectSceneNGUIController.ABTestProperty, " = 0") : string.Concat(empty, " AND ", ConnectSceneNGUIController.ABTestProperty, " = 1"));
			}
			if (Defs.isActivABTestBuffSystem || Defs.isActivABTestRatingSystem)
			{
				empty = string.Concat(empty, " AND ", ConnectSceneNGUIController.ABTestEnum, " = ");
				if (abTestSeparate)
				{
					if (Defs.isActivABTestRatingSystem)
					{
						empty = string.Concat(empty, (!FriendsController.isUseRatingSystem ? 1 : 2));
					}
					else if (Defs.isActivABTestBuffSystem)
					{
						empty = string.Concat(empty, (!FriendsController.useBuffSystem ? 1 : 3));
					}
				}
				else if (Defs.isActivABTestRatingSystem)
				{
					empty = string.Concat(empty, (!FriendsController.isUseRatingSystem ? 2 : 1));
				}
				else if (Defs.isActivABTestBuffSystem)
				{
					empty = string.Concat(empty, (!FriendsController.useBuffSystem ? 3 : 1));
				}
			}
		}
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[ConnectSceneNGUIController.passwordProperty] = string.Empty;
		if (!Defs.useSqlLobby)
		{
			hashtable[ConnectSceneNGUIController.mapProperty] = mapIndex;
		}
		if (!Defs.isDaterRegim && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.DeadlyGames && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.TimeBattle)
		{
			hashtable[ConnectSceneNGUIController.maxKillProperty] = 3;
		}
		if (joinToNewRound == 0)
		{
			hashtable[ConnectSceneNGUIController.endingProperty] = 0;
		}
		if (!Defs.isDaterRegim)
		{
			hashtable[ConnectSceneNGUIController.platformProperty] = (int)ConnectSceneNGUIController.myPlatformConnect;
		}
		if (ExpController.Instance.OurTier == 0)
		{
			if (!Defs.isABTestBalansCohortActual)
			{
				hashtable[ConnectSceneNGUIController.ABTestProperty] = 0;
			}
			else
			{
				hashtable[ConnectSceneNGUIController.ABTestProperty] = 1;
			}
		}
		PlayerPrefs.SetString("TypeGame", "client");
		if (!Defs.useSqlLobby)
		{
			PhotonNetwork.JoinRandomRoom(hashtable, 0);
		}
		else
		{
			TypedLobby typedLobby = new TypedLobby("PixelGun3D", LobbyType.SqlLobby);
			UnityEngine.Debug.Log(empty);
			PhotonNetwork.JoinRandomRoom(hashtable, 0, MatchmakingMode.FillRoom, typedLobby, empty, null);
		}
		FlurryPluginWrapper.LogMultiplayerWayStart();
	}

	private void JoinRandomRoom(int mapIndex, ConnectSceneNGUIController.RegimGame gameMode)
	{
		this.joinNewRoundTries = 0;
		this.tryJoinRoundMap = mapIndex;
		if (mapIndex != -1)
		{
			SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(mapIndex);
			if (infoScene == null)
			{
				UnityEngine.Debug.LogError("scInfo == null");
				return;
			}
			FlurryPluginWrapper.LogEnteringMap(0, infoScene.NameScene);
			this.goMapName = infoScene.NameScene;
		}
		else if (Defs.useSqlLobby)
		{
			this.goMapName = string.Empty;
		}
		else
		{
			mapIndex = this.GetRandomMapIndex();
			if (mapIndex == -1)
			{
				return;
			}
			SceneInfo sceneInfo = SceneInfoController.instance.GetInfoScene(mapIndex);
			if (sceneInfo == null)
			{
				UnityEngine.Debug.LogError("scInfo == null");
				return;
			}
			FlurryPluginWrapper.LogEnteringMap(0, sceneInfo.NameScene);
			this.goMapName = sceneInfo.NameScene;
		}
		if (!string.IsNullOrEmpty(this.goMapName))
		{
			if (WeaponManager.sharedManager != null)
			{
				WeaponManager.sharedManager.Reset((!Defs.filterMaps.ContainsKey(this.goMapName) ? 0 : Defs.filterMaps[this.goMapName]));
			}
			base.StartCoroutine(this.SetFonLoadingWaitForReset(this.goMapName, false));
			this.loadingMapPanel.SetActive(true);
			ActivityIndicator.IsActiveIndicator = true;
		}
		ConnectSceneNGUIController.JoinRandomGameRoom(mapIndex, gameMode, this.joinNewRoundTries, this.abTestConnect);
	}

	public void JoinToLocalRoom(LANBroadcastService.ReceivedMessage _roomInfo)
	{
		if (_roomInfo.connectedPlayers == _roomInfo.playerLimit)
		{
			this.gameIsfullLabel.timer = 3f;
			this.gameIsfullLabel.gameObject.SetActive(true);
			this.incorrectPasswordLabel.timer = -1f;
			this.incorrectPasswordLabel.gameObject.SetActive(false);
			return;
		}
		GlobalGameController.countKillsBlue = 0;
		GlobalGameController.countKillsRed = 0;
		Defs.ServerIp = _roomInfo.ipAddress;
		PlayerPrefs.SetString("MaxKill", _roomInfo.comment);
		PlayerPrefs.SetString("MapName", _roomInfo.map);
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset((!Defs.filterMaps.ContainsKey(_roomInfo.map) ? 0 : Defs.filterMaps[_roomInfo.map]));
		}
		base.StartCoroutine(this.SetFonLoadingWaitForReset(_roomInfo.map, false));
		base.Invoke("goGame", 0.1f);
	}

	public void JoinToRoomPhoton(RoomInfo _roomInfo)
	{
		if (_roomInfo.playerCount == _roomInfo.maxPlayers)
		{
			this.gameIsfullLabel.timer = 3f;
			this.gameIsfullLabel.gameObject.SetActive(true);
			this.incorrectPasswordLabel.timer = -1f;
			this.incorrectPasswordLabel.gameObject.SetActive(false);
			return;
		}
		this.joinRoomInfoFromCustom = _roomInfo;
		if (!string.IsNullOrEmpty(_roomInfo.customProperties[ConnectSceneNGUIController.passwordProperty].ToString()))
		{
			this.gameIsfullLabel.timer = -1f;
			this.gameIsfullLabel.gameObject.SetActive(false);
			this.incorrectPasswordLabel.timer = 3f;
			this.incorrectPasswordLabel.gameObject.SetActive(true);
			this.enterPasswordInput.@value = string.Empty;
			this.enterPasswordPanel.SetActive(true);
			this.enterPasswordInput.isSelected = false;
			this.enterPasswordInput.isSelected = true;
			ExperienceController.sharedController.isShowRanks = false;
			this.customPanel.SetActive(false);
		}
		else
		{
			this.JoinToRoomPhotonAfterCheck();
		}
	}

	public void JoinToRoomPhotonAfterCheck()
	{
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(this.joinRoomInfoFromCustom.customProperties[ConnectSceneNGUIController.mapProperty].ToString()));
		base.StartCoroutine(this.SetFonLoadingWaitForReset(infoScene.NameScene, false));
		this.loadingMapPanel.SetActive(true);
		PhotonNetwork.JoinRoom(this.joinRoomInfoFromCustom.name);
		ActivityIndicator.IsActiveIndicator = true;
	}

	[DebuggerHidden]
	private IEnumerator LoadMapPreview()
	{
		ConnectSceneNGUIController.u003cLoadMapPreviewu003ec__Iterator1E variable = null;
		return variable;
	}

	public static void Local()
	{
		if (EveryplayWrapper.Instance.CurrentState == EveryplayWrapper.State.Paused || EveryplayWrapper.Instance.CurrentState == EveryplayWrapper.State.Recording)
		{
			EveryplayWrapper.Instance.Stop();
		}
		PhotonNetwork.Disconnect();
		if (Defs.isGameFromFriends)
		{
			ConnectSceneNGUIController.GoToFriends();
		}
		else if (!Defs.isGameFromClans)
		{
			LoadConnectScene.textureToShow = null;
			if (Defs.isDaterRegim)
			{
				LoadConnectScene.sceneToLoad = "ConnectSceneSandbox";
			}
			else
			{
				LoadConnectScene.sceneToLoad = "ConnectScene";
			}
			LoadConnectScene.noteToShow = null;
			SceneManager.LoadScene(Defs.PromSceneName);
		}
		else
		{
			ConnectSceneNGUIController.GoToClans();
		}
	}

	private int LocalServerComparison(LANBroadcastService.ReceivedMessage msg1, LANBroadcastService.ReceivedMessage msg2)
	{
		return msg1.ipAddress.CompareTo(msg2.ipAddress);
	}

	private void LogBuyMap(string context)
	{
		try
		{
			AnalyticsStuff.LogSales(context, "Premium Maps", false);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("LogBuyMap exception: ", exception));
		}
		Dictionary<string, string> strs = new Dictionary<string, string>()
		{
			{ "Premium Maps", context }
		};
		if (ExperienceController.sharedController != null)
		{
			strs.Add("Level", ExperienceController.sharedController.currentLevel.ToString());
		}
		if (ExpController.Instance != null)
		{
			strs.Add("Tier", ExpController.Instance.OurTier.ToString());
		}
		FlurryPluginWrapper.LogEventAndDublicateToConsole(string.Concat("Purchases Premium Maps ", FlurryPluginWrapper.GetPayingSuffixNo10()), strs, true);
	}

	private void LogIsShowAdvert(string context, bool isShow)
	{
		Dictionary<string, string> strs = new Dictionary<string, string>()
		{
			{ "Context", context },
			{ "Show", isShow.ToString() }
		};
		if (ExperienceController.sharedController != null)
		{
			strs.Add("Level", ExperienceController.sharedController.currentLevel.ToString());
		}
		if (ExpController.Instance != null)
		{
			strs.Add("Tier", ExpController.Instance.OurTier.ToString());
		}
		FlurryPluginWrapper.LogEventAndDublicateToConsole("Advert show", strs, true);
	}

	private void LogUserInterstitialRequest()
	{
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Round {0}", this._countOfLoopsRequestAdThisTime + 1);
			stringBuilder.AppendFormat(", Slot {0} ({1})", InterstitialManager.Instance.ProviderClampedIndex + 1, AnalyticsHelper.GetAdProviderName(InterstitialManager.Instance.Provider));
			if (InterstitialManager.Instance.Provider == AdProvider.GoogleMobileAds)
			{
				stringBuilder.AppendFormat(", Unit {0}", MobileAdManager.Instance.ImageAdUnitIndexClamped + 1);
			}
			stringBuilder.Append(" - Request");
			string str = stringBuilder.ToString();
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ "Quit - Interstitial", str },
				{ "Statistics - Interstitial", str }
			};
			FlurryPluginWrapper.LogEventAndDublicateToConsole("ADS Statistics Total", strs, true);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
	}

	private void LogUserQuit()
	{
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Round {0}", this._countOfLoopsRequestAdThisTime + 1);
			stringBuilder.AppendFormat(", Slot {0} ({1})", InterstitialManager.Instance.ProviderClampedIndex + 1, AnalyticsHelper.GetAdProviderName(InterstitialManager.Instance.Provider));
			if (InterstitialManager.Instance.Provider == AdProvider.GoogleMobileAds)
			{
				stringBuilder.AppendFormat(", Unit {0}", MobileAdManager.Instance.ImageAdUnitIndexClamped + 1);
			}
			stringBuilder.Append(" - User quit");
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ "Quit - Interstitial", stringBuilder.ToString() }
			};
			FlurryPluginWrapper.LogEventAndDublicateToConsole("ADS Statistics Total", strs, true);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
	}

	public static string MainLoadingTexture()
	{
		return (!Device.isRetinaAndStrong ? "main_loading" : "main_loading_Hi");
	}

	[DebuggerHidden]
	private IEnumerator MoveToGameScene(string _goMapName)
	{
		ConnectSceneNGUIController.u003cMoveToGameSceneu003ec__Iterator21 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator OnApplicationPause(bool pausing)
	{
		ConnectSceneNGUIController.u003cOnApplicationPauseu003ec__Iterator1B variable = null;
		return variable;
	}

	public void OnConnectedToMaster()
	{
		UnityEngine.Debug.Log("OnConnectedToMaster");
		this.firstConnectToPhoton = true;
		PhotonNetwork.playerName = ProfileController.GetPlayerNameOrDefault();
		if (this.connectToPhotonPanel.activeSelf && this.actAfterConnectToPhoton != new Action(this.RandomBtnAct))
		{
			this.connectToPhotonPanel.SetActive(false);
		}
		if (this.actAfterConnectToPhoton == null)
		{
			PhotonNetwork.Disconnect();
		}
		else
		{
			this.actAfterConnectToPhoton();
			this.actAfterConnectToPhoton = null;
		}
	}

	public void OnConnectedToPhoton()
	{
		UnityEngine.Debug.Log("OnConnectedToPhoton");
	}

	private void OnCreatedRoom()
	{
		UnityEngine.Debug.Log("OnCreatedRoom");
	}

	private void OnDestroy()
	{
		UnityEngine.Debug.Log("OnDestroy ConnectSceneController");
		if (this.isStartShowAdvert)
		{
			this.LogIsShowAdvert("Connect Scene", false);
		}
		this.LogUserQuit();
		if (!Defs.isInet || !this.isGoInPhotonGame && PhotonNetwork.connectionState == ConnectionState.Connected || PhotonNetwork.connectionState == ConnectionState.Connecting)
		{
			PhotonNetwork.Disconnect();
			UnityEngine.Debug.Log("PhotonNetwork.Disconnect()");
		}
		if (ExperienceController.sharedController != null)
		{
			ExperienceController.sharedController.isShowRanks = false;
			ExperienceController.sharedController.isMenu = false;
			ExperienceController.sharedController.isConnectScene = false;
		}
		this.lanScan.StopBroadCasting();
		ConnectSceneNGUIController.sharedController = null;
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	private void OnDisconnectedFromPhoton()
	{
		UnityEngine.Debug.Log("OnDisconnectedFromPhoton");
		if ((!this.mainPanel.activeSelf || this.loadingMapPanel.activeSelf) && this.firstConnectToPhoton && Defs.isInet)
		{
			this.mainPanel.SetActive(true);
			this.selectMapPanel.SetActive(true);
			this.createPanel.SetActive(false);
			this.customPanel.SetActive(false);
			this.searchPanel.SetActive(false);
			this.setPasswordPanel.SetActive(false);
			this.enterPasswordPanel.SetActive(false);
			ExperienceController.sharedController.isShowRanks = true;
			this.loadingMapPanel.SetActive(false);
			this.SetPosSelectMapPanelInMainMenu();
			this.serverIsNotAvalible.timer = 3f;
			this.serverIsNotAvalible.gameObject.SetActive(true);
			UICamera.selectedObject = null;
			ConnectSceneNGUIController.RegimGame regimGame = ConnectSceneNGUIController.regim;
			ConnectSceneNGUIController.ResetWeaponManagerForDeathmatch();
			this.SetRegim(regimGame);
		}
		if (this.actAfterConnectToPhoton != null)
		{
			base.Invoke("ConnectToPhoton", 0.5f);
		}
		if (this.connectToPhotonPanel.activeSelf)
		{
			this.failInternetLabel.SetActive(true);
		}
	}

	private void OnEnable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(() => {
			if (!(BannerWindowController.SharedController != null) || !BannerWindowController.SharedController.IsAnyBannerShown)
			{
				this.HandleBackBtnClicked(null, EventArgs.Empty);
			}
			else
			{
				BannerWindowController.SharedController.HideBannerWindow();
			}
		}, "Connect Scene");
		this.OnEnableWhenAnimate();
	}

	private void OnEnableWhenAnimate()
	{
		if (this.animationStarted)
		{
			this.StopFingerAnim();
			this.modeAnimObj.SetActive(false);
			this.fingerStopped = false;
			base.StartCoroutine(this.AnimateModeOpen());
		}
	}

	private void OnFailedToConnectToPhoton(object parameters)
	{
		UnityEngine.Debug.Log(string.Concat("OnFailedToConnectToPhoton. StatusCode: ", parameters));
		if (this.connectToPhotonPanel.activeSelf)
		{
			this.failInternetLabel.SetActive(true);
		}
		if (!this.isCancelConnectingToPhoton)
		{
			base.Invoke("ConnectToPhoton", 1f);
		}
	}

	private void OnInitializeItem(GameObject go, int wrapInd, int realInd)
	{
		if (Defs.isInet)
		{
			this.SetRoomInfo(go.GetComponent<GameInfo>(), Mathf.Abs(realInd));
		}
		else
		{
			this.SetLocalRoomInfo(go.GetComponent<GameInfo>(), Mathf.Abs(realInd));
		}
	}

	public void OnJoinedLobby()
	{
		UnityEngine.Debug.Log(string.Concat("OnJoinedLobby: ", PhotonNetwork.lobby.Name));
		this.OnConnectedToMaster();
	}

	private void OnJoinedRoom()
	{
		AnalyticsStuff.LogMultiplayer();
		UnityEngine.Debug.Log(string.Concat("OnJoinedRoom ", PhotonNetwork.room.customProperties[ConnectSceneNGUIController.mapProperty].ToString()));
		PhotonNetwork.isMessageQueueRunning = false;
		NotificationController.ResetPaused();
		GlobalGameController.healthMyPlayer = 0f;
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(PhotonNetwork.room.customProperties[ConnectSceneNGUIController.mapProperty].ToString()));
		this.goMapName = infoScene.NameScene;
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset((!Defs.filterMaps.ContainsKey(this.goMapName) ? 0 : Defs.filterMaps[this.goMapName]));
		}
		base.StartCoroutine(this.MoveToGameScene(infoScene.NameScene));
	}

	private void OnPaswordSelected()
	{
		this.password = this.setPasswordInput.@value;
		this.BackFromSetPasswordPanel();
	}

	private void OnPhotonCreateRoomFailed()
	{
		UnityEngine.Debug.Log("OnPhotonCreateRoomFailed");
		this.nameAlreadyUsedLabel.timer = 3f;
		this.nameAlreadyUsedLabel.gameObject.SetActive(true);
		this.loadingMapPanel.SetActive(false);
		ActivityIndicator.IsActiveIndicator = false;
	}

	private void OnPhotonJoinRoomFailed()
	{
		ActivityIndicator.IsActiveIndicator = false;
		this.loadingMapPanel.SetActive(false);
		this.gameIsfullLabel.timer = 3f;
		this.gameIsfullLabel.gameObject.SetActive(true);
		this.incorrectPasswordLabel.timer = -1f;
		this.incorrectPasswordLabel.gameObject.SetActive(false);
		UnityEngine.Debug.Log("OnPhotonJoinRoomFailed");
	}

	private void OnPhotonRandomJoinFailed()
	{
		int num;
		int num1;
		UnityEngine.Debug.Log("OnPhotonJoinRoomFailed");
		if (string.IsNullOrEmpty(this.goMapName))
		{
			int randomMapIndex = this.GetRandomMapIndex();
			if (randomMapIndex == -1)
			{
				return;
			}
			SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(randomMapIndex);
			if (infoScene == null)
			{
				return;
			}
			this.goMapName = infoScene.name;
		}
		SceneInfo sceneInfo = SceneInfoController.instance.GetInfoScene(this.goMapName);
		if (sceneInfo == null)
		{
			return;
		}
		if (this.joinNewRoundTries >= 2 && this.abTestConnect)
		{
			this.abTestConnect = false;
			this.joinNewRoundTries = 0;
		}
		if (this.joinNewRoundTries < 2)
		{
			UnityEngine.Debug.Log(string.Concat("No rooms with new round: ", this.joinNewRoundTries, (!this.abTestConnect ? string.Empty : " <color=yellow>AbTestSeparate</color>")));
			this.joinNewRoundTries++;
			ConnectSceneNGUIController.JoinRandomGameRoom(this.tryJoinRoundMap, ConnectSceneNGUIController.regim, this.joinNewRoundTries, this.abTestConnect);
			return;
		}
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset((!Defs.filterMaps.ContainsKey(this.goMapName) ? 0 : Defs.filterMaps[this.goMapName]));
		}
		base.StartCoroutine(this.SetFonLoadingWaitForReset(this.goMapName, false));
		if (ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.DeadlyGames)
		{
			num = (!Defs.isDaterRegim ? 4 : 5);
		}
		else
		{
			num = 10;
		}
		int num2 = num;
		if (Defs.isCOOP)
		{
			num1 = 4;
		}
		else if (!Defs.isCompany)
		{
			num1 = (!Defs.isHunger ? 10 : 6);
		}
		else
		{
			num1 = 10;
		}
		int num3 = num1;
		ConnectSceneNGUIController.CreateGameRoom(null, num3, sceneInfo.indexMap, num2, string.Empty, ConnectSceneNGUIController.regim);
	}

	public void OnReceivedRoomListUpdate()
	{
		if (!this.customPanel.activeSelf || !Defs.isInet)
		{
			return;
		}
		this.updateFilteredRoomList(this.gameNameFilter);
	}

	public static void PhotonCreateRoom(string roomName, bool isVisible, bool isOpen, int maxPlayers, ExitGames.Client.Photon.Hashtable roomProps, string[] roomPropsInLobby)
	{
		PlayerPrefs.SetString("TypeGame", "server");
		RoomOptions roomOption = new RoomOptions()
		{
			customRoomProperties = roomProps,
			customRoomPropertiesForLobby = roomPropsInLobby
		};
		RoomOptions roomOption1 = roomOption;
		roomOption1.maxPlayers = (byte)maxPlayers;
		roomOption1.isOpen = isOpen;
		roomOption1.isVisible = isVisible;
		if (Defs.useSqlLobby)
		{
			TypedLobby typedLobby = new TypedLobby("PixelGun3D", LobbyType.SqlLobby);
			PhotonNetwork.CreateRoom(roomName, roomOption1, typedLobby);
		}
		else
		{
			PhotonNetwork.CreateRoom(roomName, roomOption1, TypedLobby.Default);
		}
	}

	private void RandomBtnAct()
	{
		this.JoinRandomRoom(-1, ConnectSceneNGUIController.regim);
	}

	private static void ResetWeaponManagerForDeathmatch()
	{
		ConnectSceneNGUIController.SetFlagsForDeathmatchRegim();
		WeaponManager.sharedManager.Reset(0);
	}

	private void SeachServer(string ipServerSeaches)
	{
		bool flag = false;
		if (this.servers.Count > 0)
		{
			foreach (ConnectSceneNGUIController.infoServer server in this.servers)
			{
				if (!server.ipAddress.Equals(ipServerSeaches))
				{
					continue;
				}
				flag = true;
			}
		}
		if (!flag)
		{
			ConnectSceneNGUIController.infoServer _infoServer = new ConnectSceneNGUIController.infoServer()
			{
				ipAddress = ipServerSeaches
			};
			this.servers.Add(_infoServer);
		}
	}

	private static void SetFlagsForDeathmatchRegim()
	{
		Defs.isMulti = true;
		Defs.isInet = true;
		Defs.isCOOP = false;
		Defs.isCompany = false;
		Defs.isHunger = false;
		Defs.isFlag = false;
		Defs.isCapturePoints = false;
		Defs.IsSurvival = false;
		StoreKitEventListener.State.Mode = "Deathmatch Wordwide";
		StoreKitEventListener.State.Parameters.Clear();
	}

	private void SetFonLoading(string _mapName = "", bool isAddCountRun = false)
	{
		this.GetMapName(_mapName, isAddCountRun);
		if (this._loadingNGUIController != null)
		{
			UnityEngine.Object.Destroy(this._loadingNGUIController.gameObject);
			this._loadingNGUIController = null;
		}
		this.ShowLoadingGUI(_mapName);
	}

	[DebuggerHidden]
	private IEnumerator SetFonLoadingWaitForReset(string _mapName = "", bool isAddCountRun = false)
	{
		ConnectSceneNGUIController.u003cSetFonLoadingWaitForResetu003ec__Iterator20 variable = null;
		return variable;
	}

	private void SetLocalRoomInfo(GameInfo _gameInfo, int index)
	{
		_gameInfo.index = index;
		if (this._copy == null || (int)this._copy.Length <= index)
		{
			_gameInfo.gameObject.SetActive(false);
		}
		else
		{
			_gameInfo.gameObject.SetActive(true);
			LANBroadcastService.ReceivedMessage receivedMessage = this._copy[index];
			string str = receivedMessage.name;
			if (string.IsNullOrEmpty(str))
			{
				str = LocalizationStore.Get("Key_0948");
			}
			_gameInfo.serverNameLabel.text = str;
			_gameInfo.countPlayersLabel.text = string.Concat(receivedMessage.connectedPlayers.ToString(), "/", receivedMessage.playerLimit.ToString());
			_gameInfo.openSprite.SetActive(true);
			_gameInfo.closeSprite.SetActive(false);
			string translateName = receivedMessage.map;
			SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(receivedMessage.map);
			if (infoScene != null)
			{
				translateName = infoScene.TranslateName;
			}
			_gameInfo.mapNameLabel.text = string.Format("{0}: {1}", LocalizationStore.Get("Key_0947"), translateName);
			_gameInfo.roomInfoLocal = receivedMessage;
		}
	}

	private void SetPosSelectMapPanelInCreatePanel()
	{
		if (Defs.isDaterRegim)
		{
			return;
		}
		this.selectMapPanelTransform.localPosition = new Vector3(0f, 35f, 0f);
		this.fonMapPreview.width = Mathf.RoundToInt((float)Screen.width * 768f / (float)Screen.height + 10f);
		this.fonMapPreview.height = 376;
		if (!Defs.isDaterRegim)
		{
			this.fonMapPreview.transform.localPosition = new Vector3(0f, -24f, 0f);
		}
		UIPanel vector4 = this.mapPreviewPanel;
		float single = (float)Screen.width * 768f / (float)Screen.height;
		Vector4 vector41 = this.mapPreviewPanel.baseClipRegion;
		vector4.baseClipRegion = new Vector4(0f, 0f, single, vector41.w);
		this.ChooseMapLabelSmall.SetActive(false);
	}

	private void SetPosSelectMapPanelInMainMenu()
	{
		if (Defs.isDaterRegim)
		{
			this.ChooseMapLabelSmall.SetActive(false);
			return;
		}
		float single = (float)Screen.width * 768f / (float)Screen.height - 322f;
		if (!Defs.isDaterRegim)
		{
			this.selectMapPanelTransform.localPosition = new Vector3(149f, 73f, 0f);
		}
		this.fonMapPreview.width = Mathf.RoundToInt(single);
		this.fonMapPreview.height = 434;
		this.fonMapPreview.transform.localPosition = Vector3.zero;
		UIPanel vector4 = this.mapPreviewPanel;
		Vector4 vector41 = this.mapPreviewPanel.baseClipRegion;
		vector4.baseClipRegion = new Vector4(0f, 0f, single, vector41.w);
		this.ChooseMapLabelSmall.SetActive(true);
	}

	private void SetRegim(ConnectSceneNGUIController.RegimGame _regim)
	{
		bool flag = true;
		bool flag1 = true;
		PlayerPrefs.SetInt("RegimMulty", (int)_regim);
		ConnectSceneNGUIController.regim = _regim;
		if (!Defs.isDaterRegim)
		{
			this.deathmatchToggle.GetComponent<UIButton>().pressedSprite = (ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.Deathmatch ? "yell_btn_n" : "green_btn_n");
			this.timeBattleToogle.GetComponent<UIButton>().pressedSprite = (ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.TimeBattle ? "yell_btn_n" : "green_btn_n");
			this.teamFightToogle.GetComponent<UIButton>().pressedSprite = (ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.TeamFight ? "yell_btn_n" : "green_btn_n");
			if (flag)
			{
				this.deadlyGamesToogle.GetComponent<UIButton>().pressedSprite = (ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.DeadlyGames ? "yell_btn_n" : "green_btn_n");
			}
			if (flag1)
			{
				this.flagCaptureToogle.GetComponent<UIButton>().pressedSprite = (ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.FlagCapture ? "yell_btn_n" : "green_btn_n");
			}
			this.unlockMapBtn.SetActive(false);
			this.unlockMapBtnInCreate.SetActive(false);
		}
		this.createRoomBtn.SetActive(true);
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Deathmatch)
		{
			ConnectSceneNGUIController.SetFlagsForDeathmatchRegim();
			if (this.unlockBtn != null)
			{
				this.unlockBtn.SetActive(false);
			}
			this.customBtn.SetActive(true);
			if (this.randomBtn != null)
			{
				this.randomBtn.SetActive(true);
			}
			if (this.conditionLabel != null)
			{
				this.conditionLabel.gameObject.SetActive(false);
			}
			this.goBtn.SetActive(true);
			this.localBtn.SetActive(BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64);
			if (!Defs.isDaterRegim)
			{
				this.rulesLabel.text = this.rulesDeadmatch;
			}
			else
			{
				this.rulesLabel.text = this.rulesDater;
			}
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle)
		{
			Defs.isMulti = true;
			Defs.isInet = true;
			Defs.isCOOP = true;
			Defs.isCompany = false;
			Defs.isHunger = false;
			Defs.isFlag = false;
			Defs.isCapturePoints = false;
			if (this.unlockBtn != null)
			{
				this.unlockBtn.SetActive(false);
			}
			this.customBtn.SetActive(true);
			this.randomBtn.SetActive(true);
			this.conditionLabel.gameObject.SetActive(false);
			this.goBtn.SetActive(true);
			StoreKitEventListener.State.Mode = "Time Survival";
			StoreKitEventListener.State.Parameters.Clear();
			this.localBtn.SetActive(false);
			this.rulesLabel.text = this.rulesTimeBattle;
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight)
		{
			Defs.isMulti = true;
			Defs.isInet = true;
			Defs.isCOOP = false;
			Defs.isCompany = true;
			Defs.isHunger = false;
			Defs.isFlag = false;
			Defs.isCapturePoints = false;
			if (this.unlockBtn != null)
			{
				this.unlockBtn.SetActive(false);
			}
			this.customBtn.SetActive(true);
			this.randomBtn.SetActive(true);
			this.conditionLabel.gameObject.SetActive(false);
			this.goBtn.SetActive(true);
			this.localBtn.SetActive(BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64);
			StoreKitEventListener.State.Mode = "Team Battle";
			StoreKitEventListener.State.Parameters.Clear();
			this.rulesLabel.text = this.rulesTeamFight;
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
		{
			Defs.isMulti = true;
			Defs.isInet = true;
			Defs.isCOOP = false;
			Defs.isCompany = false;
			Defs.isHunger = false;
			Defs.isFlag = true;
			Defs.isCapturePoints = false;
			this.localBtn.SetActive(false);
			this.rulesLabel.text = this.rulesFlagCapture;
			if (flag1)
			{
				if (this.unlockBtn != null)
				{
					this.unlockBtn.SetActive(false);
				}
				this.customBtn.SetActive(true);
				this.randomBtn.SetActive(true);
				this.conditionLabel.gameObject.SetActive(false);
				this.goBtn.SetActive(true);
			}
			else
			{
				this.priceRegimLabel.text = Defs.CaptureFlagPrice.ToString();
				if (this.unlockBtn != null)
				{
					this.unlockBtn.SetActive(true);
				}
				this.customBtn.SetActive(false);
				this.randomBtn.SetActive(false);
				this.conditionLabel.gameObject.SetActive(true);
				this.conditionLabel.text = "REACH LEVEL 4 TO OPEN";
				this.goBtn.SetActive(false);
			}
			StoreKitEventListener.State.Mode = "Flag Capture";
			StoreKitEventListener.State.Parameters.Clear();
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
		{
			Defs.isMulti = true;
			Defs.isInet = true;
			Defs.isCOOP = false;
			Defs.isCompany = false;
			Defs.isHunger = false;
			Defs.isCapturePoints = true;
			Defs.isFlag = false;
			this.localBtn.SetActive(false);
			this.rulesLabel.text = this.rulesCapturePoint;
			if (this.unlockBtn != null)
			{
				this.unlockBtn.SetActive(false);
			}
			this.customBtn.SetActive(true);
			this.randomBtn.SetActive(true);
			this.conditionLabel.gameObject.SetActive(false);
			this.goBtn.SetActive(true);
			StoreKitEventListener.State.Mode = "Capture points";
			StoreKitEventListener.State.Parameters.Clear();
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.DeadlyGames)
		{
			Defs.isMulti = true;
			Defs.isInet = true;
			Defs.isCOOP = false;
			Defs.isCompany = false;
			Defs.isHunger = true;
			Defs.isFlag = false;
			Defs.isCapturePoints = false;
			this.localBtn.SetActive(false);
			this.rulesLabel.text = this.rulesDeadlyGames;
			if (flag)
			{
				if (this.unlockBtn != null)
				{
					this.unlockBtn.SetActive(false);
				}
				this.customBtn.SetActive(true);
				this.randomBtn.SetActive(true);
				this.conditionLabel.gameObject.SetActive(false);
				this.goBtn.SetActive(true);
			}
			else
			{
				this.priceRegimLabel.text = Defs.HungerGamesPrice.ToString();
				if (this.unlockBtn != null)
				{
					this.unlockBtn.SetActive(true);
				}
				this.customBtn.SetActive(false);
				this.randomBtn.SetActive(false);
				this.conditionLabel.gameObject.SetActive(true);
				this.conditionLabel.text = "REACH LEVEL 3 TO OPEN";
				this.goBtn.SetActive(false);
			}
			Defs.IsSurvival = false;
			StoreKitEventListener.State.Mode = "Deadly Games";
			StoreKitEventListener.State.Parameters.Clear();
			if (WeaponManager.sharedManager != null)
			{
				WeaponManager.sharedManager.GetWeaponPrefabs(0);
			}
		}
		base.StartCoroutine(this.SetUseMasMap());
	}

	private void SetRegimCapturePoints(object sender, EventArgs e)
	{
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
		{
			return;
		}
		this.SetRegim(ConnectSceneNGUIController.RegimGame.CapturePoints);
	}

	private void SetRegimDeadleGames(object sender, EventArgs e)
	{
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.DeadlyGames)
		{
			return;
		}
		this.SetRegim(ConnectSceneNGUIController.RegimGame.DeadlyGames);
	}

	private void SetRegimDeathmatch(object sender, EventArgs e)
	{
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Deathmatch)
		{
			return;
		}
		this.SetRegim(ConnectSceneNGUIController.RegimGame.Deathmatch);
	}

	private void SetRegimFlagCapture(object sender, EventArgs e)
	{
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
		{
			return;
		}
		this.SetRegim(ConnectSceneNGUIController.RegimGame.FlagCapture);
	}

	private void SetRegimTeamFight(object sender, EventArgs e)
	{
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight)
		{
			return;
		}
		this.SetRegim(ConnectSceneNGUIController.RegimGame.TeamFight);
	}

	private void SetRegimTimeBattle(object sender, EventArgs e)
	{
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle)
		{
			return;
		}
		this.SetRegim(ConnectSceneNGUIController.RegimGame.TimeBattle);
	}

	private void SetRoomInfo(GameInfo _gameInfo, int index)
	{
		_gameInfo.index = index;
		if (this.filteredRoomList.Count <= index)
		{
			_gameInfo.gameObject.SetActive(false);
		}
		else
		{
			_gameInfo.gameObject.SetActive(true);
			RoomInfo item = this.filteredRoomList[index];
			string str = item.name;
			if (str.Length == 36 && str.IndexOf("-") == 8 && str.LastIndexOf("-") == 23)
			{
				str = LocalizationStore.Get("Key_0088");
			}
			_gameInfo.serverNameLabel.text = str;
			_gameInfo.countPlayersLabel.text = string.Concat(item.playerCount, "/", item.maxPlayers);
			bool flag = string.IsNullOrEmpty(item.customProperties[ConnectSceneNGUIController.passwordProperty].ToString());
			_gameInfo.openSprite.SetActive(flag);
			_gameInfo.closeSprite.SetActive(!flag);
			SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(item.customProperties[ConnectSceneNGUIController.mapProperty].ToString()));
			string str1 = string.Format("{0}: {1}", LocalizationStore.Get("Key_0947"), infoScene.TranslateName);
			_gameInfo.mapNameLabel.text = str1;
			_gameInfo.roomInfo = item;
		}
	}

	private void SetUnLockedButton(UIToggle butToogle)
	{
		UIButton component = butToogle.gameObject.GetComponent<UIButton>();
		component.normalSprite = "yell_btn";
		component.hoverSprite = "yell_btn";
		component.pressedSprite = "green_btn_n";
		butToogle.transform.FindChild("LockedSprite").gameObject.SetActive(false);
		butToogle.transform.FindChild("Checkmark").GetComponent<UISprite>().spriteName = "green_btn";
	}

	[DebuggerHidden]
	private IEnumerator SetUseMasMap()
	{
		ConnectSceneNGUIController.u003cSetUseMasMapu003ec__Iterator1F variable = null;
		return variable;
	}

	public void ShowBankWindow()
	{
		if (BankController.Instance == null)
		{
			UnityEngine.Debug.LogWarning("BankController.Instance == null");
		}
		else
		{
			EventHandler instance = null;
			instance = (object backSender, EventArgs backArgs) => {
				BankController.Instance.BackRequested -= this.backFromBankHandler;
				this.u003cu003ef__this.mainPanel.transform.root.gameObject.SetActive(true);
				BankController.Instance.InterfaceEnabled = false;
			};
			BankController.Instance.BackRequested += instance;
			this.mainPanel.transform.root.gameObject.SetActive(false);
			BankController.Instance.InterfaceEnabled = true;
		}
	}

	private void ShowConnectToPhotonPanel()
	{
		this._someWindowSubscription = BackSystem.Instance.Register(new Action(this.HandleCancelFromConnectToPhotonBtnClicked), "Connect to Photon panel");
		if (!(FriendsController.sharedController != null) || FriendsController.sharedController.Banned != 1)
		{
			this.ConnectToPhoton();
			this.connectToPhotonPanel.SetActive(true);
			return;
		}
		this.accountBlockedLabel.timer = 3f;
		this.accountBlockedLabel.gameObject.SetActive(true);
	}

	private void ShowKillToWinPanel(bool show)
	{
		if (!show)
		{
			Transform vector3 = this.numberOfPlayer.transform;
			float single = this.numberOfPlayer.transform.localPosition.y;
			Vector3 vector31 = this.numberOfPlayer.transform.localPosition;
			vector3.localPosition = new Vector3(0f, single, vector31.z);
			Transform transforms = this.teamCountPlayer.transform;
			float single1 = this.teamCountPlayer.transform.localPosition.y;
			Vector3 vector32 = this.teamCountPlayer.transform.localPosition;
			transforms.localPosition = new Vector3(0f, single1, vector32.z);
			this.killToWin.gameObject.SetActive(false);
		}
		else
		{
			Transform transforms1 = this.numberOfPlayer.transform;
			float single2 = this.posNumberOffPlayersX;
			float single3 = this.numberOfPlayer.transform.localPosition.y;
			Vector3 vector33 = this.numberOfPlayer.transform.localPosition;
			transforms1.localPosition = new Vector3(single2, single3, vector33.z);
			Transform transforms2 = this.teamCountPlayer.transform;
			float single4 = this.posNumberOffPlayersX;
			float single5 = this.teamCountPlayer.transform.localPosition.y;
			Vector3 vector34 = this.teamCountPlayer.transform.localPosition;
			transforms2.localPosition = new Vector3(single4, single5, vector34.z);
			this.killToWin.headLabel.text = LocalizationStore.Get("Key_0953");
			this.killToWin.gameObject.SetActive(true);
		}
	}

	private void ShowLoadingGUI(string _mapName)
	{
		BannerWindowController.SharedController.HideBannerWindowNoShowNext();
		this._loadingNGUIController = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("LoadingGUI")).GetComponent<LoadingNGUIController>();
		this._loadingNGUIController.SceneToLoad = _mapName;
		this._loadingNGUIController.loadingNGUITexture.mainTexture = LoadConnectScene.textureToShow;
		this._loadingNGUIController.transform.parent = this.loadingMapPanel.transform;
		this._loadingNGUIController.transform.localPosition = Vector3.zero;
		this._loadingNGUIController.Init();
	}

	private void Start()
	{
		int num;
		ConnectSceneNGUIController.RegimGame regimGame;
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted)
		{
			WeaponManager.sharedManager.SaveWeaponAsLastUsed(0);
		}
		if (FriendsController.sharedController != null)
		{
			FriendsController.sharedController.profileInfo.Clear();
		}
		Defs.isDaterRegim = SceneLoader.ActiveSceneName.Equals("ConnectSceneSandbox");
		GlobalGameController.CountKills = 0;
		GlobalGameController.Score = 0;
		WeaponManager.RefreshExpControllers();
		this.rulesDeadmatch = LocalizationStore.Key_0550;
		this.rulesTeamFight = LocalizationStore.Key_0551;
		this.rulesTimeBattle = LocalizationStore.Key_0552;
		this.rulesDeadlyGames = LocalizationStore.Key_0553;
		this.rulesFlagCapture = LocalizationStore.Key_0554;
		this.rulesCapturePoint = LocalizationStore.Get("Key_1368");
		this.rulesDater = LocalizationStore.Get("Key_1538");
		ConnectSceneNGUIController.sharedController = this;
		if (!(ExperienceController.sharedController != null) || ExperienceController.sharedController.currentLevel > 2)
		{
			num = (!(ExperienceController.sharedController != null) || ExperienceController.sharedController.currentLevel > 5 ? 2 : 1);
		}
		else
		{
			num = 0;
		}
		this.myLevelGame = num;
		this.mainPanel.SetActive(false);
		this.selectMapPanel.SetActive(false);
		this.createPanel.SetActive(false);
		this.customPanel.SetActive(false);
		this.searchPanel.SetActive(false);
		this.setPasswordPanel.SetActive(false);
		this.enterPasswordPanel.SetActive(false);
		this.StartSearchLocalServers();
		PlayerPrefs.SetString("TypeGame", "client");
		this.gameIsfullLabel.gameObject.SetActive(false);
		this.accountBlockedLabel.gameObject.SetActive(false);
		this.serverIsNotAvalible.gameObject.SetActive(false);
		this.nameAlreadyUsedLabel.gameObject.SetActive(false);
		this.incorrectPasswordLabel.gameObject.SetActive(false);
		this.unlockMapBtn.SetActive(false);
		this.unlockMapBtnInCreate.SetActive(false);
		this.unlockBtn.SetActive(false);
		string str = ConnectSceneNGUIController.MainLoadingTexture();
		this.loadingToDraw.mainTexture = Resources.Load<Texture>(str);
		this.loadingMapPanel.SetActive(true);
		this.connectToPhotonPanel.SetActive(false);
		if (PhotonNetwork.connectionState == ConnectionState.Connected)
		{
			this.firstConnectToPhoton = true;
		}
		if (!Defs.isDaterRegim)
		{
			this.ScrollTransform.GetComponent<UIPanel>().baseClipRegion = new Vector4(0f, 0f, (float)(760 * Screen.width / Screen.height), 350f);
		}
		this.SetPosSelectMapPanelInMainMenu();
		if (!TrainingController.TrainingCompleted)
		{
			regimGame = ConnectSceneNGUIController.RegimGame.TeamFight;
		}
		else if (!Defs.isDaterRegim)
		{
			regimGame = (ConnectSceneNGUIController.RegimGame)PlayerPrefs.GetInt("RegimMulty", 2);
		}
		else
		{
			regimGame = ConnectSceneNGUIController.RegimGame.Deathmatch;
		}
		ConnectSceneNGUIController.regim = regimGame;
		ConnectSceneNGUIController.directedFromQuests = false;
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(ConnectSceneNGUIController.selectedMap);
		if (infoScene != null)
		{
			if (infoScene.IsAvaliableForMode(TypeModeGame.TeamFight))
			{
				ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.TeamFight;
			}
			else if (infoScene.IsAvaliableForMode(TypeModeGame.Deathmatch))
			{
				ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.Deathmatch;
			}
			else if (infoScene.IsAvaliableForMode(TypeModeGame.FlagCapture))
			{
				ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.FlagCapture;
			}
			else if (infoScene.IsAvaliableForMode(TypeModeGame.CapturePoints))
			{
				ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.CapturePoints;
			}
			else if (infoScene.IsAvaliableForMode(TypeModeGame.DeadlyGames))
			{
				ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.DeadlyGames;
			}
			else if (infoScene.IsAvaliableForMode(TypeModeGame.TimeBattle))
			{
				ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.TimeBattle;
			}
		}
		if (!Defs.isDaterRegim)
		{
			this.deathmatchToggle.@value = ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Deathmatch;
			this.timeBattleToogle.@value = ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle;
			this.teamFightToogle.@value = ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight;
			this.deadlyGamesToogle.@value = ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.DeadlyGames;
			this.flagCaptureToogle.@value = ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture;
			this.capturePointsToogle.@value = ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints;
			this.deathmatchToggle.GetComponent<ButtonHandler>().Clicked += new EventHandler(this.SetRegimDeathmatch);
			this.timeBattleToogle.GetComponent<ButtonHandler>().Clicked += new EventHandler(this.SetRegimTimeBattle);
			this.teamFightToogle.GetComponent<ButtonHandler>().Clicked += new EventHandler(this.SetRegimTeamFight);
			this.deadlyGamesToogle.GetComponent<ButtonHandler>().Clicked += new EventHandler(this.SetRegimDeadleGames);
			this.flagCaptureToogle.GetComponent<ButtonHandler>().Clicked += new EventHandler(this.SetRegimFlagCapture);
			this.capturePointsToogle.GetComponent<ButtonHandler>().Clicked += new EventHandler(this.SetRegimCapturePoints);
		}
		base.StartCoroutine(this.LoadMapPreview());
		if (this.localBtn != null)
		{
			ButtonHandler component = this.localBtn.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += new EventHandler(this.HandleLocalBtnClicked);
			}
		}
		if (this.customBtn != null)
		{
			ButtonHandler buttonHandler = this.customBtn.GetComponent<ButtonHandler>();
			if (buttonHandler != null)
			{
				buttonHandler.Clicked += new EventHandler(this.HandleCustomBtnClicked);
			}
		}
		if (this.randomBtn != null)
		{
			ButtonHandler component1 = this.randomBtn.GetComponent<ButtonHandler>();
			if (component1 != null)
			{
				component1.Clicked += new EventHandler(this.HandleRandomBtnClicked);
			}
		}
		if (this.goBtn != null)
		{
			ButtonHandler buttonHandler1 = this.goBtn.GetComponent<ButtonHandler>();
			if (buttonHandler1 != null)
			{
				buttonHandler1.Clicked += new EventHandler(this.HandleGoBtnClicked);
			}
		}
		if (this.backBtn != null)
		{
			ButtonHandler component2 = this.backBtn.GetComponent<ButtonHandler>();
			if (component2 != null)
			{
				component2.Clicked += new EventHandler(this.HandleBackBtnClicked);
			}
		}
		if (this.unlockBtn != null)
		{
			ButtonHandler buttonHandler2 = this.unlockBtn.GetComponent<ButtonHandler>();
			if (buttonHandler2 != null)
			{
				buttonHandler2.Clicked += new EventHandler(this.HandleUnlockBtnClicked);
			}
		}
		if (this.unlockMapBtn != null)
		{
			ButtonHandler component3 = this.unlockMapBtn.GetComponent<ButtonHandler>();
			if (component3 != null)
			{
				component3.Clicked += new EventHandler(this.HandleUnlockMapBtnClicked);
			}
		}
		if (this.unlockMapBtnInCreate != null)
		{
			ButtonHandler buttonHandler3 = this.unlockMapBtnInCreate.GetComponent<ButtonHandler>();
			if (buttonHandler3 != null)
			{
				buttonHandler3.Clicked += new EventHandler(this.HandleUnlockMapBtnClicked);
			}
		}
		if (this.cancelFromConnectToPhotonBtn != null)
		{
			ButtonHandler component4 = this.cancelFromConnectToPhotonBtn.GetComponent<ButtonHandler>();
			if (component4 != null)
			{
				component4.Clicked += new EventHandler(this.HandleCancelFromConnectToPhotonBtnClicked);
			}
		}
		if (this.clearBtn != null)
		{
			ButtonHandler buttonHandler4 = this.clearBtn.GetComponent<ButtonHandler>();
			if (buttonHandler4 != null)
			{
				buttonHandler4.Clicked += new EventHandler(this.HandleClearBtnClicked);
			}
		}
		if (this.searchBtn != null)
		{
			ButtonHandler component5 = this.searchBtn.GetComponent<ButtonHandler>();
			if (component5 != null)
			{
				component5.Clicked += new EventHandler(this.HandleSearchBtnClicked);
			}
		}
		if (this.showSearchPanelBtn != null)
		{
			ButtonHandler buttonHandler5 = this.showSearchPanelBtn.GetComponent<ButtonHandler>();
			if (buttonHandler5 != null)
			{
				buttonHandler5.Clicked += new EventHandler(this.HandleShowSearchPanelBtnClicked);
			}
		}
		if (this.goToCreateRoomBtn != null)
		{
			ButtonHandler component6 = this.goToCreateRoomBtn.GetComponent<ButtonHandler>();
			if (component6 != null)
			{
				component6.Clicked += new EventHandler(this.HandleGoToCreateRoomBtnClicked);
			}
		}
		if (this.createRoomBtn != null)
		{
			this.createRoomUIBtn = this.createRoomBtn.GetComponent<UIButton>();
			ButtonHandler buttonHandler6 = this.createRoomBtn.GetComponent<ButtonHandler>();
			if (buttonHandler6 != null)
			{
				buttonHandler6.Clicked += new EventHandler(this.HandleCreateRoomBtnClicked);
			}
		}
		if (this.setPasswordBtn != null)
		{
			ButtonHandler component7 = this.setPasswordBtn.GetComponent<ButtonHandler>();
			if (component7 != null)
			{
				component7.Clicked += new EventHandler(this.HandleSetPasswordBtnClicked);
			}
		}
		if (this.clearInSetPasswordBtn != null)
		{
			ButtonHandler buttonHandler7 = this.clearInSetPasswordBtn.GetComponent<ButtonHandler>();
			if (buttonHandler7 != null)
			{
				buttonHandler7.Clicked += new EventHandler(this.HandleClearInSetPasswordBtnClicked);
			}
		}
		if (this.okInsetPasswordBtn != null)
		{
			ButtonHandler component8 = this.okInsetPasswordBtn.GetComponent<ButtonHandler>();
			if (component8 != null)
			{
				component8.Clicked += new EventHandler((object sender, EventArgs e) => this.OnPaswordSelected());
			}
		}
		if (this.joinRoomFromEnterPasswordBtn != null)
		{
			ButtonHandler buttonHandler8 = this.joinRoomFromEnterPasswordBtn.GetComponent<ButtonHandler>();
			if (buttonHandler8 != null)
			{
				buttonHandler8.Clicked += new EventHandler(this.HandleJoinRoomFromEnterPasswordBtnClicked);
			}
		}
		if (!Defs.isDaterRegim)
		{
			if (true)
			{
				this.SetUnLockedButton(this.flagCaptureToogle);
			}
			if (true)
			{
				this.SetUnLockedButton(this.deadlyGamesToogle);
			}
		}
		this.InitializeBannerWindow();
		InterstitialManager.Instance.ResetAdProvider();
		if (!ConnectSceneNGUIController.NeedShowReviewInConnectScene)
		{
			if (!ReplaceAdmobPerelivController.ReplaceAdmobWithPerelivApplicable() || !ConnectSceneNGUIController.ReplaceAdmobWithPerelivRequest)
			{
				UnityEngine.Debug.LogFormat("{0}, Start(), InterstitialRequest: {1}", new object[] { base.GetType().Name, ConnectSceneNGUIController.InterstitialRequest });
				if (MobileAdManager.AdIsApplicable(MobileAdManager.Type.Image, true) && ConnectSceneNGUIController.InterstitialRequest)
				{
					this.isStartShowAdvert = true;
					base.StartCoroutine(this.WaitLoadingAndShowInterstitialCoroutine("Connect Scene", false));
				}
			}
			else
			{
				ConnectSceneNGUIController.ReplaceAdmobWithPerelivRequest = false;
				base.StartCoroutine(this.WaitLoadingAndShowReplaceAdmobPereliv("Connect Scene", false));
			}
		}
		this.wrapGames.onInitializeItem = new UIWrapContent.OnInitializeItem(this.OnInitializeItem);
		this.enterPasswordInput.onSubmit.Add(new EventDelegate(new EventDelegate.Callback(this.EnterPassInputSubmit)));
	}

	private void StartSearchLocalServers()
	{
		this.lanScan.StartSearchBroadCasting(new LANBroadcastService.delJoinServer(this.SeachServer));
	}

	public void StopFingerAnim()
	{
		if (this.fingerAnimObj != null && this.fingerAnimObj.activeSelf)
		{
			this.fingerStopped = true;
			this.fingerAnimObj.SetActive(false);
			this.scrollViewSelectMapTransform.GetComponent<UIScrollView>().onDragStarted -= new UIScrollView.OnDragNotification(this.StopFingerAnim);
		}
	}

	private void Update()
	{
		bool flag = (this.deathmatchToggle == null ? false : this.deathmatchToggle.gameObject.activeInHierarchy);
		if (this.armoryButton != null && this.armoryButton.activeSelf != flag)
		{
			this.armoryButton.SetActive(flag);
		}
		if (this.customPanel.activeSelf && !Defs.isInet)
		{
			this.UpdateLocalServersList();
		}
		if (Defs.isInet)
		{
			if (this.connectToWiFIInCreateLabel.activeSelf)
			{
				this.connectToWiFIInCreateLabel.SetActive(false);
			}
			if (this.connectToWiFIInCreateLabel.activeSelf)
			{
				this.connectToWiFIInCustomLabel.SetActive(false);
			}
		}
		else
		{
			this.connectToWiFIInCreateLabel.SetActive(!this.CheckLocalAvailability());
			this.connectToWiFIInCustomLabel.SetActive(!this.CheckLocalAvailability());
			if (this.createRoomUIBtn.isEnabled != this.CheckLocalAvailability())
			{
				this.createRoomUIBtn.isEnabled = this.CheckLocalAvailability();
			}
		}
		if (this.selectMapPanel.activeInHierarchy && this.centerScript != null && this.centerScript.centeredObject != null)
		{
			this.selectMap = this.centerScript.centeredObject.GetComponent<MapPreviewController>();
		}
		if (!this.unlockBtn.activeSelf && (this.mainPanel.activeSelf || this.createPanel.activeSelf) && this.selectMap != null)
		{
			SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(this.selectMap.mapID);
			if (infoScene == null)
			{
				return;
			}
			if ((this.isSetUseMap ? false : infoScene.isPremium) && Storager.getInt(string.Concat(infoScene.NameScene, "Key"), true) == 0 && !PremiumAccountController.MapAvailableDueToPremiumAccount(infoScene.NameScene))
			{
				if (!this.unlockMapBtn.activeSelf)
				{
					UILabel str = this.priceMapLabel;
					int item = Defs.PremiumMaps[infoScene.NameScene];
					str.text = item.ToString();
					this.unlockMapBtn.SetActive(true);
					this.goBtn.SetActive(false);
					UILabel uILabel = this.priceMapLabelInCreate;
					int num = Defs.PremiumMaps[infoScene.NameScene];
					uILabel.text = num.ToString();
					this.unlockMapBtnInCreate.SetActive(true);
					this.createRoomBtn.SetActive(false);
				}
			}
			else if (this.unlockMapBtn.activeSelf)
			{
				this.unlockMapBtn.SetActive(false);
				this.goBtn.SetActive(true);
				this.unlockMapBtnInCreate.SetActive(false);
				this.createRoomBtn.SetActive(true);
			}
		}
		if ((!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled) && (!(BannerWindowController.SharedController != null) || !BannerWindowController.SharedController.IsAnyBannerShown) && (!(this.loadingToDraw != null) || !this.loadingToDraw.gameObject.activeInHierarchy) && (!(this._loadingNGUIController != null) || !this._loadingNGUIController.gameObject.activeInHierarchy) && ExperienceController.sharedController != null)
		{
			ExperienceController.sharedController.isShowRanks = true;
		}
	}

	public void updateFilteredRoomList(string gFilter)
	{
		this.filteredRoomList.Clear();
		RoomInfo[] roomList = PhotonNetwork.GetRoomList();
		bool flag = !string.IsNullOrEmpty(gFilter);
		for (int i = 0; i < (int)roomList.Length; i++)
		{
			if (Defs.isDaterRegim || roomList[i].customProperties[ConnectSceneNGUIController.platformProperty] == null || roomList[i].customProperties[ConnectSceneNGUIController.platformProperty].ToString().Equals(ConnectSceneNGUIController.myPlatformConnect.ToString()) || roomList[i].customProperties[ConnectSceneNGUIController.platformProperty].ToString().Equals(3.ToString()))
			{
				if (ExpController.Instance.OurTier != 0 || !Defs.isABTestBalansCohortActual || roomList[i].customProperties[ConnectSceneNGUIController.ABTestProperty] != null && (int)roomList[i].customProperties[ConnectSceneNGUIController.ABTestProperty] == 1)
				{
					if (ExpController.Instance.OurTier != 0 || Defs.isABTestBalansCohortActual || roomList[i].customProperties[ConnectSceneNGUIController.ABTestProperty] == null || (int)roomList[i].customProperties[ConnectSceneNGUIController.ABTestProperty] != 1)
					{
						bool flag1 = true;
						if (flag)
						{
							flag1 = (!roomList[i].name.StartsWith(gFilter, true, null) ? false : (roomList[i].name.Length != 36 || roomList[i].name.IndexOf("-") != 8 ? 0 : (int)(roomList[i].name.LastIndexOf("-") == 23)) == 0);
						}
						if (flag1 && this.IsUseMap((int)roomList[i].customProperties[ConnectSceneNGUIController.mapProperty]))
						{
							this.filteredRoomList.Add(roomList[i]);
						}
					}
				}
			}
		}
		if (this.filteredRoomList.Count >= 4)
		{
			this.scrollGames.enabled = true;
		}
		else if (this.scrollGames.enabled)
		{
			this.wrapGames.SortAlphabetically();
			this.scrollGames.ResetPosition();
			this.scrollGames.enabled = false;
		}
		this.wrapGames.minIndex = this.filteredRoomList.Count * -1;
		if (this.filteredRoomList.Count > 0 && this.roomFields == null)
		{
			this.roomFields = new GameInfo[5];
			for (int j = 0; j < (int)this.roomFields.Length; j++)
			{
				GameObject gameObject = NGUITools.AddChild(this.wrapGames.gameObject, this.gameInfoItemPrefab);
				gameObject.name = string.Concat("GameInfo_", j);
				this.roomFields[j] = gameObject.GetComponent<GameInfo>();
			}
			this.wrapGames.SortAlphabetically();
			this.scrollGames.enabled = true;
			this.scrollGames.ResetPosition();
		}
		if (this.roomFields != null)
		{
			for (int k = 0; k < (int)this.roomFields.Length; k++)
			{
				this.SetRoomInfo(this.roomFields[k], this.roomFields[k].index);
			}
		}
	}

	[Obfuscation(Exclude=true)]
	private void UpdateFilteredRoomListInvoke()
	{
		this.updateFilteredRoomList(this.gameNameFilter);
	}

	private void UpdateLocalServersList()
	{
		bool item;
		List<LANBroadcastService.ReceivedMessage> receivedMessages = new List<LANBroadcastService.ReceivedMessage>();
		for (int i = 0; i < this.lanScan.lstReceivedMessages.Count; i++)
		{
			if (!Defs.filterMaps.ContainsKey(this.lanScan.lstReceivedMessages[i].map))
			{
				item = false;
			}
			else
			{
				Dictionary<string, int> strs = Defs.filterMaps;
				LANBroadcastService.ReceivedMessage receivedMessage = this.lanScan.lstReceivedMessages[i];
				item = strs[receivedMessage.map] == 3;
			}
			bool flag = item;
			if ((Defs.isDaterRegim && flag || !Defs.isDaterRegim && !flag) && this.lanScan.lstReceivedMessages[i].regim == (int)ConnectSceneNGUIController.regim)
			{
				receivedMessages.Add(this.lanScan.lstReceivedMessages[i]);
			}
		}
		if (receivedMessages.Count <= 0)
		{
			this._copy = null;
		}
		else
		{
			this._copy = receivedMessages.ToArray();
			Array.Sort<LANBroadcastService.ReceivedMessage>(this._copy, new Comparison<LANBroadcastService.ReceivedMessage>(this.LocalServerComparison));
		}
		if (this._copy != null)
		{
			if ((int)this._copy.Length >= 4)
			{
				this.scrollGames.enabled = true;
			}
			else if (this.scrollGames.enabled)
			{
				this.wrapGames.SortAlphabetically();
				this.scrollGames.ResetPosition();
				this.scrollGames.enabled = false;
			}
			this.wrapGames.minIndex = (int)this._copy.Length * -1;
			if ((int)this._copy.Length > 0 && this.roomFields == null)
			{
				this.roomFields = new GameInfo[5];
				for (int j = 0; j < (int)this.roomFields.Length; j++)
				{
					GameObject gameObject = NGUITools.AddChild(this.wrapGames.gameObject, this.gameInfoItemPrefab);
					gameObject.name = string.Concat("GameInfo_", j);
					this.roomFields[j] = gameObject.GetComponent<GameInfo>();
				}
				this.wrapGames.SortAlphabetically();
				this.scrollGames.enabled = true;
				this.scrollGames.ResetPosition();
			}
		}
		if (this.roomFields != null)
		{
			for (int k = 0; k < (int)this.roomFields.Length; k++)
			{
				this.SetLocalRoomInfo(this.roomFields[k], this.roomFields[k].index);
			}
		}
	}

	public static void UpdateUseMasMaps()
	{
		if (Defs.isDaterRegim)
		{
			ConnectSceneNGUIController.curSelectMode = TypeModeGame.Dater;
		}
		else if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle)
		{
			ConnectSceneNGUIController.curSelectMode = TypeModeGame.TimeBattle;
		}
		else if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight)
		{
			ConnectSceneNGUIController.curSelectMode = TypeModeGame.TeamFight;
		}
		else if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.DeadlyGames)
		{
			ConnectSceneNGUIController.curSelectMode = TypeModeGame.DeadlyGames;
		}
		else if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
		{
			ConnectSceneNGUIController.curSelectMode = TypeModeGame.FlagCapture;
		}
		else if (ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.CapturePoints)
		{
			ConnectSceneNGUIController.curSelectMode = TypeModeGame.Deathmatch;
		}
		else
		{
			ConnectSceneNGUIController.curSelectMode = TypeModeGame.CapturePoints;
		}
	}

	[DebuggerHidden]
	private IEnumerator WaitLoadingAndShowInterstitialCoroutine(string context, bool loadData = true)
	{
		ConnectSceneNGUIController.u003cWaitLoadingAndShowInterstitialCoroutineu003ec__Iterator1D variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator WaitLoadingAndShowReplaceAdmobPereliv(string context, bool loadData = true)
	{
		ConnectSceneNGUIController.u003cWaitLoadingAndShowReplaceAdmobPerelivu003ec__Iterator1C variable = null;
		return variable;
	}

	public enum ABTestParams
	{
		Old = 1,
		Rating = 2,
		Buff = 3
	}

	public struct infoClient
	{
		public string ipAddress;

		public string name;

		public string coments;
	}

	public struct infoServer
	{
		public string ipAddress;

		public int port;

		public string name;

		public string map;

		public int playerLimit;

		public int connectedPlayers;

		public string coments;
	}

	public enum PlatformConnect
	{
		ios = 1,
		android = 2,
		custom = 3
	}

	public enum RegimGame
	{
		Deathmatch,
		TimeBattle,
		TeamFight,
		DeadlyGames,
		FlagCapture,
		CapturePoints,
		InFriendWindow,
		InClanWindow
	}
}