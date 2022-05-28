using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Prime31;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

internal sealed class TwitterController : MonoBehaviour
{
	private abstract class TwitterFacadeBase
	{
		public abstract void Init(string consumerKey, string consumerSecret);

		public abstract bool IsLoggedIn();

		public abstract void PostStatusUpdate(string status);

		public abstract void ShowLoginDialog(Action WP8customOnSuccessLogin = null);

		public abstract void Logout();
	}

	private class IosTwitterFacade : TwitterFacadeBase
	{
		public override void Init(string consumerKey, string consumerSecret)
		{
		}

		public override bool IsLoggedIn()
		{
			throw new NotSupportedException();
		}

		public string LoggedInUsername()
		{
			throw new NotSupportedException();
		}

		public override void PostStatusUpdate(string status)
		{
		}

		public override void ShowLoginDialog(Action WP8customOnSuccessLogin = null)
		{
		}

		public override void Logout()
		{
		}
	}

	private class AndroidTwitterFacade : TwitterFacadeBase
	{
		public override void Init(string consumerKey, string consumerSecret)
		{
			TwitterAndroid.init(consumerKey, consumerSecret);
		}

		public override bool IsLoggedIn()
		{
			return TwitterAndroid.isLoggedIn();
		}

		public override void PostStatusUpdate(string status)
		{
			TwitterAndroid.postStatusUpdate(status);
		}

		public override void ShowLoginDialog(Action WP8customOnSuccessLogin = null)
		{
			TwitterAndroid.showLoginDialog();
		}

		public override void Logout()
		{
			TwitterAndroid.logout();
		}
	}

	private class DummyTwitterFacade : TwitterFacadeBase
	{
		public override void Init(string consumerKey, string consumerSecret)
		{
		}

		public override bool IsLoggedIn()
		{
			return BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64;
		}

		public override void PostStatusUpdate(string status)
		{
		}

		public override void ShowLoginDialog(Action WP8customOnSuccessLogin = null)
		{
		}

		public override void Logout()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003CLogin_003Ec__AnonStorey357
	{
		internal Action onSuccess;

		internal Action<string> onSuccessCallback;

		internal Action<string> onFailCallback;

		internal Action onFailure;

		internal void _003C_003Em__597(string unusedResult)
		{
			if (onSuccess != null)
			{
				onSuccess();
			}
			TwitterManager.loginSucceededEvent -= onSuccessCallback;
			TwitterManager.loginFailedEvent -= onFailCallback;
		}

		internal void _003C_003Em__598(string unusedResult)
		{
			if (onFailure != null)
			{
				onFailure();
			}
			TwitterManager.loginSucceededEvent -= onSuccessCallback;
			TwitterManager.loginFailedEvent -= onFailCallback;
		}
	}

	[CompilerGenerated]
	private sealed class _003CPostStatusUpdate_003Ec__AnonStorey358
	{
		internal FacebookController.StoryPriority priority;

		internal TwitterController _003C_003Ef__this;

		internal void _003C_003Em__599()
		{
			_003C_003Ef__this.RegisterStoryPostedWithPriorityCore(priority);
		}
	}

	[CompilerGenerated]
	private sealed class _003CPostStatusUpdate_003Ec__AnonStorey359
	{
		private sealed class _003CPostStatusUpdate_003Ec__AnonStorey35A
		{
			internal Action<object> onSuccessPost;

			internal Action<string> onFailedPost;

			internal _003CPostStatusUpdate_003Ec__AnonStorey359 _003C_003Ef__ref_0024857;

			internal void _003C_003Em__59F(object unused)
			{
				_003C_003Ef__ref_0024857.customOnSuccess();
				TwitterManager.requestDidFinishEvent -= onSuccessPost;
				TwitterManager.requestDidFailEvent -= onFailedPost;
			}

			internal void _003C_003Em__5A0(string unused)
			{
				TwitterManager.requestDidFinishEvent -= onSuccessPost;
				TwitterManager.requestDidFailEvent -= onFailedPost;
			}
		}

