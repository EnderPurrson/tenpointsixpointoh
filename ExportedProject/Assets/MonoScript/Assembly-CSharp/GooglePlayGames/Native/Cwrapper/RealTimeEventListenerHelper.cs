using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	internal static class RealTimeEventListenerHelper
	{
		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern IntPtr RealTimeEventListenerHelper_Construct();

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void RealTimeEventListenerHelper_Dispose(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void RealTimeEventListenerHelper_SetOnDataReceivedCallback(HandleRef self, RealTimeEventListenerHelper.OnDataReceivedCallback callback, IntPtr callback_arg);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void RealTimeEventListenerHelper_SetOnP2PConnectedCallback(HandleRef self, RealTimeEventListenerHelper.OnP2PConnectedCallback callback, IntPtr callback_arg);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void RealTimeEventListenerHelper_SetOnP2PDisconnectedCallback(HandleRef self, RealTimeEventListenerHelper.OnP2PDisconnectedCallback callback, IntPtr callback_arg);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void RealTimeEventListenerHelper_SetOnParticipantStatusChangedCallback(HandleRef self, RealTimeEventListenerHelper.OnParticipantStatusChangedCallback callback, IntPtr callback_arg);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void RealTimeEventListenerHelper_SetOnRoomConnectedSetChangedCallback(HandleRef self, RealTimeEventListenerHelper.OnRoomConnectedSetChangedCallback callback, IntPtr callback_arg);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void RealTimeEventListenerHelper_SetOnRoomStatusChangedCallback(HandleRef self, RealTimeEventListenerHelper.OnRoomStatusChangedCallback callback, IntPtr callback_arg);

		internal delegate void OnDataReceivedCallback(IntPtr arg0, IntPtr arg1, IntPtr arg2, UIntPtr arg3, bool arg4, IntPtr arg5);

		internal delegate void OnP2PConnectedCallback(IntPtr arg0, IntPtr arg1, IntPtr arg2);

		internal delegate void OnP2PDisconnectedCallback(IntPtr arg0, IntPtr arg1, IntPtr arg2);

		internal delegate void OnParticipantStatusChangedCallback(IntPtr arg0, IntPtr arg1, IntPtr arg2);

		internal delegate void OnRoomConnectedSetChangedCallback(IntPtr arg0, IntPtr arg1);

		internal delegate void OnRoomStatusChangedCallback(IntPtr arg0, IntPtr arg1);
	}
}