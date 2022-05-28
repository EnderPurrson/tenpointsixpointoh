using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;

public sealed class ClansGUIController : MonoBehaviour, IFriendsGUIController
{
	internal enum State
	{
		Default = 0,
		Inbox = 1,
		ProfileDetails = 2
	}

	[CompilerGenerated]
	private sealed class _003CStart_003Ec__AnonStorey1E2
	{
		internal FacebookController.StoryPriority priority;

		internal void _003C_003Em__16()
		{
			FacebookController.PostOpenGraphStory("create", "clan", priority, new Dictionary<string, string> { { "mode", "create" } });
		}
	}

	[CompilerGenerated]
	private sealed class _003CStart_003Ec__AnonStorey1E3
	{
		internal StringComparer nameComparer;

		internal int _003C_003Em__18(Texture2D a, Texture2D b)
		{
			return nameComparer.Compare(a.name, b.name);
		}
	}

	[CompilerGenerated]
	private sealed class _003CChangeClanName_003Ec__AnonStorey1E4
	{
		internal string oldText;

		internal ClansGUIController _003C_003Ef__this;

		internal void _003C_003Em__1A()
		{
			FriendsController.sharedController.clanName = _003C_003Ef__this.nameClanLabel.text;
			_003C_003Ef__this.BlockGUI = false;
		}

