using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public class GiftBannerWindow : BannerWindow
{
	public enum StepAnimation
	{
		none = 0,
		WaitForShowAward = 1,
		ShowAward = 2,
		waitForClose = 3
	}

	[CompilerGenerated]
	private sealed class _003CBuyCanGetGift_003Ec__AnonStorey2B0
	{
		internal bool buySuccess;

		internal GiftBannerWindow _003C_003Ef__this;

		internal void _003C_003Em__2C2()
		{
			buySuccess = true;
			_003C_003Ef__this.GetGiftCore(true);
		}
	}

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

	public StepAnimation curStateAnimAward;

	public static event Action onClose;

	public static event Action onGetGift;

	public static event Action onHideInfoGift;

	public static event Action onOpenInfoGift;

	private void Awake()
	{
		instance = this;
		if (animatorBanner != null)
		{
			animatorBanner = GetComponent<Animator>();
		}
		GiftController.OnUpdateTimer += UpdateLabelTimer;
		GiftController.OnTimerEnded += OnEndTimer;
		BankController.Instance.BackRequested += BackFromBank;
	}

	private void OnDestroy()
	{
		GiftController.OnUpdateTimer -= UpdateLabelTimer;
		GiftController.OnTimerEnded -= OnEndTimer;
		BankController.Instance.BackRequested -= BackFromBank;
		instance = null;
	}

	private void OnEnable()
	{
		if (!bannerObj.activeSelf)
		{
			needPlayStartAnim = true;
		}
		if ((bool)objSound)
		{
			objSound.SetActive(Defs.isSoundFX);
		}
		MainMenuHeroCamera.onEndOpenGift += OnOpenBannerWindow;
		SetViewState();
		if (GiftController.Instance != null)
		{
			if (GiftController.Instance.ActiveGift)
			{
				GiftController.Instance.CheckAvaliableSlots();
			}
			else
			{
				SetVisibleBanner(false);
				if (GiftBannerWindow.onClose != null)
				{
					GiftBannerWindow.onClose();
				}
			}
		}
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
		_backSubscription = BackSystem.Instance.Register(CloseBanner, "Gift (Gotcha)");
	}

	private void OnDisable()
	{
		MainMenuHeroCamera.onEndOpenGift -= OnOpenBannerWindow;
		needPlayStartAnim = true;
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
	}

	private void BackFromBank(object sender, EventArgs e)
	{
		if (base.IsShow)
		{
			Invoke("OnCloseBank", 0.2f);
		}
	}

	private void OnCloseBank()
	{
		needPlayStartAnim = true;
		OnOpenBannerWindow();
	}

	public void ShowShop()
	{
		if (!blockedButton)
		{
			SetVisibleBanner(false);
			MainMenuController.sharedController.ShowBankWindow();
		}
	}

	public void GetGift()
	{
		if (!blockedButton)
		{
			GetGiftCore(false);
		}
	}

	private void OnOpenBannerWindow()
	{
		if (!isForceClose && needPlayStartAnim)
		{
			SetVisibleBanner(true);
			needPlayStartAnim = false;
			scrollGift.SetCanDraggable(true);
			HideDarkFon();
			animatorBanner.SetTrigger("OpenGiftPanel");
		}
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
		awardSlot = CopySlot(gift);
		if (awardSlot != null)
		{
			if (awardSlot.gift != null)
			{
				AnalyticsStuff.LogDailyGift(awardSlot.gift.Id, awardSlot.CountGift, isForMoneyGift);
			}
			else
			{
				Debug.LogError("GetGiftCore: awardSlot.gift = null");
			}
		}
		else
		{
			Debug.LogError("GetGiftCore: awardSlot = null");
		}
		blockedButton = true;
		scrollGift.SetCanDraggable(false);
		scrollGift.AnimScrollGift(awardSlot.numInScroll);
		animatorBanner.SetTrigger("OpenGiftBtnRelease");
		GiftScroll.canReCreateSlots = false;
		GiftController.Instance.ReCreateSlots();
		ShowDarkFon();
		StartShowAwardGift();
		if (GiftBannerWindow.onGetGift != null)
		{
			GiftBannerWindow.onGetGift();
		}
		_freeSpinsText.gameObject.SetActive(false);
	}

	private SlotInfo CopySlot(SlotInfo curSlot)
	{
		SlotInfo slotInfo = new SlotInfo();
		slotInfo.gift = new GiftInfo();
		slotInfo.gift.Id = curSlot.gift.Id;
		slotInfo.gift.Count.Value = curSlot.gift.Count.Value;
		slotInfo.gift.KeyTranslateInfo = curSlot.gift.KeyTranslateInfo;
		slotInfo.CountGift = curSlot.CountGift;
		slotInfo.numInScroll = curSlot.numInScroll;
		slotInfo.category = curSlot.category;
		slotInfo.isActiveEvent = curSlot.isActiveEvent;
		return slotInfo;
	}

	public void BuyCanGetGift()
	{
		_003CBuyCanGetGift_003Ec__AnonStorey2B0 _003CBuyCanGetGift_003Ec__AnonStorey2B = new _003CBuyCanGetGift_003Ec__AnonStorey2B0();
		_003CBuyCanGetGift_003Ec__AnonStorey2B._003C_003Ef__this = this;
		if (!blockedButton)
		{
			_003CBuyCanGetGift_003Ec__AnonStorey2B.buySuccess = false;
			ItemPrice price = new ItemPrice(GiftController.Instance.CostBuyCanGetGift.Value, "GemsCurrency");
			ShopNGUIController.TryToBuy(base.transform.root.gameObject, price, _003CBuyCanGetGift_003Ec__AnonStorey2B._003C_003Em__2C2);
			if (!_003CBuyCanGetGift_003Ec__AnonStorey2B.buySuccess)
			{
				SetVisibleBanner(false);
			}
			SetViewState();
		}
	}

	public void StartShowAwardGift()
	{
		if (awardSlot != null)
		{
			canTapOnGift = false;
			curStateAnimAward = StepAnimation.WaitForShowAward;
			AnimationGift.instance.StartAnimForGetGift();
			CancelInvoke("StartNextStep");
			Invoke("StartNextStep", delayBeforeNextStep);
		}
		else
		{
			CloseInfoGift();
		}
	}

	public void OnClickGift()
	{
		if (canTapOnGift)
		{
			StartNextStep();
		}
	}

	private void StartNextStep()
	{
		switch (curStateAnimAward)
		{
		case StepAnimation.WaitForShowAward:
			CancelInvoke("StartNextStep");
			curStateAnimAward = StepAnimation.ShowAward;
			StartNextStep();
			break;
		case StepAnimation.ShowAward:
			crtForShowAward = StartCoroutine(OnAnimOpenGift());
			break;
		case StepAnimation.waitForClose:
			CloseInfoGift();
			break;
		}
	}

	private IEnumerator OnAnimOpenGift()
	{
		CancelInvoke("StartNextStep");
		HideDarkFon();
		AnimationGift.instance.StopAnimForGetGift();
		if (GiftBannerWindow.onOpenInfoGift != null)
		{
			GiftBannerWindow.onOpenInfoGift();
		}
		panelInfoGift.SetInfoButton(awardSlot);
		awardSlot = null;
		yield return new WaitForSeconds(1f);
		BankController.canShowIndication = true;
		animatorBanner.SetTrigger("GiftInfoShow");
		yield return new WaitForSeconds(1.5f);
		curStateAnimAward = StepAnimation.waitForClose;
		canTapOnGift = true;
		Invoke("StartNextStep", delayBeforeNextStep);
	}

	public void CloseInfoGift(bool isForce = false)
	{
		canTapOnGift = true;
		CancelInvoke("StartNextStep");
		SpringPanel component = scrollGift.GetComponent<SpringPanel>();
		if ((bool)component)
		{
			UnityEngine.Object.Destroy(component);
		}
		if (crtForShowAward != null)
		{
			StopCoroutine(crtForShowAward);
		}
		animatorBanner.SetTrigger("GiftInfoClose");
		crtForShowAward = null;
		curStateAnimAward = StepAnimation.none;
		GiftScroll.canReCreateSlots = true;
		scrollGift.SetCanDraggable(true);
		SetViewState();
		HideDarkFon();
		Invoke("UnlockedBut", 1.5f);
		if (scrollGift != null)
		{
			scrollGift.UpdateListButton();
		}
		if (GiftBannerWindow.onHideInfoGift != null)
		{
			GiftBannerWindow.onHideInfoGift();
		}
		StartCoroutine(WaitAndSort());
		if (!isForce && FriendsController.ServerTime < 0)
		{
			AnimationGift.instance.CheckVisibleGift();
			ForceCloseAll();
		}
	}

	private IEnumerator WaitAndSort()
	{
		yield return null;
		scrollGift.transform.parent.localScale = Vector3.one;
		scrollGift.transform.localScale = Vector3.one;
		scrollGift.Sort();
		yield return null;
		while (scrollGift.transform.parent.localScale.Equals(Vector3.one))
		{
			yield return null;
		}
		scrollGift.Sort();
	}

	private void UnlockedBut()
	{
		blockedButton = false;
	}

	public void CloseBanner()
	{
		if (!blockedButton && (bool)BannerWindowController.SharedController && bannerObj != null && bannerObj.activeSelf)
		{
			ButtonClickSound.Instance.PlayClick();
			SetVisibleBanner(false);
			if (GiftBannerWindow.onClose != null)
			{
				GiftBannerWindow.onClose();
			}
			isActiveBanner = false;
		}
	}

	public void CloseBannerEndAnimtion()
	{
		BannerWindowController.SharedController.HideBannerWindow();
		SetVisibleBanner(true);
		if (MainMenuController.sharedController != null)
		{
			MainMenuController.sharedController.ShowSavePanel();
		}
	}

	public void SetVisibleBanner(bool val)
	{
		if (bannerObj != null)
		{
			bannerObj.SetActive(val);
		}
	}

	public string GetNameSpriteForSlot(SlotInfo curSlot)
	{
		GiftCategoryType giftCategoryType = curSlot.category.Type;
		if (giftCategoryType == GiftCategoryType.ArmorAndHat)
		{
			return "shop_icons_armor";
		}
		return string.Empty;
	}

	private void SetViewState()
	{
		lbPriceForBuy.text = GiftController.Instance.CostBuyCanGetGift.Value.ToString();
		string text = LocalizationStore.Get("Key_2196");
		int num = GiftController.Instance.FreeSpins + (GiftController.Instance.CanGetTimerGift ? 1 : 0);
		if (GiftController.Instance.CanGetFreeSpinGift)
		{
			text = string.Format(text, (num <= 1) ? string.Empty : num.ToString());
		}
		_freeSpinsText.Text = text;
		_freeSpinsText.gameObject.SetActiveSafe(GiftController.Instance.CanGetFreeSpinGift && num > 1);
		butBuy.SetActiveSafe(!GiftController.Instance.CanGetGift);
		butGift.SetActiveSafe(GiftController.Instance.CanGetGift);
		lbTimer.gameObject.SetActive(!GiftController.Instance.CanGetTimerGift);
		objLabelTapGift.SetActiveSafe(GiftController.Instance.CanGetGift);
	}

	public void ShowDarkFon()
	{
	}

	public void HideDarkFon()
	{
	}

	public void AnimFonShow(bool val)
	{
		if (sprDarkFon != null)
		{
			if (val)
			{
				TweenAlpha.Begin(sprDarkFon.gameObject, 1f, 0.4f);
			}
			else
			{
				TweenAlpha.Begin(sprDarkFon.gameObject, 0.1f, 0f);
			}
		}
	}

	private void UpdateLabelTimer(string curTime)
	{
		SetViewState();
		if (lbTimer != null)
		{
			lbTimer.text = curTime;
		}
	}

	private void OnEndTimer()
	{
		SetViewState();
	}

	private void OnApplicationPause(bool pausing)
	{
		if (!pausing)
		{
			ForceCloseAll();
		}
	}

	public void ForceCloseAll()
	{
		if (!(instance == null) && curStateAnimAward == StepAnimation.none && isActiveBanner)
		{
			isActiveBanner = false;
			isForceClose = true;
			needPlayStartAnim = true;
			blockedButton = false;
			BankController.canShowIndication = true;
			canTapOnGift = true;
			CloseInfoGift(true);
			HideDarkFon();
			SetVisibleBanner(false);
			MainMenuController.canRotationLobbyPlayer = true;
			if (GiftBannerWindow.onClose != null)
			{
				GiftBannerWindow.onClose();
			}
			if (AnimationGift.instance != null)
			{
				AnimationGift.instance.ResetAnimation();
			}
			curStateAnimAward = StepAnimation.none;
		}
	}

	public void UpdateSizeScroll()
	{
		int num = scrollGift.listButton.Count;
		if (num > 8)
		{
			num = 8;
		}
		num--;
		int num2 = num * scrollGift.wrapScript.itemSize;
		UIPanel panel = scrollGift.scView.panel;
		sprFonScroll.SetDimensions(num2 + 30, (int)sprFonScroll.localSize.y);
		panel.baseClipRegion = new Vector4(panel.baseClipRegion.x, panel.baseClipRegion.y, num2, panel.baseClipRegion.w);
	}

	[ContextMenu("simulate pause")]
	private void SimPause()
	{
		ForceCloseAll();
	}
}
