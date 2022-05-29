using AOT;
using GooglePlayGames.Native.Cwrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NearbyConnectionsManager : BaseReferenceHolder
	{
		private readonly static string sServiceId;

		public string AppBundleId
		{
			get
			{
				string str;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
					str = @static.Call<string>("getPackageName", new object[0]);
				}
				return str;
			}
		}

		public static string ServiceId
		{
			get
			{
				return NearbyConnectionsManager.sServiceId;
			}
		}

		static NearbyConnectionsManager()
		{
			NearbyConnectionsManager.sServiceId = NearbyConnectionsManager.ReadServiceId();
		}

		internal NearbyConnectionsManager(IntPtr selfPointer) : base(selfPointer)
		{
		}

		internal void AcceptConnectionRequest(string remoteEndpointId, byte[] payload, NativeMessageListenerHelper listener)
		{
			NearbyConnections.NearbyConnections_AcceptConnectionRequest(base.SelfPtr(), remoteEndpointId, payload, new UIntPtr((ulong)((int)payload.Length)), listener.AsPointer());
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			NearbyConnections.NearbyConnections_Dispose(selfPointer);
		}

		internal void DisconnectFromEndpoint(string remoteEndpointId)
		{
			NearbyConnections.NearbyConnections_Disconnect(base.SelfPtr(), remoteEndpointId);
		}

		[MonoPInvokeCallback(typeof(NearbyConnectionTypes.ConnectionRequestCallback))]
		private static void InternalConnectionRequestCallback(long id, IntPtr result, IntPtr userData)
		{
			Callbacks.PerformInternalCallback<long>("NearbyConnectionsManager#InternalConnectionRequestCallback", Callbacks.Type.Permanent, id, result, userData);
		}

		[MonoPInvokeCallback(typeof(NearbyConnectionTypes.ConnectionResponseCallback))]
		private static void InternalConnectResponseCallback(long localClientId, IntPtr response, IntPtr userData)
		{
			Callbacks.PerformInternalCallback<long>("NearbyConnectionManager#InternalConnectResponseCallback", Callbacks.Type.Temporary, localClientId, response, userData);
		}

		[MonoPInvokeCallback(typeof(NearbyConnectionTypes.StartAdvertisingCallback))]
		private static void InternalStartAdvertisingCallback(long id, IntPtr result, IntPtr userData)
		{
			Callbacks.PerformInternalCallback<long>("NearbyConnectionsManager#InternalStartAdvertisingCallback", Callbacks.Type.Permanent, id, result, userData);
		}

		internal string LocalDeviceId()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_arg, UIntPtr out_size) => NearbyConnections.NearbyConnections_GetLocalDeviceId(base.SelfPtr(), out_arg, out_size));
		}

		internal string LocalEndpointId()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_arg, UIntPtr out_size) => NearbyConnections.NearbyConnections_GetLocalEndpointId(base.SelfPtr(), out_arg, out_size));
		}

		internal static string ReadServiceId()
		{
			string str;
			Debug.Log("Initializing ServiceId property!!!!");
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					string str1 = @static.Call<string>("getPackageName", new object[0]);
					AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getPackageManager", new object[0]);
					AndroidJavaObject androidJavaObject1 = androidJavaObject.Call<AndroidJavaObject>("getApplicationInfo", new object[] { str1, 128 }).Get<AndroidJavaObject>("metaData");
					string str2 = androidJavaObject1.Call<string>("getString", new object[] { "com.google.android.gms.nearby.connection.SERVICE_ID" });
					Debug.Log(string.Concat("SystemId from Manifest: ", str2));
					str = str2;
				}
			}
			return str;
		}

		internal void RejectConnectionRequest(string remoteEndpointId)
		{
			NearbyConnections.NearbyConnections_RejectConnectionRequest(base.SelfPtr(), remoteEndpointId);
		}

		internal void SendConnectionRequest(string name, string remoteEndpointId, byte[] payload, Action<long, NativeConnectionResponse> callback, NativeMessageListenerHelper listener)
		{
			NearbyConnections.NearbyConnections_SendConnectionRequest(base.SelfPtr(), name, remoteEndpointId, payload, new UIntPtr((ulong)((int)payload.Length)), new NearbyConnectionTypes.ConnectionResponseCallback(NearbyConnectionsManager.InternalConnectResponseCallback), Callbacks.ToIntPtr<long, NativeConnectionResponse>(callback, new Func<IntPtr, NativeConnectionResponse>(NativeConnectionResponse.FromPointer)), listener.AsPointer());
		}

		internal void SendReliable(string remoteEndpointId, byte[] payload)
		{
			NearbyConnections.NearbyConnections_SendReliableMessage(base.SelfPtr(), remoteEndpointId, payload, new UIntPtr((ulong)((int)payload.Length)));
		}

		internal void SendUnreliable(string remoteEndpointId, byte[] payload)
		{
			NearbyConnections.NearbyConnections_SendUnreliableMessage(base.SelfPtr(), remoteEndpointId, payload, new UIntPtr((ulong)((int)payload.Length)));
		}

		internal void Shutdown()
		{
			NearbyConnections.NearbyConnections_Stop(base.SelfPtr());
		}

		internal void StartAdvertising(string name, List<NativeAppIdentifier> appIds, long advertisingDuration, Action<long, NativeStartAdvertisingResult> advertisingCallback, Action<long, NativeConnectionRequest> connectionRequestCallback)
		{
			NearbyConnections.NearbyConnections_StartAdvertising(base.SelfPtr(), name, (
				from id in appIds
				select id.AsPointer()).ToArray<IntPtr>(), new UIntPtr((ulong)appIds.Count), advertisingDuration, new NearbyConnectionTypes.StartAdvertisingCallback(NearbyConnectionsManager.InternalStartAdvertisingCallback), Callbacks.ToIntPtr<long, NativeStartAdvertisingResult>(advertisingCallback, new Func<IntPtr, NativeStartAdvertisingResult>(NativeStartAdvertisingResult.FromPointer)), new NearbyConnectionTypes.ConnectionRequestCallback(NearbyConnectionsManager.InternalConnectionRequestCallback), Callbacks.ToIntPtr<long, NativeConnectionRequest>(connectionRequestCallback, new Func<IntPtr, NativeConnectionRequest>(NativeConnectionRequest.FromPointer)));
		}

		internal void StartDiscovery(string serviceId, long duration, NativeEndpointDiscoveryListenerHelper listener)
		{
			NearbyConnections.NearbyConnections_StartDiscovery(base.SelfPtr(), serviceId, duration, listener.AsPointer());
		}

		internal void StopAdvertising()
		{
			NearbyConnections.NearbyConnections_StopAdvertising(base.SelfPtr());
		}

		internal void StopAllConnections()
		{
			NearbyConnections.NearbyConnections_Stop(base.SelfPtr());
		}

		internal void StopDiscovery(string serviceId)
		{
			NearbyConnections.NearbyConnections_StopDiscovery(base.SelfPtr(), serviceId);
		}
	}
}