using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class FindFriendsFromLocalLAN : MonoBehaviour
{
	public static bool isFindLocalFriends;

	private string ipaddress;

	public static List<string> lanPlayerInfo;

	public static Action lanPlayerInfoUpdate;

	private UdpClient objUDPClient;

	private float periodSendMyInfo = 30f;

	private float timeSendMyInfo;

	private bool isGetMessage;

	private bool isActiveFriends;

	private List<string> idsForInfo = new List<string>();

	static FindFriendsFromLocalLAN()
	{
		FindFriendsFromLocalLAN.isFindLocalFriends = false;
		FindFriendsFromLocalLAN.lanPlayerInfo = new List<string>();
		FindFriendsFromLocalLAN.lanPlayerInfoUpdate = null;
	}

	public FindFriendsFromLocalLAN()
	{
	}

	private void BeginAsyncReceive()
	{
		if (this.objUDPClient == null)
		{
			return;
		}
		this.objUDPClient.BeginReceive(new AsyncCallback(this.GetAsyncReceive), null);
	}

	private void GetAsyncReceive(IAsyncResult objResult)
	{
		if (this.objUDPClient == null)
		{
			return;
		}
		IPEndPoint pEndPoint = new IPEndPoint(IPAddress.Any, 0);
		byte[] numArray = this.objUDPClient.EndReceive(objResult, ref pEndPoint);
		if ((int)numArray.Length > 0 && !pEndPoint.Address.ToString().Equals(this.ipaddress))
		{
			string str = Encoding.Unicode.GetString(numArray);
			List<object> objs = Json.Deserialize(str) as List<object>;
			string empty = string.Empty;
			if (objs != null && objs.Count == 1)
			{
				empty = Convert.ToString(objs[0]);
			}
			if (!string.IsNullOrEmpty(empty) && !FindFriendsFromLocalLAN.lanPlayerInfo.Contains(empty) && !FriendsController.sharedController.friends.Contains(empty))
			{
				FindFriendsFromLocalLAN.lanPlayerInfo.Add(empty);
				if (this.isActiveFriends)
				{
					this.idsForInfo.Add(empty);
				}
			}
			this.isGetMessage = true;
		}
		this.BeginAsyncReceive();
	}

	[DebuggerHidden]
	private IEnumerator OnApplicationPause(bool pause)
	{
		FindFriendsFromLocalLAN.u003cOnApplicationPauseu003ec__Iterator28 variable = null;
		return variable;
	}

	private void SendMyInfo()
	{
		if (string.IsNullOrEmpty(FriendsController.sharedController.id))
		{
			return;
		}
		this.timeSendMyInfo = Time.time;
		List<string> strs = new List<string>()
		{
			FriendsController.sharedController.id
		};
		string str = Json.Serialize(strs);
		byte[] bytes = Encoding.Unicode.GetBytes(str);
		if (this.objUDPClient == null)
		{
			UnityEngine.Debug.Log("objUDPClient=NULL");
		}
		else
		{
			try
			{
				this.objUDPClient.Send(bytes, (int)bytes.Length, new IPEndPoint(IPAddress.Broadcast, 22044));
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.Log(string.Concat("soccet close ", exception));
			}
		}
	}

	private void Start()
	{
		this.ipaddress = Network.player.ipAddress.ToString();
		this.StartBroadcastingSession();
	}

	private void StartBroadcastingSession()
	{
		this.objUDPClient = new UdpClient(22044)
		{
			EnableBroadcast = true
		};
		this.BeginAsyncReceive();
	}

	public void StopBroadCasting()
	{
		if (this.objUDPClient != null)
		{
			UdpClient udpClient = this.objUDPClient;
			this.objUDPClient = null;
			udpClient.Close();
		}
	}

	private void Update()
	{
		this.isActiveFriends = (FriendsWindowGUI.Instance == null ? false : FriendsWindowGUI.Instance.InterfaceEnabled);
		if (this.idsForInfo.Count > 0)
		{
			FriendsController.sharedController.GetInfoAboutPlayers(this.idsForInfo);
			this.idsForInfo.Clear();
		}
		if ((this.isActiveFriends || this.isGetMessage) && Time.time - this.timeSendMyInfo > this.periodSendMyInfo)
		{
			this.isGetMessage = false;
			this.SendMyInfo();
		}
	}
}