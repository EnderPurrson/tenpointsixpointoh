using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;

internal class LoadBalancingPeer : PhotonPeer
{
	private readonly Dictionary<byte, object> opParameters = new Dictionary<byte, object>();

	internal bool IsProtocolSecure
	{
		get
		{
			return base.UsedProtocol == ConnectionProtocol.WebSocketSecure;
		}
	}

	public LoadBalancingPeer(ConnectionProtocol protocolType) : base(protocolType)
	{
	}

	public LoadBalancingPeer(IPhotonPeerListener listener, ConnectionProtocol protocolType) : base(listener, protocolType)
	{
	}

	public virtual bool OpAuthenticate(string appId, string appVersion, AuthenticationValues authValues, string regionCode, bool getLobbyStatistics)
	{
		if (base.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpAuthenticate()");
		}
		Dictionary<byte, object> nums = new Dictionary<byte, object>();
		if (getLobbyStatistics)
		{
			nums[211] = true;
		}
		if (authValues != null && authValues.Token != null)
		{
			nums[221] = authValues.Token;
			return this.OpCustom(230, nums, true, 0, false);
		}
		nums[220] = appVersion;
		nums[224] = appId;
		if (!string.IsNullOrEmpty(regionCode))
		{
			nums[210] = regionCode;
		}
		if (authValues != null)
		{
			if (!string.IsNullOrEmpty(authValues.UserId))
			{
				nums[225] = authValues.UserId;
			}
			if (authValues.AuthType != CustomAuthenticationType.None)
			{
				if (!this.IsProtocolSecure && !base.IsEncryptionAvailable)
				{
					base.Listener.DebugReturn(DebugLevel.ERROR, "OpAuthenticate() failed. When you want Custom Authentication encryption is mandatory.");
					return false;
				}
				nums[217] = (byte)authValues.AuthType;
				if (string.IsNullOrEmpty(authValues.Token))
				{
					if (!string.IsNullOrEmpty(authValues.AuthGetParameters))
					{
						nums[216] = authValues.AuthGetParameters;
					}
					if (authValues.AuthPostData != null)
					{
						nums[214] = authValues.AuthPostData;
					}
				}
				else
				{
					nums[221] = authValues.Token;
				}
			}
		}
		bool flag = this.OpCustom(230, nums, true, 0, base.IsEncryptionAvailable);
		if (!flag)
		{
			base.Listener.DebugReturn(DebugLevel.ERROR, "Error calling OpAuthenticate! Did not work. Check log output, AuthValues and if you're connected.");
		}
		return flag;
	}

	public virtual bool OpChangeGroups(byte[] groupsToRemove, byte[] groupsToAdd)
	{
		if (base.DebugOut >= DebugLevel.ALL)
		{
			base.Listener.DebugReturn(DebugLevel.ALL, "OpChangeGroups()");
		}
		Dictionary<byte, object> nums = new Dictionary<byte, object>();
		if (groupsToRemove != null)
		{
			nums[239] = groupsToRemove;
		}
		if (groupsToAdd != null)
		{
			nums[238] = groupsToAdd;
		}
		return this.OpCustom(248, nums, true, 0);
	}

	public virtual bool OpCreateRoom(EnterRoomParams opParams)
	{
		if (base.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpCreateRoom()");
		}
		Dictionary<byte, object> nums = new Dictionary<byte, object>();
		if (!string.IsNullOrEmpty(opParams.RoomName))
		{
			nums[255] = opParams.RoomName;
		}
		if (opParams.Lobby != null && !string.IsNullOrEmpty(opParams.Lobby.Name))
		{
			nums[213] = opParams.Lobby.Name;
			nums[212] = (byte)opParams.Lobby.Type;
		}
		if (opParams.ExpectedUsers != null && (int)opParams.ExpectedUsers.Length > 0)
		{
			nums[238] = opParams.ExpectedUsers;
		}
		if (opParams.OnGameServer)
		{
			if (opParams.PlayerProperties != null && opParams.PlayerProperties.Count > 0)
			{
				nums[249] = opParams.PlayerProperties;
				nums[250] = true;
			}
			this.RoomOptionsToOpParameters(nums, opParams.RoomOptions);
		}
		return this.OpCustom(227, nums, true);
	}

	public virtual bool OpFindFriends(string[] friendsToFind)
	{
		Dictionary<byte, object> nums = new Dictionary<byte, object>();
		if (friendsToFind != null && (int)friendsToFind.Length > 0)
		{
			nums[1] = friendsToFind;
		}
		return this.OpCustom(222, nums, true);
	}

