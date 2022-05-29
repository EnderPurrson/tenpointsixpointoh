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

public class JoinToFriendRoomController : MonoBehaviour
{
	public int gameMode;

	public string roomName;

	public GameObject connectPanel;

	public UITexture backgroundConnectTexture;

	public UILabel infoBoxLabel;

	public GameObject infoBoxContainer;

	public GameObject passwordPanel;

	public GameObject wrongPasswordLabel;

	public UIInput inputPasswordLabel;

	public static JoinToFriendRoomController Instance;

	private bool _isFaledConnectToRoom;

	private string _passwordRoom;

	private float _timerShowWrongPassword;

	private bool _isBackFromPassword;

	private IDisposable _backSubscription;

	private LoadingNGUIController _loadingNGUIController;

	public JoinToFriendRoomController()
	{
	}

	private void Awake()
	{
		this.inputPasswordLabel.onSubmit.Add(new EventDelegate(() => this.EnterPassword(this.inputPasswordLabel.@value)));
	}

	public void BackFromPasswordButton()
	{
		this._isBackFromPassword = true;
		this.SetEnabledPasswordPanel(false);
		PhotonNetwork.Disconnect();
	}

	[Obfuscation(Exclude=true)]
	public void closeConnectPanel()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
		this.backgroundConnectTexture.mainTexture = null;
		this.RemoveLoadingGUI();
		this.connectPanel.SetActive(false);
		this.infoBoxLabel.gameObject.SetActive(false);
		this.infoBoxContainer.SetActive(false);
		ActivityIndicator.IsActiveIndicator = false;
	}

	public void ConnectToRoom(int gameModeCode, string nameRoom, string map)
	{
		string str;
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.OnEsc), "Connect To Friend");
		InfoWindowController.HideCurrentWindow();
		this.SetEnabledPasswordPanel(false);
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(map));
		if (infoScene.isPremium)
		{
			if ((Storager.getInt(string.Concat(infoScene.NameScene, "Key"), true) == 1 ? false : !PremiumAccountController.MapAvailableDueToPremiumAccount(infoScene.NameScene)))
			{
				this.ShowUnlockMapDialog(() => {
				}, infoScene.NameScene);
				return;
			}
		}
		int num = (gameModeCode <= 99 ? gameModeCode / 10 : gameModeCode % 100 / 10);
		this.gameMode = gameModeCode % 10;
		this.roomName = nameRoom;
		Defs.isMulti = true;
		Defs.isInet = true;
		Defs.isFlag = false;
		Defs.isCOOP = false;
		Defs.isCompany = false;
		Defs.isHunger = false;
		Defs.isCapturePoints = false;
		switch (this.gameMode)
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
		this.connectPanel.SetActive(true);
		this.infoBoxLabel.gameObject.SetActive(false);
		this.infoBoxContainer.SetActive(false);
		WeaponManager.sharedManager.Reset((int)infoScene.AvaliableWeapon);
		base.StartCoroutine(this.SetFonLoadingWaitForReset(infoScene.NameScene, false));
		Defs.isDaterRegim = infoScene.AvaliableWeapon == ModeWeapon.dater;
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
		PhotonNetwork.autoJoinLobby = false;
		PhotonNetwork.ConnectUsingSettings(str1);
	}

	[Obfuscation(Exclude=true)]
	private void ConnectToRoom()
	{
		PhotonNetwork.playerName = ProfileController.GetPlayerNameOrDefault();
		UnityEngine.Debug.Log(string.Concat("OnJoinedLobby ", this.roomName));
		PhotonNetwork.JoinRoom(this.roomName);
		PlayerPrefs.SetString("RoomName", this.roomName);
	}

	public void EnterPassword(string pass)
	{
		if (pass != this._passwordRoom)
		{
			this._timerShowWrongPassword = 3f;
			this.wrongPasswordLabel.SetActive(true);
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
		this.closeConnectPanel();
		UnityEngine.Object.Destroy(unlockPremiumMapView.gameObject);
	}

	private void HandleUnlockPressed(UnlockPremiumMapView unlockPremiumMapView, Action successfulUnlockCallback, string levelName)
	{
		int price = unlockPremiumMapView.Price;
		ShopNGUIController.TryToBuy(base.transform.root.gameObject, new ItemPrice(unlockPremiumMapView.Price, "Coins"), () => {
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
		return new JoinToFriendRoomController.u003cMoveToGameSceneu003ec__Iterator142();
	}

	public void OnClickAcceptPassword()
	{
		this.EnterPassword(this.inputPasswordLabel.@value);
	}

	public void OnConnectedToMaster()
	{
		this.ConnectToRoom();
	}

	private void OnDestroy()
	{
		JoinToFriendRoomController.Instance = null;
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	private void OnDisable()
	{
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	private void OnDisconnectedFromPhoton()
	{
		if (this._isFaledConnectToRoom)
		{
			this.ShowLabel(LocalizationStore.Get("Key_1410"));
		}
		else if (!this._isBackFromPassword)
		{
			this.ShowLabel(LocalizationStore.Get("Key_1410"));
		}
		else
		{
			this.closeConnectPanel();
		}
		this._isFaledConnectToRoom = false;
		this._isBackFromPassword = false;
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
		this.ShowLabel(LocalizationStore.Get("Key_1411"));
		UnityEngine.Debug.Log(string.Concat("OnFailedToConnectToPhoton. StatusCode: ", parameters));
	}

	public void OnJoinedLobby()
	{
		this.ConnectToRoom();
	}

	private void OnJoinedRoom()
	{
		UnityEngine.Debug.Log("OnJoinedRoom - init");
		if (PhotonNetwork.room == null)
		{
			PhotonNetwork.Disconnect();
			this.ShowLabel(LocalizationStore.Get("Key_1410"));
		}
		else
		{
			this._passwordRoom = PhotonNetwork.room.customProperties[ConnectSceneNGUIController.passwordProperty].ToString();
			PhotonNetwork.isMessageQueueRunning = false;
			if (!this._passwordRoom.Equals(string.Empty))
			{
				UnityEngine.Debug.Log(string.Concat("Show Password Panel ", this._passwordRoom));
				ActivityIndicator.IsActiveIndicator = false;
				this.inputPasswordLabel.@value = string.Empty;
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
		this._isFaledConnectToRoom = true;
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
		this.passwordPanel.SetActive(enabled);
		if (this._loadingNGUIController != null)
		{
			UITexture uITexture = this.backgroundConnectTexture;
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
		JoinToFriendRoomController.u003cSetFonLoadingWaitForResetu003ec__Iterator141 variable = null;
		return variable;
	}

	private void ShowLabel(string text)
	{
		this.infoBoxLabel.text = text;
		this.infoBoxLabel.gameObject.SetActive(true);
		this.infoBoxContainer.SetActive(true);
		ActivityIndicator.IsActiveIndicator = false;
		base.Invoke("closeConnectPanel", 3f);
	}

	private void ShowLoadingGUI(string _mapName)
	{
		this._loadingNGUIController = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("LoadingGUI")).GetComponent<LoadingNGUIController>();
		this._loadingNGUIController.SceneToLoad = _mapName;
		this._loadingNGUIController.loadingNGUITexture.mainTexture = Resources.Load<Texture2D>(string.Concat("LevelLoadings", (!Device.isRetinaAndStrong ? string.Empty : "/Hi"), "/Loading_", _mapName));
		this._loadingNGUIController.transform.parent = this.backgroundConnectTexture.transform.parent;
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
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
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
		JoinToFriendRoomController.Instance = this;
	}

	private void Update()
	{
		if (coinsPlashka.thisScript != null)
		{
			coinsPlashka.thisScript.enabled = (coinsShop.thisScript == null ? false : coinsShop.thisScript.enabled);
		}
		if (this._timerShowWrongPassword > 0f && this.wrongPasswordLabel.activeSelf)
		{
			this._timerShowWrongPassword -= Time.deltaTime;
		}
		if (this._timerShowWrongPassword <= 0f && this.wrongPasswordLabel.activeSelf)
		{
			this.wrongPasswordLabel.SetActive(false);
		}
	}
}