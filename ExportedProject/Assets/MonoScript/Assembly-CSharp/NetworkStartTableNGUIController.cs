using Facebook.Unity;
using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class NetworkStartTableNGUIController : MonoBehaviour
{
	public static NetworkStartTableNGUIController sharedController;

	public GameObject facebookButton;

	public GameObject twitterButton;

	public Transform rentScreenPoint;

	public GameObject ranksInterface;

	public RanksTable ranksTable;

	public GameObject shopAnchor;

	public GameObject finishedInterface;

	public UILabel[] finishedInterfaceLabels;

	public GameObject startInterfacePanel;

	public GameObject winnerPanelCom1;

	public GameObject winnerPanelCom2;

	public GameObject endInterfacePanel;

	public Animator interfaceAnimator;

	public GameObject allInterfacePanel;

	public GameObject randomBtn;

	public GameObject socialPnl;

	public GameObject spectratorModePnl;

	public GameObject spectatorModeBtnPnl;

	public GameObject spectatorModeOnBtn;

	public GameObject spectatorModeOffBtn;

	public GameObject MapSelectPanel;

	public string winner;

	public int winnerCommand;

	public UILabel HungerStartLabel;

	private int addCoins;

	private int addExperience;

	private bool isCancelHideAvardPanel;

	private bool updateRealTableAfterActionPanel = true;

	public GameObject SexualButton;

	public GameObject InAppropriateActButton;

	public GameObject OtherButton;

	public GameObject ReasonsPanel;

	public GameObject ActionPanel;

	public GameObject AddButton;

	public GameObject ReportButton;

	public GameObject questsButton;

	public GameObject hideOldRanksButton;

	public GameObject rewardButton;

	public GameObject shopButton;

	public GameObject labelNewItems;

	public UILabel[] actionPanelNicklabel;

	public GameObject trophiAddIcon;

	public GameObject trophiMinusIcon;

	public string pixelbookID;

	public string nick;

	public GoMapInEndGame[] goMapInEndGameButtons = new GoMapInEndGame[3];

	public int CountAddFriens;

	public UILabel[] totalBlue;

	public UILabel[] totalRed;

	private GameObject cameraObj;

	public GameObject changeMapLabel;

	public GameObject rewardPanel;

	public GameObject listOfPlayers;

	public GameObject backButtonInHunger;

	public GameObject goBattleLabel;

	public GameObject daterButtonLabel;

	public UITexture rewardCoinsObject;

	public UITexture rewardExpObject;

	public UISprite rewardTrophysObject;

	public UITexture[] trophyItems;

	public UISprite currentCup;

	public UISprite NewCup;

	public GameObject trophyPanel;

	public GameObject trophyShine;

	public UISprite currentBar;

	public UISprite nextBar;

	public UILabel trophyPoints;

	public Transform rewardCoinsAnimPoint;

	public Transform rewardExpAnimPoint;

	public UILabel[] rewardCoins;

	public UILabel[] rewardExperience;

	public UILabel[] gameModeLabel;

	public UILabel[] rewardTrophy;

	public GameObject[] finishWin;

	public GameObject[] finishDefeat;

	public GameObject[] finishDraw;

	public UILabel teamOneLabel;

	public UILabel teamTwoLabel;

	private Vector3 defaultTeamOneState;

	private Vector3 defaultTeamTwoState;

	public UIToggle shareToggle;

	public UILabel[] textLeagueUp;

	public UILabel[] textLeagueDown;

	public FrameResizer rewardFrame;

	public bool isRewardShow;

	private readonly Lazy<string> _versionString = new Lazy<string>(new Func<string>(() => typeof(NetworkStartTableNGUIController).Assembly.GetName().Version.ToString()));

	private IDisposable _backSubscription;

	private bool waitForAnimationDone;

	private bool leagueUp;

	private int expRewardValue;

	private int coinsRewardValue;

	private int trophyRewardValue;

	private float currentBarFillAmount;

	private float nextBarFillAmount;

	private bool isUsed;

	private bool waitForTrophyAnimationDone;

	private bool oldRanksIsActive;

	private FacebookController.StoryPriority _facebookPriority;

	private FacebookController.StoryPriority _twiiterPriority;

	public Action shareAction;

	public Action customHide;

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

	public FacebookController.StoryPriority faceBookPriority
	{
		set
		{
			this.facebookPriority = value;
			this.twitterPriority = value;
		}
	}

	public RewardWindowBase rewardWindow
	{
		get;
		set;
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

	public NetworkStartTableNGUIController()
	{
	}

	public void AddButtonHandler(object sender, EventArgs e)
	{
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		UnityEngine.Debug.Log(string.Concat("[Add] ", this.pixelbookID));
		this.CountAddFriens++;
		string str = (!Defs.isDaterRegim ? "Multiplayer Battle" : "Sandbox (Dating)");
		Dictionary<string, object> strs = new Dictionary<string, object>()
		{
			{ "Added Friends", str },
			{ "Deleted Friends", "Add" }
		};
		FriendsController.sharedController.SendInvitation(this.pixelbookID, strs);
		if (!FriendsController.sharedController.notShowAddIds.Contains(this.pixelbookID))
		{
			FriendsController.sharedController.notShowAddIds.Add(this.pixelbookID);
		}
		this.AddButton.GetComponent<UIButton>().isEnabled = false;
	}

	private void Awake()
	{
		NetworkStartTableNGUIController.sharedController = this;
	}

	public void BackFromReasonPanel()
	{
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		this.HideReasonPanel();
	}

	public void BackPressFromRanksTable(bool isHideTable = true)
	{
		if (this.CheckHideInternalPanel())
		{
			return;
		}
		this.HideRanksTable(isHideTable);
		this.ReasonsPanel.SetActive(false);
		this.ActionPanel.SetActive(false);
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.BackRanksPressed();
		}
	}

	public void CancelFromActionPanel()
	{
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		this.HideActionPanel();
	}

	public bool CheckHideInternalPanel()
	{
		if (this.ActionPanel.activeInHierarchy)
		{
			this.CancelFromActionPanel();
			return true;
		}
		if (!this.ReasonsPanel.activeInHierarchy)
		{
			return false;
		}
		this.BackFromReasonPanel();
		return true;
	}

	public void EndSpectatorMode()
	{
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.aimPanel.SetActive(false);
		}
		this.spectatorModeOnBtn.SetActive(false);
		this.spectatorModeOffBtn.SetActive(true);
		this.spectratorModePnl.SetActive(false);
		this.MapSelectPanel.SetActive(true);
		if (WeaponManager.sharedManager.myTable != null)
		{
			if (WeaponManager.sharedManager.myNetworkStartTable.currentGameObjectPlayer != null)
			{
				Player_move_c.SetLayerRecursively(WeaponManager.sharedManager.myNetworkStartTable.currentGameObjectPlayer.transform.GetChild(0).gameObject, 0);
			}
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().isRegimVidos = false;
		}
		this.ShowTable(true);
	}

	public void FinishHideOldRanks()
	{
		this.shopButton.GetComponent<Collider>().enabled = true;
		this.interfaceAnimator.SetTrigger("RewardTaken");
		if (this.oldRanksIsActive || Defs.isHunger)
		{
			this.trophyRewardValue = 0;
			this.oldRanksIsActive = false;
			this.questsButton.SetActive(TrainingController.TrainingCompleted);
			this.isRewardShow = false;
			if (Defs.isMulti)
			{
				this.MapSelectPanel.SetActive(true);
			}
			if (Defs.isHunger)
			{
				this.backButtonInHunger.SetActive(true);
				this.randomBtn.SetActive(true);
				this.spectatorModeBtnPnl.SetActive(true);
			}
			else
			{
				this.finishedInterface.SetActive(false);
				this.HideEndInterface();
				this.ShowNewMatchInterface();
				WeaponManager.sharedManager.myNetworkStartTable.ResetOldScore();
			}
			if (WeaponManager.sharedManager.myTable == null)
			{
				base.StartCoroutine(this.WaitAndRemoveInterfaceOnReconnect());
			}
			else
			{
				WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().isShowAvard = false;
			}
		}
	}

	public void HandleEscape()
	{
		if (this.ReasonsPanel != null && this.ReasonsPanel.activeInHierarchy)
		{
			this.BackFromReasonPanel();
		}
		else if (this.ActionPanel != null && this.ActionPanel.activeInHierarchy)
		{
			this.CancelFromActionPanel();
		}
		else if (ShopNGUIController.GuiActive)
		{
			if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myTable != null)
			{
				WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().HandleResumeFromShop();
			}
		}
		else if (this.hideOldRanksButton.activeInHierarchy)
		{
			EventDelegate.Execute(this.hideOldRanksButton.GetComponent<UIButton>().onClick);
		}
	}

	public void HandleHideOldRanksClick()
	{
		if (this.oldRanksIsActive)
		{
			if (this.shareToggle.@value && (this.expRewardValue > 0 || this.coinsRewardValue > 0))
			{
				this.ShareResults();
			}
			this.HideOldRanks();
		}
	}

	public void HideActionPanel()
	{
		this.ActionPanel.SetActive(false);
		this.ShowTable(this.updateRealTableAfterActionPanel);
		if ((Defs.isHunger || Defs.isRegimVidosDebug) && Initializer.players.Count > 0)
		{
			this.spectatorModeBtnPnl.SetActive(Initializer.players.Count != 0);
		}
	}

	[Obfuscation(Exclude=true)]
	public void HideAvardPanel()
	{
		if (this.isCancelHideAvardPanel)
		{
			return;
		}
		this.rewardWindow = null;
		this.ShowEndInterface(this.winner, this.winnerCommand, false);
		if (WeaponManager.sharedManager.myTable == null)
		{
			UnityEngine.Object.Destroy(NetworkStartTableNGUIController.sharedController.gameObject);
		}
		else
		{
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().isShowAvard = false;
		}
		this.isCancelHideAvardPanel = true;
	}

	public void HideEndInterface()
	{
		UnityEngine.Debug.Log("HideEndInterface");
		this.socialPnl.SetActive(false);
		this.allInterfacePanel.SetActive(false);
		this.endInterfacePanel.SetActive(false);
		this.winnerPanelCom1.SetActive(false);
		this.winnerPanelCom2.SetActive(false);
		if (this.defaultTeamOneState == Vector3.zero)
		{
			this.defaultTeamOneState = this.teamOneLabel.transform.localPosition;
		}
		if (this.defaultTeamTwoState == Vector3.zero)
		{
			this.defaultTeamTwoState = this.teamTwoLabel.transform.localPosition;
		}
		this.teamOneLabel.transform.localPosition = this.defaultTeamOneState;
		this.teamTwoLabel.transform.localPosition = this.defaultTeamTwoState;
		this.HideTable();
		this.ReasonsPanel.SetActive(false);
		this.ActionPanel.SetActive(false);
		this.updateRealTableAfterActionPanel = true;
		base.StopCoroutine("TryToShowExpiredBanner");
	}

	public void HideOldRanks()
	{
		if (this.oldRanksIsActive && this.hideOldRanksButton.activeSelf)
		{
			base.CancelInvoke("HideOldRanks");
			this.interfaceAnimator.SetTrigger("OkPressed");
			if (this.expRewardValue > 0 || this.coinsRewardValue > 0 || this.trophyRewardValue != 0)
			{
				this.interfaceAnimator.SetTrigger("GetReward");
			}
			this.hideOldRanksButton.SetActive(false);
		}
	}

	public void HideRanksTable(bool isHideTable = true)
	{
		if (isHideTable)
		{
			this.HideTable();
		}
		this.ranksInterface.SetActive(false);
	}

	public void HideReasonPanel()
	{
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		this.ReasonsPanel.SetActive(false);
		this.ActionPanel.SetActive(true);
	}

	public void HideStartInterface()
	{
		this.isRewardShow = false;
		this.rewardPanel.SetActive(false);
		UnityEngine.Debug.Log("HideStartInterface");
		this.finishedInterface.SetActive(false);
		this.allInterfacePanel.SetActive(false);
		this.startInterfacePanel.SetActive(false);
		this.ReasonsPanel.SetActive(false);
		this.ActionPanel.SetActive(false);
		this.updateRealTableAfterActionPanel = true;
		this.HideTable();
		base.StopCoroutine("TryToShowExpiredBanner");
	}

	public void HideTable()
	{
		this.ranksTable.isShowRanks = false;
		this.ranksTable.tekPanel.SetActive(false);
	}

	public void InAppropriateActButtonHandler(object sender, EventArgs e)
	{
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		FeedbackMenuController.ShowDialogWithCompletion(() => {
			string value = this._versionString.Value;
			string str = string.Concat(new object[] { "mailto:", Defs.SupportMail, "?subject=INAPPROPRIATE ACT ", this.nick, "(", this.pixelbookID, ")&body=%0D%0A%0D%0A%0D%0A%0D%0A%0D%0A------------%20DO NOT DELETE%20------------%0D%0AUTC%20Time:%20", DateTime.Now.ToString(), "%0D%0AGame:%20PixelGun3D%0D%0AVersion:%20", value, "%0D%0APlayerID:%20", FriendsController.sharedController.id, "%0D%0ACategory:%20INAPPROPRIATE ACT ", this.nick, "(", this.pixelbookID, ")%0D%0ADevice%20Type:%20", SystemInfo.deviceType, "%20", SystemInfo.deviceModel, "%0D%0AOS%20Version:%20", SystemInfo.operatingSystem, "%0D%0A------------------------" }).Replace(" ", "%20");
			FlurryPluginWrapper.LogEventWithParameterAndValue("User Feedback", "Menu", "In Game Menu_inappropriate");
			Application.OpenURL(str);
		});
	}

	public static bool IsEndInterfaceShown()
	{
		return (!(NetworkStartTableNGUIController.sharedController != null) || !(NetworkStartTableNGUIController.sharedController.endInterfacePanel != null) ? false : NetworkStartTableNGUIController.sharedController.endInterfacePanel.activeSelf);
	}

	public static bool IsStartInterfaceShown()
	{
		return (!(NetworkStartTableNGUIController.sharedController != null) || !(NetworkStartTableNGUIController.sharedController.startInterfacePanel != null) ? false : NetworkStartTableNGUIController.sharedController.startInterfacePanel.activeSelf);
	}

	[DebuggerHidden]
	public IEnumerator MatchFinishedInterface(string _winner, RatingSystem.RatingChange ratingChange, bool showAward, int _addCoin, int _addExpierence, bool _isCustom, bool firstPlace, bool iAmWinnerInTeam, int _winnerCommand, int blueTotal, int redTotal, bool deadInHunger = false)
	{
		NetworkStartTableNGUIController.u003cMatchFinishedInterfaceu003ec__IteratorC7 variable = null;
		return variable;
	}

	[DebuggerHidden]
	public IEnumerator MatchFinishedInterfaceOld(string _winner, RatingSystem.RatingChange ratingChange, bool showAward, int _addCoin, int _addExpierence, bool _isCustom, bool firstPlace, bool iAmWinnerInTeam, int _winnerCommand, int blueTotal, int redTotal)
	{
		NetworkStartTableNGUIController.u003cMatchFinishedInterfaceOldu003ec__IteratorC8 variable = null;
		return variable;
	}

	public void MathFinishedDeadInHunger()
	{
		if (!this.spectratorModePnl.activeSelf)
		{
			this.spectatorModeOnBtn.SetActive(false);
			this.spectatorModeOffBtn.SetActive(true);
			this.spectratorModePnl.SetActive(false);
		}
		else
		{
			this.EndSpectatorMode();
		}
	}

	private void OnDestroy()
	{
		NetworkStartTableNGUIController.sharedController = null;
	}

	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	private void OnEnable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "Network Start Table GUI");
	}

	public void OnMatchEndAnimationDone()
	{
		this.interfaceAnimator.SetBool("interfaceAnimationDone", true);
	}

	public void OnRewardAnimationEnds()
	{
		this.interfaceAnimator.SetTrigger("AnimationEnds");
	}

	public void OnRewardShow()
	{
		base.StartCoroutine(this.StartRewardAnimation());
	}

	public void OnTablesShow()
	{
		this.waitForAnimationDone = false;
	}

	public void OnTablesShown()
	{
		if (!Defs.isDaterRegim)
		{
			if (!Defs.isHunger || this.expRewardValue > 0 || this.coinsRewardValue > 0)
			{
				base.Invoke("HideOldRanks", 60f);
				this.oldRanksIsActive = true;
				this.hideOldRanksButton.SetActive(true);
				if (Defs.isHunger)
				{
					this.backButtonInHunger.SetActive(false);
					this.randomBtn.SetActive(false);
					this.MapSelectPanel.SetActive(false);
					this.questsButton.SetActive(false);
					this.spectatorModeBtnPnl.SetActive(false);
				}
			}
			else
			{
				this.hideOldRanksButton.SetActive(false);
				this.MapSelectPanel.SetActive(true);
				this.questsButton.SetActive(true);
			}
		}
	}

	public void OnTrophyAnimationDone()
	{
		if (this.isUsed)
		{
			return;
		}
		if (!this.leagueUp)
		{
			this.waitForTrophyAnimationDone = false;
			this.interfaceAnimator.SetBool("isTrophyAdded", true);
		}
		this.isUsed = true;
	}

	public void OnTrophyOkButtonPress()
	{
		this.waitForTrophyAnimationDone = false;
		this.interfaceAnimator.SetBool("isTrophyAdded", true);
	}

	public void OtherButtonHandler(object sender, EventArgs e)
	{
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		FeedbackMenuController.ShowDialogWithCompletion(() => {
			string value = this._versionString.Value;
			string str = string.Concat(new object[] { "mailto:", Defs.SupportMail, "?subject=Report ", this.nick, "(", this.pixelbookID, ")&body=%0D%0A%0D%0A%0D%0A%0D%0A%0D%0A------------%20DO NOT DELETE%20------------%0D%0AUTC%20Time:%20", DateTime.Now.ToString(), "%0D%0AGame:%20PixelGun3D%0D%0AVersion:%20", value, "%0D%0APlayerID:%20", FriendsController.sharedController.id, "%0D%0ACategory:%20Report ", this.nick, "(", this.pixelbookID, ")%0D%0ADevice%20Type:%20", SystemInfo.deviceType, "%20", SystemInfo.deviceModel, "%0D%0AOS%20Version:%20", SystemInfo.operatingSystem, "%0D%0A------------------------" }).Replace(" ", "%20");
			FlurryPluginWrapper.LogEventWithParameterAndValue("User Feedback", "Menu", "In Game Menu_other");
			Application.OpenURL(str);
		});
	}

	public void SexualButtonHandler(object sender, EventArgs e)
	{
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		FeedbackMenuController.ShowDialogWithCompletion(() => {
			string value = this._versionString.Value;
			string str = string.Concat(new object[] { "mailto:", Defs.SupportMail, "?subject=CHEATING ", this.nick, "(", this.pixelbookID, ")&body=%0D%0A%0D%0A%0D%0A%0D%0A%0D%0A------------%20DO NOT DELETE%20------------%0D%0AUTC%20Time:%20", DateTime.Now.ToString(), "%0D%0AGame:%20PixelGun3D%0D%0AVersion:%20", value, "%0D%0APlayerID:%20", FriendsController.sharedController.id, "%0D%0ACategory:%20CHEATING ", this.nick, "(", this.pixelbookID, ")%0D%0ADevice%20Type:%20", SystemInfo.deviceType, "%20", SystemInfo.deviceModel, "%0D%0AOS%20Version:%20", SystemInfo.operatingSystem, "%0D%0A------------------------" }).Replace(" ", "%20");
			FlurryPluginWrapper.LogEventWithParameterAndValue("User Feedback", "Menu", "In Game Menu_cheater");
			Application.OpenURL(str);
		});
	}

	private void ShareResults()
	{
		Dictionary<string, object> strs;
		this.faceBookPriority = FacebookController.StoryPriority.Red;
		this.twitterPriority = FacebookController.StoryPriority.MultyWinLimit;
		this.twitterStatus = () => "I've won a battle in @PixelGun3D! Join the fight now! #pixelgun3d #pixelgun #3d #pg3d #mobile #fps #shooter http://goo.gl/8fzL9u";
		this.EventTitle = "Won Batlle";
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
			FacebookController.StoryPriority storyPriority = this.facebookPriority;
			Dictionary<string, string> strs1 = new Dictionary<string, string>()
			{
				{ "battle", ConnectSceneNGUIController.regim.ToString().ToLower() }
			};
			FacebookController.PostOpenGraphStory("win", "battle", storyPriority, strs1);
			strs = new Dictionary<string, object>()
			{
				{ "Post Facebook", this.EventTitle },
				{ "Total Facebook", "Posts" }
			};
			AnalyticsFacade.SendCustomEvent("Virality", strs);
		}
	}

	public void ShowActionPanel(string _pixelbookID, string _nick)
	{
		this.pixelbookID = _pixelbookID;
		this.nick = _nick;
		this.HideTable();
		for (int i = 0; i < (int)this.actionPanelNicklabel.Length; i++)
		{
			this.actionPanelNicklabel[i].text = this.nick;
		}
		this.ActionPanel.SetActive(true);
		this.spectatorModeBtnPnl.SetActive(false);
		if (!FriendsController.sharedController.IsShowAdd(this.pixelbookID) || this.CountAddFriens >= 3)
		{
			this.AddButton.GetComponent<UIButton>().isEnabled = false;
		}
		else
		{
			this.AddButton.GetComponent<UIButton>().isEnabled = true;
		}
	}

	[Obsolete]
	public void showAvardPanel(string _winner, int _addCoin, int _addExpierence, bool _isCustom, bool firstPlace, int _winnerCommand)
	{
		this.isCancelHideAvardPanel = false;
		if (!_isCustom)
		{
			this.addCoins = _addCoin;
			this.addExperience = _addExpierence;
		}
		else
		{
			this.addCoins = 0;
			this.addExperience = 0;
		}
		string str = string.Format("+ {0} {1}", _addCoin, LocalizationStore.Key_0275);
		string str1 = string.Format("+ {0} {1}", _addExpierence, LocalizationStore.Key_0204);
		ConnectSceneNGUIController.RegimGame regimGame = ConnectSceneNGUIController.regim;
		PremiumAccountController instance = PremiumAccountController.Instance;
		bool flag = (!PromoActionsManager.sharedManager.IsDayOfValorEventActive ? false : (regimGame == ConnectSceneNGUIController.RegimGame.Deathmatch || regimGame == ConnectSceneNGUIController.RegimGame.FlagCapture || regimGame == ConnectSceneNGUIController.RegimGame.TeamFight ? true : regimGame == ConnectSceneNGUIController.RegimGame.CapturePoints));
		bool flag1 = instance.IsActiveOrWasActiveBeforeStartMatch();
		int num = 1;
		int num1 = 1;
		if (flag1 || flag)
		{
			num = (Defs.isCOOP || Defs.isHunger ? PremiumAccountController.Instance.RewardCoeff : AdminSettingsController.GetMultiplyerRewardWithBoostEvent(false));
			num1 = (Defs.isCOOP || Defs.isHunger ? PremiumAccountController.Instance.RewardCoeff : AdminSettingsController.GetMultiplyerRewardWithBoostEvent(true));
		}
		this.rewardWindow = NetworkStartTableNGUIController.ShowRewardWindow(firstPlace, NetworkStartTableNGUIController.sharedController.allInterfacePanel.transform.parent);
		this.rewardWindow.customHide = () => {
			this.rewardWindow.CancelInvoke("Hide");
			this.HideAvardPanel();
		};
		RewardWindowAfterMatch component = this.rewardWindow.GetComponent<RewardWindowAfterMatch>();
		component.victory.SetActive(true);
		component.lose.SetActive(false);
		if (flag1 && flag)
		{
			component.daysAndPremiumBack.SetActive(true);
			component.premiumBackground.SetActive(false);
			component.daysOfValorBackground.SetActive(false);
			component.normlaBeckground.SetActive(false);
		}
		else if (flag1)
		{
			component.daysAndPremiumBack.SetActive(false);
			component.premiumBackground.SetActive(true);
			component.daysOfValorBackground.SetActive(false);
			component.normlaBeckground.SetActive(false);
		}
		else if (!flag)
		{
			component.daysAndPremiumBack.SetActive(false);
			component.premiumBackground.SetActive(false);
			component.daysOfValorBackground.SetActive(false);
			component.normlaBeckground.SetActive(true);
		}
		else
		{
			component.daysAndPremiumBack.SetActive(false);
			component.premiumBackground.SetActive(false);
			component.daysOfValorBackground.SetActive(true);
			component.normlaBeckground.SetActive(false);
		}
		component.coinsMultiplierContainer.SetActive((num1 <= 1 ? false : _addCoin > 0));
		component.coinsMultiplier.text = string.Concat("x", num1.ToString());
		component.expMultiplierContainer.SetActive(num > 1);
		component.expMilyiplier.text = string.Concat("x", num.ToString());
		foreach (UILabel coin in component.coins)
		{
			coin.text = str;
		}
		foreach (UILabel uILabel in component.exp)
		{
			uILabel.text = str1;
		}
		if (_addCoin == 0)
		{
			component.coinsContainer.SetActive(false);
			Transform vector3 = component.expContainer.transform;
			float single = component.expContainer.transform.localPosition.y;
			Vector3 vector31 = component.expContainer.transform.localPosition;
			vector3.localPosition = new Vector3(0f, single, vector31.z);
		}
		this.endInterfacePanel.SetActive(true);
		this.finishedInterface.SetActive(false);
		this.MapSelectPanel.SetActive(false);
		this.socialPnl.SetActive(BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64);
		this.winnerCommand = _winnerCommand;
		this.winner = _winner;
		!Defs.isDaterRegim;
		if (this.addExperience > 0)
		{
			ExperienceController.sharedController.addExperience(this.addExperience);
		}
		if (this.addCoins > 0)
		{
			int num2 = Storager.getInt("Coins", false);
			Storager.setInt("Coins", num2 + this.addCoins, false);
			AnalyticsFacade.CurrencyAccrual(this.addCoins, "Coins", AnalyticsConstants.AccrualType.Earned);
			FlurryEvents.LogCoinsGained(FlurryEvents.GetPlayingMode(), this.addCoins);
		}
	}

	public void ShowAwardEndInterface(string _winner, int _addCoin, int _addExpierence, bool _isCustom, bool firstPlace, int _winnerCommand)
	{
		if (!_isCustom)
		{
			this.addCoins = _addCoin;
			this.addExperience = _addExpierence;
		}
		else
		{
			this.addCoins = 0;
			this.addExperience = 0;
		}
		string str = string.Format("+{0} {1}", _addCoin, LocalizationStore.Key_0275);
		string str1 = string.Format("+{0} {1}", _addExpierence, LocalizationStore.Key_0204);
		ConnectSceneNGUIController.RegimGame regimGame = ConnectSceneNGUIController.regim;
		PremiumAccountController instance = PremiumAccountController.Instance;
		bool flag = (!PromoActionsManager.sharedManager.IsDayOfValorEventActive ? false : (regimGame == ConnectSceneNGUIController.RegimGame.Deathmatch || regimGame == ConnectSceneNGUIController.RegimGame.FlagCapture || regimGame == ConnectSceneNGUIController.RegimGame.TeamFight ? true : regimGame == ConnectSceneNGUIController.RegimGame.CapturePoints));
		bool flag1 = instance.IsActiveOrWasActiveBeforeStartMatch();
		int num = 1;
		int num1 = 1;
		if (flag1 || flag)
		{
			num = (Defs.isCOOP || Defs.isHunger ? PremiumAccountController.Instance.RewardCoeff : AdminSettingsController.GetMultiplyerRewardWithBoostEvent(false));
			num1 = (Defs.isCOOP || Defs.isHunger ? PremiumAccountController.Instance.RewardCoeff : AdminSettingsController.GetMultiplyerRewardWithBoostEvent(true));
		}
		if (!flag1 || !flag)
		{
			if (!flag1)
			{
				if (flag)
				{
				}
			}
		}
		UILabel[] uILabelArray = this.rewardCoins;
		for (int i = 0; i < (int)uILabelArray.Length; i++)
		{
			uILabelArray[i].text = str;
		}
		UILabel[] uILabelArray1 = this.rewardExperience;
		for (int j = 0; j < (int)uILabelArray1.Length; j++)
		{
			uILabelArray1[j].text = str1;
		}
		!Defs.isDaterRegim;
		this.ShowEndInterface(_winner, _winnerCommand, false);
	}

	public void ShowEndInterface(string _winner, int _winnerCommand, bool deadInHunger = false)
	{
		if (!ShopNGUIController.NoviceArmorAvailable)
		{
			base.GetComponent<HintController>().HideHintByName("shop_remove_novice_armor");
		}
		NotificationController.instance.SaveTimeValues();
		if (!FriendsController.useBuffSystem)
		{
			KillRateCheck.instance.CheckKillRate();
		}
		else
		{
			BuffSystem.instance.EndRound();
		}
		WeaponManager.sharedManager.myNetworkStartTable.ClearKillrate();
		if (Defs.isCompany || Defs.isFlag || Defs.isCapturePoints)
		{
			this.winnerPanelCom1.SetActive(_winnerCommand == 1);
			this.winnerPanelCom2.SetActive(_winnerCommand == 2);
		}
		this.startInterfacePanel.SetActive(Defs.isDaterRegim);
		this.endInterfacePanel.SetActive(!Defs.isDaterRegim);
		this.goBattleLabel.SetActive(!Defs.isDaterRegim);
		this.daterButtonLabel.SetActive(Defs.isDaterRegim);
		this.backButtonInHunger.SetActive(Defs.isHunger);
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.aimPanel.SetActive(false);
		}
		this.socialPnl.SetActive(BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64);
		this.winner = _winner;
		this.allInterfacePanel.SetActive(true);
		this.ranksTable.UpdateRanksFromOldSpisok();
		if (Defs.isHunger || Defs.isRegimVidosDebug)
		{
			if (Defs.isHunger)
			{
				this.randomBtn.SetActive(true);
				this.questsButton.SetActive(true);
			}
			this.spectatorModeBtnPnl.SetActive(true);
			this.updateRealTableAfterActionPanel = deadInHunger;
			if (!this.ActionPanel.activeSelf && !this.ReasonsPanel.activeSelf)
			{
				this.ShowTable(deadInHunger);
			}
		}
		else
		{
			this.updateRealTableAfterActionPanel = false;
			this.ShowTable(false);
			this.MapSelectPanel.SetActive(false);
			this.questsButton.SetActive(false);
		}
		base.StartCoroutine("TryToShowExpiredBanner");
	}

	public void ShowEndInterfaceDeadInHunger(string _winner, RatingSystem.RatingChange ratingChange)
	{
		base.StartCoroutine(this.MatchFinishedInterface(_winner, ratingChange, false, 0, 0, false, false, false, 0, 0, 0, true));
	}

	public void ShowFinishedInterface(bool isWinner, bool deadHeat)
	{
		string str;
		bool flag = (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture ? true : ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints);
		this.finishedInterface.SetActive(true);
		if (Defs.isDaterRegim)
		{
			str = "Key_1987";
		}
		else if (deadHeat)
		{
			str = "Key_0571";
		}
		else if (!isWinner)
		{
			str = (!flag ? "Key_1976" : "Key_1116");
		}
		else
		{
			str = "Key_1115";
		}
		string str1 = LocalizationStore.Get(str);
		GameObject[] gameObjectArray = this.finishDraw;
		for (int i = 0; i < (int)gameObjectArray.Length; i++)
		{
			gameObjectArray[i].SetActive((deadHeat || !isWinner && !flag ? !Defs.isDaterRegim : false));
		}
		GameObject[] gameObjectArray1 = this.finishWin;
		for (int j = 0; j < (int)gameObjectArray1.Length; j++)
		{
			gameObjectArray1[j].SetActive((!isWinner || deadHeat ? false : !Defs.isDaterRegim));
		}
		GameObject[] gameObjectArray2 = this.finishDefeat;
		for (int k = 0; k < (int)gameObjectArray2.Length; k++)
		{
			gameObjectArray2[k].SetActive((!flag || isWinner || deadHeat ? false : !Defs.isDaterRegim));
		}
		for (int l = 0; l < (int)this.finishedInterfaceLabels.Length; l++)
		{
			this.finishedInterfaceLabels[l].text = str1;
		}
	}

	public void ShowNewMatchInterface()
	{
		this.isRewardShow = false;
		this.rewardPanel.SetActive(false);
		this.allInterfacePanel.SetActive(true);
		this.startInterfacePanel.SetActive(true);
		this.ShowTable(true);
	}

	public void ShowRanksTable()
	{
		this.ShowTable(true);
		this.ranksInterface.SetActive(true);
	}

	public void ShowReasonPanel(object sender, EventArgs e)
	{
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		UnityEngine.Debug.Log("ShowReasonPanel");
		this.ReasonsPanel.SetActive(true);
		this.ActionPanel.SetActive(false);
	}

	public static RewardWindowBase ShowRewardWindow(bool win, Transform par)
	{
		GameObject vector3 = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("NguiWindows/WinWindowNGUI"));
		RewardWindowBase component = vector3.GetComponent<RewardWindowBase>();
		FacebookController.StoryPriority storyPriority = FacebookController.StoryPriority.Red;
		component.priority = storyPriority;
		component.twitterPriority = FacebookController.StoryPriority.MultyWinLimit;
		component.shareAction = () => FacebookController.PostOpenGraphStory("win", "battle", storyPriority, new Dictionary<string, string>()
		{
			{ "battle", ConnectSceneNGUIController.regim.ToString().ToLower() }
		});
		component.HasReward = true;
		component.CollectOnlyNoShare = !win;
		component.twitterStatus = () => "I've won a battle in @PixelGun3D! Join the fight now! #pixelgun3d #pixelgun #3d #pg3d #mobile #fps #shooter http://goo.gl/8fzL9u";
		component.EventTitle = "Won Batlle";
		vector3.transform.parent = par;
		Player_move_c.SetLayerRecursively(vector3, LayerMask.NameToLayer("NGUITable"));
		vector3.transform.localPosition = new Vector3(0f, 0f, -130f);
		vector3.transform.localRotation = Quaternion.identity;
		vector3.transform.localScale = new Vector3(1f, 1f, 1f);
		return component;
	}

	public void ShowStartInterface()
	{
		string str;
		if (Defs.isDaterRegim)
		{
			UILabel[] uILabelArray = this.gameModeLabel;
			for (int i = 0; i < (int)uILabelArray.Length; i++)
			{
				uILabelArray[i].text = LocalizationStore.Get("Key_1567");
			}
		}
		else if (ConnectSceneNGUIController.gameModesLocalizeKey.TryGetValue(Convert.ToInt32(ConnectSceneNGUIController.regim).ToString(), out str))
		{
			UILabel[] uILabelArray1 = this.gameModeLabel;
			for (int j = 0; j < (int)uILabelArray1.Length; j++)
			{
				uILabelArray1[j].text = LocalizationStore.Get(str);
			}
		}
		this.questsButton.SetActive((!TrainingController.TrainingCompleted ? false : !Defs.isHunger));
		this.MapSelectPanel.SetActive(false);
		this.goBattleLabel.SetActive(!Defs.isDaterRegim);
		this.daterButtonLabel.SetActive(Defs.isDaterRegim);
		this.allInterfacePanel.SetActive(true);
		this.startInterfacePanel.SetActive(true);
		this.rewardPanel.SetActive(false);
		this.isRewardShow = false;
		this.ShowTable(true);
		base.StartCoroutine("TryToShowExpiredBanner");
	}

	private void ShowTable(bool _isRealUpdate = true)
	{
		this.ranksTable.isShowRanks = _isRealUpdate;
		this.ranksTable.tekPanel.SetActive(true);
	}

	private void Start()
	{
		if (BuffSystem.instance != null && !BuffSystem.instance.haveAllInteractons && Storager.getInt("Training.ShouldRemoveNoviceArmorInShopKey", false) == 1 && HintController.instance != null)
		{
			HintController.instance.ShowHintByName("shop_remove_novice_armor", 0f);
		}
		this.cameraObj = base.transform.GetChild(0).gameObject;
		if (this.SexualButton != null)
		{
			ButtonHandler component = this.SexualButton.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += new EventHandler(this.SexualButtonHandler);
			}
		}
		if (this.InAppropriateActButton != null)
		{
			ButtonHandler buttonHandler = this.InAppropriateActButton.GetComponent<ButtonHandler>();
			if (buttonHandler != null)
			{
				buttonHandler.Clicked += new EventHandler(this.InAppropriateActButtonHandler);
			}
		}
		if (this.OtherButton != null)
		{
			ButtonHandler component1 = this.OtherButton.GetComponent<ButtonHandler>();
			if (component1 != null)
			{
				component1.Clicked += new EventHandler(this.OtherButtonHandler);
			}
		}
		if (this.ReportButton != null)
		{
			ButtonHandler buttonHandler1 = this.ReportButton.GetComponent<ButtonHandler>();
			if (buttonHandler1 != null)
			{
				buttonHandler1.Clicked += new EventHandler(this.ShowReasonPanel);
			}
		}
		if (this.AddButton != null)
		{
			ButtonHandler component2 = this.AddButton.GetComponent<ButtonHandler>();
			if (component2 != null)
			{
				component2.Clicked += new EventHandler(this.AddButtonHandler);
			}
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
		{
			Transform transforms = this.listOfPlayers.transform;
			transforms.localPosition = transforms.localPosition - (50f * Vector3.up);
			if (NetworkStartTable.LocalOrPasswordRoom())
			{
				Transform mapSelectPanel = this.MapSelectPanel.transform;
				mapSelectPanel.localPosition = mapSelectPanel.localPosition + (80f * Vector3.up);
			}
		}
	}

	[DebuggerHidden]
	public IEnumerator StartRewardAnimation()
	{
		NetworkStartTableNGUIController.u003cStartRewardAnimationu003ec__IteratorC9 variable = null;
		return variable;
	}

	public void StartSpectatorMode()
	{
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.aimPanel.SetActive(true);
		}
		this.spectatorModeOnBtn.SetActive(true);
		this.spectatorModeOffBtn.SetActive(false);
		this.spectratorModePnl.SetActive(true);
		this.socialPnl.SetActive(false);
		this.MapSelectPanel.SetActive(false);
		this.HideTable();
		if (WeaponManager.sharedManager.myTable != null)
		{
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().isRegimVidos = true;
		}
	}

	public void StartTrophyAnim()
	{
		base.StartCoroutine(this.TrophyFillAnimation());
	}

	[DebuggerHidden]
	private IEnumerator TrophyFillAnimation()
	{
		NetworkStartTableNGUIController.u003cTrophyFillAnimationu003ec__IteratorCA variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator TryToShowExpiredBanner()
	{
		NetworkStartTableNGUIController.u003cTryToShowExpiredBanneru003ec__IteratorCC variable = null;
		return variable;
	}

	private void Update()
	{
		if (ExpController.Instance != null && ExpController.Instance.experienceView != null)
		{
			bool levelUpPanelOpened = ExpController.Instance.experienceView.LevelUpPanelOpened;
			if (this.cameraObj.activeSelf == levelUpPanelOpened)
			{
				this.cameraObj.SetActive(!levelUpPanelOpened);
			}
		}
		if ((Defs.isHunger || Defs.isRegimVidosDebug) && this.spectatorModeBtnPnl.activeSelf && Initializer.players.Count == 0)
		{
			this.spectatorModeBtnPnl.SetActive(false);
			this.spectratorModePnl.SetActive(false);
			this.ShowTable(false);
		}
		this.facebookButton.SetActive((!FacebookController.FacebookSupported || !FacebookController.FacebookPost_Old_Supported ? false : FB.IsLoggedIn));
		this.twitterButton.SetActive((!TwitterController.TwitterSupported || !TwitterController.TwitterSupported_OldPosts ? false : TwitterController.IsLoggedIn));
		bool flag = (this.facebookButton.activeSelf ? true : this.twitterButton.activeSelf);
		if (this.socialPnl.activeSelf != flag)
		{
			this.socialPnl.SetActive(flag);
		}
	}

	public void UpdateGoMapButtons(bool show = true)
	{
		bool flag = (!show ? true : ConnectSceneNGUIController.gameTier != ExpController.Instance.OurTier);
		for (int i = 0; i < (int)this.goMapInEndGameButtons.Length; i++)
		{
			this.goMapInEndGameButtons[i].gameObject.SetActive(!flag);
		}
		this.changeMapLabel.SetActive(!flag);
		if (flag)
		{
			return;
		}
		AllScenesForMode listScenesForMode = SceneInfoController.instance.GetListScenesForMode(ConnectSceneNGUIController.curSelectMode);
		SceneInfo[] sceneInfoArray = new SceneInfo[(int)this.goMapInEndGameButtons.Length];
		for (int j = 0; j < (int)sceneInfoArray.Length; j++)
		{
			int num = 0;
			bool flag1 = true;
			int num1 = UnityEngine.Random.Range(0, listScenesForMode.avaliableScenes.Count);
			while (flag1)
			{
				flag1 = false;
				SceneInfo item = listScenesForMode.avaliableScenes[num1];
				int num2 = 0;
				while (num2 < j)
				{
					if (sceneInfoArray[num2] != item)
					{
						num2++;
					}
					else
					{
						flag1 = true;
						break;
					}
				}
				if (!flag1 && (item.NameScene.Equals(Application.loadedLevelName) || item.AvaliableWeapon == ModeWeapon.dater || item.isPremium && Storager.getInt(string.Concat(item.NameScene, "Key"), true) == 0 && !PremiumAccountController.MapAvailableDueToPremiumAccount(item.NameScene)))
				{
					flag1 = true;
				}
				if (flag1)
				{
					num1++;
					num++;
					if (num1 > listScenesForMode.avaliableScenes.Count - 1)
					{
						num1 = 0;
					}
				}
				else
				{
					sceneInfoArray[j] = item;
				}
				if (num <= listScenesForMode.avaliableScenes.Count)
				{
					continue;
				}
				UnityEngine.Debug.LogWarning("no map");
				break;
			}
			this.goMapInEndGameButtons[j].SetMap(sceneInfoArray[j]);
		}
	}

	[DebuggerHidden]
	private IEnumerator WaitAndRemoveInterfaceOnReconnect()
	{
		return new NetworkStartTableNGUIController.u003cWaitAndRemoveInterfaceOnReconnectu003ec__IteratorCB();
	}
}