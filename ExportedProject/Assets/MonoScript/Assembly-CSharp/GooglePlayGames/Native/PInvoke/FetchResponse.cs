using GooglePlayGames.Native.Cwrapper;
using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.PInvoke
{
	internal class FetchResponse : BaseReferenceHolder
	{
		internal FetchResponse(IntPtr selfPointer) : base(selfPointer)
		{
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchResponse_Dispose(base.SelfPtr());
		}

		internal static FetchResponse FromPointer(IntPtr pointer)
		{
			if (pointer.Equals(IntPtr.Zero))
			{
				return null;
			}
			return new FetchResponse(pointer);
		}

		internal CommonErrorStatus.ResponseStatus GetStatus()
		{
			return GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchResponse_GetStatus(base.SelfPtr());
		}

		internal NativeLeaderboard Leaderboard()
		{
			return NativeLeaderboard.FromPointer(GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchResponse_GetData(base.SelfPtr()));
		}
	}
}