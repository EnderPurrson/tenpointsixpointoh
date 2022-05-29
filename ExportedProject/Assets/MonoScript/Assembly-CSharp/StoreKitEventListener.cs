using com.amazon.device.iap.cpt;
using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public sealed class StoreKitEventListener : MonoBehaviour
{
	public const string coin1 = "coin1";

	public const string coin2 = "coin2";

	public const string coin3 = "coin3.";

	public const string coin4 = "coin4";

	public const string coin5 = "coin5";

	public const string coin7 = "coin7";

	public const string coin8 = "coin8";

	private const string AmazonFulfilledReceiptsKey = "Amazon.FulfilledReceipts";

	private const string GooglePlayConsumedOrderIdsKey = "Android.GooglePlayOrderIdsKey";

	public const string bigAmmoPackID = "bigammopack";

	public const string crystalswordID = "crystalsword";

	public const string fullHealthID = "Fullhealth";

	public const string minerWeaponID = "MinerWeapon";

	public const string skin_july1 = "skin_july1";

	public const string skin_july2 = "skin_july2";

	public const string skin_july3 = "skin_july3";

	public const string skin_july4 = "skin_july4";

	public const string LockedSkinName = "super_socialman";

	[NonSerialized]
	internal readonly ICollection<IMarketProduct> _products = new List<IMarketProduct>();

	[NonSerialized]
	public readonly ICollection<GoogleSkuInfo> _skinProducts = new GoogleSkuInfo[0];

	[NonSerialized]
	public static bool billingSupported;

	[NonSerialized]
	public readonly static string[] coinIds;

	private static string[] _productIds;

	private HashSet<GooglePurchase> _purchasesToConsume = new HashSet<GooglePurchase>();

	private HashSet<GooglePurchase> _cheatedPurchasesToConsume = new HashSet<GooglePurchase>();

	private readonly TaskCompletionSource<AmazonUserData> _amazonUserPromise = new TaskCompletionSource<AmazonUserData>();

	private IDisposable _purchaseFailedSubscription = new ActionDisposable(null);

	private static List<string> listOfIdsForWhichX3WaitingCoroutinesRun;

	private readonly Lazy<SHA1Managed> _sha1 = new Lazy<SHA1Managed>(new Func<SHA1Managed>(() => new SHA1Managed()));

	private readonly Lazy<RSACryptoServiceProvider> _rsa = new Lazy<RSACryptoServiceProvider>(new Func<RSACryptoServiceProvider>(StoreKitEventListener.InitializeRsa));

	public static StoreKitEventListener Instance;

	private static string gem1;

	private static string gem2;

	private static string gem3;

	private static string gem4;

	private static string gem5;

	private static string gem6;

	private static string gem7;

	private static string starterPack2;

	private static string starterPack4;

	private static string starterPack6;

	private static string starterPack3;

	private static string starterPack5;

	private static string starterPack7;

	private static string starterPack8;

	public readonly static int[] realValue;

	public readonly static string[] gemsIds;

	public readonly static string[] starterPackIds;

	public static Dictionary<string, string> inAppsReadableNames;

	public static string elixirSettName;

	public static bool purchaseInProcess;

	public static bool restoreInProcess;

	public static string elixirID;

	public static string endmanskin;

	public static string chief;

	public static string spaceengineer;

	public static string nanosoldier;

	public static string steelman;

	public static string CaptainSkin;

	public static string HawkSkin;

	public static string GreenGuySkin;

	public static string TunderGodSkin;

	public static string GordonSkin;

	public static string animeGirl;

	public static string EMOGirl;

	public static string Nurse;

	public static string magicGirl;

	public static string braveGirl;

	public static string glamDoll;

	public static string kittyGirl;

	public static string famosBoy;

	public static string skin810_1;

	public static string skin810_2;

	public static string skin810_3;

	public static string skin810_4;

	public static string skin810_5;

	public static string skin810_6;

	public static string skin931_1;

	public static string skin931_2;

	public static string skin_may1;

	public static string skin_may2;

	public static string skin_may3;

	public static string skin_may4;

	public static string easter_skin1;

	public static string easter_skin2;

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

	public static string skin_rapid_girl;

	public static string skin_silent_killer;

	public static string skin_daemon_fighter;

	public static string skin_scary_demon;

	public static string skin_orc_warrior;

	public static string skin_kung_fu_master;

	public static string skin_fire_wizard;

	public static string skin_ice_wizard;

	public static string skin_storm_wizard;

	public static string fullVersion;

	public static string armor;

	public static string armor2;

	public static string armor3;

	public readonly static string[] skinIDs;

	public readonly static string[] idsForSingle;

	public readonly static string[] idsForMulti;

	public readonly static string[] idsForFull;

	public readonly static string[][] categoriesSingle;

	public readonly static string[][] categoriesMulti;

	public GameObject messagePrefab;

	public static string[] categoryNames;

	public AudioClip onEarnCoinsSound;

	public AudioClip onEarnGemsSound;

	[NonSerialized]
	public static List<string> buyStarterPack;

	private readonly static StoreKitEventListener.StoreKitEventListenerState _state;

	public Task<AmazonUserData> AmazonUser
	{
		get
		{
			return this._amazonUserPromise.Task;
		}
	}

	internal ICollection<IMarketProduct> Products
	{
		get
		{
			return this._products;
		}
	}

	private static string starterPack1
	{
		get
		{
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				return "starterpack1andr";
			}
			return "starterpack1";
		}
	}

	internal static StoreKitEventListener.StoreKitEventListenerState State
	{
		get
		{
			return StoreKitEventListener._state;
		}
	}

	static StoreKitEventListener()
	{
		int j;
		StoreKitEventListener.Instance = null;
		StoreKitEventListener.gem1 = "gem1";
		StoreKitEventListener.gem2 = "gem2";
		StoreKitEventListener.gem3 = "gem3";
		StoreKitEventListener.gem4 = "gem4";
		StoreKitEventListener.gem5 = "gem5";
		StoreKitEventListener.gem6 = "gem6";
		StoreKitEventListener.gem7 = "gem7";
		StoreKitEventListener.starterPack2 = "starterpack2";
		StoreKitEventListener.starterPack4 = "starterpack4";
		StoreKitEventListener.starterPack6 = "starterpack6";
		StoreKitEventListener.starterPack3 = "starterpack3";
		StoreKitEventListener.starterPack5 = "starterpack5";
		StoreKitEventListener.starterPack7 = "starterpack7";
		StoreKitEventListener.starterPack8 = "starterpack8";
		StoreKitEventListener.realValue = new int[] { 1, 3, 5, 10, 20, 50, 100 };
		StoreKitEventListener.gemsIds = new string[] { StoreKitEventListener.gem1, StoreKitEventListener.gem2, StoreKitEventListener.gem3, StoreKitEventListener.gem4, StoreKitEventListener.gem5, StoreKitEventListener.gem6, StoreKitEventListener.gem7 };
		StoreKitEventListener.starterPackIds = new string[] { StoreKitEventListener.starterPack1, StoreKitEventListener.starterPack2, StoreKitEventListener.starterPack3, StoreKitEventListener.starterPack4, StoreKitEventListener.starterPack5, StoreKitEventListener.starterPack6, StoreKitEventListener.starterPack7, StoreKitEventListener.starterPack8 };
		Dictionary<string, string> strs = new Dictionary<string, string>()
		{
			{ "coin1", "Small Stack of Coins" },
			{ "coin7", "Medium Stack of Coins" },
			{ "coin2", "Big Stack of Coins" },
			{ "coin3.", "Huge Stack of Coins" },
			{ "coin4", "Chest with Coins" },
			{ "coin5", "Golden Chest with Coins" },
			{ "coin8", "Holy Grail" },
			{ StoreKitEventListener.gem1, "Few Gems" },
			{ StoreKitEventListener.gem2, "Handful of Gems" },
			{ StoreKitEventListener.gem3, "Pile of Gems" },
			{ StoreKitEventListener.gem4, "Chest with Gems" },
			{ StoreKitEventListener.gem5, "Treasure with Gems" },
			{ StoreKitEventListener.gem6, "Expensive Relic" },
			{ StoreKitEventListener.gem7, "Safe with Gems" },
			{ StoreKitEventListener.starterPack1, "Newbie Set" },
			{ "starterpack2", "Golden Coins Extra Pack" },
			{ StoreKitEventListener.starterPack3, "Trooper Set" },
			{ "starterpack4", "Gems Extra Pack" },
			{ StoreKitEventListener.starterPack5, "Veteran Set" },
			{ "starterpack6", "Mega Gems Pack" },
			{ StoreKitEventListener.starterPack7, "Hero Set" },
			{ StoreKitEventListener.starterPack8, "Winner Set" }
		};
		StoreKitEventListener.inAppsReadableNames = strs;
		StoreKitEventListener.elixirSettName = Defs.NumberOfElixirsSett;
		StoreKitEventListener.purchaseInProcess = false;
		StoreKitEventListener.restoreInProcess = false;
		StoreKitEventListener.elixirID = (!GlobalGameController.isFullVersion ? "elixirlite" : "elixir");
		StoreKitEventListener.endmanskin = (!GlobalGameController.isFullVersion ? "endmanskinlite" : "endmanskin");
		StoreKitEventListener.chief = (!GlobalGameController.isFullVersion ? "chiefskinlite" : "chief");
		StoreKitEventListener.spaceengineer = (!GlobalGameController.isFullVersion ? "spaceengineerskinlite" : "spaceengineer");
		StoreKitEventListener.nanosoldier = (!GlobalGameController.isFullVersion ? "nanosoldierlite" : "nanosoldier");
		StoreKitEventListener.steelman = (!GlobalGameController.isFullVersion ? "steelmanlite" : "steelman");
		StoreKitEventListener.CaptainSkin = "captainskin";
		StoreKitEventListener.HawkSkin = "hawkskin";
		StoreKitEventListener.GreenGuySkin = "greenguyskin";
		StoreKitEventListener.TunderGodSkin = "thundergodskin";
		StoreKitEventListener.GordonSkin = "gordonskin";
		StoreKitEventListener.animeGirl = "animeGirl";
		StoreKitEventListener.EMOGirl = "EMOGirl";
		StoreKitEventListener.Nurse = "Nurse";
		StoreKitEventListener.magicGirl = "magicGirl";
		StoreKitEventListener.braveGirl = "braveGirl";
		StoreKitEventListener.glamDoll = "glamDoll";
		StoreKitEventListener.kittyGirl = "kittyGirl";
		StoreKitEventListener.famosBoy = "famosBoy";
		StoreKitEventListener.skin810_1 = "skin810_1";
		StoreKitEventListener.skin810_2 = "skin810_2";
		StoreKitEventListener.skin810_3 = "skin810_3";
		StoreKitEventListener.skin810_4 = "skin810_4";
		StoreKitEventListener.skin810_5 = "skin810_5";
		StoreKitEventListener.skin810_6 = "skin810_6";
		StoreKitEventListener.skin931_1 = "skin931_1";
		StoreKitEventListener.skin931_2 = "skin931_2";
		StoreKitEventListener.skin_may1 = "skin_may1";
		StoreKitEventListener.skin_may2 = "skin_may2";
		StoreKitEventListener.skin_may3 = "skin_may3";
		StoreKitEventListener.skin_may4 = "skin_may4";
		StoreKitEventListener.easter_skin1 = "easter_skin1";
		StoreKitEventListener.easter_skin2 = "easter_skin2";
		StoreKitEventListener.Skins_11_040915 = new string[] { "skin_fiance", "skin_bride", "skin_bigalien", "skin_minialien", "skin_hippo", "skin_alligator" };
		StoreKitEventListener.skin_tiger = "skin_tiger";
		StoreKitEventListener.skin_pitbull = "skin_pitbull";
		StoreKitEventListener.skin_santa = "skin_santa";
		StoreKitEventListener.skin_elf_new_year = "skin_elf_new_year";
		StoreKitEventListener.skin_girl_new_year = "skin_girl_new_year";
		StoreKitEventListener.skin_cookie_new_year = "skin_cookie_new_year";
		StoreKitEventListener.skin_snowman_new_year = "skin_snowman_new_year";
		StoreKitEventListener.skin_jetti_hnight = "skin_jetti_hnight";
		StoreKitEventListener.skin_startrooper = "skin_startrooper";
		StoreKitEventListener.skin_rapid_girl = "skin_rapid_girl";
		StoreKitEventListener.skin_silent_killer = "skin_silent_killer";
		StoreKitEventListener.skin_daemon_fighter = "skin_daemon_fighter";
		StoreKitEventListener.skin_scary_demon = "skin_scary_demon";
		StoreKitEventListener.skin_orc_warrior = "skin_orc_warrior";
		StoreKitEventListener.skin_kung_fu_master = "skin_kung_fu_master";
		StoreKitEventListener.skin_fire_wizard = "skin_fire_wizard";
		StoreKitEventListener.skin_ice_wizard = "skin_ice_wizard";
		StoreKitEventListener.skin_storm_wizard = "skin_storm_wizard";
		StoreKitEventListener.fullVersion = "extendedversion";
		StoreKitEventListener.armor = "armor";
		StoreKitEventListener.armor2 = "armor2";
		StoreKitEventListener.armor3 = "armor3";
		StoreKitEventListener.categoryNames = new string[] { "Armory", "Guns", "Melee", "Special", "Gear" };
		StoreKitEventListener.buyStarterPack = new List<string>();
		StoreKitEventListener._state = new StoreKitEventListener.StoreKitEventListenerState();
		StoreKitEventListener.billingSupported = false;
		StoreKitEventListener.coinIds = new string[] { "coin1", "coin7", "coin2", "coin3.", "coin4", "coin5", "coin8", "coin9" };
		StoreKitEventListener._productIds = new string[] { "bigammopack", "Fullhealth", "crystalsword", "MinerWeapon", StoreKitEventListener.elixirID };
		StoreKitEventListener.listOfIdsForWhichX3WaitingCoroutinesRun = new List<string>();
		StoreKitEventListener.skinIDs = new string[] { StoreKitEventListener.endmanskin, StoreKitEventListener.chief, StoreKitEventListener.spaceengineer, StoreKitEventListener.nanosoldier, StoreKitEventListener.steelman, StoreKitEventListener.CaptainSkin, StoreKitEventListener.HawkSkin, StoreKitEventListener.GreenGuySkin, StoreKitEventListener.TunderGodSkin, StoreKitEventListener.GordonSkin, StoreKitEventListener.animeGirl, StoreKitEventListener.EMOGirl, StoreKitEventListener.Nurse, StoreKitEventListener.magicGirl, StoreKitEventListener.braveGirl, StoreKitEventListener.glamDoll, StoreKitEventListener.kittyGirl, StoreKitEventListener.famosBoy };
		List<string> strs1 = new List<string>();
		string[] strArrays = StoreKitEventListener.skinIDs;
		for (int i = 0; i < (int)strArrays.Length; i++)
		{
			strs1.Add(strArrays[i]);
		}
		for (j = 0; j < 11; j++)
		{
			strs1.Add(string.Concat("newskin_", j));
		}
		while (j < 19)
		{
			strs1.Add(string.Concat("newskin_", j));
			j++;
		}
		strs1.Add(StoreKitEventListener.skin810_1);
		strs1.Add(StoreKitEventListener.skin810_2);
		strs1.Add(StoreKitEventListener.skin810_3);
		strs1.Add(StoreKitEventListener.skin810_4);
		strs1.Add(StoreKitEventListener.skin810_5);
		strs1.Add(StoreKitEventListener.skin810_6);
		strs1.Add(StoreKitEventListener.skin931_1);
		strs1.Add(StoreKitEventListener.skin931_2);
		for (int k = 0; k < (int)StoreKitEventListener.Skins_11_040915.Length; k++)
		{
			strs1.Add(StoreKitEventListener.Skins_11_040915[k]);
		}
		strs1.Add("super_socialman");
		strs1.Add(StoreKitEventListener.skin_tiger);
		strs1.Add(StoreKitEventListener.skin_pitbull);
		strs1.Add(StoreKitEventListener.skin_santa);
		strs1.Add(StoreKitEventListener.skin_elf_new_year);
		strs1.Add(StoreKitEventListener.skin_girl_new_year);
		strs1.Add(StoreKitEventListener.skin_cookie_new_year);
		strs1.Add(StoreKitEventListener.skin_snowman_new_year);
		strs1.Add(StoreKitEventListener.skin_jetti_hnight);
		strs1.Add(StoreKitEventListener.skin_startrooper);
		strs1.Add(StoreKitEventListener.easter_skin1);
		strs1.Add(StoreKitEventListener.easter_skin2);
		strs1.Add(StoreKitEventListener.skin_rapid_girl);
		strs1.Add(StoreKitEventListener.skin_silent_killer);
		strs1.Add(StoreKitEventListener.skin_daemon_fighter);
		strs1.Add(StoreKitEventListener.skin_scary_demon);
		strs1.Add(StoreKitEventListener.skin_orc_warrior);
		strs1.Add(StoreKitEventListener.skin_kung_fu_master);
		strs1.Add(StoreKitEventListener.skin_fire_wizard);
		strs1.Add(StoreKitEventListener.skin_ice_wizard);
		strs1.Add(StoreKitEventListener.skin_storm_wizard);
		strs1.Add(StoreKitEventListener.skin_may1);
		strs1.Add(StoreKitEventListener.skin_may2);
		strs1.Add(StoreKitEventListener.skin_may3);
		strs1.Add(StoreKitEventListener.skin_may4);
		strs1.Add("skin_july1");
		strs1.Add("skin_july2");
		strs1.Add("skin_july3");
		strs1.Add("skin_july4");
		StoreKitEventListener.skinIDs = strs1.ToArray();
		StoreKitEventListener.idsForSingle = new string[] { "bigammopack", "Fullhealth", "ironSword", "MinerWeapon", "steelAxe", "spas", StoreKitEventListener.elixirID, "glock", "chainsaw", "scythe", "shovel" };
		StoreKitEventListener.idsForMulti = new string[] { StoreKitEventListener.idsForSingle[2], StoreKitEventListener.idsForSingle[3], "steelAxe", "woodenBow", "combatrifle", "spas", "goldeneagle", StoreKitEventListener.idsForSingle[7], StoreKitEventListener.idsForSingle[8], "famas" };
		StoreKitEventListener.idsForFull = new string[] { StoreKitEventListener.fullVersion };
		StoreKitEventListener.categoriesMulti = new string[][] { new string[] { StoreKitEventListener.idsForSingle[0], StoreKitEventListener.idsForSingle[1], StoreKitEventListener.armor, StoreKitEventListener.armor2, StoreKitEventListener.armor3 }, PotionsController.potions };
		StoreKitEventListener.categoriesSingle = StoreKitEventListener.categoriesMulti;
	}

	public StoreKitEventListener()
	{
	}

	private void AddCurrencyAndConsumeNextGooglePlayPurchase()
	{
		try
		{
			GooglePurchase googlePurchase = null;
			if (this._cheatedPurchasesToConsume.Count <= 0)
			{
				googlePurchase = this._purchasesToConsume.FirstOrDefault<GooglePurchase>((GooglePurchase p) => StoreKitEventListener.IsVirtualCurrency(p.productId));
				if (googlePurchase != null)
				{
					string empty = string.Empty;
					if (Storager.hasKey("Android.GooglePlayOrderIdsKey"))
					{
						empty = Storager.getString("Android.GooglePlayOrderIdsKey", false);
					}
					if (string.IsNullOrEmpty(empty))
					{
						empty = "[]";
					}
					if ((new HashSet<string>((Json.Deserialize(empty) as List<object> ?? new List<object>()).OfType<string>())).Contains(googlePurchase.orderId))
					{
						UnityEngine.Debug.Log(string.Concat("StoreKitEventListener.AddCurrencyAndConsumeNextGooglePlayPurchase(): Consuming Goole purchase ", googlePurchase.ToString()));
						StoreKitEventListener.GooglePlayConsumeAndSave(googlePurchase);
					}
					else if (!StoreKitEventListener.listOfIdsForWhichX3WaitingCoroutinesRun.Contains(googlePurchase.orderId))
					{
						if (CoroutineRunner.Instance == null)
						{
							UnityEngine.Debug.LogError("AddCurrencyAndConsumeNextGooglePlayPurchase CoroutineRunner.Instance == null ");
							this.AndroidAddCurrencyAndConsume(googlePurchase);
						}
						else
						{
							CoroutineRunner.Instance.StartCoroutine(this.WaitForX3AndGiveCurrency(googlePurchase, null));
						}
					}
				}
			}
			else
			{
				googlePurchase = this._cheatedPurchasesToConsume.FirstOrDefault<GooglePurchase>((GooglePurchase p) => StoreKitEventListener.IsVirtualCurrency(p.productId));
				if (googlePurchase != null)
				{
					UnityEngine.Debug.Log(string.Concat("StoreKitEventListener.AddCurrencyAndConsumeNextGooglePlayPurchase(): Consuming Goole purchase ", googlePurchase.ToString()));
					StoreKitEventListener.GooglePlayConsumeAndSave(googlePurchase);
				}
			}
		}
		catch (Exception exception1)
		{
			Exception exception = exception1;
			UnityEngine.Debug.LogError(string.Concat(new object[] { "AddCurrencyAndConsumeNextGooglePlayPurchase exception: ", exception, "\nstacktrace:\n", Environment.StackTrace }));
		}
	}

	private static void AmazonNotifyFulfillmentAndSave(NotifyFulfillmentInput notifyFulfillmentInput)
	{
		if (notifyFulfillmentInput == null)
		{
			throw new ArgumentNullException("notifyFulfillmentInput");
		}
		string empty = string.Empty;
		if (Storager.hasKey("Amazon.FulfilledReceipts"))
		{
			empty = Storager.getString("Amazon.FulfilledReceipts", false);
		}
		if (string.IsNullOrEmpty(empty))
		{
			empty = "[]";
		}
		List<object> objs = Json.Deserialize(empty) as List<object> ?? new List<object>();
		AmazonIapV2Impl.Instance.NotifyFulfillment(notifyFulfillmentInput);
		HashSet<string> strs = new HashSet<string>(objs.OfType<string>());
		strs.Add(notifyFulfillmentInput.ReceiptId);
		empty = Json.Serialize(strs.ToList<string>());
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log(string.Concat("[Rilisoft] Saving fulfillments: ", empty));
		}
		Storager.setString("Amazon.FulfilledReceipts", empty, false);
	}

	private void AndroidAddCurrencyAndConsume(GooglePurchase purchase)
	{
		this.TryAddVirtualCrrency(purchase.productId);
		UnityEngine.Debug.Log(string.Concat("StoreKitEventListener.AddCurrencyAndConsumeNextGooglePlayPurchase(): Consuming Goole purchase ", purchase.ToString()));
		StoreKitEventListener.GooglePlayConsumeAndSave(purchase);
		if (StoreKitEventListener.IsSinglePurchase(purchase))
		{
			this.SendFirstTimePayment(purchase);
		}
		this.LogRealPayment(purchase);
	}

	private void Awake()
	{
		StoreKitEventListener.Instance = this;
	}

	private void billingNotSupportedEvent(string error)
	{
		StoreKitEventListener.billingSupported = false;
		UnityEngine.Debug.LogWarning(string.Concat("billingNotSupportedEvent: ", error));
	}

	private void billingSupportedEvent()
	{
		StoreKitEventListener.billingSupported = true;
		UnityEngine.Debug.Log("billingSupportedEvent");
		StoreKitEventListener.RefreshProducts();
	}

	internal static void CheckIfFirstTimePayment()
	{
		if (!Storager.hasKey("PayingUser") || Storager.getInt("PayingUser", true) != 1)
		{
			Storager.setInt("PayingUser", 1, true);
			if (CoroutineRunner.Instance == null)
			{
				UnityEngine.Debug.LogError("CheckIfFirstTimePayment CoroutineRunner.Instance == null");
			}
			else
			{
				CoroutineRunner.Instance.StartCoroutine(StoreKitEventListener.WaitForFyberAndSetIsPaying());
			}
			FlurryPluginWrapper.LogEvent("USER FirstTimePayment");
		}
	}

	private void ConsumeProductIfCheating(GooglePurchase purchase)
	{
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.LogError(string.Concat("Consuming cheated purchase: ", purchase.ToString()));
		}
		StoreKitEventListener.GooglePlayConsumeAndSave(purchase);
	}

	private void consumePurchaseFailedEvent(string error)
	{
		UnityEngine.Debug.LogWarning(string.Concat("consumePurchaseFailedEvent: ", error));
	}

	private void consumePurchaseSucceededEvent(GooglePurchase purchase)
	{
		UnityEngine.Debug.Log(string.Concat("consumePurchaseSucceededEvent: ", purchase));
		if (this._cheatedPurchasesToConsume.RemoveWhere((GooglePurchase p) => p.productId == purchase.productId) == 0)
		{
			this._purchasesToConsume.RemoveWhere((GooglePurchase p) => p.productId == purchase.productId);
		}
		this.AddCurrencyAndConsumeNextGooglePlayPurchase();
	}

	private static NotifyFulfillmentInput FulfillmentInputForReceipt(PurchaseReceipt receipt)
	{
		NotifyFulfillmentInput notifyFulfillmentInput = new NotifyFulfillmentInput()
		{
			ReceiptId = receipt.ReceiptId,
			FulfillmentResult = "FULFILLED"
		};
		return notifyFulfillmentInput;
	}

	public static int GetDollarsSpent()
	{
		return PlayerPrefs.GetInt("ALLCoins", 0) + PlayerPrefs.GetInt("ALLGems", 0);
	}

	private static void GiveCoinsOrGemsOnAmazon(PurchaseReceipt receipt)
	{
		try
		{
			UnityEngine.Debug.Log(string.Concat("[Rilisoft] Amazon: restoring purchase: ", receipt.Sku));
			int num = Array.IndexOf<string>(StoreKitEventListener.coinIds, receipt.Sku);
			if (num >= StoreKitEventListener.coinIds.GetLowerBound(0))
			{
				int num1 = Mathf.RoundToInt((float)VirtualCurrencyHelper.GetCoinInappsQuantity(num) * PremiumAccountController.VirtualCurrencyMultiplier);
				int num2 = Storager.getInt("Coins", false) + num1;
				Storager.setInt("Coins", num2, false);
				AnalyticsFacade.CurrencyAccrual(num1, "Coins", AnalyticsConstants.AccrualType.Purchased);
				try
				{
					ChestBonusController.TryTakeChestBonus(false, num);
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogError(string.Concat("[Rilisoft] Amazon: TryTakeChestBonus exception: ", exception));
				}
				coinsShop.TryToFireCurrenciesAddEvent("Coins");
				FlurryEvents.PaymentTime = new float?(Time.realtimeSinceStartup);
				StoreKitEventListener.CheckIfFirstTimePayment();
				StoreKitEventListener.SetLastPaymentTime();
				StoreKitEventListener.LogVirtualCurrencyPurchased(receipt.Sku, num1, false);
			}
			num = Array.IndexOf<string>(StoreKitEventListener.gemsIds, receipt.Sku);
			if (num >= StoreKitEventListener.gemsIds.GetLowerBound(0))
			{
				int num3 = Mathf.RoundToInt((float)VirtualCurrencyHelper.GetGemsInappsQuantity(num) * PremiumAccountController.VirtualCurrencyMultiplier);
				string str = string.Format("Process purchase {0}, VirtualCurrencyHelper.GetGemsInappsQuantity({1})", receipt.Sku, num);
				UnityEngine.Debug.Log(str);
				int num4 = Storager.getInt("GemsCurrency", false) + num3;
				Storager.setInt("GemsCurrency", num4, false);
				AnalyticsFacade.CurrencyAccrual(num3, "GemsCurrency", AnalyticsConstants.AccrualType.Purchased);
				try
				{
					ChestBonusController.TryTakeChestBonus(true, num);
				}
				catch (Exception exception1)
				{
					UnityEngine.Debug.LogError(string.Concat("[Rilisoft] Amazon: TryTakeChestBonus exception: ", exception1));
				}
				coinsShop.TryToFireCurrenciesAddEvent("GemsCurrency");
				FlurryEvents.PaymentTime = new float?(Time.realtimeSinceStartup);
				StoreKitEventListener.CheckIfFirstTimePayment();
				StoreKitEventListener.SetLastPaymentTime();
				StoreKitEventListener.LogVirtualCurrencyPurchased(receipt.Sku, num3, true);
			}
			NotifyFulfillmentInput notifyFulfillmentInput = StoreKitEventListener.FulfillmentInputForReceipt(receipt);
			UnityEngine.Debug.Log(string.Concat("Amazon NotifyFulfillment (HandlePurchaseUpdatesRequestSuccessfulEvent): ", notifyFulfillmentInput.ToJson()));
			StoreKitEventListener.AmazonNotifyFulfillmentAndSave(notifyFulfillmentInput);
		}
		catch (Exception exception2)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception GiveCoinsOrGemsOnAmazon: ", exception2));
		}
	}

	private static void GooglePlayConsumeAndSave(GooglePurchase purchase)
	{
		try
		{
			if (purchase != null)
			{
				string empty = string.Empty;
				if (Storager.hasKey("Android.GooglePlayOrderIdsKey"))
				{
					empty = Storager.getString("Android.GooglePlayOrderIdsKey", false);
				}
				if (string.IsNullOrEmpty(empty))
				{
					empty = "[]";
				}
				List<object> objs = Json.Deserialize(empty) as List<object> ?? new List<object>();
				GoogleIAB.consumeProduct(purchase.productId);
				HashSet<string> strs = new HashSet<string>(objs.OfType<string>());
				strs.Add(purchase.orderId);
				empty = Json.Serialize(strs.ToList<string>());
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.Log(string.Concat("[Rilisoft] Saving consumed order ids: ", empty));
				}
				Storager.setString("Android.GooglePlayOrderIdsKey", empty, false);
			}
			else
			{
				UnityEngine.Debug.LogWarning("GooglePlayConsumeAndSave: purchase == null");
			}
		}
		catch (Exception exception1)
		{
			Exception exception = exception1;
			UnityEngine.Debug.LogError(string.Concat(new object[] { "GooglePlayConsumeAndSave exception: ", exception, "\nstacktrace:\n", Environment.StackTrace }));
		}
	}

	private void HandleAmazonSdkAvailableEvent(bool isSandboxMode)
	{
		UnityEngine.Debug.Log(string.Concat("Amazon SDK available in sandbox mode: ", isSandboxMode));
		StoreKitEventListener.billingSupported = true;
		StoreKitEventListener.RefreshProducts();
	}

	private void HandleGetUserIdResponseEvent(GetUserDataResponse response)
	{
		string str = string.Concat("Amazon GetUserDataResponse: ", response.Status);
		if (!"SUCCESSFUL".Equals(response.Status, StringComparison.OrdinalIgnoreCase))
		{
			UnityEngine.Debug.LogWarning(str);
			this._amazonUserPromise.TrySetException(new InvalidOperationException(str));
			return;
		}
		UnityEngine.Debug.Log(str);
		this._amazonUserPromise.TrySetResult(response.AmazonUserData);
	}

	private void HandleGooglePurchaseSucceeded(GooglePurchase purchase)
	{
		decimal num;
		UnityEngine.Debug.Log(string.Concat("HandleGooglePurchaseSucceeded: ", purchase));
		if (coinsShop.IsWideLayoutAvailable)
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogError("Cheating attempt.");
			}
			this.ConsumeProductIfCheating(purchase);
			return;
		}
		if (!coinsShop.CheckHostsTimestamp())
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogError("Hosts tampering attempt.");
			}
			this.ConsumeProductIfCheating(purchase);
			return;
		}
		if (!this.VerifyPurchase(purchase.originalJson, purchase.signature))
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogError("Purchase verification failed.");
			}
			this.ConsumeProductIfCheating(purchase);
			return;
		}
		StoreKitEventListener.ContentType contentType = StoreKitEventListener.ContentType.Unknown;
		try
		{
			if (this.TryAddVirtualCrrency(purchase.productId))
			{
				if (Array.IndexOf<string>(StoreKitEventListener.coinIds, purchase.productId) >= 0)
				{
					contentType = StoreKitEventListener.ContentType.Coins;
				}
				else if (Array.IndexOf<string>(StoreKitEventListener.gemsIds, purchase.productId) >= 0)
				{
					contentType = StoreKitEventListener.ContentType.Gems;
				}
				UnityEngine.Debug.Log(string.Concat("StoreKitEventListener.HandleGooglePurchaseSucceeded(): Consuming Goole product ", purchase.productId));
				StoreKitEventListener.GooglePlayConsumeAndSave(purchase);
			}
			else if (this.TryAddStarterPackItem(purchase.productId))
			{
				contentType = StoreKitEventListener.ContentType.StarterPack;
			}
			if (StoreKitEventListener.IsSinglePurchase(purchase))
			{
				this.SendFirstTimePayment(purchase);
			}
			this.LogRealPayment(purchase);
		}
		finally
		{
			StoreKitEventListener.purchaseInProcess = false;
			StoreKitEventListener.restoreInProcess = false;
			if (purchase.purchaseState == GooglePurchase.GooglePurchaseState.Purchased)
			{
				if (!VirtualCurrencyHelper.ReferencePricesInUsd.TryGetValue(purchase.productId, out num))
				{
					UnityEngine.Debug.LogErrorFormat("Cannot find price for product {0}", new object[] { purchase.productId });
				}
				else
				{
					decimal num1 = Math.Round(num, 0, MidpointRounding.AwayFromZero);
					Dictionary<string, string> strs = new Dictionary<string, string>()
					{
						{ "af_revenue", num1.ToString("F2") },
						{ "af_content_type", contentType.ToString() },
						{ "af_content_id", purchase.productId },
						{ "af_currency", "USD" },
						{ "af_validated", "true" },
						{ "af_receipt_id", purchase.orderId }
					};
					FlurryPluginWrapper.LogEventToAppsFlyer("af_purchase", strs);
				}
			}
		}
	}

	private void HandleItemDataRequestFailedEvent()
	{
		UnityEngine.Debug.LogWarning("Amamzon: Item data request failed.");
	}

	private void HandleItemDataRequestFinishedEvent(GetProductDataResponse response)
	{
		string str = string.Concat("Amazon: GetProductDataResponse: ", response.Status);
		if (!"SUCCESSFUL".Equals(response.Status, StringComparison.OrdinalIgnoreCase))
		{
			UnityEngine.Debug.LogWarning(str);
			return;
		}
		UnityEngine.Debug.Log(str);
		this._products.Clear();
		try
		{
			List<string> list = response.ProductDataMap.Keys.ToList<string>();
			string str1 = Json.Serialize(list);
			string str2 = Json.Serialize(response.UnavailableSkus);
			UnityEngine.Debug.Log(string.Format("Item data request finished;    Unavailable skus: {0}, Available skus: {1}", str2, str1));
			IEnumerable<AmazonMarketProduct> amazonMarketProducts = response.ProductDataMap.Values.Select<ProductData, AmazonMarketProduct>(new Func<ProductData, AmazonMarketProduct>(MarketProductFactory.CreateAmazonMarketProduct));
			IEnumerator<AmazonMarketProduct> enumerator = amazonMarketProducts.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					AmazonMarketProduct current = enumerator.Current;
					if (this._products.Contains(current))
					{
						continue;
					}
					this._products.Add(current);
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
		finally
		{
			StoreKitEventListener.purchaseInProcess = false;
			StoreKitEventListener.restoreInProcess = false;
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log("[Rilisoft] Amazon: calling GetPurchaseUpdates()");
			}
			IAmazonIapV2 instance = AmazonIapV2Impl.Instance;
			ResetInput resetInput = new ResetInput()
			{
				Reset = true
			};
			instance.GetPurchaseUpdates(resetInput);
		}
	}

	private void HandlePurchaseSuccessfulEventAmazon(PurchaseResponse response)
	{
		string str = string.Concat("Amazon PurchaseResponse (StoreKitEventListener): ", response.Status);
		if (!"SUCCESSFUL".Equals(response.Status, StringComparison.OrdinalIgnoreCase))
		{
			UnityEngine.Debug.LogWarning(str);
			StoreKitEventListener.purchaseInProcess = false;
			return;
		}
		UnityEngine.Debug.Log(str);
		PurchaseReceipt purchaseReceipt = response.PurchaseReceipt;
		UnityEngine.Debug.Log(string.Concat("Amazon PurchaseResponse.PurchaseReceipt: ", purchaseReceipt.ToJson()));
		try
		{
			NotifyFulfillmentInput notifyFulfillmentInput = new NotifyFulfillmentInput()
			{
				ReceiptId = purchaseReceipt.ReceiptId,
				FulfillmentResult = "FULFILLED"
			};
			NotifyFulfillmentInput notifyFulfillmentInput1 = notifyFulfillmentInput;
			int num = Array.IndexOf<string>(StoreKitEventListener.coinIds, purchaseReceipt.Sku);
			if (num >= StoreKitEventListener.coinIds.GetLowerBound(0))
			{
				int num1 = Mathf.RoundToInt((float)VirtualCurrencyHelper.GetCoinInappsQuantity(num) * PremiumAccountController.VirtualCurrencyMultiplier);
				string str1 = string.Format("Process purchase {0}, VirtualCurrencyHelper.GetCoinInappsQuantity({1})", purchaseReceipt.Sku, num);
				UnityEngine.Debug.Log(str1);
				int num2 = Storager.getInt("Coins", false) + num1;
				Storager.setInt("Coins", num2, false);
				AnalyticsFacade.CurrencyAccrual(num1, "Coins", AnalyticsConstants.AccrualType.Purchased);
				UnityEngine.Debug.Log(string.Concat("Amazon NotifyFulfillment (HandlePurchaseSuccessfulEvent): ", notifyFulfillmentInput1.ToJson()));
				StoreKitEventListener.AmazonNotifyFulfillmentAndSave(notifyFulfillmentInput1);
				ChestBonusController.TryTakeChestBonus(false, num);
				coinsShop.TryToFireCurrenciesAddEvent("Coins");
				FlurryEvents.PaymentTime = new float?(Time.realtimeSinceStartup);
				StoreKitEventListener.CheckIfFirstTimePayment();
				StoreKitEventListener.SetLastPaymentTime();
				StoreKitEventListener.LogVirtualCurrencyPurchased(purchaseReceipt.Sku, num1, false);
			}
			num = Array.IndexOf<string>(StoreKitEventListener.gemsIds, purchaseReceipt.Sku);
			if (num >= StoreKitEventListener.gemsIds.GetLowerBound(0))
			{
				int num3 = Mathf.RoundToInt((float)VirtualCurrencyHelper.GetGemsInappsQuantity(num) * PremiumAccountController.VirtualCurrencyMultiplier);
				string str2 = string.Format("Process purchase {0}, VirtualCurrencyHelper.GetGemsInappsQuantity({1})", purchaseReceipt.Sku, num);
				UnityEngine.Debug.Log(str2);
				int num4 = Storager.getInt("GemsCurrency", false) + num3;
				Storager.setInt("GemsCurrency", num4, false);
				AnalyticsFacade.CurrencyAccrual(num3, "GemsCurrency", AnalyticsConstants.AccrualType.Purchased);
				UnityEngine.Debug.Log(string.Concat("Amazon NotifyFulfillment (HandlePurchaseSuccessfulEvent): ", notifyFulfillmentInput1.ToJson()));
				StoreKitEventListener.AmazonNotifyFulfillmentAndSave(notifyFulfillmentInput1);
				ChestBonusController.TryTakeChestBonus(true, num);
				coinsShop.TryToFireCurrenciesAddEvent("GemsCurrency");
				FlurryEvents.PaymentTime = new float?(Time.realtimeSinceStartup);
				StoreKitEventListener.CheckIfFirstTimePayment();
				StoreKitEventListener.SetLastPaymentTime();
				StoreKitEventListener.LogVirtualCurrencyPurchased(purchaseReceipt.Sku, num3, true);
			}
			if (this.TryAddStarterPackItem(purchaseReceipt.Sku))
			{
				string str3 = string.Format("Process purchase {0}. Starter pack.", purchaseReceipt.Sku, num);
				UnityEngine.Debug.Log(str3);
			}
			FriendsController.sharedController.SendOurData(false);
		}
		finally
		{
			StoreKitEventListener.purchaseInProcess = false;
			StoreKitEventListener.restoreInProcess = false;
		}
	}

	private void HandlePurchaseUpdatesRequestFailedEvent()
	{
		UnityEngine.Debug.LogWarning("Amazon: Purchase updates request failed.");
	}

	private void HandlePurchaseUpdatesRequestSuccessfulEvent(GetPurchaseUpdatesResponse response)
	{
		string str = string.Concat("[Rilisoft] Amazon GetPurchaseUpdatesResponse: ", response.ToJson());
		if (!"SUCCESSFUL".Equals(response.Status, StringComparison.OrdinalIgnoreCase))
		{
			UnityEngine.Debug.LogWarning(str);
			return;
		}
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log(str);
		}
		string empty = string.Empty;
		if (Storager.hasKey("Amazon.FulfilledReceipts"))
		{
			empty = Storager.getString("Amazon.FulfilledReceipts", false);
		}
		if (string.IsNullOrEmpty(empty))
		{
			empty = "[]";
		}
		HashSet<string> strs = new HashSet<string>((Json.Deserialize(empty) as List<object> ?? new List<object>()).OfType<string>());
		List<PurchaseReceipt> receipts = response.Receipts;
		for (int i = 0; i != receipts.Count; i++)
		{
			PurchaseReceipt item = receipts[i];
			string sku = item.Sku;
			if (!StoreKitEventListener.starterPackIds.Contains<string>(sku))
			{
				try
				{
					if (StoreKitEventListener.coinIds.Contains<string>(sku) || StoreKitEventListener.gemsIds.Contains<string>(sku))
					{
						if (strs.Contains(item.ReceiptId))
						{
							NotifyFulfillmentInput notifyFulfillmentInput = StoreKitEventListener.FulfillmentInputForReceipt(item);
							UnityEngine.Debug.Log(string.Concat("Amazon NotifyFulfillment (HandlePurchaseUpdatesRequestSuccessfulEvent): ", notifyFulfillmentInput.ToJson()));
							StoreKitEventListener.AmazonNotifyFulfillmentAndSave(notifyFulfillmentInput);
						}
						else if (!StoreKitEventListener.listOfIdsForWhichX3WaitingCoroutinesRun.Contains(item.ReceiptId))
						{
							if (CoroutineRunner.Instance == null)
							{
								UnityEngine.Debug.LogError("Amazon NotifyFulfillment CoroutineRunner.Instance == null ");
								StoreKitEventListener.GiveCoinsOrGemsOnAmazon(item);
							}
							else
							{
								CoroutineRunner.Instance.StartCoroutine(this.WaitForX3AndGiveCurrency(null, item));
							}
						}
					}
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogError(string.Concat("Exception HandlePurchaseUpdatesRequestSuccessfulEvent: ", exception));
				}
			}
			else
			{
				StarterPackController.Get.AddBuyAndroidStarterPack(sku);
				StarterPackController.Get.TryRestoreStarterPack(sku);
			}
		}
	}

	private static RSACryptoServiceProvider InitializeRsa()
	{
		RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
		rSACryptoServiceProvider.FromXmlString("<RSAKeyValue><Modulus>oTzMTaqsFhaywvCFKawFwL5KM+djLJfOCT/rbGQRfHmHYmOY2sBMgDWsA/67Szx6EVTZPVlFzHMgkAq1TwdL/A5aYGpGzaCX7o96cyp8R6wSF+xCuj++LAkTaDnLW0veI2bke3EVHu3At9xgM46e+VDucRUqQLvf6SQRb15nuflY5i08xKnewgX7I4U2H0RvAZDyoip+qZPmI4ZvaufAfc0jwZbw7XGiV41zibY3LU0N57mYKk51Wx+tOaJ7Tkc9Rl1qVCTjb+bwXshTqhVXVP6r4kabLWw/8OJUh0Sm69lbps6amP7vPy571XjscCTMLfXQan1959rHbNgkb2mLLQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
		return rSACryptoServiceProvider;
	}

	private static bool IsSinglePurchase(GooglePurchase purchase)
	{
		if (!Storager.hasKey("Android.GooglePlayOrderIdsKey"))
		{
			return false;
		}
		string str = Storager.getString("Android.GooglePlayOrderIdsKey", false);
		if (string.IsNullOrEmpty(str))
		{
			return false;
		}
		List<object> objs = Json.Deserialize(str) as List<object>;
		if (objs == null)
		{
			return false;
		}
		if (objs.Count != 1)
		{
			return false;
		}
		if (objs.OfType<string>().FirstOrDefault<string>(new Func<string, bool>(purchase.productId.Equals)) == null)
		{
			return false;
		}
		return true;
	}

	private static bool IsVirtualCurrency(string productId)
	{
		if (productId == null)
		{
			return false;
		}
		int num = Array.IndexOf<string>(StoreKitEventListener.coinIds, productId);
		int num1 = Array.IndexOf<string>(StoreKitEventListener.gemsIds, productId);
		return (num >= StoreKitEventListener.coinIds.GetLowerBound(0) ? true : num1 >= StoreKitEventListener.gemsIds.GetLowerBound(0));
	}

	private void LogRealPayment(GooglePurchase purchase)
	{
		if (purchase.purchaseState != GooglePurchase.GooglePurchaseState.Purchased)
		{
			return;
		}
		try
		{
			GoogleSkuInfo platformProduct = this.Products.FirstOrDefault<IMarketProduct>((IMarketProduct p) => (p.PlatformProduct as GoogleSkuInfo).productId == purchase.productId).PlatformProduct as GoogleSkuInfo;
			decimal num = decimal.Divide(platformProduct.priceAmountMicros, new decimal(1000000));
			AnalyticsFacade.RealPayment(purchase.orderId, (float)num, AnalyticsStuff.ReadableNameForInApp(platformProduct.productId), platformProduct.priceCurrencyCode);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in RealPayment: ", exception));
		}
	}

	public static void LogVirtualCurrencyPurchased(string purchaseId, int virtualCurrencyCount, bool isGems)
	{
		int num;
		try
		{
			if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.AnyDiscountForTryGuns)
			{
				AnalyticsStuff.LogWEaponsSpecialOffers_MoneySpended(purchaseId);
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("LogVirtualCurrencyPurchased exception (Weapons Special Offers): ", exception));
		}
		try
		{
			FlurryEvents.LogBecomePaying(purchaseId);
			if (GiftBannerWindow.instance != null && GiftBannerWindow.instance.IsShow)
			{
				AnalyticsStuff.LogDailyGiftPurchases(purchaseId);
			}
			if (BuySmileBannerController.openedFromPromoActions || ShopNGUIController.sharedShop != null && ShopNGUIController.sharedShop.IsFromPromoActions)
			{
				AnalyticsStuff.LogSpecialOffersPanel((!isGems ? "Buy Coins" : "Buy Gems"), AnalyticsStuff.ReadableNameForInApp(purchaseId), null, null);
			}
		}
		catch (Exception exception1)
		{
			UnityEngine.Debug.LogError(string.Concat("LogVirtualCurrencyPurchased exception: ", exception1));
		}
		string str = SystemInfo.deviceModel;
		ShopNGUIController.AddBoughtCurrency((!isGems ? "Coins" : "GemsCurrency"), virtualCurrencyCount);
		string str1 = string.Format("{0} ({1})", purchaseId, virtualCurrencyCount);
		int num1 = PlayerPrefs.GetInt(Defs.SessionNumberKey, 1);
		string str2 = num1.ToString();
		string str3 = (!isGems ? "Coins Purchased Total" : "Gems Purchased Total");
		string str4 = (ExperienceController.sharedController == null ? "Unknown" : ExperienceController.sharedController.currentLevel.ToString());
		string eventX3State = FlurryPluginWrapper.GetEventX3State();
		Dictionary<string, string> strs = new Dictionary<string, string>()
		{
			{ "Mode", StoreKitEventListener.State.Mode ?? string.Empty },
			{ "Rank", str4 },
			{ "Session number", str2 },
			{ "SKU", str1 },
			{ "Device model", str },
			{ "X3", eventX3State }
		};
		FlurryPluginWrapper.LogEventAndDublicateToConsole(str3, strs, true);
		string str5 = string.Concat((!isGems ? "Coins Purchased " : "Gems Purchased "), StoreKitEventListener.State.Mode) ?? string.Empty;
		strs = new Dictionary<string, string>(StoreKitEventListener.State.Parameters)
		{
			{ StoreKitEventListener.State.PurchaseKey, purchaseId },
			{ "Rank", str4 },
			{ "Session number", str2 },
			{ "SKU", str1 },
			{ "Device model", str }
		};
		Dictionary<string, string> strs1 = strs;
		FlurryPluginWrapper.LogEventAndDublicateToConsole(str5, strs1, true);
		FlurryPluginWrapper.LogEventToAppsFlyer(str5, strs1);
		if (ExperienceController.sharedController != null)
		{
			int num2 = ExperienceController.sharedController.currentLevel;
			int num3 = (num2 - 1) / 9;
			string str6 = string.Format("[{0}, {1})", num3 * 9 + 1, (num3 + 1) * 9 + 1);
			string str7 = string.Format((!isGems ? "Coins Payment On Level {0}{1}" : "Gems Payment On Level {0}{1}"), str6, string.Empty);
			strs = new Dictionary<string, string>()
			{
				{ string.Concat("Level ", num2), str1 }
			};
			FlurryPluginWrapper.LogEventAndDublicateToConsole(str7, strs, true);
		}
		int ourTier = ExpController.GetOurTier();
		int num4 = (ExperienceController.sharedController == null ? 1000 : ExperienceController.sharedController.currentLevel);
		strs = new Dictionary<string, string>()
		{
			{ string.Concat("Lev", num4), str1 },
			{ "TOTAL", str1 }
		};
		Dictionary<string, string> strs2 = strs;
		string str8 = string.Format(" (Tier {0}){1}{2}", ourTier, FlurryPluginWrapper.GetPayingSuffix(), string.Empty);
		FlurryPluginWrapper.LogEventAndDublicateToConsole(string.Concat((!isGems ? "Coins Purchase Total" : "Gems Purchase Total"), str8), strs2, true);
		FlurryPluginWrapper.LogEventAndDublicateToConsole(string.Concat("IAP Purchase Total", str8), strs2, true);
		int num5 = PlayerPrefs.GetInt("CountPaying", 0);
		int num6 = Array.IndexOf<string>(StoreKitEventListener.coinIds, purchaseId);
		bool flag = false;
		if (num6 == -1)
		{
			num6 = Array.IndexOf<string>(StoreKitEventListener.gemsIds, purchaseId);
			if (num6 == -1)
			{
				num6 = Array.IndexOf<string>(StoreKitEventListener.starterPackIds, purchaseId);
				flag = true;
			}
		}
		if (FriendsController.sharedController != null)
		{
			FriendsController friendsController = FriendsController.sharedController;
			if (num6 == -1 || flag)
			{
				num = 0;
			}
			else
			{
				num = (!isGems ? VirtualCurrencyHelper.coinPriceIds[num6] : VirtualCurrencyHelper.gemsPriceIds[num6]);
			}
			friendsController.SendAddPurchaseEvent(num, purchaseId);
		}
		if (num6 != -1)
		{
			if (FriendsController.useBuffSystem)
			{
				BuffSystem.instance.OnCurrencyBuyed(isGems, num6);
			}
			int num7 = 0;
			if (!isGems)
			{
				num7 = PlayerPrefs.GetInt("ALLCoins", 0);
				num7 = num7 + (!flag ? VirtualCurrencyHelper.coinPriceIds[num6] : VirtualCurrencyHelper.starterPackFakePrice[num6]);
				PlayerPrefs.SetInt("ALLCoins", num7);
			}
			else
			{
				num7 = PlayerPrefs.GetInt("ALLGems", 0);
				num7 = num7 + (!flag ? VirtualCurrencyHelper.gemsPriceIds[num6] : VirtualCurrencyHelper.starterPackFakePrice[num6]);
				PlayerPrefs.SetInt("ALLGems", num7);
			}
			if (!flag)
			{
				Storager.setInt(string.Concat(Defs.AllCurrencyBought, (!isGems ? "Coins" : "GemsCurrency")), Storager.getInt(string.Concat(Defs.AllCurrencyBought, (!isGems ? "Coins" : "GemsCurrency")), false) + virtualCurrencyCount, false);
			}
			num5++;
			PlayerPrefs.SetInt("CountPaying", num5);
			if (num5 >= 1 && PlayerPrefs.GetInt("Paying_User", 0) == 0)
			{
				PlayerPrefs.SetInt("Paying_User", 1);
				FacebookController.LogEvent("Paying_User", null);
				UnityEngine.Debug.Log("Paying_User detected.");
			}
			if (num5 > 1 && PlayerPrefs.GetInt("Paying_User_Dolphin", 0) == 0)
			{
				PlayerPrefs.SetInt("Paying_User_Dolphin", 1);
				FacebookController.LogEvent("Paying_User_Dolphin", null);
				UnityEngine.Debug.Log("Paying_User_Dolphin detected.");
			}
			if (num5 > 3 && PlayerPrefs.GetInt("Paying_User_Whale", 0) == 0)
			{
				PlayerPrefs.SetInt("Paying_User_Whale", 1);
				FacebookController.LogEvent("Paying_User_Whale", null);
				UnityEngine.Debug.Log("Paying_User_Whale detected.");
			}
			if (num7 >= 100 && PlayerPrefs.GetInt("SendKit", 0) == 0)
			{
				PlayerPrefs.SetInt("SendKit", 1);
				FacebookController.LogEvent("Whale_detected", null);
				UnityEngine.Debug.Log("Whale detected.");
			}
			if (PlayerPrefs.GetInt("confirmed_1st_time", 0) == 0)
			{
				PlayerPrefs.SetInt("confirmed_1st_time", 1);
				FacebookController.LogEvent("Purchase_confirmed_1st_time", null);
				UnityEngine.Debug.Log("Purchase confirmed first time.");
			}
			if (PlayerPrefs.GetInt("Active_loyal_users_payed_send", 0) == 0 && PlayerPrefs.GetInt("PostFacebookCount", 0) > 2 && PlayerPrefs.GetInt("PostVideo", 0) > 0)
			{
				FacebookController.LogEvent("Active_loyal_users_payed", null);
				PlayerPrefs.SetInt("Active_loyal_users_payed_send", 1);
			}
		}
		else
		{
			UnityEngine.Debug.Log(string.Format("Could not find “{0}” value in coinIds array.", purchaseId));
		}
	}

	[DebuggerHidden]
	public IEnumerator MyWaitForSeconds(float tm)
	{
		StoreKitEventListener.u003cMyWaitForSecondsu003ec__Iterator194 variable = null;
		return variable;
	}

	private void OnDestroy()
	{
		StoreKitEventListener.Instance = null;
	}

	private void OnDisable()
	{
		this._purchaseFailedSubscription.Dispose();
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			GoogleIABManager.billingSupportedEvent -= new Action(this.billingSupportedEvent);
			GoogleIABManager.billingNotSupportedEvent -= new Action<string>(this.billingNotSupportedEvent);
			GoogleIABManager.queryInventorySucceededEvent -= new Action<List<GooglePurchase>, List<GoogleSkuInfo>>(this.queryInventorySucceededEvent);
			GoogleIABManager.queryInventoryFailedEvent -= new Action<string>(this.queryInventoryFailedEvent);
			GoogleIABManager.purchaseCompleteAwaitingVerificationEvent -= new Action<string, string>(this.purchaseCompleteAwaitingVerificationEvent);
			GoogleIABManager.purchaseSucceededEvent -= new Action<GooglePurchase>(this.HandleGooglePurchaseSucceeded);
			GoogleIABManager.consumePurchaseSucceededEvent -= new Action<GooglePurchase>(this.consumePurchaseSucceededEvent);
			GoogleIABManager.consumePurchaseFailedEvent -= new Action<string>(this.consumePurchaseFailedEvent);
		}
		else
		{
			AmazonIapV2Impl.Instance.RemoveGetUserDataResponseListener(new GetUserDataResponseDelegate(this.HandleGetUserIdResponseEvent));
			AmazonIapV2Impl.Instance.RemoveGetProductDataResponseListener(new GetProductDataResponseDelegate(this.HandleItemDataRequestFinishedEvent));
			AmazonIapV2Impl.Instance.RemovePurchaseResponseListener(new PurchaseResponseDelegate(this.HandlePurchaseSuccessfulEventAmazon));
			AmazonIapV2Impl.Instance.AddGetPurchaseUpdatesResponseListener(new GetPurchaseUpdatesResponseDelegate(this.HandlePurchaseUpdatesRequestSuccessfulEvent));
		}
	}

	private void OnEnable()
	{
		this._purchaseFailedSubscription.Dispose();
		Action<string, int> action = (string error, int response) => {
			StoreKitEventListener.purchaseInProcess = false;
			UnityEngine.Debug.LogWarning(string.Format("googlePurchaseFailedHandler({0}): {1}", response, error));
		};
		this._purchaseFailedSubscription = new ActionDisposable(() => GoogleIABManager.purchaseFailedEvent -= action);
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			GoogleIABManager.billingSupportedEvent += new Action(this.billingSupportedEvent);
			GoogleIABManager.billingNotSupportedEvent += new Action<string>(this.billingNotSupportedEvent);
			GoogleIABManager.queryInventorySucceededEvent += new Action<List<GooglePurchase>, List<GoogleSkuInfo>>(this.queryInventorySucceededEvent);
			GoogleIABManager.queryInventoryFailedEvent += new Action<string>(this.queryInventoryFailedEvent);
			GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += new Action<string, string>(this.purchaseCompleteAwaitingVerificationEvent);
			GoogleIABManager.purchaseSucceededEvent += new Action<GooglePurchase>(this.HandleGooglePurchaseSucceeded);
			GoogleIABManager.purchaseFailedEvent += action;
			GoogleIABManager.consumePurchaseSucceededEvent += new Action<GooglePurchase>(this.consumePurchaseSucceededEvent);
			GoogleIABManager.consumePurchaseFailedEvent += new Action<string>(this.consumePurchaseFailedEvent);
		}
		else
		{
			AmazonIapV2Impl.Instance.AddGetUserDataResponseListener(new GetUserDataResponseDelegate(this.HandleGetUserIdResponseEvent));
			AmazonIapV2Impl.Instance.AddGetProductDataResponseListener(new GetProductDataResponseDelegate(this.HandleItemDataRequestFinishedEvent));
			AmazonIapV2Impl.Instance.AddPurchaseResponseListener(new PurchaseResponseDelegate(this.HandlePurchaseSuccessfulEventAmazon));
			AmazonIapV2Impl.Instance.AddGetPurchaseUpdatesResponseListener(new GetPurchaseUpdatesResponseDelegate(this.HandlePurchaseUpdatesRequestSuccessfulEvent));
			this.HandleAmazonSdkAvailableEvent(false);
			UnityEngine.Debug.Log("Amazon GetUserData (StoreKitEventListener.OnEnable)");
			AmazonIapV2Impl.Instance.GetUserData();
		}
	}

	public void ProvideContent()
	{
	}

	private void purchaseCompleteAwaitingVerificationEvent(string purchaseData, string signature)
	{
		UnityEngine.Debug.Log(string.Concat("purchaseCompleteAwaitingVerificationEvent. purchaseData: ", purchaseData, ", signature: ", signature));
	}

	private void queryInventoryFailedEvent(string error)
	{
		UnityEngine.Debug.LogWarning(string.Concat("Google: queryInventoryFailedEvent: ", error));
		base.StartCoroutine(this.WaitAndQueryInventory());
	}

	private void queryInventorySucceededEvent(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
	{
		this._products.Clear();
		this._purchasesToConsume.Clear();
		this._cheatedPurchasesToConsume.Clear();
		try
		{
			if (!skus.Any<GoogleSkuInfo>((GoogleSkuInfo s) => s.productId == "skinsmaker"))
			{
				string[] array = (
					from sku in skus
					select sku.productId).ToArray<string>();
				string str = string.Join(", ", array);
				string[] strArrays = (
					from p in purchases
					select string.Format("<{0}, {1}>", p.productId, p.purchaseState)).ToArray<string>();
				string str1 = string.Join(", ", strArrays);
				UnityEngine.Debug.Log(string.Format("Google billing. Query inventory succeeded, purchases: [{0}], skus: [{1}]", str1, str));
				IEnumerable<GoogleMarketProduct> googleMarketProducts = (
					from s in skus
					where array.Contains<string>(s.productId)
					select s).Select<GoogleSkuInfo, GoogleMarketProduct>(new Func<GoogleSkuInfo, GoogleMarketProduct>(MarketProductFactory.CreateGoogleMarketProduct));
				IEnumerator<GoogleMarketProduct> enumerator = googleMarketProducts.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						GoogleMarketProduct current = enumerator.Current;
						if (current.Price.Contains("$0.0"))
						{
							UnityEngine.Debug.LogWarningFormat("Unexpected price '{0}': '{1}' ('{2}')", new object[] { current.Price, current.Id, current.Title });
							coinsShop.HasTamperedProducts = true;
						}
						if (this._products.Contains(current))
						{
							continue;
						}
						this._products.Add(current);
					}
				}
				finally
				{
					if (enumerator == null)
					{
					}
					enumerator.Dispose();
				}
				foreach (GooglePurchase purchase in purchases)
				{
					if (purchase.productId == "MinerWeapon" || purchase.productId == "MinerWeapon".ToLower())
					{
						GameObject gameObject = GameObject.FindGameObjectWithTag("WeaponManager");
						if (gameObject)
						{
							gameObject.SendMessage("AddMinerWeaponToInventoryAndSaveInApp");
						}
					}
					else if (purchase.productId == "crystalsword")
					{
						GameObject gameObject1 = GameObject.FindGameObjectWithTag("WeaponManager");
						if (gameObject1)
						{
							gameObject1.SendMessage("AddSwordToInventoryAndSaveInApp");
						}
					}
					else if (StoreKitEventListener.starterPackIds.Contains<string>(purchase.productId))
					{
						StarterPackController.Get.AddBuyAndroidStarterPack(purchase.productId);
						StarterPackController.Get.TryRestoreStarterPack(purchase.productId);
					}
					else if (!this.VerifyPurchase(purchase.originalJson, purchase.signature))
					{
						this._cheatedPurchasesToConsume.Add(purchase);
					}
					else
					{
						this._purchasesToConsume.Add(purchase);
					}
				}
				this.AddCurrencyAndConsumeNextGooglePlayPurchase();
			}
		}
		finally
		{
			StoreKitEventListener.purchaseInProcess = false;
			StoreKitEventListener.restoreInProcess = false;
		}
	}

	public static void RefreshProducts()
	{
		if (!StoreKitEventListener.billingSupported && Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			return;
		}
		IEnumerable<string> strs = StoreKitEventListener._productIds.Concat<string>(StoreKitEventListener.coinIds).Concat<string>(StoreKitEventListener.gemsIds).Concat<string>(StoreKitEventListener.starterPackIds);
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			GoogleIAB.queryInventory(strs.ToArray<string>());
		}
		else
		{
			SkusInput skusInput = new SkusInput()
			{
				Skus = strs.ToList<string>()
			};
			SkusInput skusInput1 = skusInput;
			UnityEngine.Debug.Log(string.Concat("Amazon GetProductData (RefreshProducts): ", skusInput1.ToJson()));
			AmazonIapV2Impl.Instance.GetProductData(skusInput1);
		}
	}

	private void SendFirstTimePayment(GooglePurchase purchase)
	{
		if (purchase.purchaseState != GooglePurchase.GooglePurchaseState.Purchased)
		{
			return;
		}
		try
		{
			if (new Version(Switcher.InitialAppVersion) <= new Version(10, 3, 2, 891))
			{
				return;
			}
		}
		catch
		{
			return;
		}
		IEnumerable<GoogleSkuInfo> googleSkuInfos = (
			from p in this.Products
			select p.PlatformProduct).OfType<GoogleSkuInfo>();
		string str = purchase.productId;
		GoogleSkuInfo googleSkuInfo = googleSkuInfos.FirstOrDefault<GoogleSkuInfo>(new Func<GoogleSkuInfo, bool>(str.Equals));
		if (googleSkuInfo == null)
		{
			return;
		}
		decimal num = decimal.Divide(googleSkuInfo.priceAmountMicros, new decimal(1000000));
		AnalyticsFacade.SendFirstTimeRealPayment(purchase.orderId, (float)num, AnalyticsStuff.ReadableNameForInApp(googleSkuInfo.productId), googleSkuInfo.priceCurrencyCode);
	}

	internal static void SetLastPaymentTime()
	{
		string str = DateTime.UtcNow.ToString("s");
		PlayerPrefs.SetString("Last Payment Time", str);
		Storager.setInt("PayingUser", 1, true);
		PlayerPrefs.SetString("Last Payment Time (Advertisement)", str);
	}

	public static bool ShouldDelayCompletingTransactions()
	{
		bool flag;
		try
		{
			flag = (Time.realtimeSinceStartup - PromoActionsManager.startupTime >= 45f ? false : (!PromoActionsManager.x3InfoDownloadaedOnceDuringCurrentRun || !ChestBonusController.chestBonusesObtainedOnceInCurrentRun ? 0 : (int)coinsShop.IsStoreAvailable) == 0);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in ShouldDelayCompletingTransactions: ", exception));
			flag = false;
		}
		return flag;
	}

	private void Start()
	{
		Dictionary<string, object> strs;
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			if (!Application.isEditor || this._products.Any<IMarketProduct>())
			{
				List<string> list = StoreKitEventListener.coinIds.Concat<string>(StoreKitEventListener.gemsIds).ToList<string>();
				SkusInput skusInput = new SkusInput()
				{
					Skus = list
				};
				UnityEngine.Debug.Log(string.Concat("Amazon GetProductData (StoreKitEventListener.Start): ", skusInput.ToJson()));
				AmazonIapV2Impl.Instance.GetProductData(skusInput);
			}
			else
			{
				strs = new Dictionary<string, object>()
				{
					{ "description", "Test coin product for editor in Amazon edition" },
					{ "productType", "Not defined" },
					{ "price", "33\u00a0руб." },
					{ "sku", "coin1" },
					{ "smallIconUrl", "http://example.com" },
					{ "title", "Small pack of coins" }
				};
				this._products.Add(new AmazonMarketProduct(ProductData.CreateFromDictionary(strs)));
				strs = new Dictionary<string, object>()
				{
					{ "description", "Test gem product for editor in Amazon edition" },
					{ "productType", "Not defined" },
					{ "price", "99\u00a0руб." },
					{ "sku", "gem7" },
					{ "smallIconUrl", "http://example.com" },
					{ "title", "Small pack of gems" }
				};
				this._products.Add(new AmazonMarketProduct(ProductData.CreateFromDictionary(strs)));
				strs = new Dictionary<string, object>()
				{
					{ "description", "Test starter pack product for editor in Amazon edition" },
					{ "productType", "Not defined" },
					{ "price", "33 руб." },
					{ "sku", StoreKitEventListener.starterPack1 },
					{ "smallIconUrl", "http://example.com" },
					{ "title", "First starter pack(amazon)" }
				};
				this._products.Add(new AmazonMarketProduct(ProductData.CreateFromDictionary(strs)));
			}
		}
		else if (!Application.isEditor)
		{
			GoogleIAB.init((Defs.AndroidEdition != Defs.RuntimeAndroidEdition.GoogleLite ? string.Empty : "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAoTzMTaqsFhaywvCFKawFwL5KM+djLJfOCT/rbGQRfHmHYmOY2sBMgDWsA/67Szx6EVTZPVlFzHMgkAq1TwdL/A5aYGpGzaCX7o96cyp8R6wSF+xCuj++LAkTaDnLW0veI2bke3EVHu3At9xgM46e+VDucRUqQLvf6SQRb15nuflY5i08xKnewgX7I4U2H0RvAZDyoip+qZPmI4ZvaufAfc0jwZbw7XGiV41zibY3LU0N57mYKk51Wx+tOaJ7Tkc9Rl1qVCTjb+bwXshTqhVXVP6r4kabLWw/8OJUh0Sm69lbps6amP7vPy571XjscCTMLfXQan1959rHbNgkb2mLLQIDAQAB"));
			GoogleIAB.setAutoVerifySignatures(false);
			if (Defs.IsDeveloperBuild)
			{
				GoogleIAB.enableLogging(true);
			}
		}
		else
		{
			strs = new Dictionary<string, object>()
			{
				{ "description", "Test coin product for editor in Google edition" },
				{ "type", "Not defined" },
				{ "price", "99\u00a0руб." },
				{ "productId", "coin7" },
				{ "title", "Average pack of coins" }
			};
			this._products.Add(new GoogleMarketProduct(new GoogleSkuInfo(strs)));
			strs = new Dictionary<string, object>()
			{
				{ "description", "Test gem product for editor in Google edition" },
				{ "type", "Not defined" },
				{ "price", "33\u00a0руб." },
				{ "productId", "gem1" },
				{ "title", "Average pack of gems" }
			};
			this._products.Add(new GoogleMarketProduct(new GoogleSkuInfo(strs)));
			strs = new Dictionary<string, object>()
			{
				{ "description", "Test starter pack product for editor in Google edition" },
				{ "type", "Not defined" },
				{ "price", "33 руб." },
				{ "productId", StoreKitEventListener.starterPack1 },
				{ "title", "First starter pack(android)" }
			};
			this._products.Add(new GoogleMarketProduct(new GoogleSkuInfo(strs)));
		}
	}

	private bool TryAddStarterPackItem(string productId)
	{
		if (!StoreKitEventListener.starterPackIds.Contains<string>(productId))
		{
			return false;
		}
		bool flag = StarterPackController.Get.TryTakePurchasesForCurrentPack(productId, false);
		if (flag)
		{
			FlurryEvents.PaymentTime = new float?(Time.realtimeSinceStartup);
			StoreKitEventListener.CheckIfFirstTimePayment();
			StoreKitEventListener.SetLastPaymentTime();
		}
		FriendsController.sharedController.SendOurData(false);
		return flag;
	}

	private bool TryAddVirtualCrrency(string productId)
	{
		if (string.IsNullOrEmpty(productId))
		{
			UnityEngine.Debug.LogError("TryAddVirtualCrrency string.IsNullOrEmpty(productId)");
			return false;
		}
		int? nullable = null;
		int num = Array.IndexOf<string>(StoreKitEventListener.coinIds, productId);
		int num1 = Array.IndexOf<string>(StoreKitEventListener.gemsIds, productId);
		if (num >= StoreKitEventListener.coinIds.GetLowerBound(0))
		{
			nullable = new int?(Mathf.RoundToInt((float)VirtualCurrencyHelper.GetCoinInappsQuantity(num) * PremiumAccountController.VirtualCurrencyMultiplier));
			int num2 = Storager.getInt("Coins", false) + nullable.Value;
			Storager.setInt("Coins", num2, false);
			AnalyticsFacade.CurrencyAccrual(nullable.Value, "Coins", AnalyticsConstants.AccrualType.Purchased);
			coinsShop.TryToFireCurrenciesAddEvent("Coins");
			try
			{
				ChestBonusController.TryTakeChestBonus(false, num);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogError(string.Concat("TryAddVirtualCrrency ChestBonusController.TryTakeChestBonus exception: ", exception));
			}
		}
		else if (num1 >= StoreKitEventListener.gemsIds.GetLowerBound(0))
		{
			nullable = new int?(Mathf.RoundToInt((float)VirtualCurrencyHelper.GetGemsInappsQuantity(num1) * PremiumAccountController.VirtualCurrencyMultiplier));
			int num3 = Storager.getInt("GemsCurrency", false) + nullable.Value;
			Storager.setInt("GemsCurrency", num3, false);
			AnalyticsFacade.CurrencyAccrual(nullable.Value, "GemsCurrency", AnalyticsConstants.AccrualType.Purchased);
			coinsShop.TryToFireCurrenciesAddEvent("GemsCurrency");
			try
			{
				ChestBonusController.TryTakeChestBonus(true, num1);
			}
			catch (Exception exception1)
			{
				UnityEngine.Debug.LogError(string.Concat("TryAddVirtualCrrency ChestBonusController.TryTakeChestBonus exception: ", exception1));
			}
		}
		if (nullable.HasValue)
		{
			try
			{
				FlurryEvents.PaymentTime = new float?(Time.realtimeSinceStartup);
				StoreKitEventListener.LogVirtualCurrencyPurchased(productId, nullable.Value, num1 >= StoreKitEventListener.gemsIds.GetLowerBound(0));
				StoreKitEventListener.CheckIfFirstTimePayment();
				StoreKitEventListener.SetLastPaymentTime();
			}
			catch (Exception exception3)
			{
				Exception exception2 = exception3;
				UnityEngine.Debug.LogWarningFormat("TryAddVirtualCrrency ANALYTICS, LogVirtualCurrencyPurchased({0}, {1}) threw exception: {2}", new object[] { productId, nullable.Value, exception2 });
			}
		}
		try
		{
			if (FriendsController.sharedController != null)
			{
				FriendsController.sharedController.SendOurData(false);
			}
		}
		catch (Exception exception4)
		{
			UnityEngine.Debug.LogWarning(string.Concat("FriendsController.sharedController.SendOurData ", exception4));
		}
		return nullable.HasValue;
	}

	private bool VerifyPurchase(string purchaseJson, string base64Signature)
	{
		bool flag;
		try
		{
			byte[] numArray = Convert.FromBase64String(base64Signature);
			byte[] bytes = Encoding.UTF8.GetBytes(purchaseJson);
			flag = this._rsa.Value.VerifyData(bytes, this._sha1.Value, numArray);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
			return false;
		}
		return flag;
	}

	[DebuggerHidden]
	private IEnumerator WaitAndQueryInventory()
	{
		return new StoreKitEventListener.u003cWaitAndQueryInventoryu003ec__Iterator193();
	}

	[DebuggerHidden]
	private static IEnumerator WaitForFyberAndSetIsPaying()
	{
		return new StoreKitEventListener.u003cWaitForFyberAndSetIsPayingu003ec__Iterator196();
	}

	[DebuggerHidden]
	private IEnumerator WaitForX3AndGiveCurrency(GooglePurchase purchase, PurchaseReceipt receipt)
	{
		StoreKitEventListener.u003cWaitForX3AndGiveCurrencyu003ec__Iterator195 variable = null;
		return variable;
	}

	private enum ContentType
	{
		Unknown,
		Coins,
		Gems,
		StarterPack
	}

	internal sealed class StoreKitEventListenerState
	{
		public string Mode
		{
			get;
			set;
		}

		public IDictionary<string, string> Parameters
		{
			get;
			private set;
		}

		public string PurchaseKey
		{
			get;
			set;
		}

		public StoreKitEventListenerState()
		{
			this.Mode = string.Empty;
			this.PurchaseKey = string.Empty;
			this.Parameters = new Dictionary<string, string>();
		}
	}
}