		internal void _003C_003Em__1B(string error)
		{
			_003C_003Ef__this.nameClanLabel.text = oldText;
			Debug.Log("error " + error);
			if (!string.IsNullOrEmpty(error))
			{
				if (error.Equals("fail"))
				{
					_003C_003Ef__this.StartCoroutine(_003C_003Ef__this.ShowThisNameInUse());
				}
				else
				{
					_003C_003Ef__this.StartCoroutine(_003C_003Ef__this.ShowCheckConnection());
				}
			}
			else
			{
				_003C_003Ef__this.BlockGUI = false;
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003C_SortFriendPreviews_003Ec__AnonStorey1E5
	{
		internal StringComparer nameComparer;

		internal int _003C_003Em__1C(FriendPreview fp1, FriendPreview fp2)
		{
			return nameComparer.Compare(fp1.name, fp2.name);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGoToSM_003Ec__AnonStorey1E6
	{
		internal Action<string> backHandler;

		internal ClansGUIController _003C_003Ef__this;

		internal void _003C_003Em__21(string name)
		{
			MenuBackgroundMusic.sharedMusic.StopCustomMusicFrom(SkinEditorController.sharedController.gameObject);
			SkinEditorController.ExitFromSkinEditor -= backHandler;
			_003C_003Ef__this.logo.mainTexture = EditorTextures.CreateCopyTexture(SkinsController.logoClanUserTexture);
			if (_003C_003Ef__this.InClan)
			{
				Debug.Log("InClan");
				byte[] inArray = SkinsController.logoClanUserTexture.EncodeToPNG();
				FriendsController.sharedController.clanLogo = Convert.ToBase64String(inArray);
				FriendsController.sharedController.ChangeClanLogo();
				_003C_003Ef__this.previewLogo.mainTexture = EditorTextures.CreateCopyTexture(SkinsController.logoClanUserTexture);
			}
			else if (!string.IsNullOrEmpty(name))
			{
				_003C_003Ef__this._logos.Add(_003C_003Ef__this.logo.mainTexture as Texture2D);
				_003C_003Ef__this._currentLogoInd = _003C_003Ef__this._logos.Count - 1;
			}
			_003C_003Ef__this.gameObject.SetActive(true);
		}
	}

	[CompilerGenerated]
	private sealed class _003CHandleCreateClanClicked_003Ec__AnonStorey1E8
	{
		private sealed class _003CHandleCreateClanClicked_003Ec__AnonStorey1E7
		{
			internal Action<string> showShop;

			internal _003CHandleCreateClanClicked_003Ec__AnonStorey1E8 _003C_003Ef__ref_0024488;

			internal void _003C_003Em__27(string pressedbutton)
			{
				EtceteraAndroidManager.alertButtonClickedEvent -= showShop;
				if (!pressedbutton.Equals(Defs.CancelButtonTitle))
				{
					coinsShop.thisScript.notEnoughCurrency = "Coins";
					coinsShop.thisScript.onReturnAction = _003C_003Ef__ref_0024488.act;
					coinsShop.showCoinsShop();
				}
			}
		}

		internal Action act;

		internal ClansGUIController _003C_003Ef__this;

		internal void _003C_003Em__22()
		{
			_003CHandleCreateClanClicked_003Ec__AnonStorey1E7 _003CHandleCreateClanClicked_003Ec__AnonStorey1E = new _003CHandleCreateClanClicked_003Ec__AnonStorey1E7();
			_003CHandleCreateClanClicked_003Ec__AnonStorey1E._003C_003Ef__ref_0024488 = this;
			_003C_003Ef__this.CreateClanPanel.SetActive(true);
			coinsShop.thisScript.notEnoughCurrency = null;
			coinsShop.thisScript.onReturnAction = null;
			int clansPrice = Defs.ClansPrice;
			int @int = Storager.getInt("Coins", false);
			int num = @int - clansPrice;
			_003CHandleCreateClanClicked_003Ec__AnonStorey1E.showShop = null;
			_003CHandleCreateClanClicked_003Ec__AnonStorey1E.showShop = _003CHandleCreateClanClicked_003Ec__AnonStorey1E._003C_003Em__27;
			Texture2D texture2D = _003C_003Ef__this.logo.mainTexture as Texture2D;
			byte[] inArray = texture2D.EncodeToPNG();
			string skinClan = Convert.ToBase64String(inArray);
			if (num >= 0)
			{
				if (_003C_003Ef__this.inputNameClanLabel.text.Equals(string.Empty))
				{
					_003C_003Ef__this.StartCoroutine(_003C_003Ef__this.ShowThisNameInUse());
				}
				else
				{
					FriendsController.sharedController.SendCreateClan(FriendsController.sharedController.id, _003C_003Ef__this.inputNameClanLabel.text, skinClan, _003C_003Ef__this.ErrorHandler);
					FriendsController.sharedController.FailedSendNewClan += _003C_003Ef__this.FailedSendBuyClan;
					FriendsController.sharedController.ReturnNewIDClan += _003C_003Ef__this.ReturnIDNewClan;
				}
				_003C_003Ef__this.BlockGUI = true;
			}
			else
			{
				_003CHandleCreateClanClicked_003Ec__AnonStorey1E.showShop("Yes!");
			}
		}
	}

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

	[CompilerGenerated]
	private static Func<string> _003C_003Ef__am_0024cache37;

	[CompilerGenerated]
	private static Comparison<FriendPreview> _003C_003Ef__am_0024cache38;

	[CompilerGenerated]
	private static Func<ClanIncomingInvitesController, GameObject> _003C_003Ef__am_0024cache39;

	[CompilerGenerated]
	private static Func<GameObject, bool> _003C_003Ef__am_0024cache3A;

	[CompilerGenerated]
	private static Action<ClanIncomingInvitesController> _003C_003Ef__am_0024cache3B;

	[CompilerGenerated]
	private static Func<Task<List<object>>, bool> _003C_003Ef__am_0024cache3C;

	[CompilerGenerated]
	private static Func<GameObject, UISprite[]> _003C_003Ef__am_0024cache3D;

	[CompilerGenerated]
	private static Func<GameObject, UISprite[]> _003C_003Ef__am_0024cache3E;

	[CompilerGenerated]
	private static Func<UISprite, bool> _003C_003Ef__am_0024cache3F;

	internal State CurrentState { get; set; }

	public ClansGUIController()
	{
		_clanIncomingInvitesController = new Lazy<ClanIncomingInvitesController>(_003CClansGUIController_003Em__14);
		_newMessagesOverlays = new Lazy<UISprite[]>(_003CClansGUIController_003Em__15);
	}

	void IFriendsGUIController.Hide(bool h)
	{
		topLevelObject.SetActive(!h);
		ShowProfile = h;
	}

	public void HideRewardWindow()
	{
		rewardCreateClanWindow.SetActive(false);
	}

	private IEnumerator SetName(string nm)
	{
		yield return null;
		inputNameClanLabel.text = nm;
		inputNameClanLabel.parent.GetComponent<UIInput>().value = nm;
	}

	private void OnChangeClanName(string newName)
	{
		if (!nameClanLabel.text.Equals(newName) && !changeClanNameInput.isSelected)
		{
			nameClanLabel.text = newName;
		}
	}

	private void Start()
	{
		_003CStart_003Ec__AnonStorey1E2 _003CStart_003Ec__AnonStorey1E = new _003CStart_003Ec__AnonStorey1E2();
		sharedController = this;
		RewardWindowBase component = rewardCreateClanWindow.GetComponent<RewardWindowBase>();
		_003CStart_003Ec__AnonStorey1E.priority = FacebookController.StoryPriority.Green;
		component.shareAction = _003CStart_003Ec__AnonStorey1E._003C_003Em__16;
		component.priority = _003CStart_003Ec__AnonStorey1E.priority;
		if (_003C_003Ef__am_0024cache37 == null)
		{
			_003C_003Ef__am_0024cache37 = _003CStart_003Em__17;
		}
		component.twitterStatus = _003C_003Ef__am_0024cache37;
		component.EventTitle = "Created Clan";
		component.HasReward = false;
		UIInputRilisoft uIInputRilisoft = ((!(ClanName != null)) ? null : ClanName.GetComponent<UIInputRilisoft>());
		if (uIInputRilisoft != null)
		{
			uIInputRilisoft.value = LocalizationStore.Key_0589;
			uIInputRilisoft.onFocus = (UIInputRilisoft.OnFocus)Delegate.Combine(uIInputRilisoft.onFocus, new UIInputRilisoft.OnFocus(OnFocusCreateClanName));
			uIInputRilisoft.onFocusLost = (UIInputRilisoft.OnFocusLost)Delegate.Combine(uIInputRilisoft.onFocusLost, new UIInputRilisoft.OnFocusLost(onFocusLostCreateClanName));
		}
		_friendProfileController = new FriendProfileController(this);
		InClan = !string.IsNullOrEmpty(FriendsController.sharedController.ClanID);
		FriendsController friendsController = FriendsController.sharedController;
		friendsController.onChangeClanName = (FriendsController.OnChangeClanName)Delegate.Combine(friendsController.onChangeClanName, new FriendsController.OnChangeClanName(OnChangeClanName));
		if (InClan && !string.IsNullOrEmpty(FriendsController.sharedController.clanName))
		{
			nameClanLabel.text = FriendsController.sharedController.clanName;
			changeClanNameInput.value = nameClanLabel.text;
		}
		AtAddPanel = false;
		AtStatsPanel = false;
		timeOfLastSort = Time.realtimeSinceStartup;
		FriendsController.sharedController.StartRefreshingClanOnline();
		startPanel.SetActive(!FriendsController.readyToOperate);
		NoClanPanel.SetActive(FriendsController.readyToOperate && !InClan);
		clanPanel.SetActive(FriendsController.readyToOperate && InClan);
		if (GlobalGameController.Logos == null)
		{
			_003CStart_003Ec__AnonStorey1E3 _003CStart_003Ec__AnonStorey1E2 = new _003CStart_003Ec__AnonStorey1E3();
			Texture2D[] array = Resources.LoadAll<Texture2D>("Clan_Previews/");
			if (array == null)
			{
				array = new Texture2D[0];
			}
			_logos.AddRange(array);
			_003CStart_003Ec__AnonStorey1E2.nameComparer = StringComparer.OrdinalIgnoreCase;
			_logos.Sort(_003CStart_003Ec__AnonStorey1E2._003C_003Em__18);
			_currentLogoInd = 0;
		}
		else if (InClan)
		{
			if (GlobalGameController.LogoToEdit != null)
			{
				byte[] inArray = GlobalGameController.LogoToEdit.EncodeToPNG();
				FriendsController.sharedController.clanLogo = Convert.ToBase64String(inArray);
				FriendsController.sharedController.ChangeClanLogo();
			}
		}
		else
		{
			CreateClanPanel.SetActive(FriendsController.readyToOperate);
			_logos = GlobalGameController.Logos;
			StartCoroutine(SetName(GlobalGameController.TempClanName));
			if (GlobalGameController.LogoToEdit != null)
			{
				_logos.Add(GlobalGameController.LogoToEdit);
				_currentLogoInd = _logos.Count - 1;
			}
			else
			{
				_currentLogoInd = 0;
			}
		}
		if (InClan && !string.IsNullOrEmpty(FriendsController.sharedController.clanLogo))
		{
			try
			{
				byte[] data = Convert.FromBase64String(FriendsController.sharedController.clanLogo);
				Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoHeight, TextureFormat.ARGB32, false);
				texture2D.LoadImage(data);
				texture2D.filterMode = FilterMode.Point;
				texture2D.Apply();
				Texture mainTexture = previewLogo.mainTexture;
				previewLogo.mainTexture = texture2D;
				SkinsController.logoClanUserTexture = EditorTextures.CreateCopyTexture(previewLogo.mainTexture);
			}
			catch
			{
			}
		}
		GlobalGameController.Logos = null;
		GlobalGameController.LogoToEdit = null;
		GlobalGameController.TempClanName = null;
		if (_logos.Count > _currentLogoInd)
		{
			logo.mainTexture = _logos[_currentLogoInd];
			SkinsController.logoClanUserTexture = EditorTextures.CreateCopyTexture(logo.mainTexture);
		}
		if (CreateClanButton != null)
		{
			ButtonHandler component2 = CreateClanButton.GetComponent<ButtonHandler>();
			if (component2 != null)
			{
				component2.Clicked += HandleCreateClanClicked;
			}
		}
		if (EditLogoBut != null)
		{
			ButtonHandler component3 = EditLogoBut.GetComponent<ButtonHandler>();
			if (component3 != null)
			{
				component3.Clicked += HandleEditClicked;
			}
		}
		if (BackBut != null)
		{
			ButtonHandler component4 = BackBut.GetComponent<ButtonHandler>();
			if (component4 != null)
			{
				component4.Clicked += HandleBackClicked;
			}
		}
		if (Left != null)
		{
			ButtonHandler component5 = Left.GetComponent<ButtonHandler>();
			if (component5 != null)
			{
				component5.Clicked += HandleArrowClicked;
			}
		}
		if (Right != null)
		{
			ButtonHandler component6 = Right.GetComponent<ButtonHandler>();
			if (component6 != null)
			{
				component6.Clicked += HandleArrowClicked;
			}
		}
		if (addMembersButton != null)
		{
			ButtonHandler component7 = addMembersButton.GetComponent<ButtonHandler>();
			if (component7 != null)
			{
				component7.Clicked += HandleAddMembersClicked;
			}
		}
		if (deleteClanButton != null)
		{
			ButtonHandler component8 = deleteClanButton.GetComponent<ButtonHandler>();
			if (component8 != null)
			{
				component8.Clicked += HandleDeleteClanClicked;
			}
		}
		if (leaveButton != null)
		{
			ButtonHandler component9 = leaveButton.GetComponent<ButtonHandler>();
			if (component9 != null)
			{
				component9.Clicked += HandleLeaveClicked;
			}
		}
		if (editLogoInPreviewButton != null)
		{
			ButtonHandler component10 = editLogoInPreviewButton.GetComponent<ButtonHandler>();
			if (component10 != null)
			{
				component10.Clicked += HandleEditLogoInPreviewClicked;
			}
		}
		if (statisticsButton != null)
		{
			ButtonHandler component11 = statisticsButton.GetComponent<ButtonHandler>();
			if (component11 != null)
			{
				component11.Clicked += HandleStatisticsButtonClicked;
			}
		}
		if (yesDelteClan != null)
		{
			ButtonHandler component12 = yesDelteClan.GetComponent<ButtonHandler>();
			if (component12 != null)
			{
				component12.Clicked += HandleYesDelClanClicked;
			}
		}
		if (noDeleteClan != null)
		{
			ButtonHandler component13 = noDeleteClan.GetComponent<ButtonHandler>();
			if (component13 != null)
			{
				component13.Clicked += HandleNoDelClanClicked;
			}
		}
	}

	private void OnEnable()
	{
		FriendsController.ClanUpdated += UpdateGUI;
		UpdateGUI();
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
		_backSubscription = BackSystem.Instance.Register(_003COnEnable_003Em__19, "Clans");
	}

	private void OnDisable()
	{
		FriendsController.ClanUpdated -= UpdateGUI;
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
	}

	public void UpdateGUI()
	{
		StartCoroutine(__UpdateGUI());
	}

	public void ChangeClanName()
	{
		_003CChangeClanName_003Ec__AnonStorey1E4 _003CChangeClanName_003Ec__AnonStorey1E = new _003CChangeClanName_003Ec__AnonStorey1E4();
		_003CChangeClanName_003Ec__AnonStorey1E._003C_003Ef__this = this;
		_003CChangeClanName_003Ec__AnonStorey1E.oldText = FriendsController.sharedController.clanName ?? string.Empty;
		if (string.IsNullOrEmpty(nameClanLabel.text))
		{
			nameClanLabel.text = _003CChangeClanName_003Ec__AnonStorey1E.oldText;
			StartCoroutine(ShowThisNameInUse());
		}
		else
		{
			FriendsController.sharedController.ChangeClanName(nameClanLabel.text, _003CChangeClanName_003Ec__AnonStorey1E._003C_003Em__1A, _003CChangeClanName_003Ec__AnonStorey1E._003C_003Em__1B);
		}
		BlockGUI = true;
	}

	private void _SortFriendPreviews()
	{
		_003C_SortFriendPreviews_003Ec__AnonStorey1E5 _003C_SortFriendPreviews_003Ec__AnonStorey1E = new _003C_SortFriendPreviews_003Ec__AnonStorey1E5();
		FriendPreview[] componentsInChildren = friendsGrid.GetComponentsInChildren<FriendPreview>(true);
		FriendPreview[] array = friendsGrid.GetComponentsInChildren<FriendPreview>(false);
		if (array == null)
		{
			array = new FriendPreview[0];
		}
		_003C_SortFriendPreviews_003Ec__AnonStorey1E.nameComparer = StringComparer.Ordinal;
		Array.Sort(array, _003C_SortFriendPreviews_003Ec__AnonStorey1E._003C_003Em__1C);
		string text = null;
		float num = 0f;
		if (array.Length > 0)
		{
			text = array[0].gameObject.name;
			Transform parent = friendsGrid.transform.parent;
			if (parent != null)
			{
				UIPanel component = parent.GetComponent<UIPanel>();
				if (component != null)
				{
					num = array[0].transform.localPosition.x - component.clipOffset.x;
				}
			}
		}
		if (_003C_003Ef__am_0024cache38 == null)
		{
			_003C_003Ef__am_0024cache38 = _003C_SortFriendPreviews_003Em__1D;
		}
		Array.Sort(componentsInChildren, _003C_003Ef__am_0024cache38);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.name = i.ToString("D7");
		}
		friendsGrid.SortAlphabetically();
		friendsGrid.WrapContent();
		Transform transform = null;
		if (text != null)
		{
			FriendPreview[] array2 = componentsInChildren;
			foreach (FriendPreview friendPreview in array2)
			{
				if (friendPreview.name.Equals(text))
				{
					transform = friendPreview.transform;
					break;
				}
			}
		}
		if (transform == null && componentsInChildren.Length > 0 && friendsGrid.gameObject.activeInHierarchy)
		{
			transform = componentsInChildren[0].transform;
		}
		if (transform != null)
		{
			float num2 = transform.localPosition.x - num;
			Transform parent2 = friendsGrid.transform.parent;
			if (parent2 != null)
			{
				UIPanel component2 = parent2.GetComponent<UIPanel>();
				if (component2 != null)
				{
					component2.clipOffset = new Vector2(num2, component2.clipOffset.y);
					parent2.localPosition = new Vector3(0f - num2, parent2.localPosition.y, parent2.localPosition.z);
				}
			}
		}
		friendsGrid.WrapContent();
	}

	private IEnumerator __UpdateGUI()
	{
		try
		{
			byte[] _skinByte = Convert.FromBase64String(FriendsController.sharedController.clanLogo);
			Texture2D _skinNew = new Texture2D(Defs.LogoWidth, Defs.LogoHeight, TextureFormat.ARGB32, false);
			_skinNew.LoadImage(_skinByte);
			_skinNew.filterMode = FilterMode.Point;
			_skinNew.Apply();
			Texture oldTexture = previewLogo.mainTexture;
			previewLogo.mainTexture = _skinNew;
			SkinsController.logoClanUserTexture = EditorTextures.CreateCopyTexture(previewLogo.mainTexture);
		}
		catch (Exception ex)
		{
			Exception e = ex;
			Debug.LogWarning(e);
		}
		FriendPreview[] fps = friendsGrid.GetComponentsInChildren<FriendPreview>(true);
		List<FriendPreview> toRemove = new List<FriendPreview>();
		List<string> existingPreviews = new List<string>();
		FriendPreview[] array = fps;
		foreach (FriendPreview fp in array)
		{
			bool found = false;
			foreach (Dictionary<string, string> member in FriendsController.sharedController.clanMembers)
			{
				string _id;
				if (!member.TryGetValue("id", out _id) || !_id.Equals(fp.id))
				{
					continue;
				}
				found = true;
				fp.nm.text = member["nick"];
				break;
			}
			if (!found)
			{
				toRemove.Add(fp);
			}
			else if (fp.id != null)
			{
				existingPreviews.Add(fp.id);
			}
		}
		foreach (FriendPreview fp2 in toRemove)
		{
			fp2.transform.parent = null;
			UnityEngine.Object.Destroy(fp2.gameObject);
		}
		foreach (Dictionary<string, string> member2 in FriendsController.sharedController.clanMembers)
		{
			if (member2.ContainsKey("id") && !existingPreviews.Contains(member2["id"]) && !member2["id"].Equals(FriendsController.sharedController.id))
			{
				GameObject f = UnityEngine.Object.Instantiate(Resources.Load("Friend")) as GameObject;
				f.transform.parent = friendsGrid.transform;
				f.transform.localScale = new Vector3(1f, 1f, 1f);
				f.GetComponent<FriendPreview>().id = member2["id"];
				f.GetComponent<FriendPreview>().ClanMember = true;
				f.GetComponent<FriendPreview>().join.GetComponent<JoinRoomFromFrendsButton>().joinRoomFromFrends = joinRoomFromFrends;
				if (member2.ContainsKey("nick"))
				{
					f.GetComponent<FriendPreview>().nm.text = member2["nick"];
				}
			}
		}
		yield return null;
		timeOfLastSort = Time.realtimeSinceStartup;
		_SortFriendPreviews();
	}

	private void HandleArrowClicked(object sender, EventArgs e)
	{
		if ((sender as ButtonHandler).gameObject == Left)
		{
			_currentLogoInd--;
			if (_currentLogoInd < 0)
			{
				_currentLogoInd = _logos.Count - 1;
				if (_currentLogoInd < 0)
				{
					_currentLogoInd = 0;
				}
			}
		}
		else
		{
			_currentLogoInd++;
			if (_currentLogoInd >= _logos.Count)
			{
				_currentLogoInd = 0;
			}
		}
		logo.mainTexture = _logos[_currentLogoInd];
		SkinsController.logoClanUserTexture = EditorTextures.CreateCopyTexture(logo.mainTexture);
	}

	private void HandleBackClicked(object sender, EventArgs e)
	{
		_isCancellationRequested = true;
	}

	private void HandleCancellation()
	{
		ClanIncomingInvitesController value = _clanIncomingInvitesController.Value;
		if (_003C_003Ef__am_0024cache39 == null)
		{
			_003C_003Ef__am_0024cache39 = _003CHandleCancellation_003Em__1E;
		}
		GameObject o = value.Map(_003C_003Ef__am_0024cache39);
		if (_003C_003Ef__am_0024cache3A == null)
		{
			_003C_003Ef__am_0024cache3A = _003CHandleCancellation_003Em__1F;
		}
		if (o.Map(_003C_003Ef__am_0024cache3A))
		{
			ClanIncomingInvitesController value2 = _clanIncomingInvitesController.Value;
			if (_003C_003Ef__am_0024cache3B == null)
			{
				_003C_003Ef__am_0024cache3B = _003CHandleCancellation_003Em__20;
			}
			value2.Do(_003C_003Ef__am_0024cache3B);
		}
		else if (deleteClanDialog.activeSelf)
		{
			deleteClanDialog.SetActive(false);
			DisableStatisticsPanel(false);
		}
		else if (!(_defendTime > 0f))
		{
			if (CreateClanPanel.activeInHierarchy)
			{
				CreateClanPanel.SetActive(false);
				NoClanPanel.SetActive(true);
			}
			else if (statisiticPanel.activeInHierarchy)
			{
				statisiticPanel.SetActive(false);
				AtStatsPanel = false;
				clanPanel.SetActive(true);
			}
			else if (addInClanPanel.activeInHierarchy)
			{
				HideAddPanel();
				clanPanel.SetActive(true);
			}
			else
			{
				MenuBackgroundMusic.keepPlaying = true;
				LoadConnectScene.textureToShow = null;
				LoadConnectScene.sceneToLoad = Defs.MainMenuScene;
				LoadConnectScene.noteToShow = null;
				Singleton<SceneLoader>.Instance.LoadScene(Defs.PromSceneName);
			}
		}
	}

	private void HideAddPanel()
	{
		addInClanPanel.SetActive(false);
		FriendsController.sharedController.StopRefreshingOnlineWithClanInfo();
		FriendsController.sharedController.StartRefreshingClanOnline();
		AtAddPanel = false;
		foreach (Transform item in addFriendsGrid.transform)
		{
			item.parent = null;
			UnityEngine.Object.Destroy(item.gameObject);
		}
	}

	private void HandleEditClicked(object sender, EventArgs e)
	{
		GoToSM();
	}

	public void GoToSM()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("SkinEditorController"));
		SkinEditorController component = gameObject.GetComponent<SkinEditorController>();
		if (component != null)
		{
			_003CGoToSM_003Ec__AnonStorey1E6 _003CGoToSM_003Ec__AnonStorey1E = new _003CGoToSM_003Ec__AnonStorey1E6();
			_003CGoToSM_003Ec__AnonStorey1E._003C_003Ef__this = this;
			_003CGoToSM_003Ec__AnonStorey1E.backHandler = null;
			_003CGoToSM_003Ec__AnonStorey1E.backHandler = _003CGoToSM_003Ec__AnonStorey1E._003C_003Em__21;
			SkinEditorController.ExitFromSkinEditor += _003CGoToSM_003Ec__AnonStorey1E.backHandler;
			if (InClan)
			{
				SkinsController.logoClanUserTexture = EditorTextures.CreateCopyTexture((Texture2D)previewLogo.mainTexture);
			}
			else
			{
				SkinsController.logoClanUserTexture = EditorTextures.CreateCopyTexture((Texture2D)logo.mainTexture);
			}
			SkinEditorController.modeEditor = SkinEditorController.ModeEditor.LogoClan;
			SkinEditorController.currentSkin = EditorTextures.CreateCopyTexture((Texture2D)logo.mainTexture);
			gameObject.transform.parent = null;
			base.gameObject.SetActive(false);
		}
	}

