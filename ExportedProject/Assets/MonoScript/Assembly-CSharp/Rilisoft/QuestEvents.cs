using System;
using System.Runtime.CompilerServices;
using Rilisoft.NullExtensions;

namespace Rilisoft
{
	public class QuestEvents
	{
		[CompilerGenerated]
		private sealed class _003CRaiseWin_003Ec__AnonStorey2DC
		{
			internal WinEventArgs e;

			internal QuestEvents _003C_003Ef__this;

			internal void _003C_003Em__3BE(EventHandler<WinEventArgs> handler)
			{
				handler(_003C_003Ef__this, e);
			}
		}

		[CompilerGenerated]
		private sealed class _003CRaiseKillOtherPlayer_003Ec__AnonStorey2DD
		{
			internal KillOtherPlayerEventArgs e;

			internal QuestEvents _003C_003Ef__this;

			internal void _003C_003Em__3BF(EventHandler<KillOtherPlayerEventArgs> handler)
			{
				handler(_003C_003Ef__this, e);
			}
		}

		[CompilerGenerated]
		private sealed class _003CRaiseKillOtherPlayerWithFlag_003Ec__AnonStorey2DE
		{
			internal EventArgs e;

			internal QuestEvents _003C_003Ef__this;

			internal void _003C_003Em__3C0(EventHandler handler)
			{
				handler(_003C_003Ef__this, e);
			}
		}

		[CompilerGenerated]
		private sealed class _003CRaiseCapture_003Ec__AnonStorey2DF
		{
			internal CaptureEventArgs e;

			internal QuestEvents _003C_003Ef__this;

			internal void _003C_003Em__3C1(EventHandler<CaptureEventArgs> handler)
			{
				handler(_003C_003Ef__this, e);
			}
		}

		[CompilerGenerated]
		private sealed class _003CRaiseKillMonster_003Ec__AnonStorey2E0
		{
			internal KillMonsterEventArgs e;

			internal QuestEvents _003C_003Ef__this;

			internal void _003C_003Em__3C2(EventHandler<KillMonsterEventArgs> handler)
			{
				handler(_003C_003Ef__this, e);
			}
		}

		[CompilerGenerated]
		private sealed class _003CRaiseBreakSeries_003Ec__AnonStorey2E1
		{
			internal EventArgs e;

			internal QuestEvents _003C_003Ef__this;

			internal void _003C_003Em__3C3(EventHandler handler)
			{
				handler(_003C_003Ef__this, e);
			}
		}

		[CompilerGenerated]
		private sealed class _003CRaiseMakeSeries_003Ec__AnonStorey2E2
		{
			internal EventArgs e;

			internal QuestEvents _003C_003Ef__this;

			internal void _003C_003Em__3C4(EventHandler handler)
			{
				handler(_003C_003Ef__this, e);
			}
		}

		[CompilerGenerated]
		private sealed class _003CRaiseSurviveWaveInArena_003Ec__AnonStorey2E3
		{
			internal EventArgs e;

			internal QuestEvents _003C_003Ef__this;

			internal void _003C_003Em__3C5(EventHandler handler)
			{
				handler(_003C_003Ef__this, e);
			}
		}

		[CompilerGenerated]
		private sealed class _003CRaiseGetGotcha_003Ec__AnonStorey2E4
		{
			internal EventArgs e;

			internal QuestEvents _003C_003Ef__this;

			internal void _003C_003Em__3C6(EventHandler handler)
			{
				handler(_003C_003Ef__this, e);
			}
		}

		[CompilerGenerated]
		private sealed class _003CRaiseSocialInteraction_003Ec__AnonStorey2E5
		{
			internal SocialInteractionEventArgs e;

			internal QuestEvents _003C_003Ef__this;

			internal void _003C_003Em__3C7(EventHandler<SocialInteractionEventArgs> handler)
			{
				handler(_003C_003Ef__this, e);
			}
		}

		public event EventHandler<WinEventArgs> Win;

		public event EventHandler<KillOtherPlayerEventArgs> KillOtherPlayer;

		public event EventHandler KillOtherPlayerWithFlag;

		public event EventHandler<CaptureEventArgs> Capture;

		public event EventHandler<KillMonsterEventArgs> KillMonster;

		public event EventHandler BreakSeries;

		public event EventHandler MakeSeries;

		public event EventHandler SurviveWaveInArena;

		public event EventHandler GetGotcha;

		public event EventHandler<SocialInteractionEventArgs> SocialInteraction;

		protected void RaiseWin(WinEventArgs e)
		{
			_003CRaiseWin_003Ec__AnonStorey2DC _003CRaiseWin_003Ec__AnonStorey2DC = new _003CRaiseWin_003Ec__AnonStorey2DC();
			_003CRaiseWin_003Ec__AnonStorey2DC.e = e;
			_003CRaiseWin_003Ec__AnonStorey2DC._003C_003Ef__this = this;
			this.Win.Do(_003CRaiseWin_003Ec__AnonStorey2DC._003C_003Em__3BE);
		}

		protected void RaiseKillOtherPlayer(KillOtherPlayerEventArgs e)
		{
			_003CRaiseKillOtherPlayer_003Ec__AnonStorey2DD _003CRaiseKillOtherPlayer_003Ec__AnonStorey2DD = new _003CRaiseKillOtherPlayer_003Ec__AnonStorey2DD();
			_003CRaiseKillOtherPlayer_003Ec__AnonStorey2DD.e = e;
			_003CRaiseKillOtherPlayer_003Ec__AnonStorey2DD._003C_003Ef__this = this;
			this.KillOtherPlayer.Do(_003CRaiseKillOtherPlayer_003Ec__AnonStorey2DD._003C_003Em__3BF);
		}

