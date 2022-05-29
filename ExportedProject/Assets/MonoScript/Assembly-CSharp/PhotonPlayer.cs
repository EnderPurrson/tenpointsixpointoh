using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PhotonPlayer
{
	private int actorID = -1;

	private string nameField = string.Empty;

	public readonly bool isLocal;

	public object TagObject;

	public Hashtable allProperties
	{
		get
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Merge(this.customProperties);
			hashtable[(byte)255] = this.name;
			return hashtable;
		}
	}

	public Hashtable customProperties
	{
		get;
		internal set;
	}

	public int ID
	{
		get
		{
			return this.actorID;
		}
	}

	public bool isInactive
	{
		get;
		set;
	}

	public bool isMasterClient
	{
		get
		{
			return PhotonNetwork.networkingPeer.mMasterClientId == this.ID;
		}
	}

	public string name
	{
		get
		{
			return this.nameField;
		}
		set
		{
			if (!this.isLocal)
			{
				Debug.LogError("Error: Cannot change the name of a remote player!");
				return;
			}
			if (string.IsNullOrEmpty(value) || value.Equals(this.nameField))
			{
				return;
			}
			this.nameField = value;
			PhotonNetwork.playerName = value;
		}
	}

	public string userId
	{
		get;
		internal set;
	}

	public PhotonPlayer(bool isLocal, int actorID, string name)
	{
		this.customProperties = new Hashtable();
		this.isLocal = isLocal;
		this.actorID = actorID;
		this.nameField = name;
	}

	protected internal PhotonPlayer(bool isLocal, int actorID, Hashtable properties)
	{
		this.customProperties = new Hashtable();
		this.isLocal = isLocal;
		this.actorID = actorID;
		this.InternalCacheProperties(properties);
	}

	public override bool Equals(object p)
	{
		PhotonPlayer photonPlayer = p as PhotonPlayer;
		return (photonPlayer == null ? false : this.GetHashCode() == photonPlayer.GetHashCode());
	}

	public static PhotonPlayer Find(int ID)
	{
		if (PhotonNetwork.networkingPeer == null)
		{
			return null;
		}
		return PhotonNetwork.networkingPeer.GetPlayerWithId(ID);
	}

	public PhotonPlayer Get(int id)
	{
		return PhotonPlayer.Find(id);
	}

	public override int GetHashCode()
	{
		return this.ID;
	}

	public PhotonPlayer GetNext()
	{
		return this.GetNextFor(this.ID);
	}

	public PhotonPlayer GetNextFor(PhotonPlayer currentPlayer)
	{
		if (currentPlayer == null)
		{
			return null;
		}
		return this.GetNextFor(currentPlayer.ID);
	}

	public PhotonPlayer GetNextFor(int currentPlayerId)
	{
		if (PhotonNetwork.networkingPeer == null || PhotonNetwork.networkingPeer.mActors == null || PhotonNetwork.networkingPeer.mActors.Count < 2)
		{
			return null;
		}
		Dictionary<int, PhotonPlayer> nums = PhotonNetwork.networkingPeer.mActors;
		int num = 2147483647;
		int num1 = currentPlayerId;
		foreach (int key in nums.Keys)
		{
			if (key >= num1)
			{
				if (key <= currentPlayerId || key >= num)
				{
					continue;
				}
				num = key;
			}
			else
			{
				num1 = key;
			}
		}
		return (num == 2147483647 ? nums[num1] : nums[num]);
	}

	internal void InternalCacheProperties(Hashtable properties)
	{
		if (properties == null || properties.Count == 0 || this.customProperties.Equals(properties))
		{
			return;
		}
		if (properties.ContainsKey((byte)255))
		{
			this.nameField = (string)properties[(byte)255];
		}
		if (properties.ContainsKey((byte)253))
		{
			this.userId = (string)properties[(byte)253];
		}
		if (properties.ContainsKey((byte)254))
		{
			this.isInactive = (bool)properties[(byte)254];
		}
		this.customProperties.MergeStringKeys(properties);
		this.customProperties.StripKeysWithNullValues();
	}

	internal void InternalChangeLocalID(int newID)
	{
		if (!this.isLocal)
		{
			Debug.LogError("ERROR You should never change PhotonPlayer IDs!");
			return;
		}
		this.actorID = newID;
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
		bool flag1 = (this.actorID <= 0 ? false : !PhotonNetwork.offlineMode);
		if (flag1)
		{
			PhotonNetwork.networkingPeer.OpSetPropertiesOfActor(this.actorID, stringKeys, hashtable, webForward);
		}
		if (!flag1 || flag)
		{
			this.InternalCacheProperties(stringKeys);
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, new object[] { this, stringKeys });
		}
	}

	public override string ToString()
	{
		if (!string.IsNullOrEmpty(this.name))
		{
			return string.Format("'{0}'{1}{2}", this.name, (!this.isInactive ? " " : " (inactive)"), (!this.isMasterClient ? string.Empty : "(master)"));
		}
		return string.Format("#{0:00}{1}{2}", this.ID, (!this.isInactive ? " " : " (inactive)"), (!this.isMasterClient ? string.Empty : "(master)"));
	}

	public string ToStringFull()
	{
		object[] d = new object[] { this.ID, this.name, null, null };
		d[2] = (!this.isInactive ? string.Empty : " (inactive)");
		d[3] = this.customProperties.ToStringFull();
		return string.Format("#{0:00} '{1}'{2} {3}", d);
	}
}