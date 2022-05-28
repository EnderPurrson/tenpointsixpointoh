using System;
using System.Runtime.CompilerServices;
using DevToDev.Core.Utils;
using DevToDev.Logic;
using DevToDev.Push;
using UnityEngine;

namespace DevToDev
{
	public static class PushManager
	{
		private static PushClient pushClient;

		[CompilerGenerated]
		private static Action CS_0024_003C_003E9__CachedAnonymousMethodDelegate1;

		internal static PushClient PushClient
		{
			get
			{
				if (pushClient == null)
				{
					pushClient = new PushClient();
				}
				return pushClient;
			}
		}

		public static OnPushTokenReceivedHandler PushTokenReceived
		{
			get
			{
				return PushClient.OnPushTokenReceived;
			}
			set
			{
				PushClient.OnPushTokenReceived = value;
			}
		}

		public static OnPushTokenFailedHandler PushTokenFailed
		{
			get
			{
				return PushClient.OnPushTokenFailed;
			}
			set
			{
				PushClient.OnPushTokenFailed = value;
			}
		}

		public static OnPushReceivedHandler PushReceived
		{
			get
			{
				return PushClient.OnPushReceived;
			}
			set
			{
				PushClient.OnPushReceived = value;
			}
		}

		public static void Initialize()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			if (!SDKClient.Instance.IsInitialized)
			{
				SDKClient instance = SDKClient.Instance;
				if (CS_0024_003C_003E9__CachedAnonymousMethodDelegate1 == null)
				{
					CS_0024_003C_003E9__CachedAnonymousMethodDelegate1 = new Action(_003CInitialize_003Eb__0);
				}
				instance.Execute(CS_0024_003C_003E9__CachedAnonymousMethodDelegate1);
			}
			else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || UnityPlayerPlatform.isUnityWSAPlatform())
			{
				PushClient.Initialize();
			}
		}

		public static void Destroy()
		{
			if (pushClient != null)
			{
				pushClient = null;
			}
		}

		public static void SetCustomSmallIcon(string pathToResource)
		{
			PushClient.SetCustomSmallIcon(pathToResource);
		}

		public static void SetCustomLargeIcon(string pathToResource)
		{
			PushClient.SetCustomLargeIcon(pathToResource);
		}

		[CompilerGenerated]
		private static void _003CInitialize_003Eb__0()
		{
			Initialize();
		}
	}
}
