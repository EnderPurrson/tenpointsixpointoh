using System;
using UnityEngine;

public class loaddex
{
	private static AndroidJavaObject _plugin;

	static loaddex()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.appodeal.loaddex.LoadDex"))
		{
			loaddex._plugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
		}
	}

	public loaddex()
	{
	}

	private static void load()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		loaddex._plugin.Call("loadDex", new object[0]);
	}

	public static void loadDex()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				@static.Call("runOnUiThread", new object[] { new AndroidJavaRunnable(loaddex.load) });
			}
		}
	}
}