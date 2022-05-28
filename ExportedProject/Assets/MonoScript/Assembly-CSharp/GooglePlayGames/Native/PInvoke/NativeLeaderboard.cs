using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeLeaderboard : BaseReferenceHolder
	{
		internal NativeLeaderboard(IntPtr selfPtr)
			: base(selfPtr)
		{
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			Leaderboard.Leaderboard_Dispose(selfPointer);
		}

		internal string Title()
		{
			return PInvokeUtilities.OutParamsToString(_003CTitle_003Em__140);
		}

		internal static NativeLeaderboard FromPointer(IntPtr pointer)
		{
			if (pointer.Equals(IntPtr.Zero))
			{
				return null;
			}
			return new NativeLeaderboard(pointer);
		}

		[CompilerGenerated]
		private UIntPtr _003CTitle_003Em__140(StringBuilder out_string, UIntPtr out_size)
		{
			return Leaderboard.Leaderboard_Name(SelfPtr(), out_string, out_size);
		}
	}
}
