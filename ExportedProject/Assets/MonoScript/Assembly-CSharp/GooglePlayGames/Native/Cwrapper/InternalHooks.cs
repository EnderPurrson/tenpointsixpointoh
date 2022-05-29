using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	internal static class InternalHooks
	{
		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void InternalHooks_ConfigureForUnityPlugin(HandleRef builder);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern IntPtr InternalHooks_GetApiClient(HandleRef services);
	}
}