using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ExitGames.Client.Photon
{
	public class NetworkSimulationSet
	{
		private bool isSimulationEnabled = false;

		private int outgoingLag = 100;

		private int outgoingJitter = 0;

		private int outgoingLossPercentage = 1;

		private int incomingLag = 100;

		private int incomingJitter = 0;

		private int incomingLossPercentage = 1;

		internal PeerBase peerBase;

		private Thread netSimThread;

		public readonly ManualResetEvent NetSimManualResetEvent = new ManualResetEvent(false);

		protected internal bool IsSimulationEnabled
		{
			get
			{
				return isSimulationEnabled;
			}
			set
			{
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
				//IL_013e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0148: Expected O, but got Unknown
				//IL_0143: Unknown result type (might be due to invalid IL or missing references)
				//IL_014d: Expected O, but got Unknown
				lock (NetSimManualResetEvent)
				{
					if (!value)
					{
						lock (peerBase.NetSimListIncoming)
						{
							Enumerator<SimulationItem> enumerator = peerBase.NetSimListIncoming.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									SimulationItem current = enumerator.get_Current();
									current.ActionToExecute();
								}
							}
							finally
							{
								((global::System.IDisposable)enumerator).Dispose();
							}
							peerBase.NetSimListIncoming.Clear();
						}
						lock (peerBase.NetSimListOutgoing)
						{
							Enumerator<SimulationItem> enumerator2 = peerBase.NetSimListOutgoing.GetEnumerator();
							try
							{
								while (enumerator2.MoveNext())
								{
									SimulationItem current2 = enumerator2.get_Current();
									current2.ActionToExecute();
								}
							}
							finally
							{
								((global::System.IDisposable)enumerator2).Dispose();
							}
							peerBase.NetSimListOutgoing.Clear();
						}
					}
					isSimulationEnabled = value;
					if (isSimulationEnabled)
					{
						if (netSimThread == null)
						{
							netSimThread = new Thread(new ThreadStart(peerBase.NetworkSimRun));
							netSimThread.set_IsBackground(true);
							netSimThread.set_Name(string.Concat((object)"netSim", (object)SupportClass.GetTickCount()));
							netSimThread.Start();
						}
						((EventWaitHandle)NetSimManualResetEvent).Set();
					}
					else
					{
						((EventWaitHandle)NetSimManualResetEvent).Reset();
					}
				}
			}
		}

		public int OutgoingLag
		{
			get
			{
				return outgoingLag;
			}
			set
			{
				outgoingLag = value;
			}
		}

		public int OutgoingJitter
		{
			get
			{
				return outgoingJitter;
			}
			set
			{
				outgoingJitter = value;
			}
		}

		public int OutgoingLossPercentage
		{
			get
			{
				return outgoingLossPercentage;
			}
			set
			{
				outgoingLossPercentage = value;
			}
		}

		public int IncomingLag
		{
			get
			{
				return incomingLag;
			}
			set
			{
				incomingLag = value;
			}
		}

		public int IncomingJitter
		{
			get
			{
				return incomingJitter;
			}
			set
			{
				incomingJitter = value;
			}
		}

		public int IncomingLossPercentage
		{
			get
			{
				return incomingLossPercentage;
			}
			set
			{
				incomingLossPercentage = value;
			}
		}

		public int LostPackagesOut
		{
			[CompilerGenerated]
			get
			{
				return _003CLostPackagesOut_003Ek__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				_003CLostPackagesOut_003Ek__BackingField = value;
			}
		}

		public int LostPackagesIn
		{
			[CompilerGenerated]
			get
			{
				return _003CLostPackagesIn_003Ek__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				_003CLostPackagesIn_003Ek__BackingField = value;
			}
		}

		public override string ToString()
		{
			return string.Format("NetworkSimulationSet {6}.  Lag in={0} out={1}. Jitter in={2} out={3}. Loss in={4} out={5}.", new object[7] { incomingLag, outgoingLag, incomingJitter, outgoingJitter, incomingLossPercentage, outgoingLossPercentage, IsSimulationEnabled });
		}
	}
}
