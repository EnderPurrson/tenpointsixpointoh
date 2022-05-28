using System;
using System.Net;
using System.Runtime.CompilerServices;

namespace ExitGames.Client.Photon
{
	public abstract class IPhotonSocket
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass37_0
		{
			public byte[] inBufferCopy;

			public _003C_003Ec__DisplayClass37_1 CS_0024_003C_003E8__locals1;

			internal void _003CHandleReceivedDatagram_003Eb__0()
			{
				CS_0024_003C_003E8__locals1._003C_003E4__this.peerBase.ReceiveIncomingCommands(inBufferCopy, CS_0024_003C_003E8__locals1.length);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass37_1
		{
			public int length;

			public byte[] inBuffer;

			public IPhotonSocket _003C_003E4__this;

			internal void _003CHandleReceivedDatagram_003Eb__1()
			{
				_003C_003E4__this.peerBase.ReceiveIncomingCommands(inBuffer, length);
			}
		}

		protected internal PeerBase peerBase;

		public bool PollReceive;

		protected IPhotonPeerListener Listener
		{
			get
			{
				return peerBase.Listener;
			}
		}

		public ConnectionProtocol Protocol
		{
			[CompilerGenerated]
			get
			{
				return _003CProtocol_003Ek__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				_003CProtocol_003Ek__BackingField = value;
			}
		}

		public PhotonSocketState State
		{
			[CompilerGenerated]
			get
			{
				return _003CState_003Ek__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				_003CState_003Ek__BackingField = value;
			}
		}

		public string ServerAddress
		{
			[CompilerGenerated]
			get
			{
				return _003CServerAddress_003Ek__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				_003CServerAddress_003Ek__BackingField = value;
			}
		}

		public int ServerPort
		{
			[CompilerGenerated]
			get
			{
				return _003CServerPort_003Ek__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				_003CServerPort_003Ek__BackingField = value;
			}
		}

		public string UrlProtocol
		{
			[CompilerGenerated]
			get
			{
				return _003CUrlProtocol_003Ek__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				_003CUrlProtocol_003Ek__BackingField = value;
			}
		}

		public string UrlPath
		{
			[CompilerGenerated]
			get
			{
				return _003CUrlPath_003Ek__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				_003CUrlPath_003Ek__BackingField = value;
			}
		}

		public bool Connected
		{
			get
			{
				return State == PhotonSocketState.Connected;
			}
		}

		public int MTU
		{
			get
			{
				return peerBase.mtu;
			}
		}

		public IPhotonSocket(PeerBase peerBase)
		{
			if (peerBase == null)
			{
				throw new global::System.Exception("Can't init without peer");
			}
			this.peerBase = peerBase;
		}

		public virtual bool Connect()
		{
			if (State != 0)
			{
				if ((int)peerBase.debugOut >= 1)
				{
					peerBase.Listener.DebugReturn(DebugLevel.ERROR, string.Concat((object)"Connect() failed: connection in State: ", (object)State));
				}
				return false;
			}
			if (peerBase == null || Protocol != peerBase.usedProtocol)
			{
				return false;
			}
			string address;
			ushort port;
			string urlProtocol;
			string urlPath;
			if (!TryParseAddress(peerBase.ServerAddress, out address, out port, out urlProtocol, out urlPath))
			{
				if ((int)peerBase.debugOut >= 1)
				{
					peerBase.Listener.DebugReturn(DebugLevel.ERROR, "Failed parsing address: " + peerBase.ServerAddress);
				}
				return false;
			}
			ServerAddress = address;
			ServerPort = port;
			UrlProtocol = urlProtocol;
			UrlPath = urlPath;
			return true;
		}

		public abstract bool Disconnect();

		public abstract PhotonSocketError Send(byte[] data, int length);

		public abstract PhotonSocketError Receive(out byte[] data);

		public void HandleReceivedDatagram(byte[] inBuffer, int length, bool willBeReused)
		{
			_003C_003Ec__DisplayClass37_1 _003C_003Ec__DisplayClass37_ = new _003C_003Ec__DisplayClass37_1();
			_003C_003Ec__DisplayClass37_._003C_003E4__this = this;
			_003C_003Ec__DisplayClass37_.length = length;
			_003C_003Ec__DisplayClass37_.inBuffer = inBuffer;
			if (peerBase.NetworkSimulationSettings.IsSimulationEnabled)
			{
				if (willBeReused)
				{
					_003C_003Ec__DisplayClass37_0 _003C_003Ec__DisplayClass37_2 = new _003C_003Ec__DisplayClass37_0();
					_003C_003Ec__DisplayClass37_2.CS_0024_003C_003E8__locals1 = _003C_003Ec__DisplayClass37_;
					_003C_003Ec__DisplayClass37_2.inBufferCopy = new byte[_003C_003Ec__DisplayClass37_2.CS_0024_003C_003E8__locals1.length];
					Buffer.BlockCopy((global::System.Array)_003C_003Ec__DisplayClass37_2.CS_0024_003C_003E8__locals1.inBuffer, 0, (global::System.Array)_003C_003Ec__DisplayClass37_2.inBufferCopy, 0, _003C_003Ec__DisplayClass37_2.CS_0024_003C_003E8__locals1.length);
					peerBase.ReceiveNetworkSimulated(_003C_003Ec__DisplayClass37_2._003CHandleReceivedDatagram_003Eb__0);
				}
				else
				{
					peerBase.ReceiveNetworkSimulated(_003C_003Ec__DisplayClass37_._003CHandleReceivedDatagram_003Eb__1);
				}
			}
			else
			{
				peerBase.ReceiveIncomingCommands(_003C_003Ec__DisplayClass37_.inBuffer, _003C_003Ec__DisplayClass37_.length);
			}
		}

		public bool ReportDebugOfLevel(DebugLevel levelOfMessage)
		{
			return (int)peerBase.debugOut >= (int)levelOfMessage;
		}

		public void EnqueueDebugReturn(DebugLevel debugLevel, string message)
		{
			peerBase.EnqueueDebugReturn(debugLevel, message);
		}

		protected internal void HandleException(StatusCode statusCode)
		{
			State = PhotonSocketState.Disconnecting;
			peerBase.EnqueueStatusCallback(statusCode);
			peerBase.EnqueueActionForDispatch(_003CHandleException_003Eb__40_0);
		}

		protected internal bool TryParseAddress(string url, out string address, out ushort port, out string urlProtocol, out string urlPath)
		{
			address = string.Empty;
			port = 0;
			urlProtocol = string.Empty;
			urlPath = string.Empty;
			string text = url;
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			int num = text.IndexOf("://");
			if (num >= 0)
			{
				urlProtocol = text.Substring(0, num);
				text = text.Substring(num + 3);
			}
			num = text.IndexOf("/");
			if (num >= 0)
			{
				urlPath = text.Substring(num);
				text = text.Substring(0, num);
			}
			num = text.LastIndexOf(':');
			if (num < 0)
			{
				return false;
			}
			if (text.IndexOf(':') != num && (!text.Contains("[") || !text.Contains("]")))
			{
				return false;
			}
			address = text.Substring(0, num);
			string text2 = text.Substring(num + 1);
			return ushort.TryParse(text2, ref port);
		}

		protected internal static IPAddress GetIpAddress(string serverIp)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Invalid comparison between Unknown and I4
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Invalid comparison between Unknown and I4
			IPAddress result = null;
			if (IPAddress.TryParse(serverIp, ref result))
			{
				return result;
			}
			IPHostEntry hostEntry = Dns.GetHostEntry(serverIp);
			IPAddress[] addressList = hostEntry.get_AddressList();
			IPAddress[] array = addressList;
			foreach (IPAddress val in array)
			{
				if ((int)val.get_AddressFamily() == 23)
				{
					return val;
				}
			}
			IPAddress[] array2 = addressList;
			foreach (IPAddress val2 in array2)
			{
				if ((int)val2.get_AddressFamily() == 2)
				{
					return val2;
				}
			}
			return null;
		}

		[CompilerGenerated]
		private void _003CHandleException_003Eb__40_0()
		{
			peerBase.Disconnect();
		}
	}
}
