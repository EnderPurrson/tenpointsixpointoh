using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Threading;

namespace ExitGames.Client.Photon
{
	internal class SocketTcp : IPhotonSocket, global::System.IDisposable
	{
		private Socket sock;

		private readonly object syncer = new object();

		public SocketTcp(PeerBase npeer)
			: base(npeer)
		{
			if (ReportDebugOfLevel(DebugLevel.ALL))
			{
				base.Listener.DebugReturn(DebugLevel.ALL, "SocketTcp: TCP, DotNet, Unity.");
			}
			base.Protocol = ConnectionProtocol.Tcp;
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
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
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

		public override bool Disconnect()
		{
			if (ReportDebugOfLevel(DebugLevel.INFO))
			{
				EnqueueDebugReturn(DebugLevel.INFO, "SocketTcp.Disconnect()");
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
			if (!sock.get_Connected())
			{
				return PhotonSocketError.Skipped;
			}
			try
			{
				sock.Send(data);
			}
			catch (global::System.Exception ex)
			{
				if (ReportDebugOfLevel(DebugLevel.ERROR))
				{
					EnqueueDebugReturn(DebugLevel.ERROR, "Cannot send to: " + base.ServerAddress + ". " + ex.get_Message());
				}
				HandleException(StatusCode.Exception);
				return PhotonSocketError.Exception;
			}
			return PhotonSocketError.Success;
		}

		public override PhotonSocketError Receive(out byte[] data)
		{
			data = null;
			return PhotonSocketError.NoData;
		}

		public void DnsAndConnect()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Invalid comparison between Unknown and I4
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Invalid comparison between Unknown and I4
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Expected O, but got Unknown
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Invalid comparison between Unknown and I4
			//IL_0116: Expected O, but got Unknown
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Expected O, but got Unknown
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Expected O, but got Unknown
			try
			{
				IPAddress ipAddress = IPhotonSocket.GetIpAddress(base.ServerAddress);
				if (ipAddress == null)
				{
					throw new ArgumentException("Invalid IPAddress. Address: " + base.ServerAddress);
				}
				if ((int)ipAddress.get_AddressFamily() != 2 && (int)ipAddress.get_AddressFamily() != 23)
				{
					throw new ArgumentException(string.Concat(new object[4]
					{
						"AddressFamily '",
						ipAddress.get_AddressFamily(),
						"' not supported. Address: ",
						base.ServerAddress
					}));
				}
				sock = new Socket(ipAddress.get_AddressFamily(), (SocketType)1, (ProtocolType)6);
				sock.set_NoDelay(true);
				sock.set_ReceiveTimeout(peerBase.DisconnectTimeout);
				sock.set_SendTimeout(peerBase.DisconnectTimeout);
				sock.Connect(ipAddress, base.ServerPort);
				base.State = PhotonSocketState.Connected;
				peerBase.SetInitIPV6Bit((int)ipAddress.get_AddressFamily() == 23);
				peerBase.OnConnect();
			}
			catch (SecurityException val)
			{
				SecurityException val2 = val;
				if (ReportDebugOfLevel(DebugLevel.ERROR))
				{
					base.Listener.DebugReturn(DebugLevel.ERROR, "Connect() to '" + base.ServerAddress + "' failed: " + ((object)val2).ToString());
				}
				HandleException(StatusCode.SecurityExceptionOnConnect);
				return;
			}
			catch (global::System.Exception ex)
			{
				if (ReportDebugOfLevel(DebugLevel.ERROR))
				{
					base.Listener.DebugReturn(DebugLevel.ERROR, "Connect() to '" + base.ServerAddress + "' failed: " + ((object)ex).ToString());
				}
				HandleException(StatusCode.ExceptionOnConnect);
				return;
			}
			Thread val3 = new Thread(new ThreadStart(ReceiveLoop));
			val3.set_Name("photon receive thread");
			val3.set_IsBackground(true);
			val3.Start();
		}

		public void ReceiveLoop()
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Expected O, but got Unknown
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Invalid comparison between Unknown and I4
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Invalid comparison between Unknown and I4
			StreamBuffer streamBuffer = new StreamBuffer(base.MTU);
			while (base.State == PhotonSocketState.Connected)
			{
				((Stream)streamBuffer).set_Position(0L);
				((Stream)streamBuffer).SetLength(0L);
				try
				{
					int num = 0;
					int num2 = 0;
					byte[] array = new byte[9];
					while (num < 9)
					{
						num2 = sock.Receive(array, num, 9 - num, (SocketFlags)0);
						num += num2;
						if (num2 == 0)
						{
							throw new SocketException(10054);
						}
					}
					if (array[0] == 240)
					{
						HandleReceivedDatagram(array, array.Length, false);
						continue;
					}
					int num3 = (array[1] << 24) | (array[2] << 16) | (array[3] << 8) | array[4];
					if (peerBase.TrafficStatsEnabled)
					{
						if (array[5] == 0)
						{
							peerBase.TrafficStatsIncoming.CountReliableOpCommand(num3);
						}
						else
						{
							peerBase.TrafficStatsIncoming.CountUnreliableOpCommand(num3);
						}
					}
					if (ReportDebugOfLevel(DebugLevel.ALL))
					{
						EnqueueDebugReturn(DebugLevel.ALL, string.Concat((object)"message length: ", (object)num3));
					}
					((Stream)streamBuffer).Write(array, 7, num - 7);
					num = 0;
					num3 -= 9;
					array = new byte[num3];
					while (num < num3)
					{
						num2 = sock.Receive(array, num, num3 - num, (SocketFlags)0);
						num += num2;
						if (num2 == 0)
						{
							throw new SocketException(10054);
						}
					}
					((Stream)streamBuffer).Write(array, 0, num);
					if (((Stream)streamBuffer).get_Length() > 0)
					{
						HandleReceivedDatagram(streamBuffer.ToArray(), (int)((Stream)streamBuffer).get_Length(), false);
					}
					if (ReportDebugOfLevel(DebugLevel.ALL))
					{
						EnqueueDebugReturn(DebugLevel.ALL, string.Concat((object)"TCP < ", (object)((Stream)streamBuffer).get_Length(), (object)((((Stream)streamBuffer).get_Length() == num3 + 2) ? " OK" : " BAD")));
					}
				}
				catch (SocketException val)
				{
					SocketException val2 = val;
					if (base.State != PhotonSocketState.Disconnecting && base.State != 0)
					{
						if (ReportDebugOfLevel(DebugLevel.ERROR))
						{
							EnqueueDebugReturn(DebugLevel.ERROR, string.Concat((object)"Receiving failed. SocketException: ", (object)val2.get_SocketErrorCode()));
						}
						if ((int)val2.get_SocketErrorCode() == 10054 || (int)val2.get_SocketErrorCode() == 10053)
						{
							HandleException(StatusCode.DisconnectByServer);
						}
						else
						{
							HandleException(StatusCode.ExceptionOnReceive);
						}
					}
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
