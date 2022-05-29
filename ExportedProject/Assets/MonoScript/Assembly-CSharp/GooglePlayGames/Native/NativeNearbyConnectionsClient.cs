using GooglePlayGames.BasicApi.Nearby;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace GooglePlayGames.Native
{
	internal class NativeNearbyConnectionsClient : INearbyConnectionClient
	{
		private readonly NearbyConnectionsManager mManager;

		internal NativeNearbyConnectionsClient(NearbyConnectionsManager manager)
		{
			this.mManager = Misc.CheckNotNull<NearbyConnectionsManager>(manager);
		}

		public void AcceptConnectionRequest(string remoteEndpointId, byte[] payload, IMessageListener listener)
		{
			Misc.CheckNotNull<string>(remoteEndpointId, "remoteEndpointId");
			Misc.CheckNotNull<byte[]>(payload, "payload");
			Misc.CheckNotNull<IMessageListener>(listener, "listener");
			Logger.d("Calling AcceptConncectionRequest");
			this.mManager.AcceptConnectionRequest(remoteEndpointId, payload, NativeNearbyConnectionsClient.ToMessageListener(listener));
			Logger.d("Called!");
		}

		public void DisconnectFromEndpoint(string remoteEndpointId)
		{
			this.mManager.DisconnectFromEndpoint(remoteEndpointId);
		}

		public string GetAppBundleId()
		{
			return this.mManager.AppBundleId;
		}

		public string GetServiceId()
		{
			return NearbyConnectionsManager.ServiceId;
		}

		private void InternalSend(List<string> recipientEndpointIds, byte[] payload, bool isReliable)
		{
			if (recipientEndpointIds == null)
			{
				throw new ArgumentNullException("recipientEndpointIds");
			}
			if (payload == null)
			{
				throw new ArgumentNullException("payload");
			}
			if (recipientEndpointIds.Contains(null))
			{
				throw new InvalidOperationException("Cannot send a message to a null recipient");
			}
			if (recipientEndpointIds.Count == 0)
			{
				Logger.w("Attempted to send a reliable message with no recipients");
				return;
			}
			if (isReliable)
			{
				if ((int)payload.Length > this.MaxReliableMessagePayloadLength())
				{
					throw new InvalidOperationException(string.Concat("cannot send more than ", this.MaxReliableMessagePayloadLength(), " bytes"));
				}
			}
			else if ((int)payload.Length > this.MaxUnreliableMessagePayloadLength())
			{
				throw new InvalidOperationException(string.Concat("cannot send more than ", this.MaxUnreliableMessagePayloadLength(), " bytes"));
			}
			foreach (string recipientEndpointId in recipientEndpointIds)
			{
				if (!isReliable)
				{
					this.mManager.SendUnreliable(recipientEndpointId, payload);
				}
				else
				{
					this.mManager.SendReliable(recipientEndpointId, payload);
				}
			}
		}

		public string LocalDeviceId()
		{
			return this.mManager.LocalDeviceId();
		}

		public string LocalEndpointId()
		{
			return this.mManager.LocalEndpointId();
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
			Misc.CheckNotNull<string>(requestingEndpointId, "requestingEndpointId");
			this.mManager.RejectConnectionRequest(requestingEndpointId);
		}

		public void SendConnectionRequest(string name, string remoteEndpointId, byte[] payload, Action<ConnectionResponse> responseCallback, IMessageListener listener)
		{
			Action<ConnectionResponse> action = responseCallback;
			Misc.CheckNotNull<string>(remoteEndpointId, "remoteEndpointId");
			Misc.CheckNotNull<byte[]>(payload, "payload");
			Misc.CheckNotNull<Action<ConnectionResponse>>(action, "responseCallback");
			Misc.CheckNotNull<IMessageListener>(listener, "listener");
			action = Callbacks.AsOnGameThreadCallback<ConnectionResponse>(action);
			using (NativeMessageListenerHelper messageListener = NativeNearbyConnectionsClient.ToMessageListener(listener))
			{
				this.mManager.SendConnectionRequest(name, remoteEndpointId, payload, (long localClientId, NativeConnectionResponse response) => action(response.AsResponse(localClientId)), messageListener);
			}
		}

		public void SendReliable(List<string> recipientEndpointIds, byte[] payload)
		{
			this.InternalSend(recipientEndpointIds, payload, true);
		}

		public void SendUnreliable(List<string> recipientEndpointIds, byte[] payload)
		{
			this.InternalSend(recipientEndpointIds, payload, false);
		}

		public void StartAdvertising(string name, List<string> appIdentifiers, TimeSpan? advertisingDuration, Action<AdvertisingResult> resultCallback, Action<ConnectionRequest> requestCallback)
		{
			Action<AdvertisingResult> action = resultCallback;
			Action<ConnectionRequest> action1 = requestCallback;
			Misc.CheckNotNull<List<string>>(appIdentifiers, "appIdentifiers");
			Misc.CheckNotNull<Action<AdvertisingResult>>(action, "resultCallback");
			Misc.CheckNotNull<Action<ConnectionRequest>>(action1, "connectionRequestCallback");
			if (advertisingDuration.HasValue && advertisingDuration.Value.Ticks < (long)0)
			{
				throw new InvalidOperationException("advertisingDuration must be positive");
			}
			action = Callbacks.AsOnGameThreadCallback<AdvertisingResult>(action);
			action1 = Callbacks.AsOnGameThreadCallback<ConnectionRequest>(action1);
			this.mManager.StartAdvertising(name, appIdentifiers.Select<string, NativeAppIdentifier>(new Func<string, NativeAppIdentifier>(NativeAppIdentifier.FromString)).ToList<NativeAppIdentifier>(), NativeNearbyConnectionsClient.ToTimeoutMillis(advertisingDuration), (long localClientId, NativeStartAdvertisingResult result) => action(result.AsResult()), (long localClientId, NativeConnectionRequest request) => action1(request.AsRequest()));
		}

		public void StartDiscovery(string serviceId, TimeSpan? advertisingTimeout, IDiscoveryListener listener)
		{
			Misc.CheckNotNull<string>(serviceId, "serviceId");
			Misc.CheckNotNull<IDiscoveryListener>(listener, "listener");
			using (NativeEndpointDiscoveryListenerHelper discoveryListener = NativeNearbyConnectionsClient.ToDiscoveryListener(listener))
			{
				this.mManager.StartDiscovery(serviceId, NativeNearbyConnectionsClient.ToTimeoutMillis(advertisingTimeout), discoveryListener);
			}
		}

		public void StopAdvertising()
		{
			this.mManager.StopAdvertising();
		}

		public void StopAllConnections()
		{
			this.mManager.StopAllConnections();
		}

		public void StopDiscovery(string serviceId)
		{
			Misc.CheckNotNull<string>(serviceId, "serviceId");
			this.mManager.StopDiscovery(serviceId);
		}

		private static NativeEndpointDiscoveryListenerHelper ToDiscoveryListener(IDiscoveryListener listener)
		{
			IDiscoveryListener onGameThreadDiscoveryListener = listener;
			onGameThreadDiscoveryListener = new NativeNearbyConnectionsClient.OnGameThreadDiscoveryListener(onGameThreadDiscoveryListener);
			NativeEndpointDiscoveryListenerHelper nativeEndpointDiscoveryListenerHelper = new NativeEndpointDiscoveryListenerHelper();
			nativeEndpointDiscoveryListenerHelper.SetOnEndpointFound((long localClientId, NativeEndpointDetails endpoint) => onGameThreadDiscoveryListener.OnEndpointFound(endpoint.ToDetails()));
			nativeEndpointDiscoveryListenerHelper.SetOnEndpointLostCallback((long localClientId, string lostEndpointId) => onGameThreadDiscoveryListener.OnEndpointLost(lostEndpointId));
			return nativeEndpointDiscoveryListenerHelper;
		}

		private static NativeMessageListenerHelper ToMessageListener(IMessageListener listener)
		{
			IMessageListener onGameThreadMessageListener = listener;
			onGameThreadMessageListener = new NativeNearbyConnectionsClient.OnGameThreadMessageListener(onGameThreadMessageListener);
			NativeMessageListenerHelper nativeMessageListenerHelper = new NativeMessageListenerHelper();
			nativeMessageListenerHelper.SetOnMessageReceivedCallback((long localClientId, string endpointId, byte[] data, bool isReliable) => onGameThreadMessageListener.OnMessageReceived(endpointId, data, isReliable));
			nativeMessageListenerHelper.SetOnDisconnectedCallback((long localClientId, string endpointId) => onGameThreadMessageListener.OnRemoteEndpointDisconnected(endpointId));
			return nativeMessageListenerHelper;
		}

		private static long ToTimeoutMillis(TimeSpan? span)
		{
			return (!span.HasValue ? (long)0 : PInvokeUtilities.ToMilliseconds(span.Value));
		}

		protected class OnGameThreadDiscoveryListener : IDiscoveryListener
		{
			private readonly IDiscoveryListener mListener;

			public OnGameThreadDiscoveryListener(IDiscoveryListener listener)
			{
				this.mListener = Misc.CheckNotNull<IDiscoveryListener>(listener);
			}

			public void OnEndpointFound(EndpointDetails discoveredEndpoint)
			{
				PlayGamesHelperObject.RunOnGameThread(() => this.mListener.OnEndpointFound(discoveredEndpoint));
			}

			public void OnEndpointLost(string lostEndpointId)
			{
				PlayGamesHelperObject.RunOnGameThread(() => this.mListener.OnEndpointLost(lostEndpointId));
			}
		}

		protected class OnGameThreadMessageListener : IMessageListener
		{
			private readonly IMessageListener mListener;

			public OnGameThreadMessageListener(IMessageListener listener)
			{
				this.mListener = Misc.CheckNotNull<IMessageListener>(listener);
			}

			public void OnMessageReceived(string remoteEndpointId, byte[] data, bool isReliableMessage)
			{
				PlayGamesHelperObject.RunOnGameThread(() => this.mListener.OnMessageReceived(remoteEndpointId, data, isReliableMessage));
			}

			public void OnRemoteEndpointDisconnected(string remoteEndpointId)
			{
				PlayGamesHelperObject.RunOnGameThread(() => this.mListener.OnRemoteEndpointDisconnected(remoteEndpointId));
			}
		}
	}
}