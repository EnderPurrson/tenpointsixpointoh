using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

public sealed class PromoActionsManager : MonoBehaviour
{
	private const float OffersUpdateTimeout = 900f;

	private const float EventX3UpdateTimeout = 930f;

	private const float AdvertInfoTimeout = 960f;

	public const long NewbieEventX3Duration = 259200L;

	public const long NewbieEventX3Timeout = 259200L;

	private const float BestBuyInfoTimeout = 1020f;

	public const int ShownCountDaysOfValor = 1;

	private const float DayOfValorInfoTimeout = 1050f;

	public static PromoActionsManager sharedManager;

	public static bool ActionsAvailable;

	public Dictionary<string, List<SaltedInt>> discounts = new Dictionary<string, List<SaltedInt>>();

	public List<string> topSellers = new List<string>();

	public List<string> news = new List<string>();

	private float startTime;

	private string promoActionAddress = URLs.PromoActionsTest;

	private float _eventX3GetInfoStartTime;

	private float _eventX3LastCheckTime;

	private long _newbieEventX3StartTime;

	private long _newbieEventX3StartTimeAdditional;

	private long _eventX3StartTime;

	private long _eventX3Duration;

	private bool _eventX3Active;

	private long _eventX3AmazonEventStartTime;

	private long _eventX3AmazonEventEndTime;

	private List<long> _eventX3AmazonEventValidTimeZone = new List<long>();

	private bool _eventX3AmazonEventActive;

	private float _advertGetInfoStartTime;

	private static PromoActionsManager.AdvertInfo _paidAdvert;

	private static PromoActionsManager.AdvertInfo _freeAdvert;

	private static PromoActionsManager.ReplaceAdmobPerelivInfo _replaceAdmobPereliv;

	private static PromoActionsManager.MobileAdvertInfo _mobileAdvert;

	public static float startupTime;

	private bool _isGetEventX3InfoRunning;

	private PromoActionsManager.AmazonEventInfo _amazonEventInfo;

	public static bool x3InfoDownloadaedOnceDuringCurrentRun;

	private bool _isGetAdvertInfoRunning;

	private string _previousResponseText;

	private List<string> _bestBuyIds = new List<string>();

	private bool _isGetBestBuyInfoRunning;

	private float _bestBuyGetInfoStartTime;

	private long _dayOfValorStartTime;

	private long _dayOfValorEndTime;

	private long _dayOfValorMultiplyerForExp;

	private long _dayOfValorMultiplyerForMoney;

	private bool _isGetDayOfValorInfoRunning;

	private float _dayOfValorGetInfoStartTime;

	private static TimeSpan TimeToShowDaysOfValor;

	private TimeSpan _timeToEndDayOfValor;

	public static PromoActionsManager.AdvertInfo Advert
	{
		get
		{
			return (!FlurryPluginWrapper.IsPayingUser() ? PromoActionsManager._freeAdvert : PromoActionsManager._paidAdvert);
		}
	}

	internal PromoActionsManager.AmazonEventInfo AmazonEvent
	{
		get
		{
			return this._amazonEventInfo;
		}
	}

