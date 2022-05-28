using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ExitGames.Client.Photon
{
	internal class SimulationItem
	{
		internal readonly Stopwatch stopw;

		public int TimeToExecute;

		public PeerBase.MyAction ActionToExecute;

		public int Delay
		{
			[CompilerGenerated]
			get
			{
				return _003CDelay_003Ek__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				_003CDelay_003Ek__BackingField = value;
			}
		}

		public SimulationItem()
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			stopw = new Stopwatch();
			stopw.Start();
		}
	}
}
