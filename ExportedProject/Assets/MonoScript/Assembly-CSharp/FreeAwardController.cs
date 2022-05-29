using FyberPlugin;
using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

internal sealed class FreeAwardController : MonoBehaviour
{
	private const string AdvertTimeDuringCurrentPeriodKey = "AdvertTimeDuringCurrentPeriod";

	public const string PendingFreeAwardKey = "PendingFreeAward";

	public Camera renderCamera;

	public FreeAwardView view;

	private static FreeAwardController _instance;

	private int _adProviderIndex;

	private FreeAwardController.State _currentState = FreeAwardController.IdleState.Instance;

	private IDisposable _backSubscription;

	private static bool _initializedOnce;

	private DateTime _currentTime;

	private EventHandler<FreeAwardController.StateEventArgs> StateChanged;

	public int CountMoneyForAward
	{
		get
		{
			if (PromoActionsManager.MobileAdvert == null)
			{
				return 1;
			}
			return (!MobileAdManager.IsPayingUser() ? PromoActionsManager.MobileAdvert.AwardCoinsNonpaying : PromoActionsManager.MobileAdvert.AwardCoinsPaying);
		}
	}

	public string CurrencyForAward
	{
		get
		{
			if (PromoActionsManager.MobileAdvert == null)
			{
				return "Coins";
			}
			return (!MobileAdManager.IsPayingUser() ? PromoActionsManager.MobileAdvert.AwardCoinsNonpayingCurrency : PromoActionsManager.MobileAdvert.AwardCoinsPayingCurrency);
		}
	}

	private FreeAwardController.State CurrentState
	{
		get
		{
			return this._currentState;
		}
		set
		{
			if (value == null)
			{
				return;
			}
			if (this._backSubscription != null)
			{
				this._backSubscription.Dispose();
				this._backSubscription = null;
			}
			if (!(value is FreeAwardController.IdleState))
			{
				this._backSubscription = BackSystem.Instance.Register(new Action(this.HandleClose), "Rewarded Video");
			}
			if (this.view != null)
			{
				this.view.CurrentState = value;
			}
			FreeAwardController.State state = this._currentState;
			this._currentState = value;
			EventHandler<FreeAwardController.StateEventArgs> stateChanged = this.StateChanged;
			if (stateChanged != null)
			{
				FreeAwardController.StateEventArgs stateEventArg = new FreeAwardController.StateEventArgs()
				{
					State = value,
					OldState = state
				};
				stateChanged(this, stateEventArg);
			}
		}
	}

	private DateTime CurrentTime
	{
		get
		{
			return DateTime.UtcNow;
		}
	}

	public static bool FreeAwardChestIsInIdleState
	{
		get
		{
			return (FreeAwardController.Instance == null ? true : FreeAwardController.Instance.IsInState<FreeAwardController.IdleState>());
		}
	}

	internal static Future<object> FyberVideoLoaded
	{
		get;
		set;
	}

	public static FreeAwardController Instance
	{
		get
		{
			return FreeAwardController._instance;
		}
	}

	public AdProvider Provider
	{
		get
		{
			return this.GetProviderByIndex(this._adProviderIndex);
		}
	}

	public int ProviderClampedIndex
	{
		get
		{
			if (PromoActionsManager.MobileAdvert == null)
			{
				return -1;
			}
			if (PromoActionsManager.MobileAdvert.AdProviders.Count == 0)
			{
				return 0;
			}
			return this._adProviderIndex % PromoActionsManager.MobileAdvert.AdProviders.Count;
		}
	}

	public int RoundIndex
	{
		get
		{
			if (PromoActionsManager.MobileAdvert == null)
			{
				return -1;
			}
			if (PromoActionsManager.MobileAdvert.AdProviders.Count == 0)
			{
				return 0;
			}
			return this._adProviderIndex / PromoActionsManager.MobileAdvert.AdProviders.Count;
		}
	}

	public FreeAwardController()
	{
	}

	public int AddAdvertTime(DateTime time)
	{
		object obj;
		List<string> strs;
		string str = time.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
		string str1 = time.ToString("T", CultureInfo.InvariantCulture);
		bool flag = Storager.hasKey("AdvertTimeDuringCurrentPeriod");
		if (!flag)
		{
			Dictionary<string, object> strs1 = new Dictionary<string, object>();
			strs = new List<string>(1)
			{
				str1
			};
			strs1.Add(str, strs);
			this.SetAdvertTime(strs1);
			return 1;
		}
		Dictionary<string, object> strs2 = Json.Deserialize((!flag ? "{}" : Storager.getString("AdvertTimeDuringCurrentPeriod", false))) as Dictionary<string, object> ?? new Dictionary<string, object>();
		if (!strs2.TryGetValue(str, out obj))
		{
			strs = new List<string>(1)
			{
				str1
			};
			strs2[str] = strs;
			this.SetAdvertTime(strs2);
			return 1;
		}
		List<string> list = (obj as List<object> ?? new List<object>()).OfType<string>().ToList<string>();
		list.Add(str1);
		strs2[str] = list.ToList<string>();
		this.SetAdvertTime(strs2);
		return list.Count;
	}

