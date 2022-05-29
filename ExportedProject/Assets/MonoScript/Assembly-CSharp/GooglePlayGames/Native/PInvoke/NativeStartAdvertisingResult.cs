using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Nearby;
using GooglePlayGames.Native.Cwrapper;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeStartAdvertisingResult : BaseReferenceHolder
	{
		internal NativeStartAdvertisingResult(IntPtr pointer) : base(pointer)
		{
		}

		internal AdvertisingResult AsResult()
		{
			return new AdvertisingResult((ResponseStatus)((int)Enum.ToObject(typeof(ResponseStatus), this.GetStatus())), this.LocalEndpointName());
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			NearbyConnectionTypes.StartAdvertisingResult_Dispose(selfPointer);
		}

		internal static NativeStartAdvertisingResult FromPointer(IntPtr pointer)
		{
			if (pointer == IntPtr.Zero)
			{
				return null;
			}
			return new NativeStartAdvertisingResult(pointer);
		}

		internal int GetStatus()
		{
			return NearbyConnectionTypes.StartAdvertisingResult_GetStatus(base.SelfPtr());
		}

		internal string LocalEndpointName()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_arg, UIntPtr out_size) => NearbyConnectionTypes.StartAdvertisingResult_GetLocalEndpointName(base.SelfPtr(), out_arg, out_size));
		}
	}
}