using Rilisoft;
using Rilisoft.NullExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class ClansGUIController : MonoBehaviour, IFriendsGUIController
{
	private const string ShownCreateClanRewardWindow = "ShownCreateClanRewardWindowKey";

	public static ClansGUIController sharedController;

	public static bool AtAddPanel;

	public static bool AtStatsPanel;

	public GameObject rewardCreateClanWindow;

	public UIWrapContent friendsGrid;

	public UIGrid addFriendsGrid;

	public JoinRoomFromFrends joinRoomFromFrends;

	public bool InClan;

	public GameObject NoClanPanel;

	public GameObject CreateClanPanel;

	public GameObject CreateClanButton;

	public GameObject EditLogoBut;

	public GameObject BackBut;

	public GameObject Left;

	public GameObject Right;

	public GameObject ClanName;

	public GameObject clanPanel;

	public GameObject editLogoInPreviewButton;

	public GameObject leaveButton;

	public GameObject addMembersButton;

	public GameObject noMembersLabel;

	public GameObject statisticsButton;

	public GameObject statisiticPanel;

	public GameObject deleteClanButton;

	public GameObject startPanel;

	public GameObject addInClanPanel;

	public GameObject NameIsUsedPanel;

	public GameObject CheckConnectionPanel;

	public GameObject topLevelObject;

	public UITexture logo;

	public UITexture previewLogo;

	public UILabel nameClanLabel;

	public UILabel countMembersLabel;

	public UILabel inputNameClanLabel;

	public UILabel tapToEdit;

	public UILabel clanIsFull;

	public UILabel changeClanResult;

	public GameObject receivingPlashka;

	public GameObject deleteClanDialog;

	public UIButton yesDelteClan;

	public UIButton noDeleteClan;

	public UIInput changeClanNameInput;

	private bool BlockGUI;

	private List<Texture2D> _logos = new List<Texture2D>();

	private int _currentLogoInd;

	private bool _inCoinsShop;

	private float timeOfLastSort;

	private readonly Lazy<UISprite[]> _newMessagesOverlays;

	private readonly Lazy<ClanIncomingInvitesController> _clanIncomingInvitesController;

	private FriendProfileController _friendProfileController;

	private IDisposable _backSubscription;

	public static bool ShowProfile;

	private bool _isCancellationRequested;

	private float _defendTime;

	internal ClansGUIController.State CurrentState
	{
		get;
		set;
	}

	static ClansGUIController()
	{
	}

	public ClansGUIController()
	{
		this._clanIncomingInvitesController = new Lazy<ClanIncomingInvitesController>(() => base.gameObject.GetComponent<ClanIncomingInvitesController>());
		this._newMessagesOverlays = new Lazy<UISprite[]>(() => (
			from s in this.clanPanel.Map<GameObject, UISprite[]>((GameObject c) => c.GetComponentsInChildren<UISprite>(true), new UISprite[0]).Concat<UISprite>(this.NoClanPanel.Map<GameObject, UISprite[]>((GameObject c) => c.GetComponentsInChildren<UISprite>(true), new UISprite[0]))
			where "NewMessages".Equals(s.name)
			select s).ToArray<UISprite>());
	}

	[DebuggerHidden]
	private IEnumerator __UpdateGUI()
	{
		ClansGUIController.u003c__UpdateGUIu003ec__Iterator12 variable = null;
		return variable;
	}

	private void _SortFriendPreviews()
	{
		FriendPreview[] componentsInChildren = this.friendsGrid.GetComponentsInChildren<FriendPreview>(true);
		FriendPreview[] friendPreviewArray = this.friendsGrid.GetComponentsInChildren<FriendPreview>(false) ?? new FriendPreview[0];
		StringComparer ordinal = StringComparer.Ordinal;
		Array.Sort<FriendPreview>(friendPreviewArray, (FriendPreview fp1, FriendPreview fp2) => ordinal.Compare(fp1.name, fp2.name));
		string str2 = null;
		float single = 0f;
		if ((int)friendPreviewArray.Length > 0)
		{
			str2 = friendPreviewArray[0].gameObject.name;
			Transform transforms = this.friendsGrid.transform.parent;
			if (transforms != null)
			{
				UIPanel component = transforms.GetComponent<UIPanel>();
				if (component != null)
				{
					Vector3 vector3 = friendPreviewArray[0].transform.localPosition;
					single = vector3.x - component.clipOffset.x;
				}
			}
		}
		Array.Sort<FriendPreview>(componentsInChildren, (FriendPreview fp1, FriendPreview fp2) => {
			int num;
			int num1;
			int num2;
			int num3;
			if (fp1.id == null || !FriendsController.sharedController.onlineInfo.ContainsKey(fp1.id))
			{
				return 1;
			}
			if (fp2.id == null || !FriendsController.sharedController.onlineInfo.ContainsKey(fp2.id))
			{
				return -1;
			}
			string item = FriendsController.sharedController.onlineInfo[fp1.id]["delta"];
			string str = FriendsController.sharedController.onlineInfo[fp1.id]["game_mode"];
			int num4 = int.Parse(item);
			int num5 = int.Parse(str);
			num = ((float)num4 > FriendsController.onlineDelta || num5 > 99 && num5 / 100 != (int)ConnectSceneNGUIController.myPlatformConnect && num5 / 100 != 3 ? 2 : (num5 != -1 ? 0 : 1));
			if (FriendsController.sharedController.clanLeaderID != null && fp1.id.Equals(FriendsController.sharedController.clanLeaderID))
			{
				num = -1;
			}
			string item1 = FriendsController.sharedController.onlineInfo[fp2.id]["delta"];
			string str1 = FriendsController.sharedController.onlineInfo[fp2.id]["game_mode"];
			int num6 = int.Parse(item1);
			int num7 = int.Parse(str1);
			num1 = ((float)num6 > FriendsController.onlineDelta || num7 > 99 && num7 / 100 != (int)ConnectSceneNGUIController.myPlatformConnect && num7 / 100 != 3 ? 2 : (num7 <= -1 ? 1 : 0));
			if (FriendsController.sharedController.clanLeaderID != null && fp2.id.Equals(FriendsController.sharedController.clanLeaderID))
			{
				num1 = -1;
			}
			if (num == num1 && int.TryParse(fp1.id, out num2) && int.TryParse(fp2.id, out num3))
			{
				return num2 - num3;
			}
			return num - num1;
		});
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.name = i.ToString("D7");
		}
		this.friendsGrid.SortAlphabetically();
		this.friendsGrid.WrapContent();
		Transform transforms1 = null;
		if (str2 != null)
		{
			FriendPreview[] friendPreviewArray1 = componentsInChildren;
			int num8 = 0;
			while (num8 < (int)friendPreviewArray1.Length)
			{
				FriendPreview friendPreview = friendPreviewArray1[num8];
				if (!friendPreview.name.Equals(str2))
				{
					num8++;
				}
				else
				{
					transforms1 = friendPreview.transform;
					break;
				}
			}
		}
		if (transforms1 == null && (int)componentsInChildren.Length > 0 && this.friendsGrid.gameObject.activeInHierarchy)
		{
			transforms1 = componentsInChildren[0].transform;
		}
		if (transforms1 != null)
		{
			float single1 = transforms1.localPosition.x - single;
			Transform vector31 = this.friendsGrid.transform.parent;
			if (vector31 != null)
			{
				UIPanel vector2 = vector31.GetComponent<UIPanel>();
				if (vector2 != null)
				{
					vector2.clipOffset = new Vector2(single1, vector2.clipOffset.y);
					float single2 = vector31.localPosition.y;
					Vector3 vector32 = vector31.localPosition;
					vector31.localPosition = new Vector3(-single1, single2, vector32.z);
				}
			}
		}
		this.friendsGrid.WrapContent();
	}

	public void BuyNewClan()
	{
		int clansPrice = Defs.ClansPrice;
		int num = Storager.getInt("Coins", false);
		Storager.setInt("Coins", num - clansPrice, false);
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
		this.CreateClanPanel.SetActive(false);
		this.InClan = true;
		this.ShowClanPanel();
	}

	public void ChangeClanName()
	{
		string empty = FriendsController.sharedController.clanName ?? string.Empty;
		if (!string.IsNullOrEmpty(this.nameClanLabel.text))
		{
			FriendsController.sharedController.ChangeClanName(this.nameClanLabel.text, () => {
				FriendsController.sharedController.clanName = this.nameClanLabel.text;
				this.BlockGUI = false;
			}, (string error) => {
				this.nameClanLabel.text = empty;
				UnityEngine.Debug.Log(string.Concat("error ", error));
				if (string.IsNullOrEmpty(error))
				{
					this.BlockGUI = false;
				}
				else if (!error.Equals("fail"))
				{
					base.StartCoroutine(this.ShowCheckConnection());
				}
				else
				{
					base.StartCoroutine(this.ShowThisNameInUse());
				}
			});
		}
		else
		{
			this.nameClanLabel.text = empty;
			base.StartCoroutine(this.ShowThisNameInUse());
		}
		this.BlockGUI = true;
	}

	private void DisableStatisticsPanel(bool disable)
	{
		this.BackBut.GetComponent<UIButton>().isEnabled = !disable;
		this.deleteClanButton.GetComponent<UIButton>().isEnabled = !disable;
	}

	private void ErrorHandler(string error)
	{
		FriendsController.sharedController.FailedSendNewClan -= new Action(this.FailedSendBuyClan);
		FriendsController.sharedController.ReturnNewIDClan -= new Action<int>(this.ReturnIDNewClan);
		this.BlockGUI = false;
	}

	public void FailedSendBuyClan()
	{
		FriendsController.sharedController.FailedSendNewClan -= new Action(this.FailedSendBuyClan);
		FriendsController.sharedController.ReturnNewIDClan -= new Action<int>(this.ReturnIDNewClan);
	}

	[DebuggerHidden]
	private IEnumerator FillAddMembers()
	{
		ClansGUIController.u003cFillAddMembersu003ec__Iterator15 variable = null;
		return variable;
	}

	public void GoToSM()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("SkinEditorController"));
		if (gameObject.GetComponent<SkinEditorController>() != null)
		{
			Action<string> action = null;
			action = (string name) => {
				MenuBackgroundMusic.sharedMusic.StopCustomMusicFrom(SkinEditorController.sharedController.gameObject);
				SkinEditorController.ExitFromSkinEditor -= this.backHandler;
				this.u003cu003ef__this.logo.mainTexture = EditorTextures.CreateCopyTexture(SkinsController.logoClanUserTexture);
				if (this.u003cu003ef__this.InClan)
				{
					UnityEngine.Debug.Log("InClan");
					byte[] pNG = SkinsController.logoClanUserTexture.EncodeToPNG();
					FriendsController.sharedController.clanLogo = Convert.ToBase64String(pNG);
					FriendsController.sharedController.ChangeClanLogo();
					this.u003cu003ef__this.previewLogo.mainTexture = EditorTextures.CreateCopyTexture(SkinsController.logoClanUserTexture);
				}
				else if (!string.IsNullOrEmpty(name))
				{
					this.u003cu003ef__this._logos.Add(this.u003cu003ef__this.logo.mainTexture as Texture2D);
					this.u003cu003ef__this._currentLogoInd = this.u003cu003ef__this._logos.Count - 1;
				}
				this.u003cu003ef__this.gameObject.SetActive(true);
			};
			SkinEditorController.ExitFromSkinEditor += action;
			if (!this.InClan)
			{
				SkinsController.logoClanUserTexture = EditorTextures.CreateCopyTexture((Texture2D)this.logo.mainTexture);
			}
			else
			{
				SkinsController.logoClanUserTexture = EditorTextures.CreateCopyTexture((Texture2D)this.previewLogo.mainTexture);
			}
			SkinEditorController.modeEditor = SkinEditorController.ModeEditor.LogoClan;
			SkinEditorController.currentSkin = EditorTextures.CreateCopyTexture((Texture2D)this.logo.mainTexture);
			gameObject.transform.parent = null;
			base.gameObject.SetActive(false);
		}
	}

	private void HandleAddMembersClicked(object sender, EventArgs e)
	{
		this.ShowAddMembersScreen();
	}

	private void HandleArrowClicked(object sender, EventArgs e)
	{
		if ((sender as ButtonHandler).gameObject != this.Left)
		{
			this._currentLogoInd++;
			if (this._currentLogoInd >= this._logos.Count)
			{
				this._currentLogoInd = 0;
			}
		}
		else
		{
			this._currentLogoInd--;
			if (this._currentLogoInd < 0)
			{
				this._currentLogoInd = this._logos.Count - 1;
				if (this._currentLogoInd < 0)
				{
					this._currentLogoInd = 0;
				}
			}
		}
		this.logo.mainTexture = this._logos[this._currentLogoInd];
		SkinsController.logoClanUserTexture = EditorTextures.CreateCopyTexture(this.logo.mainTexture);
	}

	private void HandleBackClicked(object sender, EventArgs e)
	{
		this._isCancellationRequested = true;
	}

	private void HandleCancellation()
	{
		if (this._clanIncomingInvitesController.Value.Map<ClanIncomingInvitesController, GameObject>((ClanIncomingInvitesController c) => c.inboxPanel).Map<GameObject, bool>((GameObject p) => p.activeInHierarchy))
		{
			this._clanIncomingInvitesController.Value.Do<ClanIncomingInvitesController>((ClanIncomingInvitesController c) => c.HandleBackFromInboxPressed());
			return;
		}
		if (this.deleteClanDialog.activeSelf)
		{
			this.deleteClanDialog.SetActive(false);
			this.DisableStatisticsPanel(false);
			return;
		}
		if (this._defendTime > 0f)
		{
			return;
		}
		if (this.CreateClanPanel.activeInHierarchy)
		{
			this.CreateClanPanel.SetActive(false);
			this.NoClanPanel.SetActive(true);
			return;
		}
		if (this.statisiticPanel.activeInHierarchy)
		{
			this.statisiticPanel.SetActive(false);
			ClansGUIController.AtStatsPanel = false;
			this.clanPanel.SetActive(true);
			return;
		}
		if (this.addInClanPanel.activeInHierarchy)
		{
			this.HideAddPanel();
			this.clanPanel.SetActive(true);
			return;
		}
		MenuBackgroundMusic.keepPlaying = true;
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.sceneToLoad = Defs.MainMenuScene;
		LoadConnectScene.noteToShow = null;
		Singleton<SceneLoader>.Instance.LoadScene(Defs.PromSceneName, LoadSceneMode.Single);
	}

	private void HandleCreateClanClicked(object sender, EventArgs e)
	{
		Action action1 = null;
		action1 = () => {
			this.u003cu003ef__this.CreateClanPanel.SetActive(true);
			coinsShop.thisScript.notEnoughCurrency = null;
			coinsShop.thisScript.onReturnAction = null;
			int clansPrice = Defs.ClansPrice;
			int num = Storager.getInt("Coins", false) - clansPrice;
			Action<string> action = null;
			action = (string pressedbutton) => {
				EtceteraAndroidManager.alertButtonClickedEvent -= action;
				if (pressedbutton.Equals(Defs.CancelButtonTitle))
				{
					return;
				}
				coinsShop.thisScript.notEnoughCurrency = "Coins";
				coinsShop.thisScript.onReturnAction = this.act;
				coinsShop.showCoinsShop();
			};
			string base64String = Convert.ToBase64String((this.u003cu003ef__this.logo.mainTexture as Texture2D).EncodeToPNG());
			if (num < 0)
			{
				action("Yes!");
			}
			else
			{
				if (!this.u003cu003ef__this.inputNameClanLabel.text.Equals(string.Empty))
				{
					FriendsController.sharedController.SendCreateClan(FriendsController.sharedController.id, this.u003cu003ef__this.inputNameClanLabel.text, base64String, new Action<string>(this.u003cu003ef__this.ErrorHandler));
					FriendsController.sharedController.FailedSendNewClan += new Action(this.u003cu003ef__this.FailedSendBuyClan);
					FriendsController.sharedController.ReturnNewIDClan += new Action<int>(this.u003cu003ef__this.ReturnIDNewClan);
				}
				else
				{
					this.u003cu003ef__this.StartCoroutine(this.u003cu003ef__this.ShowThisNameInUse());
				}
				this.u003cu003ef__this.BlockGUI = true;
			}
		};
		action1();
	}

	private void HandleDeleteClanClicked(object sender, EventArgs e)
	{
		this.deleteClanDialog.SetActive(true);
		this.DisableStatisticsPanel(true);
	}

	private void HandleEditClicked(object sender, EventArgs e)
	{
		this.GoToSM();
	}

	private void HandleEditLogoInPreviewClicked(object sender, EventArgs e)
	{
		this.GoToSM();
	}

	private void HandleLeaveClicked(object sender, EventArgs e)
	{
		this.InClan = false;
		this.NoClanPanel.SetActive(true);
		FriendsController.sharedController.ExitClan(null);
		FriendsController.sharedController.ClearClanData();
	}

	private void HandleNoDelClanClicked(object sender, EventArgs e)
	{
		this._isCancellationRequested = true;
	}

	private void HandleStatisticsButtonClicked(object sender, EventArgs e)
	{
		this.clanPanel.SetActive(false);
		this.statisiticPanel.SetActive(true);
		ClansGUIController.AtStatsPanel = true;
	}

	private void HandleYesDelClanClicked(object sender, EventArgs e)
	{
		this.deleteClanDialog.SetActive(false);
		this.DisableStatisticsPanel(false);
		this.InClan = false;
		this.statisiticPanel.SetActive(false);
		FriendsController.sharedController.DeleteClan();
		FriendsController.sharedController.ClearClanData();
	}

	private void HideAddPanel()
	{
		this.addInClanPanel.SetActive(false);
		FriendsController.sharedController.StopRefreshingOnlineWithClanInfo();
		FriendsController.sharedController.StartRefreshingClanOnline();
		ClansGUIController.AtAddPanel = false;
		IEnumerator enumerator = this.addFriendsGrid.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform current = (Transform)enumerator.Current;
				current.parent = null;
				UnityEngine.Object.Destroy(current.gameObject);
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
	}

	public void HideRewardWindow()
	{
		this.rewardCreateClanWindow.SetActive(false);
	}

	void IFriendsGUIController.Hide(bool h)
	{
		this.topLevelObject.SetActive(!h);
		ClansGUIController.ShowProfile = h;
	}

	private void OnChangeClanName(string newName)
	{
		if (this.nameClanLabel.text.Equals(newName))
		{
			return;
		}
		if (this.changeClanNameInput.isSelected)
		{
			return;
		}
		this.nameClanLabel.text = newName;
	}

	private void OnDestroy()
	{
		UIInputRilisoft component;
		ClansGUIController.sharedController = null;
		this._friendProfileController.Dispose();
		FriendsController.sharedController.StopRefreshingClanOnline();
		FriendsController.sharedController.onChangeClanName -= new FriendsController.OnChangeClanName(this.OnChangeClanName);
		ClansGUIController.AtAddPanel = false;
		ClansGUIController.AtStatsPanel = false;
		ClansGUIController.ShowProfile = false;
		if (this.ClanName == null)
		{
			component = null;
		}
		else
		{
			component = this.ClanName.GetComponent<UIInputRilisoft>();
		}
		UIInputRilisoft onFocu = component;
		if (onFocu != null)
		{
			onFocu.onFocus -= new UIInputRilisoft.OnFocus(this.OnFocusCreateClanName);
			onFocu.onFocusLost -= new UIInputRilisoft.OnFocusLost(this.onFocusLostCreateClanName);
		}
		FriendsController.DisposeProfile();
	}

	private void OnDisable()
	{
		FriendsController.ClanUpdated -= new Action(this.UpdateGUI);
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	private void OnEnable()
	{
		FriendsController.ClanUpdated += new Action(this.UpdateGUI);
		this.UpdateGUI();
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(() => this._isCancellationRequested = true, "Clans");
	}

	private void OnFocusCreateClanName()
	{
		if (this.ClanName != null)
		{
			this.ClanName.GetComponent<UIInputRilisoft>().@value = string.Empty;
		}
	}

	private void onFocusLostCreateClanName()
	{
		if (this.ClanName != null)
		{
			UIInputRilisoft component = this.ClanName.GetComponent<UIInputRilisoft>();
			if (string.IsNullOrEmpty(component.@value))
			{
				component.@value = LocalizationStore.Key_0589;
			}
		}
	}

	public void ReturnIDNewClan(int _idNewClan)
	{
		FriendsController.sharedController.FailedSendNewClan -= new Action(this.FailedSendBuyClan);
		FriendsController.sharedController.ReturnNewIDClan -= new Action<int>(this.ReturnIDNewClan);
		if (_idNewClan <= 0)
		{
			base.StartCoroutine(this.ShowThisNameInUse());
		}
		else
		{
			this.BlockGUI = false;
			FriendsController.sharedController.ClanID = _idNewClan.ToString();
			FriendsController.sharedController.clanLeaderID = FriendsController.sharedController.id;
			Texture2D texture2D = this.logo.mainTexture as Texture2D;
			string base64String = Convert.ToBase64String(texture2D.EncodeToPNG());
			FriendsController.sharedController.clanLogo = base64String;
			Texture texture = this.previewLogo.mainTexture;
			this.previewLogo.mainTexture = this.logo.mainTexture;
			SkinsController.logoClanUserTexture = EditorTextures.CreateCopyTexture(this.previewLogo.mainTexture);
			!(texture != null);
			FriendsController.sharedController.clanName = this.inputNameClanLabel.text;
			this.nameClanLabel.text = FriendsController.sharedController.clanName;
			this.BuyNewClan();
			string str = (ExperienceController.sharedController == null ? "Unknown" : ExperienceController.sharedController.currentLevel.ToString());
			int num = PlayerPrefs.GetInt(Defs.SessionNumberKey, 1);
			string str1 = num.ToString();
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ "Rank", str },
				{ "Session", str1 }
			};
			FlurryPluginWrapper.LogEventAndDublicateToConsole("Clan Created", strs, true);
			if ((FacebookController.FacebookSupported || TwitterController.TwitterSupported) && Storager.getInt("ShownCreateClanRewardWindowKey", false) == 0 && !Device.isPixelGunLow)
			{
				this.rewardCreateClanWindow.SetActive(true);
				Storager.setInt("ShownCreateClanRewardWindowKey", 1, false);
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator SetName(string nm)
	{
		ClansGUIController.u003cSetNameu003ec__Iterator11 variable = null;
		return variable;
	}

	private void SetScreenMessage()
	{
		if (this.receivingPlashka == null)
		{
			return;
		}
		string empty = string.Empty;
		if (!FriendsController.ClanDataSettted && this.InClan)
		{
			empty = LocalizationStore.Key_0348;
		}
		else if (!(FriendsController.sharedController != null) || !this.InClan)
		{
			if ((!this.InClan || !FriendsController.readyToOperate) && !this.NoClanPanel.activeInHierarchy && !this.CreateClanPanel.activeInHierarchy && !this.clanPanel.activeInHierarchy && !this.statisiticPanel.activeInHierarchy && !this.addInClanPanel.activeInHierarchy)
			{
				if (this.CurrentState == ClansGUIController.State.Inbox)
				{
					if (this.CurrentState != ClansGUIController.State.Inbox)
					{
						if (string.IsNullOrEmpty(empty))
						{
							this.receivingPlashka.SetActive(false);
						}
						else
						{
							this.receivingPlashka.GetComponent<UILabel>().text = empty;
							this.receivingPlashka.SetActive(true);
						}
						return;
					}
					else if (ClanIncomingInvitesController.CurrentRequest.Filter<Task<List<object>>>((Task<List<object>> t) => t.IsCompleted) != null)
					{
						if (string.IsNullOrEmpty(empty))
						{
							this.receivingPlashka.SetActive(false);
						}
						else
						{
							this.receivingPlashka.GetComponent<UILabel>().text = empty;
							this.receivingPlashka.SetActive(true);
						}
						return;
					}
				}
				empty = LocalizationStore.Key_0348;
			}
		}
		else if (this._friendProfileController != null && this._friendProfileController.FriendProfileGo != null && this._friendProfileController.FriendProfileGo.activeInHierarchy)
		{
			if (FriendsController.sharedController.NumberOffFullInfoRequests > 0)
			{
				empty = LocalizationStore.Key_0348;
			}
		}
		else if (this.CreateClanPanel.activeInHierarchy && FriendsController.sharedController.NumberOfCreateClanRequests > 0)
		{
			empty = LocalizationStore.Key_0348;
		}
		if (string.IsNullOrEmpty(empty))
		{
			this.receivingPlashka.SetActive(false);
		}
		else
		{
			this.receivingPlashka.GetComponent<UILabel>().text = empty;
			this.receivingPlashka.SetActive(true);
		}
	}

	internal void ShowAddMembersScreen()
	{
		this.clanPanel.SetActive(false);
		this.addInClanPanel.SetActive(true);
		FriendsController.sharedController.StartRefreshingOnlineWithClanInfo();
		FriendsController.sharedController.StopRefreshingClanOnline();
		ClansGUIController.AtAddPanel = true;
		base.StartCoroutine(this.FillAddMembers());
	}

	[DebuggerHidden]
	public IEnumerator ShowCheckConnection()
	{
		ClansGUIController.u003cShowCheckConnectionu003ec__Iterator14 variable = null;
		return variable;
	}

	public void ShowClanPanel()
	{
		this.clanPanel.SetActive(true);
	}

	[DebuggerHidden]
	public IEnumerator ShowThisNameInUse()
	{
		ClansGUIController.u003cShowThisNameInUseu003ec__Iterator13 variable = null;
		return variable;
	}

	private void Start()
	{
		UIInputRilisoft component;
		ClansGUIController.sharedController = this;
		RewardWindowBase rewardWindowBase = this.rewardCreateClanWindow.GetComponent<RewardWindowBase>();
		FacebookController.StoryPriority storyPriority = FacebookController.StoryPriority.Green;
		rewardWindowBase.shareAction = () => FacebookController.PostOpenGraphStory("create", "clan", storyPriority, new Dictionary<string, string>()
		{
			{ "mode", "create" }
		});
		rewardWindowBase.priority = storyPriority;
		rewardWindowBase.twitterStatus = () => "I’ve created a CLAN in @PixelGun3D! Join my team and get ready to fight! #pixelgun3d #pixelgun #pg3d #mobile #fps http://goo.gl/8fzL9u";
		rewardWindowBase.EventTitle = "Created Clan";
		rewardWindowBase.HasReward = false;
		if (this.ClanName == null)
		{
			component = null;
		}
		else
		{
			component = this.ClanName.GetComponent<UIInputRilisoft>();
		}
		UIInputRilisoft key0589 = component;
		if (key0589 != null)
		{
			key0589.@value = LocalizationStore.Key_0589;
			key0589.onFocus += new UIInputRilisoft.OnFocus(this.OnFocusCreateClanName);
			key0589.onFocusLost += new UIInputRilisoft.OnFocusLost(this.onFocusLostCreateClanName);
		}
		this._friendProfileController = new FriendProfileController(this, true);
		this.InClan = !string.IsNullOrEmpty(FriendsController.sharedController.ClanID);
		FriendsController.sharedController.onChangeClanName += new FriendsController.OnChangeClanName(this.OnChangeClanName);
		if (this.InClan && !string.IsNullOrEmpty(FriendsController.sharedController.clanName))
		{
			this.nameClanLabel.text = FriendsController.sharedController.clanName;
			this.changeClanNameInput.@value = this.nameClanLabel.text;
		}
		ClansGUIController.AtAddPanel = false;
		ClansGUIController.AtStatsPanel = false;
		this.timeOfLastSort = Time.realtimeSinceStartup;
		FriendsController.sharedController.StartRefreshingClanOnline();
		this.startPanel.SetActive(!FriendsController.readyToOperate);
		this.NoClanPanel.SetActive((!FriendsController.readyToOperate ? false : !this.InClan));
		this.clanPanel.SetActive((!FriendsController.readyToOperate ? false : this.InClan));
		if (GlobalGameController.Logos == null)
		{
			Texture2D[] texture2DArray = Resources.LoadAll<Texture2D>("Clan_Previews/") ?? new Texture2D[0];
			this._logos.AddRange(texture2DArray);
			StringComparer ordinalIgnoreCase = StringComparer.OrdinalIgnoreCase;
			this._logos.Sort((Texture2D a, Texture2D b) => ordinalIgnoreCase.Compare(a.name, b.name));
			this._currentLogoInd = 0;
		}
		else if (!this.InClan)
		{
			this.CreateClanPanel.SetActive(FriendsController.readyToOperate);
			this._logos = GlobalGameController.Logos;
			base.StartCoroutine(this.SetName(GlobalGameController.TempClanName));
			if (GlobalGameController.LogoToEdit == null)
			{
				this._currentLogoInd = 0;
			}
			else
			{
				this._logos.Add(GlobalGameController.LogoToEdit);
				this._currentLogoInd = this._logos.Count - 1;
			}
		}
		else if (GlobalGameController.LogoToEdit != null)
		{
			byte[] pNG = GlobalGameController.LogoToEdit.EncodeToPNG();
			FriendsController.sharedController.clanLogo = Convert.ToBase64String(pNG);
			FriendsController.sharedController.ChangeClanLogo();
		}
		if (this.InClan && !string.IsNullOrEmpty(FriendsController.sharedController.clanLogo))
		{
			try
			{
				byte[] numArray = Convert.FromBase64String(FriendsController.sharedController.clanLogo);
				Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoHeight, TextureFormat.ARGB32, false);
				texture2D.LoadImage(numArray);
				texture2D.filterMode = FilterMode.Point;
				texture2D.Apply();
				Texture texture = this.previewLogo.mainTexture;
				this.previewLogo.mainTexture = texture2D;
				SkinsController.logoClanUserTexture = EditorTextures.CreateCopyTexture(this.previewLogo.mainTexture);
			}
			catch
			{
			}
		}
		GlobalGameController.Logos = null;
		GlobalGameController.LogoToEdit = null;
		GlobalGameController.TempClanName = null;
		if (this._logos.Count > this._currentLogoInd)
		{
			this.logo.mainTexture = this._logos[this._currentLogoInd];
			SkinsController.logoClanUserTexture = EditorTextures.CreateCopyTexture(this.logo.mainTexture);
		}
		if (this.CreateClanButton != null)
		{
			ButtonHandler buttonHandler = this.CreateClanButton.GetComponent<ButtonHandler>();
			if (buttonHandler != null)
			{
				buttonHandler.Clicked += new EventHandler(this.HandleCreateClanClicked);
			}
		}
		if (this.EditLogoBut != null)
		{
			ButtonHandler component1 = this.EditLogoBut.GetComponent<ButtonHandler>();
			if (component1 != null)
			{
				component1.Clicked += new EventHandler(this.HandleEditClicked);
			}
		}
		if (this.BackBut != null)
		{
			ButtonHandler buttonHandler1 = this.BackBut.GetComponent<ButtonHandler>();
			if (buttonHandler1 != null)
			{
				buttonHandler1.Clicked += new EventHandler(this.HandleBackClicked);
			}
		}
		if (this.Left != null)
		{
			ButtonHandler component2 = this.Left.GetComponent<ButtonHandler>();
			if (component2 != null)
			{
				component2.Clicked += new EventHandler(this.HandleArrowClicked);
			}
		}
		if (this.Right != null)
		{
			ButtonHandler buttonHandler2 = this.Right.GetComponent<ButtonHandler>();
			if (buttonHandler2 != null)
			{
				buttonHandler2.Clicked += new EventHandler(this.HandleArrowClicked);
			}
		}
		if (this.addMembersButton != null)
		{
			ButtonHandler component3 = this.addMembersButton.GetComponent<ButtonHandler>();
			if (component3 != null)
			{
				component3.Clicked += new EventHandler(this.HandleAddMembersClicked);
			}
		}
		if (this.deleteClanButton != null)
		{
			ButtonHandler buttonHandler3 = this.deleteClanButton.GetComponent<ButtonHandler>();
			if (buttonHandler3 != null)
			{
				buttonHandler3.Clicked += new EventHandler(this.HandleDeleteClanClicked);
			}
		}
		if (this.leaveButton != null)
		{
			ButtonHandler component4 = this.leaveButton.GetComponent<ButtonHandler>();
			if (component4 != null)
			{
				component4.Clicked += new EventHandler(this.HandleLeaveClicked);
			}
		}
		if (this.editLogoInPreviewButton != null)
		{
			ButtonHandler buttonHandler4 = this.editLogoInPreviewButton.GetComponent<ButtonHandler>();
			if (buttonHandler4 != null)
			{
				buttonHandler4.Clicked += new EventHandler(this.HandleEditLogoInPreviewClicked);
			}
		}
		if (this.statisticsButton != null)
		{
			ButtonHandler component5 = this.statisticsButton.GetComponent<ButtonHandler>();
			if (component5 != null)
			{
				component5.Clicked += new EventHandler(this.HandleStatisticsButtonClicked);
			}
		}
		if (this.yesDelteClan != null)
		{
			ButtonHandler buttonHandler5 = this.yesDelteClan.GetComponent<ButtonHandler>();
			if (buttonHandler5 != null)
			{
				buttonHandler5.Clicked += new EventHandler(this.HandleYesDelClanClicked);
			}
		}
		if (this.noDeleteClan != null)
		{
			ButtonHandler component6 = this.noDeleteClan.GetComponent<ButtonHandler>();
			if (component6 != null)
			{
				component6.Clicked += new EventHandler(this.HandleNoDelClanClicked);
			}
		}
	}

	private void Update()
	{
		if (this.startPanel.activeSelf != !FriendsController.readyToOperate)
		{
			this.startPanel.SetActive(!FriendsController.readyToOperate);
		}
		if (this._isCancellationRequested)
		{
			this.HandleCancellation();
			this._isCancellationRequested = false;
		}
		this.addMembersButton.SetActive((string.IsNullOrEmpty(FriendsController.sharedController.id) || FriendsController.sharedController.clanLeaderID == null || !FriendsController.sharedController.id.Equals(FriendsController.sharedController.clanLeaderID) || this.BlockGUI ? false : FriendsController.ClanDataSettted));
		this.previewLogo.transform.parent.GetComponent<UIButton>().isEnabled = this.addMembersButton.activeInHierarchy;
		this.tapToEdit.gameObject.SetActive(this.addMembersButton.activeInHierarchy);
		this.leaveButton.SetActive((string.IsNullOrEmpty(FriendsController.sharedController.id) || FriendsController.sharedController.clanLeaderID == null ? false : !FriendsController.sharedController.id.Equals(FriendsController.sharedController.clanLeaderID)));
		this.deleteClanButton.SetActive((string.IsNullOrEmpty(FriendsController.sharedController.id) || FriendsController.sharedController.clanLeaderID == null ? false : FriendsController.sharedController.id.Equals(FriendsController.sharedController.clanLeaderID)));
		this.changeClanNameInput.gameObject.SetActive((string.IsNullOrEmpty(FriendsController.sharedController.id) || FriendsController.sharedController.clanLeaderID == null || !FriendsController.sharedController.id.Equals(FriendsController.sharedController.clanLeaderID) ? false : !this.BlockGUI));
		this.InClan = !string.IsNullOrEmpty(FriendsController.sharedController.ClanID);
		this.NoClanPanel.SetActive((!FriendsController.readyToOperate || this.InClan || this.CreateClanPanel.activeInHierarchy || !(SkinEditorController.sharedController == null) || this.CurrentState == ClansGUIController.State.Inbox ? false : this.CurrentState != ClansGUIController.State.ProfileDetails));
		this.clanPanel.SetActive((!FriendsController.readyToOperate || !this.InClan || ClansGUIController.AtAddPanel || ClansGUIController.AtStatsPanel || ClansGUIController.ShowProfile || this.CurrentState == ClansGUIController.State.Inbox ? false : this.CurrentState != ClansGUIController.State.ProfileDetails));
		this.statisiticPanel.SetActive((!FriendsController.readyToOperate || !this.InClan || ClansGUIController.AtAddPanel ? false : ClansGUIController.AtStatsPanel));
		bool flag = this.addInClanPanel.activeInHierarchy;
		this.addInClanPanel.SetActive((!FriendsController.readyToOperate || !this.InClan || !ClansGUIController.AtAddPanel || ClansGUIController.AtStatsPanel ? false : this.CurrentState != ClansGUIController.State.ProfileDetails));
		if (!this.InClan)
		{
			this.deleteClanDialog.SetActive(false);
			this.DisableStatisticsPanel(false);
		}
		if (this.clanPanel.activeInHierarchy)
		{
			this.statisticsButton.SetActive(!this.BlockGUI);
			this.friendsGrid.gameObject.SetActive(!this.BlockGUI);
		}
		if (!this.addInClanPanel.activeInHierarchy && flag)
		{
			this.HideAddPanel();
		}
		if (!this.statisiticPanel.activeInHierarchy)
		{
			ClansGUIController.AtStatsPanel = false;
		}
		if (ClansGUIController.ShowProfile && (!this.InClan || !FriendsController.readyToOperate))
		{
			this._friendProfileController.HandleBackClicked();
		}
		if (ClansGUIController.AtAddPanel)
		{
			this.clanIsFull.gameObject.SetActive(FriendsController.sharedController.ClanLimitReached);
		}
		this.SetScreenMessage();
		if (this.InClan)
		{
			this.countMembersLabel.text = string.Format("{0}\n{1}", LocalizationStore.Get("Key_0983"), FriendsController.sharedController.clanMembers.Count);
		}
		this.noMembersLabel.SetActive((FriendsController.sharedController.clanMembers == null ? false : FriendsController.sharedController.clanMembers.Count < 2));
		this.ClanName.SetActive(!this.BlockGUI);
		if (!this.statisiticPanel.activeInHierarchy)
		{
			this.BackBut.GetComponent<UIButton>().isEnabled = !this.BlockGUI;
		}
		this.CreateClanButton.GetComponent<UIButton>().isEnabled = !this.BlockGUI;
		this.Left.GetComponent<UIButton>().isEnabled = !this.BlockGUI;
		this.Right.GetComponent<UIButton>().isEnabled = !this.BlockGUI;
		this.EditLogoBut.GetComponent<UIButton>().isEnabled = !this.BlockGUI;
		if (this._defendTime > 0f)
		{
			this._defendTime -= Time.deltaTime;
		}
		this.friendsGrid.transform.parent.GetComponent<UIScrollView>().enabled = this.friendsGrid.transform.childCount > 4;
		if (this.friendsGrid.transform.childCount > 0 && this.friendsGrid.transform.childCount <= 4)
		{
			float current = 0f;
			IEnumerator enumerator = this.friendsGrid.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					current += ((Transform)enumerator.Current).localPosition.x;
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable == null)
				{
				}
				disposable.Dispose();
			}
			current /= (float)this.friendsGrid.transform.childCount;
			Transform vector3 = this.friendsGrid.transform.parent;
			if (vector3 != null)
			{
				UIPanel component = vector3.GetComponent<UIPanel>();
				if (component != null)
				{
					component.clipOffset = new Vector2(current, component.clipOffset.y);
					float single = vector3.localPosition.y;
					Vector3 vector31 = vector3.localPosition;
					vector3.localPosition = new Vector3(-current, single, vector31.z);
				}
			}
		}
		if (Time.realtimeSinceStartup - this.timeOfLastSort > 10f)
		{
			FriendsGUIController.RaiseUpdaeOnlineEvent();
			this.timeOfLastSort = Time.realtimeSinceStartup;
			this._SortFriendPreviews();
		}
		UISprite[] value = this._newMessagesOverlays.Value;
		for (int i = 0; i < (int)value.Length; i++)
		{
			UISprite uISprite = value[i];
			if (ClanIncomingInvitesController.CurrentRequest == null || !ClanIncomingInvitesController.CurrentRequest.IsCompleted)
			{
				uISprite.gameObject.SetActive(false);
			}
			else if (ClanIncomingInvitesController.CurrentRequest.IsCanceled || ClanIncomingInvitesController.CurrentRequest.IsFaulted)
			{
				uISprite.gameObject.SetActive(false);
			}
			else
			{
				uISprite.gameObject.SetActive(ClanIncomingInvitesController.CurrentRequest.Result.Count > 0);
			}
		}
	}

	public void UpdateGUI()
	{
		base.StartCoroutine(this.__UpdateGUI());
	}

	internal enum State
	{
		Default,
		Inbox,
		ProfileDetails
	}
}