	private static void AddEmptyEntryForAdvertTime(DateTime date)
	{
		string str = date.ToString("yyyy-MM-dd");
		Action action = () => Storager.setString("AdvertTimeDuringCurrentPeriod", Json.Serialize(new Dictionary<string, object>()
		{
			{ str, new string[0] }
		}), false);
		if (!Storager.hasKey("AdvertTimeDuringCurrentPeriod"))
		{
			action();
			return;
		}
		Dictionary<string, object> strs = Json.Deserialize(Storager.getString("AdvertTimeDuringCurrentPeriod", false)) as Dictionary<string, object>;
		if (strs == null)
		{
			action();
			return;
		}
		if (strs.ContainsKey(str))
		{
			return;
		}
		strs.Add(str, new string[0]);
		Storager.setString("AdvertTimeDuringCurrentPeriod", Json.Serialize(strs), false);
	}

	internal bool AdvertCountLessThanLimit()
	{
		List<double> nums = (!MobileAdManager.IsPayingUser() ? PromoActionsManager.MobileAdvert.RewardedVideoDelayMinutesNonpaying : PromoActionsManager.MobileAdvert.RewardedVideoDelayMinutesPaying);
		int count = nums.Count;
		int advertCountDuringCurrentPeriod = this.GetAdvertCountDuringCurrentPeriod();
		if (advertCountDuringCurrentPeriod >= count)
		{
			return false;
		}
		DateTime utcNow = DateTime.UtcNow;
		TimeSpan timeSpan = TimeSpan.FromMinutes(nums[advertCountDuringCurrentPeriod]);
		return (utcNow + timeSpan).Date <= utcNow.Date;
	}

	private void Awake()
	{
		this._currentTime = DateTime.UtcNow;
		if (FreeAwardController._instance == null)
		{
			FreeAwardController._instance = this;
		}
		this.CurrentState = FreeAwardController.IdleState.Instance;
	}

	public int GetAdvertCountDuringCurrentPeriod()
	{
		object obj;
		if (!Storager.hasKey("AdvertTimeDuringCurrentPeriod"))
		{
			return 0;
		}
		string str = Storager.getString("AdvertTimeDuringCurrentPeriod", false);
		Dictionary<string, object> strs = Json.Deserialize(str) as Dictionary<string, object>;
		if (strs == null)
		{
			if (Application.isEditor || Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogWarning(string.Concat("Cannot parse “AdvertTimeDuringCurrentPeriod” to dictionary: “", str, "”"));
			}
			return 0;
		}
		string str1 = this.CurrentTime.ToString("yyyy-MM-dd");
		string str2 = strs.Keys.Max<string>();
		if (str1.CompareTo(str2) < 0)
		{
			int num = 2147483647;
			int.TryParse(str2.Replace("-", string.Empty), out num);
			return Math.Max(10000000, num);
		}
		if (!strs.TryGetValue(str1, out obj))
		{
			return 0;
		}
		return (obj as List<object> ?? new List<object>()).OfType<string>().Count<string>();
	}

	public AdProvider GetProviderByIndex(int index)
	{
		if (PromoActionsManager.MobileAdvert == null)
		{
			return AdProvider.None;
		}
		if (PromoActionsManager.MobileAdvert.AdProviders.Count == 0)
		{
			return (AdProvider)PromoActionsManager.MobileAdvert.AdProvider;
		}
		return (AdProvider)PromoActionsManager.MobileAdvert.AdProviders[index % PromoActionsManager.MobileAdvert.AdProviders.Count];
	}

	public int GiveAwardAndIncrementCount()
	{
		int num = this.AddAdvertTime(DateTime.UtcNow);
		if (this.CurrencyForAward != "GemsCurrency")
		{
			BankController.AddCoins(this.CountMoneyForAward, true, AnalyticsConstants.AccrualType.Earned);
			FlurryEvents.LogCoinsGained(FlurryEvents.GetPlayingMode(), this.CountMoneyForAward);
		}
		else
		{
			BankController.AddGems(this.CountMoneyForAward, true, AnalyticsConstants.AccrualType.Earned);
			FlurryEvents.LogGemsGained(FlurryEvents.GetPlayingMode(), this.CountMoneyForAward);
		}
		Storager.setInt("PendingFreeAward", 0, false);
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
		return num;
	}

