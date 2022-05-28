using System;
using System.Runtime.CompilerServices;

namespace ExitGames.Client.Photon
{
	public class TrafficStatsGameLevel
	{
		private int timeOfLastDispatchCall;

		private int timeOfLastSendCall;

		public int OperationByteCount
		{
			[CompilerGenerated]
			get
			{
				return _003COperationByteCount_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003COperationByteCount_003Ek__BackingField = value;
			}
		}

		public int OperationCount
		{
			[CompilerGenerated]
			get
			{
				return _003COperationCount_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003COperationCount_003Ek__BackingField = value;
			}
		}

		public int ResultByteCount
		{
			[CompilerGenerated]
			get
			{
				return _003CResultByteCount_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CResultByteCount_003Ek__BackingField = value;
			}
		}

		public int ResultCount
		{
			[CompilerGenerated]
			get
			{
				return _003CResultCount_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CResultCount_003Ek__BackingField = value;
			}
		}

		public int EventByteCount
		{
			[CompilerGenerated]
			get
			{
				return _003CEventByteCount_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CEventByteCount_003Ek__BackingField = value;
			}
		}

		public int EventCount
		{
			[CompilerGenerated]
			get
			{
				return _003CEventCount_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CEventCount_003Ek__BackingField = value;
			}
		}

		public int LongestOpResponseCallback
		{
			[CompilerGenerated]
			get
			{
				return _003CLongestOpResponseCallback_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CLongestOpResponseCallback_003Ek__BackingField = value;
			}
		}

		public byte LongestOpResponseCallbackOpCode
		{
			[CompilerGenerated]
			get
			{
				return _003CLongestOpResponseCallbackOpCode_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CLongestOpResponseCallbackOpCode_003Ek__BackingField = value;
			}
		}

		public int LongestEventCallback
		{
			[CompilerGenerated]
			get
			{
				return _003CLongestEventCallback_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CLongestEventCallback_003Ek__BackingField = value;
			}
		}

		public byte LongestEventCallbackCode
		{
			[CompilerGenerated]
			get
			{
				return _003CLongestEventCallbackCode_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CLongestEventCallbackCode_003Ek__BackingField = value;
			}
		}

		public int LongestDeltaBetweenDispatching
		{
			[CompilerGenerated]
			get
			{
				return _003CLongestDeltaBetweenDispatching_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CLongestDeltaBetweenDispatching_003Ek__BackingField = value;
			}
		}

		public int LongestDeltaBetweenSending
		{
			[CompilerGenerated]
			get
			{
				return _003CLongestDeltaBetweenSending_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CLongestDeltaBetweenSending_003Ek__BackingField = value;
			}
		}

		[Obsolete("Use DispatchIncomingCommandsCalls, which has proper naming.")]
		public int DispatchCalls
		{
			get
			{
				return DispatchIncomingCommandsCalls;
			}
		}

		public int DispatchIncomingCommandsCalls
		{
			[CompilerGenerated]
			get
			{
				return _003CDispatchIncomingCommandsCalls_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CDispatchIncomingCommandsCalls_003Ek__BackingField = value;
			}
		}

		public int SendOutgoingCommandsCalls
		{
			[CompilerGenerated]
			get
			{
				return _003CSendOutgoingCommandsCalls_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CSendOutgoingCommandsCalls_003Ek__BackingField = value;
			}
		}

		public int TotalByteCount
		{
			get
			{
				return OperationByteCount + ResultByteCount + EventByteCount;
			}
		}

		public int TotalMessageCount
		{
			get
			{
				return OperationCount + ResultCount + EventCount;
			}
		}

		public int TotalIncomingByteCount
		{
			get
			{
				return ResultByteCount + EventByteCount;
			}
		}

		public int TotalIncomingMessageCount
		{
			get
			{
				return ResultCount + EventCount;
			}
		}

		public int TotalOutgoingByteCount
		{
			get
			{
				return OperationByteCount;
			}
		}

		public int TotalOutgoingMessageCount
		{
			get
			{
				return OperationCount;
			}
		}

		internal void CountOperation(int operationBytes)
		{
			OperationByteCount += operationBytes;
			OperationCount++;
		}

		internal void CountResult(int resultBytes)
		{
			ResultByteCount += resultBytes;
			ResultCount++;
		}

		internal void CountEvent(int eventBytes)
		{
			EventByteCount += eventBytes;
			EventCount++;
		}

		internal void TimeForResponseCallback(byte code, int time)
		{
			if (time > LongestOpResponseCallback)
			{
				LongestOpResponseCallback = time;
				LongestOpResponseCallbackOpCode = code;
			}
		}

		internal void TimeForEventCallback(byte code, int time)
		{
			if (time > LongestEventCallback)
			{
				LongestEventCallback = time;
				LongestEventCallbackCode = code;
			}
		}

		internal void DispatchIncomingCommandsCalled()
		{
			if (timeOfLastDispatchCall != 0)
			{
				int num = SupportClass.GetTickCount() - timeOfLastDispatchCall;
				if (num > LongestDeltaBetweenDispatching)
				{
					LongestDeltaBetweenDispatching = num;
				}
			}
			DispatchIncomingCommandsCalls++;
			timeOfLastDispatchCall = SupportClass.GetTickCount();
		}

		internal void SendOutgoingCommandsCalled()
		{
			if (timeOfLastSendCall != 0)
			{
				int num = SupportClass.GetTickCount() - timeOfLastSendCall;
				if (num > LongestDeltaBetweenSending)
				{
					LongestDeltaBetweenSending = num;
				}
			}
			SendOutgoingCommandsCalls++;
			timeOfLastSendCall = SupportClass.GetTickCount();
		}

		public void ResetMaximumCounters()
		{
			LongestDeltaBetweenDispatching = 0;
			LongestDeltaBetweenSending = 0;
			LongestEventCallback = 0;
			LongestEventCallbackCode = 0;
			LongestOpResponseCallback = 0;
			LongestOpResponseCallbackOpCode = 0;
			timeOfLastDispatchCall = 0;
			timeOfLastSendCall = 0;
		}

		public override string ToString()
		{
			return string.Format("OperationByteCount: {0} ResultByteCount: {1} EventByteCount: {2}", (object)OperationByteCount, (object)ResultByteCount, (object)EventByteCount);
		}

		public string ToStringVitalStats()
		{
			return string.Format("Longest delta between Send: {0}ms Dispatch: {1}ms. Longest callback OnEv: {3}={2}ms OnResp: {5}={4}ms. Calls of Send: {6} Dispatch: {7}.", new object[8] { LongestDeltaBetweenSending, LongestDeltaBetweenDispatching, LongestEventCallback, LongestEventCallbackCode, LongestOpResponseCallback, LongestOpResponseCallbackOpCode, SendOutgoingCommandsCalls, DispatchIncomingCommandsCalls });
		}
	}
}
