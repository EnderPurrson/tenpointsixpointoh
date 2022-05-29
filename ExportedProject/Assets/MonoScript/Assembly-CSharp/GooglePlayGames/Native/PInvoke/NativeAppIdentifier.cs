using GooglePlayGames.Native.Cwrapper;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeAppIdentifier : BaseReferenceHolder
	{
		internal NativeAppIdentifier(IntPtr pointer) : base(pointer)
		{
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			NearbyConnectionTypes.AppIdentifier_Dispose(selfPointer);
		}

		internal static NativeAppIdentifier FromString(string appId)
		{
			return new NativeAppIdentifier(NativeAppIdentifier.NearbyUtils_ConstructAppIdentifier(appId));
		}

		internal string Id()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_arg, UIntPtr out_size) => NearbyConnectionTypes.AppIdentifier_GetIdentifier(base.SelfPtr(), out_arg, out_size));
		}

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern IntPtr NearbyUtils_ConstructAppIdentifier(string appId);
	}
}