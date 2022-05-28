using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Photon.SocketServer.Security;

namespace ExitGames.Client.Photon
{
	public abstract class PeerBase
	{
		internal delegate void MyAction();

		public enum ConnectionStateValue : byte
		{
			Disconnected = 0,
			Connecting = 1,
			Connected = 3,
			Disconnecting = 4,
			AcknowledgingDisconnect = 5,
			Zombie = 6
		}

		internal enum EgMessageType : byte
		{
			Init = 0,
			InitResponse = 1,
			Operation = 2,
			OperationResponse = 3,
			Event = 4,
			InternalOperationRequest = 6,
			InternalOperationResponse = 7,
			Message = 8,
			RawMessage = 9
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass136_0
		{
			public DebugLevel level;

			public string debugReturn;

			public PeerBase _003C_003E4__this;

			internal void _003CEnqueueDebugReturn_003Eb__0()
			{
				_003C_003E4__this.Listener.DebugReturn(level, debugReturn);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass137_0
		{
			public StatusCode statusValue;

			public PeerBase _003C_003E4__this;

			internal void _003CEnqueueStatusCallback_003Eb__0()
			{
				_003C_003E4__this.Listener.OnStatusChanged(statusValue);
			}
		}

		protected IProtocol protocol = Protocol.ProtocolDefault;

		protected internal global::System.Type SocketImplementation = null;

		internal IPhotonSocket rt;

		public int ByteCountLastOperation;

		public int ByteCountCurrentDispatch;

		internal NCommand CommandInCurrentDispatch;

		internal int TrafficPackageHeaderSize;

		public TrafficStats TrafficStatsIncoming;

		public TrafficStats TrafficStatsOutgoing;

		public TrafficStatsGameLevel TrafficStatsGameLevel;

		private Stopwatch trafficStatsStopwatch;

		private bool trafficStatsEnabled = false;

		internal ConnectionProtocol usedProtocol;

		internal bool crcEnabled = false;

		internal int packetLossByCrc;

		internal int packetLossByChallenge;

		internal DebugLevel debugOut = DebugLevel.ERROR;

		internal readonly Queue<MyAction> ActionQueue = new Queue<MyAction>();

		internal short peerID = -1;

		internal ConnectionStateValue peerConnectionState;

		internal int serverTimeOffset;

		internal bool serverTimeOffsetIsAvailable;

		internal int roundTripTime;

		internal int roundTripTimeVariance;

		internal int lastRoundTripTime;

		internal int lowestRoundTripTime;

		internal int lastRoundTripTimeVariance;

		internal int highestRoundTripTimeVariance;

		internal int timestampOfLastReceive;

		internal int packetThrottleInterval;

		internal static short peerCount;

		internal long bytesOut;

		internal long bytesIn;

		internal int commandBufferSize = 100;

		internal int warningSize = 100;

		internal int sentCountAllowance = 5;

		internal int DisconnectTimeout = 10000;

		internal int timePingInterval = 1000;

		internal byte ChannelCount = 2;

		internal int limitOfUnreliableCommands = 0;

		internal ICryptoProvider CryptoProvider;

		private readonly Random lagRandomizer = new Random();

		internal readonly LinkedList<SimulationItem> NetSimListOutgoing = new LinkedList<SimulationItem>();

		internal readonly LinkedList<SimulationItem> NetSimListIncoming = new LinkedList<SimulationItem>();

		private readonly NetworkSimulationSet networkSimulationSettings = new NetworkSimulationSet();

		internal int CommandLogSize = 0;

		internal Queue<CmdLogItem> CommandLog;

		internal Queue<CmdLogItem> InReliableLog;

		internal byte[] INIT_BYTES = new byte[41];

		internal int timeBase;

		internal int timeInt;

		internal int timeoutInt;

		internal int timeLastAckReceive;

		internal int timeLastSendAck;

		internal int timeLastSendOutgoing;

		internal const int ENET_PEER_PACKET_LOSS_SCALE = 65536;

		internal const int ENET_PEER_DEFAULT_ROUND_TRIP_TIME = 300;

		internal const int ENET_PEER_PACKET_THROTTLE_INTERVAL = 5000;

		internal bool ApplicationIsInitialized;

		internal bool isEncryptionAvailable;

		internal static int outgoingStreamBufferSize = 1200;

		internal int outgoingCommandsInStream = 0;

		internal int mtu = 1200;

		internal int rhttpMinConnections = 2;

		internal int rhttpMaxConnections = 6;

		protected StreamBuffer SerializeMemStream = new StreamBuffer();

		public string ClientVersion
		{
			get
			{
				return string.Format("{0}.{1}.{2}.{3}", new object[4]
				{
					Version.clientVersion[0],
					Version.clientVersion[1],
					Version.clientVersion[2],
					Version.clientVersion[3]
				});
			}
		}

		public long TrafficStatsEnabledTime
		{
			get
			{
				return (trafficStatsStopwatch != null) ? trafficStatsStopwatch.get_ElapsedMilliseconds() : 0;
			}
		}

		public bool TrafficStatsEnabled
		{
			get
			{
				return trafficStatsEnabled;
			}
			set
			{
				trafficStatsEnabled = value;
				if (value)
				{
					if (trafficStatsStopwatch == null)
					{
						InitializeTrafficStats();
					}
					trafficStatsStopwatch.Start();
				}
				else if (trafficStatsStopwatch != null)
				{
					trafficStatsStopwatch.Stop();
				}
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
			internal set
			{
				_003CServerAddress_003Ek__BackingField = value;
			}
		}

		internal string HttpUrlParameters
		{
			[CompilerGenerated]
			get
			{
				return _003CHttpUrlParameters_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CHttpUrlParameters_003Ek__BackingField = value;
			}
		}

		internal IPhotonPeerListener Listener
		{
			[CompilerGenerated]
			get
			{
				return _003CListener_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CListener_003Ek__BackingField = value;
			}
		}

		public byte QuickResendAttempts
		{
			[CompilerGenerated]
			get
			{
				return _003CQuickResendAttempts_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CQuickResendAttempts_003Ek__BackingField = value;
			}
		}

		public NetworkSimulationSet NetworkSimulationSettings
		{
			get
			{
				return networkSimulationSettings;
			}
		}

		internal long BytesOut
		{
			get
			{
				return bytesOut;
			}
		}

		internal long BytesIn
		{
			get
			{
				return bytesIn;
			}
		}

		internal abstract int QueuedIncomingCommandsCount { get; }

		internal abstract int QueuedOutgoingCommandsCount { get; }

		public virtual string PeerID
		{
			get
			{
				return ((ushort)peerID).ToString();
			}
		}

		protected internal byte[] TcpConnectionPrefix
		{
			[CompilerGenerated]
			get
			{
				return _003CTcpConnectionPrefix_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CTcpConnectionPrefix_003Ek__BackingField = value;
			}
		}

		internal bool IsSendingOnlyAcks
		{
			[CompilerGenerated]
			get
			{
				return _003CIsSendingOnlyAcks_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CIsSendingOnlyAcks_003Ek__BackingField = value;
			}
		}

		internal void CommandLogResize()
		{
			if (CommandLogSize <= 0)
			{
				CommandLog = null;
				InReliableLog = null;
				return;
			}
			if (CommandLog == null || InReliableLog == null)
			{
				CommandLogInit();
			}
			while (CommandLog.get_Count() > 0 && CommandLog.get_Count() > CommandLogSize)
			{
				CommandLog.Dequeue();
			}
			while (InReliableLog.get_Count() > 0 && InReliableLog.get_Count() > CommandLogSize)
			{
				InReliableLog.Dequeue();
			}
		}

		internal void CommandLogInit()
		{
			if (CommandLogSize <= 0)
			{
				CommandLog = null;
				InReliableLog = null;
			}
			else if (CommandLog == null || InReliableLog == null)
			{
				CommandLog = new Queue<CmdLogItem>(CommandLogSize);
				InReliableLog = new Queue<CmdLogItem>(CommandLogSize);
			}
			else
			{
				CommandLog.Clear();
				InReliableLog.Clear();
			}
		}

		public string CommandLogToString()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			StringBuilder val = new StringBuilder();
			int num = ((usedProtocol == ConnectionProtocol.Udp) ? ((EnetPeer)this).reliableCommandsRepeated : 0);
			val.AppendFormat("PeerId: {0} Now: {1} Server: {2} State: {3} Total Resends: {4} Received {5}ms ago.\n", new object[6]
			{
				PeerID,
				timeInt,
				ServerAddress,
				peerConnectionState,
				num,
				SupportClass.GetTickCount() - timestampOfLastReceive
			});
			if (CommandLog == null)
			{
				return ((object)val).ToString();
			}
			Enumerator<CmdLogItem> enumerator = CommandLog.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					CmdLogItem current = enumerator.get_Current();
					val.AppendLine(((object)current).ToString());
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			val.AppendLine("Received Reliable Log: ");
			Enumerator<CmdLogItem> enumerator2 = InReliableLog.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					CmdLogItem current2 = enumerator2.get_Current();
					val.AppendLine(((object)current2).ToString());
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator2).Dispose();
			}
			return ((object)val).ToString();
		}

