using System;
using System.Runtime.CompilerServices;

namespace RilisoftBot
{
	public class BotDebuff
	{
		private BotDebuff.OnRunDelegate OnRun;

		private BotDebuff.OnStopDelegate OnStop;

		public bool isRun
		{
			get;
			private set;
		}

		public object parametrs
		{
			get;
			private set;
		}

		public float timeLife
		{
			get;
			set;
		}

		public BotDebuffType type
		{
			get;
			private set;
		}

		public BotDebuff(BotDebuffType typeDebuff, float timeLifeDebuff, object parametrsDebuff)
		{
			this.type = typeDebuff;
			this.timeLife = timeLifeDebuff;
			this.parametrs = parametrsDebuff;
			this.isRun = false;
		}

		public float GetFloatParametr()
		{
			if (this.parametrs == null)
			{
				return 0f;
			}
			return (float)this.parametrs;
		}

		public void ReplaceValues(float newTimeLife, object newParametrs)
		{
			this.timeLife = newTimeLife;
			this.parametrs = newParametrs;
		}

		public void Run()
		{
			if (this.OnRun == null)
			{
				return;
			}
			this.isRun = true;
			this.OnRun(this);
		}

		public void Stop()
		{
			if (this.OnStop == null)
			{
				return;
			}
			this.isRun = false;
			this.OnStop(this);
		}

		public event BotDebuff.OnRunDelegate OnRun
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.OnRun += value;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.OnRun -= value;
			}
		}

		public event BotDebuff.OnStopDelegate OnStop
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.OnStop += value;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.OnStop -= value;
			}
		}

		public delegate void OnRunDelegate(BotDebuff debuff);

		public delegate void OnStopDelegate(BotDebuff debuff);
	}
}