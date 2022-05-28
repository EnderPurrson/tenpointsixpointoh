using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace ExitGames.Client.Photon
{
	internal class TPeer : PeerBase
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass31_0
		{
			public byte[] data;

			public TPeer _003C_003E4__this;

			internal void _003CSendData_003Eb__0()
			{
				_003C_003E4__this.rt.Send(data, data.Length);
			}
		}

		internal const int TCP_HEADER_BYTES = 7;

		internal const int MSG_HEADER_BYTES = 2;

		public const int ALL_HEADER_BYTES = 9;

		private Queue<byte[]> incomingList = new Queue<byte[]>(32);

		internal List<byte[]> outgoingStream;

		private int lastPingResult;

		private byte[] pingRequest = new byte[5] { 240, 0, 0, 0, 0 };

		internal static readonly byte[] tcpFramedMessageHead;

		internal static readonly byte[] tcpMsgHead;

		internal byte[] messageHeader;

		protected internal bool DoFraming = true;

		internal override int QueuedIncomingCommandsCount
		{
			get
			{
				return incomingList.get_Count();
			}
		}

		internal override int QueuedOutgoingCommandsCount
		{
			get
			{
				return outgoingCommandsInStream;
			}
		}

		internal TPeer()
		{
			PeerBase.peerCount++;
			InitOnce();
			TrafficPackageHeaderSize = 0;
		}

		internal TPeer(IPhotonPeerListener listener)
			: this()
		{
			base.Listener = listener;
		}

		internal override void InitPeerBase()
		{
			base.InitPeerBase();
			incomingList = new Queue<byte[]>(32);
		}

		internal override bool Connect(string serverAddress, string appID)
		{
			if (peerConnectionState != 0)
			{
				base.Listener.DebugReturn(DebugLevel.WARNING, "Connect() can't be called if peer is not Disconnected. Not connecting.");
				return false;
			}
			if ((int)debugOut >= 5)
			{
				base.Listener.DebugReturn(DebugLevel.ALL, "Connect()");
			}
			base.ServerAddress = serverAddress;
			InitPeerBase();
			outgoingStream = new List<byte[]>();
			if (appID == null)
			{
				appID = "LoadBalancing";
			}
			for (int i = 0; i < 32; i++)
			{
				INIT_BYTES[i + 9] = (byte)((i < appID.get_Length()) ? ((byte)appID.get_Chars(i)) : 0);
			}
			if (SocketImplementation != null)
			{
				rt = (IPhotonSocket)Activator.CreateInstance(SocketImplementation, new object[1] { this });
			}
			else
			{
				rt = new SocketTcp(this);
			}
			if (rt == null)
			{
				base.Listener.DebugReturn(DebugLevel.ERROR, string.Concat((object)"Connect() failed, because SocketImplementation or socket was null. Set PhotonPeer.SocketImplementation before Connect(). SocketImplementation: ", (object)SocketImplementation));
				return false;
			}
			messageHeader = (DoFraming ? tcpFramedMessageHead : tcpMsgHead);
			if (rt.Connect())
			{
				peerConnectionState = ConnectionStateValue.Connecting;
				return true;
			}
			peerConnectionState = ConnectionStateValue.Disconnected;
			return false;
		}

		public override void OnConnect()
		{
			EnqueueInit();
			SendOutgoingCommands();
		}

		internal override void Disconnect()
		{
			if (peerConnectionState != 0 && peerConnectionState != ConnectionStateValue.Disconnecting)
			{
				if ((int)debugOut >= 5)
				{
					base.Listener.DebugReturn(DebugLevel.ALL, "TPeer.Disconnect()");
				}
				StopConnection();
			}
		}

		internal override void StopConnection()
		{
			peerConnectionState = ConnectionStateValue.Disconnecting;
			if (rt != null)
			{
				rt.Disconnect();
			}
			lock (incomingList)
			{
				incomingList.Clear();
			}
			peerConnectionState = ConnectionStateValue.Disconnected;
			base.Listener.OnStatusChanged(StatusCode.Disconnect);
		}

		internal override void FetchServerTimestamp()
		{
			if (peerConnectionState != ConnectionStateValue.Connected)
			{
				if ((int)debugOut >= 3)
				{
					base.Listener.DebugReturn(DebugLevel.INFO, string.Concat((object)"FetchServerTimestamp() was skipped, as the client is not connected. Current ConnectionState: ", (object)peerConnectionState));
				}
				base.Listener.OnStatusChanged(StatusCode.SendError);
			}
			else
			{
				SendPing();
				serverTimeOffsetIsAvailable = false;
			}
		}

		private void EnqueueInit()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			if (DoFraming)
			{
				StreamBuffer streamBuffer = new StreamBuffer();
				BinaryWriter val = new BinaryWriter((Stream)(object)streamBuffer);
				byte[] array = new byte[7] { 251, 0, 0, 0, 0, 0, 1 };
				int targetOffset = 1;
				Protocol.Serialize(INIT_BYTES.Length + array.Length, array, ref targetOffset);
				val.Write(array);
				val.Write(INIT_BYTES);
				byte[] array2 = streamBuffer.ToArray();
				if (base.TrafficStatsEnabled)
				{
					TrafficStatsOutgoing.TotalPacketCount++;
					TrafficStatsOutgoing.TotalCommandsInPackets++;
					TrafficStatsOutgoing.CountControlCommand(array2.Length);
				}
				EnqueueMessageAsPayload(true, array2, 0);
			}
		}

		internal override bool DispatchIncomingCommands()
		{
			while (true)
			{
				MyAction myAction;
				lock (ActionQueue)
				{
					if (ActionQueue.get_Count() <= 0)
					{
						break;
					}
					myAction = ActionQueue.Dequeue();
					goto IL_0042;
				}
				IL_0042:
				myAction();
			}
			byte[] array;
			lock (incomingList)
			{
				if (incomingList.get_Count() <= 0)
				{
					return false;
				}
				array = incomingList.Dequeue();
			}
			ByteCountCurrentDispatch = array.Length + 3;
			return DeserializeMessageAndCallback(array);
		}

		internal override bool SendOutgoingCommands()
		{
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			if (peerConnectionState == ConnectionStateValue.Disconnected)
			{
				return false;
			}
			if (!rt.Connected)
			{
				return false;
			}
			timeInt = SupportClass.GetTickCount() - timeBase;
			timeLastSendOutgoing = timeInt;
			if (peerConnectionState == ConnectionStateValue.Connected && SupportClass.GetTickCount() - lastPingResult > timePingInterval)
			{
				SendPing();
			}
			lock (outgoingStream)
			{
				Enumerator<byte[]> enumerator = outgoingStream.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						byte[] current = enumerator.get_Current();
						SendData(current);
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator).Dispose();
				}
				outgoingStream.Clear();
				outgoingCommandsInStream = 0;
			}
			return false;
		}

		internal override bool SendAcksOnly()
		{
			if (rt == null || !rt.Connected)
			{
				return false;
			}
			timeInt = SupportClass.GetTickCount() - timeBase;
			timeLastSendOutgoing = timeInt;
			if (peerConnectionState == ConnectionStateValue.Connected && SupportClass.GetTickCount() - lastPingResult > timePingInterval)
			{
				SendPing();
			}
			return false;
		}

		internal override bool EnqueueOperation(Dictionary<byte, object> parameters, byte opCode, bool sendReliable, byte channelId, bool encrypt, EgMessageType messageType)
		{
			if (peerConnectionState != ConnectionStateValue.Connected)
			{
				if ((int)debugOut >= 1)
				{
					base.Listener.DebugReturn(DebugLevel.ERROR, string.Concat(new object[4] { "Cannot send op: ", opCode, "! Not connected. PeerState: ", peerConnectionState }));
				}
				base.Listener.OnStatusChanged(StatusCode.SendError);
				return false;
			}
			if (channelId >= ChannelCount)
			{
				if ((int)debugOut >= 1)
				{
					base.Listener.DebugReturn(DebugLevel.ERROR, string.Concat(new object[5] { "Cannot send op: Selected channel (", channelId, ")>= channelCount (", ChannelCount, ")." }));
				}
				base.Listener.OnStatusChanged(StatusCode.SendError);
				return false;
			}
			byte[] opMessage = SerializeOperationToMessage(opCode, parameters, messageType, encrypt);
			return EnqueueMessageAsPayload(sendReliable, opMessage, channelId);
		}

		internal override byte[] SerializeOperationToMessage(byte opc, Dictionary<byte, object> parameters, EgMessageType messageType, bool encrypt)
		{
			byte[] array;
			lock (SerializeMemStream)
			{
				((Stream)SerializeMemStream).set_Position(0L);
				((Stream)SerializeMemStream).SetLength(0L);
				if (!encrypt)
				{
					((Stream)SerializeMemStream).Write(messageHeader, 0, messageHeader.Length);
				}
				protocol.SerializeOperationRequest(SerializeMemStream, opc, parameters, false);
				if (encrypt)
				{
					byte[] data = SerializeMemStream.ToArray();
					data = CryptoProvider.Encrypt(data);
					((Stream)SerializeMemStream).set_Position(0L);
					((Stream)SerializeMemStream).SetLength(0L);
					((Stream)SerializeMemStream).Write(messageHeader, 0, messageHeader.Length);
					((Stream)SerializeMemStream).Write(data, 0, data.Length);
				}
				array = SerializeMemStream.ToArray();
			}
			if (messageType != EgMessageType.Operation)
			{
				array[messageHeader.Length - 1] = (byte)messageType;
			}
			if (encrypt)
			{
				array[messageHeader.Length - 1] = (byte)(array[messageHeader.Length - 1] | 0x80u);
			}
			if (DoFraming)
			{
				int targetOffset = 1;
				Protocol.Serialize(array.Length, array, ref targetOffset);
			}
			return array;
		}

		internal bool EnqueueMessageAsPayload(bool sendReliable, byte[] opMessage, byte channelId)
		{
			if (opMessage == null)
			{
				return false;
			}
			if (DoFraming)
			{
				opMessage[5] = channelId;
				opMessage[6] = (byte)(sendReliable ? 1 : 0);
			}
			lock (outgoingStream)
			{
				outgoingStream.Add(opMessage);
				outgoingCommandsInStream++;
				if (outgoingCommandsInStream % warningSize == 0)
				{
					base.Listener.OnStatusChanged(StatusCode.QueueOutgoingReliableWarning);
				}
			}
			int num = (ByteCountLastOperation = opMessage.Length);
			if (base.TrafficStatsEnabled)
			{
				if (sendReliable)
				{
					TrafficStatsOutgoing.CountReliableOpCommand(num);
				}
				else
				{
					TrafficStatsOutgoing.CountUnreliableOpCommand(num);
				}
				TrafficStatsGameLevel.CountOperation(num);
			}
			return true;
		}

		internal void SendPing()
		{
			lastPingResult = SupportClass.GetTickCount();
			if (!DoFraming)
			{
				int tickCount = SupportClass.GetTickCount();
				Dictionary<byte, object> obj = new Dictionary<byte, object>();
				obj.Add((byte)1, (object)tickCount);
				EnqueueOperation(obj, PhotonCodes.Ping, true, 0, false, EgMessageType.InternalOperationRequest);
				return;
			}
			int targetOffset = 1;
			Protocol.Serialize(SupportClass.GetTickCount(), pingRequest, ref targetOffset);
			if (base.TrafficStatsEnabled)
			{
				TrafficStatsOutgoing.CountControlCommand(pingRequest.Length);
			}
			SendData(pingRequest);
		}

		internal void SendData(byte[] data)
		{
			_003C_003Ec__DisplayClass31_0 _003C_003Ec__DisplayClass31_ = new _003C_003Ec__DisplayClass31_0();
			_003C_003Ec__DisplayClass31_._003C_003E4__this = this;
			_003C_003Ec__DisplayClass31_.data = data;
			try
			{
				bytesOut += _003C_003Ec__DisplayClass31_.data.Length;
				if (base.TrafficStatsEnabled)
				{
					TrafficStatsOutgoing.TotalPacketCount++;
					TrafficStatsOutgoing.TotalCommandsInPackets += outgoingCommandsInStream;
				}
				if (base.NetworkSimulationSettings.IsSimulationEnabled)
				{
					SendNetworkSimulated(_003C_003Ec__DisplayClass31_._003CSendData_003Eb__0);
				}
				else
				{
					rt.Send(_003C_003Ec__DisplayClass31_.data, _003C_003Ec__DisplayClass31_.data.Length);
				}
			}
			catch (global::System.Exception ex)
			{
				if ((int)debugOut >= 1)
				{
					base.Listener.DebugReturn(DebugLevel.ERROR, ((object)ex).ToString());
				}
				SupportClass.WriteStackTrace(ex);
			}
		}

		internal override void ReceiveIncomingCommands(byte[] inbuff, int dataLength)
		{
			if (inbuff == null)
			{
				if ((int)debugOut >= 1)
				{
					EnqueueDebugReturn(DebugLevel.ERROR, "checkAndQueueIncomingCommands() inBuff: null");
				}
				return;
			}
			timestampOfLastReceive = SupportClass.GetTickCount();
			timeInt = SupportClass.GetTickCount() - timeBase;
			timeLastSendOutgoing = timeInt;
			bytesIn += inbuff.Length + 7;
			if (base.TrafficStatsEnabled)
			{
				TrafficStatsIncoming.TotalPacketCount++;
				TrafficStatsIncoming.TotalCommandsInPackets++;
			}
			if (inbuff[0] == 243 || inbuff[0] == 244)
			{
				lock (incomingList)
				{
					incomingList.Enqueue(inbuff);
					if (incomingList.get_Count() % warningSize == 0)
					{
						EnqueueStatusCallback(StatusCode.QueueIncomingReliableWarning);
					}
					return;
				}
			}
			if (inbuff[0] == 240)
			{
				TrafficStatsIncoming.CountControlCommand(inbuff.Length);
				ReadPingResult(inbuff);
			}
			else if ((int)debugOut >= 1)
			{
				EnqueueDebugReturn(DebugLevel.ERROR, string.Concat((object)"receiveIncomingCommands() MagicNumber should be 0xF0, 0xF3 or 0xF4. Is: ", (object)inbuff[0]));
			}
		}

		private void ReadPingResult(byte[] inbuff)
		{
			int value = 0;
			int value2 = 0;
			int offset = 1;
			Protocol.Deserialize(out value, inbuff, ref offset);
			Protocol.Deserialize(out value2, inbuff, ref offset);
			lastRoundTripTime = SupportClass.GetTickCount() - value2;
			if (!serverTimeOffsetIsAvailable)
			{
				roundTripTime = lastRoundTripTime;
			}
			UpdateRoundTripTimeAndVariance(lastRoundTripTime);
			if (!serverTimeOffsetIsAvailable)
			{
				serverTimeOffset = value + (lastRoundTripTime >> 1) - SupportClass.GetTickCount();
				serverTimeOffsetIsAvailable = true;
			}
		}

		protected internal void ReadPingResult(OperationResponse operationResponse)
		{
			int num = (int)operationResponse.Parameters.get_Item((byte)2);
			int num2 = (int)operationResponse.Parameters.get_Item((byte)1);
			lastRoundTripTime = SupportClass.GetTickCount() - num2;
			if (!serverTimeOffsetIsAvailable)
			{
				roundTripTime = lastRoundTripTime;
			}
			UpdateRoundTripTimeAndVariance(lastRoundTripTime);
			if (!serverTimeOffsetIsAvailable)
			{
				serverTimeOffset = num + (lastRoundTripTime >> 1) - SupportClass.GetTickCount();
				serverTimeOffsetIsAvailable = true;
			}
		}

		static TPeer()
		{
			byte[] array = new byte[9];
			RuntimeHelpers.InitializeArray((global::System.Array)array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			tcpFramedMessageHead = array;
			tcpMsgHead = new byte[2] { 243, 2 };
		}
	}
}