		internal Action<string> postAction;

		internal Action<string> loginFailedAction;

		internal Action customOnSuccess;

		internal string status;

		internal TwitterController _003C_003Ef__this;

		internal void _003C_003Em__59A(string unusedString)
		{
			TwitterManager.loginSucceededEvent -= postAction;
			TwitterManager.loginFailedEvent -= loginFailedAction;
			if (!_003C_003Ef__this._postInProcess)
			{
				if (customOnSuccess != null)
				{
					_003CPostStatusUpdate_003Ec__AnonStorey35A _003CPostStatusUpdate_003Ec__AnonStorey35A = new _003CPostStatusUpdate_003Ec__AnonStorey35A();
					_003CPostStatusUpdate_003Ec__AnonStorey35A._003C_003Ef__ref_0024857 = this;
					_003CPostStatusUpdate_003Ec__AnonStorey35A.onSuccessPost = null;
					_003CPostStatusUpdate_003Ec__AnonStorey35A.onFailedPost = null;
					_003CPostStatusUpdate_003Ec__AnonStorey35A.onSuccessPost = _003CPostStatusUpdate_003Ec__AnonStorey35A._003C_003Em__59F;
					_003CPostStatusUpdate_003Ec__AnonStorey35A.onFailedPost = _003CPostStatusUpdate_003Ec__AnonStorey35A._003C_003Em__5A0;
					TwitterManager.requestDidFinishEvent += _003CPostStatusUpdate_003Ec__AnonStorey35A.onSuccessPost;
					TwitterManager.requestDidFailEvent += _003CPostStatusUpdate_003Ec__AnonStorey35A.onFailedPost;
				}
				_003C_003Ef__this._postInProcess = true;
				TwitterFacade.PostStatusUpdate(status);
			}
		}

		internal void _003C_003Em__59B(string unused)
		{
			TwitterManager.loginSucceededEvent -= postAction;
			TwitterManager.loginFailedEvent -= loginFailedAction;
		}

		internal void _003C_003Em__59C()
		{
			postAction(string.Empty);
		}
	}

	[CompilerGenerated]
	private sealed class _003CCanPostStatusUpdateWithPriority_003Ec__AnonStorey35B
	{
		internal FacebookController.StoryPriority priority;

		internal bool _003C_003Em__59D(Dictionary<string, object> rec)
		{
			return int.Parse((string)rec["priority"]) == (int)priority;
		}
	}

	[CompilerGenerated]
	private sealed class _003CCleanStoryPostHistory_003Ec__AnonStorey35C
	{
		internal long minimumValidTime;

		internal bool _003C_003Em__59E(Dictionary<string, object> entry)
		{
			return long.Parse((string)entry["time"]) < minimumValidTime;
		}
	}

	public const int DefaultGreenPriorityLimit = 7;

	public const int DefaultRedPriorityLimit = 3;

	public const int DefaultMultyWinPriorityLimit = 1;

	public const int DefaultArenaPriorityLimit = 1;

	private const string DefaultCallerContext = "Unknown";

	private const string StoryPostHistoryKey = "TwitterControllerStoryPostHistoryKey";

	private const string PriorityKey = "priority";

	private const string TimeKey = "time";

	public static TwitterController Instance;

	public static readonly Dictionary<FacebookController.StoryPriority, int> StoryPostLimits;

	private string _loginContext = "Unknown";

	private static readonly Lazy<TwitterFacadeBase> _twitterFacade;

	private bool _postInProcess;

	private float _timeSinceLastStoryPostHistoryClean;

	private List<Dictionary<string, object>> storiesPostHistory = new List<Dictionary<string, object>>();

	public static bool IsLoggedIn
	{
		get
		{
			return TwitterFacade.IsLoggedIn();
		}
	}

	public static bool TwitterSupported
	{
		get
		{
			return BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64;
		}
	}

	public static bool TwitterSupported_OldPosts
	{
		get
		{
			return TwitterSupported && BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64 && Instance != null && !Instance.CanPostStatusUpdateWithPriority(FacebookController.StoryPriority.Green);
		}
	}

