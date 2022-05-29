using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Prime31
{
	public class GoogleCloudMessaging
	{
		private static AndroidJavaObject _plugin;

		static GoogleCloudMessaging()
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.prime31.GoogleCloudMessagingPlugin"))
			{
				GoogleCloudMessaging._plugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
			}
		}

		public GoogleCloudMessaging()
		{
		}

		public static void cancelAll()
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}
			GoogleCloudMessaging._plugin.Call("cancelAll", new object[0]);
		}

		public static void checkForNotifications()
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}
			GoogleCloudMessaging._plugin.Call("checkForNotifications", new object[0]);
		}

		public static void register(string gcmSenderId)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}
			GoogleCloudMessaging._plugin.Call("register", new object[] { gcmSenderId });
		}

		[DebuggerHidden]
		public static IEnumerator registerDeviceWithPushIO(string deviceId, string pushIOApiKey, List<string> pushIOCategories, Action<bool, string> completionHandler)
		{
			GoogleCloudMessaging.u003cregisterDeviceWithPushIOu003ec__Iterator8 variable = null;
			return variable;
		}

		public static void setPushNotificationAlternateKey(string originalKey, string alternateKey)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}
			GoogleCloudMessaging._plugin.Call("setPushNotificationAlternateKey", new object[] { originalKey, alternateKey });
		}

		public static void setPushNotificationDefaultValueForKey(string key, string value)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}
			GoogleCloudMessaging._plugin.Call("setPushNotificationDefaultValueForKey", new object[] { key, value });
		}

		public static void unRegister()
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}
			GoogleCloudMessaging._plugin.Call("unRegister", new object[0]);
		}
	}
}