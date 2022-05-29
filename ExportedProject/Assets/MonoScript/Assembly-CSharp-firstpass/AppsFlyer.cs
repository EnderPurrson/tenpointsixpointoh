using System;
using System.Collections.Generic;
using UnityEngine;

public class AppsFlyer : MonoBehaviour
{
	private static AndroidJavaClass obj;

	private static AndroidJavaObject cls_AppsFlyer;

	private static AndroidJavaClass cls_AppsFlyerHelper;

	private static string devKey;

	static AppsFlyer()
	{
		AppsFlyer.obj = new AndroidJavaClass("com.appsflyer.AppsFlyerLib");
		AppsFlyer.cls_AppsFlyer = AppsFlyer.obj.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
		AppsFlyer.cls_AppsFlyerHelper = new AndroidJavaClass("com.appsflyer.AppsFlyerUnityHelper");
	}

	public AppsFlyer()
	{
	}

	public static void createValidateInAppListener(string aObject, string callbackMethod, string callbackFailedMethod)
	{
		MonoBehaviour.print("AF.cs createValidateInAppListener called");
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				AppsFlyer.cls_AppsFlyerHelper.CallStatic("createValidateInAppListener", new object[] { @static, aObject, callbackMethod, callbackFailedMethod });
			}
		}
	}

	public static string getAppsFlyerId()
	{
		string str;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				str = AppsFlyer.cls_AppsFlyer.Call<string>("getAppsFlyerUID", new object[] { @static });
			}
		}
		return str;
	}

	public static void getConversionData()
	{
	}

	public static void handleOpenUrl(string url, string sourceApplication, string annotation)
	{
	}

	public static void init(string key)
	{
		MonoBehaviour.print("AF.cs init");
		AppsFlyer.devKey = key;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				@static.Call("runOnUiThread", new object[] { new AndroidJavaRunnable(AppsFlyer.init_cb) });
			}
		}
	}

	private static void init_cb()
	{
		MonoBehaviour.print("AF.cs init_cb");
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				AppsFlyer.cls_AppsFlyer.Call("init", new object[] { @static, AppsFlyer.devKey });
			}
		}
	}

	public static void loadConversionData(string callbackObject, string callbackMethod, string callbackFailedMethod)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				AppsFlyer.cls_AppsFlyerHelper.CallStatic("createConversionDataListener", new object[] { @static, callbackObject, callbackMethod, callbackFailedMethod });
			}
		}
	}

	public static void setAndroidIdData(string androidIdData)
	{
		MonoBehaviour.print("AF.cs setImeiData");
		AppsFlyer.cls_AppsFlyer.Call("setAndroidIdData", new object[] { androidIdData });
	}

	public static void setAppID(string packageName)
	{
		AppsFlyer.cls_AppsFlyer.Call("setAppId", new object[] { packageName });
	}

	public static void setAppsFlyerKey(string key)
	{
		MonoBehaviour.print("AF.cs setAppsFlyerKey");
		AppsFlyer.init(key);
	}

	public static void setCollectAndroidID(bool shouldCollect)
	{
		MonoBehaviour.print("AF.cs setCollectAndroidID");
		AppsFlyer.cls_AppsFlyer.Call("setCollectAndroidID", new object[] { shouldCollect });
	}

	public static void setCollectIMEI(bool shouldCollect)
	{
		AppsFlyer.cls_AppsFlyer.Call("setCollectIMEI", new object[] { shouldCollect });
	}

	public static void setCurrencyCode(string currencyCode)
	{
		AppsFlyer.cls_AppsFlyer.Call("setCurrencyCode", new object[] { currencyCode });
	}

	public static void setCustomerUserID(string customerUserID)
	{
		AppsFlyer.cls_AppsFlyer.Call("setAppUserId", new object[] { customerUserID });
	}

	public static void setImeiData(string imeiData)
	{
		MonoBehaviour.print("AF.cs setImeiData");
		AppsFlyer.cls_AppsFlyer.Call("setImeiData", new object[] { imeiData });
	}

	public static void setIsDebug(bool isDebug)
	{
		MonoBehaviour.print("AF.cs setDebugLog");
		AppsFlyer.cls_AppsFlyer.Call("setDebugLog", new object[] { isDebug });
	}

	public static void setIsSandbox(bool isSandbox)
	{
	}

	public static void trackAppLaunch()
	{
		MonoBehaviour.print("AF.cs trackAppLaunch");
		AppsFlyer.trackEvent(null, null);
	}

	public static void trackEvent(string eventName, string eventValue)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				AppsFlyer.cls_AppsFlyer.Call("trackEvent", new object[] { @static, eventName, eventValue });
			}
		}
	}

	public static void trackRichEvent(string eventName, Dictionary<string, string> eventValues)
	{
		using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap", new object[0]))
		{
			IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
			object[] objArray = new object[2];
			foreach (KeyValuePair<string, string> eventValue in eventValues)
			{
				using (AndroidJavaObject androidJavaObject1 = new AndroidJavaObject("java.lang.String", new object[] { eventValue.Key }))
				{
					using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.String", new object[] { eventValue.Value }))
					{
						objArray[0] = androidJavaObject1;
						objArray[1] = androidJavaObject2;
						AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(objArray));
					}
				}
			}
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					AppsFlyer.cls_AppsFlyer.Call("trackEvent", new object[] { @static, eventName, androidJavaObject });
				}
			}
		}
	}

	public static void validateReceipt(string publicKey, string purchaseData, string signature, string price, string currency)
	{
		MonoBehaviour.print(string.Concat(new string[] { "AF.cs validateReceipt pk = ", publicKey, " data = ", purchaseData, "sig = ", signature }));
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				MonoBehaviour.print("inside cls_activity");
				AppsFlyer.cls_AppsFlyer.Call("validateAndTrackInAppPurchase", new object[] { @static, publicKey, signature, purchaseData, price, currency, null });
			}
		}
	}
}