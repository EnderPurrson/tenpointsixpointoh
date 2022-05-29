using Facebook.Unity;
using I2.Loc;
using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class RewardWindowBase : MonoBehaviour
{
	private const string DefaultCallerContext = "Reward Window";

	public bool ShowLoginsButton = true;

	public GameObject connectToSocialContainer;

	public List<UILabel> connectToSocial;

	public UIWidget containerWidget;

	public float widgetExpanded;

	public float widgetCollapsed;

	public float widgetNoConnectToSocial;

	public UIButton facebook;

	public UIButton twitter;

	public UIButton continueButton;

	public UIButton hideButton;

	public UIButton facebookNoReward;

	public UIButton twitterNoReward;

	public UIButton continueAndShare;

	public UIButton collect;

	public UIButton collectAndShare;

	public UIGrid innerGrid;

	public ToggleButton share;

	public GameObject shareContainer;

	public bool shouldHideExpGui = true;

	public GameObject soundsController;

	[Header("Not Connected")]
	public Vector3 continue_NotConnected;

	[Header("Not Connected")]
	public Vector3 facebook_NotConnected;

	[Header("Not Connected")]
	public Vector3 twitter_NotConnected;

	[Header("Twitter Connected")]
	public Vector3 continue_TwiiterConnected;

	[Header("Twitter Connected")]
	public Vector3 facebook_TwiiterConnected;

	[Header("Facebook Connected")]
	public Vector3 continue_FacebookConnected;

	[Header("Facebook Connected")]
	public Vector3 twitter_FacebookConnected;

	[Header("All Connected")]
	public Vector3 continue_AllConnected;

	public Action shareAction;

	public Action customHide;

	private bool _collectOnlyNoShare;

	[HideInInspector]
	public Animator animatorLevel;

	private IDisposable _backSubscription;

	private float _timeSinceUpdateConnetToSocialText;

	private FacebookController.StoryPriority _facebookPriority;

	private FacebookController.StoryPriority _twiiterPriority;

	private string CallerContext
	{
		get
		{
			return (!string.IsNullOrEmpty(this.EventTitle) ? string.Format("{0}: {1}", "Reward Window", this.EventTitle) : "Reward Window");
		}
	}

	public bool CollectOnlyNoShare
	{
		get
		{
			return this._collectOnlyNoShare;
		}
		set
		{
			this._collectOnlyNoShare = value;
		}
	}

	public string EventTitle
	{
		get;
		set;
	}

	public FacebookController.StoryPriority facebookPriority
	{
		get
		{
			return this._facebookPriority;
		}
		set
		{
			this._facebookPriority = value;
		}
	}

	public bool HasReward
	{
		get;
		set;
	}

	public FacebookController.StoryPriority priority
	{
		set
		{
			this.facebookPriority = value;
			this.twitterPriority = value;
		}
	}

	public FacebookController.StoryPriority twitterPriority
	{
		get
		{
			return this._twiiterPriority;
		}
		set
		{
			this._twiiterPriority = value;
		}
	}

	public Func<string> twitterStatus
	{
		get;
		set;
	}

	public RewardWindowBase()
	{
	}

	private void Awake()
	{
		if (this.share != null)
		{
			this.share.IsChecked = true;
		}
		this.animatorLevel = base.GetComponent<Animator>();
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	public void HandleContinueButton()
	{
		this.HandleContinueButtonNoHide();
		this.Hide();
	}

	public void HandleContinueButtonNoHide()
	{
		Dictionary<string, object> strs;
		ButtonClickSound.TryPlayClick();
		if (TwitterController.TwitterSupported && TwitterController.IsLoggedIn && TwitterController.Instance.CanPostStatusUpdateWithPriority(this.twitterPriority))
		{
			TwitterController.Instance.PostStatusUpdate(this.twitterStatus(), this.twitterPriority);
			strs = new Dictionary<string, object>()
			{
				{ "Post Twitter", this.EventTitle },
				{ "Total Twitter", "Posts" }
			};
			AnalyticsFacade.SendCustomEvent("Virality", strs);
		}
		if (FacebookController.FacebookSupported && FB.IsLoggedIn && FacebookController.sharedController.CanPostStoryWithPriority(this.facebookPriority))
		{
			Action action = this.shareAction;
			if (action != null)
			{
				action();
				strs = new Dictionary<string, object>()
				{
					{ "Post Facebook", this.EventTitle },
					{ "Total Facebook", "Posts" }
				};
				AnalyticsFacade.SendCustomEvent("Virality", strs);
			}
		}
	}

	public void HandleFacebookButton()
	{
		ButtonClickSound.TryPlayClick();
		MainMenuController.DoMemoryConsumingTaskInEmptyScene(() => FacebookController.Login(new Action(this.ShowAuthorizationSucceded), new Action(this.ShowAuthorizationFailed), this.CallerContext, null), () => FacebookController.Login(null, null, this.CallerContext, null));
	}

	private void HandleLocalizationChanged()
	{
		this.SetConnectToSocialText();
	}

	public void HandleShareToggle(UIToggle toggle)
	{
	}

	public void HandleTwitterButton()
	{
		ButtonClickSound.TryPlayClick();
		MainMenuController.DoMemoryConsumingTaskInEmptyScene(() => {
			if (TwitterController.Instance != null)
			{
				TwitterController.Instance.Login(new Action(this.ShowAuthorizationSucceded), new Action(this.ShowAuthorizationFailed), this.CallerContext);
			}
		}, () => {
			if (TwitterController.Instance != null)
			{
				TwitterController.Instance.Login(null, null, this.CallerContext);
			}
		});
	}

	public void Hide()
	{
		ButtonClickSound.TryPlayClick();
		Action action = this.customHide;
		if (action != null)
		{
			action();
			this.customHide = null;
		}
		base.transform.parent = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	private void OnDisable()
	{
		if (this.shouldHideExpGui)
		{
			RentScreenController.SetDepthForExpGUI(99);
		}
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	private void OnEnable()
	{
		this.SetConnectToSocialText();
		if (this.soundsController)
		{
			this.soundsController.SetActive(Defs.isSoundFX);
		}
		if (Application.isEditor)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		Action action = RewardWindowBase.Shown;
		if (action != null)
		{
			action();
		}
		if (this.shouldHideExpGui)
		{
			RentScreenController.SetDepthForExpGUI(0);
		}
		base.StartCoroutine(this.SendAnalyticsOnShowCoroutine());
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(() => {
			if (this.hideButton != null)
			{
				EventDelegate.Execute(this.hideButton.onClick);
			}
		}, "Reward Window");
		if (this.animatorLevel != null)
		{
			if (ExperienceController.sharedController.currentLevel == 2)
			{
				this.animatorLevel.SetTrigger("Weapons");
			}
			else if (ExperienceController.sharedController.AddHealthOnCurLevel != 0)
			{
				this.animatorLevel.SetTrigger("is3items");
			}
			else
			{
				this.animatorLevel.SetTrigger("is2items");
			}
			this.animatorLevel.SetBool("IsRatingSystem", FriendsController.isUseRatingSystem);
		}
	}

	[DebuggerHidden]
	private IEnumerator SendAnalyticsOnShowCoroutine()
	{
		RewardWindowBase.u003cSendAnalyticsOnShowCoroutineu003ec__Iterator102 variable = null;
		return variable;
	}

	private void SetButtonPositionsAndActive()
	{
		bool flag;
		if (!this.ShowLoginsButton)
		{
			return;
		}
		bool flag1 = false;
		bool flag2 = ((!FacebookController.FacebookSupported || Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) != 0) && (!TwitterController.TwitterSupported || Storager.getInt(Defs.IsTwitterLoginRewardaGained, true) != 0) || this.CollectOnlyNoShare || !this.ShowLoginsButton ? false : !Device.isPixelGunLow);
		if (this.connectToSocialContainer != null && this.connectToSocialContainer.activeSelf != flag2)
		{
			this.connectToSocialContainer.SetActive(flag2);
			flag1 = true;
		}
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = (!FacebookController.FacebookSupported || !this.ShouldShowFacebookWithRewardButton() ? false : TrainingController.TrainingCompleted);
		if (this.facebook != null && this.facebook.gameObject.activeSelf != flag5)
		{
			this.facebook.gameObject.SetActive(flag5);
			flag3 = true;
		}
		bool flag6 = (!FacebookController.FacebookSupported || FB.IsLoggedIn || Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) != 1 || this.CollectOnlyNoShare || !TrainingController.TrainingCompleted || !this.ShowLoginsButton ? false : !Device.isPixelGunLow);
		if (this.facebookNoReward != null && this.facebookNoReward.gameObject.activeSelf != flag6)
		{
			this.facebookNoReward.gameObject.SetActive(flag6);
			flag3 = true;
		}
		bool flag7 = (!TwitterController.TwitterSupported || !this.ShouldShowTwitterWithRewardButton() ? false : TrainingController.TrainingCompleted);
		if (this.twitter != null && this.twitter.gameObject.activeSelf != flag7)
		{
			this.twitter.gameObject.SetActive(flag7);
			flag3 = true;
		}
		bool flag8 = (!TwitterController.TwitterSupported || TwitterController.IsLoggedIn || Storager.getInt(Defs.IsTwitterLoginRewardaGained, true) != 1 || this.CollectOnlyNoShare || !TrainingController.TrainingCompleted || !this.ShowLoginsButton ? false : !Device.isPixelGunLow);
		if (this.twitterNoReward != null && this.twitterNoReward.gameObject.activeSelf != flag8)
		{
			this.twitterNoReward.gameObject.SetActive(flag8);
			flag3 = true;
		}
		if (FacebookController.FacebookSupported && (flag5 || flag6))
		{
			flag = true;
		}
		else if (!TwitterController.TwitterSupported)
		{
			flag = false;
		}
		else
		{
			flag = (flag7 ? true : flag8);
		}
		bool flag9 = flag;
		if (this.innerGrid != null && this.innerGrid.gameObject.activeSelf != flag9)
		{
			this.innerGrid.gameObject.SetActive(flag9);
			flag4 = true;
		}
		if (this.innerGrid != null && flag9 && flag3)
		{
			this.innerGrid.Reposition();
		}
		bool flag10 = (!TwitterController.TwitterSupported || !(TwitterController.Instance != null) || !TwitterController.Instance.CanPostStatusUpdateWithPriority(this.twitterPriority) ? false : TwitterController.IsLoggedIn);
		bool flag11 = (!FacebookController.FacebookSupported || !(FacebookController.sharedController != null) || !FacebookController.sharedController.CanPostStoryWithPriority(this.facebookPriority) ? false : FB.IsLoggedIn);
		bool flag12 = (this.HasReward || flag10 || flag11 ? false : !this.CollectOnlyNoShare);
		if (this.continueButton != null && this.continueButton.gameObject.activeSelf != flag12)
		{
			this.continueButton.gameObject.SetActive(flag12);
		}
		bool flag13 = (!this.HasReward || flag10 || flag11 ? this.CollectOnlyNoShare : true);
		if (this.collect != null && this.collect.gameObject.activeSelf != flag13)
		{
			this.collect.gameObject.SetActive(flag13);
		}
		bool flag14 = (this.HasReward || !flag10 && !flag11 ? false : !this.CollectOnlyNoShare);
		if (this.continueAndShare != null && this.continueAndShare.gameObject.activeSelf != flag14)
		{
			this.continueAndShare.gameObject.SetActive(flag14);
		}
		bool flag15 = (!this.HasReward || !flag10 && !flag11 ? false : !this.CollectOnlyNoShare);
		if (this.collectAndShare != null && this.collectAndShare.gameObject.activeSelf != flag15)
		{
			this.collectAndShare.gameObject.SetActive(flag15);
		}
		if (this.containerWidget != null && flag1)
		{
			if (flag2)
			{
				this.containerWidget.height = (int)this.widgetExpanded;
			}
			else if (flag9)
			{
				this.containerWidget.height = (int)this.widgetNoConnectToSocial;
			}
			else if (!flag9)
			{
				this.containerWidget.height = (int)this.widgetCollapsed;
			}
		}
		if (this.containerWidget != null && flag4)
		{
			if (flag9)
			{
				if (!flag2)
				{
					this.containerWidget.height = (int)this.widgetNoConnectToSocial;
				}
				else
				{
					this.containerWidget.height = (int)this.widgetExpanded;
				}
			}
			else if (!flag9)
			{
				this.containerWidget.height = (int)this.widgetCollapsed;
			}
		}
		if (this.hideButton != null)
		{
			this.hideButton.gameObject.SetActive((flag15 ? true : flag14));
		}
	}

	private void SetConnectToSocialText()
	{
		this._timeSinceUpdateConnetToSocialText = Time.realtimeSinceStartup;
		foreach (UILabel uILabel in this.connectToSocial)
		{
			if (uILabel == null)
			{
				continue;
			}
			uILabel.text = string.Format(LocalizationStore.Get("Key_1460"), 10);
		}
	}

	private bool ShouldShowFacebookWithRewardButton()
	{
		return (FB.IsLoggedIn || Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) != 0 || this.CollectOnlyNoShare || !this.ShowLoginsButton ? false : !Device.isPixelGunLow);
	}

	private bool ShouldShowTwitterWithRewardButton()
	{
		return (TwitterController.IsLoggedIn || Storager.getInt(Defs.IsTwitterLoginRewardaGained, true) != 0 || this.CollectOnlyNoShare || !this.ShowLoginsButton ? false : !Device.isPixelGunLow);
	}

	public void ShowAuthorizationFailed()
	{
		this.ShowAuthorizationResultWindow(false);
	}

	private void ShowAuthorizationResultWindow(bool success)
	{
		GameObject vector3 = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>(string.Concat("NguiWindows/", (!success ? "PanelAuthFailed" : "PanelAuthSucces"))));
		vector3.transform.parent = base.transform;
		Player_move_c.SetLayerRecursively(vector3, base.gameObject.layer);
		vector3.transform.localPosition = new Vector3(0f, 0f, -130f);
		vector3.transform.localRotation = Quaternion.identity;
		vector3.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	public void ShowAuthorizationSucceded()
	{
		this.ShowAuthorizationResultWindow(true);
	}

	private void Start()
	{
		this.SetButtonPositionsAndActive();
	}

	public void StartCoinsStarterAnimation()
	{
		base.StartCoroutine(base.transform.parent.GetComponent<LevelUpWithOffers>().CoinsStarterAnimation());
	}

	public void StartGemsStarterAnimation()
	{
		base.StartCoroutine(base.transform.parent.GetComponent<LevelUpWithOffers>().GemsStarterAnimation());
	}

	private void Update()
	{
		this.SetButtonPositionsAndActive();
	}

	public static event Action Shown;
}