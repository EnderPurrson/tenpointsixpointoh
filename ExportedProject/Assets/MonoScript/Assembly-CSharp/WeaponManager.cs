using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class WeaponManager : MonoBehaviour
{
	public const int DefaultNumberOfMatchesForTryGuns = 3;

	private const string TryGunsTableServerKey = "TryGuns";

	public const string NumberOfMatchesKey = "NumberOfMatchesKey";

	public const string EquippedBeforeKey = "EquippedBeforeKey";

	public const string TryGunsDictionaryKey = "TryGunsDictionaryKey";

	public const string ExpiredTryGunsListKey = "ExpiredTryGunsListKey";

	public const string TryGunsKey = "WeaponManager.TryGunsKey";

	public const string TryGunsDiscountsKey = "WeaponManager.TryGunsDiscountsKey";

	public const string TryGunsDiscountsValuesKey = "WeaponManager.TryGunsDiscountsValuesKey";

	public const int NumOfWeaponCategories = 6;

	private const string SniperCategoryAddedToWeaponSetsKey = "WeaponManager_SniperCategoryAddedToWeaponSetsKey";

	private const string LastUsedWeaponsKey = "WeaponManager.LastUsedWeaponsKey";

	private static bool _buffsPAramsInitialized;

	private static Dictionary<ShopNGUIController.CategoryNames, List<List<string>>> _defaultTryGunsTable;

	private Dictionary<string, long> tryGunPromos = new Dictionary<string, long>();

	private Dictionary<string, SaltedLong> tryGunDiscounts = new Dictionary<string, SaltedLong>();

	public Dictionary<string, Dictionary<string, object>> TryGuns = new Dictionary<string, Dictionary<string, object>>();

	public List<string> ExpiredTryGuns = new List<string>();

	public static Dictionary<int, FilterMapSettings> WeaponSetSettingNamesForFilterMaps;

	public static List<string> GotchaGuns;

	public static List<KeyValuePair<string, string>> replaceConstWithTemp;

	public GameObject _grenadeWeaponCache;

	public GameObject _turretWeaponCache;

	public GameObject _rocketCache;

	public GameObject _turretCache;

	public static string WeaponPreviewsPath;

	public static string DaterFreeWeaponPrefabName;

	private static List<WeaponSounds> allWeaponPrefabs;

	public static List<GameObject> cachedInnerPrefabsForCurrentShopCategory;

	public static Dictionary<string, string> campaignBonusWeapons;

	public static Dictionary<string, string> tagToStoreIDMapping;

	public static Dictionary<string, string> storeIDtoDefsSNMapping;

	private readonly static HashSet<string> _purchasableWeaponSet;

	public static string _3_shotgun_2_WN;

	public static string _3_shotgun_3_WN;

	public static string flower_2_WN;

	public static string flower_3_WN;

	public static string gravity_2_WN;

	public static string gravity_3_WN;

	public static string grenade_launcher_3_WN;

	public static string revolver_2_2_WN;

	public static string revolver_2_3_WN;

	public static string scythe_3_WN;

	public static string plazma_2_WN;

	public static string plazma_3_WN;

	public static string plazma_pistol_2_WN;

	public static string plazma_pistol_3_WN;

	public static string railgun_2_WN;

	public static string railgun_3_WN;

	public static string Razer_3_WN;

	public static string tesla_3_WN;

	public static string Flamethrower_3_WN;

	public static string FreezeGun_0_WN;

	public static string svd_3_WN;

	public static string barret_3_WN;

	public static string minigun_3_WN;

	public static string LightSword_3_WN;

	public static string Sword_2_3_WN;

	public static string Staff_3_WN;

	public static string DragonGun_WN;

	public static string Bow_3_WN;

	public static string Bazooka_1_3_WN;

	public static string Bazooka_2_1_WN;

	public static string Bazooka_2_3_WN;

	public static string m79_2_WN;

	public static string m79_3_WN;

	public static string m32_1_2_WN;

	public static string Red_Stone_3_WN;

	public static string XM8_1_WN;

	public static string PumpkinGun_1_WN;

	public static string XM8_2_WN;

	public static string XM8_3_WN;

	public static string PumpkinGun_2_WN;

	public static string Rocketnitza_WN;

	public static WeaponManager sharedManager;

	public readonly static int LastNotNewWeapon;

	public List<string> shownWeapons = new List<string>();

	public HostData hostDataServer;

	public string ServerIp;

	public GameObject myPlayer;

	public Player_move_c myPlayerMoveC;

	public GameObject myGun;

	public GameObject myTable;

	public NetworkStartTable myNetworkStartTable;

	private UnityEngine.Object[] _weaponsInGame;

	private List<GameObject> _highMEmoryDevicesInnerPrefabsCache = new List<GameObject>();

	private UnityEngine.Object[] _multiWeapons;

	private UnityEngine.Object[] _hungerWeapons;

	private ArrayList _playerWeapons = new ArrayList();

	private ArrayList _allAvailablePlayerWeapons = new ArrayList();

	private int currentWeaponIndex;

	private Dictionary<string, int> lastUsedWeaponsForFilterMaps = new Dictionary<string, int>()
	{
		{ "0", 0 },
		{ "1", 2 },
		{ "2", 4 },
		{ "3", 2 }
	};

	public Camera useCam;

	private WeaponSounds _currentWeaponSounds = new WeaponSounds();

	private Dictionary<string, Action<string, int>> _purchaseActinos = new Dictionary<string, Action<string, int>>(300);

	public List<WeaponManager.infoClient> players = new List<WeaponManager.infoClient>();

	public List<List<GameObject>> _weaponsByCat = new List<List<GameObject>>();

	public List<List<GameObject>> FilteredShopLists;

	private List<GameObject> _playerWeaponsSetInnerPrefabsCache = new List<GameObject>();

	private int _lockGetWeaponPrefabs;

	private List<WeaponSounds> outerWeaponPrefabs;

	private static List<string> _Removed150615_Guns;

	private static List<string> _Removed150615_GunsPrefabNAmes;

	private static bool firstTagsForTiersInitialized;

	private static Dictionary<string, string> firstTagsWithRespecToOurTier;

	private static string[] oldTags;

	private bool _resetLock;

	public int _currentFilterMap;

	private bool _initialized;

	private static List<string> weaponsMovedToSniperCategory;

	private List<ShopPositionParams> _wearInfoPrefabsToCache = new List<ShopPositionParams>();

	private AnimationClip[] _profileAnimClips;

	private static Comparison<WeaponSounds> dpsComparerWS;

	private Comparison<GameObject> dpsComparer = new Comparison<GameObject>((GameObject leftw, GameObject rightw) => {
		if (leftw == null || rightw == null)
		{
			return 0;
		}
		WeaponSounds component = leftw.GetComponent<WeaponSounds>();
		WeaponSounds weaponSound = rightw.GetComponent<WeaponSounds>();
		return WeaponManager.dpsComparerWS(component, weaponSound);
	});

	public static string _3pl_shotgunWN
	{
		get
		{
			return "Weapon58";
		}
	}

	public static string _initialWeaponName
	{
		get
		{
			return "FirstPistol";
		}
	}

	public static string AK47WN
	{
		get
		{
			return "Weapon8";
		}
	}

	public static string AK74_2_WN
	{
		get
		{
			return "Weapon103";
		}
	}

	public static string AK74_3_WN
	{
		get
		{
			return "Weapon104";
		}
	}

	public static string AK74_WN
	{
		get
		{
			return "Weapon102";
		}
	}

	public static string AlienGunWN
	{
		get
		{
			return "Weapon52";
		}
	}

	public ArrayList allAvailablePlayerWeapons
	{
		get
		{
			return this._allAvailablePlayerWeapons;
		}
		private set
		{
			this._allAvailablePlayerWeapons = value;
		}
	}

	public bool AnyDiscountForTryGuns
	{
		get
		{
			return (this.tryGunPromos == null ? false : this.tryGunPromos.Count > 0);
		}
	}

	public static string AUG_2_WN
	{
		get
		{
			return "Weapon85";
		}
	}

	public static string AUG_WN
	{
		get
		{
			return "Weapon84";
		}
	}

	public static string Barrett_2WN
	{
		get
		{
			return "Weapon65";
		}
	}

	public static string BarrettWN
	{
		get
		{
			return "Weapon60";
		}
	}

	public static string BassCannon_WN
	{
		get
		{
			return "Weapon99";
		}
	}

	public static string Bazooka_2_WN
	{
		get
		{
			return "Weapon76";
		}
	}

	public static string Bazooka_3_WN
	{
		get
		{
			return "Weapon82";
		}
	}

	public static string Bazooka_WN
	{
		get
		{
			return "Weapon75";
		}
	}

	public static string Beretta_2_WN
	{
		get
		{
			return "Weapon71";
		}
	}

	public static string BerettaWN
	{
		get
		{
			return "Weapon25";
		}
	}

	public static string BlackEagleWN
	{
		get
		{
			return "Weapon41";
		}
	}

	public static string Buddy_WN
	{
		get
		{
			return "Weapon94";
		}
	}

	public static string BugGunWN
	{
		get
		{
			return "Weapon250";
		}
	}

	public static string CampaignRifle_WN
	{
		get
		{
			return "Weapon67";
		}
	}

	public static string Chainsaw2WN
	{
		get
		{
			return "Weapon45";
		}
	}

	public static string ChainsawWN
	{
		get
		{
			return "Weapon15";
		}
	}

	public static string CherryGun_WN
	{
		get
		{
			return "Weapon101";
		}
	}

	public static string CombatRifleWeaponName
	{
		get
		{
			return "Weapon10";
		}
	}

	public static string CrossbowWN
	{
		get
		{
			return "Weapon27";
		}
	}

	public static string CrystalAxeWN
	{
		get
		{
			return "Weapon42";
		}
	}

	public static string CrystalCrossbowWN
	{
		get
		{
			return "Weapon37";
		}
	}

	public static string CrystalGlockWN
	{
		get
		{
			return "Weapon54";
		}
	}

	public static string CrystalPickWN
	{
		get
		{
			return "Weapon30";
		}
	}

	public static string CrystalSPASWN
	{
		get
		{
			return "Weapon55";
		}
	}

	public int CurrentFilterMap
	{
		get
		{
			return this._currentFilterMap;
		}
	}

	public int CurrentWeaponIndex
	{
		get
		{
			return this.currentWeaponIndex;
		}
		set
		{
			this.currentWeaponIndex = value;
		}
	}

	public WeaponSounds currentWeaponSounds
	{
		get
		{
			return this._currentWeaponSounds;
		}
		set
		{
			this._currentWeaponSounds = value;
		}
	}

	public static string Eagle_3WN
	{
		get
		{
			return "Weapon64";
		}
	}

	public static string FAMASWN
	{
		get
		{
			return "Weapon16";
		}
	}

	public static string FireAxeWN
	{
		get
		{
			return "Weapon57";
		}
	}

	public static string Flamethrower_2_WN
	{
		get
		{
			return "Weapon74";
		}
	}

	public static string Flamethrower_WN
	{
		get
		{
			return "Weapon73";
		}
	}

	public static string Flower_WN
	{
		get
		{
			return "Weapon93";
		}
	}

	public static string FreezeGun_WN
	{
		get
		{
			return "Weapon105";
		}
	}

	public static string GlockWN
	{
		get
		{
			return "Weapon17";
		}
	}

	public static string GoldenAxeWeaponnName
	{
		get
		{
			return "Weapon14";
		}
	}

	public static string GoldenEagleWeaponName
	{
		get
		{
			return "Weapon11";
		}
	}

	public static string GoldenGlockWN
	{
		get
		{
			return "Weapon35";
		}
	}

	public static string GoldenPickWN
	{
		get
		{
			return "Weapon29";
		}
	}

	public static string GoldenRed_StoneWN
	{
		get
		{
			return "Weapon33";
		}
	}

	public static string GoldenSPASWN
	{
		get
		{
			return "Weapon34";
		}
	}

	public static string GoldenSwordWN
	{
		get
		{
			return "Weapon32";
		}
	}

	public static string Gravigun_WN
	{
		get
		{
			return "Weapon83";
		}
	}

	public static string GrenadeLunacher_2_WN
	{
		get
		{
			return "Weapon80";
		}
	}

	public static string GrenadeLunacher_WN
	{
		get
		{
			return "Weapon79";
		}
	}

	public static string Hammer2WN
	{
		get
		{
			return "Weapon47";
		}
	}

	public static string HammerWN
	{
		get
		{
			return "Weapon20";
		}
	}

	public bool Initialized
	{
		get
		{
			return this._initialized;
		}
	}

	public static string IronSwordWN
	{
		get
		{
			return "Weapon31";
		}
	}

	public static string katana_2_WN
	{
		get
		{
			return "Weapon89";
		}
	}

	public static string katana_3_WN
	{
		get
		{
			return "Weapon90";
		}
	}

	public static string katana_WN
	{
		get
		{
			return "Weapon88";
		}
	}

	public static string KnifeWN
	{
		get
		{
			return "Weapon9";
		}
	}

	public static string LaserRifleWN
	{
		get
		{
			return "Weapon23";
		}
	}

	public static string LightSwordWN
	{
		get
		{
			return "Weapon24";
		}
	}

	public int LockGetWeaponPrefabs
	{
		get
		{
			return this._lockGetWeaponPrefabs;
		}
	}

	public static string M16_2WN
	{
		get
		{
			return "Weapon53";
		}
	}

	public static string M16_3WN
	{
		get
		{
			return "Weapon69";
		}
	}

	public static string M16_4WN
	{
		get
		{
			return "Weapon70";
		}
	}

	public static string Mace2WN
	{
		get
		{
			return "Weapon48";
		}
	}

	public static string MaceWN
	{
		get
		{
			return "Weapon26";
		}
	}

	public static string MachinegunWN
	{
		get
		{
			return "Weapon5";
		}
	}

	public static string MagicBowWeaponName
	{
		get
		{
			return "Weapon12";
		}
	}

	public static string Mauser_WN
	{
		get
		{
			return "Weapon95";
		}
	}

	public static string MinigunWN
	{
		get
		{
			return "Weapon28";
		}
	}

	public static string MP5WN
	{
		get
		{
			return "Weapon3";
		}
	}

	public static string MultiplayerMeleeTag
	{
		get
		{
			return "Knife";
		}
	}

	public static string NavyFamasWN
	{
		get
		{
			return "Weapon62";
		}
	}

	public static string ObrezWN
	{
		get
		{
			return "Weapon51";
		}
	}

	public static string PickWeaponName
	{
		get
		{
			return "Weapon6";
		}
	}

	public static string PistolWN
	{
		get
		{
			return "Weapon1";
		}
	}

	public ArrayList playerWeapons
	{
		get
		{
			return this._playerWeapons;
		}
	}

	public static string plazma_pistol_WN
	{
		get
		{
			return "Weapon92";
		}
	}

	public static string plazma_WN
	{
		get
		{
			return "Weapon91";
		}
	}

	public static string Railgun_WN
	{
		get
		{
			return "Weapon77";
		}
	}

	public static string Razer_2_WN
	{
		get
		{
			return "Weapon87";
		}
	}

	public static string Razer_WN
	{
		get
		{
			return "Weapon86";
		}
	}

	public static string RedLightSaberWN
	{
		get
		{
			return "Weapon38";
		}
	}

	public static string RedMinigunWN
	{
		get
		{
			return "Weapon36";
		}
	}

	public static List<string> Removed150615_Guns
	{
		get
		{
			if (WeaponManager._Removed150615_Guns == null)
			{
				WeaponManager.InitializeRemoved150615Weapons();
			}
			return WeaponManager._Removed150615_Guns;
		}
	}

	public static List<string> Removed150615_PrefabNames
	{
		get
		{
			if (WeaponManager._Removed150615_Guns == null)
			{
				WeaponManager.InitializeRemoved150615Weapons();
			}
			return WeaponManager._Removed150615_GunsPrefabNAmes;
		}
	}

	public bool ResetLockSet
	{
		get
		{
			return this._resetLock;
		}
	}

	public static string Revolver2WN
	{
		get
		{
			return "Weapon59";
		}
	}

	public static string RevolverWN
	{
		get
		{
			return "Weapon4";
		}
	}

	public static string SandFamasWN
	{
		get
		{
			return "Weapon39";
		}
	}

	public static string Scythe_2_WN
	{
		get
		{
			return "Weapon68";
		}
	}

	public static string ScytheWN
	{
		get
		{
			return "Weapon18";
		}
	}

	public static string Shmaiser_WN
	{
		get
		{
			return "Weapon96";
		}
	}

	public int ShopListsTierConstraint
	{
		get
		{
			return 10000;
		}
	}

	public static string ShotgunWN
	{
		get
		{
			return "Weapon2";
		}
	}

	public static string ShovelWN
	{
		get
		{
			return "Weapon19";
		}
	}

	public static string SimpleFlamethrower_WN
	{
		get
		{
			return "Weapon333";
		}
	}

	public static string SocialGunWN
	{
		get
		{
			return "Weapon302";
		}
	}

	public static string SpakrlyBlaster_WN
	{
		get
		{
			return "Weapon100";
		}
	}

	public static string SpasWeaponName
	{
		get
		{
			return "Weapon13";
		}
	}

	public static string Staff2WN
	{
		get
		{
			return "Weapon50";
		}
	}

	public static string StaffWN
	{
		get
		{
			return "Weapon22";
		}
	}

	public static string SteelAxeWN
	{
		get
		{
			return "Weapon43";
		}
	}

	public static string SteelCrossbowWN
	{
		get
		{
			return "Weapon46";
		}
	}

	public static string svd_2WN
	{
		get
		{
			return "Weapon63";
		}
	}

	public static string svdWN
	{
		get
		{
			return "Weapon61";
		}
	}

	public static string Sword_2_WN
	{
		get
		{
			return "Weapon21";
		}
	}

	public static string Sword_22WN
	{
		get
		{
			return "Weapon49";
		}
	}

	public static string SwordWeaponName
	{
		get
		{
			return "Weapon7";
		}
	}

	public static string Tesla_2_WN
	{
		get
		{
			return "Weapon81";
		}
	}

	public static string Tesla_WN
	{
		get
		{
			return "Weapon78";
		}
	}

	public static string Thompson_2_WN
	{
		get
		{
			return "Weapon98";
		}
	}

	public static string Thompson_WN
	{
		get
		{
			return "Weapon97";
		}
	}

	public static string Tree_2_WN
	{
		get
		{
			return "Weapon72";
		}
	}

	public static string TreeWN
	{
		get
		{
			return "Weapon56";
		}
	}

	public Dictionary<string, long> TryGunPromos
	{
		get
		{
			return this.tryGunPromos;
		}
	}

	public static Dictionary<ShopNGUIController.CategoryNames, List<List<string>>> tryGunsTable
	{
		get
		{
			Dictionary<ShopNGUIController.CategoryNames, List<List<string>>> dictionary = null;
			try
			{
				if (!WeaponManager._buffsPAramsInitialized && !Storager.hasKey("BuffsParam"))
				{
					Storager.setString("BuffsParam", "{}", false);
				}
				WeaponManager._buffsPAramsInitialized = true;
				Dictionary<string, object> strs = Json.Deserialize(Storager.getString("BuffsParam", false)) as Dictionary<string, object>;
				if (strs != null && strs.ContainsKey("TryGuns"))
				{
					dictionary = (strs["TryGuns"] as Dictionary<string, object>).ToDictionary<KeyValuePair<string, object>, ShopNGUIController.CategoryNames, List<List<string>>>((KeyValuePair<string, object> kvp) => (ShopNGUIController.CategoryNames)((int)Enum.Parse(typeof(ShopNGUIController.CategoryNames), kvp.Key, true)), (KeyValuePair<string, object> kvp) => (
						from listObject in (kvp.Value as List<object>).OfType<List<object>>()
						select listObject.OfType<string>().ToList<string>()).ToList<List<string>>());
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogError(string.Concat("Exception in reading try guns table from storager: ", exception));
			}
			return (dictionary == null ? WeaponManager._defaultTryGunsTable : dictionary);
		}
	}

	public static string UZI_WN
	{
		get
		{
			return "Weapon66";
		}
	}

	public UnityEngine.Object[] weaponsInGame
	{
		get
		{
			return this._weaponsInGame;
		}
	}

	public static string WhiteBerettaWN
	{
		get
		{
			return "Weapon40";
		}
	}

	public static string WoodenBowWN
	{
		get
		{
			return "Weapon44";
		}
	}

	static WeaponManager()
	{
		Dictionary<int, FilterMapSettings> nums = new Dictionary<int, FilterMapSettings>();
		FilterMapSettings filterMapSetting = new FilterMapSettings()
		{
			settingName = Defs.MultiplayerWSSN,
			defaultWeaponSet = new Func<string>(WeaponManager._KnifeAndPistolAndMP5AndSniperAndRocketnitzaSet)
		};
		nums.Add(0, filterMapSetting);
		filterMapSetting = new FilterMapSettings()
		{
			settingName = "WeaponManager.KnifesModeWSSN",
			defaultWeaponSet = new Func<string>(WeaponManager._KnifeSet)
		};
		nums.Add(1, filterMapSetting);
		filterMapSetting = new FilterMapSettings()
		{
			settingName = "WeaponManager.SniperModeWSSN",
			defaultWeaponSet = new Func<string>(WeaponManager._KnifeAndPistolAndSniperSet)
		};
		nums.Add(2, filterMapSetting);
		filterMapSetting = new FilterMapSettings()
		{
			settingName = Defs.DaterWSSN,
			defaultWeaponSet = new Func<string>(WeaponManager._InitialDaterSet)
		};
		nums.Add(3, filterMapSetting);
		WeaponManager.WeaponSetSettingNamesForFilterMaps = nums;
		List<string> strs = new List<string>()
		{
			"gift_gun",
			"Candy_Baton",
			"mp5_gold_gift",
			WeaponTags.spark_shark_Tag,
			WeaponTags.power_claw_Tag
		};
		WeaponManager.GotchaGuns = strs;
		WeaponManager.replaceConstWithTemp = new List<KeyValuePair<string, string>>();
		WeaponManager.WeaponPreviewsPath = "WeaponPreviews";
		WeaponManager.DaterFreeWeaponPrefabName = "Weapon298";
		WeaponManager.allWeaponPrefabs = null;
		WeaponManager.cachedInnerPrefabsForCurrentShopCategory = new List<GameObject>();
		WeaponManager.campaignBonusWeapons = new Dictionary<string, string>();
		WeaponManager.tagToStoreIDMapping = new Dictionary<string, string>(200);
		WeaponManager.storeIDtoDefsSNMapping = new Dictionary<string, string>(200);
		WeaponManager._purchasableWeaponSet = new HashSet<string>();
		WeaponManager._3_shotgun_2_WN = "Weapon107";
		WeaponManager._3_shotgun_3_WN = "Weapon108";
		WeaponManager.flower_2_WN = "Weapon109";
		WeaponManager.flower_3_WN = "Weapon110";
		WeaponManager.gravity_2_WN = "Weapon111";
		WeaponManager.gravity_3_WN = "Weapon112";
		WeaponManager.grenade_launcher_3_WN = "Weapon113";
		WeaponManager.revolver_2_2_WN = "Weapon114";
		WeaponManager.revolver_2_3_WN = "Weapon115";
		WeaponManager.scythe_3_WN = "Weapon116";
		WeaponManager.plazma_2_WN = "Weapon117";
		WeaponManager.plazma_3_WN = "Weapon118";
		WeaponManager.plazma_pistol_2_WN = "Weapon119";
		WeaponManager.plazma_pistol_3_WN = "Weapon120";
		WeaponManager.railgun_2_WN = "Weapon121";
		WeaponManager.railgun_3_WN = "Weapon122";
		WeaponManager.Razer_3_WN = "Weapon123";
		WeaponManager.tesla_3_WN = "Weapon124";
		WeaponManager.Flamethrower_3_WN = "Weapon125";
		WeaponManager.FreezeGun_0_WN = "Weapon126";
		WeaponManager.svd_3_WN = "Weapon128";
		WeaponManager.barret_3_WN = "Weapon129";
		WeaponManager.minigun_3_WN = "Weapon127";
		WeaponManager.LightSword_3_WN = "Weapon130";
		WeaponManager.Sword_2_3_WN = "Weapon131";
		WeaponManager.Staff_3_WN = "Weapon132";
		WeaponManager.DragonGun_WN = "Weapon133";
		WeaponManager.Bow_3_WN = "Weapon134";
		WeaponManager.Bazooka_1_3_WN = "Weapon135";
		WeaponManager.Bazooka_2_1_WN = "Weapon136";
		WeaponManager.Bazooka_2_3_WN = "Weapon137";
		WeaponManager.m79_2_WN = "Weapon138";
		WeaponManager.m79_3_WN = "Weapon139";
		WeaponManager.m32_1_2_WN = "Weapon140";
		WeaponManager.Red_Stone_3_WN = "Weapon141";
		WeaponManager.XM8_1_WN = "Weapon142";
		WeaponManager.PumpkinGun_1_WN = "Weapon143";
		WeaponManager.XM8_2_WN = "Weapon144";
		WeaponManager.XM8_3_WN = "Weapon145";
		WeaponManager.PumpkinGun_2_WN = "Weapon147";
		WeaponManager.Rocketnitza_WN = "Weapon162";
		WeaponManager.sharedManager = null;
		WeaponManager.LastNotNewWeapon = 76;
		WeaponManager._Removed150615_Guns = null;
		WeaponManager._Removed150615_GunsPrefabNAmes = null;
		WeaponManager.firstTagsForTiersInitialized = false;
		WeaponManager.firstTagsWithRespecToOurTier = new Dictionary<string, string>();
		WeaponManager.oldTags = new string[] { WeaponTags.MinersWeaponTag, WeaponTags.Sword_2_3_Tag, WeaponTags.RailgunTag, WeaponTags.SteelAxeTag, WeaponTags.IronSwordTag, WeaponTags.Red_Stone_3_Tag, WeaponTags.SPASTag, WeaponTags.SteelCrossbowTag, WeaponTags.minigun_3_Tag, WeaponTags.LightSword_3_Tag, WeaponTags.FAMASTag, WeaponTags.FreezeGunTag, WeaponTags.BerettaTag, WeaponTags.EagleTag, WeaponTags.GlockTag, WeaponTags.svdTag, WeaponTags.m16Tag, WeaponTags.TreeTag, WeaponTags.revolver_2_3_Tag, WeaponTags.FreezeGun_0_Tag, WeaponTags.TeslaTag, WeaponTags.Bazooka_3Tag, WeaponTags.GrenadeLuancher_2Tag, WeaponTags.BazookaTag, WeaponTags.AUGTag, WeaponTags.AK74Tag, WeaponTags.GravigunTag, WeaponTags.XM8_1_Tag, WeaponTags.PumpkinGun_1_Tag, WeaponTags.SnowballMachingun_Tag, WeaponTags.SnowballGun_Tag, WeaponTags.HeavyShotgun_Tag, WeaponTags.TwoBolters_Tag, WeaponTags.TwoRevolvers_Tag, WeaponTags.AutoShotgun_Tag, WeaponTags.Solar_Ray_Tag, WeaponTags.Water_Pistol_Tag, WeaponTags.Solar_Power_Cannon_Tag, WeaponTags.Water_Rifle_Tag, WeaponTags.Valentine_Shotgun_Tag, WeaponTags.Needle_Throw_Tag, WeaponTags.Needle_Throw_Tag, WeaponTags.Carrot_Sword_Tag, WeaponTags._3_shotgun_3_Tag, WeaponTags.plazma_3_Tag, WeaponTags.katana_3_Tag, WeaponTags.DragonGun_Tag, WeaponTags.Bazooka_2_3_Tag, WeaponTags.buddy_Tag, WeaponTags.barret_3_Tag, WeaponTags.Flamethrower_3_Tag, WeaponTags.SparklyBlasterTag, WeaponTags.Thompson_2_Tag };
		strs = new List<string>()
		{
			"Weapon299",
			"Weapon322",
			"Weapon323",
			WeaponManager.CampaignRifle_WN,
			"Weapon44",
			"Weapon46",
			"Weapon61",
			"Weapon256",
			"Weapon77",
			"Weapon209",
			"Weapon65",
			"Weapon27",
			"Weapon63",
			"Weapon134",
			"Weapon37",
			"Weapon268",
			"Weapon121",
			"Weapon210",
			"Weapon251",
			"Weapon128",
			"Weapon269",
			"Weapon122",
			"Weapon211",
			"Weapon271",
			"Weapon221",
			"Weapon188",
			"Weapon192",
			"Weapon129",
			"Weapon241"
		};
		WeaponManager.weaponsMovedToSniperCategory = strs;
		WeaponManager.dpsComparerWS = (WeaponSounds leftWS, WeaponSounds rightWS) => {
			int num;
			if (ExpController.Instance == null || leftWS == null || rightWS == null)
			{
				return 0;
			}
			float dPS = leftWS.DPS - rightWS.DPS;
			if (dPS > 0f)
			{
				return 1;
			}
			if (dPS < 0f)
			{
				return -1;
			}
			try
			{
				ItemRecord byPrefabName = ItemDb.GetByPrefabName(leftWS.name.Replace("(Clone)", string.Empty));
				ItemPrice itemPrice = (!byPrefabName.CanBuy ? new ItemPrice(10, "Coins") : byPrefabName.Price);
				ItemRecord itemRecord = ItemDb.GetByPrefabName(rightWS.name.Replace("(Clone)", string.Empty));
				ItemPrice itemPrice1 = (!itemRecord.CanBuy ? new ItemPrice(10, "Coins") : itemRecord.Price);
				num = (!(itemPrice.Currency == "GemsCurrency") || !(itemPrice1.Currency == "Coins") ? (!(itemPrice.Currency == "Coins") || !(itemPrice1.Currency == "GemsCurrency") ? (itemPrice.Price.CompareTo(itemPrice1.Price) == 0 ? Array.IndexOf<string>(WeaponComparer.multiplayerWeaponsOrd, ItemDb.GetByPrefabName(leftWS.name.Replace("(Clone)", string.Empty)).Tag).CompareTo(Array.IndexOf<string>(WeaponComparer.multiplayerWeaponsOrd, ItemDb.GetByPrefabName(rightWS.name.Replace("(Clone)", string.Empty)).Tag)) : itemPrice.Price.CompareTo(itemPrice1.Price)) : -1) : 1);
			}
			catch
			{
				num = 0;
			}
			return num;
		};
		WeaponManager.WeaponEquipped = null;
		WeaponManager._buffsPAramsInitialized = false;
		Dictionary<ShopNGUIController.CategoryNames, List<List<string>>> categoryNames = new Dictionary<ShopNGUIController.CategoryNames, List<List<string>>>();
		List<List<string>> lists = new List<List<string>>();
		strs = new List<string>()
		{
			"Weapon127",
			"Weapon142",
			"Weapon206",
			"Weapon167"
		};
		lists.Add(strs);
		strs = new List<string>()
		{
			"Weapon163",
			"Weapon141"
		};
		lists.Add(strs);
		strs = new List<string>()
		{
			"Weapon84"
		};
		lists.Add(strs);
		lists.Add(new List<string>());
		lists.Add(new List<string>());
		strs = new List<string>()
		{
			"Weapon220"
		};
		lists.Add(strs);
		categoryNames.Add(ShopNGUIController.CategoryNames.PrimaryCategory, lists);
		lists = new List<List<string>>();
		strs = new List<string>()
		{
			"Weapon160",
			"Weapon203"
		};
		lists.Add(strs);
		lists.Add(new List<string>());
		lists.Add(new List<string>());
		lists.Add(new List<string>());
		strs = new List<string>()
		{
			"Weapon308"
		};
		lists.Add(strs);
		strs = new List<string>()
		{
			"Weapon223"
		};
		lists.Add(strs);
		categoryNames.Add(ShopNGUIController.CategoryNames.BackupCategory, lists);
		lists = new List<List<string>>()
		{
			new List<string>(),
			new List<string>(),
			new List<string>(),
			new List<string>(),
			new List<string>(),
			new List<string>()
		};
		categoryNames.Add(ShopNGUIController.CategoryNames.MeleeCategory, lists);
		lists = new List<List<string>>();
		strs = new List<string>()
		{
			"Weapon178"
		};
		lists.Add(strs);
		strs = new List<string>()
		{
			"Weapon105"
		};
		lists.Add(strs);
		lists.Add(new List<string>());
		lists.Add(new List<string>());
		strs = new List<string>()
		{
			"Weapon306"
		};
		lists.Add(strs);
		lists.Add(new List<string>());
		categoryNames.Add(ShopNGUIController.CategoryNames.SpecilCategory, lists);
		lists = new List<List<string>>();
		strs = new List<string>()
		{
			"Weapon77",
			"Weapon209"
		};
		lists.Add(strs);
		strs = new List<string>()
		{
			"Weapon339"
		};
		lists.Add(strs);
		lists.Add(new List<string>());
		strs = new List<string>()
		{
			"Weapon251"
		};
		lists.Add(strs);
		lists.Add(new List<string>());
		strs = new List<string>()
		{
			"Weapon221"
		};
		lists.Add(strs);
		categoryNames.Add(ShopNGUIController.CategoryNames.SniperCategory, lists);
		lists = new List<List<string>>();
		strs = new List<string>()
		{
			"Weapon82",
			"Weapon212"
		};
		lists.Add(strs);
		strs = new List<string>()
		{
			"Weapon180"
		};
		lists.Add(strs);
		strs = new List<string>()
		{
			"Weapon133",
			"Weapon253",
			"Weapon99"
		};
		lists.Add(strs);
		lists.Add(new List<string>());
		strs = new List<string>()
		{
			"Weapon161"
		};
		lists.Add(strs);
		lists.Add(new List<string>());
		categoryNames.Add(ShopNGUIController.CategoryNames.PremiumCategory, lists);
		WeaponManager._defaultTryGunsTable = categoryNames;
		ItemDb.Fill_tagToStoreIDMapping(WeaponManager.tagToStoreIDMapping);
		ItemDb.Fill_storeIDtoDefsSNMapping(WeaponManager.storeIDtoDefsSNMapping);
		WeaponManager._purchasableWeaponSet.UnionWith(WeaponManager.storeIDtoDefsSNMapping.Values);
	}

	public WeaponManager()
	{
	}

	private void _AddWeaponToShopListsIfNeeded(GameObject w)
	{
		WeaponSounds component = w.GetComponent<WeaponSounds>();
		bool flag = false;
		bool flag1 = false;
		List<string> strs = null;
		string tag = "Undefined";
		try
		{
			tag = ItemDb.GetByPrefabName(w.name.Replace("(Clone)", string.Empty)).Tag;
		}
		catch (UnityException unityException1)
		{
			UnityException unityException = unityException1;
			UnityEngine.Debug.LogError("Tag issue encountered.");
			UnityEngine.Debug.LogException(unityException);
		}
		foreach (List<string> upgrade in WeaponUpgrades.upgrades)
		{
			if (!upgrade.Contains(tag))
			{
				continue;
			}
			flag1 = true;
			strs = upgrade;
			break;
		}
		if (!flag1)
		{
			Lazy<string> lazy = new Lazy<string>(() => {
				string str;
				string str1;
				if (!WeaponManager.tagToStoreIDMapping.TryGetValue(tag, out str))
				{
					UnityEngine.Debug.LogError(string.Concat("Weapon tag not found in tagToStoreIDMapping: ", tag));
					return string.Empty;
				}
				if (WeaponManager.storeIDtoDefsSNMapping.TryGetValue(str, out str1))
				{
					return str1;
				}
				UnityEngine.Debug.LogError(string.Concat("Weapon name not found in storeIDtoDefsSNMapping: ", str1));
				return string.Empty;
			});
			if (TrainingController.TrainingCompleted || !(tag == WeaponTags.BASIC_FLAMETHROWER_Tag) && !(tag == WeaponTags.SignalPistol_Tag))
			{
				flag = ((!(ExpController.Instance != null) || ExpController.Instance.OurTier < component.tier) && Storager.getInt(lazy.Value, true) != 1 ? false : (!WeaponManager.Removed150615_Guns.Contains(WeaponUpgrades.TagOfFirstUpgrade(tag)) ? 0 : (int)(WeaponManager.LastBoughtTag(tag) == null)) == 0);
			}
			else
			{
				flag = false;
			}
		}
		else
		{
			int num = strs.IndexOf(tag);
			if (Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[tag]], true) != 1)
			{
				string str2 = WeaponManager.FirstTagForOurTier(tag);
				if ((num > 0 && (str2 != null && str2.Equals(tag) || Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[strs[num - 1]]], true) == 1) && component.tier < 100 || num == 0 && str2 != null && str2.Equals(tag) && ExpController.Instance != null && ExpController.Instance.OurTier >= component.tier) && (!WeaponManager.Removed150615_Guns.Contains(WeaponUpgrades.TagOfFirstUpgrade(tag)) || WeaponManager.LastBoughtTag(tag) != null))
				{
					flag = true;
				}
			}
			else if (num == strs.Count - 1)
			{
				flag = true;
			}
			else if (num < strs.Count - 1 && Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[strs[num + 1]]], true) == 0)
			{
				flag = true;
			}
		}
		if (flag)
		{
			try
			{
				this._weaponsByCat[component.categoryNabor - 1].Add(w);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				if (Application.isEditor || UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.LogError(string.Concat("WeaponManager: exception: ", exception));
				}
			}
		}
	}

	public static string _InitialDaterSet()
	{
		return string.Concat("##", WeaponManager.DaterFreeWeaponPrefabName, "###");
	}

	private void _InitShopCategoryLists(int filterMap = 0)
	{
		bool flag = Defs.isMulti;
		bool flag1 = (!flag ? false : Defs.isHunger);
		bool flag2 = (Defs.IsSurvival || !TrainingController.TrainingCompleted ? false : !flag);
		string[] strArrays = Storager.getString(Defs.WeaponsGotInCampaign, false).Split(new char[] { '#' });
		List<string> strs = new List<string>();
		string[] strArrays1 = strArrays;
		for (int i = 0; i < (int)strArrays1.Length; i++)
		{
			strs.Add(strArrays1[i]);
		}
		foreach (List<GameObject> gameObjects in this._weaponsByCat)
		{
			gameObjects.Clear();
		}
		this.AddTempGunsToShopCategoryLists(filterMap, flag1);
		if (flag && !flag1 || Defs.IsSurvival && TrainingController.TrainingCompleted)
		{
			UnityEngine.Object[] objArray = this.weaponsInGame;
			for (int j = 0; j < (int)objArray.Length; j++)
			{
				GameObject gameObject = (GameObject)objArray[j];
				string tag = ItemDb.GetByPrefabName(gameObject.name).Tag;
				WeaponSounds component = gameObject.GetComponent<WeaponSounds>();
				if (gameObject.name == WeaponManager.DaterFreeWeaponPrefabName)
				{
					if (filterMap == 3)
					{
						this._weaponsByCat[component.categoryNabor - 1].Add(gameObject);
					}
				}
				else if (!component.campaignOnly)
				{
					if (gameObject.name.Equals(WeaponManager.AlienGunWN))
					{
						!strs.Contains(WeaponManager.AlienGunWN);
					}
					else if (gameObject.name.Equals(WeaponManager.BugGunWN))
					{
						if (strs.Contains(WeaponManager.BugGunWN))
						{
							this._weaponsByCat[component.categoryNabor - 1].Add(gameObject);
						}
					}
					else if (gameObject.name.Equals(WeaponManager.SocialGunWN))
					{
						if (Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) > 0)
						{
							this._weaponsByCat[component.categoryNabor - 1].Add(gameObject);
						}
					}
					else if (tag != null && WeaponManager.GotchaGuns.Contains(tag))
					{
						if (Storager.getInt(tag, true) > 0)
						{
							this._weaponsByCat[component.categoryNabor - 1].Add(gameObject);
						}
					}
					else if (!ItemDb.IsTemporaryGun(tag))
					{
						this._AddWeaponToShopListsIfNeeded(gameObject);
					}
				}
			}
			this._SortShopLists();
			return;
		}
		if (!flag2)
		{
			if (!flag1)
			{
				return;
			}
			UnityEngine.Object[] objArray1 = this.weaponsInGame;
			int num = 0;
			while (num < (int)objArray1.Length)
			{
				GameObject gameObject1 = (GameObject)objArray1[num];
				if (!gameObject1.name.Equals(WeaponManager.KnifeWN))
				{
					num++;
				}
				else
				{
					this._AddWeaponToShopListsIfNeeded(gameObject1);
					break;
				}
			}
			this._SortShopLists();
			return;
		}
		UnityEngine.Object[] objArray2 = this.weaponsInGame;
		for (int k = 0; k < (int)objArray2.Length; k++)
		{
			GameObject gameObject2 = (GameObject)objArray2[k];
			string str = ItemDb.GetByPrefabName(gameObject2.name).Tag;
			WeaponSounds weaponSound = gameObject2.GetComponent<WeaponSounds>();
			if (gameObject2.name != WeaponManager.DaterFreeWeaponPrefabName)
			{
				if (weaponSound.campaignOnly || gameObject2.name.Equals(WeaponManager.BugGunWN) || gameObject2.name.Equals(WeaponManager.AlienGunWN) || gameObject2.name.Equals(WeaponManager.MP5WN) || gameObject2.name.Equals(WeaponManager.CampaignRifle_WN) || gameObject2.name.Equals(WeaponManager.SimpleFlamethrower_WN) || gameObject2.name.Equals(WeaponManager.Rocketnitza_WN))
				{
					if (strs.Contains(gameObject2.name))
					{
						this._weaponsByCat[weaponSound.categoryNabor - 1].Add(gameObject2);
					}
				}
				else if (gameObject2.name.Equals(WeaponManager.SocialGunWN))
				{
					if (Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) > 0)
					{
						this._weaponsByCat[weaponSound.categoryNabor - 1].Add(gameObject2);
					}
				}
				else if (str != null && WeaponManager.GotchaGuns.Contains(str))
				{
					if (Storager.getInt(str, true) > 0)
					{
						this._weaponsByCat[weaponSound.categoryNabor - 1].Add(gameObject2);
					}
				}
				else if (!ItemDb.IsTemporaryGun(str))
				{
					this._AddWeaponToShopListsIfNeeded(gameObject2);
				}
			}
		}
		this._SortShopLists();
	}

	public static string _KnifeAndPistolAndMP5AndSniperAndRocketnitzaSet()
	{
		string[] mP5WN = new string[] { WeaponManager.MP5WN, "#", WeaponManager.PistolWN, "#", WeaponManager.KnifeWN, "#", null, null, null, null, null };
		mP5WN[6] = (!TrainingController.TrainingCompleted ? string.Empty : WeaponManager.SimpleFlamethrower_WN);
		mP5WN[7] = "#";
		mP5WN[8] = (!TrainingController.TrainingCompleted ? string.Empty : WeaponManager.CampaignRifle_WN);
		mP5WN[9] = "#";
		mP5WN[10] = (!TrainingController.TrainingCompleted ? string.Empty : WeaponManager.Rocketnitza_WN);
		return string.Concat(mP5WN);
	}

	public static string _KnifeAndPistolAndShotgunSet()
	{
		return string.Concat(new string[] { WeaponManager.ShotgunWN, "#", WeaponManager.PistolWN, "#", WeaponManager.KnifeWN, "###" });
	}

	public static string _KnifeAndPistolAndSniperSet()
	{
		return string.Concat(new string[] { "#", WeaponManager.PistolWN, "#", WeaponManager.KnifeWN, "##", WeaponManager.CampaignRifle_WN, "#" });
	}

	public static string _KnifeAndPistolSet()
	{
		return string.Concat(new string[] { "#", WeaponManager.PistolWN, "#", WeaponManager.KnifeWN, "###" });
	}

	public static string _KnifeSet()
	{
		return string.Concat("##", WeaponManager.KnifeWN, "###");
	}

	private int _RemovePrevVersionsOfUpgrade(string tg)
	{
		int count = 0;
		foreach (List<string> upgrade in WeaponUpgrades.upgrades)
		{
			int num = upgrade.IndexOf(tg);
			if (num == -1)
			{
				continue;
			}
			for (int i = 0; i < num; i++)
			{
				List<Weapon> weapons = new List<Weapon>();
				for (int j = 0; j < this.allAvailablePlayerWeapons.Count; j++)
				{
					Weapon item = this.allAvailablePlayerWeapons[j] as Weapon;
					if (ItemDb.GetByPrefabName(item.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag.Equals(upgrade[i]))
					{
						weapons.Add(item);
					}
				}
				for (int k = 0; k < weapons.Count; k++)
				{
					this.allAvailablePlayerWeapons.Remove(weapons[k]);
				}
				count += weapons.Count;
			}
			break;
		}
		return count;
	}

	private void _SortShopLists()
	{
		for (int i = 0; i < 6; i++)
		{
			Dictionary<string, List<GameObject>> strs = new Dictionary<string, List<GameObject>>();
			foreach (GameObject item in this._weaponsByCat[i])
			{
				string str = WeaponUpgrades.TagOfFirstUpgrade(ItemDb.GetByPrefabName(item.name.Replace("(Clone)", string.Empty)).Tag);
				if (!strs.ContainsKey(str))
				{
					strs.Add(str, new List<GameObject>()
					{
						item
					});
				}
				else
				{
					strs[str].Add(item);
				}
			}
			List<List<GameObject>> list = strs.Values.ToList<List<GameObject>>();
			foreach (List<GameObject> gameObjects in list)
			{
				if (gameObjects.Count <= 1)
				{
					continue;
				}
				gameObjects.Sort(this.dpsComparer);
			}
			List<List<GameObject>> lists = new List<List<GameObject>>();
			List<List<GameObject>> lists1 = new List<List<GameObject>>();
			foreach (List<GameObject> gameObjects1 in list)
			{
				((!ItemDb.IsCanBuy(WeaponUpgrades.TagOfFirstUpgrade(ItemDb.GetByPrefabName(gameObjects1[0].name.Replace("(Clone)", string.Empty)).Tag)) ? lists1 : lists)).Add(gameObjects1);
			}
			Comparison<List<GameObject>> comparison = (List<GameObject> leftList, List<GameObject> rightList) => {
				if (leftList == null || rightList == null || leftList.Count < 1 || rightList.Count < 1)
				{
					return 0;
				}
				WeaponSounds component = leftList[0].GetComponent<WeaponSounds>();
				WeaponSounds weaponSound = rightList[0].GetComponent<WeaponSounds>();
				return WeaponManager.dpsComparerWS(component, weaponSound);
			};
			lists.Sort(comparison);
			lists1.Sort(comparison);
			List<GameObject> gameObjects2 = new List<GameObject>();
			foreach (List<GameObject> list1 in lists1)
			{
				gameObjects2.AddRange(list1);
			}
			foreach (List<GameObject> list2 in lists)
			{
				gameObjects2.AddRange(list2);
			}
			this._weaponsByCat[i] = gameObjects2;
		}
	}

	private void _UpdateShopCategList(Weapon w)
	{
		WeaponSounds component = w.weaponPrefab.GetComponent<WeaponSounds>();
		string tag = ItemDb.GetByPrefabName(w.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag;
		if (!WeaponManager.tagToStoreIDMapping.ContainsKey(tag))
		{
			this._weaponsByCat[component.categoryNabor - 1].Add(w.weaponPrefab);
		}
		else
		{
			bool flag = false;
			List<string> strs = null;
			foreach (List<string> upgrade in WeaponUpgrades.upgrades)
			{
				if (!upgrade.Contains(tag))
				{
					continue;
				}
				strs = upgrade;
				flag = true;
				break;
			}
			if (flag)
			{
				int num = strs.IndexOf(ItemDb.GetByPrefabName(w.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag);
				int num1 = -1;
				foreach (GameObject item in this._weaponsByCat[component.categoryNabor - 1])
				{
					if (item.name.Replace("(Clone)", string.Empty) != w.weaponPrefab.name.Replace("(Clone)", string.Empty))
					{
						continue;
					}
					num1 = this._weaponsByCat[component.categoryNabor - 1].IndexOf(item);
					break;
				}
				if (num >= strs.Count - 1)
				{
					string str = ItemDb.GetByPrefabName(w.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag;
					string str1 = WeaponManager.FirstTagForOurTier(str);
					if (str1 == null || !str1.Equals(str))
					{
						this._weaponsByCat[component.categoryNabor - 1].RemoveAt(num1 - 1);
					}
				}
				else
				{
					GameObject gameObject = null;
					foreach (WeaponSounds weaponSound in WeaponManager.AllWrapperPrefabs())
					{
						if (weaponSound.name != ItemDb.GetByTag(strs[num + 1]).PrefabName)
						{
							continue;
						}
						gameObject = weaponSound.gameObject;
						break;
					}
					if (num1 == -1)
					{
						UnityEngine.Debug.LogWarning(string.Concat("_UpdateShopCategList: prevInd = -1   ws.categoryNabor - 1: ", component.categoryNabor - 1));
					}
					else
					{
						string tag1 = ItemDb.GetByPrefabName(w.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag;
						string str2 = WeaponManager.FirstTagForOurTier(tag1);
						if (num > 0 && (str2 == null || !str2.Equals(tag1)))
						{
							this._weaponsByCat[component.categoryNabor - 1].RemoveAt(num1 - 1);
						}
						this._weaponsByCat[component.categoryNabor - 1].Insert(num1, gameObject);
					}
				}
			}
		}
		this._SortShopLists();
	}

	private bool _WeaponAvailable(GameObject prefab, List<string> weaponsGotInCampaign, int filterMap)
	{
		bool flag;
		string tag = ItemDb.GetByPrefabName(prefab.name.Replace("(Clone)", string.Empty)).Tag;
		bool flag1 = Defs.isMulti;
		bool flag2 = Defs.isHunger;
		bool flag3 = (Defs.IsSurvival || !TrainingController.TrainingCompleted ? false : !flag1);
		if (flag1 && filterMap == 3 && prefab.name.Equals(WeaponManager.DaterFreeWeaponPrefabName))
		{
			return true;
		}
		if (prefab.name.Replace("(Clone)", string.Empty) == WeaponManager.KnifeWN)
		{
			return true;
		}
		if (prefab.name.Replace("(Clone)", string.Empty) == WeaponManager.PistolWN && !flag2)
		{
			return true;
		}
		if (prefab.name.Equals(WeaponManager.ShotgunWN) && !flag2)
		{
			return true;
		}
		if (prefab.name.Equals(WeaponManager.MP5WN) && (flag1 || Defs.IsSurvival) && !flag2)
		{
			return true;
		}
		if (prefab.name.Equals(WeaponManager.CampaignRifle_WN) && (flag1 || Defs.IsSurvival) && !flag2)
		{
			return true;
		}
		if (prefab.name.Equals(WeaponManager.SimpleFlamethrower_WN) && (flag1 || Defs.IsSurvival) && !flag2)
		{
			return true;
		}
		if (prefab.name.Equals(WeaponManager.Rocketnitza_WN) && (flag1 || Defs.IsSurvival) && !flag2)
		{
			return true;
		}
		WeaponSounds component = prefab.GetComponent<WeaponSounds>();
		if (!flag2 && tag != null && TempItemsController.sharedController.ContainsItem(tag) && (filterMap == 0 || component.filterMap != null && component.filterMap.Contains(filterMap)))
		{
			return true;
		}
		if (flag3 && LevelBox.weaponsFromBosses.ContainsValue(prefab.name) && weaponsGotInCampaign.Contains(prefab.name))
		{
			return true;
		}
		bool flag4 = (!prefab.name.Equals(WeaponManager.BugGunWN) ? false : weaponsGotInCampaign.Contains(WeaponManager.BugGunWN));
		if (Defs.IsSurvival && TrainingController.TrainingCompleted && !flag1 && flag4)
		{
			return true;
		}
		if (!Defs.IsSurvival && TrainingController.TrainingCompleted && flag1 && !flag2 && flag4)
		{
			return true;
		}
		if (!prefab.name.Equals(WeaponManager.SocialGunWN) || Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) <= 0)
		{
			flag = (tag == null || !WeaponManager.GotchaGuns.Contains(tag) ? false : Storager.getInt(tag, true) > 0);
		}
		else
		{
			flag = true;
		}
		bool flag5 = flag;
		if ((Defs.IsSurvival && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None) && !flag1 || !Defs.IsSurvival && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None) && flag1 && !flag2 || flag3 && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None)) && flag5)
		{
			return true;
		}
		return false;
	}

	public static void ActualizeWeaponsForCampaignProgress()
	{
		string str;
		try
		{
			string[] strArrays = Storager.getString(Defs.WeaponsGotInCampaign, false).Split(new char[] { '#' });
			List<string> strs = new List<string>();
			string[] strArrays1 = strArrays;
			for (int i = 0; i < (int)strArrays1.Length; i++)
			{
				strs.Add(strArrays1[i]);
			}
			foreach (string key in CampaignProgress.boxesLevelsAndStars.Keys)
			{
				foreach (string key1 in CampaignProgress.boxesLevelsAndStars[key].Keys)
				{
					if (!LevelBox.weaponsFromBosses.TryGetValue(key1, out str) || strs.Contains(str))
					{
						continue;
					}
					strs.Add(str);
				}
			}
			if (strs.Contains(WeaponManager.ShotgunWN))
			{
				strs[strs.IndexOf(WeaponManager.ShotgunWN)] = WeaponManager.UZI_WN;
			}
			string str1 = string.Join("#", strs.ToArray());
			Storager.setString(Defs.WeaponsGotInCampaign, str1, false);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in ActualizeWeaponsForCampaignProgress: ", exception));
		}
	}

	public bool AddAmmo(int idx = -1)
	{
		if (idx == -1)
		{
			idx = this.allAvailablePlayerWeapons.IndexOf(this.playerWeapons[this.CurrentWeaponIndex]);
		}
		if (this.allAvailablePlayerWeapons[idx] == this.playerWeapons[this.CurrentWeaponIndex] && this.currentWeaponSounds.isMelee && !this.currentWeaponSounds.isShotMelee)
		{
			return false;
		}
		Weapon item = (Weapon)this.allAvailablePlayerWeapons[idx];
		WeaponSounds component = item.weaponPrefab.GetComponent<WeaponSounds>();
		if (item.currentAmmoInBackpack >= component.MaxAmmoWithEffectApplied)
		{
			return false;
		}
		Weapon weapon = item;
		weapon.currentAmmoInBackpack = weapon.currentAmmoInBackpack + (!(this.currentWeaponSounds != null) || !this.currentWeaponSounds.isShotMelee && !(this.currentWeaponSounds.name.Replace("(Clone)", string.Empty) == "Weapon335") && !(this.currentWeaponSounds.name.Replace("(Clone)", string.Empty) == "Weapon353") && !(this.currentWeaponSounds.name.Replace("(Clone)", string.Empty) == "Weapon354") ? component.ammoInClip : component.ammoForBonusShotMelee);
		if (item.currentAmmoInBackpack > component.MaxAmmoWithEffectApplied)
		{
			item.currentAmmoInBackpack = component.MaxAmmoWithEffectApplied;
		}
		return true;
	}

	public bool AddAmmoForAllGuns()
	{
		bool flag = false;
		for (int i = 0; i < this.playerWeapons.Count; i++)
		{
			Weapon item = (Weapon)this.playerWeapons[i];
			WeaponSounds component = item.weaponPrefab.GetComponent<WeaponSounds>();
			if (!component.isMelee || component.isShotMelee)
			{
				if (item.currentAmmoInBackpack < component.MaxAmmoWithEffectApplied)
				{
					Weapon weapon = item;
					weapon.currentAmmoInBackpack = weapon.currentAmmoInBackpack + (!(component != null) || !component.isShotMelee && !(component.name.Replace("(Clone)", string.Empty) == "Weapon335") && !(component.name.Replace("(Clone)", string.Empty) == "Weapon353") && !(component.name.Replace("(Clone)", string.Empty) == "Weapon354") ? component.ammoInClip : component.ammoForBonusShotMelee);
					if (item.currentAmmoInBackpack > component.MaxAmmoWithEffectApplied)
					{
						item.currentAmmoInBackpack = component.MaxAmmoWithEffectApplied;
					}
					flag = true;
				}
			}
		}
		return flag;
	}

	public static void AddExclusiveWeaponToWeaponStructures(string prefabName)
	{
		int num;
		if (string.IsNullOrEmpty(prefabName))
		{
			UnityEngine.Debug.LogError("Error in AddExclusiveWeaponToWeaponStructures: string.IsNullOrEmpty(prefabName)");
			return;
		}
		WeaponManager.SetRememberedTierForWeapon(prefabName);
		if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.Initialized)
		{
			GameObject gameObject = null;
			try
			{
				gameObject = WeaponManager.sharedManager.weaponsInGame.OfType<GameObject>().FirstOrDefault<GameObject>((GameObject w) => w.name.Equals(prefabName));
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogError(string.Concat("Exception in AddExclusiveWeaponToWeaponStructures: ", exception));
			}
			if (gameObject != null)
			{
				WeaponManager.sharedManager.AddWeapon(gameObject, out num);
			}
		}
	}

	private void AddInnerPrefabToCacheForHighMemoryDeivces(WeaponSounds ws)
	{
	}

	public void AddMinerWeapon(string id, int timeForRentIndex = 0)
	{
		if (id == null)
		{
			throw new ArgumentNullException("id");
		}
		if (this._purchaseActinos.ContainsKey(id))
		{
			this._purchaseActinos[id](id, timeForRentIndex);
		}
	}

	public static GameObject AddRay(Vector3 pos, Vector3 forw, string nm, float len = 150f)
	{
		Transform child;
		GameObject objectFromName = RayAndExplosionsStackController.sharedController.GetObjectFromName(ResPath.Combine("Rays", nm));
		if (objectFromName == null)
		{
			return null;
		}
		Transform transforms = objectFromName.transform;
		if (transforms.childCount <= 0)
		{
			child = null;
		}
		else
		{
			child = transforms.GetChild(0);
		}
		Transform transforms1 = child;
		if (transforms1 != null)
		{
			transforms1.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0f, 0f, len));
		}
		objectFromName.transform.position = pos;
		objectFromName.transform.forward = forw;
		return objectFromName;
	}

	private void AddTempGunsToShopCategoryLists(int filterMap, bool isHungry)
	{
		if (!isHungry && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None))
		{
			try
			{
				IEnumerable<WeaponSounds> weaponSounds = 
					from o in this.weaponsInGame.OfType<GameObject>()
					select o.GetComponent<WeaponSounds>() into ws
					where (!ItemDb.IsTemporaryGun(ItemDb.GetByPrefabName(ws.name.Replace("(Clone)", string.Empty)).Tag) ? false : (TempItemsController.sharedController == null ? false : TempItemsController.sharedController.ContainsItem(ItemDb.GetByPrefabName(ws.name.Replace("(Clone)", string.Empty)).Tag)))
					select ws;
				if (filterMap != 0)
				{
					weaponSounds = 
						from ws in weaponSounds
						where (ws.filterMap == null ? false : ws.filterMap.Contains(filterMap))
						select ws;
				}
				IEnumerator<WeaponSounds> enumerator = weaponSounds.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						WeaponSounds current = enumerator.Current;
						this._weaponsByCat[current.categoryNabor - 1].Add(current.gameObject);
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
			catch (Exception exception)
			{
				UnityEngine.Debug.LogWarning(string.Concat("Exception ", exception));
			}
		}
	}

	public void AddTryGun(string tg)
	{
		try
		{
			int num = 3;
			try
			{
				num = (!FriendsController.useBuffSystem ? KillRateCheck.instance.GetRoundsForGun() : BuffSystem.instance.GetRoundsForGun());
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogError(string.Concat("Exception in numOfMatches = KillRateCheck.instance.GetRoundsForGun(): ", exception));
			}
			Dictionary<string, Dictionary<string, object>> tryGuns = this.TryGuns;
			Dictionary<string, object> strs = new Dictionary<string, object>()
			{
				{ "NumberOfMatchesKey", new SaltedInt(52394, num) }
			};
			tryGuns.Add(tg, strs);
			Weapon allAvailable = this.AddWeaponWithTagToAllAvailable(tg);
			WeaponSounds component = allAvailable.weaponPrefab.GetComponent<WeaponSounds>();
			string tag = null;
			try
			{
				tag = ItemDb.GetByPrefabName(this.playerWeapons.OfType<Weapon>().FirstOrDefault<Weapon>((Weapon w) => w.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor == component.categoryNabor).weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag;
			}
			catch (Exception exception1)
			{
				UnityEngine.Debug.LogWarning(string.Concat("Exception in try guns get equipped before: ", exception1));
			}
			this.TryGuns[tg].Add("EquippedBeforeKey", tag ?? string.Empty);
			this.EquipWeapon(allAvailable, true, false);
			WeaponManager.sharedManager.SaveWeaponAsLastUsed(WeaponManager.sharedManager.CurrentWeaponIndex);
		}
		catch (Exception exception2)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in AddTryGun: ", exception2));
		}
	}

	public void AddTryGunPromo(string tg)
	{
		if (tg == null)
		{
			UnityEngine.Debug.LogError("AddTryGunPromo tg == null");
			return;
		}
		this.tryGunPromos.Add(tg, PromoActionsManager.CurrentUnixTime);
		int price = WeaponManager.BaseTryGunDiscount();
		try
		{
			string currency = ItemDb.GetByTag(tg).Price.Currency;
			int num = Storager.getInt(currency, false);
			int num1 = ShopNGUIController.PriceIfGunWillBeTryGun(tg);
			bool flag = currency == "GemsCurrency";
			int index = ((!flag ? BankView.goldPurchasesInfo : BankView.gemsPurchasesInfo))[0].Index;
			int num2 = (!flag ? VirtualCurrencyHelper.GetCoinInappsQuantity(index) : VirtualCurrencyHelper.GetGemsInappsQuantity(index));
			if (num1 > num + num2)
			{
				int num3 = num + num2 - 1;
				ItemPrice itemPrice = ShopNGUIController.currentPrice(tg, (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(tg), false, false);
				price = (int)((1f - (float)num3 / (float)itemPrice.Price) * 100f) + 1;
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in AddTryGunPromo: ", exception));
		}
		price = Mathf.Min(70, price);
		this.tryGunDiscounts.Add(tg, new SaltedLong((long)685488, (long)price));
	}

	public bool AddWeapon(GameObject weaponPrefab, out int score)
	{
		bool flag;
		score = 0;
		if (TempItemsController.PriceCoefs.ContainsKey(ItemDb.GetByPrefabName(weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag) && (SceneLoader.ActiveSceneName.Equals("ConnectScene") || this._currentFilterMap != 0 && !weaponPrefab.GetComponent<WeaponSounds>().IsAvalibleFromFilter(this._currentFilterMap) || Defs.isHunger))
		{
			return false;
		}
		bool flag1 = false;
		IEnumerator enumerator = this.allAvailablePlayerWeapons.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Weapon current = (Weapon)enumerator.Current;
				if (current.weaponPrefab.name.Replace("(Clone)", string.Empty) != weaponPrefab.name.Replace("(Clone)", string.Empty))
				{
					continue;
				}
				if (!this.AddAmmo(this.allAvailablePlayerWeapons.IndexOf(current)))
				{
					score += Defs.ScoreForSurplusAmmo;
				}
				if (ItemDb.IsTemporaryGun(ItemDb.GetByPrefabName(weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag) || this.IsAvailableTryGun(ItemDb.GetByPrefabName(weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag))
				{
					flag1 = true;
				}
				else
				{
					flag = false;
					return flag;
				}
			}
			goto Label0;
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
		return flag;
	Label0:
		Weapon weapon = new Weapon()
		{
			weaponPrefab = weaponPrefab,
			currentAmmoInBackpack = weapon.weaponPrefab.GetComponent<WeaponSounds>().InitialAmmoWithEffectsApplied,
			currentAmmoInClip = weapon.weaponPrefab.GetComponent<WeaponSounds>().ammoInClip
		};
		if (flag1)
		{
			int num = -1;
			IEnumerator enumerator1 = this.allAvailablePlayerWeapons.GetEnumerator();
			try
			{
				while (enumerator1.MoveNext())
				{
					Weapon current1 = (Weapon)enumerator1.Current;
					if (!current1.weaponPrefab.name.Equals(weaponPrefab.name))
					{
						continue;
					}
					num = this.allAvailablePlayerWeapons.IndexOf(current1);
					break;
				}
			}
			finally
			{
				IDisposable disposable1 = enumerator1 as IDisposable;
				if (disposable1 == null)
				{
				}
				disposable1.Dispose();
			}
			if (num > -1 && num < this.allAvailablePlayerWeapons.Count)
			{
				this.allAvailablePlayerWeapons[num] = weapon;
			}
		}
		else
		{
			this.allAvailablePlayerWeapons.Add(weapon);
		}
		string tag = ItemDb.GetByPrefabName(weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag;
		this._RemovePrevVersionsOfUpgrade(tag);
		bool flag2 = true;
		List<string> strs = new List<string>()
		{
			WeaponManager.CampaignRifle_WN,
			WeaponManager.AlienGunWN,
			WeaponManager.SimpleFlamethrower_WN,
			WeaponManager.BugGunWN,
			WeaponManager.Rocketnitza_WN
		};
		List<string> strs1 = strs;
		WeaponSounds component = weapon.weaponPrefab.GetComponent<WeaponSounds>();
		if (component.campaignOnly || weapon.weaponPrefab.name.Replace("(Clone)", string.Empty) == WeaponManager.MP5WN || strs1.Contains(weapon.weaponPrefab.name.Replace("(Clone)", string.Empty)))
		{
			try
			{
				if (this.CurrentWeaponIndex >= 0 && this.CurrentWeaponIndex < this.playerWeapons.Count)
				{
					Weapon item = this.playerWeapons[this.CurrentWeaponIndex] as Weapon;
					if (item != null)
					{
						ItemRecord byPrefabName = ItemDb.GetByPrefabName(item.weaponPrefab.nameNoClone());
						if (byPrefabName != null && WeaponManager.tagToStoreIDMapping.ContainsKey(byPrefabName.Tag))
						{
							flag2 = false;
						}
					}
				}
				WeaponSounds weaponSound = (
					from w in this.playerWeapons.OfType<Weapon>()
					select w.weaponPrefab.GetComponent<WeaponSounds>()).FirstOrDefault<WeaponSounds>((WeaponSounds ws) => ws.categoryNabor == component.categoryNabor);
				if (weaponSound != null && WeaponManager.tagToStoreIDMapping.ContainsKey(ItemDb.GetByPrefabName(weaponSound.name.Replace("(Clone)", string.Empty)).Tag))
				{
					flag2 = false;
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogError(string.Concat("Exception in finding weapon of checking notBoughtToCampaign: ", exception));
				flag2 = false;
			}
		}
		if (flag2)
		{
			this.EquipWeapon(weapon, true, false);
			this.SaveWeaponAsLastUsed(this.CurrentWeaponIndex);
		}
		this._UpdateShopCategList(weapon);
		this.UpdateFilteredShopLists();
		return flag2;
	}

	private void AddWeapon(GooglePurchase p)
	{
		try
		{
			this.AddMinerWeapon(p.productId, 0);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(exception);
		}
	}

	public void AddWeaponToInv(string shopId, int timeForRentIndex = 0)
	{
		int num;
		string tagByShopId = ItemDb.GetTagByShopId(shopId);
		Player_move_c.SaveWeaponInPrefs(tagByShopId, timeForRentIndex);
		GameObject prefabByTag = this.GetPrefabByTag(tagByShopId);
		if (prefabByTag != null)
		{
			this.AddWeapon(prefabByTag, out num);
		}
	}

	private Weapon AddWeaponWithTagToAllAvailable(string tagToAdd)
	{
		Weapon weapon;
		try
		{
			WeaponSounds weaponSound = Resources.Load<WeaponSounds>(string.Concat("Weapons/", ItemDb.GetByTag(tagToAdd).PrefabName));
			Weapon weapon1 = new Weapon()
			{
				weaponPrefab = weaponSound.gameObject,
				currentAmmoInBackpack = weaponSound.InitialAmmoWithEffectsApplied,
				currentAmmoInClip = weaponSound.ammoInClip
			};
			this.allAvailablePlayerWeapons.Add(weapon1);
			weapon = weapon1;
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in AddWeaponWithTagToAllAvailable: ", exception));
			weapon = null;
		}
		return weapon;
	}

	private static List<string> AllWeaponSetsSettingNames()
	{
		List<string> strs = new List<string>()
		{
			Defs.CampaignWSSN
		};
		return strs.Concat<string>(
			from fms in WeaponManager.WeaponSetSettingNamesForFilterMaps.Values
			select fms.settingName).ToList<string>();
	}

	public static List<WeaponSounds> AllWrapperPrefabs()
	{
		if (WeaponManager.allWeaponPrefabs == null)
		{
			WeaponManager.allWeaponPrefabs = Resources.LoadAll<WeaponSounds>("Weapons").ToList<WeaponSounds>();
		}
		return WeaponManager.allWeaponPrefabs;
	}

	private void Awake()
	{
		ScopeLogger scopeLogger = new ScopeLogger("WeaponManager.Awake()", Defs.IsDeveloperBuild);
		try
		{
			if (Storager.getInt("WeaponManager_SniperCategoryAddedToWeaponSetsKey", false) == 0)
			{
				foreach (string str in WeaponManager.AllWeaponSetsSettingNames())
				{
					try
					{
						if (!Storager.hasKey(str))
						{
							Storager.setString(str, this.DefaultSetForWeaponSetSettingName(str), false);
						}
						else
						{
							string str1 = Storager.getString(str, false);
							if (str1 == null)
							{
								UnityEngine.Debug.LogError(string.Concat("Adding sniper category to weapon sets error: weaponSet == null  wssn = ", str));
							}
							int num = str1.LastIndexOf("#");
							if (num == -1)
							{
								UnityEngine.Debug.LogError(string.Concat("Adding sniper category to weapon sets error: lastIndexOfHash == -1  wssn = ", str, "  weaponSet = ", str1));
							}
							str1 = str1.Insert(num, "#");
							string[] empty = str1.Split(new char[] { '#' });
							if (empty == null)
							{
								UnityEngine.Debug.LogError(string.Concat("Adding sniper category to weapon sets error: splittedWeaponSet == null  wssn = ", str, "  weaponSet = ", str1));
							}
							bool flag = true;
							if ((int)empty.Length > 6)
							{
								UnityEngine.Debug.LogError(string.Concat("Adding sniper category to weapon sets error: splittedWeaponSet.Length > NumOfWeaponCategories  wssn = ", str, "  weaponSet = ", str1));
								Storager.setString(str, this.DefaultSetForWeaponSetSettingName(str), false);
								flag = false;
							}
							if ((int)empty.Length < 6)
							{
								UnityEngine.Debug.LogError(string.Concat("Adding sniper category to weapon sets error: splittedWeaponSet.Length < NumOfWeaponCategories  wssn = ", str, "  weaponSet = ", str1));
								Storager.setString(str, this.DefaultSetForWeaponSetSettingName(str), false);
								flag = false;
							}
							if (flag)
							{
								for (int i = 0; i < (int)empty.Length; i++)
								{
									if (empty[i] == null)
									{
										empty[i] = string.Empty;
									}
								}
								Dictionary<ShopNGUIController.CategoryNames, string> categoryNames = new Dictionary<ShopNGUIController.CategoryNames, string>();
								for (int j = 0; j < (int)empty.Length; j++)
								{
									if (empty[j] != null && WeaponManager.weaponsMovedToSniperCategory.Contains(empty[j]))
									{
										categoryNames.Add((ShopNGUIController.CategoryNames)j, empty[j]);
										empty[j] = string.Empty;
										if (j != 3)
										{
											if (j == 0)
											{
												if (str == Defs.MultiplayerWSSN)
												{
													empty[j] = WeaponManager.MP5WN;
												}
												else if (str == Defs.CampaignWSSN)
												{
													empty[j] = "Weapon2";
												}
												else if (str == Defs.DaterWSSN)
												{
													empty[j] = string.Empty;
												}
											}
										}
										else if (str == Defs.MultiplayerWSSN)
										{
											empty[j] = WeaponManager.SimpleFlamethrower_WN;
										}
										else if (str == Defs.CampaignWSSN)
										{
											Dictionary<string, Dictionary<string, int>> strs = CampaignProgress.boxesLevelsAndStars;
											Dictionary<string, int> strs1 = new Dictionary<string, int>();
											bool flag1 = false;
											if (strs.TryGetValue("minecraft", out strs1) && strs1.ContainsKey("Maze"))
											{
												empty[j] = WeaponManager.SimpleFlamethrower_WN;
												flag1 = true;
											}
											if (!flag1 && strs.TryGetValue("Real", out strs1) && strs1.ContainsKey("Jail"))
											{
												empty[j] = WeaponManager.MachinegunWN;
												flag1 = true;
											}
										}
									}
								}
								int num1 = 4;
								Action<string> length = (string weaponName) => {
									if ((int)empty.Length <= num1)
									{
										UnityEngine.Debug.LogError(string.Concat(new object[] { "Adding sniper category to weapon sets error: splittedWeaponSet.Length > newSniperIndex    newSniperIndex: ", num1, "   wssn = ", str, "   weaponSet = ", str1 }));
									}
									else
									{
										empty[num1] = weaponName;
									}
								};
								if (categoryNames.Values.Count > 0)
								{
									length(categoryNames.Values.FirstOrDefault<string>() ?? string.Empty);
								}
								else if (str == Defs.MultiplayerWSSN)
								{
									length(WeaponManager.CampaignRifle_WN);
								}
								else if (str == Defs.CampaignWSSN)
								{
									Dictionary<string, Dictionary<string, int>> strs2 = CampaignProgress.boxesLevelsAndStars;
									Dictionary<string, int> strs3 = new Dictionary<string, int>();
									if (strs2.TryGetValue("minecraft", out strs3) && strs3.ContainsKey("Utopia"))
									{
										length(WeaponManager.CampaignRifle_WN);
									}
								}
								if (empty[3] == "Weapon317" || empty[3] == "Weapon318" || empty[3] == "Weapon319")
								{
									if (str == Defs.MultiplayerWSSN)
									{
										empty[3] = WeaponManager.SimpleFlamethrower_WN;
									}
									else if (str == Defs.CampaignWSSN)
									{
										Dictionary<string, Dictionary<string, int>> strs4 = CampaignProgress.boxesLevelsAndStars;
										Dictionary<string, int> strs5 = new Dictionary<string, int>();
										bool flag2 = false;
										if (strs4.TryGetValue("minecraft", out strs5) && strs5.ContainsKey("Maze"))
										{
											empty[3] = WeaponManager.SimpleFlamethrower_WN;
											flag2 = true;
										}
										if (!flag2 && strs4.TryGetValue("Real", out strs5) && strs5.ContainsKey("Jail"))
										{
											empty[3] = WeaponManager.MachinegunWN;
											flag2 = true;
										}
									}
								}
								Storager.setString(str, string.Join("#", empty), false);
							}
						}
					}
					catch (Exception exception3)
					{
						Exception exception = exception3;
						UnityEngine.Debug.LogError(string.Concat(new object[] { "Exceptio in foreach (var wssn in AllWeaponSetsSettingNames())  wssn = ", str, "   exception: ", exception }));
						try
						{
							Storager.setString(str, this.DefaultSetForWeaponSetSettingName(str), false);
						}
						catch (Exception exception2)
						{
							Exception exception1 = exception2;
							UnityEngine.Debug.LogError(string.Concat(new object[] { "Exceptio in Storager.setString (wssn, DefaultSetForWeaponSetSettingName(wssn),false);  wssn = ", str, "   exception: ", exception1 }));
						}
					}
				}
				Storager.setInt("WeaponManager_SniperCategoryAddedToWeaponSetsKey", 1, false);
			}
			if (Storager.hasKey("WeaponManager.LastUsedWeaponsKey"))
			{
				try
				{
					Dictionary<string, object> strs6 = Json.Deserialize(Storager.getString("WeaponManager.LastUsedWeaponsKey", false)) as Dictionary<string, object>;
					foreach (string key in strs6.Keys)
					{
						this.lastUsedWeaponsForFilterMaps[key] = (int)((long)strs6[key]);
					}
				}
				catch (Exception exception4)
				{
					UnityEngine.Debug.LogError(string.Concat("Loading last used weapons: ", exception4));
				}
			}
			else
			{
				this.SaveLastUsedWeapons();
			}
			this.LoadTryGunsInfo();
			this.LoadTryGunDiscounts();
		}
		finally
		{
			scopeLogger.Dispose();
		}
		this.LoadWearInfoPrefabsToCache();
	}

	public static int BaseTryGunDiscount()
	{
		int num = (!FriendsController.useBuffSystem ? 50 : 50);
		try
		{
			num = (!FriendsController.useBuffSystem ? KillRateCheck.instance.discountValue : BuffSystem.instance.discountValue);
			num = Math.Max(0, num);
			num = Math.Min(75, num);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in getting KillRateCheck.instance.discountValue: ", exception));
		}
		return num;
	}

	public static void ClearCachedInnerPrefabs()
	{
		WeaponManager.cachedInnerPrefabsForCurrentShopCategory.Clear();
		if (Device.IsLoweMemoryDevice)
		{
			Resources.UnloadUnusedAssets();
		}
	}

	public int CurrentIndexOfLastUsedWeaponInPlayerWeapons()
	{
		if (Defs.isHunger)
		{
			return 0;
		}
		int num = 0;
		try
		{
			if (this.lastUsedWeaponsForFilterMaps.ContainsKey(this._currentFilterMap.ToString()))
			{
				int item = this.lastUsedWeaponsForFilterMaps[this._currentFilterMap.ToString()];
				int num1 = this.playerWeapons.Cast<Weapon>().ToList<Weapon>().FindIndex((Weapon w) => w.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1 == item);
				if (num1 != -1)
				{
					num = num1;
				}
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in CurrentIndexOfLastUsedWeaponInPlayerWeapons: ", exception));
			num = 0;
		}
		return num;
	}

	public void DecreaseTryGunsMatchesCount()
	{
		if (Defs.isHunger)
		{
			return;
		}
		try
		{
			List<string> strs = new List<string>();
			foreach (KeyValuePair<string, Dictionary<string, object>> tryGun in this.TryGuns)
			{
				if (this.weaponsInGame.FirstOrDefault<UnityEngine.Object>((UnityEngine.Object w) => ItemDb.GetByPrefabName(w.name).Tag == tryGun.Key) != null)
				{
					SaltedInt item = (SaltedInt)tryGun.Value["NumberOfMatchesKey"];
					int num = Math.Max(0, item.Value - 1);
					tryGun.Value["NumberOfMatchesKey"] = new SaltedInt(838318, num);
					if (num != 0)
					{
						continue;
					}
					strs.Add(tryGun.Key);
				}
			}
			foreach (string str in strs)
			{
				this.RemoveTryGun(str);
			}
			if (strs.Count > 0)
			{
				Action action = WeaponManager.TryGunRemoved;
				if (action != null)
				{
					action();
				}
				if (!FriendsController.useBuffSystem)
				{
					KillRateCheck.OnGunTakeOff();
				}
				else
				{
					BuffSystem.instance.OnGunTakeOff();
				}
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in DecreaseTryGunsMatchesCount: ", exception));
		}
	}

	private string DefaultSetForWeaponSetSettingName(string sn)
	{
		string value = WeaponManager._KnifeAndPistolAndShotgunSet();
		if (sn != Defs.CampaignWSSN)
		{
			try
			{
				KeyValuePair<int, FilterMapSettings> keyValuePair = (
					from kvp in WeaponManager.WeaponSetSettingNamesForFilterMaps
					where kvp.Value.settingName == sn
					select kvp).FirstOrDefault<KeyValuePair<int, FilterMapSettings>>();
				value = keyValuePair.Value.defaultWeaponSet();
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				UnityEngine.Debug.LogError(string.Concat(new object[] { "Exception in LoadWeaponSet: sn = ", sn, "    exception: ", exception }));
			}
		}
		return value;
	}

	public long DiscountForTryGun(string tg)
	{
		if (tg == null)
		{
			return (long)0;
		}
		if (this.tryGunDiscounts == null || !this.tryGunDiscounts.ContainsKey(tg))
		{
			return (long)WeaponManager.BaseTryGunDiscount();
		}
		return this.tryGunDiscounts[tg].Value;
	}

	public void EquipWeapon(Weapon w, bool shouldSave = true, bool shouldEquipToDaterSetOnly = false)
	{
		if (w == null)
		{
			UnityEngine.Debug.LogWarning("Exiting from EquipWeapon(), because weapon is null.");
			return;
		}
		WeaponSounds component = w.weaponPrefab.GetComponent<WeaponSounds>();
		int num = component.categoryNabor;
		bool flag = false;
		int num1 = 0;
		while (num1 < this.playerWeapons.Count)
		{
			if ((this.playerWeapons[num1] as Weapon).weaponPrefab.GetComponent<WeaponSounds>().categoryNabor != num)
			{
				num1++;
			}
			else
			{
				flag = true;
				this.playerWeapons[num1] = w;
				this.UpdatePlayersWeaponSetCache();
				break;
			}
		}
		if (!flag)
		{
			this.playerWeapons.Add(w);
			this.UpdatePlayersWeaponSetCache();
		}
		this.playerWeapons.Sort(new WeaponComparer());
		this.playerWeapons.Reverse();
		this.CurrentWeaponIndex = this.playerWeapons.IndexOf(w);
		if (!shouldSave)
		{
			return;
		}
		string[] strArrays = Storager.getString(Defs.WeaponsGotInCampaign, false).Split(new char[] { '#' });
		List<string> strs = new List<string>();
		string[] strArrays1 = strArrays;
		for (int i = 0; i < (int)strArrays1.Length; i++)
		{
			strs.Add(strArrays1[i]);
		}
		bool flag1 = ((!(w.weaponPrefab.name == WeaponManager.Rocketnitza_WN) || strs.Contains(WeaponManager.Rocketnitza_WN)) && (!w.weaponPrefab.name.Equals(WeaponManager.MP5WN) || strs.Contains(WeaponManager.MP5WN)) && (!w.weaponPrefab.name.Equals(WeaponManager.CampaignRifle_WN) || strs.Contains(WeaponManager.CampaignRifle_WN)) ? (!w.weaponPrefab.name.Equals(WeaponManager.SimpleFlamethrower_WN) ? 0 : (int)(!strs.Contains(WeaponManager.SimpleFlamethrower_WN))) == 0 : false);
		if (Defs.isMulti)
		{
			if (!Defs.isHunger)
			{
				if (!shouldEquipToDaterSetOnly || !Defs.isDaterRegim)
				{
					this.SetWeaponInAppropriateMultyModes(component);
				}
				else
				{
					this.SaveWeaponSet(Defs.DaterWSSN, w.weaponPrefab.name, num - 1);
				}
				if (flag1)
				{
					this.SaveWeaponSet(Defs.CampaignWSSN, w.weaponPrefab.name, num - 1);
				}
			}
			else if (SceneLoader.ActiveSceneName == "ConnectScene" || SceneLoader.ActiveSceneName == "ConnectSceneSandbox")
			{
				this.SetWeaponInAppropriateMultyModes(component);
				if (flag1)
				{
					this.SaveWeaponSet(Defs.CampaignWSSN, w.weaponPrefab.name, num - 1);
				}
			}
		}
		else if (!Defs.IsSurvival && TrainingController.TrainingCompleted)
		{
			if (!w.weaponPrefab.GetComponent<WeaponSounds>().campaignOnly && !w.weaponPrefab.name.Equals(WeaponManager.AlienGunWN))
			{
				this.SetWeaponInAppropriateMultyModes(component);
			}
			this.SaveWeaponSet(Defs.CampaignWSSN, w.weaponPrefab.name, num - 1);
		}
		else if (Defs.IsSurvival && TrainingController.TrainingCompleted && !w.weaponPrefab.GetComponent<WeaponSounds>().campaignOnly && !w.weaponPrefab.name.Equals(WeaponManager.AlienGunWN))
		{
			this.SetWeaponInAppropriateMultyModes(component);
			if (flag1)
			{
				this.SaveWeaponSet(Defs.CampaignWSSN, w.weaponPrefab.name, num - 1);
			}
		}
		if (WeaponManager.WeaponEquipped != null)
		{
			WeaponManager.WeaponEquipped(num - 1);
		}
	}

	public static string FirstTagForOurTier(string tg)
	{
		string item;
		if (tg == null)
		{
			return null;
		}
		if (!WeaponManager.firstTagsForTiersInitialized)
		{
			WeaponManager.InitFirstTagsData();
			WeaponManager.firstTagsForTiersInitialized = true;
		}
		List<string> strs = WeaponUpgrades.ChainForTag(tg);
		if (strs == null || strs.Count <= 0)
		{
			return null;
		}
		if (!WeaponManager.firstTagsWithRespecToOurTier.ContainsKey(strs[0]))
		{
			item = null;
		}
		else
		{
			item = WeaponManager.firstTagsWithRespecToOurTier[strs[0]];
		}
		return item;
	}

	public static string FirstUnboughtOrForOurTier(string tg)
	{
		string str = WeaponManager.FirstUnboughtTag(tg);
		if (WeaponManager.tagToStoreIDMapping.ContainsKey(tg))
		{
			string str1 = WeaponManager.FirstTagForOurTier(tg);
			List<string> strs = WeaponUpgrades.ChainForTag(tg);
			if (str1 != null && strs != null && strs.IndexOf(str1) > strs.IndexOf(str))
			{
				str = str1;
			}
		}
		return str;
	}

	public static string FirstUnboughtTag(string tg)
	{
		string item;
		if (tg == null)
		{
			return null;
		}
		if (tg == "Armor_Novice")
		{
			return "Armor_Novice";
		}
		if (!WeaponManager.tagToStoreIDMapping.ContainsKey(tg))
		{
			if (TempItemsController.PriceCoefs.ContainsKey(tg))
			{
				return tg;
			}
			Dictionary<ShopNGUIController.CategoryNames, List<List<string>>>.Enumerator enumerator = Wear.wear.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					List<List<string>>.Enumerator enumerator1 = enumerator.Current.Value.GetEnumerator();
					try
					{
						while (enumerator1.MoveNext())
						{
							List<string> current = enumerator1.Current;
							if (!current.Contains(tg))
							{
								continue;
							}
							int num = 0;
							while (num < current.Count)
							{
								if (Storager.getInt(current[num], true) != 0)
								{
									num++;
								}
								else
								{
									item = current[num];
									return item;
								}
							}
							item = current[current.Count - 1];
							return item;
						}
					}
					finally
					{
						((IDisposable)(object)enumerator1).Dispose();
					}
				}
				return tg;
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			return item;
		}
		bool flag = false;
		List<string> strs = null;
		foreach (List<string> upgrade in WeaponUpgrades.upgrades)
		{
			if (!upgrade.Contains(tg))
			{
				continue;
			}
			strs = upgrade;
			flag = true;
			break;
		}
		if (!flag)
		{
			return tg;
		}
		for (int i = strs.Count - 1; i >= 0; i--)
		{
			if (Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[strs[i]]], true) == 1)
			{
				if (i >= strs.Count - 1)
				{
					return strs[i];
				}
				return strs[i + 1];
			}
		}
		return strs[0];
	}

	private void FixWeaponsDueToCategoriesMoved911()
	{
		Storager.setInt(Defs.FixWeapons911, 1, false);
		Storager.setString(Defs.MultiplayerWSSN, this.DefaultSetForWeaponSetSettingName(Defs.MultiplayerWSSN), false);
		Storager.setString(Defs.CampaignWSSN, this.DefaultSetForWeaponSetSettingName(Defs.CampaignWSSN), false);
	}

	public GameObject GetPrefabByTag(string weaponTag)
	{
		return (
			from ws in WeaponManager.AllWrapperPrefabs()
			select ws.gameObject).FirstOrDefault<GameObject>((GameObject w) => ItemDb.GetByPrefabName(w.name).Tag.Equals(weaponTag));
	}

	public void GetWeaponPrefabs(int filterMap = 0)
	{
		IEnumerator weaponPrefabsCoroutine = this.GetWeaponPrefabsCoroutine(filterMap);
		while (weaponPrefabsCoroutine.MoveNext())
		{
			object current = weaponPrefabsCoroutine.Current;
		}
	}

	[DebuggerHidden]
	private IEnumerator GetWeaponPrefabsCoroutine(int filterMap = 0)
	{
		WeaponManager.u003cGetWeaponPrefabsCoroutineu003ec__Iterator19E variable = null;
		return variable;
	}

	public static List<string> GetWeaponsForBuy()
	{
		List<string> strs = new List<string>();
		string[] canBuyWeaponTags = ItemDb.GetCanBuyWeaponTags(false);
		for (int i = 0; i < (int)canBuyWeaponTags.Length; i++)
		{
			string str = canBuyWeaponTags[i];
			if (WeaponManager.tagToStoreIDMapping.ContainsKey(str) && !ItemDb.IsTemporaryGun(str))
			{
				strs.Add(str);
			}
		}
		List<string> strs1 = PromoActionsGUIController.FilterPurchases(strs, true, true, false, false);
		return strs.Except<string>(strs1).ToList<string>();
	}

	private static void InitFirstTagsData()
	{
		if (!Storager.hasKey("FirstTagsForOurTier"))
		{
			Storager.setString("FirstTagsForOurTier", "{}", false);
		}
		string str = Storager.getString("FirstTagsForOurTier", false);
		try
		{
			foreach (KeyValuePair<string, object> keyValuePair in Json.Deserialize(str) as Dictionary<string, object>)
			{
				WeaponManager.firstTagsWithRespecToOurTier.Add(keyValuePair.Key, (string)keyValuePair.Value);
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
			return;
		}
		foreach (List<string> upgrade in WeaponUpgrades.upgrades)
		{
			if (upgrade.Count != 0)
			{
				if (WeaponManager.firstTagsWithRespecToOurTier.ContainsKey(upgrade[0]))
				{
					continue;
				}
				if (!WeaponManager.OldChainThatAlwaysShownFromStart(upgrade[0]))
				{
					List<WeaponSounds> list = (
						from ws in WeaponManager.AllWrapperPrefabs()
						where upgrade.Contains(ItemDb.GetByPrefabName(ws.name.Replace("(Clone)", string.Empty)).Tag)
						select ws).ToList<WeaponSounds>();
					bool flag = false;
					int num = 0;
					while (num < upgrade.Count)
					{
						WeaponSounds weaponSound = list.Find((WeaponSounds ws) => ItemDb.GetByPrefabName(ws.name.Replace("(Clone)", string.Empty)).Tag.Equals(upgrade[num]));
						if (!(weaponSound != null) || weaponSound.tier <= ExpController.GetOurTier())
						{
							num++;
						}
						else
						{
							if (num != 0)
							{
								WeaponManager.firstTagsWithRespecToOurTier.Add(upgrade[0], upgrade[num - 1]);
							}
							else
							{
								WeaponManager.firstTagsWithRespecToOurTier.Add(upgrade[0], upgrade[0]);
							}
							flag = true;
							break;
						}
					}
					if (flag)
					{
						continue;
					}
					WeaponManager.firstTagsWithRespecToOurTier.Add(upgrade[0], upgrade[upgrade.Count - 1]);
				}
				else
				{
					WeaponManager.firstTagsWithRespecToOurTier.Add(upgrade[0], upgrade[0]);
				}
			}
		}
		Storager.setString("FirstTagsForOurTier", Json.Serialize(WeaponManager.firstTagsWithRespecToOurTier), false);
	}

	private static void InitializeRemoved150615Weapons()
	{
		List<string> strs = new List<string>()
		{
			"Weapon20",
			"Weapon47",
			"Weapon50",
			"Weapon57",
			"Weapon95",
			"Weapon96",
			"Weapon97",
			"Weapon98",
			"Weapon101",
			"Weapon110",
			"Weapon120",
			"Weapon123",
			"Weapon129",
			"Weapon132",
			"Weapon137",
			"Weapon139",
			"Weapon165",
			"Weapon170",
			"Weapon171",
			"Weapon189",
			"Weapon190",
			"Weapon191",
			"Weapon241",
			"Weapon247",
			"Weapon94",
			"Weapon244",
			"Weapon245",
			"Weapon285",
			"Weapon289",
			"Weapon290",
			"Weapon134",
			"Weapon181",
			"Weapon182",
			"Weapon183",
			"Weapon310",
			"Weapon315",
			"Weapon316",
			"Weapon312",
			"Weapon313",
			"Weapon314",
			"Weapon284",
			"Weapon287",
			"Weapon288",
			"Weapon198",
			"Weapon199",
			"Weapon200",
			"Weapon179",
			"Weapon184",
			"Weapon236",
			"Weapon342",
			"Weapon343",
			"Weapon344",
			"Weapon166",
			"Weapon168",
			"Weapon169",
			"Weapon377",
			"Weapon378",
			"Weapon379",
			"Weapon364",
			"Weapon365",
			"Weapon366",
			"Weapon261",
			"Weapon272",
			"Weapon273",
			"Weapon345",
			"Weapon346",
			"Weapon347"
		};
		WeaponManager._Removed150615_GunsPrefabNAmes = strs;
		WeaponManager._Removed150615_Guns = new List<string>(WeaponManager._Removed150615_GunsPrefabNAmes.Count);
		foreach (string _Removed150615GunsPrefabNAme in WeaponManager._Removed150615_GunsPrefabNAmes)
		{
			ItemRecord byPrefabName = ItemDb.GetByPrefabName(_Removed150615GunsPrefabNAme);
			if (byPrefabName == null || byPrefabName.Tag == null)
			{
				continue;
			}
			WeaponManager._Removed150615_Guns.Add(byPrefabName.Tag);
		}
	}

	public static GameObject InnerPrefabForWeapon(GameObject weapon)
	{
		return WeaponManager.InnerPrefabForWeapon(weapon.name);
	}

	public static GameObject InnerPrefabForWeapon(string weapon)
	{
		return null;
	}

	public static ResourceRequest InnerPrefabForWeaponAsync(string weapon)
	{
		return Resources.LoadAsync<GameObject>(string.Concat(Defs.InnerWeaponsFolder, "/", weapon, Defs.InnerWeapons_Suffix));
	}

	public static GameObject InnerPrefabForWeaponBuffered(GameObject weapon)
	{
		return LoadAsyncTool.Get(string.Concat(Defs.InnerWeaponsFolder, "/", weapon.name, Defs.InnerWeapons_Suffix), true).asset as GameObject;
	}

	public static GameObject InnerPrefabForWeaponSync(string weapon)
	{
		return Resources.Load<GameObject>(string.Concat(Defs.InnerWeaponsFolder, "/", weapon, Defs.InnerWeapons_Suffix));
	}

	public bool IsAvailableTryGun(string tryGunTag)
	{
		bool flag;
		bool value;
		try
		{
			if (tryGunTag == null || this.TryGuns == null || !this.TryGuns.Keys.Contains<string>(tryGunTag) || !this.TryGuns[tryGunTag].ContainsKey("NumberOfMatchesKey"))
			{
				value = false;
			}
			else
			{
				SaltedInt item = (SaltedInt)this.TryGuns[tryGunTag]["NumberOfMatchesKey"];
				value = item.Value > 0;
			}
			flag = value;
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in IsAvailableTryGun: ", exception));
			flag = false;
		}
		return flag;
	}

	public static bool IsExclusiveWeapon(string weaponTag)
	{
		return (WeaponManager.GotchaGuns.Contains(weaponTag) ? true : weaponTag == WeaponManager.SocialGunWN);
	}

	public bool IsWeaponDiscountedAsTryGun(string tg)
	{
		return (this.tryGunPromos == null ? false : this.tryGunPromos.ContainsKey(tg));
	}

	public static string LastBoughtTag(string tg)
	{
		string item;
		string str;
		string str1;
		if (tg == null)
		{
			return null;
		}
		if (tg == "Armor_Novice")
		{
			if (!ShopNGUIController.NoviceArmorAvailable)
			{
				str1 = null;
			}
			else
			{
				str1 = "Armor_Novice";
			}
			return str1;
		}
		if (WeaponManager.tagToStoreIDMapping.ContainsKey(tg))
		{
			bool flag = false;
			List<string> strs = null;
			foreach (List<string> upgrade in WeaponUpgrades.upgrades)
			{
				if (!upgrade.Contains(tg))
				{
					continue;
				}
				strs = upgrade;
				flag = true;
				break;
			}
			if (!flag)
			{
				bool flag1 = ItemDb.IsTemporaryGun(tg);
				if (!flag1 && Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[tg]], true) == 1 || flag1 && TempItemsController.sharedController.ContainsItem(tg))
				{
					return tg;
				}
				return null;
			}
			for (int i = strs.Count - 1; i >= 0; i--)
			{
				if (Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[strs[i]]], true) == 1)
				{
					return strs[i];
				}
			}
			return null;
		}
		Dictionary<ShopNGUIController.CategoryNames, List<List<string>>>.Enumerator enumerator = Wear.wear.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				List<List<string>>.Enumerator enumerator1 = enumerator.Current.Value.GetEnumerator();
				try
				{
					while (enumerator1.MoveNext())
					{
						List<string> current = enumerator1.Current;
						if (!current.Contains(tg))
						{
							continue;
						}
						if (TempItemsController.PriceCoefs.ContainsKey(tg))
						{
							if (!(TempItemsController.sharedController != null) || !TempItemsController.sharedController.ContainsItem(tg))
							{
								str = null;
							}
							else
							{
								str = tg;
							}
							item = str;
							return item;
						}
						else if (Storager.getInt(current[0], true) != 0)
						{
							int num = 1;
							while (num < current.Count)
							{
								if (Storager.getInt(current[num], true) != 0)
								{
									num++;
								}
								else
								{
									item = current[num - 1];
									return item;
								}
							}
							item = current[current.Count - 1];
							return item;
						}
						else
						{
							item = null;
							return item;
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator1).Dispose();
				}
			}
			return tg;
		}
		finally
		{
			((IDisposable)(object)enumerator).Dispose();
		}
		return item;
	}

	private void LoadTryGunDiscounts()
	{
		try
		{
			if (!Storager.hasKey("WeaponManager.TryGunsDiscountsKey"))
			{
				Storager.setString("WeaponManager.TryGunsDiscountsKey", "{}", false);
			}
			foreach (KeyValuePair<string, object> keyValuePair in Json.Deserialize(Storager.getString("WeaponManager.TryGunsDiscountsKey", false)) as Dictionary<string, object>)
			{
				this.tryGunPromos.Add(keyValuePair.Key, (long)keyValuePair.Value);
			}
			if (!Storager.hasKey("WeaponManager.TryGunsDiscountsValuesKey"))
			{
				Storager.setString("WeaponManager.TryGunsDiscountsValuesKey", "{}", false);
			}
			foreach (KeyValuePair<string, object> keyValuePair1 in Json.Deserialize(Storager.getString("WeaponManager.TryGunsDiscountsValuesKey", false)) as Dictionary<string, object>)
			{
				this.tryGunDiscounts.Add(keyValuePair1.Key, new SaltedLong((long)17425, (long)((int)((long)keyValuePair1.Value))));
			}
			this.RemoveExpiredPromosForTryGuns();
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in LoadTryGunDiscounts: ", exception));
		}
	}

	private void LoadTryGunsInfo()
	{
		object obj;
		try
		{
			if (!Storager.hasKey("WeaponManager.TryGunsKey"))
			{
				Storager.setString("WeaponManager.TryGunsKey", "{}", false);
			}
			Dictionary<string, object> strs1 = Json.Deserialize(Storager.getString("WeaponManager.TryGunsKey", false)) as Dictionary<string, object>;
			if (strs1.TryGetValue("TryGunsDictionaryKey", out obj))
			{
				this.TryGuns = (obj as Dictionary<string, object>).Select<KeyValuePair<string, object>, KeyValuePair<string, Dictionary<string, object>>>((KeyValuePair<string, object> kvp) => {
					Dictionary<string, object> strs = new Dictionary<string, object>()
					{
						{ "NumberOfMatchesKey", new SaltedInt(52394, (int)((long)(kvp.Value as Dictionary<string, object>)["NumberOfMatchesKey"])) },
						{ "EquippedBeforeKey", (kvp.Value as Dictionary<string, object>)["EquippedBeforeKey"] }
					};
					return new KeyValuePair<string, Dictionary<string, object>>(kvp.Key, strs);
				}).ToDictionary<KeyValuePair<string, Dictionary<string, object>>, string, Dictionary<string, object>>((KeyValuePair<string, Dictionary<string, object>> kvp) => kvp.Key, (KeyValuePair<string, Dictionary<string, object>> kvp) => kvp.Value);
			}
			if (strs1.ContainsKey("ExpiredTryGunsListKey"))
			{
				this.ExpiredTryGuns = (strs1["ExpiredTryGunsListKey"] as List<object>).OfType<string>().ToList<string>();
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in LoadTryGunsInfo: ", exception));
		}
	}

	public string LoadWeaponSet(string sn)
	{
		if (Application.isEditor)
		{
			return PlayerPrefs.GetString(sn, this.DefaultSetForWeaponSetSettingName(sn));
		}
		if (!Storager.hasKey(sn))
		{
			Storager.setString(sn, this.DefaultSetForWeaponSetSettingName(sn), false);
		}
		return Storager.getString(sn, false);
	}

	private void LoadWearInfoPrefabsToCache()
	{
		this._wearInfoPrefabsToCache.AddRange(Resources.LoadAll<ShopPositionParams>("Armor_Info"));
		this._wearInfoPrefabsToCache.AddRange(Resources.LoadAll<ShopPositionParams>("Capes_Info"));
		this._wearInfoPrefabsToCache.AddRange(Resources.LoadAll<ShopPositionParams>("Shop_Boots_Info"));
		this._wearInfoPrefabsToCache.AddRange(Resources.LoadAll<ShopPositionParams>("Masks_Info"));
		this._wearInfoPrefabsToCache.AddRange(Resources.LoadAll<ShopPositionParams>("Hats_Info"));
	}

	private static bool OldChainThatAlwaysShownFromStart(string tg)
	{
		string str = WeaponUpgrades.TagOfFirstUpgrade(tg);
		return WeaponManager.oldTags.Contains<string>(str);
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			this.LoadTryGunsInfo();
			this.LoadTryGunDiscounts();
		}
		else
		{
			try
			{
				this.SaveLastUsedWeapons();
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogError(string.Concat("Saving last used weapons: ", exception));
			}
			this.SaveTryGunsInfo();
			this.SaveTryGunsDiscounts();
		}
	}

	private void OnDestroy()
	{
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			GoogleIABManager.purchaseSucceededEvent -= new Action<GooglePurchase>(this.AddWeapon);
		}
	}

	public static void ProvideExclusiveWeaponByTag(string weaponTag)
	{
		if (string.IsNullOrEmpty(weaponTag))
		{
			UnityEngine.Debug.LogError("Error in ProvideExclusiveWeaponByTag: string.IsNullOrEmpty(weaponTag)");
			return;
		}
		if (Storager.getInt(weaponTag, true) > 0)
		{
			UnityEngine.Debug.LogError("Error in ProvideExclusiveWeaponByTag: Storager.getInt (weaponTag, true) > 0");
			return;
		}
		ItemRecord byTag = ItemDb.GetByTag(weaponTag);
		if (byTag == null)
		{
			UnityEngine.Debug.LogError("Error in ProvideExclusiveWeaponByTag: weaponRecord == null");
			return;
		}
		if (byTag.PrefabName == null)
		{
			UnityEngine.Debug.LogError("Error in ProvideExclusiveWeaponByTag: weaponRecord.PrefabName == null");
			return;
		}
		Storager.setInt(weaponTag, 1, true);
		WeaponManager.AddExclusiveWeaponToWeaponStructures(byTag.PrefabName);
	}

	public static bool PurchasableWeaponSetContains(string weaponTag)
	{
		return WeaponManager._purchasableWeaponSet.Contains(weaponTag);
	}

	private bool ReequipItemsAfterCloudSync()
	{
		bool flag = (WeaponManager.sharedManager == null ? false : WeaponManager.sharedManager.myPlayerMoveC != null);
		List<ShopNGUIController.CategoryNames> categoryNames = new List<ShopNGUIController.CategoryNames>();
		ShopNGUIController.CategoryNames[] categoryNamesArray = new ShopNGUIController.CategoryNames[] { ShopNGUIController.CategoryNames.ArmorCategory, ShopNGUIController.CategoryNames.BootsCategory, ShopNGUIController.CategoryNames.CapesCategory, ShopNGUIController.CategoryNames.HatsCategory, ShopNGUIController.CategoryNames.MaskCategory };
		for (int i = 0; i < (int)categoryNamesArray.Length; i++)
		{
			ShopNGUIController.CategoryNames categoryName = categoryNamesArray[i];
			string str = ShopNGUIController.NoneEquippedForWearCategory(categoryName);
			string str1 = Storager.getString(ShopNGUIController.SnForWearCategory(categoryName), false);
			if (str1 != null && str != null && !str1.Equals(str) && str1 != "Armor_Novice")
			{
				string str2 = WeaponManager.LastBoughtTag(str1);
				if (str2 != null && str2 != str1)
				{
					ShopNGUIController.EquipWearInCategoryIfNotEquiped(str2, categoryName, flag);
					categoryNames.Add(categoryName);
				}
			}
		}
		bool flag1 = false;
		foreach (string str3 in WeaponManager.AllWeaponSetsSettingNames())
		{
			string str4 = this.LoadWeaponSet(str3);
			string[] strArrays = str4.Split(new char[] { '#' });
			for (int j = 0; j < (int)strArrays.Length; j++)
			{
				string str5 = strArrays[j];
				if (!string.IsNullOrEmpty(str5))
				{
					ItemRecord byPrefabName = ItemDb.GetByPrefabName(str5);
					if (byPrefabName != null && byPrefabName.Tag != null && byPrefabName.CanBuy)
					{
						string str6 = WeaponManager.LastBoughtTag(byPrefabName.Tag);
						if (str6 != null)
						{
							if (str6 != byPrefabName.Tag)
							{
								ItemRecord byTag = ItemDb.GetByTag(str6);
								if (byTag != null && byTag.PrefabName != null)
								{
									this.SaveWeaponSet(str3, byTag.PrefabName, j);
									flag1 = true;
								}
							}
						}
					}
				}
			}
		}
		if (flag)
		{
			if (this.myPlayerMoveC.mySkinName != null)
			{
				if (categoryNames.Contains(ShopNGUIController.CategoryNames.ArmorCategory))
				{
					this.myPlayerMoveC.mySkinName.SetArmor(null);
				}
				if (categoryNames.Contains(ShopNGUIController.CategoryNames.BootsCategory))
				{
					this.myPlayerMoveC.mySkinName.SetBoots(null);
				}
				if (categoryNames.Contains(ShopNGUIController.CategoryNames.CapesCategory))
				{
					this.myPlayerMoveC.mySkinName.SetCape(null);
				}
				if (categoryNames.Contains(ShopNGUIController.CategoryNames.HatsCategory))
				{
					this.myPlayerMoveC.mySkinName.SetHat(null);
				}
				if (categoryNames.Contains(ShopNGUIController.CategoryNames.MaskCategory))
				{
					this.myPlayerMoveC.mySkinName.SetMask(null);
				}
			}
		}
		else if (PersConfigurator.currentConfigurator != null && categoryNames.Count > 0)
		{
			PersConfigurator.currentConfigurator._AddCapeAndHat();
		}
		return flag1;
	}

	private void ReequipWeaponsForGuiAndRpcAndUpdateIcons()
	{
		if (this.myPlayerMoveC != null && ShopNGUIController.sharedShop != null && ShopNGUIController.sharedShop.equipAction != null)
		{
			IEnumerator enumerator = this.playerWeapons.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Weapon current = (Weapon)enumerator.Current;
					if (current != null && current.weaponPrefab != null)
					{
						ShopNGUIController.sharedShop.equipAction(ItemDb.GetByPrefabName(current.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag);
					}
					if (!ShopNGUIController.GuiActive)
					{
						continue;
					}
					ShopNGUIController.sharedShop.UpdateIcon((ShopNGUIController.CategoryNames)(current.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1), false);
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable == null)
				{
				}
				disposable.Dispose();
			}
		}
	}

	public static void RefreshExpControllers()
	{
		if (ExperienceController.sharedController == null)
		{
			UnityEngine.Debug.LogError("RefreshLevelAndSetRememberedTiersFromCloud ExperienceController.sharedController == null");
		}
		else
		{
			ExperienceController.sharedController.Refresh();
		}
		if (ExpController.Instance == null)
		{
			UnityEngine.Debug.LogError("RefreshLevelAndSetRememberedTiersFromCloud ExpController.Instance == null");
		}
		else
		{
			ExpController.Instance.Refresh();
		}
	}

	public static void RefreshLevelAndSetRememberedTiersFromCloud(List<string> weaponsForWhichSetRememberedTier)
	{
		try
		{
			WeaponManager.SetRememberedTiersForWeaponsComesFromCloud(weaponsForWhichSetRememberedTier);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("RefreshLevelAndSetRememberedTiersFromCloud exception: ", exception));
		}
	}

	public void Reload()
	{
		if (!this.currentWeaponSounds.isShotMelee)
		{
			this.currentWeaponSounds.animationObject.GetComponent<Animation>().Stop("Empty");
			if (!this.currentWeaponSounds.isDoubleShot)
			{
				this.currentWeaponSounds.animationObject.GetComponent<Animation>().CrossFade("Shoot");
			}
			this.currentWeaponSounds.animationObject.GetComponent<Animation>().Play("Reload");
			this.currentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].speed = this.myPlayerMoveC._currentReloadAnimationSpeed;
		}
	}

	public void ReloadAmmo()
	{
		this.ReloadWeaponFromSet(this.CurrentWeaponIndex);
		if (this.myPlayerMoveC != null)
		{
			this.myPlayerMoveC.isReloading = false;
		}
	}

	public void ReloadWeaponFromSet(int index)
	{
		int component = ((Weapon)this.playerWeapons[index]).weaponPrefab.GetComponent<WeaponSounds>().ammoInClip - ((Weapon)this.playerWeapons[index]).currentAmmoInClip;
		if (((Weapon)this.playerWeapons[index]).currentAmmoInBackpack < component)
		{
			Weapon item = (Weapon)this.playerWeapons[index];
			item.currentAmmoInClip = item.currentAmmoInClip + ((Weapon)this.playerWeapons[index]).currentAmmoInBackpack;
			((Weapon)this.playerWeapons[index]).currentAmmoInBackpack = 0;
		}
		else
		{
			Weapon weapon = (Weapon)this.playerWeapons[index];
			weapon.currentAmmoInClip = weapon.currentAmmoInClip + component;
			if (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != TrainingController.NewTrainingCompletedStage.None)
			{
				Weapon item1 = (Weapon)this.playerWeapons[index];
				item1.currentAmmoInBackpack = item1.currentAmmoInBackpack - component;
			}
		}
	}

	public void RemoveDiscountForTryGun(string tg)
	{
		this.tryGunPromos.Remove(tg);
		this.tryGunDiscounts.Remove(tg);
	}

	public void RemoveExpiredPromosForTryGuns()
	{
		try
		{
			float single = WeaponManager.TryGunPromoDuration();
			List<KeyValuePair<string, long>> list = (
				from kvp in this.tryGunPromos
				where (float)(PromoActionsManager.CurrentUnixTime - kvp.Value) >= single
				select kvp).ToList<KeyValuePair<string, long>>();
			foreach (KeyValuePair<string, long> keyValuePair in list)
			{
				this.RemoveDiscountForTryGun(keyValuePair.Key);
			}
			if (list.Count<KeyValuePair<string, long>>() > 0)
			{
				if (ShopNGUIController.sharedShop != null && ShopNGUIController.GuiActive)
				{
					ShopNGUIController.sharedShop.UpdateButtons();
					ShopNGUIController.sharedShop.UpdateItemParameters();
				}
				Action action = WeaponManager.TryGunExpired;
				if (action != null)
				{
					action();
				}
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in RemoveExpiredPromosForTryGuns: ", exception));
		}
	}

	public static bool RemoveGunFromAllTryGunRelated(string tg)
	{
		if (tg == null)
		{
			UnityEngine.Debug.LogError("RemoveGunFromAllTryGunRelated: tg == null");
			return false;
		}
		string str = WeaponManager.LastBoughtTag(tg);
		if (str == null)
		{
			UnityEngine.Debug.LogError(string.Concat("RemoveGunFromAllTryGunRelated: lastBought == null,  tg = ", tg));
			return false;
		}
		bool flag = WeaponManager.sharedManager.TryGuns.Remove(str);
		WeaponManager.sharedManager.ExpiredTryGuns.RemoveAll((string expiredGunTag) => expiredGunTag == str);
		WeaponManager.sharedManager.RemoveDiscountForTryGun(str);
		return flag;
	}

	public void RemoveTemporaryItem(string tg)
	{
		if (tg == null)
		{
			return;
		}
		ItemRecord byTag = ItemDb.GetByTag(tg);
		if (byTag == null || byTag.PrefabName == null)
		{
			return;
		}
		string str = this.LoadWeaponSet(Defs.MultiplayerWSSN);
		string[] empty = str.Split(new char[] { '#' });
		for (int i = 0; i < (int)empty.Length; i++)
		{
			if (empty[i] == null)
			{
				empty[i] = string.Empty;
			}
		}
		int num = -1;
		IEnumerator enumerator = this.allAvailablePlayerWeapons.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Weapon current = (Weapon)enumerator.Current;
				if (!ItemDb.GetByPrefabName(current.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag.Equals(tg))
				{
					continue;
				}
				num = this.allAvailablePlayerWeapons.IndexOf(current);
				break;
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
		if (num != -1)
		{
			this.allAvailablePlayerWeapons.RemoveAt(num);
		}
		int num1 = Array.IndexOf<string>(empty, byTag.PrefabName);
		if (num1 != -1)
		{
			WeaponManager.sharedManager.SaveWeaponSet(Defs.MultiplayerWSSN, this.TopWeaponForCat(num1, false), num1);
			WeaponManager.sharedManager.SaveWeaponSet(Defs.CampaignWSSN, this.TopWeaponForCat(num1, true), num1);
		}
		this.SetWeaponsSet(this._currentFilterMap);
		this._InitShopCategoryLists(this._currentFilterMap);
		this.UpdateFilteredShopLists();
	}

	public void RemoveTryGun(string tryGunTag)
	{
		string str;
		if (this.TryGuns == null || !this.TryGuns.ContainsKey(tryGunTag))
		{
			return;
		}
		try
		{
			if (!this.TryGuns[tryGunTag].TryGetValue<string>("EquippedBeforeKey", out str))
			{
				UnityEngine.Debug.LogError(string.Concat("RemoveTryGun: No EquippedBeforeKey for ", tryGunTag));
			}
			else if (!string.IsNullOrEmpty(str))
			{
				try
				{
					string str1 = WeaponManager.LastBoughtTag(str);
					Weapon weapon = this.allAvailablePlayerWeapons.OfType<Weapon>().FirstOrDefault<Weapon>((Weapon w) => ItemDb.GetByPrefabName(w.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag == str1);
					if (weapon == null)
					{
						int itemCategory = ItemDb.GetItemCategory(str1);
						Weapon weapon1 = this.allAvailablePlayerWeapons.OfType<Weapon>().FirstOrDefault<Weapon>((Weapon w) => (w.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1 != itemCategory || !(ItemDb.GetByPrefabName(w.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag != tryGunTag) ? false : !this.IsAvailableTryGun(tryGunTag)));
						if (weapon1 == null)
						{
							this.SaveWeaponSet(Defs.CampaignWSSN, string.Empty, itemCategory);
							int num = -1;
							int num1 = 0;
							while (num1 < this.playerWeapons.Count)
							{
								if (this.playerWeapons[num1] == null || !(ItemDb.GetByPrefabName(((Weapon)this.playerWeapons[num1]).weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag == tryGunTag))
								{
									num1++;
								}
								else
								{
									num = num1;
									break;
								}
							}
							if (num == -1)
							{
								UnityEngine.Debug.LogError("RemoveTryGun: error removing weapon from playerWeapons");
							}
							else
							{
								this.playerWeapons.RemoveAt(num);
							}
							this.SetWeaponsSet(0);
							if (itemCategory == 4)
							{
								this.SaveWeaponSet("WeaponManager.SniperModeWSSN", WeaponManager.CampaignRifle_WN, itemCategory);
							}
							if (itemCategory == 2)
							{
								this.SaveWeaponSet("WeaponManager.KnifesModeWSSN", WeaponManager.KnifeWN, itemCategory);
							}
							this.SaveWeaponSet(Defs.MultiplayerWSSN, WeaponManager._KnifeAndPistolAndMP5AndSniperAndRocketnitzaSet().Split(new char[] { "#"[0] })[itemCategory], itemCategory);
						}
						else
						{
							this.EquipWeapon(weapon1, true, false);
						}
					}
					else
					{
						this.EquipWeapon(weapon, true, false);
					}
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogError(string.Concat("tryGun.TryGetValue(EquippedBeforeKey, out gunBefore) exception: ", exception));
				}
			}
			this.allAvailablePlayerWeapons = new ArrayList((
				from w in this.allAvailablePlayerWeapons.OfType<Weapon>()
				where ItemDb.GetByPrefabName(w.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag != tryGunTag
				select w).ToList<Weapon>());
			this.TryGuns.Remove(tryGunTag);
			if (!this.ExpiredTryGuns.Contains(tryGunTag))
			{
				this.ExpiredTryGuns.Add(tryGunTag);
			}
		}
		catch (Exception exception1)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in RemoveTryGun: ", exception1));
		}
	}

	public void Reset(int filterMap = 0)
	{
		IEnumerator enumerator = this.ResetCoroutine(filterMap);
		while (enumerator.MoveNext())
		{
			object current = enumerator.Current;
		}
	}

	[DebuggerHidden]
	public IEnumerator ResetCoroutine(int filterMap = 0)
	{
		WeaponManager.u003cResetCoroutineu003ec__Iterator19F variable = null;
		return variable;
	}

	private void ReturnAlienGunToCampaignBack()
	{
		Storager.setInt(Defs.ReturnAlienGun930, 1, false);
		Storager.setString(Defs.MultiplayerWSSN, this.DefaultSetForWeaponSetSettingName(Defs.MultiplayerWSSN), false);
		Storager.setString(Defs.CampaignWSSN, this.DefaultSetForWeaponSetSettingName(Defs.CampaignWSSN), false);
	}

	private void SaveLastUsedWeapons()
	{
		Storager.setString("WeaponManager.LastUsedWeaponsKey", Json.Serialize(this.lastUsedWeaponsForFilterMaps), false);
	}

	private void SaveTryGunsDiscounts()
	{
		try
		{
			Storager.setString("WeaponManager.TryGunsDiscountsKey", Json.Serialize(this.tryGunPromos), false);
			Storager.setString("WeaponManager.TryGunsDiscountsValuesKey", Json.Serialize(this.tryGunDiscounts.ToDictionary<KeyValuePair<string, SaltedLong>, string, long>((KeyValuePair<string, SaltedLong> kvp) => kvp.Key, (KeyValuePair<string, SaltedLong> kvp) => kvp.Value.Value)), false);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in SaveTryGunsDiscounts: ", exception));
		}
	}

	private void SaveTryGunsInfo()
	{
		try
		{
			Dictionary<string, Dictionary<string, object>> dictionary = (
				from kvp in this.TryGuns
				select new KeyValuePair<string, Dictionary<string, object>>(kvp.Key, new Dictionary<string, object>()
				{
					{ "NumberOfMatchesKey", ((SaltedInt)kvp.Value["NumberOfMatchesKey"]).Value },
					{ "EquippedBeforeKey", kvp.Value["EquippedBeforeKey"] }
				})).ToDictionary<KeyValuePair<string, Dictionary<string, object>>, string, Dictionary<string, object>>((KeyValuePair<string, Dictionary<string, object>> kvp) => kvp.Key, (KeyValuePair<string, Dictionary<string, object>> kvp) => kvp.Value);
			Dictionary<string, object> strs = new Dictionary<string, object>()
			{
				{ "TryGunsDictionaryKey", dictionary },
				{ "ExpiredTryGunsListKey", this.ExpiredTryGuns }
			};
			Storager.setString("WeaponManager.TryGunsKey", Json.Serialize(strs), false);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in SaveTryGunsInfo: ", exception));
		}
	}

	public void SaveWeaponAsLastUsed(int index)
	{
		if (Defs.isMulti && (!Defs.isHunger || SceneLoader.ActiveSceneName == "ConnectScene" || SceneLoader.ActiveSceneName == "ConnectSceneSandbox") && this.playerWeapons != null && this.playerWeapons.Count > index && index >= 0)
		{
			try
			{
				int component = (this.playerWeapons[index] as Weapon).weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1;
				if (!this.lastUsedWeaponsForFilterMaps.ContainsKey(this._currentFilterMap.ToString()))
				{
					this.lastUsedWeaponsForFilterMaps.Add(this._currentFilterMap.ToString(), component);
				}
				else
				{
					this.lastUsedWeaponsForFilterMaps[this._currentFilterMap.ToString()] = component;
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogError(string.Concat("Exception in SaveWeaponAsLastUsed index = ", index));
			}
		}
	}

	public void SaveWeaponSet(string sn, string wn, int pos)
	{
		string str = this.LoadWeaponSet(sn);
		string[] strArrays = str.Split(new char[] { '#' });
		strArrays[pos] = wn;
		string str1 = string.Join("#", strArrays);
		if (Application.isEditor)
		{
			PlayerPrefs.SetString(sn, str1);
		}
		else
		{
			Storager.hasKey(sn);
			Storager.setString(sn, str1, false);
		}
	}

	public static void SetGunFlashActive(GameObject gunFlash, bool _a)
	{
		if (gunFlash == null)
		{
			return;
		}
		Transform child = null;
		if (gunFlash.transform.childCount > 0)
		{
			child = gunFlash.transform.GetChild(0);
		}
		if (child != null && child.gameObject.activeSelf != _a)
		{
			child.gameObject.SetActive(_a);
		}
	}

	public void SetMaxAmmoFrAllWeapons()
	{
		IEnumerator enumerator = this.allAvailablePlayerWeapons.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Weapon current = (Weapon)enumerator.Current;
				current.currentAmmoInClip = current.weaponPrefab.GetComponent<WeaponSounds>().ammoInClip;
				current.currentAmmoInBackpack = current.weaponPrefab.GetComponent<WeaponSounds>().MaxAmmoWithEffectApplied;
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
	}

	public static void SetRememberedTierForWeapon(string prefabName)
	{
		Storager.setInt(string.Concat("RememberedTierWhenObtainGun_", prefabName), ExpController.OurTierForAnyPlace(), false);
	}

	public static void SetRememberedTiersForWeaponsComesFromCloud(List<string> weaponsForWhichSetRememberedTier)
	{
		try
		{
			foreach (string str in weaponsForWhichSetRememberedTier)
			{
				ItemRecord byTag = ItemDb.GetByTag(str);
				if (byTag == null)
				{
					UnityEngine.Debug.LogWarning("SetRememberedTiersForWeaponsComesFromCloud record == null");
				}
				else
				{
					string prefabName = byTag.PrefabName;
					if (prefabName == null)
					{
						UnityEngine.Debug.LogError("SetRememberedTiersForWeaponsComesFromCloud prefabName == null");
					}
					else
					{
						WeaponManager.SetRememberedTierForWeapon(prefabName);
					}
				}
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("SetRememberedTiersForWeaponsComesFromCloud exception: ", exception));
		}
	}

	private void SetWeaponInAppropriateMultyModes(WeaponSounds ws)
	{
		List<int> nums = new List<int>()
		{
			0
		};
		List<int> list = nums.Concat<int>((ws.filterMap == null ? new int[0] : ws.filterMap)).Distinct<int>().ToList<int>();
		foreach (int num in list)
		{
			if (!WeaponManager.WeaponSetSettingNamesForFilterMaps.ContainsKey(num))
			{
				UnityEngine.Debug.LogError(string.Concat("WeaponSetSettingNamesForFilterMaps.ContainsKey (mode): ", num));
			}
			else
			{
				this.SaveWeaponSet(WeaponManager.WeaponSetSettingNamesForFilterMaps[num].settingName, ws.gameObject.name, ws.categoryNabor - 1);
			}
		}
	}

	public void SetWeaponsSet(int filterMap = 0)
	{
		this._playerWeapons.Clear();
		bool flag = Defs.isMulti;
		bool flag1 = Defs.isHunger;
		string str = null;
		if (!flag)
		{
			if (Defs.IsSurvival || !TrainingController.TrainingCompleted)
			{
				str = (!Defs.IsSurvival || !TrainingController.TrainingCompleted ? WeaponManager._KnifeAndPistolSet() : this.LoadWeaponSet(Defs.MultiplayerWSSN));
			}
			else
			{
				str = this.LoadWeaponSet(Defs.CampaignWSSN);
			}
		}
		else if (flag1)
		{
			str = WeaponManager._KnifeSet();
		}
		else if (!WeaponManager.WeaponSetSettingNamesForFilterMaps.ContainsKey(filterMap))
		{
			UnityEngine.Debug.LogError(string.Concat("WeaponSetSettingNamesForFilterMaps.ContainsKey (filterMap): filterMap = ", filterMap));
		}
		else
		{
			str = this.LoadWeaponSet(WeaponManager.WeaponSetSettingNamesForFilterMaps[filterMap].settingName);
		}
		string[] strArrays = str.Split(new char[] { '#' });
		for (int i = 0; i < (int)strArrays.Length; i++)
		{
			string str1 = strArrays[i];
			if (!string.IsNullOrEmpty(str1))
			{
				IEnumerator enumerator = this.allAvailablePlayerWeapons.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Weapon current = (Weapon)enumerator.Current;
						if (!current.weaponPrefab.name.Equals(str1))
						{
							continue;
						}
						this.EquipWeapon(current, false, false);
						break;
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable == null)
					{
					}
					disposable.Dispose();
				}
			}
		}
		if (filterMap == 2)
		{
			IEnumerator enumerator1 = this.allAvailablePlayerWeapons.GetEnumerator();
			try
			{
				while (enumerator1.MoveNext())
				{
					Weapon weapon = (Weapon)enumerator1.Current;
					if (!weapon.weaponPrefab.name.Equals(WeaponManager.KnifeWN))
					{
						continue;
					}
					this.EquipWeapon(weapon, false, false);
					break;
				}
			}
			finally
			{
				IDisposable disposable1 = enumerator1 as IDisposable;
				if (disposable1 == null)
				{
				}
				disposable1.Dispose();
			}
			IEnumerator enumerator2 = this.allAvailablePlayerWeapons.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					Weapon current1 = (Weapon)enumerator2.Current;
					if (!current1.weaponPrefab.name.Equals(WeaponManager.PistolWN))
					{
						continue;
					}
					this.EquipWeapon(current1, false, false);
					break;
				}
			}
			finally
			{
				IDisposable disposable2 = enumerator2 as IDisposable;
				if (disposable2 == null)
				{
				}
				disposable2.Dispose();
			}
		}
		if (filterMap == 2 && this.playerWeapons.Count == 2)
		{
			IEnumerator enumerator3 = this.allAvailablePlayerWeapons.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					Weapon weapon1 = (Weapon)enumerator3.Current;
					if (!weapon1.weaponPrefab.name.Equals(WeaponManager.CampaignRifle_WN))
					{
						continue;
					}
					this.EquipWeapon(weapon1, false, false);
					break;
				}
			}
			finally
			{
				IDisposable disposable3 = enumerator3 as IDisposable;
				if (disposable3 == null)
				{
				}
				disposable3.Dispose();
			}
		}
		if (filterMap == 3)
		{
			if (this.playerWeapons.OfType<Weapon>().FirstOrDefault<Weapon>((Weapon w) => w.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1 == 2) == null)
			{
				IEnumerator enumerator4 = this.allAvailablePlayerWeapons.GetEnumerator();
				try
				{
					while (enumerator4.MoveNext())
					{
						Weapon current2 = (Weapon)enumerator4.Current;
						if (!current2.weaponPrefab.name.Equals(WeaponManager.DaterFreeWeaponPrefabName))
						{
							continue;
						}
						this.EquipWeapon(current2, false, false);
						break;
					}
				}
				finally
				{
					IDisposable disposable4 = enumerator4 as IDisposable;
					if (disposable4 == null)
					{
					}
					disposable4.Dispose();
				}
			}
		}
		if (this.playerWeapons.Count == 0)
		{
			this.UpdatePlayersWeaponSetCache();
		}
	}

	public static float ShotgunShotsCountModif()
	{
		return 0.6666667f;
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		WeaponManager.u003cStartu003ec__Iterator1A0 variable = null;
		return variable;
	}

	public long StartTimeForTryGunDiscount(string tg)
	{
		if (tg == null || this.tryGunPromos == null || !this.tryGunPromos.ContainsKey(tg))
		{
			return (long)0;
		}
		return this.tryGunPromos[tg];
	}

	[DebuggerHidden]
	private IEnumerator Step()
	{
		WeaponManager.u003cStepu003ec__Iterator19D variable = null;
		return variable;
	}

	private string TopWeaponForCat(int ind, bool campaign = false)
	{
		string item = WeaponManager._KnifeAndPistolAndMP5AndSniperAndRocketnitzaSet();
		if (campaign)
		{
			item = WeaponManager._KnifeAndPistolAndShotgunSet();
		}
		List<WeaponSounds> weaponSounds = new List<WeaponSounds>();
		IEnumerator enumerator = this.allAvailablePlayerWeapons.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				WeaponSounds component = ((Weapon)enumerator.Current).weaponPrefab.GetComponent<WeaponSounds>();
				if (component.categoryNabor - 1 != ind)
				{
					continue;
				}
				ItemRecord byPrefabName = ItemDb.GetByPrefabName(component.name.Replace("(Clone)", string.Empty));
				if (byPrefabName == null || !byPrefabName.CanBuy)
				{
					continue;
				}
				weaponSounds.Add(component);
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
		weaponSounds.Sort(WeaponManager.dpsComparerWS);
		if (weaponSounds.Count > 0)
		{
			item = weaponSounds[weaponSounds.Count - 1].gameObject.name;
		}
		return item;
	}

	public static float TryGunPromoDuration()
	{
		float single = (float)((!FriendsController.useBuffSystem ? 3600 : 3600));
		try
		{
			single = (!FriendsController.useBuffSystem ? KillRateCheck.instance.timeForDiscount : BuffSystem.instance.timeForDiscount);
			single = Math.Max(60f, single);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in duration = KillRateCheck.instance.timeForDiscount: ", exception));
		}
		return single;
	}

	public void UnloadAll()
	{
		this._highMEmoryDevicesInnerPrefabsCache.Clear();
		this._rocketCache = null;
		this._turretCache = null;
		this._rocketCache = null;
		this._playerWeaponsSetInnerPrefabsCache.Clear();
		this._turretWeaponCache = null;
		this._playerWeapons.Clear();
		this._allAvailablePlayerWeapons.Clear();
		this._weaponsInGame = null;
		Resources.UnloadUnusedAssets();
	}

	private void UpdateFilteredShopLists()
	{
		this.FilteredShopLists = new List<List<GameObject>>();
		for (int i = 0; i < this._weaponsByCat.Count; i++)
		{
			this.FilteredShopLists.Add(new List<GameObject>());
			for (int j = 0; j < this._weaponsByCat[i].Count; j++)
			{
				bool flag = true;
				try
				{
					ItemRecord byPrefabName = ItemDb.GetByPrefabName(this._weaponsByCat[i][j].name.Replace("(Clone)", string.Empty));
					if (byPrefabName.CanBuy)
					{
						List<string> strs = WeaponUpgrades.ChainForTag(byPrefabName.Tag);
						ItemRecord byTag = ItemDb.GetByTag((strs == null || strs.Count <= 0 ? byPrefabName.Tag : strs[0]));
						if (WeaponManager.AllWrapperPrefabs().First<WeaponSounds>((WeaponSounds ws) => ws.name == byTag.PrefabName).tier < ExpController.OurTierForAnyPlace() - this.ShopListsTierConstraint && WeaponManager.LastBoughtTag(byTag.Tag) == null && !this.IsWeaponDiscountedAsTryGun(byPrefabName.Tag) && !this.TryGuns.ContainsKey(byPrefabName.Tag))
						{
							flag = false;
						}
					}
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogError(string.Concat("Exception in UpdateFilteredShopLists: ", exception));
				}
				if (flag)
				{
					this.FilteredShopLists[i].Add(this._weaponsByCat[i][j]);
				}
			}
		}
	}

	private void UpdatePlayersWeaponSetCache()
	{
		if (Device.IsLoweMemoryDevice)
		{
			Resources.UnloadUnusedAssets();
		}
	}

	[DebuggerHidden]
	private IEnumerator UpdateWeapons800To801()
	{
		WeaponManager.u003cUpdateWeapons800To801u003ec__Iterator1A1 variable = null;
		return variable;
	}

	public static event Action TryGunExpired;

	public static event Action TryGunRemoved;

	public static event Action<int> WeaponEquipped;

	public struct infoClient
	{
		public string ipAddress;

		public string name;

		public string coments;
	}

	public enum WeaponTypeForLow
	{
		AssaultRifle_1,
		AssaultRifle_2,
		Shotgun_1,
		Shotgun_2,
		Machinegun,
		Pistol_1,
		Pistol_2,
		Submachinegun,
		Knife,
		Sword,
		Flamethrower_1,
		Flamethrower_2,
		SniperRifle_1,
		SniperRifle_2,
		Bow,
		RocketLauncher_1,
		RocketLauncher_2,
		RocketLauncher_3,
		GrenadeLauncher,
		Snaryad,
		Snaryad_Otskok,
		Snaryad_Disk,
		Railgun,
		Ray,
		AOE,
		Instant_Area_Damage,
		X3_Snaryad,
		NOT_CHANGE
	}
}