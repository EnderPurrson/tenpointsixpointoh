using GooglePlayGames;
using GooglePlayGames.Native.Cwrapper;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeScoreEntry : BaseReferenceHolder
	{
		private const ulong MinusOne = 18446744073709551615L;

		internal NativeScoreEntry(IntPtr selfPtr) : base(selfPtr)
		{
		}

		internal PlayGamesScore AsScore(string leaderboardId)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			ulong lastModifiedTime = this.GetLastModifiedTime();
			if (lastModifiedTime == (long)-1)
			{
				lastModifiedTime = (ulong)0;
			}
			DateTime dateTime1 = dateTime.AddMilliseconds((double)((float)lastModifiedTime));
			PlayGamesScore playGamesScore = new PlayGamesScore(dateTime1, leaderboardId, this.GetScore().GetRank(), this.GetPlayerId(), this.GetScore().GetValue(), this.GetScore().GetMetadata());
			return playGamesScore;
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			ScorePage.ScorePage_Entry_Dispose(selfPointer);
		}

		internal ulong GetLastModifiedTime()
		{
			return ScorePage.ScorePage_Entry_LastModifiedTime(base.SelfPtr());
		}

		internal string GetPlayerId()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => ScorePage.ScorePage_Entry_PlayerId(base.SelfPtr(), out_string, out_size));
		}

		internal NativeScore GetScore()
		{
			return new NativeScore(ScorePage.ScorePage_Entry_Score(base.SelfPtr()));
		}
	}
}