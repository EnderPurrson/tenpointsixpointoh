using System;
using System.Text;
using UnityEngine;

public class SupportLogging : MonoBehaviour
{
	public bool LogTrafficStats;

	public SupportLogging()
	{
	}

	private void LogBasics()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("SupportLogger Info: PUN {0}: ", "1.73");
		stringBuilder.AppendFormat("AppID: {0}*** GameVersion: {1} ", PhotonNetwork.networkingPeer.AppId.Substring(0, 8), PhotonNetwork.networkingPeer.AppVersion);
		stringBuilder.AppendFormat("Server: {0}. Region: {1} ", PhotonNetwork.ServerAddress, PhotonNetwork.networkingPeer.CloudRegion);
		stringBuilder.AppendFormat("HostType: {0} ", PhotonNetwork.PhotonServerSettings.HostType);
		Debug.Log(stringBuilder.ToString());
	}

	public void LogStats()
	{
		if (this.LogTrafficStats)
		{
			Debug.Log(string.Concat("SupportLogger ", PhotonNetwork.NetworkStatisticsToString()));
		}
	}

	protected void OnApplicationPause(bool pause)
	{
		Debug.Log(string.Concat(new object[] { "SupportLogger OnApplicationPause: ", pause, " connected: ", PhotonNetwork.connected }));
	}

	public void OnApplicationQuit()
	{
		base.CancelInvoke();
	}

	public void OnConnectedToPhoton()
	{
		Debug.Log("SupportLogger OnConnectedToPhoton().");
		this.LogBasics();
		if (this.LogTrafficStats)
		{
			PhotonNetwork.NetworkStatisticsEnabled = true;
		}
	}

	public void OnCreatedRoom()
	{
		Debug.Log(string.Concat(new object[] { "SupportLogger OnCreatedRoom(", PhotonNetwork.room, "). ", PhotonNetwork.lobby, " GameServer:", PhotonNetwork.ServerAddress }));
	}

	public void OnDisconnectedFromPhoton()
	{
		Debug.Log("SupportLogger OnDisconnectedFromPhoton().");
	}

	public void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.Log(string.Concat("SupportLogger OnFailedToConnectToPhoton(", cause, ")."));
		this.LogBasics();
	}

	public void OnJoinedLobby()
	{
		Debug.Log(string.Concat("SupportLogger OnJoinedLobby(", PhotonNetwork.lobby, ")."));
	}

	public void OnJoinedRoom()
	{
		Debug.Log(string.Concat(new object[] { "SupportLogger OnJoinedRoom(", PhotonNetwork.room, "). ", PhotonNetwork.lobby, " GameServer:", PhotonNetwork.ServerAddress }));
	}

	public void OnLeftRoom()
	{
		Debug.Log("SupportLogger OnLeftRoom().");
	}

	public void Start()
	{
		if (this.LogTrafficStats)
		{
			base.InvokeRepeating("LogStats", 10f, 10f);
		}
	}
}