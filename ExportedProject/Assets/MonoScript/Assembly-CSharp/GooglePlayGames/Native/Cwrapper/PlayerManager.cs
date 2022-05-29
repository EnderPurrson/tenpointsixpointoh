using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	internal static class PlayerManager
	{
		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void PlayerManager_Fetch(HandleRef self, Types.DataSource data_source, string player_id, PlayerManager.FetchCallback callback, IntPtr callback_arg);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void PlayerManager_FetchConnected(HandleRef self, Types.DataSource data_source, PlayerManager.FetchListCallback callback, IntPtr callback_arg);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void PlayerManager_FetchInvitable(HandleRef self, Types.DataSource data_source, PlayerManager.FetchListCallback callback, IntPtr callback_arg);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void PlayerManager_FetchListResponse_Dispose(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern IntPtr PlayerManager_FetchListResponse_GetData_GetElement(HandleRef self, UIntPtr index);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern UIntPtr PlayerManager_FetchListResponse_GetData_Length(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern CommonErrorStatus.ResponseStatus PlayerManager_FetchListResponse_GetStatus(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void PlayerManager_FetchRecentlyPlayed(HandleRef self, Types.DataSource data_source, PlayerManager.FetchListCallback callback, IntPtr callback_arg);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void PlayerManager_FetchResponse_Dispose(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern IntPtr PlayerManager_FetchResponse_GetData(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern CommonErrorStatus.ResponseStatus PlayerManager_FetchResponse_GetStatus(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void PlayerManager_FetchSelf(HandleRef self, Types.DataSource data_source, PlayerManager.FetchSelfCallback callback, IntPtr callback_arg);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void PlayerManager_FetchSelfResponse_Dispose(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern IntPtr PlayerManager_FetchSelfResponse_GetData(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern CommonErrorStatus.ResponseStatus PlayerManager_FetchSelfResponse_GetStatus(HandleRef self);

		internal delegate void FetchCallback(IntPtr arg0, IntPtr arg1);

		internal delegate void FetchListCallback(IntPtr arg0, IntPtr arg1);

		internal delegate void FetchSelfCallback(IntPtr arg0, IntPtr arg1);
	}
}