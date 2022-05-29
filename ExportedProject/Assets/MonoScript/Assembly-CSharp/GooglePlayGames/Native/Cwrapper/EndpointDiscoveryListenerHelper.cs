using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	internal static class EndpointDiscoveryListenerHelper
	{
		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern IntPtr EndpointDiscoveryListenerHelper_Construct();

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void EndpointDiscoveryListenerHelper_Dispose(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void EndpointDiscoveryListenerHelper_SetOnEndpointFoundCallback(HandleRef self, EndpointDiscoveryListenerHelper.OnEndpointFoundCallback callback, IntPtr callback_arg);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void EndpointDiscoveryListenerHelper_SetOnEndpointLostCallback(HandleRef self, EndpointDiscoveryListenerHelper.OnEndpointLostCallback callback, IntPtr callback_arg);

		internal delegate void OnEndpointFoundCallback(long arg0, IntPtr arg1, IntPtr arg2);

		internal delegate void OnEndpointLostCallback(long arg0, string arg1, IntPtr arg2);
	}
}