	public void HandleClose()
	{
		ButtonClickSound.TryPlayClick();
		if (this.IsInState<FreeAwardController.CloseState>())
		{
			this.HideButtonsShowAward();
		}
		if (this.IsInState<FreeAwardController.AwardState>())
		{
			this.HandleGetAward();
		}
		else
		{
			if (this.Provider == AdProvider.GoogleMobileAds)
			{
				MobileAdManager.Instance.DestroyVideoInterstitial();
			}
			this.CurrentState = FreeAwardController.IdleState.Instance;
		}
	}

	public void HandleDeveloperSkip()
	{
		this.CurrentState = new FreeAwardController.WatchingState();
	}

	public void HandleGetAward()
	{
		FreeAwardController.State watchState;
		int num = this.GiveAwardAndIncrementCount();
		List<double> nums = (!MobileAdManager.IsPayingUser() ? PromoActionsManager.MobileAdvert.RewardedVideoDelayMinutesNonpaying : PromoActionsManager.MobileAdvert.RewardedVideoDelayMinutesPaying);
		if (num >= nums.Count)
		{
			this.CurrentState = FreeAwardController.CloseState.Instance;
		}
		else
		{
			this.ResetAdProvider();
			DateTime utcNow = DateTime.UtcNow;
			TimeSpan timeSpan = TimeSpan.FromMinutes(nums[num]);
			DateTime dateTime = utcNow + timeSpan;
			bool date = utcNow.Date < dateTime.Date;
			date;
			if (!date)
			{
				watchState = new FreeAwardController.WatchState(dateTime);
			}
			else
			{
				watchState = FreeAwardController.CloseState.Instance;
			}
			this.CurrentState = watchState;
		}
	}

	public void HandleWatch()
	{
		this.LoadVideo("HandleWatch");
		this.CurrentState = new FreeAwardController.WaitingState();
	}

	private void HideButtonsShowAward()
	{
		BankController instance = BankController.Instance;
		bool flag = false;
		if (instance != null && instance.InterfaceEnabled)
		{
			instance.bankView.freeAwardButton.gameObject.SetActive(false);
			flag = true;
		}
		MainMenuController mainMenuController = MainMenuController.sharedController;
		if (mainMenuController != null)
		{
			FreeAwardShowHandler component = mainMenuController.freeAwardChestObj.GetComponent<FreeAwardShowHandler>();
			if (!flag)
			{
				component.HideChestWithAnimation();
			}
			else
			{
				component.HideChestTitle();
			}
		}
	}

	public bool IsInState<T>()
	where T : FreeAwardController.State
	{
		return this.CurrentState is T;
	}

