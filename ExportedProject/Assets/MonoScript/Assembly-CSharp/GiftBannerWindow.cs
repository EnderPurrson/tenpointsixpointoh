using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GiftBannerWindow : BannerWindow
{
	public const string keyTrigOpenBanner = "OpenGiftPanel";

	public const string keyTrigTapButton = "OpenGiftBtnRelease";

	public const string keyTrigShowInfoGift = "GiftInfoShow";

	public const string keyTrigCloseInfoGift = "GiftInfoClose";

	public static GiftBannerWindow instance;

	public GameObject butBuy;

	public GameObject butGift;

	public GameObject bannerObj;

	public GiftScroll scrollGift;

	public UILabel lbPriceForBuy;

	public UILabel lbTimer;

	public UISprite sprDarkFon;

	public GameObject objSound;

	public UISprite sprFonScroll;

	public GiftHUDItem panelInfoGift;

	public Animator animatorBanner;

	public GameObject objLabelTapGift;

	public float speedAnimCenter = 2f;

	[SerializeField]
	private TextGroup _freeSpinsText;

	public static bool blockedButton;

	private SlotInfo awardSlot;

	private float delayBeforeNextStep = 5f;

	private bool canTapOnGift = true;

	private Coroutine crtForShowAward;

	private bool needShowGift;

	[HideInInspector]
	public bool needForceShowAwardGift;

	private IDisposable _backSubscription;

	private bool needPlayStartAnim = true;

	public static bool isForceClose;

	public static bool isActiveBanner;

	public GiftBannerWindow.StepAnimation curStateAnimAward;

	static GiftBannerWindow()
	{
	}

	public GiftBannerWindow()
	{
	}

	public void AnimFonShow(bool val)
	{
		if (this.sprDarkFon != null)
		{
			if (!val)
			{
				TweenAlpha.Begin(this.sprDarkFon.gameObject, 0.1f, 0f);
			}
			else
			{
				TweenAlpha.Begin(this.sprDarkFon.gameObject, 1f, 0.4f);
			}
		}
	}

	private void Awake()
	{
		GiftBannerWindow.instance = this;
		if (this.animatorBanner != null)
		{
			this.animatorBanner = base.GetComponent<Animator>();
		}
		GiftController.OnUpdateTimer += new Action<string>(this.UpdateLabelTimer);
		GiftController.OnTimerEnded += new Action(this.OnEndTimer);
		BankController.Instance.BackRequested += new EventHandler(this.BackFromBank);
	}

	private void BackFromBank(object sender, EventArgs e)
	{
		if (base.IsShow)
		{
			base.Invoke("OnCloseBank", 0.2f);
		}
	}

	public void BuyCanGetGift()
	{
		if (GiftBannerWindow.blockedButton)
		{
			return;
		}
		bool flag = false;
		ItemPrice itemPrice = new ItemPrice(GiftController.Instance.CostBuyCanGetGift.Value, "GemsCurrency");
		ShopNGUIController.TryToBuy(base.transform.root.gameObject, itemPrice, () => {
			flag = true;
			this.GetGiftCore(true);
		}, null, null, null, null, null);
		if (!flag)
		{
			this.SetVisibleBanner(false);
		}
		this.SetViewState();
	}

	public void CloseBanner()
	{
		if (GiftBannerWindow.blockedButton)
		{
			return;
		}
		if (BannerWindowController.SharedController && this.bannerObj != null && this.bannerObj.activeSelf)
		{
			ButtonClickSound.Instance.PlayClick();
			this.SetVisibleBanner(false);
			if (GiftBannerWindow.onClose != null)
			{
				GiftBannerWindow.onClose();
			}
			GiftBannerWindow.isActiveBanner = false;
		}
	}

	public void CloseBannerEndAnimtion()
	{
		BannerWindowController.SharedController.HideBannerWindow();
		this.SetVisibleBanner(true);
		if (MainMenuController.sharedController != null)
		{
			MainMenuController.sharedController.ShowSavePanel(true);
		}
	}

	public void CloseInfoGift(bool isForce = false)
	{
		this.canTapOnGift = true;
		base.CancelInvoke("StartNextStep");
		SpringPanel component = this.scrollGift.GetComponent<SpringPanel>();
		if (component)
		{
			UnityEngine.Object.Destroy(component);
		}
		if (this.crtForShowAward != null)
		{
			base.StopCoroutine(this.crtForShowAward);
		}
		this.animatorBanner.SetTrigger("GiftInfoClose");
		this.crtForShowAward = null;
		this.curStateAnimAward = GiftBannerWindow.StepAnimation.none;
		GiftScroll.canReCreateSlots = true;
		this.scrollGift.SetCanDraggable(true);
		this.SetViewState();
		this.HideDarkFon();
		base.Invoke("UnlockedBut", 1.5f);
		if (this.scrollGift != null)
		{
			this.scrollGift.UpdateListButton();
		}
		if (GiftBannerWindow.onHideInfoGift != null)
		{
			GiftBannerWindow.onHideInfoGift();
		}
		base.StartCoroutine(this.WaitAndSort());
		if (!isForce && FriendsController.ServerTime < (long)0)
		{
			AnimationGift.instance.CheckVisibleGift();
			this.ForceCloseAll();
		}
	}

	private SlotInfo CopySlot(SlotInfo curSlot)
	{
		SlotInfo slotInfo = new SlotInfo()
		{
			gift = new GiftInfo()
			{
				Id = curSlot.gift.Id
			}
		};
		slotInfo.gift.Count.Value = curSlot.gift.Count.Value;
		slotInfo.gift.KeyTranslateInfo = curSlot.gift.KeyTranslateInfo;
		slotInfo.CountGift = curSlot.CountGift;
		slotInfo.numInScroll = curSlot.numInScroll;
		slotInfo.category = curSlot.category;
		slotInfo.isActiveEvent = curSlot.isActiveEvent;
		return slotInfo;
	}

	public void ForceCloseAll()
	{
		if (GiftBannerWindow.instance == null || this.curStateAnimAward != GiftBannerWindow.StepAnimation.none || !GiftBannerWindow.isActiveBanner)
		{
			return;
		}
		GiftBannerWindow.isActiveBanner = false;
		GiftBannerWindow.isForceClose = true;
		this.needPlayStartAnim = true;
		GiftBannerWindow.blockedButton = false;
		BankController.canShowIndication = true;
		this.canTapOnGift = true;
		this.CloseInfoGift(true);
		this.HideDarkFon();
		this.SetVisibleBanner(false);
		MainMenuController.canRotationLobbyPlayer = true;
		if (GiftBannerWindow.onClose != null)
		{
			GiftBannerWindow.onClose();
		}
		if (AnimationGift.instance != null)
		{
			AnimationGift.instance.ResetAnimation();
		}
		this.curStateAnimAward = GiftBannerWindow.StepAnimation.none;
	}

	public void GetGift()
	{
		if (GiftBannerWindow.blockedButton)
		{
			return;
		}
		this.GetGiftCore(false);
	}

	private void GetGiftCore(bool isForMoneyGift)
	{
		if (!isForMoneyGift && !GiftController.Instance.CanGetGift)
		{
			return;
		}
		ButtonClickSound.Instance.PlayClick();
		BankController.UpdateAllIndicatorsMoney();
		BankController.canShowIndication = false;
		SlotInfo gift = GiftController.Instance.GetGift(isForMoneyGift);
		if (gift == null)
		{
			throw new Exception("failed get gift");
		}
		this.awardSlot = this.CopySlot(gift);
		if (this.awardSlot == null)
		{
			UnityEngine.Debug.LogError("GetGiftCore: awardSlot = null");
		}
		else if (this.awardSlot.gift == null)
		{
			UnityEngine.Debug.LogError("GetGiftCore: awardSlot.gift = null");
		}
		else
		{
			AnalyticsStuff.LogDailyGift(this.awardSlot.gift.Id, this.awardSlot.CountGift, isForMoneyGift);
		}
		GiftBannerWindow.blockedButton = true;
		this.scrollGift.SetCanDraggable(false);
		this.scrollGift.AnimScrollGift(this.awardSlot.numInScroll);
		this.animatorBanner.SetTrigger("OpenGiftBtnRelease");
		GiftScroll.canReCreateSlots = false;
		GiftController.Instance.ReCreateSlots();
		this.ShowDarkFon();
		this.StartShowAwardGift();
		if (GiftBannerWindow.onGetGift != null)
		{
			GiftBannerWindow.onGetGift();
		}
		this._freeSpinsText.gameObject.SetActive(false);
	}

	public string GetNameSpriteForSlot(SlotInfo curSlot)
	{
		if (curSlot.category.Type == GiftCategoryType.ArmorAndHat)
		{
			return "shop_icons_armor";
		}
		return string.Empty;
	}

	public void HideDarkFon()
	{
	}

	[DebuggerHidden]
	private IEnumerator OnAnimOpenGift()
	{
		GiftBannerWindow.u003cOnAnimOpenGiftu003ec__Iterator145 variable = null;
		return variable;
	}

	private void OnApplicationPause(bool pausing)
	{
		if (!pausing)
		{
			this.ForceCloseAll();
		}
	}

	public void OnClickGift()
	{
		if (this.canTapOnGift)
		{
			this.StartNextStep();
		}
	}

	private void OnCloseBank()
	{
		this.needPlayStartAnim = true;
		this.OnOpenBannerWindow();
	}

	private void OnDestroy()
	{
		GiftController.OnUpdateTimer -= new Action<string>(this.UpdateLabelTimer);
		GiftController.OnTimerEnded -= new Action(this.OnEndTimer);
		BankController.Instance.BackRequested -= new EventHandler(this.BackFromBank);
		GiftBannerWindow.instance = null;
	}

	private void OnDisable()
	{
		MainMenuHeroCamera.onEndOpenGift -= new Action(this.OnOpenBannerWindow);
		this.needPlayStartAnim = true;
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	private void OnEnable()
	{
		if (!this.bannerObj.activeSelf)
		{
			this.needPlayStartAnim = true;
		}
		if (this.objSound)
		{
			this.objSound.SetActive(Defs.isSoundFX);
		}
		MainMenuHeroCamera.onEndOpenGift += new Action(this.OnOpenBannerWindow);
		this.SetViewState();
		if (GiftController.Instance != null)
		{
			if (!GiftController.Instance.ActiveGift)
			{
				this.SetVisibleBanner(false);
				if (GiftBannerWindow.onClose != null)
				{
					GiftBannerWindow.onClose();
				}
			}
			else
			{
				GiftController.Instance.CheckAvaliableSlots();
			}
		}
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.CloseBanner), "Gift (Gotcha)");
	}

	private void OnEndTimer()
	{
		this.SetViewState();
	}

	private void OnOpenBannerWindow()
	{
		if (GiftBannerWindow.isForceClose)
		{
			return;
		}
		if (this.needPlayStartAnim)
		{
			this.SetVisibleBanner(true);
			this.needPlayStartAnim = false;
			this.scrollGift.SetCanDraggable(true);
			this.HideDarkFon();
			this.animatorBanner.SetTrigger("OpenGiftPanel");
		}
	}

	private void SetViewState()
	{
		this.lbPriceForBuy.text = GiftController.Instance.CostBuyCanGetGift.Value.ToString();
		string str = LocalizationStore.Get("Key_2196");
		int freeSpins = GiftController.Instance.FreeSpins + (!GiftController.Instance.CanGetTimerGift ? 0 : 1);
		if (GiftController.Instance.CanGetFreeSpinGift)
		{
			str = string.Format(str, (freeSpins <= 1 ? string.Empty : freeSpins.ToString()));
		}
		this._freeSpinsText.Text = str;
		this._freeSpinsText.gameObject.SetActiveSafe((!GiftController.Instance.CanGetFreeSpinGift ? false : freeSpins > 1));
		this.butBuy.SetActiveSafe(!GiftController.Instance.CanGetGift);
		this.butGift.SetActiveSafe(GiftController.Instance.CanGetGift);
		this.lbTimer.gameObject.SetActive(!GiftController.Instance.CanGetTimerGift);
		this.objLabelTapGift.SetActiveSafe(GiftController.Instance.CanGetGift);
	}

	public void SetVisibleBanner(bool val)
	{
		if (this.bannerObj != null)
		{
			this.bannerObj.SetActive(val);
		}
	}

	public void ShowDarkFon()
	{
	}

	public void ShowShop()
	{
		if (GiftBannerWindow.blockedButton)
		{
			return;
		}
		this.SetVisibleBanner(false);
		MainMenuController.sharedController.ShowBankWindow();
	}

	[ContextMenu("simulate pause")]
	private void SimPause()
	{
		this.ForceCloseAll();
	}

	private void StartNextStep()
	{
		switch (this.curStateAnimAward)
		{
			case GiftBannerWindow.StepAnimation.WaitForShowAward:
			{
				base.CancelInvoke("StartNextStep");
				this.curStateAnimAward = GiftBannerWindow.StepAnimation.ShowAward;
				this.StartNextStep();
				break;
			}
			case GiftBannerWindow.StepAnimation.ShowAward:
			{
				this.crtForShowAward = base.StartCoroutine(this.OnAnimOpenGift());
				break;
			}
			case GiftBannerWindow.StepAnimation.waitForClose:
			{
				this.CloseInfoGift(false);
				break;
			}
		}
	}

	public void StartShowAwardGift()
	{
		if (this.awardSlot == null)
		{
			this.CloseInfoGift(false);
		}
		else
		{
			this.canTapOnGift = false;
			this.curStateAnimAward = GiftBannerWindow.StepAnimation.WaitForShowAward;
			AnimationGift.instance.StartAnimForGetGift();
			base.CancelInvoke("StartNextStep");
			base.Invoke("StartNextStep", this.delayBeforeNextStep);
		}
	}

	private void UnlockedBut()
	{
		GiftBannerWindow.blockedButton = false;
	}

	private void UpdateLabelTimer(string curTime)
	{
		this.SetViewState();
		if (this.lbTimer != null)
		{
			this.lbTimer.text = curTime;
		}
	}

	public void UpdateSizeScroll()
	{
		int count = this.scrollGift.listButton.Count;
		if (count > 8)
		{
			count = 8;
		}
		count--;
		int num = count * this.scrollGift.wrapScript.itemSize;
		UIPanel vector4 = this.scrollGift.scView.panel;
		UISprite uISprite = this.sprFonScroll;
		Vector2 vector2 = this.sprFonScroll.localSize;
		uISprite.SetDimensions(num + 30, (int)vector2.y);
		float single = vector4.baseClipRegion.x;
		float single1 = vector4.baseClipRegion.y;
		float single2 = (float)num;
		Vector4 vector41 = vector4.baseClipRegion;
		vector4.baseClipRegion = new Vector4(single, single1, single2, vector41.w);
	}

	[DebuggerHidden]
	private IEnumerator WaitAndSort()
	{
		GiftBannerWindow.u003cWaitAndSortu003ec__Iterator146 variable = null;
		return variable;
	}

	public static event Action onClose;

	public static event Action onGetGift;

	public static event Action onHideInfoGift;

	public static event Action onOpenInfoGift;

	public enum StepAnimation
	{
		none,
		WaitForShowAward,
		ShowAward,
		waitForClose
	}
}