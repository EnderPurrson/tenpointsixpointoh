using Rilisoft;
using Rilisoft.NullExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class BankController : MonoBehaviour
{
	public const int InitialIosGems = 0;

	public const int InitialIosCoins = 0;

	public BankView bankView_AB_NoGrid_Common;

	public BankView bankView_AB_NoGrid_X3;

	public BankView bankViewCommon;

	public BankView bankViewX3;

	public GameObject uiRoot;

	public ChestBonusView bonusDetailView;

	public static bool canShowIndication;

	private BankView _bankViewCurrent;

	private bool firsEnterToBankOccured;

	private float tmOfFirstEnterTheBank;

	private bool _lockInterfaceEnabledCoroutine;

	private int _counterEn;

	private IDisposable _backSubscription;

	private readonly Lazy<bool> _timeTamperingDetected = new Lazy<bool>(new Func<bool>(() => {
		bool flag = FreeAwardController.Instance.TimeTamperingDetected();
		!flag;
		return flag;
	}));

	private static float _lastTimePurchaseButtonPressed;

	private bool _debugOptionsEnabled;

	private string _debugMessage = string.Empty;

	private bool _escapePressed;

	private static BankController _instance;

	private EventHandler EscapePressed;

	public static bool ABTestStaticBank
	{
		get;
		private set;
	}

	public BankView bankView
	{
		get
		{
			BankView bankView;
			if (PromoActionsManager.sharedManager == null)
			{
				UnityEngine.Debug.LogWarning("PromoActionsManager.sharedManager == null");
				return (!BankController.ABTestStaticBank ? this.bankViewCommon : this.bankView_AB_NoGrid_Common);
			}
			if (!PromoActionsManager.sharedManager.IsEventX3Active)
			{
				bankView = (!BankController.ABTestStaticBank ? this.bankViewCommon : this.bankView_AB_NoGrid_Common);
			}
			else
			{
				bankView = (!BankController.ABTestStaticBank ? this.bankViewX3 : this.bankView_AB_NoGrid_X3);
			}
			return bankView;
		}
	}

	public static BankController Instance
	{
		get
		{
			return BankController._instance;
		}
	}

	public bool InterfaceEnabled
	{
		get
		{
			return (!(this.bankView != null) || !(this.bankView.interfaceHolder != null) ? false : this.bankView.interfaceHolder.gameObject.activeInHierarchy);
		}
		set
		{
			if (value)
			{
				BankController.ABTestStaticBank = FriendsController.isShowStaticBank;
			}
			this.InterfaceEnabledCoroutine(value);
		}
	}

	public bool InterfaceEnabledCoroutineLocked
	{
		get
		{
			return this._lockInterfaceEnabledCoroutine;
		}
	}

	static BankController()
	{
		BankController.canShowIndication = true;
	}

	public BankController()
	{
	}

	public static void AddCoins(int count, bool needIndication = true, AnalyticsConstants.AccrualType accrualType = 0)
	{
		int num = Storager.getInt("Coins", false);
		Storager.setInt("Coins", num + count, false);
		AnalyticsFacade.CurrencyAccrual(count, "Coins", accrualType);
		if (needIndication)
		{
			CoinsMessage.FireCoinsAddedEvent(false, 2);
		}
	}

	public static void AddGems(int count, bool needIndication = true, AnalyticsConstants.AccrualType accrualType = 0)
	{
		int num = Storager.getInt("GemsCurrency", false);
		Storager.setInt("GemsCurrency", num + count, false);
		AnalyticsFacade.CurrencyAccrual(count, "GemsCurrency", accrualType);
		if (needIndication)
		{
			CoinsMessage.FireCoinsAddedEvent(true, 2);
		}
	}

	private void Awake()
	{
		BankController.GiveInitialNumOfCoins();
	}

	private static string ClampCoinCount(int coinCount)
	{
		return coinCount.ToString();
	}

	public void FreeAwardButtonClick()
	{
		ButtonClickSound.TryPlayClick();
		if (FreeAwardController.Instance == null)
		{
			return;
		}
		if (!FreeAwardController.Instance.AdvertCountLessThanLimit())
		{
			return;
		}
		List<double> nums = (!MobileAdManager.IsPayingUser() ? PromoActionsManager.MobileAdvert.RewardedVideoDelayMinutesNonpaying : PromoActionsManager.MobileAdvert.RewardedVideoDelayMinutesPaying);
		if (nums.Count == 0)
		{
			return;
		}
		DateTime date = DateTime.UtcNow.Date;
		KeyValuePair<int, DateTime> keyValuePair = FreeAwardController.Instance.LastAdvertShow(date);
		int num = Math.Max(0, keyValuePair.Key + 1);
		if (num > nums.Count)
		{
			return;
		}
		DateTime dateTime = (keyValuePair.Value >= date ? keyValuePair.Value : date);
		FreeAwardController.Instance.SetWatchState(dateTime + TimeSpan.FromMinutes(nums[num]));
	}

	public static void GiveInitialNumOfCoins()
	{
		if (!Storager.hasKey("Coins"))
		{
			Storager.setInt("Coins", 0, false);
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				PlayerPrefs.Save();
			}
		}
		if (!Storager.hasKey("GemsCurrency"))
		{
			switch (BuildSettings.BuildTargetPlatform)
			{
				case RuntimePlatform.IPhonePlayer:
				{
					Storager.setInt("GemsCurrency", 0, false);
					break;
				}
				case RuntimePlatform.PS3:
				case RuntimePlatform.XBOX360:
				{
					break;
				}
				case RuntimePlatform.Android:
				{
					Storager.setInt("GemsCurrency", 0, false);
					break;
				}
				default:
				{
					goto case RuntimePlatform.XBOX360;
				}
			}
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				PlayerPrefs.Save();
			}
		}
	}

	private void HandleEscape()
	{
		if (!(FreeAwardController.Instance != null) || FreeAwardController.Instance.IsInState<FreeAwardController.IdleState>())
		{
			this._escapePressed = true;
			return;
		}
		FreeAwardController.Instance.HandleClose();
		this._escapePressed = false;
	}

	private void HandlePurchaseButtonPressed(object sender, PurchaseEventArgs e)
	{
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64 && Time.realtimeSinceStartup - BankController._lastTimePurchaseButtonPressed < 1f)
		{
			UnityEngine.Debug.Log(string.Concat("Bank button pressed but ignored: ", e));
			return;
		}
		BankController._lastTimePurchaseButtonPressed = Time.realtimeSinceStartup;
		UnityEngine.Debug.Log(string.Concat("Bank button pressed: ", e));
		if (StoreKitEventListener.purchaseInProcess)
		{
			UnityEngine.Debug.Log("Cannot perform request while purchase is in progress.");
		}
		if (coinsShop.thisScript != null)
		{
			coinsShop.thisScript.HandlePurchaseButton(e.Index, e.Currency);
		}
	}

	private void InterfaceEnabledCoroutine(bool value)
	{
		BankView bankView;
		if (!value && this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
		this._lockInterfaceEnabledCoroutine = true;
		BankController bankController = this;
		int num = bankController._counterEn;
		int num1 = num;
		bankController._counterEn = num + 1;
		int num2 = num1;
		UnityEngine.Debug.Log(string.Concat(new object[] { "InterfaceEnabledCoroutine ", num2, " start: ", value }));
		try
		{
			if (value && !this.firsEnterToBankOccured)
			{
				this.firsEnterToBankOccured = true;
				this.tmOfFirstEnterTheBank = Time.realtimeSinceStartup;
			}
			value;
			if (this.bankView != this._bankViewCurrent && this._bankViewCurrent != null && this._bankViewCurrent.interfaceHolder != null)
			{
				this._bankViewCurrent.interfaceHolder.gameObject.SetActive(false);
				this._bankViewCurrent = null;
			}
			if (this.bankView != null && this.bankView.interfaceHolder != null)
			{
				this.bankView.interfaceHolder.gameObject.SetActive(value);
				if (!value)
				{
					bankView = null;
				}
				else
				{
					bankView = this.bankView;
				}
				this._bankViewCurrent = bankView;
				if (value)
				{
					this.bankView.LoadCurrencyIcons(value);
				}
			}
			this.uiRoot.SetActive(value);
			if (!value)
			{
				ActivityIndicator.IsActiveIndicator = false;
				this.bankViewCommon.LoadCurrencyIcons(value);
				this.bankViewX3.LoadCurrencyIcons(value);
				this.bankView_AB_NoGrid_Common.LoadCurrencyIcons(value);
				this.bankView_AB_NoGrid_X3.LoadCurrencyIcons(value);
			}
			FreeAwardShowHandler.CheckShowChest(value);
			if (value)
			{
				coinsShop.thisScript.RefreshProductsIfNeed(false);
				this.OnEventX3AmazonBonusUpdated();
			}
		}
		finally
		{
			if (value)
			{
				if (this._backSubscription != null)
				{
					this._backSubscription.Dispose();
				}
				this._backSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "Bank");
			}
			if (Device.IsLoweMemoryDevice)
			{
				ActivityIndicator.ClearMemory();
			}
			this._lockInterfaceEnabledCoroutine = false;
			UnityEngine.Debug.Log(string.Concat(new object[] { "InterfaceEnabledCoroutine ", num2, " stop: ", value }));
		}
	}

	private void LateUpdate()
	{
		if (this.InterfaceEnabled && ExperienceController.sharedController != null && !this._lockInterfaceEnabledCoroutine)
		{
			ExperienceController.sharedController.isShowRanks = false;
		}
	}

	private void OnDestroy()
	{
		PromoActionsManager.EventX3Updated -= new Action(this.OnEventX3Updated);
		PromoActionsManager.EventAmazonX3Updated -= new Action(this.OnEventX3AmazonBonusUpdated);
		if (this.bankViewCommon != null)
		{
			this.bankViewCommon.PurchaseButtonPressed -= new EventHandler<PurchaseEventArgs>(this.HandlePurchaseButtonPressed);
		}
		if (this.bankViewX3 != null)
		{
			this.bankViewX3.PurchaseButtonPressed -= new EventHandler<PurchaseEventArgs>(this.HandlePurchaseButtonPressed);
		}
		if (this.bankView_AB_NoGrid_Common != null)
		{
			this.bankView_AB_NoGrid_Common.PurchaseButtonPressed -= new EventHandler<PurchaseEventArgs>(this.HandlePurchaseButtonPressed);
		}
		if (this.bankView_AB_NoGrid_X3 != null)
		{
			this.bankView_AB_NoGrid_X3.PurchaseButtonPressed -= new EventHandler<PurchaseEventArgs>(this.HandlePurchaseButtonPressed);
		}
	}

	private void OnEventX3AmazonBonusUpdated()
	{
		if (this._bankViewCurrent == null || this._bankViewCurrent.eventX3AmazonBonusWidget == null)
		{
			return;
		}
		GameObject gameObject = this._bankViewCurrent.eventX3AmazonBonusWidget.gameObject;
		gameObject.SetActive(PromoActionsManager.sharedManager.IsAmazonEventX3Active);
		UILabel[] componentsInChildren = gameObject.GetComponentsInChildren<UILabel>();
		UILabel uILabel = this.bankView.Map<BankView, UILabel>((BankView b) => b.amazonEventCaptionLabel) ?? ((IEnumerable<UILabel>)componentsInChildren).FirstOrDefault<UILabel>((UILabel l) => "CaptionLabel".Equals(l.name, StringComparison.OrdinalIgnoreCase));
		PromoActionsManager.AmazonEventInfo amazonEventInfo = PromoActionsManager.sharedManager.Map<PromoActionsManager, PromoActionsManager.AmazonEventInfo>((PromoActionsManager p) => p.AmazonEvent);
		if (uILabel != null)
		{
			uILabel.text = amazonEventInfo.Map<PromoActionsManager.AmazonEventInfo, string>((PromoActionsManager.AmazonEventInfo e) => e.Caption) ?? string.Empty;
		}
		UILabel[] uILabelArray = (this.bankView.Map<BankView, UILabel>((BankView b) => b.amazonEventTitleLabel) ?? ((IEnumerable<UILabel>)componentsInChildren).FirstOrDefault<UILabel>((UILabel l) => "TitleLabel".Equals(l.name, StringComparison.OrdinalIgnoreCase))).Map<UILabel, UILabel[]>((UILabel t) => t.GetComponentsInChildren<UILabel>()) ?? new UILabel[0];
		float single = amazonEventInfo.Map<PromoActionsManager.AmazonEventInfo, float>((PromoActionsManager.AmazonEventInfo e) => e.Percentage);
		string str = LocalizationStore.Get("Key_1672");
		UILabel[] uILabelArray1 = uILabelArray;
		for (int i = 0; i < (int)uILabelArray1.Length; i++)
		{
			uILabelArray1[i].text = ("Key_1672".Equals(str, StringComparison.OrdinalIgnoreCase) ? string.Empty : string.Format(str, single));
		}
	}

	private void OnEventX3Updated()
	{
		if (this._bankViewCurrent != null)
		{
			this.InterfaceEnabledCoroutine(true);
		}
	}

	private void Start()
	{
		BankController._instance = this;
		PromoActionsManager.EventX3Updated += new Action(this.OnEventX3Updated);
		if (this.bankViewCommon != null)
		{
			this.bankViewCommon.PurchaseButtonPressed += new EventHandler<PurchaseEventArgs>(this.HandlePurchaseButtonPressed);
		}
		if (this.bankViewX3 != null)
		{
			this.bankViewX3.PurchaseButtonPressed += new EventHandler<PurchaseEventArgs>(this.HandlePurchaseButtonPressed);
		}
		if (this.bankView_AB_NoGrid_Common != null)
		{
			this.bankView_AB_NoGrid_Common.PurchaseButtonPressed += new EventHandler<PurchaseEventArgs>(this.HandlePurchaseButtonPressed);
		}
		if (this.bankView_AB_NoGrid_X3 != null)
		{
			this.bankView_AB_NoGrid_X3.PurchaseButtonPressed += new EventHandler<PurchaseEventArgs>(this.HandlePurchaseButtonPressed);
		}
		PromoActionsManager.EventAmazonX3Updated += new Action(this.OnEventX3AmazonBonusUpdated);
		HashSet<string> strs = new HashSet<string>();
		strs.Add("7FFC6ACA-F568-46C3-86AD-8A4FA2DF4401");
		this._debugOptionsEnabled = strs.Contains(SystemInfo.deviceUniqueIdentifier);
		this.bankView.freeAwardButton.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (!this.InterfaceEnabled)
		{
			this._escapePressed = false;
			return;
		}
		if (FreeAwardController.Instance == null)
		{
			this.bankView.freeAwardButton.gameObject.SetActive(false);
		}
		else if (!Defs.MainMenuScene.Equals(Application.loadedLevelName, StringComparison.Ordinal))
		{
			this.bankView.freeAwardButton.gameObject.SetActive(false);
		}
		else if (MobileAdManager.AdIsApplicable(MobileAdManager.Type.Video))
		{
			if (!this._timeTamperingDetected.Value)
			{
				if (FreeAwardController.Instance.IsInState<FreeAwardController.IdleState>())
				{
					bool flag = FreeAwardController.Instance.AdvertCountLessThanLimit();
					this.bankView.freeAwardButton.gameObject.SetActive(flag);
				}
			}
		}
		this.UpdateBankView(this.bankViewCommon);
		this.UpdateBankView(this.bankViewX3);
		this.UpdateBankView(this.bankView_AB_NoGrid_Common);
		this.UpdateBankView(this.bankView_AB_NoGrid_X3);
		EventHandler escapePressed = this.EscapePressed;
		if (this._escapePressed && escapePressed != null)
		{
			escapePressed(this, EventArgs.Empty);
			this._escapePressed = false;
		}
	}

	public static void UpdateAllIndicatorsMoney()
	{
		if (BankController.onUpdateMoney != null)
		{
			BankController.onUpdateMoney();
		}
	}

	private void UpdateBankView(BankView bankView)
	{
		if (bankView == null || !bankView.gameObject.activeSelf)
		{
			return;
		}
		if (coinsShop.IsWideLayoutAvailable)
		{
			bankView.ConnectionProblemLabelEnabled = false;
			bankView.CrackersWarningLabelEnabled = true;
			bankView.NotEnoughCoinsLabelEnabled = false;
			bankView.NotEnoughGemsLabelEnabled = false;
			bankView.PurchaseButtonsEnabled = false;
			bankView.PurchaseSuccessfulLabelEnabled = false;
		}
		else if (coinsShop.thisScript != null)
		{
			bankView.NotEnoughCoinsLabelEnabled = (coinsShop.thisScript.notEnoughCurrency == null ? false : coinsShop.thisScript.notEnoughCurrency.Equals("Coins"));
			bankView.NotEnoughGemsLabelEnabled = (coinsShop.thisScript.notEnoughCurrency == null ? false : coinsShop.thisScript.notEnoughCurrency.Equals("GemsCurrency"));
			ActivityIndicator.IsActiveIndicator = StoreKitEventListener.purchaseInProcess;
			if (!coinsShop.IsNoConnection)
			{
				bankView.ConnectionProblemLabelEnabled = false;
				bankView.PurchaseButtonsEnabled = true;
			}
			else
			{
				if (Time.realtimeSinceStartup - this.tmOfFirstEnterTheBank > 3f)
				{
					bankView.ConnectionProblemLabelEnabled = true;
				}
				bankView.NotEnoughCoinsLabelEnabled = false;
				bankView.NotEnoughGemsLabelEnabled = false;
				bankView.PurchaseButtonsEnabled = false;
				bankView.PurchaseSuccessfulLabelEnabled = false;
			}
			bankView.PurchaseSuccessfulLabelEnabled = coinsShop.thisScript.ProductPurchasedRecently;
		}
	}

	[DebuggerHidden]
	public static IEnumerator WaitForIndicationGems(bool isGems)
	{
		BankController.u003cWaitForIndicationGemsu003ec__Iterator107 variable = null;
		return variable;
	}

	public event EventHandler BackRequested
	{
		add
		{
			if (this.bankViewCommon != null)
			{
				this.bankViewCommon.BackButtonPressed += value;
			}
			if (this.bankViewX3 != null)
			{
				this.bankViewX3.BackButtonPressed += value;
			}
			if (this.bankView_AB_NoGrid_Common != null)
			{
				this.bankView_AB_NoGrid_Common.BackButtonPressed += value;
			}
			if (this.bankView_AB_NoGrid_X3 != null)
			{
				this.bankView_AB_NoGrid_X3.BackButtonPressed += value;
			}
			this.EscapePressed += value;
		}
		remove
		{
			if (this.bankViewCommon != null)
			{
				this.bankViewCommon.BackButtonPressed -= value;
			}
			if (this.bankViewX3 != null)
			{
				this.bankViewX3.BackButtonPressed -= value;
			}
			if (this.bankView_AB_NoGrid_Common != null)
			{
				this.bankView_AB_NoGrid_Common.BackButtonPressed -= value;
			}
			if (this.bankView_AB_NoGrid_X3 != null)
			{
				this.bankView_AB_NoGrid_X3.BackButtonPressed -= value;
			}
			this.EscapePressed -= value;
		}
	}

	private event EventHandler EscapePressed
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.EscapePressed += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.EscapePressed -= value;
		}
	}

	public static event Action onUpdateMoney;
}