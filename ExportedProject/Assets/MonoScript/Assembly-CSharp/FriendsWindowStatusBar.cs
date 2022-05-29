using Facebook.Unity;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FriendsWindowStatusBar : MonoBehaviour
{
	public UIWidget findContainer;

	public UIWidget inboxContainer;

	public UILabel warningMessageAboutLimit;

	public MyUIInput findFriendInput;

	public UIWidget facebookContainer;

	public UILabel messageFacebookLabel;

	public UILabel[] facebookButtonLabels;

	public UILabel[] facebookRewardLabels;

	public UILabel[] facebookInviteLabels;

	public UIWidget facebookRewardLabelContainer;

	public UIButton refreshButton;

	public UIButton sendMyIdButton;

	public UIButton clearAllInviteButton;

	public bool IsFindFriendByIdStateActivate
	{
		get;
		set;
	}

	public FriendsWindowStatusBar()
	{
	}

	private void BlockClickInWindow(bool enable)
	{
	}

	private void InitFacebookRewardButtonText()
	{
		if (this.facebookButtonLabels == null || (int)this.facebookButtonLabels.Length == 0)
		{
			return;
		}
		for (int i = 0; i < (int)this.facebookButtonLabels.Length; i++)
		{
			this.facebookButtonLabels[i].text = LocalizationStore.Get("Key_1582");
		}
	}

	public void OnClickBackButton()
	{
		ButtonClickSound.TryPlayClick();
		FriendsWindowGUI.Instance.HideInterface();
	}

	public void OnClickCancelFindFriendButton()
	{
		ButtonClickSound.TryPlayClick();
		this.findFriendInput.@value = string.Empty;
	}

	public void OnClickChatTab()
	{
		this.SetupStateFacebookContainer(true);
		this.findContainer.gameObject.SetActive(false);
		this.inboxContainer.gameObject.SetActive(false);
		this.warningMessageAboutLimit.gameObject.SetActive(false);
		this.refreshButton.gameObject.SetActive(false);
	}

	public void OnClickClearAllButton()
	{
		ButtonClickSound.TryPlayClick();
		FriendsWindowController instance = FriendsWindowController.Instance;
		if (instance == null)
		{
			return;
		}
		instance.OnClickClearAllInboxButton();
	}

	public void OnClickFacebookButton()
	{
		ButtonClickSound.TryPlayClick();
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		FacebookController facebookController = FacebookController.sharedController;
		if (facebookController == null)
		{
			return;
		}
		this.BlockClickInWindow(true);
		if (!FB.IsLoggedIn)
		{
			MainMenuController.DoMemoryConsumingTaskInEmptyScene(() => FacebookController.Login(new Action(this.OnFacebookLoginComplete), new Action(this.OnFacebookLoginCancel), "Friends", null), () => FacebookController.Login(null, null, "Friends", null));
		}
		else
		{
			facebookController.InvitePlayer(null);
		}
	}

	public void OnClickFindFriendButton()
	{
		ButtonClickSound.TryPlayClick();
		FriendsWindowController instance = FriendsWindowController.Instance;
		if (instance == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(this.findFriendInput.@value))
		{
			return;
		}
		this.IsFindFriendByIdStateActivate = true;
		base.StartCoroutine(instance.ShowResultFindPlayer(this.findFriendInput.@value));
		this.findFriendInput.@value = string.Empty;
	}

	public void OnClickInboxFriendsTab(bool isInboxListFound, bool isFriendsMax)
	{
		this.SetupStateFacebookContainer((isFriendsMax ? true : isInboxListFound));
		this.findContainer.gameObject.SetActive(false);
		bool flag = (isFriendsMax ? false : isInboxListFound);
		this.inboxContainer.gameObject.SetActive(flag);
		if (flag && Application.platform != RuntimePlatform.IPhonePlayer)
		{
			this.sendMyIdButton.gameObject.SetActive(false);
			this.clearAllInviteButton.transform.position = this.sendMyIdButton.transform.position;
		}
		this.refreshButton.gameObject.SetActive(false);
	}

	public void OnClickRefreshButton()
	{
		ButtonClickSound.TryPlayClick();
		FriendsController friendsController = FriendsController.sharedController;
		if (friendsController == null)
		{
			return;
		}
		FriendsWindowController instance = FriendsWindowController.Instance;
		if (instance == null)
		{
			return;
		}
		instance.ShowMessageBoxProcessingData();
		this.refreshButton.isEnabled = false;
		base.Invoke("SetEnableRefreshButton", Defs.timeBlockRefreshFriendDate);
		friendsController.GetFriendsData(true);
	}

	public void OnClickSendMyIdButton()
	{
		ButtonClickSound.TryPlayClick();
		FriendsController.SendMyIdByEmail();
	}

	private void OnDestroy()
	{
		if (this.findFriendInput != null)
		{
			this.findFriendInput.onKeyboardInter -= new Action(this.OnClickFindFriendButton);
			this.findFriendInput.onKeyboardCancel -= new Action(this.OnClickCancelFindFriendButton);
		}
	}

	private void OnEnable()
	{
		string str = string.Format(LocalizationStore.Get("Key_1416"), Defs.maxCountFriend, Defs.maxCountFriend);
		this.warningMessageAboutLimit.text = str;
		this.findFriendInput.defaultText = LocalizationStore.Get("Key_1422");
		this.InitFacebookRewardButtonText();
	}

	private void OnFacebookLoginCancel()
	{
		this.BlockClickInWindow(false);
	}

	private void OnFacebookLoginComplete()
	{
		this.BlockClickInWindow(false);
		this.SetupStateFacebookContainer(false);
		FacebookController facebookController = FacebookController.sharedController;
		if (facebookController != null)
		{
			facebookController.InputFacebookFriends(new Action(this.OnSuccsessGetFacebookFriendList), false);
		}
	}

	private void OnSuccsessGetFacebookFriendList()
	{
		FriendsController friendsController = FriendsController.sharedController;
		if (friendsController == null)
		{
			return;
		}
		friendsController.DownloadDataAboutPossibleFriends();
		FriendsWindowController.Instance.NeedUpdateCurrentFriendsList = true;
	}

	private void SetEnableRefreshButton()
	{
		this.refreshButton.isEnabled = true;
	}

	private void SetFacebookNotRewardButtonText(string text)
	{
		if (this.facebookInviteLabels == null || (int)this.facebookInviteLabels.Length == 0)
		{
			return;
		}
		for (int i = 0; i < (int)this.facebookInviteLabels.Length; i++)
		{
			this.facebookInviteLabels[i].text = text;
		}
	}

	private void SetTextFacebookButton(bool isLogin, bool isRewardTake)
	{
		this.SetVisibleFacebookButtonTextByState((isLogin ? true : isRewardTake));
		if (!isRewardTake)
		{
			this.facebookRewardLabelContainer.gameObject.SetActive(true);
			for (int i = 0; i < (int)this.facebookRewardLabels.Length; i++)
			{
				this.facebookRewardLabels[i].text = string.Format("+{0}", 10);
			}
		}
		else
		{
			this.facebookRewardLabelContainer.gameObject.SetActive(false);
			this.SetFacebookNotRewardButtonText((isLogin ? LocalizationStore.Get("Key_1437") : LocalizationStore.Get("Key_1582")));
		}
	}

	private void SetTextFacebookDescription()
	{
		if (FB.IsLoggedIn)
		{
			this.messageFacebookLabel.text = LocalizationStore.Get("Key_1413");
			return;
		}
		this.messageFacebookLabel.text = LocalizationStore.Get("Key_1415");
	}

	private void SetupStateFacebookContainer(bool needHide)
	{
		if (!FacebookController.FacebookSupported)
		{
			needHide = true;
		}
		if (needHide)
		{
			this.facebookContainer.gameObject.SetActive(false);
			return;
		}
		bool num = Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 1;
		this.facebookContainer.gameObject.SetActive(true);
		this.SetTextFacebookButton(FB.IsLoggedIn, num);
	}

	private void SetVisibleFacebookButtonTextByState(bool needFullLabel)
	{
		if ((int)this.facebookButtonLabels.Length == 0 || (int)this.facebookInviteLabels.Length == 0)
		{
			return;
		}
		this.facebookButtonLabels[0].gameObject.SetActive(!needFullLabel);
		this.facebookInviteLabels[0].gameObject.SetActive(needFullLabel);
	}

	private void Start()
	{
		if (this.findFriendInput != null)
		{
			this.findFriendInput.onKeyboardInter += new Action(this.OnClickFindFriendButton);
			this.findFriendInput.onKeyboardCancel += new Action(this.OnClickCancelFindFriendButton);
		}
	}

	public void UpdateFindFriendsState(bool isFriendsMax)
	{
		this.IsFindFriendByIdStateActivate = false;
		this.SetupStateFacebookContainer(true);
		this.warningMessageAboutLimit.gameObject.SetActive(isFriendsMax);
		this.findContainer.gameObject.SetActive(!isFriendsMax);
		this.inboxContainer.gameObject.SetActive(false);
		this.refreshButton.gameObject.SetActive(false);
	}

	public void UpdateFriendListState(bool isFriendsMax)
	{
		this.SetupStateFacebookContainer(isFriendsMax);
		this.warningMessageAboutLimit.gameObject.SetActive(isFriendsMax);
		this.findContainer.gameObject.SetActive(false);
		this.inboxContainer.gameObject.SetActive(false);
		this.refreshButton.gameObject.SetActive(true);
	}
}