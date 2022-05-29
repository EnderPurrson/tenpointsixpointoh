using Rilisoft;
using Rilisoft.NullExtensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FriendPreviewItem : MonoBehaviour
{
	public string id;

	public UIWidget friendListButtonContainer;

	public UIWidget findFrinedsButtonContainer;

	public UIWidget inboxFriendsButtonContainer;

	public UIButton connectToRoomButton;

	public UIButton goToChat;

	public UILabel levelAndName;

	public UISprite level;

	public UIWidget detailInfoConatiner;

	public UILabel detailInfo;

	public UITexture avatarIcon;

	public UIWidget clanContainer;

	public UITexture clanIcon;

	public UILabel clanName;

	public UISprite playingIcon;

	public UISprite inFriendsIcon;

	public UISprite offlineIcon;

	public UIGrid playerDetailInfoGrid;

	[Header("Not connect button")]
	public UISprite notConnectIcon;

	public UILabel notConnectLabel;

	[Header("add friend button")]
	public UIButton addFriendButton;

	public UIWidget invitationAddSentContainer;

	public UIWidget friendAddContainer;

	public UIWidget selfAddContainer;

	[Header("inbox button")]
	public UIButton acceptInviteButton;

	public UIButton cancelInviteButton;

	[NonSerialized]
	public int FindOrigin;

	public int myWrapIndex;

	private FriendItemPreviewType _type;

	public GameObject inYourNetworkIcon;

	public int OnlineCodeStatus
	{
		get;
		private set;
	}

	public FriendPreviewItem()
	{
	}

	private void CallbackFriendAddRequest(bool isComplete, bool isRequestExist)
	{
		this.addFriendButton.enabled = true;
		InfoWindowController.CheckShowRequestServerInfoBox(isComplete, isRequestExist);
		if (isComplete)
		{
			this.SetupFindStateButtons();
		}
	}

	private void FillClanAttrs(Dictionary<string, string> plDict)
	{
		if (!plDict.ContainsKey("clan_logo") || string.IsNullOrEmpty(plDict["clan_logo"]) || plDict["clan_logo"].Equals("null"))
		{
			this.clanIcon.gameObject.SetActive(false);
		}
		else
		{
			this.clanIcon.gameObject.SetActive(true);
			try
			{
				byte[] numArray = Convert.FromBase64String(plDict["clan_logo"]);
				Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoHeight, TextureFormat.ARGB32, false);
				texture2D.LoadImage(numArray);
				texture2D.filterMode = FilterMode.Point;
				texture2D.Apply();
				Texture texture = this.clanIcon.mainTexture;
				this.clanIcon.mainTexture = texture2D;
				if (texture != null)
				{
					UnityEngine.Object.DestroyImmediate(texture, true);
				}
			}
			catch (Exception exception)
			{
				Texture texture1 = this.clanIcon.mainTexture;
				this.clanIcon.mainTexture = null;
				if (texture1 != null)
				{
					UnityEngine.Object.DestroyImmediate(texture1, true);
				}
			}
		}
		if (!plDict.ContainsKey("clan_name") || string.IsNullOrEmpty(plDict["clan_name"]) || plDict["clan_name"].Equals("null"))
		{
			this.clanName.gameObject.SetActive(false);
		}
		else
		{
			this.clanName.gameObject.SetActive(true);
			string item = plDict["clan_name"];
			if (item != null)
			{
				this.clanName.text = item;
			}
		}
	}

	private void FillCommonAttrsByPlayerData(Dictionary<string, object> playerData)
	{
		string str = Convert.ToString(playerData["nick"]);
		string str1 = Convert.ToString(playerData["rank"]);
		this.levelAndName.text = str;
		this.level.spriteName = string.Concat("Rank_", str1);
		if (playerData.ContainsKey("skin"))
		{
			this.SetSkin(playerData["skin"] as string);
		}
		Dictionary<string, string> strs = new Dictionary<string, string>();
		foreach (KeyValuePair<string, object> playerDatum in playerData)
		{
			strs.Add(playerDatum.Key, Convert.ToString(playerDatum.Value));
		}
		bool flag = (!strs.ContainsKey("clan_name") ? false : !string.IsNullOrEmpty(strs["clan_name"]));
		this.ResetPositionElementsDetailInfo(flag);
		if (flag)
		{
			this.FillClanAttrs(strs);
		}
	}

	private void FillCommonAttrsByPlayerInfo()
	{
		Dictionary<string, object> strs;
		Dictionary<string, object> fullPlayerDataById = FriendsController.GetFullPlayerDataById(this.id);
		if (fullPlayerDataById != null && fullPlayerDataById.TryGetValue<Dictionary<string, object>>("player", out strs))
		{
			this.FillCommonAttrsByPlayerData(strs);
		}
	}

	public void FillData(string playerId, FriendItemPreviewType typeItem)
	{
		this.id = playerId;
		this._type = typeItem;
		this.ShowButtonsByTypePreview(typeItem);
		this.FillCommonAttrsByPlayerInfo();
		this.inYourNetworkIcon.SetActive(false);
		if (typeItem == FriendItemPreviewType.find)
		{
			this.FindOrigin = (int)FriendsController.GetPossibleFriendFindOrigin(playerId);
			if (this.FindOrigin != 0)
			{
				this.SetStatusLabelFindOrigin((FriendsController.PossiblleOrigin)this.FindOrigin);
			}
			else
			{
				this.HideDetailInfo();
			}
		}
		else if (typeItem != FriendItemPreviewType.view)
		{
			this.HideDetailInfo();
		}
		this.UpdateOnline();
	}

	private Color[] FlipColorsHorizontally(Color[] colors, int width, int height)
	{
		Color[] colorArray = new Color[(int)colors.Length];
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				colorArray[i + width * j] = colors[width - i - 1 + width * j];
			}
		}
		return colorArray;
	}

	private string GetLevelAndNameLabel(string level, string name)
	{
		return string.Format("[b]{0} {1}[/b]", level, name);
	}

	private void HideDetailInfo()
	{
		this.detailInfoConatiner.gameObject.SetActive(false);
		this.playerDetailInfoGrid.Reposition();
	}

	public void OnClick()
	{
		ButtonClickSound.TryPlayClick();
		FriendsWindowController.Instance.ShowProfileWindow(this.id, this);
	}

	public void OnClickAcceptButton()
	{
		ButtonClickSound.TryPlayClick();
		this.acceptInviteButton.isEnabled = false;
		this.cancelInviteButton.isEnabled = false;
		if (FriendsController.IsFriendsMax())
		{
			InfoWindowController.ShowInfoBox(LocalizationStore.Get("Key_1424"));
			return;
		}
		FriendsController.sharedController.AcceptInvite(this.id, new Action<bool>(this.OnCompleteAcceptInviteAction));
	}

	public void OnClickAddFriend()
	{
		if (string.IsNullOrEmpty(this.id))
		{
			return;
		}
		ButtonClickSound.TryPlayClick();
		this.addFriendButton.enabled = false;
		bool flag = FriendsWindowController.Instance.Map<FriendsWindowController, FriendsWindowStatusBar>((FriendsWindowController fwc) => fwc.statusBar).Map<FriendsWindowStatusBar, bool>((FriendsWindowStatusBar s) => s.IsFindFriendByIdStateActivate);
		string str = (!flag ? string.Format("Find Friends: {0}", (FriendsController.PossiblleOrigin)this.FindOrigin) : "Search");
		Dictionary<string, object> strs = new Dictionary<string, object>()
		{
			{ "Added Friends", str },
			{ "Deleted Friends", "Add" }
		};
		Dictionary<string, object> strs1 = strs;
		if (flag)
		{
			strs1.Add("Search Friends", "Add");
		}
		FriendsController.SendFriendshipRequest(this.id, strs1, new Action<bool, bool>(this.CallbackFriendAddRequest));
	}

	public void OnClickConnectToFriendRoom()
	{
		ButtonClickSound.TryPlayClick();
		FriendsController.JoinToFriendRoom(this.id);
	}

	public void OnClickDeclineButton()
	{
		this.acceptInviteButton.isEnabled = false;
		this.cancelInviteButton.isEnabled = false;
		ButtonClickSound.TryPlayClick();
		FriendsController.sharedController.RejectInvite(this.id, new Action<bool>(this.OnCompletetRejectInviteAction));
	}

	public void OnClickGoTohatButton()
	{
		FriendsWindowController.Instance.SetActiveChatTab(this.id);
	}

	private void OnCompleteAcceptInviteAction(bool isComplete)
	{
		InfoWindowController.CheckShowRequestServerInfoBox(isComplete, false);
		this.acceptInviteButton.isEnabled = true;
		this.cancelInviteButton.isEnabled = true;
		if (isComplete)
		{
			AnalyticsFacade.SendCustomEvent("Social", new Dictionary<string, object>()
			{
				{ "Friend Requests", "Accepted" }
			});
			FriendsWindowController.Instance.UpdateCurrentTabState();
		}
	}

	private void OnCompletetRejectInviteAction(bool isComplete)
	{
		InfoWindowController.CheckShowRequestServerInfoBox(isComplete, false);
		this.acceptInviteButton.isEnabled = true;
		this.cancelInviteButton.isEnabled = true;
		if (isComplete)
		{
			AnalyticsFacade.SendCustomEvent("Social", new Dictionary<string, object>()
			{
				{ "Friend Requests", "Rejected" }
			});
			FriendsWindowController.Instance.UpdateCurrentTabState();
		}
	}

	private void OnDestroy()
	{
		FriendsWindowController.UpdateFriendsOnlineEvent -= new Action(this.UpdateOnline);
	}

	private void ResetPositionElementsDetailInfo(bool isPlayerInClan)
	{
		this.clanContainer.gameObject.SetActive(isPlayerInClan);
		this.playerDetailInfoGrid.Reposition();
	}

	private void SetOfflineStatePreview()
	{
		this.SetStatusLabelPlayerBusy();
		this.OnlineCodeStatus = 3;
		this.SetStateButtonConnectContainer(false, LocalizationStore.Get("Key_1577"));
	}

	public void SetSkin(string skinStr)
	{
		Texture texture = this.avatarIcon.mainTexture;
		if (texture != null && !texture.name.Equals("dude") && !texture.name.Equals("multi_skin_1"))
		{
			UnityEngine.Object.DestroyImmediate(texture, true);
		}
		this.avatarIcon.mainTexture = Tools.GetPreviewFromSkin(skinStr, Tools.PreviewType.HeadAndBody);
	}

	private void SetStateButtonConnectContainer(bool isCanConnect, string conditionNotConnect)
	{
		this.connectToRoomButton.gameObject.SetActive(isCanConnect);
		this.notConnectLabel.gameObject.SetActive(!isCanConnect);
		this.notConnectLabel.text = conditionNotConnect;
	}

	private void SetStatusLabelFindOrigin(FriendsController.PossiblleOrigin findOrigin)
	{
		if (!this.detailInfoConatiner.gameObject.activeSelf)
		{
			this.detailInfoConatiner.gameObject.SetActive(true);
			this.playerDetailInfoGrid.Reposition();
		}
		if (findOrigin == FriendsController.PossiblleOrigin.None)
		{
			return;
		}
		if (findOrigin == FriendsController.PossiblleOrigin.Local)
		{
			this.detailInfo.text = string.Format("[ffe400]{0}[-]", LocalizationStore.Get("Key_1569"));
			this.inYourNetworkIcon.SetActive(true);
		}
		else if (findOrigin == FriendsController.PossiblleOrigin.Facebook)
		{
			this.detailInfo.text = string.Format("[00aeff]{0}[-]", LocalizationStore.Get("Key_1570"));
		}
		else if (findOrigin == FriendsController.PossiblleOrigin.RandomPlayer)
		{
			this.detailInfo.text = string.Format("[77ef00]{0}[-]", LocalizationStore.Get("Key_1571"));
		}
	}

	private void SetStatusLabelPlayerBusy()
	{
		if (!this.detailInfoConatiner.gameObject.activeSelf)
		{
			this.detailInfoConatiner.gameObject.SetActive(true);
			this.playerDetailInfoGrid.Reposition();
		}
		this.detailInfo.text = string.Format("[ff0000]{0}[-]", LocalizationStore.Get("Key_0576"));
	}

	private void SetStatusLabelPlayerPlaying(string gameModeName, string mapName)
	{
		if (!this.detailInfoConatiner.gameObject.activeSelf)
		{
			this.detailInfoConatiner.gameObject.SetActive(true);
			this.playerDetailInfoGrid.Reposition();
		}
		if (!string.IsNullOrEmpty(mapName))
		{
			this.detailInfo.text = string.Format("[77ef00]{0}: {1}[-]", gameModeName, mapName);
		}
		else
		{
			this.detailInfo.text = string.Format("[00aeff]{0}[-]", gameModeName);
		}
	}

	private void SetupFindStateButtons()
	{
		bool flag = FriendsController.IsAlreadySendInvitePlayer(this.id);
		bool flag1 = FriendsController.IsMyPlayerId(this.id);
		bool flag2 = FriendsController.IsPlayerOurFriend(this.id);
		this.addFriendButton.gameObject.SetActive((flag2 || flag1 ? false : !flag));
		this.invitationAddSentContainer.gameObject.SetActive(flag);
		this.friendAddContainer.gameObject.SetActive(flag2);
		this.selfAddContainer.gameObject.SetActive(flag1);
	}

	private void ShowButtonsByTypePreview(FriendItemPreviewType typePreview)
	{
		this.friendListButtonContainer.gameObject.SetActive(typePreview == FriendItemPreviewType.view);
		bool flag = typePreview == FriendItemPreviewType.find;
		this.findFrinedsButtonContainer.gameObject.SetActive(flag);
		if (flag)
		{
			this.SetupFindStateButtons();
		}
		this.inboxFriendsButtonContainer.gameObject.SetActive(typePreview == FriendItemPreviewType.inbox);
	}

	private void Start()
	{
		FriendsWindowController.UpdateFriendsOnlineEvent += new Action(this.UpdateOnline);
	}

	public void UpdateData()
	{
		if (string.IsNullOrEmpty(this.id))
		{
			return;
		}
		this.FillData(this.id, this._type);
	}

	private void UpdateOnline()
	{
		if (this._type == FriendItemPreviewType.find)
		{
			return;
		}
		if (!FriendsController.sharedController.onlineInfo.ContainsKey(this.id))
		{
			this.SetOfflineStatePreview();
			return;
		}
		Dictionary<string, string> item = FriendsController.sharedController.onlineInfo[this.id];
		FriendsController.ResultParseOnlineData resultParseOnlineDatum = FriendsController.ParseOnlineData(item);
		if (resultParseOnlineDatum == null)
		{
			this.SetOfflineStatePreview();
			return;
		}
		this.SetStatusLabelPlayerPlaying(resultParseOnlineDatum.GetGameModeName(), resultParseOnlineDatum.GetMapName());
		this.SetStateButtonConnectContainer(resultParseOnlineDatum.IsCanConnect, resultParseOnlineDatum.GetNotConnectConditionShortString());
		this.OnlineCodeStatus = (int)resultParseOnlineDatum.GetOnlineStatus();
	}
}