using System;
using System.Reflection;
using UnityEngine;

namespace DevToDev.Core.Utils
{
	public class InitializationPlatform
	{
		public static void StartSessionTracker(string gameObjectName)
		{
			if (Application.platform == RuntimePlatform.OSXPlayer)
			{
				global::System.Type type = global::System.Type.GetType("DevToDev.MacOSHelper, Assembly-CSharp");
				((MethodBase)type.GetMethod("dtd_z", (BindingFlags)24)).Invoke((object)default(object), (object[])default(object[]));
			}
		}

		public static string GetReferral()
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.devtodev.InstallReceiver");
			return androidJavaClass.CallStatic<string>("getReferral", new object[0]);
		}
	}
}