	public void FailedSendBuyClan()
	{
		FriendsController.sharedController.FailedSendNewClan -= FailedSendBuyClan;
		FriendsController.sharedController.ReturnNewIDClan -= ReturnIDNewClan;
	}

	public void ReturnIDNewClan(int _idNewClan)
	{
		FriendsController.sharedController.FailedSendNewClan -= FailedSendBuyClan;
		FriendsController.sharedController.ReturnNewIDClan -= ReturnIDNewClan;
		if (_idNewClan > 0)
		{
			BlockGUI = false;
			FriendsController.sharedController.ClanID = _idNewClan.ToString();
			FriendsController.sharedController.clanLeaderID = FriendsController.sharedController.id;
			Texture2D texture2D = logo.mainTexture as Texture2D;
			byte[] inArray = texture2D.EncodeToPNG();
			string clanLogo = Convert.ToBase64String(inArray);
			FriendsController.sharedController.clanLogo = clanLogo;
			Texture mainTexture = previewLogo.mainTexture;
			previewLogo.mainTexture = logo.mainTexture;
			SkinsController.logoClanUserTexture = EditorTextures.CreateCopyTexture(previewLogo.mainTexture);
			if (mainTexture != null)
			{
			}
			FriendsController.sharedController.clanName = inputNameClanLabel.text;
			nameClanLabel.text = FriendsController.sharedController.clanName;
			BuyNewClan();
			string value = ((!(ExperienceController.sharedController != null)) ? "Unknown" : ExperienceController.sharedController.currentLevel.ToString());
			string value2 = PlayerPrefs.GetInt(Defs.SessionNumberKey, 1).ToString();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("Rank", value);
			dictionary.Add("Session", value2);
			Dictionary<string, string> parameters = dictionary;
			FlurryPluginWrapper.LogEventAndDublicateToConsole("Clan Created", parameters);
			if ((FacebookController.FacebookSupported || TwitterController.TwitterSupported) && Storager.getInt("ShownCreateClanRewardWindowKey", false) == 0 && !Device.isPixelGunLow)
			{
				rewardCreateClanWindow.SetActive(true);
				Storager.setInt("ShownCreateClanRewardWindowKey", 1, false);
			}
		}
		else
		{
			StartCoroutine(ShowThisNameInUse());
		}
	}

