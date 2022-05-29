using GooglePlayGames.Android;
using GooglePlayGames.BasicApi.Nearby;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;
using System;
using UnityEngine;

namespace GooglePlayGames.Native
{
	public class NativeNearbyConnectionClientFactory
	{
		private static volatile NearbyConnectionsManager sManager;

		private static Action<INearbyConnectionClient> sCreationCallback;

		public NativeNearbyConnectionClientFactory()
		{
		}

		public static void Create(Action<INearbyConnectionClient> callback)
		{
			if (NativeNearbyConnectionClientFactory.sManager != null)
			{
				callback(new NativeNearbyConnectionsClient(NativeNearbyConnectionClientFactory.GetManager()));
			}
			else
			{
				NativeNearbyConnectionClientFactory.sCreationCallback = callback;
				NativeNearbyConnectionClientFactory.InitializeFactory();
			}
		}

		internal static NearbyConnectionsManager GetManager()
		{
			return NativeNearbyConnectionClientFactory.sManager;
		}

		internal static void InitializeFactory()
		{
			PlayGamesHelperObject.CreateObject();
			NearbyConnectionsManager.ReadServiceId();
			NearbyConnectionsManagerBuilder nearbyConnectionsManagerBuilder = new NearbyConnectionsManagerBuilder();
			nearbyConnectionsManagerBuilder.SetOnInitializationFinished(new Action<NearbyConnectionsStatus.InitializationStatus>(NativeNearbyConnectionClientFactory.OnManagerInitialized));
			PlatformConfiguration platformConfiguration = (new AndroidClient()).CreatePlatformConfiguration();
			Debug.Log("Building manager Now");
			NativeNearbyConnectionClientFactory.sManager = nearbyConnectionsManagerBuilder.Build(platformConfiguration);
		}

		internal static void OnManagerInitialized(NearbyConnectionsStatus.InitializationStatus status)
		{
			Debug.Log(string.Concat(new object[] { "Nearby Init Complete: ", status, " sManager = ", NativeNearbyConnectionClientFactory.sManager }));
			if (status != NearbyConnectionsStatus.InitializationStatus.VALID)
			{
				Debug.LogError(string.Concat("ERROR: NearbyConnectionManager not initialized: ", status));
			}
			else if (NativeNearbyConnectionClientFactory.sCreationCallback != null)
			{
				NativeNearbyConnectionClientFactory.sCreationCallback(new NativeNearbyConnectionsClient(NativeNearbyConnectionClientFactory.GetManager()));
				NativeNearbyConnectionClientFactory.sCreationCallback = null;
			}
		}
	}
}