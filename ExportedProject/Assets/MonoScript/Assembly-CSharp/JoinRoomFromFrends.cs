using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class JoinRoomFromFrends : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CShowUnlockMapDialog_003Ec__AnonStorey27D
	{
		internal UnlockPremiumMapView unlockPremiumMapView;

		internal Action successfulUnlockCallback;

		internal string levelName;

		internal JoinRoomFromFrends _003C_003Ef__this;

		internal void _003C_003Em__186(object sender, EventArgs e)
		{
			_003C_003Ef__this.HandleCloseUnlockDialog(unlockPremiumMapView);
		}

		internal void _003C_003Em__187(object sender, EventArgs e)
		{
			_003C_003Ef__this.HandleUnlockPressed(unlockPremiumMapView, successfulUnlockCallback, levelName);
		}
	}

	[CompilerGenerated]
	private sealed class _003CHandleUnlockPressed_003Ec__AnonStorey27E
	{
		internal string levelName;

		internal int priceAmount;

		internal Action successfulUnlockCallback;

		internal UnlockPremiumMapView unlockPremiumMapView;

		internal void _003C_003Em__188()
		{
			Storager.setInt(levelName + "Key", 1, true);
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				PlayerPrefs.Save();
			}
			ShopNGUIController.SynchronizeAndroidPurchases("Friend's map unlocked: " + levelName);
			FlurryPluginWrapper.LogEvent("Unlock " + levelName + " map");
			AnalyticsStuff.LogSales(levelName, "Premium Maps");
			AnalyticsFacade.InAppPurchase(levelName, "Premium Maps", 1, priceAmount, "Coins");
			if (coinsPlashka.thisScript != null)
			{
				coinsPlashka.thisScript.enabled = false;
			}
			successfulUnlockCallback();
			UnityEngine.Object.Destroy(unlockPremiumMapView.gameObject);
		}

		internal void _003C_003Em__189()
		{
			FlurryPluginWrapper.LogEvent("Try_Enable " + levelName + " map");
			StoreKitEventListener.State.PurchaseKey = "In map selection In Friends";
		}
	}

	public int game_mode;

	public string room_name;

	public static JoinRoomFromFrends sharedJoinRoomFromFrends;

	public GameObject friendsPanel;

	public GameObject connectPanel;

	public static GameObject friendProfilePanel;

	public UILabel label;

	public GameObject plashkaLabel;

	private bool isFaledConnectToRoom;

	private bool oldActivFriendPanel;

	private bool oldActivProfileProfile;

	public UITexture fonConnectTexture;

	private string passwordRoom;

	public GameObject WrongPasswordLabel;

	private float timerShowWrongPassword;

	public GameObject PasswordPanel;

	private bool isBackFromPassword;

	public UIInput inputPassworLabel;

	public GameObject objectForOffWhenUlockDialog;

	private IDisposable _backSubscription;

	private LoadingNGUIController _loadingNGUIController;

	[CompilerGenerated]
	private static Action _003C_003Ef__am_0024cache15;

	private void Start()
	{
		sharedJoinRoomFromFrends = this;
	}

	private void OnEnable()
	{
		PhotonObjectCacher.AddObject(base.gameObject);
	}

	private void OnEsc()
	{
		PhotonNetwork.Disconnect();
		closeConnectPanel();
	}

	private void OnDisable()
	{
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	private void OnDestroy()
	{
		sharedJoinRoomFromFrends = null;
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	public void BackFromPasswordButton()
	{
		isBackFromPassword = true;
		SetEnabledPasswordPanel(false);
		PhotonNetwork.Disconnect();
		Debug.Log("BackFromPasswordButton");
	}

	public void EnterPassword(string pass)
	{
		if (pass == passwordRoom)
		{
			PhotonNetwork.isMessageQueueRunning = false;
			StartCoroutine(MoveToGameScene());
			ActivityIndicator.IsActiveIndicator = true;
		}
		else
		{
			timerShowWrongPassword = 3f;
			WrongPasswordLabel.SetActive(true);
		}
	}

	private void ShowLoadingGUI(string _mapName)
	{
		_loadingNGUIController = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("LoadingGUI")).GetComponent<LoadingNGUIController>();
		_loadingNGUIController.SceneToLoad = _mapName;
		_loadingNGUIController.loadingNGUITexture.mainTexture = Resources.Load<Texture2D>("LevelLoadings" + ((!Device.isRetinaAndStrong) ? string.Empty : "/Hi") + "/Loading_" + _mapName);
		_loadingNGUIController.transform.parent = fonConnectTexture.transform.parent;
		_loadingNGUIController.transform.localPosition = Vector3.zero;
		_loadingNGUIController.Init();
	}

	private void RemoveLoadingGUI()
	{
		if (_loadingNGUIController != null)
		{
			UnityEngine.Object.Destroy(_loadingNGUIController.gameObject);
			_loadingNGUIController = null;
		}
	}

	private IEnumerator SetFonLoadingWaitForReset(string _mapName = "", bool isAddCountRun = false)
	{
		RemoveLoadingGUI();
		while (WeaponManager.sharedManager == null)
		{
			yield return null;
		}
		while (WeaponManager.sharedManager.LockGetWeaponPrefabs > 0)
		{
			yield return null;
		}
		ShowLoadingGUI(_mapName);
	}

	public void ConnectToRoom(int _game_mode, string _room_name, string _map)
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
		_backSubscription = BackSystem.Instance.Register(OnEsc, "Connect To Friend");
		InfoWindowController.HideCurrentWindow();
		SetEnabledPasswordPanel(false);
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(_map));
		if (infoScene.isPremium && Storager.getInt(infoScene.NameScene + "Key", true) != 1 && !PremiumAccountController.MapAvailableDueToPremiumAccount(infoScene.NameScene))
		{
			if (objectForOffWhenUlockDialog != null)
			{
				objectForOffWhenUlockDialog.SetActive(false);
			}
			if (_003C_003Ef__am_0024cache15 == null)
			{
				_003C_003Ef__am_0024cache15 = _003CConnectToRoom_003Em__185;
			}
			Action successfulUnlockCallback = _003C_003Ef__am_0024cache15;
			ShowUnlockMapDialog(successfulUnlockCallback, infoScene.NameScene);
			return;
		}
		int gameTier = ((_game_mode <= 99) ? (_game_mode / 10) : (_game_mode % 100 / 10));
		game_mode = _game_mode % 10;
		room_name = _room_name;
		Defs.isMulti = true;
		Defs.isInet = true;
		Defs.isFlag = false;
		Defs.isCOOP = false;
		Defs.isCompany = false;
		Defs.isHunger = false;
		Defs.isCapturePoints = false;
		switch (game_mode)
		{
		default:
			return;
		case 0:
			StoreKitEventListener.State.Mode = "Deathmatch Wordwide";
			ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.Deathmatch;
			break;
		case 1:
			StoreKitEventListener.State.Mode = "Time Survival";
			Defs.isCOOP = true;
			ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.TimeBattle;
			break;
		case 2:
			StoreKitEventListener.State.Mode = "Team Battle";
			Defs.isCompany = true;
			ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.TeamFight;
			break;
		case 3:
			if (true)
			{
				Defs.isHunger = true;
				StoreKitEventListener.State.Mode = "Deadly Games";
				ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.DeadlyGames;
				break;
			}
			if (ShowNoJoinConnectFromRanks.sharedController != null)
			{
				ShowNoJoinConnectFromRanks.sharedController.resetShow(3);
			}
			return;
		case 4:
			if (true)
			{
				Defs.isFlag = true;
				ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.FlagCapture;
				break;
			}
			StoreKitEventListener.State.Mode = "Flag Capture";
			if (ShowNoJoinConnectFromRanks.sharedController != null)
			{
				ShowNoJoinConnectFromRanks.sharedController.resetShow(4);
			}
			return;
		case 5:
			Defs.isCapturePoints = true;
			ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.CapturePoints;
			break;
		}
		ActivityIndicator.IsActiveIndicator = true;
		oldActivFriendPanel = friendsPanel.activeSelf;
		if (friendProfilePanel != null)
		{
			oldActivProfileProfile = friendProfilePanel.activeSelf;
		}
		connectPanel.SetActive(true);
		friendsPanel.SetActive(false);
		if (friendProfilePanel != null)
		{
			friendProfilePanel.SetActive(false);
		}
		label.gameObject.SetActive(false);
		plashkaLabel.SetActive(false);
		Debug.Log("fonConnectTexture.mainTexture=" + _map + " " + infoScene.NameScene);
		Defs.isDaterRegim = Defs.filterMaps.ContainsKey(infoScene.NameScene) && infoScene.AvaliableWeapon == ModeWeapon.dater;
		WeaponManager.sharedManager.Reset(Defs.isDaterRegim ? 3 : 0);
		StartCoroutine(SetFonLoadingWaitForReset(infoScene.NameScene));
		string text = Initializer.Separator + ConnectSceneNGUIController.regim.ToString() + (Defs.isDaterRegim ? "Dater" : ((!Defs.isHunger) ? gameTier.ToString() : "0")) + "v" + GlobalGameController.MultiplayerProtocolVersion;
		ConnectSceneNGUIController.gameTier = gameTier;
		Debug.Log("Connect -" + text);
		PhotonNetwork.autoJoinLobby = false;
		PhotonNetwork.ConnectUsingSettings(text);
	}

	private void Update()
	{
		if (coinsPlashka.thisScript != null)
		{
			coinsPlashka.thisScript.enabled = coinsShop.thisScript != null && coinsShop.thisScript.enabled;
		}
		if (timerShowWrongPassword > 0f && WrongPasswordLabel.activeSelf)
		{
			timerShowWrongPassword -= Time.deltaTime;
		}
		if (timerShowWrongPassword <= 0f && WrongPasswordLabel.activeSelf)
		{
			WrongPasswordLabel.SetActive(false);
		}
	}

	private void ShowUnlockMapDialog(Action successfulUnlockCallback, string levelName)
	{
		_003CShowUnlockMapDialog_003Ec__AnonStorey27D _003CShowUnlockMapDialog_003Ec__AnonStorey27D = new _003CShowUnlockMapDialog_003Ec__AnonStorey27D();
		_003CShowUnlockMapDialog_003Ec__AnonStorey27D.successfulUnlockCallback = successfulUnlockCallback;
		_003CShowUnlockMapDialog_003Ec__AnonStorey27D.levelName = levelName;
		_003CShowUnlockMapDialog_003Ec__AnonStorey27D._003C_003Ef__this = this;
		if (string.IsNullOrEmpty(_003CShowUnlockMapDialog_003Ec__AnonStorey27D.levelName))
		{
			Debug.LogWarning("Level name shoul not be empty.");
			return;
		}
		UnityEngine.Object original = Resources.Load("UnlockPremiumMapView");
		GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
		gameObject.transform.parent = base.transform;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		Tools.SetLayerRecursively(gameObject, base.gameObject.layer);
		ActivityIndicator.IsActiveIndicator = false;
		_003CShowUnlockMapDialog_003Ec__AnonStorey27D.unlockPremiumMapView = gameObject.GetComponent<UnlockPremiumMapView>();
		if (_003CShowUnlockMapDialog_003Ec__AnonStorey27D.unlockPremiumMapView == null)
		{
			Debug.LogError("UnlockPremiumMapView should not be null.");
			return;
		}
		int value = 0;
		Defs.PremiumMaps.TryGetValue(_003CShowUnlockMapDialog_003Ec__AnonStorey27D.levelName, out value);
		_003CShowUnlockMapDialog_003Ec__AnonStorey27D.unlockPremiumMapView.Price = value;
		EventHandler value2 = _003CShowUnlockMapDialog_003Ec__AnonStorey27D._003C_003Em__186;
		EventHandler value3 = _003CShowUnlockMapDialog_003Ec__AnonStorey27D._003C_003Em__187;
		_003CShowUnlockMapDialog_003Ec__AnonStorey27D.unlockPremiumMapView.ClosePressed += value2;
		_003CShowUnlockMapDialog_003Ec__AnonStorey27D.unlockPremiumMapView.UnlockPressed += value3;
	}

	private void HandleCloseUnlockDialog(UnlockPremiumMapView unlockPremiumMapView)
	{
		if (objectForOffWhenUlockDialog != null)
		{
			objectForOffWhenUlockDialog.SetActive(true);
		}
		closeConnectPanel();
		UnityEngine.Object.Destroy(unlockPremiumMapView.gameObject);
	}

	private void HandleUnlockPressed(UnlockPremiumMapView unlockPremiumMapView, Action successfulUnlockCallback, string levelName)
	{
		_003CHandleUnlockPressed_003Ec__AnonStorey27E _003CHandleUnlockPressed_003Ec__AnonStorey27E = new _003CHandleUnlockPressed_003Ec__AnonStorey27E();
		_003CHandleUnlockPressed_003Ec__AnonStorey27E.levelName = levelName;
		_003CHandleUnlockPressed_003Ec__AnonStorey27E.successfulUnlockCallback = successfulUnlockCallback;
		_003CHandleUnlockPressed_003Ec__AnonStorey27E.unlockPremiumMapView = unlockPremiumMapView;
		_003CHandleUnlockPressed_003Ec__AnonStorey27E.priceAmount = _003CHandleUnlockPressed_003Ec__AnonStorey27E.unlockPremiumMapView.Price;
		ShopNGUIController.TryToBuy((!(FriendsWindowGUI.Instance != null)) ? _003CHandleUnlockPressed_003Ec__AnonStorey27E.unlockPremiumMapView.gameObject : FriendsWindowGUI.Instance.gameObject, new ItemPrice(_003CHandleUnlockPressed_003Ec__AnonStorey27E.unlockPremiumMapView.Price, "Coins"), _003CHandleUnlockPressed_003Ec__AnonStorey27E._003C_003Em__188, _003CHandleUnlockPressed_003Ec__AnonStorey27E._003C_003Em__189);
	}

	[Obfuscation(Exclude = true)]
	public void closeConnectPanel()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
		fonConnectTexture.mainTexture = null;
		RemoveLoadingGUI();
		connectPanel.SetActive(false);
		label.gameObject.SetActive(false);
		plashkaLabel.SetActive(false);
		friendsPanel.SetActive(true);
		ActivityIndicator.IsActiveIndicator = false;
	}

	private void ShowLabel(string text)
	{
		label.text = text;
		label.gameObject.SetActive(true);
		plashkaLabel.SetActive(true);
		ActivityIndicator.IsActiveIndicator = false;
		Invoke("closeConnectPanel", 3f);
	}

	private void OnDisconnectedFromPhoton()
	{
		if (isFaledConnectToRoom)
		{
			ShowLabel("Game is unavailable...");
		}
		else if (isBackFromPassword)
		{
			closeConnectPanel();
		}
		else
		{
			ShowLabel("Can't connect ...");
		}
		isFaledConnectToRoom = false;
		isBackFromPassword = false;
		Debug.Log("OnDisconnectedFromPhoton");
	}

	private void OnFailedToConnectToPhoton(object parameters)
	{
		ShowLabel("Can't connect ...");
		Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters);
	}

	public void OnConnectedToMaster()
	{
		ConnectToRoom();
	}

	public void OnJoinedLobby()
	{
		ConnectToRoom();
	}

	[Obfuscation(Exclude = true)]
	private void ConnectToRoom()
	{
		PhotonNetwork.playerName = ProfileController.GetPlayerNameOrDefault();
		Debug.Log("OnJoinedLobby " + room_name);
		PhotonNetwork.JoinRoom(room_name);
		PlayerPrefs.SetString("RoomName", room_name);
	}

	private void OnPhotonJoinRoomFailed()
	{
		Debug.Log("OnPhotonJoinRoomFailed - init");
		isFaledConnectToRoom = true;
		PhotonNetwork.Disconnect();
		InfoWindowController.ShowInfoBox(LocalizationStore.Get("Key_0137"));
		InfoWindowController.HideProcessing(3f);
		Defs.isFlag = false;
		Defs.isCOOP = false;
		Defs.isCompany = false;
		Defs.isHunger = false;
		Defs.isCapturePoints = false;
		Defs.isDaterRegim = false;
		WeaponManager.sharedManager.Reset();
	}

	private void SetEnabledPasswordPanel(bool enabled)
	{
		PasswordPanel.SetActive(enabled);
		if (_loadingNGUIController != null)
		{
			fonConnectTexture.gameObject.SetActive(enabled);
			fonConnectTexture.mainTexture = ((!enabled) ? null : _loadingNGUIController.loadingNGUITexture.mainTexture);
			_loadingNGUIController.gameObject.SetActive(!enabled);
		}
	}

	private void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom - init");
		GlobalGameController.healthMyPlayer = 0f;
		if (PhotonNetwork.room != null)
		{
			passwordRoom = PhotonNetwork.room.customProperties[ConnectSceneNGUIController.passwordProperty].ToString();
			PhotonNetwork.isMessageQueueRunning = false;
			if (passwordRoom.Equals(string.Empty))
			{
				PhotonNetwork.isMessageQueueRunning = false;
				StartCoroutine(MoveToGameScene());
				return;
			}
			Debug.Log("Show Password Panel " + passwordRoom);
			ActivityIndicator.IsActiveIndicator = false;
			inputPassworLabel.value = string.Empty;
			SetEnabledPasswordPanel(true);
		}
		else
		{
			PhotonNetwork.Disconnect();
			ShowLabel("Game is unavailable...");
		}
	}

	private IEnumerator MoveToGameScene()
	{
		AnalyticsStuff.LogMultiplayer();
		if (SceneLoader.ActiveSceneName.Equals("Clans"))
		{
			Defs.isGameFromFriends = false;
			Defs.isGameFromClans = true;
		}
		else
		{
			Defs.isGameFromFriends = true;
			Defs.isGameFromClans = false;
		}
		SceneInfo scInfo = SceneInfoController.instance.GetInfoScene(int.Parse(PhotonNetwork.room.customProperties[ConnectSceneNGUIController.mapProperty].ToString()));
		WeaponManager.sharedManager.Reset((int)((scInfo != null) ? scInfo.AvaliableWeapon : ModeWeapon.all));
		Debug.Log("MoveToGameScene");
		while (PhotonNetwork.room == null)
		{
			yield return 0;
		}
		Debug.Log("map=" + PhotonNetwork.room.customProperties[ConnectSceneNGUIController.mapProperty].ToString());
		Debug.Log(scInfo.NameScene);
		LoadConnectScene.textureToShow = Resources.Load("LevelLoadings" + ((!Device.isRetinaAndStrong) ? string.Empty : "/Hi") + "/Loading_" + scInfo.NameScene) as Texture2D;
		LoadingInAfterGame.loadingTexture = LoadConnectScene.textureToShow;
		LoadConnectScene.sceneToLoad = scInfo.NameScene;
		LoadConnectScene.noteToShow = null;
		yield return Singleton<SceneLoader>.Instance.LoadSceneAsync("PromScene");
	}

	[CompilerGenerated]
	private static void _003CConnectToRoom_003Em__185()
	{
	}
}
