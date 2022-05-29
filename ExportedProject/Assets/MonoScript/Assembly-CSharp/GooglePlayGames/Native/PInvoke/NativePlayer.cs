using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.Native.Cwrapper;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativePlayer : BaseReferenceHolder
	{
		internal NativePlayer(IntPtr selfPointer) : base(selfPointer)
		{
		}

		internal GooglePlayGames.BasicApi.Multiplayer.Player AsPlayer()
		{
			return new GooglePlayGames.BasicApi.Multiplayer.Player(this.Name(), this.Id(), this.AvatarURL());
		}

		internal string AvatarURL()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => GooglePlayGames.Native.Cwrapper.Player.Player_AvatarUrl(base.SelfPtr(), Types.ImageResolution.ICON, out_string, out_size));
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			GooglePlayGames.Native.Cwrapper.Player.Player_Dispose(selfPointer);
		}

		internal string Id()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => GooglePlayGames.Native.Cwrapper.Player.Player_Id(base.SelfPtr(), out_string, out_size));
		}

		internal string Name()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => GooglePlayGames.Native.Cwrapper.Player.Player_Name(base.SelfPtr(), out_string, out_size));
		}
	}
}