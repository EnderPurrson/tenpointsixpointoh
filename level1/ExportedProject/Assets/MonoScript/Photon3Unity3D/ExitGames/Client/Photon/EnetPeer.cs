using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExitGames.Client.Photon
{
	internal class EnetPeer : PeerBase
	{
		private const int CRC_LENGTH = 4;

		private Dictionary<byte, EnetChannel> channels = new Dictionary<byte, EnetChannel>();

		private List<NCommand> sentReliableCommands = new List<NCommand>();

		private Queue<NCommand> outgoingAcknowledgementsList = new Queue<NCommand>();

		internal readonly int windowSize = 128;

		private byte udpCommandCount;

		private byte[] udpBuffer;

		private int udpBufferIndex;

		internal int challenge;

		internal int reliableCommandsRepeated;

		internal int reliableCommandsSent;

		internal int serverSentTime;

		internal static readonly byte[] udpHeader0xF3 = new byte[2] { 243, 2 };

		internal static readonly byte[] messageHeader = udpHeader0xF3;

		private byte[] initData = null;

		private EnetChannel[] channelArray = new EnetChannel[0];

		private Queue<int> commandsToRemove = new Queue<int>();

		internal override int QueuedIncomingCommandsCount
		{
			get
			{
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				int num = 0;
				lock (channels)
				{
					Enumerator<byte, EnetChannel> enumerator = channels.get_Values().GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							EnetChannel current = enumerator.get_Current();
							num += current.incomingReliableCommandsList.get_Count();
							num += current.incomingUnreliableCommandsList.get_Count();
						}
					}
					finally
					{
						((global::System.IDisposable)enumerator).Dispose();
					}
				}
				return num;
			}
		}

		internal override int QueuedOutgoingCommandsCount
		{
			get
			{
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				int num = 0;
				lock (channels)
				{
					Enumerator<byte, EnetChannel> enumerator = channels.get_Values().GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							EnetChannel current = enumerator.get_Current();
							num += current.outgoingReliableCommandsList.get_Count();
							num += current.outgoingUnreliableCommandsList.get_Count();
						}
					}
					finally
					{
						((global::System.IDisposable)enumerator).Dispose();
					}
				}
				return num;
			}
		}

		internal EnetPeer()
		{
			PeerBase.peerCount++;
			InitOnce();
			TrafficPackageHeaderSize = 12;
		}

		internal EnetPeer(IPhotonPeerListener listener)
			: this()
		{
			base.Listener = listener;
		}

		internal override void InitPeerBase()
		{
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			base.InitPeerBase();
			peerID = -1;
			challenge = SupportClass.ThreadSafeRandom.Next();
			udpBuffer = new byte[mtu];
			reliableCommandsSent = 0;
			reliableCommandsRepeated = 0;
			lock (channels)
			{
				channels = new Dictionary<byte, EnetChannel>();
			}
			lock (channels)
			{
				channels.set_Item((byte)255, new EnetChannel(255, commandBufferSize));
				for (byte b = 0; b < ChannelCount; b = (byte)(b + 1))
				{
					channels.set_Item(b, new EnetChannel(b, commandBufferSize));
				}
				channelArray = new EnetChannel[ChannelCount + 1];
				int num = 0;
				Enumerator<byte, EnetChannel> enumerator = channels.get_Values().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						EnetChannel current = enumerator.get_Current();
						channelArray[num++] = current;
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator).Dispose();
				}
			}
			lock (sentReliableCommands)
			{
				sentReliableCommands = new List<NCommand>(commandBufferSize);
			}
			lock (outgoingAcknowledgementsList)
			{
				outgoingAcknowledgementsList = new Queue<NCommand>(commandBufferSize);
			}
			CommandLogInit();
		}

		internal override bool Connect(string ipport, string appID)
		{
			if (peerConnectionState != 0)
			{
				base.Listener.DebugReturn(DebugLevel.WARNING, string.Concat((object)"Connect() can't be called if peer is not Disconnected. Not connecting. peerConnectionState: ", (object)peerConnectionState));
				return false;
			}
			if ((int)debugOut >= 5)
			{
				base.Listener.DebugReturn(DebugLevel.ALL, "Connect()");
			}
			base.ServerAddress = ipport;
			InitPeerBase();
			if (appID == null)
			{
				appID = "LoadBalancing";
			}
			for (int i = 0; i < 32; i++)
			{
				INIT_BYTES[i + 9] = (byte)((i < appID.get_Length()) ? ((byte)appID.get_Chars(i)) : 0);
			}
			base.Listener.DebugReturn(DebugLevel.ALL, string.Concat((object)"EnetPeer connect: Init data IPv6 bit set: ", (object)INIT_BYTES[5]));
			initData = INIT_BYTES;
			rt = new SocketUdp(this);
			if (rt == null)
			{
				base.Listener.DebugReturn(DebugLevel.ERROR, "Connect() failed, because SocketImplementation or socket was null. Set PhotonPeer.SocketImplementation before Connect().");
				return false;
			}
			if (rt.Connect())
			{
				if (base.TrafficStatsEnabled)
				{
					TrafficStatsOutgoing.ControlCommandBytes += 44;
					TrafficStatsOutgoing.ControlCommandCount++;
				}
				peerConnectionState = ConnectionStateValue.Connecting;
				return true;
			}
			return false;
		}

		public override void OnConnect()
		{
			QueueOutgoingReliableCommand(new NCommand(this, 2, null, 255));
		}

		internal override void Disconnect()
		{
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			if (peerConnectionState == ConnectionStateValue.Disconnected || peerConnectionState == ConnectionStateValue.Disconnecting)
			{
				return;
			}
			if (outgoingAcknowledgementsList != null)
			{
				lock (outgoingAcknowledgementsList)
				{
					outgoingAcknowledgementsList.Clear();
				}
			}
			if (sentReliableCommands != null)
			{
				lock (sentReliableCommands)
				{
					sentReliableCommands.Clear();
				}
			}
			lock (channels)
			{
				Enumerator<byte, EnetChannel> enumerator = channels.get_Values().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						EnetChannel current = enumerator.get_Current();
						current.clearAll();
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator).Dispose();
				}
			}
			bool isSimulationEnabled = base.NetworkSimulationSettings.IsSimulationEnabled;
			base.NetworkSimulationSettings.IsSimulationEnabled = false;
			NCommand nCommand = new NCommand(this, 4, null, 255);
			QueueOutgoingReliableCommand(nCommand);
			SendOutgoingCommands();
			if (base.TrafficStatsEnabled)
			{
				TrafficStatsOutgoing.CountControlCommand(nCommand.Size);
			}
			base.NetworkSimulationSettings.IsSimulationEnabled = isSimulationEnabled;
			rt.Disconnect();
			peerConnectionState = ConnectionStateValue.Disconnected;
			base.Listener.OnStatusChanged(StatusCode.Disconnect);
		}

		internal override void StopConnection()
		{
			if (rt != null)
			{
				rt.Disconnect();
			}
			peerConnectionState = ConnectionStateValue.Disconnected;
			if (base.Listener != null)
			{
				base.Listener.OnStatusChanged(StatusCode.Disconnect);
			}
		}

		internal override void FetchServerTimestamp()
		{
			if (peerConnectionState != ConnectionStateValue.Connected)
			{
				if ((int)debugOut >= 3)
				{
					EnqueueDebugReturn(DebugLevel.INFO, string.Concat((object)"FetchServerTimestamp() was skipped, as the client is not connected. Current ConnectionState: ", (object)peerConnectionState));
				}
			}
			else
			{
				CreateAndEnqueueCommand(12, new byte[0], 255);
			}
		}

		internal override bool DispatchIncomingCommands()
		{
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
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
			NCommand nCommand = null;
			lock (channels)
			{
				for (int i = 0; i < channelArray.Length; i++)
				{
					EnetChannel enetChannel = channelArray[i];
					if (enetChannel.incomingUnreliableCommandsList.get_Count() > 0)
					{
						int num = 2147483647;
						Enumerator<int, NCommand> enumerator = enetChannel.incomingUnreliableCommandsList.get_Keys().GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								int current = enumerator.get_Current();
								NCommand nCommand2 = enetChannel.incomingUnreliableCommandsList.get_Item(current);
								if (current < enetChannel.incomingUnreliableSequenceNumber || nCommand2.reliableSequenceNumber < enetChannel.incomingReliableSequenceNumber)
								{
									commandsToRemove.Enqueue(current);
								}
								else if (limitOfUnreliableCommands > 0 && enetChannel.incomingUnreliableCommandsList.get_Count() > limitOfUnreliableCommands)
								{
									commandsToRemove.Enqueue(current);
								}
								else if (current < num && nCommand2.reliableSequenceNumber <= enetChannel.incomingReliableSequenceNumber)
								{
									num = current;
								}
							}
						}
						finally
						{
							((global::System.IDisposable)enumerator).Dispose();
						}
						while (commandsToRemove.get_Count() > 0)
						{
							enetChannel.incomingUnreliableCommandsList.Remove(commandsToRemove.Dequeue());
						}
						if (num < 2147483647)
						{
							nCommand = enetChannel.incomingUnreliableCommandsList.get_Item(num);
						}
						if (nCommand != null)
						{
							enetChannel.incomingUnreliableCommandsList.Remove(nCommand.unreliableSequenceNumber);
							enetChannel.incomingUnreliableSequenceNumber = nCommand.unreliableSequenceNumber;
							break;
						}
					}
					if (nCommand != null || enetChannel.incomingReliableCommandsList.get_Count() <= 0)
					{
						continue;
					}
					enetChannel.incomingReliableCommandsList.TryGetValue(enetChannel.incomingReliableSequenceNumber + 1, ref nCommand);
					if (nCommand == null)
					{
						continue;
					}
					if (nCommand.commandType != 8)
					{
						enetChannel.incomingReliableSequenceNumber = nCommand.reliableSequenceNumber;
						enetChannel.incomingReliableCommandsList.Remove(nCommand.reliableSequenceNumber);
						break;
					}
					if (nCommand.fragmentsRemaining > 0)
					{
						nCommand = null;
						break;
					}
					byte[] array = new byte[nCommand.totalLength];
					for (int j = nCommand.startSequenceNumber; j < nCommand.startSequenceNumber + nCommand.fragmentCount; j++)
					{
						if (enetChannel.ContainsReliableSequenceNumber(j))
						{
							NCommand nCommand3 = enetChannel.FetchReliableSequenceNumber(j);
							Buffer.BlockCopy((global::System.Array)nCommand3.Payload, 0, (global::System.Array)array, nCommand3.fragmentOffset, nCommand3.Payload.Length);
							enetChannel.incomingReliableCommandsList.Remove(nCommand3.reliableSequenceNumber);
							continue;
						}
						throw new global::System.Exception("command.fragmentsRemaining was 0, but not all fragments are found to be combined!");
					}
					if ((int)debugOut >= 5)
					{
						base.Listener.DebugReturn(DebugLevel.ALL, string.Concat((object)"assembled fragmented payload from ", (object)nCommand.fragmentCount, (object)" parts. Dispatching now."));
					}
					nCommand.Payload = array;
					nCommand.Size = 12 * nCommand.fragmentCount + nCommand.totalLength;
					enetChannel.incomingReliableSequenceNumber = nCommand.reliableSequenceNumber + nCommand.fragmentCount - 1;
					break;
				}
			}
			if (nCommand != null && nCommand.Payload != null)
			{
				ByteCountCurrentDispatch = nCommand.Size;
				CommandInCurrentDispatch = nCommand;
				if (DeserializeMessageAndCallback(nCommand.Payload))
				{
					CommandInCurrentDispatch = null;
					return true;
				}
				CommandInCurrentDispatch = null;
			}
			return false;
		}

		internal override bool SendAcksOnly()
		{
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			if (peerConnectionState == ConnectionStateValue.Disconnected)
			{
				return false;
			}
			if (rt == null || !rt.Connected)
			{
				return false;
			}
			lock (udpBuffer)
			{
				int num = 0;
				udpBufferIndex = 12;
				if (crcEnabled)
				{
					udpBufferIndex += 4;
				}
				udpCommandCount = 0;
				timeInt = SupportClass.GetTickCount() - timeBase;
				lock (outgoingAcknowledgementsList)
				{
					if (outgoingAcknowledgementsList.get_Count() > 0)
					{
						num = SerializeToBuffer(outgoingAcknowledgementsList);
						timeLastSendAck = timeInt;
					}
				}
				if (timeInt > timeoutInt && sentReliableCommands.get_Count() > 0)
				{
					lock (sentReliableCommands)
					{
						Enumerator<NCommand> enumerator = sentReliableCommands.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								NCommand current = enumerator.get_Current();
								if (current != null && current.roundTripTimeout != 0 && timeInt - current.commandSentTime > current.roundTripTimeout)
								{
									current.commandSentCount = 1;
									current.roundTripTimeout = 0;
									current.timeoutTime = 2147483647;
									current.commandSentTime = timeInt;
								}
							}
						}
						finally
						{
							((global::System.IDisposable)enumerator).Dispose();
						}
					}
				}
				if (udpCommandCount <= 0)
				{
					return false;
				}
				if (base.TrafficStatsEnabled)
				{
					TrafficStatsOutgoing.TotalPacketCount++;
					TrafficStatsOutgoing.TotalCommandsInPackets += udpCommandCount;
				}
				SendData(udpBuffer, udpBufferIndex);
				return num > 0;
			}
		}

		internal override bool SendOutgoingCommands()
		{
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			if (peerConnectionState == ConnectionStateValue.Disconnected)
			{
				return false;
			}
			if (!rt.Connected)
			{
				return false;
			}
			lock (udpBuffer)
			{
				int num = 0;
				udpBufferIndex = 12;
				if (crcEnabled)
				{
					udpBufferIndex += 4;
				}
				udpCommandCount = 0;
				timeInt = SupportClass.GetTickCount() - timeBase;
				timeLastSendOutgoing = timeInt;
				lock (outgoingAcknowledgementsList)
				{
					if (outgoingAcknowledgementsList.get_Count() > 0)
					{
						num = SerializeToBuffer(outgoingAcknowledgementsList);
						timeLastSendAck = timeInt;
					}
				}
				if (!base.IsSendingOnlyAcks && timeInt > timeoutInt && sentReliableCommands.get_Count() > 0)
				{
					lock (sentReliableCommands)
					{
						Queue<NCommand> val = new Queue<NCommand>();
						Enumerator<NCommand> enumerator = sentReliableCommands.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								NCommand current = enumerator.get_Current();
								if (current == null || timeInt - current.commandSentTime <= current.roundTripTimeout)
								{
									continue;
								}
								if (current.commandSentCount > sentCountAllowance || timeInt > current.timeoutTime)
								{
									if ((int)debugOut >= 2)
									{
										base.Listener.DebugReturn(DebugLevel.WARNING, string.Concat(new object[6]
										{
											"Timeout-disconnect! Command: ",
											current,
											" now: ",
											timeInt,
											" challenge: ",
											Convert.ToString(challenge, 16)
										}));
									}
									if (CommandLog != null)
									{
										CommandLog.Enqueue((CmdLogItem)new CmdLogSentReliable(current, timeInt, roundTripTime, roundTripTimeVariance, true));
										CommandLogResize();
									}
									peerConnectionState = ConnectionStateValue.Zombie;
									base.Listener.OnStatusChanged(StatusCode.TimeoutDisconnect);
									Disconnect();
									return false;
								}
								val.Enqueue(current);
							}
						}
						finally
						{
							((global::System.IDisposable)enumerator).Dispose();
						}
						while (val.get_Count() > 0)
						{
							NCommand nCommand = val.Dequeue();
							QueueOutgoingReliableCommand(nCommand);
							sentReliableCommands.Remove(nCommand);
							reliableCommandsRepeated++;
							if ((int)debugOut >= 3)
							{
								base.Listener.DebugReturn(DebugLevel.INFO, string.Format("Resending: {0}. times out after: {1} sent: {3} now: {2} rtt/var: {4}/{5} last recv: {6}", new object[7]
								{
									nCommand,
									nCommand.roundTripTimeout,
									timeInt,
									nCommand.commandSentTime,
									roundTripTime,
									roundTripTimeVariance,
									SupportClass.GetTickCount() - timestampOfLastReceive
								}));
							}
						}
					}
				}
				if (!base.IsSendingOnlyAcks && peerConnectionState == ConnectionStateValue.Connected && timePingInterval > 0 && sentReliableCommands.get_Count() == 0 && timeInt - timeLastAckReceive > timePingInterval && !AreReliableCommandsInTransit() && udpBufferIndex + 12 < udpBuffer.Length)
				{
					NCommand nCommand2 = new NCommand(this, 5, null, 255);
					QueueOutgoingReliableCommand(nCommand2);
					if (base.TrafficStatsEnabled)
					{
						TrafficStatsOutgoing.CountControlCommand(nCommand2.Size);
					}
				}
				if (!base.IsSendingOnlyAcks)
				{
					lock (channels)
					{
						for (int i = 0; i < channelArray.Length; i++)
						{
							EnetChannel enetChannel = channelArray[i];
							num += SerializeToBuffer(enetChannel.outgoingReliableCommandsList);
							num += SerializeToBuffer(enetChannel.outgoingUnreliableCommandsList);
						}
					}
				}
				if (udpCommandCount <= 0)
				{
					return false;
				}
				if (base.TrafficStatsEnabled)
				{
					TrafficStatsOutgoing.TotalPacketCount++;
					TrafficStatsOutgoing.TotalCommandsInPackets += udpCommandCount;
				}
				SendData(udpBuffer, udpBufferIndex);
				return num > 0;
			}
		}

		private bool AreReliableCommandsInTransit()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			lock (channels)
			{
				Enumerator<byte, EnetChannel> enumerator = channels.get_Values().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						EnetChannel current = enumerator.get_Current();
						if (current.outgoingReliableCommandsList.get_Count() > 0)
						{
							return true;
						}
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator).Dispose();
				}
			}
			return false;
		}

		internal override bool EnqueueOperation(Dictionary<byte, object> parameters, byte opCode, bool sendReliable, byte channelId, bool encrypt, EgMessageType messageType)
		{
			if (peerConnectionState != ConnectionStateValue.Connected)
			{
				if ((int)debugOut >= 1)
				{
					base.Listener.DebugReturn(DebugLevel.ERROR, string.Concat(new object[4] { "Cannot send op: ", opCode, " Not connected. PeerState: ", peerConnectionState }));
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
			byte[] payload = SerializeOperationToMessage(opCode, parameters, messageType, encrypt);
			return CreateAndEnqueueCommand((byte)(sendReliable ? 6 : 7), payload, channelId);
		}

		internal bool CreateAndEnqueueCommand(byte commandType, byte[] payload, byte channelNumber)
		{
			if (payload == null)
			{
				return false;
			}
			EnetChannel enetChannel = channels.get_Item(channelNumber);
			ByteCountLastOperation = 0;
			int num = mtu - 12 - 36;
			if (payload.Length > num)
			{
				int fragmentCount = (payload.Length + num - 1) / num;
				int startSequenceNumber = enetChannel.outgoingReliableSequenceNumber + 1;
				int num2 = 0;
				for (int i = 0; i < payload.Length; i += num)
				{
					if (payload.Length - i < num)
					{
						num = payload.Length - i;
					}
					byte[] array = new byte[num];
					Buffer.BlockCopy((global::System.Array)payload, i, (global::System.Array)array, 0, num);
					NCommand nCommand = new NCommand(this, 8, array, enetChannel.ChannelNumber);
					nCommand.fragmentNumber = num2;
					nCommand.startSequenceNumber = startSequenceNumber;
					nCommand.fragmentCount = fragmentCount;
					nCommand.totalLength = payload.Length;
					nCommand.fragmentOffset = i;
					QueueOutgoingReliableCommand(nCommand);
					ByteCountLastOperation += nCommand.Size;
					if (base.TrafficStatsEnabled)
					{
						TrafficStatsOutgoing.CountFragmentOpCommand(nCommand.Size);
						TrafficStatsGameLevel.CountOperation(nCommand.Size);
					}
					num2++;
				}
			}
			else
			{
				NCommand nCommand2 = new NCommand(this, commandType, payload, enetChannel.ChannelNumber);
				if (nCommand2.commandFlags == 1)
				{
					QueueOutgoingReliableCommand(nCommand2);
					ByteCountLastOperation = nCommand2.Size;
					if (base.TrafficStatsEnabled)
					{
						TrafficStatsOutgoing.CountReliableOpCommand(nCommand2.Size);
						TrafficStatsGameLevel.CountOperation(nCommand2.Size);
					}
				}
				else
				{
					QueueOutgoingUnreliableCommand(nCommand2);
					ByteCountLastOperation = nCommand2.Size;
					if (base.TrafficStatsEnabled)
					{
						TrafficStatsOutgoing.CountUnreliableOpCommand(nCommand2.Size);
						TrafficStatsGameLevel.CountOperation(nCommand2.Size);
					}
				}
			}
			return true;
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
			return array;
		}

		internal int SerializeToBuffer(Queue<NCommand> commandList)
		{
			while (commandList.get_Count() > 0)
			{
				NCommand nCommand = commandList.Peek();
				if (nCommand == null)
				{
					commandList.Dequeue();
					continue;
				}
				if (udpBufferIndex + nCommand.Size > udpBuffer.Length)
				{
					if ((int)debugOut >= 3)
					{
						base.Listener.DebugReturn(DebugLevel.INFO, string.Concat(new object[4]
						{
							"UDP package is full. Commands in Package: ",
							udpCommandCount,
							". Commands left in queue: ",
							commandList.get_Count()
						}));
					}
					break;
				}
				Buffer.BlockCopy((global::System.Array)nCommand.SerializeHeader(), 0, (global::System.Array)udpBuffer, udpBufferIndex, nCommand.SizeOfHeader);
				udpBufferIndex += nCommand.SizeOfHeader;
				if (nCommand.SizeOfPayload > 0)
				{
					Buffer.BlockCopy((global::System.Array)nCommand.Serialize(), 0, (global::System.Array)udpBuffer, udpBufferIndex, nCommand.SizeOfPayload);
					udpBufferIndex += nCommand.SizeOfPayload;
				}
				udpCommandCount++;
				if ((nCommand.commandFlags & 1) > 0)
				{
					QueueSentCommand(nCommand);
					if (CommandLog != null)
					{
						CommandLog.Enqueue((CmdLogItem)new CmdLogSentReliable(nCommand, timeInt, roundTripTime, roundTripTimeVariance));
						CommandLogResize();
					}
				}
				commandList.Dequeue();
			}
			return commandList.get_Count();
		}

		internal void SendData(byte[] data, int length)
		{
			_003C_003Ec__DisplayClass37_1 _003C_003Ec__DisplayClass37_ = new _003C_003Ec__DisplayClass37_1();
			_003C_003Ec__DisplayClass37_._003C_003E4__this = this;
			_003C_003Ec__DisplayClass37_.length = length;
			try
			{
				int targetOffset = 0;
				Protocol.Serialize(peerID, data, ref targetOffset);
				data[2] = (byte)(crcEnabled ? 204 : 0);
				data[3] = udpCommandCount;
				targetOffset = 4;
				Protocol.Serialize(timeInt, data, ref targetOffset);
				Protocol.Serialize(challenge, data, ref targetOffset);
				if (crcEnabled)
				{
					Protocol.Serialize(0, data, ref targetOffset);
					uint value = SupportClass.CalculateCrc(data, _003C_003Ec__DisplayClass37_.length);
					targetOffset -= 4;
					Protocol.Serialize((int)value, data, ref targetOffset);
				}
				bytesOut += _003C_003Ec__DisplayClass37_.length;
				if (base.NetworkSimulationSettings.IsSimulationEnabled)
				{
					_003C_003Ec__DisplayClass37_0 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass37_0();
					CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1 = _003C_003Ec__DisplayClass37_;
					CS_0024_003C_003E8__locals0.dataCopy = new byte[CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.length];
					Buffer.BlockCopy((global::System.Array)data, 0, (global::System.Array)CS_0024_003C_003E8__locals0.dataCopy, 0, CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.length);
					SendNetworkSimulated(delegate
					{
						CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1._003C_003E4__this.rt.Send(CS_0024_003C_003E8__locals0.dataCopy, CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.length);
					});
				}
				else
				{
					rt.Send(data, _003C_003Ec__DisplayClass37_.length);
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

		internal void QueueSentCommand(NCommand command)
		{
			command.commandSentTime = timeInt;
			command.commandSentCount++;
			if (command.roundTripTimeout == 0)
			{
				command.roundTripTimeout = roundTripTime + 4 * roundTripTimeVariance;
				command.timeoutTime = timeInt + DisconnectTimeout;
			}
			else if (command.commandSentCount > base.QuickResendAttempts + 1)
			{
				command.roundTripTimeout *= 2;
			}
			lock (sentReliableCommands)
			{
				if (sentReliableCommands.get_Count() == 0)
				{
					int num = command.commandSentTime + command.roundTripTimeout;
					if (num < timeoutInt)
					{
						timeoutInt = num;
					}
				}
				reliableCommandsSent++;
				sentReliableCommands.Add(command);
			}
			if (sentReliableCommands.get_Count() >= warningSize && sentReliableCommands.get_Count() % warningSize == 0)
			{
				base.Listener.OnStatusChanged(StatusCode.QueueSentWarning);
			}
		}

		internal void QueueOutgoingReliableCommand(NCommand command)
		{
			EnetChannel enetChannel = channels.get_Item(command.commandChannelID);
			lock (enetChannel)
			{
				Queue<NCommand> outgoingReliableCommandsList = enetChannel.outgoingReliableCommandsList;
				if (outgoingReliableCommandsList.get_Count() >= warningSize && outgoingReliableCommandsList.get_Count() % warningSize == 0)
				{
					base.Listener.OnStatusChanged(StatusCode.QueueOutgoingReliableWarning);
				}
				if (command.reliableSequenceNumber == 0)
				{
					command.reliableSequenceNumber = ++enetChannel.outgoingReliableSequenceNumber;
				}
				outgoingReliableCommandsList.Enqueue(command);
			}
		}

		internal void QueueOutgoingUnreliableCommand(NCommand command)
		{
			Queue<NCommand> outgoingUnreliableCommandsList = channels.get_Item(command.commandChannelID).outgoingUnreliableCommandsList;
			if (outgoingUnreliableCommandsList.get_Count() >= warningSize && outgoingUnreliableCommandsList.get_Count() % warningSize == 0)
			{
				base.Listener.OnStatusChanged(StatusCode.QueueOutgoingUnreliableWarning);
			}
			EnetChannel enetChannel = channels.get_Item(command.commandChannelID);
			command.reliableSequenceNumber = enetChannel.outgoingReliableSequenceNumber;
			command.unreliableSequenceNumber = ++enetChannel.outgoingUnreliableSequenceNumber;
			outgoingUnreliableCommandsList.Enqueue(command);
		}

		internal void QueueOutgoingAcknowledgement(NCommand command)
		{
			lock (outgoingAcknowledgementsList)
			{
				if (outgoingAcknowledgementsList.get_Count() >= warningSize && outgoingAcknowledgementsList.get_Count() % warningSize == 0)
				{
					base.Listener.OnStatusChanged(StatusCode.QueueOutgoingAcksWarning);
				}
				outgoingAcknowledgementsList.Enqueue(command);
			}
		}

		internal override void ReceiveIncomingCommands(byte[] inBuff, int dataLength)
		{
			timestampOfLastReceive = SupportClass.GetTickCount();
			try
			{
				int offset = 0;
				short value;
				Protocol.Deserialize(out value, inBuff, ref offset);
				byte b = inBuff[offset++];
				byte b2 = inBuff[offset++];
				Protocol.Deserialize(out serverSentTime, inBuff, ref offset);
				int value2;
				Protocol.Deserialize(out value2, inBuff, ref offset);
				if (b == 204)
				{
					int value3;
					Protocol.Deserialize(out value3, inBuff, ref offset);
					bytesIn += 4L;
					offset -= 4;
					Protocol.Serialize(0, inBuff, ref offset);
					uint num = SupportClass.CalculateCrc(inBuff, dataLength);
					if (value3 != (int)num)
					{
						packetLossByCrc++;
						if (peerConnectionState != 0 && (int)debugOut >= 3)
						{
							EnqueueDebugReturn(DebugLevel.INFO, string.Format("Ignored package due to wrong CRC. Incoming:  {0:X} Local: {1:X}", (object)(uint)value3, (object)num));
						}
						return;
					}
				}
				bytesIn += 12L;
				if (base.TrafficStatsEnabled)
				{
					TrafficStatsIncoming.TotalPacketCount++;
					TrafficStatsIncoming.TotalCommandsInPackets += b2;
				}
				if (b2 > commandBufferSize || b2 <= 0)
				{
					EnqueueDebugReturn(DebugLevel.ERROR, string.Concat(new object[4] { "too many/few incoming commands in package: ", b2, " > ", commandBufferSize }));
				}
				if (value2 != challenge)
				{
					packetLossByChallenge++;
					if (peerConnectionState != 0 && (int)debugOut >= 5)
					{
						EnqueueDebugReturn(DebugLevel.ALL, string.Concat(new object[6] { "Info: Ignoring received package due to wrong challenge. Challenge in-package!=local:", value2, "!=", challenge, " Commands in it: ", b2 }));
					}
					return;
				}
				timeInt = SupportClass.GetTickCount() - timeBase;
				for (int i = 0; i < b2; i++)
				{
					_003C_003Ec__DisplayClass42_0 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass42_0();
					CS_0024_003C_003E8__locals0._003C_003E4__this = this;
					CS_0024_003C_003E8__locals0.readCommand = new NCommand(this, inBuff, ref offset);
					if (CS_0024_003C_003E8__locals0.readCommand.commandType != 1)
					{
						EnqueueActionForDispatch(delegate
						{
							CS_0024_003C_003E8__locals0._003C_003E4__this.ExecuteCommand(CS_0024_003C_003E8__locals0.readCommand);
						});
					}
					else
					{
						TrafficStatsIncoming.TimestampOfLastAck = SupportClass.GetTickCount();
						ExecuteCommand(CS_0024_003C_003E8__locals0.readCommand);
					}
					if ((CS_0024_003C_003E8__locals0.readCommand.commandFlags & 1) > 0)
					{
						if (InReliableLog != null)
						{
							InReliableLog.Enqueue((CmdLogItem)new CmdLogReceivedReliable(CS_0024_003C_003E8__locals0.readCommand, timeInt, roundTripTime, roundTripTimeVariance, timeInt - timeLastSendOutgoing, timeInt - timeLastSendAck));
							CommandLogResize();
						}
						NCommand nCommand = NCommand.CreateAck(this, CS_0024_003C_003E8__locals0.readCommand, serverSentTime);
						QueueOutgoingAcknowledgement(nCommand);
						if (base.TrafficStatsEnabled)
						{
							TrafficStatsIncoming.TimestampOfLastReliableCommand = SupportClass.GetTickCount();
							TrafficStatsOutgoing.CountControlCommand(nCommand.Size);
						}
					}
				}
			}
			catch (global::System.Exception ex)
			{
				if ((int)debugOut >= 1)
				{
					EnqueueDebugReturn(DebugLevel.ERROR, string.Format("Exception while reading commands from incoming data: {0}", (object)ex));
				}
				SupportClass.WriteStackTrace(ex);
			}
		}

		internal bool ExecuteCommand(NCommand command)
		{
			bool flag = true;
			switch (command.commandType)
			{
			case 2:
			case 5:
				if (base.TrafficStatsEnabled)
				{
					TrafficStatsIncoming.CountControlCommand(command.Size);
				}
				break;
			case 4:
			{
				if (base.TrafficStatsEnabled)
				{
					TrafficStatsIncoming.CountControlCommand(command.Size);
				}
				StatusCode statusCode = StatusCode.DisconnectByServer;
				if (command.reservedByte == 1)
				{
					statusCode = StatusCode.DisconnectByServerLogic;
				}
				else if (command.reservedByte == 3)
				{
					statusCode = StatusCode.DisconnectByServerUserLimit;
				}
				if ((int)debugOut >= 3)
				{
					base.Listener.DebugReturn(DebugLevel.INFO, string.Concat(new object[10]
					{
						"Server ",
						base.ServerAddress,
						" sent disconnect. PeerId: ",
						(ushort)peerID,
						" RTT/Variance:",
						roundTripTime,
						"/",
						roundTripTimeVariance,
						" reason byte: ",
						command.reservedByte
					}));
				}
				ConnectionStateValue connectionStateValue = peerConnectionState;
				peerConnectionState = ConnectionStateValue.Disconnecting;
				base.Listener.OnStatusChanged(statusCode);
				peerConnectionState = connectionStateValue;
				Disconnect();
				break;
			}
			case 1:
			{
				if (base.TrafficStatsEnabled)
				{
					TrafficStatsIncoming.CountControlCommand(command.Size);
				}
				timeLastAckReceive = timeInt;
				lastRoundTripTime = timeInt - command.ackReceivedSentTime;
				NCommand nCommand = RemoveSentReliableCommand(command.ackReceivedReliableSequenceNumber, command.commandChannelID);
				if (CommandLog != null)
				{
					CommandLog.Enqueue((CmdLogItem)new CmdLogReceivedAck(command, timeInt, roundTripTime, roundTripTimeVariance));
					CommandLogResize();
				}
				if (nCommand == null)
				{
					break;
				}
				if (nCommand.commandType == 12)
				{
					if (lastRoundTripTime <= roundTripTime)
					{
						serverTimeOffset = serverSentTime + (lastRoundTripTime >> 1) - SupportClass.GetTickCount();
						serverTimeOffsetIsAvailable = true;
					}
					else
					{
						FetchServerTimestamp();
					}
					break;
				}
				UpdateRoundTripTimeAndVariance(lastRoundTripTime);
				if (nCommand.commandType == 4 && peerConnectionState == ConnectionStateValue.Disconnecting)
				{
					if ((int)debugOut >= 3)
					{
						EnqueueDebugReturn(DebugLevel.INFO, "Received disconnect ACK by server");
					}
					EnqueueActionForDispatch(delegate
					{
						rt.Disconnect();
					});
				}
				else if (nCommand.commandType == 2)
				{
					roundTripTime = lastRoundTripTime;
				}
				break;
			}
			case 6:
				if (base.TrafficStatsEnabled)
				{
					TrafficStatsIncoming.CountReliableOpCommand(command.Size);
				}
				if (peerConnectionState == ConnectionStateValue.Connected)
				{
					flag = QueueIncomingCommand(command);
				}
				break;
			case 7:
				if (base.TrafficStatsEnabled)
				{
					TrafficStatsIncoming.CountUnreliableOpCommand(command.Size);
				}
				if (peerConnectionState == ConnectionStateValue.Connected)
				{
					flag = QueueIncomingCommand(command);
				}
				break;
			case 8:
			{
				if (base.TrafficStatsEnabled)
				{
					TrafficStatsIncoming.CountFragmentOpCommand(command.Size);
				}
				if (peerConnectionState != ConnectionStateValue.Connected)
				{
					break;
				}
				if (command.fragmentNumber > command.fragmentCount || command.fragmentOffset >= command.totalLength || command.fragmentOffset + command.Payload.Length > command.totalLength)
				{
					if ((int)debugOut >= 1)
					{
						base.Listener.DebugReturn(DebugLevel.ERROR, string.Concat((object)"Received fragment has bad size: ", (object)command));
					}
					break;
				}
				flag = QueueIncomingCommand(command);
				if (!flag)
				{
					break;
				}
				EnetChannel enetChannel = channels.get_Item(command.commandChannelID);
				if (command.reliableSequenceNumber == command.startSequenceNumber)
				{
					command.fragmentsRemaining--;
					int num = command.startSequenceNumber + 1;
					while (command.fragmentsRemaining > 0 && num < command.startSequenceNumber + command.fragmentCount)
					{
						if (enetChannel.ContainsReliableSequenceNumber(num++))
						{
							command.fragmentsRemaining--;
						}
					}
				}
				else if (enetChannel.ContainsReliableSequenceNumber(command.startSequenceNumber))
				{
					NCommand nCommand2 = enetChannel.FetchReliableSequenceNumber(command.startSequenceNumber);
					nCommand2.fragmentsRemaining--;
				}
				break;
			}
			case 3:
				if (base.TrafficStatsEnabled)
				{
					TrafficStatsIncoming.CountControlCommand(command.Size);
				}
				if (peerConnectionState == ConnectionStateValue.Connecting)
				{
					command = new NCommand(this, 6, initData, 0);
					QueueOutgoingReliableCommand(command);
					if (base.TrafficStatsEnabled)
					{
						TrafficStatsOutgoing.CountControlCommand(command.Size);
					}
					peerConnectionState = ConnectionStateValue.Connected;
				}
				break;
			}
			return flag;
		}

		internal bool QueueIncomingCommand(NCommand command)
		{
			EnetChannel enetChannel = default(EnetChannel);
			channels.TryGetValue(command.commandChannelID, ref enetChannel);
			if (enetChannel == null)
			{
				if ((int)debugOut >= 1)
				{
					base.Listener.DebugReturn(DebugLevel.ERROR, string.Concat((object)"Received command for non-existing channel: ", (object)command.commandChannelID));
				}
				return false;
			}
			if ((int)debugOut >= 5)
			{
				base.Listener.DebugReturn(DebugLevel.ALL, string.Concat(new object[6] { "queueIncomingCommand() ", command, " channel seq# r/u: ", enetChannel.incomingReliableSequenceNumber, "/", enetChannel.incomingUnreliableSequenceNumber }));
			}
			if (command.commandFlags == 1)
			{
				if (command.reliableSequenceNumber <= enetChannel.incomingReliableSequenceNumber)
				{
					if ((int)debugOut >= 3)
					{
						base.Listener.DebugReturn(DebugLevel.INFO, string.Concat(new object[4] { "incoming command ", command, " is old (not saving it). Dispatched incomingReliableSequenceNumber: ", enetChannel.incomingReliableSequenceNumber }));
					}
					return false;
				}
				if (enetChannel.ContainsReliableSequenceNumber(command.reliableSequenceNumber))
				{
					if ((int)debugOut >= 3)
					{
						base.Listener.DebugReturn(DebugLevel.INFO, string.Concat(new object[6]
						{
							"Info: command was received before! Old/New: ",
							enetChannel.FetchReliableSequenceNumber(command.reliableSequenceNumber),
							"/",
							command,
							" inReliableSeq#: ",
							enetChannel.incomingReliableSequenceNumber
						}));
					}
					return false;
				}
				if (enetChannel.incomingReliableCommandsList.get_Count() >= warningSize && enetChannel.incomingReliableCommandsList.get_Count() % warningSize == 0)
				{
					base.Listener.OnStatusChanged(StatusCode.QueueIncomingReliableWarning);
				}
				enetChannel.incomingReliableCommandsList.Add(command.reliableSequenceNumber, command);
				return true;
			}
			if (command.commandFlags == 0)
			{
				if (command.reliableSequenceNumber < enetChannel.incomingReliableSequenceNumber)
				{
					if ((int)debugOut >= 3)
					{
						base.Listener.DebugReturn(DebugLevel.INFO, "incoming reliable-seq# < Dispatched-rel-seq#. not saved.");
					}
					return true;
				}
				if (command.unreliableSequenceNumber <= enetChannel.incomingUnreliableSequenceNumber)
				{
					if ((int)debugOut >= 3)
					{
						base.Listener.DebugReturn(DebugLevel.INFO, "incoming unreliable-seq# < Dispatched-unrel-seq#. not saved.");
					}
					return true;
				}
				if (enetChannel.ContainsUnreliableSequenceNumber(command.unreliableSequenceNumber))
				{
					if ((int)debugOut >= 3)
					{
						base.Listener.DebugReturn(DebugLevel.INFO, string.Concat(new object[4]
						{
							"command was received before! Old/New: ",
							enetChannel.incomingUnreliableCommandsList.get_Item(command.unreliableSequenceNumber),
							"/",
							command
						}));
					}
					return false;
				}
				if (enetChannel.incomingUnreliableCommandsList.get_Count() >= warningSize && enetChannel.incomingUnreliableCommandsList.get_Count() % warningSize == 0)
				{
					base.Listener.OnStatusChanged(StatusCode.QueueIncomingUnreliableWarning);
				}
				enetChannel.incomingUnreliableCommandsList.Add(command.unreliableSequenceNumber, command);
				return true;
			}
			return false;
		}

		internal NCommand RemoveSentReliableCommand(int ackReceivedReliableSequenceNumber, int ackReceivedChannel)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			NCommand nCommand = null;
			lock (sentReliableCommands)
			{
				Enumerator<NCommand> enumerator = sentReliableCommands.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						NCommand current = enumerator.get_Current();
						if (current != null && current.reliableSequenceNumber == ackReceivedReliableSequenceNumber && current.commandChannelID == ackReceivedChannel)
						{
							nCommand = current;
							break;
						}
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator).Dispose();
				}
				if (nCommand != null)
				{
					sentReliableCommands.Remove(nCommand);
					if (sentReliableCommands.get_Count() > 0)
					{
						timeoutInt = timeInt + 25;
					}
				}
				else if ((int)debugOut >= 5 && peerConnectionState != ConnectionStateValue.Connected && peerConnectionState != ConnectionStateValue.Disconnecting)
				{
					EnqueueDebugReturn(DebugLevel.ALL, string.Format("No sent command for ACK (Ch: {0} Sq#: {1}). PeerState: {2}.", (object)ackReceivedReliableSequenceNumber, (object)ackReceivedChannel, (object)peerConnectionState));
				}
			}
			return nCommand;
		}

		internal string CommandListToString(NCommand[] list)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			if ((int)debugOut < 5)
			{
				return string.Empty;
			}
			StringBuilder val = new StringBuilder();
			for (int i = 0; i < list.Length; i++)
			{
				val.Append(string.Concat((object)i, (object)"="));
				val.Append((object)list[i]);
				val.Append(" # ");
			}
			return ((object)val).ToString();
		}
	}
}
