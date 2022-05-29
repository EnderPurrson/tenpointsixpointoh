using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PrivateChatController : MonoBehaviour
{
	public static PrivateChatController sharedController;

	public string selectedPlayerID = string.Empty;

	public FriendPrevInChatItem selectedPlayerItem;

	public GameObject friendPreviewPrefab;

	public GameObject messagePrefab;

	public MyUIInput sendMessageInput;

	public UIButton sendMessageButton;

	public UIWrapContent friendsWrap;

	public UITable messageTable;

	public UIScrollView scrollMessages;

	public UIScrollView scrollFriends;

	public Transform smilesBtnContainer;

	public Transform leftInputAnchor;

	private UIPanel scrollFriensPanel;

	private UIPanel scrollPanel;

	private Transform scrollFriendsTransform;

	private Transform scrollTransform;

	private float heightMessage = 134f;

	private float heightFriends = 100f;

	private float stickerPosShow = -150f;

	private float stickerPosHide = -314f;

	private float speedHideOrShowStiker = 500f;

	public bool isShowSmilePanel;

	public Transform smilePanelTransform;

	public GameObject showSmileButton;

	public GameObject hideSmileButton;

	public GameObject buySmileButton;

	public bool isBuySmile;

	public GameObject buySmileBannerPrefab;

	private bool wrapsInit;

	private List<string> _friends = new List<string>();

	private List<string> friendsWithInfo = new List<string>();

	private List<ChatController.PrivateMessage> curListMessages = new List<ChatController.PrivateMessage>();

	private float keyboardSize;

	public Transform bottomAnchor;

	public UIPanel panelSmiles;

	private List<PrivateMessageItem> privateMessageItems = new List<PrivateMessageItem>();

	private bool isKeyboardVisible;

	public PrivateChatController()
	{
	}

	private void Awake()
	{
		this.heightFriends = (float)this.friendsWrap.itemSize;
		this.scrollTransform = this.scrollMessages.transform;
		this.scrollFriendsTransform = this.scrollFriends.transform;
		this.scrollPanel = this.scrollMessages.GetComponent<UIPanel>();
		this.scrollFriensPanel = this.scrollFriends.GetComponent<UIPanel>();
		PrivateChatController.sharedController = this;
		if (this.sendMessageInput != null)
		{
			this.sendMessageInput.onKeyboardInter += new Action(this.SendMessageFromInput);
			this.sendMessageInput.onKeyboardCancel += new Action(this.CancelSendPrivateMessage);
			this.sendMessageInput.onKeyboardVisible += new Action(this.OnKeyboardVisible);
			this.sendMessageInput.onKeyboardHide += new Action(this.OnKeyboardHide);
		}
		Transform vector3 = this.smilePanelTransform;
		float single = this.smilePanelTransform.localPosition.x;
		float single1 = this.stickerPosHide;
		Vector3 vector31 = this.smilePanelTransform.localPosition;
		vector3.localPosition = new Vector3(single, single1, vector31.z);
		this.isShowSmilePanel = false;
		this.isBuySmile = StickersController.IsBuyAnyPack();
		if (!this.isBuySmile)
		{
			this.showSmileButton.SetActive(false);
			this.buySmileButton.SetActive(true);
		}
		else
		{
			this.showSmileButton.SetActive(true);
			this.buySmileButton.SetActive(false);
		}
		this.hideSmileButton.SetActive(false);
	}

	public void BuySmileOnClick()
	{
		ButtonClickSound.TryPlayClick();
		this.buySmileBannerPrefab.SetActive(true);
		this.sendMessageInput.DeselectInput();
	}

	public void CancelSendPrivateMessage()
	{
		this.sendMessageInput.@value = string.Empty;
	}

	public void HideSmilePannel()
	{
		this.isShowSmilePanel = false;
		if (!this.isBuySmile)
		{
			this.showSmileButton.SetActive(false);
			this.buySmileButton.SetActive(true);
		}
		else
		{
			this.showSmileButton.SetActive(true);
			this.buySmileButton.SetActive(false);
		}
		this.hideSmileButton.SetActive(false);
	}

	public void HideSmilePannelOnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		this.HideSmilePannel();
	}

	private void MoveFriendWrapToPosition(float yPosition)
	{
		bool flag = yPosition < 0f;
		float single = Mathf.Abs(yPosition);
		int num = (int)Mathf.Floor(single / 42f);
		float single1 = single - (float)(42 * num);
		for (int i = 0; i < num; i++)
		{
			this.scrollFriends.MoveRelative(new Vector3(0f, (float)((!flag ? 42 : -42)), 0f));
		}
		this.scrollFriends.MoveRelative(new Vector3(0f, (!flag ? single1 : -single1), 0f));
		SpringPanel.Begin(this.scrollFriensPanel.gameObject, this.scrollFriensPanel.transform.localPosition, 100000f);
	}

	private void OnDestroy()
	{
		FriendsController.FriendsUpdated -= new Action(this.Start_UpdateFriendList);
		PrivateChatController.sharedController = null;
		if (this.sendMessageInput != null)
		{
			this.sendMessageInput.onKeyboardInter -= new Action(this.SendPrivateMessage);
			this.sendMessageInput.onKeyboardCancel -= new Action(this.CancelSendPrivateMessage);
			this.sendMessageInput.onKeyboardVisible -= new Action(this.OnKeyboardVisible);
			this.sendMessageInput.onKeyboardHide -= new Action(this.OnKeyboardHide);
		}
	}

	private void OnDisable()
	{
		this.OnKeyboardHide();
		this.HideSmilePannelOnClick();
		this.sendMessageInput.DeselectInput();
		FriendsController.FriendsUpdated -= new Action(this.Start_UpdateFriendList);
	}

	private void OnEnable()
	{
		FriendsController.FriendsUpdated += new Action(this.Start_UpdateFriendList);
		this.Start_UpdateFriendListCore(true);
		this.sendMessageInput.@value = string.Empty;
		if (string.IsNullOrEmpty(this.selectedPlayerID) && this._friends.Count > 0)
		{
			this.selectedPlayerID = this._friends[0];
		}
		base.StartCoroutine(this.SetSelectedPlayerWithPause(this.selectedPlayerID, true));
	}

	private void onFriendItemWrap(GameObject go, int wrapInd, int realInd)
	{
		go.GetComponent<FriendPrevInChatItem>().myWrapIndex = Mathf.Abs(realInd);
		this.UpdateItemInfo(go.GetComponent<FriendPrevInChatItem>());
	}

	public void OnKeyboardHide()
	{
		if (!this.isKeyboardVisible)
		{
			return;
		}
		this.isKeyboardVisible = false;
		Transform vector3 = this.bottomAnchor;
		float single = this.bottomAnchor.localPosition.x;
		Vector3 vector31 = this.bottomAnchor.localPosition;
		float coef = vector31.y - this.keyboardSize / Defs.Coef;
		Vector3 vector32 = this.bottomAnchor.localPosition;
		vector3.localPosition = new Vector3(single, coef, vector32.z);
		this.sendMessageButton.gameObject.SetActive(false);
		Transform transforms = this.smilesBtnContainer;
		float single1 = -1f * this.smilesBtnContainer.localPosition.x;
		float single2 = this.smilesBtnContainer.localPosition.y;
		Vector3 vector33 = this.smilesBtnContainer.localPosition;
		transforms.localPosition = new Vector3(single1, single2, vector33.z);
		Transform transforms1 = this.leftInputAnchor;
		Vector3 vector34 = this.leftInputAnchor.localPosition;
		float single3 = this.leftInputAnchor.localPosition.y;
		Vector3 vector35 = this.leftInputAnchor.localPosition;
		transforms1.localPosition = new Vector3(vector34.x + 162f, single3, vector35.z);
		base.StartCoroutine(this.ResetpositionCoroutine());
		this.smilePanelTransform.gameObject.SetActive(false);
		this.smilePanelTransform.gameObject.SetActive(true);
	}

	public void OnKeyboardVisible()
	{
		if (this.isKeyboardVisible)
		{
			return;
		}
		this.isKeyboardVisible = true;
		this.keyboardSize = this.sendMessageInput.heightKeyboard;
		if (Application.isEditor)
		{
			this.keyboardSize = 200f;
		}
		Transform vector3 = this.bottomAnchor;
		float single = this.bottomAnchor.localPosition.x;
		Vector3 vector31 = this.bottomAnchor.localPosition;
		float coef = vector31.y + this.keyboardSize / Defs.Coef;
		Vector3 vector32 = this.bottomAnchor.localPosition;
		vector3.localPosition = new Vector3(single, coef, vector32.z);
		this.sendMessageButton.gameObject.SetActive(true);
		Transform transforms = this.smilesBtnContainer;
		float single1 = -1f * this.smilesBtnContainer.localPosition.x;
		float single2 = this.smilesBtnContainer.localPosition.y;
		Vector3 vector33 = this.smilesBtnContainer.localPosition;
		transforms.localPosition = new Vector3(single1, single2, vector33.z);
		Transform transforms1 = this.leftInputAnchor;
		Vector3 vector34 = this.leftInputAnchor.localPosition;
		float single3 = this.leftInputAnchor.localPosition.y;
		Vector3 vector35 = this.leftInputAnchor.localPosition;
		transforms1.localPosition = new Vector3(vector34.x - 162f, single3, vector35.z);
		base.StartCoroutine(this.ResetpositionCoroutine());
	}

	[DebuggerHidden]
	private IEnumerator RepositionNextFrame(bool resetPosition)
	{
		PrivateChatController.u003cRepositionNextFrameu003ec__IteratorEC variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator ResetpositionCoroutine()
	{
		PrivateChatController.u003cResetpositionCoroutineu003ec__IteratorED variable = null;
		return variable;
	}

	public void SendMessageFromInput()
	{
		this.SendPrivateMessage();
		if (this.isShowSmilePanel)
		{
			this.HideSmilePannel();
		}
	}

	public void SendPrivateMessage()
	{
		this.SendPrivateMessageCore(string.Empty);
	}

	public void SendPrivateMessageCore(string customMessage)
	{
		if (string.IsNullOrEmpty(customMessage) && (string.IsNullOrEmpty(this.sendMessageInput.@value) || this.sendMessageInput.@value.Contains(Defs.SmileMessageSuffix)))
		{
			return;
		}
		bool flag = !string.IsNullOrEmpty(customMessage);
		ChatController.PrivateMessage privateMessage = new ChatController.PrivateMessage(FriendsController.sharedController.id, (!flag ? FilterBadWorld.FilterString(this.sendMessageInput.@value) : customMessage), (double)(Tools.CurrentUnixTime + (long)10000000), false, true);
		if (!ChatController.privateMessagesForSend.ContainsKey(this.selectedPlayerID))
		{
			ChatController.privateMessagesForSend.Add(this.selectedPlayerID, new List<ChatController.PrivateMessage>());
		}
		ChatController.privateMessagesForSend[this.selectedPlayerID].Add(privateMessage);
		ChatController.SavePrivatMessageInPrefs();
		if (!flag)
		{
			this.sendMessageInput.@value = string.Empty;
		}
		this.UpdateMessageForSelectedUsers(true);
		FriendsController.sharedController.GetFriendsData(false);
		if (this.selectedPlayerItem.myWrapIndex != 0)
		{
			this._friends = new List<string>(this.friendsWithInfo);
			this._friends.Sort(new Comparison<string>(this.SortByMessagesCount));
			this.friendsWrap.SortAlphabetically();
			this.scrollFriends.ResetPosition();
		}
	}

	public void SendSmile(string smile)
	{
		this.SendPrivateMessageCore(string.Concat(Defs.SmileMessageSuffix, smile));
		this.HideSmilePannel();
	}

	public void SetSelectedPlayer(string _playerId, bool updateToogleState = true)
	{
		this.selectedPlayerItem = null;
		if (!string.IsNullOrEmpty(_playerId))
		{
			if (!this.sendMessageInput.gameObject.activeSelf)
			{
				this.sendMessageInput.gameObject.SetActive(true);
			}
			this.showSmileButton.SetActive((!this.isBuySmile ? false : !this.isShowSmilePanel));
			this.buySmileButton.SetActive(!this.isBuySmile);
			this.hideSmileButton.SetActive((!this.isBuySmile ? false : this.isShowSmilePanel));
		}
		else
		{
			this.sendMessageInput.gameObject.SetActive(false);
			this.showSmileButton.SetActive(false);
			this.buySmileButton.SetActive(false);
			this.hideSmileButton.SetActive(false);
		}
		FriendPrevInChatItem[] componentsInChildren = this.friendsWrap.GetComponentsInChildren<FriendPrevInChatItem>(true);
		for (int i = 0; i < this._friends.Count; i++)
		{
			if (this._friends[i].Equals(_playerId))
			{
				float single = (float)(42 * i);
				if (Mathf.Abs(this.scrollFriensPanel.clipOffset.y + 291f) > single)
				{
					Vector3 vector3 = this.scrollFriends.transform.localPosition;
					this.MoveFriendWrapToPosition(single - (vector3.y - 291f));
				}
				if (Mathf.Abs(this.scrollFriensPanel.clipOffset.y + 291f) + this.scrollFriensPanel.baseClipRegion.w - 42f < single)
				{
					Vector3 vector31 = this.scrollFriends.transform.localPosition;
					Vector4 vector4 = this.scrollFriensPanel.baseClipRegion;
					float single1 = single - (vector31.y - 291f + vector4.w - 50f);
					this.MoveFriendWrapToPosition(single1);
				}
			}
		}
		for (int j = 0; j < (int)componentsInChildren.Length; j++)
		{
			if (componentsInChildren[j].playerID.Equals(_playerId))
			{
				this.selectedPlayerItem = componentsInChildren[j];
			}
		}
		this.selectedPlayerID = _playerId;
		this.UpdateMessageForSelectedUsers(true);
	}

	[DebuggerHidden]
	private IEnumerator SetSelectedPlayerWithPause(string _playerId, bool updateToogleState = true)
	{
		PrivateChatController.u003cSetSelectedPlayerWithPauseu003ec__IteratorEA variable = null;
		return variable;
	}

	public void ShowSmilePannelOnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		this.isShowSmilePanel = true;
		this.showSmileButton.SetActive(false);
		this.hideSmileButton.SetActive(true);
		this.scrollMessages.ResetPosition();
	}

	private int SortByMessagesCount(string x, string y)
	{
		int num;
		string str = x;
		string str1 = y;
		double num1 = 0;
		double num2 = 0;
		int num3 = 0;
		int num4 = 0;
		if (ChatController.privateMessages.ContainsKey(str))
		{
			for (int i = 0; i < ChatController.privateMessages[str].Count; i++)
			{
				double item = ChatController.privateMessages[str][i].timeStamp;
				if (item > num1)
				{
					num1 = item;
				}
				if (!ChatController.privateMessages[str][i].isRead)
				{
					num3++;
				}
			}
		}
		if (ChatController.privateMessages.ContainsKey(str1))
		{
			for (int j = 0; j < ChatController.privateMessages[str1].Count; j++)
			{
				double item1 = ChatController.privateMessages[str1][j].timeStamp;
				if (item1 > num2)
				{
					num2 = item1;
				}
				if (!ChatController.privateMessages[str1][j].isRead)
				{
					num4++;
				}
			}
		}
		if (ChatController.privateMessagesForSend.ContainsKey(str))
		{
			for (int k = 0; k < ChatController.privateMessagesForSend[str].Count; k++)
			{
				double item2 = ChatController.privateMessagesForSend[str][k].timeStamp;
				if (item2 > num1)
				{
					num1 = item2;
				}
			}
		}
		if (ChatController.privateMessagesForSend.ContainsKey(str1))
		{
			for (int l = 0; l < ChatController.privateMessagesForSend[str1].Count; l++)
			{
				double item3 = ChatController.privateMessagesForSend[str1][l].timeStamp;
				if (item3 > num2)
				{
					num2 = item3;
				}
			}
		}
		if (num3 != num4)
		{
			num = (num3 >= num4 ? -1 : 1);
		}
		else
		{
			num = (num1 > num2 ? -1 : 1);
		}
		return num;
	}

	private void Start_UpdateFriendList()
	{
		this.Start_UpdateFriendListCore(false);
	}

	private void Start_UpdateFriendListCore(bool isUpdatePos)
	{
		base.StartCoroutine(this.UpdateFriendList(isUpdatePos));
	}

	private void Update()
	{
		if (!this.isKeyboardVisible)
		{
			this.panelSmiles.UpdateAnchors();
			this.smilePanelTransform.GetComponent<UISprite>().UpdateAnchors();
			this.leftInputAnchor.GetComponent<UIWidget>().UpdateAnchors();
		}
		if (this.isShowSmilePanel && this.smilePanelTransform.localPosition.y < this.stickerPosShow)
		{
			Transform vector3 = this.smilePanelTransform;
			float single = this.smilePanelTransform.localPosition.x;
			Vector3 vector31 = this.smilePanelTransform.localPosition;
			float single1 = vector31.y + Time.deltaTime * this.speedHideOrShowStiker;
			Vector3 vector32 = this.smilePanelTransform.localPosition;
			vector3.localPosition = new Vector3(single, single1, vector32.z);
			this.scrollMessages.MoveRelative((Vector3.up * Time.deltaTime) * this.speedHideOrShowStiker);
			if (this.smilePanelTransform.localPosition.y > this.stickerPosShow)
			{
				Transform transforms = this.smilePanelTransform;
				float single2 = this.smilePanelTransform.localPosition.x;
				float single3 = this.stickerPosShow;
				Vector3 vector33 = this.smilePanelTransform.localPosition;
				transforms.localPosition = new Vector3(single2, single3, vector33.z);
				this.scrollMessages.ResetPosition();
				this.smilePanelTransform.gameObject.SetActive(false);
				this.smilePanelTransform.gameObject.SetActive(true);
			}
		}
		if (!this.isShowSmilePanel && this.smilePanelTransform.localPosition.y > this.stickerPosHide)
		{
			Transform transforms1 = this.smilePanelTransform;
			float single4 = this.smilePanelTransform.localPosition.x;
			Vector3 vector34 = this.smilePanelTransform.localPosition;
			float single5 = vector34.y - Time.deltaTime * this.speedHideOrShowStiker;
			Vector3 vector35 = this.smilePanelTransform.localPosition;
			transforms1.localPosition = new Vector3(single4, single5, vector35.z);
			this.scrollMessages.MoveRelative((Vector3.down * Time.deltaTime) * this.speedHideOrShowStiker);
			if (this.smilePanelTransform.localPosition.y < this.stickerPosHide)
			{
				Transform transforms2 = this.smilePanelTransform;
				float single6 = this.smilePanelTransform.localPosition.x;
				float single7 = this.stickerPosHide;
				Vector3 vector36 = this.smilePanelTransform.localPosition;
				transforms2.localPosition = new Vector3(single6, single7, vector36.z);
				this.scrollMessages.ResetPosition();
				this.smilePanelTransform.gameObject.SetActive(false);
				this.smilePanelTransform.gameObject.SetActive(true);
			}
		}
		bool flag = string.IsNullOrEmpty(this.sendMessageInput.@value);
		if (this.sendMessageButton.isEnabled == flag)
		{
			this.sendMessageButton.isEnabled = !flag;
		}
	}

	public void UpdateFriendItemsInfoAndSort()
	{
		this._friends = new List<string>(this.friendsWithInfo);
		this._friends.Sort(new Comparison<string>(this.SortByMessagesCount));
		FriendPrevInChatItem[] componentsInChildren = this.friendsWrap.GetComponentsInChildren<FriendPrevInChatItem>(true);
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			this.UpdateItemInfo(componentsInChildren[i]);
		}
	}

	[DebuggerHidden]
	public IEnumerator UpdateFriendList(bool isUpdatePos = false)
	{
		PrivateChatController.u003cUpdateFriendListu003ec__IteratorEB variable = null;
		return variable;
	}

	private void UpdateItemInfo(FriendPrevInChatItem previewItem)
	{
		Dictionary<string, object> strs;
		if (this._friends.Count > previewItem.myWrapIndex)
		{
			if (!previewItem.gameObject.activeSelf)
			{
				previewItem.gameObject.SetActive(true);
			}
			string item = this._friends[previewItem.myWrapIndex];
			Dictionary<string, string> strs1 = new Dictionary<string, string>();
			if (FriendsController.sharedController.friendsInfo.ContainsKey(item) && FriendsController.sharedController.friendsInfo[item].TryGetValue<Dictionary<string, object>>("player", out strs))
			{
				foreach (KeyValuePair<string, object> keyValuePair in strs)
				{
					strs1.Add(keyValuePair.Key, Convert.ToString(keyValuePair.Value));
				}
				previewItem.playerID = item;
				previewItem.UpdateCountNewMessage();
				previewItem.nickLabel.text = strs1["nick"];
				previewItem.rank.spriteName = string.Concat("Rank_", strs1["rank"]);
				previewItem.previewTexture.mainTexture = Tools.GetPreviewFromSkin(strs1["skin"], Tools.PreviewType.Head);
				previewItem.GetComponent<UIToggle>().Set(this.selectedPlayerID == item);
			}
		}
		else if (previewItem.gameObject.activeSelf)
		{
			previewItem.gameObject.SetActive(false);
		}
	}

	public void UpdateMessageForSelectedUsers(bool resetPosition = false)
	{
		this.UpdateMessageForSelectedUsersCoroutine(resetPosition);
	}

	private void UpdateMessageForSelectedUsersCoroutine(bool resetPosition)
	{
		this.curListMessages.Clear();
		if (!string.IsNullOrEmpty(this.selectedPlayerID))
		{
			if (ChatController.privateMessages.ContainsKey(this.selectedPlayerID))
			{
				this.curListMessages.AddRange(ChatController.privateMessages[this.selectedPlayerID]);
			}
			if (ChatController.privateMessagesForSend.ContainsKey(this.selectedPlayerID))
			{
				this.curListMessages.AddRange(ChatController.privateMessagesForSend[this.selectedPlayerID]);
			}
		}
		this.curListMessages.Sort((ChatController.PrivateMessage x, ChatController.PrivateMessage y) => (x.timeStamp > y.timeStamp ? -1 : 1));
		if (this.selectedPlayerItem != null)
		{
			this.UpdateItemInfo(this.selectedPlayerItem);
		}
		while (this.privateMessageItems.Count < this.curListMessages.Count)
		{
			GameObject item = NGUITools.AddChild(this.messageTable.gameObject, this.messagePrefab);
			if (this.privateMessageItems.Count > 0)
			{
				item.transform.position = this.privateMessageItems[0].transform.position;
			}
			item.name = this.privateMessageItems.Count.ToString();
			item.GetComponent<PrivateMessageItem>().myWrapIndex = this.privateMessageItems.Count;
			this.privateMessageItems.Add(item.GetComponent<PrivateMessageItem>());
		}
		while (this.privateMessageItems.Count > this.curListMessages.Count)
		{
			UnityEngine.Object.Destroy(this.privateMessageItems[this.privateMessageItems.Count - 1].gameObject);
			this.privateMessageItems.RemoveAt(this.privateMessageItems.Count - 1);
			for (int i = 0; i < this.privateMessageItems.Count; i++)
			{
				this.privateMessageItems[i].gameObject.name = i.ToString();
				this.privateMessageItems[i].myWrapIndex = i;
			}
		}
		this.messageTable.onCustomSort = (Transform x, Transform y) => (int.Parse(x.name) <= int.Parse(y.name) ? -1 : 1);
		for (int j = 0; j < this.privateMessageItems.Count; j++)
		{
			this.UpdateMessageItemInfo(this.privateMessageItems[j]);
		}
		Transform vector3 = this.messageTable.transform;
		float single = this.scrollPanel.baseClipRegion.x;
		float single1 = this.messageTable.transform.localPosition.y;
		Vector3 vector31 = this.messageTable.transform.localPosition;
		vector3.localPosition = new Vector3(single, single1, vector31.z);
		this.messageTable.repositionNow = true;
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.RepositionNextFrame(resetPosition));
		}
	}

	private void UpdateMessageItemInfo(PrivateMessageItem messageItem)
	{
		if (this.curListMessages.Count > messageItem.myWrapIndex)
		{
			if (!messageItem.gameObject.activeSelf)
			{
				messageItem.gameObject.SetActive(true);
			}
			Transform vector3 = messageItem.transform;
			float single = messageItem.transform.localPosition.y;
			Vector3 vector31 = messageItem.transform.localPosition;
			vector3.localPosition = new Vector3(0f, single, vector31.z);
			Vector4 vector4 = this.scrollPanel.baseClipRegion;
			messageItem.SetWidth(Mathf.RoundToInt(vector4.z));
			ChatController.PrivateMessage item = this.curListMessages[messageItem.myWrapIndex];
			messageItem.isRead = item.isRead;
			messageItem.timeStamp = item.timeStamp.ToString("F8", CultureInfo.InvariantCulture);
			if (!item.playerIDFrom.Equals(FriendsController.sharedController.id))
			{
				messageItem.SetFon(false);
				messageItem.yourMessageLabel.text = string.Empty;
				if (!item.message.Contains(Defs.SmileMessageSuffix))
				{
					messageItem.otherMessageLabel.text = item.message;
					messageItem.otherMessageLabel.overflowMethod = UILabel.Overflow.ResizeHeight;
					messageItem.otherMessageLabel.width = Mathf.CeilToInt((float)messageItem.otherWidget.width * 0.8f);
					messageItem.otherSmileSprite.gameObject.SetActive(false);
				}
				else
				{
					messageItem.otherSmileSprite.spriteName = item.message.Substring(Defs.SmileMessageSuffix.Length);
					messageItem.otherMessageLabel.text = string.Empty;
					messageItem.otherMessageLabel.overflowMethod = UILabel.Overflow.ShrinkContent;
					messageItem.otherMessageLabel.height = 80;
					messageItem.otherMessageLabel.width = 80;
					messageItem.otherSmileSprite.gameObject.SetActive(true);
				}
				int num = (int)item.timeStamp;
				TimeSpan offset = DateTimeOffset.Now.Offset;
				DateTime currentTimeByUnixTime = Tools.GetCurrentTimeByUnixTime((long)(num + offset.Hours * 3600));
				messageItem.otherTimeLabel.text = string.Concat(new object[] { currentTimeByUnixTime.Day.ToString("D2"), ".", currentTimeByUnixTime.Month.ToString("D2"), ".", currentTimeByUnixTime.Year, " ", currentTimeByUnixTime.Hour, ":", currentTimeByUnixTime.Minute.ToString("D2") });
			}
			else
			{
				messageItem.SetFon(true);
				messageItem.otherMessageLabel.text = string.Empty;
				if (!item.message.Contains(Defs.SmileMessageSuffix))
				{
					messageItem.yourMessageLabel.text = item.message;
					messageItem.yourMessageLabel.overflowMethod = UILabel.Overflow.ResizeHeight;
					messageItem.yourMessageLabel.width = Mathf.CeilToInt((float)messageItem.yourWidget.width * 0.8f);
					messageItem.yourSmileSprite.gameObject.SetActive(false);
				}
				else
				{
					messageItem.yourSmileSprite.spriteName = item.message.Substring(Defs.SmileMessageSuffix.Length);
					messageItem.yourMessageLabel.text = string.Empty;
					messageItem.yourMessageLabel.overflowMethod = UILabel.Overflow.ShrinkContent;
					messageItem.yourMessageLabel.height = 80;
					messageItem.yourMessageLabel.width = 80;
					messageItem.yourSmileSprite.gameObject.SetActive(true);
				}
				if (!item.isSending)
				{
					messageItem.yourTimeLabel.text = LocalizationStore.Get("Key_1556");
				}
				else
				{
					int num1 = (int)item.timeStamp;
					TimeSpan timeSpan = DateTimeOffset.Now.Offset;
					DateTime dateTime = Tools.GetCurrentTimeByUnixTime((long)(num1 + timeSpan.Hours * 3600));
					messageItem.yourTimeLabel.text = string.Concat(new object[] { dateTime.Day.ToString("D2"), ".", dateTime.Month.ToString("D2"), ".", dateTime.Year, " ", dateTime.Hour, ":", dateTime.Minute.ToString("D2") });
				}
			}
		}
		else if (messageItem.gameObject.activeSelf)
		{
			messageItem.gameObject.SetActive(false);
		}
	}
}