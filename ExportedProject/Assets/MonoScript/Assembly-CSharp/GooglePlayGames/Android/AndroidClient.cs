using GooglePlayGames;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GooglePlayGames.Android
{
	internal class AndroidClient : IClientImpl
	{
		internal const string BridgeActivityClass = "com.google.games.bridge.NativeBridgeActivity";

		private const string LaunchBridgeMethod = "launchBridgeIntent";

		private const string LaunchBridgeSignature = "(Landroid/app/Activity;Landroid/content/Intent;)V";

		private TokenClient tokenClient;

		public AndroidClient()
		{
		}

		public PlatformConfiguration CreatePlatformConfiguration()
		{
			AndroidPlatformConfiguration androidPlatformConfiguration = AndroidPlatformConfiguration.Create();
			using (AndroidJavaObject activity = AndroidTokenClient.GetActivity())
			{
				androidPlatformConfiguration.SetActivity(activity.GetRawObject());
				androidPlatformConfiguration.SetOptionalIntentHandlerForUI((IntPtr intent) => {
					IntPtr intPtr = AndroidJNI.NewGlobalRef(intent);
					PlayGamesHelperObject.RunOnGameThread(() => {
						try
						{
							AndroidClient.LaunchBridgeIntent(intPtr);
						}
						finally
						{
							AndroidJNI.DeleteGlobalRef(intPtr);
						}
					});
				});
			}
			return androidPlatformConfiguration;
		}

		public TokenClient CreateTokenClient(string playerId, bool reset)
		{
			if (this.tokenClient == null || reset)
			{
				this.tokenClient = new AndroidTokenClient(playerId);
			}
			return this.tokenClient;
		}

		private static void LaunchBridgeIntent(IntPtr bridgedIntent)
		{
			object[] objArray = new object[2];
			jvalue[] rawObject = AndroidJNIHelper.CreateJNIArgArray(objArray);
			try
			{
				try
				{
					using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.games.bridge.NativeBridgeActivity"))
					{
						using (AndroidJavaObject activity = AndroidTokenClient.GetActivity())
						{
							IntPtr staticMethodID = AndroidJNI.GetStaticMethodID(androidJavaClass.GetRawClass(), "launchBridgeIntent", "(Landroid/app/Activity;Landroid/content/Intent;)V");
							rawObject[0].l = activity.GetRawObject();
							rawObject[1].l = bridgedIntent;
							AndroidJNI.CallStaticVoidMethod(androidJavaClass.GetRawClass(), staticMethodID, rawObject);
						}
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					GooglePlayGames.OurUtils.Logger.e(string.Concat("Exception launching bridge intent: ", exception.Message));
					GooglePlayGames.OurUtils.Logger.e(exception.ToString());
				}
			}
			finally
			{
				AndroidJNIHelper.DeleteJNIArgArray(objArray, rawObject);
			}
		}
	}
}