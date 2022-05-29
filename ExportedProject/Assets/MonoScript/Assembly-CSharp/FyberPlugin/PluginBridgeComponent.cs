using System;
using UnityEngine;

namespace FyberPlugin
{
	internal class PluginBridgeComponent : IPluginBridge
	{
		static PluginBridgeComponent()
		{
			FyberGameObject.Init();
		}

		public PluginBridgeComponent()
		{
		}

		public bool Banner(string json)
		{
			bool flag;
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.fyber.unity.ads.AdWrapper", new object[0]))
			{
				flag = androidJavaObject.CallStatic<bool>("performAdActions", new object[] { json });
			}
			return flag;
		}

		public void Cache(string action)
		{
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.fyber.unity.cache.CacheWrapper", new object[0]))
			{
				androidJavaObject.CallStatic(action, new object[0]);
			}
		}

		public void EnableLogging(bool shouldLog)
		{
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.fyber.utils.FyberLogger", new object[0]))
			{
				androidJavaObject.CallStatic<bool>("enableLogging", new object[] { shouldLog });
			}
		}

		public void Report(string json)
		{
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.fyber.unity.reporters.ReporterWrapper", new object[0]))
			{
				androidJavaObject.CallStatic("report", new object[] { json });
			}
		}

		public void Request(string json)
		{
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.fyber.unity.requesters.RequesterWrapper", new object[0]))
			{
				androidJavaObject.CallStatic("request", new object[] { json });
			}
		}

		public void Settings(string json)
		{
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.fyber.unity.settings.SettingsWrapper", new object[0]))
			{
				androidJavaObject.CallStatic("perform", new object[] { json });
			}
		}

		public void StartAd(string json)
		{
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.fyber.unity.ads.AdWrapper", new object[0]))
			{
				androidJavaObject.CallStatic("start", new object[] { json });
			}
		}

		public void StartSDK(string json)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.fyber.mediation.MediationAdapterStarter"))
			{
				FyberSettings instance = FyberSettings.Instance;
				androidJavaClass.CallStatic("setup", new object[] { instance.BundlesInfoJson(), instance.BundlesCount() });
			}
			using (AndroidJavaClass androidJavaClass1 = new AndroidJavaClass("com.fyber.mediation.MediationConfigProvider"))
			{
				FyberSettings fyberSetting = FyberSettings.Instance;
				androidJavaClass1.CallStatic("setup", new object[] { fyberSetting.BundlesConfigJson() });
			}
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.fyber.unity.FyberPlugin", new object[0]))
			{
				androidJavaObject.CallStatic("setPluginParameters", new object[] { "8.3.0", Application.unityVersion });
				androidJavaObject.CallStatic("start", new object[] { json });
			}
		}
	}
}