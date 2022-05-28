using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class FriendsGUIController : MonoBehaviour, IFriendsGUIController
{
	[CompilerGenerated]
	private sealed class _003CHandleProfileButton_003Ec__AnonStorey1F4
	{
		internal IFriendsGUIController hidable;

		internal void _003C_003Em__59()
		{
			if (ExperienceController.sharedController != null && ExpController.Instance != null)
			{
				ExperienceController.sharedController.isShowRanks = false;
				ExpController.Instance.InterfaceEnabled = false;
			}
			hidable.Hide(false);
		}
	}

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

	[CompilerGenerated]
	private static Comparison<FriendPreview> _003C_003Ef__am_0024cache16;

	[CompilerGenerated]
	private static Comparison<FriendPreview> _003C_003Ef__am_0024cache17;

	void IFriendsGUIController.Hide(bool h)
	{
		friendsPanel.gameObject.SetActive(!h);
		fon.SetActive(!h);
		ShowProfile = h;
	}

	public static void RaiseUpdaeOnlineEvent()
	{
		if (UpdaeOnlineEvent != null)
		{
			UpdaeOnlineEvent();
		}
	}

	public void HandleProfileButton()
	{
		if (ProfileController.Instance != null)
		{
			_003CHandleProfileButton_003Ec__AnonStorey1F4 _003CHandleProfileButton_003Ec__AnonStorey1F = new _003CHandleProfileButton_003Ec__AnonStorey1F4();
			_003CHandleProfileButton_003Ec__AnonStorey1F.hidable = this;
			_003CHandleProfileButton_003Ec__AnonStorey1F.hidable.Hide(true);
			ProfileController.Instance.ShowInterface(_003CHandleProfileButton_003Ec__AnonStorey1F._003C_003Em__59);
		}
	}

	public void ShowBestPlayers(bool h)
	{
		friendsPanel.gameObject.SetActive(!h);
		leaderboardsView.gameObject.SetActive(h);
	}

	public void RequestLeaderboards()
	{
		if (_leaderboardsController != null)
		{
			_leaderboardsController.RequestLeaderboards();
		}
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

	private void Start()
	{
		StoreKitEventListener.State.Mode = "Friends";
		StoreKitEventListener.State.PurchaseKey = "In friends";
		StoreKitEventListener.State.Parameters.Clear();
		if (multyButton != null)
		{
			if (!ProtocolListGetter.currentVersionIsSupported)
			{
				multyButton.gameObject.SetActive(false);
			}
			else
			{
				ButtonHandler component = multyButton.GetComponent<ButtonHandler>();
				if (component != null)
				{
					component.Clicked += MultyButtonHandler;
				}
			}
		}
		timeOfLastSort = Time.realtimeSinceStartup;
		Defs.ProfileFromFriends = 0;
		_friendProfileController = new FriendProfileController(this);
		if (leaderboardsView != null && _leaderboardsController == null)
		{
			_leaderboardsController = leaderboardsView.gameObject.AddComponent<LeaderboardsController>();
			_leaderboardsController.LeaderboardsView = leaderboardsView;
			_leaderboardsController.FriendsGuiController = this;
			_leaderboardsController.PlayerId = Storager.getString("AccountCreated", false);
		}
		FriendsController.sharedController.StartRefreshingOnline();
		StartCoroutine(SortFriendPreviewsAfterDelay());
	}

	private void OnEnable()
	{
		FriendsController.FriendsUpdated += UpdateGUI;
		StartCoroutine(__UpdateGUI());
	}

	public void UpdateGUI()
	{
		StartCoroutine(__UpdateGUI());
	}

	private IEnumerator SortFriendPreviewsAfterDelay()
	{
		yield return null;
		yield return null;
		_SortFriendPreviews();
	}

	private void _SortFriendPreviews()
	{
		FriendPreview[] componentsInChildren = friendsGrid.GetComponentsInChildren<FriendPreview>(true);
		FriendPreview[] array = friendsGrid.GetComponentsInChildren<FriendPreview>(false);
		if (array == null)
		{
			array = new FriendPreview[0];
		}
		FriendPreview[] array2 = array;
		if (_003C_003Ef__am_0024cache16 == null)
		{
			_003C_003Ef__am_0024cache16 = _003C_SortFriendPreviews_003Em__5A;
		}
		Array.Sort(array2, _003C_003Ef__am_0024cache16);
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
		if (_003C_003Ef__am_0024cache17 == null)
		{
			_003C_003Ef__am_0024cache17 = _003C_SortFriendPreviews_003Em__5B;
		}
		Array.Sort(componentsInChildren, _003C_003Ef__am_0024cache17);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.name = i.ToString("D7");
		}
		friendsGrid.SortAlphabetically();
		friendsGrid.WrapContent();
		Transform transform = null;
		if (text != null)
		{
			FriendPreview[] array3 = componentsInChildren;
			foreach (FriendPreview friendPreview in array3)
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
		FriendPreview[] fps = friendsGrid.GetComponentsInChildren<FriendPreview>(true);
		Invitation[] invs = invitationsGrid.GetComponentsInChildren<Invitation>(true);
		Invitation[] sentInvs = sentInvitationsGrid.GetComponentsInChildren<Invitation>(true);
		Invitation[] clanInvs = ClanInvitationsGrid.GetComponentsInChildren<Invitation>(true);
		List<Invitation> clanInvtoRemove = new List<Invitation>();
		List<string> existingClanInvs = new List<string>();
		Invitation[] array = clanInvs;
		foreach (Invitation i in array)
		{
			bool found = false;
			foreach (Dictionary<string, string> ClanInv in FriendsController.sharedController.ClanInvites)
			{
			}
			if (!found)
			{
				clanInvtoRemove.Add(i);
			}
			else if (i.id != null)
			{
				existingClanInvs.Add(i.id);
			}
		}
		foreach (Invitation inv in clanInvtoRemove)
		{
			inv.transform.parent = null;
			UnityEngine.Object.Destroy(inv.gameObject);
		}
		foreach (Dictionary<string, string> ClanInv2 in FriendsController.sharedController.ClanInvites)
		{
			if (!existingClanInvs.Contains(ClanInv2["id"]))
			{
				GameObject f = UnityEngine.Object.Instantiate(Resources.Load("Invitation") as GameObject);
				f.transform.parent = ClanInvitationsGrid.transform;
				f.transform.localScale = new Vector3(1f, 1f, 1f);
				f.GetComponent<Invitation>().IsClanInv = true;
				if (ClanInv2.ContainsKey("id"))
				{
					f.GetComponent<Invitation>().id = ClanInv2["id"];
					f.GetComponent<Invitation>().recordId = ClanInv2["id"];
				}
				if (ClanInv2.ContainsKey("name"))
				{
					f.GetComponent<Invitation>().nm.text = ClanInv2["name"];
				}
				string clanLogo;
				if (ClanInv2.TryGetValue("logo", out clanLogo) && !string.IsNullOrEmpty(clanLogo))
				{
					f.GetComponent<Invitation>().clanLogoString = clanLogo;
				}
			}
		}
		yield return null;
		invitationsGrid.Reposition();
		sentInvitationsGrid.Reposition();
		ClanInvitationsGrid.Reposition();
		timeOfLastSort = Time.realtimeSinceStartup;
		_SortFriendPreviews();
	}

	private void Update()
	{
		if (receivingPlashka != null && FriendsController.sharedController != null)
		{
			if ((friendsPanel != null && friendsPanel.gameObject.activeInHierarchy) || (inboxPanel != null && inboxPanel.gameObject.activeInHierarchy))
			{
				receivingPlashka.SetActive(FriendsController.sharedController.NumberOfFriendsRequests > 0);
				receivingPlashka.GetComponent<UILabel>().text = LocalizationStore.Key_0348;
			}
			else if (_friendProfileController != null && _friendProfileController.FriendProfileGo != null && _friendProfileController.FriendProfileGo.activeInHierarchy)
			{
				receivingPlashka.SetActive(FriendsController.sharedController.NumberOffFullInfoRequests > 0);
				receivingPlashka.GetComponent<UILabel>().text = LocalizationStore.Key_0348;
			}
			else if (leaderboardsView != null && leaderboardsView.gameObject.activeInHierarchy)
			{
				receivingPlashka.SetActive(FriendsController.sharedController.NumberOfBestPlayersRequests > 0);
				receivingPlashka.GetComponent<UILabel>().text = LocalizationStore.Key_0348;
			}
			else
			{
				receivingPlashka.SetActive(false);
			}
		}
		friendsGrid.transform.parent.GetComponent<UIScrollView>().enabled = friendsGrid.transform.childCount > 4;
		if (friendsGrid.transform.childCount > 0 && friendsGrid.transform.childCount <= 4 && Time.realtimeSinceStartup - _timeLastFriendsScrollUpdate > 0.5f)
		{
			_timeLastFriendsScrollUpdate = Time.realtimeSinceStartup;
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
			if (UpdaeOnlineEvent != null)
			{
				UpdaeOnlineEvent();
			}
			timeOfLastSort = Time.realtimeSinceStartup;
			_SortFriendPreviews();
		}
		newMEssage.SetActive(FriendsController.sharedController.invitesToUs.Count > 0 || FriendsController.sharedController.ClanInvites.Count > 0);
		canAddLAbel.SetActive(FriendsController.sharedController.friends.Count == 0);
	}

	private void OnDisable()
	{
		FriendsController.FriendsUpdated -= UpdateGUI;
	}

	private void OnDestroy()
	{
		FriendsController.sharedController.StopRefreshingOnline();
		_friendProfileController.Dispose();
		ShowProfile = false;
	}

	[CompilerGenerated]
	private static int _003C_SortFriendPreviews_003Em__5A(FriendPreview fp1, FriendPreview fp2)
	{
		return fp1.name.CompareTo(fp2.name);
	}

	[CompilerGenerated]
	private static int _003C_SortFriendPreviews_003Em__5B(FriendPreview fp1, FriendPreview fp2)
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
		string s3 = FriendsController.sharedController.onlineInfo[fp2.id]["delta"];
		string s4 = FriendsController.sharedController.onlineInfo[fp2.id]["game_mode"];
		int num4 = int.Parse(s3);
		int num5 = int.Parse(s4);
		int num6 = (((float)num4 > FriendsController.onlineDelta || (num5 > 99 && num5 / 100 != (int)ConnectSceneNGUIController.myPlatformConnect && num5 / 100 != 3)) ? 2 : ((num5 <= -1) ? 1 : 0));
		int result;
		int result2;
		if (num3 == num6 && int.TryParse(fp1.id, out result) && int.TryParse(fp2.id, out result2))
		{
			return result - result2;
		}
		return num3 - num6;
	}
}