	private static TwitterFacadeBase TwitterFacade
	{
		get
		{
			return _twitterFacade.Value;
		}
	}

	static TwitterController()
	{
		StoryPostLimits = new Dictionary<FacebookController.StoryPriority, int>
		{
			{
				FacebookController.StoryPriority.Green,
				7
			},
			{
				FacebookController.StoryPriority.Red,
				3
			},
			{
				FacebookController.StoryPriority.MultyWinLimit,
				1
			},
			{
				FacebookController.StoryPriority.ArenaLimit,
				1
			}
		};
		_twitterFacade = new Lazy<TwitterFacadeBase>(InitializeFacade);
	}

	private void Awake()
	{
		Instance = this;
		if (TwitterSupported)
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			empty = "13K6E5YAJvXSEaig0GVVFd68K";
			empty2 = "I4DtR7TC0OU26OMYI0hLmP1jhVHfNuPRMskbYDIoOS2xYBBWdS";
			TwitterFacade.Init(empty, empty2);
			TwitterManager.loginSucceededEvent += OnTwitterLogin;
			TwitterManager.loginFailedEvent += OnTwitterLoginFailed;
			TwitterManager.requestDidFinishEvent += OnTwitterPost;
			TwitterManager.requestDidFailEvent += OnTwitterPostFailed;
			FriendsController.NewTwitterLimitsAvailable += HandleNewTwitterLimitsAvailable;
		}
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		InitStoryPostHistoryKey();
		LoadStoryPostHistory();
	}

	public void Login(Action onSuccess = null, Action onFailure = null, string context = "Unknown")
	{
		_003CLogin_003Ec__AnonStorey357 _003CLogin_003Ec__AnonStorey = new _003CLogin_003Ec__AnonStorey357();
		_003CLogin_003Ec__AnonStorey.onSuccess = onSuccess;
		_003CLogin_003Ec__AnonStorey.onFailure = onFailure;
		if (TwitterSupported)
		{
			_003CLogin_003Ec__AnonStorey.onSuccessCallback = null;
			_003CLogin_003Ec__AnonStorey.onFailCallback = null;
			_003CLogin_003Ec__AnonStorey.onSuccessCallback = _003CLogin_003Ec__AnonStorey._003C_003Em__597;
			_003CLogin_003Ec__AnonStorey.onFailCallback = _003CLogin_003Ec__AnonStorey._003C_003Em__598;
			TwitterManager.loginSucceededEvent += _003CLogin_003Ec__AnonStorey.onSuccessCallback;
			TwitterManager.loginFailedEvent += _003CLogin_003Ec__AnonStorey.onFailCallback;
			if (Storager.getInt(Defs.IsTwitterLoginRewardaGained, true) == 0)
			{
				Storager.setInt(Defs.TwitterRewardGainStarted, 1, false);
			}
			_loginContext = context;
			TwitterFacade.ShowLoginDialog();
		}
	}

	public void Logout()
	{
		if (TwitterSupported)
		{
			TwitterFacade.Logout();
		}
	}

	public void PostStatusUpdate(string status, FacebookController.StoryPriority priority)
	{
		_003CPostStatusUpdate_003Ec__AnonStorey358 _003CPostStatusUpdate_003Ec__AnonStorey = new _003CPostStatusUpdate_003Ec__AnonStorey358();
		_003CPostStatusUpdate_003Ec__AnonStorey.priority = priority;
		_003CPostStatusUpdate_003Ec__AnonStorey._003C_003Ef__this = this;
		if (TwitterSupported)
		{
			PostStatusUpdate(status, _003CPostStatusUpdate_003Ec__AnonStorey._003C_003Em__599);
		}
	}

