using GooglePlayGames.BasicApi.Nearby;
using GooglePlayGames.Native.Cwrapper;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeConnectionResponse : BaseReferenceHolder
	{
		internal NativeConnectionResponse(IntPtr pointer) : base(pointer)
		{
		}

		internal ConnectionResponse AsResponse(long localClientId)
		{
			switch (this.ResponseCode())
			{
				case NearbyConnectionTypes.ConnectionResponse_ResponseCode.ERROR_ENDPOINT_NOT_CONNECTED:
				{
					return ConnectionResponse.EndpointNotConnected(localClientId, this.RemoteEndpointId());
				}
				case NearbyConnectionTypes.ConnectionResponse_ResponseCode.ERROR_ENDPOINT_ALREADY_CONNECTED:
				{
					return ConnectionResponse.AlreadyConnected(localClientId, this.RemoteEndpointId());
				}
				case NearbyConnectionTypes.ConnectionResponse_ResponseCode.ERROR_NETWORK_NOT_CONNECTED:
				{
					return ConnectionResponse.NetworkNotConnected(localClientId, this.RemoteEndpointId());
				}
				case NearbyConnectionTypes.ConnectionResponse_ResponseCode.ERROR_INTERNAL:
				{
					return ConnectionResponse.InternalError(localClientId, this.RemoteEndpointId());
				}
				case 0:
				{
					throw new InvalidOperationException(string.Concat("Found connection response of unknown type: ", this.ResponseCode()));
				}
				case NearbyConnectionTypes.ConnectionResponse_ResponseCode.ACCEPTED:
				{
					return ConnectionResponse.Accepted(localClientId, this.RemoteEndpointId(), this.Payload());
				}
				case NearbyConnectionTypes.ConnectionResponse_ResponseCode.REJECTED:
				{
					return ConnectionResponse.Rejected(localClientId, this.RemoteEndpointId());
				}
				default:
				{
					throw new InvalidOperationException(string.Concat("Found connection response of unknown type: ", this.ResponseCode()));
				}
			}
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			NearbyConnectionTypes.ConnectionResponse_Dispose(selfPointer);
		}

		internal static NativeConnectionResponse FromPointer(IntPtr pointer)
		{
			if (pointer == IntPtr.Zero)
			{
				return null;
			}
			return new NativeConnectionResponse(pointer);
		}

		internal byte[] Payload()
		{
			return PInvokeUtilities.OutParamsToArray<byte>((byte[] out_arg, UIntPtr out_size) => NearbyConnectionTypes.ConnectionResponse_GetPayload(base.SelfPtr(), out_arg, out_size));
		}

		internal string RemoteEndpointId()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_arg, UIntPtr out_size) => NearbyConnectionTypes.ConnectionResponse_GetRemoteEndpointId(base.SelfPtr(), out_arg, out_size));
		}

		internal NearbyConnectionTypes.ConnectionResponse_ResponseCode ResponseCode()
		{
			return NearbyConnectionTypes.ConnectionResponse_GetStatus(base.SelfPtr());
		}
	}
}