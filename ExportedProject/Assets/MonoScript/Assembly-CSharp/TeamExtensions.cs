using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class TeamExtensions
{
	public static PunTeams.Team GetTeam(this PhotonPlayer player)
	{
		object obj;
		if (!player.customProperties.TryGetValue("team", out obj))
		{
			return PunTeams.Team.none;
		}
		return (PunTeams.Team)((byte)obj);
	}

	public static void SetTeam(this PhotonPlayer player, PunTeams.Team team)
	{
		if (!PhotonNetwork.connectedAndReady)
		{
			Debug.LogWarning(string.Concat("JoinTeam was called in state: ", PhotonNetwork.connectionStateDetailed, ". Not connectedAndReady."));
		}
		if (PhotonNetwork.player.GetTeam() != team)
		{
			PhotonPlayer photonPlayer = PhotonNetwork.player;
			Hashtable hashtable = new Hashtable()
			{
				{ "team", (byte)team }
			};
			photonPlayer.SetCustomProperties(hashtable, null, false);
		}
	}
}