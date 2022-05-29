using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExitGames.Client.Photon.Chat
{
	internal class ChatPeer : PhotonPeer
	{
		public const string NameServerHost = "ns.exitgames.com";

		public const string NameServerHttp = "http://ns.exitgamescloud.com:80/photon/n";

		private readonly static Dictionary<ConnectionProtocol, int> ProtocolToNameServerPort;

		internal virtual bool IsProtocolSecure
		{
			get
			{
				return base.UsedProtocol == ConnectionProtocol.WebSocketSecure;
			}
		}

		public string NameServerAddress
		{
			get
			{
				return this.GetNameServerAddress();
			}
		}

		static ChatPeer()
		{
			Dictionary<ConnectionProtocol, int> connectionProtocols = new Dictionary<ConnectionProtocol, int>()
			{
				{ ConnectionProtocol.Udp, 5058 },
				{ ConnectionProtocol.Tcp, 4533 },
				{ ConnectionProtocol.WebSocket, 9093 },
				{ ConnectionProtocol.WebSocketSecure, 19093 }
			};
			ChatPeer.ProtocolToNameServerPort = connectionProtocols;
		}

		public ChatPeer(IPhotonPeerListener listener, ConnectionProtocol protocol) : base(listener, protocol)
		{
			if (protocol == ConnectionProtocol.WebSocket || protocol == ConnectionProtocol.WebSocketSecure)
			{
				Debug.Log("Using SocketWebTcp");
				base.SocketImplementation = Type.GetType("ExitGames.Client.Photon.SocketWebTcp, Assembly-CSharp");
			}
		}

		public bool AuthenticateOnNameServer(string appId, string appVersion, string region, AuthenticationValues authValues)
		{
			if (base.DebugOut >= DebugLevel.INFO)
			{
				base.Listener.DebugReturn(DebugLevel.INFO, "OpAuthenticate()");
			}
			Dictionary<byte, object> nums = new Dictionary<byte, object>();
			nums[220] = appVersion;
			nums[224] = appId;
			nums[210] = region;
			if (authValues != null)
			{
				if (!string.IsNullOrEmpty(authValues.UserId))
				{
					nums[225] = authValues.UserId;
				}
				if (authValues != null && authValues.AuthType != CustomAuthenticationType.None)
				{
					nums[217] = (byte)authValues.AuthType;
					if (string.IsNullOrEmpty(authValues.Token))
					{
						if (!string.IsNullOrEmpty(authValues.AuthGetParameters))
						{
							nums[216] = authValues.AuthGetParameters;
						}
						if (authValues.AuthPostData != null)
						{
							nums[214] = authValues.AuthPostData;
						}
					}
					else
					{
						nums[221] = authValues.Token;
					}
				}
			}
			return this.OpCustom(230, nums, true, 0, base.IsEncryptionAvailable);
		}

		public bool Connect()
		{
			base.Listener.DebugReturn(DebugLevel.INFO, string.Concat("Connecting to nameserver ", this.NameServerAddress));
			return this.Connect(this.NameServerAddress, "NameServer");
		}

		private string GetNameServerAddress()
		{
			ConnectionProtocol usedProtocol = base.UsedProtocol;
			int num = 0;
			ChatPeer.ProtocolToNameServerPort.TryGetValue(usedProtocol, out num);
			string empty = string.Empty;
			if (usedProtocol == ConnectionProtocol.WebSocket)
			{
				empty = "ws://";
			}
			else if (usedProtocol == ConnectionProtocol.WebSocketSecure)
			{
				empty = "wss://";
			}
			return string.Format("{0}{1}:{2}", empty, "ns.exitgames.com", num);
		}
	}
}