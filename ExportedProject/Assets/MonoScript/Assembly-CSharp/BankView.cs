using I2.Loc;
using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class BankView : MonoBehaviour, IDisposable
{
	public List<BankViewItem> coinItemsABStatic;

	public List<BankViewItem> gemItemsABStatic;

	public GameObject goldContainerABStatic;

	public GameObject gemsContainerABStatic;

	public ButtonHandler backButton;

	public GameObject premium;

	public GameObject premium5percent;

	public GameObject premium10percent;

	public GameObject interfaceHolder;

	public UILabel connectionProblemLabel;

	public UILabel crackersWarningLabel;

	public UILabel notEnoughCoinsLabel;

	public UILabel notEnoughGemsLabel;

	public UISprite purchaseSuccessfulLabel;

	public UILabel[] eventX3RemainTime;

	public GameObject btnTabContainer;

	public UIButton btnTabGold;

	public UIButton btnTabGems;

	public UIScrollView goldScrollView;

	public UIGrid goldItemGrid;

	public BankViewItem goldItemPrefab;

	public UIScrollView gemsScrollView;

	public UIGrid gemsItemGrid;

	public BankViewItem gemsItemPrefab;

	public UIButton freeAwardButton;

	public UIWidget eventX3AmazonBonusWidget;

	public UILabel amazonEventCaptionLabel;

	public UILabel amazonEventTitleLabel;

	private UILabel _freeAwardButtonLagelCont;

	private StoreKitEventListener _storeKitEventListener;

	private bool _needResetScrollView;

	private bool _isPurchasesAreadyEnabled;

	public TweenColor colorBlinkForX3;

	private float _lastUpdateTime;

	private string _localizeSaleLabel;

	private readonly List<Action> _disposeActions = new List<Action>();

	private bool _disposed;

	public static int[] discountsCoins;

	private static bool _isInitGoldPurchasesInfo_AB_Static;

	private static IList<PurchaseEventArgs> _goldPurchasesInfo_AB_Static;

	public static int[] discountsGems;

	private static bool _isInitGemsPurchasesInfo_AB_Static;

	private static IList<PurchaseEventArgs> _gemsPurchasesInfo_AB_Static;

	private EventHandler<PurchaseEventArgs> PurchaseButtonPressed;

	private UILabel _freeAwardButtonLabel
	{
		get
		{
			if (this._freeAwardButtonLagelCont != null)
			{
				return this._freeAwardButtonLagelCont;
			}
			if (this.freeAwardButton == null)
			{
				return this._freeAwardButtonLagelCont;
			}
			UILabel componentInChildren = this.freeAwardButton.GetComponentInChildren<UILabel>();
			UILabel uILabel = componentInChildren;
			this._freeAwardButtonLagelCont = componentInChildren;
			return uILabel;
		}
	}

	public bool ConnectionProblemLabelEnabled
	{
		get
		{
			return (this.connectionProblemLabel == null ? false : this.connectionProblemLabel.gameObject.GetActive());
		}
		set
		{
			if (this.connectionProblemLabel != null)
			{
				this.connectionProblemLabel.gameObject.SetActive(value);
			}
		}
	}

	public bool CrackersWarningLabelEnabled
	{
		get
		{
			return (this.crackersWarningLabel == null ? false : this.crackersWarningLabel.gameObject.GetActive());
		}
		set
		{
			if (this.crackersWarningLabel != null)
			{
				this.crackersWarningLabel.gameObject.SetActive(value);
			}
		}
	}

	public static IList<PurchaseEventArgs> gemsPurchasesInfo
	{
		get
		{
			List<PurchaseEventArgs> purchaseEventArgs = new List<PurchaseEventArgs>()
			{
				new PurchaseEventArgs(0, 0, new decimal(0), "GemsCurrency", BankView.discountsCoins[0]),
				new PurchaseEventArgs(1, 0, new decimal(0), "GemsCurrency", BankView.discountsCoins[1]),
				new PurchaseEventArgs(2, 0, new decimal(0), "GemsCurrency", BankView.discountsCoins[2]),
				new PurchaseEventArgs(3, 0, new decimal(0), "GemsCurrency", BankView.discountsCoins[3]),
				new PurchaseEventArgs(4, 0, new decimal(0), "GemsCurrency", BankView.discountsCoins[4]),
				new PurchaseEventArgs(5, 0, new decimal(0), "GemsCurrency", BankView.discountsCoins[5]),
				new PurchaseEventArgs(6, 0, new decimal(0), "GemsCurrency", BankView.discountsCoins[6])
			};
			return purchaseEventArgs;
		}
	}

	public static IList<PurchaseEventArgs> gemsPurchasesInfo_AB_Static
	{
		get
		{
			if (!BankView._isInitGemsPurchasesInfo_AB_Static)
			{
				BankView._gemsPurchasesInfo_AB_Static = BankView.gemsPurchasesInfo_AB_StaticDefault;
				FriendsController.ParseABTestBankViewConfig();
			}
			return BankView._gemsPurchasesInfo_AB_Static;
		}
		set
		{
			BankView._gemsPurchasesInfo_AB_Static = value;
			BankView._isInitGemsPurchasesInfo_AB_Static = true;
		}
	}

	public static IList<PurchaseEventArgs> gemsPurchasesInfo_AB_StaticDefault
	{
		get
		{
			return (
				from pi in BankView.gemsPurchasesInfo
				where pi.Index != 0
				select pi).ToList<PurchaseEventArgs>();
		}
	}

	public static IList<PurchaseEventArgs> goldPurchasesInfo
	{
		get
		{
			List<PurchaseEventArgs> purchaseEventArgs = new List<PurchaseEventArgs>()
			{
				new PurchaseEventArgs(0, 0, new decimal(0), "Coins", BankView.discountsCoins[0]),
				new PurchaseEventArgs(1, 0, new decimal(0), "Coins", BankView.discountsCoins[1]),
				new PurchaseEventArgs(2, 0, new decimal(0), "Coins", BankView.discountsCoins[2]),
				new PurchaseEventArgs(3, 0, new decimal(0), "Coins", BankView.discountsCoins[3]),
				new PurchaseEventArgs(4, 0, new decimal(0), "Coins", BankView.discountsCoins[4]),
				new PurchaseEventArgs(5, 0, new decimal(0), "Coins", BankView.discountsCoins[5]),
				new PurchaseEventArgs(6, 0, new decimal(0), "Coins", BankView.discountsCoins[6])
			};
			return purchaseEventArgs;
		}
	}

	public static IList<PurchaseEventArgs> goldPurchasesInfo_AB_Static
	{
		get
		{
			if (!BankView._isInitGoldPurchasesInfo_AB_Static)
			{
				BankView._goldPurchasesInfo_AB_Static = BankView.goldPurchasesInfo_AB_StaticDefault;
				FriendsController.ParseABTestBankViewConfig();
			}
			return BankView._goldPurchasesInfo_AB_Static;
		}
		set
		{
			BankView._goldPurchasesInfo_AB_Static = value;
			BankView._isInitGoldPurchasesInfo_AB_Static = true;
		}
	}

	public static IList<PurchaseEventArgs> goldPurchasesInfo_AB_StaticDefault
	{
		get
		{
			return (
				from pi in BankView.goldPurchasesInfo
				where pi.Index != 0
				select pi).ToList<PurchaseEventArgs>();
		}
	}

	public bool InterfaceEnabled
	{
		get
		{
			return (this.interfaceHolder == null ? false : this.interfaceHolder.activeInHierarchy);
		}
		set
		{
			if (this.interfaceHolder != null)
			{
				this.interfaceHolder.SetActive(value);
			}
		}
	}

	private bool IsInAB_Static_Bank
	{
		get
		{
			return (this.coinItemsABStatic == null || !this.coinItemsABStatic.Any<BankViewItem>() || this.gemItemsABStatic == null ? false : this.gemItemsABStatic.Any<BankViewItem>());
		}
	}

	public bool NotEnoughCoinsLabelEnabled
	{
		get
		{
			return (this.notEnoughCoinsLabel == null ? false : this.notEnoughCoinsLabel.gameObject.GetActive());
		}
		set
		{
			if (this.notEnoughCoinsLabel != null)
			{
				this.notEnoughCoinsLabel.gameObject.SetActive(value);
			}
		}
	}

	public bool NotEnoughGemsLabelEnabled
	{
		get
		{
			return (this.notEnoughGemsLabel == null ? false : this.notEnoughGemsLabel.gameObject.GetActive());
		}
		set
		{
			if (this.notEnoughGemsLabel != null)
			{
				this.notEnoughGemsLabel.gameObject.SetActive(value);
			}
		}
	}

	public bool PurchaseButtonsEnabled
	{
		set
		{
			this.btnTabContainer.SetActive(value);
			if (!value)
			{
				if (!this.IsInAB_Static_Bank)
				{
					this.goldScrollView.gameObject.SetActive(value);
					this.gemsScrollView.gameObject.SetActive(value);
				}
				else
				{
					this.goldContainerABStatic.SetActive(value);
					this.gemsContainerABStatic.SetActive(value);
				}
				this._isPurchasesAreadyEnabled = false;
			}
			else if (!this._isPurchasesAreadyEnabled)
			{
				this._isPurchasesAreadyEnabled = true;
				bool flag = this.btnTabGold.isEnabled;
				if (!this.IsInAB_Static_Bank)
				{
					this.goldScrollView.gameObject.SetActive(!flag);
					this.gemsScrollView.gameObject.SetActive(flag);
					this.ResetScrollView(flag, false);
				}
				else
				{
					this.goldContainerABStatic.SetActive(!flag);
					this.gemsContainerABStatic.SetActive(flag);
				}
			}
		}
	}

	public bool PurchaseSuccessfulLabelEnabled
	{
		get
		{
			return (this.purchaseSuccessfulLabel == null ? false : this.purchaseSuccessfulLabel.gameObject.GetActive());
		}
		set
		{
			if (this.purchaseSuccessfulLabel != null)
			{
				this.purchaseSuccessfulLabel.gameObject.SetActive(value);
			}
		}
	}

	static BankView()
	{
		BankView.discountsCoins = new int[] { 0, 0, 7, 10, 12, 15, 33 };
		BankView.discountsGems = new int[] { 0, 0, 7, 10, 12, 15, 33 };
	}

	public BankView()
	{
	}

	private void Awake()
	{
		this.InitializeButtonsCoroutine();
		if (this.IsInAB_Static_Bank)
		{
			FriendsController.StaticBankConfigUpdated += new Action(this.UpdateViewConfigChanged);
		}
	}

	private EventHandler CreateButtonHandler(PurchaseEventArgs purchaseInfo)
	{
		return (object sender, EventArgs e) => {
			EventHandler<PurchaseEventArgs> purchaseButtonPressed = this.PurchaseButtonPressed;
			if (purchaseButtonPressed != null)
			{
				purchaseButtonPressed(this, purchaseInfo);
			}
		};
	}

	public void Dispose()
	{
		if (this._disposed)
		{
			return;
		}
		UnityEngine.Debug.Log(string.Concat("Disposing ", base.GetType().Name));
		IEnumerator<Action> enumerator = (
			from a in this._disposeActions
			where a != null
			select a).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				enumerator.Current();
			}
		}
		finally
		{
			if (enumerator == null)
			{
			}
			enumerator.Dispose();
		}
		this._disposed = true;
	}

	private void InitializeButtonsCoroutine()
	{
		this._storeKitEventListener = UnityEngine.Object.FindObjectOfType<StoreKitEventListener>();
		if (this._storeKitEventListener != null)
		{
			if (!this.IsInAB_Static_Bank)
			{
				this.UpdateViewConfigChanged();
			}
			this.OnEnable();
			return;
		}
		UnityEngine.Debug.LogWarning("storeKitEventListener == null");
		if (this.goldItemPrefab != null)
		{
			this.goldItemPrefab.gameObject.SetActive(false);
		}
		if (this.gemsItemPrefab != null)
		{
			this.gemsItemPrefab.gameObject.SetActive(false);
		}
	}

	public void LoadCurrencyIcons(bool load)
	{
		List<BankViewItem> bankViewItems = (!this.IsInAB_Static_Bank ? ((IEnumerable<BankViewItem>)(this.goldItemGrid.GetComponentsInChildren<BankViewItem>(true) ?? new BankViewItem[0])).ToList<BankViewItem>() : this.coinItemsABStatic);
		for (int i = 0; i < bankViewItems.Count; i++)
		{
			if (bankViewItems[i].purchaseInfo == null)
			{
				return;
			}
			this.LoadIconForItem(bankViewItems[i], bankViewItems[i].purchaseInfo.Index, false, load);
		}
		List<BankViewItem> bankViewItems1 = (!this.IsInAB_Static_Bank ? ((IEnumerable<BankViewItem>)(this.gemsItemGrid.GetComponentsInChildren<BankViewItem>(true) ?? new BankViewItem[0])).ToList<BankViewItem>() : this.gemItemsABStatic);
		for (int j = 0; j < bankViewItems1.Count; j++)
		{
			this.LoadIconForItem(bankViewItems1[j], bankViewItems1[j].purchaseInfo.Index, true, load);
		}
	}

	private void LoadIconForItem(BankViewItem item, int i, bool isGems, bool load)
	{
		string str;
		if (!load)
		{
			item.icon.mainTexture = null;
		}
		else
		{
			if (!isGems)
			{
				object[] objArray = new object[] { "Textures/Bank", null, null, null };
				objArray[1] = (!this.IsInAB_Static_Bank ? string.Empty : "/Static_Bank_Textures");
				objArray[2] = "/Coins_Shop_";
				objArray[3] = i + 1;
				str = string.Concat(objArray);
			}
			else
			{
				str = string.Concat("Textures/Bank/Coins_Shop_Gem_", i + 1);
			}
			string str1 = str;
			if (!Device.IsLoweMemoryDevice)
			{
				item.icon.mainTexture = Resources.Load<Texture>(str1);
			}
			else
			{
				PreloadTexture component = item.icon.GetComponent<PreloadTexture>();
				if (component != null)
				{
					component.pathTexture = str1;
				}
			}
		}
	}

	public void OnBtnTabClick(UIButton btnTab)
	{
		if (btnTab == this.btnTabGold)
		{
			UnityEngine.Debug.Log("Activated Tab Gold");
			this.btnTabGold.isEnabled = false;
			this.btnTabGems.isEnabled = true;
			if (!this.IsInAB_Static_Bank)
			{
				this.goldScrollView.gameObject.SetActive(true);
				this.gemsScrollView.gameObject.SetActive(false);
				this.ResetScrollView(false, false);
			}
			else
			{
				this.goldContainerABStatic.SetActive(true);
				this.gemsContainerABStatic.SetActive(false);
			}
		}
		else if (btnTab != this.btnTabGems)
		{
			UnityEngine.Debug.Log("Unknown btnTab");
		}
		else
		{
			UnityEngine.Debug.Log("Activated Tab Gems");
			this.btnTabGold.isEnabled = true;
			this.btnTabGems.isEnabled = false;
			if (!this.IsInAB_Static_Bank)
			{
				this.goldScrollView.gameObject.SetActive(false);
				this.gemsScrollView.gameObject.SetActive(true);
				this.ResetScrollView(true, false);
			}
			else
			{
				this.goldContainerABStatic.SetActive(false);
				this.gemsContainerABStatic.SetActive(true);
			}
		}
	}

	private void OnDestroy()
	{
		if (this.IsInAB_Static_Bank)
		{
			FriendsController.StaticBankConfigUpdated -= new Action(this.UpdateViewConfigChanged);
		}
		this.Dispose();
	}

	private void OnEnable()
	{
		if (!this.IsInAB_Static_Bank)
		{
			this.SortItemGrid(false);
			this.SortItemGrid(true);
		}
		else
		{
			this.UpdateViewConfigChanged();
		}
		UIButton uIButton = this.btnTabGems;
		if (coinsShop.thisScript != null && coinsShop.thisScript.notEnoughCurrency != null && coinsShop.thisScript.notEnoughCurrency.Equals("Coins"))
		{
			uIButton = this.btnTabGold;
		}
		this.OnBtnTabClick(uIButton);
		this._localizeSaleLabel = LocalizationStore.Get("Key_0419");
		if (this.connectionProblemLabel != null)
		{
			this.connectionProblemLabel.text = LocalizationStore.Get("Key_0278");
		}
	}

	private void PopulateItemGrid(bool isGems)
	{
		IList<PurchaseEventArgs> purchaseEventArgs = (!isGems ? BankView.goldPurchasesInfo : BankView.gemsPurchasesInfo);
		if (!this.IsInAB_Static_Bank)
		{
			BankViewItem bankViewItem = (!isGems ? this.goldItemPrefab : this.gemsItemPrefab);
			UIScrollView uIScrollView = (!isGems ? this.goldScrollView : this.gemsScrollView);
			UIGrid uIGrid = (!isGems ? this.goldItemGrid : this.gemsItemGrid);
			for (int i = 0; i < purchaseEventArgs.Count; i++)
			{
				BankViewItem bankViewItem1 = UnityEngine.Object.Instantiate<BankViewItem>(bankViewItem);
				bankViewItem1.name = string.Format("{0:00}", i);
				bankViewItem1.transform.parent = uIGrid.transform;
				bankViewItem1.transform.localScale = Vector3.one;
				bankViewItem1.transform.localPosition = Vector3.zero;
				bankViewItem1.transform.localRotation = Quaternion.identity;
				this.UpdateItem(bankViewItem1, i, isGems);
			}
			bankViewItem.gameObject.SetActive(false);
			this.ResetScrollView(isGems, false);
		}
		else
		{
			for (int j = 0; j < purchaseEventArgs.Count; j++)
			{
				this.UpdateItem((!isGems ? this.coinItemsABStatic[j] : this.gemItemsABStatic[j]), j, isGems);
			}
		}
	}

	private void ResetScrollView(bool isGems, bool needDelayedUpdate = true)
	{
		if (this.IsInAB_Static_Bank)
		{
			return;
		}
		UIScrollView uIScrollView = (!isGems ? this.goldScrollView : this.gemsScrollView);
		UIGrid uIGrid = (!isGems ? this.goldItemGrid : this.gemsItemGrid);
		if (!needDelayedUpdate)
		{
			uIGrid.Reposition();
			uIScrollView.ResetPosition();
		}
		else
		{
			this._needResetScrollView = needDelayedUpdate;
		}
	}

	[DebuggerHidden]
	private IEnumerator ResetScrollViewsDelayed()
	{
		BankView.u003cResetScrollViewsDelayedu003ec__Iterator109 variable = null;
		return variable;
	}

	private void SortItemGrid(bool isGems)
	{
		if (this.IsInAB_Static_Bank)
		{
			return;
		}
		Transform transforms = ((!isGems ? this.goldItemGrid : this.gemsItemGrid)).transform;
		List<BankViewItem> bankViewItems = new List<BankViewItem>();
		for (int i = 0; i < transforms.childCount; i++)
		{
			bankViewItems.Add(transforms.GetChild(i).GetComponent<BankViewItem>());
		}
		bankViewItems.Sort();
		for (int j = 0; j < bankViewItems.Count; j++)
		{
			bankViewItems[j].gameObject.name = string.Format("{0:00}", j);
		}
		this.ResetScrollView(isGems, false);
	}

	private void Start()
	{
		if (!this.IsInAB_Static_Bank)
		{
			this.goldScrollView.panel.UpdateAnchors();
			this.gemsScrollView.panel.UpdateAnchors();
			this.ResetScrollView(false, false);
			this.ResetScrollView(true, false);
		}
	}

	private void Update()
	{
		if (this._needResetScrollView)
		{
			this._needResetScrollView = false;
			base.StartCoroutine(this.ResetScrollViewsDelayed());
		}
		if (Time.realtimeSinceStartup - this._lastUpdateTime >= 0.5f)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)PromoActionsManager.sharedManager.EventX3RemainedTime);
			string empty = string.Empty;
			if (timeSpan.Days <= 0)
			{
				empty = string.Format("{0}: {1:00}:{2:00}:{3:00}", new object[] { this._localizeSaleLabel, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds });
			}
			else
			{
				object[] days = new object[] { this._localizeSaleLabel, timeSpan.Days, null, null, null, null };
				days[2] = (timeSpan.Days != 1 ? "Days" : "Day");
				days[3] = timeSpan.Hours;
				days[4] = timeSpan.Minutes;
				days[5] = timeSpan.Seconds;
				empty = string.Format("{0}: {1} {2} {3:00}:{4:00}:{5:00}", days);
			}
			if (this.eventX3RemainTime != null)
			{
				for (int i = 0; i < (int)this.eventX3RemainTime.Length; i++)
				{
					this.eventX3RemainTime[i].text = empty;
				}
			}
			if (this.colorBlinkForX3 != null && timeSpan.TotalHours < (double)Defs.HoursToEndX3ForIndicate && !this.colorBlinkForX3.enabled)
			{
				this.colorBlinkForX3.enabled = true;
			}
			this._lastUpdateTime = Time.realtimeSinceStartup;
		}
		PremiumAccountController.AccountType currentAccount = PremiumAccountController.AccountType.None;
		if (PremiumAccountController.Instance != null)
		{
			currentAccount = PremiumAccountController.Instance.GetCurrentAccount();
		}
		this.premium.SetActive((currentAccount == PremiumAccountController.AccountType.SevenDays ? true : currentAccount == PremiumAccountController.AccountType.Month));
		this.premium5percent.SetActive(currentAccount == PremiumAccountController.AccountType.SevenDays);
		this.premium10percent.SetActive(currentAccount == PremiumAccountController.AccountType.Month);
		if (this._freeAwardButtonLabel != null && this.freeAwardButton.isActiveAndEnabled)
		{
			this._freeAwardButtonLabel.text = (FreeAwardController.Instance.CurrencyForAward != "GemsCurrency" ? string.Format("[FFA300FF]{0}[-]", ScriptLocalization.Get("Key_1155")) : string.Format("[50CEFFFF]{0}[-]", ScriptLocalization.Get("Key_2046")));
		}
	}

	private void UpdateItem(BankViewItem item, int i, bool isGems)
	{
		string str;
		PurchaseEventArgs num = (!isGems ? BankView.goldPurchasesInfo[i] : BankView.gemsPurchasesInfo[i]);
		string[] strArrays = (!isGems ? StoreKitEventListener.coinIds : StoreKitEventListener.gemsIds);
		if (num.Index < (int)strArrays.Length)
		{
			num.Count = (!isGems ? VirtualCurrencyHelper.coinInappsQuantity[num.Index] : VirtualCurrencyHelper.gemsInappsQuantity[num.Index]);
			decimal num1 = (!isGems ? VirtualCurrencyHelper.coinPriceIds[num.Index] : VirtualCurrencyHelper.gemsPriceIds[num.Index]);
			num.CurrencyAmount = num1 - new decimal(1, 0, 0, false, 2);
		}
		string price = string.Format("${0}", num.CurrencyAmount);
		if (num.Index >= (int)strArrays.Length)
		{
			UnityEngine.Debug.LogWarning("purchaseInfo.Index >= StoreKitEventListener.coinIds.Length");
		}
		else
		{
			string str1 = strArrays[num.Index];
			IMarketProduct marketProduct = this._storeKitEventListener.Products.FirstOrDefault<IMarketProduct>((IMarketProduct p) => p.Id == str1);
			if (marketProduct == null)
			{
				UnityEngine.Debug.LogWarning(string.Concat("marketProduct == null,    id: ", str1));
			}
			else
			{
				price = marketProduct.Price;
			}
		}
		item.Price = price;
		try
		{
			if (item.inappNameLabels != null)
			{
				foreach (UILabel inappNameLabel in item.inappNameLabels)
				{
					inappNameLabel.text = LocalizationStore.Get((!isGems ? VirtualCurrencyHelper.coinInappsLocalizationKeys[num.Index] : VirtualCurrencyHelper.gemsInappsLocalizationKeys[num.Index]));
				}
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception setting inapp localizations: ", exception));
		}
		if (!isGems)
		{
			object[] index = new object[] { "Textures/Bank", null, null, null };
			index[1] = (!this.IsInAB_Static_Bank ? string.Empty : "/Static_Bank_Textures");
			index[2] = "/Coins_Shop_";
			index[3] = num.Index + 1;
			str = string.Concat(index);
		}
		else
		{
			str = string.Concat("Textures/Bank/Coins_Shop_Gem_", num.Index + 1);
		}
		string str2 = str;
		if (!Device.IsLoweMemoryDevice)
		{
			item.icon.mainTexture = Resources.Load<Texture>(str2);
		}
		else
		{
			PreloadTexture component = item.icon.GetComponent<PreloadTexture>();
			if (component != null)
			{
				component.pathTexture = str2;
			}
		}
		ButtonHandler buttonHandler = item.btnBuy.GetComponent<ButtonHandler>();
		if (buttonHandler == null)
		{
			return;
		}
		item.Count = num.Count;
		item.CountX3 = 3 * num.Count;
		if (item.discountSprite != null)
		{
			item.discountSprite.gameObject.SetActive(num.Discount > 0);
		}
		if (item.discountPercentsLabel != null && num.Discount > 0)
		{
			item.discountPercentsLabel.text = string.Format("{0}%", num.Discount);
		}
		item.purchaseInfo = num;
		item.UpdateViewBestBuy();
		if (item.bonusButtonView != null)
		{
			item.bonusButtonView.UpdateState(num);
		}
		if (!buttonHandler.HasClickedHandlers)
		{
			EventHandler eventHandler = this.CreateButtonHandler(num);
			buttonHandler.Clicked += eventHandler;
			this._disposeActions.Add(new Action(() => buttonHandler.Clicked -= eventHandler));
		}
	}

	private void UpdateViewConfigChanged()
	{
		this.PopulateItemGrid(false);
		this.PopulateItemGrid(true);
	}

	public event EventHandler BackButtonPressed
	{
		add
		{
			if (this.backButton != null)
			{
				this.backButton.Clicked += value;
			}
		}
		remove
		{
			if (this.backButton != null)
			{
				this.backButton.Clicked -= value;
			}
		}
	}

	public event EventHandler<PurchaseEventArgs> PurchaseButtonPressed
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.PurchaseButtonPressed += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.PurchaseButtonPressed -= value;
		}
	}
}