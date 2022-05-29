using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FriendsWindowController : MonoBehaviour
{
	public static Action UpdateFriendsOnlineEvent;

	public UIButton goInBattleButton;

	public UIWrapContent friendsListWrap;

	public UIWrapContent inviteFriendsWrap;

	public UIWrapContent findFriendsWrap;

	private string[] inviteFriendsList;

	private string[] findFriendsList;

	private string[] myFriendsList;

	private string[] localFriendsList;

	public GameObject friendPreviewPrefab;

	public UIToggle chatTab;

	public UIToggle friendsTab;

	public FriendsWindowStatusBar statusBar;

	public UILabel emptyStateTabLabel;

	public GameObject emptyStateLocalPlayersTabLabel;

	public GameObject chatContainer;

	public GameObject joinToFriendRoomPhoton;

	public static FriendsWindowController Instance;

	private FriendsWindowController.WindowState currentWindowState;

	private bool _isAnyFriendsDataExists;

	private bool _isFriendsMax;

	private bool wrapsInit;

	[NonSerialized]
	public bool isNeedRebuildFindFriendsList = true;

	private FriendPreviewItem _selectProfileItem;

	private bool _isWindowInStartState;

	public bool NeedUpdateCurrentFriendsList
	{
		get;
		set;
	}

	public FriendsWindowController()
	{
	}

	private void Awake()
	{
		FriendsWindowController.Instance = this;
	}

	private void CheckShowEmptyStateTabLabel(bool isListNotEmpty, bool isFriendsMax)
	{
		if (isListNotEmpty)
		{
			if (this.currentWindowState == FriendsWindowController.WindowState.chat)
			{
				this.chatContainer.SetActive(true);
			}
			this.HideMessageByEmptyStateTab();
			return;
		}
		if (this.currentWindowState == FriendsWindowController.WindowState.chat)
		{
			this.ShowMessageByEmptyStateTab(LocalizationStore.Get("Key_1557"));
			this.chatContainer.SetActive(false);
		}
		else if (isFriendsMax)
		{
			this.ShowMessageByEmptyStateTab(LocalizationStore.Get("Key_1424"));
		}
		else if (this.currentWindowState == FriendsWindowController.WindowState.inbox)
		{
			this.ShowMessageByEmptyStateTab(LocalizationStore.Get("Key_1425"));
		}
		else if (this.currentWindowState != FriendsWindowController.WindowState.friendList)
		{
			this.ShowMessageByEmptyStateTab(LocalizationStore.Get("Key_1423"));
		}
		else
		{
			this.ShowMessageByEmptyStateTab(LocalizationStore.Get("Key_0223"));
		}
	}

	private void EventUpdateFriendsOnlineAndSorting()
	{
		if (this.currentWindowState != FriendsWindowController.WindowState.friendList)
		{
			return;
		}
		this.UpdateFriendsOnlineAndSorting(false);
	}

	private UIWrapContent GetCurrentWrap()
	{
		switch (this.currentWindowState)
		{
			case FriendsWindowController.WindowState.friendList:
			{
				return this.friendsListWrap;
			}
			case FriendsWindowController.WindowState.findFriends:
			{
				return this.findFriendsWrap;
			}
			case FriendsWindowController.WindowState.chat:
			{
				return null;
			}
			case FriendsWindowController.WindowState.inbox:
			{
				return this.inviteFriendsWrap;
			}
		}
		return null;
	}

	private string GetItemFromCurrentState(int index)
	{
		switch (this.currentWindowState)
		{
			case FriendsWindowController.WindowState.friendList:
			{
				if (index < (int)this.localFriendsList.Length)
				{
					return this.localFriendsList[index];
				}
				if (index - (int)this.localFriendsList.Length >= (int)this.myFriendsList.Length)
				{
					return string.Empty;
				}
				return this.myFriendsList[index - (int)this.localFriendsList.Length];
			}
			case FriendsWindowController.WindowState.findFriends:
			{
				if (index >= (int)this.findFriendsList.Length || this._isFriendsMax)
				{
					return string.Empty;
				}
				return this.findFriendsList[index];
			}
			case FriendsWindowController.WindowState.chat:
			{
				if (index >= (int)this.myFriendsList.Length)
				{
					return string.Empty;
				}
				return this.myFriendsList[index];
			}
			case FriendsWindowController.WindowState.inbox:
			{
				if (index >= (int)this.inviteFriendsList.Length)
				{
					return string.Empty;
				}
				return this.inviteFriendsList[index];
			}
		}
		return null;
	}

	private int GetLenghtFromCurrentList()
	{
		switch (this.currentWindowState)
		{
			case FriendsWindowController.WindowState.friendList:
			{
				return (this.localFriendsList == null ? 0 : (int)this.localFriendsList.Length) + (int)this.myFriendsList.Length;
			}
			case FriendsWindowController.WindowState.findFriends:
			{
				return (this.findFriendsList == null ? 0 : (int)this.findFriendsList.Length);
			}
			case FriendsWindowController.WindowState.chat:
			{
				return 0;
			}
			case FriendsWindowController.WindowState.inbox:
			{
				return (this.inviteFriendsList == null ? 0 : (int)this.inviteFriendsList.Length);
			}
			default:
			{
				return 0;
			}
		}
	}

	private string GetLocalPlayerItem(int index)
	{
		if (index >= (int)this.localFriendsList.Length)
		{
			return string.Empty;
		}
		return this.localFriendsList[index];
	}

	private int GetMinForCurrentState()
	{
		return 4;
	}

	private int GetModeByInfo(Dictionary<string, string> onlineData)
	{
		int num = Convert.ToInt32(onlineData["game_mode"]);
		if (num == 6)
		{
			return 1;
		}
		if (num == 7)
		{
			return 2;
		}
		return 0;
	}

	private FriendItemPreviewType GetPreviewTypeForCurrentWindowState()
	{
		switch (this.currentWindowState)
		{
			case FriendsWindowController.WindowState.friendList:
			{
				return FriendItemPreviewType.view;
			}
			case FriendsWindowController.WindowState.findFriends:
			{
				return FriendItemPreviewType.find;
			}
			case FriendsWindowController.WindowState.chat:
			{
				return FriendItemPreviewType.none;
			}
			case FriendsWindowController.WindowState.inbox:
			{
				return FriendItemPreviewType.inbox;
			}
			default:
			{
				return FriendItemPreviewType.none;
			}
		}
	}

	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	public void HideInfoBox()
	{
		InfoWindowController.HideProcessing();
	}

	private void HideMessageByEmptyStateTab()
	{
		this.emptyStateTabLabel.gameObject.SetActive(false);
	}

	public static bool IsActiveFindFriendTab()
	{
		if (FriendsWindowController.Instance == null)
		{
			return false;
		}
		return FriendsWindowController.Instance.currentWindowState == FriendsWindowController.WindowState.findFriends;
	}

	public static bool IsActiveFriendListTab()
	{
		if (FriendsWindowController.Instance == null)
		{
			return false;
		}
		return FriendsWindowController.Instance.currentWindowState == FriendsWindowController.WindowState.friendList;
	}

	public void OnClickClearAllInboxButton()
	{
		ButtonClickSound.TryPlayClick();
		this.statusBar.clearAllInviteButton.isEnabled = false;
		FriendsController friendsController = FriendsController.sharedController;
		if (friendsController != null)
		{
			friendsController.ClearAllFriendsInvites();
		}
	}

	public void OnClickFindFriendsTab(UIToggle toggle)
	{
		if (toggle.@value)
		{
			ButtonClickSound.TryPlayClick();
		}
		if (!toggle.@value)
		{
			return;
		}
		this.HideInfoBox();
		this.NeedUpdateCurrentFriendsList = true;
		if (this.statusBar.IsFindFriendByIdStateActivate)
		{
			this.UpdateFriendsArray(FriendsWindowController.WindowState.findFriends, false);
			this.ResetWrapPosition(this.findFriendsWrap);
		}
		this.SetActiveFindFriendsContainer();
		this.statusBar.findFriendInput.@value = string.Empty;
	}

	public void OnClickFriendListTab(UIToggle toggle)
	{
		if (toggle.@value)
		{
			ButtonClickSound.TryPlayClick();
		}
		if (!toggle.@value)
		{
			return;
		}
		this.HideInfoBox();
		this.NeedUpdateCurrentFriendsList = true;
		this.SetActiveFriendsListContainer();
	}

	public void OnClickFriendsChatTab(UIToggle toggle)
	{
		if (toggle.@value)
		{
			ButtonClickSound.TryPlayClick();
		}
		if (!toggle.@value)
		{
			return;
		}
		this.HideInfoBox();
		this.SetActiveChatFriendsContainer();
	}

	public void OnClickGoInBattleButton()
	{
		ButtonClickSound.TryPlayClick();
		GlobalGameController.GoInBattle();
	}

	public void OnClickInboxFriendsTab(UIToggle toggle)
	{
		if (toggle.@value)
		{
			ButtonClickSound.TryPlayClick();
		}
		if (!toggle.@value)
		{
			return;
		}
		this.HideInfoBox();
		this.SetActiveInboxFriendsContainer();
	}

	private void OnCloseProfileWindow(bool needUpdateFriendList)
	{
		if (needUpdateFriendList)
		{
			this.NeedUpdateCurrentFriendsList = true;
		}
		base.gameObject.SetActive(true);
		this.joinToFriendRoomPhoton.SetActive(true);
		if (this._selectProfileItem != null)
		{
			this._selectProfileItem.UpdateData();
		}
		this.UpdateCurrentTabState();
	}

	private void OnDestroy()
	{
		FriendsController.sharedController.StopRefreshingOnline();
		FriendsController.DisposeProfile();
		FriendsWindowController.Instance = null;
	}

	private void OnDisable()
	{
		FriendsController.FriendsUpdated -= new Action(this.UpdateFriendsListInterface);
		FriendsController.OnShowBoxProcessFriendsData -= new Action(this.ShowMessageBoxProcessingData);
		FriendsController.OnHideBoxProcessFriendsData -= new Action(this.HideInfoBox);
		FriendsController.UpdateFriendsInfoAction -= new Action(this.EventUpdateFriendsOnlineAndSorting);
		InfoWindowController.HideCurrentWindow();
	}

	private void OnEnable()
	{
		FriendsController.FriendsUpdated += new Action(this.UpdateFriendsListInterface);
		FriendsController.OnShowBoxProcessFriendsData += new Action(this.ShowMessageBoxProcessingData);
		FriendsController.OnHideBoxProcessFriendsData += new Action(this.HideInfoBox);
		FriendsController.UpdateFriendsInfoAction += new Action(this.EventUpdateFriendsOnlineAndSorting);
	}

	private void OnFindFriendsItemWrap(GameObject go, int wrapInd, int realInd)
	{
		go.GetComponent<FriendPreviewItem>().myWrapIndex = Mathf.Abs(realInd);
		if (this.currentWindowState == FriendsWindowController.WindowState.findFriends)
		{
			this.updateItemInfo(go.GetComponent<FriendPreviewItem>(), FriendItemPreviewType.find);
		}
	}

	private void OnFriendsItemWrap(GameObject go, int wrapInd, int realInd)
	{
		go.GetComponent<FriendPreviewItem>().myWrapIndex = Mathf.Abs(realInd);
		if (this.currentWindowState == FriendsWindowController.WindowState.friendList)
		{
			this.updateItemInfo(go.GetComponent<FriendPreviewItem>(), FriendItemPreviewType.view);
		}
	}

	private void OnIniviteItemWrap(GameObject go, int wrapInd, int realInd)
	{
		go.GetComponent<FriendPreviewItem>().myWrapIndex = Mathf.Abs(realInd);
		if (this.currentWindowState == FriendsWindowController.WindowState.inbox)
		{
			this.updateItemInfo(go.GetComponent<FriendPreviewItem>(), FriendItemPreviewType.inbox);
		}
	}

	private void OnLocalFriendsItemWrap(GameObject go, int wrapInd, int realInd)
	{
		go.GetComponent<FriendPreviewItem>().myWrapIndex = Mathf.Abs(realInd);
		if (this.currentWindowState == FriendsWindowController.WindowState.friendList)
		{
			this.updateItemInfo(go.GetComponent<FriendPreviewItem>(), FriendItemPreviewType.find);
		}
	}

	private void ResetWrapPosition(UIWrapContent wrap)
	{
		wrap.SortAlphabetically();
		wrap.transform.parent.GetComponent<UIScrollView>().ResetPosition();
	}

	private void SetActiveChatFriendsContainer()
	{
		this.currentWindowState = FriendsWindowController.WindowState.chat;
		bool flag = FriendsController.IsFriendsDataExist();
		this.CheckShowEmptyStateTabLabel(flag, false);
		this.chatContainer.SetActive(flag);
		this.statusBar.OnClickChatTab();
	}

	public void SetActiveChatTab(string id)
	{
		if (PrivateChatController.sharedController != null)
		{
			PrivateChatController.sharedController.selectedPlayerID = id;
		}
		this.chatTab.Set(true);
		this.SetActiveChatFriendsContainer();
	}

	private void SetActiveFindFriendsContainer()
	{
		this.currentWindowState = FriendsWindowController.WindowState.findFriends;
		this.UpdateFindFriendsState();
	}

	private void SetActiveFriendsListContainer()
	{
		this.currentWindowState = FriendsWindowController.WindowState.friendList;
		this.UpdateFriendsListState();
		if (!this._isAnyFriendsDataExists)
		{
			this._isWindowInStartState = false;
			return;
		}
		if (this.NeedUpdateCurrentFriendsList)
		{
			base.StartCoroutine(this.UpdateCurrentFriendsList());
		}
		this._isWindowInStartState = false;
	}

	private void SetActiveInboxFriendsContainer()
	{
		this.currentWindowState = FriendsWindowController.WindowState.inbox;
		this.UpdateFriendsInboxState();
	}

	public void SetCancelState()
	{
		this.friendsTab.Set(false);
	}

	public void SetStartState()
	{
		base.StartCoroutine(this.SetStartStateCoroutine());
	}

	[DebuggerHidden]
	private IEnumerator SetStartStateCoroutine()
	{
		FriendsWindowController.u003cSetStartStateCoroutineu003ec__Iterator140 variable = null;
		return variable;
	}

	public void Show()
	{
		base.gameObject.SetActive(true);
	}

	public void ShowMessageBoxProcessingData()
	{
		InfoWindowController.ShowProcessingDataBox();
	}

	private void ShowMessageByEmptyStateTab(string text)
	{
		this.emptyStateTabLabel.gameObject.SetActive(true);
		this.emptyStateTabLabel.text = text;
	}

	public void ShowProfileWindow(string friendId, FriendPreviewItem selectedItem)
	{
		this._selectProfileItem = selectedItem;
		base.gameObject.SetActive(false);
		this.joinToFriendRoomPhoton.SetActive(false);
		FriendsController.ShowProfile(friendId, ProfileWindowType.friend, new Action<bool>(this.OnCloseProfileWindow));
	}

	[DebuggerHidden]
	public IEnumerator ShowResultFindPlayer(string param)
	{
		FriendsWindowController.u003cShowResultFindPlayeru003ec__Iterator13F variable = null;
		return variable;
	}

	private int SortFriendsByFindOrigin(string player1, string player2)
	{
		int possibleFriendFindOrigin = (int)FriendsController.GetPossibleFriendFindOrigin(player1);
		int num = (int)FriendsController.GetPossibleFriendFindOrigin(player2);
		if (possibleFriendFindOrigin < num)
		{
			return -1;
		}
		if (possibleFriendFindOrigin > num)
		{
			return 1;
		}
		return 0;
	}

	private int SortFriendsByOnlineStatusAndClickJoin(string friend1, string friend2)
	{
		int num;
		int num1;
		if (FriendsController.sharedController.onlineInfo.ContainsKey(friend1))
		{
			Dictionary<string, string> item = FriendsController.sharedController.onlineInfo[friend1];
			num = (item != null ? this.GetModeByInfo(item) : 3);
		}
		else
		{
			num = 3;
		}
		if (FriendsController.sharedController.onlineInfo.ContainsKey(friend2))
		{
			Dictionary<string, string> strs = FriendsController.sharedController.onlineInfo[friend2];
			num1 = (strs != null ? this.GetModeByInfo(strs) : 3);
		}
		else
		{
			num1 = 3;
		}
		if (num < num1)
		{
			return -1;
		}
		if (num > num1)
		{
			return 1;
		}
		FriendsController friendsController = FriendsController.sharedController;
		if (friendsController == null)
		{
			return 0;
		}
		DateTime dateLastClickJoinFriend = friendsController.GetDateLastClickJoinFriend(friend1);
		DateTime dateTime = friendsController.GetDateLastClickJoinFriend(friend2);
		if (dateLastClickJoinFriend < dateTime)
		{
			return -1;
		}
		if (dateLastClickJoinFriend > dateTime)
		{
			return 1;
		}
		return 0;
	}

	private void Start()
	{
		this.currentWindowState = FriendsWindowController.WindowState.friendList;
	}

	public void UpdateCurrentFriendsArrayAndItems()
	{
		this.UpdateFriendsArray(this.currentWindowState, false);
		UIWrapContent currentWrap = this.GetCurrentWrap();
		if (currentWrap == null)
		{
			return;
		}
		this.UpdateList(currentWrap, this.GetPreviewTypeForCurrentWindowState());
		UIScrollView component = currentWrap.transform.parent.GetComponent<UIScrollView>();
		int minForCurrentState = this.GetMinForCurrentState();
		int lenghtFromCurrentList = this.GetLenghtFromCurrentList();
		if (lenghtFromCurrentList > minForCurrentState && component.transform.localPosition.y + (float)(currentWrap.itemSize * minForCurrentState) > (float)(lenghtFromCurrentList * currentWrap.itemSize))
		{
			component.MoveRelative(Vector3.down * (float)currentWrap.itemSize);
		}
	}

	[DebuggerHidden]
	private IEnumerator UpdateCurrentFriendsList()
	{
		FriendsWindowController.u003cUpdateCurrentFriendsListu003ec__Iterator13E variable = null;
		return variable;
	}

	public void UpdateCurrentTabState()
	{
		if (this.currentWindowState == FriendsWindowController.WindowState.friendList)
		{
			this.SetActiveFriendsListContainer();
		}
		else if (this.currentWindowState == FriendsWindowController.WindowState.inbox)
		{
			this.SetActiveInboxFriendsContainer();
		}
		else if (this.currentWindowState == FriendsWindowController.WindowState.findFriends)
		{
			this.SetActiveFindFriendsContainer();
		}
	}

	private void UpdateFindFriendsState()
	{
		FriendsController friendsController = FriendsController.sharedController;
		this._isFriendsMax = FriendsController.IsFriendsMax();
		if (!this.statusBar.IsFindFriendByIdStateActivate)
		{
			this._isAnyFriendsDataExists = (!FriendsController.IsPossibleFriendsDataExist() ? false : !this._isFriendsMax);
		}
		else
		{
			this._isAnyFriendsDataExists = !this._isFriendsMax;
		}
		this.statusBar.UpdateFindFriendsState(this._isFriendsMax);
		this.CheckShowEmptyStateTabLabel(this._isAnyFriendsDataExists, this._isFriendsMax);
		this.UpdateList(this.findFriendsWrap, FriendItemPreviewType.find);
	}

	private void UpdateFriendsArray(FriendsWindowController.WindowState state, bool resetScroll = false)
	{
		switch (state)
		{
			case FriendsWindowController.WindowState.friendList:
			{
				List<string> strs = new List<string>();
				for (int i = 0; i < FriendsController.sharedController.friends.Count; i++)
				{
					string item = FriendsController.sharedController.friends[i];
					if (FriendsController.sharedController.friendsInfo.ContainsKey(item))
					{
						strs.Add(item);
					}
				}
				strs.Sort(new Comparison<string>(this.SortFriendsByOnlineStatusAndClickJoin));
				this.myFriendsList = strs.ToArray();
				if (this.localFriendsList != null)
				{
					this.friendsListWrap.minIndex = ((int)this.myFriendsList.Length + (int)this.localFriendsList.Length) * -1;
				}
				else
				{
					this.friendsListWrap.minIndex = (int)this.myFriendsList.Length * -1;
				}
				if (resetScroll)
				{
					this.friendsListWrap.transform.parent.GetComponent<UIScrollView>().ResetPosition();
				}
				return;
			}
			case FriendsWindowController.WindowState.findFriends:
			{
				List<string> strs1 = new List<string>();
				foreach (KeyValuePair<string, FriendsController.PossiblleOrigin> keyValuePair in FriendsController.sharedController.getPossibleFriendsResult)
				{
					string key = keyValuePair.Key;
					if (!FriendsController.sharedController.profileInfo.ContainsKey(key))
					{
						continue;
					}
					strs1.Add(key);
				}
				this.findFriendsList = strs1.ToArray();
				Array.Sort<string>(this.findFriendsList, new Comparison<string>(this.SortFriendsByFindOrigin));
				this.findFriendsWrap.minIndex = (int)this.findFriendsList.Length * -1;
				if (resetScroll)
				{
					this.findFriendsWrap.transform.parent.GetComponent<UIScrollView>().ResetPosition();
				}
				return;
			}
			case FriendsWindowController.WindowState.chat:
			{
				return;
			}
			case FriendsWindowController.WindowState.inbox:
			{
				List<string> strs2 = new List<string>();
				for (int j = 0; j < FriendsController.sharedController.invitesToUs.Count; j++)
				{
					string str = FriendsController.sharedController.invitesToUs[j];
					if (FriendsController.sharedController.profileInfo.ContainsKey(str))
					{
						strs2.Add(str);
					}
				}
				this.inviteFriendsList = strs2.ToArray();
				this.inviteFriendsWrap.minIndex = (int)this.inviteFriendsList.Length * -1;
				if (resetScroll)
				{
					this.inviteFriendsWrap.transform.parent.GetComponent<UIScrollView>().ResetPosition();
				}
				return;
			}
			default:
			{
				return;
			}
		}
	}

	private void UpdateFriendsInboxState()
	{
		this.UpdateFriendsArray(FriendsWindowController.WindowState.inbox, false);
		this.UpdateList(this.inviteFriendsWrap, FriendItemPreviewType.inbox);
		this._isAnyFriendsDataExists = FriendsController.IsFriendInvitesDataExist();
		this._isFriendsMax = FriendsController.IsFriendsMax();
		this.statusBar.OnClickInboxFriendsTab(this._isAnyFriendsDataExists, this._isFriendsMax);
		this.CheckShowEmptyStateTabLabel(this._isAnyFriendsDataExists, false);
	}

	private void UpdateFriendsListInterface()
	{
		this.UpdateFriendsListState();
		this.NeedUpdateCurrentFriendsList = true;
		if (this.currentWindowState == FriendsWindowController.WindowState.inbox)
		{
			this.NeedUpdateCurrentFriendsList = true;
			this.UpdateFriendsInboxState();
		}
		else if (this.currentWindowState == FriendsWindowController.WindowState.findFriends)
		{
			this.NeedUpdateCurrentFriendsList = true;
			if (!this.statusBar.IsFindFriendByIdStateActivate)
			{
				this.UpdateFindFriendsState();
			}
		}
		if (this.NeedUpdateCurrentFriendsList && (this.currentWindowState != FriendsWindowController.WindowState.findFriends || !this.statusBar.IsFindFriendByIdStateActivate))
		{
			base.StartCoroutine(this.UpdateCurrentFriendsList());
		}
	}

	private void UpdateFriendsListState()
	{
		bool flag;
		if (this.myFriendsList != null)
		{
			this.UpdateList(this.friendsListWrap, FriendItemPreviewType.view);
		}
		this._isAnyFriendsDataExists = FriendsController.IsFriendsOrLocalDataExist();
		this._isFriendsMax = FriendsController.IsFriendsMax();
		if (this.currentWindowState != FriendsWindowController.WindowState.chat)
		{
			this.CheckShowEmptyStateTabLabel(this._isAnyFriendsDataExists, this._isFriendsMax);
		}
		else
		{
			this.CheckShowEmptyStateTabLabel(FriendsController.IsFriendsDataExist(), this._isFriendsMax);
		}
		flag = (this._isAnyFriendsDataExists ? false : ProtocolListGetter.currentVersionIsSupported);
		this.goInBattleButton.gameObject.SetActive(flag);
		if (this.currentWindowState == FriendsWindowController.WindowState.friendList)
		{
			this.statusBar.UpdateFriendListState(this._isFriendsMax);
		}
	}

	private void UpdateFriendsOnlineAndSorting(bool isNeedReposition)
	{
		if (FriendsWindowController.UpdateFriendsOnlineEvent != null)
		{
			FriendsWindowController.UpdateFriendsOnlineEvent();
		}
		if (this.currentWindowState == FriendsWindowController.WindowState.friendList)
		{
			this.UpdateFriendsArray(FriendsWindowController.WindowState.friendList, false);
			this.UpdateList(this.GetCurrentWrap(), this.GetPreviewTypeForCurrentWindowState());
		}
	}

	private void updateItemInfo(FriendPreviewItem previewItem, FriendItemPreviewType _typeItems)
	{
		string itemFromCurrentState = this.GetItemFromCurrentState(previewItem.myWrapIndex);
		if (!string.IsNullOrEmpty(itemFromCurrentState))
		{
			if (_typeItems != FriendItemPreviewType.view || previewItem.myWrapIndex >= (int)this.localFriendsList.Length)
			{
				previewItem.FillData(itemFromCurrentState, _typeItems);
			}
			else
			{
				previewItem.FillData(itemFromCurrentState, FriendItemPreviewType.find);
			}
			previewItem.gameObject.SetActive(false);
			previewItem.gameObject.SetActive(true);
		}
		else if (previewItem.gameObject.activeSelf)
		{
			previewItem.gameObject.SetActive(false);
		}
	}

	private void UpdateList(UIWrapContent _wrap, FriendItemPreviewType _typeItems)
	{
		if (_wrap == null)
		{
			return;
		}
		FriendPreviewItem[] componentsInChildren = _wrap.GetComponentsInChildren<FriendPreviewItem>(true);
		bool flag = false;
		int minForCurrentState = this.GetMinForCurrentState();
		int lenghtFromCurrentList = this.GetLenghtFromCurrentList();
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			if (lenghtFromCurrentList != 0)
			{
				if (lenghtFromCurrentList <= minForCurrentState)
				{
					componentsInChildren[i].GetComponent<UIDragScrollView>().enabled = false;
					flag = true;
				}
				else
				{
					componentsInChildren[i].GetComponent<UIDragScrollView>().enabled = true;
				}
			}
			this.updateItemInfo(componentsInChildren[i], _typeItems);
		}
		if (flag)
		{
			this.ResetWrapPosition(_wrap);
		}
	}

	private void UpdateLocalFriendsArray(bool resetScroll)
	{
		List<string> strs = new List<string>();
		foreach (KeyValuePair<string, FriendsController.PossiblleOrigin> keyValuePair in FriendsController.sharedController.getPossibleFriendsResult)
		{
			if (!FriendsController.sharedController.profileInfo.ContainsKey(keyValuePair.Key) || keyValuePair.Value != FriendsController.PossiblleOrigin.Local)
			{
				continue;
			}
			strs.Add(keyValuePair.Key);
		}
		this.localFriendsList = strs.ToArray();
		this.friendsListWrap.minIndex = ((int)this.myFriendsList.Length + (int)this.localFriendsList.Length) * -1;
	}

	private enum WindowState
	{
		friendList,
		findFriends,
		chat,
		inbox
	}
}