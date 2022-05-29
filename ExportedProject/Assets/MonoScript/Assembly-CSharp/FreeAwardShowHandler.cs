using I2.Loc;
using Rilisoft.NullExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FreeAwardShowHandler : MonoBehaviour
{
	public GameObject chestModelCoins;

	public GameObject chestModelGems;

	public GameObject freeAwardGuiPrefab;

	public static FreeAwardShowHandler Instance;

	private NickLabelController nickController;

	private bool clicked;

	private bool inside;

	public bool IsInteractable
	{
		get
		{
			return (base.gameObject.GetComponent<Collider>() == null ? false : base.gameObject.GetComponent<Collider>().enabled);
		}
		set
		{
			if (base.gameObject.GetComponent<Collider>() != null)
			{
				base.gameObject.GetComponent<Collider>().enabled = value;
			}
		}
	}

	public FreeAwardShowHandler()
	{
	}

	private void Awake()
	{
		FreeAwardShowHandler.Instance = this;
		if (FreeAwardController.Instance == null && this.freeAwardGuiPrefab != null)
		{
			UnityEngine.Object.Instantiate(this.freeAwardGuiPrefab, Vector3.zero, Quaternion.identity);
		}
		LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.OnLocalizationChanged);
	}

	public static void CheckShowChest(bool interfaceEnabled)
	{
		if (FreeAwardShowHandler.Instance == null)
		{
			return;
		}
		if (interfaceEnabled && FreeAwardShowHandler.Instance.gameObject.activeSelf)
		{
			FreeAwardShowHandler.Instance.HideChestTitle();
			FreeAwardShowHandler.Instance.gameObject.SetActive(false);
		}
	}

	private void CheckShowFreeAwardTitle(bool isEnable, bool needExitAnim = false)
	{
		if (this.nickController == null)
		{
			this.nickController = this.GetNickController();
		}
		if (isEnable && this.nickController != null)
		{
			this.SetFreeAwardLocalization();
			this.nickController.freeAwardTitle.gameObject.SetActive(true);
			this.PlayeAnimationTitle(false, this.nickController.freeAwardTitle);
		}
		else if (needExitAnim)
		{
			this.PlayeAnimationTitle(true, this.nickController.freeAwardTitle);
		}
	}

	private NickLabelController GetNickController()
	{
		NickLabelController nickLabelController;
		if (MainMenuController.sharedController == null)
		{
			nickLabelController = null;
		}
		else
		{
			nickLabelController = MainMenuController.sharedController.persNickLabel;
		}
		return nickLabelController;
	}

	[DebuggerHidden]
	private IEnumerator HideChestCoroutine()
	{
		FreeAwardShowHandler.u003cHideChestCoroutineu003ec__Iterator13C variable = null;
		return variable;
	}

	public void HideChestTitle()
	{
		if (this.nickController == null)
		{
			return;
		}
		this.nickController.freeAwardTitle.gameObject.SetActive(false);
	}

	public void HideChestWithAnimation()
	{
		base.StartCoroutine(this.HideChestCoroutine());
	}

	private bool NeedToSkip()
	{
		FreeAwardShowHandler.SkipReason skipCore = this.NeedToSkipCore();
		if (Defs.IsDeveloperBuild && skipCore != FreeAwardShowHandler.SkipReason.None)
		{
			UnityEngine.Debug.Log(string.Concat("Skipping free award chest: ", skipCore));
		}
		return skipCore != FreeAwardShowHandler.SkipReason.None;
	}

	private FreeAwardShowHandler.SkipReason NeedToSkipCore()
	{
		if (UICamera.currentTouch.Map<UICamera.MouseOrTouch, bool>((UICamera.MouseOrTouch t) => t.isOverUI))
		{
			return FreeAwardShowHandler.SkipReason.CameraTouchOverGui;
		}
		if (FriendsWindowGUI.Instance.InterfaceEnabled)
		{
			return FreeAwardShowHandler.SkipReason.FriendsInterfaceEnabled;
		}
		if (BankController.Instance != null && BankController.Instance.InterfaceEnabled)
		{
			return FreeAwardShowHandler.SkipReason.BankInterfaceEnabled;
		}
		if (ShopNGUIController.sharedShop != null && ShopNGUIController.GuiActive)
		{
			return FreeAwardShowHandler.SkipReason.ShopInterfaceEnabled;
		}
		if (FreeAwardController.Instance != null && !FreeAwardController.Instance.IsInState<FreeAwardController.IdleState>())
		{
			return FreeAwardShowHandler.SkipReason.RewardedVideoInterfaceEnabled;
		}
		if (BannerWindowController.SharedController != null && BannerWindowController.SharedController.IsAnyBannerShown)
		{
			return FreeAwardShowHandler.SkipReason.BannerEnabled;
		}
		if (AskNameManager.instance != null && !AskNameManager.isComplete)
		{
			return FreeAwardShowHandler.SkipReason.AskNameWindow;
		}
		MainMenuController mainMenuController = MainMenuController.sharedController;
		if (mainMenuController != null)
		{
			if (mainMenuController.RentExpiredPoint.Map<Transform, bool>((Transform r) => r.childCount > 0) || mainMenuController.SettingsJoysticksPanel.activeSelf || FeedbackMenuController.Instance != null && FeedbackMenuController.Instance.gameObject.activeSelf || mainMenuController.settingsPanel.activeSelf || mainMenuController.FreePanelIsActive || mainMenuController.singleModePanel.activeSelf)
			{
				return FreeAwardShowHandler.SkipReason.MainMenuComponentEnabled;
			}
		}
		if (LeaderboardScript.Instance != null && LeaderboardScript.Instance.UIEnabled)
		{
			return FreeAwardShowHandler.SkipReason.LeaderboardEnabled;
		}
		if (FriendsController.sharedController.Map<FriendsController, bool>((FriendsController c) => c.ProfileInterfaceActive))
		{
			return FreeAwardShowHandler.SkipReason.ProfileEnabled;
		}
		if (NewsLobbyController.sharedController != null && NewsLobbyController.sharedController.gameObject.activeSelf)
		{
			return FreeAwardShowHandler.SkipReason.NewsEnabled;
		}
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return FreeAwardShowHandler.SkipReason.LevelUpShown;
		}
		return FreeAwardShowHandler.SkipReason.None;
	}

	private void OnDestroy()
	{
		LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.OnLocalizationChanged);
		FreeAwardShowHandler.Instance = null;
	}

	private void OnEnable()
	{
		base.StartCoroutine(this.ShowChestCoroutine());
	}

	private void OnLocalizationChanged()
	{
		this.SetFreeAwardLocalization();
	}

	private void OnMouseDown()
	{
		this.clicked = true;
		this.inside = true;
	}

	private void OnMouseEnter()
	{
		if (this.clicked)
		{
			this.inside = true;
		}
	}

	private void OnMouseExit()
	{
		this.inside = false;
	}

	private void OnMouseUp()
	{
		this.clicked = false;
		if (!this.inside || this.NeedToSkip())
		{
			return;
		}
		this.inside = false;
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
		TimeSpan timeSpan = TimeSpan.FromMinutes(nums[num]);
		FreeAwardController.Instance.SetWatchState(dateTime + timeSpan);
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
	}

	private void PlayAnimationShowChest(bool isReserse)
	{
		Animation component = ((FreeAwardController.Instance.CurrencyForAward != "GemsCurrency" ? this.chestModelCoins : this.chestModelGems)).GetComponent<Animation>();
		if (component == null)
		{
			return;
		}
		if (!isReserse)
		{
			component["Animate"].speed = 1f;
			component["Animate"].time = 0f;
		}
		else
		{
			component["Animate"].speed = -1f;
			component["Animate"].time = component["Animate"].length;
		}
		component.Play();
	}

	private void PlayeAnimationTitle(bool isReverse, GameObject titleLabel)
	{
		UIPlayTween component = titleLabel.GetComponent<UIPlayTween>();
		component.resetOnPlay = true;
		component.tweenGroup = (!isReverse ? 0 : 1);
		component.Play(true);
	}

	private void SetFreeAwardLocalization()
	{
		if (this.nickController == null)
		{
			this.nickController = this.GetNickController();
		}
		if (this.nickController == null)
		{
			return;
		}
		(this.nickController.freeAwardTitle.GetComponent<UILabel>() ?? this.nickController.freeAwardTitle.GetComponentInChildren<UILabel>()).text = (FreeAwardController.Instance.CurrencyForAward != "GemsCurrency" ? ScriptLocalization.Get("Key_1155") : ScriptLocalization.Get("Key_2046"));
	}

	[DebuggerHidden]
	private IEnumerator ShowChestCoroutine()
	{
		FreeAwardShowHandler.u003cShowChestCoroutineu003ec__Iterator13B variable = null;
		return variable;
	}

	private enum SkipReason
	{
		None,
		CameraTouchOverGui,
		FriendsInterfaceEnabled,
		BankInterfaceEnabled,
		ShopInterfaceEnabled,
		RewardedVideoInterfaceEnabled,
		BannerEnabled,
		MainMenuComponentEnabled,
		LeaderboardEnabled,
		ProfileEnabled,
		NewsEnabled,
		LevelUpShown,
		AskNameWindow
	}
}