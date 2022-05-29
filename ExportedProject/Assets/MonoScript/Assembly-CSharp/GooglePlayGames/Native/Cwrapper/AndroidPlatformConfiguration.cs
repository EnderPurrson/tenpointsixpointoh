using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	internal static class AndroidPlatformConfiguration
	{
		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern IntPtr AndroidPlatformConfiguration_Construct();

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void AndroidPlatformConfiguration_Dispose(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void AndroidPlatformConfiguration_SetActivity(HandleRef self, IntPtr android_app_activity);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void AndroidPlatformConfiguration_SetOnLaunchedWithQuest(HandleRef self, AndroidPlatformConfiguration.OnLaunchedWithQuestCallback callback, IntPtr callback_arg);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void AndroidPlatformConfiguration_SetOnLaunchedWithSnapshot(HandleRef self, AndroidPlatformConfiguration.OnLaunchedWithSnapshotCallback callback, IntPtr callback_arg);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void AndroidPlatformConfiguration_SetOptionalIntentHandlerForUI(HandleRef self, AndroidPlatformConfiguration.IntentHandler intent_handler, IntPtr intent_handler_arg);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern bool AndroidPlatformConfiguration_Valid(HandleRef self);

		internal delegate void IntentHandler(IntPtr arg0, IntPtr arg1);

		internal delegate void OnLaunchedWithQuestCallback(IntPtr arg0, IntPtr arg1);

		internal delegate void OnLaunchedWithSnapshotCallback(IntPtr arg0, IntPtr arg1);
	}
}