using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativePlayer : BaseReferenceHolder
	{
		internal NativePlayer(IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		internal string Id()
		{
			return PInvokeUtilities.OutParamsToString(_003CId_003Em__141);
		}

		internal string Name()
		{
			return PInvokeUtilities.OutParamsToString(_003CName_003Em__142);
		}

		internal string AvatarURL()
		{
			return PInvokeUtilities.OutParamsToString(_003CAvatarURL_003Em__143);
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			GooglePlayGames.Native.Cwrapper.Player.Player_Dispose(selfPointer);
		}

		internal GooglePlayGames.BasicApi.Multiplayer.Player AsPlayer()
		{
			return new GooglePlayGames.BasicApi.Multiplayer.Player(Name(), Id(), AvatarURL());
		}

		[CompilerGenerated]
		private UIntPtr _003CId_003Em__141(StringBuilder out_string, UIntPtr out_size)
		{
			return GooglePlayGames.Native.Cwrapper.Player.Player_Id(SelfPtr(), out_string, out_size);
		}

		[CompilerGenerated]
		private UIntPtr _003CName_003Em__142(StringBuilder out_string, UIntPtr out_size)
		{
			return GooglePlayGames.Native.Cwrapper.Player.Player_Name(SelfPtr(), out_string, out_size);
		}

		[CompilerGenerated]
		private UIntPtr _003CAvatarURL_003Em__143(StringBuilder out_string, UIntPtr out_size)
		{
			return GooglePlayGames.Native.Cwrapper.Player.Player_AvatarUrl(SelfPtr(), Types.ImageResolution.ICON, out_string, out_size);
		}
	}
}
