using ExitGames.Client.Photon;
using Rilisoft;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Initializer : MonoBehaviour
{
	public GameObject tc;

	public GameObject tempCam;

	public bool isDisconnect;

	public int countConnectToRoom;

	public float timerShowNotConnectToRoom;

	public UIButton buttonCancel;

	public UILabel descriptionLabel;

	public bool isCancelReConnect;

	private GameObject _playerPrefab;

	private UnityEngine.Object networkTablePref;

	private bool _isMultiplayer;

	public bool isNotConnectRoom;

	private Vector2 scrollPosition = Vector2.zero;

	private List<Vector3> _initPlayerPositions = new List<Vector3>();

	private List<float> _rots = new List<float>();

	public static List<NetworkStartTable> networkTables;

	public readonly static List<Player_move_c> players;

	public static List<Player_move_c> bluePlayers;

	public static List<Player_move_c> redPlayers;

	public static List<GameObject> playersObj;

	public static List<GameObject> enemiesObj;

	public static List<GameObject> turretsObj;

	public static List<GameObject> chestsObj;

	public static List<GameObject> damagedObj;

	public static FlagController flag1;

	public static FlagController flag2;

	private float koofScreen = (float)Screen.height / 768f;

	public WeaponManager _weaponManager;

	public float timerShow = -1f;

	public Transform playerPrefab;

	public Texture fonLoadingScene;

	private bool showLoading;

	private bool isMulti;

	private bool isInet;

	private PauseONGuiDrawer _onGUIDrawer;

	private readonly IDictionary<string, int> _killedWithWeaponMap = new Dictionary<string, int>();

	public static int GameModeCampaign;

	public static int GameModeSurvival;

	public static int lastGameMode;

	private Stopwatch _gameSessionStopwatch = new Stopwatch();

	public string goMapName = string.Empty;

	private bool _needReconnectShow;

	private bool _roomNotExistShow;

	private IDisposable someWindowBackFromReconnectSubscription;

	public LoadingNGUIController _loadingNGUIController;

	private readonly static Lazy<string> _separator;

	private int joinNewRoundTries;

	private bool abTestConnect = (!Defs.isActivABTestBuffSystem || !FriendsController.useBuffSystem ? (!Defs.isActivABTestRatingSystem ? false : FriendsController.isUseRatingSystem) : true);

	public static Initializer Instance
	{
		get;
		private set;
	}

	internal static string Separator
	{
		get
		{
			return Initializer._separator.Value;
		}
	}

	static Initializer()
	{
		Initializer.networkTables = new List<NetworkStartTable>();
		Initializer.players = new List<Player_move_c>();
		Initializer.bluePlayers = new List<Player_move_c>();
		Initializer.redPlayers = new List<Player_move_c>();
		Initializer.playersObj = new List<GameObject>();
		Initializer.enemiesObj = new List<GameObject>();
		Initializer.turretsObj = new List<GameObject>();
		Initializer.chestsObj = new List<GameObject>();
		Initializer.damagedObj = new List<GameObject>();
		Initializer.GameModeCampaign = 100;
		Initializer.GameModeSurvival = 101;
		Initializer.lastGameMode = -1;
		Initializer._separator = new Lazy<string>(new Func<string>(Initializer.InitialiseSeparatorWrapper));
	}

	public Initializer()
	{
	}

	private void AddPlayer()
	{
		Vector3 vector3;
		float single;
		this._playerPrefab = Resources.Load<GameObject>("Player");
		if (!Defs.IsSurvival)
		{
			if (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != TrainingController.NewTrainingCompletedStage.None)
			{
				int num = Mathf.Max(0, CurrentCampaignGame.currentLevel - 1);
				vector3 = (CurrentCampaignGame.currentLevel != 0 ? this._initPlayerPositions[num] : new Vector3(-0.72f, 1.75f, -13.23f));
				single = (CurrentCampaignGame.currentLevel != 0 ? this._rots[num] : 0f);
				GameObject gameObject = GameObject.FindGameObjectWithTag("PlayerRespawnPoint");
				if (gameObject != null)
				{
					vector3 = gameObject.transform.position;
					single = gameObject.transform.rotation.eulerAngles.y;
				}
			}
			else
			{
				TrainingController trainingController = UnityEngine.Object.FindObjectOfType<TrainingController>();
				vector3 = (trainingController == null ? TrainingController.PlayerDefaultPosition : trainingController.PlayerDesiredPosition);
				single = 0f;
			}
		}
		else if (SceneLoader.ActiveSceneName.Equals("Arena_Underwater"))
		{
			vector3 = new Vector3(0f, 3.5f, 0f);
			single = 0f;
		}
		else if (!SceneLoader.ActiveSceneName.Equals("Pizza"))
		{
			vector3 = new Vector3(0f, 2.5f, 0f);
			single = 0f;
		}
		else
		{
			vector3 = new Vector3(-32.48f, 2.46f, 2.01f);
			single = 90f;
		}
		GameObject gameObject1 = UnityEngine.Object.Instantiate(this._playerPrefab, vector3, Quaternion.Euler(0f, single, 0f)) as GameObject;
		NickLabelController.currentCamera = gameObject1.GetComponent<SkinName>().camPlayer.GetComponent<Camera>();
		base.Invoke("SetupObjectThatNeedsPlayer", 0.01f);
	}

	private void Awake()
	{
		string str = string.Format(CultureInfo.InvariantCulture, "{0}.Awake()", new object[] { base.GetType().Name });
		ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
		try
		{
			Initializer.Instance = this;
			this.isMulti = Defs.isMulti;
			this.isInet = Defs.isInet;
			Initializer.lastGameMode = -1;
			if (Device.IsLoweMemoryDevice)
			{
				ActivityIndicator.ClearMemory();
			}
			if (!Defs.IsSurvival && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted))
			{
				this.networkTablePref = Resources.Load("NetworkTable");
			}
			Defs.typeDisconnectGame = Defs.DisconectGameType.Reconnect;
			GameObject gameObject = null;
			string activeScene = SceneManager.GetActiveScene().name;
			if (Defs.isMulti)
			{
				SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(activeScene);
				if (infoScene != null)
				{
					GlobalGameController.currentLevel = infoScene.indexMap;
				}
				gameObject = Resources.Load(string.Concat("BackgroundMusic/BackgroundMusic_Level", GlobalGameController.currentLevel)) as GameObject;
			}
			else if (CurrentCampaignGame.currentLevel != 0)
			{
				gameObject = Resources.Load(string.Concat("BackgroundMusic/BackgroundMusic_Level", CurrentCampaignGame.currentLevel)) as GameObject;
			}
			else
			{
				string str1 = string.Concat("BackgroundMusic/", (!Defs.IsSurvival ? "Background_Training" : "BackgroundMusic_Level0"));
				gameObject = Resources.Load(str1) as GameObject;
			}
			if (gameObject)
			{
				UnityEngine.Object.Instantiate<GameObject>(gameObject);
			}
			if (!Defs.isMulti && !Defs.IsSurvival)
			{
				FlurryPluginWrapper.LogEventWithParameterAndValue("Campaign Progress", "Level Started", activeScene);
				StoreKitEventListener.State.PurchaseKey = "In game";
				StoreKitEventListener.State.Parameters.Clear();
				StoreKitEventListener.State.Parameters.Add("Level", string.Concat(activeScene, " In game"));
				GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("Configurator");
				if ((int)gameObjectArray.Length > 0)
				{
					bool flag = (TrainingController.TrainingCompleted ? false : TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None);
					for (int i = 0; i != (int)gameObjectArray.Length; i++)
					{
						CoinConfigurator component = gameObjectArray[i].GetComponent<CoinConfigurator>();
						if (!(component == null) && component.CoinIsPresent)
						{
							VirtualCurrencyBonusType virtualCurrencyBonusType = (component.BonusType != VirtualCurrencyBonusType.None ? component.BonusType : VirtualCurrencyBonusType.Coin);
							if (!CoinBonus.GetLevelsWhereGotBonus(virtualCurrencyBonusType).Contains(activeScene) || flag)
							{
								Initializer.CreateBonusAtPosition((component.coinCreatePoint != null ? component.coinCreatePoint.position : component.pos), virtualCurrencyBonusType);
							}
						}
					}
				}
				Initializer.lastGameMode = Initializer.GameModeCampaign;
			}
			else if (!Defs.isMulti && Defs.IsSurvival)
			{
				FlurryPluginWrapper.LogEvent("Survival Started");
				Initializer.lastGameMode = Initializer.GameModeSurvival;
			}
			else if (Defs.isMulti)
			{
				FlurryPluginWrapper.LogEventWithParameterAndValue("Multiplayer Started", "Level", activeScene);
				Initializer.lastGameMode = (int)ConnectSceneNGUIController.regim;
			}
			string abuseKeyD4d3cbab = Initializer.GetAbuseKey_d4d3cbab(-724317269);
			if (Storager.hasKey(abuseKeyD4d3cbab))
			{
				string str2 = Storager.getString(abuseKeyD4d3cbab, false);
				if (!string.IsNullOrEmpty(str2) && str2 != "0")
				{
					long ticks = DateTime.UtcNow.Ticks >> 1;
					long num = ticks;
					if (!long.TryParse(str2, out num))
					{
						Storager.setString(abuseKeyD4d3cbab, ticks.ToString(), false);
					}
					else
					{
						num = Math.Min(ticks, num);
						Storager.setString(abuseKeyD4d3cbab, num.ToString(), false);
					}
					TimeSpan timeSpan = TimeSpan.FromTicks(ticks - num);
					bool flag1 = (!Defs.IsDeveloperBuild ? timeSpan.TotalDays >= 1 : timeSpan.TotalMinutes >= 3);
					Player_move_c.NeedApply = flag1;
					Player_move_c.AnotherNeedApply = flag1;
				}
			}
			PhotonObjectCacher.AddObject(base.gameObject);
		}
		finally
		{
			scopeLogger.Dispose();
		}
	}

	private void CheckRoom()
	{
		if (PhotonNetwork.room != null)
		{
			if (PhotonNetwork.room.maxPlayers >= 2)
			{
				if (PhotonNetwork.room.maxPlayers <= (!Defs.isCOOP ? 10 : 4))
				{
					return;
				}
			}
			this.goToConnect();
		}
	}

	[Obfuscation(Exclude=true)]
	private void ConnectToPhoton()
	{
		string str;
		if (this.isCancelReConnect)
		{
			return;
		}
		bool guiActive = ShopNGUIController.GuiActive;
		bool interfaceEnabled = BankController.Instance.InterfaceEnabled;
		bool flag = ProfileController.Instance.InterfaceEnabled;
		if (guiActive || interfaceEnabled || flag || ExpController.Instance.experienceView.LevelUpPanelOpened || NetworkStartTableNGUIController.sharedController != null && NetworkStartTableNGUIController.sharedController.isRewardShow || ExpController.Instance.WaitingForLevelUpView)
		{
			base.Invoke("ConnectToPhoton", 3f);
			return;
		}
		UnityEngine.Debug.Log("ConnectToPhoton ");
		ActivityIndicator.IsActiveIndicator = true;
		ExperienceController.sharedController.isShowRanks = false;
		if (NetworkStartTableNGUIController.sharedController != null)
		{
			NetworkStartTableNGUIController.sharedController.shopAnchor.SetActive(false);
		}
		PhotonNetwork.autoJoinLobby = false;
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

	[Obfuscation(Exclude=true)]
	private void ConnectToRoom()
	{
		base.CancelInvoke("OnCancelButtonClick");
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(this.goMapName);
		if (Defs.typeDisconnectGame == Defs.DisconectGameType.RandomGameInHunger)
		{
			UnityEngine.Debug.Log("JoinRandomRoom");
			this.isCancelReConnect = true;
			UnityEngine.Random.Range(0, SceneInfoController.instance.GetCountScenesForMode(TypeModeGame.DeadlyGames));
			PlayerPrefs.SetString("TypeGame", "client");
			PlayerPrefs.SetInt("CustomGame", 0);
			ConnectSceneNGUIController.JoinRandomGameRoom(infoScene.indexMap, ConnectSceneNGUIController.RegimGame.DeadlyGames, this.joinNewRoundTries, this.abTestConnect);
			ActivityIndicator.IsActiveIndicator = true;
			return;
		}
		if (Defs.typeDisconnectGame == Defs.DisconectGameType.SelectNewMap)
		{
			UnityEngine.Debug.Log(string.Concat("ConnectToRoom() ", this.goMapName));
			this.JoinRandomRoom(infoScene);
			return;
		}
		UnityEngine.Debug.Log(string.Concat("ConnectToRoom ", PlayerPrefs.GetString("RoomName")));
		if (this.isCancelReConnect)
		{
			return;
		}
		PhotonNetwork.JoinRoom(PlayerPrefs.GetString("RoomName"));
	}

	internal static GameObject CreateBonusAtPosition(Vector3 position, VirtualCurrencyBonusType bonusType)
	{
		string empty = string.Empty;
		VirtualCurrencyBonusType virtualCurrencyBonusType = bonusType;
		if (virtualCurrencyBonusType == VirtualCurrencyBonusType.Coin)
		{
			empty = "coin";
		}
		else
		{
			if (virtualCurrencyBonusType != VirtualCurrencyBonusType.Gem)
			{
				UnityEngine.Debug.LogErrorFormat("Failed to determine resource for '{0}'", new object[] { bonusType });
				return null;
			}
			empty = "gem";
		}
		UnityEngine.Object obj = Resources.Load(empty);
		if (obj == null)
		{
			UnityEngine.Debug.LogErrorFormat("Failed to load '{0}'", new object[] { empty });
			return null;
		}
		UnityEngine.Object obj1 = UnityEngine.Object.Instantiate(obj, position, Quaternion.Euler(270f, 0f, 0f));
		if (obj1 == null)
		{
			UnityEngine.Debug.LogErrorFormat("Failed to instantiate '{0}'", new object[] { empty });
			return null;
		}
		GameObject gameObject = obj1 as GameObject;
		if (gameObject == null)
		{
			return gameObject;
		}
		CoinBonus component = gameObject.GetComponent<CoinBonus>();
		if (component != null)
		{
			component.BonusType = bonusType;
			return gameObject;
		}
		UnityEngine.Debug.LogErrorFormat("Cannot find '{0}' script.", new object[] { typeof(CoinBonus).Name });
		return gameObject;
	}

	private static string GetAbuseKey_d4d3cbab(uint pad)
	{
		return (272218770 ^ pad).ToString("x");
	}

	[Obfuscation(Exclude=true)]
	public void goToConnect()
	{
		UnityEngine.Debug.Log("goToConnect()");
		ConnectSceneNGUIController.Local();
	}

	private void GoToRandomRoom()
	{
	}

	public void HideReconnectInterface()
	{
		this.descriptionLabel.gameObject.SetActive(false);
		this.buttonCancel.gameObject.SetActive(false);
		if (this.someWindowBackFromReconnectSubscription != null)
		{
			this.someWindowBackFromReconnectSubscription.Dispose();
			this.someWindowBackFromReconnectSubscription = null;
		}
	}

	public bool IncrementKillCountForWeapon(string weapon)
	{
		int num;
		if (string.IsNullOrEmpty(weapon))
		{
			UnityEngine.Debug.LogError("Weapon must not be null or empty.");
			return false;
		}
		bool flag = this._killedWithWeaponMap.TryGetValue(weapon, out num);
		if (!flag)
		{
			this._killedWithWeaponMap.Add(weapon, 1);
		}
		else
		{
			this._killedWithWeaponMap[weapon] = num + 1;
		}
		return flag;
	}

	private static string InitialiseSeparatorWrapper()
	{
		return Initializer.InitializeSeparator();
	}

	private static string InitializeSeparator()
	{
		string str;
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			return "bada8a20";
		}
		AndroidJavaObject currentActivity = AndroidSystem.Instance.CurrentActivity;
		if (currentActivity == null)
		{
			return "deadac71";
		}
		AndroidJavaObject androidJavaObject = currentActivity.Call<AndroidJavaObject>("getPackageManager", new object[0]);
		if (androidJavaObject == null)
		{
			return "dead3a9a";
		}
		AndroidJavaObject androidJavaObject1 = androidJavaObject.Call<AndroidJavaObject>("getPackageInfo", new object[] { "com.pixel.gun3d", 64 });
		if (androidJavaObject1 == null)
		{
			return "dead6ac5";
		}
		AndroidJavaObject[] androidJavaObjectArray = androidJavaObject1.Get<AndroidJavaObject[]>("signatures");
		if (androidJavaObjectArray == null)
		{
			return "deadc199";
		}
		if ((int)androidJavaObjectArray.Length != 1)
		{
			return "dead139c";
		}
		AndroidJavaObject androidJavaObject2 = androidJavaObjectArray[0];
		byte[] numArray = androidJavaObject2.Call<byte[]>("toByteArray", new object[0]);
		using (SHA1Managed sHA1Managed = new SHA1Managed())
		{
			byte[] numArray1 = sHA1Managed.ComputeHash(numArray);
			string lower = BitConverter.ToString(numArray1.Take<byte>(4).ToArray<byte>()).Replace("-", string.Empty).ToLower();
			str = lower;
		}
		return str;
	}

	private void JoinRandomRoom(SceneInfo _map)
	{
		if (Defs.typeDisconnectGame != Defs.DisconectGameType.SelectNewMap)
		{
			this.goMapName = _map.NameScene;
		}
		UnityEngine.Debug.Log(string.Concat("JoinRandomRoom ", this.goMapName));
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset((!Defs.filterMaps.ContainsKey(this.goMapName) ? 0 : Defs.filterMaps[this.goMapName]));
		}
		FlurryPluginWrapper.LogEnteringMap(0, this.goMapName);
		FlurryPluginWrapper.LogMultiplayerWayStart();
		ActivityIndicator.IsActiveIndicator = true;
		ConnectSceneNGUIController.JoinRandomGameRoom(_map.indexMap, ConnectSceneNGUIController.regim, this.joinNewRoundTries, this.abTestConnect);
	}

	[DebuggerHidden]
	private IEnumerator MoveToGameScene()
	{
		Initializer.u003cMoveToGameSceneu003ec__Iterator8F variable = null;
		return variable;
	}

	[Obfuscation(Exclude=true)]
	public void OnCancelButtonClick()
	{
		bool guiActive = ShopNGUIController.GuiActive;
		bool interfaceEnabled = BankController.Instance.InterfaceEnabled;
		bool flag = ProfileController.Instance.InterfaceEnabled;
		if (guiActive || interfaceEnabled || flag || ExpController.Instance.experienceView.LevelUpPanelOpened || NetworkStartTableNGUIController.sharedController != null && NetworkStartTableNGUIController.sharedController.isRewardShow)
		{
			base.Invoke("OnCancelButtonClick", 60f);
			return;
		}
		this.isCancelReConnect = true;
		this.goToConnect();
	}

	public void OnConnectedToMaster()
	{
		this.ConnectToRoom();
	}

	public void OnConnectedToPhoton()
	{
		UnityEngine.Debug.Log("OnConnectedToPhotoninit");
	}

	private void OnConnectedToServer()
	{
		this._weaponManager.myTable = (GameObject)Network.Instantiate(this.networkTablePref, base.transform.position, base.transform.rotation, 0);
		this._weaponManager.myNetworkStartTable = this._weaponManager.myTable.GetComponent<NetworkStartTable>();
	}

	private void OnConnectionFail(DisconnectCause cause)
	{
		BankController.canShowIndication = true;
		Defs.inRespawnWindow = false;
		QuestSystem.Instance.SaveQuestProgressIfDirty();
		if (Defs.typeDisconnectGame == Defs.DisconectGameType.SelectNewMap)
		{
			if (this._loadingNGUIController != null)
			{
				UnityEngine.Object.Destroy(this._loadingNGUIController.gameObject);
				this._loadingNGUIController = null;
			}
			Defs.typeDisconnectGame = Defs.DisconectGameType.Reconnect;
		}
		if (Defs.typeDisconnectGame == Defs.DisconectGameType.Exit)
		{
			this.goToConnect();
			return;
		}
		if (WeaponManager.sharedManager.myNetworkStartTable != null)
		{
			WeaponManager.sharedManager.myNetworkStartTable.CalculateMatchRatingOnDisconnect();
		}
		this.timerShowNotConnectToRoom = -1f;
		this.isCancelReConnect = false;
		this.isNotConnectRoom = false;
		this.countConnectToRoom = 0;
		PlayerPrefs.SetString("TypeGame", "client");
		UnityEngine.Debug.Log(string.Concat("OnConnectionFail ", cause));
		this.tc.SetActive(true);
		BonusController.sharedController.ClearBonuses();
		for (int i = 0; i < Initializer.enemiesObj.Count; i++)
		{
			UnityEngine.Object.Destroy(Initializer.enemiesObj[i]);
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("InGameGUI");
		if (gameObject != null)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
		GameObject gameObject1 = GameObject.FindGameObjectWithTag("ChatViewer");
		if (gameObject1 != null)
		{
			UnityEngine.Object.Destroy(gameObject1);
		}
		this.isDisconnect = true;
		base.Invoke("ConnectToPhoton", 3f);
		base.Invoke("OnCancelButtonClick", 60f);
		bool guiActive = ShopNGUIController.GuiActive;
		bool interfaceEnabled = BankController.Instance.InterfaceEnabled;
		bool flag = ProfileController.Instance.InterfaceEnabled;
		if (guiActive || interfaceEnabled || flag || ExpController.Instance.experienceView.LevelUpPanelOpened || NetworkStartTableNGUIController.sharedController != null && NetworkStartTableNGUIController.sharedController.isRewardShow)
		{
			return;
		}
		ActivityIndicator.IsActiveIndicator = true;
		ExperienceController.sharedController.isShowRanks = false;
		if (NetworkStartTableNGUIController.sharedController != null && NetworkStartTableNGUIController.sharedController.shopAnchor != null)
		{
			NetworkStartTableNGUIController.sharedController.shopAnchor.SetActive(false);
		}
	}

	private void OnDestroy()
	{
		QuestSystem.Instance.SaveQuestProgressIfDirty();
		Initializer.Instance = null;
		Initializer.players.Clear();
		Initializer.bluePlayers.Clear();
		Initializer.redPlayers.Clear();
		Defs.showTableInNetworkStartTable = false;
		Defs.showNickTableInNetworkStartTable = false;
		if (this._onGUIDrawer)
		{
			this._onGUIDrawer.act = null;
		}
		this._gameSessionStopwatch.Stop();
		if (Initializer.lastGameMode == Initializer.GameModeCampaign || Initializer.lastGameMode == Initializer.GameModeSurvival)
		{
			NetworkStartTable.IncreaseTimeInMode(Initializer.lastGameMode, this._gameSessionStopwatch.Elapsed.TotalMinutes);
		}
		FlurryEvents.StopLoggingGameModeEvent();
		this.ReportWeaponStatistics();
		ExperienceController.sharedController.isShowRanks = false;
		if (ReviewController.IsNeedActive)
		{
			ConnectSceneNGUIController.NeedShowReviewInConnectScene = true;
		}
		else if (Defs.isMulti)
		{
			bool flag = (!ReplaceAdmobPerelivController.ReplaceAdmobWithPerelivApplicable() ? false : ReplaceAdmobPerelivController.sharedController != null);
			if (flag)
			{
				ReplaceAdmobPerelivController.IncreaseTimesCounter();
			}
			if (flag && ReplaceAdmobPerelivController.ShouldShowAtThisTime)
			{
				if (!ReplaceAdmobPerelivController.sharedController.DataLoading)
				{
					ReplaceAdmobPerelivController.sharedController.LoadPerelivData();
				}
				ConnectSceneNGUIController.ReplaceAdmobWithPerelivRequest = true;
			}
			else if (MobileAdManager.AdIsApplicable(MobileAdManager.Type.Image, true))
			{
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.Log("Setting request for interstitial advertisement.");
				}
				ConnectSceneNGUIController.InterstitialRequest = true;
			}
		}
		PhotonObjectCacher.RemoveObject(base.gameObject);
		Defs.inComingMessagesCounter = 0;
	}

	public void OnDisconnectedFromPhoton()
	{
		UnityEngine.Debug.Log("OnDisconnectedFromPhotoninit");
		this.OnConnectionFail(DisconnectCause.DisconnectByServerUserLimit);
	}

	private void OnFailedToConnect(NetworkConnectionError error)
	{
		if (error == NetworkConnectionError.TooManyConnectedPlayers)
		{
			this.ShowDescriptionLabel(LocalizationStore.Get("Key_0992"));
		}
		if (error == NetworkConnectionError.ConnectionFailed)
		{
			this.ShowDescriptionLabel(LocalizationStore.Get("Key_0993"));
		}
		this.timerShow = 5f;
		if (this._weaponManager == null)
		{
			return;
		}
		if (this._weaponManager.myTable == null)
		{
			return;
		}
		this._weaponManager.myTable.GetComponent<NetworkStartTable>().isShowNickTable = false;
		this._weaponManager.myTable.GetComponent<NetworkStartTable>().showTable = false;
	}

	private void OnFailedToConnectToPhoton(object parameters)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("NetworkStartTableNGUI");
		if (gameObject != null)
		{
			NetworkStartTableNGUIController component = gameObject.GetComponent<NetworkStartTableNGUIController>();
			if (component != null)
			{
				component.shopAnchor.SetActive(false);
			}
		}
		UnityEngine.Debug.Log(string.Concat("OnFailedToConnectToPhoton. StatusCode: ", parameters));
		if (this.isCancelReConnect)
		{
			return;
		}
		base.Invoke("ConnectToPhoton", 3f);
	}

	public void OnFailedToConnectToPhoton()
	{
		UnityEngine.Debug.Log("OnFailedToConnectToPhotoninit");
	}

	public void OnJoinedLobby()
	{
		UnityEngine.Debug.Log("OnJoinedLobby()");
		this.ConnectToRoom();
	}

	private void OnJoinedRoom()
	{
		if (!this.isDisconnect)
		{
			AnalyticsStuff.LogMultiplayer();
		}
		this.CheckRoom();
		UnityEngine.Debug.Log("OnJoinedRoom - init");
		PlayerPrefs.SetString("RoomName", PhotonNetwork.room.name);
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(PhotonNetwork.room.customProperties[ConnectSceneNGUIController.mapProperty].ToString()));
		Initializer.Instance.goMapName = infoScene.NameScene;
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset((!Defs.filterMaps.ContainsKey(Initializer.Instance.goMapName) ? 0 : Defs.filterMaps[Initializer.Instance.goMapName]));
		}
		if (!this.isDisconnect || ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.Deathmatch && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.DeadlyGames)
		{
			GlobalGameController.healthMyPlayer = 0f;
			PlayerPrefs.SetInt("StartAfterDisconnect", 0);
			PhotonNetwork.isMessageQueueRunning = false;
			base.StartCoroutine(this.MoveToGameScene());
		}
		else
		{
			base.Invoke("StartGameAfterDisconnectInvoke", 3f);
		}
		this.isDisconnect = false;
		this._roomNotExistShow = false;
		this._needReconnectShow = false;
		this.HideReconnectInterface();
	}

	public void OnLeftRoom()
	{
		UnityEngine.Debug.Log("OnLeftRoom (local) init");
		NickLabelController.currentCamera = null;
		if (Defs.typeDisconnectGame == Defs.DisconectGameType.Exit)
		{
			this.showLoading = true;
			this.fonLoadingScene = null;
			base.Invoke("goToConnect", 0.1f);
			this.ShowLoadingGUI("main_loading");
			if (this._weaponManager == null)
			{
				return;
			}
			if (this._weaponManager.myTable == null)
			{
				return;
			}
			this._weaponManager.myTable.GetComponent<NetworkStartTable>().isShowNickTable = false;
			this._weaponManager.myTable.GetComponent<NetworkStartTable>().showTable = false;
			WeaponManager.sharedManager.myNetworkStartTable.CalculateMatchRatingOnDisconnect();
		}
		if (Defs.typeDisconnectGame == Defs.DisconectGameType.SelectNewMap)
		{
			bool guiActive = ShopNGUIController.GuiActive;
			bool interfaceEnabled = BankController.Instance.InterfaceEnabled;
			bool flag = ProfileController.Instance.InterfaceEnabled;
			if (!guiActive && !interfaceEnabled && !flag)
			{
				ActivityIndicator.IsActiveIndicator = true;
			}
			this.ShowLoadingGUI(this.goMapName);
		}
	}

	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		UnityEngine.Debug.Log(string.Concat("OnPhotonInstantiate init", info.sender));
	}

	private void OnPhotonJoinRoomFailed()
	{
		this.countConnectToRoom++;
		UnityEngine.Debug.Log("OnPhotonJoinRoomFailed - init");
		this.isNotConnectRoom = true;
		if (this.countConnectToRoom >= 6)
		{
			this.timerShowNotConnectToRoom = 3f;
		}
		else
		{
			base.Invoke("ConnectToRoom", 3f);
		}
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
	}

	public void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
	}

	private void OnPhotonRandomJoinFailed()
	{
		int num;
		int num1;
		int num2;
		UnityEngine.Debug.Log("OnPhotonJoinRoomFailed");
		PlayerPrefs.SetString("TypeGame", "server");
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(this.goMapName);
		if (this.joinNewRoundTries >= 2 && this.abTestConnect)
		{
			this.abTestConnect = false;
			this.joinNewRoundTries = 0;
		}
		if (this.joinNewRoundTries < 2)
		{
			UnityEngine.Debug.Log(string.Concat("No rooms with new round: ", this.joinNewRoundTries, (!this.abTestConnect ? string.Empty : " AbTestSeparate")));
			this.joinNewRoundTries++;
			ConnectSceneNGUIController.JoinRandomGameRoom(infoScene.indexMap, ConnectSceneNGUIController.regim, this.joinNewRoundTries, this.abTestConnect);
			return;
		}
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset((!Defs.filterMaps.ContainsKey(this.goMapName) ? 0 : Defs.filterMaps[this.goMapName]));
		}
		if (Defs.isCOOP)
		{
			num = 4;
		}
		else if (!Defs.isCompany)
		{
			num = (!Defs.isHunger ? 10 : 6);
		}
		else
		{
			num = 10;
		}
		int num3 = num;
		if (!(ExperienceController.sharedController != null) || ExperienceController.sharedController.currentLevel > 2)
		{
			num1 = (!(ExperienceController.sharedController != null) || ExperienceController.sharedController.currentLevel > 5 ? 2 : 1);
		}
		else
		{
			num1 = 0;
		}
		if (ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.DeadlyGames)
		{
			num2 = (!Defs.isDaterRegim ? 4 : 5);
		}
		else
		{
			num2 = 10;
		}
		int num4 = num2;
		ConnectSceneNGUIController.CreateGameRoom(null, num3, infoScene.indexMap, num4, string.Empty, ConnectSceneNGUIController.regim);
	}

	public void OnReceivedRoomList()
	{
	}

	public void OnReceivedRoomListUpdate()
	{
	}

	private void ReconnectGUI()
	{
		bool guiActive = ShopNGUIController.GuiActive;
		bool interfaceEnabled = BankController.Instance.InterfaceEnabled;
		bool flag = ProfileController.Instance.Map<ProfileController, bool>((ProfileController p) => p.InterfaceEnabled);
		if (guiActive || interfaceEnabled || flag)
		{
			return;
		}
		if (this.isDisconnect && (!(NetworkStartTableNGUIController.sharedController != null) || !NetworkStartTableNGUIController.sharedController.isRewardShow))
		{
			if (this.timerShowNotConnectToRoom > 0f)
			{
				this.timerShowNotConnectToRoom -= Time.deltaTime;
				if (this.timerShowNotConnectToRoom <= 0f)
				{
					SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(this.goMapName);
					if (infoScene == null)
					{
						this.goToConnect();
					}
					else
					{
						this.isDisconnect = false;
						this.JoinRandomRoom(infoScene);
					}
				}
				else if (!this._needReconnectShow)
				{
					this._needReconnectShow = true;
					this.ShowDescriptionLabel(LocalizationStore.Get("Key_1005"));
					this.buttonCancel.gameObject.SetActive(false);
					if (this.someWindowBackFromReconnectSubscription != null)
					{
						this.someWindowBackFromReconnectSubscription.Dispose();
						this.someWindowBackFromReconnectSubscription = null;
					}
				}
			}
			else if (!this._roomNotExistShow)
			{
				this._roomNotExistShow = true;
				this.ShowDescriptionLabel(LocalizationStore.Get("Key_1004"));
				bool flag1 = (ShopNGUIController.GuiActive ? false : !ProfileController.Instance.InterfaceEnabled);
				this.buttonCancel.gameObject.SetActive(flag1);
				if (flag1 && this.someWindowBackFromReconnectSubscription == null)
				{
					this.someWindowBackFromReconnectSubscription = BackSystem.Instance.Register(new Action(this.OnCancelButtonClick), "Cancel from reconnect");
				}
				if (!flag1 && this.someWindowBackFromReconnectSubscription != null)
				{
					this.someWindowBackFromReconnectSubscription.Dispose();
					this.someWindowBackFromReconnectSubscription = null;
				}
			}
		}
	}

	private void ReportWeaponStatistics()
	{
		Dictionary<string, string> strs;
		if (UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log(string.Concat("Killed with weapon:    ", Json.Serialize(this._killedWithWeaponMap)));
		}
		string eventName = FlurryPluginWrapper.GetEventName("Killed With Weapon Worldwide");
		string str = FlurryPluginWrapper.GetEventName("Killed With Weapon Worldwide (Tier 1-3)");
		string eventName1 = FlurryPluginWrapper.GetEventName("Killed With Weapon Worldwide (Tier 4-5)");
		int ourTier = ExpController.GetOurTier();
		string str1 = (ourTier + 1 <= 3 ? str : eventName1);
		IEnumerator<KeyValuePair<string, int>> enumerator = this._killedWithWeaponMap.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, int> current = enumerator.Current;
				int value = current.Value;
				for (int i = 1; value > 0 && i < 10; i *= 10)
				{
					int num = value % 10;
					string str2 = string.Format("{0}x", i);
					strs = new Dictionary<string, string>()
					{
						{ str2, current.Key }
					};
					Dictionary<string, string> strs1 = strs;
					strs = new Dictionary<string, string>()
					{
						{ str2, current.Key },
						{ string.Format("{0} (Tier {1})", str2, ourTier), current.Key }
					};
					Dictionary<string, string> strs2 = strs;
					for (int j = 0; j < num; j++)
					{
						FlurryPluginWrapper.LogEventAndDublicateToConsole(eventName, strs1, true);
						FlurryPluginWrapper.LogEventAndDublicateToConsole(str1, strs2, true);
					}
					value /= 10;
				}
				if (value <= 0)
				{
					continue;
				}
				string str3 = string.Format("{0}x", 10);
				strs = new Dictionary<string, string>()
				{
					{ str3, current.Key }
				};
				Dictionary<string, string> strs3 = strs;
				strs = new Dictionary<string, string>()
				{
					{ str3, current.Key },
					{ string.Format("{0} (Tier {1})", str3, ourTier), current.Key }
				};
				Dictionary<string, string> strs4 = strs;
				for (int k = 0; k < value; k++)
				{
					FlurryPluginWrapper.LogEventAndDublicateToConsole(eventName, strs3, true);
					FlurryPluginWrapper.LogEventAndDublicateToConsole(str1, strs4, true);
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
	}

	[Obfuscation(Exclude=true)]
	public void SetupObjectThatNeedsPlayer()
	{
		if (Defs.isMulti)
		{
			if (Initializer.PlayerAddedEvent != null)
			{
				Initializer.PlayerAddedEvent();
			}
			return;
		}
		GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("CoinBonus");
		for (int i = 0; i != (int)gameObjectArray.Length; i++)
		{
			CoinBonus component = gameObjectArray[i].GetComponent<CoinBonus>();
			if (component != null)
			{
				component.SetPlayer();
			}
		}
		if (TrainingController.TrainingCompleted)
		{
			ZombieCreator.sharedCreator.BeganCreateEnemies();
		}
		base.GetComponent<BonusCreator>().BeginCreateBonuses();
		if (Initializer.PlayerAddedEvent != null)
		{
			Initializer.PlayerAddedEvent();
		}
	}

	private void ShowDescriptionLabel(string text)
	{
		this.descriptionLabel.gameObject.SetActive(true);
		this.descriptionLabel.text = text;
	}

	private void ShowLoadingGUI(string _mapName)
	{
		this._loadingNGUIController = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("LoadingGUI")).GetComponent<LoadingNGUIController>();
		this._loadingNGUIController.SceneToLoad = _mapName;
		this._loadingNGUIController.loadingNGUITexture.mainTexture = Resources.Load((!_mapName.Equals("main_loading") ? string.Concat("LevelLoadings", (!Device.isRetinaAndStrong ? string.Empty : "/Hi"), "/Loading_", _mapName) : string.Empty)) as Texture2D;
		this._loadingNGUIController.transform.localPosition = Vector3.zero;
		this._loadingNGUIController.Init();
		ExperienceController.sharedController.isShowRanks = false;
	}

	[PunRPC]
	[RPC]
	private void SpawnOnNetwork(Vector3 pos, Quaternion rot, int id1, PhotonPlayer np)
	{
		if (this.networkTablePref != null)
		{
			Transform transforms = UnityEngine.Object.Instantiate(this.networkTablePref, pos, rot) as Transform;
			transforms.GetComponent<PhotonView>().viewID = id1;
		}
	}

	private void Start()
	{
		ConnectSceneNGUIController.isReturnFromGame = true;
		FriendsController.sharedController.profileInfo.Clear();
		FriendsController.sharedController.notShowAddIds.Clear();
		FacebookController.LogEvent("Campaign_ACHIEVED_LEVEL", null);
		Defs.inRespawnWindow = false;
		PlayerPrefs.SetInt("StartAfterDisconnect", 0);
		PhotonNetwork.isMessageQueueRunning = true;
		this._isMultiplayer = Defs.isMulti;
		this._weaponManager = WeaponManager.sharedManager;
		this._weaponManager.players.Clear();
		this.CheckRoom();
		if (PhotonNetwork.room != null)
		{
			SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(PhotonNetwork.room.customProperties[ConnectSceneNGUIController.mapProperty].ToString()));
			this.goMapName = infoScene.NameScene;
			try
			{
				string str = ConnectSceneNGUIController.regim.ToString();
				string translateEngShortName = infoScene.TranslateEngShortName;
				Dictionary<string, string> strs = new Dictionary<string, string>()
				{
					{ "Mode", str },
					{ "Map", translateEngShortName },
					{ string.Format("Mode ({0})", str), translateEngShortName },
					{ "Mode, Map", string.Concat(str, ", ", translateEngShortName) }
				};
				FlurryPluginWrapper.LogEventAndDublicateToConsole(FlurryPluginWrapper.GetEventName("Maps Popularity Worldwide"), strs, true);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}
		if (this._isMultiplayer)
		{
			this.tc = UnityEngine.Object.Instantiate(this.tempCam, Vector3.zero, Quaternion.identity) as GameObject;
			if (Defs.isInet)
			{
				this._weaponManager.myTable = PhotonNetwork.Instantiate("NetworkTable", base.transform.position, base.transform.rotation, 0);
				if (this._weaponManager.myTable == null)
				{
					this.OnConnectionFail(DisconnectCause.DisconnectByClientTimeout);
				}
				else
				{
					this._weaponManager.myNetworkStartTable = this._weaponManager.myTable.GetComponent<NetworkStartTable>();
				}
			}
			else if (!PlayerPrefs.GetString("TypeGame").Equals("client"))
			{
				this._weaponManager.myTable = (GameObject)Network.Instantiate(this.networkTablePref, base.transform.position, base.transform.rotation, 0);
				this._weaponManager.myNetworkStartTable = this._weaponManager.myTable.GetComponent<NetworkStartTable>();
			}
			else
			{
				Network.useNat = !Network.HavePublicAddress();
				UnityEngine.Debug.Log(string.Concat(Defs.ServerIp, " ", Network.Connect(Defs.ServerIp, 25002)));
			}
		}
		else
		{
			this._initPlayerPositions.Add(new Vector3(12f, 1f, 9f));
			this._initPlayerPositions.Add(new Vector3(17f, 1f, -15f));
			this._initPlayerPositions.Add(new Vector3(-42f, 1f, -10.487f));
			this._initPlayerPositions.Add(new Vector3(0f, 1f, 19.5f));
			this._initPlayerPositions.Add(new Vector3(-33f, 1.2f, -13f));
			this._initPlayerPositions.Add(new Vector3(-2.67f, 1f, 2.67f));
			this._initPlayerPositions.Add(new Vector3(0f, 1f, 0f));
			this._initPlayerPositions.Add(new Vector3(19f, 1f, -0.8f));
			this._initPlayerPositions.Add(new Vector3(-28.5f, 1.75f, -3.73f));
			this._initPlayerPositions.Add(new Vector3(-2.5f, 1.75f, 0f));
			this._initPlayerPositions.Add(new Vector3(-1.596549f, 2.5f, 2.684792f));
			this._initPlayerPositions.Add(new Vector3(-6.611357f, 1.5f, -105.2573f));
			this._initPlayerPositions.Add(new Vector3(-20.3f, 2f, 17.6f));
			this._initPlayerPositions.Add(new Vector3(5f, 2.5f, 0f));
			this._initPlayerPositions.Add(new Vector3(0f, 2.5f, 0f));
			this._initPlayerPositions.Add(new Vector3(-7.3f, 3.6f, 6.46f));
			this._initPlayerPositions.Add(new Vector3(17f, 1f, -15f));
			this._initPlayerPositions.Add(new Vector3(17f, 1f, 0f));
			this._initPlayerPositions.Add(new Vector3(0.2f, 11.2f, -0.28f));
			this._initPlayerPositions.Add(new Vector3(-1.76f, 100.9f, 20.8f));
			this._initPlayerPositions.Add(new Vector3(20f, -0.4f, 17f));
			this._rots.Add(0f);
			this._rots.Add(0f);
			this._rots.Add(0f);
			this._rots.Add(180f);
			this._rots.Add(180f);
			this._rots.Add(0f);
			this._rots.Add(0f);
			this._rots.Add(270f);
			this._rots.Add(270f);
			this._rots.Add(270f);
			this._rots.Add(0f);
			this._rots.Add(0f);
			this._rots.Add(90f);
			this._rots.Add(0f);
			this._rots.Add(0f);
			this._rots.Add(90f);
			this._rots.Add(0f);
			this._rots.Add(0f);
			this._rots.Add(0f);
			this._rots.Add(0f);
			this._rots.Add(0f);
			if (Storager.getInt(Defs.EarnedCoins, false) > 0)
			{
				UnityEngine.Object.Instantiate<GameObject>(Resources.Load("MessageCoinsObject") as GameObject);
			}
			this.AddPlayer();
		}
		FlurryEvents.StartLoggingGameModeEvent();
		this._gameSessionStopwatch.Start();
	}

	[Obfuscation(Exclude=true)]
	private void StartGameAfterDisconnectInvoke()
	{
		if (ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.TimeBattle && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.FlagCapture && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.TeamFight && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.CapturePoints && !Defs.showTableInNetworkStartTable && !Defs.showNickTableInNetworkStartTable)
		{
			PlayerPrefs.SetInt("StartAfterDisconnect", 1);
		}
		this._weaponManager.myTable = PhotonNetwork.Instantiate("NetworkTable", base.transform.position, base.transform.rotation, 0);
		this._weaponManager.myNetworkStartTable = this._weaponManager.myTable.GetComponent<NetworkStartTable>();
		ActivityIndicator.IsActiveIndicator = false;
	}

	private void Update()
	{
		if (this._onGUIDrawer)
		{
			this._onGUIDrawer.gameObject.SetActive((this.isDisconnect ? true : this.showLoading));
		}
		if (this.timerShow > 0f)
		{
			this.timerShow -= Time.deltaTime;
			this.showLoading = true;
			this.fonLoadingScene = null;
			base.Invoke("goToConnect", 0.1f);
		}
		this.ReconnectGUI();
	}

	public static event Action PlayerAddedEvent;
}