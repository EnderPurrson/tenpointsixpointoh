using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public static class TurnExtensions
{
	public readonly static string TurnPropKey;

	public readonly static string TurnStartPropKey;

	public readonly static string FinishedTurnPropKey;

	static TurnExtensions()
	{
		TurnExtensions.TurnPropKey = "Turn";
		TurnExtensions.TurnStartPropKey = "TStart";
		TurnExtensions.FinishedTurnPropKey = "FToA";
	}

	public static int GetFinishedTurn(this PhotonPlayer player)
	{
		Room room = PhotonNetwork.room;
		if (room == null || room.customProperties == null || !room.customProperties.ContainsKey(TurnExtensions.TurnPropKey))
		{
			return 0;
		}
		string str = string.Concat(TurnExtensions.FinishedTurnPropKey, player.ID);
		return (int)room.customProperties[str];
	}

	public static int GetTurn(this RoomInfo room)
	{
		if (room == null || room.customProperties == null || !room.customProperties.ContainsKey(TurnExtensions.TurnPropKey))
		{
			return 0;
		}
		return (int)room.customProperties[TurnExtensions.TurnPropKey];
	}

	public static int GetTurnStart(this RoomInfo room)
	{
		if (room == null || room.customProperties == null || !room.customProperties.ContainsKey(TurnExtensions.TurnStartPropKey))
		{
			return 0;
		}
		return (int)room.customProperties[TurnExtensions.TurnStartPropKey];
	}

	public static void SetFinishedTurn(this PhotonPlayer player, int turn)
	{
		Room room = PhotonNetwork.room;
		if (room == null || room.customProperties == null)
		{
			return;
		}
		string str = string.Concat(TurnExtensions.FinishedTurnPropKey, player.ID);
		Hashtable hashtable = new Hashtable();
		hashtable[str] = turn;
		room.SetCustomProperties(hashtable, null, false);
	}

	public static void SetTurn(this Room room, int turn, bool setStartTime = false)
	{
		if (room == null || room.customProperties == null)
		{
			return;
		}
		Hashtable hashtable = new Hashtable();
		hashtable[TurnExtensions.TurnPropKey] = turn;
		if (setStartTime)
		{
			hashtable[TurnExtensions.TurnStartPropKey] = PhotonNetwork.ServerTimestamp;
		}
		room.SetCustomProperties(hashtable, null, false);
	}
}