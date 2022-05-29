using com.amazon.device.iap.cpt;
using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class StarterPackController : MonoBehaviour
{
	private DateTime _timeStartEvent;

	private TimeSpan _timeLiveEvent;

	private TimeSpan _timeToEndEvent;

	private List<StarterPackData> _starterPacksData;

	private int _orderCurrentPack;

	private bool _isDownloadDataRun;

	private StoreKitEventListener _storeKitEventListener;

	private float _lastCheckEventTime;

	private float timeUpdateConfig = -3600f;

	private List<string> BuyAnroidStarterPack
	{
		get;
		set;
	}

	public static StarterPackController Get
	{
		get;
		private set;
	}

	public bool isEventActive
	{
		get;
		private set;
	}

	public StarterPackController()
	{
	}

	public void AddBuyAndroidStarterPack(string starterPackId)
	{
		this.AddBuyingStarterPack(this.BuyAnroidStarterPack, starterPackId);
	}

	public void AddBuyingStarterPack(List<string> buyingList, string starterPackId)
	{
		if (buyingList.Contains(starterPackId))
		{
			return;
		}
		buyingList.Add(starterPackId);
	}

	private void CancelCurrentEvent()
	{
		Storager.setString("StartTimeShowStarterPack", string.Empty, false);
		Storager.setString("TimeEndStarterPack", string.Empty, false);
		Storager.setInt("NextNumberStarterPack", 0, false);
		PlayerPrefs.SetString("LastTimeShowStarterPack", string.Empty);
		PlayerPrefs.SetInt("CountShownStarterPack", 1);
		PlayerPrefs.Save();
		this.isEventActive = false;
		this.CheckSendEventChangeEnabled();
	}

	public void CheckBuyPackForGameMoney(StarterPackView view)
	{
		ItemPrice priceDataForItemsPack = this.GetPriceDataForItemsPack();
		if (priceDataForItemsPack == null)
		{
			return;
		}
		ShopNGUIController.TryToBuy(view.gameObject, priceDataForItemsPack, () => {
			string storageIdByPackOrder = this.GetStorageIdByPackOrder(this._orderCurrentPack);
			this.TryTakePurchasesForCurrentPack(storageIdByPackOrder, false);
			view.HideWindow();
		}, null, null, null, null, null);
	}

	public void CheckBuyRealMoney()
	{
		if (this._orderCurrentPack >= (int)StoreKitEventListener.starterPackIds.Length)
		{
			UnityEngine.Debug.Log(string.Concat("Not purchase data for starter pack number: ", this._orderCurrentPack));
			return;
		}
		ButtonClickSound.Instance.PlayClick();
		StoreKitEventListener.purchaseInProcess = true;
		string str = StoreKitEventListener.starterPackIds[this._orderCurrentPack];
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			FlurryPluginWrapper.LogEventToAppsFlyer("af_initiated_checkout", new Dictionary<string, string>()
			{
				{ "af_content_id", str }
			});
			GoogleIAB.purchaseProduct(str);
		}
		else
		{
			SkuInput skuInput = new SkuInput()
			{
				Sku = str
			};
			UnityEngine.Debug.Log(string.Concat("Amazon Purchase (StarterPackController.CheckBuyMoney): ", skuInput.ToJson()));
			AmazonIapV2Impl.Instance.Purchase(skuInput);
		}
		MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
	}

	private void CheckCancelCurrentStarterPack()
	{
		this.ResetToDefaultStateIfNeed();
		if (this.isEventActive)
		{
			this.isEventActive = false;
			this.CheckSendEventChangeEnabled();
		}
	}

	public void CheckFindStoreKitEventListner()
	{
		if (this._storeKitEventListener != null)
		{
			return;
		}
		this._storeKitEventListener = UnityEngine.Object.FindObjectOfType<StoreKitEventListener>();
	}

	public void CheckSendEventChangeEnabled()
	{
		if (StarterPackController.OnStarterPackEnable == null)
		{
			return;
		}
		StarterPackController.OnStarterPackEnable(this.isEventActive);
	}

	public void CheckShowStarterPack()
	{
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			UnityEngine.Debug.Log("Skipping CheckShowStarterPack() on WSA.");
			return;
		}
		base.StartCoroutine(this.CheckStartStarterPackEvent());
	}

	[DebuggerHidden]
	private IEnumerator CheckStartStarterPackEvent()
	{
		StarterPackController.u003cCheckStartStarterPackEventu003ec__Iterator191 variable = null;
		return variable;
	}

	public void ClearAllGooglePurchases()
	{
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.GoogleLite)
		{
			return;
		}
		if (this.BuyAnroidStarterPack.Count == 0)
		{
			return;
		}
		GoogleIAB.consumeProducts(this.BuyAnroidStarterPack.ToArray());
	}

	private void Destroy()
	{
		StarterPackController.Get = null;
	}

	[DebuggerHidden]
	private IEnumerator DownloadDataAboutEvent()
	{
		StarterPackController.u003cDownloadDataAboutEventu003ec__Iterator190 variable = null;
		return variable;
	}

	private void FinishCurrentStarterPack()
	{
		Storager.setString("StartTimeShowStarterPack", string.Empty, false);
		DateTime utcNow = DateTime.UtcNow;
		Storager.setString("TimeEndStarterPack", utcNow.ToString("s"), false);
		Storager.setInt("NextNumberStarterPack", this._orderCurrentPack + 1, false);
		this.isEventActive = false;
		this.CheckSendEventChangeEnabled();
	}

	public StarterPackData GetCurrentPackData()
	{
		if (this._orderCurrentPack == -1)
		{
			return null;
		}
		return this._starterPacksData[this._orderCurrentPack];
	}

	public Texture2D GetCurrentPackImage()
	{
		StarterPackModel.TypePack currentPackType = this.GetCurrentPackType();
		string empty = string.Empty;
		switch (currentPackType)
		{
			case StarterPackModel.TypePack.Items:
			{
				empty = "Textures/Bank/StarterPack_Weapon";
				break;
			}
			case StarterPackModel.TypePack.Coins:
			{
				empty = "Textures/Bank/Coins_Shop_5";
				break;
			}
			case StarterPackModel.TypePack.Gems:
			{
				empty = "Textures/Bank/Coins_Shop_Gem_5";
				break;
			}
		}
		if (string.IsNullOrEmpty(empty))
		{
			return null;
		}
		return Resources.Load<Texture2D>(empty);
	}

	public string GetCurrentPackName()
	{
		if (this._orderCurrentPack == -1)
		{
			return string.Empty;
		}
		if (this._orderCurrentPack >= (int)StarterPackModel.packNameLocalizeKey.Length)
		{
			return string.Empty;
		}
		return LocalizationStore.Get(StarterPackModel.packNameLocalizeKey[this._orderCurrentPack]);
	}

	public StarterPackModel.TypePack GetCurrentPackType()
	{
		return this.GetPackType(this._orderCurrentPack);
	}

	private int GetMaxValidOrderPack()
	{
		int num = -1;
		int currentLevel = ExperienceController.GetCurrentLevel();
		for (int i = 0; i < this._starterPacksData.Count; i++)
		{
			if (this._starterPacksData[i].blockLevel <= currentLevel)
			{
				num = i;
			}
		}
		return num;
	}

	private int GetOrderCurrentPack()
	{
		int num = Storager.getInt("NextNumberStarterPack", false);
		if (num >= this._starterPacksData.Count)
		{
			return -1;
		}
		return num;
	}

	private int GetOrderPackByProductId(string productId)
	{
		if (!StoreKitEventListener.starterPackIds.Contains<string>(productId))
		{
			return -1;
		}
		return Array.IndexOf<string>(StoreKitEventListener.starterPackIds, productId);
	}

	public StarterPackModel.TypePack GetPackType(int packOrder)
	{
		if (packOrder == -1)
		{
			return StarterPackModel.TypePack.None;
		}
		if (this._starterPacksData[packOrder].items != null)
		{
			return StarterPackModel.TypePack.Items;
		}
		if (this._starterPacksData[packOrder].coinsCount > 0)
		{
			return StarterPackModel.TypePack.Coins;
		}
		if (this._starterPacksData[packOrder].gemsCount > 0)
		{
			return StarterPackModel.TypePack.Gems;
		}
		return StarterPackModel.TypePack.None;
	}

	public ItemPrice GetPriceDataForItemsPack()
	{
		StarterPackData currentPackData = this.GetCurrentPackData();
		if (currentPackData == null)
		{
			return null;
		}
		if (currentPackData.coinsCost <= 0 && currentPackData.gemsCost <= 0)
		{
			return null;
		}
		string empty = string.Empty;
		int num = 0;
		if (currentPackData.coinsCost > 0)
		{
			empty = "Coins";
			num = currentPackData.coinsCost;
		}
		else if (currentPackData.gemsCost > 0)
		{
			empty = "GemsCurrency";
			num = currentPackData.gemsCost;
		}
		return new ItemPrice(num, empty);
	}

	public string GetPriceLabelForCurrentPack()
	{
		if (this._orderCurrentPack >= (int)StoreKitEventListener.starterPackIds.Length)
		{
			return string.Empty;
		}
		if (Application.isEditor)
		{
			return string.Format("{0}$", VirtualCurrencyHelper.starterPackFakePrice[this._orderCurrentPack]);
		}
		string str = StoreKitEventListener.starterPackIds[this._orderCurrentPack];
		IMarketProduct marketProduct = this._storeKitEventListener.Products.FirstOrDefault<IMarketProduct>((IMarketProduct p) => p.Id == str);
		if (marketProduct != null)
		{
			return marketProduct.Price;
		}
		UnityEngine.Debug.LogWarning(string.Concat("marketProduct == null,    id: ", str));
		return string.Empty;
	}

	public string GetSavingMoneyByCarrentPack()
	{
		int orderCurrentPack = this.GetOrderCurrentPack();
		if (orderCurrentPack == -1)
		{
			return string.Empty;
		}
		if (orderCurrentPack >= (int)StarterPackModel.savingMoneyForBuyPack.Length)
		{
			return string.Empty;
		}
		if (this.IsPackSellForGameMoney())
		{
			return string.Empty;
		}
		return string.Format("{0} {1}$", LocalizationStore.Get("Key_1047"), StarterPackModel.savingMoneyForBuyPack[orderCurrentPack]);
	}

	private string GetStorageIdByPackOrder(int packOrder)
	{
		if (packOrder < 0 || packOrder >= (int)StoreKitEventListener.starterPackIds.Length)
		{
			return string.Empty;
		}
		return StoreKitEventListener.starterPackIds[packOrder];
	}

	public string GetTimeToEndEvent()
	{
		if (!this.isEventActive)
		{
			return string.Empty;
		}
		this._timeToEndEvent = StarterPackModel.MaxLiveTimeEvent - this._timeLiveEvent;
		return string.Format("{0:00}:{1:00}:{2:00}", this._timeToEndEvent.Hours, this._timeToEndEvent.Minutes, this._timeToEndEvent.Seconds);
	}

	private StarterPackModel.TypeCost GetTypeCostPack()
	{
		StarterPackData currentPackData = this.GetCurrentPackData();
		if (currentPackData == null)
		{
			return StarterPackModel.TypeCost.None;
		}
		if (currentPackData.coinsCost > 0)
		{
			return StarterPackModel.TypeCost.Money;
		}
		if (currentPackData.gemsCost > 0)
		{
			return StarterPackModel.TypeCost.Gems;
		}
		return StarterPackModel.TypeCost.InApp;
	}

	private void InitializeEvent()
	{
		this._timeStartEvent = DateTime.UtcNow;
		Storager.setString("StartTimeShowStarterPack", this._timeStartEvent.ToString("s"), false);
		Storager.setString("TimeEndStarterPack", string.Empty, false);
	}

	private bool IsCooldownEventEnd()
	{
		DateTime utcNow = DateTime.UtcNow;
		DateTime timeDataEvent = StarterPackModel.GetTimeDataEvent("TimeEndStarterPack");
		return (utcNow - timeDataEvent) >= StarterPackModel.CooldownTimeEvent;
	}

	private bool IsCurrentPackEnable()
	{
		StarterPackData currentPackData = this.GetCurrentPackData();
		if (currentPackData == null)
		{
			return true;
		}
		return currentPackData.enable;
	}

	private bool IsEventInEndState()
	{
		string str = Storager.getString("StartTimeShowStarterPack", false);
		string str1 = Storager.getString("TimeEndStarterPack", false);
		return (str != string.Empty ? false : !string.IsNullOrEmpty(str1));
	}

	private bool IsInvalidCurrentPack()
	{
		return this.IsInvalidPack(this._orderCurrentPack);
	}

	private bool IsInvalidPack(int packOrder)
	{
		if (this.GetPackType(packOrder) != StarterPackModel.TypePack.Items)
		{
			return false;
		}
		List<StarterPackItemData> item = this._starterPacksData[packOrder].items;
		for (int i = 0; i < item.Count; i++)
		{
			if (string.IsNullOrEmpty(item[i].validTag))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsNeedShowEventWindow()
	{
		int num = PlayerPrefs.GetInt("CountShownStarterPack", 1);
		return (!this.isEventActive || num <= 0 ? false : SceneLoader.ActiveSceneName.Equals(Defs.MainMenuScene));
	}

	public bool IsPackSellForGameMoney()
	{
		StarterPackModel.TypeCost typeCostPack = this.GetTypeCostPack();
		return (typeCostPack == StarterPackModel.TypeCost.Gems ? true : typeCostPack == StarterPackModel.TypeCost.Money);
	}

	private bool IsPlayerNotPayBeforeStartEvent()
	{
		if (Storager.getInt("PayingUser", true) == 0)
		{
			return true;
		}
		if (this.isEventActive)
		{
			return true;
		}
		return false;
	}

	private bool IsStarterPackBuy(string storageId)
	{
		return (!Storager.hasKey(storageId) ? false : Storager.getInt(storageId, false) == 1);
	}

	private bool IsStarterPackBuyByPackOrder(int packOrder)
	{
		string storageIdByPackOrder = this.GetStorageIdByPackOrder(packOrder);
		if (string.IsNullOrEmpty(storageIdByPackOrder))
		{
			return false;
		}
		return this.IsStarterPackBuy(storageIdByPackOrder);
	}

	private bool IsStarterPackBuying(List<string> buyingList, int orderPack)
	{
		string storageIdByPackOrder = this.GetStorageIdByPackOrder(orderPack);
		if (string.IsNullOrEmpty(storageIdByPackOrder))
		{
			return false;
		}
		return buyingList.Contains(storageIdByPackOrder);
	}

	private bool IsStarterPackBuyOnAndroid(int orderPack)
	{
		return this.IsStarterPackBuying(this.BuyAnroidStarterPack, orderPack);
	}

	private void ResetToDefaultStateIfNeed()
	{
		if (Storager.hasKey("StartTimeShowStarterPack") && !string.IsNullOrEmpty(Storager.getString("StartTimeShowStarterPack", false)))
		{
			Storager.setString("StartTimeShowStarterPack", string.Empty, false);
		}
		if (Storager.hasKey("TimeEndStarterPack") && !string.IsNullOrEmpty(Storager.getString("TimeEndStarterPack", false)))
		{
			Storager.setString("TimeEndStarterPack", string.Empty, false);
		}
		if (Storager.getInt("NextNumberStarterPack", false) > 0)
		{
			Storager.setInt("NextNumberStarterPack", 0, false);
		}
		if (!string.IsNullOrEmpty(PlayerPrefs.GetString("LastTimeShowStarterPack", string.Empty)))
		{
			PlayerPrefs.SetString("LastTimeShowStarterPack", string.Empty);
		}
		if (PlayerPrefs.GetInt("CountShownStarterPack", 1) != 1)
		{
			PlayerPrefs.SetInt("CountShownStarterPack", 1);
		}
	}

	public void RestoreBuyingStarterPack(List<string> buyingList)
	{
		for (int i = 0; i < buyingList.Count; i++)
		{
			this.TryRestoreStarterPack(buyingList[i]);
		}
	}

	public void RestoreStarterPackForAmazon()
	{
		this.RestoreBuyingStarterPack(this.BuyAnroidStarterPack);
	}

	private void Start()
	{
		StarterPackController.Get = this;
		this._timeLiveEvent = new TimeSpan();
		this._starterPacksData = new List<StarterPackData>();
		this._orderCurrentPack = -1;
		this._storeKitEventListener = UnityEngine.Object.FindObjectOfType<StoreKitEventListener>();
		this.BuyAnroidStarterPack = new List<string>();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void TryRestoreStarterPack(string productId)
	{
		base.StartCoroutine(this.TryRestoreStarterPackByProductId(productId));
	}

	[DebuggerHidden]
	private IEnumerator TryRestoreStarterPackByProductId(string productId)
	{
		StarterPackController.u003cTryRestoreStarterPackByProductIdu003ec__Iterator192 variable = null;
		return variable;
	}

	private bool TryTakePurchases(string productId, int packOrder, bool isRestore = false)
	{
		if (this._starterPacksData.Count == 0)
		{
			return false;
		}
		if (packOrder == -1)
		{
			return false;
		}
		StarterPackModel.TypePack packType = this.GetPackType(packOrder);
		if (packType == StarterPackModel.TypePack.None)
		{
			return false;
		}
		if (this.IsStarterPackBuy(productId))
		{
			return false;
		}
		StarterPackData item = this._starterPacksData[packOrder];
		switch (packType)
		{
			case StarterPackModel.TypePack.Items:
			{
				if (item.items.Count != 0)
				{
					for (int i = 0; i < item.items.Count; i++)
					{
						string str = item.items[i].validTag;
						int itemCategory = ItemDb.GetItemCategory(str);
						int num = item.items[i].count;
						if (itemCategory == 7 || ShopNGUIController.IsWeaponCategory((ShopNGUIController.CategoryNames)itemCategory))
						{
							ShopNGUIController.FireWeaponOrArmorBought();
						}
						ShopNGUIController.ProvideShopItemOnStarterPackBoguht((ShopNGUIController.CategoryNames)itemCategory, str, num, false, 0, null, null, true, true, true);
					}
					if (ShopNGUIController.sharedShop != null && ShopNGUIController.sharedShop.wearEquipAction != null)
					{
						ShopNGUIController.sharedShop.wearEquipAction(7, string.Empty, string.Empty);
					}
					if (ShopNGUIController.sharedShop != null && ShopNGUIController.GuiActive)
					{
						ShopNGUIController.sharedShop.CategoryChosen(ShopNGUIController.sharedShop.currentCategory, ShopNGUIController.sharedShop.viewedId, false);
					}
					break;
				}
				else
				{
					break;
				}
			}
			case StarterPackModel.TypePack.Coins:
			{
				BankController.AddCoins(item.coinsCount, true, AnalyticsConstants.AccrualType.Purchased);
				StoreKitEventListener.LogVirtualCurrencyPurchased(productId, item.coinsCount, false);
				break;
			}
			case StarterPackModel.TypePack.Gems:
			{
				BankController.AddGems(item.gemsCount, true, AnalyticsConstants.AccrualType.Purchased);
				StoreKitEventListener.LogVirtualCurrencyPurchased(productId, item.gemsCount, true);
				break;
			}
		}
		Storager.setInt(productId, 1, false);
		this.FinishCurrentStarterPack();
		int currentLevel = ExperienceController.GetCurrentLevel();
		int num1 = (ExpController.Instance != null ? ExpController.Instance.OurTier : 0);
		string str1 = (!isRestore ? "SKU" : "RESTORE");
		string empty = string.Empty;
		empty = (isRestore || !this.IsPackSellForGameMoney() ? "StarterPacks Purchases" : "StarterPacks Purchases(gems)");
		Dictionary<string, string> strs = new Dictionary<string, string>()
		{
			{ "Levels", currentLevel.ToString() },
			{ "Tiers", num1.ToString() },
			{ str1, productId }
		};
		FlurryPluginWrapper.LogEventAndDublicateToConsole(empty, strs, true);
		if (!isRestore && !this.IsPackSellForGameMoney() && packType == StarterPackModel.TypePack.Items)
		{
			FlurryEvents.LogBecomePaying(productId);
		}
		if (!isRestore)
		{
			AnalyticsStuff.LogSales((!StoreKitEventListener.inAppsReadableNames.ContainsKey(productId) ? productId : StoreKitEventListener.inAppsReadableNames[productId]), "Starter Pack", false);
		}
		return true;
	}

	public bool TryTakePurchasesForCurrentPack(string productId, bool isRestore = false)
	{
		return this.TryTakePurchases(productId, this._orderCurrentPack, isRestore);
	}

	private void Update()
	{
		if (!this.isEventActive)
		{
			return;
		}
		if (Time.realtimeSinceStartup - this._lastCheckEventTime >= 1f)
		{
			this._timeLiveEvent = DateTime.UtcNow - this._timeStartEvent;
			this.isEventActive = this._timeLiveEvent <= StarterPackModel.MaxLiveTimeEvent;
			if (!this.isEventActive)
			{
				this.FinishCurrentStarterPack();
			}
			this._lastCheckEventTime = Time.realtimeSinceStartup;
		}
	}

	public void UpdateCountShownWindowByShowCondition()
	{
		if (PlayerPrefs.GetInt("CountShownStarterPack", 1) == 0)
		{
			return;
		}
		PlayerPrefs.SetString("LastTimeShowStarterPack", DateTime.UtcNow.ToString("s"));
		int num = PlayerPrefs.GetInt("CountShownStarterPack", 1);
		PlayerPrefs.SetInt("CountShownStarterPack", num - 1);
		PlayerPrefs.Save();
	}

	public void UpdateCountShownWindowByTimeCondition()
	{
		string str = PlayerPrefs.GetString("LastTimeShowStarterPack", string.Empty);
		if (string.IsNullOrEmpty(str))
		{
			return;
		}
		DateTime dateTime = new DateTime();
		if (!DateTime.TryParse(str, out dateTime))
		{
			return;
		}
		if ((DateTime.UtcNow - dateTime) >= StarterPackModel.TimeOutShownWindow)
		{
			PlayerPrefs.SetInt("CountShownStarterPack", 1);
		}
	}

	public static event StarterPackController.OnStarterPackEnableDelegate OnStarterPackEnable;

	public delegate void OnStarterPackEnableDelegate(bool enable);
}