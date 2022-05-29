using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Prime31;
using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(FrameStopwatchScript))]
internal sealed class Switcher : MonoBehaviour
{
	internal const string AbuseMethodKey = "AbuseMethod";

	public static Dictionary<string, int> sceneNameToGameNum;

	public static Dictionary<string, int> counCreateMobsInLevel;

	public static string LoadingInResourcesPath;

	public static string[] loadingNames;

	public GameObject coinsShopPrefab;

	public GameObject nickStackPrefab;

	public GameObject skinsManagerPrefab;

	public GameObject ExperienceControllerPrefab;

	public GameObject weaponManagerPrefab;

	public GameObject flurryPrefab;

	public GameObject experienceGuiPrefab;

	public GameObject bankGuiPrefab;

	public GameObject freeAwardGuiPrefab;

	public GameObject buttonClickSoundPrefab;

	public GameObject faceBookControllerPrefab;

	public GameObject licenseVerificationControllerPrefab;

	public GameObject potionsControllerPrefab;

	public GameObject protocolListGetterPrefab;

	public GameObject updateCheckerPrefab;

	public GameObject promoActionsManagerPrefab;

	public GameObject starterPackManagerPrefab;

	public GameObject tempItemsControllerPrefab;

	public GameObject remotePushNotificationControllerPrefab;

	public GameObject premiumAccountControllerPrefab;

	public GameObject twitterControllerPrefab;

	public GameObject sponsorPayPluginHolderPrefab;

	public GameObject giftControllerPrefab;

	public GameObject disabler;

	public GameObject sceneInfoController;

	private Rect plashkaCoinsRect;

	private Texture fonToDraw;

	private bool _newLaunchingApproach;

	public static Stopwatch timer;

	private static bool _initialAppVersionInitialized;

	private static string _InitialAppVersion;

	public static GameObject comicsSound;

	private float _progress;

	private static AbuseMetod? _abuseMethod;

	internal static AbuseMetod AbuseMethod
	{
		get
		{
			if (!Switcher._abuseMethod.HasValue)
			{
				Switcher._abuseMethod = new AbuseMetod?((AbuseMetod)Storager.getInt("AbuseMethod", false));
			}
			return Switcher._abuseMethod.Value;
		}
	}

	public static string InitialAppVersion
	{
		get
		{
			if (!Switcher._initialAppVersionInitialized)
			{
				Switcher._InitialAppVersion = PlayerPrefs.GetString(Defs.InitialAppVersionKey);
				Switcher._initialAppVersionInitialized = true;
			}
			return Switcher._InitialAppVersion;
		}
		private set
		{
			Switcher._InitialAppVersion = value;
			Switcher._initialAppVersionInitialized = true;
		}
	}

