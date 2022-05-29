using GooglePlayGames;
using GooglePlayGames.Native.Cwrapper;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeScore : BaseReferenceHolder
	{
		private const ulong MinusOne = 18446744073709551615L;

		internal NativeScore(IntPtr selfPtr) : base(selfPtr)
		{
		}

		internal PlayGamesScore AsScore(string leaderboardId, string selfPlayerId)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			ulong date = this.GetDate();
			if (date == (long)-1)
			{
				date = (ulong)0;
			}
			DateTime dateTime1 = dateTime.AddMilliseconds((double)((float)date));
			PlayGamesScore playGamesScore = new PlayGamesScore(dateTime1, leaderboardId, this.GetRank(), selfPlayerId, this.GetValue(), this.GetMetadata());
			return playGamesScore;
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			Score.Score_Dispose(base.SelfPtr());
		}

		internal static NativeScore FromPointer(IntPtr pointer)
		{
			if (pointer.Equals(IntPtr.Zero))
			{
				return null;
			}
			return new NativeScore(pointer);
		}

		internal ulong GetDate()
		{
			return (ulong)-1;
		}

		internal string GetMetadata()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => Score.Score_Metadata(base.SelfPtr(), out_string, out_size));
		}

		internal ulong GetRank()
		{
			return Score.Score_Rank(base.SelfPtr());
		}

		internal ulong GetValue()
		{
			return Score.Score_Value(base.SelfPtr());
		}
	}
}