using System.Runtime.CompilerServices;

namespace ExitGames.Client.Photon
{
	public class TrafficStats
	{
		public int PackageHeaderSize
		{
			[CompilerGenerated]
			get
			{
				return _003CPackageHeaderSize_003Ek__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				_003CPackageHeaderSize_003Ek__BackingField = value;
			}
		}

		public int ReliableCommandCount
		{
			[CompilerGenerated]
			get
			{
				return _003CReliableCommandCount_003Ek__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				_003CReliableCommandCount_003Ek__BackingField = value;
			}
		}

		public int UnreliableCommandCount
		{
			[CompilerGenerated]
			get
			{
				return _003CUnreliableCommandCount_003Ek__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				_003CUnreliableCommandCount_003Ek__BackingField = value;
			}
		}

		public int FragmentCommandCount
		{
			[CompilerGenerated]
			get
			{
				return _003CFragmentCommandCount_003Ek__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				_003CFragmentCommandCount_003Ek__BackingField = value;
			}
		}

		public int ControlCommandCount
		{
			[CompilerGenerated]
			get
			{
				return _003CControlCommandCount_003Ek__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				_003CControlCommandCount_003Ek__BackingField = value;
			}
		}

		public int TotalPacketCount
		{
			[CompilerGenerated]
			get
			{
				return _003CTotalPacketCount_003Ek__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				_003CTotalPacketCount_003Ek__BackingField = value;
			}
		}

		public int TotalCommandsInPackets
		{
			[CompilerGenerated]
			get
			{
				return _003CTotalCommandsInPackets_003Ek__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				_003CTotalCommandsInPackets_003Ek__BackingField = value;
			}
		}

		public int ReliableCommandBytes
		{
			[CompilerGenerated]
			get
			{
				return _003CReliableCommandBytes_003Ek__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				_003CReliableCommandBytes_003Ek__BackingField = value;
			}
		}

		public int UnreliableCommandBytes
		{
			[CompilerGenerated]
			get
			{
				return _003CUnreliableCommandBytes_003Ek__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				_003CUnreliableCommandBytes_003Ek__BackingField = value;
			}
		}

		public int FragmentCommandBytes
		{
			[CompilerGenerated]
			get
			{
				return _003CFragmentCommandBytes_003Ek__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				_003CFragmentCommandBytes_003Ek__BackingField = value;
			}
		}

		public int ControlCommandBytes
		{
			[CompilerGenerated]
			get
			{
				return _003CControlCommandBytes_003Ek__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				_003CControlCommandBytes_003Ek__BackingField = value;
			}
		}

		public int TotalCommandCount
		{
			get
			{
				return ReliableCommandCount + UnreliableCommandCount + FragmentCommandCount + ControlCommandCount;
			}
		}

		public int TotalCommandBytes
		{
			get
			{
				return ReliableCommandBytes + UnreliableCommandBytes + FragmentCommandBytes + ControlCommandBytes;
			}
		}

		public int TotalPacketBytes
		{
			get
			{
				return TotalCommandBytes + TotalPacketCount * PackageHeaderSize;
			}
		}

		public int TimestampOfLastAck
		{
			[CompilerGenerated]
			get
			{
				return _003CTimestampOfLastAck_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CTimestampOfLastAck_003Ek__BackingField = value;
			}
		}

		public int TimestampOfLastReliableCommand
		{
			[CompilerGenerated]
			get
			{
				return _003CTimestampOfLastReliableCommand_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CTimestampOfLastReliableCommand_003Ek__BackingField = value;
			}
		}

		internal TrafficStats(int packageHeaderSize)
		{
			PackageHeaderSize = packageHeaderSize;
		}

		internal void CountControlCommand(int size)
		{
			ControlCommandBytes += size;
			ControlCommandCount++;
		}

		internal void CountReliableOpCommand(int size)
		{
			ReliableCommandBytes += size;
			ReliableCommandCount++;
		}

		internal void CountUnreliableOpCommand(int size)
		{
			UnreliableCommandBytes += size;
			UnreliableCommandCount++;
		}

		internal void CountFragmentOpCommand(int size)
		{
			FragmentCommandBytes += size;
			FragmentCommandCount++;
		}

		public override string ToString()
		{
			return string.Format("TotalPacketBytes: {0} TotalCommandBytes: {1} TotalPacketCount: {2} TotalCommandsInPackets: {3}", new object[4] { TotalPacketBytes, TotalCommandBytes, TotalPacketCount, TotalCommandsInPackets });
		}
	}
}
