using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AGSWhispersyncClient : MonoBehaviour
{
	private static AmazonJavaWrapper javaObject;

	private readonly static string PROXY_CLASS_NAME;

	public static string failReason;

	static AGSWhispersyncClient()
	{
		AGSWhispersyncClient.PROXY_CLASS_NAME = "com.amazon.ags.api.unity.WhispersyncClientProxyImpl";
		AGSWhispersyncClient.javaObject = new AmazonJavaWrapper();
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(AGSWhispersyncClient.PROXY_CLASS_NAME))
		{
			if (androidJavaClass.GetRawClass() != IntPtr.Zero)
			{
				AGSWhispersyncClient.javaObject.setAndroidJavaObject(androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]));
			}
			else
			{
				AGSClient.LogGameCircleWarning(string.Concat("No java class ", AGSWhispersyncClient.PROXY_CLASS_NAME, " present, can't use AGSWhispersyncClient"));
			}
		}
	}

	public AGSWhispersyncClient()
	{
	}

	public static void Flush()
	{
		AGSWhispersyncClient.javaObject.Call("flush", new object[0]);
	}

	public static AGSGameDataMap GetGameData()
	{
		AndroidJavaObject androidJavaObject = AGSWhispersyncClient.javaObject.Call<AndroidJavaObject>("getGameData", new object[0]);
		if (androidJavaObject == null)
		{
			return null;
		}
		return new AGSGameDataMap(new AmazonJavaWrapper(androidJavaObject));
	}

	public static void OnAlreadySynchronized()
	{
		if (AGSWhispersyncClient.OnAlreadySynchronizedEvent != null)
		{
			AGSWhispersyncClient.OnAlreadySynchronizedEvent();
		}
	}

	public static void OnDataUploadedToCloud()
	{
		if (AGSWhispersyncClient.OnDataUploadedToCloudEvent != null)
		{
			AGSWhispersyncClient.OnDataUploadedToCloudEvent();
		}
	}

	public static void OnDiskWriteComplete()
	{
		if (AGSWhispersyncClient.OnDiskWriteCompleteEvent != null)
		{
			AGSWhispersyncClient.OnDiskWriteCompleteEvent();
		}
	}

	public static void OnFirstSynchronize()
	{
		if (AGSWhispersyncClient.OnFirstSynchronizeEvent != null)
		{
			AGSWhispersyncClient.OnFirstSynchronizeEvent();
		}
	}

	public static void OnNewCloudData()
	{
		if (AGSWhispersyncClient.OnNewCloudDataEvent != null)
		{
			AGSWhispersyncClient.OnNewCloudDataEvent();
		}
	}

	public static void OnSyncFailed(string failReason)
	{
		AGSWhispersyncClient.failReason = failReason;
		if (AGSWhispersyncClient.OnSyncFailedEvent != null)
		{
			AGSWhispersyncClient.OnSyncFailedEvent();
		}
	}

	public static void OnThrottled()
	{
		if (AGSWhispersyncClient.OnThrottledEvent != null)
		{
			AGSWhispersyncClient.OnThrottledEvent();
		}
	}

	public static void Synchronize()
	{
		AGSWhispersyncClient.javaObject.Call("synchronize", new object[0]);
	}

	public static event Action OnAlreadySynchronizedEvent;

	public static event Action OnDataUploadedToCloudEvent;

	public static event Action OnDiskWriteCompleteEvent;

	public static event Action OnFirstSynchronizeEvent;

	public static event Action OnNewCloudDataEvent;

	public static event Action OnSyncFailedEvent;

	public static event Action OnThrottledEvent;
}