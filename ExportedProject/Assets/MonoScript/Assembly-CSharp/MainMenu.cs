using Prime31;
using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class MainMenu : MonoBehaviour
{
	public static MainMenu sharedMenu;

	public GameObject JoysticksUIRoot;

	public static bool BlockInterface;

	public static bool IsAdvertRun;

	private bool isShowDeadMatch;

	private bool isShowCOOP;

	public bool isFirstFrame = true;

	public bool isInappWinOpen;

	private bool musicOld;

	private bool fxOld;

	public Texture inAppFon;

	public GUIStyle puliInApp;

	public GUIStyle healthInApp;

	public GUIStyle pulemetInApp;

	public GUIStyle crystalSwordInapp;

	public GUIStyle elixirInapp;

	private bool showUnlockDialog;

	private bool isPressFullOnMulty;

	private float _timeWhenPurchShown;

	public GameObject skinsManagerPrefab;

	public GameObject weaponManagerPrefab;

	public GUIStyle backBut;

	private ExperienceController expController;

	private AdvertisementController _advertisementController;

	public bool isShowAvard;

	public readonly static string iTunesEnderManID;

	private static bool firstEnterLobbyAtThisLaunch;

	private bool _skinsMakerQuerySucceeded;

	public static int FontSizeForMessages
	{
		get
		{
			return Mathf.RoundToInt((float)Screen.height * 0.03f);
		}
	}

	public static float iOSVersion
	{
		get
		{
			float single = -1f;
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				string str = SystemInfo.operatingSystem.Replace("iPhone OS ", string.Empty);
				float.TryParse(str.Substring(0, 1), out single);
			}
			return single;
		}
	}

	public static string RateUsURL
	{
		get
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				return "https://play.google.com/store/apps/details?id=com.pixel.gun3d&hl=en";
			}
			return Defs2.ApplicationUrl;
		}
	}

	static MainMenu()
	{
		MainMenu.iTunesEnderManID = "811995374";
		MainMenu.firstEnterLobbyAtThisLaunch = true;
	}

	public MainMenu()
	{
	}

	private void Awake()
	{
		if (!MainMenu.firstEnterLobbyAtThisLaunch)
		{
			using (StopwatchLogger stopwatchLogger = new StopwatchLogger("MainMenu.Awake()"))
			{
				GlobalGameController.SetMultiMode();
				if (WeaponManager.sharedManager != null)
				{
					WeaponManager.sharedManager.Reset(0);
				}
				else if (!WeaponManager.sharedManager && this.weaponManagerPrefab)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(this.weaponManagerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
					gameObject.GetComponent<WeaponManager>().Reset(0);
				}
			}
		}
		else
		{
			MainMenu.firstEnterLobbyAtThisLaunch = false;
			GlobalGameController.SetMultiMode();
		}
	}

	private void completionHandler(string error, object result)
	{
		if (error == null)
		{
			Utils.logObject(result);
		}
		else
		{
			UnityEngine.Debug.LogError(error);
		}
	}

	public static string GetEndermanUrl()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.isEditor)
		{
			return "https://itunes.apple.com/app/apple-store/id811995374?pt=1579002&ct=pgapp&mt=8-";
		}
		if (Application.platform != RuntimePlatform.Android)
		{
			return string.Empty;
		}
		return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon ? "https://play.google.com/store/apps/details?id=com.slender.android" : "http://www.amazon.com/Pocket-Slenderman-Rising-your-virtual/dp/B00I6IXU5A/ref=sr_1_5?s=mobile-apps&ie=UTF8&qid=1395990920&sr=1-5&keywords=slendy");
	}

	private void OnDestroy()
	{
		MainMenu.sharedMenu = null;
		if (this.expController != null)
		{
			this.expController.isShowRanks = false;
			this.expController.isMenu = false;
		}
	}

	private void SetInApp()
	{
		this.isInappWinOpen = !this.isInappWinOpen;
		if (this.expController != null)
		{
			this.expController.isShowRanks = !this.isInappWinOpen;
			this.expController.isMenu = !this.isInappWinOpen;
		}
		if (!this.isInappWinOpen)
		{
			ActivityIndicator.IsActiveIndicator = false;
		}
		else
		{
			if (StoreKitEventListener.restoreInProcess)
			{
				ActivityIndicator.IsActiveIndicator = true;
			}
			if (!Defs.isMulti)
			{
				Time.timeScale = 0f;
			}
		}
	}

	public static bool SkinsMakerSupproted()
	{
		bool buildTargetPlatform = BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64;
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
		{
			buildTargetPlatform = Defs.AndroidEdition != Defs.RuntimeAndroidEdition.GoogleLite;
		}
		return buildTargetPlatform;
	}

	private void Start()
	{
		using (StopwatchLogger stopwatchLogger = new StopwatchLogger("MainMenu.Start()"))
		{
			MainMenu.sharedMenu = this;
			StoreKitEventListener.State.Mode = "In_main_menu";
			StoreKitEventListener.State.PurchaseKey = "In shop";
			StoreKitEventListener.State.Parameters.Clear();
			if (EveryplayWrapper.Instance.CurrentState == EveryplayWrapper.State.Paused || EveryplayWrapper.Instance.CurrentState == EveryplayWrapper.State.Recording)
			{
				EveryplayWrapper.Instance.Stop();
			}
			if (!FriendsController.sharedController.dataSent)
			{
				FriendsController.sharedController.InitOurInfo();
				FriendsController.sharedController.StartCoroutine(FriendsController.sharedController.WaitForReadyToOperateAndUpdatePlayer());
				FriendsController.sharedController.dataSent = true;
			}
			if (NotificationController.isGetEveryDayMoney)
			{
				this.isShowAvard = true;
			}
			bool flag = false;
			this.expController = ExperienceController.sharedController;
			if (this.expController == null)
			{
				UnityEngine.Debug.LogError("MainMenu.Start():    expController == null");
			}
			if (this.expController != null)
			{
				this.expController.isMenu = true;
			}
			float coef = Defs.Coef;
			if (this.expController != null)
			{
				this.expController.posRanks = new Vector2(21f * Defs.Coef, 21f * Defs.Coef);
			}
			if (PlayerPrefs.GetString(Defs.ShouldReoeatActionSett, string.Empty).Equals(Defs.GoToProfileAction))
			{
				PlayerPrefs.SetString(Defs.ShouldReoeatActionSett, string.Empty);
				PlayerPrefs.Save();
			}
			Storager.setInt(Defs.EarnedCoins, 0, false);
			base.Invoke("setEnabledGUI", 0.1f);
			ActivityIndicator.IsActiveIndicator = true;
			PlayerPrefs.SetInt("typeConnect__", -1);
			if (!GameObject.FindGameObjectWithTag("SkinsManager") && this.skinsManagerPrefab)
			{
				UnityEngine.Object.Instantiate(this.skinsManagerPrefab, Vector3.zero, Quaternion.identity);
			}
			if (!WeaponManager.sharedManager && this.weaponManagerPrefab)
			{
				UnityEngine.Object.Instantiate(this.weaponManagerPrefab, Vector3.zero, Quaternion.identity);
			}
			GlobalGameController.ResetParameters();
			GlobalGameController.Score = 0;
			bool flag1 = false;
			if (PlayerPrefs.GetInt(Defs.ShouldEnableShopSN, 0) == 1)
			{
				flag1 = true;
				PlayerPrefs.SetInt(Defs.ShouldEnableShopSN, 0);
				PlayerPrefs.Save();
			}
			if (Tools.RuntimePlatform != RuntimePlatform.MetroPlayerX64 && (Application.platform != RuntimePlatform.Android || Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon) && Defs.EnderManAvailable && !flag1 && !flag && !this.isShowAvard && PlayerPrefs.GetInt(Defs.ShowEnder_SN, 0) == 1)
			{
				float num = PlayerPrefs.GetFloat(Defs.TimeFromWhichShowEnder_SN, 0f);
				float single = Switcher.SecondsFrom1970() - num;
				UnityEngine.Debug.Log(string.Concat("diff mainmenu: ", single));
				if (single >= (Application.isEditor || UnityEngine.Debug.isDebugBuild ? 0f : 86400f))
				{
					PlayerPrefs.SetInt(Defs.ShowEnder_SN, 0);
					UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Ender") as GameObject);
				}
			}
		}
	}

	private void Update()
	{
		object obj;
		float coef = (float)Screen.width - 42f * Defs.Coef;
		float single = Defs.Coef;
		if (!MainMenu.SkinsMakerSupproted())
		{
			obj = null;
		}
		else
		{
			obj = 262;
		}
		float single1 = (coef - single * (672f + (float)obj)) / (!MainMenu.SkinsMakerSupproted() ? 2f : 3f);
		if (this.expController != null)
		{
			this.expController.posRanks = new Vector2(21f * Defs.Coef, 21f * Defs.Coef);
		}
	}

	[DebuggerHidden]
	private IEnumerator WaitForExperienceGuiAndAdd(ExperienceController legacyExperienceController, int addend)
	{
		MainMenu.u003cWaitForExperienceGuiAndAddu003ec__IteratorA9 variable = null;
		return variable;
	}
}