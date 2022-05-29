using ExitGames.Client.Photon;
using Rilisoft;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public sealed class FriendsController : MonoBehaviour
{
	private const string FriendsKey = "FriendsKey";

	private const string ToUsKey = "ToUsKey";

	private const string PlayerInfoKey = "PlayerInfoKey";

	private const string FriendsInfoKey = "FriendsInfoKey";

	private const string ClanFriendsInfoKey = "ClanFriendsInfoKey";

	private const string ClanInvitesKey = "ClanInvitesKey";

	private const string PixelbookSettingsKey = "PixelbookSettingsKey";

	public const string LobbyNewsKey = "LobbyNewsKey";

	public const string LobbyIsAnyNewsKey = "LobbyIsAnyNewsKey";

	public const string PixelFilterWordsKey = "PixelFilterWordsKey";

	public const string PixelFilterSymbolsKey = "PixelFilterSymbolsKey";

	public const float TimeUpdateFriendAndClanData = 20f;

	public static bool isDebugLogWWW;

	public int Banned = -1;

	public static float onlineDelta;

	public static Dictionary<string, Dictionary<string, string>> mapPopularityDictionary;

	public static bool readyToOperate;

	public static FriendsController sharedController;

	private static bool _sandboxEnabled;

	private static bool _specialOffersEnabled;

	private bool friendsReceivedOnce;

	public string ClanID;

	public string clanLeaderID;

	public string clanLogo;

	public string clanName;

	public int NumberOfFriendsRequests;

	public int NumberOffFullInfoRequests;

	public int NumberOfBestPlayersRequests;

	public int NumberOfClanInfoRequests;

	public int NumberOfCreateClanRequests;

	private float lastTouchTm;

	public bool idle;

	private List<int> ids = new List<int>();

	public List<string> friendsDeletedLocal = new List<string>();

	public string JoinClanSent;

	private string AccountCreated = "AccountCreated";

	private string _id;

	public List<string> friends = new List<string>();

	public List<Dictionary<string, string>> clanMembers = new List<Dictionary<string, string>>();

	public List<string> invitesFromUs = new List<string>();

	public List<string> invitesToUs = new List<string>();

	public List<Dictionary<string, string>> ClanInvites = new List<Dictionary<string, string>>();

	public List<string> ClanSentInvites = new List<string>();

	public List<string> clanSentInvitesLocal = new List<string>();

	public List<string> clanCancelledInvitesLocal = new List<string>();

	public List<string> clanDeletedLocal = new List<string>();

	public Dictionary<string, Dictionary<string, object>> playersInfo = new Dictionary<string, Dictionary<string, object>>();

	public readonly Dictionary<string, Dictionary<string, object>> friendsInfo = new Dictionary<string, Dictionary<string, object>>();

	public Dictionary<string, Dictionary<string, object>> clanFriendsInfo = new Dictionary<string, Dictionary<string, object>>();

	public Dictionary<string, Dictionary<string, object>> profileInfo = new Dictionary<string, Dictionary<string, object>>();

	public Dictionary<string, Dictionary<string, string>> onlineInfo = new Dictionary<string, Dictionary<string, string>>();

	public List<string> notShowAddIds = new List<string>();

	public Dictionary<string, Dictionary<string, object>> facebookFriendsInfo = new Dictionary<string, Dictionary<string, object>>();

	public string alphaIvory;

	private static HMAC _hmac;

	public string nick;

	public string skin;

	public int rank;

	public int coopScore;

	public int survivalScore;

	internal SaltedInt wins = new SaltedInt(641227346);

	public Dictionary<string, object> ourInfo;

	public string id_fb;

	private float timerUpdatePixelbookSetting = 900f;

	private static long localServerTime;

	private static float tickForServerTime;

	private static bool isUpdateServerTimeAfterRun;

	public static bool isInitPixelbookSettingsFromServer;

	private string FacebookIDKey = "FacebookIDKey";

	private static bool _isCurrentOnRatingSystemInit;

	private static bool _isCurrentOnRatingSystem;

	private static bool _isConfigNameRatingSystemInit;

	private static string _configNameABTestRatingSystem;

	private static string configNameRatingSystemSending;

	private static bool _isUseRatingSystem;

	private static bool _isInitUseRatingSystem;

	private static string useRatingSystemKey;

	private static bool _isCohortUseRatingSystem;

	private static bool _isInitCohortUseRatingSystem;

	private static string useRatingSystemCohortKey;

	private static bool _isCurrentBankViewStaticInit;

	private static bool _isCurrentBankViewStatic;

	private static bool _isConfigNameBankViewInit;

	private static string _configNameABTestBankView;

	private static string configNameSending;

	private static bool _isShowStaticBank;

	private static bool _isInitShowStaticBank;

	private static string staticBankKey;

	private static bool _isCohortStaticBank;

	private static bool _isInitCohortStaticBank;

	private static string staticBankCohortKey;

	private static bool _isUseBuffSystem;

	private static bool _isInitUseBuffSystem;

	private static string useBuffSystemKey;

	private static bool _isCurrentStateBuffSystemInit;

	private static bool _isCurrentStateBuffSystem;

	private static bool _isConfigNameBuffSystemInit;

	private static string _configNameABTestBuffSystem;

	private static string configNameBuffSystemSending;

	private static bool _isCohortBuffSystem;

	private static bool _isInitCohortBuffSystem;

	private static string buffSystemCohortKey;

	public static Dictionary<string, float[]> dpsWeaponsFromABTestBalans;

	public static Dictionary<string, float[]> damageWeaponsFromABTestBalans;

	public FriendsController.OnChangeClanName onChangeClanName;

	private string _prevClanName;

	public bool dataSent;

	private bool infoLoaded;

	public string tempClanID;

	public string tempClanLogo;

	public string tempClanName;

	public string tempClanCreatorID;

	private bool _shouldStopOnline;

	private bool _shouldStopOnlineWithClanInfo;

	private bool _shouldStopRefrClanOnline;

	public Action GetFacebookFriendsCallback;

	private string _inputToken;

	private KeyValuePair<string, int>? _winCountTimestamp;

	private bool ReceivedLastOnline;

	private bool getCohortInfo;

	public float timerUpdateFriend = 20f;

	public static Action OnShowBoxProcessFriendsData;

	public static Action OnHideBoxProcessFriendsData;

	private bool _shouldStopRefreshingInfo;

	private float deltaTimeInGame;

	private float sendingTime;

	public Dictionary<string, FriendsController.PossiblleOrigin> getPossibleFriendsResult = new Dictionary<string, FriendsController.PossiblleOrigin>();

	private bool isUpdateInfoAboutAllFriends;

	public static Action UpdateFriendsInfoAction;

	public Dictionary<string, string> clicksJoinByFriends = new Dictionary<string, string>();

	private static FriendProfileController _friendProfileController;

	private static DateTime timeSendTrafficForwarding;

	private static bool _isConfigNameSandBoxInit;

	private static string _configNameABTestSandBox;

	private static bool _isConfigNameQuestSystemInit;

	private static string _configNameABTestQuestSystem;

	private static bool _isConfigNameSpecialOffersInit;

	private static string _configNameABTestSpecialOffers;

	private Action FailedSendNewClan;

	private Action<int> ReturnNewIDClan;

	public static string actionAddress
	{
		get
		{
			return URLs.Friends;
		}
	}

	public static bool ClanDataSettted
	{
		get;
		private set;
	}

	public int ClanLimit
	{
		get
		{
			return Defs.maxMemberClanCount;
		}
	}

	public bool ClanLimitReached
	{
		get
		{
			FriendsController friendsController = FriendsController.sharedController;
			return friendsController.clanMembers.Count + friendsController.ClanSentInvites.Count + friendsController.clanSentInvitesLocal.Count >= friendsController.ClanLimit;
		}
	}

	public static string configNameABTestBankView
	{
		get
		{
			if (!FriendsController._isConfigNameBankViewInit)
			{
				FriendsController.ParseABTestBankViewConfig();
			}
			return FriendsController._configNameABTestBankView;
		}
		set
		{
			FriendsController._isConfigNameBankViewInit = true;
			FriendsController._configNameABTestBankView = value;
			if (string.IsNullOrEmpty(FriendsController.configNameSending))
			{
				FriendsController.configNameSending = PlayerPrefs.GetString("cNS", "none");
			}
			string str = string.Concat((!FriendsController.isCohortStaticBank ? "Scroll_" : "Static_"), FriendsController._configNameABTestBankView);
			if (!str.Equals(FriendsController.configNameSending))
			{
				AnalyticsStuff.LogABTest("Assortment", str, true);
				PlayerPrefs.SetString("cNS", str);
				FriendsController.configNameSending = str;
			}
		}
	}

	public static string configNameABTestBuffSystem
	{
		get
		{
			if (!FriendsController._isConfigNameBuffSystemInit)
			{
				FriendsController.ParseABTestBuffSystemConfig();
			}
			return FriendsController._configNameABTestBuffSystem;
		}
		set
		{
			FriendsController._isConfigNameBuffSystemInit = true;
			FriendsController._configNameABTestBuffSystem = value;
			if (string.IsNullOrEmpty(FriendsController.configNameBuffSystemSending))
			{
				FriendsController.configNameBuffSystemSending = PlayerPrefs.GetString("cNBSS", "none");
			}
			string str = string.Concat((!FriendsController.isCohortBuffSystem ? "NotBuff_" : "BuffSys_"), FriendsController.configNameBuffSystemSending);
			if (!str.Equals(FriendsController.configNameBuffSystemSending))
			{
				AnalyticsStuff.LogABTest("BuffSystem", str, true);
				PlayerPrefs.SetString("cNBSS", str);
				FriendsController.configNameBuffSystemSending = str;
			}
		}
	}

	public static string configNameABTestQuestSystem
	{
		get
		{
			if (!FriendsController._isConfigNameQuestSystemInit)
			{
				FriendsController._configNameABTestQuestSystem = PlayerPrefs.GetString("CNQuestSystem", "none");
				FriendsController._isConfigNameQuestSystemInit = true;
			}
			return FriendsController._configNameABTestQuestSystem;
		}
		set
		{
			FriendsController._isConfigNameQuestSystemInit = true;
			FriendsController._configNameABTestQuestSystem = value;
			PlayerPrefs.SetString("CNQuestSystem", FriendsController._configNameABTestQuestSystem);
		}
	}

	public static string configNameABTestRatingSystem
	{
		get
		{
			if (!FriendsController._isConfigNameRatingSystemInit)
			{
				FriendsController.ParseRatingSystemConfig();
			}
			return FriendsController._configNameABTestRatingSystem;
		}
		set
		{
			FriendsController._isConfigNameRatingSystemInit = true;
			FriendsController._configNameABTestRatingSystem = value;
			if (string.IsNullOrEmpty(FriendsController.configNameRatingSystemSending))
			{
				FriendsController.configNameRatingSystemSending = PlayerPrefs.GetString("cNRatSysS", "none");
			}
			string str = string.Concat((!FriendsController.isCohortUseRatingSystem ? "OffRating_" : "UseRaing_"), FriendsController._configNameABTestRatingSystem);
			if (!str.Equals(FriendsController.configNameRatingSystemSending))
			{
				AnalyticsStuff.LogABTest("RatingSystem", str, true);
				PlayerPrefs.SetString("cNRatSysS", str);
				FriendsController.configNameRatingSystemSending = str;
			}
		}
	}

	public static string configNameABTestSandBox
	{
		get
		{
			if (!FriendsController._isConfigNameSandBoxInit)
			{
				FriendsController._configNameABTestSandBox = PlayerPrefs.GetString("CNSandBox", "none");
				FriendsController._isConfigNameSandBoxInit = true;
			}
			return FriendsController._configNameABTestSandBox;
		}
		set
		{
			FriendsController._isConfigNameSandBoxInit = true;
			FriendsController._configNameABTestSandBox = value;
			PlayerPrefs.SetString("CNSandBox", FriendsController._configNameABTestSandBox);
		}
	}

	public static string configNameABTestSpecialOffers
	{
		get
		{
			if (!FriendsController._isConfigNameSpecialOffersInit)
			{
				FriendsController._configNameABTestSpecialOffers = PlayerPrefs.GetString("CNSpecialOffers", "none");
				FriendsController._isConfigNameSpecialOffersInit = true;
			}
			return FriendsController._configNameABTestSpecialOffers;
		}
		set
		{
			FriendsController._isConfigNameSpecialOffersInit = true;
			FriendsController._configNameABTestSpecialOffers = value;
			PlayerPrefs.SetString("CNSpecialOffers", FriendsController._configNameABTestSpecialOffers);
		}
	}

	public static int CurrentPlatform
	{
		get
		{
			return ProtocolListGetter.CurrentPlatform;
		}
	}

	public List<string> findPlayersByParamResult
	{
		get;
		private set;
	}

	public Dictionary<string, object> getInfoPlayerResult
	{
		get;
		private set;
	}

	public static bool HasFriends
	{
		get
		{
			string str = PlayerPrefs.GetString("FriendsKey", "[]");
			return (string.IsNullOrEmpty(str) ? false : str != "[]");
		}
	}

	public string id
	{
		get
		{
			return this._id;
		}
		set
		{
			this._id = value;
		}
	}

	public static bool isCohortBuffSystem
	{
		get
		{
			if (!FriendsController._isInitCohortBuffSystem)
			{
				if (!PlayerPrefs.HasKey(FriendsController.buffSystemCohortKey))
				{
					FriendsController._isCohortBuffSystem = UnityEngine.Random.Range(0, 2) == 1;
					FriendsController._isCohortBuffSystem = true;
					PlayerPrefs.SetInt(FriendsController.buffSystemCohortKey, (!FriendsController._isCohortBuffSystem ? 0 : 1));
					PlayerPrefs.Save();
				}
				FriendsController._isCohortBuffSystem = PlayerPrefs.GetInt(FriendsController.buffSystemCohortKey) == 1;
				FriendsController._isInitCohortBuffSystem = true;
			}
			return FriendsController._isCohortBuffSystem;
		}
		set
		{
			FriendsController._isCohortBuffSystem = value;
			PlayerPrefs.SetInt(FriendsController.buffSystemCohortKey, (!FriendsController._isCohortBuffSystem ? 0 : 1));
			PlayerPrefs.Save();
		}
	}

	public static bool isCohortStaticBank
	{
		get
		{
			if (!FriendsController._isInitCohortStaticBank)
			{
				if (!PlayerPrefs.HasKey(FriendsController.staticBankCohortKey))
				{
					FriendsController._isCohortStaticBank = UnityEngine.Random.Range(0, 2) == 1;
					PlayerPrefs.SetInt(FriendsController.staticBankCohortKey, (!FriendsController._isCohortStaticBank ? 0 : 1));
					PlayerPrefs.Save();
				}
				FriendsController._isCohortStaticBank = PlayerPrefs.GetInt(FriendsController.staticBankCohortKey) == 1;
				FriendsController._isInitCohortStaticBank = true;
			}
			return FriendsController._isCohortStaticBank;
		}
	}

	public static bool isCohortUseRatingSystem
	{
		get
		{
			if (!FriendsController._isInitCohortUseRatingSystem)
			{
				if (!PlayerPrefs.HasKey(FriendsController.useRatingSystemCohortKey))
				{
					FriendsController._isCohortUseRatingSystem = UnityEngine.Random.Range(0, 2) == 1;
					PlayerPrefs.SetInt(FriendsController.useRatingSystemCohortKey, (!FriendsController._isCohortUseRatingSystem ? 0 : 1));
					PlayerPrefs.Save();
				}
				FriendsController._isCohortUseRatingSystem = PlayerPrefs.GetInt(FriendsController.useRatingSystemCohortKey) == 1;
				FriendsController._isInitCohortUseRatingSystem = true;
			}
			return FriendsController._isCohortUseRatingSystem;
		}
		set
		{
			FriendsController._isCohortUseRatingSystem = value;
			PlayerPrefs.SetInt(FriendsController.useRatingSystemCohortKey, (!FriendsController._isCohortUseRatingSystem ? 0 : 1));
			PlayerPrefs.Save();
		}
	}

	public static bool isCurrentBankViewStatic
	{
		get
		{
			if (!FriendsController._isCurrentBankViewStaticInit)
			{
				FriendsController.ParseABTestBankViewConfig();
			}
			return FriendsController._isCurrentBankViewStatic;
		}
		set
		{
			FriendsController._isCurrentBankViewStaticInit = true;
			FriendsController._isCurrentBankViewStatic = value;
		}
	}

	public static bool isCurrentStateBuffSystem
	{
		get
		{
			if (!FriendsController._isCurrentStateBuffSystemInit)
			{
				FriendsController.ParseABTestBuffSystemConfig();
			}
			return FriendsController._isCurrentStateBuffSystem;
		}
		set
		{
			FriendsController._isCurrentStateBuffSystemInit = true;
			FriendsController._isCurrentStateBuffSystem = value;
		}
	}

	public static bool isCurrentUseRatingSystem
	{
		get
		{
			if (!FriendsController._isCurrentOnRatingSystemInit)
			{
				FriendsController.ParseRatingSystemConfig();
			}
			return FriendsController._isCurrentOnRatingSystem;
		}
		set
		{
			FriendsController._isCurrentOnRatingSystemInit = true;
			FriendsController._isCurrentOnRatingSystem = value;
		}
	}

	private bool isRunABTest
	{
		get
		{
			return (Defs.abTestBalansCohort != Defs.ABTestCohortsType.NONE || Defs.isActivABTestBuffSystem || Defs.isActivABTestRatingSystem || Defs.isActivABTestStaticBank || Defs.cohortABTestSandBox == Defs.ABTestCohortsType.A || Defs.cohortABTestSandBox == Defs.ABTestCohortsType.B || Defs.cohortABTestQuestSystem == Defs.ABTestCohortsType.A || Defs.cohortABTestQuestSystem == Defs.ABTestCohortsType.B || Defs.cohortABTestSpecialOffers == Defs.ABTestCohortsType.A ? true : Defs.cohortABTestSpecialOffers == Defs.ABTestCohortsType.B);
		}
	}

	public static bool isShowStaticBank
	{
		get
		{
			if (!FriendsController._isInitShowStaticBank)
			{
				if (!PlayerPrefs.HasKey(FriendsController.staticBankKey))
				{
					FriendsController._isShowStaticBank = (!Defs.isActivABTestStaticBank ? FriendsController.isCurrentBankViewStatic : FriendsController.isCohortStaticBank);
					PlayerPrefs.SetInt(FriendsController.staticBankKey, (!FriendsController._isShowStaticBank ? 0 : 1));
					PlayerPrefs.Save();
				}
				FriendsController._isShowStaticBank = PlayerPrefs.GetInt(FriendsController.staticBankKey) == 1;
				FriendsController._isInitShowStaticBank = true;
			}
			return FriendsController._isShowStaticBank;
		}
		set
		{
			FriendsController._isShowStaticBank = value;
			PlayerPrefs.SetInt(FriendsController.staticBankKey, (!value ? 0 : 1));
			FriendsController._isInitShowStaticBank = true;
		}
	}

	public static bool isUseRatingSystem
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	public bool ProfileInterfaceActive
	{
		get
		{
			if (FriendsController._friendProfileController == null)
			{
				return false;
			}
			return FriendsController._friendProfileController.FriendProfileGo.Map<GameObject, bool>((GameObject g) => g.activeInHierarchy);
		}
	}

	internal static bool QuestSystemEnabled
	{
		get
		{
			return QuestSystem.Instance.Enabled;
		}
		set
		{
			QuestSystem.Instance.Enabled = value;
		}
	}

	internal static bool SandboxEnabled
	{
		get
		{
			return FriendsController._sandboxEnabled;
		}
		set
		{
			FriendsController._sandboxEnabled = value;
		}
	}

	public static long ServerTime
	{
		get
		{
			if (FriendsController.isUpdateServerTimeAfterRun)
			{
				return FriendsController.localServerTime;
			}
			return (long)-1;
		}
	}

	internal static bool SpecialOffersEnabled
	{
		get
		{
			return FriendsController._specialOffersEnabled;
		}
		set
		{
			FriendsController._specialOffersEnabled = value;
		}
	}

	public static bool useBuffSystem
	{
		get
		{
			bool flag;
			if (!FriendsController._isInitUseBuffSystem)
			{
				if (!PlayerPrefs.HasKey(FriendsController.useBuffSystemKey))
				{
					if (!Defs.isActivABTestRatingSystem)
					{
						flag = FriendsController.isCurrentStateBuffSystem;
					}
					else
					{
						flag = (!FriendsController.isCohortUseRatingSystem ? false : FriendsController.isCurrentStateBuffSystem);
					}
					FriendsController._isUseBuffSystem = flag;
					PlayerPrefs.SetInt(FriendsController.useBuffSystemKey, (!FriendsController._isUseBuffSystem ? 0 : 1));
					PlayerPrefs.Save();
				}
				FriendsController._isUseBuffSystem = PlayerPrefs.GetInt(FriendsController.useBuffSystemKey) == 1;
				FriendsController._isInitUseBuffSystem = true;
			}
			return FriendsController._isUseBuffSystem;
		}
		set
		{
			FriendsController._isUseBuffSystem = value;
			PlayerPrefs.SetInt(FriendsController.useBuffSystemKey, (!value ? 0 : 1));
			FriendsController._isInitUseBuffSystem = true;
		}
	}

	public KeyValuePair<string, int>? WinCountTimestamp
	{
		get
		{
			return this._winCountTimestamp;
		}
	}

	static FriendsController()
	{
		FriendsController.isDebugLogWWW = true;
		FriendsController.onlineDelta = 60f;
		FriendsController.mapPopularityDictionary = new Dictionary<string, Dictionary<string, string>>();
		FriendsController.readyToOperate = false;
		FriendsController.sharedController = null;
		FriendsController._sandboxEnabled = true;
		FriendsController._specialOffersEnabled = true;
		FriendsController.tickForServerTime = 0f;
		FriendsController.isInitPixelbookSettingsFromServer = false;
		FriendsController._configNameABTestRatingSystem = "none";
		FriendsController.configNameRatingSystemSending = string.Empty;
		FriendsController.useRatingSystemKey = "UseRatingSystemKey";
		FriendsController.useRatingSystemCohortKey = "useRatingSystemCohortKey";
		FriendsController._configNameABTestBankView = "none";
		FriendsController.configNameSending = string.Empty;
		FriendsController.staticBankKey = "staticBankKey";
		FriendsController.staticBankCohortKey = "staticBankCohortKey";
		FriendsController.useBuffSystemKey = "useBuffSystemKey";
		FriendsController._configNameABTestBuffSystem = "none";
		FriendsController.configNameBuffSystemSending = string.Empty;
		FriendsController.buffSystemCohortKey = "buffSystemCohortKey";
		FriendsController.dpsWeaponsFromABTestBalans = new Dictionary<string, float[]>();
		FriendsController.damageWeaponsFromABTestBalans = new Dictionary<string, float[]>();
		FriendsController.timeSendTrafficForwarding = new DateTime(2000, 1, 1, 12, 0, 0);
		FriendsController._configNameABTestSandBox = "none";
		FriendsController._configNameABTestQuestSystem = "none";
		FriendsController._configNameABTestSpecialOffers = "none";
		FriendsController.FriendsUpdated = null;
		FriendsController.FullInfoUpdated = null;
		FriendsController.ServerTimeUpdated = null;
	}

	public FriendsController()
	{
	}

	[DebuggerHidden]
	private IEnumerator _AcceptClanInvite(string recordId)
	{
		FriendsController.u003c_AcceptClanInviteu003ec__Iterator43 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator _ChangeClanLogo()
	{
		FriendsController.u003c_ChangeClanLogou003ec__Iterator35 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator _ChangeClanName(string newNm, Action onSuccess, Action<string> onFailure)
	{
		FriendsController.u003c_ChangeClanNameu003ec__Iterator36 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator _DeleteClan()
	{
		FriendsController.u003c_DeleteClanu003ec__Iterator63 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator _DeleteClanMember(string memberID)
	{
		FriendsController.u003c_DeleteClanMemberu003ec__Iterator69 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator _ExitClan(string who)
	{
		FriendsController.u003c_ExitClanu003ec__Iterator62 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator _GetFacebookFriendsInfo(Action callb)
	{
		FriendsController.u003c_GetFacebookFriendsInfou003ec__Iterator4C variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator _GetOnlineForPlayerIDs(List<string> ids)
	{
		FriendsController.u003c_GetOnlineForPlayerIDsu003ec__Iterator4A variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator _GetOnlineWithClanInfoForPlayerIDs(List<string> ids)
	{
		FriendsController.u003c_GetOnlineWithClanInfoForPlayerIDsu003ec__Iterator4B variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator _GetOurWins()
	{
		FriendsController.u003c_GetOurWinsu003ec__Iterator33 variable = null;
		return variable;
	}

	private void _ProcessClanInvitesList(List<object> clanInv)
	{
		List<Dictionary<string, string>> dictionaries = new List<Dictionary<string, string>>();
		foreach (Dictionary<string, object> strs in clanInv)
		{
			Dictionary<string, string> strs1 = new Dictionary<string, string>();
			foreach (KeyValuePair<string, object> keyValuePair in strs)
			{
				strs1.Add(keyValuePair.Key, keyValuePair.Value as string);
			}
			dictionaries.Add(strs1);
		}
		this.ClanInvites.Clear();
		this.ClanInvites = dictionaries;
	}

	private void _ProcessFriendsList(List<object> __list, bool requestAllInfo)
	{
		List<Dictionary<string, string>> dictionaries = new List<Dictionary<string, string>>();
		foreach (Dictionary<string, object> _List in __list)
		{
			Dictionary<string, string> strs = new Dictionary<string, string>();
			foreach (KeyValuePair<string, object> keyValuePair in _List)
			{
				strs.Add(keyValuePair.Key, keyValuePair.Value as string);
			}
			dictionaries.Add(strs);
		}
		foreach (Dictionary<string, string> dictionary in dictionaries)
		{
			Dictionary<string, string> strs1 = new Dictionary<string, string>();
			if (dictionary["whom"].Equals(this.id) && dictionary["status"].Equals("0"))
			{
				foreach (string key in dictionary.Keys)
				{
					if (!key.Equals("whom") && !key.Equals("status"))
					{
						try
						{
							strs1.Add((!key.Equals("who") ? key : "friend"), dictionary[key]);
						}
						catch
						{
						}
					}
				}
				this.invitesToUs.Add(strs1["friend"]);
				this.notShowAddIds.Remove(dictionary["who"]);
			}
			if (!dictionary["status"].Equals("1"))
			{
				continue;
			}
			string str = (!dictionary["who"].Equals(this.id) ? "whom" : "who");
			string str1 = (!str.Equals("who") ? "who" : "whom");
			foreach (string key1 in dictionary.Keys)
			{
				if (!key1.Equals(str) && !key1.Equals("status"))
				{
					strs1.Add((!key1.Equals(str1) ? key1 : "friend"), dictionary[key1]);
				}
			}
			this.friends.Add(strs1["friend"]);
			this.notShowAddIds.Remove(dictionary[str1]);
		}
		if (!requestAllInfo)
		{
			this._UpdatePlayersInfo();
		}
		else
		{
			this.UpdatePLayersInfo();
		}
	}

	[DebuggerHidden]
	private IEnumerator _RejectClanInvite(string clanID, string playerID)
	{
		FriendsController.u003c_RejectClanInviteu003ec__Iterator68 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator _SendCreateClan(string personId, string nameClan, string skinClan, Action<string> ErrorHandler)
	{
		FriendsController.u003c_SendCreateClanu003ec__Iterator61 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator _SendRoundWon(int mode)
	{
		FriendsController.u003c_SendRoundWonu003ec__Iterator34 variable = null;
		return variable;
	}

	private void _UpdateClanMembers(string text)
	{
		object obj1;
		object obj2;
		object obj3;
		int num;
		int num1;
		Dictionary<string, object> strs = Json.Deserialize(text) as Dictionary<string, object>;
		if (strs == null)
		{
			if (Application.isEditor || UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning(" _UpdateClanMembers dict = null");
			}
			return;
		}
		foreach (KeyValuePair<string, object> keyValuePair in strs)
		{
			string key = keyValuePair.Key;
			if (key != null)
			{
				if (FriendsController.u003cu003ef__switchu0024map2 == null)
				{
					Dictionary<string, int> strs1 = new Dictionary<string, int>(3)
					{
						{ "info", 0 },
						{ "players", 1 },
						{ "invites", 2 }
					};
					FriendsController.u003cu003ef__switchu0024map2 = strs1;
				}
				if (FriendsController.u003cu003ef__switchu0024map2.TryGetValue(key, out num1))
				{
					switch (num1)
					{
						case 0:
						{
							Dictionary<string, object> value = keyValuePair.Value as Dictionary<string, object>;
							if (value != null)
							{
								if (value.TryGetValue("name", out obj1))
								{
									this._prevClanName = this.clanName;
									this.clanName = obj1 as string;
									if (!this._prevClanName.Equals(this.clanName) && this.onChangeClanName != null)
									{
										this.onChangeClanName(this.clanName);
									}
								}
								if (value.TryGetValue("logo", out obj2))
								{
									this.clanLogo = obj2 as string;
								}
								if (value.TryGetValue("creator_id", out obj3))
								{
									this.clanLeaderID = obj3 as string;
								}
							}
							continue;
						}
						case 1:
						{
							List<object> objs = keyValuePair.Value as List<object>;
							if (objs != null)
							{
								this.clanMembers.Clear();
								foreach (Dictionary<string, object> strs2 in objs)
								{
									Dictionary<string, string> strs3 = new Dictionary<string, string>();
									foreach (KeyValuePair<string, object> keyValuePair1 in strs2)
									{
										if (!(keyValuePair1.Value is string))
										{
											continue;
										}
										strs3.Add(keyValuePair1.Key, keyValuePair1.Value as string);
									}
									this.clanMembers.Add(strs3);
								}
							}
							List<string> strs4 = new List<string>();
							foreach (string str in this.clanDeletedLocal)
							{
								bool flag = false;
								foreach (Dictionary<string, string> clanMember in this.clanMembers)
								{
									if (!clanMember.ContainsKey("id") || !clanMember["id"].Equals(str))
									{
										continue;
									}
									flag = true;
									break;
								}
								if (flag)
								{
									continue;
								}
								strs4.Add(str);
							}
							this.clanDeletedLocal.RemoveAll((string obj) => strs4.Contains(obj));
							continue;
						}
						case 2:
						{
							this.ClanSentInvites.Clear();
							List<object> value1 = keyValuePair.Value as List<object>;
							if (value1 != null)
							{
								foreach (string str1 in value1)
								{
									if (!int.TryParse(str1, out num) || this.ClanSentInvites.Contains(num.ToString()))
									{
										continue;
									}
									this.ClanSentInvites.Add(num.ToString());
									this.clanSentInvitesLocal.Remove(num.ToString());
								}
							}
							continue;
						}
					}
				}
			}
		}
		List<string> strs5 = new List<string>();
		foreach (string str2 in this.clanCancelledInvitesLocal)
		{
			if (this.ClanSentInvites.Contains(str2))
			{
				continue;
			}
			strs5.Add(str2);
		}
		this.clanCancelledInvitesLocal.RemoveAll((string obj) => strs5.Contains(obj));
		if (FriendsController.ClanUpdated != null)
		{
			FriendsController.ClanUpdated();
		}
		FriendsController.ClanDataSettted = true;
	}

	private void _UpdateFriends(string text, bool requestAllInfo)
	{
		object obj;
		object obj1;
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		this.invitesFromUs.Clear();
		this.invitesToUs.Clear();
		this.friends.Clear();
		this.ClanInvites.Clear();
		this.friendsDeletedLocal.Clear();
		Dictionary<string, object> strs = Json.Deserialize(text) as Dictionary<string, object>;
		if (strs == null)
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning(" _UpdateFriends dict = null");
			}
			return;
		}
		if (!strs.TryGetValue("friends", out obj))
		{
			UnityEngine.Debug.LogWarning(" _UpdateFriends friendsObj!");
			return;
		}
		List<object> objs = obj as List<object>;
		if (objs == null)
		{
			if (Application.isEditor || UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.LogWarning(" _UpdateFriends __list = null");
			}
			return;
		}
		this._ProcessFriendsList(objs, requestAllInfo);
		if (!strs.TryGetValue("clans_invites", out obj1))
		{
			UnityEngine.Debug.LogWarning(" _UpdateFriends clanInvObj!");
			return;
		}
		List<object> objs1 = obj1 as List<object>;
		if (objs1 != null)
		{
			this._ProcessClanInvitesList(objs1);
			return;
		}
		if (Application.isEditor || UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.LogWarning(" _UpdateFriends clanInv = null");
		}
	}

	private void _UpdatePlayersInfo()
	{
		List<string> strs = new List<string>();
		strs.AddRange(this.friends);
		strs.AddRange(this.invitesToUs);
		if (strs.Count > 0)
		{
			base.StartCoroutine(this.GetInfoAboutNPlayers(strs));
		}
	}

	public void AcceptClanInvite(string recordId)
	{
		if (!string.IsNullOrEmpty(recordId) && FriendsController.readyToOperate)
		{
			base.StartCoroutine(this._AcceptClanInvite(recordId));
		}
	}

	[DebuggerHidden]
	private IEnumerator AcceptFriend(string accepteeId, Action<bool> action = null)
	{
		FriendsController.u003cAcceptFriendu003ec__Iterator65 variable = null;
		return variable;
	}

	public void AcceptInvite(string accepteeId, Action<bool> action = null)
	{
		if (!string.IsNullOrEmpty(accepteeId) && FriendsController.readyToOperate)
		{
			base.StartCoroutine(this.AcceptFriend(accepteeId, action));
		}
	}

	[DebuggerHidden]
	private IEnumerator AddPurchaseEvent(int inapp, string purchaseId)
	{
		FriendsController.u003cAddPurchaseEventu003ec__Iterator4E variable = null;
		return variable;
	}

	private void Awake()
	{
		if (!Storager.hasKey(this.FacebookIDKey))
		{
			Storager.setString(this.FacebookIDKey, string.Empty, false);
		}
		this.id_fb = Storager.getString(this.FacebookIDKey, false);
		FriendsController.sharedController = this;
	}

	public void ChangeClanLogo()
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		base.StartCoroutine(this._ChangeClanLogo());
	}

	public void ChangeClanName(string newNm, Action onSuccess, Action<string> onFailure)
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		base.StartCoroutine(this._ChangeClanName(newNm, onSuccess, onFailure));
	}

	[DebuggerHidden]
	private IEnumerator CheckOurIDExists()
	{
		FriendsController.u003cCheckOurIDExistsu003ec__Iterator40 variable = null;
		return variable;
	}

	public void ClearAllFriendsInvites()
	{
		base.StartCoroutine(this.ClearAllFriendsInvitesCoroutine());
	}

	[DebuggerHidden]
	private IEnumerator ClearAllFriendsInvitesCoroutine()
	{
		FriendsController.u003cClearAllFriendsInvitesCoroutineu003ec__Iterator6B variable = null;
		return variable;
	}

	public void ClearClanData()
	{
		this.ClanID = null;
		this.clanName = string.Empty;
		this.clanLogo = string.Empty;
		this.clanLeaderID = string.Empty;
		this.clanMembers.Clear();
		this.ClanSentInvites.Clear();
		this.clanSentInvitesLocal.Clear();
	}

	private void ClearListClickJoinFriends()
	{
		this.clicksJoinByFriends.Clear();
		PlayerPrefs.SetString("CachedFriendsJoinClickList", string.Empty);
	}

	public static void CopyMyIdToClipboard()
	{
		FriendsController.CopyPlayerIdToClipboard(FriendsController.sharedController.id);
	}

	public static void CopyPlayerIdToClipboard(string playerId)
	{
		UniPasteBoard.SetClipBoardString(playerId);
		InfoWindowController.ShowInfoBox(LocalizationStore.Get("Key_1618"));
	}

	[DebuggerHidden]
	private IEnumerator CreatePlayer()
	{
		FriendsController.u003cCreatePlayeru003ec__Iterator52 variable = null;
		return variable;
	}

	public void DeleteClan()
	{
		if (FriendsController.readyToOperate && this.ClanID != null)
		{
			base.StartCoroutine(this._DeleteClan());
		}
	}

	public void DeleteClanMember(string memebrID)
	{
		if (!string.IsNullOrEmpty(memebrID) && FriendsController.readyToOperate)
		{
			base.StartCoroutine(this._DeleteClanMember(memebrID));
		}
	}

	public static void DeleteFriend(string rejecteeId, Action<bool> action = null)
	{
		if (FriendsController.sharedController == null)
		{
			return;
		}
		if (FriendsController.readyToOperate)
		{
			FriendsController.sharedController.StartCoroutine(FriendsController.sharedController.DeleteFriendCoroutine(rejecteeId, action));
		}
	}

	[DebuggerHidden]
	private IEnumerator DeleteFriendCoroutine(string rejecteeId, Action<bool> action = null)
	{
		FriendsController.u003cDeleteFriendCoroutineu003ec__Iterator66 variable = null;
		return variable;
	}

	public static void DisposeProfile()
	{
		if (FriendsController._friendProfileController == null)
		{
			return;
		}
		FriendsController._friendProfileController.Dispose();
		FriendsController._friendProfileController = null;
	}

	public void DownloadDataAboutPossibleFriends()
	{
		int currentLevel = ExperienceController.GetCurrentLevel();
		int num = (int)ConnectSceneNGUIController.myPlatformConnect;
		string multiplayerProtocolVersion = GlobalGameController.MultiplayerProtocolVersion;
		base.StartCoroutine(this.GetPossibleFriendsList(currentLevel, num, multiplayerProtocolVersion));
	}

	public void DownloadInfoByEverydayDelta()
	{
		base.StartCoroutine(this.GetInfoByEverydayDelta());
	}

	private void DumpCurrentState()
	{
	}

	public void ExitClan(string who = null)
	{
		if (FriendsController.readyToOperate)
		{
			base.StartCoroutine(this._ExitClan(who));
		}
	}

	public void FastGetPixelbookSettings()
	{
		this.timerUpdatePixelbookSetting = -1f;
	}

	private void FillClickJoinFriendsListByCachedValue()
	{
		string str = PlayerPrefs.GetString("CachedFriendsJoinClickList", string.Empty);
		if (string.IsNullOrEmpty(str))
		{
			return;
		}
		Dictionary<string, object> strs = Json.Deserialize(str) as Dictionary<string, object>;
		if (strs == null)
		{
			return;
		}
		foreach (KeyValuePair<string, object> keyValuePair in strs)
		{
			this.clicksJoinByFriends.Add(keyValuePair.Key, Convert.ToString(keyValuePair.Value));
		}
	}

	private static void FillDictionary(string key, Dictionary<string, Dictionary<string, object>> dictionary)
	{
		string empty = string.Empty;
		using (StopwatchLogger stopwatchLogger = new StopwatchLogger(string.Concat("Storager extracting ", key)))
		{
			empty = PlayerPrefs.GetString(key, "{}");
		}
		UnityEngine.Debug.Log(string.Concat(key, " (length): ", empty.Length));
		Dictionary<string, object> strs = null;
		using (StopwatchLogger stopwatchLogger1 = new StopwatchLogger(string.Concat("Json decoding ", key)))
		{
			strs = Json.Deserialize(empty) as Dictionary<string, object>;
		}
		if (strs != null && strs.Count > 0)
		{
			UnityEngine.Debug.Log(string.Concat(key, " (count): ", strs.Count));
			using (StopwatchLogger stopwatchLogger2 = new StopwatchLogger(string.Concat("Dictionary copying ", key)))
			{
				foreach (KeyValuePair<string, object> keyValuePair in strs)
				{
					Dictionary<string, object> value = keyValuePair.Value as Dictionary<string, object>;
					if (value == null)
					{
						continue;
					}
					dictionary.Add(keyValuePair.Key, value);
				}
			}
		}
	}

	private static List<string> FillList(string key)
	{
		List<string> strs = new List<string>();
		string str = PlayerPrefs.GetString(key, "[]");
		List<object> objs = Json.Deserialize(str) as List<object>;
		if (objs != null && objs.Count > 0)
		{
			IEnumerator<string> enumerator = objs.OfType<string>().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					strs.Add(enumerator.Current);
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
		}
		return strs;
	}

	private static void FillListDictionary(string key, List<Dictionary<string, string>> list)
	{
		string str = PlayerPrefs.GetString(key, "[]");
		List<object> objs = Json.Deserialize(str) as List<object>;
		if (objs != null && objs.Count > 0)
		{
			IEnumerator<Dictionary<string, object>> enumerator = objs.OfType<Dictionary<string, object>>().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Dictionary<string, object> current = enumerator.Current;
					Dictionary<string, string> strs = new Dictionary<string, string>();
					Dictionary<string, object>.Enumerator enumerator1 = current.GetEnumerator();
					try
					{
						while (enumerator1.MoveNext())
						{
							KeyValuePair<string, object> keyValuePair = enumerator1.Current;
							string value = keyValuePair.Value as string;
							if (value == null)
							{
								continue;
							}
							strs.Add(keyValuePair.Key, value);
						}
					}
					finally
					{
						((IDisposable)(object)enumerator1).Dispose();
					}
					list.Add(strs);
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
		}
	}

	[DebuggerHidden]
	public IEnumerator FriendRequest(string personId, Dictionary<string, object> socialEventParameters, Action<bool, bool> callbackAnswer = null)
	{
		FriendsController.u003cFriendRequestu003ec__Iterator60 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetABTestBalansCohortNameActual()
	{
		FriendsController.u003cGetABTestBalansCohortNameActualu003ec__Iterator3E variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetABTestBalansConfig(string nameConfig)
	{
		FriendsController.u003cGetABTestBalansConfigu003ec__Iterator3D variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetABTestBalansConfigName()
	{
		FriendsController.u003cGetABTestBalansConfigNameu003ec__Iterator39 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetABTestBankViewConfig()
	{
		FriendsController.u003cGetABTestBankViewConfigu003ec__Iterator3B variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetABTestBuffSystemConfig()
	{
		FriendsController.u003cGetABTestBuffSystemConfigu003ec__Iterator3C variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetABTestQuestSystemConfig()
	{
		FriendsController.u003cGetABTestQuestSystemConfigu003ec__Iterator74 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetABTestSandBoxConfig()
	{
		FriendsController.u003cGetABTestSandBoxConfigu003ec__Iterator73 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetABTestSpecialOffersConfig()
	{
		FriendsController.u003cGetABTestSpecialOffersConfigu003ec__Iterator75 variable = null;
		return variable;
	}

	private string GetAccesoriesString()
	{
		string empty;
		string str = Storager.getString(Defs.CapeEquppedSN, false);
		if (str != "cape_Custom")
		{
			empty = string.Empty;
		}
		else
		{
			string str1 = PlayerPrefs.GetString("NewUserCape");
			empty = Tools.DeserializeJson<CapeMemento>(str1).Cape;
			if (string.IsNullOrEmpty(empty))
			{
				empty = SkinsController.StringFromTexture(Resources.Load<Texture2D>("cape_CustomTexture"));
			}
		}
		string str2 = Storager.getString(Defs.HatEquppedSN, false);
		string str3 = Storager.getString(Defs.BootsEquppedSN, false);
		string str4 = Storager.getString("MaskEquippedSN", false);
		string str5 = Storager.getString(Defs.ArmorNewEquppedSN, false);
		Dictionary<string, string> strs = new Dictionary<string, string>()
		{
			{ "type", "0" },
			{ "name", str },
			{ "skin", empty }
		};
		Dictionary<string, string> strs1 = new Dictionary<string, string>()
		{
			{ "type", "1" },
			{ "name", str2 },
			{ "skin", string.Empty }
		};
		Dictionary<string, string> strs2 = new Dictionary<string, string>()
		{
			{ "type", "2" },
			{ "name", str3 },
			{ "skin", string.Empty }
		};
		Dictionary<string, string> strs3 = new Dictionary<string, string>()
		{
			{ "type", "3" },
			{ "name", str5 },
			{ "skin", string.Empty }
		};
		Dictionary<string, string> strs4 = new Dictionary<string, string>()
		{
			{ "type", "4" },
			{ "name", str4 },
			{ "skin", string.Empty }
		};
		List<Dictionary<string, string>> dictionaries = new List<Dictionary<string, string>>()
		{
			strs,
			strs1,
			strs2,
			strs3,
			strs4
		};
		return Json.Serialize(dictionaries);
	}

	[DebuggerHidden]
	private IEnumerator GetAllPlayersOnline()
	{
		FriendsController.u003cGetAllPlayersOnlineu003ec__Iterator48 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetAllPlayersOnlineWithClanInfo()
	{
		FriendsController.u003cGetAllPlayersOnlineWithClanInfou003ec__Iterator49 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetBanList()
	{
		FriendsController.u003cGetBanListu003ec__Iterator3F variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetBuffSettings(Task futureToWait)
	{
		FriendsController.u003cGetBuffSettingsu003ec__Iterator2D variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetClanDataLoop()
	{
		FriendsController.u003cGetClanDataLoopu003ec__Iterator57 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetClanDataOnce()
	{
		FriendsController.u003cGetClanDataOnceu003ec__Iterator59 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetClanPlayersOnline()
	{
		FriendsController.u003cGetClanPlayersOnlineu003ec__Iterator47 variable = null;
		return variable;
	}

	public DateTime GetDateLastClickJoinFriend(string friendId)
	{
		DateTime dateTime;
		if (!this.clicksJoinByFriends.ContainsKey(friendId))
		{
			return DateTime.MaxValue;
		}
		return (!DateTime.TryParse(this.clicksJoinByFriends[friendId], out dateTime) ? dateTime : DateTime.MaxValue);
	}

	public void GetFacebookFriendsInfo(Action callb)
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		base.StartCoroutine(this._GetFacebookFriendsInfo(callb));
	}

	[DebuggerHidden]
	private IEnumerator GetFiltersSettings()
	{
		return new FriendsController.u003cGetFiltersSettingsu003ec__Iterator2C();
	}

	public void GetFriendsData(bool _isUpdateInfoAboutAllFriends = false)
	{
		this.timerUpdateFriend = -1f;
		if (_isUpdateInfoAboutAllFriends)
		{
			this.isUpdateInfoAboutAllFriends = true;
		}
	}

	[DebuggerHidden]
	private IEnumerator GetFriendsDataLoop()
	{
		FriendsController.u003cGetFriendsDataLoopu003ec__Iterator6E variable = null;
		return variable;
	}

	public static Dictionary<string, object> GetFullPlayerDataById(string playerId)
	{
		Dictionary<string, object> strs;
		if (FriendsController.sharedController == null)
		{
			return null;
		}
		if (FriendsController.sharedController.friendsInfo.TryGetValue(playerId, out strs))
		{
			return strs;
		}
		if (FriendsController.sharedController.clanFriendsInfo.TryGetValue(playerId, out strs))
		{
			return strs;
		}
		if (FriendsController.sharedController.profileInfo.TryGetValue(playerId, out strs))
		{
			return strs;
		}
		return null;
	}

	[DebuggerHidden]
	private IEnumerator GetIfnoAboutPlayerLoop(string playerId)
	{
		FriendsController.u003cGetIfnoAboutPlayerLoopu003ec__Iterator5C variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetInfoAboutNPlayers()
	{
		FriendsController.u003cGetInfoAboutNPlayersu003ec__Iterator5A variable = null;
		return variable;
	}

	[DebuggerHidden]
	public IEnumerator GetInfoAboutNPlayers(List<string> ids)
	{
		FriendsController.u003cGetInfoAboutNPlayersu003ec__Iterator5B variable = null;
		return variable;
	}

	public void GetInfoAboutPlayers(List<string> ids)
	{
		base.StartCoroutine(this.GetInfoAboutNPlayers(ids));
	}

	[DebuggerHidden]
	private IEnumerator GetInfoByEverydayDelta()
	{
		FriendsController.u003cGetInfoByEverydayDeltau003ec__Iterator54 variable = null;
		return variable;
	}

	[DebuggerHidden]
	public IEnumerator GetInfoByIdCoroutine(string playerId)
	{
		FriendsController.u003cGetInfoByIdCoroutineu003ec__Iterator5D variable = null;
		return variable;
	}

	[DebuggerHidden]
	public IEnumerator GetInfoByParamCoroutine(string param)
	{
		FriendsController.u003cGetInfoByParamCoroutineu003ec__Iterator5E variable = null;
		return variable;
	}

	private string GetJsonIdsFacebookFriends()
	{
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log("Start GetJsonIdsFacebookFriends");
		}
		FacebookController facebookController = FacebookController.sharedController;
		if (facebookController == null)
		{
			return "[]";
		}
		if (facebookController.friendsList == null || facebookController.friendsList.Count == 0)
		{
			return "[]";
		}
		List<string> strs = new List<string>();
		for (int i = 0; i < facebookController.friendsList.Count; i++)
		{
			strs.Add(facebookController.friendsList[i].id);
		}
		string str = Json.Serialize(strs);
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log(string.Concat("GetJsonIdsFacebookFriends: ", str));
		}
		return str;
	}

	[DebuggerHidden]
	private IEnumerator GetLobbyNews(bool fromPause)
	{
		return new FriendsController.u003cGetLobbyNewsu003ec__Iterator2E();
	}

	[DebuggerHidden]
	private IEnumerator GetNewsLoop(Task futureToWait)
	{
		FriendsController.u003cGetNewsLoopu003ec__Iterator2B variable = null;
		return variable;
	}

	private void GetOurLAstOnline()
	{
		base.StartCoroutine(this.GetInfoByEverydayDelta());
		this.ReceivedLastOnline = true;
		base.StartCoroutine(this.UpdatePlayerOnlineLoop());
	}

	public void GetOurWins()
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		base.StartCoroutine(this._GetOurWins());
	}

	[DebuggerHidden]
	private IEnumerator GetPixelbookSettings()
	{
		return new FriendsController.u003cGetPixelbookSettingsu003ec__Iterator31();
	}

	[DebuggerHidden]
	private IEnumerator GetPixelbookSettingsLoop(Task futureToWait)
	{
		FriendsController.u003cGetPixelbookSettingsLoopu003ec__Iterator2A variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetPopularityMap()
	{
		FriendsController.u003cGetPopularityMapu003ec__Iterator38 variable = null;
		return variable;
	}

	public static FriendsController.PossiblleOrigin GetPossibleFriendFindOrigin(string playerId)
	{
		if (FriendsController.sharedController == null)
		{
			return FriendsController.PossiblleOrigin.None;
		}
		if (!FriendsController.sharedController.getPossibleFriendsResult.ContainsKey(playerId))
		{
			return FriendsController.PossiblleOrigin.None;
		}
		return FriendsController.sharedController.getPossibleFriendsResult[playerId];
	}

	[DebuggerHidden]
	private IEnumerator GetPossibleFriendsList(int playerLevel, int platformId, string clientVersion)
	{
		FriendsController.u003cGetPossibleFriendsListu003ec__Iterator6A variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetRatingSystemConfig()
	{
		FriendsController.u003cGetRatingSystemConfigu003ec__Iterator3A variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetTimeFromServer()
	{
		return new FriendsController.u003cGetTimeFromServeru003ec__Iterator30();
	}

	[DebuggerHidden]
	private IEnumerator GetTimeFromServerLoop()
	{
		FriendsController.u003cGetTimeFromServerLoopu003ec__Iterator2F variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetToken()
	{
		FriendsController.u003cGetTokenu003ec__Iterator51 variable = null;
		return variable;
	}

	private void HandleReceivedSelfID(string idfb)
	{
		if (idfb == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(this.id_fb) || !idfb.Equals(this.id_fb))
		{
			this.id_fb = idfb;
			if (!Storager.hasKey(this.FacebookIDKey))
			{
				Storager.setString(this.FacebookIDKey, string.Empty, false);
			}
			Storager.setString(this.FacebookIDKey, this.id_fb, false);
			this.SendOurData(false);
		}
	}

	public static string Hash(string action, string token = null)
	{
		if (action == null)
		{
			UnityEngine.Debug.LogWarning("Hash: action is null");
			return string.Empty;
		}
		object obj = token;
		if (obj == null)
		{
			if (FriendsController.sharedController == null)
			{
				obj = null;
			}
			else
			{
				obj = FriendsController.sharedController.id;
			}
		}
		string str = (string)obj;
		if (str == null)
		{
			UnityEngine.Debug.LogWarning("Hash: Token is null");
			return string.Empty;
		}
		string str1 = string.Concat((!action.Equals("get_player_online") ? string.Concat(ProtocolListGetter.CurrentPlatform, ":", GlobalGameController.AppVersion) : "*:*.*.*"), str, action);
		byte[] bytes = Encoding.UTF8.GetBytes(str1);
		string str2 = BitConverter.ToString(FriendsController._hmac.ComputeHash(bytes));
		return str2.Replace("-", string.Empty).ToLower();
	}

	public static string HashForPush(byte[] responceData)
	{
		if (responceData == null)
		{
			UnityEngine.Debug.LogWarning("HashForPush: responceData is null");
			return string.Empty;
		}
		if (FriendsController._hmac == null)
		{
			throw new InvalidOperationException("Hmac is not initialized yet.");
		}
		string str = BitConverter.ToString(FriendsController._hmac.ComputeHash(responceData));
		return str.Replace("-", string.Empty).ToLower();
	}

	[DebuggerHidden]
	public IEnumerable<float> InitController()
	{
		FriendsController.u003cInitControlleru003ec__Iterator32 variable = null;
		return variable;
	}

	public void InitOurInfo()
	{
		this.nick = ProfileController.GetPlayerNameOrDefault();
		this.skin = Convert.ToBase64String(SkinsController.currentSkinForPers.EncodeToPNG());
		this.rank = ExperienceController.sharedController.currentLevel;
		this.wins = Storager.getInt("Rating", false);
		this.survivalScore = PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0);
		this.coopScore = PlayerPrefs.GetInt(Defs.COOPScore, 0);
		this.infoLoaded = true;
	}

	public static bool IsAlreadySendClanInvitePlayer(string playerId)
	{
		if (FriendsController.sharedController == null)
		{
			return false;
		}
		return FriendsController.sharedController.ClanSentInvites.Contains(playerId);
	}

	public static bool IsAlreadySendInvitePlayer(string playerId)
	{
		if (FriendsController.sharedController == null)
		{
			return false;
		}
		return FriendsController.sharedController.invitesFromUs.Contains(playerId);
	}

	public static bool IsDataAboutFriendDownload(string playerId)
	{
		if (FriendsController.sharedController == null)
		{
			return false;
		}
		if (FriendsController.sharedController.friendsInfo.ContainsKey(playerId))
		{
			return true;
		}
		if (FriendsController.sharedController.clanFriendsInfo.ContainsKey(playerId))
		{
			return true;
		}
		if (FriendsController.sharedController.profileInfo.ContainsKey(playerId))
		{
			return true;
		}
		return false;
	}

	public static bool IsFriendInvitesDataExist()
	{
		bool flag;
		if (FriendsController.sharedController == null)
		{
			return false;
		}
		if (FriendsController.sharedController.invitesToUs.Count <= 0)
		{
			flag = false;
		}
		else
		{
			flag = (FriendsController.sharedController.clanFriendsInfo.Count > 0 ? true : FriendsController.sharedController.profileInfo.Count > 0);
		}
		return flag;
	}

	public static bool IsFriendsDataExist()
	{
		if (FriendsController.sharedController == null)
		{
			return false;
		}
		return (FriendsController.sharedController.friends.Count <= 0 ? false : FriendsController.sharedController.friendsInfo.Count > 0);
	}

	public static bool IsFriendsMax()
	{
		if (FriendsController.sharedController == null)
		{
			return false;
		}
		return FriendsController.sharedController.friends.Count >= Defs.maxCountFriend;
	}

	public static bool IsFriendsOrLocalDataExist()
	{
		if (FriendsController.sharedController == null)
		{
			return false;
		}
		List<string> strs = new List<string>();
		foreach (KeyValuePair<string, FriendsController.PossiblleOrigin> keyValuePair in FriendsController.sharedController.getPossibleFriendsResult)
		{
			if (!FriendsController.sharedController.profileInfo.ContainsKey(keyValuePair.Key) || keyValuePair.Value != FriendsController.PossiblleOrigin.Local)
			{
				continue;
			}
			strs.Add(keyValuePair.Key);
		}
		return (FriendsController.sharedController.friends.Count <= 0 || FriendsController.sharedController.friendsInfo.Count <= 0 ? strs.Count > 0 : true);
	}

	public static bool IsMaxClanMembers()
	{
		if (FriendsController.sharedController == null)
		{
			return false;
		}
		return FriendsController.sharedController.clanMembers.Count >= Defs.maxMemberClanCount;
	}

	public static bool IsMyPlayerId(string playerId)
	{
		if (FriendsController.sharedController == null)
		{
			return false;
		}
		if (string.IsNullOrEmpty(FriendsController.sharedController.id))
		{
			return false;
		}
		return FriendsController.sharedController.id.Equals(playerId);
	}

	public static bool IsPlayerOurClanMember(string playerId)
	{
		if (FriendsController.sharedController == null)
		{
			return false;
		}
		for (int i = 0; i < FriendsController.sharedController.clanMembers.Count; i++)
		{
			if (FriendsController.sharedController.clanMembers[i]["id"].Equals(playerId))
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsPlayerOurFriend(string playerId)
	{
		if (FriendsController.sharedController == null)
		{
			return false;
		}
		return FriendsController.sharedController.friends.Contains(playerId);
	}

	public static bool IsPossibleFriendsDataExist()
	{
		if (FriendsController.sharedController == null)
		{
			return false;
		}
		return (FriendsController.sharedController.getPossibleFriendsResult.Count <= 0 ? false : FriendsController.sharedController.profileInfo.Count > 0);
	}

	public static bool IsSelfClanLeader()
	{
		if (FriendsController.sharedController == null)
		{
			return false;
		}
		if (string.IsNullOrEmpty(FriendsController.sharedController.clanLeaderID))
		{
			return false;
		}
		return FriendsController.sharedController.clanLeaderID.Equals(FriendsController.sharedController.id);
	}

	public bool IsShowAdd(string _pixelBookID)
	{
		bool flag = true;
		if (this.friends.Count >= Defs.maxCountFriend || _pixelBookID.Equals("-1") || _pixelBookID.Equals(FriendsController.sharedController.id))
		{
			flag = false;
		}
		else
		{
			flag = (!FriendsController.sharedController.friends.Contains(_pixelBookID) ? !FriendsController.sharedController.notShowAddIds.Contains(_pixelBookID) : false);
		}
		return flag;
	}

	public static void JoinToFriendRoom(string friendId)
	{
		int num;
		if (FriendsController.sharedController == null)
		{
			return;
		}
		if (!FriendsController.sharedController.onlineInfo.ContainsKey(friendId))
		{
			return;
		}
		Dictionary<string, string> item = FriendsController.sharedController.onlineInfo[friendId];
		int.TryParse(item["game_mode"], out num);
		string str = item["room_name"];
		string item1 = item["map"];
		JoinToFriendRoomController instance = JoinToFriendRoomController.Instance;
		if (SceneInfoController.instance.GetInfoScene(int.Parse(item1)) != null && instance != null)
		{
			instance.ConnectToRoom(num, str, item1);
			FriendsController.sharedController.UpdateRecordByFriendsJoinClick(friendId);
		}
	}

	public static void LogPromoTrafficForwarding(FriendsController.TypeTrafficForwardingLog type)
	{
		if (type == FriendsController.TypeTrafficForwardingLog.view && (DateTime.Now - FriendsController.timeSendTrafficForwarding).TotalMinutes < 60)
		{
			return;
		}
		if (type == FriendsController.TypeTrafficForwardingLog.view || type == FriendsController.TypeTrafficForwardingLog.newView)
		{
			FriendsController.timeSendTrafficForwarding = DateTime.Now;
		}
		if (FriendsController.sharedController != null)
		{
			FriendsController.sharedController.StartCoroutine(FriendsController.sharedController.SendPromoTrafficForwarding(type));
		}
	}

	[DebuggerHidden]
	public IEnumerator MyWaitForSeconds(float tm)
	{
		FriendsController.u003cMyWaitForSecondsu003ec__Iterator58 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator OnApplicationPause(bool pause)
	{
		FriendsController.u003cOnApplicationPauseu003ec__Iterator42 variable = null;
		return variable;
	}

	private void OnDestroy()
	{
		this.DumpCurrentState();
	}

	private static void ParseABTestBalansArmorsConfig(object obj)
	{
		Dictionary<string, ItemPrice> strs = new Dictionary<string, ItemPrice>();
		List<object> objs = obj as List<object>;
		for (int i = 0; i < objs.Count; i++)
		{
			Dictionary<string, object> item = objs[i] as Dictionary<string, object>;
			string str = item["Name"] as string;
			int num = Convert.ToInt32(item["Price"]);
			string item1 = item["Currency"] as string;
			strs.Add(str, new ItemPrice(num, item1));
		}
		Defs2.ArmorPricesFromServer = strs;
	}

	private static void ParseABTestBalansAwardConfig(object obj)
	{
		List<object> objs = obj as List<object>;
		for (int i = 0; i < objs.Count; i++)
		{
			Dictionary<string, object> item = objs[i] as Dictionary<string, object>;
			string str = Convert.ToString(item["Mode"]);
			int[] num = new int[10];
			for (int j = 1; j <= 10; j++)
			{
				num[j - 1] = Convert.ToInt32(item[j.ToString()]);
			}
			if (str.Equals("XP Team"))
			{
				AdminSettingsController.expAvardTeamFight[0] = num;
				AdminSettingsController.minScoreTeamFight = 5;
			}
			if (str.Equals("Coins Team"))
			{
				AdminSettingsController.coinAvardTeamFight[0] = num;
				AdminSettingsController.minScoreTeamFight = 5;
			}
			if (str.Equals("XP DM"))
			{
				AdminSettingsController.expAvardDeathMath[0] = num;
				AdminSettingsController.minScoreDeathMath = 5;
			}
			if (str.Equals("Coins DM"))
			{
				AdminSettingsController.coinAvardDeathMath[0] = num;
				AdminSettingsController.minScoreDeathMath = 5;
			}
			if (str.Equals("XP Coop"))
			{
				AdminSettingsController.expAvardTimeBattle = num;
				AdminSettingsController.minScoreTimeBattle = 5;
			}
			if (str.Equals("Coins Coop"))
			{
				AdminSettingsController.coinAvardTimeBattle = num;
				AdminSettingsController.minScoreTimeBattle = 5;
			}
			if (str.Equals("XP Flag"))
			{
				AdminSettingsController.expAvardFlagCapture[0] = num;
				AdminSettingsController.minScoreFlagCapture = 5;
			}
			if (str.Equals("Coins Flag"))
			{
				AdminSettingsController.coinAvardFlagCapture[0] = num;
				AdminSettingsController.minScoreFlagCapture = 5;
			}
			if (str.Equals("XP Deadly"))
			{
				AdminSettingsController.expAvardDeadlyGames = num;
			}
			if (str.Equals("Coins Deadly"))
			{
				AdminSettingsController.coinAvardDeadlyGames = num;
			}
			if (str.Equals("XP Points"))
			{
				AdminSettingsController.expAvardCapturePoint[0] = num;
				AdminSettingsController.minScoreCapturePoint = 5;
			}
			if (str.Equals("Coins Points"))
			{
				AdminSettingsController.coinAvardCapturePoint[0] = num;
				AdminSettingsController.minScoreCapturePoint = 5;
			}
		}
	}

	public static void ParseABTestBalansConfig(bool isFirstParse = false)
	{
		string str = Storager.getString("abTestBalansConfigKey", false);
		if (string.IsNullOrEmpty(str))
		{
			if (Defs.abTestBalansCohort != Defs.ABTestCohortsType.NONE)
			{
				AnalyticsStuff.LogABTest("New Balance", Defs.abTestBalansCohortName, false);
				Defs.abTestBalansCohort = Defs.ABTestCohortsType.NONE;
				Defs.abTestBalansCohortName = string.Empty;
				FriendsController.ResetABTestsBalans();
				if (FriendsController.sharedController != null)
				{
					FriendsController.sharedController.SendOurData(false);
				}
			}
			return;
		}
		List<object> objs = Json.Deserialize(str) as List<object>;
		if (objs == null)
		{
			return;
		}
		for (int i = 0; i < objs.Count; i++)
		{
			Dictionary<string, object> item = objs[i] as Dictionary<string, object>;
			if (item.ContainsKey("NameConfig"))
			{
				FriendsController.ParseABTestBalansNameConfig(item["NameConfig"], isFirstParse);
			}
			if (item.ContainsKey("Weapons"))
			{
				FriendsController.ParseABTestBalansWeaponConfig(item["Weapons"]);
			}
			if (item.ContainsKey("Armors"))
			{
				FriendsController.ParseABTestBalansArmorsConfig(item["Armors"]);
			}
			if (item.ContainsKey("Levelling"))
			{
				FriendsController.ParseABTestBalansLevelingConfig(item["Levelling"]);
			}
			if (item.ContainsKey("Inapps"))
			{
				FriendsController.ParseABTestBalansInappsConfig(item["Inapps"]);
			}
			if (item.ContainsKey("Rewards"))
			{
				FriendsController.ParseABTestBalansAwardConfig(item["Rewards"]);
			}
			if (item.ContainsKey("SpecialEvents"))
			{
				FriendsController.ParseABTestBalansSpecialEventsConfig(item["SpecialEvents"]);
			}
		}
	}

	private static void ParseABTestBalansInappsConfig(object obj)
	{
		List<object> objs = obj as List<object>;
		for (int i = 0; i < objs.Count; i++)
		{
			Dictionary<string, object> item = objs[i] as Dictionary<string, object>;
			int num = Convert.ToInt32(item["Real"]);
			int num1 = Convert.ToInt32(item["Coins"]);
			int num2 = Convert.ToInt32(item["Gems"]);
			int num3 = Convert.ToInt32(item["Bonus Coins"]);
			int num4 = Convert.ToInt32(item["Bonus Gems"]);
			VirtualCurrencyHelper.RewriteInappsQuantity(num, num1, num2, num3, num4);
		}
	}

	private static void ParseABTestBalansLevelingConfig(object obj)
	{
		List<object> objs = obj as List<object>;
		for (int i = 0; i < objs.Count; i++)
		{
			Dictionary<string, object> item = objs[i] as Dictionary<string, object>;
			int num = Convert.ToInt32(item["Level"]);
			int num1 = Convert.ToInt32(item["XP"]);
			int num2 = Convert.ToInt32(item["Coins"]);
			int num3 = Convert.ToInt32(item["Gems"]);
			ExperienceController.RewriteLevelingParametersForLevel(num, num1, num2, num3);
		}
	}

	private static void ParseABTestBalansNameConfig(object obj, bool isFirstParse)
	{
		Dictionary<string, object> item = (obj as List<object>)[0] as Dictionary<string, object>;
		if (item.ContainsKey("Group"))
		{
			Defs.abTestBalansCohort = (Defs.ABTestCohortsType)((int)Enum.Parse(typeof(Defs.ABTestCohortsType), item["Group"] as string));
			if (Defs.abTestBalansCohort == Defs.ABTestCohortsType.B)
			{
				Defs.isABTestBalansCohortActual = true;
			}
			if (Application.isEditor)
			{
				UnityEngine.Debug.Log(string.Concat("Defs.abTestBalansCohort = ", Defs.abTestBalansCohort.ToString()));
			}
		}
		if (item.ContainsKey("NameGroup"))
		{
			Defs.abTestBalansCohortName = item["NameGroup"] as string;
			if (isFirstParse)
			{
				AnalyticsStuff.LogABTest("New Balance", Defs.abTestBalansCohortName, true);
				if (FriendsController.sharedController != null)
				{
					FriendsController.sharedController.SendOurData(false);
				}
			}
			if (Application.isEditor)
			{
				UnityEngine.Debug.Log(string.Concat("abTestBalansCohortName = ", Defs.abTestBalansCohortName.ToString()));
			}
		}
	}

	private static void ParseABTestBalansSpecialEventsConfig(object obj)
	{
		List<object> objs = obj as List<object>;
		for (int i = 0; i < objs.Count; i++)
		{
			Dictionary<string, object> item = objs[i] as Dictionary<string, object>;
			string str = Convert.ToString(item["Event"]);
			int num = Convert.ToInt32(item["Coins"]);
			int num1 = Convert.ToInt32(item["Gems"]);
			Convert.ToInt32(item["XP"]);
			if (str.Equals("StartCapital"))
			{
				Defs.abTestBalansStartCapitalCoins = num;
				Defs.abTestBalansStartCapitalGems = num1;
			}
		}
	}

	private static void ParseABTestBalansWeaponConfig(object obj)
	{
		Dictionary<string, ItemPrice> strs = new Dictionary<string, ItemPrice>();
		List<object> objs = obj as List<object>;
		for (int i = 0; i < objs.Count; i++)
		{
			Dictionary<string, object> item = objs[i] as Dictionary<string, object>;
			string str = item["Name"] as string;
			int num = Convert.ToInt32(item["Price"]);
			string item1 = item["Currency"] as string;
			strs.Add(str, new ItemPrice(num, item1));
			if (item.ContainsKey("DPS"))
			{
				float single = Convert.ToSingle(item["DPS"]);
				float[] singleArray = new float[6];
				for (int j = 0; j < 6; j++)
				{
					singleArray[j] = single;
				}
				FriendsController.dpsWeaponsFromABTestBalans.Add(str, singleArray);
			}
			if (item.ContainsKey("Damage"))
			{
				float single1 = Convert.ToSingle(item["Damage"]);
				float[] singleArray1 = new float[6];
				for (int k = 0; k < 6; k++)
				{
					singleArray1[k] = single1;
				}
				FriendsController.damageWeaponsFromABTestBalans.Add(str, singleArray1);
			}
		}
		Defs2.GunPricesFromServer = strs;
	}

	public static void ParseABTestBankViewConfig()
	{
		if (!Storager.hasKey("abTestBankViewKey") || string.IsNullOrEmpty(Storager.getString("abTestBankViewKey", false)))
		{
			return;
		}
		string str = Storager.getString("abTestBankViewKey", false);
		Dictionary<string, object> strs = Json.Deserialize(str) as Dictionary<string, object>;
		if (strs != null && strs.ContainsKey("enableABTest"))
		{
			int num = Convert.ToInt32(strs["enableABTest"]);
			Defs.isActivABTestStaticBank = num == 1;
			if (strs.ContainsKey("currentViewBank"))
			{
				FriendsController.isCurrentBankViewStatic = Convert.ToString(strs["currentViewBank"]).Equals("static");
			}
			if (Defs.isActivABTestStaticBank)
			{
				FriendsController.isShowStaticBank = FriendsController.isCohortStaticBank;
			}
			else if (!FriendsController.isCurrentBankViewStatic)
			{
				FriendsController.isShowStaticBank = false;
			}
			else
			{
				FriendsController.isShowStaticBank = true;
			}
			if (strs.ContainsKey("configName"))
			{
				FriendsController.configNameABTestBankView = Convert.ToString(strs["configName"]);
			}
			if (FriendsController.isShowStaticBank)
			{
				List<object> item = strs["coinInappsQuantity"] as List<object>;
				List<object> objs = strs["gemsInappsQuantity"] as List<object>;
				for (int i = 0; i < 7; i++)
				{
					VirtualCurrencyHelper._coinInappsQuantityStaticBank[i] = Convert.ToInt32(item[i]);
					VirtualCurrencyHelper._gemsInappsQuantityStaticBank[i] = Convert.ToInt32(objs[i]);
				}
			}
		}
	}

	public static void ParseABTestBuffSystemConfig()
	{
		if (!Storager.hasKey("abTestBuffSystemKey") || string.IsNullOrEmpty(Storager.getString("abTestBuffSystemKey", false)))
		{
			return;
		}
		string str = Storager.getString("abTestBuffSystemKey", false);
		Dictionary<string, object> strs = Json.Deserialize(str) as Dictionary<string, object>;
		if (strs != null && strs.ContainsKey("enableABTest"))
		{
			int num = Convert.ToInt32(strs["enableABTest"]);
			Defs.isActivABTestBuffSystem = num == 1;
			if (strs.ContainsKey("currentState"))
			{
				FriendsController.isCurrentStateBuffSystem = Convert.ToString(strs["currentState"]).Equals("BuffSystem");
			}
			if (Defs.isActivABTestBuffSystem)
			{
				FriendsController.useBuffSystem = FriendsController.isCohortBuffSystem;
			}
			else if (!FriendsController.isCurrentStateBuffSystem)
			{
				FriendsController.useBuffSystem = false;
			}
			else
			{
				FriendsController.useBuffSystem = true;
			}
			if (strs.ContainsKey("cohortSuffix"))
			{
				FriendsController.configNameABTestBuffSystem = Convert.ToString(strs["cohortSuffix"]);
			}
		}
	}

	public static void ParseABTestQuestSystemConfig(bool isFromReset = false)
	{
		if (!Storager.hasKey("abTestQuestSystemConfigKey") || string.IsNullOrEmpty(Storager.getString("abTestQuestSystemConfigKey", false)))
		{
			return;
		}
		string str = Storager.getString("abTestQuestSystemConfigKey", false);
		Dictionary<string, object> strs = Json.Deserialize(str) as Dictionary<string, object>;
		if (strs != null && strs.ContainsKey("enableABTest"))
		{
			int num = Convert.ToInt32(strs["enableABTest"]);
			int num1 = Convert.ToInt32(strs["currentStateEnable"]);
			if (num != 1 || Defs.cohortABTestQuestSystem == Defs.ABTestCohortsType.SKIP)
			{
				if (!isFromReset)
				{
					FriendsController.ResetABTestQuestSystem();
				}
				FriendsController.QuestSystemEnabled = num1 == 1;
			}
			else
			{
				FriendsController.configNameABTestQuestSystem = Convert.ToString(strs["configName"]);
				if (Defs.cohortABTestQuestSystem == Defs.ABTestCohortsType.NONE)
				{
					Defs.cohortABTestQuestSystem = (Defs.ABTestCohortsType)UnityEngine.Random.Range(1, 3);
					string str1 = string.Concat((Defs.cohortABTestQuestSystem != Defs.ABTestCohortsType.A ? "QuestOff_" : "QuestON_"), FriendsController.configNameABTestQuestSystem);
					AnalyticsStuff.LogABTest("QuestSystem", str1, true);
				}
				if (Defs.cohortABTestQuestSystem != Defs.ABTestCohortsType.B)
				{
					FriendsController.QuestSystemEnabled = true;
				}
				else
				{
					FriendsController.QuestSystemEnabled = false;
				}
			}
		}
	}

	public static void ParseABTestSandBoxConfig(bool isFromReset = false)
	{
		if (!Storager.hasKey("abTestSandBoxConfigKey") || string.IsNullOrEmpty(Storager.getString("abTestSandBoxConfigKey", false)))
		{
			return;
		}
		string str = Storager.getString("abTestSandBoxConfigKey", false);
		Dictionary<string, object> strs = Json.Deserialize(str) as Dictionary<string, object>;
		if (strs != null && strs.ContainsKey("enableABTest"))
		{
			int num = Convert.ToInt32(strs["enableABTest"]);
			int num1 = Convert.ToInt32(strs["currentStateEnable"]);
			if (num != 1 || Defs.cohortABTestSandBox == Defs.ABTestCohortsType.SKIP)
			{
				if (!isFromReset)
				{
					FriendsController.ResetABTestSandBox();
				}
				FriendsController.SandboxEnabled = num1 == 1;
			}
			else
			{
				FriendsController.configNameABTestSandBox = Convert.ToString(strs["configName"]);
				if (Defs.cohortABTestSandBox == Defs.ABTestCohortsType.NONE)
				{
					Defs.cohortABTestSandBox = (Defs.ABTestCohortsType)UnityEngine.Random.Range(1, 3);
					string str1 = string.Concat((Defs.cohortABTestSandBox != Defs.ABTestCohortsType.A ? "SandBoxOff_" : "SandBoxON_"), FriendsController.configNameABTestSandBox);
					AnalyticsStuff.LogABTest("SandBox", str1, true);
				}
				if (Defs.cohortABTestSandBox != Defs.ABTestCohortsType.B)
				{
					FriendsController.SandboxEnabled = true;
				}
				else
				{
					FriendsController.SandboxEnabled = false;
				}
			}
		}
	}

	public static void ParseABTestSpecialOffersConfig(bool isFromReset = false)
	{
		if (!Storager.hasKey("abTestSpecialOffersConfigKey") || string.IsNullOrEmpty(Storager.getString("abTestSpecialOffersConfigKey", false)))
		{
			return;
		}
		string str = Storager.getString("abTestSpecialOffersConfigKey", false);
		Dictionary<string, object> strs = Json.Deserialize(str) as Dictionary<string, object>;
		if (strs != null && strs.ContainsKey("enableABTest"))
		{
			int num = Convert.ToInt32(strs["enableABTest"]);
			int num1 = Convert.ToInt32(strs["currentStateEnable"]);
			if (num != 1 || Defs.cohortABTestSpecialOffers == Defs.ABTestCohortsType.SKIP)
			{
				if (!isFromReset)
				{
					FriendsController.ResetABTestSpecialOffers();
				}
				FriendsController.SpecialOffersEnabled = num1 == 1;
			}
			else
			{
				FriendsController.configNameABTestSpecialOffers = Convert.ToString(strs["configName"]);
				if (Defs.cohortABTestSpecialOffers == Defs.ABTestCohortsType.NONE)
				{
					Defs.cohortABTestSpecialOffers = (Defs.ABTestCohortsType)UnityEngine.Random.Range(1, 3);
					string str1 = string.Concat((Defs.cohortABTestSpecialOffers != Defs.ABTestCohortsType.A ? "OffersOff_" : "OffersON_"), FriendsController.configNameABTestSpecialOffers);
					AnalyticsStuff.LogABTest("SpecialOffers", str1, true);
				}
				if (Defs.cohortABTestSpecialOffers != Defs.ABTestCohortsType.B)
				{
					FriendsController.SpecialOffersEnabled = true;
				}
				else
				{
					FriendsController.SpecialOffersEnabled = false;
				}
			}
		}
	}

	private Dictionary<string, object> ParseInfo(string info)
	{
		return Json.Deserialize(info) as Dictionary<string, object>;
	}

	public static FriendsController.ResultParseOnlineData ParseOnlineData(Dictionary<string, string> onlineData)
	{
		string item = onlineData["game_mode"];
		string str = onlineData["protocol"];
		string empty = string.Empty;
		if (onlineData.ContainsKey("map"))
		{
			empty = onlineData["map"];
		}
		return FriendsController.ParseOnlineData(item, str, empty);
	}

	private static FriendsController.ResultParseOnlineData ParseOnlineData(string gameModeString, string protocolString, string mapIndex)
	{
		int num;
		int num1 = int.Parse(gameModeString);
		if (num1 <= 99)
		{
			num1 = -1;
		}
		else
		{
			num1 /= 100;
		}
		if (int.TryParse(gameModeString, out num))
		{
			if (num > 99)
			{
				num = num - num1 * 100;
			}
			num /= 10;
		}
		else
		{
			num = -1;
		}
		FriendsController.ResultParseOnlineData resultParseOnlineDatum = new FriendsController.ResultParseOnlineData();
		bool flag = (num1 == -1 ? true : num1 != (int)ConnectSceneNGUIController.myPlatformConnect);
		bool flag1 = (num == -1 ? true : ExpController.GetOurTier() != num);
		bool flag2 = num1 == 3;
		int num2 = Convert.ToInt32(gameModeString);
		string str = protocolString;
		string multiplayerProtocolVersion = GlobalGameController.MultiplayerProtocolVersion;
		resultParseOnlineDatum.gameMode = gameModeString;
		resultParseOnlineDatum.mapIndex = mapIndex;
		bool flag3 = num2 == 6;
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(mapIndex));
		bool flag4 = (!(infoScene != null) || !infoScene.IsAvaliableForMode(TypeModeGame.Dater) ? false : true);
		if (flag4 && !FriendsController.SandboxEnabled)
		{
			return null;
		}
		if (flag3)
		{
			resultParseOnlineDatum.notConnectCondition = FriendsController.NotConnectCondition.InChat;
		}
		else if (!flag2 && flag && !flag4)
		{
			resultParseOnlineDatum.notConnectCondition = FriendsController.NotConnectCondition.platform;
		}
		else if (!flag2 && flag1 && !flag4)
		{
			resultParseOnlineDatum.notConnectCondition = FriendsController.NotConnectCondition.level;
		}
		else if (multiplayerProtocolVersion != str)
		{
			resultParseOnlineDatum.notConnectCondition = FriendsController.NotConnectCondition.clientVersion;
		}
		else if (infoScene != null)
		{
			resultParseOnlineDatum.notConnectCondition = FriendsController.NotConnectCondition.None;
		}
		else
		{
			resultParseOnlineDatum.notConnectCondition = FriendsController.NotConnectCondition.map;
		}
		resultParseOnlineDatum.isPlayerInChat = flag3;
		return resultParseOnlineDatum;
	}

	private void ParseOnlinesResponse(string response)
	{
		Dictionary<string, object> strs = Json.Deserialize(response) as Dictionary<string, object>;
		if (strs == null)
		{
			if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
			{
				UnityEngine.Debug.LogWarning(" GetAllPlayersOnline info = null");
			}
			return;
		}
		Dictionary<string, Dictionary<string, string>> strs1 = new Dictionary<string, Dictionary<string, string>>();
		foreach (string key in strs.Keys)
		{
			Dictionary<string, object> item = strs[key] as Dictionary<string, object>;
			Dictionary<string, string> strs2 = new Dictionary<string, string>();
			foreach (KeyValuePair<string, object> keyValuePair in item)
			{
				strs2.Add(keyValuePair.Key, keyValuePair.Value as string);
			}
			strs1.Add(key, strs2);
		}
		this.onlineInfo.Clear();
		foreach (string str in strs1.Keys)
		{
			Dictionary<string, string> item1 = strs1[str];
			int num = int.Parse(item1["game_mode"]);
			if (num - num / 10 * 10 == 3)
			{
				continue;
			}
			if (this.onlineInfo.ContainsKey(str))
			{
				this.onlineInfo[str] = item1;
			}
			else
			{
				this.onlineInfo.Add(str, item1);
			}
		}
	}

	public static void ParseRatingSystemConfig()
	{
		if (!Storager.hasKey("rSCKey") || string.IsNullOrEmpty(Storager.getString("rSCKey", false)))
		{
			return;
		}
		string str = Storager.getString("rSCKey", false);
		Dictionary<string, object> strs = Json.Deserialize(str) as Dictionary<string, object>;
		Defs.isActivABTestRatingSystem = (!strs.ContainsKey("abTestEnabled") ? 0 : Convert.ToInt32(strs["abTestEnabled"])) == 1;
		if (strs.ContainsKey("currentStateUseRatingSystem"))
		{
			FriendsController.isCurrentUseRatingSystem = Convert.ToInt32(strs["currentStateUseRatingSystem"]) == 1;
		}
		if (strs.ContainsKey("currentStateBuffSystem"))
		{
			FriendsController.isCurrentStateBuffSystem = Convert.ToString(strs["currentStateBuffSystem"]).Equals("BuffSystem");
		}
		if (Defs.isActivABTestRatingSystem)
		{
			FriendsController.isUseRatingSystem = (!FriendsController.isCohortUseRatingSystem ? false : FriendsController.isCurrentUseRatingSystem);
			FriendsController.useBuffSystem = (!FriendsController.isCohortUseRatingSystem ? false : FriendsController.isCurrentStateBuffSystem);
		}
		else
		{
			if (!FriendsController.isCurrentUseRatingSystem)
			{
				FriendsController.isUseRatingSystem = false;
			}
			else
			{
				FriendsController.isUseRatingSystem = true;
			}
			if (!FriendsController.isCurrentStateBuffSystem)
			{
				FriendsController.useBuffSystem = false;
			}
			else
			{
				FriendsController.useBuffSystem = true;
			}
		}
		if (strs.ContainsKey("configName"))
		{
			FriendsController.configNameABTestRatingSystem = Convert.ToString(strs["configName"]);
		}
		RatingSystem.instance.ParseConfig();
	}

	private void ParseUpdateFriendsInfoResponse(string response, bool _isUpdateInfoAboutAllFriends)
	{
		Dictionary<string, object> strs = Json.Deserialize(response) as Dictionary<string, object>;
		List<string> strs1 = new List<string>();
		HashSet<string> strs2 = new HashSet<string>(this.friends);
		HashSet<string> strs3 = new HashSet<string>(this.invitesToUs);
		if (strs.ContainsKey("friends"))
		{
			this.friends.Clear();
			List<object> item = strs["friends"] as List<object>;
			for (int i = 0; i < item.Count; i++)
			{
				string str = item[i] as string;
				if (this.getPossibleFriendsResult.ContainsKey(str))
				{
					this.getPossibleFriendsResult.Remove(str);
				}
				this.friends.Add(str);
				if (!_isUpdateInfoAboutAllFriends && !this.friendsInfo.ContainsKey(str) || _isUpdateInfoAboutAllFriends)
				{
					strs1.Add(str);
				}
			}
		}
		if (strs.ContainsKey("invites"))
		{
			this.invitesToUs.Clear();
			List<object> objs = strs["invites"] as List<object>;
			for (int j = 0; j < objs.Count; j++)
			{
				string item1 = objs[j] as string;
				if (!this.friends.Contains(item1))
				{
					this.invitesToUs.Add(item1);
				}
				if (!_isUpdateInfoAboutAllFriends && !this.friendsInfo.ContainsKey(item1) && !this.clanFriendsInfo.ContainsKey(item1) && !this.profileInfo.ContainsKey(item1) || _isUpdateInfoAboutAllFriends)
				{
					strs1.Add(item1);
				}
			}
		}
		if (strs.ContainsKey("invites_outcoming"))
		{
			this.invitesFromUs.Clear();
			List<object> objs1 = strs["invites_outcoming"] as List<object>;
			for (int k = 0; k < objs1.Count; k++)
			{
				string str1 = objs1[k] as string;
				if (!this.friends.Contains(str1))
				{
					this.invitesFromUs.Add(str1);
				}
			}
		}
		if (_isUpdateInfoAboutAllFriends)
		{
			List<string> strs4 = new List<string>(this.friends);
			List<string> strs5 = new List<string>();
			strs4.AddRange(this.invitesToUs);
			foreach (KeyValuePair<string, Dictionary<string, object>> keyValuePair in this.friendsInfo)
			{
				if (strs4.Contains(keyValuePair.Key))
				{
					continue;
				}
				strs5.Add(keyValuePair.Key);
			}
			if (strs5.Count > 0)
			{
				for (int l = 0; l < strs5.Count; l++)
				{
					this.friendsInfo.Remove(strs5[l]);
				}
				this.SaveCurrentState();
			}
		}
		if (strs.ContainsKey("onLines"))
		{
			this.ParseOnlinesResponse(Json.Serialize(strs["onLines"]));
		}
		if (strs1.Count <= 0)
		{
			if ((!strs2.SetEquals(this.friends) ? 0 : (int)strs3.SetEquals(this.invitesToUs)) == 0 && FriendsController.FriendsUpdated != null)
			{
				FriendsController.FriendsUpdated();
			}
			this.TrySendEventHideBoxProcessFriendsData();
		}
		else
		{
			base.StartCoroutine(this.GetInfoAboutNPlayers(strs1));
		}
		if (strs.ContainsKey("chat"))
		{
			string str2 = Json.Serialize(strs["chat"]);
			if (ChatController.sharedController != null)
			{
				ChatController.sharedController.ParseUpdateChatMessageResponse(str2);
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator RefreshClanOnline()
	{
		FriendsController.u003cRefreshClanOnlineu003ec__Iterator46 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator RefreshOnlinePlayer()
	{
		FriendsController.u003cRefreshOnlinePlayeru003ec__Iterator45 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator RefreshOnlinePlayerWithClanInfo()
	{
		FriendsController.u003cRefreshOnlinePlayerWithClanInfou003ec__Iterator44 variable = null;
		return variable;
	}

	public void RejectClanInvite(string clanID, string playerID = null)
	{
		if (!string.IsNullOrEmpty(clanID) && FriendsController.readyToOperate)
		{
			base.StartCoroutine(this._RejectClanInvite(clanID, playerID));
		}
	}

	public void RejectInvite(string rejecteeId, Action<bool> action = null)
	{
		if (FriendsController.readyToOperate)
		{
			base.StartCoroutine(this.RejectInviteFriendCoroutine(rejecteeId, action));
		}
	}

	[DebuggerHidden]
	private IEnumerator RejectInviteFriendCoroutine(string rejecteeId, Action<bool> action = null)
	{
		FriendsController.u003cRejectInviteFriendCoroutineu003ec__Iterator67 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator RequestWinCountTimestampCoroutine()
	{
		return new FriendsController.u003cRequestWinCountTimestampCoroutineu003ec__Iterator53();
	}

	public void ResetABTestBalansConfig()
	{
		Defs.abTestBalansCohort = Defs.ABTestCohortsType.NONE;
		Defs.abTestBalansCohortName = string.Empty;
		Defs2.GunPricesFromServer = null;
		Defs2.ArmorPricesFromServer = null;
		ExperienceController.ResetLevelingOnDefault();
		if (ExperienceController.sharedController != null && ExperienceController.sharedController.CurrentExperience >= ExperienceController.MaxExpLevels[ExperienceController.sharedController.currentLevel])
		{
			ExperienceController.sharedController.SetCurrentExperience(ExperienceController.MaxExpLevels[ExperienceController.sharedController.currentLevel] - 5);
		}
		VirtualCurrencyHelper.ResetInappsQuantityOnDefault();
		AdminSettingsController.ResetAvardSettingsOnDefault();
	}

	public static void ResetABTestQuestSystem()
	{
		if (Defs.cohortABTestQuestSystem != Defs.ABTestCohortsType.SKIP)
		{
			if (Defs.cohortABTestQuestSystem == Defs.ABTestCohortsType.A || Defs.cohortABTestQuestSystem == Defs.ABTestCohortsType.B)
			{
				string str = string.Concat((Defs.cohortABTestQuestSystem != Defs.ABTestCohortsType.A ? "QuestOff_" : "QuestON_"), FriendsController.configNameABTestQuestSystem);
				AnalyticsStuff.LogABTest("QuestSystem", str, false);
			}
			Defs.cohortABTestQuestSystem = Defs.ABTestCohortsType.SKIP;
			FriendsController.ParseABTestQuestSystemConfig(true);
		}
	}

	public static void ResetABTestSandBox()
	{
		if (Defs.cohortABTestSandBox != Defs.ABTestCohortsType.SKIP)
		{
			if (Defs.cohortABTestSandBox == Defs.ABTestCohortsType.A || Defs.cohortABTestSandBox == Defs.ABTestCohortsType.B)
			{
				string str = string.Concat((Defs.cohortABTestSandBox != Defs.ABTestCohortsType.A ? "SandBoxOff_" : "SandBoxON_"), FriendsController.configNameABTestSandBox);
				AnalyticsStuff.LogABTest("SandBox", str, false);
			}
			Defs.cohortABTestSandBox = Defs.ABTestCohortsType.SKIP;
			FriendsController.ParseABTestSandBoxConfig(true);
		}
	}

	public static void ResetABTestsBalans()
	{
		Storager.setString("abTestBalansConfigKey", string.Empty, false);
		if (FriendsController.sharedController != null)
		{
			FriendsController.sharedController.ResetABTestBalansConfig();
		}
	}

	public static void ResetABTestSpecialOffers()
	{
		if (Defs.cohortABTestSpecialOffers != Defs.ABTestCohortsType.SKIP)
		{
			if (Defs.cohortABTestSpecialOffers == Defs.ABTestCohortsType.A || Defs.cohortABTestSpecialOffers == Defs.ABTestCohortsType.B)
			{
				string str = string.Concat((Defs.cohortABTestSpecialOffers != Defs.ABTestCohortsType.A ? "OffersOff_" : "OffersON_"), FriendsController.configNameABTestSpecialOffers);
				AnalyticsStuff.LogABTest("SpecialOffers", str, false);
			}
			Defs.cohortABTestSpecialOffers = Defs.ABTestCohortsType.SKIP;
			FriendsController.ParseABTestSpecialOffersConfig(true);
		}
	}

	private void SaveCurrentState()
	{
		if (this.friends != null)
		{
			PlayerPrefs.SetString("FriendsKey", Json.Serialize(this.friends) ?? "[]");
		}
		if (this.invitesToUs != null)
		{
			PlayerPrefs.SetString("ToUsKey", Json.Serialize(this.invitesToUs) ?? "[]");
		}
		if (this.playersInfo != null)
		{
			PlayerPrefs.SetString("PlayerInfoKey", Json.Serialize(this.playersInfo) ?? "{}");
		}
		if (this.friendsInfo != null)
		{
			PlayerPrefs.SetString("FriendsInfoKey", Json.Serialize(this.friendsInfo) ?? "{}");
		}
		if (this.clanFriendsInfo != null)
		{
			PlayerPrefs.SetString("ClanFriendsInfoKey", Json.Serialize(this.clanFriendsInfo) ?? "{}");
		}
		if (this.ClanInvites != null)
		{
			PlayerPrefs.SetString("ClanInvitesKey", Json.Serialize(this.ClanInvites) ?? "[]");
		}
		this.UpdateCachedClickJoinListValue();
	}

	private void SaveProfileData()
	{
		if (this.ourInfo != null && this.ourInfo.ContainsKey("wincount"))
		{
			int num = 0;
			Dictionary<string, object> item = this.ourInfo["wincount"] as Dictionary<string, object>;
			num = 0;
			item.TryGetValue<int>("0", out num);
			Storager.setInt(Defs.RatingDeathmatch, num, false);
			num = 0;
			item.TryGetValue<int>("2", out num);
			Storager.setInt(Defs.RatingTeamBattle, num, false);
			num = 0;
			item.TryGetValue<int>("3", out num);
			Storager.setInt(Defs.RatingHunger, num, false);
			num = 0;
			item.TryGetValue<int>("4", out num);
			Storager.setInt(Defs.RatingCapturePoint, num, false);
		}
	}

	public void SendAccessories()
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		base.StartCoroutine(this.UpdateAccessories());
	}

	public void SendAddPurchaseEvent(int inapp, string purchaseId)
	{
		base.StartCoroutine(this.AddPurchaseEvent(inapp, purchaseId));
	}

	[DebuggerHidden]
	private IEnumerator SendClanInvitation(string personID, Action<bool, bool> callbackResult = null)
	{
		FriendsController.u003cSendClanInvitationu003ec__Iterator64 variable = null;
		return variable;
	}

	public void SendCreateClan(string personId, string nameClan, string skinClan, Action<string> ErrorHandler)
	{
		if (!string.IsNullOrEmpty(personId) && !string.IsNullOrEmpty(nameClan) && !string.IsNullOrEmpty(skinClan) && FriendsController.readyToOperate)
		{
			base.StartCoroutine(this._SendCreateClan(personId, nameClan, skinClan, ErrorHandler));
		}
		else if (ErrorHandler != null)
		{
			ErrorHandler("Error: FALSE:  ! string.IsNullOrEmpty (personId) && ! string.IsNullOrEmpty (nameClan) && ! string.IsNullOrEmpty (skinClan) && readyToOperate");
		}
	}

	private static void SendEmailWithMyId()
	{
		MailUrlBuilder mailUrlBuilder = new MailUrlBuilder()
		{
			to = string.Empty,
			subject = LocalizationStore.Get("Key_1565")
		};
		string str = (FriendsController.sharedController == null ? string.Empty : FriendsController.sharedController.id);
		string str1 = LocalizationStore.Get("Key_1508");
		mailUrlBuilder.body = string.Format(str1, FriendsController.sharedController.id);
		Application.OpenURL(mailUrlBuilder.GetUrl());
	}

	public static void SendFriendshipRequest(string playerId, Dictionary<string, object> socialEventParameters, Action<bool, bool> callbackResult)
	{
		if (FriendsController.sharedController == null)
		{
			return;
		}
		if (socialEventParameters == null)
		{
			throw new ArgumentNullException("socialEventParameters");
		}
		FriendsController.sharedController.StartCoroutine(FriendsController.sharedController.FriendRequest(playerId, socialEventParameters, callbackResult));
	}

	[DebuggerHidden]
	private IEnumerator SendGameTime()
	{
		FriendsController.u003cSendGameTimeu003ec__Iterator6D variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator SendGameTimeLoop()
	{
		FriendsController.u003cSendGameTimeLoopu003ec__Iterator6C variable = null;
		return variable;
	}

	public void SendInvitation(string personId, Dictionary<string, object> socialEventParameters)
	{
		if (!string.IsNullOrEmpty(personId) && FriendsController.readyToOperate)
		{
			if (socialEventParameters == null)
			{
				throw new ArgumentNullException("socialEventParameters");
			}
			base.StartCoroutine(this.FriendRequest(personId, socialEventParameters, null));
		}
	}

	public static void SendMyIdByEmail()
	{
		MainMenuController.DoMemoryConsumingTaskInEmptyScene(() => InfoWindowController.ShowDialogBox(LocalizationStore.Get("Key_1572"), new Action(FriendsController.SendEmailWithMyId), null), null);
	}

	public void SendOurData(bool SendSkin = false)
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		base.StartCoroutine(this.UpdatePlayer(SendSkin));
	}

	public static void SendPlayerInviteToClan(string personId, Action<bool, bool> callbackResult = null)
	{
		if (FriendsController.sharedController == null)
		{
			return;
		}
		if (!string.IsNullOrEmpty(personId) && FriendsController.readyToOperate)
		{
			FriendsController.sharedController.StartCoroutine(FriendsController.sharedController.SendClanInvitation(personId, callbackResult));
		}
	}

	[DebuggerHidden]
	public IEnumerator SendPromoTrafficForwarding(FriendsController.TypeTrafficForwardingLog type)
	{
		FriendsController.u003cSendPromoTrafficForwardingu003ec__Iterator72 variable = null;
		return variable;
	}

	[DebuggerHidden]
	public IEnumerator SendReviewForPlayerWithID(int rating, string msgReview)
	{
		FriendsController.u003cSendReviewForPlayerWithIDu003ec__Iterator71 variable = null;
		return variable;
	}

	public void SendRoundWon()
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		int num = -1;
		if (PhotonNetwork.room != null)
		{
			num = (int)ConnectSceneNGUIController.regim;
		}
		if (num != -1)
		{
			base.StartCoroutine(this._SendRoundWon(num));
		}
	}

	private void SetWinCountTimestamp(string timestamp, int winCount)
	{
		this._winCountTimestamp = new KeyValuePair<string, int>?(new KeyValuePair<string, int>(timestamp, winCount));
		string str = string.Format("{{ \"{0}\": {1} }}", timestamp, winCount);
		Storager.setString("Win Count Timestamp", str, false);
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log(string.Concat("Setting win count timestamp:    ", str));
		}
	}

	public static void ShowProfile(string id, ProfileWindowType type, Action<bool> onCloseEvent = null)
	{
		if (FriendsController._friendProfileController == null)
		{
			FriendsController._friendProfileController = new FriendProfileController(onCloseEvent);
		}
		FriendsController._friendProfileController.HandleProfileClickedCore(id, type, onCloseEvent);
	}

	public void StartRefreshingClanOnline()
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		this._shouldStopRefrClanOnline = false;
		base.StartCoroutine(this.RefreshClanOnline());
	}

	public void StartRefreshingInfo(string playerId)
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		this._shouldStopRefreshingInfo = false;
		base.StartCoroutine(this.GetIfnoAboutPlayerLoop(playerId));
	}

	public void StartRefreshingOnline()
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		this._shouldStopOnline = false;
		base.StartCoroutine(this.RefreshOnlinePlayer());
	}

	public void StartRefreshingOnlineWithClanInfo()
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		this._shouldStopOnlineWithClanInfo = false;
		base.StartCoroutine(this.RefreshOnlinePlayerWithClanInfo());
	}

	public static void StartSendReview()
	{
		if (FriendsController.sharedController != null)
		{
			FriendsController.sharedController.StopCoroutine("WaitReviewAndSend");
			FriendsController.sharedController.StartCoroutine("WaitReviewAndSend");
		}
	}

	public void StopRefreshingClanOnline()
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		this._shouldStopRefrClanOnline = true;
	}

	public void StopRefreshingInfo()
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		this._shouldStopRefreshingInfo = true;
	}

	public void StopRefreshingOnline()
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		this._shouldStopOnline = true;
	}

	public void StopRefreshingOnlineWithClanInfo()
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		this._shouldStopOnlineWithClanInfo = true;
	}

	private void SyncClickJoinFriendsListWithListFriends()
	{
		if (this.clicksJoinByFriends.Count == 0)
		{
			return;
		}
		if (this.friends.Count == 0)
		{
			this.ClearListClickJoinFriends();
			return;
		}
		List<string> strs = new List<string>();
		foreach (KeyValuePair<string, string> clicksJoinByFriend in this.clicksJoinByFriends)
		{
			if (!this.playersInfo.ContainsKey(clicksJoinByFriend.Key))
			{
				strs.Add(clicksJoinByFriend.Key);
			}
		}
		if (strs.Count == 0)
		{
			return;
		}
		for (int i = 0; i < strs.Count; i++)
		{
			string item = strs[i];
			this.clicksJoinByFriends.Remove(item);
		}
		this.UpdateCachedClickJoinListValue();
	}

	[DebuggerHidden]
	public IEnumerator SynchRating(int rating)
	{
		FriendsController.u003cSynchRatingu003ec__Iterator4F variable = null;
		return variable;
	}

	public bool TryIncrementWinCountTimestamp()
	{
		if (!this._winCountTimestamp.HasValue)
		{
			return false;
		}
		string key = this._winCountTimestamp.Value.Key;
		KeyValuePair<string, int> value = this._winCountTimestamp.Value;
		this._winCountTimestamp = new KeyValuePair<string, int>?(new KeyValuePair<string, int>(key, value.Value + 1));
		return true;
	}

	private void TrySendEventHideBoxProcessFriendsData()
	{
		if (FriendsController.OnHideBoxProcessFriendsData == null)
		{
			return;
		}
		FriendsController.OnHideBoxProcessFriendsData();
	}

	private void TrySendEventShowBoxProcessFriendsData()
	{
		if (FriendsController.OnShowBoxProcessFriendsData == null)
		{
			return;
		}
		FriendsController.OnShowBoxProcessFriendsData();
	}

	public void UnbanUs(Action onSuccess)
	{
	}

	private void Update()
	{
		if (FriendsController.isUpdateServerTimeAfterRun)
		{
			FriendsController.tickForServerTime += Time.deltaTime;
			if (FriendsController.tickForServerTime >= 1f)
			{
				FriendsController.localServerTime += (long)1;
				FriendsController.tickForServerTime -= 1f;
			}
		}
		this.deltaTimeInGame += Time.deltaTime;
		if (this.Banned == 1 && PhotonNetwork.connected)
		{
			PhotonNetwork.Disconnect();
		}
		if (Input.touchCount <= 0)
		{
			this.lastTouchTm = Time.realtimeSinceStartup;
			this.idle = false;
		}
		else if (Time.realtimeSinceStartup - this.lastTouchTm > 30f)
		{
			this.idle = true;
		}
	}

	[DebuggerHidden]
	private IEnumerator UpdateAccessories()
	{
		FriendsController.u003cUpdateAccessoriesu003ec__Iterator55 variable = null;
		return variable;
	}

	private void UpdateCachedClickJoinListValue()
	{
		if (this.clicksJoinByFriends.Count == 0)
		{
			return;
		}
		PlayerPrefs.SetString("CachedFriendsJoinClickList", Json.Serialize(this.clicksJoinByFriends) ?? string.Empty);
	}

	[DebuggerHidden]
	private IEnumerator UpdateFriendsInfo(bool _isUpdateInfoAboutAllFriends)
	{
		FriendsController.u003cUpdateFriendsInfou003ec__Iterator6F variable = null;
		return variable;
	}

	public static void UpdatePixelbookSettingsFromPrefs()
	{
		string str = PlayerPrefs.GetString("PixelbookSettingsKey", "{}");
		Dictionary<string, object> strs = Json.Deserialize(str) as Dictionary<string, object>;
		if (strs == null)
		{
			return;
		}
		if (strs.ContainsKey("MaxFriendCount"))
		{
			if (strs.ContainsKey("FriendsUrl"))
			{
				URLs.Friends = Convert.ToString(strs["FriendsUrl"]);
			}
			if (strs.ContainsKey("MaxFriendCount"))
			{
				Defs.maxCountFriend = Convert.ToInt32(strs["MaxFriendCount"]);
			}
			if (strs.ContainsKey("MaxMemberClanCount"))
			{
				Defs.maxMemberClanCount = Convert.ToInt32(strs["MaxMemberClanCount"]);
			}
			if (strs.ContainsKey("TimeUpdateFriendInfo"))
			{
				Defs.timeUpdateFriendInfo = (float)Convert.ToInt32(strs["TimeUpdateFriendInfo"]);
			}
			if (strs.ContainsKey("TimeUpdateOnlineInGame"))
			{
				Defs.timeUpdateOnlineInGame = (float)Convert.ToInt32(strs["TimeUpdateOnlineInGame"]);
			}
			if (strs.ContainsKey("TimeUpdateInfoInProfile"))
			{
				Defs.timeUpdateInfoInProfile = (float)Convert.ToInt32(strs["TimeUpdateInfoInProfile"]);
			}
			if (strs.ContainsKey("TimeUpdateLeaderboardIfNullResponce"))
			{
				Defs.timeUpdateLeaderboardIfNullResponce = (float)Convert.ToInt32(strs["TimeUpdateLeaderboardIfNullResponce"]);
			}
			if (strs.ContainsKey("TimeBlockRefreshFriendDate"))
			{
				Defs.timeBlockRefreshFriendDate = (float)Convert.ToInt32(strs["TimeBlockRefreshFriendDate"]);
			}
			if (strs.ContainsKey("TimeWaitLoadPossibleFriends"))
			{
				Defs.timeWaitLoadPossibleFriends = (float)Convert.ToInt32(strs["TimeWaitLoadPossibleFriends"]);
			}
			if (strs.ContainsKey("PauseUpdateLeaderboard"))
			{
				Defs.pauseUpdateLeaderboard = (float)Convert.ToInt32(strs["PauseUpdateLeaderboard"]);
			}
			if (strs.ContainsKey("TimeUpdatePixelbookInfo"))
			{
				Defs.timeUpdatePixelbookInfo = (float)Convert.ToInt32(strs["TimeUpdatePixelbookInfo"]);
			}
			if (strs.ContainsKey("HistoryPrivateMessageLength"))
			{
				Defs.historyPrivateMessageLength = Convert.ToInt32(strs["HistoryPrivateMessageLength"]);
			}
			if (strs.ContainsKey("TimeUpdateStartCheckIfNullResponce"))
			{
				Defs.timeUpdateStartCheckIfNullResponce = (float)Convert.ToInt32(strs["TimeUpdateStartCheckIfNullResponce"]);
			}
			if (strs.ContainsKey("FacebookLimits"))
			{
				try
				{
					Dictionary<string, object> item = strs["FacebookLimits"] as Dictionary<string, object>;
					int num = (int)((long)item["GreenLimit"]);
					int item1 = (int)((long)item["RedLimit"]);
					Action<int, int> action = FriendsController.NewFacebookLimitsAvailable;
					if (action != null)
					{
						action(num, item1);
					}
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception);
				}
			}
			if (strs.ContainsKey("TwitterLimits"))
			{
				try
				{
					Dictionary<string, object> strs1 = strs["TwitterLimits"] as Dictionary<string, object>;
					int num1 = (int)((long)strs1["GreenLimit"]);
					int item2 = (int)((long)strs1["RedLimit"]);
					int num2 = (int)((long)strs1["MultyWinLimit"]);
					int item3 = (int)((long)strs1["ArenaLimit"]);
					Action<int, int, int, int> action1 = FriendsController.NewTwitterLimitsAvailable;
					if (action1 != null)
					{
						action1(num1, item2, num2, item3);
					}
				}
				catch (Exception exception1)
				{
					UnityEngine.Debug.LogException(exception1);
				}
			}
			if (strs.ContainsKey("CheaterDetectParameters"))
			{
				try
				{
					Dictionary<string, object> strs2 = strs["CheaterDetectParameters"] as Dictionary<string, object>;
					Dictionary<string, object> strs3 = strs2["Paying"] as Dictionary<string, object>;
					int num3 = (int)((long)strs3["Coins"]);
					int item4 = (int)((long)strs3["GemsCurrency"]);
					Dictionary<string, object> strs4 = strs2["NonPaying"] as Dictionary<string, object>;
					int num4 = (int)((long)strs4["Coins"]);
					int item5 = (int)((long)strs4["GemsCurrency"]);
					Action<int, int, int, int> action2 = FriendsController.NewCheaterDetectParametersAvailable;
					if (action2 != null)
					{
						action2(num3, item4, num4, item5);
					}
				}
				catch (Exception exception2)
				{
					UnityEngine.Debug.LogException(exception2);
				}
			}
			if (strs.ContainsKey("UseSqlLobby1031"))
			{
				Defs.useSqlLobby = (Convert.ToInt32(strs["UseSqlLobby1031"]) != 1 ? false : true);
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator UpdatePlayer(bool sendSkin)
	{
		FriendsController.u003cUpdatePlayeru003ec__Iterator56 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator UpdatePlayerInfoById(string playerId)
	{
		FriendsController.u003cUpdatePlayerInfoByIdu003ec__Iterator5F variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator UpdatePlayerOnline(int gameMode)
	{
		FriendsController.u003cUpdatePlayerOnlineu003ec__Iterator50 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator UpdatePlayerOnlineLoop()
	{
		FriendsController.u003cUpdatePlayerOnlineLoopu003ec__Iterator4D variable = null;
		return variable;
	}

	public void UpdatePLayersInfo()
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		base.StartCoroutine(this.GetInfoAboutNPlayers());
	}

	public void UpdatePopularityMaps()
	{
		base.StopCoroutine("GetPopularityMap");
		base.StartCoroutine("GetPopularityMap");
	}

	[DebuggerHidden]
	private IEnumerator UpdatePopularityMapsLoop()
	{
		FriendsController.u003cUpdatePopularityMapsLoopu003ec__Iterator37 variable = null;
		return variable;
	}

	public void UpdateRecordByFriendsJoinClick(string friendId)
	{
		if (this.clicksJoinByFriends.ContainsKey(friendId))
		{
			this.clicksJoinByFriends[friendId] = DateTime.UtcNow.ToString("s");
			return;
		}
		this.clicksJoinByFriends.Add(friendId, DateTime.UtcNow.ToString("s"));
	}

	[DebuggerHidden]
	public IEnumerator WaitForReadyToOperateAndUpdatePlayer()
	{
		FriendsController.u003cWaitForReadyToOperateAndUpdatePlayeru003ec__Iterator41 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator WaitReviewAndSend()
	{
		FriendsController.u003cWaitReviewAndSendu003ec__Iterator70 variable = null;
		return variable;
	}

	public static event Action ClanUpdated;

	public event Action FailedSendNewClan
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.FailedSendNewClan += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.FailedSendNewClan -= value;
		}
	}

	public static event Action FriendsUpdated;

	public static event Action FullInfoUpdated;

	public static event Action<int, int, int, int> NewCheaterDetectParametersAvailable;

	public static event Action<int, int> NewFacebookLimitsAvailable;

	public static event Action<int, int, int, int> NewTwitterLimitsAvailable;

	public static event Action OurInfoUpdated;

	public event Action<int> ReturnNewIDClan
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.ReturnNewIDClan += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.ReturnNewIDClan -= value;
		}
	}

	public static event Action ServerTimeUpdated;

	public static event Action StaticBankConfigUpdated;

	public enum NotConnectCondition
	{
		level,
		platform,
		map,
		clientVersion,
		InChat,
		None
	}

	public delegate void OnChangeClanName(string newName);

	public enum PossiblleOrigin
	{
		None,
		Local,
		Facebook,
		RandomPlayer
	}

	public class ResultParseOnlineData
	{
		public string mapIndex;

		public bool isPlayerInChat;

		public FriendsController.NotConnectCondition notConnectCondition;

		private string _gameRegim;

		private string _gameMode;

		public string gameMode
		{
			get
			{
				return this._gameMode;
			}
			set
			{
				this._gameMode = value;
				this._gameRegim = this._gameMode.Substring(this._gameMode.Length - 1);
			}
		}

		public bool IsCanConnect
		{
			get
			{
				return this.notConnectCondition == FriendsController.NotConnectCondition.None;
			}
		}

		public ResultParseOnlineData()
		{
		}

		public string GetGameModeName()
		{
			IDictionary<string, string> strs = ConnectSceneNGUIController.gameModesLocalizeKey;
			if (!strs.ContainsKey(this._gameRegim))
			{
				return string.Empty;
			}
			SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(this.mapIndex));
			if (infoScene != null && infoScene.IsAvaliableForMode(TypeModeGame.Dater))
			{
				return LocalizationStore.Get("Key_1567");
			}
			return LocalizationStore.Get(strs[this._gameRegim]);
		}

		public string GetMapName()
		{
			SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(this.mapIndex));
			if (infoScene == null)
			{
				return string.Empty;
			}
			return infoScene.TranslateName;
		}

		public string GetNotConnectConditionShortString()
		{
			if (this.IsCanConnect)
			{
				return string.Empty;
			}
			string empty = string.Empty;
			switch (this.notConnectCondition)
			{
				case FriendsController.NotConnectCondition.level:
				{
					empty = LocalizationStore.Get("Key_1574");
					break;
				}
				case FriendsController.NotConnectCondition.platform:
				{
					empty = LocalizationStore.Get("Key_1576");
					break;
				}
				case FriendsController.NotConnectCondition.map:
				{
					empty = LocalizationStore.Get("Key_1575");
					break;
				}
				case FriendsController.NotConnectCondition.clientVersion:
				{
					empty = LocalizationStore.Get("Key_1573");
					break;
				}
				case FriendsController.NotConnectCondition.InChat:
				{
					empty = LocalizationStore.Get("Key_1577");
					break;
				}
			}
			return empty;
		}

		public string GetNotConnectConditionString()
		{
			if (this.IsCanConnect)
			{
				return string.Empty;
			}
			string empty = string.Empty;
			switch (this.notConnectCondition)
			{
				case FriendsController.NotConnectCondition.level:
				{
					empty = LocalizationStore.Get("Key_1420");
					break;
				}
				case FriendsController.NotConnectCondition.platform:
				{
					empty = LocalizationStore.Get("Key_1414");
					break;
				}
				case FriendsController.NotConnectCondition.map:
				{
					empty = LocalizationStore.Get("Key_1419");
					break;
				}
				case FriendsController.NotConnectCondition.clientVersion:
				{
					empty = LocalizationStore.Get("Key_1418");
					break;
				}
			}
			return empty;
		}

		public OnlineState GetOnlineStatus()
		{
			int num = Convert.ToInt32(this._gameRegim);
			if (num == 6)
			{
				return OnlineState.inFriends;
			}
			if (num == 7)
			{
				return OnlineState.inClans;
			}
			return OnlineState.playing;
		}
	}

	public enum TypeTrafficForwardingLog
	{
		newView,
		view,
		click
	}
}