		internal void InitOnce()
		{
			int num = 0;
			networkSimulationSettings.peerBase = this;
			byte[] clientVersion = Version.clientVersion;
			INIT_BYTES[0] = 243;
			INIT_BYTES[1] = 0;
			INIT_BYTES[2] = protocol.VersionBytes[0];
			INIT_BYTES[3] = protocol.VersionBytes[1];
			INIT_BYTES[4] = (byte)(0x1Eu | (uint)num);
			INIT_BYTES[5] = (byte)((byte)(clientVersion[0] << 4) | clientVersion[1]);
			INIT_BYTES[6] = clientVersion[2];
			INIT_BYTES[7] = clientVersion[3];
			INIT_BYTES[8] = 0;
		}

		public virtual void SetInitIPV6Bit(bool isV6)
		{
			Listener.DebugReturn(DebugLevel.ALL, "Setting IPv6 bit " + isV6);
			if (isV6)
			{
				INIT_BYTES[5] |= 128;
			}
			else
			{
				INIT_BYTES[5] &= 127;
			}
		}

		internal abstract bool Connect(string serverAddress, string appID);

		public abstract void OnConnect();

		private string GetHttpKeyValueString(Dictionary<string, string> dic)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			StringBuilder val = new StringBuilder();
			Enumerator<string, string> enumerator = dic.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, string> current = enumerator.get_Current();
					val.Append(current.get_Key()).Append("=").Append(current.get_Value())
						.Append("&");
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			return ((object)val).ToString();
		}

