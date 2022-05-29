using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Room : RoomInfo
{
	public bool autoCleanUp
	{
		get
		{
			return this.autoCleanUpField;
		}
	}

	public string[] expectedUsers
	{
		get
		{
			return this.expectedUsersField;
		}
	}

	protected internal int masterClientId
	{
		get
		{
			return this.masterClientIdField;
		}
		set
		{
			this.masterClientIdField = value;
		}
	}

	public new int maxPlayers
	{
		get
		{
			return this.maxPlayersField;
		}
		set
		{
			if (!this.Equals(PhotonNetwork.room))
			{
				Debug.LogWarning("Can't set MaxPlayers when not in that room.");
			}
			if (value > 255)
			{
				Debug.LogWarning(string.Concat("Can't set Room.MaxPlayers to: ", value, ". Using max value: 255."));
				value = 255;
			}
			if (value != this.maxPlayersField && !PhotonNetwork.offlineMode)
			{
				NetworkingPeer networkingPeer = PhotonNetwork.networkingPeer;
				Hashtable hashtable = new Hashtable()
				{
					{ (byte)255, (byte)value }
				};
				networkingPeer.OpSetPropertiesOfRoom(hashtable, null, false);
			}
			this.maxPlayersField = (byte)value;
		}
	}

	public new string name
	{
		get
		{
			return this.nameField;
		}
		internal set
		{
			this.nameField = value;
		}
	}

	public new bool open
	{
		get
		{
			return this.openField;
		}
		set
		{
			if (!this.Equals(PhotonNetwork.room))
			{
				Debug.LogWarning("Can't set open when not in that room.");
			}
			if (value != this.openField && !PhotonNetwork.offlineMode)
			{
				NetworkingPeer networkingPeer = PhotonNetwork.networkingPeer;
				Hashtable hashtable = new Hashtable()
				{
					{ (byte)253, value }
				};
				networkingPeer.OpSetPropertiesOfRoom(hashtable, null, false);
			}
			this.openField = value;
		}
	}

	public new int playerCount
	{
		get
		{
			if (PhotonNetwork.playerList == null)
			{
				return 0;
			}
			return (int)PhotonNetwork.playerList.Length;
		}
	}

	public string[] propertiesListedInLobby
	{
		get;
		private set;
	}

	public new bool visible
	{
		get
		{
			return this.visibleField;
		}
		set
		{
			if (!this.Equals(PhotonNetwork.room))
			{
				Debug.LogWarning("Can't set visible when not in that room.");
			}
			if (value != this.visibleField && !PhotonNetwork.offlineMode)
			{
				NetworkingPeer networkingPeer = PhotonNetwork.networkingPeer;
				Hashtable hashtable = new Hashtable()
				{
					{ (byte)254, value }
				};
				networkingPeer.OpSetPropertiesOfRoom(hashtable, null, false);
			}
			this.visibleField = value;
		}
	}

	internal Room(string roomName, RoomOptions options) : base(roomName, null)
	{
		if (options == null)
		{
			options = new RoomOptions();
		}
		this.visibleField = options.IsVisible;
		this.openField = options.IsOpen;
		this.maxPlayersField = options.MaxPlayers;
		this.autoCleanUpField = false;
		base.InternalCacheProperties(options.CustomRoomProperties);
		this.propertiesListedInLobby = options.CustomRoomPropertiesForLobby;
	}

	public void ClearExpectedUsers()
	{
		Hashtable hashtable = new Hashtable();
		hashtable[(byte)247] = null;
		PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(hashtable, null, false);
	}

	public void SetCustomProperties(Hashtable propertiesToSet, Hashtable expectedValues = null, bool webForward = false)
	{
		if (propertiesToSet == null)
		{
			return;
		}
		Hashtable stringKeys = propertiesToSet.StripToStringKeys();
		Hashtable hashtable = expectedValues.StripToStringKeys();
		bool flag = (hashtable == null ? true : hashtable.Count == 0);
		if (!PhotonNetwork.offlineMode)
		{
			PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(stringKeys, hashtable, webForward);
		}
		if (PhotonNetwork.offlineMode || flag)
		{
			base.InternalCacheProperties(stringKeys);
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonCustomRoomPropertiesChanged, new object[] { stringKeys });
		}
	}

	public void SetPropertiesListedInLobby(string[] propsListedInLobby)
	{
		Hashtable hashtable = new Hashtable();
		hashtable[(byte)250] = propsListedInLobby;
		PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(hashtable, null, false);
		this.propertiesListedInLobby = propsListedInLobby;
	}

	public override string ToString()
	{
		object[] objArray = new object[] { this.nameField, null, null, null, null };
		objArray[1] = (!this.visibleField ? "hidden" : "visible");
		objArray[2] = (!this.openField ? "closed" : "open");
		objArray[3] = this.maxPlayersField;
		objArray[4] = this.playerCount;
		return string.Format("Room: '{0}' {1},{2} {4}/{3} players.", objArray);
	}

	public new string ToStringFull()
	{
		object[] stringFull = new object[] { this.nameField, null, null, null, null, null };
		stringFull[1] = (!this.visibleField ? "hidden" : "visible");
		stringFull[2] = (!this.openField ? "closed" : "open");
		stringFull[3] = this.maxPlayersField;
		stringFull[4] = this.playerCount;
		stringFull[5] = base.customProperties.ToStringFull();
		return string.Format("Room: '{0}' {1},{2} {4}/{3} players.\ncustomProps: {5}", stringFull);
	}
}