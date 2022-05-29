using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class Defs
{
	public const string NoviceArmorUsedKey = "Training.NoviceArmorUsedKey";

	public const string ShouldRemoveNoviceArmorInShopKey = "Training.ShouldRemoveNoviceArmorInShopKey";

	public const string GoogleSignInDeniedKey = "GoogleSignInDenied";

	public const string CampaignChooseBoxSceneName = "CampaignChooseBox";

	public const string KillRateKey = "KillRateKeyStatistics";

	public const string keyOldVersion = "keyOldVersion";

	public const string countSessionDayOnStartCorrentVersion = "countSessionDayOnStartCorrentVersion";

	public const string MoneyGivenRemovedArmorHat = "MoneyGivenRemovedArmorHat";

	public const string RemovedArmorHatMethodExecuted = "RemovedArmorHatMethodExecuted";

	public const string GrenadeID = "GrenadeID";

	public const string LikeID = "LikeID";

	public const string keyGameTotalKills = "keyGameTotalKills";

	public const string keyGameDeath = "keyGameDeath";

	public const string keyGameShoot = "keyGameShoot";

	public const string keyGameHit = "keyGameHit";

	public const string keyCountLikes = "keyCountLikes";

	public const string abTestBalansConfigKey = "abTestBalansConfigKey";

	public const string abTestSandBoxConfigKey = "abTestSandBoxConfigKey";

	public const string abTestSpecialOffersConfigKey = "abTestSpecialOffersConfigKey";

	public const string abTestQuestSystemConfigKey = "abTestQuestSystemConfigKey";

	public const string ratingSystemConfigKey = "rSCKey";

	public const string abTestBankViewKey = "abTestBankViewKey";

	public const string abTestBuffSystemKey = "abTestBuffSystemKey";

	public const string SniperModeWSSN = "WeaponManager.SniperModeWSSN";

	public const string KnifesModeWSSN = "WeaponManager.KnifesModeWSSN";

	public const string MaskEquippedSN = "MaskEquippedSN";

	public const string MaskNoneEquipped = "MaskNoneEquipped";

	public const string skin_july1 = "skin_july1";

	public const string skin_july2 = "skin_july2";

	public const string skin_july3 = "skin_july3";

	public const string skin_july4 = "skin_july4";

	public const string Coins = "Coins";

	public const string Gems = "GemsCurrency";

	public const string payingUserKey = "PayingUser";

	public const string LastPaymentTimeKey = "Last Payment Time";

	public const string LastPaymentTimeAdvertisementKey = "Last Payment Time (Advertisement)";

	public const string WinCountTimestampKey = "Win Count Timestamp";

	public const string WeaponPopularityKey = "Statistics.WeaponPopularity";

	public const string WeaponPopularityForTierKey = "Statistics.WeaponPopularityForTier";

	public const string WeaponPopularityTimestampKey = "Statistics.WeaponPopularityTimestamp";

	public const string WeaponPopularityForTierTimestampKey = "Statistics.WeaponPopularityForTierTimestamp";

	public const string ArmorPopularityKey = "Statistics.ArmorPopularity";

	public const string ArmorPopularityForTierKey = "Statistics.ArmorPopularityForTier";

	public const string ArmorPopularityForLevelKey = "Statistics.ArmorPopularityForLevel";

	public const string ArmorPopularityTimestampKey = "Statistics.ArmorPopularityTimestamp";

	public const string ArmorPopularityForTierTimestampKey = "Statistics.ArmorPopularityForTierTimestamp";

	public const string ArmorPopularityForLevelTimestampKey = "Statistics.ArmorPopularityForLevelTimestamp";

	public const string TimeInModeKeyPrefix = "Statistics.TimeInMode.Level";

	public const string RoundsInModeKeyPrefix = "Statistics.RoundsInMode.Level";

	public const string ExpInModeKeyPrefix = "Statistics.ExpInMode.Level";

	public const string WantToResetKeychain = "WantToResetKeychain";

	public const string StartTimeStarterPack = "StartTimeShowStarterPack";

	public const string EndTimeStarterPack = "TimeEndStarterPack";

	public const string NextNumberStarterPack = "NextNumberStarterPack";

	public const string LastTimeShowStarterPack = "LastTimeShowStarterPack";

	public const string CountShownStarterPack = "CountShownStarterPack";

	public const string CountShownGunForLogin = "FacebookController.CountShownGunForLogin";

	public const string PendingGooglePlayGamesSync = "PendingGooglePlayGamesSync";

	public const int EVENT_X3_SHOW_COUNT = 3;

	public const string FirstLaunchAdvertisementKey = "First Launch (Advertisement)";

	public const string IsLeaderboardsOpened = "Leaderboards.opened";

	public const string DaysOfValorShownCount = "DaysOfValorShownCount";

	public const string LastTimeShowDaysOfValor = "LastTimeShowDaysOfValor";

	public const string StartTimePremiumAccount = "StartTimePremiumAccount";

	public const string EndTimePremiumAccount = "EndTimePremiumAccount";

	public const string BuyHistoryPremiumAccount = "BuyHistoryPremiumAccount";

	public const string LastLoggedTimePremiumAccount = "LastLoggedTimePremiumAccount";

	public const string CachedFriendsJoinClickList = "CachedFriendsJoinClickList";

	public const int ConnectFacebookGemsReward = 10;

	public const int ConnectTwitterGemsReward = 10;

	public const string LockedSkinId = "61";

	public const string LockedSkinName = "super_socialman";

	public const float BotHpBarShowTime = 2f;

	public static string LastSendKillRateTimeKey;

	public static string StrongDeviceDev;

	public static string TrafficForwardingShowAnalyticsSent;

	public static string DateOfInstallAppForInAppPurchases041215;

	public static string FirstInAppPurchaseDate_041215;

	public static string SecondInAppPurchaseDate_041215;

	public static int CoinsForTraining;

	public static int GemsForTraining;

	public static int ExpForTraining;

	public static string GemsGivenRemovedGear;

	public static string LastTimeShowSocialGun;

	public static string ShownRewardWindowForCape;

	public static string ShownRewardWindowForSkin;

	public static string DaterWSSN;

	public static string SmileMessageSuffix;

	public static string IsFacebookLoginRewardaGained;

	public static string FacebookRewardGainStarted;

	public static string IsTwitterLoginRewardaGained;

	public static string TwitterRewardGainStarted;

	public static bool ResetTrainingInDevBuild;

	public static bool useSqlLobby;

	public static string keyTestCountGetGift;

	public static string BuyGiftKey;

	public static bool isTouchControlSmoothDump;

	public readonly static string initValsInKeychain43;

	public readonly static string initValsInKeychain44;

	public readonly static string initValsInKeychain45;

	public readonly static string initValsInKeychain46;

	public static bool isMouseControl;

	public static bool isRegimVidosDebug;

	private static bool _isActivABTestRatingSystem;

	private static bool _isInitActivABTestRatingSystem;

	private static string nonActivABTestRatingSystemKey;

	private static bool _isActivABTestStaticBank;

	private static bool _isInitActivABTestStaticBank;

	private static string nonActivABTestStaticBankKey;

	private static bool _isActivABTestBuffSystem;

	private static bool _isInitActivABTestBuffSystem;

	private static string nonActivABTestBuffSystemKey;

	private static Defs.ABTestCohortsType _cohortABTestSandBox;

	private static bool _isInitCohortABTestSandBox;

	private static string cohortABTestSandBoxKey;

	private static Defs.ABTestCohortsType _cohortABTestQuestSystem;

	private static bool _isInitCohortABTestQuestSystem;

	private static string cohortABTestQuestSystemKey;

	private static Defs.ABTestCohortsType _cohortABTestSpecialOffers;

	private static bool _isInitCohortABTestSpecialOffers;

	private static string cohortABTestSpecialOffersKey;

	public readonly static string MoneyGiven831to901;

	public static string GotCoinsForTraining;

	public static Defs.DisconectGameType typeDisconnectGame;

	public static Defs.GameSecondFireButtonMode gameSecondFireButtonMode;

	public static int ZoomButtonX;

	public static int ZoomButtonY;

	public static int ReloadButtonX;

	public static int ReloadButtonY;

	public static int JumpButtonX;

	public static int JumpButtonY;

	public static int FireButtonX;

	public static int FireButtonY;

	public static int JoyStickX;

	public static int JoyStickY;

	public static int GrenadeX;

	public static int GrenadeY;

	public static int FireButton2X;

	public static int FireButton2Y;

	public static string VisualHatArmor;

	public static string VisualArmor;

	public static string RatingDeathmatch;

	public static string RatingTeamBattle;

	public static string RatingHunger;

	public static string RatingFlag;

	public static string RatingCapturePoint;

	public static bool isDaterRegim;

	public static int LogoWidth;

	public static int LogoHeight;

	public static string[] SurvivalMaps;

	public static int CurrentSurvMapIndex;

	public static float FreezerSlowdownTime;

	private static bool _initializedJoystickParams;

	private static bool _isJumpAndShootButtonOn;

	private static bool _isInitJumpAndShootButtonOn;

	public static bool isShowUserAgrement;

	public static int maxCountFriend;

	public static int maxMemberClanCount;

	public static float timeUpdateFriendInfo;

	public static float timeUpdateOnlineInGame;

	public static float timeUpdateInfoInProfile;

	public static float timeBlockRefreshFriendDate;

	public static float timeUpdateLeaderboardIfNullResponce;

	public static float timeUpdateStartCheckIfNullResponce;

	public static float timeWaitLoadPossibleFriends;

	public static float pauseUpdateLeaderboard;

	public static float timeUpdatePixelbookInfo;

	public static float timeUpdateNews;

	public static int historyPrivateMessageLength;

	public static float timeUpdateServerTime;

	public static Defs.ABTestCohortsType abTestBalansCohort;

	private static bool? _isAbTestBalansCohortActual;

	public static string abTestBalansCohortName;

	public static int abTestBalansStartCapitalCoins;

	public static int abTestBalansStartCapitalGems;

	public static string bigPorogString;

	public static string smallPorogString;

	public static string friendsSceneName;

	public static int ammoInGamePanelPrice;

	public static int healthInGamePanelPrice;

	public static int ClansPrice;

	public static int ProfileFromFriends;

	public static string ServerIp;

	public static bool isMulti;

	public static bool isInet;

	public static bool isCOOP;

	public static bool isCompany;

	public static bool isFlag;

	public static bool isHunger;

	public static bool isGameFromFriends;

	public static bool isGameFromClans;

	public static bool isCapturePoints;

	public readonly static string PixelGunAppID;

	public readonly static string AppStoreURL;

	public readonly static string SupportMail;

	public static bool EnderManAvailable;

	public static bool isSoundMusic;

	public static bool isSoundFX;

	public static float BottomOffs;

	public static Dictionary<string, int> filterMaps;

	private readonly static Dictionary<string, int> _premiumMaps;

	public static int NumberOfElixirs;

	public static bool isGrenateFireEnable;

	public static bool isZooming;

	public static bool isJetpackEnabled;

	public static float GoToProfileShopInterval;

	public readonly static string InvertCamSN;

	public static List<GameObject> players;

	public static string PromSceneName;

	public static string _3_shotgun_2;

	public static string _3_shotgun_3;

	public static string flower_2;

	public static string flower_3;

	public static string gravity_2;

	public static string gravity_3;

	public static string grenade_launcher_3;

	public static string revolver_2_2;

	public static string revolver_2_3;

	public static string scythe_3;

	public static string plazma_2;

	public static string plazma_3;

	public static string plazma_pistol_2;

	public static string plazma_pistol_3;

	public static string railgun_2;

	public static string railgun_3;

	public static string Razer_3;

	public static string tesla_3;

	public static string Flamethrower_3;

	public static string FreezeGun_0;

	public static string svd_3;

	public static string barret_3;

	public static string minigun_3;

	public static string LightSword_3;

	public static string Sword_2_3;

	public static string Staff_3;

	public static string DragonGun;

	public static string Bow_3;

	public static string Bazooka_1_3;

	public static string Bazooka_2_1;

	public static string Bazooka_2_3;

	public static string m79_2;

	public static string m79_3;

	public static string m32_1_2;

	public static string Red_Stone_3;

	public static string XM8_1;

	public static string PumpkinGun_1;

	public static string XM8_2;

	public static string XM8_3;

	public static string PumpkinGun_2;

	public static string PumpkinGun_5;

	public static string[] Skins_11_040915;

	public static string skin_tiger;

	public static string skin_pitbull;

	public static string skin_santa;

	public static string skin_elf_new_year;

	public static string skin_girl_new_year;

	public static string skin_cookie_new_year;

	public static string skin_snowman_new_year;

	public static string skin_jetti_hnight;

	public static string skin_startrooper;

	public static string easter_skin1;

	public static string easter_skin2;

	public static string skin_rapid_girl;

	public static string skin_silent_killer;

	public static string skin_daemon_fighter;

	public static string skin_scary_demon;

	public static string skin_orc_warrior;

	public static string skin_kung_fu_master;

	public static string skin_fire_wizard;

	public static string skin_ice_wizard;

	public static string skin_storm_wizard;

	private static int _isUse3DTouch;

	public readonly static string Weapons800to801;

	public readonly static string Weapons831to901;

	public readonly static string Update_AddSniperCateogryKey;

	public readonly static string FixWeapons911;

	public readonly static string ReturnAlienGun930;

	public static int diffGame;

	public static bool IsSurvival;

	public static string StartTimeShowBannersString;

	private static int _countReturnInConnectScene;

	private static bool _countReturnInConnectSceneInit;

	public static bool showTableInNetworkStartTable;

	public static bool showNickTableInNetworkStartTable;

	public static bool isTurretWeapon;

	private static float? _sensitivity;

	private static bool? _isChatOn;

	public static int inComingMessagesCounter;

	public static HashSet<string> unimportantRPCList;

	public static bool inRespawnWindow;

	public readonly static string IsFirstLaunchFreshInstall;

	public readonly static string NewbieEventX3StartTime;

	public readonly static string NewbieEventX3StartTimeAdditional;

	public readonly static string NewbieEventX3LastLoggedTime;

	public readonly static string WasNewbieEventX3;

	public static string _3PLShotgunSN
	{
		get
		{
			return "_3PLShotgunSN";
		}
	}

	public static string AdvertWindowShownCount
	{
		get
		{
			return "AdvertWindowShownCount";
		}
	}

	public static string AdvertWindowShownLastTime
	{
		get
		{
			return "AdvertWindowShownLastTime";
		}
	}

	public static string AK74_2_SN
	{
		get
		{
			return "AK74_2_SN";
		}
	}

	public static string AK74_3_SN
	{
		get
		{
			return "AK74_3_SN";
		}
	}

	public static string AK74_SN
	{
		get
		{
			return "AK74_SN";
		}
	}

	public static string AllCurrencyBought
	{
		get
		{
			return "AllCurrencyBought";
		}
	}

	public static string AmmoBoughtSN
	{
		get
		{
			return "AmmoBoughtSN";
		}
	}

	public static Defs.RuntimeAndroidEdition AndroidEdition
	{
		get
		{
			return Defs.RuntimeAndroidEdition.GoogleLite;
		}
	}

	public static string animeGirlSett
	{
		get
		{
			return "animeGirlSett";
		}
	}

	public static string ArmorEquppedSN
	{
		get
		{
			return "ArmorEquppedSN";
		}
	}

	public static string ArmorNewEquppedSN
	{
		get
		{
			return "ArmorNewEquppedSN";
		}
	}

	public static string ArmorNewNoneEqupped
	{
		get
		{
			return "__no_armor_NEW";
		}
	}

	public static string ArmorNoneEqupped
	{
		get
		{
			return "__no_armor";
		}
	}

	public static string ArtBoxS
	{
		get
		{
			return "ArtBoxS";
		}
	}

	public static string ArtLevsS
	{
		get
		{
			return "ArtLevsS";
		}
	}

	public static string AUG_2SN
	{
		get
		{
			return "AUG_2SN";
		}
	}

	public static string AUGSN
	{
		get
		{
			return "AUGSett";
		}
	}

	public static string Barrett2SN
	{
		get
		{
			return "Barrett2SN";
		}
	}

	public static string BarrettSN
	{
		get
		{
			return "BarrettSN";
		}
	}

	public static string BassCannonSN
	{
		get
		{
			return "BassCannonSN";
		}
	}

	public static string Bazooka_2SN
	{
		get
		{
			return "Bazooka_2SN";
		}
	}

	public static string Bazooka_3SN
	{
		get
		{
			return "Bazooka_3SN";
		}
	}

	public static string BazookaSN
	{
		get
		{
			return "BazookaSN";
		}
	}

	public static string Beretta_2_SN
	{
		get
		{
			return "Beretta_2_SN";
		}
	}

	public static string BerettaSN
	{
		get
		{
			return "BerettaSN";
		}
	}

	public static string BestScoreSett
	{
		get
		{
			return "BestScoreSett";
		}
	}

	public static string BlackEagleSN
	{
		get
		{
			return "BlackEagleSN";
		}
	}

	public static string BootsDir
	{
		get
		{
			return "Boots";
		}
	}

	public static string BootsEquppedSN
	{
		get
		{
			return "BootsEquppedSN";
		}
	}

	public static string BootsNoneEqupped
	{
		get
		{
			return "boots_NoneEquipped";
		}
	}

	public static string braveGirlSett
	{
		get
		{
			return "braveGirlSett";
		}
	}

	public static string BuddySN
	{
		get
		{
			return "BuddySN";
		}
	}

	public static string CampaignWSSN
	{
		get
		{
			return "CampaignWSSN";
		}
	}

	public static string CancelButtonTitle
	{
		get
		{
			return "Cancel";
		}
	}

	public static string CapeEquppedSN
	{
		get
		{
			return "CapeEquppedSN";
		}
	}

	public static string CapeNoneEqupped
	{
		get
		{
			return "cape_NoneEquipped";
		}
	}

	public static string CapesDir
	{
		get
		{
			return "Capes";
		}
	}

	public static string captainSett
	{
		get
		{
			return "captainSett";
		}
	}

	public static int CaptureFlagPrice
	{
		get
		{
			return 100;
		}
	}

	public static string CaptureFlagPurchasedKey
	{
		get
		{
			return "CaptureFlagGamesPuchased";
		}
	}

	public static string Chainsaw2SN
	{
		get
		{
			return "Chainsaw2SN";
		}
	}

	public static string ChainsawS
	{
		get
		{
			return "ChainsawS";
		}
	}

	public static string ChangeOldLanguageName
	{
		get
		{
			return "ChangeOldLanguageName";
		}
	}

	public static string CherryGunSN
	{
		get
		{
			return "CherryGunSN";
		}
	}

	public static string chiefBoughtSett
	{
		get
		{
			return "chiefBoughtSett";
		}
	}

	public static string code010110_Key
	{
		get
		{
			return "MatrixKey";
		}
	}

	public static float Coef
	{
		get
		{
			return (float)Screen.height / 768f;
		}
	}

	public static Defs.ABTestCohortsType cohortABTestQuestSystem
	{
		get
		{
			if (!Defs._isInitCohortABTestQuestSystem)
			{
				Defs._cohortABTestQuestSystem = (Defs.ABTestCohortsType)PlayerPrefs.GetInt(Defs.cohortABTestQuestSystemKey, 0);
				Defs._isInitCohortABTestQuestSystem = true;
			}
			return Defs._cohortABTestQuestSystem;
		}
		set
		{
			Defs._cohortABTestQuestSystem = value;
			Defs._isInitCohortABTestQuestSystem = true;
			PlayerPrefs.SetInt(Defs.cohortABTestQuestSystemKey, (int)Defs._cohortABTestQuestSystem);
		}
	}

	public static Defs.ABTestCohortsType cohortABTestSandBox
	{
		get
		{
			if (!Defs._isInitCohortABTestSandBox)
			{
				Defs._cohortABTestSandBox = (Defs.ABTestCohortsType)PlayerPrefs.GetInt(Defs.cohortABTestSandBoxKey, 0);
				Defs._isInitCohortABTestSandBox = true;
			}
			return Defs._cohortABTestSandBox;
		}
		set
		{
			Defs._cohortABTestSandBox = value;
			Defs._isInitCohortABTestSandBox = true;
			PlayerPrefs.SetInt(Defs.cohortABTestSandBoxKey, (int)Defs._cohortABTestSandBox);
		}
	}

	public static Defs.ABTestCohortsType cohortABTestSpecialOffers
	{
		get
		{
			if (!Defs._isInitCohortABTestSpecialOffers)
			{
				Defs._cohortABTestSpecialOffers = (Defs.ABTestCohortsType)PlayerPrefs.GetInt(Defs.cohortABTestSpecialOffersKey, 0);
				Defs._isInitCohortABTestSpecialOffers = true;
			}
			return Defs._cohortABTestSpecialOffers;
		}
		set
		{
			Defs._cohortABTestSpecialOffers = value;
			Defs._isInitCohortABTestSpecialOffers = true;
			PlayerPrefs.SetInt(Defs.cohortABTestSpecialOffersKey, (int)Defs._cohortABTestSpecialOffers);
		}
	}

	public static string CombatRifleSett
	{
		get
		{
			return "CombatRifleSett";
		}
	}

	public static string COOPScore
	{
		get
		{
			return "COOPScore";
		}
	}

	public static int countReturnInConnectScene
	{
		get
		{
			if (!Defs._countReturnInConnectSceneInit)
			{
				Defs._countReturnInConnectScene = PlayerPrefs.GetInt("countReturnInConnectScene", 0);
				Defs._countReturnInConnectSceneInit = true;
			}
			return Defs._countReturnInConnectScene;
		}
		set
		{
			Defs._countReturnInConnectScene = value;
			PlayerPrefs.SetInt("countReturnInConnectScene", Defs._countReturnInConnectScene);
			Defs._countReturnInConnectSceneInit = true;
		}
	}

	public static string CrossbowSN
	{
		get
		{
			return "CrossbowSN";
		}
	}

	public static string CrystakPickSN
	{
		get
		{
			return "CrystakPickSN";
		}
	}

	public static string CrystalAxeSN
	{
		get
		{
			return "CrystalAxeSN";
		}
	}

	public static string CrystalCrossbowSN
	{
		get
		{
			return "CrystalCrossbowSN";
		}
	}

	public static string CrystalGlockSN
	{
		get
		{
			return "CrystalGlockSN";
		}
	}

	public static string CrystalSPASSN
	{
		get
		{
			return "CrystalSPASSN";
		}
	}

	public static string CurrentLanguage
	{
		get
		{
			return "CurrentLanguage";
		}
	}

	public static string CurrentWeaponSett
	{
		get
		{
			return "CurrentWeapon";
		}
	}

	public static int CustomCapeTextureHeight
	{
		get
		{
			return 16;
		}
	}

	public static int CustomCapeTextureWidth
	{
		get
		{
			return 12;
		}
	}

	public static string CustomTextureName
	{
		get
		{
			return "cape_CustomTexture";
		}
	}

	public static float DiffModif
	{
		get
		{
			float single = 0.6f;
			int num = Defs.diffGame;
			if (num == 1)
			{
				single = 0.8f;
			}
			else if (num == 2)
			{
				single = 1f;
			}
			return single;
		}
	}

	public static string DiffSett
	{
		get
		{
			return "DifficultySett";
		}
	}

	public static string Eagle_3SN
	{
		get
		{
			return "Eagle_3SN";
		}
	}

	public static string EarnedCoins
	{
		get
		{
			return "EarnedCoins";
		}
	}

	public static string emoGirlSett
	{
		get
		{
			return "emoGirlSett";
		}
	}

	public static string endmanskinBoughtSett
	{
		get
		{
			return "endmanskinBoughtSett";
		}
	}

	public static string EventX3WindowShownCount
	{
		get
		{
			return "EventX3WindowShownCount";
		}
	}

	public static string EventX3WindowShownLastTime
	{
		get
		{
			return "EventX3WindowShownLastTime";
		}
	}

	public static string FAMASS
	{
		get
		{
			return "FAMASS";
		}
	}

	public static string famosBoySett
	{
		get
		{
			return "famosBoySett";
		}
	}

	public static string FireAxeSN
	{
		get
		{
			return "FireAxeSN";
		}
	}

	public static string FirstLaunch
	{
		get
		{
			return "FirstLaunch";
		}
	}

	public static string FlameThrower_2SN
	{
		get
		{
			return "FlameThrower_2SN";
		}
	}

	public static string FlameThrowerSN
	{
		get
		{
			return "FlameThrowerSN";
		}
	}

	public static string FlowePowerSN
	{
		get
		{
			return "FlowePowerSN";
		}
	}

	public static string FreezeGun_SN
	{
		get
		{
			return "FreezeGun_SN";
		}
	}

	public static string GameGUIOffMode
	{
		get
		{
			return "GameGUIOffMode";
		}
	}

	public static string GameModesEventKey
	{
		get
		{
			return "Game Modes";
		}
	}

	public static string glamGirlSett
	{
		get
		{
			return "glamGirlSett";
		}
	}

	public static string GlockSett
	{
		get
		{
			return "GlockSett";
		}
	}

	public static string GoldenAxeSett
	{
		get
		{
			return "GoldenAxeSett";
		}
	}

	public static string GoldenEagleSett
	{
		get
		{
			return "GoldenEagleSett";
		}
	}

	public static string GoldenGlockSN
	{
		get
		{
			return "GoldenGlockSN";
		}
	}

	public static string GoldenPickSN
	{
		get
		{
			return "GoldenPickSN";
		}
	}

	public static string GoldenRed_StoneSN
	{
		get
		{
			return "GoldenRed_StoneSN";
		}
	}

	public static string GoldenSPASSN
	{
		get
		{
			return "GoldenSPASSN";
		}
	}

	public static string GoldenSwordSN
	{
		get
		{
			return "GoldenSwordSN";
		}
	}

	public static string gordonSett
	{
		get
		{
			return "gordonSett";
		}
	}

	public static string GoToPresetsAction
	{
		get
		{
			return "GoToPresetsAction";
		}
	}

	public static string GoToProfileAction
	{
		get
		{
			return "GoToProfileAction";
		}
	}

	public static string GoToSkinsMakerAction
	{
		get
		{
			return "GoToSkinsMakerAction";
		}
	}

	public static string GravigunSN
	{
		get
		{
			return "GravigunSN";
		}
	}

	public static string greenGuySett
	{
		get
		{
			return "greenGuySett";
		}
	}

	public static string GrenadeLnch_2SN
	{
		get
		{
			return "GrenadeLnch_2SN";
		}
	}

	public static string GrenadeLnchSN
	{
		get
		{
			return "GrenadeLnchSN";
		}
	}

	public static float HalfLength
	{
		get
		{
			return 17f;
		}
	}

	public static string Hammer2SN
	{
		get
		{
			return "Hammer2SN";
		}
	}

	public static string HammerSN
	{
		get
		{
			return "HammerSN";
		}
	}

	public static string HatEquppedSN
	{
		get
		{
			return "HatEquppedSN";
		}
	}

	public static string HatNoneEqupped
	{
		get
		{
			return "hat_NoneEquipped";
		}
	}

	public static string HatsDir
	{
		get
		{
			return "Hats";
		}
	}

	public static string hawkSett
	{
		get
		{
			return "hawkSett";
		}
	}

	public static string HockeyAppID
	{
		get
		{
			return "2d830f37b5a8daaef2b7ada172fc767d";
		}
	}

	public static int HoursToEndX3ForIndicate
	{
		get
		{
			return 6;
		}
	}

	public static int HungerGamesPrice
	{
		get
		{
			return 75;
		}
	}

	public static string hungerGamesPurchasedKey
	{
		get
		{
			return "HungerGamesPuchased";
		}
	}

	public static string InAppBoughtSett
	{
		get
		{
			return "BigAmmoPackBought";
		}
	}

	public static string inappsRestored_3_1
	{
		get
		{
			return "inappsRestored_3_1";
		}
	}

	public static string InitialAppVersionKey
	{
		get
		{
			return "InitialAppVersion";
		}
	}

	public static string initValsInKeychain15
	{
		get
		{
			return "_initValsInKeychain15_";
		}
	}

	public static string initValsInKeychain17
	{
		get
		{
			return "initValsInKeychain17";
		}
	}

	public static string initValsInKeychain27
	{
		get
		{
			return "initValsInKeychain27";
		}
	}

	public static string initValsInKeychain40
	{
		get
		{
			return "initValsInKeychain40";
		}
	}

	public static string initValsInKeychain41
	{
		get
		{
			return "initValsInKeychain41";
		}
	}

	public static string InnerWeapons_Suffix
	{
		get
		{
			return "_Inner";
		}
	}

	public static string InnerWeaponsFolder
	{
		get
		{
			return "WeaponSystem/Inner";
		}
	}

	public static string IronSwordSN
	{
		get
		{
			return "IronSwordSN";
		}
	}

	public static bool isABTestBalansCohortActual
	{
		get
		{
			if (Defs._isAbTestBalansCohortActual.HasValue)
			{
				bool? nullable = Defs._isAbTestBalansCohortActual;
				return ((!nullable.GetValueOrDefault() ? true : !nullable.HasValue) ? false : true);
			}
			Defs._isAbTestBalansCohortActual = new bool?(PlayerPrefs.GetInt("ABTCA", 0) == 1);
			bool? nullable1 = Defs._isAbTestBalansCohortActual;
			return ((!nullable1.GetValueOrDefault() ? true : !nullable1.HasValue) ? false : true);
		}
		set
		{
			Defs._isAbTestBalansCohortActual = new bool?(value);
			bool? nullable = Defs._isAbTestBalansCohortActual;
			PlayerPrefs.SetInt("ABTCA", ((!nullable.GetValueOrDefault() ? true : !nullable.HasValue) ? 0 : 1));
		}
	}

	public static bool isABTestBalansNoneSkip
	{
		get
		{
			return PlayerPrefs.GetInt("NoneSkipABTestBalans", 0) == 1;
		}
		set
		{
			PlayerPrefs.SetInt("NoneSkipABTestBalans", (!value ? 0 : 1));
		}
	}

	public static bool isActivABTestBuffSystem
	{
		get
		{
			if (!Defs._isInitActivABTestBuffSystem)
			{
				Defs._isActivABTestBuffSystem = PlayerPrefs.GetInt(Defs.nonActivABTestBuffSystemKey, 1) == 0;
				Defs._isInitActivABTestBuffSystem = true;
			}
			return Defs._isActivABTestBuffSystem;
		}
		set
		{
			Defs._isActivABTestBuffSystem = value;
			Defs._isInitActivABTestBuffSystem = true;
			PlayerPrefs.SetInt(Defs.nonActivABTestBuffSystemKey, (!value ? 1 : 0));
		}
	}

	public static bool isActivABTestRatingSystem
	{
		get
		{
			if (!Defs._isInitActivABTestRatingSystem)
			{
				Defs._isActivABTestRatingSystem = PlayerPrefs.GetInt(Defs.nonActivABTestRatingSystemKey, 1) == 0;
				Defs._isInitActivABTestRatingSystem = true;
			}
			return Defs._isActivABTestRatingSystem;
		}
		set
		{
			Defs._isActivABTestRatingSystem = value;
			Defs._isInitActivABTestRatingSystem = true;
			PlayerPrefs.SetInt(Defs.nonActivABTestRatingSystemKey, (!value ? 1 : 0));
		}
	}

	public static bool isActivABTestStaticBank
	{
		get
		{
			if (!Defs._isInitActivABTestStaticBank)
			{
				Defs._isActivABTestStaticBank = PlayerPrefs.GetInt(Defs.nonActivABTestStaticBankKey, 0) == 0;
				Defs._isInitActivABTestStaticBank = true;
			}
			return Defs._isActivABTestStaticBank;
		}
		set
		{
			Defs._isActivABTestStaticBank = value;
			Defs._isInitActivABTestStaticBank = true;
			PlayerPrefs.SetInt(Defs.nonActivABTestStaticBankKey, (!value ? 1 : 0));
		}
	}

	public static bool IsChatOn
	{
		get
		{
			if (!Defs._isChatOn.HasValue)
			{
				Defs._isChatOn = new bool?(PlayerPrefs.GetInt("ChatOn", 1) == 1);
			}
			return Defs._isChatOn.Value;
		}
		set
		{
			Defs._isChatOn = new bool?(value);
			PlayerPrefs.SetInt("ChatOn", (!value ? 0 : 1));
		}
	}

	public static bool IsDeveloperBuild
	{
		get
		{
			return false;
		}
	}

	public static bool isJumpAndShootButtonOn
	{
		get
		{
			if (!Defs._isInitJumpAndShootButtonOn)
			{
				Defs._isJumpAndShootButtonOn = (!Defs.touchPressureSupported ? true : PlayerPrefs.GetInt("isJumpAndShootButtonOn", 1) == 1);
				Defs._isInitJumpAndShootButtonOn = true;
			}
			return Defs._isJumpAndShootButtonOn;
		}
		set
		{
			Defs._isJumpAndShootButtonOn = value;
			Defs._isInitJumpAndShootButtonOn = true;
			PlayerPrefs.SetInt("isJumpAndShootButtonOn", (!value ? 0 : 1));
		}
	}

	public static bool isUse3DTouch
	{
		get
		{
			if (Defs._isUse3DTouch < 0)
			{
				Defs._isUse3DTouch = PlayerPrefs.GetInt("Use3dTOUCH", 1);
			}
			return Defs._isUse3DTouch == 1;
		}
		set
		{
			Defs._isUse3DTouch = (!value ? 0 : 1);
			PlayerPrefs.SetInt("Use3dTOUCH", Defs._isUse3DTouch);
		}
	}

	public static string katana_2_SN
	{
		get
		{
			return "katana_2_SN";
		}
	}

	public static string katana_3_SN
	{
		get
		{
			return "katana_3_SN";
		}
	}

	public static string katana_SN
	{
		get
		{
			return "katana_SN";
		}
	}

	public static string KilledZombiesMaxSett
	{
		get
		{
			return "KilledZombiesMaxSett";
		}
	}

	public static string KilledZombiesSett
	{
		get
		{
			return "KilledZombiesSett";
		}
	}

	public static string kityyGirlSett
	{
		get
		{
			return "kityyGirlSett";
		}
	}

	public static string LaserRifleSN
	{
		get
		{
			return "LaserRifleSN";
		}
	}

	public static string LastTimeSessionDayKey
	{
		get
		{
			return "LastTimeSessionDay";
		}
	}

	public static string LastTimeShowBanerKey
	{
		get
		{
			return "LastTimeShowBanerKey";
		}
	}

	public static string LastTimeTempItemsSuspended
	{
		get
		{
			return "LastTimeTempItemsSuspended";
		}
	}

	public static string LastTimeUpdateAvailableShownSN
	{
		get
		{
			return "LastTimeUpdateAvailableShownSN";
		}
	}

	public static string LeftHandedSN
	{
		get
		{
			return "LeftHandedSN";
		}
	}

	public static string LevelsWhereGetCoinS
	{
		get
		{
			return "LevelsWhereGetCoinS";
		}
	}

	public static string LevelsWhereGotGems
	{
		get
		{
			return "Bonus.LevelsWhereGotGems";
		}
	}

	public static string LightSwordSN
	{
		get
		{
			return "LightSwordSN";
		}
	}

	public static string LobbyLevelApplied
	{
		get
		{
			return "LobbyLevelApplied";
		}
	}

	public static string m_16_3_Sett
	{
		get
		{
			return "m_16_3_Sett";
		}
	}

	public static string m_16_4_Sett
	{
		get
		{
			return "m_16_4_Sett";
		}
	}

	public static string Mace2SN
	{
		get
		{
			return "Mace2SN";
		}
	}

	public static string MaceSN
	{
		get
		{
			return "MaceSN";
		}
	}

	public static string MagicBowSett
	{
		get
		{
			return "MagicBowSett";
		}
	}

	public static string magicGirlSett
	{
		get
		{
			return "magicGirlSett";
		}
	}

	public static string MainMenuScene
	{
		get
		{
			return "Menu_Heaven";
		}
	}

	public static string MarathonTestMode
	{
		get
		{
			return "MarathonTestMode";
		}
	}

	public static string MauserSN
	{
		get
		{
			return "MauserSN";
		}
	}

	public static string MenuPersWeaponTag
	{
		get
		{
			return "MenuPersWeaponTag";
		}
	}

	public static string MinerWeaponSett
	{
		get
		{
			return "MinerWeaponSett";
		}
	}

	public static string MinigunSN
	{
		get
		{
			return "MinigunSN";
		}
	}

	public static string MostExpensiveWeapon
	{
		get
		{
			return "MostExpensiveWeapon";
		}
	}

	public static string MultiplayerModesKey
	{
		get
		{
			return "Multiplayer Modes";
		}
	}

	public static string MultiplayerWSSN
	{
		get
		{
			return "MultiplayerWSSN";
		}
	}

	public static string MultSkinsDirectoryName
	{
		get
		{
			return "Multiplayer Skins";
		}
	}

	public static string nanosoldierBoughtSett
	{
		get
		{
			return "nanosoldierBoughtSett";
		}
	}

	public static string NavyFamasSN
	{
		get
		{
			return "NavyFamasSN";
		}
	}

	public static string NeedTakeMarathonBonus
	{
		get
		{
			return "NeedTakeMarathonBonus";
		}
	}

	public static string NextMarafonBonusIndex
	{
		get
		{
			return "NextMarafonBonusIndex";
		}
	}

	public static string NumberOfElixirsSett
	{
		get
		{
			return "NumberOfElixirsSett";
		}
	}

	public static string NumOfMultSkinsSett
	{
		get
		{
			return "NumOfMultSkinsSett";
		}
	}

	public static string nurseSett
	{
		get
		{
			return "nurseSett";
		}
	}

	public static string plazma_pistol_SN
	{
		get
		{
			return "plazma_pistol_SN";
		}
	}

	public static string plazmaSN
	{
		get
		{
			return "plazmaSN";
		}
	}

	public static string PremiumEnabledFromServer
	{
		get
		{
			return "PremiumEnabledFromServer";
		}
	}

	public static IDictionary<string, int> PremiumMaps
	{
		get
		{
			return Defs._premiumMaps;
		}
	}

	public static string ProfileEnteredFromMenu
	{
		get
		{
			return "ProfileEnteredFromMenu";
		}
	}

	public static string RailgunSN
	{
		get
		{
			return "RailgunSN";
		}
	}

	public static string RankParameterKey
	{
		get
		{
			return "Rank";
		}
	}

	public static string Razer_2SN
	{
		get
		{
			return "Razer_2SN";
		}
	}

	public static string RazerSN
	{
		get
		{
			return "RazerSN";
		}
	}

	public static string RedLightSaberSN
	{
		get
		{
			return "RedLightSaberSN";
		}
	}

	public static string RedMinigunSN
	{
		get
		{
			return "RedMinigunSN";
		}
	}

	public static string ReplaceSkins_1_SN
	{
		get
		{
			return "ReplaceSkins_1_SN";
		}
	}

	public static string restoreWindowShownMult
	{
		get
		{
			return "restoreWindowShownMult";
		}
	}

	public static string restoreWindowShownProfile
	{
		get
		{
			return "restoreWindowShownProfile";
		}
	}

	public static string restoreWindowShownSingle
	{
		get
		{
			return "restoreWindowShownSingle";
		}
	}

	public static string Revolver2SN
	{
		get
		{
			return "Revolver2SN";
		}
	}

	internal static int SaltSeed
	{
		get
		{
			return 2083243184;
		}
	}

	public static string SandFamasSN
	{
		get
		{
			return "SandFamasSN";
		}
	}

	public static int ScoreForSurplusAmmo
	{
		get
		{
			return 50;
		}
	}

	public static float ScreenDiagonal
	{
		get
		{
			float single = Mathf.Sqrt((float)(Screen.width * Screen.width + Screen.height * Screen.height)) / Screen.dpi;
			return single;
		}
	}

	public static float screenRation
	{
		get
		{
			return (float)Screen.width / (float)Screen.height;
		}
	}

	public static string Scythe_2_SN
	{
		get
		{
			return "Scythe_2_SN";
		}
	}

	public static string ScytheSN
	{
		get
		{
			return "ScytheSN";
		}
	}

	public static float Sensitivity
	{
		get
		{
			if (!Defs._sensitivity.HasValue)
			{
				Defs._sensitivity = new float?(PlayerPrefs.GetFloat("SensitivitySett", 12f));
			}
			return Defs._sensitivity.Value;
		}
		set
		{
			Defs._sensitivity = new float?(value);
			PlayerPrefs.SetFloat("SensitivitySett", value);
		}
	}

	public static string SessionDayNumberKey
	{
		get
		{
			return "SessionDayNumber";
		}
	}

	public static string SessionNumberKey
	{
		get
		{
			return "SessionNumber";
		}
	}

	public static string ShmaiserSN
	{
		get
		{
			return "ShmaiserSN";
		}
	}

	public static string ShouldEnableShopSN
	{
		get
		{
			return "ShouldEnableShopSN";
		}
	}

	public static string ShouldReoeatActionSett
	{
		get
		{
			return "ShouldReoeatActionSett";
		}
	}

	public static string ShovelSN
	{
		get
		{
			return "ShovelSN";
		}
	}

	public static string ShowEnder_SN
	{
		get
		{
			return "ShowEnder_SN";
		}
	}

	public static string ShownLobbyLevelSN
	{
		get
		{
			return "ShownLobbyLevelSN";
		}
	}

	public static string ShownNewWeaponsSN
	{
		get
		{
			return "ShownNewWeaponsSN";
		}
	}

	public static string ShowRecSN
	{
		get
		{
			return "ShowRecSN";
		}
	}

	public static string ShowSorryWeaponAndArmor
	{
		get
		{
			return "ShowSorryWeaponAndArmor";
		}
	}

	public static string skin_may1
	{
		get
		{
			return "skin_may1";
		}
	}

	public static string skin_may2
	{
		get
		{
			return "skin_may2";
		}
	}

	public static string skin_may3
	{
		get
		{
			return "skin_may3";
		}
	}

	public static string skin_may4
	{
		get
		{
			return "skin_may4";
		}
	}

	public static string skin810_1
	{
		get
		{
			return "skin810_1";
		}
	}

	public static string skin810_2
	{
		get
		{
			return "skin810_2";
		}
	}

	public static string skin810_3
	{
		get
		{
			return "skin810_3";
		}
	}

	public static string skin810_4
	{
		get
		{
			return "skin810_4";
		}
	}

	public static string skin810_5
	{
		get
		{
			return "skin810_5";
		}
	}

	public static string skin810_6
	{
		get
		{
			return "skin810_6";
		}
	}

	public static string skin931_1
	{
		get
		{
			return "skin931_1";
		}
	}

	public static string skin931_2
	{
		get
		{
			return "skin931_2";
		}
	}

	public static string SkinBaseName
	{
		get
		{
			return "Mult_Skin_";
		}
	}

	public static string SkinEditorMode
	{
		get
		{
			return "SkinEditorMode";
		}
	}

	public static string SkinIndexMultiplayer
	{
		get
		{
			return "SkinIndexMultiplayer";
		}
	}

	public static string SkinNameMultiplayer
	{
		get
		{
			return "SkinNameMultiplayer";
		}
	}

	public static string SkinsMakerInMainMenuPurchased
	{
		get
		{
			return "SkinsMakerInMainMenuPurchased";
		}
	}

	public static string SkinsMakerInProfileBought
	{
		get
		{
			return "SkinsMakerInProfileBought";
		}
	}

	public static int skinsMakerPrice
	{
		get
		{
			return 50;
		}
	}

	public static string SkinsWrittenToGallery
	{
		get
		{
			return "SkinsWrittenToGallery";
		}
	}

	public static string smallAsAntKey
	{
		get
		{
			return "AntsKey";
		}
	}

	public static string spaceengineerBoughtSett
	{
		get
		{
			return "spaceengineerBoughtSett";
		}
	}

	public static string SparklyBlasterSN
	{
		get
		{
			return "SparklyBlasterSN";
		}
	}

	public static string SPASSett
	{
		get
		{
			return "SPASSett";
		}
	}

	public static string Staff2SN
	{
		get
		{
			return "Staff2SN";
		}
	}

	public static string StaffSN
	{
		get
		{
			return "StaffSN";
		}
	}

	public static string StartTimeShowBannersKey
	{
		get
		{
			return "StartTimeShowBanners";
		}
	}

	public static string SteelAxeSN
	{
		get
		{
			return "SteelAxeSN";
		}
	}

	public static string SteelCrossbowSN
	{
		get
		{
			return "SteelCrossbowSN";
		}
	}

	public static string steelmanBoughtSett
	{
		get
		{
			return "steelmanBoughtSett";
		}
	}

	public static string SurvivalScoreSett
	{
		get
		{
			return "SurvivalScoreSett";
		}
	}

	public static string SurvSkinsPath
	{
		get
		{
			return "EnemySkins/Survival";
		}
	}

	public static string SVD_2SN
	{
		get
		{
			return "SVD_2SN";
		}
	}

	public static string SVDSN
	{
		get
		{
			return "SVDSN";
		}
	}

	public static string SwitchingWeaponsSwipeRegimSN
	{
		get
		{
			return "SwitchingWeaponsSwipeRegimSN";
		}
	}

	public static string Sword_2_SN
	{
		get
		{
			return "Sword_2_SN";
		}
	}

	public static string Sword_22SN
	{
		get
		{
			return "Sword_22SN";
		}
	}

	public static string SwordSett
	{
		get
		{
			return "SwordSett";
		}
	}

	public static string TempItemsDictionaryKey
	{
		get
		{
			return "TempItemsDictionaryKey";
		}
	}

	public static string Tesla_2SN
	{
		get
		{
			return "Tesla_2SN";
		}
	}

	public static string TeslaSN
	{
		get
		{
			return "TeslaSN";
		}
	}

	public static string Thompson_2SN
	{
		get
		{
			return "Thompson_2SN";
		}
	}

	public static string ThompsonSN
	{
		get
		{
			return "ThompsonSN";
		}
	}

	public static string TierAfter8_3_0Key
	{
		get
		{
			return "TierAfter8_3_0Key";
		}
	}

	public static string TimeFromWhichShowEnder_SN
	{
		get
		{
			return "TimeFromWhichShowEnder_SN";
		}
	}

	public static bool touchPressureSupported
	{
		get
		{
			return Input.touchPressureSupported;
		}
	}

	public static string TrainingCompleted_4_4_Sett
	{
		get
		{
			return "TrainingCompleted_4_4_Sett";
		}
	}

	public static string TrainingSceneName
	{
		get
		{
			return "Training";
		}
	}

	public static string Tree_2_SN
	{
		get
		{
			return "Tree_2_SN";
		}
	}

	public static string TreeSN
	{
		get
		{
			return "TreeSN";
		}
	}

	public static string TunderGodSett
	{
		get
		{
			return "TunderGodSett";
		}
	}

	public static string UnderwaterKey
	{
		get
		{
			return "UnderwaterKey";
		}
	}

	public static string UpdateAvailableShownTimesSN
	{
		get
		{
			return "UpdateAvailableShownTimesSN";
		}
	}

	public static string WavesSurvivedMaxS
	{
		get
		{
			return "WavesSurvivedMaxS";
		}
	}

	public static string WavesSurvivedS
	{
		get
		{
			return "WavesSurvivedS";
		}
	}

	public static string WeaponsGotInCampaign
	{
		get
		{
			return "WeaponsGotInCampaign";
		}
	}

	public static string WhiteBerettaSN
	{
		get
		{
			return "WhiteBerettaSN";
		}
	}

	public static string WoodenBowSN
	{
		get
		{
			return "WoodenBowSN";
		}
	}

	static Defs()
	{
		Defs.LastSendKillRateTimeKey = "LastSendKillRateTimeKey";
		Defs.StrongDeviceDev = "StrongDeviceDev_DevSetting";
		Defs.TrafficForwardingShowAnalyticsSent = "TrafficForwardingShowAnalyticsSent";
		Defs.DateOfInstallAppForInAppPurchases041215 = "test_window_frames_margins_set_date";
		Defs.FirstInAppPurchaseDate_041215 = "test_additional_window_layout_saved_date_221";
		Defs.SecondInAppPurchaseDate_041215 = "test_second_additional_window_layout_saved_date_221";
		Defs.CoinsForTraining = 15;
		Defs.GemsForTraining = 10;
		Defs.ExpForTraining = 10;
		Defs.GemsGivenRemovedGear = "GemsGivenRemovedGear_10_3_0";
		Defs.LastTimeShowSocialGun = "FacebookControllerLastTimeShowSocialGunKey";
		Defs.ShownRewardWindowForCape = "ShownRewardWindowForCape";
		Defs.ShownRewardWindowForSkin = "ShownRewardWindowForSkin";
		Defs.DaterWSSN = "DaterWSSN";
		Defs.SmileMessageSuffix = "ýSýMýIýLýEý";
		Defs.IsFacebookLoginRewardaGained = "IsFacebookLoginRewardaGained";
		Defs.FacebookRewardGainStarted = "FacebookRewardGainStarted";
		Defs.IsTwitterLoginRewardaGained = "IsTwitterLoginRewardaGained";
		Defs.TwitterRewardGainStarted = "TwitterRewardGainStarted";
		Defs.ResetTrainingInDevBuild = false;
		Defs.useSqlLobby = true;
		Defs.keyTestCountGetGift = "keyTestCountGetGift";
		Defs.BuyGiftKey = "BuyGiftKey";
		Defs.initValsInKeychain43 = "initValsInKeychain43";
		Defs.initValsInKeychain44 = "initValsInKeychain44";
		Defs.initValsInKeychain45 = "initValsInKeychain45";
		Defs.initValsInKeychain46 = "initValsInKeychain46";
		Defs.isMouseControl = false;
		Defs.isRegimVidosDebug = false;
		Defs.nonActivABTestRatingSystemKey = "nonActivABTestRatingSystemKey";
		Defs.nonActivABTestStaticBankKey = "nonActivABTestStaticBankKey";
		Defs.nonActivABTestBuffSystemKey = "nonActivABTestBuffSystemKey";
		Defs.cohortABTestSandBoxKey = "cohortABTestSandBoxKey";
		Defs.cohortABTestQuestSystemKey = "cohortABTestQuestSystemKey";
		Defs.cohortABTestSpecialOffersKey = "cohortABTestSpecialOffersKey";
		Defs.MoneyGiven831to901 = "MoneyGiven831to901";
		Defs.GotCoinsForTraining = "GotCoinsForTraining";
		Defs.typeDisconnectGame = Defs.DisconectGameType.Exit;
		Defs.gameSecondFireButtonMode = Defs.GameSecondFireButtonMode.Sniper;
		Defs.ZoomButtonX = -176;
		Defs.ZoomButtonY = 431;
		Defs.ReloadButtonX = -72;
		Defs.ReloadButtonY = 340;
		Defs.JumpButtonX = -95;
		Defs.JumpButtonY = 79;
		Defs.FireButtonX = -250;
		Defs.FireButtonY = 150;
		Defs.JoyStickX = 172;
		Defs.JoyStickY = 160;
		Defs.GrenadeX = -46;
		Defs.GrenadeY = 445;
		Defs.FireButton2X = 160;
		Defs.FireButton2Y = 337;
		Defs.VisualHatArmor = "VisualHatArmor";
		Defs.VisualArmor = "VisualArmor";
		Defs.RatingDeathmatch = "RatingDeathmatch";
		Defs.RatingTeamBattle = "RatingTeamBattle";
		Defs.RatingHunger = "RatingHunger";
		Defs.RatingFlag = "RatingFlag";
		Defs.RatingCapturePoint = "RatingCapturePoint";
		Defs.LogoWidth = 8;
		Defs.LogoHeight = 8;
		Defs.SurvivalMaps = new string[] { "Arena_Swamp", "Arena_Underwater", "Coliseum", "Arena_Castle", "Arena_Space", "Arena_Hockey", "Arena_Mine", "Pizza" };
		Defs.CurrentSurvMapIndex = -1;
		Defs.FreezerSlowdownTime = 5f;
		Defs._initializedJoystickParams = false;
		Defs.isShowUserAgrement = false;
		Defs.maxCountFriend = 100;
		Defs.maxMemberClanCount = 20;
		Defs.timeUpdateFriendInfo = 15f;
		Defs.timeUpdateOnlineInGame = 12f;
		Defs.timeUpdateInfoInProfile = 15f;
		Defs.timeBlockRefreshFriendDate = 5f;
		Defs.timeUpdateLeaderboardIfNullResponce = 15f;
		Defs.timeUpdateStartCheckIfNullResponce = 15f;
		Defs.timeWaitLoadPossibleFriends = 5f;
		Defs.pauseUpdateLeaderboard = 60f;
		Defs.timeUpdatePixelbookInfo = 900f;
		Defs.timeUpdateNews = 1800f;
		Defs.historyPrivateMessageLength = 100;
		Defs.timeUpdateServerTime = 15f;
		Defs.abTestBalansCohort = Defs.ABTestCohortsType.NONE;
		Defs._isAbTestBalansCohortActual = null;
		Defs.abTestBalansCohortName = "NONE";
		Defs.abTestBalansStartCapitalCoins = 0;
		Defs.abTestBalansStartCapitalGems = 0;
		Defs.bigPorogString = "No space for new friends. Delete friends or requests";
		Defs.smallPorogString = "Tap ADD TO FRIENDS to send a friendship request to the player";
		Defs.friendsSceneName = "FriendsScene";
		Defs.ammoInGamePanelPrice = 3;
		Defs.healthInGamePanelPrice = 5;
		Defs.ClansPrice = 0;
		Defs.ProfileFromFriends = 0;
		Defs.isInet = true;
		Defs.PixelGunAppID = "640111933";
		Defs.AppStoreURL = string.Concat("https://itunes.apple.com/app/pixel-gun-3d-block-world-pocket/id", Defs.PixelGunAppID, "?mt=8");
		Defs.SupportMail = "pixelgun3D.supp0rt@gmail.com";
		Defs.EnderManAvailable = true;
		Defs.isSoundMusic = false;
		Defs.isSoundFX = false;
		Defs.BottomOffs = 21f;
		Defs.filterMaps = new Dictionary<string, int>();
		Defs._premiumMaps = new Dictionary<string, int>();
		Defs.NumberOfElixirs = 1;
		Defs.isGrenateFireEnable = true;
		Defs.isZooming = false;
		Defs.isJetpackEnabled = false;
		Defs.GoToProfileShopInterval = 1f;
		Defs.InvertCamSN = "InvertCamSN";
		Defs.players = new List<GameObject>();
		Defs.PromSceneName = "PromScene";
		Defs._3_shotgun_2 = "_3_shotgun_2";
		Defs._3_shotgun_3 = "_3_shotgun_3";
		Defs.flower_2 = "flower_2";
		Defs.flower_3 = "flower_3";
		Defs.gravity_2 = "gravity_2";
		Defs.gravity_3 = "gravity_3";
		Defs.grenade_launcher_3 = "grenade_launcher_3";
		Defs.revolver_2_2 = "revolver_2_2";
		Defs.revolver_2_3 = "revolver_2_3";
		Defs.scythe_3 = "scythe_3";
		Defs.plazma_2 = "plazma_2";
		Defs.plazma_3 = "plazma_3";
		Defs.plazma_pistol_2 = "plazma_pistol_2";
		Defs.plazma_pistol_3 = "plazma_pistol_3";
		Defs.railgun_2 = "railgun_2";
		Defs.railgun_3 = "railgun_3";
		Defs.Razer_3 = "Razer_3";
		Defs.tesla_3 = "tesla_3";
		Defs.Flamethrower_3 = "Flamethrower_3";
		Defs.FreezeGun_0 = "FreezeGun_0";
		Defs.svd_3 = "svd_3";
		Defs.barret_3 = "barret_3";
		Defs.minigun_3 = "minigun_3";
		Defs.LightSword_3 = "LightSword_3";
		Defs.Sword_2_3 = "Sword_2_3";
		Defs.Staff_3 = "Staff 3";
		Defs.DragonGun = "DragonGun";
		Defs.Bow_3 = "Bow_3";
		Defs.Bazooka_1_3 = "Bazooka_1_3";
		Defs.Bazooka_2_1 = "Bazooka_2_1";
		Defs.Bazooka_2_3 = "Bazooka_2_3";
		Defs.m79_2 = "m79_2";
		Defs.m79_3 = "m79_3";
		Defs.m32_1_2 = "m32_1_2";
		Defs.Red_Stone_3 = "Red_Stone_3";
		Defs.XM8_1 = "XM8_1";
		Defs.PumpkinGun_1 = "PumpkinGun_1";
		Defs.XM8_2 = "XM8_2";
		Defs.XM8_3 = "XM8_3";
		Defs.PumpkinGun_2 = "PumpkinGun_2";
		Defs.PumpkinGun_5 = "PumpkinGun_5";
		Defs.Skins_11_040915 = new string[] { "skin_fiance", "skin_bride", "skin_bigalien", "skin_minialien", "skin_hippo", "skin_alligator" };
		Defs.skin_tiger = "skin_tiger";
		Defs.skin_pitbull = "skin_pitbull";
		Defs.skin_santa = "skin_santa";
		Defs.skin_elf_new_year = "skin_elf_new_year";
		Defs.skin_girl_new_year = "skin_girl_new_year";
		Defs.skin_cookie_new_year = "skin_cookie_new_year";
		Defs.skin_snowman_new_year = "skin_snowman_new_year";
		Defs.skin_jetti_hnight = "skin_jetti_hnight";
		Defs.skin_startrooper = "skin_startrooper";
		Defs.easter_skin1 = "easter_skin1";
		Defs.easter_skin2 = "easter_skin2";
		Defs.skin_rapid_girl = "skin_rapid_girl";
		Defs.skin_silent_killer = "skin_silent_killer";
		Defs.skin_daemon_fighter = "skin_daemon_fighter";
		Defs.skin_scary_demon = "skin_scary_demon";
		Defs.skin_orc_warrior = "skin_orc_warrior";
		Defs.skin_kung_fu_master = "skin_kung_fu_master";
		Defs.skin_fire_wizard = "skin_fire_wizard";
		Defs.skin_ice_wizard = "skin_ice_wizard";
		Defs.skin_storm_wizard = "skin_storm_wizard";
		Defs._isUse3DTouch = -1;
		Defs.Weapons800to801 = "Weapons800to801";
		Defs.Weapons831to901 = "Weapons831to901";
		Defs.Update_AddSniperCateogryKey = "Update_AddSniperCateogryKey";
		Defs.FixWeapons911 = "FixWeapons911";
		Defs.ReturnAlienGun930 = "ReturnAlienGun930";
		Defs.diffGame = 2;
		Defs.StartTimeShowBannersString = string.Empty;
		Defs._countReturnInConnectScene = 0;
		Defs.showTableInNetworkStartTable = false;
		Defs.showNickTableInNetworkStartTable = false;
		Defs.isTurretWeapon = false;
		Defs.inComingMessagesCounter = 0;
		HashSet<string> strs = new HashSet<string>();
		strs.Add("fireFlash");
		strs.Add("HoleRPC");
		strs.Add("ShowBonuseParticleRPC");
		strs.Add("ShowMultyKillRPC");
		strs.Add("ReloadGun");
		Defs.unimportantRPCList = strs;
		Defs.inRespawnWindow = false;
		Defs.IsFirstLaunchFreshInstall = "IsFirstLaunchFreshInstall";
		Defs.NewbieEventX3StartTime = "NewbieEventX3StartTime";
		Defs.NewbieEventX3StartTimeAdditional = "NewbieEventX3StartTimeAdditional";
		Defs.NewbieEventX3LastLoggedTime = "NewbieEventX3LastLoggedTime";
		Defs.WasNewbieEventX3 = "WasNewbieEventX3";
		Defs._premiumMaps.Add("Ants", 15);
		Defs._premiumMaps.Add("Matrix", 15);
		Defs._premiumMaps.Add("Underwater", 15);
		Defs.filterMaps.Add("Knife", 1);
		Defs.filterMaps.Add("Sniper", 2);
		Defs.filterMaps.Add("LoveIsland", 3);
		Defs.filterMaps.Add("WinterIsland", 3);
	}

	public Defs()
	{
	}

	public static Color AmbientLightColorForShop()
	{
		return new Color(0.39215687f, 0.39215687f, 0.39215687f, 1f);
	}

	public static int CompareAlphaNumerically(object x, object y)
	{
		int num;
		string str = x as string;
		if (str == null)
		{
			return 0;
		}
		string str1 = y as string;
		if (str1 == null)
		{
			return 0;
		}
		int length = str.Length;
		int length1 = str1.Length;
		int num1 = 0;
		int num2 = 0;
		while (num1 < length && num2 < length1)
		{
			char chr = str[num1];
			char chr1 = str1[num2];
			char[] chrArray = new char[length];
			int num3 = 0;
			char[] chrArray1 = new char[length1];
			int num4 = 0;
			do
			{
				int num5 = num3;
				num3 = num5 + 1;
				chrArray[num5] = chr;
				num1++;
				if (num1 >= length)
				{
					break;
				}
				else
				{
					chr = str[num1];
				}
			}
			while (char.IsDigit(chr) == char.IsDigit(chrArray[0]));
			do
			{
				int num6 = num4;
				num4 = num6 + 1;
				chrArray1[num6] = chr1;
				num2++;
				if (num2 >= length1)
				{
					break;
				}
				else
				{
					chr1 = str1[num2];
				}
			}
			while (char.IsDigit(chr1) == char.IsDigit(chrArray1[0]));
			string str2 = new string(chrArray);
			string str3 = new string(chrArray1);
			if (!char.IsDigit(chrArray[0]) || !char.IsDigit(chrArray1[0]))
			{
				num = str2.CompareTo(str3);
			}
			else
			{
				int num7 = int.Parse(str2);
				num = num7.CompareTo(int.Parse(str3));
			}
			if (num == 0)
			{
				continue;
			}
			return num;
		}
		return length - length1;
	}

	public static string GetIntendedAndroidPackageName()
	{
		return Defs.GetIntendedAndroidPackageName(Defs.AndroidEdition);
	}

	public static string GetIntendedAndroidPackageName(Defs.RuntimeAndroidEdition androidEdition)
	{
		Defs.RuntimeAndroidEdition runtimeAndroidEdition = androidEdition;
		if (runtimeAndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			return "com.PixelGun.a3D";
		}
		if (runtimeAndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
		{
			return "com.pixel.gun3d";
		}
		return string.Empty;
	}

	public static void InitCoordsIphone()
	{
		if (!Defs._initializedJoystickParams)
		{
			float screenDiagonal = Defs.ScreenDiagonal;
			if (screenDiagonal > 9f)
			{
				Defs.ZoomButtonX = -216;
				Defs.ZoomButtonY = 369;
				Defs.ReloadButtonX = -49;
				Defs.ReloadButtonY = 302;
				Defs.JumpButtonX = -101;
				Defs.JumpButtonY = 90;
				Defs.FireButtonX = -258;
				Defs.FireButtonY = 179;
				Defs.JoyStickX = 172;
				Defs.JoyStickY = 160;
				Defs.GrenadeX = -110;
				Defs.GrenadeY = 381;
				Defs.FireButton2X = 173;
				Defs.FireButton2Y = 340;
			}
			if (screenDiagonal > 7.5f && screenDiagonal <= 9f)
			{
				Defs.ZoomButtonX = -230;
				Defs.ZoomButtonY = 397;
				Defs.ReloadButtonX = -53;
				Defs.ReloadButtonY = 355;
				Defs.JumpButtonX = -116;
				Defs.JumpButtonY = 99;
				Defs.FireButtonX = -284;
				Defs.FireButtonY = 175;
				Defs.JoyStickX = 172;
				Defs.JoyStickY = 160;
				Defs.GrenadeX = -130;
				Defs.GrenadeY = 419;
				Defs.FireButton2X = 173;
				Defs.FireButton2Y = 352;
			}
			if (screenDiagonal > 6f && screenDiagonal <= 7.5f)
			{
				Defs.ZoomButtonX = -227;
				Defs.ZoomButtonY = 404;
				Defs.ReloadButtonX = -67;
				Defs.ReloadButtonY = 351;
				Defs.JumpButtonX = -125;
				Defs.JumpButtonY = 104;
				Defs.FireButtonX = -291;
				Defs.FireButtonY = 189;
				Defs.JoyStickX = 170;
				Defs.JoyStickY = 167;
				Defs.GrenadeX = -131;
				Defs.GrenadeY = 441;
				Defs.FireButton2X = 173;
				Defs.FireButton2Y = 352;
			}
			if (screenDiagonal > 4.8f && screenDiagonal <= 6f)
			{
				Defs.ZoomButtonX = -263;
				Defs.ZoomButtonY = 409;
				Defs.ReloadButtonX = -61;
				Defs.ReloadButtonY = 359;
				Defs.JumpButtonX = -126;
				Defs.JumpButtonY = 105;
				Defs.FireButtonX = -319;
				Defs.FireButtonY = 181;
				Defs.JoyStickX = 170;
				Defs.JoyStickY = 167;
				Defs.GrenadeX = -155;
				Defs.GrenadeY = 424;
				Defs.FireButton2X = 176;
				Defs.FireButton2Y = 361;
			}
			if (screenDiagonal > 4f && screenDiagonal <= 4.8f)
			{
				Defs.ZoomButtonX = -298;
				Defs.ZoomButtonY = 402;
				Defs.ReloadButtonX = -68;
				Defs.ReloadButtonY = 369;
				Defs.JumpButtonX = -133;
				Defs.JumpButtonY = 99;
				Defs.FireButtonX = -331;
				Defs.FireButtonY = 179;
				Defs.JoyStickX = 170;
				Defs.JoyStickY = 167;
				Defs.GrenadeX = -175;
				Defs.GrenadeY = 428;
				Defs.FireButton2X = 189;
				Defs.FireButton2Y = 357;
			}
		}
		Defs._initializedJoystickParams = true;
	}

	public enum ABTestCohortsType
	{
		NONE,
		A,
		B,
		SKIP
	}

	public enum DisconectGameType
	{
		Exit,
		Reconnect,
		RandomGameInHunger,
		SelectNewMap
	}

	public enum GameSecondFireButtonMode
	{
		Sniper,
		On,
		Off
	}

	public enum RuntimeAndroidEdition
	{
		None,
		Amazon,
		GoogleLite
	}

	public enum WeaponIndex
	{
		Grenade = 1000,
		Turret = 1001
	}
}