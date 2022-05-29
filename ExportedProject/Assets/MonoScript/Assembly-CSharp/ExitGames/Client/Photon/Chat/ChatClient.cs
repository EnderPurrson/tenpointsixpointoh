using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ExitGames.Client.Photon.Chat
{
	public class ChatClient : IPhotonPeerListener
	{
		private const int FriendRequestListMax = 1024;

		private const string ChatApppName = "chat";

		private string chatRegion = "EU";

		public int MessageLimit;

		public readonly Dictionary<string, ChatChannel> PublicChannels;

		public readonly Dictionary<string, ChatChannel> PrivateChannels;

		private readonly IChatClientListener listener;

		internal ChatPeer chatPeer;

		private bool didAuthenticate;

		private int msDeltaForServiceCalls = 50;

		private int msTimestampOfLastServiceCall;

		public string AppId
		{
			get;
			private set;
		}

		public string AppVersion
		{
			get;
			private set;
		}

		public AuthenticationValues AuthValues
		{
			get;
			set;
		}

		public bool CanChat
		{
			get
			{
				return (this.State != ChatState.ConnectedToFrontEnd ? false : this.HasPeer);
			}
		}

		public string ChatRegion
		{
			get
			{
				return this.chatRegion;
			}
			set
			{
				this.chatRegion = value;
			}
		}

		public DebugLevel DebugOut
		{
			get
			{
				return this.chatPeer.DebugOut;
			}
			set
			{
				this.chatPeer.DebugOut = value;
			}
		}

		public ChatDisconnectCause DisconnectedCause
		{
			get;
			private set;
		}

		public string FrontendAddress
		{
			get;
			private set;
		}

		private bool HasPeer
		{
			get
			{
				return this.chatPeer != null;
			}
		}

		public string NameServerAddress
		{
			get;
			private set;
		}

		public ChatState State
		{
			get;
			private set;
		}

		public string UserId
		{
			get
			{
				string userId;
				if (this.AuthValues == null)
				{
					userId = null;
				}
				else
				{
					userId = this.AuthValues.UserId;
				}
				return userId;
			}
			private set
			{
				if (this.AuthValues == null)
				{
					this.AuthValues = new AuthenticationValues();
				}
				this.AuthValues.UserId = value;
			}
		}

		public ChatClient(IChatClientListener listener, ConnectionProtocol protocol = 0)
		{
			this.listener = listener;
			this.State = ChatState.Uninitialized;
			this.chatPeer = new ChatPeer(this, protocol);
			this.PublicChannels = new Dictionary<string, ChatChannel>();
			this.PrivateChannels = new Dictionary<string, ChatChannel>();
		}

		public bool AddFriends(string[] friends)
		{
			if (!this.CanChat)
			{
				this.listener.DebugReturn(DebugLevel.ERROR, "AddFriends called while not connected to front end server.");
				return false;
			}
			if (friends == null || (int)friends.Length == 0)
			{
				this.listener.DebugReturn(DebugLevel.WARNING, "AddFriends can't be called for empty or null list.");
				return false;
			}
			if ((int)friends.Length <= 1024)
			{
				Dictionary<byte, object> nums = new Dictionary<byte, object>()
				{
					{ 11, friends }
				};
				return this.chatPeer.OpCustom(6, nums, true);
			}
			this.listener.DebugReturn(DebugLevel.WARNING, string.Concat(new object[] { "AddFriends max list size exceeded: ", (int)friends.Length, " > ", 1024 }));
			return false;
		}

		private bool AuthenticateOnFrontEnd()
		{
			if (this.AuthValues == null)
			{
				this.listener.DebugReturn(DebugLevel.ERROR, "Can't authenticate on front end server. Authentication Values are not set");
				return false;
			}
			if (this.AuthValues.Token == null || this.AuthValues.Token == string.Empty)
			{
				this.listener.DebugReturn(DebugLevel.ERROR, "Can't authenticate on front end server. Secret is not set");
				return false;
			}
			Dictionary<byte, object> nums = new Dictionary<byte, object>()
			{
				{ 221, this.AuthValues.Token }
			};
			return this.chatPeer.OpCustom(230, nums, true);
		}

		public bool Connect(string appId, string appVersion, AuthenticationValues authValues)
		{
			this.chatPeer.TimePingInterval = 3000;
			this.DisconnectedCause = ChatDisconnectCause.None;
			if (authValues == null)
			{
				this.listener.DebugReturn(DebugLevel.ERROR, "Connect failed: no authentication values specified");
				return false;
			}
			this.AuthValues = authValues;
			if (this.AuthValues.UserId == null || this.AuthValues.UserId == string.Empty)
			{
				this.listener.DebugReturn(DebugLevel.ERROR, "Connect failed: no UserId specified in authentication values");
				return false;
			}
			this.AppId = appId;
			this.AppVersion = appVersion;
			this.didAuthenticate = false;
			this.msDeltaForServiceCalls = 100;
			this.chatPeer.QuickResendAttempts = 2;
			this.chatPeer.SentCountAllowance = 7;
			this.PublicChannels.Clear();
			this.PrivateChannels.Clear();
			this.NameServerAddress = this.chatPeer.NameServerAddress;
			bool flag = this.chatPeer.Connect();
			if (flag)
			{
				this.State = ChatState.ConnectingToNameServer;
			}
			return flag;
		}

		private void ConnectToFrontEnd()
		{
			this.State = ChatState.ConnectingToFrontEnd;
			this.listener.DebugReturn(DebugLevel.INFO, string.Concat("Connecting to frontend ", this.FrontendAddress));
			this.chatPeer.Connect(this.FrontendAddress, "chat");
		}

		public void Disconnect()
		{
			if (this.HasPeer && this.chatPeer.PeerState != PeerStateValue.Disconnected)
			{
				this.chatPeer.Disconnect();
			}
		}

		void ExitGames.Client.Photon.IPhotonPeerListener.DebugReturn(DebugLevel level, string message)
		{
			this.listener.DebugReturn(level, message);
		}

		void ExitGames.Client.Photon.IPhotonPeerListener.OnEvent(EventData eventData)
		{
			switch (eventData.Code)
			{
				case 0:
				{
					this.HandleChatMessagesEvent(eventData);
					return;
				}
				case 1:
				case 3:
				{
					return;
				}
				case 2:
				{
					this.HandlePrivateMessageEvent(eventData);
					return;
				}
				case 4:
				{
					this.HandleStatusUpdate(eventData);
					return;
				}
				case 5:
				{
					this.HandleSubscribeEvent(eventData);
					return;
				}
				case 6:
				{
					this.HandleUnsubscribeEvent(eventData);
					return;
				}
				default:
				{
					return;
				}
			}
		}

		void ExitGames.Client.Photon.IPhotonPeerListener.OnOperationResponse(OperationResponse operationResponse)
		{
			byte operationCode = operationResponse.OperationCode;
			switch (operationCode)
			{
				case 0:
				case 1:
				case 2:
				case 3:
				{
					if (operationResponse.ReturnCode != 0)
					{
						if (operationResponse.ReturnCode != -2)
						{
							this.listener.DebugReturn(DebugLevel.ERROR, string.Format("Chat Operation {0} failed (Code: {1}). Debug Message: {2}", operationResponse.OperationCode, operationResponse.ReturnCode, operationResponse.DebugMessage));
						}
						else
						{
							this.listener.DebugReturn(DebugLevel.ERROR, string.Format("Chat Operation {0} unknown on server. Check your AppId and make sure it's for a Chat application.", operationResponse.OperationCode));
						}
					}
					break;
				}
				default:
				{
					if (operationCode == 230)
					{
						this.HandleAuthResponse(operationResponse);
						break;
					}
					else
					{
						goto case 3;
					}
				}
			}
		}

		void ExitGames.Client.Photon.IPhotonPeerListener.OnStatusChanged(StatusCode statusCode)
		{
			StatusCode statusCode1 = statusCode;
			if (statusCode1 == StatusCode.Connect)
			{
				if (this.chatPeer.IsProtocolSecure)
				{
					Debug.Log("Skipping Encryption");
					if (!this.didAuthenticate)
					{
						this.didAuthenticate = this.chatPeer.AuthenticateOnNameServer(this.AppId, this.AppVersion, this.chatRegion, this.AuthValues);
						if (!this.didAuthenticate)
						{
							((IPhotonPeerListener)this).DebugReturn(DebugLevel.ERROR, string.Concat("Error calling OpAuthenticate! Did not work. Check log output, AuthValues and if you're connected. State: ", this.State));
						}
					}
				}
				else
				{
					Debug.Log("Establishing Encryption");
					this.chatPeer.EstablishEncryption();
				}
				if (this.State == ChatState.ConnectingToNameServer)
				{
					this.State = ChatState.ConnectedToNameServer;
					this.listener.OnChatStateChange(this.State);
				}
				else if (this.State == ChatState.ConnectingToFrontEnd)
				{
					this.AuthenticateOnFrontEnd();
				}
			}
			else if (statusCode1 == StatusCode.Disconnect)
			{
				if (this.State != ChatState.Authenticated)
				{
					this.State = ChatState.Disconnected;
					this.listener.OnChatStateChange(ChatState.Disconnected);
					this.listener.OnDisconnected();
				}
				else
				{
					this.ConnectToFrontEnd();
				}
			}
			else if (statusCode1 != StatusCode.EncryptionEstablished)
			{
				if (statusCode1 == StatusCode.EncryptionFailedToEstablish)
				{
					this.State = ChatState.Disconnecting;
					this.chatPeer.Disconnect();
				}
			}
			else if (!this.didAuthenticate)
			{
				this.didAuthenticate = this.chatPeer.AuthenticateOnNameServer(this.AppId, this.AppVersion, this.chatRegion, this.AuthValues);
				if (!this.didAuthenticate)
				{
					((IPhotonPeerListener)this).DebugReturn(DebugLevel.ERROR, string.Concat("Error calling OpAuthenticate! Did not work. Check log output, AuthValues and if you're connected. State: ", this.State));
				}
			}
		}

		public string GetPrivateChannelNameByUser(string userName)
		{
			return string.Format("{0}:{1}", this.UserId, userName);
		}

		private void HandleAuthResponse(OperationResponse operationResponse)
		{
			this.listener.DebugReturn(DebugLevel.INFO, string.Concat(operationResponse.ToStringFull(), " on: ", this.chatPeer.NameServerAddress));
			if (operationResponse.ReturnCode != 0)
			{
				short returnCode = operationResponse.ReturnCode;
				switch (returnCode)
				{
					case 32755:
					{
						this.DisconnectedCause = ChatDisconnectCause.CustomAuthenticationFailed;
						break;
					}
					case 32756:
					{
						this.DisconnectedCause = ChatDisconnectCause.InvalidRegion;
						break;
					}
					case 32757:
					{
						this.DisconnectedCause = ChatDisconnectCause.MaxCcuReached;
						break;
					}
					default:
					{
						if (returnCode == -3)
						{
							this.DisconnectedCause = ChatDisconnectCause.OperationNotAllowedInCurrentState;
							break;
						}
						else if (returnCode == 32767)
						{
							this.DisconnectedCause = ChatDisconnectCause.InvalidAuthentication;
							break;
						}
						else
						{
							break;
						}
					}
				}
				this.listener.DebugReturn(DebugLevel.ERROR, string.Concat("Authentication request error: ", operationResponse.ReturnCode, ". Disconnecting."));
				this.State = ChatState.Disconnecting;
				this.chatPeer.Disconnect();
			}
			else if (this.State == ChatState.ConnectedToNameServer)
			{
				this.State = ChatState.Authenticated;
				this.listener.OnChatStateChange(this.State);
				if (!operationResponse.Parameters.ContainsKey(221))
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "No secret in authentication response.");
				}
				else
				{
					if (this.AuthValues == null)
					{
						this.AuthValues = new AuthenticationValues();
					}
					this.AuthValues.Token = operationResponse[221] as string;
					this.FrontendAddress = (string)operationResponse[230];
					this.chatPeer.Disconnect();
				}
			}
			else if (this.State == ChatState.ConnectingToFrontEnd)
			{
				this.msDeltaForServiceCalls *= 4;
				this.State = ChatState.ConnectedToFrontEnd;
				this.listener.OnChatStateChange(this.State);
				this.listener.OnConnected();
			}
		}

		private void HandleChatMessagesEvent(EventData eventData)
		{
			ChatChannel chatChannel;
			object[] item = (object[])eventData.Parameters[2];
			string[] strArrays = (string[])eventData.Parameters[4];
			string str = (string)eventData.Parameters[1];
			if (!this.PublicChannels.TryGetValue(str, out chatChannel))
			{
				this.listener.DebugReturn(DebugLevel.WARNING, string.Concat("Channel ", str, " for incoming message event not found."));
				return;
			}
			chatChannel.Add(strArrays, item);
			this.listener.OnGetMessages(str, strArrays, item);
		}

		private void HandlePrivateMessageEvent(EventData eventData)
		{
			string privateChannelNameByUser;
			ChatChannel chatChannel;
			object item = eventData.Parameters[3];
			string str = (string)eventData.Parameters[5];
			if (this.UserId == null || !this.UserId.Equals(str))
			{
				privateChannelNameByUser = this.GetPrivateChannelNameByUser(str);
			}
			else
			{
				string item1 = (string)eventData.Parameters[225];
				privateChannelNameByUser = this.GetPrivateChannelNameByUser(item1);
			}
			if (!this.PrivateChannels.TryGetValue(privateChannelNameByUser, out chatChannel))
			{
				chatChannel = new ChatChannel(privateChannelNameByUser)
				{
					IsPrivate = true,
					MessageLimit = this.MessageLimit
				};
				this.PrivateChannels.Add(chatChannel.Name, chatChannel);
			}
			chatChannel.Add(str, item);
			this.listener.OnPrivateMessage(str, item, privateChannelNameByUser);
		}

		private void HandleStatusUpdate(EventData eventData)
		{
			string item = (string)eventData.Parameters[5];
			int num = (int)eventData.Parameters[10];
			object obj = null;
			bool flag = eventData.Parameters.ContainsKey(3);
			if (flag)
			{
				obj = eventData.Parameters[3];
			}
			this.listener.OnStatusUpdate(item, num, flag, obj);
		}

		private void HandleSubscribeEvent(EventData eventData)
		{
			string[] item = (string[])eventData.Parameters[0];
			bool[] flagArray = (bool[])eventData.Parameters[15];
			for (int i = 0; i < (int)item.Length; i++)
			{
				if (flagArray[i])
				{
					string str = item[i];
					if (!this.PublicChannels.ContainsKey(str))
					{
						ChatChannel chatChannel = new ChatChannel(str)
						{
							MessageLimit = this.MessageLimit
						};
						this.PublicChannels.Add(chatChannel.Name, chatChannel);
					}
				}
			}
			this.listener.OnSubscribed(item, flagArray);
		}

		private void HandleUnsubscribeEvent(EventData eventData)
		{
			string[] item = (string[])eventData[0];
			for (int i = 0; i < (int)item.Length; i++)
			{
				string str = item[i];
				this.PublicChannels.Remove(str);
			}
			this.listener.OnUnsubscribed(item);
		}

		public bool PublishMessage(string channelName, object message)
		{
			if (!this.CanChat)
			{
				this.listener.DebugReturn(DebugLevel.ERROR, "PublishMessage called while not connected to front end server.");
				return false;
			}
			if (string.IsNullOrEmpty(channelName) || message == null)
			{
				this.listener.DebugReturn(DebugLevel.WARNING, "PublishMessage parameters must be non-null and not empty.");
				return false;
			}
			Dictionary<byte, object> nums = new Dictionary<byte, object>()
			{
				{ 1, channelName },
				{ 3, message }
			};
			return this.chatPeer.OpCustom(2, nums, true);
		}

		public bool RemoveFriends(string[] friends)
		{
			if (!this.CanChat)
			{
				this.listener.DebugReturn(DebugLevel.ERROR, "RemoveFriends called while not connected to front end server.");
				return false;
			}
			if (friends == null || (int)friends.Length == 0)
			{
				this.listener.DebugReturn(DebugLevel.WARNING, "RemoveFriends can't be called for empty or null list.");
				return false;
			}
			if ((int)friends.Length <= 1024)
			{
				Dictionary<byte, object> nums = new Dictionary<byte, object>()
				{
					{ 11, friends }
				};
				return this.chatPeer.OpCustom(7, nums, true);
			}
			this.listener.DebugReturn(DebugLevel.WARNING, string.Concat(new object[] { "RemoveFriends max list size exceeded: ", (int)friends.Length, " > ", 1024 }));
			return false;
		}

		public void SendAcksOnly()
		{
			if (this.chatPeer != null)
			{
				this.chatPeer.SendAcksOnly();
			}
		}

		private bool SendChannelOperation(string[] channels, byte operation, int historyLength)
		{
			Dictionary<byte, object> nums = new Dictionary<byte, object>()
			{
				{ 0, channels }
			};
			if (historyLength != 0)
			{
				nums.Add(14, historyLength);
			}
			return this.chatPeer.OpCustom(operation, nums, true);
		}

		public bool SendPrivateMessage(string target, object message)
		{
			return this.SendPrivateMessage(target, message, false);
		}

		public bool SendPrivateMessage(string target, object message, bool encrypt)
		{
			if (!this.CanChat)
			{
				this.listener.DebugReturn(DebugLevel.ERROR, "SendPrivateMessage called while not connected to front end server.");
				return false;
			}
			if (string.IsNullOrEmpty(target) || message == null)
			{
				this.listener.DebugReturn(DebugLevel.WARNING, "SendPrivateMessage parameters must be non-null and not empty.");
				return false;
			}
			Dictionary<byte, object> nums = new Dictionary<byte, object>()
			{
				{ 225, target },
				{ 3, message }
			};
			return this.chatPeer.OpCustom(3, nums, true, 0, encrypt);
		}

		public void Service()
		{
			if (this.HasPeer && (Environment.TickCount - this.msTimestampOfLastServiceCall > this.msDeltaForServiceCalls || this.msTimestampOfLastServiceCall == 0))
			{
				this.msTimestampOfLastServiceCall = Environment.TickCount;
				this.chatPeer.Service();
			}
		}

		private bool SetOnlineStatus(int status, object message, bool skipMessage)
		{
			if (!this.CanChat)
			{
				this.listener.DebugReturn(DebugLevel.ERROR, "SetOnlineStatus called while not connected to front end server.");
				return false;
			}
			Dictionary<byte, object> nums = new Dictionary<byte, object>()
			{
				{ 10, status }
			};
			if (!skipMessage)
			{
				nums[3] = message;
			}
			else
			{
				nums[12] = true;
			}
			return this.chatPeer.OpCustom(5, nums, true);
		}

		public bool SetOnlineStatus(int status)
		{
			return this.SetOnlineStatus(status, null, true);
		}

		public bool SetOnlineStatus(int status, object message)
		{
			return this.SetOnlineStatus(status, message, false);
		}

		public void StopThread()
		{
			if (this.HasPeer)
			{
				this.chatPeer.StopThread();
			}
		}

		public bool Subscribe(string[] channels)
		{
			return this.Subscribe(channels, 0);
		}

		public bool Subscribe(string[] channels, int messagesFromHistory)
		{
			if (!this.CanChat)
			{
				this.listener.DebugReturn(DebugLevel.ERROR, "Subscribe called while not connected to front end server.");
				return false;
			}
			if (channels != null && (int)channels.Length != 0)
			{
				return this.SendChannelOperation(channels, 0, messagesFromHistory);
			}
			this.listener.DebugReturn(DebugLevel.WARNING, "Subscribe can't be called for empty or null channels-list.");
			return false;
		}

		public bool TryGetChannel(string channelName, bool isPrivate, out ChatChannel channel)
		{
			if (!isPrivate)
			{
				return this.PublicChannels.TryGetValue(channelName, out channel);
			}
			return this.PrivateChannels.TryGetValue(channelName, out channel);
		}

		public bool TryGetChannel(string channelName, out ChatChannel channel)
		{
			if (this.PublicChannels.TryGetValue(channelName, out channel))
			{
				return true;
			}
			return this.PrivateChannels.TryGetValue(channelName, out channel);
		}

		public bool Unsubscribe(string[] channels)
		{
			if (!this.CanChat)
			{
				this.listener.DebugReturn(DebugLevel.ERROR, "Unsubscribe called while not connected to front end server.");
				return false;
			}
			if (channels != null && (int)channels.Length != 0)
			{
				return this.SendChannelOperation(channels, 1, 0);
			}
			this.listener.DebugReturn(DebugLevel.WARNING, "Unsubscribe can't be called for empty or null channels-list.");
			return false;
		}
	}
}