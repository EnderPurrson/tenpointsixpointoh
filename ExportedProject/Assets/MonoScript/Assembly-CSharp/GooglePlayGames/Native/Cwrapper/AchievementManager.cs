using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	internal static class AchievementManager
	{
		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void AchievementManager_Fetch(HandleRef self, Types.DataSource data_source, string achievement_id, AchievementManager.FetchCallback callback, IntPtr callback_arg);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void AchievementManager_FetchAll(HandleRef self, Types.DataSource data_source, AchievementManager.FetchAllCallback callback, IntPtr callback_arg);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void AchievementManager_FetchAllResponse_Dispose(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern IntPtr AchievementManager_FetchAllResponse_GetData_GetElement(HandleRef self, UIntPtr index);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern UIntPtr AchievementManager_FetchAllResponse_GetData_Length(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern CommonErrorStatus.ResponseStatus AchievementManager_FetchAllResponse_GetStatus(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void AchievementManager_FetchResponse_Dispose(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern IntPtr AchievementManager_FetchResponse_GetData(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern CommonErrorStatus.ResponseStatus AchievementManager_FetchResponse_GetStatus(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void AchievementManager_Increment(HandleRef self, string achievement_id, uint steps);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void AchievementManager_Reveal(HandleRef self, string achievement_id);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void AchievementManager_SetStepsAtLeast(HandleRef self, string achievement_id, uint steps);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void AchievementManager_ShowAllUI(HandleRef self, AchievementManager.ShowAllUICallback callback, IntPtr callback_arg);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void AchievementManager_Unlock(HandleRef self, string achievement_id);

		internal delegate void FetchAllCallback(IntPtr arg0, IntPtr arg1);

		internal delegate void FetchCallback(IntPtr arg0, IntPtr arg1);

		internal delegate void ShowAllUICallback(CommonErrorStatus.UIStatus arg0, IntPtr arg1);
	}
}