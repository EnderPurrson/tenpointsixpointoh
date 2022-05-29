using Rilisoft.MiniJson;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class PrivateMessageItem : MonoBehaviour
{
	[Header("Your message obj")]
	public GameObject yourFonSprite;

	public UILabel yourMessageLabel;

	public UILabel yourTimeLabel;

	public UISprite yourSmileSprite;

	public UIWidget yourWidget;

	[Header("Other message obj")]
	public GameObject otherFonSprite;

	public UILabel otherMessageLabel;

	public UILabel otherTimeLabel;

	public UISprite otherSmileSprite;

	public UIWidget otherWidget;

	[NonSerialized]
	public bool isRead;

	public string timeStamp;

	private UIPanel myPanel;

	private Transform myTransform;

	public int myWrapIndex;

	public PrivateMessageItem()
	{
	}

	private void Awake()
	{
		this.myTransform = base.transform;
	}

	private void DetectNew()
	{
	}

	private void OnEnable()
	{
	}

	public void SetFon(bool isMine)
	{
		this.yourWidget.gameObject.SetActive(isMine);
		this.otherWidget.gameObject.SetActive(!isMine);
	}

	public void SetWidth(int width)
	{
		this.otherWidget.width = width;
		this.yourWidget.width = width;
	}

	private void Start()
	{
		if (this.myTransform.parent != null)
		{
			this.myPanel = base.transform.parent.parent.GetComponent<UIPanel>();
		}
	}

	private void Update()
	{
		if (!this.isRead)
		{
			float single = this.myTransform.localPosition.y;
			float single1 = this.myPanel.clipOffset.y;
			Vector4 vector4 = this.myPanel.baseClipRegion;
			Vector4 vector41 = this.myPanel.baseClipRegion;
			if (single >= single1 - vector4.w * 0.5f + vector41.y)
			{
				float single2 = this.myTransform.localPosition.y;
				float single3 = this.myPanel.clipOffset.y;
				Vector4 vector42 = this.myPanel.baseClipRegion;
				Vector4 vector43 = this.myPanel.baseClipRegion;
				if (single2 <= single3 + vector42.w * 0.5f + vector43.y)
				{
					Dictionary<string, List<Dictionary<string, string>>> strs = new Dictionary<string, List<Dictionary<string, string>>>();
					foreach (KeyValuePair<string, List<ChatController.PrivateMessage>> privateMessage in ChatController.privateMessages)
					{
						List<Dictionary<string, string>> dictionaries = new List<Dictionary<string, string>>();
						foreach (ChatController.PrivateMessage value in privateMessage.Value)
						{
							Dictionary<string, string> strs1 = new Dictionary<string, string>()
							{
								{ "playerIDFrom", value.playerIDFrom },
								{ "message", value.message },
								{ "timeStamp", string.Empty },
								{ "isRead", value.isRead.ToString() },
								{ "isSending", value.isSending.ToString() }
							};
							dictionaries.Add(strs1);
						}
						strs.Add(privateMessage.Key, dictionaries);
					}
					Json.Serialize(strs);
					int num = 0;
					while (num < ChatController.privateMessages[PrivateChatController.sharedController.selectedPlayerID].Count)
					{
						if (!ChatController.privateMessages[PrivateChatController.sharedController.selectedPlayerID][num].timeStamp.ToString("F8", CultureInfo.InvariantCulture).Equals(this.timeStamp) || ChatController.privateMessages[PrivateChatController.sharedController.selectedPlayerID][num].isRead)
						{
							num++;
						}
						else
						{
							this.isRead = true;
							ChatController.PrivateMessage item = ChatController.privateMessages[PrivateChatController.sharedController.selectedPlayerID][num];
							item.isRead = true;
							ChatController.countNewPrivateMessage--;
							ChatController.privateMessages[PrivateChatController.sharedController.selectedPlayerID][num] = item;
							PrivateChatController.sharedController.selectedPlayerItem.UpdateCountNewMessage();
							ChatController.SavePrivatMessageInPrefs();
							break;
						}
					}
					Dictionary<string, List<Dictionary<string, string>>> strs2 = new Dictionary<string, List<Dictionary<string, string>>>();
					foreach (KeyValuePair<string, List<ChatController.PrivateMessage>> keyValuePair in ChatController.privateMessages)
					{
						List<Dictionary<string, string>> dictionaries1 = new List<Dictionary<string, string>>();
						foreach (ChatController.PrivateMessage value1 in keyValuePair.Value)
						{
							Dictionary<string, string> strs3 = new Dictionary<string, string>()
							{
								{ "playerIDFrom", value1.playerIDFrom },
								{ "message", value1.message },
								{ "timeStamp", string.Empty },
								{ "isRead", value1.isRead.ToString() },
								{ "isSending", value1.isSending.ToString() }
							};
							dictionaries1.Add(strs3);
						}
						strs2.Add(keyValuePair.Key, dictionaries1);
					}
				}
			}
		}
	}
}