	public virtual bool OpGetRegions(string appId)
	{
		Dictionary<byte, object> nums = new Dictionary<byte, object>();
		nums[224] = appId;
		return this.OpCustom(220, nums, true, 0, true);
	}

	public virtual bool OpJoinLobby(TypedLobby lobby = null)
	{
		if (base.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpJoinLobby()");
		}
		Dictionary<byte, object> nums = null;
		if (lobby != null && !lobby.IsDefault)
		{
			nums = new Dictionary<byte, object>();
			nums[213] = lobby.Name;
			nums[212] = (byte)lobby.Type;
		}
		return this.OpCustom(229, nums, true);
	}

	public virtual bool OpJoinRandomRoom(OpJoinRandomRoomParams opJoinRandomRoomParams)
	{
		if (base.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpJoinRandomRoom()");
		}
		Hashtable hashtable = new Hashtable();
		hashtable.MergeStringKeys(opJoinRandomRoomParams.ExpectedCustomRoomProperties);
		if (opJoinRandomRoomParams.ExpectedMaxPlayers > 0)
		{
			hashtable[(byte)255] = opJoinRandomRoomParams.ExpectedMaxPlayers;
		}
		Dictionary<byte, object> nums = new Dictionary<byte, object>();
		if (hashtable.Count > 0)
		{
			nums[248] = hashtable;
		}
		if (opJoinRandomRoomParams.MatchingType != MatchmakingMode.FillRoom)
		{
			nums[223] = (byte)opJoinRandomRoomParams.MatchingType;
		}
		if (opJoinRandomRoomParams.TypedLobby != null && !string.IsNullOrEmpty(opJoinRandomRoomParams.TypedLobby.Name))
		{
			nums[213] = opJoinRandomRoomParams.TypedLobby.Name;
			nums[212] = (byte)opJoinRandomRoomParams.TypedLobby.Type;
		}
		if (!string.IsNullOrEmpty(opJoinRandomRoomParams.SqlLobbyFilter))
		{
			nums[245] = opJoinRandomRoomParams.SqlLobbyFilter;
		}
		if (opJoinRandomRoomParams.ExpectedUsers != null && (int)opJoinRandomRoomParams.ExpectedUsers.Length > 0)
		{
			nums[238] = opJoinRandomRoomParams.ExpectedUsers;
		}
		return this.OpCustom(225, nums, true);
	}

	public virtual bool OpJoinRoom(EnterRoomParams opParams)
	{
		if (base.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpJoinRoom()");
		}
		Dictionary<byte, object> nums = new Dictionary<byte, object>();
		if (!string.IsNullOrEmpty(opParams.RoomName))
		{
			nums[255] = opParams.RoomName;
		}
		if (opParams.CreateIfNotExists)
		{
			nums[215] = (byte)1;
			if (opParams.Lobby != null)
			{
				nums[213] = opParams.Lobby.Name;
				nums[212] = (byte)opParams.Lobby.Type;
			}
		}
		if (opParams.RejoinOnly)
		{
			nums[215] = (byte)3;
		}
		if (opParams.ExpectedUsers != null && (int)opParams.ExpectedUsers.Length > 0)
		{
			nums[238] = opParams.ExpectedUsers;
		}
		if (opParams.OnGameServer)
		{
			if (opParams.PlayerProperties != null && opParams.PlayerProperties.Count > 0)
			{
				nums[249] = opParams.PlayerProperties;
				nums[250] = true;
			}
			if (opParams.CreateIfNotExists)
			{
				this.RoomOptionsToOpParameters(nums, opParams.RoomOptions);
			}
		}
		return this.OpCustom(226, nums, true);
	}

	public virtual bool OpLeaveLobby()
	{
		if (base.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpLeaveLobby()");
		}
		return this.OpCustom(228, null, true);
	}

	public virtual bool OpLeaveRoom(bool becomeInactive)
	{
		Dictionary<byte, object> nums = new Dictionary<byte, object>();
		if (becomeInactive)
		{
			nums[233] = becomeInactive;
		}
		return this.OpCustom(254, nums, true);
	}

