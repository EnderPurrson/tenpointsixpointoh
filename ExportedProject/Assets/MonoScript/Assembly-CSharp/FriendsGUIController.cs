using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class FriendsGUIController : MonoBehaviour, IFriendsGUIController
{
	public static Action UpdaeOnlineEvent;

	public GameObject multyButton;

	public GameObject receivingPlashka;

	public UIWrapContent friendsGrid;

	public UIGrid invitationsGrid;

	public UIGrid sentInvitationsGrid;

	public UIGrid ClanInvitationsGrid;

	public LeaderboardsView leaderboardsView;

	public UIPanel friendsPanel;

	public UIPanel inboxPanel;

	public UIPanel friendProfilePanel;

	public UIPanel facebookFriensPanel;

	public UIPanel bestPlayersPanel;

	public GameObject fon;

	public GameObject newMEssage;

	public GameObject canAddLAbel;

	private float timeOfLastSort;

	public static bool ShowProfile;

	private bool invitationsInitialized;

	private float _timeLastFriendsScrollUpdate;

	private FriendProfileController _friendProfileController;

	private LeaderboardsController _leaderboardsController;

	static FriendsGUIController()
	{
	}

	public FriendsGUIController()
	{
	}

	[DebuggerHidden]
	private IEnumerator __UpdateGUI()
	{
		FriendsGUIController.u003c__UpdateGUIu003ec__Iterator77 variable = null;
		return variable;
	}

	private void _SortFriendPreviews()
	{
		FriendPreview[] componentsInChildren = this.friendsGrid.GetComponentsInChildren<FriendPreview>(true);
		FriendPreview[] friendPreviewArray = this.friendsGrid.GetComponentsInChildren<FriendPreview>(false) ?? new FriendPreview[0];
		Array.Sort<FriendPreview>(friendPreviewArray, (FriendPreview fp1, FriendPreview fp2) => fp1.name.CompareTo(fp2.name));
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
			string item1 = FriendsController.sharedController.onlineInfo[fp2.id]["delta"];
			string str1 = FriendsController.sharedController.onlineInfo[fp2.id]["game_mode"];
			int num6 = int.Parse(item1);
			int num7 = int.Parse(str1);
			num1 = ((float)num6 > FriendsController.onlineDelta || num7 > 99 && num7 / 100 != (int)ConnectSceneNGUIController.myPlatformConnect && num7 / 100 != 3 ? 2 : (num7 <= -1 ? 1 : 0));
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

	public void HandleProfileButton()
	{
		if (ProfileController.Instance != null)
		{
			this.Hide(true);
			ProfileController.Instance.ShowInterface(new Action[] { new Action(() => {
				if (ExperienceController.sharedController != null && ExpController.Instance != null)
				{
					ExperienceController.sharedController.isShowRanks = false;
					ExpController.Instance.InterfaceEnabled = false;
				}
				this.Hide(false);
			}) });
		}
	}

	void IFriendsGUIController.Hide(bool h)
	{
		this.friendsPanel.gameObject.SetActive(!h);
		this.fon.SetActive(!h);
		FriendsGUIController.ShowProfile = h;
	}

	public void MultyButtonHandler(object sender, EventArgs e)
	{
		Defs.isFlag = false;
		Defs.isCOOP = false;
		Defs.isMulti = true;
		Defs.isHunger = false;
		Defs.isCompany = false;
		Defs.IsSurvival = false;
		Defs.isFlag = false;
		FlurryPluginWrapper.LogDeathmatchModePress();
		MenuBackgroundMusic.keepPlaying = true;
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.sceneToLoad = "ConnectScene";
		FlurryPluginWrapper.LogEvent("Launch_Multiplayer");
		LoadConnectScene.noteToShow = null;
		Application.LoadLevel(Defs.PromSceneName);
	}

	private void OnDestroy()
	{
		FriendsController.sharedController.StopRefreshingOnline();
		this._friendProfileController.Dispose();
		FriendsGUIController.ShowProfile = false;
	}

	private void OnDisable()
	{
		FriendsController.FriendsUpdated -= new Action(this.UpdateGUI);
	}

	private void OnEnable()
	{
		FriendsController.FriendsUpdated += new Action(this.UpdateGUI);
		base.StartCoroutine(this.__UpdateGUI());
	}

	public static void RaiseUpdaeOnlineEvent()
	{
		if (FriendsGUIController.UpdaeOnlineEvent != null)
		{
			FriendsGUIController.UpdaeOnlineEvent();
		}
	}

	public void RequestLeaderboards()
	{
		if (this._leaderboardsController != null)
		{
			this._leaderboardsController.RequestLeaderboards();
		}
	}

	public void ShowBestPlayers(bool h)
	{
		this.friendsPanel.gameObject.SetActive(!h);
		this.leaderboardsView.gameObject.SetActive(h);
	}

	[DebuggerHidden]
	private IEnumerator SortFriendPreviewsAfterDelay()
	{
		FriendsGUIController.u003cSortFriendPreviewsAfterDelayu003ec__Iterator76 variable = null;
		return variable;
	}

	private void Start()
	{
		StoreKitEventListener.State.Mode = "Friends";
		StoreKitEventListener.State.PurchaseKey = "In friends";
		StoreKitEventListener.State.Parameters.Clear();
		if (this.multyButton != null)
		{
			if (ProtocolListGetter.currentVersionIsSupported)
			{
				ButtonHandler component = this.multyButton.GetComponent<ButtonHandler>();
				if (component != null)
				{
					component.Clicked += new EventHandler(this.MultyButtonHandler);
				}
			}
			else
			{
				this.multyButton.gameObject.SetActive(false);
			}
		}
		this.timeOfLastSort = Time.realtimeSinceStartup;
		Defs.ProfileFromFriends = 0;
		this._friendProfileController = new FriendProfileController(this, true);
		if (this.leaderboardsView != null && this._leaderboardsController == null)
		{
			this._leaderboardsController = this.leaderboardsView.gameObject.AddComponent<LeaderboardsController>();
			this._leaderboardsController.LeaderboardsView = this.leaderboardsView;
			this._leaderboardsController.FriendsGuiController = this;
			this._leaderboardsController.PlayerId = Storager.getString("AccountCreated", false);
		}
		FriendsController.sharedController.StartRefreshingOnline();
		base.StartCoroutine(this.SortFriendPreviewsAfterDelay());
	}

	private void Update()
	{
		if (this.receivingPlashka != null && FriendsController.sharedController != null)
		{
			if (this.friendsPanel != null && this.friendsPanel.gameObject.activeInHierarchy || this.inboxPanel != null && this.inboxPanel.gameObject.activeInHierarchy)
			{
				this.receivingPlashka.SetActive(FriendsController.sharedController.NumberOfFriendsRequests > 0);
				this.receivingPlashka.GetComponent<UILabel>().text = LocalizationStore.Key_0348;
			}
			else if (this._friendProfileController != null && this._friendProfileController.FriendProfileGo != null && this._friendProfileController.FriendProfileGo.activeInHierarchy)
			{
				this.receivingPlashka.SetActive(FriendsController.sharedController.NumberOffFullInfoRequests > 0);
				this.receivingPlashka.GetComponent<UILabel>().text = LocalizationStore.Key_0348;
			}
			else if (!(this.leaderboardsView != null) || !this.leaderboardsView.gameObject.activeInHierarchy)
			{
				this.receivingPlashka.SetActive(false);
			}
			else
			{
				this.receivingPlashka.SetActive(FriendsController.sharedController.NumberOfBestPlayersRequests > 0);
				this.receivingPlashka.GetComponent<UILabel>().text = LocalizationStore.Key_0348;
			}
		}
		this.friendsGrid.transform.parent.GetComponent<UIScrollView>().enabled = this.friendsGrid.transform.childCount > 4;
		if (this.friendsGrid.transform.childCount > 0 && this.friendsGrid.transform.childCount <= 4 && Time.realtimeSinceStartup - this._timeLastFriendsScrollUpdate > 0.5f)
		{
			this._timeLastFriendsScrollUpdate = Time.realtimeSinceStartup;
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
			if (FriendsGUIController.UpdaeOnlineEvent != null)
			{
				FriendsGUIController.UpdaeOnlineEvent();
			}
			this.timeOfLastSort = Time.realtimeSinceStartup;
			this._SortFriendPreviews();
		}
		this.newMEssage.SetActive((FriendsController.sharedController.invitesToUs.Count > 0 ? true : FriendsController.sharedController.ClanInvites.Count > 0));
		this.canAddLAbel.SetActive(FriendsController.sharedController.friends.Count == 0);
	}

	public void UpdateGUI()
	{
		base.StartCoroutine(this.__UpdateGUI());
	}
}