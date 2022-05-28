using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Facebook.Unity;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

public class FacebookController : MonoBehaviour
{
	public enum StoryPriority
	{
		Green = 0,
		Red = 1,
		MultyWinLimit = 2,
		ArenaLimit = 3
	}

	[CompilerGenerated]
	private sealed class _003CCanPostStoryWithPriority_003Ec__AnonStorey340
	{
		internal StoryPriority priority;

		internal bool _003C_003Em__53A(Dictionary<string, object> rec)
		{
			return int.Parse((string)rec["priority"]) == (int)priority;
		}
	}

	[CompilerGenerated]
	private sealed class _003CCleanStoryPostHistory_003Ec__AnonStorey341
	{
		internal long minimumValidTime;

		internal bool _003C_003Em__53B(Dictionary<string, object> entry)
		{
			return long.Parse((string)entry["time"]) < minimumValidTime;
		}
	}

	[CompilerGenerated]
	private sealed class _003CFBGet_003Ec__AnonStorey342
	{
		internal Action<IDictionary<string, object>> act;

		internal Action<IResult> onError;

		internal void _003C_003Em__53C(IGraphResult result)
		{
			try
			{
				PrintFBResult(result);
				act(result.ResultDictionary);
			}
			catch (Exception ex)
			{
				if (onError != null)
				{
					onError(result);
				}
				Debug.LogError("Exception FBGet: " + ex);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CLogin_003Ec__AnonStorey343
	{
		internal string context;

		internal Action onSuccess;

		internal Action onSuccessAfterPublishPermissions;

		internal Action onFailure;

		internal void _003C_003Em__53D(ILoginResult result)
		{
			LoggingIn = false;
			PrintFBResult(result);
			CheckAndGiveFacebookReward(context);
			if (FB.IsLoggedIn)
			{
				if (onSuccess != null)
				{
					onSuccess();
				}
				try
				{
					Action<string> receivedSelfID = FacebookController.ReceivedSelfID;
					if (receivedSelfID != null)
					{
						receivedSelfID((string)result.ResultDictionary["user_id"]);
					}
				}
				catch (Exception ex)
				{
					Debug.LogError("FacebookController Login ReceivedSelfID exception: " + ex);
				}
				try
				{
					if (sharedController != null)
					{
						sharedController.InputFacebookFriends();
					}
				}
				catch (Exception ex2)
				{
					Debug.LogError("FacebookController Login InputFacebookFriends exception: " + ex2);
				}
				CoroutineRunner.Instance.StartCoroutine(RunActionAfterDelay(_003C_003Em__548));
			}
			else if (onFailure != null)
			{
				onFailure();
			}
		}

		internal void _003C_003Em__548()
		{
			LoggingIn = true;
			try
			{
				FB.LogInWithPublishPermissions(new List<string> { "publish_actions" }, _003C_003Em__549);
			}
			catch (Exception ex)
			{
				Debug.LogError("Exception in Facebook Login: " + ex);
				LoggingIn = false;
			}
		}

		internal void _003C_003Em__549(ILoginResult publishLoginResult)
		{
			LoggingIn = false;
			PrintFBResult(publishLoginResult);
			if (string.IsNullOrEmpty(publishLoginResult.Error) && !publishLoginResult.Cancelled && onSuccessAfterPublishPermissions != null)
			{
				onSuccessAfterPublishPermissions();
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CPostOpenGraphStory_003Ec__AnonStorey344
	{
		internal string obj;

		internal Dictionary<string, string> pars;

		internal string action;

		internal StoryPriority priority;

		private static Action<IDictionary<string, object>> _003C_003Ef__am_0024cache4;

		internal void _003C_003Em__53E()
		{
			string text = "https://secure.pixelgunserver.com/fb/ogobjects.php?type=" + WWW.EscapeURL(this.obj);
			if (pars != null)
			{
				foreach (KeyValuePair<string, string> par in pars)
				{
					string text2 = text;
					text = text2 + "&" + par.Key.Replace(" "[0], "_"[0]) + "=" + WWW.EscapeURL(par.Value);
				}
			}
			string graphPath = "/me/pixelgun:" + action;
			Dictionary<string, string> obj = new Dictionary<string, string>
			{
				{ this.obj, text },
				{ "fb:explicitly_shared", "true" }
			};
			if (_003C_003Ef__am_0024cache4 == null)
			{
				_003C_003Ef__am_0024cache4 = _003C_003Em__54A;
			}
			FBPost(graphPath, obj, _003C_003Ef__am_0024cache4, _003C_003Em__54B);
		}

		private static void _003C_003Em__54A(IDictionary<string, object> result)
		{
		}

		internal void _003C_003Em__54B(IResult result)
		{
			if (result != null && result.Error == null)
			{
				RegisterStoryPostedWithPriority(priority);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CFBPost_003Ec__AnonStorey345
	{
		internal Action<IResult> actionWithFBResult;

		internal Action<IDictionary<string, object>> act;

		internal void _003C_003Em__53F(IGraphResult result)
		{
			try
			{
				if (actionWithFBResult != null)
				{
					actionWithFBResult(result);
				}
				PrintFBResult(result);
				act(result.ResultDictionary);
			}
			catch (Exception ex)
			{
				Debug.LogError("Exception FBPost: " + ex);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CSetMyId_003Ec__AnonStorey346
	{
		internal Action onSuccess;

		internal FacebookController _003C_003Ef__this;

		internal void _003C_003Em__540()
		{
			string graphPath = "/me/friends?fields=id,name,installed&limit=1000000";
			FBGet(graphPath, _003C_003Em__54C);
		}

		internal void _003C_003Em__54C(IDictionary<string, object> result)
		{
			IList list = result["data"] as IList;
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("Result facebook friends" + result.ToString());
			}
			_003C_003Ef__this.friendsList.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				IDictionary dictionary = list[i] as IDictionary;
				_003C_003Ef__this.friendsList.Add(new Friend(dictionary["name"] as string, dictionary["id"].ToString(), dictionary["installed"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase)));
			}
			if (onSuccess != null)
			{
				onSuccess();
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CRunApiWithAskForPermissions_003Ec__AnonStorey347
	{
		private sealed class _003CRunApiWithAskForPermissions_003Ec__AnonStorey348
		{
			internal bool loginIfNoPermissions;

			internal _003CRunApiWithAskForPermissions_003Ec__AnonStorey347 _003C_003Ef__ref_0024839;

			private static Func<object, bool> _003C_003Ef__am_0024cache2;

			private static Func<object, string> _003C_003Ef__am_0024cache3;

			internal void _003C_003Em__54D(IDictionary<string, object> result)
			{
				List<object> source = result["data"] as List<object>;
				if (_003C_003Ef__am_0024cache2 == null)
				{
					_003C_003Ef__am_0024cache2 = _003C_003Em__552;
				}
				IEnumerable<object> source2 = source.Where(_003C_003Ef__am_0024cache2);
				if (_003C_003Ef__am_0024cache3 == null)
				{
					_003C_003Ef__am_0024cache3 = _003C_003Em__553;
				}
				List<string> list = source2.Select(_003C_003Ef__am_0024cache3).ToList();
				if (list.Contains(_003C_003Ef__ref_0024839.requiredPermission))
				{
					_003C_003Ef__ref_0024839.runApi();
				}
				else if (loginIfNoPermissions)
				{
					_003C_003Ef__ref_0024839.loginWithRequiredPermissions(false);
				}
				else if (_003C_003Ef__ref_0024839.onFailToRunApi != null)
				{
					_003C_003Ef__ref_0024839.onFailToRunApi();
				}
			}

			internal void _003C_003Em__54E(IResult result)
			{
				if (result != null && result.RawResult != null && result.RawResult.Contains("OAuthException"))
				{
					_003C_003Ef__ref_0024839.loginWithRequiredPermissionsThroughLoginMethod();
					_003C_003Ef__ref_0024839.countTryingUpdateToken++;
				}
				else if (_003C_003Ef__ref_0024839.onFailToRunApi != null)
				{
					_003C_003Ef__ref_0024839.onFailToRunApi();
				}
			}

			private static bool _003C_003Em__552(object p)
			{
				return (p as Dictionary<string, object>)["status"].Equals("granted");
			}

			private static string _003C_003Em__553(object p)
			{
				return (string)(p as Dictionary<string, object>)["permission"];
			}
		}

		private sealed class _003CRunApiWithAskForPermissions_003Ec__AnonStorey349
		{
			internal Action loginWithRequiredPermissionsOneStep;

			internal _003CRunApiWithAskForPermissions_003Ec__AnonStorey347 _003C_003Ef__ref_0024839;

			internal void _003C_003Em__54F()
			{
				FacebookDelegate<ILoginResult> callback = _003C_003Em__554;
				if (_003C_003Ef__ref_0024839.requiredPermission == "publish_actions")
				{
					LoggingIn = true;
					try
					{
						FB.LogInWithPublishPermissions(new List<string> { _003C_003Ef__ref_0024839.requiredPermission }, callback);
						return;
					}
					catch (Exception ex)
					{
						Debug.LogError("Exception in Facebook Login: " + ex);
						LoggingIn = false;
						return;
					}
				}
				LoggingIn = true;
				try
				{
					FB.LogInWithReadPermissions(new List<string> { _003C_003Ef__ref_0024839.requiredPermission }, callback);
				}
				catch (Exception ex2)
				{
					Debug.LogError("Exception in Facebook Login: " + ex2);
					LoggingIn = false;
				}
			}

			internal void _003C_003Em__550(ILoginResult result)
			{
				LoggingIn = false;
				PrintFBResult(result);
				if (string.IsNullOrEmpty(result.Error) && !result.Cancelled)
				{
					CoroutineRunner.Instance.StartCoroutine(RunActionAfterDelay(loginWithRequiredPermissionsOneStep));
				}
				else
				{
					Debug.LogError("LogInWithReadPermissions: ! (string.IsNullOrEmpty(result.Error) && ! result.Cancelled)");
				}
			}

			internal void _003C_003Em__554(ILoginResult result)
			{
				LoggingIn = false;
				PrintFBResult(result);
				if (FB.IsLoggedIn)
				{
					_003C_003Ef__ref_0024839.checkPermissionsAndRunApi(false);
				}
				else if (_003C_003Ef__ref_0024839.onFailToRunApi != null)
				{
					_003C_003Ef__ref_0024839.onFailToRunApi();
				}
			}
		}

		internal string requiredPermission;

		internal Action runApi;

		internal Action<bool> loginWithRequiredPermissions;

		internal Action onFailToRunApi;

		internal Action loginWithRequiredPermissionsThroughLoginMethod;

		internal int countTryingUpdateToken;

		internal Action<bool> checkPermissionsAndRunApi;

		internal void _003C_003Em__543(bool loginIfNoPermissions)
		{
			_003CRunApiWithAskForPermissions_003Ec__AnonStorey348 _003CRunApiWithAskForPermissions_003Ec__AnonStorey = new _003CRunApiWithAskForPermissions_003Ec__AnonStorey348();
			_003CRunApiWithAskForPermissions_003Ec__AnonStorey._003C_003Ef__ref_0024839 = this;
			_003CRunApiWithAskForPermissions_003Ec__AnonStorey.loginIfNoPermissions = loginIfNoPermissions;
			if (!string.IsNullOrEmpty(requiredPermission))
			{
				FBGet("/me/permissions?limit=500", _003CRunApiWithAskForPermissions_003Ec__AnonStorey._003C_003Em__54D, _003CRunApiWithAskForPermissions_003Ec__AnonStorey._003C_003Em__54E);
			}
			else
			{
				runApi();
			}
		}

		internal void _003C_003Em__544(bool bothReadAndWriteLogins)
		{
			_003CRunApiWithAskForPermissions_003Ec__AnonStorey349 _003CRunApiWithAskForPermissions_003Ec__AnonStorey = new _003CRunApiWithAskForPermissions_003Ec__AnonStorey349();
			_003CRunApiWithAskForPermissions_003Ec__AnonStorey._003C_003Ef__ref_0024839 = this;
			_003CRunApiWithAskForPermissions_003Ec__AnonStorey.loginWithRequiredPermissionsOneStep = _003CRunApiWithAskForPermissions_003Ec__AnonStorey._003C_003Em__54F;
			if (bothReadAndWriteLogins && requiredPermission == "publish_actions")
			{
				LoggingIn = true;
				try
				{
					FB.LogInWithReadPermissions(new List<string>(), _003CRunApiWithAskForPermissions_003Ec__AnonStorey._003C_003Em__550);
					return;
				}
				catch (Exception ex)
				{
					Debug.LogError("Exception in Facebook Login: " + ex);
					LoggingIn = false;
					return;
				}
			}
			_003CRunApiWithAskForPermissions_003Ec__AnonStorey.loginWithRequiredPermissionsOneStep();
		}

		internal void _003C_003Em__545()
		{
			FB.LogOut();
			Action onSuccessAfterPublishPermissions = _003C_003Em__551;
			Login(null, null, "Unknown", onSuccessAfterPublishPermissions);
		}

		internal void _003C_003Em__551()
		{
			checkPermissionsAndRunApi(false);
		}
	}

	[CompilerGenerated]
	private sealed class _003CInvitePlayer_003Ec__AnonStorey34A
	{
		internal Action onComplete;

		internal FacebookController _003C_003Ef__this;

		internal void _003C_003Em__546()
		{
			FB.AppRequest(_003C_003Ef__this.messageInvite, null, null, null, null, string.Empty, _003C_003Ef__this.titleInvite, _003C_003Em__555);
		}

		internal void _003C_003Em__547()
		{
			_003C_003Ef__this.InvitePlayerApiIsRunning = false;
		}

		internal void _003C_003Em__555(IAppRequestResult result)
		{
			_003C_003Ef__this.InvitePlayerApiIsRunning = false;
			PrintFBResult(result);
			if (onComplete != null)
			{
				onComplete();
			}
		}
	}

	public const int MaxCountShownGunForLogin = 1;

	public const int DefaultGreenPriorityLimit = 7;

	public const int DefaultRedPriorityLimit = 3;

	private const string PriorityKey = "priority";

	private const string TimeKey = "time";

	private const string StoryPostHistoryKey = "FacebookControllerStoryPostHistoryKey";

	public const string FacebookScriptAddress = "https://secure.pixelgunserver.com/fb/ogobjects.php";

	private const string SocialGunEventStartedKey = "FacebookControllerSocialGunEventStartedKey";

	public static FacebookController sharedController;

	public string selfID = string.Empty;

	private Action<string, object> handlerForPost;

	private bool hasPublishActions;

	private bool isGetPermitionFromSendMessage;

	private string postMessage;

	public List<Friend> friendsList;

	private string titleInvite = "Invite a Friend to Play!";

	private string messageInvite = "Join me in playing a new game!";

	public static readonly Dictionary<StoryPriority, int> StoryPostLimits = new Dictionary<StoryPriority, int>
	{
		{
			StoryPriority.Green,
			7
		},
		{
			StoryPriority.Red,
			3
		}
	};

	private string _appId = string.Empty;

	private bool socialGunEventActive;

	private float _timeSinceLastStoryPostHistoryClean;

	public bool InvitePlayerApiIsRunning;

	private TimeSpan DurationSocialGunEvent = TimeSpan.FromDays(1000000.0);

	private TimeSpan TimeBetweenSocialGunBannerSeries = TimeSpan.FromHours(24.0);

	private DateTime socialEventStartTime;

	private List<Dictionary<string, object>> storiesPostHistory = new List<Dictionary<string, object>>();

	[CompilerGenerated]
	private static HideUnityDelegate _003C_003Ef__am_0024cache16;

	public static bool FacebookSupported
	{
		get
		{
			return BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64;
		}
	}

	public string AppId
	{
		get
		{
			return _appId;
		}
		set
		{
			_appId = value ?? string.Empty;
		}
	}

	public static bool FacebookPost_Old_Supported
	{
		get
		{
			return FacebookSupported && ((BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64 && sharedController != null && !sharedController.CanPostStoryWithPriority(StoryPriority.Green)) || (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64 && Defs.isMulti && PhotonNetwork.connected && NetworkStartTable.LocalOrPasswordRoom()));
		}
	}

	public bool SocialGunEventActive
	{
		get
		{
			return socialGunEventActive;
		}
	}

	public static bool LoggingIn { get; private set; }

	public static event Action<bool> SocialGunEventStateChanged;

	public static event Action<string> PostCompleted;

	public static event Action<string> ReceivedSelfID;

	public bool CanPostStoryWithPriority(StoryPriority priority)
	{
		//Discarded unreachable code: IL_006f
		_003CCanPostStoryWithPriority_003Ec__AnonStorey340 _003CCanPostStoryWithPriority_003Ec__AnonStorey = new _003CCanPostStoryWithPriority_003Ec__AnonStorey340();
		_003CCanPostStoryWithPriority_003Ec__AnonStorey.priority = priority;
		try
		{
			if (_003CCanPostStoryWithPriority_003Ec__AnonStorey.priority == StoryPriority.Green)
			{
				return storiesPostHistory.Count < StoryPostLimits[_003CCanPostStoryWithPriority_003Ec__AnonStorey.priority];
			}
			return storiesPostHistory.Where(_003CCanPostStoryWithPriority_003Ec__AnonStorey._003C_003Em__53A).Count() < StoryPostLimits[_003CCanPostStoryWithPriority_003Ec__AnonStorey.priority];
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Caught exception in CanPostStoryWithPriority:\n{0}", ex);
		}
		return false;
	}

	public string GetTimeToEndSocialGunEvent()
	{
		if (!SocialGunEventActive)
		{
			return string.Empty;
		}
		TimeSpan timeSpan = socialEventStartTime + DurationSocialGunEvent - DateTime.UtcNow;
		return string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
	}

	public bool IsNeedShowGunFroLoginWindow()
	{
		int @int = Storager.getInt("FacebookController.CountShownGunForLogin", false);
		return socialGunEventActive && @int < 1 && SceneLoader.ActiveSceneName.Equals(Defs.MainMenuScene) && Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 0;
	}

	public void UpdateCountShownWindowByShowCondition()
	{
		int @int = Storager.getInt("FacebookController.CountShownGunForLogin", false);
		Storager.setString(Defs.LastTimeShowSocialGun, DateTime.UtcNow.ToString("s"), false);
		Storager.setInt("FacebookController.CountShownGunForLogin", @int + 1, false);
	}

	private void UpdateCountShownSocialGunWindowByTimeCondition()
	{
		if (!FacebookSupported)
		{
			return;
		}
		string text = string.Empty;
		if (!Storager.hasKey(Defs.LastTimeShowSocialGun))
		{
			Storager.setString(Defs.LastTimeShowSocialGun, text, false);
		}
		else
		{
			text = Storager.getString(Defs.LastTimeShowSocialGun, false);
		}
		if (!string.IsNullOrEmpty(text))
		{
			DateTime result = default(DateTime);
			if (DateTime.TryParse(text, out result) && DateTime.UtcNow - result >= TimeBetweenSocialGunBannerSeries)
			{
				Storager.setInt("FacebookController.CountShownGunForLogin", 0, false);
			}
		}
	}

	private bool CurrentSocialGunEventState()
	{
		return FacebookSupported && Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 0 && DateTime.UtcNow - socialEventStartTime < DurationSocialGunEvent && ExpController.LobbyLevel > 1 && !MainMenuController.SavedShwonLobbyLevelIsLessThanActual();
	}

	private void Awake()
	{
		friendsList = new List<Friend>();
		if (!FacebookSupported)
		{
			return;
		}
		FriendsController.NewFacebookLimitsAvailable += HandleNewFacebookLimitsAvailable;
		string empty = string.Empty;
		if (!Storager.hasKey("FacebookControllerSocialGunEventStartedKey"))
		{
			empty = DateTime.UtcNow.ToString("s");
			Storager.setString("FacebookControllerSocialGunEventStartedKey", empty, false);
		}
		else
		{
			empty = Storager.getString("FacebookControllerSocialGunEventStartedKey", false);
			DateTime result = default(DateTime);
			if (!DateTime.TryParse(empty, out result))
			{
				empty = DateTime.UtcNow.ToString("s");
				Storager.setString("FacebookControllerSocialGunEventStartedKey", empty, false);
			}
		}
		if (DateTime.TryParse(empty, out socialEventStartTime))
		{
			socialGunEventActive = CurrentSocialGunEventState();
		}
		else
		{
			Debug.LogError("FacebookController: invalid timeStartEvent");
		}
	}

	private void HandleNewFacebookLimitsAvailable(int greenLimit, int redLimit)
	{
		StoryPostLimits[StoryPriority.Green] = greenLimit;
		StoryPostLimits[StoryPriority.Red] = redLimit;
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		sharedController = this;
		if (FacebookSupported)
		{
			InitFacebook();
		}
		InitStoryPostHistoryKey();
		LoadStoryPostHistory();
		UpdateCountShownSocialGunWindowByTimeCondition();
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			SaveStoryPostHistory();
			return;
		}
		if (FB.IsInitialized)
		{
			FB.ActivateApp();
		}
		LoadStoryPostHistory();
		UpdateCountShownSocialGunWindowByTimeCondition();
	}

	private void OnDestroy()
	{
		SaveStoryPostHistory();
		if (FacebookSupported)
		{
			FriendsController.NewFacebookLimitsAvailable -= HandleNewFacebookLimitsAvailable;
		}
	}

	private void SaveStoryPostHistory()
	{
		Storager.setString("FacebookControllerStoryPostHistoryKey", Json.Serialize(storiesPostHistory), false);
	}

	private void LoadStoryPostHistory()
	{
		try
		{
			List<object> list = Json.Deserialize(Storager.getString("FacebookControllerStoryPostHistoryKey", false)) as List<object>;
			storiesPostHistory.Clear();
			foreach (object item2 in list)
			{
				Dictionary<string, object> item = item2 as Dictionary<string, object>;
				storiesPostHistory.Add(item);
			}
			CleanStoryPostHistory();
		}
		catch (Exception)
		{
			storiesPostHistory.Clear();
		}
	}

	private void InitStoryPostHistoryKey()
	{
		if (!Storager.hasKey("FacebookControllerStoryPostHistoryKey"))
		{
			Storager.setString("FacebookControllerStoryPostHistoryKey", "[]", false);
		}
	}

	private void CleanStoryPostHistory()
	{
		_timeSinceLastStoryPostHistoryClean = Time.realtimeSinceStartup;
		try
		{
			_003CCleanStoryPostHistory_003Ec__AnonStorey341 _003CCleanStoryPostHistory_003Ec__AnonStorey = new _003CCleanStoryPostHistory_003Ec__AnonStorey341();
			long num = 86400L;
			_003CCleanStoryPostHistory_003Ec__AnonStorey.minimumValidTime = PromoActionsManager.CurrentUnixTime - num;
			storiesPostHistory.RemoveAll(_003CCleanStoryPostHistory_003Ec__AnonStorey._003C_003Em__53B);
		}
		catch (Exception ex)
		{
			Debug.LogError("Exeption in CleanStoryPostHistory:\n" + ex);
		}
	}

	private void Update()
	{
		bool flag = CurrentSocialGunEventState();
		if (socialGunEventActive != flag)
		{
			socialGunEventActive = flag;
			Action<bool> socialGunEventStateChanged = FacebookController.SocialGunEventStateChanged;
			if (socialGunEventStateChanged != null)
			{
				socialGunEventStateChanged(SocialGunEventActive);
			}
		}
		if (Time.realtimeSinceStartup - _timeSinceLastStoryPostHistoryClean > 10f)
		{
			CleanStoryPostHistory();
		}
		if (FacebookSupported && !FB.IsLoggedIn && FriendsController.sharedController != null && !string.IsNullOrEmpty(FriendsController.sharedController.id_fb))
		{
			Action<string> receivedSelfID = FacebookController.ReceivedSelfID;
			if (receivedSelfID != null)
			{
				receivedSelfID(string.Empty);
			}
		}
	}

	public static void LogEvent(string eventName, Dictionary<string, object> parameters = null)
	{
		if (!FacebookSupported)
		{
			return;
		}
		try
		{
			FB.LogAppEvent(eventName, null, parameters);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	public static void ShowPostDialog()
	{
		if (FacebookSupported)
		{
			PlayerPrefs.SetInt("PostFacebookCount", PlayerPrefs.GetInt("PostFacebookCount", 0) + 1);
			PlayerPrefs.Save();
			if (PlayerPrefs.GetInt("Active_loyal_users_payed_send", 0) == 0 && PlayerPrefs.GetInt("PostFacebookCount", 0) > 2 && StoreKitEventListener.GetDollarsSpent() > 0 && PlayerPrefs.GetInt("PostVideo", 0) > 0)
			{
				LogEvent("Active_loyal_users_payed");
				PlayerPrefs.SetInt("Active_loyal_users_payed_send", 1);
			}
			if (PlayerPrefs.GetInt("Active_loyal_users_send", 0) == 0 && PlayerPrefs.GetInt("PostFacebookCount", 0) > 2 && PlayerPrefs.GetInt("PostVideo", 0) > 0)
			{
				LogEvent("Active_loyal_users");
				PlayerPrefs.SetInt("Active_loyal_users_send", 1);
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("link", Defs2.ApplicationUrl);
			dictionary.Add("name", "Pixel Gun 3D");
			dictionary.Add("picture", "http://pixelgun3d.com/iconforpost.png");
			dictionary.Add("caption", "I've just played the super battle in Pixel Gun 3D :)");
			dictionary.Add("description", "DOWNLOAD IT FOR FREE AND JOIN ME NOW!");
			Dictionary<string, object> dictionary2 = dictionary;
			Uri picture = new Uri((string)dictionary2["picture"]);
			FB.FeedShare(string.Empty, new Uri((string)dictionary2["link"]), (string)dictionary2["name"], (string)dictionary2["caption"], (string)dictionary2["description"], picture, string.Empty);
		}
	}

	public void PostMessage(string _message, Action<string, object> _completionHandler)
	{
		Debug.Log("Post to Facebook");
	}

	public static void PrintFBResult(IResult result)
	{
	}

	public static void FBGet(string graphPath, Action<IDictionary<string, object>> act, Action<IResult> onError = null)
	{
		_003CFBGet_003Ec__AnonStorey342 _003CFBGet_003Ec__AnonStorey = new _003CFBGet_003Ec__AnonStorey342();
		_003CFBGet_003Ec__AnonStorey.act = act;
		_003CFBGet_003Ec__AnonStorey.onError = onError;
		if (FacebookSupported)
		{
			FB.API(graphPath, HttpMethod.GET, _003CFBGet_003Ec__AnonStorey._003C_003Em__53C);
		}
	}

	internal static void CheckAndGiveFacebookReward(string context)
	{
		if (FacebookSupported)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (FacebookSupported && Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 0 && Storager.getInt(Defs.FacebookRewardGainStarted, false) == 1 && FB.IsLoggedIn)
			{
				Storager.setInt(Defs.FacebookRewardGainStarted, 0, false);
				Storager.setInt(Defs.IsFacebookLoginRewardaGained, 1, true);
				BankController.AddGems(10);
				TutorialQuestManager.Instance.AddFulfilledQuest("loginFacebook");
				QuestMediator.NotifySocialInteraction("loginFacebook");
				ShopNGUIController.ProvideShopItemOnStarterPackBoguht(ShopNGUIController.CategoryNames.SkinsCategory, "61", 1, false, 0, null, null, false, true, false);
				AnalyticsFacade.SendCustomEvent("Virality", new Dictionary<string, object> { { "Login Facebook", context } });
				FlurryPluginWrapper.LogEventToAppsFlyer("Virality", new Dictionary<string, string> { { "Login Facebook", context } });
				WeaponManager.AddExclusiveWeaponToWeaponStructures(WeaponManager.SocialGunWN);
			}
		}
	}

	public static void Login(Action onSuccess = null, Action onFailure = null, string context = "Unknown", Action onSuccessAfterPublishPermissions = null)
	{
		_003CLogin_003Ec__AnonStorey343 _003CLogin_003Ec__AnonStorey = new _003CLogin_003Ec__AnonStorey343();
		_003CLogin_003Ec__AnonStorey.context = context;
		_003CLogin_003Ec__AnonStorey.onSuccess = onSuccess;
		_003CLogin_003Ec__AnonStorey.onSuccessAfterPublishPermissions = onSuccessAfterPublishPermissions;
		_003CLogin_003Ec__AnonStorey.onFailure = onFailure;
		if (!FacebookSupported)
		{
			return;
		}
		if (Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 0)
		{
			Storager.setInt(Defs.FacebookRewardGainStarted, 1, false);
		}
		LoggingIn = true;
		try
		{
			List<string> list = new List<string>();
			list.Add("user_friends");
			FB.LogInWithReadPermissions(list, _003CLogin_003Ec__AnonStorey._003C_003Em__53D);
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception in Facebook Login: " + ex);
			LoggingIn = false;
		}
	}

	private static IEnumerator RunActionAfterDelay(Action action)
	{
		for (int i = 0; i < 30; i++)
		{
			yield return null;
		}
		if (action != null)
		{
			action();
		}
	}

	private static void RegisterStoryPostedWithPriority(StoryPriority priority)
	{
		if (!(sharedController == null))
		{
			sharedController.RegisterStoryPostedWithPriorityCore(priority);
		}
	}

	private void RegisterStoryPostedWithPriorityCore(StoryPriority priority)
	{
		if (FacebookSupported)
		{
			List<Dictionary<string, object>> list = storiesPostHistory;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			int num = (int)priority;
			dictionary.Add("priority", num.ToString());
			dictionary.Add("time", PromoActionsManager.CurrentUnixTime.ToString());
			list.Add(dictionary);
		}
	}

	public static void PostOpenGraphStory(string action, string obj, StoryPriority priority, Dictionary<string, string> pars = null)
	{
		_003CPostOpenGraphStory_003Ec__AnonStorey344 _003CPostOpenGraphStory_003Ec__AnonStorey = new _003CPostOpenGraphStory_003Ec__AnonStorey344();
		_003CPostOpenGraphStory_003Ec__AnonStorey.obj = obj;
		_003CPostOpenGraphStory_003Ec__AnonStorey.pars = pars;
		_003CPostOpenGraphStory_003Ec__AnonStorey.action = action;
		_003CPostOpenGraphStory_003Ec__AnonStorey.priority = priority;
		if (FacebookSupported)
		{
			RunApiWithAskForPermissions(_003CPostOpenGraphStory_003Ec__AnonStorey._003C_003Em__53E, "publish_actions");
		}
	}

	public static void FBPost(string graphPath, Dictionary<string, string> pars, Action<IDictionary<string, object>> act, Action<IResult> actionWithFBResult = null)
	{
		_003CFBPost_003Ec__AnonStorey345 _003CFBPost_003Ec__AnonStorey = new _003CFBPost_003Ec__AnonStorey345();
		_003CFBPost_003Ec__AnonStorey.actionWithFBResult = actionWithFBResult;
		_003CFBPost_003Ec__AnonStorey.act = act;
		if (FacebookSupported)
		{
			FB.API(graphPath, HttpMethod.POST, _003CFBPost_003Ec__AnonStorey._003C_003Em__53F, pars);
		}
	}

	public void SetMyId(Action onSuccess = null, bool dontRelogin = false)
	{
		_003CSetMyId_003Ec__AnonStorey346 _003CSetMyId_003Ec__AnonStorey = new _003CSetMyId_003Ec__AnonStorey346();
		_003CSetMyId_003Ec__AnonStorey.onSuccess = onSuccess;
		_003CSetMyId_003Ec__AnonStorey._003C_003Ef__this = this;
		if (FacebookSupported)
		{
			RunApiWithAskForPermissions(_003CSetMyId_003Ec__AnonStorey._003C_003Em__540, "user_friends", dontRelogin);
		}
	}

	private void InitFacebook()
	{
		if (!FacebookSupported)
		{
			return;
		}
		InitDelegate onInitComplete = _003CInitFacebook_003Em__541;
		try
		{
			if (_003C_003Ef__am_0024cache16 == null)
			{
				_003C_003Ef__am_0024cache16 = _003CInitFacebook_003Em__542;
			}
			FB.Init(onInitComplete, _003C_003Ef__am_0024cache16);
		}
		catch (NotImplementedException ex)
		{
			Debug.LogWarningFormat("Catching exception during FB.Init(): {0}", ex.Message);
		}
	}

	public static void RunApiWithAskForPermissions(Action runApi, string requiredPermission = "", bool dontRelogin = false, Action onFailToRunApi = null)
	{
		_003CRunApiWithAskForPermissions_003Ec__AnonStorey347 _003CRunApiWithAskForPermissions_003Ec__AnonStorey = new _003CRunApiWithAskForPermissions_003Ec__AnonStorey347();
		_003CRunApiWithAskForPermissions_003Ec__AnonStorey.requiredPermission = requiredPermission;
		_003CRunApiWithAskForPermissions_003Ec__AnonStorey.runApi = runApi;
		_003CRunApiWithAskForPermissions_003Ec__AnonStorey.onFailToRunApi = onFailToRunApi;
		if (!FacebookSupported)
		{
			return;
		}
		if (dontRelogin)
		{
			if (FB.IsLoggedIn && _003CRunApiWithAskForPermissions_003Ec__AnonStorey.runApi != null)
			{
				_003CRunApiWithAskForPermissions_003Ec__AnonStorey.runApi();
			}
			return;
		}
		_003CRunApiWithAskForPermissions_003Ec__AnonStorey.countTryingUpdateToken = 0;
		_003CRunApiWithAskForPermissions_003Ec__AnonStorey.loginWithRequiredPermissions = null;
		_003CRunApiWithAskForPermissions_003Ec__AnonStorey.loginWithRequiredPermissionsThroughLoginMethod = null;
		_003CRunApiWithAskForPermissions_003Ec__AnonStorey.checkPermissionsAndRunApi = _003CRunApiWithAskForPermissions_003Ec__AnonStorey._003C_003Em__543;
		_003CRunApiWithAskForPermissions_003Ec__AnonStorey.loginWithRequiredPermissions = _003CRunApiWithAskForPermissions_003Ec__AnonStorey._003C_003Em__544;
		_003CRunApiWithAskForPermissions_003Ec__AnonStorey.loginWithRequiredPermissionsThroughLoginMethod = _003CRunApiWithAskForPermissions_003Ec__AnonStorey._003C_003Em__545;
		if (!FB.IsLoggedIn)
		{
			_003CRunApiWithAskForPermissions_003Ec__AnonStorey.loginWithRequiredPermissions(_003CRunApiWithAskForPermissions_003Ec__AnonStorey.requiredPermission == "publish_actions");
		}
		else
		{
			_003CRunApiWithAskForPermissions_003Ec__AnonStorey.checkPermissionsAndRunApi(true);
		}
	}

	public void InputFacebookFriends(Action onSuccess = null, bool dontRelogin = false)
	{
		if (FacebookSupported)
		{
			SetMyId(onSuccess, dontRelogin);
		}
	}

	public void InvitePlayer(Action onComplete = null)
	{
		_003CInvitePlayer_003Ec__AnonStorey34A _003CInvitePlayer_003Ec__AnonStorey34A = new _003CInvitePlayer_003Ec__AnonStorey34A();
		_003CInvitePlayer_003Ec__AnonStorey34A.onComplete = onComplete;
		_003CInvitePlayer_003Ec__AnonStorey34A._003C_003Ef__this = this;
		if (FacebookSupported && !InvitePlayerApiIsRunning)
		{
			InvitePlayerApiIsRunning = true;
			Action runApi = _003CInvitePlayer_003Ec__AnonStorey34A._003C_003Em__546;
			RunApiWithAskForPermissions(runApi, "publish_actions", false, _003CInvitePlayer_003Ec__AnonStorey34A._003C_003Em__547);
		}
	}

	private IEnumerator LoadAvatar(int userCount, string url)
	{
		WWW www = Tools.CreateWwwIfNotConnected(url);
		if (www == null)
		{
			yield break;
		}
		yield return www;
		float elapsedTime = 0f;
		while (!www.isDone)
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= 4f)
			{
				break;
			}
			yield return null;
		}
		friendsList[userCount].SetAvatar(www.texture);
		Debug.LogFormat("Added avatar to friend: {0}", friendsList[userCount].name);
	}

	private IEnumerator GetOurFbId()
	{
		if (!FacebookSupported)
		{
			yield break;
		}
		while (FriendsController.sharedController == null)
		{
			yield return null;
		}
		bool success = false;
		while (FB.IsLoggedIn && !success)
		{
			FBGet("/me", ((_003CGetOurFbId_003Ec__Iterator1B8)(object)this)._003C_003Em__556);
			float startTime = Time.realtimeSinceStartup;
			do
			{
				yield return null;
			}
			while (Time.realtimeSinceStartup - startTime < 30f);
		}
	}

	[CompilerGenerated]
	private void _003CInitFacebook_003Em__541()
	{
		try
		{
			FB.ActivateApp();
			if (FB.IsLoggedIn)
			{
				StartCoroutine(GetOurFbId());
			}
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
			Debug.LogError("[Rilisoft] Exception in onInitComplete calback of FB.Init() method. Stacktrace: " + ex.StackTrace);
		}
	}

	[CompilerGenerated]
	private static void _003CInitFacebook_003Em__542(bool isGameShown)
	{
	}
}
