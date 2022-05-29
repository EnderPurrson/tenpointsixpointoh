using Facebook.Unity;
using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FacebookController : MonoBehaviour
{
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

	public readonly static Dictionary<FacebookController.StoryPriority, int> StoryPostLimits;

	private string _appId = string.Empty;

	private bool socialGunEventActive;

	private float _timeSinceLastStoryPostHistoryClean;

	public bool InvitePlayerApiIsRunning;

	private TimeSpan DurationSocialGunEvent = TimeSpan.FromDays(1000000);

	private TimeSpan TimeBetweenSocialGunBannerSeries = TimeSpan.FromHours(24);

	private DateTime socialEventStartTime;

	private List<Dictionary<string, object>> storiesPostHistory = new List<Dictionary<string, object>>();

	public string AppId
	{
		get
		{
			return this._appId;
		}
		set
		{
			this._appId = value ?? string.Empty;
		}
	}

	public static bool FacebookPost_Old_Supported
	{
		get
		{
			bool flag;
			if (!FacebookController.FacebookSupported)
			{
				flag = false;
			}
			else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64 || !(FacebookController.sharedController != null) || FacebookController.sharedController.CanPostStoryWithPriority(FacebookController.StoryPriority.Green))
			{
				flag = (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64 || !Defs.isMulti || !PhotonNetwork.connected ? false : NetworkStartTable.LocalOrPasswordRoom());
			}
			else
			{
				flag = true;
			}
			return flag;
		}
	}

	public static bool FacebookSupported
	{
		get
		{
			return BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64;
		}
	}

	public static bool LoggingIn
	{
		get;
		private set;
	}

	public bool SocialGunEventActive
	{
		get
		{
			return this.socialGunEventActive;
		}
	}

	static FacebookController()
	{
		Dictionary<FacebookController.StoryPriority, int> storyPriorities = new Dictionary<FacebookController.StoryPriority, int>()
		{
			{ FacebookController.StoryPriority.Green, 7 },
			{ FacebookController.StoryPriority.Red, 3 }
		};
		FacebookController.StoryPostLimits = storyPriorities;
	}

	public FacebookController()
	{
	}

	private void Awake()
	{
		this.friendsList = new List<Friend>();
		if (FacebookController.FacebookSupported)
		{
			FriendsController.NewFacebookLimitsAvailable += new Action<int, int>(this.HandleNewFacebookLimitsAvailable);
			string empty = string.Empty;
			if (Storager.hasKey("FacebookControllerSocialGunEventStartedKey"))
			{
				empty = Storager.getString("FacebookControllerSocialGunEventStartedKey", false);
				DateTime dateTime = new DateTime();
				if (!DateTime.TryParse(empty, out dateTime))
				{
					empty = DateTime.UtcNow.ToString("s");
					Storager.setString("FacebookControllerSocialGunEventStartedKey", empty, false);
				}
			}
			else
			{
				empty = DateTime.UtcNow.ToString("s");
				Storager.setString("FacebookControllerSocialGunEventStartedKey", empty, false);
			}
			if (!DateTime.TryParse(empty, out this.socialEventStartTime))
			{
				UnityEngine.Debug.LogError("FacebookController: invalid timeStartEvent");
			}
			else
			{
				this.socialGunEventActive = this.CurrentSocialGunEventState();
			}
		}
	}

	public bool CanPostStoryWithPriority(FacebookController.StoryPriority priority)
	{
		bool flag;
		try
		{
			flag = (priority != FacebookController.StoryPriority.Green ? (
				from rec in this.storiesPostHistory
				where int.Parse((string)rec["priority"]) == (int)priority
				select rec).Count<Dictionary<string, object>>() < FacebookController.StoryPostLimits[priority] : this.storiesPostHistory.Count < FacebookController.StoryPostLimits[priority]);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogErrorFormat("Caught exception in CanPostStoryWithPriority:\n{0}", new object[] { exception });
			return false;
		}
		return flag;
	}

	internal static void CheckAndGiveFacebookReward(string context)
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		if (context == null)
		{
			throw new ArgumentNullException("context");
		}
		if (FacebookController.FacebookSupported && Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 0 && Storager.getInt(Defs.FacebookRewardGainStarted, false) == 1 && FB.IsLoggedIn)
		{
			Storager.setInt(Defs.FacebookRewardGainStarted, 0, false);
			Storager.setInt(Defs.IsFacebookLoginRewardaGained, 1, true);
			BankController.AddGems(10, true, AnalyticsConstants.AccrualType.Earned);
			TutorialQuestManager.Instance.AddFulfilledQuest("loginFacebook");
			QuestMediator.NotifySocialInteraction("loginFacebook");
			ShopNGUIController.ProvideShopItemOnStarterPackBoguht(ShopNGUIController.CategoryNames.SkinsCategory, "61", 1, false, 0, null, null, false, true, false);
			AnalyticsFacade.SendCustomEvent("Virality", new Dictionary<string, object>()
			{
				{ "Login Facebook", context }
			});
			FlurryPluginWrapper.LogEventToAppsFlyer("Virality", new Dictionary<string, string>()
			{
				{ "Login Facebook", context }
			});
			WeaponManager.AddExclusiveWeaponToWeaponStructures(WeaponManager.SocialGunWN);
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
			UnityEngine.Debug.LogError(string.Concat("Exeption in CleanStoryPostHistory:\n", exception));
		}
	}

	private bool CurrentSocialGunEventState()
	{
		return (!FacebookController.FacebookSupported || Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) != 0 || !((DateTime.UtcNow - this.socialEventStartTime) < this.DurationSocialGunEvent) || ExpController.LobbyLevel <= 1 ? false : !MainMenuController.SavedShwonLobbyLevelIsLessThanActual());
	}

	public static void FBGet(string graphPath, Action<IDictionary<string, object>> act, Action<IResult> onError = null)
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		FB.API(graphPath, HttpMethod.GET, (IGraphResult result) => {
			try
			{
				FacebookController.PrintFBResult(result);
				act(result.ResultDictionary);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				if (onError != null)
				{
					onError(result);
				}
				UnityEngine.Debug.LogError(string.Concat("Exception FBGet: ", exception));
			}
		}, (IDictionary<string, string>)null);
	}

	public static void FBPost(string graphPath, Dictionary<string, string> pars, Action<IDictionary<string, object>> act, Action<IResult> actionWithFBResult = null)
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		FB.API(graphPath, HttpMethod.POST, (IGraphResult result) => {
			try
			{
				if (actionWithFBResult != null)
				{
					actionWithFBResult(result);
				}
				FacebookController.PrintFBResult(result);
				act(result.ResultDictionary);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogError(string.Concat("Exception FBPost: ", exception));
			}
		}, pars);
	}

	[DebuggerHidden]
	private IEnumerator GetOurFbId()
	{
		return new FacebookController.u003cGetOurFbIdu003ec__Iterator1B8();
	}

	public string GetTimeToEndSocialGunEvent()
	{
		if (!this.SocialGunEventActive)
		{
			return string.Empty;
		}
		TimeSpan durationSocialGunEvent = (this.socialEventStartTime + this.DurationSocialGunEvent) - DateTime.UtcNow;
		return string.Format("{0:00}:{1:00}:{2:00}", durationSocialGunEvent.Hours, durationSocialGunEvent.Minutes, durationSocialGunEvent.Seconds);
	}

	private void HandleNewFacebookLimitsAvailable(int greenLimit, int redLimit)
	{
		FacebookController.StoryPostLimits[FacebookController.StoryPriority.Green] = greenLimit;
		FacebookController.StoryPostLimits[FacebookController.StoryPriority.Red] = redLimit;
	}

	private void InitFacebook()
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		InitDelegate initDelegate = () => {
			try
			{
				FB.ActivateApp();
				if (FB.IsLoggedIn)
				{
					base.StartCoroutine(this.GetOurFbId());
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				UnityEngine.Debug.LogException(exception);
				UnityEngine.Debug.LogError(string.Concat("[Rilisoft] Exception in onInitComplete calback of FB.Init() method. Stacktrace: ", exception.StackTrace));
			}
		};
		try
		{
			FB.Init(initDelegate, (bool isGameShown) => {
			}, null);
		}
		catch (NotImplementedException notImplementedException)
		{
			UnityEngine.Debug.LogWarningFormat("Catching exception during FB.Init(): {0}", new object[] { notImplementedException.Message });
		}
	}

	private void InitStoryPostHistoryKey()
	{
		if (!Storager.hasKey("FacebookControllerStoryPostHistoryKey"))
		{
			Storager.setString("FacebookControllerStoryPostHistoryKey", "[]", false);
		}
	}

	public void InputFacebookFriends(Action onSuccess = null, bool dontRelogin = false)
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		this.SetMyId(onSuccess, dontRelogin);
	}

	public void InvitePlayer(Action onComplete = null)
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		if (this.InvitePlayerApiIsRunning)
		{
			return;
		}
		this.InvitePlayerApiIsRunning = true;
		Action action = () => FB.AppRequest(this.messageInvite, null, null, null, null, string.Empty, this.titleInvite, (IAppRequestResult result) => {
			this.InvitePlayerApiIsRunning = false;
			FacebookController.PrintFBResult(result);
			if (onComplete != null)
			{
				onComplete();
			}
		});
		FacebookController.RunApiWithAskForPermissions(action, "publish_actions", false, () => this.InvitePlayerApiIsRunning = false);
	}

	public bool IsNeedShowGunFroLoginWindow()
	{
		int num = Storager.getInt("FacebookController.CountShownGunForLogin", false);
		return (!this.socialGunEventActive || num >= 1 || !SceneLoader.ActiveSceneName.Equals(Defs.MainMenuScene) ? false : Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 0);
	}

	[DebuggerHidden]
	private IEnumerator LoadAvatar(int userCount, string url)
	{
		FacebookController.u003cLoadAvataru003ec__Iterator1B7 variable = null;
		return variable;
	}

	private void LoadStoryPostHistory()
	{
		try
		{
			List<object> objs = Json.Deserialize(Storager.getString("FacebookControllerStoryPostHistoryKey", false)) as List<object>;
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

	public static void LogEvent(string eventName, Dictionary<string, object> parameters = null)
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		try
		{
			FB.LogAppEvent(eventName, null, parameters);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
	}

	public static void Login(Action onSuccess = null, Action onFailure = null, string context = "Unknown", Action onSuccessAfterPublishPermissions = null)
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		if (Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 0)
		{
			Storager.setInt(Defs.FacebookRewardGainStarted, 1, false);
		}
		FacebookController.LoggingIn = true;
		try
		{
			List<string> strs = new List<string>()
			{
				"user_friends"
			};
			FB.LogInWithReadPermissions(strs, (ILoginResult result) => {
				FacebookController.LoggingIn = false;
				FacebookController.PrintFBResult(result);
				FacebookController.CheckAndGiveFacebookReward(context);
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
					catch (Exception exception1)
					{
						UnityEngine.Debug.LogError(string.Concat("FacebookController Login ReceivedSelfID exception: ", exception1));
					}
					try
					{
						if (FacebookController.sharedController != null)
						{
							FacebookController.sharedController.InputFacebookFriends(null, false);
						}
					}
					catch (Exception exception2)
					{
						UnityEngine.Debug.LogError(string.Concat("FacebookController Login InputFacebookFriends exception: ", exception2));
					}
					CoroutineRunner.Instance.StartCoroutine(FacebookController.RunActionAfterDelay(() => {
						FacebookController.LoggingIn = true;
						try
						{
							FB.LogInWithPublishPermissions(new List<string>()
							{
								"publish_actions"
							}, (ILoginResult publishLoginResult) => {
								FacebookController.LoggingIn = false;
								FacebookController.PrintFBResult(publishLoginResult);
								if (string.IsNullOrEmpty(publishLoginResult.Error) && !publishLoginResult.Cancelled && onSuccessAfterPublishPermissions != null)
								{
									onSuccessAfterPublishPermissions();
								}
							});
						}
						catch (Exception exception)
						{
							UnityEngine.Debug.LogError(string.Concat("Exception in Facebook Login: ", exception));
							FacebookController.LoggingIn = false;
						}
					}));
				}
				else if (onFailure != null)
				{
					onFailure();
				}
			});
		}
		catch (Exception exception3)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in Facebook Login: ", exception3));
			FacebookController.LoggingIn = false;
		}
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			if (FB.IsInitialized)
			{
				FB.ActivateApp();
			}
			this.LoadStoryPostHistory();
			this.UpdateCountShownSocialGunWindowByTimeCondition();
		}
		else
		{
			this.SaveStoryPostHistory();
		}
	}

	private void OnDestroy()
	{
		this.SaveStoryPostHistory();
		if (FacebookController.FacebookSupported)
		{
			FriendsController.NewFacebookLimitsAvailable -= new Action<int, int>(this.HandleNewFacebookLimitsAvailable);
		}
	}

	public void PostMessage(string _message, Action<string, object> _completionHandler)
	{
		UnityEngine.Debug.Log("Post to Facebook");
	}

	public static void PostOpenGraphStory(string action, string obj, FacebookController.StoryPriority priority, Dictionary<string, string> pars = null)
	{
		Action<IDictionary<string, object>> action1 = null;
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		FacebookController.RunApiWithAskForPermissions(() => {
			string str = string.Concat("https://secure.pixelgunserver.com/fb/ogobjects.php?type=", WWW.EscapeURL(obj));
			if (pars != null)
			{
				foreach (KeyValuePair<string, string> par in pars)
				{
					str = string.Concat(new string[] { str, "&", par.Key.Replace(" "[0], "_"[0]), "=", WWW.EscapeURL(par.Value) });
				}
			}
			string str1 = string.Concat("/me/pixelgun:", action);
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ obj, str },
				{ "fb:explicitly_shared", "true" }
			};
			if (action1 == null)
			{
				action1 = (IDictionary<string, object> result) => {
				};
			}
			FacebookController.FBPost(str1, strs, action1, (IResult result) => {
				if (result != null && result.Error == null)
				{
					FacebookController.RegisterStoryPostedWithPriority(priority);
				}
			});
		}, "publish_actions", false, null);
	}

	public static void PrintFBResult(IResult result)
	{
	}

	private static void RegisterStoryPostedWithPriority(FacebookController.StoryPriority priority)
	{
		if (FacebookController.sharedController == null)
		{
			return;
		}
		FacebookController.sharedController.RegisterStoryPostedWithPriorityCore(priority);
	}

	private void RegisterStoryPostedWithPriorityCore(FacebookController.StoryPriority priority)
	{
		if (!FacebookController.FacebookSupported)
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

	[DebuggerHidden]
	private static IEnumerator RunActionAfterDelay(Action action)
	{
		FacebookController.u003cRunActionAfterDelayu003ec__Iterator1B6 variable = null;
		return variable;
	}

	public static void RunApiWithAskForPermissions(Action runApi, string requiredPermission = "", bool dontRelogin = false, Action onFailToRunApi = null)
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		if (dontRelogin)
		{
			if (FB.IsLoggedIn && runApi != null)
			{
				runApi();
			}
			return;
		}
		int num = 0;
		Action<bool> action1 = null;
		Action action2 = null;
		Action<bool> action3 = (bool loginIfNoPermissions) => {
			Func<object, bool> func = null;
			Func<object, string> func1 = null;
			if (string.IsNullOrEmpty(requiredPermission))
			{
				runApi();
			}
			else
			{
				FacebookController.FBGet("/me/permissions?limit=500", (IDictionary<string, object> result) => {
					List<object> item = result["data"] as List<object>;
					if (func == null)
					{
						func = (object p) => (p as Dictionary<string, object>)["status"].Equals("granted");
					}
					IEnumerable<object> objs = item.Where<object>(func);
					if (func1 == null)
					{
						func1 = (object p) => (string)(p as Dictionary<string, object>)["permission"];
					}
					if (objs.Select<object, string>(func1).ToList<string>().Contains(requiredPermission))
					{
						runApi();
					}
					else if (loginIfNoPermissions)
					{
						action1(false);
					}
					else if (onFailToRunApi != null)
					{
						onFailToRunApi();
					}
				}, (IResult result) => {
					if (result != null && result.RawResult != null && result.RawResult.Contains("OAuthException"))
					{
						action2();
						num++;
					}
					else if (onFailToRunApi != null)
					{
						onFailToRunApi();
					}
				});
			}
		};
		action1 = (bool bothReadAndWriteLogins) => {
			Action action = () => {
				List<string> strs;
				FacebookDelegate<ILoginResult> loggingIn = (ILoginResult result) => {
					FacebookController.LoggingIn = false;
					FacebookController.PrintFBResult(result);
					if (FB.IsLoggedIn)
					{
						action3(false);
					}
					else if (onFailToRunApi != null)
					{
						onFailToRunApi();
					}
				};
				if (requiredPermission != "publish_actions")
				{
					FacebookController.LoggingIn = true;
					try
					{
						strs = new List<string>()
						{
							requiredPermission
						};
						FB.LogInWithReadPermissions(strs, loggingIn);
					}
					catch (Exception exception)
					{
						UnityEngine.Debug.LogError(string.Concat("Exception in Facebook Login: ", exception));
						FacebookController.LoggingIn = false;
					}
				}
				else
				{
					FacebookController.LoggingIn = true;
					try
					{
						strs = new List<string>()
						{
							requiredPermission
						};
						FB.LogInWithPublishPermissions(strs, loggingIn);
					}
					catch (Exception exception1)
					{
						UnityEngine.Debug.LogError(string.Concat("Exception in Facebook Login: ", exception1));
						FacebookController.LoggingIn = false;
					}
				}
			};
			if (!bothReadAndWriteLogins || !(requiredPermission == "publish_actions"))
			{
				action();
			}
			else
			{
				FacebookController.LoggingIn = true;
				try
				{
					FB.LogInWithReadPermissions(new List<string>(), (ILoginResult result) => {
						FacebookController.LoggingIn = false;
						FacebookController.PrintFBResult(result);
						if (!string.IsNullOrEmpty(result.Error) || result.Cancelled)
						{
							UnityEngine.Debug.LogError("LogInWithReadPermissions: ! (string.IsNullOrEmpty(result.Error) && ! result.Cancelled)");
						}
						else
						{
							CoroutineRunner.Instance.StartCoroutine(FacebookController.RunActionAfterDelay(action));
						}
					});
				}
				catch (Exception exception2)
				{
					UnityEngine.Debug.LogError(string.Concat("Exception in Facebook Login: ", exception2));
					FacebookController.LoggingIn = false;
				}
			}
		};
		action2 = () => {
			FB.LogOut();
			FacebookController.Login(null, null, "Unknown", () => action3(false));
		};
		if (FB.IsLoggedIn)
		{
			action3(true);
		}
		else
		{
			action1(requiredPermission == "publish_actions");
		}
	}

	private void SaveStoryPostHistory()
	{
		Storager.setString("FacebookControllerStoryPostHistoryKey", Json.Serialize(this.storiesPostHistory), false);
	}

	public void SetMyId(Action onSuccess = null, bool dontRelogin = false)
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		FacebookController.RunApiWithAskForPermissions(() => FacebookController.FBGet("/me/friends?fields=id,name,installed&limit=1000000", (IDictionary<string, object> result) => {
			IList item = result["data"] as IList;
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log(string.Concat("Result facebook friends", result.ToString()));
			}
			this.friendsList.Clear();
			for (int i = 0; i < item.Count; i++)
			{
				IDictionary dictionaries = item[i] as IDictionary;
				this.friendsList.Add(new Friend(dictionaries["name"] as string, dictionaries["id"].ToString(), dictionaries["installed"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase)));
			}
			if (onSuccess != null)
			{
				onSuccess();
			}
		}, null), "user_friends", dontRelogin, null);
	}

	public static void ShowPostDialog()
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		PlayerPrefs.SetInt("PostFacebookCount", PlayerPrefs.GetInt("PostFacebookCount", 0) + 1);
		PlayerPrefs.Save();
		if (PlayerPrefs.GetInt("Active_loyal_users_payed_send", 0) == 0 && PlayerPrefs.GetInt("PostFacebookCount", 0) > 2 && StoreKitEventListener.GetDollarsSpent() > 0 && PlayerPrefs.GetInt("PostVideo", 0) > 0)
		{
			FacebookController.LogEvent("Active_loyal_users_payed", null);
			PlayerPrefs.SetInt("Active_loyal_users_payed_send", 1);
		}
		if (PlayerPrefs.GetInt("Active_loyal_users_send", 0) == 0 && PlayerPrefs.GetInt("PostFacebookCount", 0) > 2 && PlayerPrefs.GetInt("PostVideo", 0) > 0)
		{
			FacebookController.LogEvent("Active_loyal_users", null);
			PlayerPrefs.SetInt("Active_loyal_users_send", 1);
		}
		Dictionary<string, object> strs = new Dictionary<string, object>()
		{
			{ "link", Defs2.ApplicationUrl },
			{ "name", "Pixel Gun 3D" },
			{ "picture", "http://pixelgun3d.com/iconforpost.png" },
			{ "caption", "I've just played the super battle in Pixel Gun 3D :)" },
			{ "description", "DOWNLOAD IT FOR FREE AND JOIN ME NOW!" }
		};
		Dictionary<string, object> strs1 = strs;
		Uri uri = new Uri((string)strs1["picture"]);
		FB.FeedShare(string.Empty, new Uri((string)strs1["link"]), (string)strs1["name"], (string)strs1["caption"], (string)strs1["description"], uri, string.Empty, null);
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		FacebookController.sharedController = this;
		if (FacebookController.FacebookSupported)
		{
			this.InitFacebook();
		}
		this.InitStoryPostHistoryKey();
		this.LoadStoryPostHistory();
		this.UpdateCountShownSocialGunWindowByTimeCondition();
	}

	private void Update()
	{
		bool flag = this.CurrentSocialGunEventState();
		if (this.socialGunEventActive != flag)
		{
			this.socialGunEventActive = flag;
			Action<bool> action = FacebookController.SocialGunEventStateChanged;
			if (action != null)
			{
				action(this.SocialGunEventActive);
			}
		}
		if (Time.realtimeSinceStartup - this._timeSinceLastStoryPostHistoryClean > 10f)
		{
			this.CleanStoryPostHistory();
		}
		if (FacebookController.FacebookSupported && !FB.IsLoggedIn && FriendsController.sharedController != null && !string.IsNullOrEmpty(FriendsController.sharedController.id_fb))
		{
			Action<string> action1 = FacebookController.ReceivedSelfID;
			if (action1 != null)
			{
				action1(string.Empty);
			}
		}
	}

	private void UpdateCountShownSocialGunWindowByTimeCondition()
	{
		if (FacebookController.FacebookSupported)
		{
			string empty = string.Empty;
			if (Storager.hasKey(Defs.LastTimeShowSocialGun))
			{
				empty = Storager.getString(Defs.LastTimeShowSocialGun, false);
			}
			else
			{
				Storager.setString(Defs.LastTimeShowSocialGun, empty, false);
			}
			if (string.IsNullOrEmpty(empty))
			{
				return;
			}
			DateTime dateTime = new DateTime();
			if (!DateTime.TryParse(empty, out dateTime))
			{
				return;
			}
			if ((DateTime.UtcNow - dateTime) >= this.TimeBetweenSocialGunBannerSeries)
			{
				Storager.setInt("FacebookController.CountShownGunForLogin", 0, false);
			}
		}
	}

	public void UpdateCountShownWindowByShowCondition()
	{
		int num = Storager.getInt("FacebookController.CountShownGunForLogin", false);
		string lastTimeShowSocialGun = Defs.LastTimeShowSocialGun;
		DateTime utcNow = DateTime.UtcNow;
		Storager.setString(lastTimeShowSocialGun, utcNow.ToString("s"), false);
		Storager.setInt("FacebookController.CountShownGunForLogin", num + 1, false);
	}

	public static event Action<string> PostCompleted;

	public static event Action<string> ReceivedSelfID;

	public static event Action<bool> SocialGunEventStateChanged;

	public enum StoryPriority
	{
		Green,
		Red,
		MultyWinLimit,
		ArenaLimit
	}
}