using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	internal static class NearbyConnectionsBuilder
	{
		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern IntPtr NearbyConnections_Builder_Construct();

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern IntPtr NearbyConnections_Builder_Create(HandleRef self, IntPtr platform);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void NearbyConnections_Builder_Dispose(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void NearbyConnections_Builder_SetClientId(HandleRef self, long client_id);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void NearbyConnections_Builder_SetDefaultOnLog(HandleRef self, Types.LogLevel min_level);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void NearbyConnections_Builder_SetOnInitializationFinished(HandleRef self, NearbyConnectionsBuilder.OnInitializationFinishedCallback callback, IntPtr callback_arg);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void NearbyConnections_Builder_SetOnLog(HandleRef self, NearbyConnectionsBuilder.OnLogCallback callback, IntPtr callback_arg, Types.LogLevel min_level);

		internal delegate void OnInitializationFinishedCallback(NearbyConnectionsStatus.InitializationStatus arg0, IntPtr arg1);

		internal delegate void OnLogCallback(Types.LogLevel arg0, string arg1, IntPtr arg2);
	}
}