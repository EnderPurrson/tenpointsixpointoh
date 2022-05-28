using System;
using System.Reflection;
using UnityEngine;

namespace DevToDev.Core.Serialization
{
	public class NativeDataLoaderPlatform
	{
		public static string Load(string applicationKey)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.devtodev.nativedata.NativeDataLoader");
			return androidJavaClass.CallStatic<string>("GetNativeData", new object[1] { applicationKey });
		}

		public static void Clear(string applicationKey)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.devtodev.nativedata.NativeDataLoader");
			androidJavaClass.CallStatic("RemoveNativeData", applicationKey);
			if (Application.platform == RuntimePlatform.OSXPlayer)
			{
				global::System.Type type = global::System.Type.GetType("DevToDev.MacOSHelper, Assembly-CSharp");
				((MethodBase)type.GetMethod("dtd_j", (BindingFlags)24)).Invoke((object)default(object), new object[1] { applicationKey });
			}
		}
	}
}
