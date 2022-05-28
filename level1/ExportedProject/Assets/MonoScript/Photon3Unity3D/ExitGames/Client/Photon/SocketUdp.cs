using System;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Threading;

namespace ExitGames.Client.Photon
{
	internal class SocketUdp : IPhotonSocket, global::System.IDisposable
	{
		private Socket sock;

		private readonly object syncer = new object();

		public SocketUdp(PeerBase npeer)
			: base(npeer)
		{
			if (ReportDebugOfLevel(DebugLevel.ALL))
			{
				base.Listener.DebugReturn(DebugLevel.ALL, "CSharpSocket: UDP, Unity3d.");
			}
			base.Protocol = ConnectionProtocol.Udp;
			PollReceive = false;
		}

		public void Dispose()
		{
			base.State = PhotonSocketState.Disconnecting;
			if (sock != null)
			{
				try
				{
					if (sock.get_Connected())
					{
						sock.Close();
					}
				}
				catch (global::System.Exception ex)
				{
					EnqueueDebugReturn(DebugLevel.INFO, string.Concat((object)"Exception in Dispose(): ", (object)ex));
				}
			}
			sock = null;
			base.State = PhotonSocketState.Disconnected;
		}

		public override bool Connect()
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			lock (syncer)
			{
				if (!base.Connect())
				{
					return false;
				}
				base.State = PhotonSocketState.Connecting;
				Thread val = new Thread(new ThreadStart(DnsAndConnect));
				val.set_Name("photon dns thread");
				val.set_IsBackground(true);
				val.Start();
				return true;
			}
		}

		public override bool Disconnect()
		{
			if (ReportDebugOfLevel(DebugLevel.INFO))
			{
				EnqueueDebugReturn(DebugLevel.INFO, "CSharpSocket.Disconnect()");
			}
			base.State = PhotonSocketState.Disconnecting;
			lock (syncer)
			{
				if (sock != null)
				{
					try
					{
						sock.Close();
					}
					catch (global::System.Exception ex)
					{
						EnqueueDebugReturn(DebugLevel.INFO, string.Concat((object)"Exception in Disconnect(): ", (object)ex));
					}
					sock = null;
				}
			}
			base.State = PhotonSocketState.Disconnected;
			return true;
		}

		public override PhotonSocketError Send(byte[] data, int length)
		{
			lock (syncer)
			{
				if (sock == null || !sock.get_Connected())
				{
					return PhotonSocketError.Skipped;
				}
				try
				{
					sock.Send(data, 0, length, (SocketFlags)0);
				}
				catch (global::System.Exception ex)
				{
					if (ReportDebugOfLevel(DebugLevel.ERROR))
					{
						EnqueueDebugReturn(DebugLevel.ERROR, "Cannot send to: " + base.ServerAddress + ". " + ex.get_Message());
					}
					return PhotonSocketError.Exception;
				}
			}
			return PhotonSocketError.Success;
		}

		public override PhotonSocketError Receive(out byte[] data)
		{
			data = null;
			return PhotonSocketError.NoData;
		}

		internal void DnsAndConnect()
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Invalid comparison between Unknown and I4
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Invalid comparison between Unknown and I4
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Expected O, but got Unknown
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Invalid comparison between Unknown and I4
			//IL_00fa: Expected O, but got Unknown
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Expected O, but got Unknown
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Expected O, but got Unknown
			IPAddress val = null;
			AddressFamily addressFamily;
			try
			{
				lock (syncer)
				{
					val = IPhotonSocket.GetIpAddress(base.ServerAddress);
					if (val == null)
					{
						throw new ArgumentException("Invalid IPAddress. Address: " + base.ServerAddress);
					}
					if ((int)val.get_AddressFamily() != 2 && (int)val.get_AddressFamily() != 23)
					{
						throw new ArgumentException(string.Concat(new object[4]
						{
							"AddressFamily '",
							val.get_AddressFamily(),
							"' not supported. Address: ",
							base.ServerAddress
						}));
					}
					sock = new Socket(val.get_AddressFamily(), (SocketType)2, (ProtocolType)17);
					sock.Connect(val, base.ServerPort);
					base.State = PhotonSocketState.Connected;
					peerBase.SetInitIPV6Bit((int)val.get_AddressFamily() == 23);
					peerBase.OnConnect();
				}
			}
			catch (SecurityException val2)
			{
				SecurityException val3 = val2;
				if (ReportDebugOfLevel(DebugLevel.ERROR))
				{
					IPhotonPeerListener listener = base.Listener;
					string[] obj = new string[6] { "Connect() to '", base.ServerAddress, "' (", null, null, null };
					object obj2;
					if (val != null)
					{
						addressFamily = val.get_AddressFamily();
						obj2 = ((object)(AddressFamily)(ref addressFamily)).ToString();
					}
					else
					{
						obj2 = "";
					}
					obj[3] = (string)obj2;
					obj[4] = ") failed: ";
					obj[5] = ((object)val3).ToString();
					listener.DebugReturn(DebugLevel.ERROR, string.Concat(obj));
				}
				HandleException(StatusCode.SecurityExceptionOnConnect);
				return;
			}
			catch (global::System.Exception ex)
			{
				if (ReportDebugOfLevel(DebugLevel.ERROR))
				{
					IPhotonPeerListener listener2 = base.Listener;
					string[] obj3 = new string[6] { "Connect() to '", base.ServerAddress, "' (", null, null, null };
					object obj4;
					if (val != null)
					{
						addressFamily = val.get_AddressFamily();
						obj4 = ((object)(AddressFamily)(ref addressFamily)).ToString();
					}
					else
					{
						obj4 = "";
					}
					obj3[3] = (string)obj4;
					obj3[4] = ") failed: ";
					obj3[5] = ((object)ex).ToString();
					listener2.DebugReturn(DebugLevel.ERROR, string.Concat(obj3));
				}
				HandleException(StatusCode.ExceptionOnConnect);
				return;
			}
			Thread val4 = new Thread(new ThreadStart(ReceiveLoop));
			val4.set_Name("photon receive thread");
			val4.set_IsBackground(true);
			val4.Start();
		}

		public void ReceiveLoop()
		{
			byte[] array = new byte[base.MTU];
			while (base.State == PhotonSocketState.Connected)
			{
				try
				{
					int length = sock.Receive(array);
					HandleReceivedDatagram(array, length, true);
				}
				catch (global::System.Exception ex)
				{
					if (base.State != PhotonSocketState.Disconnecting && base.State != 0)
					{
						if (ReportDebugOfLevel(DebugLevel.ERROR))
						{
							EnqueueDebugReturn(DebugLevel.ERROR, string.Concat(new object[6] { "Receive issue. State: ", base.State, ". Server: '", base.ServerAddress, "' Exception: ", ex }));
						}
						HandleException(StatusCode.ExceptionOnReceive);
					}
				}
			}
			Disconnect();
		}
	}
}
