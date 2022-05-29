using com.amazon.device.iap.cpt;
using Rilisoft;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

internal sealed class coinsShop : MonoBehaviour
{
	public static coinsShop thisScript;

	public static bool showPlashkuPriExit;

	public Action onReturnAction;

	private bool productPurchased;

	private float _timeWhenPurchShown;

	private List<string> currenciesBought = new List<string>();

	private bool productsReceived;

	public Action onResumeFronNGUI;

	private bool itemBought;

	private readonly static HashSet<string> _loggedPackages;

	private static DateTime? _etcFileTimestamp;

	private Action _drawInnerInterface;

	internal static bool HasTamperedProducts
	{
		private get;
		set;
	}

	public static bool IsBillingSupported
	{
		get
		{
			if (!Application.isEditor)
			{
				return StoreKitEventListener.billingSupported;
			}
			return true;
		}
	}

	public static bool IsNoConnection
	{
		get
		{
			bool flag;
			if (coinsShop.thisScript == null)
			{
				return true;
			}
			if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
				flag = (!coinsShop.thisScript.productsReceived ? true : !coinsShop.IsBillingSupported);
			}
			else
			{
				flag = !coinsShop.thisScript.productsReceived;
			}
			return flag;
		}
	}

	public static bool IsStoreAvailable
	{
		get
		{
			return (coinsShop.IsWideLayoutAvailable ? false : !coinsShop.IsNoConnection);
		}
	}

	public static bool IsWideLayoutAvailable
	{
		get
		{
			return (coinsShop.CheckAndroidHostsTampering() || coinsShop.CheckLuckyPatcherInstalled() || FlurryPluginWrapper.IsLoggingFlurryAnalyticsSupported() ? true : coinsShop.HasTamperedProducts);
		}
	}

	public string notEnoughCurrency
	{
		get;
		set;
	}

	public bool ProductPurchasedRecently
	{
		get
		{
			return this.productPurchased;
		}
	}

	static coinsShop()
	{
		coinsShop.showPlashkuPriExit = false;
		coinsShop._loggedPackages = new HashSet<string>();
	}

	public coinsShop()
	{
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.notEnoughCurrency = null;
		if (Application.isEditor)
		{
			this.productsReceived = true;
		}
		coinsShop.thisScript = this;
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			GoogleIABManager.queryInventorySucceededEvent += new Action<List<GooglePurchase>, List<GoogleSkuInfo>>(this.HandleQueryInventorySucceededEvent);
		}
		else
		{
			AmazonIapV2Impl.Instance.AddGetProductDataResponseListener(new GetProductDataResponseDelegate(this.HandleItemDataRequestFinishedEvent));
		}
		this.RefreshProductsIfNeed(false);
	}

	internal static bool CheckAndroidHostsTampering()
	{
		bool flag;
		if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
		{
			return false;
		}
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.GoogleLite)
		{
			return false;
		}
		if (!File.Exists("/etc/hosts"))
		{
			return false;
		}
		try
		{
			IEnumerable<string> strs = 
				from l in File.ReadAllLines("/etc/hosts")
				where l.TrimStart(new char[0]).StartsWith("127.")
				select l;
			flag = strs.Any<string>((string l) => (l.Contains("android.clients.google.com") ? true : l.Contains("mtalk.google.com ")));
		}
		catch (Exception exception)
		{
			Debug.LogError(exception);
			flag = false;
		}
		return flag;
	}

	internal static bool CheckHostsTimestamp()
	{
		if (coinsShop._etcFileTimestamp.HasValue)
		{
			DateTime? hostsTimestamp = coinsShop.GetHostsTimestamp();
			if (hostsTimestamp.HasValue && coinsShop._etcFileTimestamp.Value != hostsTimestamp.Value)
			{
				Debug.LogError(string.Format("Timestamp check failed: {0:s} expcted, but actual value is {1:s}.", coinsShop._etcFileTimestamp.Value, hostsTimestamp.Value));
				return false;
			}
		}
		return true;
	}

	internal static bool CheckLuckyPatcherInstalled()
	{
		if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
		{
			return false;
		}
		string[] strArrays = new string[] { "Y29tLmRpbW9udmlkZW8ubHVja3lwYXRjaGVy", "Y29tLmNoZWxwdXMubGFja3lwYXRjaA==", "Y29tLmZvcnBkYS5scA==" };
		IEnumerable<string> strs = strArrays.Select<string, byte[]>(new Func<string, byte[]>(Convert.FromBase64String)).Where<byte[]>((byte[] bytes) => bytes != null).Select<byte[], string>((byte[] bytes) => Encoding.UTF8.GetString(bytes, 0, (int)bytes.Length));
		return strs.Any<string>(new Func<string, bool>(coinsShop.PackageExists));
	}

	public static void ExitFromShop(bool performOnExitActs)
	{
		coinsShop.hideCoinsShop();
		if (coinsShop.showPlashkuPriExit)
		{
			coinsPlashka.hidePlashka();
		}
		coinsPlashka.hideButtonCoins = false;
		if (!performOnExitActs)
		{
			return;
		}
		if (coinsShop.thisScript.onReturnAction == null || coinsShop.thisScript.notEnoughCurrency == null || !coinsShop.thisScript.currenciesBought.Contains(coinsShop.thisScript.notEnoughCurrency))
		{
			coinsShop.thisScript.onReturnAction = null;
		}
		else
		{
			coinsShop.thisScript.currenciesBought.Clear();
			coinsShop.thisScript.onReturnAction();
		}
		if (coinsShop.thisScript.onResumeFronNGUI != null)
		{
			coinsShop.thisScript.onResumeFronNGUI();
			coinsShop.thisScript.onResumeFronNGUI = null;
			coinsPlashka.hidePlashka();
		}
	}

	private static DateTime? GetHostsTimestamp()
	{
		DateTime? nullable;
		try
		{
			Debug.Log("Trying to get /ets/hosts timestamp...");
			DateTime lastWriteTimeUtc = (new FileInfo("/etc/hosts")).LastWriteTimeUtc;
			Debug.Log(string.Concat("/ets/hosts timestamp: ", lastWriteTimeUtc.ToString("s")));
			nullable = new DateTime?(lastWriteTimeUtc);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			nullable = null;
		}
		return nullable;
	}

	private void HandleItemDataRequestFinishedEvent(GetProductDataResponse response)
	{
		if (!"SUCCESSFUL".Equals(response.Status, StringComparison.OrdinalIgnoreCase))
		{
			Debug.LogWarning(string.Concat("Amazon GetProductDataResponse (CoinsShop): ", response.Status));
			return;
		}
		Debug.Log(string.Concat("Amazon GetProductDataResponse (CoinsShop): ", response.ToJson()));
		this.productsReceived = true;
	}

	public void HandlePurchaseButton(int i, string currency = "Coins")
	{
		string str;
		ButtonClickSound.Instance.PlayClick();
		if (currency.Equals("Coins") && (i >= (int)StoreKitEventListener.coinIds.Length || i >= (int)VirtualCurrencyHelper.coinInappsQuantity.Length) || currency.Equals("GemsCurrency") && (i >= (int)StoreKitEventListener.gemsIds.Length || i >= (int)VirtualCurrencyHelper.gemsInappsQuantity.Length))
		{
			Debug.LogWarning(string.Concat("Index of purchase is out of range: ", i));
			return;
		}
		this.currenciesBought.Add(currency);
		this.itemBought = true;
		StoreKitEventListener.purchaseInProcess = true;
		if (!"Coins".Equals(currency))
		{
			if (!"GemsCurrency".Equals(currency))
			{
				Debug.LogError(string.Concat("Unknown currency: ", currency));
				return;
			}
			str = StoreKitEventListener.gemsIds[i];
		}
		else
		{
			str = StoreKitEventListener.coinIds[i];
		}
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			coinsShop._etcFileTimestamp = coinsShop.GetHostsTimestamp();
			FlurryPluginWrapper.LogEventToAppsFlyer("af_initiated_checkout", new Dictionary<string, string>()
			{
				{ "af_content_id", str }
			});
			GoogleIAB.purchaseProduct(str);
			MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
		}
		else
		{
			SkuInput skuInput = new SkuInput()
			{
				Sku = str
			};
			Debug.Log(string.Concat("Amazon Purchase (HandlePurchaseButton): ", skuInput.ToJson()));
			AmazonIapV2Impl.Instance.Purchase(skuInput);
			MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
		}
	}

	private void HandlePurchaseSucceededEvent(GooglePurchase purchase)
	{
		this.HandlePurchaseSuccessfullCore();
	}

	private void HandlePurchaseSuccessfulEvent(PurchaseResponse response)
	{
		string str = string.Concat("Amazon PurchaseResponse (CoinsShop): ", response.Status);
		if (!"SUCCESSFUL".Equals(response.Status, StringComparison.OrdinalIgnoreCase))
		{
			Debug.LogWarning(str);
			return;
		}
		Debug.Log(str);
		this.HandlePurchaseSuccessfullCore();
	}

	private void HandlePurchaseSuccessfullCore()
	{
		try
		{
			if (this.itemBought)
			{
				this.itemBought = false;
				this.productPurchased = true;
				this._timeWhenPurchShown = Time.realtimeSinceStartup;
			}
		}
		catch (Exception exception)
		{
			Debug.LogError(exception);
		}
	}

	private void HandleQueryInventorySucceededEvent(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
	{
		if (skus.Any<GoogleSkuInfo>((GoogleSkuInfo s) => s.productId == "skinsmaker"))
		{
			return;
		}
		string[] array = (
			from sku in skus
			select sku.productId).ToArray<string>();
		string str = string.Join(", ", array);
		string str1 = string.Format("Google: Query inventory succeeded;\tPurchases count: {0}, Skus: [{1}]", purchases.Count, str);
		Debug.Log(str1);
		this.productsReceived = true;
	}

	public static void hideCoinsShop()
	{
		if (coinsShop.thisScript != null)
		{
			coinsShop.thisScript.enabled = false;
			coinsShop.thisScript.notEnoughCurrency = null;
			Resources.UnloadUnusedAssets();
		}
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			this.RefreshProductsIfNeed(false);
		}
	}

	private void OnDestroy()
	{
		coinsShop.thisScript = null;
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			GoogleIABManager.queryInventorySucceededEvent -= new Action<List<GooglePurchase>, List<GoogleSkuInfo>>(this.HandleQueryInventorySucceededEvent);
		}
		else
		{
			AmazonIapV2Impl.Instance.RemoveGetProductDataResponseListener(new GetProductDataResponseDelegate(this.HandleItemDataRequestFinishedEvent));
		}
	}

	private void OnDisable()
	{
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			GoogleIABManager.purchaseSucceededEvent -= new Action<GooglePurchase>(this.HandlePurchaseSucceededEvent);
		}
		else
		{
			AmazonIapV2Impl.Instance.RemovePurchaseResponseListener(new PurchaseResponseDelegate(this.HandlePurchaseSuccessfulEvent));
		}
		ActivityIndicator.IsActiveIndicator = false;
		this.itemBought = false;
		this.currenciesBought.Clear();
	}

	private void OnEnable()
	{
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			GoogleIABManager.purchaseSucceededEvent += new Action<GooglePurchase>(this.HandlePurchaseSucceededEvent);
		}
		else
		{
			AmazonIapV2Impl.Instance.AddPurchaseResponseListener(new PurchaseResponseDelegate(this.HandlePurchaseSuccessfulEvent));
		}
		if (Application.loadedLevelName != "Loading")
		{
			ActivityIndicator.IsActiveIndicator = false;
		}
		this.itemBought = false;
		this.currenciesBought.Clear();
	}

	private static bool PackageExists(string packageName)
	{
		bool flag;
		if (packageName == null)
		{
			throw new ArgumentNullException("packageName");
		}
		if (Application.isEditor)
		{
			return false;
		}
		try
		{
			AndroidJavaObject currentActivity = AndroidSystem.Instance.CurrentActivity;
			if (currentActivity != null)
			{
				AndroidJavaObject androidJavaObject = currentActivity.Call<AndroidJavaObject>("getPackageManager", new object[0]);
				if (androidJavaObject == null)
				{
					Debug.LogWarning("manager == null");
					flag = false;
				}
				else if (androidJavaObject.Call<AndroidJavaObject>("getPackageInfo", new object[] { packageName, 0 }) != null)
				{
					flag = true;
				}
				else
				{
					Debug.LogWarning("packageInfo == null");
					flag = false;
				}
			}
			else
			{
				Debug.LogWarning("activity == null");
				flag = false;
			}
		}
		catch (Exception exception1)
		{
			Exception exception = exception1;
			if (!coinsShop._loggedPackages.Contains(packageName))
			{
				string str = string.Format("Error while retrieving Android package info:    {0}", exception);
				if (Defs.IsDeveloperBuild)
				{
					Debug.LogWarning(str);
					coinsShop._loggedPackages.Add(packageName);
				}
				return false;
			}
			else
			{
				flag = false;
			}
		}
		return flag;
	}

	public void RefreshProductsIfNeed(bool force = false)
	{
		if (!this.productsReceived || force)
		{
			StoreKitEventListener.RefreshProducts();
		}
	}

	public static void showCoinsShop()
	{
		coinsShop.thisScript.enabled = true;
		coinsPlashka.hideButtonCoins = true;
		coinsPlashka.showPlashka();
	}

	public static void TryToFireCurrenciesAddEvent(string currency)
	{
		try
		{
			CoinsMessage.FireCoinsAddedEvent(currency == "GemsCurrency", 2);
		}
		catch (Exception exception)
		{
			Debug.LogError(string.Concat("coinsShop.TryToFireCurrenciesAddEvent: FireCoinsAddedEvent( currency == Defs.Gems ): ", exception));
		}
	}

	private void Update()
	{
		if (Time.realtimeSinceStartup - this._timeWhenPurchShown >= 1.25f)
		{
			this.productPurchased = false;
		}
	}
}