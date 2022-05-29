using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class LANBroadcastService : MonoBehaviour
{
	public LANBroadcastService.ReceivedMessage serverMessage;

	private string strMessage = string.Empty;

	private LANBroadcastService.enuState currentState;

	private UdpClient objUDPClient;

	public List<LANBroadcastService.ReceivedMessage> lstReceivedMessages;

	private LANBroadcastService.delJoinServer delWhenServerFound;

	private LANBroadcastService.delStartServer delWhenServerMustStarted;

	private string strServerNotReady = "wanttobeaserver";

	private string strServerReady = "iamaserver";

	private float fTimeLastMessageSent;

	private float fIntervalMessageSending = 1f;

	private float fTimeMessagesLive = 5f;

	private float fTimeToSearch = 5f;

	private float fTimeSearchStarted;

	private string ipaddress;

	public string Message
	{
		get
		{
			return this.strMessage;
		}
	}

	public LANBroadcastService()
	{
	}

	private void BeginAsyncReceive()
	{
		if (this.objUDPClient == null)
		{
			return;
		}
		this.objUDPClient.BeginReceive(new AsyncCallback(this.EndAsyncReceive), null);
	}

	private void EndAsyncReceive(IAsyncResult objResult)
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
			string[] strArrays = str.Split(new char[] { 'ý' }, str.Length);
			if ((int)strArrays.Length == 8 && strArrays[7].Equals(GlobalGameController.MultiplayerProtocolVersion))
			{
				for (int i = 0; i < this.lstReceivedMessages.Count; i++)
				{
					LANBroadcastService.ReceivedMessage item = this.lstReceivedMessages[i];
					if (pEndPoint.Address.ToString().Equals(item.ipAddress))
					{
						this.lstReceivedMessages.RemoveAt(i);
					}
				}
				LANBroadcastService.ReceivedMessage receivedMessage = new LANBroadcastService.ReceivedMessage()
				{
					ipAddress = pEndPoint.Address.ToString(),
					name = strArrays[1],
					map = strArrays[2],
					connectedPlayers = int.Parse(strArrays[3]),
					playerLimit = int.Parse(strArrays[4]),
					comment = strArrays[5],
					regim = int.Parse(strArrays[6]),
					protocol = strArrays[7],
					fTime = -1f
				};
				this.lstReceivedMessages.Add(receivedMessage);
			}
		}
		if (this.currentState == LANBroadcastService.enuState.Searching)
		{
			this.BeginAsyncReceive();
		}
	}

	private void Start()
	{
		this.lstReceivedMessages = new List<LANBroadcastService.ReceivedMessage>();
		this.ipaddress = Network.player.ipAddress.ToString();
	}

	public void StartAnnounceBroadCasting()
	{
		this.StartBroadcastingSession();
		this.StartAnnouncing();
	}

	private void StartAnnouncing()
	{
		this.currentState = LANBroadcastService.enuState.Announcing;
		this.strMessage = "Announcing we are a server...";
	}

	private void StartBroadcastingSession()
	{
		if (this.currentState != LANBroadcastService.enuState.NotActive)
		{
			this.StopBroadCasting();
		}
		this.objUDPClient = new UdpClient(22043)
		{
			EnableBroadcast = true
		};
		this.fTimeLastMessageSent = Time.time;
	}

	public void StartSearchBroadCasting(LANBroadcastService.delJoinServer connectToServer)
	{
		this.delWhenServerFound = connectToServer;
		this.StartBroadcastingSession();
		this.StartSearching();
	}

	private void StartSearching()
	{
		if (this.lstReceivedMessages == null)
		{
			this.lstReceivedMessages = new List<LANBroadcastService.ReceivedMessage>();
		}
		this.lstReceivedMessages.Clear();
		this.BeginAsyncReceive();
		this.fTimeSearchStarted = Time.time;
		this.currentState = LANBroadcastService.enuState.Searching;
		this.strMessage = "Searching for other players...";
	}

	private void StopAnnouncing()
	{
		this.currentState = LANBroadcastService.enuState.NotActive;
		this.strMessage = "Announcements stopped.";
	}

	public void StopBroadCasting()
	{
		if (this.currentState == LANBroadcastService.enuState.Searching)
		{
			this.StopSearching();
		}
		else if (this.currentState == LANBroadcastService.enuState.Announcing)
		{
			this.StopAnnouncing();
		}
		if (this.objUDPClient != null)
		{
			UdpClient udpClient = this.objUDPClient;
			this.objUDPClient = null;
			udpClient.Close();
		}
	}

	private void StopSearching()
	{
		this.currentState = LANBroadcastService.enuState.NotActive;
		this.strMessage = "Search stopped.";
	}

	private void Update()
	{
		if (this.currentState == LANBroadcastService.enuState.Announcing && Time.time > this.fTimeLastMessageSent + this.fIntervalMessageSending)
		{
			string str = string.Concat(new object[] { this.strServerReady, "ý", this.serverMessage.name, "ý", this.serverMessage.map, "ý", this.serverMessage.connectedPlayers, "ý", this.serverMessage.playerLimit, "ý", this.serverMessage.comment, "ý", this.serverMessage.regim, "ý", GlobalGameController.MultiplayerProtocolVersion });
			byte[] bytes = Encoding.Unicode.GetBytes(str);
			if (this.objUDPClient == null)
			{
				Debug.Log("objUDPClient=NULL");
			}
			else
			{
				try
				{
					this.objUDPClient.Send(bytes, (int)bytes.Length, new IPEndPoint(IPAddress.Broadcast, 22043));
				}
				catch (Exception exception)
				{
					Debug.Log("soccet close");
				}
			}
			this.fTimeLastMessageSent = Time.time;
		}
		if (this.currentState == LANBroadcastService.enuState.Searching)
		{
			if (this.lstReceivedMessages == null)
			{
				return;
			}
			int num = 0;
			while (num < this.lstReceivedMessages.Count)
			{
				LANBroadcastService.ReceivedMessage item = this.lstReceivedMessages[num];
				if (item.fTime < 0f)
				{
					LANBroadcastService.ReceivedMessage receivedMessage = new LANBroadcastService.ReceivedMessage()
					{
						ipAddress = item.ipAddress,
						name = item.name,
						map = item.map,
						connectedPlayers = item.connectedPlayers,
						playerLimit = item.playerLimit,
						comment = item.comment,
						fTime = Time.time,
						regim = item.regim
					};
					this.lstReceivedMessages.RemoveAt(num);
					this.lstReceivedMessages.Add(receivedMessage);
				}
				if (Time.time <= item.fTime + this.fTimeMessagesLive)
				{
					num++;
				}
				else
				{
					this.lstReceivedMessages.Remove(item);
					break;
				}
			}
		}
		this.currentState != LANBroadcastService.enuState.Searching;
	}

	public delegate void delJoinServer(string strIP);

	public delegate void delStartServer();

	private enum enuState
	{
		NotActive,
		Searching,
		Announcing
	}

	public struct ReceivedMessage
	{
		public string ipAddress;

		public string name;

		public string map;

		public int connectedPlayers;

		public int playerLimit;

		public string comment;

		public float fTime;

		public int regim;

		public string protocol;
	}
}