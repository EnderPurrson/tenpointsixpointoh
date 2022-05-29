using GooglePlayGames.OurUtils;
using System;

namespace GooglePlayGames.BasicApi.Nearby
{
	public struct ConnectionRequest
	{
		private readonly EndpointDetails mRemoteEndpoint;

		private readonly byte[] mPayload;

		public byte[] Payload
		{
			get
			{
				return this.mPayload;
			}
		}

		public EndpointDetails RemoteEndpoint
		{
			get
			{
				return this.mRemoteEndpoint;
			}
		}

		public ConnectionRequest(string remoteEndpointId, string remoteDeviceId, string remoteEndpointName, string serviceId, byte[] payload)
		{
			Logger.d("Constructing ConnectionRequest");
			this.mRemoteEndpoint = new EndpointDetails(remoteEndpointId, remoteDeviceId, remoteEndpointName, serviceId);
			this.mPayload = Misc.CheckNotNull<byte[]>(payload);
		}
	}
}