	public IEnumerator ShowThisNameInUse()
	{
		NameIsUsedPanel.SetActive(true);
		yield return new WaitForSeconds(3f);
		NameIsUsedPanel.SetActive(false);
		BlockGUI = false;
	}

	public IEnumerator ShowCheckConnection()
	{
		CheckConnectionPanel.SetActive(true);
		yield return new WaitForSeconds(3f);
		CheckConnectionPanel.SetActive(false);
		BlockGUI = false;
	}

	public void BuyNewClan()
	{
		int clansPrice = Defs.ClansPrice;
		int @int = Storager.getInt("Coins", false);
		int val = @int - clansPrice;
		Storager.setInt("Coins", val, false);
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
		CreateClanPanel.SetActive(false);
		InClan = true;
		ShowClanPanel();
	}

	private void HandleCreateClanClicked(object sender, EventArgs e)
	{
		_003CHandleCreateClanClicked_003Ec__AnonStorey1E8 _003CHandleCreateClanClicked_003Ec__AnonStorey1E = new _003CHandleCreateClanClicked_003Ec__AnonStorey1E8();
		_003CHandleCreateClanClicked_003Ec__AnonStorey1E._003C_003Ef__this = this;
		_003CHandleCreateClanClicked_003Ec__AnonStorey1E.act = null;
		_003CHandleCreateClanClicked_003Ec__AnonStorey1E.act = _003CHandleCreateClanClicked_003Ec__AnonStorey1E._003C_003Em__22;
		_003CHandleCreateClanClicked_003Ec__AnonStorey1E.act();
	}