	public void PostStatusUpdate(string status, Action customOnSuccess = null)
	{
		_003CPostStatusUpdate_003Ec__AnonStorey359 _003CPostStatusUpdate_003Ec__AnonStorey = new _003CPostStatusUpdate_003Ec__AnonStorey359();
		_003CPostStatusUpdate_003Ec__AnonStorey.customOnSuccess = customOnSuccess;
		_003CPostStatusUpdate_003Ec__AnonStorey.status = status;
		_003CPostStatusUpdate_003Ec__AnonStorey._003C_003Ef__this = this;
		if (TwitterSupported)
		{
			_003CPostStatusUpdate_003Ec__AnonStorey.postAction = null;
			_003CPostStatusUpdate_003Ec__AnonStorey.loginFailedAction = null;
			_003CPostStatusUpdate_003Ec__AnonStorey.postAction = _003CPostStatusUpdate_003Ec__AnonStorey._003C_003Em__59A;
			_003CPostStatusUpdate_003Ec__AnonStorey.loginFailedAction = _003CPostStatusUpdate_003Ec__AnonStorey._003C_003Em__59B;
			if (TwitterFacade.IsLoggedIn())
			{
				_003CPostStatusUpdate_003Ec__AnonStorey.postAction(string.Empty);
				return;
			}
			TwitterManager.loginSucceededEvent += _003CPostStatusUpdate_003Ec__AnonStorey.postAction;
			TwitterManager.loginFailedEvent += _003CPostStatusUpdate_003Ec__AnonStorey.loginFailedAction;
			TwitterFacade.ShowLoginDialog(_003CPostStatusUpdate_003Ec__AnonStorey._003C_003Em__59C);
		}
	}

	public bool CanPostStatusUpdateWithPriority(FacebookController.StoryPriority priority)
	{
		//Discarded unreachable code: IL_006f
		_003CCanPostStatusUpdateWithPriority_003Ec__AnonStorey35B _003CCanPostStatusUpdateWithPriority_003Ec__AnonStorey35B = new _003CCanPostStatusUpdateWithPriority_003Ec__AnonStorey35B();
		_003CCanPostStatusUpdateWithPriority_003Ec__AnonStorey35B.priority = priority;
		try
		{
			if (_003CCanPostStatusUpdateWithPriority_003Ec__AnonStorey35B.priority == FacebookController.StoryPriority.Green)
			{
				return storiesPostHistory.Count < StoryPostLimits[_003CCanPostStatusUpdateWithPriority_003Ec__AnonStorey35B.priority];
			}
			return storiesPostHistory.Where(_003CCanPostStatusUpdateWithPriority_003Ec__AnonStorey35B._003C_003Em__59D).Count() < StoryPostLimits[_003CCanPostStatusUpdateWithPriority_003Ec__AnonStorey35B.priority];
		}
		catch (Exception ex)
		{
			Debug.LogError("Exeption in CanPostStoryWithPriority:\n" + ex);
		}
		return false;
	}

	public static void CheckAndGiveTwitterReward(string context)
	{
		if (TwitterSupported && Storager.getInt(Defs.IsTwitterLoginRewardaGained, true) == 0 && Storager.getInt(Defs.TwitterRewardGainStarted, false) == 1 && TwitterFacade.IsLoggedIn())
		{
			Storager.setInt(Defs.TwitterRewardGainStarted, 0, false);
			Storager.setInt(Defs.IsTwitterLoginRewardaGained, 1, true);
			BankController.AddGems(10);
			TutorialQuestManager.Instance.AddFulfilledQuest("loginTwitter");
			QuestMediator.NotifySocialInteraction("loginTwitter");
			AnalyticsFacade.SendCustomEvent("Virality", new Dictionary<string, object> { 
			{
				"Login Twitter",
				context ?? "Unknown"
			} });
			FlurryPluginWrapper.LogEventToAppsFlyer("Virality", new Dictionary<string, string> { 
			{
				"Login Twitter",
				context ?? "Unknown"
			} });
		}
	}

	private void OnTwitterLogin(string result)
	{
		CheckAndGiveTwitterReward(_loginContext);
		string message = string.Format("TwitterController.OnTwitterLogin(“{0}”)    {1}", result, _loginContext);
		Debug.Log(message);
		_loginContext = "Unknown";
	}

