using Prime31;
using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class TwitterController : MonoBehaviour
{
	public const int DefaultGreenPriorityLimit = 7;

	public const int DefaultRedPriorityLimit = 3;

	public const int DefaultMultyWinPriorityLimit = 1;

	public const int DefaultArenaPriorityLimit = 1;

	private const string DefaultCallerContext = "Unknown";

	private const string StoryPostHistoryKey = "TwitterControllerStoryPostHistoryKey";

	private const string PriorityKey = "priority";

	private const string TimeKey = "time";

	public static TwitterController Instance;

	public readonly static Dictionary<FacebookController.StoryPriority, int> StoryPostLimits;

	private string _loginContext = "Unknown";

	private readonly static Lazy<TwitterController.TwitterFacadeBase> _twitterFacade;

	private bool _postInProcess;

	private float _timeSinceLastStoryPostHistoryClean;

	private List<Dictionary<string, object>> storiesPostHistory = new List<Dictionary<string, object>>();

	public static bool IsLoggedIn
	{
		get
		{
			return TwitterController.TwitterFacade.IsLoggedIn();
		}
	}

	private static TwitterController.TwitterFacadeBase TwitterFacade
	{
		get
		{
			return TwitterController._twitterFacade.Value;
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
			return (!TwitterController.TwitterSupported || BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64 || !(TwitterController.Instance != null) ? false : !TwitterController.Instance.CanPostStatusUpdateWithPriority(FacebookController.StoryPriority.Green));
		}
	}

	static TwitterController()
	{
		Dictionary<FacebookController.StoryPriority, int> storyPriorities = new Dictionary<FacebookController.StoryPriority, int>()
		{
			{ FacebookController.StoryPriority.Green, 7 },
			{ FacebookController.StoryPriority.Red, 3 },
			{ FacebookController.StoryPriority.MultyWinLimit, 1 },
			{ FacebookController.StoryPriority.ArenaLimit, 1 }
		};
		TwitterController.StoryPostLimits = storyPriorities;
		TwitterController._twitterFacade = new Lazy<TwitterController.TwitterFacadeBase>(new Func<TwitterController.TwitterFacadeBase>(TwitterController.InitializeFacade));
	}

	public TwitterController()
	{
	}

	private void Awake()
	{
		TwitterController.Instance = this;
		if (TwitterController.TwitterSupported)
		{
			string empty = string.Empty;
			string str = string.Empty;
			TwitterController.TwitterFacade.Init("13K6E5YAJvXSEaig0GVVFd68K", "I4DtR7TC0OU26OMYI0hLmP1jhVHfNuPRMskbYDIoOS2xYBBWdS");
			TwitterManager.loginSucceededEvent += new Action<string>(this.OnTwitterLogin);
			TwitterManager.loginFailedEvent += new Action<string>(this.OnTwitterLoginFailed);
			TwitterManager.requestDidFinishEvent += new Action<object>(this.OnTwitterPost);
			TwitterManager.requestDidFailEvent += new Action<string>(this.OnTwitterPostFailed);
			FriendsController.NewTwitterLimitsAvailable += new Action<int, int, int, int>(this.HandleNewTwitterLimitsAvailable);
		}
	}

	public bool CanPostStatusUpdateWithPriority(FacebookController.StoryPriority priority)
	{
		bool flag;
		try
		{
			flag = (priority != FacebookController.StoryPriority.Green ? (
				from rec in this.storiesPostHistory
				where int.Parse((string)rec["priority"]) == (int)priority
				select rec).Count<Dictionary<string, object>>() < TwitterController.StoryPostLimits[priority] : this.storiesPostHistory.Count < TwitterController.StoryPostLimits[priority]);
		}
		catch (Exception exception)
		{
			Debug.LogError(string.Concat("Exeption in CanPostStoryWithPriority:\n", exception));
			return false;
		}
		return flag;
	}

	public static void CheckAndGiveTwitterReward(string context)
	{
		if (!TwitterController.TwitterSupported)
		{
			return;
		}
		if (Storager.getInt(Defs.IsTwitterLoginRewardaGained, true) == 0 && Storager.getInt(Defs.TwitterRewardGainStarted, false) == 1 && TwitterController.TwitterFacade.IsLoggedIn())
		{
			Storager.setInt(Defs.TwitterRewardGainStarted, 0, false);
			Storager.setInt(Defs.IsTwitterLoginRewardaGained, 1, true);
			BankController.AddGems(10, true, AnalyticsConstants.AccrualType.Earned);
			TutorialQuestManager.Instance.AddFulfilledQuest("loginTwitter");
			QuestMediator.NotifySocialInteraction("loginTwitter");
			Dictionary<string, object> strs = new Dictionary<string, object>()
			{
				{ "Login Twitter", context ?? "Unknown" }
			};
			AnalyticsFacade.SendCustomEvent("Virality", strs);
			Dictionary<string, string> strs1 = new Dictionary<string, string>()
			{
				{ "Login Twitter", context ?? "Unknown" }
			};
			FlurryPluginWrapper.LogEventToAppsFlyer("Virality", strs1);
		}
	}

	private void CleanStoryPostHistory()
	{
		this._timeSinceLastStoryPostHistoryClean = Time.realtimeSinceStartup;
		try
		{
			long num = (long)86400;
			long currentUnixTime = PromoActionsManager.CurrentUnixTime - num;
			this.storiesPostHistory.RemoveAll((Dictionary<string, object> entry) => long.Parse((string)entry["time"]) < currentUnixTime);
		}
		catch (Exception exception)
		{
			Debug.LogError(string.Concat("TwitterController Exeption in CleanStoryPostHistory:\n", exception));
		}
	}

	private void HandleNewTwitterLimitsAvailable(int greenLimit, int redLimit, int multyWinLimit, int arenaLimit)
	{
		TwitterController.StoryPostLimits[FacebookController.StoryPriority.Green] = greenLimit;
		TwitterController.StoryPostLimits[FacebookController.StoryPriority.Red] = redLimit;
		TwitterController.StoryPostLimits[FacebookController.StoryPriority.MultyWinLimit] = multyWinLimit;
		TwitterController.StoryPostLimits[FacebookController.StoryPriority.ArenaLimit] = arenaLimit;
	}

	private static TwitterController.TwitterFacadeBase InitializeFacade()
	{
		if (!TwitterController.TwitterSupported)
		{
			return new TwitterController.DummyTwitterFacade();
		}
		switch (BuildSettings.BuildTargetPlatform)
		{
			case RuntimePlatform.IPhonePlayer:
			{
				return new TwitterController.IosTwitterFacade();
			}
			case RuntimePlatform.PS3:
			case RuntimePlatform.XBOX360:
			{
				return new TwitterController.DummyTwitterFacade();
			}
			case RuntimePlatform.Android:
			{
				return new TwitterController.AndroidTwitterFacade();
			}
			default:
			{
				return new TwitterController.DummyTwitterFacade();
			}
		}
	}

	private void InitStoryPostHistoryKey()
	{
		if (!Storager.hasKey("TwitterControllerStoryPostHistoryKey"))
		{
			Storager.setString("TwitterControllerStoryPostHistoryKey", "[]", false);
		}
	}

	private void LoadStoryPostHistory()
	{
		try
		{
			List<object> objs = Rilisoft.MiniJson.Json.Deserialize(Storager.getString("TwitterControllerStoryPostHistoryKey", false)) as List<object>;
			this.storiesPostHistory.Clear();
			foreach (object obj in objs)
			{
				this.storiesPostHistory.Add(obj as Dictionary<string, object>);
			}
			this.CleanStoryPostHistory();
		}
		catch (Exception exception)
		{
			this.storiesPostHistory.Clear();
		}
	}

	public void Login(Action onSuccess = null, Action onFailure = null, string context = "Unknown")
	{
		if (!TwitterController.TwitterSupported)
		{
			return;
		}
		Action<string> action = null;
		Action<string> action1 = null;
		action = (string unusedResult) => {
			if (onSuccess != null)
			{
				onSuccess();
			}
			TwitterManager.loginSucceededEvent -= action;
			TwitterManager.loginFailedEvent -= action1;
		};
		action1 = (string unusedResult) => {
			if (onFailure != null)
			{
				onFailure();
			}
			TwitterManager.loginSucceededEvent -= action;
			TwitterManager.loginFailedEvent -= action1;
		};
		TwitterManager.loginSucceededEvent += action;
		TwitterManager.loginFailedEvent += action1;
		if (Storager.getInt(Defs.IsTwitterLoginRewardaGained, true) == 0)
		{
			Storager.setInt(Defs.TwitterRewardGainStarted, 1, false);
		}
		this._loginContext = context;
		TwitterController.TwitterFacade.ShowLoginDialog(null);
	}

	public void Logout()
	{
		if (!TwitterController.TwitterSupported)
		{
			return;
		}
		TwitterController.TwitterFacade.Logout();
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			this.LoadStoryPostHistory();
		}
		else
		{
			this.SaveStoryPostHistory();
		}
	}

	private void OnDestroy()
	{
		this.SaveStoryPostHistory();
		if (TwitterController.TwitterSupported)
		{
			FriendsController.NewTwitterLimitsAvailable -= new Action<int, int, int, int>(this.HandleNewTwitterLimitsAvailable);
		}
	}

	private void OnTwitterLogin(string result)
	{
		TwitterController.CheckAndGiveTwitterReward(this._loginContext);
		Debug.Log(string.Format("TwitterController.OnTwitterLogin(“{0}”)    {1}", result, this._loginContext));
		this._loginContext = "Unknown";
	}

	private void OnTwitterLoginFailed(string error)
	{
		Debug.Log(string.Format("TwitterController.OnTwitterLoginFailed(“{0}”)    {1}", error, this._loginContext));
		this._loginContext = "Unknown";
	}

	private void OnTwitterPost(object result)
	{
		this._postInProcess = false;
		Debug.Log(string.Concat("TwitterController: OnTwitterPost: ", result) ?? string.Empty);
	}

	private void OnTwitterPostFailed(string _error)
	{
		this._postInProcess = false;
		Debug.Log(string.Concat("TwitterController: OnTwitterPostFailed: ", _error) ?? string.Empty);
	}

	public void PostStatusUpdate(string status, FacebookController.StoryPriority priority)
	{
		if (!TwitterController.TwitterSupported)
		{
			return;
		}
		this.PostStatusUpdate(status, () => this.RegisterStoryPostedWithPriorityCore(priority));
	}

	public void PostStatusUpdate(string status, Action customOnSuccess = null)
	{
		if (!TwitterController.TwitterSupported)
		{
			return;
		}
		Action<string> action1 = null;
		Action<string> action2 = null;
		action1 = (string unusedString) => {
			TwitterManager.loginSucceededEvent -= action1;
			TwitterManager.loginFailedEvent -= action2;
			if (this._postInProcess)
			{
				return;
			}
			if (customOnSuccess != null)
			{
				Action<object> u003cu003ef_refu0024857 = null;
				Action<string> action = null;
				u003cu003ef_refu0024857 = (object unused) => {
					customOnSuccess();
					TwitterManager.requestDidFinishEvent -= this.onSuccessPost;
					TwitterManager.requestDidFailEvent -= this.onFailedPost;
				};
				action = (string unused) => {
					TwitterManager.requestDidFinishEvent -= this.onSuccessPost;
					TwitterManager.requestDidFailEvent -= this.onFailedPost;
				};
				TwitterManager.requestDidFinishEvent += u003cu003ef_refu0024857;
				TwitterManager.requestDidFailEvent += action;
			}
			this._postInProcess = true;
			TwitterController.TwitterFacade.PostStatusUpdate(status);
		};
		action2 = (string unused) => {
			TwitterManager.loginSucceededEvent -= action1;
			TwitterManager.loginFailedEvent -= action2;
		};
		if (!TwitterController.TwitterFacade.IsLoggedIn())
		{
			TwitterManager.loginSucceededEvent += action1;
			TwitterManager.loginFailedEvent += action2;
			TwitterController.TwitterFacade.ShowLoginDialog(() => action1(string.Empty));
		}
		else
		{
			action1(string.Empty);
		}
	}

	private void RegisterStoryPostedWithPriorityCore(FacebookController.StoryPriority priority)
	{
		if (!TwitterController.TwitterSupported)
		{
			return;
		}
		List<Dictionary<string, object>> dictionaries = this.storiesPostHistory;
		Dictionary<string, object> strs = new Dictionary<string, object>()
		{
			{ "priority", priority.ToString() },
			{ "time", PromoActionsManager.CurrentUnixTime.ToString() }
		};
		dictionaries.Add(strs);
	}

	private void SaveStoryPostHistory()
	{
		Storager.setString("TwitterControllerStoryPostHistoryKey", Rilisoft.MiniJson.Json.Serialize(this.storiesPostHistory), false);
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.InitStoryPostHistoryKey();
		this.LoadStoryPostHistory();
	}

	private void Update()
	{
		if (Time.realtimeSinceStartup - this._timeSinceLastStoryPostHistoryClean > 10f)
		{
			this.CleanStoryPostHistory();
		}
	}

	private class AndroidTwitterFacade : TwitterController.TwitterFacadeBase
	{
		public AndroidTwitterFacade()
		{
		}

		public override void Init(string consumerKey, string consumerSecret)
		{
			TwitterAndroid.init(consumerKey, consumerSecret, "twitterplugin");
		}

		public override bool IsLoggedIn()
		{
			return TwitterAndroid.isLoggedIn();
		}

		public override void Logout()
		{
			TwitterAndroid.logout();
		}

		public override void PostStatusUpdate(string status)
		{
			TwitterAndroid.postStatusUpdate(status);
		}

		public override void ShowLoginDialog(Action WP8customOnSuccessLogin = null)
		{
			TwitterAndroid.showLoginDialog(false);
		}
	}

	private class DummyTwitterFacade : TwitterController.TwitterFacadeBase
	{
		public DummyTwitterFacade()
		{
		}

		public override void Init(string consumerKey, string consumerSecret)
		{
		}

		public override bool IsLoggedIn()
		{
			return BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64;
		}

		public override void Logout()
		{
		}

		public override void PostStatusUpdate(string status)
		{
		}

		public override void ShowLoginDialog(Action WP8customOnSuccessLogin = null)
		{
		}
	}

	private class IosTwitterFacade : TwitterController.TwitterFacadeBase
	{
		public IosTwitterFacade()
		{
		}

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

		public override void Logout()
		{
		}

		public override void PostStatusUpdate(string status)
		{
		}

		public override void ShowLoginDialog(Action WP8customOnSuccessLogin = null)
		{
		}
	}

	private abstract class TwitterFacadeBase
	{
		protected TwitterFacadeBase()
		{
		}

		public abstract void Init(string consumerKey, string consumerSecret);

		public abstract bool IsLoggedIn();

		public abstract void Logout();

		public abstract void PostStatusUpdate(string status);

		public abstract void ShowLoginDialog(Action WP8customOnSuccessLogin = null);
	}
}