	public KeyValuePair<int, DateTime> LastAdvertShow(DateTime date)
	{
		object obj;
		DateTime dateTime;
		KeyValuePair<int, DateTime> keyValuePair = new KeyValuePair<int, DateTime>(-2147483648, DateTime.MinValue);
		string str = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
		bool flag = Storager.hasKey("AdvertTimeDuringCurrentPeriod");
		if (!flag)
		{
			return keyValuePair;
		}
		if (!(Json.Deserialize((!flag ? "{}" : Storager.getString("AdvertTimeDuringCurrentPeriod", false))) as Dictionary<string, object> ?? new Dictionary<string, object>()).TryGetValue(str, out obj))
		{
			return keyValuePair;
		}
		List<object> objs = obj as List<object> ?? new List<object>();
		if (objs.Count == 0)
		{
			return keyValuePair;
		}
		List<string> list = objs.OfType<string>().ToList<string>();
		if (list.Count == 0)
		{
			return keyValuePair;
		}
		string str1 = list.Max<string>();
		if (!DateTime.TryParseExact(str1, "T", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
		{
			UnityEngine.Debug.LogWarning(string.Concat("Couldnot parse last time advert shown: ", str1));
			return keyValuePair;
		}
		DateTime dateTime1 = new DateTime(date.Year, date.Month, date.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, DateTimeKind.Utc);
		return new KeyValuePair<int, DateTime>(list.Count - 1, dateTime1);
	}

	internal static Future<object> LoadFyberVideo(string callerName = null)
	{
		if (callerName == null)
		{
			callerName = string.Empty;
		}
		int roundIndex = FreeAwardController.Instance.RoundIndex;
		Promise<object> promise = new Promise<object>();
		Action<Ad> isDeveloperBuild = null;
		Action<AdFormat> action = null;
		Action<RequestError> isDeveloperBuild1 = null;
		isDeveloperBuild = (Ad ad) => {
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogFormat("LoadFyberVideo > AdAvailable: {{ format: {0}, placementId: '{1}' }}", new object[] { ad.AdFormat, ad.PlacementId });
			}
			promise.SetResult(ad);
			FyberCallback.AdAvailable -= isDeveloperBuild;
			FyberCallback.AdNotAvailable -= action;
			FyberCallback.RequestFail -= isDeveloperBuild1;
		};
		action = (AdFormat adFormat) => {
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogFormat("LoadFyberVideo > AdNotAvailable: {{ format: {0} }}", new object[] { adFormat });
			}
			promise.SetResult(adFormat);
			FyberCallback.AdAvailable -= isDeveloperBuild;
			FyberCallback.AdNotAvailable -= action;
			FyberCallback.RequestFail -= isDeveloperBuild1;
		};
		isDeveloperBuild1 = (RequestError requestError) => {
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogFormat("LoadFyberVideo > RequestFail: {{ requestError: {0} }}", new object[] { requestError.Description });
			}
			promise.SetResult(requestError);
			FyberCallback.AdAvailable -= isDeveloperBuild;
			FyberCallback.AdNotAvailable -= action;
			FyberCallback.RequestFail -= isDeveloperBuild1;
		};
		FyberCallback.AdAvailable += isDeveloperBuild;
		FyberCallback.AdNotAvailable += action;
		FyberCallback.RequestFail += isDeveloperBuild1;
		FreeAwardController.RequestFyberRewardedVideo(roundIndex);
		return promise.Future;
	}

	private void LoadVideo(string callerName = null)
	{
		if (callerName == null)
		{
			callerName = string.Empty;
		}
		if (FreeAwardController.Instance.Provider != AdProvider.GoogleMobileAds)
		{
			if (FreeAwardController.Instance.Provider == AdProvider.Fyber)
			{
				FreeAwardController.FyberVideoLoaded = FreeAwardController.LoadFyberVideo(callerName);
			}
		}
	}

	private void OnDestroy()
	{
		FreeAwardController._instance = null;
	}

	private static void RemoveOldEntriesForAdvertTimes()
	{
		if (!Storager.hasKey("AdvertTimeDuringCurrentPeriod"))
		{
			return;
		}
		Dictionary<string, object> strs = Json.Deserialize(Storager.getString("AdvertTimeDuringCurrentPeriod", false)) as Dictionary<string, object>;
		if (strs == null)
		{
			return;
		}
		if (strs.Keys.Count < 2)
		{
			return;
		}
		string str = strs.Keys.Max<string>();
		string[] array = (
			from k in strs.Keys
			where !k.Equals(str, StringComparison.Ordinal)
			select k).ToArray<string>();
		string[] strArrays = array;
		for (int i = 0; i < (int)strArrays.Length; i++)
		{
			strs.Remove(strArrays[i]);
		}
		Storager.setString("AdvertTimeDuringCurrentPeriod", Json.Serialize(strs), false);
	}

	private static void RequestFyberRewardedVideo(int roundIndex)
	{
		Dictionary<string, string> strs = new Dictionary<string, string>()
		{
			{ "Fyber - Rewarded Video", "Request" }
		};
		FlurryPluginWrapper.LogEventAndDublicateToConsole("Ads Show Stats - Total", strs, true);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("Round {0}", roundIndex + 1);
		stringBuilder.AppendFormat(", Slot {0} ({1})", FreeAwardController.Instance.ProviderClampedIndex + 1, AnalyticsHelper.GetAdProviderName(FreeAwardController.Instance.Provider));
		if (InterstitialManager.Instance.Provider == AdProvider.GoogleMobileAds)
		{
			stringBuilder.AppendFormat(", Unit {0}", MobileAdManager.Instance.VideoAdUnitIndexClamped + 1);
		}
		stringBuilder.Append(" - Request");
		strs = new Dictionary<string, string>()
		{
			{ "ADS - Statistics - Rewarded", stringBuilder.ToString() }
		};
		FlurryPluginWrapper.LogEventAndDublicateToConsole("ADS Statistics Total", strs, true);
		RewardedVideoRequester.Create().NotifyUserOnCompletion(false).Request();
	}

	private void ResetAdProvider()
	{
		int num = this._adProviderIndex;
		AdProvider provider = this.Provider;
		this._adProviderIndex = 0;
		if (provider == AdProvider.GoogleMobileAds && provider != this.Provider)
		{
			MobileAdManager.Instance.DestroyVideoInterstitial();
		}
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log(string.Format("Resetting AdProvider from {0} ({1}) to {2} ({3})", new object[] { num, provider, this._adProviderIndex, this.Provider }));
		}
		MobileAdManager.Instance.ResetVideoAdUnitId();
	}

	private void SetAdvertTime(Dictionary<string, object> d)
	{
		if (d == null)
		{
			d = new Dictionary<string, object>();
		}
		Storager.setString("AdvertTimeDuringCurrentPeriod", Json.Serialize(d) ?? "{}", false);
	}

	private static void SetCookieAcceptPolicy()
	{
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log(string.Format("Setting cookie accept policy is dumb on {0}.", Application.platform));
		}
	}

	internal void SetWatchState(DateTime nextTimeEnabled)
	{
		this.ResetAdProvider();
		this.CurrentState = new FreeAwardController.WatchState(nextTimeEnabled);
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		return new FreeAwardController.u003cStartu003ec__Iterator13A();
	}

	internal int SwitchAdProvider()
	{
		int num = this._adProviderIndex;
		AdProvider provider = this.Provider;
		this._adProviderIndex++;
		if (provider == AdProvider.GoogleMobileAds)
		{
			MobileAdManager.Instance.DestroyVideoInterstitial();
		}
		if (this.Provider == AdProvider.GoogleMobileAds)
		{
			MobileAdManager.Instance.SwitchVideoIdGroup();
		}
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log(string.Format("Switching provider from {0} ({1}) to {2} ({3})", new object[] { num, provider, this._adProviderIndex, this.Provider }));
		}
		return this._adProviderIndex;
	}

	internal bool TimeTamperingDetected()
	{
		if (!Storager.hasKey("AdvertTimeDuringCurrentPeriod"))
		{
			return false;
		}
		string str = Storager.getString("AdvertTimeDuringCurrentPeriod", false);
		Dictionary<string, object> strs = Json.Deserialize(str) as Dictionary<string, object>;
		if (strs == null)
		{
			return false;
		}
		string str1 = strs.Keys.Min<string>();
		string str2 = this.CurrentTime.ToString("yyyy-MM-dd");
		return str2.CompareTo(str1) < 0;
	}

	public T TryGetState<T>()
	where T : FreeAwardController.State
	{
		return (T)(this.CurrentState as T);
	}

	private void Update()
	{
		object description;
		FreeAwardController.WaitingState waitingState = this.TryGetState<FreeAwardController.WaitingState>();
		if (waitingState == null)
		{
			FreeAwardController.WatchingState watchingState = this.TryGetState<FreeAwardController.WatchingState>();
			if (watchingState == null)
			{
				FreeAwardController.ConnectionState connectionState = this.TryGetState<FreeAwardController.ConnectionState>();
				if (connectionState == null)
				{
					return;
				}
				if (Time.realtimeSinceStartup - connectionState.StartTime > 3f)
				{
					this.CurrentState = FreeAwardController.IdleState.Instance;
				}
				return;
			}
			if (Application.isEditor && Time.realtimeSinceStartup - watchingState.StartTime > 1f)
			{
				watchingState.SimulateCallbackInEditor("CLOSE_FINISHED");
			}
			if (watchingState.AdClosed.IsCompleted)
			{
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.LogFormat("[Rilisoft] Watching rewarded video completed: '{0}'", new object[] { watchingState.AdClosed.Result });
				}
				Storager.setInt("PendingFreeAward", 0, false);
				if (watchingState.AdClosed.Result.Equals("CLOSE_FINISHED", StringComparison.Ordinal))
				{
					this.CurrentState = FreeAwardController.AwardState.Instance;
				}
				else if (watchingState.AdClosed.Result.Equals("ERROR", StringComparison.Ordinal))
				{
					this.ResetAdProvider();
					this.CurrentState = new FreeAwardController.WatchState(DateTime.MinValue);
				}
				else if (!watchingState.AdClosed.Result.Equals("CLOSE_ABORTED", StringComparison.Ordinal))
				{
					string str = string.Format("[Rilisoft] Unsupported result for rewarded video: “{0}”", watchingState.AdClosed.Result);
					UnityEngine.Debug.LogWarning(str);
					this.CurrentState = new FreeAwardController.WatchState(DateTime.MinValue);
				}
				else
				{
					this.CurrentState = new FreeAwardController.WatchState(DateTime.MinValue);
				}
			}
			return;
		}
		if (Application.isEditor || Tools.RuntimePlatform == RuntimePlatform.MetroPlayerX64)
		{
			if (Time.realtimeSinceStartup - waitingState.StartTime > (float)PromoActionsManager.MobileAdvert.TimeoutWaitVideo)
			{
				if (this.Provider != AdProvider.GoogleMobileAds)
				{
					this.SwitchAdProvider();
				}
				else if (MobileAdManager.Instance.SwitchVideoAdUnitId())
				{
					this.SwitchAdProvider();
				}
				this.CurrentState = new FreeAwardController.ConnectionState();
				return;
			}
		}
		else if (this.Provider == AdProvider.GoogleMobileAds)
		{
			if (MobileAdManager.Instance.VideoInterstitialState == MobileAdManager.State.Loaded)
			{
				this.CurrentState = new FreeAwardController.WatchingState();
				return;
			}
			if (!string.IsNullOrEmpty(MobileAdManager.Instance.VideoAdFailedToLoadMessage))
			{
				if (Defs.IsDeveloperBuild)
				{
					string str1 = string.Format("Admob loading failed after {0:F3}s of {1}. Keep waiting.", Time.realtimeSinceStartup - waitingState.StartTime, PromoActionsManager.MobileAdvert.TimeoutWaitVideo);
					UnityEngine.Debug.Log(str1);
				}
				if (MobileAdManager.Instance.SwitchVideoAdUnitId())
				{
					int num = this.SwitchAdProvider();
					if (PromoActionsManager.MobileAdvert.AdProviders.Count > 0 && num >= PromoActionsManager.MobileAdvert.CountRoundReplaceProviders * PromoActionsManager.MobileAdvert.AdProviders.Count)
					{
						string str2 = string.Format("Reporting connection issues after {0} switches.  Providers count {1}, rounds count {2}", num, PromoActionsManager.MobileAdvert.AdProviders.Count, PromoActionsManager.MobileAdvert.CountRoundReplaceProviders);
						UnityEngine.Debug.Log(str2);
						this.CurrentState = new FreeAwardController.ConnectionState();
						return;
					}
				}
				this.LoadVideo("Update");
				this.CurrentState = new FreeAwardController.WaitingState(waitingState.StartTime);
				return;
			}
			if (Time.realtimeSinceStartup - waitingState.StartTime > (float)PromoActionsManager.MobileAdvert.TimeoutWaitVideo)
			{
				if (MobileAdManager.Instance.SwitchVideoAdUnitId())
				{
					this.SwitchAdProvider();
				}
				this.CurrentState = new FreeAwardController.ConnectionState();
				return;
			}
		}
		else if (this.Provider == AdProvider.Fyber)
		{
			if (FreeAwardController.FyberVideoLoaded != null && FreeAwardController.FyberVideoLoaded.IsCompleted)
			{
				if (FreeAwardController.FyberVideoLoaded.Result is Ad)
				{
					this.CurrentState = new FreeAwardController.WatchingState();
					return;
				}
				RequestError result = FreeAwardController.FyberVideoLoaded.Result as RequestError;
				if (Defs.IsDeveloperBuild)
				{
					object[] objArray = new object[1];
					if (result == null)
					{
						description = (FreeAwardController.FyberVideoLoaded.Result as AdFormat == AdFormat.OFFER_WALL ? "?" : "Not available");
					}
					else
					{
						description = result.Description;
					}
					objArray[0] = description;
					UnityEngine.Debug.LogFormat("Fyber loading failed: {0}. Keep waiting.", objArray);
				}
				int num1 = this.SwitchAdProvider();
				if (PromoActionsManager.MobileAdvert.AdProviders.Count > 0 && num1 >= PromoActionsManager.MobileAdvert.CountRoundReplaceProviders * PromoActionsManager.MobileAdvert.AdProviders.Count)
				{
					this.CurrentState = new FreeAwardController.ConnectionState();
					return;
				}
				this.LoadVideo("Update");
				this.CurrentState = new FreeAwardController.WaitingState(waitingState.StartTime);
				return;
			}
			if (Time.realtimeSinceStartup - waitingState.StartTime > (float)PromoActionsManager.MobileAdvert.TimeoutWaitVideo)
			{
				this.SwitchAdProvider();
				this.CurrentState = new FreeAwardController.ConnectionState();
				return;
			}
		}
		else if (Time.realtimeSinceStartup - waitingState.StartTime > (float)PromoActionsManager.MobileAdvert.TimeoutWaitVideo)
		{
			this.CurrentState = new FreeAwardController.ConnectionState();
		}
	}

	public event EventHandler<FreeAwardController.StateEventArgs> StateChanged
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.StateChanged += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.StateChanged -= value;
		}
	}

	public sealed class AwardState : FreeAwardController.State
	{
		private readonly static FreeAwardController.AwardState _instance;

		internal static FreeAwardController.AwardState Instance
		{
			get
			{
				return FreeAwardController.AwardState._instance;
			}
		}

		static AwardState()
		{
			FreeAwardController.AwardState._instance = new FreeAwardController.AwardState();
		}

		private AwardState()
		{
		}
	}

	public sealed class CloseState : FreeAwardController.State
	{
		private readonly static FreeAwardController.CloseState _instance;

		internal static FreeAwardController.CloseState Instance
		{
			get
			{
				return FreeAwardController.CloseState._instance;
			}
		}

		static CloseState()
		{
			FreeAwardController.CloseState._instance = new FreeAwardController.CloseState();
		}

		private CloseState()
		{
		}
	}

	public sealed class ConnectionState : FreeAwardController.State
	{
		private readonly float _startTime;

		public float StartTime
		{
			get
			{
				return this._startTime;
			}
		}

		public ConnectionState()
		{
			this._startTime = Time.realtimeSinceStartup;
		}
	}

	public sealed class IdleState : FreeAwardController.State
	{
		private readonly static FreeAwardController.IdleState _instance;

		internal static FreeAwardController.IdleState Instance
		{
			get
			{
				return FreeAwardController.IdleState._instance;
			}
		}

		static IdleState()
		{
			FreeAwardController.IdleState._instance = new FreeAwardController.IdleState();
		}

		private IdleState()
		{
		}
	}

	public abstract class State
	{
		protected State()
		{
		}
	}

	public class StateEventArgs : EventArgs
	{
		public FreeAwardController.State OldState
		{
			get;
			set;
		}

		public FreeAwardController.State State
		{
			get;
			set;
		}

		public StateEventArgs()
		{
		}
	}

	public sealed class WaitingState : FreeAwardController.State
	{
		private readonly float _startTime;

		public float StartTime
		{
			get
			{
				return this._startTime;
			}
		}

		public WaitingState(float startTime)
		{
			this._startTime = startTime;
		}

		public WaitingState() : this(Time.realtimeSinceStartup)
		{
		}
	}

	public sealed class WatchingState : FreeAwardController.State
	{
		private readonly float _startTime;

		private readonly Promise<string> _adClosed;

		public Future<string> AdClosed
		{
			get
			{
				return this._adClosed.Future;
			}
		}

		public float StartTime
		{
			get
			{
				return this._startTime;
			}
		}

		public WatchingState()
		{
			this._startTime = Time.realtimeSinceStartup;
			string str = FreeAwardController.WatchingState.DetermineContext();
			Storager.setInt("PendingFreeAward", (int)FreeAwardController.Instance.Provider, false);
			if (FreeAwardController.Instance.Provider == AdProvider.GoogleMobileAds)
			{
				AdvertisementInfo advertisementInfo = new AdvertisementInfo(FreeAwardController.Instance.RoundIndex, FreeAwardController.Instance.ProviderClampedIndex, MobileAdManager.Instance.VideoAdUnitIndexClamped, null);
				MobileAdManager.Instance.ShowVideoInterstitial(str, () => {
					this.LogAdsEvent("AdMob (Google Mobile Ads) - Video - Impressions", str);
					FlurryPluginWrapper.LogEventAndDublicateToConsole("Ads Show Stats - Total", new Dictionary<string, string>()
					{
						{ "AdMob - Rewarded Video", "Impression" }
					}, true);
					FreeAwardController.WatchingState.LogImpressionDetails(advertisementInfo);
					this._adClosed.SetResult("CLOSE_FINISHED");
				});
			}
			else if (FreeAwardController.Instance.Provider != AdProvider.UnityAds)
			{
				if (FreeAwardController.Instance.Provider != AdProvider.Vungle)
				{
					if (FreeAwardController.Instance.Provider == AdProvider.Fyber)
					{
						AdvertisementInfo advertisementInfo1 = new AdvertisementInfo(FreeAwardController.Instance.RoundIndex, FreeAwardController.Instance.ProviderClampedIndex, 0, null);
						if (!FreeAwardController.FyberVideoLoaded.IsCompleted)
						{
							UnityEngine.Debug.LogWarning("FyberVideoLoaded.IsCompleted: False");
							return;
						}
						Ad result = FreeAwardController.FyberVideoLoaded.Result as Ad;
						if (result == null)
						{
							UnityEngine.Debug.LogWarningFormat("FyberVideoLoaded.Result: {0}", new object[] { FreeAwardController.FyberVideoLoaded.Result });
							return;
						}
						Action<AdResult> action = null;
						action = (AdResult adResult) => {
							FyberCallback.AdFinished -= action;
							this.LogAdsEvent("Fyber (SponsorPay) - Video - Impressions", str);
							FlurryPluginWrapper.LogEventAndDublicateToConsole("Ads Show Stats - Total", new Dictionary<string, string>()
							{
								{ "Fyber - Rewarded Video", string.Concat("Impression: ", adResult.Message) }
							}, true);
							FreeAwardController.WatchingState.LogImpressionDetails(advertisementInfo1);
							this._adClosed.SetResult(adResult.Message);
						};
						FyberCallback.AdFinished += action;
						result.Start();
						FreeAwardController.FyberVideoLoaded = null;
						Dictionary<string, string> strs = new Dictionary<string, string>()
						{
							{ "af_content_type", "Rewarded video" },
							{ "af_content_id", string.Format("Rewarded video ({0})", str) }
						};
						FlurryPluginWrapper.LogEventToAppsFlyer("af_content_view", strs);
					}
				}
			}
		}

		private static string DetermineContext()
		{
			if (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled)
			{
				return "At Lobby";
			}
			if (Defs.isMulti)
			{
				return "Bank (Multiplayer)";
			}
			if (Defs.isCompany)
			{
				return "Bank (Campaign)";
			}
			if (Defs.IsSurvival)
			{
				return "Bank (Survival)";
			}
			return "Bank";
		}

		private void LogAdsEvent(string eventName, string context)
		{
			Dictionary<string, string> strs = new Dictionary<string, string>(3)
			{
				{ "Context", context ?? string.Empty }
			};
			Dictionary<string, string> strs1 = strs;
			if (ExperienceController.sharedController != null)
			{
				strs1.Add("Levels", ExperienceController.sharedController.currentLevel.ToString());
			}
			if (ExpController.Instance != null)
			{
				strs1.Add("Tiers", ExpController.Instance.OurTier.ToString());
			}
			FlurryPluginWrapper.LogEventAndDublicateToConsole(eventName, strs1, true);
		}

		private static void LogImpressionDetails(AdvertisementInfo advertisementInfo)
		{
			if (advertisementInfo == null)
			{
				advertisementInfo = AdvertisementInfo.Default;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Round {0}", advertisementInfo.Round + 1);
			stringBuilder.AppendFormat(", Slot {0} ({1})", advertisementInfo.Slot + 1, AnalyticsHelper.GetAdProviderName(FreeAwardController.Instance.GetProviderByIndex(advertisementInfo.Slot)));
			if (InterstitialManager.Instance.Provider == AdProvider.GoogleMobileAds)
			{
				stringBuilder.AppendFormat(", Unit {0}", advertisementInfo.Unit + 1);
			}
			if (!string.IsNullOrEmpty(advertisementInfo.Details))
			{
				stringBuilder.AppendFormat(" - Impression: {0}", advertisementInfo.Details);
			}
			else
			{
				stringBuilder.Append(" - Impression");
			}
		}

		private void LogUnityAdsClick(string result)
		{
			Dictionary<string, string> strs = new Dictionary<string, string>(3)
			{
				{ "Result", result ?? string.Empty }
			};
			Dictionary<string, string> strs1 = strs;
			if (ExperienceController.sharedController != null)
			{
				strs1.Add("Levels", ExperienceController.sharedController.currentLevel.ToString());
			}
			if (ExpController.Instance != null)
			{
				strs1.Add("Tiers", ExpController.Instance.OurTier.ToString());
			}
			FlurryPluginWrapper.LogEventAndDublicateToConsole("Unity Ads - Video - Clicks", strs1, true);
		}

		private void LogUnityAdsImpression(string context)
		{
			this.LogAdsEvent("Unity Ads - Video - Impressions", context);
		}

		private void LogVungleImpression(string context)
		{
			this.LogAdsEvent("Vungle - Video - Impressions", context);
		}

		internal void SimulateCallbackInEditor(string result)
		{
			if (Application.isEditor)
			{
				this._adClosed.SetResult(result ?? string.Empty);
			}
		}
	}

	public sealed class WatchState : FreeAwardController.State
	{
		private readonly DateTime _nextTimeEnabled;

		public WatchState(DateTime nextTimeEnabled)
		{
			DateTime utcNow = DateTime.UtcNow;
			if (Defs.IsDeveloperBuild && utcNow < nextTimeEnabled)
			{
				UnityEngine.Debug.Log(string.Concat("Watching state inactive: need to wait till UTC ", nextTimeEnabled.ToString("T", CultureInfo.InvariantCulture)));
			}
			this._nextTimeEnabled = nextTimeEnabled;
		}

		public TimeSpan GetEstimatedTimeSpan()
		{
			return this._nextTimeEnabled - DateTime.UtcNow;
		}
	}
}