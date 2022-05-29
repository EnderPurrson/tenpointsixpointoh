using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	internal static class MessageListenerHelper
	{
		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern IntPtr MessageListenerHelper_Construct();

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void MessageListenerHelper_Dispose(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void MessageListenerHelper_SetOnDisconnectedCallback(HandleRef self, MessageListenerHelper.OnDisconnectedCallback callback, IntPtr callback_arg);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void MessageListenerHelper_SetOnMessageReceivedCallback(HandleRef self, MessageListenerHelper.OnMessageReceivedCallback callback, IntPtr callback_arg);

		internal delegate void OnDisconnectedCallback(long arg0, string arg1, IntPtr arg2);

		internal delegate void OnMessageReceivedCallback(long arg0, string arg1, IntPtr arg2, UIntPtr arg3, bool arg4, IntPtr arg5);
	}
}