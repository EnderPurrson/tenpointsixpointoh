using GooglePlayGames.BasicApi.Nearby;
using GooglePlayGames.Native.Cwrapper;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeConnectionRequest : BaseReferenceHolder
	{
		internal NativeConnectionRequest(IntPtr pointer) : base(pointer)
		{
		}

		internal ConnectionRequest AsRequest()
		{
			ConnectionRequest connectionRequest = new ConnectionRequest(this.RemoteEndpointId(), this.RemoteDeviceId(), this.RemoteEndpointName(), NearbyConnectionsManager.ServiceId, this.Payload());
			return connectionRequest;
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			NearbyConnectionTypes.ConnectionRequest_Dispose(selfPointer);
		}

		internal static NativeConnectionRequest FromPointer(IntPtr pointer)
		{
			if (pointer == IntPtr.Zero)
			{
				return null;
			}
			return new NativeConnectionRequest(pointer);
		}

		internal byte[] Payload()
		{
			return PInvokeUtilities.OutParamsToArray<byte>((byte[] out_arg, UIntPtr out_size) => NearbyConnectionTypes.ConnectionRequest_GetPayload(base.SelfPtr(), out_arg, out_size));
		}

		internal string RemoteDeviceId()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_arg, UIntPtr out_size) => NearbyConnectionTypes.ConnectionRequest_GetRemoteDeviceId(base.SelfPtr(), out_arg, out_size));
		}

		internal string RemoteEndpointId()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_arg, UIntPtr out_size) => NearbyConnectionTypes.ConnectionRequest_GetRemoteEndpointId(base.SelfPtr(), out_arg, out_size));
		}

		internal string RemoteEndpointName()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_arg, UIntPtr out_size) => NearbyConnectionTypes.ConnectionRequest_GetRemoteEndpointName(base.SelfPtr(), out_arg, out_size));
		}
	}
}