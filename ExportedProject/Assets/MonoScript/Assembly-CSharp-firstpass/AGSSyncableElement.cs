using System;
using System.Collections.Generic;
using UnityEngine;

public class AGSSyncableElement : AGSSyncable
{
	public AGSSyncableElement(AmazonJavaWrapper javaObject) : base(javaObject)
	{
	}

	public AGSSyncableElement(AndroidJavaObject javaObject) : base(javaObject)
	{
	}

	public Dictionary<string, string> GetMetadata()
	{
		Dictionary<string, string> strs = new Dictionary<string, string>();
		AndroidJNI.PushLocalFrame(10);
		AndroidJavaObject androidJavaObject = this.javaObject.Call<AndroidJavaObject>("getMetadata", new object[0]);
		if (androidJavaObject == null)
		{
			AGSClient.LogGameCircleError("Whispersync element was unable to retrieve metadata java map");
			return strs;
		}
		AndroidJavaObject androidJavaObject1 = androidJavaObject.Call<AndroidJavaObject>("keySet", new object[0]);
		if (androidJavaObject1 == null)
		{
			AGSClient.LogGameCircleError("Whispersync element was unable to retrieve java keyset");
			return strs;
		}
		AndroidJavaObject androidJavaObject2 = androidJavaObject1.Call<AndroidJavaObject>("iterator", new object[0]);
		if (androidJavaObject2 == null)
		{
			AGSClient.LogGameCircleError("Whispersync element was unable to retrieve java iterator");
			return strs;
		}
		while (androidJavaObject2.Call<bool>("hasNext", new object[0]))
		{
			string str = androidJavaObject2.Call<string>("next", new object[0]);
			if (str == null)
			{
				continue;
			}
			string str1 = androidJavaObject.Call<string>("get", new object[] { str });
			if (str1 == null)
			{
				continue;
			}
			strs.Add(str, str1);
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return strs;
	}

	public long GetTimestamp()
	{
		return this.javaObject.Call<long>("getTimestamp", new object[0]);
	}
}