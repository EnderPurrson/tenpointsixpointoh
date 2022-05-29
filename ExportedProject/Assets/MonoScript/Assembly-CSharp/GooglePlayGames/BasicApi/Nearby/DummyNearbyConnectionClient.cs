using GooglePlayGames.BasicApi;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GooglePlayGames.BasicApi.Nearby
{
	public class DummyNearbyConnectionClient : INearbyConnectionClient
	{
		public DummyNearbyConnectionClient()
		{
		}

		public void AcceptConnectionRequest(string remoteEndpointId, byte[] payload, IMessageListener listener)
		{
			Debug.LogError("AcceptConnectionRequest in dummy implementation called");
		}

		public void DisconnectFromEndpoint(string remoteEndpointId)
		{
			Debug.LogError("DisconnectFromEndpoint in dummy implementation called");
		}

		public string GetAppBundleId()
		{
			return "dummy.bundle.id";
		}

		public string GetServiceId()
		{
			return "dummy.service.id";
		}

		public string LocalDeviceId()
		{
			return "DummyDevice";
		}

		public string LocalEndpointId()
		{
			return string.Empty;
		}

		public int MaxReliableMessagePayloadLength()
		{
			return 4096;
		}

		public int MaxUnreliableMessagePayloadLength()
		{
			return 1168;
		}

		public void RejectConnectionRequest(string requestingEndpointId)
		{
			Debug.LogError("RejectConnectionRequest in dummy implementation called");
		}

		public void SendConnectionRequest(string name, string remoteEndpointId, byte[] payload, Action<ConnectionResponse> responseCallback, IMessageListener listener)
		{
			Debug.LogError("SendConnectionRequest called from dummy implementation");
			if (responseCallback != null)
			{
				responseCallback(ConnectionResponse.Rejected((long)0, string.Empty));
			}
		}

		public void SendReliable(List<string> recipientEndpointIds, byte[] payload)
		{
			Debug.LogError("SendReliable called from dummy implementation");
		}

		public void SendUnreliable(List<string> recipientEndpointIds, byte[] payload)
		{
			Debug.LogError("SendUnreliable called from dummy implementation");
		}

		public void StartAdvertising(string name, List<string> appIdentifiers, TimeSpan? advertisingDuration, Action<AdvertisingResult> resultCallback, Action<ConnectionRequest> connectionRequestCallback)
		{
			resultCallback(new AdvertisingResult(ResponseStatus.LicenseCheckFailed, string.Empty));
		}

		public void StartDiscovery(string serviceId, TimeSpan? advertisingTimeout, IDiscoveryListener listener)
		{
			Debug.LogError("StartDiscovery in dummy implementation called");
		}

		public void StopAdvertising()
		{
			Debug.LogError("StopAvertising in dummy implementation called");
		}

		public void StopAllConnections()
		{
			Debug.LogError("StopAllConnections in dummy implementation called");
		}

		public void StopDiscovery(string serviceId)
		{
			Debug.LogError("StopDiscovery in dummy implementation called");
		}
	}
}