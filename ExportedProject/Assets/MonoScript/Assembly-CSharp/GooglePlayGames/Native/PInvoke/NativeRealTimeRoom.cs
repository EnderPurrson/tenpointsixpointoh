using GooglePlayGames.Native.Cwrapper;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeRealTimeRoom : BaseReferenceHolder
	{
		internal NativeRealTimeRoom(IntPtr selfPointer) : base(selfPointer)
		{
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			RealTimeRoom.RealTimeRoom_Dispose(selfPointer);
		}

		internal static NativeRealTimeRoom FromPointer(IntPtr selfPointer)
		{
			if (selfPointer.Equals(IntPtr.Zero))
			{
				return null;
			}
			return new NativeRealTimeRoom(selfPointer);
		}

		internal string Id()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr size) => RealTimeRoom.RealTimeRoom_Id(base.SelfPtr(), out_string, size));
		}

		internal uint ParticipantCount()
		{
			return RealTimeRoom.RealTimeRoom_Participants_Length(base.SelfPtr()).ToUInt32();
		}

		internal IEnumerable<GooglePlayGames.Native.PInvoke.MultiplayerParticipant> Participants()
		{
			return PInvokeUtilities.ToEnumerable<GooglePlayGames.Native.PInvoke.MultiplayerParticipant>(RealTimeRoom.RealTimeRoom_Participants_Length(base.SelfPtr()), (UIntPtr index) => new GooglePlayGames.Native.PInvoke.MultiplayerParticipant(RealTimeRoom.RealTimeRoom_Participants_GetElement(base.SelfPtr(), index)));
		}

		internal Types.RealTimeRoomStatus Status()
		{
			return RealTimeRoom.RealTimeRoom_Status(base.SelfPtr());
		}
	}
}