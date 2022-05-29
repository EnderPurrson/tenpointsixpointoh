using Rilisoft.NullExtensions;
using System;
using System.Runtime.CompilerServices;

namespace Rilisoft
{
	public class QuestEvents
	{
		private EventHandler<WinEventArgs> Win;

		private EventHandler<KillOtherPlayerEventArgs> KillOtherPlayer;

		private EventHandler KillOtherPlayerWithFlag;

		private EventHandler<CaptureEventArgs> Capture;

		private EventHandler<KillMonsterEventArgs> KillMonster;

		private EventHandler BreakSeries;

		private EventHandler MakeSeries;

		private EventHandler SurviveWaveInArena;

		private EventHandler GetGotcha;

		private EventHandler<SocialInteractionEventArgs> SocialInteraction;

		public QuestEvents()
		{
		}

		protected void RaiseBreakSeries(EventArgs e)
		{
			this.BreakSeries.Do<EventHandler>((EventHandler handler) => handler(this, e));
		}

		protected void RaiseCapture(CaptureEventArgs e)
		{
			this.Capture.Do<EventHandler<CaptureEventArgs>>((EventHandler<CaptureEventArgs> handler) => handler(this, e));
		}

		protected void RaiseGetGotcha(EventArgs e)
		{
			this.GetGotcha.Do<EventHandler>((EventHandler handler) => handler(this, e));
		}

		protected void RaiseKillMonster(KillMonsterEventArgs e)
		{
			this.KillMonster.Do<EventHandler<KillMonsterEventArgs>>((EventHandler<KillMonsterEventArgs> handler) => handler(this, e));
		}

		protected void RaiseKillOtherPlayer(KillOtherPlayerEventArgs e)
		{
			this.KillOtherPlayer.Do<EventHandler<KillOtherPlayerEventArgs>>((EventHandler<KillOtherPlayerEventArgs> handler) => handler(this, e));
		}

		protected void RaiseKillOtherPlayerWithFlag(EventArgs e)
		{
			this.KillOtherPlayerWithFlag.Do<EventHandler>((EventHandler handler) => handler(this, e));
		}

		protected void RaiseMakeSeries(EventArgs e)
		{
			this.MakeSeries.Do<EventHandler>((EventHandler handler) => handler(this, e));
		}

		protected void RaiseSocialInteraction(SocialInteractionEventArgs e)
		{
			this.SocialInteraction.Do<EventHandler<SocialInteractionEventArgs>>((EventHandler<SocialInteractionEventArgs> handler) => handler(this, e));
		}

		protected void RaiseSurviveWaveInArena(EventArgs e)
		{
			this.SurviveWaveInArena.Do<EventHandler>((EventHandler handler) => handler(this, e));
		}

		protected void RaiseWin(WinEventArgs e)
		{
			this.Win.Do<EventHandler<WinEventArgs>>((EventHandler<WinEventArgs> handler) => handler(this, e));
		}

		public event EventHandler BreakSeries
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.BreakSeries += value;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.BreakSeries -= value;
			}
		}

		public event EventHandler<CaptureEventArgs> Capture
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.Capture += value;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.Capture -= value;
			}
		}

		public event EventHandler GetGotcha
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.GetGotcha += value;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.GetGotcha -= value;
			}
		}

		public event EventHandler<KillMonsterEventArgs> KillMonster
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.KillMonster += value;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.KillMonster -= value;
			}
		}

		public event EventHandler<KillOtherPlayerEventArgs> KillOtherPlayer
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.KillOtherPlayer += value;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.KillOtherPlayer -= value;
			}
		}

		public event EventHandler KillOtherPlayerWithFlag
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.KillOtherPlayerWithFlag += value;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.KillOtherPlayerWithFlag -= value;
			}
		}

		public event EventHandler MakeSeries
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.MakeSeries += value;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.MakeSeries -= value;
			}
		}

		public event EventHandler<SocialInteractionEventArgs> SocialInteraction
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.SocialInteraction += value;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.SocialInteraction -= value;
			}
		}

		public event EventHandler SurviveWaveInArena
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.SurviveWaveInArena += value;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.SurviveWaveInArena -= value;
			}
		}

		public event EventHandler<WinEventArgs> Win
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.Win += value;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.Win -= value;
			}
		}
	}
}