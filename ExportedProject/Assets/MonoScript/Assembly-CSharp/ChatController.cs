using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChatController : MonoBehaviour
{
	public static ChatController sharedController;

	public static int countNewPrivateMessage;

	private static string privateMessageKey;

	private static string privateMessageSendKey;

	public static float timerToUpdateMessage;

	public static bool fastSendMessage;

	public static float maxTimerToUpdateMessage;

	public static Dictionary<string, List<ChatController.PrivateMessage>> privateMessages;

	public static Dictionary<string, List<ChatController.PrivateMessage>> privateMessagesForSend;

	private Action _backButtonCallback;

	static ChatController()
	{
		ChatController.sharedController = null;
		ChatController.countNewPrivateMessage = 0;
		ChatController.privateMessageKey = "PrivateMessageKey";
		ChatController.privateMessageSendKey = "PrivateMessageSendKey";
		ChatController.maxTimerToUpdateMessage = 10f;
		ChatController.privateMessages = new Dictionary<string, List<ChatController.PrivateMessage>>();
		ChatController.privateMessagesForSend = new Dictionary<string, List<ChatController.PrivateMessage>>();
	}

	public ChatController()
	{
	}

	public static void AddPrivateMessage(string _playerIdChating, ChatController.PrivateMessage _message)
	{
		if (!ChatController.privateMessages.ContainsKey(_playerIdChating))
		{
			ChatController.privateMessages.Add(_playerIdChating, new List<ChatController.PrivateMessage>());
		}
		ChatController.privateMessages[_playerIdChating].Add(_message);
		while (ChatController.privateMessages[_playerIdChating].Count > Defs.historyPrivateMessageLength)
		{
			if (!ChatController.privateMessages[_playerIdChating][0].isRead)
			{
				ChatController.countNewPrivateMessage--;
			}
			ChatController.privateMessages[_playerIdChating].RemoveAt(0);
		}
	}

	public void BackButtonClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		if (this._backButtonCallback != null)
		{
			this._backButtonCallback();
			return;
		}
		Singleton<SceneLoader>.Instance.LoadScene(Defs.MainMenuScene, LoadSceneMode.Single);
	}

	public static void FillPrivateMessageForSendFromPrefs()
	{
		ChatController.privateMessagesForSend.Clear();
		string str = PlayerPrefs.GetString(ChatController.privateMessageSendKey, "[]");
		List<object> objs = null;
		objs = Json.Deserialize(str) as List<object>;
		if (objs != null && objs.Count > 0)
		{
			foreach (object obj in objs)
			{
				Dictionary<string, string> strs = obj as Dictionary<string, string>;
				string item = strs["to"];
				string item1 = strs["text"];
				double num = double.Parse(strs["timeStamp"], NumberStyles.Number, CultureInfo.InvariantCulture);
				bool flag = true;
				bool flag1 = false;
				if (!ChatController.privateMessagesForSend.ContainsKey(item))
				{
					ChatController.privateMessagesForSend.Add(item, new List<ChatController.PrivateMessage>());
				}
				ChatController.privateMessagesForSend[item].Add(new ChatController.PrivateMessage(item, item1, num, flag, flag1));
			}
		}
	}

	public static void FillPrivatMessageFromPrefs()
	{
		double num;
		ChatController.privateMessages.Clear();
		int num1 = 0;
		string str = PlayerPrefs.GetString(ChatController.privateMessageKey, "{}");
		Dictionary<string, object> strs = null;
		strs = Json.Deserialize(str) as Dictionary<string, object>;
		if (strs != null && strs.Count > 0)
		{
			foreach (KeyValuePair<string, object> keyValuePair in strs)
			{
				foreach (object value in keyValuePair.Value as List<object>)
				{
					Dictionary<string, object> strs1 = value as Dictionary<string, object>;
					string item = strs1["playerIDFrom"] as string;
					string item1 = strs1["message"] as string;
					double currentUnixTime = (double)Tools.CurrentUnixTime;
					if (!double.TryParse(strs1["timeStamp"].ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out num))
					{
						UnityEngine.Debug.LogWarning(string.Format("Could not parse timestamp {0}", keyValuePair.Value));
					}
					else
					{
						currentUnixTime = num;
					}
					bool flag = bool.Parse(strs1["isRead"] as string);
					if (!flag)
					{
						num1++;
					}
					bool flag1 = bool.Parse(strs1["isSending"] as string);
					ChatController.PrivateMessage privateMessage = new ChatController.PrivateMessage(item, item1, currentUnixTime, flag1, flag);
					ChatController.AddPrivateMessage(keyValuePair.Key, privateMessage);
				}
			}
		}
		ChatController.countNewPrivateMessage = num1;
	}

	public static string GetPrivateChatJsonForSend()
	{
		return PlayerPrefs.GetString(ChatController.privateMessageSendKey, "[]");
	}

	[DebuggerHidden]
	public IEnumerator MyWaitForSeconds(float tm)
	{
		ChatController.u003cMyWaitForSecondsu003ec__IteratorE variable = null;
		return variable;
	}

	private void OnDestroy()
	{
		ChatController.sharedController = null;
		base.StopAllCoroutines();
	}

	public void ParseUpdateChatMessageResponse(string response)
	{
		double num;
		double num1;
		Dictionary<string, object> strs = Json.Deserialize(response) as Dictionary<string, object>;
		bool flag = false;
		if (strs != null && strs.Count > 0)
		{
			if (strs.ContainsKey("user_messages_added"))
			{
				foreach (KeyValuePair<string, object> item in strs["user_messages_added"] as Dictionary<string, object>)
				{
					if (double.TryParse(item.Key, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
					{
						foreach (KeyValuePair<string, List<ChatController.PrivateMessage>> keyValuePair in ChatController.privateMessagesForSend)
						{
							int num2 = -1;
							int num3 = 0;
							while (num3 < keyValuePair.Value.Count)
							{
								ChatController.PrivateMessage privateMessage = keyValuePair.Value[num3];
								if (privateMessage.timeStamp != num)
								{
									num3++;
								}
								else
								{
									num2 = num3;
									if (double.TryParse(item.Value.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out num1))
									{
										privateMessage.timeStamp = num1;
									}
									string key = keyValuePair.Key;
									ChatController.PrivateMessage privateMessage1 = new ChatController.PrivateMessage(FriendsController.sharedController.id, privateMessage.message, privateMessage.timeStamp, true, true);
									ChatController.AddPrivateMessage(key, privateMessage1);
									flag = true;
									break;
								}
							}
							if (num2 < 0)
							{
								continue;
							}
							keyValuePair.Value.RemoveAt(num2);
							break;
						}
					}
					else
					{
						UnityEngine.Debug.LogWarning(string.Format("Could not parse timestamp {0}    Current culture: {1}, current UI culture: {2}", item.Value, CultureInfo.CurrentCulture.Name, CultureInfo.CurrentUICulture.Name));
					}
				}
			}
			if (strs.ContainsKey("user_messages"))
			{
				foreach (object obj in strs["user_messages"] as List<object>)
				{
					foreach (KeyValuePair<string, object> keyValuePair1 in obj as Dictionary<string, object>)
					{
						Dictionary<string, object> value = keyValuePair1.Value as Dictionary<string, object>;
						string str = value["from"] as string;
						string item1 = value["text"] as string;
						double currentUnixTime = (double)Tools.CurrentUnixTime;
						if (!double.TryParse(keyValuePair1.Key, NumberStyles.Number, CultureInfo.InvariantCulture, out currentUnixTime))
						{
							UnityEngine.Debug.LogWarning(string.Format("Could not parse message body key {0};    full response: {1}", keyValuePair1.Key, response));
						}
						double num4 = currentUnixTime;
						ChatController.AddPrivateMessage(str, new ChatController.PrivateMessage(str, item1, num4, true, false));
						ChatController.countNewPrivateMessage++;
						if (PrivateChatController.sharedController != null)
						{
							PrivateChatController.sharedController.UpdateFriendItemsInfoAndSort();
						}
						flag = true;
					}
				}
			}
			if (ChatController.privateMessages != null)
			{
				List<string> strs1 = new List<string>();
				foreach (KeyValuePair<string, List<ChatController.PrivateMessage>> privateMessage2 in ChatController.privateMessages)
				{
					if (FriendsController.sharedController.friends.Contains(privateMessage2.Key))
					{
						continue;
					}
					foreach (ChatController.PrivateMessage value1 in privateMessage2.Value)
					{
						if (value1.isRead)
						{
							continue;
						}
						ChatController.countNewPrivateMessage--;
					}
					strs1.Add(privateMessage2.Key);
					flag = true;
				}
				foreach (string str1 in strs1)
				{
					ChatController.privateMessages.Remove(str1);
				}
			}
		}
		if (flag)
		{
			ChatController.SavePrivatMessageInPrefs();
			if (PrivateChatController.sharedController != null)
			{
				PrivateChatController.sharedController.UpdateMessageForSelectedUsers(false);
			}
		}
	}

	public static void SavePrivatMessageInPrefs()
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
					{ "timeStamp", value.timeStamp.ToString("F8", CultureInfo.InvariantCulture) },
					{ "isRead", value.isRead.ToString() },
					{ "isSending", value.isSending.ToString() }
				};
				dictionaries.Add(strs1);
			}
			strs.Add(privateMessage.Key, dictionaries);
		}
		string str = Json.Serialize(strs);
		PlayerPrefs.SetString(ChatController.privateMessageKey, str ?? "{}");
		List<Dictionary<string, string>> dictionaries1 = new List<Dictionary<string, string>>();
		foreach (KeyValuePair<string, List<ChatController.PrivateMessage>> keyValuePair in ChatController.privateMessagesForSend)
		{
			List<Dictionary<string, string>> dictionaries2 = new List<Dictionary<string, string>>();
			foreach (ChatController.PrivateMessage value1 in keyValuePair.Value)
			{
				Dictionary<string, string> strs2 = new Dictionary<string, string>()
				{
					{ "to", keyValuePair.Key },
					{ "text", value1.message },
					{ "timeStamp", value1.timeStamp.ToString("F8", CultureInfo.InvariantCulture) }
				};
				dictionaries1.Add(strs2);
			}
		}
		string str1 = Json.Serialize(dictionaries1);
		PlayerPrefs.SetString(ChatController.privateMessageSendKey, str1 ?? "[]");
		PlayerPrefs.Save();
	}

	public void SelectPrivateChatMode()
	{
	}

	public void Show(Action exitCallback)
	{
		base.gameObject.SetActive(true);
		this._backButtonCallback = exitCallback;
	}

	private void Start()
	{
		ChatController.sharedController = this;
		ChatController.FillPrivatMessageFromPrefs();
	}

	public struct PrivateMessage
	{
		public string playerIDFrom;

		public string message;

		public double timeStamp;

		public bool isRead;

		public bool isSending;

		public PrivateMessage(string _playerIDFrom, string _message, double _timeStamp, bool _isSending, bool _isRead)
		{
			this.playerIDFrom = _playerIDFrom;
			this.message = _message;
			this.timeStamp = _timeStamp;
			this.isSending = _isSending;
			this.isRead = _isRead;
		}
	}
}