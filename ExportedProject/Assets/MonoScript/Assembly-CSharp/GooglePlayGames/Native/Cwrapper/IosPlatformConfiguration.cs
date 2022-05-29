using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	internal static class IosPlatformConfiguration
	{
		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern IntPtr IosPlatformConfiguration_Construct();

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void IosPlatformConfiguration_Dispose(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void IosPlatformConfiguration_SetClientID(HandleRef self, string client_id);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern bool IosPlatformConfiguration_Valid(HandleRef self);
	}
}