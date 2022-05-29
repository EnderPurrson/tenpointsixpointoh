using GooglePlayGames.Native.Cwrapper;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeLeaderboard : BaseReferenceHolder
	{
		internal NativeLeaderboard(IntPtr selfPtr) : base(selfPtr)
		{
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			Leaderboard.Leaderboard_Dispose(selfPointer);
		}

		internal static NativeLeaderboard FromPointer(IntPtr pointer)
		{
			if (pointer.Equals(IntPtr.Zero))
			{
				return null;
			}
			return new NativeLeaderboard(pointer);
		}

		internal string Title()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => Leaderboard.Leaderboard_Name(base.SelfPtr(), out_string, out_size));
		}
	}
}