		protected void RaiseKillOtherPlayerWithFlag(EventArgs e)
		{
			_003CRaiseKillOtherPlayerWithFlag_003Ec__AnonStorey2DE _003CRaiseKillOtherPlayerWithFlag_003Ec__AnonStorey2DE = new _003CRaiseKillOtherPlayerWithFlag_003Ec__AnonStorey2DE();
			_003CRaiseKillOtherPlayerWithFlag_003Ec__AnonStorey2DE.e = e;
			_003CRaiseKillOtherPlayerWithFlag_003Ec__AnonStorey2DE._003C_003Ef__this = this;
			this.KillOtherPlayerWithFlag.Do(_003CRaiseKillOtherPlayerWithFlag_003Ec__AnonStorey2DE._003C_003Em__3C0);
		}

		protected void RaiseCapture(CaptureEventArgs e)
		{
			_003CRaiseCapture_003Ec__AnonStorey2DF _003CRaiseCapture_003Ec__AnonStorey2DF = new _003CRaiseCapture_003Ec__AnonStorey2DF();
			_003CRaiseCapture_003Ec__AnonStorey2DF.e = e;
			_003CRaiseCapture_003Ec__AnonStorey2DF._003C_003Ef__this = this;
			this.Capture.Do(_003CRaiseCapture_003Ec__AnonStorey2DF._003C_003Em__3C1);
		}

		protected void RaiseKillMonster(KillMonsterEventArgs e)
		{
			_003CRaiseKillMonster_003Ec__AnonStorey2E0 _003CRaiseKillMonster_003Ec__AnonStorey2E = new _003CRaiseKillMonster_003Ec__AnonStorey2E0();
			_003CRaiseKillMonster_003Ec__AnonStorey2E.e = e;
			_003CRaiseKillMonster_003Ec__AnonStorey2E._003C_003Ef__this = this;
			this.KillMonster.Do(_003CRaiseKillMonster_003Ec__AnonStorey2E._003C_003Em__3C2);
		}

		protected void RaiseBreakSeries(EventArgs e)
		{
			_003CRaiseBreakSeries_003Ec__AnonStorey2E1 _003CRaiseBreakSeries_003Ec__AnonStorey2E = new _003CRaiseBreakSeries_003Ec__AnonStorey2E1();
			_003CRaiseBreakSeries_003Ec__AnonStorey2E.e = e;
			_003CRaiseBreakSeries_003Ec__AnonStorey2E._003C_003Ef__this = this;
			this.BreakSeries.Do(_003CRaiseBreakSeries_003Ec__AnonStorey2E._003C_003Em__3C3);
		}

		protected void RaiseMakeSeries(EventArgs e)
		{
			_003CRaiseMakeSeries_003Ec__AnonStorey2E2 _003CRaiseMakeSeries_003Ec__AnonStorey2E = new _003CRaiseMakeSeries_003Ec__AnonStorey2E2();
			_003CRaiseMakeSeries_003Ec__AnonStorey2E.e = e;
			_003CRaiseMakeSeries_003Ec__AnonStorey2E._003C_003Ef__this = this;
			this.MakeSeries.Do(_003CRaiseMakeSeries_003Ec__AnonStorey2E._003C_003Em__3C4);
		}

		protected void RaiseSurviveWaveInArena(EventArgs e)
		{
			_003CRaiseSurviveWaveInArena_003Ec__AnonStorey2E3 _003CRaiseSurviveWaveInArena_003Ec__AnonStorey2E = new _003CRaiseSurviveWaveInArena_003Ec__AnonStorey2E3();
			_003CRaiseSurviveWaveInArena_003Ec__AnonStorey2E.e = e;
			_003CRaiseSurviveWaveInArena_003Ec__AnonStorey2E._003C_003Ef__this = this;
			this.SurviveWaveInArena.Do(_003CRaiseSurviveWaveInArena_003Ec__AnonStorey2E._003C_003Em__3C5);
		}

		protected void RaiseGetGotcha(EventArgs e)
		{
			_003CRaiseGetGotcha_003Ec__AnonStorey2E4 _003CRaiseGetGotcha_003Ec__AnonStorey2E = new _003CRaiseGetGotcha_003Ec__AnonStorey2E4();
			_003CRaiseGetGotcha_003Ec__AnonStorey2E.e = e;
			_003CRaiseGetGotcha_003Ec__AnonStorey2E._003C_003Ef__this = this;
			this.GetGotcha.Do(_003CRaiseGetGotcha_003Ec__AnonStorey2E._003C_003Em__3C6);
		}

		protected void RaiseSocialInteraction(SocialInteractionEventArgs e)
		{
			_003CRaiseSocialInteraction_003Ec__AnonStorey2E5 _003CRaiseSocialInteraction_003Ec__AnonStorey2E = new _003CRaiseSocialInteraction_003Ec__AnonStorey2E5();
			_003CRaiseSocialInteraction_003Ec__AnonStorey2E.e = e;
			_003CRaiseSocialInteraction_003Ec__AnonStorey2E._003C_003Ef__this = this;
			this.SocialInteraction.Do(_003CRaiseSocialInteraction_003Ec__AnonStorey2E._003C_003Em__3C7);
		}
	}
}
