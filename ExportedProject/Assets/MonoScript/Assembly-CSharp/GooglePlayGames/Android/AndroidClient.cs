using System;
using System.Runtime.CompilerServices;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;
using UnityEngine;

namespace GooglePlayGames.Android
{
	internal class AndroidClient : IClientImpl
	{
		[CompilerGenerated]
		private sealed class _003CCreatePlatformConfiguration_003Ec__AnonStorey1FF
		{
			internal IntPtr intentRef;

			internal void _003C_003Em__83()
			{
				try
				{
					LaunchBridgeIntent(intentRef);
				}
				finally
				{
					AndroidJNI.DeleteGlobalRef(intentRef);
				}
			}
		}

		internal const string BridgeActivityClass = "com.google.games.bridge.NativeBridgeActivity";

		private const string LaunchBridgeMethod = "launchBridgeIntent";

		private const string LaunchBridgeSignature = "(Landroid/app/Activity;Landroid/content/Intent;)V";

		private TokenClient tokenClient;

		[CompilerGenerated]
		private static Action<IntPtr> _003C_003Ef__am_0024cache1;

		public PlatformConfiguration CreatePlatformConfiguration()
		{
			AndroidPlatformConfiguration androidPlatformConfiguration = AndroidPlatformConfiguration.Create();
			using (AndroidJavaObject androidJavaObject = AndroidTokenClient.GetActivity())
			{
				androidPlatformConfiguration.SetActivity(androidJavaObject.GetRawObject());
				if (_003C_003Ef__am_0024cache1 == null)
				{
					_003C_003Ef__am_0024cache1 = _003CCreatePlatformConfiguration_003Em__82;
				}
				androidPlatformConfiguration.SetOptionalIntentHandlerForUI(_003C_003Ef__am_0024cache1);
				return androidPlatformConfiguration;
			}
		}

		public TokenClient CreateTokenClient(string playerId, bool reset)
		{
			if (tokenClient == null || reset)
			{
				tokenClient = new AndroidTokenClient(playerId);
			}
			return tokenClient;
		}

		private static void LaunchBridgeIntent(IntPtr bridgedIntent)
		{
			object[] args = new object[2];
			jvalue[] array = AndroidJNIHelper.CreateJNIArgArray(args);
			try
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.games.bridge.NativeBridgeActivity"))
				{
					using (AndroidJavaObject androidJavaObject = AndroidTokenClient.GetActivity())
					{
						IntPtr staticMethodID = AndroidJNI.GetStaticMethodID(androidJavaClass.GetRawClass(), "launchBridgeIntent", "(Landroid/app/Activity;Landroid/content/Intent;)V");
						array[0].l = androidJavaObject.GetRawObject();
						array[1].l = bridgedIntent;
						AndroidJNI.CallStaticVoidMethod(androidJavaClass.GetRawClass(), staticMethodID, array);
					}
				}
			}
			catch (Exception ex)
			{
				GooglePlayGames.OurUtils.Logger.e("Exception launching bridge intent: " + ex.Message);
				GooglePlayGames.OurUtils.Logger.e(ex.ToString());
			}
			finally
			{
				AndroidJNIHelper.DeleteJNIArgArray(args, array);
			}
		}

		[CompilerGenerated]
		private static void _003CCreatePlatformConfiguration_003Em__82(IntPtr intent)
		{
			_003CCreatePlatformConfiguration_003Ec__AnonStorey1FF _003CCreatePlatformConfiguration_003Ec__AnonStorey1FF = new _003CCreatePlatformConfiguration_003Ec__AnonStorey1FF();
			_003CCreatePlatformConfiguration_003Ec__AnonStorey1FF.intentRef = AndroidJNI.NewGlobalRef(intent);
			PlayGamesHelperObject.RunOnGameThread(_003CCreatePlatformConfiguration_003Ec__AnonStorey1FF._003C_003Em__83);
		}
	}
}
