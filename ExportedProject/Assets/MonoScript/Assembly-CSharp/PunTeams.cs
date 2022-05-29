using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunTeams : MonoBehaviour
{
	public const string TeamPlayerProp = "team";

	public static Dictionary<PunTeams.Team, List<PhotonPlayer>> PlayersPerTeam;

	public PunTeams()
	{
	}

	public void OnJoinedRoom()
	{
		this.UpdateTeams();
	}

	public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
	{
		this.UpdateTeams();
	}

	public void Start()
	{
		PunTeams.PlayersPerTeam = new Dictionary<PunTeams.Team, List<PhotonPlayer>>();
		IEnumerator enumerator = Enum.GetValues(typeof(PunTeams.Team)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				PunTeams.PlayersPerTeam[(PunTeams.Team)((byte)current)] = new List<PhotonPlayer>();
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
	}

	public void UpdateTeams()
	{
		IEnumerator enumerator = Enum.GetValues(typeof(PunTeams.Team)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				PunTeams.PlayersPerTeam[(PunTeams.Team)((byte)current)].Clear();
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
		for (int i = 0; i < (int)PhotonNetwork.playerList.Length; i++)
		{
			PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
			PunTeams.Team team = photonPlayer.GetTeam();
			PunTeams.PlayersPerTeam[team].Add(photonPlayer);
		}
	}

	public enum Team : byte
	{
		none,
		red,
		blue
	}
}