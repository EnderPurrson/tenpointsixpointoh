using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AGSClient : MonoBehaviour
{
	public const string serviceName = "AmazonGameCircle";

	public const AmazonLogging.AmazonLoggingLevel errorLevel = AmazonLogging.AmazonLoggingLevel.Verbose;

	public static bool ReinitializeOnFocus;

	private static bool IsReady;

	private static bool supportsAchievements;

	private static bool supportsLeaderboards;

	private static bool supportsWhispersync;

	private static AmazonJavaWrapper JavaObject;

	private readonly static string PROXY_CLASS_NAME;

	static AGSClient()
	{
		AGSClient.PROXY_CLASS_NAME = "com.amazon.ags.api.unity.AmazonGamesClientProxyImpl";
		AGSClient.JavaObject = new AmazonJavaWrapper();
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(AGSClient.PROXY_CLASS_NAME))
		{
			if (androidJavaClass.GetRawClass() != IntPtr.Zero)
			{
				AGSClient.JavaObject.setAndroidJavaObject(androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]));
			}
			else
			{
				AGSClient.LogGameCircleWarning(string.Concat("No java class ", AGSClient.PROXY_CLASS_NAME, " present, can't use AGSClient"));
			}
		}
	}

	public AGSClient()
	{
	}

	public static void Init()
	{
		AGSClient.Init(AGSClient.supportsLeaderboards, AGSClient.supportsAchievements, AGSClient.supportsWhispersync);
	}

	public static void Init(bool supportsLeaderboards, bool supportsAchievements, bool supportsWhispersync)
	{
		AGSClient.ReinitializeOnFocus = true;
		AGSClient.supportsAchievements = supportsAchievements;
		AGSClient.supportsLeaderboards = supportsLeaderboards;
		AGSClient.supportsWhispersync = supportsWhispersync;
		AGSClient.JavaObject.Call("init", new object[] { supportsLeaderboards, supportsAchievements, supportsWhispersync });
	}

	public static bool IsServiceReady()
	{
		return AGSClient.IsReady;
	}

	public static void Log(string message)
	{
		AmazonLogging.Log(AmazonLogging.AmazonLoggingLevel.Verbose, "AmazonGameCircle", message);
	}

	public static void LogGameCircleError(string errorMessage)
	{
		AmazonLogging.LogError(AmazonLogging.AmazonLoggingLevel.Verbose, "AmazonGameCircle", errorMessage);
	}

	public static void LogGameCircleWarning(string errorMessage)
	{
		AmazonLogging.LogWarning(AmazonLogging.AmazonLoggingLevel.Verbose, "AmazonGameCircle", errorMessage);
	}

	public static void release()
	{
		AGSClient.JavaObject.Call("release", new object[0]);
	}

	public static void ServiceNotReady(string param)
	{
		AGSClient.IsReady = false;
		if (AGSClient.ServiceNotReadyEvent != null)
		{
			AGSClient.ServiceNotReadyEvent(param);
		}
	}

	public static void ServiceReady(string empty)
	{
		AGSClient.Log("Client GameCircle - Service is ready");
		AGSClient.IsReady = true;
		if (AGSClient.ServiceReadyEvent != null)
		{
			AGSClient.ServiceReadyEvent();
		}
	}

	public static void SetPopUpEnabled(bool enabled)
	{
		AGSClient.JavaObject.Call("setPopupEnabled", new object[] { enabled });
	}

	public static void SetPopUpLocation(GameCirclePopupLocation location)
	{
		AGSClient.JavaObject.Call("setPopUpLocation", new object[] { location.ToString() });
	}

	public static void ShowGameCircleOverlay()
	{
		AGSClient.JavaObject.Call("showGameCircleOverlay", new object[0]);
	}

	public static void ShowSignInPage()
	{
		AGSClient.JavaObject.Call("showSignInPage", new object[0]);
	}

	public static void Shutdown()
	{
		AGSClient.JavaObject.Call("shutdown", new object[0]);
	}

	public static event Action<string> ServiceNotReadyEvent;

	public static event Action ServiceReadyEvent;
}