	private void ErrorHandler(string error)
	{
		FriendsController.sharedController.FailedSendNewClan -= FailedSendBuyClan;
		FriendsController.sharedController.ReturnNewIDClan -= ReturnIDNewClan;
		BlockGUI = false;
	}

	private void HandleEditLogoInPreviewClicked(object sender, EventArgs e)
	{
		GoToSM();
	}

	private void HandleLeaveClicked(object sender, EventArgs e)
	{
		InClan = false;
		NoClanPanel.SetActive(true);
		FriendsController.sharedController.ExitClan();
		FriendsController.sharedController.ClearClanData();
	}

	private void HandleAddMembersClicked(object sender, EventArgs e)
	{
		ShowAddMembersScreen();
	}

	internal void ShowAddMembersScreen()
	{
		clanPanel.SetActive(false);
		addInClanPanel.SetActive(true);
		FriendsController.sharedController.StartRefreshingOnlineWithClanInfo();
		FriendsController.sharedController.StopRefreshingClanOnline();
		AtAddPanel = true;
		StartCoroutine(FillAddMembers());
	}

	private void HandleDeleteClanClicked(object sender, EventArgs e)
	{
		deleteClanDialog.SetActive(true);
		DisableStatisticsPanel(true);
	}

	private void HandleYesDelClanClicked(object sender, EventArgs e)
	{
		deleteClanDialog.SetActive(false);
		DisableStatisticsPanel(false);
		InClan = false;
		statisiticPanel.SetActive(false);
		FriendsController.sharedController.DeleteClan();
		FriendsController.sharedController.ClearClanData();
	}