	static Switcher()
	{
		Switcher.sceneNameToGameNum = new Dictionary<string, int>();
		Switcher.counCreateMobsInLevel = new Dictionary<string, int>();
		Switcher.LoadingInResourcesPath = "LevelLoadings";
		Switcher.loadingNames = new string[] { "Loading_coliseum", "loading_Cementery", "Loading_Maze", "Loading_City", "Loading_Hospital", "Loading_Jail", "Loading_end_world_2", "Loading_Arena", "Loading_Area52", "Loading_Slender", "Loading_Hell", "Loading_bloody_farm", "Loading_most", "Loading_school", "Loading_utopia", "Loading_sky", "Loading_winter" };
		Switcher.timer = new Stopwatch();
		Switcher._initialAppVersionInitialized = false;
		Switcher._InitialAppVersion = string.Empty;
		Switcher.sceneNameToGameNum.Add("Training", 0);
		Switcher.sceneNameToGameNum.Add("Cementery", 1);
		Switcher.sceneNameToGameNum.Add("Maze", 2);
		Switcher.sceneNameToGameNum.Add("City", 3);
		Switcher.sceneNameToGameNum.Add("Hospital", 4);
		Switcher.sceneNameToGameNum.Add("Jail", 5);
		Switcher.sceneNameToGameNum.Add("Gluk_2", 6);
		Switcher.sceneNameToGameNum.Add("Arena", 7);
		Switcher.sceneNameToGameNum.Add("Area52", 8);
		Switcher.sceneNameToGameNum.Add("Slender", 9);
		Switcher.sceneNameToGameNum.Add("Castle", 10);
		Switcher.sceneNameToGameNum.Add("Farm", 11);
		Switcher.sceneNameToGameNum.Add("Bridge", 12);
		Switcher.sceneNameToGameNum.Add("School", 13);
		Switcher.sceneNameToGameNum.Add("Utopia", 14);
		Switcher.sceneNameToGameNum.Add("Sky_islands", 15);
		Switcher.sceneNameToGameNum.Add("Winter", 16);
		Switcher.sceneNameToGameNum.Add("Swamp_campaign3", 17);
		Switcher.sceneNameToGameNum.Add("Castle_campaign3", 18);
		Switcher.sceneNameToGameNum.Add("Space_campaign3", 19);
		Switcher.sceneNameToGameNum.Add("Parkour", 20);
		Switcher.sceneNameToGameNum.Add("Code_campaign3", 21);
		Switcher.counCreateMobsInLevel.Add("Farm", 10);
		Switcher.counCreateMobsInLevel.Add("Cementery", 15);
		Switcher.counCreateMobsInLevel.Add("City", 20);
		Switcher.counCreateMobsInLevel.Add("Hospital", 25);
		Switcher.counCreateMobsInLevel.Add("Bridge", 25);
		Switcher.counCreateMobsInLevel.Add("Jail", 30);
		Switcher.counCreateMobsInLevel.Add("Slender", 30);
		Switcher.counCreateMobsInLevel.Add("Area52", 35);
		Switcher.counCreateMobsInLevel.Add("School", 35);
		Switcher.counCreateMobsInLevel.Add("Utopia", 25);
		Switcher.counCreateMobsInLevel.Add("Maze", 30);
		Switcher.counCreateMobsInLevel.Add("Sky_islands", 30);
		Switcher.counCreateMobsInLevel.Add("Winter", 30);
		Switcher.counCreateMobsInLevel.Add("Castle", 35);
		Switcher.counCreateMobsInLevel.Add("Gluk_2", 35);
		Switcher.counCreateMobsInLevel.Add("Swamp_campaign3", 30);
		Switcher.counCreateMobsInLevel.Add("Castle_campaign3", 35);
		Switcher.counCreateMobsInLevel.Add("Space_campaign3", 25);
		Switcher.counCreateMobsInLevel.Add("Parkour", 15);
		Switcher.counCreateMobsInLevel.Add("Code_campaign3", 35);
	}

	public Switcher()
	{
	}

	internal static void AppendAbuseMethod(AbuseMetod f)
	{
		Switcher._abuseMethod = new AbuseMetod?(Switcher.AbuseMethod | f);
		Storager.setInt("AbuseMethod", (int)Switcher._abuseMethod.Value, false);
	}

	private static void CheckHugeUpgrade()
	{
		bool flag = Storager.hasKey("Coins");
		if (flag && !Storager.hasKey(Defs.ArmorNewEquppedSN))
		{
			Switcher.AppendAbuseMethod(AbuseMetod.UpgradeFromVulnerableVersion);
			UnityEngine.Debug.LogError(string.Concat("Upgrade tampering detected: ", Switcher.AbuseMethod));
		}
	}

	private static void ClearProgress()
	{
	}

