using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class RoomInfo
{
	private Hashtable customPropertiesField = new Hashtable();

	protected byte maxPlayersField;

	protected string[] expectedUsersField;

	protected bool openField = true;

	protected bool visibleField = true;

	protected bool autoCleanUpField = PhotonNetwork.autoCleanUpPlayerObjects;

	protected string nameField;

	protected internal int masterClientIdField;

	public Hashtable customProperties
	{
		get
		{
			return this.customPropertiesField;
		}
	}

	public bool isLocalClientInside
	{
		get;
		set;
	}

	public byte maxPlayers
	{
		get
		{
			return this.maxPlayersField;
		}
	}

	public string name
	{
		get
		{
			return this.nameField;
		}
	}

	public bool open
	{
		get
		{
			return this.openField;
		}
	}

	public int playerCount
	{
		get;
		private set;
	}

	public bool removedFromList
	{
		get;
		internal set;
	}

	protected internal bool serverSideMasterClient
	{
		get;
		private set;
	}

	public bool visible
	{
		get
		{
			return this.visibleField;
		}
	}

	protected internal RoomInfo(string roomName, Hashtable properties)
	{
		this.InternalCacheProperties(properties);
		this.nameField = roomName;
	}

	public override bool Equals(object other)
	{
		RoomInfo roomInfo = other as RoomInfo;
		return (roomInfo == null ? false : this.name.Equals(roomInfo.nameField));
	}

	public override int GetHashCode()
	{
		return this.nameField.GetHashCode();
	}

	protected internal void InternalCacheProperties(Hashtable propertiesToCache)
	{
		if (propertiesToCache == null || propertiesToCache.Count == 0 || this.customPropertiesField.Equals(propertiesToCache))
		{
			return;
		}
		if (propertiesToCache.ContainsKey((byte)251))
		{
			this.removedFromList = (bool)propertiesToCache[(byte)251];
			if (this.removedFromList)
			{
				return;
			}
		}
		if (propertiesToCache.ContainsKey((byte)255))
		{
			this.maxPlayersField = (byte)propertiesToCache[(byte)255];
		}
		if (propertiesToCache.ContainsKey((byte)253))
		{
			this.openField = (bool)propertiesToCache[(byte)253];
		}
		if (propertiesToCache.ContainsKey((byte)254))
		{
			this.visibleField = (bool)propertiesToCache[(byte)254];
		}
		if (propertiesToCache.ContainsKey((byte)252))
		{
			this.playerCount = (byte)propertiesToCache[(byte)252];
		}
		if (propertiesToCache.ContainsKey((byte)249))
		{
			this.autoCleanUpField = (bool)propertiesToCache[(byte)249];
		}
		if (propertiesToCache.ContainsKey((byte)248))
		{
			this.serverSideMasterClient = true;
			bool flag = this.masterClientIdField != 0;
			this.masterClientIdField = (int)propertiesToCache[(byte)248];
			if (flag)
			{
				PhotonNetwork.networkingPeer.UpdateMasterClient();
			}
		}
		if (propertiesToCache.ContainsKey((byte)247))
		{
			this.expectedUsersField = (string[])propertiesToCache[(byte)247];
		}
		this.customPropertiesField.MergeStringKeys(propertiesToCache);
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

	public string ToStringFull()
	{
		object[] stringFull = new object[] { this.nameField, null, null, null, null, null };
		stringFull[1] = (!this.visibleField ? "hidden" : "visible");
		stringFull[2] = (!this.openField ? "closed" : "open");
		stringFull[3] = this.maxPlayersField;
		stringFull[4] = this.playerCount;
		stringFull[5] = this.customPropertiesField.ToStringFull();
		return string.Format("Room: '{0}' {1},{2} {4}/{3} players.\ncustomProps: {5}", stringFull);
	}
}