	private void HandleNoDelClanClicked(object sender, EventArgs e)
	{
		_isCancellationRequested = true;
	}

	private void DisableStatisticsPanel(bool disable)
	{
		BackBut.GetComponent<UIButton>().isEnabled = !disable;
		deleteClanButton.GetComponent<UIButton>().isEnabled = !disable;
	}

	private void HandleStatisticsButtonClicked(object sender, EventArgs e)
	{
		clanPanel.SetActive(false);
		statisiticPanel.SetActive(true);
		AtStatsPanel = true;
	}

	public void ShowClanPanel()
	{
		clanPanel.SetActive(true);
	}

	private IEnumerator FillAddMembers()
	{
		foreach (Transform child in addFriendsGrid.transform)
		{
			child.parent = null;
			UnityEngine.Object.Destroy(child.gameObject);
		}
		foreach (string friend in FriendsController.sharedController.friends)
		{
			Dictionary<string, object> playerInfo;
			if (!FriendsController.sharedController.playersInfo.TryGetValue(friend, out playerInfo))
			{
				continue;
			}
			object playerNode;
			if (playerInfo.TryGetValue("player", out playerNode))
			{
				Dictionary<string, object> o = playerNode as Dictionary<string, object>;
				if (_003CFillAddMembers_003Ec__Iterator15._003C_003Ef__am_0024cacheF == null)
				{
					_003CFillAddMembers_003Ec__Iterator15._003C_003Ef__am_0024cacheF = _003CFillAddMembers_003Ec__Iterator15._003C_003Em__28;
				}
				Dictionary<string, string> playerDictionary = o.Map(_003CFillAddMembers_003Ec__Iterator15._003C_003Ef__am_0024cacheF);
				string clanCreatorId;
				if (playerDictionary.TryGetValue("clan_creator_id", out clanCreatorId) && clanCreatorId == FriendsController.sharedController.id)
				{
					continue;
				}
			}
			GameObject f = UnityEngine.Object.Instantiate(Resources.Load("Friend")) as GameObject;
			FriendPreview fp = f.GetComponent<FriendPreview>();
			f.transform.parent = addFriendsGrid.transform;
			f.transform.localScale = new Vector3(1f, 1f, 1f);
			fp.ClanInvite = true;
			fp.id = friend;
			fp.join.GetComponent<JoinRoomFromFrendsButton>().joinRoomFromFrends = joinRoomFromFrends;
			Dictionary<string, object> source = playerInfo;
			if (_003CFillAddMembers_003Ec__Iterator15._003C_003Ef__am_0024cache10 == null)
			{
				_003CFillAddMembers_003Ec__Iterator15._003C_003Ef__am_0024cache10 = _003CFillAddMembers_003Ec__Iterator15._003C_003Em__29;
			}
			Func<KeyValuePair<string, object>, string> _003C_003Ef__am_0024cache = _003CFillAddMembers_003Ec__Iterator15._003C_003Ef__am_0024cache10;
			if (_003CFillAddMembers_003Ec__Iterator15._003C_003Ef__am_0024cache11 == null)
			{
				_003CFillAddMembers_003Ec__Iterator15._003C_003Ef__am_0024cache11 = _003CFillAddMembers_003Ec__Iterator15._003C_003Em__2A;
			}
			Dictionary<string, string> plDict = source.ToDictionary(_003C_003Ef__am_0024cache, _003CFillAddMembers_003Ec__Iterator15._003C_003Ef__am_0024cache11);
			if (playerInfo.ContainsKey("nick"))
			{
				fp.nm.text = plDict["nick"];
			}
			if (playerInfo.ContainsKey("rank"))
			{
				string r = plDict["rank"];
				if (r.Equals("0"))
				{
					r = "1";
				}
				fp.rank.spriteName = "Rank_" + r;
			}
			if (playerInfo.ContainsKey("skin"))
			{
				fp.SetSkin(plDict["skin"]);
			}
			fp.FillClanAttrs(plDict);
		}
		yield return null;
		addFriendsGrid.Reposition();
	}