	private void OnTwitterLoginFailed(string error)
	{
		string message = string.Format("TwitterController.OnTwitterLoginFailed(“{0}”)    {1}", error, _loginContext);
		Debug.Log(message);
		_loginContext = "Unknown";
	}

	private void OnTwitterPost(object result)
	{
		_postInProcess = false;
		Debug.Log(("TwitterController: OnTwitterPost: " + result) ?? string.Empty);
	}

	private void OnTwitterPostFailed(string _error)
	{
		_postInProcess = false;
		Debug.Log(("TwitterController: OnTwitterPostFailed: " + _error) ?? string.Empty);
	}

	private void HandleNewTwitterLimitsAvailable(int greenLimit, int redLimit, int multyWinLimit, int arenaLimit)
	{
		StoryPostLimits[FacebookController.StoryPriority.Green] = greenLimit;
		StoryPostLimits[FacebookController.StoryPriority.Red] = redLimit;
		StoryPostLimits[FacebookController.StoryPriority.MultyWinLimit] = multyWinLimit;
		StoryPostLimits[FacebookController.StoryPriority.ArenaLimit] = arenaLimit;
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			SaveStoryPostHistory();
		}
		else
		{
			LoadStoryPostHistory();
		}
	}

	private void Update()
	{
		if (Time.realtimeSinceStartup - _timeSinceLastStoryPostHistoryClean > 10f)
		{
			CleanStoryPostHistory();
		}
	}

	private void OnDestroy()
	{
		SaveStoryPostHistory();
		if (TwitterSupported)
		{
			FriendsController.NewTwitterLimitsAvailable -= HandleNewTwitterLimitsAvailable;
		}
	}

	private void SaveStoryPostHistory()
	{
		Storager.setString("TwitterControllerStoryPostHistoryKey", Rilisoft.MiniJson.Json.Serialize(storiesPostHistory), false);
	}

	private void LoadStoryPostHistory()
	{
		try
		{
			List<object> list = Rilisoft.MiniJson.Json.Deserialize(Storager.getString("TwitterControllerStoryPostHistoryKey", false)) as List<object>;
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

	private void CleanStoryPostHistory()
	{
		_timeSinceLastStoryPostHistoryClean = Time.realtimeSinceStartup;
		try
		{
			_003CCleanStoryPostHistory_003Ec__AnonStorey35C _003CCleanStoryPostHistory_003Ec__AnonStorey35C = new _003CCleanStoryPostHistory_003Ec__AnonStorey35C();
			long num = 86400L;
			_003CCleanStoryPostHistory_003Ec__AnonStorey35C.minimumValidTime = PromoActionsManager.CurrentUnixTime - num;
			storiesPostHistory.RemoveAll(_003CCleanStoryPostHistory_003Ec__AnonStorey35C._003C_003Em__59E);
		}
		catch (Exception ex)
		{
			Debug.LogError("TwitterController Exeption in CleanStoryPostHistory:\n" + ex);
		}
	}

	private void RegisterStoryPostedWithPriorityCore(FacebookController.StoryPriority priority)
	{
		if (TwitterSupported)
		{
			List<Dictionary<string, object>> list = storiesPostHistory;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			int num = (int)priority;
			dictionary.Add("priority", num.ToString());
			dictionary.Add("time", PromoActionsManager.CurrentUnixTime.ToString());
			list.Add(dictionary);
		}
	}

	private void InitStoryPostHistoryKey()
	{
		if (!Storager.hasKey("TwitterControllerStoryPostHistoryKey"))
		{
			Storager.setString("TwitterControllerStoryPostHistoryKey", "[]", false);
		}
	}

	private static TwitterFacadeBase InitializeFacade()
	{
		if (!TwitterSupported)
		{
			return new DummyTwitterFacade();
		}
		switch (BuildSettings.BuildTargetPlatform)
		{
		case RuntimePlatform.IPhonePlayer:
			return new IosTwitterFacade();
		case RuntimePlatform.Android:
			return new AndroidTwitterFacade();
		default:
			return new DummyTwitterFacade();
		}
	}
}
