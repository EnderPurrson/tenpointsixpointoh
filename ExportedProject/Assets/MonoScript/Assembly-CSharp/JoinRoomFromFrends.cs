using ExitGames.Client.Photon;
using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class JoinRoomFromFrends : MonoBehaviour
{
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

	static JoinRoomFromFrends()
	{
	}

	public JoinRoomFromFrends()
	{
	}

	public void BackFromPasswordButton()
	{
		this.isBackFromPassword = true;
		this.SetEnabledPasswordPanel(false);
		PhotonNetwork.Disconnect();
		UnityEngine.Debug.Log("BackFromPasswordButton");
	}

	[Obfuscation(Exclude=true)]
	public void closeConnectPanel()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
		this.fonConnectTexture.mainTexture = null;
		this.RemoveLoadingGUI();
		this.connectPanel.SetActive(false);
		this.label.gameObject.SetActive(false);
		this.plashkaLabel.SetActive(false);
		this.friendsPanel.SetActive(true);
		ActivityIndicator.IsActiveIndicator = false;
	}

	public void ConnectToRoom(int _game_mode, string _room_name, string _map)
	{
		string str;
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.OnEsc), "Connect To Friend");
		InfoWindowController.HideCurrentWindow();
		this.SetEnabledPasswordPanel(false);
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(_map));
		if (infoScene.isPremium)
		{
			if ((Storager.getInt(string.Concat(infoScene.NameScene, "Key"), true) == 1 ? false : !PremiumAccountController.MapAvailableDueToPremiumAccount(infoScene.NameScene)))
			{
				if (this.objectForOffWhenUlockDialog != null)
				{
					this.objectForOffWhenUlockDialog.SetActive(false);
				}
				this.ShowUnlockMapDialog(() => {
				}, infoScene.NameScene);
				return;
			}
		}
		int num = (_game_mode <= 99 ? _game_mode / 10 : _game_mode % 100 / 10);
		this.game_mode = _game_mode % 10;
		this.room_name = _room_name;
		Defs.isMulti = true;
		Defs.isInet = true;
		Defs.isFlag = false;
		Defs.isCOOP = false;
		Defs.isCompany = false;
		Defs.isHunger = false;
		Defs.isCapturePoints = false;
		switch (this.game_mode)
		{
			case 0:
			{
				StoreKitEventListener.State.Mode = "Deathmatch Wordwide";
				ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.Deathmatch;
				break;
			}
			case 1:
			{
				StoreKitEventListener.State.Mode = "Time Survival";
				Defs.isCOOP = true;
				ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.TimeBattle;
				break;
			}
			case 2:
			{
				StoreKitEventListener.State.Mode = "Team Battle";
				Defs.isCompany = true;
				ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.TeamFight;
				break;
			}
			case 3:
			{
				if (false)
				{
					if (ShowNoJoinConnectFromRanks.sharedController != null)
					{
						ShowNoJoinConnectFromRanks.sharedController.resetShow(3);
					}
					return;
				}
				Defs.isHunger = true;
				StoreKitEventListener.State.Mode = "Deadly Games";
				ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.DeadlyGames;
				break;
			}
			case 4:
			{
				if (false)
				{
					StoreKitEventListener.State.Mode = "Flag Capture";
					if (ShowNoJoinConnectFromRanks.sharedController != null)
					{
						ShowNoJoinConnectFromRanks.sharedController.resetShow(4);
					}
					return;
				}
				Defs.isFlag = true;
				ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.FlagCapture;
				break;
			}
			case 5:
			{
				Defs.isCapturePoints = true;
				ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.CapturePoints;
				break;
			}
			default:
			{
				return;
			}
		}
		ActivityIndicator.IsActiveIndicator = true;
		this.oldActivFriendPanel = this.friendsPanel.activeSelf;
		if (JoinRoomFromFrends.friendProfilePanel != null)
		{
			this.oldActivProfileProfile = JoinRoomFromFrends.friendProfilePanel.activeSelf;
		}
		this.connectPanel.SetActive(true);
		this.friendsPanel.SetActive(false);
		if (JoinRoomFromFrends.friendProfilePanel != null)
		{
			JoinRoomFromFrends.friendProfilePanel.SetActive(false);
		}
		this.label.gameObject.SetActive(false);
		this.plashkaLabel.SetActive(false);
		UnityEngine.Debug.Log(string.Concat("fonConnectTexture.mainTexture=", _map, " ", infoScene.NameScene));
		Defs.isDaterRegim = (!Defs.filterMaps.ContainsKey(infoScene.NameScene) ? false : infoScene.AvaliableWeapon == ModeWeapon.dater);
		WeaponManager.sharedManager.Reset((!Defs.isDaterRegim ? 0 : 3));
		base.StartCoroutine(this.SetFonLoadingWaitForReset(infoScene.NameScene, false));
		string[] separator = new string[] { Initializer.Separator, ConnectSceneNGUIController.regim.ToString(), null, null, null };
		if (!Defs.isDaterRegim)
		{
			str = (!Defs.isHunger ? num.ToString() : "0");
		}
		else
		{
			str = "Dater";
		}
		separator[2] = str;
		separator[3] = "v";
		separator[4] = GlobalGameController.MultiplayerProtocolVersion;
		string str1 = string.Concat(separator);
		ConnectSceneNGUIController.gameTier = num;
		UnityEngine.Debug.Log(string.Concat("Connect -", str1));
		PhotonNetwork.autoJoinLobby = false;
		PhotonNetwork.ConnectUsingSettings(str1);
	}

	[Obfuscation(Exclude=true)]
	private void ConnectToRoom()
	{
		PhotonNetwork.playerName = ProfileController.GetPlayerNameOrDefault();
		UnityEngine.Debug.Log(string.Concat("OnJoinedLobby ", this.room_name));
		PhotonNetwork.JoinRoom(this.room_name);
		PlayerPrefs.SetString("RoomName", this.room_name);
	}

	public void EnterPassword(string pass)
	{
		if (pass != this.passwordRoom)
		{
			this.timerShowWrongPassword = 3f;
			this.WrongPasswordLabel.SetActive(true);
		}
		else
		{
			PhotonNetwork.isMessageQueueRunning = false;
			base.StartCoroutine(this.MoveToGameScene());
			ActivityIndicator.IsActiveIndicator = true;
		}
	}

	private void HandleCloseUnlockDialog(UnlockPremiumMapView unlockPremiumMapView)
	{
		if (this.objectForOffWhenUlockDialog != null)
		{
			this.objectForOffWhenUlockDialog.SetActive(true);
		}
		this.closeConnectPanel();
		UnityEngine.Object.Destroy(unlockPremiumMapView.gameObject);
	}

	private void HandleUnlockPressed(UnlockPremiumMapView unlockPremiumMapView, Action successfulUnlockCallback, string levelName)
	{
		int price = unlockPremiumMapView.Price;
		ShopNGUIController.TryToBuy((FriendsWindowGUI.Instance == null ? unlockPremiumMapView.gameObject : FriendsWindowGUI.Instance.gameObject), new ItemPrice(unlockPremiumMapView.Price, "Coins"), () => {
			Storager.setInt(string.Concat(levelName, "Key"), 1, true);
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				PlayerPrefs.Save();
			}
			ShopNGUIController.SynchronizeAndroidPurchases(string.Concat("Friend's map unlocked: ", levelName));
			FlurryPluginWrapper.LogEvent(string.Concat("Unlock ", levelName, " map"));
			AnalyticsStuff.LogSales(levelName, "Premium Maps", false);
			AnalyticsFacade.InAppPurchase(levelName, "Premium Maps", 1, price, "Coins");
			if (coinsPlashka.thisScript != null)
			{
				coinsPlashka.thisScript.enabled = false;
			}
			successfulUnlockCallback();
			UnityEngine.Object.Destroy(unlockPremiumMapView.gameObject);
		}, () => {
			FlurryPluginWrapper.LogEvent(string.Concat("Try_Enable ", levelName, " map"));
			StoreKitEventListener.State.PurchaseKey = "In map selection In Friends";
		}, null, null, null, null);
	}

	[DebuggerHidden]
	private IEnumerator MoveToGameScene()
	{
		return new JoinRoomFromFrends.u003cMoveToGameSceneu003ec__Iterator91();
	}

	public void OnConnectedToMaster()
	{
		this.ConnectToRoom();
	}

	private void OnDestroy()
	{
		JoinRoomFromFrends.sharedJoinRoomFromFrends = null;
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	private void OnDisable()
	{
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	private void OnDisconnectedFromPhoton()
	{
		if (this.isFaledConnectToRoom)
		{
			this.ShowLabel("Game is unavailable...");
		}
		else if (!this.isBackFromPassword)
		{
			this.ShowLabel("Can't connect ...");
		}
		else
		{
			this.closeConnectPanel();
		}
		this.isFaledConnectToRoom = false;
		this.isBackFromPassword = false;
		UnityEngine.Debug.Log("OnDisconnectedFromPhoton");
	}

	private void OnEnable()
	{
		PhotonObjectCacher.AddObject(base.gameObject);
	}

	private void OnEsc()
	{
		PhotonNetwork.Disconnect();
		this.closeConnectPanel();
	}

	private void OnFailedToConnectToPhoton(object parameters)
	{
		this.ShowLabel("Can't connect ...");
		UnityEngine.Debug.Log(string.Concat("OnFailedToConnectToPhoton. StatusCode: ", parameters));
	}

	public void OnJoinedLobby()
	{
		this.ConnectToRoom();
	}

	private void OnJoinedRoom()
	{
		UnityEngine.Debug.Log("OnJoinedRoom - init");
		GlobalGameController.healthMyPlayer = 0f;
		if (PhotonNetwork.room == null)
		{
			PhotonNetwork.Disconnect();
			this.ShowLabel("Game is unavailable...");
		}
		else
		{
			this.passwordRoom = PhotonNetwork.room.customProperties[ConnectSceneNGUIController.passwordProperty].ToString();
			PhotonNetwork.isMessageQueueRunning = false;
			if (!this.passwordRoom.Equals(string.Empty))
			{
				UnityEngine.Debug.Log(string.Concat("Show Password Panel ", this.passwordRoom));
				ActivityIndicator.IsActiveIndicator = false;
				this.inputPassworLabel.@value = string.Empty;
				this.SetEnabledPasswordPanel(true);
			}
			else
			{
				PhotonNetwork.isMessageQueueRunning = false;
				base.StartCoroutine(this.MoveToGameScene());
			}
		}
	}

	private void OnPhotonJoinRoomFailed()
	{
		UnityEngine.Debug.Log("OnPhotonJoinRoomFailed - init");
		this.isFaledConnectToRoom = true;
		PhotonNetwork.Disconnect();
		InfoWindowController.ShowInfoBox(LocalizationStore.Get("Key_0137"));
		InfoWindowController.HideProcessing(3f);
		Defs.isFlag = false;
		Defs.isCOOP = false;
		Defs.isCompany = false;
		Defs.isHunger = false;
		Defs.isCapturePoints = false;
		Defs.isDaterRegim = false;
		WeaponManager.sharedManager.Reset(0);
	}

	private void RemoveLoadingGUI()
	{
		if (this._loadingNGUIController != null)
		{
			UnityEngine.Object.Destroy(this._loadingNGUIController.gameObject);
			this._loadingNGUIController = null;
		}
	}

	private void SetEnabledPasswordPanel(bool enabled)
	{
		Texture texture;
		this.PasswordPanel.SetActive(enabled);
		if (this._loadingNGUIController != null)
		{
			this.fonConnectTexture.gameObject.SetActive(enabled);
			UITexture uITexture = this.fonConnectTexture;
			if (!enabled)
			{
				texture = null;
			}
			else
			{
				texture = this._loadingNGUIController.loadingNGUITexture.mainTexture;
			}
			uITexture.mainTexture = texture;
			this._loadingNGUIController.gameObject.SetActive(!enabled);
		}
	}

	[DebuggerHidden]
	private IEnumerator SetFonLoadingWaitForReset(string _mapName = "", bool isAddCountRun = false)
	{
		JoinRoomFromFrends.u003cSetFonLoadingWaitForResetu003ec__Iterator90 variable = null;
		return variable;
	}

	private void ShowLabel(string text)
	{
		this.label.text = text;
		this.label.gameObject.SetActive(true);
		this.plashkaLabel.SetActive(true);
		ActivityIndicator.IsActiveIndicator = false;
		base.Invoke("closeConnectPanel", 3f);
	}

	private void ShowLoadingGUI(string _mapName)
	{
		this._loadingNGUIController = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("LoadingGUI")).GetComponent<LoadingNGUIController>();
		this._loadingNGUIController.SceneToLoad = _mapName;
		this._loadingNGUIController.loadingNGUITexture.mainTexture = Resources.Load<Texture2D>(string.Concat("LevelLoadings", (!Device.isRetinaAndStrong ? string.Empty : "/Hi"), "/Loading_", _mapName));
		this._loadingNGUIController.transform.parent = this.fonConnectTexture.transform.parent;
		this._loadingNGUIController.transform.localPosition = Vector3.zero;
		this._loadingNGUIController.Init();
	}

	private void ShowUnlockMapDialog(Action successfulUnlockCallback, string levelName)
	{
		if (string.IsNullOrEmpty(levelName))
		{
			UnityEngine.Debug.LogWarning("Level name shoul not be empty.");
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UnlockPremiumMapView")) as GameObject;
		gameObject.transform.parent = base.transform;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		Tools.SetLayerRecursively(gameObject, base.gameObject.layer);
		ActivityIndicator.IsActiveIndicator = false;
		UnlockPremiumMapView component = gameObject.GetComponent<UnlockPremiumMapView>();
		if (component == null)
		{
			UnityEngine.Debug.LogError("UnlockPremiumMapView should not be null.");
			return;
		}
		int num = 0;
		Defs.PremiumMaps.TryGetValue(levelName, out num);
		component.Price = num;
		EventHandler eventHandler = (object sender, EventArgs e) => this.HandleCloseUnlockDialog(component);
		EventHandler eventHandler1 = (object sender, EventArgs e) => this.HandleUnlockPressed(component, successfulUnlockCallback, levelName);
		component.ClosePressed += eventHandler;
		component.UnlockPressed += eventHandler1;
	}

	private void Start()
	{
		JoinRoomFromFrends.sharedJoinRoomFromFrends = this;
	}

	private void Update()
	{
		if (coinsPlashka.thisScript != null)
		{
			coinsPlashka.thisScript.enabled = (coinsShop.thisScript == null ? false : coinsShop.thisScript.enabled);
		}
		if (this.timerShowWrongPassword > 0f && this.WrongPasswordLabel.activeSelf)
		{
			this.timerShowWrongPassword -= Time.deltaTime;
		}
		if (this.timerShowWrongPassword <= 0f && this.WrongPasswordLabel.activeSelf)
		{
			this.WrongPasswordLabel.SetActive(false);
		}
	}
}