	private static void CountMoneyForArmorHats()
	{
		Storager.hasKey("MoneyGivenRemovedArmorHat");
		Storager.SyncWithCloud("MoneyGivenRemovedArmorHat");
		Storager.hasKey("RemovedArmorHatMethodExecuted");
		if (Storager.getInt("RemovedArmorHatMethodExecuted", false) != 0)
		{
			return;
		}
		bool num = Storager.getInt("MoneyGivenRemovedArmorHat", true) == 0;
		int price = 0;
		if (num)
		{
			foreach (string item in Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0])
			{
				if (Storager.getInt(item, true) <= 0)
				{
					continue;
				}
				price += VirtualCurrencyHelper.Price(item).Price;
			}
		}
		Storager.hasKey(Defs.HatEquppedSN);
		string str = Storager.getString(Defs.HatEquppedSN, false) ?? string.Empty;
		if (Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].Contains(str))
		{
			Storager.setString(Defs.HatEquppedSN, ShopNGUIController.NoneEquippedForWearCategory(ShopNGUIController.CategoryNames.HatsCategory), false);
		}
		Storager.setInt("RemovedArmorHatMethodExecuted", 1, false);
		Storager.setInt("MoneyGivenRemovedArmorHat", 1, true);
	}

	private static void CountMoneyForGunsFrom831To901()
	{
		Storager.hasKey(Defs.MoneyGiven831to901);
		Storager.SyncWithCloud(Defs.MoneyGiven831to901);
		Storager.hasKey(Defs.Weapons831to901);
		if (Storager.getInt(Defs.Weapons831to901, false) != 0)
		{
			return;
		}
		bool num = Storager.getInt(Defs.MoneyGiven831to901, true) == 0;
		int num1 = 0;
		int num2 = 0;
		if (num)
		{
			Dictionary<string, int> strs = new Dictionary<string, int>()
			{
				{ WeaponTags.CrossbowTag, 120 },
				{ WeaponTags.CrystalCrossbowTag, 155 },
				{ WeaponTags.SteelCrossbowTag, 120 },
				{ WeaponTags.Bow_3_Tag, 185 },
				{ WeaponTags.WoodenBowTag, 100 },
				{ WeaponTags.Staff2Tag, 200 },
				{ WeaponTags.Staff_3_Tag, 235 }
			};
			foreach (KeyValuePair<string, int> str in strs)
			{
				string key = str.Key;
				int value = str.Value;
				if (!Switcher.IsWeaponBought(key))
				{
					continue;
				}
				num1 += value;
			}
			strs = new Dictionary<string, int>()
			{
				{ WeaponTags.AutoShotgun_Tag, 255 },
				{ WeaponTags.TwoRevolvers_Tag, 267 },
				{ WeaponTags.TwoBolters_Tag, 249 },
				{ WeaponTags.SnowballGun_Tag, 281 }
			};
			foreach (KeyValuePair<string, int> keyValuePair in strs)
			{
				string key1 = keyValuePair.Key;
				int value1 = keyValuePair.Value;
				if (!Switcher.IsWeaponBought(key1))
				{
					continue;
				}
				num2 += value1;
			}
			strs = new Dictionary<string, int>()
			{
				{ "cape_EliteCrafter", 50 },
				{ "cape_Archimage", 65 },
				{ "cape_BloodyDemon", 50 },
				{ "cape_SkeletonLord", 75 },
				{ "cape_RoyalKnight", 65 }
			};
			foreach (KeyValuePair<string, int> str1 in strs)
			{
				string key2 = str1.Key;
				int value2 = str1.Value;
				if (Storager.hasKey(key2) && Storager.getInt(key2, false) != 0)
				{
					num1 += value2;
				}
			}
			strs = new Dictionary<string, int>()
			{
				{ "boots_gray", 50 },
				{ "boots_red", 50 },
				{ "boots_black", 100 },
				{ "boots_blue", 50 },
				{ "boots_green", 75 }
			};
			foreach (KeyValuePair<string, int> keyValuePair1 in strs)
			{
				string str2 = keyValuePair1.Key;
				int value3 = keyValuePair1.Value;
				if (Storager.hasKey(str2) && Storager.getInt(str2, false) != 0)
				{
					num1 += value3;
				}
			}
		}
		Storager.setInt(Defs.Weapons831to901, 1, false);
		Storager.setInt(Defs.MoneyGiven831to901, 1, true);
	}

	private static void CountMoneyForRemovedGear()
	{
		Storager.hasKey(Defs.GemsGivenRemovedGear);
		if (Storager.getInt(Defs.GemsGivenRemovedGear, false) != 0)
		{
			return;
		}
		int num = 0;
		Dictionary<string, int> strs = new Dictionary<string, int>()
		{
			{ GearManager.Turret, 5 },
			{ GearManager.Mech, 7 },
			{ GearManager.InvisibilityPotion, 3 },
			{ GearManager.Jetpack, 4 }
		};
		Dictionary<string, int> strs1 = strs;
		foreach (string key in strs1.Keys)
		{
			num = num + Storager.getInt(key, false) * strs1[key];
		}
		Storager.setInt(Defs.GemsGivenRemovedGear, 1, false);
		foreach (string str in strs1.Keys)
		{
			Storager.setInt(str, 0, false);
		}
	}

	private static string DetermineSceneName()
	{
		int num = GlobalGameController.currentLevel;
		switch (num)
		{
			case -1:
			{
				return Defs.MainMenuScene;
			}
			case 0:
			{
				return "Cementery";
			}
			case 1:
			{
				return "Maze";
			}
			case 2:
			{
				return "City";
			}
			case 3:
			{
				return "Hospital";
			}
			case 4:
			{
				return "Jail";
			}
			case 5:
			{
				return "Gluk_2";
			}
			case 6:
			{
				return "Arena";
			}
			case 7:
			{
				return "Area52";
			}
			case 8:
			{
				return "Slender";
			}
			case 9:
			{
				return "Castle";
			}
			default:
			{
				if (num == 101)
				{
					break;
				}
				else
				{
					return Defs.MainMenuScene;
				}
			}
		}
		return "Training";
	}

	private static double Hypot(double x, double y)
	{
		x = Math.Abs(x);
		y = Math.Abs(y);
		double num = Math.Max(x, y);
		double num1 = Math.Min(x, y) / num;
		return num * Math.Sqrt(1 + num1 * num1);
	}

	[DebuggerHidden]
	internal static IEnumerable<float> InitializeStorager()
	{
		Switcher.u003cInitializeStorageru003ec__Iterator1C9 variable = null;
		return variable;
	}

	[DebuggerHidden]
	public IEnumerable<float> InitializeSwitcher(Action setArmorArmy1ComesFromCloud = null)
	{
		Switcher.u003cInitializeSwitcheru003ec__Iterator1CC variable = null;
		return variable;
	}

	private static bool IsLastLoggedDateExpired(string timestampKey, DateTime nowDate)
	{
		DateTime dateTime;
		if (!DateTime.TryParse(PlayerPrefs.GetString(timestampKey, "1970-01-01"), out dateTime))
		{
			dateTime = new DateTime(1970, 1, 1);
		}
		return nowDate > dateTime;
	}

	private static bool IsWeaponBought(string weaponTag)
	{
		string str;
		string str1;
		return (!WeaponManager.tagToStoreIDMapping.TryGetValue(weaponTag, out str) || str == null || !WeaponManager.storeIDtoDefsSNMapping.TryGetValue(str, out str1) || str1 == null || !Storager.hasKey(str1) ? false : Storager.getInt(str1, true) > 0);
	}

	public static string LoadingCupTexture(int number)
	{
		return string.Concat("loading_cups_", number.ToString(), (!Device.isRetinaAndStrong ? string.Empty : "-hd"));
	}

	[DebuggerHidden]
	public IEnumerable<float> LoadMainMenu(bool armyArmor1ComesFromCloud = false)
	{
		Switcher.u003cLoadMainMenuu003ec__Iterator1CD variable = null;
		return variable;
	}

	private static void LogArmorPopularityForLevelToFlurry(string[] mostPopular)
	{
		int num = ExperienceController.sharedController.currentLevel;
		string payingSuffix = FlurryPluginWrapper.GetPayingSuffix();
		int num1 = (num - 1) / 9;
		string str = string.Format("[{0}, {1})", num1 * 9 + 1, (num1 + 1) * 9 + 1);
		string str1 = string.Format("Armor Popularity Level {0}{1}", str, payingSuffix);
		string[] strArrays = mostPopular;
		for (int i = 0; i < (int)strArrays.Length; i++)
		{
			string str2 = strArrays[i];
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ string.Concat("Level ", num), str2 }
			};
			FlurryPluginWrapper.LogEventAndDublicateToConsole(str1, strs, true);
		}
	}

	private static void LogArmorPopularityForTierToFlurry(string[] mostPopular)
	{
		int ourTier = ExpController.Instance.OurTier;
		string eventName = FlurryPluginWrapper.GetEventName("Armor Popularity Tier");
		string[] strArrays = mostPopular;
		for (int i = 0; i < (int)strArrays.Length; i++)
		{
			string str = strArrays[i];
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ string.Concat("Tier ", ourTier), str }
			};
			FlurryPluginWrapper.LogEventAndDublicateToConsole(eventName, strs, true);
		}
	}

	private static void LogArmorPopularityToFlurry(string[] mostPopular)
	{
		string eventName = FlurryPluginWrapper.GetEventName("Armor Popularity");
		string[] strArrays = mostPopular;
		for (int i = 0; i < (int)strArrays.Length; i++)
		{
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ "Name", strArrays[i] }
			};
			FlurryPluginWrapper.LogEventAndDublicateToConsole(eventName, strs, true);
		}
	}

	private static void LogPopularityToFlurry(string loggedDateTimestampKey, Func<string[]> getMostPopular, Action<string[]> logMostPopular)
	{
		DateTime date = DateTime.UtcNow.Date;
		if (Switcher.IsLastLoggedDateExpired(loggedDateTimestampKey, date))
		{
			string[] strArrays = getMostPopular();
			if ((int)strArrays.Length > 0)
			{
				logMostPopular(strArrays);
				PlayerPrefs.SetString(loggedDateTimestampKey, date.ToString("yyyy-MM-dd"));
			}
		}
	}

	private static void LogWeaponAndArmorPopularityToFlurry()
	{
		Switcher.LogPopularityToFlurry("Statistics.WeaponPopularityTimestamp", () => Statistics.Instance.GetMostPopularWeapons(), new Action<string[]>(Switcher.LogWeaponPopularityToFlurry));
		Switcher.LogPopularityToFlurry("Statistics.WeaponPopularityForTierTimestamp", () => Statistics.Instance.GetMostPopularWeaponsForTier(ExpController.Instance.OurTier), new Action<string[]>(Switcher.LogWeaponPopularityForTierToFlurry));
		Switcher.LogPopularityToFlurry("Statistics.ArmorPopularityTimestamp", () => Statistics.Instance.GetMostPopularArmors(), new Action<string[]>(Switcher.LogArmorPopularityToFlurry));
		Switcher.LogPopularityToFlurry("Statistics.ArmorPopularityForTierTimestamp", () => Statistics.Instance.GetMostPopularArmorsForTier(ExpController.Instance.OurTier), new Action<string[]>(Switcher.LogArmorPopularityForTierToFlurry));
		Switcher.LogPopularityToFlurry("Statistics.ArmorPopularityForLevelTimestamp", () => Statistics.Instance.GetMostPopularArmorsForLevel(ExperienceController.sharedController.currentLevel), new Action<string[]>(Switcher.LogArmorPopularityForLevelToFlurry));
	}

	private static void LogWeaponPopularityForTierToFlurry(string[] mostPopular)
	{
		int ourTier = ExpController.Instance.OurTier;
		string eventName = FlurryPluginWrapper.GetEventName("Weapon Popularity Tier");
		string[] strArrays = mostPopular;
		for (int i = 0; i < (int)strArrays.Length; i++)
		{
			string str = strArrays[i];
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ string.Concat("Tier ", ourTier), str }
			};
			FlurryPluginWrapper.LogEventAndDublicateToConsole(eventName, strs, true);
		}
	}

	private static void LogWeaponPopularityToFlurry(string[] mostPopular)
	{
		string eventName = FlurryPluginWrapper.GetEventName("Weapon Popularity");
		string[] strArrays = mostPopular;
		for (int i = 0; i < (int)strArrays.Length; i++)
		{
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ "Favorite Weapon", strArrays[i] }
			};
			FlurryPluginWrapper.LogEventAndDublicateToConsole(eventName, strs, true);
		}
	}

	private void OnDestroy()
	{
		ActivityIndicator.IsShowWindowLoading = false;
	}

	[DebuggerHidden]
	private IEnumerator ParseConfigsCoroutine()
	{
		return new Switcher.u003cParseConfigsCoroutineu003ec__Iterator1CA();
	}

	private static void PerformEssentialInitialization(string currencyKey, AbuseMetod abuseMethod)
	{
		if (Storager.hasKey(currencyKey))
		{
			int num = Storager.getInt(currencyKey, false);
			if (!DigestStorager.Instance.ContainsKey(currencyKey))
			{
				DigestStorager.Instance.Set(currencyKey, num);
			}
			else if (!DigestStorager.Instance.Verify(currencyKey, num))
			{
				Switcher.AppendAbuseMethod(abuseMethod);
				UnityEngine.Debug.LogError(string.Concat("Currency tampering detected: ", Switcher.AbuseMethod));
			}
		}
	}

	private static void PerformExpendablesInitialization()
	{
		byte[] array = (new string[] { GearManager.InvisibilityPotion, GearManager.Jetpack, GearManager.Turret, GearManager.Mech }).SelectMany<string, byte>((string key) => BitConverter.GetBytes(Storager.getInt(key, false))).ToArray<byte>();
		if (!DigestStorager.Instance.ContainsKey("ExpendablesCount"))
		{
			DigestStorager.Instance.Set("ExpendablesCount", array);
		}
		else if (!DigestStorager.Instance.Verify("ExpendablesCount", array))
		{
			Switcher.AppendAbuseMethod(AbuseMetod.Expendables);
			UnityEngine.Debug.LogError(string.Concat("Expendables tampering detected: ", Switcher.AbuseMethod));
		}
	}

	[Obsolete("Because of issues with CryptoPlayerPrefs")]
	private static void PerformWeaponInitialization()
	{
		IEnumerable<string> values = 
			from w in WeaponManager.storeIDtoDefsSNMapping.Values
			where Storager.getInt(w, false) == 1
			select w;
		int num = values.Count<string>();
		if (!DigestStorager.Instance.ContainsKey("WeaponsCount"))
		{
			DigestStorager.Instance.Set("WeaponsCount", num);
		}
		else if (!DigestStorager.Instance.Verify("WeaponsCount", num))
		{
			Switcher.AppendAbuseMethod(AbuseMetod.Weapons);
			UnityEngine.Debug.LogError(string.Concat("Weapon tampering detected: ", Switcher.AbuseMethod));
		}
	}

	public static void PlayComicsSound()
	{
		if (Switcher.comicsSound != null)
		{
			return;
		}
		GameObject gameObject = Resources.Load<GameObject>("BackgroundMusic/Background_Comics");
		if (gameObject == null)
		{
			UnityEngine.Debug.LogWarning("ComicsSoundPrefab is null.");
			return;
		}
		Switcher.comicsSound = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		UnityEngine.Object.DontDestroyOnLoad(Switcher.comicsSound);
	}

	public static float SecondsFrom1970()
	{
		DateTime dateTime = new DateTime(1970, 1, 9, 0, 0, 0);
		return (float)(DateTime.Now - dateTime).TotalSeconds;
	}

	private static string SelectPhotonAppId(HiddenSettings settings)
	{
		byte[] numArray = Convert.FromBase64String(settings.PhotonAppIdSignaturePad);
		byte[] numArray1 = Convert.FromBase64String(settings.PhotonAppIdSignatureEncoded);
		byte[] signatureHash = AndroidSystem.Instance.GetSignatureHash();
		byte[] array = Enumerable.Repeat<byte[]>(signatureHash, 2147483647).SelectMany<byte[], byte>((byte[] bs) => bs).Take<byte>((int)numArray1.Length).ToArray<byte>();
		byte[] numArray2 = new byte[36];
		(new BitArray(numArray)).Xor(new BitArray(numArray1)).Xor(new BitArray(array)).CopyTo(numArray2, 0);
		string str = Encoding.UTF8.GetString(numArray2, 0, (int)numArray2.Length);
		return str;
	}

	private void SetUpPhoton(HiddenSettings settings)
	{
		string str = Switcher.SelectPhotonAppId(settings);
		if (Defs.IsDeveloperBuild)
		{
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ "appId", str },
				{ "defaultAppId", PhotonNetwork.PhotonServerSettings.AppID }
			};
		}
		PhotonNetwork.PhotonServerSettings.AppID = str;
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		Switcher.u003cStartu003ec__Iterator1CB variable = null;
		return variable;
	}
}