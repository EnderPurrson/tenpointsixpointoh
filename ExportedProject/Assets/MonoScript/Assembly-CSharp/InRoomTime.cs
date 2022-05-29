using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InRoomTime : MonoBehaviour
{
	private const string StartTimeKey = "#rt";

	private int roomStartTimestamp;

	public bool IsRoomTimeSet
	{
		get
		{
			return (!PhotonNetwork.inRoom ? false : PhotonNetwork.room.customProperties.ContainsKey("#rt"));
		}
	}

	public double RoomTime
	{
		get
		{
			return (double)((float)this.RoomTimestamp) / 1000;
		}
	}

	public int RoomTimestamp
	{
		get
		{
			return (!PhotonNetwork.inRoom ? 0 : PhotonNetwork.ServerTimestamp - this.roomStartTimestamp);
		}
	}

	public InRoomTime()
	{
	}

	public void OnJoinedRoom()
	{
		base.StartCoroutine("SetRoomStartTimestamp");
	}

	public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
	{
		base.StartCoroutine("SetRoomStartTimestamp");
	}

	public void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
	{
		if (propertiesThatChanged.ContainsKey("#rt"))
		{
			this.roomStartTimestamp = (int)propertiesThatChanged["#rt"];
		}
	}

	[DebuggerHidden]
	internal IEnumerator SetRoomStartTimestamp()
	{
		InRoomTime.u003cSetRoomStartTimestampu003ec__IteratorD4 variable = null;
		return variable;
	}
}