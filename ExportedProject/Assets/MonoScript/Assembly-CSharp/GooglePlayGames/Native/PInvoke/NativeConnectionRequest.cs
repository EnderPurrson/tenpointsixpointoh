using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.BasicApi.Nearby;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeConnectionRequest : BaseReferenceHolder
	{
		internal NativeConnectionRequest(IntPtr pointer)
			: base(pointer)
		{
		}

		internal string RemoteEndpointId()
		{
			return PInvokeUtilities.OutParamsToString(_003CRemoteEndpointId_003Em__132);
		}

		internal string RemoteDeviceId()
		{
			return PInvokeUtilities.OutParamsToString(_003CRemoteDeviceId_003Em__133);
		}

		internal string RemoteEndpointName()
		{
			return PInvokeUtilities.OutParamsToString(_003CRemoteEndpointName_003Em__134);
		}

		internal byte[] Payload()
		{
			return PInvokeUtilities.OutParamsToArray<byte>(_003CPayload_003Em__135);
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			NearbyConnectionTypes.ConnectionRequest_Dispose(selfPointer);
		}

		internal ConnectionRequest AsRequest()
		{
			return new ConnectionRequest(RemoteEndpointId(), RemoteDeviceId(), RemoteEndpointName(), NearbyConnectionsManager.ServiceId, Payload());
		}

		internal static NativeConnectionRequest FromPointer(IntPtr pointer)
		{
			if (pointer == IntPtr.Zero)
			{
				return null;
			}
			return new NativeConnectionRequest(pointer);
		}

		[CompilerGenerated]
		private UIntPtr _003CRemoteEndpointId_003Em__132(StringBuilder out_arg, UIntPtr out_size)
		{
			return NearbyConnectionTypes.ConnectionRequest_GetRemoteEndpointId(SelfPtr(), out_arg, out_size);
		}

		[CompilerGenerated]
		private UIntPtr _003CRemoteDeviceId_003Em__133(StringBuilder out_arg, UIntPtr out_size)
		{
			return NearbyConnectionTypes.ConnectionRequest_GetRemoteDeviceId(SelfPtr(), out_arg, out_size);
		}

		[CompilerGenerated]
		private UIntPtr _003CRemoteEndpointName_003Em__134(StringBuilder out_arg, UIntPtr out_size)
		{
			return NearbyConnectionTypes.ConnectionRequest_GetRemoteEndpointName(SelfPtr(), out_arg, out_size);
		}

		[CompilerGenerated]
		private UIntPtr _003CPayload_003Em__135(byte[] out_arg, UIntPtr out_size)
		{
			return NearbyConnectionTypes.ConnectionRequest_GetPayload(SelfPtr(), out_arg, out_size);
		}
	}
}
