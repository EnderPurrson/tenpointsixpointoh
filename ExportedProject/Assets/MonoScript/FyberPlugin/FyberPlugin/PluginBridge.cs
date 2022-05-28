using UnityEngine;

namespace FyberPlugin
{
	public sealed class PluginBridge
	{
		public static IPluginBridge bridge;

		static PluginBridge()
		{
			Resources.Load("FyberSettings");
		}

		internal static void Start(string json)
		{
			if (bridge != null)
			{
				bridge.StartSDK(json);
			}
		}

		internal static void Cache(string action)
		{
			if (bridge != null)
			{
				bridge.Cache(action);
			}
		}

		internal static void Request(string json)
		{
			if (bridge != null)
			{
				bridge.Request(json);
			}
		}

		internal static void StartAd(string json)
		{
			if (bridge != null)
			{
				bridge.StartAd(json);
			}
		}

		internal static bool Banner(string json)
		{
			if (bridge != null)
			{
				return bridge.Banner(json);
			}
			return false;
		}

		internal static void Report(string json)
		{
			if (bridge != null)
			{
				bridge.Report(json);
			}
		}

		internal static void Settings(string json)
		{
			if (bridge != null)
			{
				bridge.Settings(json);
			}
		}

		internal static void EnableLogging(bool shouldLog)
		{
			if (bridge != null)
			{
				bridge.EnableLogging(shouldLog);
			}
		}
	}
}
