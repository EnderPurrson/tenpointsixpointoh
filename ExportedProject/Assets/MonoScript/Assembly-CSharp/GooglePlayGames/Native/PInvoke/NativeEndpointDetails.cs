using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.BasicApi.Nearby;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeEndpointDetails : BaseReferenceHolder
	{
		internal NativeEndpointDetails(IntPtr pointer)
			: base(pointer)
		{
		}

		internal string EndpointId()
		{
			return PInvokeUtilities.OutParamsToString(_003CEndpointId_003Em__138);
		}

		internal string DeviceId()
		{
			return PInvokeUtilities.OutParamsToString(_003CDeviceId_003Em__139);
		}

		internal string Name()
		{
			return PInvokeUtilities.OutParamsToString(_003CName_003Em__13A);
		}

		internal string ServiceId()
		{
			return PInvokeUtilities.OutParamsToString(_003CServiceId_003Em__13B);
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			NearbyConnectionTypes.EndpointDetails_Dispose(selfPointer);
		}

		internal EndpointDetails ToDetails()
		{
			return new EndpointDetails(EndpointId(), DeviceId(), Name(), ServiceId());
		}

		internal static NativeEndpointDetails FromPointer(IntPtr pointer)
		{
			if (pointer.Equals(IntPtr.Zero))
			{
				return null;
			}
			return new NativeEndpointDetails(pointer);
		}

		[CompilerGenerated]
		private UIntPtr _003CEndpointId_003Em__138(StringBuilder out_arg, UIntPtr out_size)
		{
			return NearbyConnectionTypes.EndpointDetails_GetEndpointId(SelfPtr(), out_arg, out_size);
		}

		[CompilerGenerated]
		private UIntPtr _003CDeviceId_003Em__139(StringBuilder out_arg, UIntPtr out_size)
		{
			return NearbyConnectionTypes.EndpointDetails_GetDeviceId(SelfPtr(), out_arg, out_size);
		}

		[CompilerGenerated]
		private UIntPtr _003CName_003Em__13A(StringBuilder out_arg, UIntPtr out_size)
		{
			return NearbyConnectionTypes.EndpointDetails_GetName(SelfPtr(), out_arg, out_size);
		}

		[CompilerGenerated]
		private UIntPtr _003CServiceId_003Em__13B(StringBuilder out_arg, UIntPtr out_size)
		{
			return NearbyConnectionTypes.EndpointDetails_GetServiceId(SelfPtr(), out_arg, out_size);
		}
	}
}
