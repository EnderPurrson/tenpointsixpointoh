using System;

namespace Rilisoft
{
	internal sealed class QuestMediator
	{
		private readonly static QuestMediator.QuestEventSource _eventSource;

		public static QuestEvents Events
		{
			get
			{
				return QuestMediator._eventSource;
			}
		}

		static QuestMediator()
		{
			QuestMediator._eventSource = new QuestMediator.QuestEventSource();
		}

		public QuestMediator()
		{
		}

		public static void NotifyBreakSeries()
		{
			QuestMediator._eventSource.RaiseBreakSeries(EventArgs.Empty);
		}

		public static void NotifyCapture(ConnectSceneNGUIController.RegimGame mode)
		{
			CaptureEventArgs captureEventArg = new CaptureEventArgs()
			{
				Mode = mode
			};
			QuestMediator._eventSource.RaiseCapture(captureEventArg);
		}

		public static void NotifyGetGotcha()
		{
			QuestMediator._eventSource.RaiseGetGotcha(EventArgs.Empty);
		}

		public static void NotifyKillMonster(ShopNGUIController.CategoryNames weaponSlot, bool campaign = false)
		{
			KillMonsterEventArgs killMonsterEventArg = new KillMonsterEventArgs()
			{
				WeaponSlot = weaponSlot,
				Campaign = campaign
			};
			QuestMediator._eventSource.RaiseKillMonster(killMonsterEventArg);
		}

		public static void NotifyKillOtherPlayer(ConnectSceneNGUIController.RegimGame mode, ShopNGUIController.CategoryNames weaponSlot, bool headshot = false, bool grenade = false, bool revenge = false)
		{
			KillOtherPlayerEventArgs killOtherPlayerEventArg = new KillOtherPlayerEventArgs()
			{
				Mode = mode,
				WeaponSlot = weaponSlot,
				Headshot = headshot,
				Grenade = grenade,
				Revenge = revenge
			};
			QuestMediator._eventSource.RaiseKillOtherPlayer(killOtherPlayerEventArg);
		}

		public static void NotifyKillOtherPlayerWithFlag()
		{
			QuestMediator._eventSource.RaiseKillOtherPlayerWithFlag(EventArgs.Empty);
		}

		public static void NotifyMakeSeries()
		{
			QuestMediator._eventSource.RaiseMakeSeries(EventArgs.Empty);
		}

		public static void NotifySocialInteraction(string kind)
		{
			SocialInteractionEventArgs socialInteractionEventArg = new SocialInteractionEventArgs()
			{
				Kind = kind ?? string.Empty
			};
			QuestMediator._eventSource.RaiseSocialInteraction(socialInteractionEventArg);
		}

		public static void NotifySurviveWaveInArena()
		{
			QuestMediator._eventSource.RaiseSurviveWaveInArena(EventArgs.Empty);
		}

		public static void NotifyWin(ConnectSceneNGUIController.RegimGame mode, string map)
		{
			WinEventArgs winEventArg = new WinEventArgs()
			{
				Mode = mode,
				Map = map ?? string.Empty
			};
			QuestMediator._eventSource.RaiseWin(winEventArg);
		}

		private sealed class QuestEventSource : QuestEvents
		{
			public QuestEventSource()
			{
			}

			internal new void RaiseBreakSeries(EventArgs e)
			{
				base.RaiseBreakSeries(e);
			}

			internal new void RaiseCapture(CaptureEventArgs e)
			{
				base.RaiseCapture(e);
			}

			internal new void RaiseGetGotcha(EventArgs e)
			{
				base.RaiseGetGotcha(e);
			}

			internal new void RaiseKillMonster(KillMonsterEventArgs e)
			{
				base.RaiseKillMonster(e);
			}

			internal new void RaiseKillOtherPlayer(KillOtherPlayerEventArgs e)
			{
				base.RaiseKillOtherPlayer(e);
			}

			internal new void RaiseKillOtherPlayerWithFlag(EventArgs e)
			{
				base.RaiseKillOtherPlayerWithFlag(e);
			}

			internal new void RaiseMakeSeries(EventArgs e)
			{
				base.RaiseMakeSeries(e);
			}

			internal new void RaiseSocialInteraction(SocialInteractionEventArgs e)
			{
				base.RaiseSocialInteraction(e);
			}

			internal new void RaiseSurviveWaveInArena(EventArgs e)
			{
				base.RaiseSurviveWaveInArena(e);
			}

			internal new void RaiseWin(WinEventArgs e)
			{
				base.RaiseWin(e);
			}
		}
	}
}