using System;
using System.Runtime.CompilerServices;

namespace ExitGames.Client.DemoParticle
{
	public class TimeKeeper
	{
		private int lastExecutionTime = Environment.TickCount;

		private bool shouldExecute;

		public int Interval
		{
			get;
			set;
		}

		public bool IsEnabled
		{
			get;
			set;
		}

		public bool ShouldExecute
		{
			get
			{
				bool flag;
				if (!this.IsEnabled)
				{
					flag = false;
				}
				else
				{
					flag = (this.shouldExecute ? true : Environment.TickCount - this.lastExecutionTime > this.Interval);
				}
				return flag;
			}
			set
			{
				this.shouldExecute = value;
			}
		}

		public TimeKeeper(int interval)
		{
			this.IsEnabled = true;
			this.Interval = interval;
		}

		public void Reset()
		{
			this.shouldExecute = false;
			this.lastExecutionTime = Environment.TickCount;
		}
	}
}