		internal abstract void Disconnect();

		internal abstract void StopConnection();

		internal abstract void FetchServerTimestamp();

		internal bool EnqueueOperation(Dictionary<byte, object> parameters, byte opCode, bool sendReliable, byte channelId, bool encrypted)
		{
			return EnqueueOperation(parameters, opCode, sendReliable, channelId, encrypted, EgMessageType.Operation);
		}

		internal abstract bool EnqueueOperation(Dictionary<byte, object> parameters, byte opCode, bool sendReliable, byte channelId, bool encrypted, EgMessageType messageType);

		internal abstract bool DispatchIncomingCommands();

		internal abstract bool SendOutgoingCommands();

		internal virtual bool SendAcksOnly()
		{
			return false;
		}

		internal byte[] SerializeMessageToMessage(object message, bool encrypt, byte[] messageHeader, bool writeLength = true)
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
				protocol.SerializeMessage(SerializeMemStream, message);
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
			array[messageHeader.Length - 1] = 8;
			if (encrypt)
			{
				array[messageHeader.Length - 1] = (byte)(array[messageHeader.Length - 1] | 0x80u);
			}
			if (writeLength)
			{
				int targetOffset = 1;
				Protocol.Serialize(array.Length, array, ref targetOffset);
			}
			return array;
		}