	private void Update()
	{
		if (startPanel.activeSelf != !FriendsController.readyToOperate)
		{
			startPanel.SetActive(!FriendsController.readyToOperate);
		}
		if (_isCancellationRequested)
		{
			HandleCancellation();
			_isCancellationRequested = false;
		}
		addMembersButton.SetActive(!string.IsNullOrEmpty(FriendsController.sharedController.id) && FriendsController.sharedController.clanLeaderID != null && FriendsController.sharedController.id.Equals(FriendsController.sharedController.clanLeaderID) && !BlockGUI && FriendsController.ClanDataSettted);
		previewLogo.transform.parent.GetComponent<UIButton>().isEnabled = addMembersButton.activeInHierarchy;
		tapToEdit.gameObject.SetActive(addMembersButton.activeInHierarchy);
		leaveButton.SetActive(!string.IsNullOrEmpty(FriendsController.sharedController.id) && FriendsController.sharedController.clanLeaderID != null && !FriendsController.sharedController.id.Equals(FriendsController.sharedController.clanLeaderID));
		deleteClanButton.SetActive(!string.IsNullOrEmpty(FriendsController.sharedController.id) && FriendsController.sharedController.clanLeaderID != null && FriendsController.sharedController.id.Equals(FriendsController.sharedController.clanLeaderID));
		changeClanNameInput.gameObject.SetActive(!string.IsNullOrEmpty(FriendsController.sharedController.id) && FriendsController.sharedController.clanLeaderID != null && FriendsController.sharedController.id.Equals(FriendsController.sharedController.clanLeaderID) && !BlockGUI);
		InClan = !string.IsNullOrEmpty(FriendsController.sharedController.ClanID);
		NoClanPanel.SetActive(FriendsController.readyToOperate && !InClan && !CreateClanPanel.activeInHierarchy && SkinEditorController.sharedController == null && CurrentState != State.Inbox && CurrentState != State.ProfileDetails);
		clanPanel.SetActive(FriendsController.readyToOperate && InClan && !AtAddPanel && !AtStatsPanel && !ShowProfile && CurrentState != State.Inbox && CurrentState != State.ProfileDetails);
		statisiticPanel.SetActive(FriendsController.readyToOperate && InClan && !AtAddPanel && AtStatsPanel);
		bool activeInHierarchy = addInClanPanel.activeInHierarchy;
		addInClanPanel.SetActive(FriendsController.readyToOperate && InClan && AtAddPanel && !AtStatsPanel && CurrentState != State.ProfileDetails);
		if (!InClan)
		{
			deleteClanDialog.SetActive(false);
			DisableStatisticsPanel(false);
		}
		if (clanPanel.activeInHierarchy)
		{
			statisticsButton.SetActive(!BlockGUI);
			friendsGrid.gameObject.SetActive(!BlockGUI);
		}
		if (!addInClanPanel.activeInHierarchy && activeInHierarchy)
		{
			HideAddPanel();
		}
		if (!statisiticPanel.activeInHierarchy)
		{
			AtStatsPanel = false;
		}
		if (ShowProfile && (!InClan || !FriendsController.readyToOperate))
		{
			_friendProfileController.HandleBackClicked();
		}
		if (AtAddPanel)
		{
			clanIsFull.gameObject.SetActive(FriendsController.sharedController.ClanLimitReached);
		}
		SetScreenMessage();
		if (InClan)
		{
			countMembersLabel.text = string.Format("{0}\n{1}", LocalizationStore.Get("Key_0983"), FriendsController.sharedController.clanMembers.Count);
		}
		noMembersLabel.SetActive(FriendsController.sharedController.clanMembers != null && FriendsController.sharedController.clanMembers.Count < 2);
		ClanName.SetActive(!BlockGUI);
		if (!statisiticPanel.activeInHierarchy)
		{
			BackBut.GetComponent<UIButton>().isEnabled = !BlockGUI;
		}
		CreateClanButton.GetComponent<UIButton>().isEnabled = !BlockGUI;
		Left.GetComponent<UIButton>().isEnabled = !BlockGUI;
		Right.GetComponent<UIButton>().isEnabled = !BlockGUI;
		EditLogoBut.GetComponent<UIButton>().isEnabled = !BlockGUI;
		if (_defendTime > 0f)
		{
			_defendTime -= Time.deltaTime;
		}
		friendsGrid.transform.parent.GetComponent<UIScrollView>().enabled = friendsGrid.transform.childCount > 4;
		if (friendsGrid.transform.childCount > 0 && friendsGrid.transform.childCount <= 4)
		{
			float num = 0f;
			foreach (Transform item in friendsGrid.transform)
			{
				num += item.localPosition.x;
			}
			num /= (float)friendsGrid.transform.childCount;
			Transform parent = friendsGrid.transform.parent;
			if (parent != null)
			{
				UIPanel component = parent.GetComponent<UIPanel>();
				if (component != null)
				{
					component.clipOffset = new Vector2(num, component.clipOffset.y);
					parent.localPosition = new Vector3(0f - num, parent.localPosition.y, parent.localPosition.z);
				}
			}
		}
		if (Time.realtimeSinceStartup - timeOfLastSort > 10f)
		{
			FriendsGUIController.RaiseUpdaeOnlineEvent();
			timeOfLastSort = Time.realtimeSinceStartup;
			_SortFriendPreviews();
		}
		UISprite[] value = _newMessagesOverlays.Value;
		UISprite[] array = value;
		foreach (UISprite uISprite in array)
		{
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

	private void SetScreenMessage()
	{
		if (receivingPlashka == null)
		{
			return;
		}
		string text = string.Empty;
		if (!FriendsController.ClanDataSettted && InClan)
		{
			text = LocalizationStore.Key_0348;
		}
		else if (FriendsController.sharedController != null && InClan)
		{
			if (_friendProfileController != null && _friendProfileController.FriendProfileGo != null && _friendProfileController.FriendProfileGo.activeInHierarchy)
			{
				if (FriendsController.sharedController.NumberOffFullInfoRequests > 0)
				{
					text = LocalizationStore.Key_0348;
				}
			}
			else if (CreateClanPanel.activeInHierarchy && FriendsController.sharedController.NumberOfCreateClanRequests > 0)
			{
				text = LocalizationStore.Key_0348;
			}
		}
		else if ((!InClan || !FriendsController.readyToOperate) && !NoClanPanel.activeInHierarchy && !CreateClanPanel.activeInHierarchy && !clanPanel.activeInHierarchy && !statisiticPanel.activeInHierarchy && !addInClanPanel.activeInHierarchy)
		{
			if (CurrentState != State.Inbox)
			{
				goto IL_0178;
			}
			if (CurrentState == State.Inbox)
			{
				Task<List<object>> currentRequest = ClanIncomingInvitesController.CurrentRequest;
				if (_003C_003Ef__am_0024cache3C == null)
				{
					_003C_003Ef__am_0024cache3C = _003CSetScreenMessage_003Em__23;
				}
				if (currentRequest.Filter(_003C_003Ef__am_0024cache3C) == null)
				{
					goto IL_0178;
				}
			}
		}
		goto IL_017e;
		IL_0178:
		text = LocalizationStore.Key_0348;
		goto IL_017e;
		IL_017e:
		if (!string.IsNullOrEmpty(text))
		{
			receivingPlashka.GetComponent<UILabel>().text = text;
			receivingPlashka.SetActive(true);
		}
		else
		{
			receivingPlashka.SetActive(false);
		}
	}

	private void OnDestroy()
	{
		sharedController = null;
		_friendProfileController.Dispose();
		FriendsController.sharedController.StopRefreshingClanOnline();
		FriendsController friendsController = FriendsController.sharedController;
		friendsController.onChangeClanName = (FriendsController.OnChangeClanName)Delegate.Remove(friendsController.onChangeClanName, new FriendsController.OnChangeClanName(OnChangeClanName));
		AtAddPanel = false;
		AtStatsPanel = false;
		ShowProfile = false;
		UIInputRilisoft uIInputRilisoft = ((!(ClanName != null)) ? null : ClanName.GetComponent<UIInputRilisoft>());
		if (uIInputRilisoft != null)
		{
			uIInputRilisoft.onFocus = (UIInputRilisoft.OnFocus)Delegate.Remove(uIInputRilisoft.onFocus, new UIInputRilisoft.OnFocus(OnFocusCreateClanName));
			uIInputRilisoft.onFocusLost = (UIInputRilisoft.OnFocusLost)Delegate.Remove(uIInputRilisoft.onFocusLost, new UIInputRilisoft.OnFocusLost(onFocusLostCreateClanName));
		}
		FriendsController.DisposeProfile();
	}

	private void OnFocusCreateClanName()
	{
		if (ClanName != null)
		{
			ClanName.GetComponent<UIInputRilisoft>().value = string.Empty;
		}
	}

	private void onFocusLostCreateClanName()
	{
		if (ClanName != null)
		{
			UIInputRilisoft component = ClanName.GetComponent<UIInputRilisoft>();
			if (string.IsNullOrEmpty(component.value))
			{
				component.value = LocalizationStore.Key_0589;
			}
		}
	}

	[CompilerGenerated]
	private ClanIncomingInvitesController _003CClansGUIController_003Em__14()
	{
		return base.gameObject.GetComponent<ClanIncomingInvitesController>();
	}

	[CompilerGenerated]
	private UISprite[] _003CClansGUIController_003Em__15()
	{
		GameObject o = clanPanel;
		if (_003C_003Ef__am_0024cache3D == null)
		{
			_003C_003Ef__am_0024cache3D = _003CClansGUIController_003Em__24;
		}
		UISprite[] first = o.Map(_003C_003Ef__am_0024cache3D, new UISprite[0]);
		GameObject noClanPanel = NoClanPanel;
		if (_003C_003Ef__am_0024cache3E == null)
		{
			_003C_003Ef__am_0024cache3E = _003CClansGUIController_003Em__25;
		}
		UISprite[] second = noClanPanel.Map(_003C_003Ef__am_0024cache3E, new UISprite[0]);
		IEnumerable<UISprite> source = first.Concat(second);
		if (_003C_003Ef__am_0024cache3F == null)
		{
			_003C_003Ef__am_0024cache3F = _003CClansGUIController_003Em__26;
		}
		IEnumerable<UISprite> source2 = source.Where(_003C_003Ef__am_0024cache3F);
		return source2.ToArray();
	}

	[CompilerGenerated]
	private static string _003CStart_003Em__17()
	{
		return "I’ve created a CLAN in @PixelGun3D! Join my team and get ready to fight! #pixelgun3d #pixelgun #pg3d #mobile #fps http://goo.gl/8fzL9u";
	}

	[CompilerGenerated]
	private void _003COnEnable_003Em__19()
	{
		_isCancellationRequested = true;
	}

	[CompilerGenerated]
	private static int _003C_SortFriendPreviews_003Em__1D(FriendPreview fp1, FriendPreview fp2)
	{
		if (fp1.id == null || !FriendsController.sharedController.onlineInfo.ContainsKey(fp1.id))
		{
			return 1;
		}
		if (fp2.id == null || !FriendsController.sharedController.onlineInfo.ContainsKey(fp2.id))
		{
			return -1;
		}
		string s = FriendsController.sharedController.onlineInfo[fp1.id]["delta"];
		string s2 = FriendsController.sharedController.onlineInfo[fp1.id]["game_mode"];
		int num = int.Parse(s);
		int num2 = int.Parse(s2);
		int num3 = (((float)num > FriendsController.onlineDelta || (num2 > 99 && num2 / 100 != (int)ConnectSceneNGUIController.myPlatformConnect && num2 / 100 != 3)) ? 2 : ((num2 == -1) ? 1 : 0));
		if (FriendsController.sharedController.clanLeaderID != null && fp1.id.Equals(FriendsController.sharedController.clanLeaderID))
		{
			num3 = -1;
		}
		string s3 = FriendsController.sharedController.onlineInfo[fp2.id]["delta"];
		string s4 = FriendsController.sharedController.onlineInfo[fp2.id]["game_mode"];
		int num4 = int.Parse(s3);
		int num5 = int.Parse(s4);
		int num6 = (((float)num4 > FriendsController.onlineDelta || (num5 > 99 && num5 / 100 != (int)ConnectSceneNGUIController.myPlatformConnect && num5 / 100 != 3)) ? 2 : ((num5 <= -1) ? 1 : 0));
		if (FriendsController.sharedController.clanLeaderID != null && fp2.id.Equals(FriendsController.sharedController.clanLeaderID))
		{
			num6 = -1;
		}
		int result;
		int result2;
		if (num3 == num6 && int.TryParse(fp1.id, out result) && int.TryParse(fp2.id, out result2))
		{
			return result - result2;
		}
		return num3 - num6;
	}

	[CompilerGenerated]
	private static GameObject _003CHandleCancellation_003Em__1E(ClanIncomingInvitesController c)
	{
		return c.inboxPanel;
	}

	[CompilerGenerated]
	private static bool _003CHandleCancellation_003Em__1F(GameObject p)
	{
		return p.activeInHierarchy;
	}

	[CompilerGenerated]
	private static void _003CHandleCancellation_003Em__20(ClanIncomingInvitesController c)
	{
		c.HandleBackFromInboxPressed();
	}

	[CompilerGenerated]
	private static bool _003CSetScreenMessage_003Em__23(Task<List<object>> t)
	{
		return t.IsCompleted;
	}

	[CompilerGenerated]
	private static UISprite[] _003CClansGUIController_003Em__24(GameObject c)
	{
		return c.GetComponentsInChildren<UISprite>(true);
	}

	[CompilerGenerated]
	private static UISprite[] _003CClansGUIController_003Em__25(GameObject c)
	{
		return c.GetComponentsInChildren<UISprite>(true);
	}

	[CompilerGenerated]
	private static bool _003CClansGUIController_003Em__26(UISprite s)
	{
		return "NewMessages".Equals(s.name);
	}
}