	public virtual bool OpRaiseEvent(byte eventCode, object customEventContent, bool sendReliable, RaiseEventOptions raiseEventOptions)
	{
		this.opParameters.Clear();
		this.opParameters[244] = eventCode;
		if (customEventContent != null)
		{
			this.opParameters[245] = customEventContent;
		}
		if (raiseEventOptions != null)
		{
			if (raiseEventOptions.CachingOption != EventCaching.DoNotCache)
			{
				this.opParameters[247] = (byte)raiseEventOptions.CachingOption;
			}
			if (raiseEventOptions.Receivers != ReceiverGroup.Others)
			{
				this.opParameters[246] = (byte)raiseEventOptions.Receivers;
			}
			if (raiseEventOptions.InterestGroup != 0)
			{
				this.opParameters[240] = raiseEventOptions.InterestGroup;
			}
			if (raiseEventOptions.TargetActors != null)
			{
				this.opParameters[252] = raiseEventOptions.TargetActors;
			}
			if (raiseEventOptions.ForwardToWebhook)
			{
				this.opParameters[234] = true;
			}
		}
		else
		{
			raiseEventOptions = RaiseEventOptions.Default;
		}
		return this.OpCustom(253, this.opParameters, sendReliable, raiseEventOptions.SequenceChannel, raiseEventOptions.Encrypt);
	}

	public bool OpSetCustomPropertiesOfActor(int actorNr, Hashtable actorProperties)
	{
		return this.OpSetPropertiesOfActor(actorNr, actorProperties.StripToStringKeys(), null, false);
	}

	public bool OpSetCustomPropertiesOfRoom(Hashtable gameProperties, bool broadcast, byte channelId)
	{
		return this.OpSetPropertiesOfRoom(gameProperties.StripToStringKeys(), null, false);
	}

	protected internal bool OpSetPropertiesOfActor(int actorNr, Hashtable actorProperties, Hashtable expectedProperties = null, bool webForward = false)
	{
		if (base.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpSetPropertiesOfActor()");
		}
		if (actorNr <= 0 || actorProperties == null)
		{
			if (base.DebugOut >= DebugLevel.INFO)
			{
				base.Listener.DebugReturn(DebugLevel.INFO, "OpSetPropertiesOfActor not sent. ActorNr must be > 0 and actorProperties != null.");
			}
			return false;
		}
		Dictionary<byte, object> nums = new Dictionary<byte, object>()
		{
			{ 251, actorProperties },
			{ 254, actorNr },
			{ 250, true }
		};
		if (expectedProperties != null && expectedProperties.Count != 0)
		{
			nums.Add(231, expectedProperties);
		}
		if (webForward)
		{
			nums[234] = true;
		}
		return this.OpCustom(252, nums, true, 0, false);
	}

	protected internal bool OpSetPropertiesOfRoom(Hashtable gameProperties, Hashtable expectedProperties = null, bool webForward = false)
	{
		if (base.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpSetPropertiesOfRoom()");
		}
		Dictionary<byte, object> nums = new Dictionary<byte, object>()
		{
			{ 251, gameProperties },
			{ 250, true }
		};
		if (expectedProperties != null && expectedProperties.Count != 0)
		{
			nums.Add(231, expectedProperties);
		}
		if (webForward)
		{
			nums[234] = true;
		}
		return this.OpCustom(252, nums, true, 0, false);
	}

	protected void OpSetPropertyOfRoom(byte propCode, object value)
	{
		Hashtable hashtable = new Hashtable();
		hashtable[propCode] = value;
		this.OpSetPropertiesOfRoom(hashtable, null, false);
	}

	private void RoomOptionsToOpParameters(Dictionary<byte, object> op, RoomOptions roomOptions)
	{
		if (roomOptions == null)
		{
			roomOptions = new RoomOptions();
		}
		Hashtable hashtable = new Hashtable();
		hashtable[(byte)253] = roomOptions.IsOpen;
		hashtable[(byte)254] = roomOptions.IsVisible;
		hashtable[(byte)250] = (roomOptions.CustomRoomPropertiesForLobby != null ? roomOptions.CustomRoomPropertiesForLobby : new string[0]);
		hashtable.MergeStringKeys(roomOptions.CustomRoomProperties);
		if (roomOptions.MaxPlayers > 0)
		{
			hashtable[(byte)255] = roomOptions.MaxPlayers;
		}
		op[248] = hashtable;
		op[241] = roomOptions.CleanupCacheOnLeave;
		if (roomOptions.CleanupCacheOnLeave)
		{
			hashtable[(byte)249] = true;
		}
		if (roomOptions.PlayerTtl > 0 || roomOptions.PlayerTtl == -1)
		{
			op[232] = true;
			op[235] = roomOptions.PlayerTtl;
			op[236] = roomOptions.PlayerTtl;
		}
		if (roomOptions.SuppressRoomEvents)
		{
			op[237] = true;
		}
		if (roomOptions.Plugins != null)
		{
			op[204] = roomOptions.Plugins;
		}
		if (roomOptions.PublishUserId)
		{
			op[239] = true;
		}
	}
}