using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.Cwrapper
{
	internal static class GameServices
	{
		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void GameServices_Dispose(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void GameServices_FetchServerAuthCode(HandleRef self, string server_client_id, GameServices.FetchServerAuthCodeCallback callback, IntPtr callback_arg);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void GameServices_FetchServerAuthCodeResponse_Dispose(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern UIntPtr GameServices_FetchServerAuthCodeResponse_GetCode(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern CommonErrorStatus.ResponseStatus GameServices_FetchServerAuthCodeResponse_GetStatus(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void GameServices_Flush(HandleRef self, GameServices.FlushCallback callback, IntPtr callback_arg);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern bool GameServices_IsAuthorized(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void GameServices_SignOut(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void GameServices_StartAuthorizationUI(HandleRef self);

		internal delegate void FetchServerAuthCodeCallback(IntPtr arg0, IntPtr arg1);

		internal delegate void FlushCallback(CommonErrorStatus.FlushStatus arg0, IntPtr arg1);
	}
}