		internal byte[] SerializeRawMessageToMessage(byte[] data, bool encrypt, byte[] messageHeader, bool writeLength = true)
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
				((Stream)SerializeMemStream).Write(data, 0, data.Length);
				if (encrypt)
				{
					byte[] data2 = SerializeMemStream.ToArray();
					data2 = CryptoProvider.Encrypt(data2);
					((Stream)SerializeMemStream).set_Position(0L);
					((Stream)SerializeMemStream).SetLength(0L);
					((Stream)SerializeMemStream).Write(messageHeader, 0, messageHeader.Length);
					((Stream)SerializeMemStream).Write(data2, 0, data2.Length);
				}
				array = SerializeMemStream.ToArray();
			}
			array[messageHeader.Length - 1] = 9;
			if (encrypt)
			{
				array[messageHeader.Length - 1] = (byte)(array[messageHeader.Length - 1] | 0x80u);
			}
			if (writeLength)
			{
				int targetOffset = 1;
				Protocol.Serialize(array.Length, array, ref targetOffset);
			}
			return array;
		}

		internal abstract byte[] SerializeOperationToMessage(byte opCode, Dictionary<byte, object> parameters, EgMessageType messageType, bool encrypt);

		internal abstract void ReceiveIncomingCommands(byte[] inBuff, int dataLength);

		internal void InitCallback()
		{
			if (peerConnectionState == ConnectionStateValue.Connecting)
			{
				peerConnectionState = ConnectionStateValue.Connected;
			}
			ApplicationIsInitialized = true;
			FetchServerTimestamp();
			Listener.OnStatusChanged(StatusCode.Connect);
		}

		internal bool ExchangeKeysForEncryption()
		{
			isEncryptionAvailable = false;
			if (CryptoProvider != null)
			{
				((global::System.IDisposable)CryptoProvider).Dispose();
				CryptoProvider = null;
			}
			if (CryptoProvider == null)
			{
				CryptoProvider = new DiffieHellmanCryptoProvider();
			}
			Dictionary<byte, object> val = new Dictionary<byte, object>(1);
			val.set_Item(PhotonCodes.ClientKey, (object)CryptoProvider.PublicKey);
			return EnqueueOperation(val, PhotonCodes.InitEncryption, true, 0, false, EgMessageType.InternalOperationRequest);
		}

		internal void DeriveSharedKey(OperationResponse operationResponse)
		{
			if (operationResponse.ReturnCode != 0)
			{
				EnqueueDebugReturn(DebugLevel.ERROR, "Establishing encryption keys failed. " + operationResponse.ToStringFull());
				EnqueueStatusCallback(StatusCode.EncryptionFailedToEstablish);
				return;
			}
			byte[] array = (byte[])operationResponse[PhotonCodes.ServerKey];
			if (array == null || array.Length == 0)
			{
				EnqueueDebugReturn(DebugLevel.ERROR, "Establishing encryption keys failed. Server's public key is null or empty. " + operationResponse.ToStringFull());
				EnqueueStatusCallback(StatusCode.EncryptionFailedToEstablish);
			}
			else
			{
				CryptoProvider.DeriveSharedKey(array);
				isEncryptionAvailable = true;
				EnqueueStatusCallback(StatusCode.EncryptionEstablished);
			}
		}

		internal void EnqueueActionForDispatch(MyAction action)
		{
			lock (ActionQueue)
			{
				ActionQueue.Enqueue(action);
			}
		}

		internal void EnqueueDebugReturn(DebugLevel level, string debugReturn)
		{
			_003C_003Ec__DisplayClass136_0 _003C_003Ec__DisplayClass136_ = new _003C_003Ec__DisplayClass136_0();
			_003C_003Ec__DisplayClass136_._003C_003E4__this = this;
			_003C_003Ec__DisplayClass136_.level = level;
			_003C_003Ec__DisplayClass136_.debugReturn = debugReturn;
			lock (ActionQueue)
			{
				ActionQueue.Enqueue((MyAction)_003C_003Ec__DisplayClass136_._003CEnqueueDebugReturn_003Eb__0);
			}
		}

		internal void EnqueueStatusCallback(StatusCode statusValue)
		{
			_003C_003Ec__DisplayClass137_0 _003C_003Ec__DisplayClass137_ = new _003C_003Ec__DisplayClass137_0();
			_003C_003Ec__DisplayClass137_._003C_003E4__this = this;
			_003C_003Ec__DisplayClass137_.statusValue = statusValue;
			lock (ActionQueue)
			{
				ActionQueue.Enqueue((MyAction)_003C_003Ec__DisplayClass137_._003CEnqueueStatusCallback_003Eb__0);
			}
		}

		internal virtual void InitPeerBase()
		{
			TrafficStatsIncoming = new TrafficStats(TrafficPackageHeaderSize);
			TrafficStatsOutgoing = new TrafficStats(TrafficPackageHeaderSize);
			TrafficStatsGameLevel = new TrafficStatsGameLevel();
			ByteCountLastOperation = 0;
			ByteCountCurrentDispatch = 0;
			bytesIn = 0L;
			bytesOut = 0L;
			packetLossByCrc = 0;
			packetLossByChallenge = 0;
			networkSimulationSettings.LostPackagesIn = 0;
			networkSimulationSettings.LostPackagesOut = 0;
			lock (NetSimListOutgoing)
			{
				NetSimListOutgoing.Clear();
			}
			lock (NetSimListIncoming)
			{
				NetSimListIncoming.Clear();
			}
			peerConnectionState = ConnectionStateValue.Disconnected;
			timeBase = SupportClass.GetTickCount();
			isEncryptionAvailable = false;
			ApplicationIsInitialized = false;
			roundTripTime = 300;
			roundTripTimeVariance = 0;
			packetThrottleInterval = 5000;
			serverTimeOffsetIsAvailable = false;
			serverTimeOffset = 0;
		}

		internal virtual bool DeserializeMessageAndCallback(byte[] inBuff)
		{
			if (inBuff.Length < 2)
			{
				if ((int)debugOut >= 1)
				{
					Listener.DebugReturn(DebugLevel.ERROR, string.Concat((object)"Incoming UDP data too short! ", (object)inBuff.Length));
				}
				return false;
			}
			if (inBuff[0] != 243 && inBuff[0] != 253)
			{
				if ((int)debugOut >= 1)
				{
					Listener.DebugReturn(DebugLevel.ALL, string.Concat((object)"No regular operation UDP message: ", (object)inBuff[0]));
				}
				return false;
			}
			byte b = (byte)(inBuff[1] & 0x7Fu);
			bool flag = (inBuff[1] & 0x80) > 0;
			StreamBuffer streamBuffer = null;
			if (b != 1)
			{
				try
				{
					if (flag)
					{
						inBuff = CryptoProvider.Decrypt(inBuff, 2, inBuff.Length - 2);
						streamBuffer = new StreamBuffer(inBuff);
					}
					else
					{
						streamBuffer = new StreamBuffer(inBuff);
						((Stream)streamBuffer).Seek(2L, (SeekOrigin)0);
					}
				}
				catch (global::System.Exception ex)
				{
					if ((int)debugOut >= 1)
					{
						Listener.DebugReturn(DebugLevel.ERROR, ((object)ex).ToString());
					}
					SupportClass.WriteStackTrace(ex);
					return false;
				}
			}
			int num = 0;
			switch (b)
			{
			case 3:
			{
				OperationResponse operationResponse = protocol.DeserializeOperationResponse(streamBuffer);
				if (TrafficStatsEnabled)
				{
					TrafficStatsGameLevel.CountResult(ByteCountCurrentDispatch);
					num = SupportClass.GetTickCount();
				}
				Listener.OnOperationResponse(operationResponse);
				if (TrafficStatsEnabled)
				{
					TrafficStatsGameLevel.TimeForResponseCallback(operationResponse.OperationCode, SupportClass.GetTickCount() - num);
				}
				break;
			}
			case 4:
			{
				EventData eventData = protocol.DeserializeEventData(streamBuffer);
				if (TrafficStatsEnabled)
				{
					TrafficStatsGameLevel.CountEvent(ByteCountCurrentDispatch);
					num = SupportClass.GetTickCount();
				}
				Listener.OnEvent(eventData);
				if (TrafficStatsEnabled)
				{
					TrafficStatsGameLevel.TimeForEventCallback(eventData.Code, SupportClass.GetTickCount() - num);
				}
				break;
			}
			case 1:
				InitCallback();
				break;
			case 7:
			{
				OperationResponse operationResponse = protocol.DeserializeOperationResponse(streamBuffer);
				if (TrafficStatsEnabled)
				{
					TrafficStatsGameLevel.CountResult(ByteCountCurrentDispatch);
					num = SupportClass.GetTickCount();
				}
				if (operationResponse.OperationCode == PhotonCodes.InitEncryption)
				{
					DeriveSharedKey(operationResponse);
				}
				else if (operationResponse.OperationCode == PhotonCodes.Ping)
				{
					TPeer tPeer = this as TPeer;
					if (tPeer != null)
					{
						tPeer.ReadPingResult(operationResponse);
					}
					else
					{
						EnqueueDebugReturn(DebugLevel.ERROR, "Ping response not used. " + operationResponse.ToStringFull());
					}
				}
				else
				{
					EnqueueDebugReturn(DebugLevel.ERROR, "Received unknown internal operation. " + operationResponse.ToStringFull());
				}
				if (TrafficStatsEnabled)
				{
					TrafficStatsGameLevel.TimeForResponseCallback(operationResponse.OperationCode, SupportClass.GetTickCount() - num);
				}
				break;
			}
			default:
				EnqueueDebugReturn(DebugLevel.ERROR, string.Concat((object)"unexpected msgType ", (object)b));
				break;
			}
			return true;
		}

		internal void SendNetworkSimulated(MyAction sendAction)
		{
			if (!NetworkSimulationSettings.IsSimulationEnabled)
			{
				sendAction();
				return;
			}
			if (usedProtocol == ConnectionProtocol.Udp && NetworkSimulationSettings.OutgoingLossPercentage > 0 && lagRandomizer.Next(101) < NetworkSimulationSettings.OutgoingLossPercentage)
			{
				networkSimulationSettings.LostPackagesOut++;
				return;
			}
			int num = ((networkSimulationSettings.OutgoingJitter > 0) ? (lagRandomizer.Next(networkSimulationSettings.OutgoingJitter * 2) - networkSimulationSettings.OutgoingJitter) : 0);
			int num2 = networkSimulationSettings.OutgoingLag + num;
			int num3 = SupportClass.GetTickCount() + num2;
			SimulationItem simulationItem = new SimulationItem
			{
				ActionToExecute = sendAction,
				TimeToExecute = num3,
				Delay = num2
			};
			lock (NetSimListOutgoing)
			{
				if (NetSimListOutgoing.get_Count() == 0 || usedProtocol == ConnectionProtocol.Tcp)
				{
					NetSimListOutgoing.AddLast(simulationItem);
					return;
				}
				LinkedListNode<SimulationItem> val = NetSimListOutgoing.get_First();
				while (val != null && val.get_Value().TimeToExecute < num3)
				{
					val = val.get_Next();
				}
				if (val == null)
				{
					NetSimListOutgoing.AddLast(simulationItem);
				}
				else
				{
					NetSimListOutgoing.AddBefore(val, simulationItem);
				}
			}
		}

		internal void ReceiveNetworkSimulated(MyAction receiveAction)
		{
			if (!networkSimulationSettings.IsSimulationEnabled)
			{
				receiveAction();
				return;
			}
			if (usedProtocol == ConnectionProtocol.Udp && networkSimulationSettings.IncomingLossPercentage > 0 && lagRandomizer.Next(101) < networkSimulationSettings.IncomingLossPercentage)
			{
				networkSimulationSettings.LostPackagesIn++;
				return;
			}
			int num = ((networkSimulationSettings.IncomingJitter > 0) ? (lagRandomizer.Next(networkSimulationSettings.IncomingJitter * 2) - networkSimulationSettings.IncomingJitter) : 0);
			int num2 = networkSimulationSettings.IncomingLag + num;
			int num3 = SupportClass.GetTickCount() + num2;
			SimulationItem simulationItem = new SimulationItem
			{
				ActionToExecute = receiveAction,
				TimeToExecute = num3,
				Delay = num2
			};
			lock (NetSimListIncoming)
			{
				if (NetSimListIncoming.get_Count() == 0 || usedProtocol == ConnectionProtocol.Tcp)
				{
					NetSimListIncoming.AddLast(simulationItem);
					return;
				}
				LinkedListNode<SimulationItem> val = NetSimListIncoming.get_First();
				while (val != null && val.get_Value().TimeToExecute < num3)
				{
					val = val.get_Next();
				}
				if (val == null)
				{
					NetSimListIncoming.AddLast(simulationItem);
				}
				else
				{
					NetSimListIncoming.AddBefore(val, simulationItem);
				}
			}
		}

		protected internal void NetworkSimRun()
		{
			while (true)
			{
				bool flag = false;
				lock (networkSimulationSettings.NetSimManualResetEvent)
				{
					flag = networkSimulationSettings.IsSimulationEnabled;
				}
				if (!flag)
				{
					((WaitHandle)networkSimulationSettings.NetSimManualResetEvent).WaitOne();
					continue;
				}
				lock (NetSimListIncoming)
				{
					SimulationItem simulationItem = null;
					while (NetSimListIncoming.get_First() != null)
					{
						simulationItem = NetSimListIncoming.get_First().get_Value();
						if (simulationItem.stopw.get_ElapsedMilliseconds() < simulationItem.Delay)
						{
							break;
						}
						simulationItem.ActionToExecute();
						NetSimListIncoming.RemoveFirst();
					}
				}
				lock (NetSimListOutgoing)
				{
					SimulationItem simulationItem2 = null;
					while (NetSimListOutgoing.get_First() != null)
					{
						simulationItem2 = NetSimListOutgoing.get_First().get_Value();
						if (simulationItem2.stopw.get_ElapsedMilliseconds() < simulationItem2.Delay)
						{
							break;
						}
						simulationItem2.ActionToExecute();
						NetSimListOutgoing.RemoveFirst();
					}
				}
				Thread.Sleep(0);
			}
		}

		internal void UpdateRoundTripTimeAndVariance(int lastRoundtripTime)
		{
			if (lastRoundtripTime >= 0)
			{
				roundTripTimeVariance -= roundTripTimeVariance / 4;
				if (lastRoundtripTime >= roundTripTime)
				{
					roundTripTime += (lastRoundtripTime - roundTripTime) / 8;
					roundTripTimeVariance += (lastRoundtripTime - roundTripTime) / 4;
				}
				else
				{
					roundTripTime += (lastRoundtripTime - roundTripTime) / 8;
					roundTripTimeVariance -= (lastRoundtripTime - roundTripTime) / 4;
				}
				if (roundTripTime < lowestRoundTripTime)
				{
					lowestRoundTripTime = roundTripTime;
				}
				if (roundTripTimeVariance > highestRoundTripTimeVariance)
				{
					highestRoundTripTimeVariance = roundTripTimeVariance;
				}
			}
		}

		internal void InitializeTrafficStats()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			TrafficStatsIncoming = new TrafficStats(TrafficPackageHeaderSize);
			TrafficStatsOutgoing = new TrafficStats(TrafficPackageHeaderSize);
			TrafficStatsGameLevel = new TrafficStatsGameLevel();
			trafficStatsStopwatch = new Stopwatch();
		}
	}
}