	public static long CurrentUnixTime
	{
		get
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			return (long)(DateTime.UtcNow - dateTime).TotalSeconds;
		}
	}

	public int DayOfValorMultiplyerForExp
	{
		get
		{
			if (!this.IsDayOfValorEventActive)
			{
				return 1;
			}
			return (int)this._dayOfValorMultiplyerForExp;
		}
	}

	public int DayOfValorMultiplyerForMoney
	{
		get
		{
			if (!this.IsDayOfValorEventActive)
			{
				return 1;
			}
			return (int)this._dayOfValorMultiplyerForMoney;
		}
	}

	public long EventX3RemainedTime
	{
		get
		{
			if (!this.IsEventX3Active)
			{
				return (long)0;
			}
			return this._eventX3StartTime + this._eventX3Duration - PromoActionsManager.CurrentUnixTime;
		}
	}

	public bool IsAmazonEventX3Active
	{
		get
		{
			if (this._amazonEventInfo == null)
			{
				return false;
			}
			if (this._amazonEventInfo.DurationSeconds <= 1E-45f)
			{
				return false;
			}
			if (!PromoActionsManager.CheckTimezone(this._amazonEventInfo.Timezones))
			{
				return false;
			}
			DateTime utcNow = DateTime.UtcNow;
			return (this._amazonEventInfo.StartTime > utcNow ? false : utcNow <= this._amazonEventInfo.EndTime);
		}
	}

	public bool IsDayOfValorEventActive
	{
		get;
		private set;
	}

	public bool IsEventX3Active
	{
		get
		{
			return this._eventX3Active;
		}
	}

	public bool IsNewbieEventX3Active
	{
		get
		{
			if (this._newbieEventX3StartTime == 0)
			{
				return false;
			}
			long currentUnixTime = PromoActionsManager.CurrentUnixTime;
			if (currentUnixTime >= this._newbieEventX3StartTime + (long)259200 + (long)259200)
			{
				this.ResetNewbieX3StartTime();
				return false;
			}
			return (this._newbieEventX3StartTime >= currentUnixTime ? false : currentUnixTime < this._newbieEventX3StartTime + (long)259200);
		}
	}

	private bool IsX3StartTimeAfterNewbieX3TimeoutEndTime
	{
		get
		{
			if (this._newbieEventX3StartTimeAdditional == 0)
			{
				return true;
			}
			long num = this._newbieEventX3StartTimeAdditional + (long)259200 + (long)259200;
			return this._eventX3StartTime >= num;
		}
	}

	public static PromoActionsManager.MobileAdvertInfo MobileAdvert
	{
		get
		{
			return PromoActionsManager._mobileAdvert;
		}
	}

	public static bool MobileAdvertIsReady
	{
		get;
		private set;
	}

	public static PromoActionsManager.ReplaceAdmobPerelivInfo ReplaceAdmobPereliv
	{
		get
		{
			return PromoActionsManager._replaceAdmobPereliv;
		}
	}

	static PromoActionsManager()
	{
		PromoActionsManager.ActionsAvailable = true;
		PromoActionsManager._paidAdvert = new PromoActionsManager.AdvertInfo();
		PromoActionsManager._freeAdvert = new PromoActionsManager.AdvertInfo();
		PromoActionsManager._replaceAdmobPereliv = new PromoActionsManager.ReplaceAdmobPerelivInfo();
		PromoActionsManager._mobileAdvert = new PromoActionsManager.MobileAdvertInfo();
		PromoActionsManager.startupTime = 0f;
		PromoActionsManager.x3InfoDownloadaedOnceDuringCurrentRun = false;
		PromoActionsManager.TimeToShowDaysOfValor = TimeSpan.FromHours(12);
	}

	public PromoActionsManager()
	{
	}

	public static List<string> AllIdsForPromosExceptArmor()
	{
		IEnumerable<string> strs = 
			from kvp in WeaponManager.tagToStoreIDMapping
			where (kvp.Value == null ? false : WeaponManager.storeIDtoDefsSNMapping.ContainsKey(kvp.Value))
			select kvp.Key;
		IEnumerable<string> strs1 = Wear.wear.SelectMany<KeyValuePair<ShopNGUIController.CategoryNames, List<List<string>>>, List<string>>((KeyValuePair<ShopNGUIController.CategoryNames, List<List<string>>> kvp) => (kvp.Key == ShopNGUIController.CategoryNames.ArmorCategory ? new List<List<string>>() : kvp.Value)).SelectMany<List<string>, string>((List<string> list) => list);
		List<string> strs2 = new List<string>()
		{
			"hat_Adamant_3"
		};
		return strs1.Except<string>(strs2).Except<string>(Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0]).Concat<string>(SkinsController.shopKeyFromNameSkin.Keys).Concat<string>(strs).Distinct<string>().ToList<string>();
	}

	private void Awake()
	{
		PromoActionsManager.startupTime = Time.realtimeSinceStartup;
		this.promoActionAddress = URLs.PromoActions;
	}

	[Obsolete]
	private void CheckAmazonEventX3Active()
	{
		if (!this._eventX3Active || !this.CheckAvailabelTimeZoneForAmazonEvent())
		{
			this._eventX3AmazonEventActive = false;
			return;
		}
		bool flag = this._eventX3AmazonEventActive;
		if (this._eventX3AmazonEventStartTime == 0 || this._eventX3AmazonEventEndTime == 0)
		{
			this._eventX3AmazonEventStartTime = (long)0;
			this._eventX3AmazonEventEndTime = (long)0;
			this._eventX3AmazonEventActive = false;
		}
		else
		{
			long currentUnixTime = PromoActionsManager.CurrentUnixTime;
			this._eventX3AmazonEventActive = (this._eventX3StartTime >= currentUnixTime ? false : currentUnixTime < this._eventX3AmazonEventEndTime);
		}
		if (this._eventX3AmazonEventActive != flag && PromoActionsManager.EventAmazonX3Updated != null)
		{
			PromoActionsManager.EventAmazonX3Updated();
		}
	}

	private bool CheckAvailabelTimeZoneForAmazonEvent()
	{
		if (!this._eventX3Active)
		{
			return false;
		}
		if (this._eventX3AmazonEventValidTimeZone == null || this._eventX3AmazonEventValidTimeZone.Count == 0)
		{
			return false;
		}
		TimeSpan offset = DateTimeOffset.Now.Offset;
		for (int i = 0; i < this._eventX3AmazonEventValidTimeZone.Count; i++)
		{
			if (this._eventX3AmazonEventValidTimeZone[i] == (long)offset.Hours)
			{
				return true;
			}
		}
		return false;
	}

	private void CheckDayOfValorActive()
	{
		bool isDayOfValorEventActive = this.IsDayOfValorEventActive;
		if (this._dayOfValorStartTime == 0 || this._dayOfValorEndTime == 0 || ExpController.LobbyLevel < 1)
		{
			this.ClearDataDayOfValor();
			this.IsDayOfValorEventActive = false;
		}
		else
		{
			long currentUnixTime = PromoActionsManager.CurrentUnixTime;
			this.IsDayOfValorEventActive = (this._dayOfValorStartTime >= currentUnixTime ? false : currentUnixTime < this._dayOfValorEndTime);
			this._timeToEndDayOfValor = TimeSpan.FromSeconds((double)(this._dayOfValorEndTime - currentUnixTime));
		}
		if (this.IsDayOfValorEventActive != isDayOfValorEventActive && PromoActionsManager.OnDayOfValorEnable != null)
		{
			PromoActionsManager.OnDayOfValorEnable(this.IsDayOfValorEventActive);
		}
	}

	private void CheckEventX3Active()
	{
		bool flag = this._eventX3Active;
		if (this.IsNewbieEventX3Active)
		{
			this._eventX3StartTime = this._newbieEventX3StartTime;
			this._eventX3Duration = (long)259200;
			this._eventX3Active = true;
		}
		else if (this._eventX3StartTime == 0 || this._eventX3Duration == 0 || !this.IsX3StartTimeAfterNewbieX3TimeoutEndTime)
		{
			this._eventX3StartTime = (long)0;
			this._eventX3Duration = (long)0;
			this._eventX3Active = false;
		}
		else
		{
			long currentUnixTime = PromoActionsManager.CurrentUnixTime;
			this._eventX3Active = (this._eventX3StartTime >= currentUnixTime ? false : currentUnixTime < this._eventX3StartTime + this._eventX3Duration);
		}
		if (this._eventX3Active != flag)
		{
			if (this._eventX3Active)
			{
				PlayerPrefs.SetInt(Defs.EventX3WindowShownCount, 1);
				PlayerPrefs.Save();
			}
			if (PromoActionsManager.EventX3Updated != null)
			{
				PromoActionsManager.EventX3Updated();
			}
		}
	}

	private static bool CheckTimezone(List<int> timezones)
	{
		if (timezones == null)
		{
			return false;
		}
		TimeSpan offset = DateTimeOffset.Now.Offset;
		bool flag = timezones.Any<int>(new Func<int, bool>((object)offset.Hours.Equals));
		return flag;
	}

	private void ClearAll()
	{
		this.discounts.Clear();
		this.topSellers.Clear();
		this.news.Clear();
	}

	private void ClearDataDayOfValor()
	{
		this._dayOfValorStartTime = (long)0;
		this._dayOfValorEndTime = (long)0;
		this._dayOfValorMultiplyerForExp = (long)1;
		this._dayOfValorMultiplyerForMoney = (long)1;
	}

	[DebuggerHidden]
	private IEnumerator DownloadBestBuyInfo()
	{
		PromoActionsManager.u003cDownloadBestBuyInfou003ec__IteratorF9 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator DownloadDayOfValorInfo()
	{
		PromoActionsManager.u003cDownloadDayOfValorInfou003ec__IteratorFB variable = null;
		return variable;
	}

	public void ForceCheckEventX3Active()
	{
		this.CheckEventX3Active();
	}

	[DebuggerHidden]
	public IEnumerator GetActions()
	{
		PromoActionsManager.u003cGetActionsu003ec__IteratorF8 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetActionsLoop(Task futureToWait)
	{
		PromoActionsManager.u003cGetActionsLoopu003ec__IteratorF2 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetAdvertInfo()
	{
		PromoActionsManager.u003cGetAdvertInfou003ec__IteratorF7 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetAdvertInfoLoop(Task futureToWait)
	{
		PromoActionsManager.u003cGetAdvertInfoLoopu003ec__IteratorF4 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetAmazonEventCoroutine()
	{
		PromoActionsManager.u003cGetAmazonEventCoroutineu003ec__IteratorF5 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetBestBuyInfoLoop(Task futureToWait)
	{
		PromoActionsManager.u003cGetBestBuyInfoLoopu003ec__IteratorFA variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetDayOfValorInfoLoop()
	{
		PromoActionsManager.u003cGetDayOfValorInfoLoopu003ec__IteratorFC variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetEventX3Info()
	{
		PromoActionsManager.u003cGetEventX3Infou003ec__IteratorF6 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetEventX3InfoLoop()
	{
		PromoActionsManager.u003cGetEventX3InfoLoopu003ec__IteratorF3 variable = null;
		return variable;
	}

	private long GetNewbieEventX3LastLoggedTime()
	{
		if (this._newbieEventX3StartTime != 0)
		{
			return PromoActionsManager.GetUnixTimeFromStorage(Defs.NewbieEventX3LastLoggedTime);
		}
		return (long)0;
	}

	public string GetTimeToEndDaysOfValor()
	{
		if (!this.IsDayOfValorEventActive)
		{
			return string.Empty;
		}
		if (this._timeToEndDayOfValor.Days <= 0)
		{
			return string.Format("{0:00}:{1:00}:{2:00}", this._timeToEndDayOfValor.Hours, this._timeToEndDayOfValor.Minutes, this._timeToEndDayOfValor.Seconds);
		}
		return string.Format("{0} days {1:00}:{2:00}:{3:00}", new object[] { this._timeToEndDayOfValor.Days, this._timeToEndDayOfValor.Hours, this._timeToEndDayOfValor.Minutes, this._timeToEndDayOfValor.Seconds });
	}

	public static long GetUnixTimeFromStorage(string storageId)
	{
		long num = (long)0;
		if (Storager.hasKey(storageId))
		{
			string str = Storager.getString(storageId, false);
			long.TryParse(str, out num);
		}
		return num;
	}

	public bool IsBankItemBestBuy(PurchaseEventArgs purchaseInfo)
	{
		if (this._bestBuyIds.Count == 0 || purchaseInfo == null)
		{
			return false;
		}
		string str = (purchaseInfo.Currency != "GemsCurrency" ? "coins" : "gems");
		string str1 = string.Format("{0}_{1}", str, purchaseInfo.Index + 1);
		return this._bestBuyIds.Contains(str1);
	}

	private bool IsNeedCheckAmazonEventX3()
	{
		if (Defs.IsDeveloperBuild)
		{
			return true;
		}
		if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
		{
			return false;
		}
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			return false;
		}
		return true;
	}

	[DebuggerHidden]
	private IEnumerator OnApplicationPause(bool pause)
	{
		PromoActionsManager.u003cOnApplicationPauseu003ec__IteratorF1 variable = null;
		return variable;
	}

	private void ParseAdvertInfo(object advertInfoObj, PromoActionsManager.AdvertInfo advertInfo)
	{
		Dictionary<string, object> strs = advertInfoObj as Dictionary<string, object>;
		if (strs == null)
		{
			return;
		}
		advertInfo.imageUrl = (string)strs["imageUrl"];
		advertInfo.adUrl = (string)strs["adUrl"];
		advertInfo.message = (string)strs["text"];
		advertInfo.showAlways = (long)strs["showAlways"] == (long)1;
		advertInfo.btnClose = (long)strs["btnClose"] == (long)1;
		advertInfo.minLevel = (int)((long)strs["minLevel"]);
		advertInfo.maxLevel = (int)((long)strs["maxLevel"]);
		advertInfo.enabled = (long)strs["enabled"] == (long)1;
	}

	private void ParseAmazonEventData(Dictionary<string, object> jsonData)
	{
		if (jsonData.ContainsKey("startAmazonEventTime"))
		{
			this._eventX3AmazonEventStartTime = (long)jsonData["startAmazonEventTime"];
		}
		if (jsonData.ContainsKey("endAmazonEventTime"))
		{
			this._eventX3AmazonEventEndTime = (long)jsonData["endAmazonEventTime"];
		}
		if (jsonData.ContainsKey("timeZonesForEventAmazon"))
		{
			List<object> item = jsonData["timeZonesForEventAmazon"] as List<object>;
			for (int i = 0; i < item.Count; i++)
			{
				this._eventX3AmazonEventValidTimeZone.Add((long)item[i]);
			}
		}
	}

	private static void ParseReplaceAdmobPereliv(Dictionary<string, object> replaceAdmob, PromoActionsManager.ReplaceAdmobPerelivInfo replaceAdmobObj)
	{
		if (replaceAdmob == null)
		{
			UnityEngine.Debug.LogWarning("replaceAdmob == null");
		}
		else
		{
			try
			{
				replaceAdmobObj.imageUrls = (replaceAdmob["imageUrls"] as List<object>).OfType<string>().ToList<string>();
				replaceAdmobObj.adUrls = (replaceAdmob["adUrls"] as List<object>).OfType<string>().ToList<string>();
				replaceAdmobObj.enabled = (long)replaceAdmob["enabled"] == (long)1;
				replaceAdmobObj.ShowEveryTimes = Mathf.Max((int)((long)replaceAdmob["showEveryTimes"]), 1);
				replaceAdmobObj.ShowTimesTotal = Mathf.Max((int)((long)replaceAdmob["showTimesTotal"]), 0);
				replaceAdmobObj.ShowToPaying = (long)replaceAdmob["showToPaying"] == (long)1;
				replaceAdmobObj.ShowToNew = (long)replaceAdmob["showToNew"] == (long)1;
				try
				{
					replaceAdmobObj.MinLevel = (int)((long)replaceAdmob["minLevel"]);
				}
				catch
				{
					replaceAdmobObj.MinLevel = -1;
				}
				try
				{
					replaceAdmobObj.MaxLevel = (int)((long)replaceAdmob["maxLevel"]);
				}
				catch
				{
					replaceAdmobObj.MaxLevel = -1;
				}
			}
			catch
			{
				UnityEngine.Debug.LogWarning("replace_admob_pereliv_10_2_0 exception whiel parsing");
			}
		}
	}

	private void RefreshAmazonEvent()
	{
		if (PromoActionsManager.EventAmazonX3Updated != null)
		{
			PromoActionsManager.EventAmazonX3Updated();
		}
	}

	private void ResetNewbieX3StartTime()
	{
		if (this._newbieEventX3StartTime == 0)
		{
			return;
		}
		Storager.setString(Defs.NewbieEventX3StartTime, ((long)0).ToString(), false);
		this._newbieEventX3StartTime = (long)0;
	}

	private void Start()
	{
		PromoActionsManager.sharedManager = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Task task = PersistentCacheManager.Instance.StartDownloadSignaturesLoop();
		base.StartCoroutine(this.GetActionsLoop(task));
		base.StartCoroutine(this.GetEventX3InfoLoop());
		base.StartCoroutine(this.GetAdvertInfoLoop(task));
		base.StartCoroutine(this.GetBestBuyInfoLoop(task));
		base.StartCoroutine(this.GetDayOfValorInfoLoop());
	}

	private void Update()
	{
		if (Time.realtimeSinceStartup - this._eventX3LastCheckTime >= 1f)
		{
			this.CheckEventX3Active();
			if (Time.frameCount % 120 == 0)
			{
				this.RefreshAmazonEvent();
			}
			this.CheckDayOfValorActive();
			this._eventX3LastCheckTime = Time.realtimeSinceStartup;
		}
	}

	public static void UpdateDaysOfValorShownCondition()
	{
		string str = PlayerPrefs.GetString("LastTimeShowDaysOfValor", string.Empty);
		if (string.IsNullOrEmpty(str))
		{
			return;
		}
		DateTime dateTime = new DateTime();
		if (!DateTime.TryParse(str, out dateTime))
		{
			return;
		}
		if ((DateTime.UtcNow - dateTime) >= PromoActionsManager.TimeToShowDaysOfValor)
		{
			PlayerPrefs.SetInt("DaysOfValorShownCount", 1);
		}
	}

	public void UpdateNewbieEventX3Info()
	{
		this._newbieEventX3StartTime = PromoActionsManager.GetUnixTimeFromStorage(Defs.NewbieEventX3StartTime);
		this._newbieEventX3StartTimeAdditional = PromoActionsManager.GetUnixTimeFromStorage(Defs.NewbieEventX3StartTimeAdditional);
	}

	public static event Action ActionsUUpdated;

	public static event Action BestBuyStateUpdate;

	public static event Action EventAmazonX3Updated;

	public static event Action EventX3Updated;

	public static event PromoActionsManager.OnDayOfValorEnableDelegate OnDayOfValorEnable;

	public class AdvertInfo
	{
		public bool enabled;

		public string imageUrl;

		public string adUrl;

		public string message;

		public bool showAlways;

		public bool btnClose;

		public int minLevel;

		public int maxLevel;

		public AdvertInfo()
		{
		}
	}

	internal sealed class AmazonEventInfo
	{
		public string Caption
		{
			get;
			set;
		}

		public float DurationSeconds
		{
			get;
			set;
		}

		public DateTime EndTime
		{
			get
			{
				return this.StartTime + TimeSpan.FromSeconds((double)this.DurationSeconds);
			}
		}

		public float Percentage
		{
			get;
			set;
		}

		public DateTime StartTime
		{
			get;
			set;
		}

		public List<int> Timezones
		{
			get;
			set;
		}

		public AmazonEventInfo()
		{
			this.StartTime = DateTime.MaxValue;
			this.Timezones = new List<int>();
			this.Caption = string.Empty;
		}
	}

	public class MobileAdvertInfo
	{
		internal const int DefaultAwardCoins = 1;

		internal const string DefaultCurrency = "Coins";

		private int _awardCoinsPayning;

		private string _awardCoinsPayningCurrency;

		private int _awardCoinsNonpayning;

		private string _awardCoinsNonpayningCurrency;

		private List<string> _admobImageAdUnitIds;

		private List<string> _admobVideoAdUnitIds;

		private List<int> _adProviders;

		private double _daysOfBeingPayingUser;

		private List<int> _interstitialProviders;

		private List<List<string>> _admobImageIdGroups;

		private List<List<string>> _admobVideoIdGroups;

		private List<double> _rewardedVideoDelayMinutesNonpaying;

		private List<double> _rewardedVideoDelayMinutesPaying;

		public List<string> AdmobImageAdUnitIds
		{
			get
			{
				return this._admobImageAdUnitIds;
			}
			set
			{
				this._admobImageAdUnitIds = value ?? new List<string>();
			}
		}

		public List<List<string>> AdmobImageIdGroups
		{
			get
			{
				return this._admobImageIdGroups;
			}
			set
			{
				this._admobImageIdGroups = value ?? new List<List<string>>();
			}
		}

		public string AdmobVideoAdUnitId
		{
			get;
			set;
		}

		public List<string> AdmobVideoAdUnitIds
		{
			get
			{
				return this._admobVideoAdUnitIds;
			}
			set
			{
				this._admobVideoAdUnitIds = value ?? new List<string>();
			}
		}

		public List<List<string>> AdmobVideoIdGroups
		{
			get
			{
				return this._admobVideoIdGroups;
			}
			set
			{
				this._admobVideoIdGroups = value ?? new List<List<string>>();
			}
		}

		public int AdProvider
		{
			get;
			set;
		}

		public List<int> AdProviders
		{
			get
			{
				return this._adProviders;
			}
			set
			{
				this._adProviders = value ?? new List<int>();
			}
		}

		public int AwardCoinsNonpaying
		{
			get
			{
				return this._awardCoinsNonpayning;
			}
			set
			{
				this._awardCoinsNonpayning = Math.Max(1, value);
			}
		}

		public string AwardCoinsNonpayingCurrency
		{
			get
			{
				return this._awardCoinsNonpayningCurrency;
			}
			set
			{
				this._awardCoinsNonpayningCurrency = (value == "Coins" || value == "GemsCurrency" ? value : "Coins");
			}
		}

		public int AwardCoinsPaying
		{
			get
			{
				return this._awardCoinsPayning;
			}
			set
			{
				this._awardCoinsPayning = Math.Max(1, value);
			}
		}

		public string AwardCoinsPayingCurrency
		{
			get
			{
				return this._awardCoinsPayningCurrency;
			}
			set
			{
				this._awardCoinsPayningCurrency = (value == "Coins" || value == "GemsCurrency" ? value : "Coins");
			}
		}

		public double ConnectSceneDelaySeconds
		{
			get;
			set;
		}

		public int CountRoundReplaceProviders
		{
			get;
			set;
		}

		public int CountSessionNewPlayer
		{
			get;
			set;
		}

		public int CountVideoShowNonpaying
		{
			get
			{
				return this._rewardedVideoDelayMinutesNonpaying.Count;
			}
		}

		public int CountVideoShowPaying
		{
			get
			{
				return this._rewardedVideoDelayMinutesPaying.Count;
			}
		}

		public double DaysOfBeingPayingUser
		{
			get
			{
				return this._daysOfBeingPayingUser;
			}
			set
			{
				this._daysOfBeingPayingUser = Math.Max(0, value);
			}
		}

		[Obsolete]
		public bool ImageEnabled
		{
			get;
			set;
		}

		public List<int> InterstitialProviders
		{
			get
			{
				return this._interstitialProviders;
			}
			set
			{
				this._interstitialProviders = value ?? new List<int>();
			}
		}

		public double MinMatchDurationMinutes
		{
			get;
			set;
		}

		public List<double> RewardedVideoDelayMinutesNonpaying
		{
			get
			{
				return this._rewardedVideoDelayMinutesNonpaying;
			}
			set
			{
				this._rewardedVideoDelayMinutesNonpaying = value ?? new List<double>();
			}
		}

		public List<double> RewardedVideoDelayMinutesPaying
		{
			get
			{
				return this._rewardedVideoDelayMinutesPaying;
			}
			set
			{
				this._rewardedVideoDelayMinutesPaying = value ?? new List<double>();
			}
		}

		public bool ShowInterstitialAfterMatchLoser
		{
			get;
			set;
		}

		public bool ShowInterstitialAfterMatchWinner
		{
			get;
			set;
		}

		public int TimeoutBetweenShowInterstitial
		{
			get;
			set;
		}

		public int TimeoutSkipVideoNonpaying
		{
			get;
			set;
		}

		public int TimeoutSkipVideoPaying
		{
			get;
			set;
		}

		public float TimeoutWaitingInterstitialAfterMatchSeconds
		{
			get;
			set;
		}

		public int TimeoutWaitVideo
		{
			get;
			set;
		}

		public bool VideoEnabled
		{
			get;
			set;
		}

		public bool VideoShowNonpaying
		{
			get;
			set;
		}

		public bool VideoShowPaying
		{
			get;
			set;
		}

		public MobileAdvertInfo()
		{
		}
	}

	public delegate void OnDayOfValorEnableDelegate(bool enable);

	public class ReplaceAdmobPerelivInfo
	{
		public bool enabled;

		public List<string> imageUrls;

		public List<string> adUrls;

		public int MaxLevel
		{
			get;
			set;
		}

		public int MinLevel
		{
			get;
			set;
		}

		public int ShowEveryTimes
		{
			get;
			set;
		}

		public int ShowTimesTotal
		{
			get;
			set;
		}

		public bool ShowToNew
		{
			get;
			set;
		}

		public bool ShowToPaying
		{
			get;
			set;
		}

		public ReplaceAdmobPerelivInfo()
		{
		}
	}
}