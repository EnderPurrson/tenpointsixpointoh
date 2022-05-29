using System;
using System.Collections.Generic;
using UnityEngine;

public class AGSSyncable : IDisposable
{
	protected AmazonJavaWrapper javaObject;

	public AGSSyncable(AmazonJavaWrapper jo)
	{
		this.javaObject = jo;
	}

	public AGSSyncable(AndroidJavaObject jo)
	{
		this.javaObject = new AmazonJavaWrapper(jo);
	}

	protected AmazonJavaWrapper DictionaryToAndroidHashMap(Dictionary<string, string> dictionary)
	{
		AndroidJNI.PushLocalFrame(10);
		AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap", new object[0]);
		IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
		object[] objArray = new object[2];
		foreach (KeyValuePair<string, string> keyValuePair in dictionary)
		{
			using (AndroidJavaObject androidJavaObject1 = new AndroidJavaObject("java.lang.String", new object[] { keyValuePair.Key }))
			{
				using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.String", new object[] { keyValuePair.Value }))
				{
					objArray[0] = androidJavaObject1;
					objArray[1] = androidJavaObject2;
					jvalue[] jvalueArray = AndroidJNIHelper.CreateJNIArgArray(objArray);
					AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, jvalueArray);
				}
			}
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return new AmazonJavaWrapper(androidJavaObject);
	}

	public void Dispose()
	{
		if (this.javaObject != null)
		{
			this.javaObject.Dispose();
		}
	}

	protected T GetAGSSyncable<T>(AGSSyncable.SyncableMethod method)
	{
		return this.GetAGSSyncable<T>(method, null);
	}

	protected T GetAGSSyncable<T>(AGSSyncable.SyncableMethod method, string key)
	{
		AndroidJavaObject androidJavaObject;
		androidJavaObject = (key == null ? this.javaObject.Call<AndroidJavaObject>(method.ToString(), new object[0]) : this.javaObject.Call<AndroidJavaObject>(method.ToString(), new object[] { key }));
		if (androidJavaObject == null)
		{
			return default(T);
		}
		return (T)Activator.CreateInstance(typeof(T), new object[] { androidJavaObject });
	}

	protected HashSet<string> GetHashSet(AGSSyncable.HashSetMethod method)
	{
		AndroidJNI.PushLocalFrame(10);
		HashSet<string> strs = new HashSet<string>();
		AndroidJavaObject androidJavaObject = this.javaObject.Call<AndroidJavaObject>(method.ToString(), new object[0]);
		if (androidJavaObject == null)
		{
			return strs;
		}
		AndroidJavaObject androidJavaObject1 = androidJavaObject.Call<AndroidJavaObject>("iterator", new object[0]);
		if (androidJavaObject1 == null)
		{
			return strs;
		}
		while (androidJavaObject1.Call<bool>("hasNext", new object[0]))
		{
			string str = androidJavaObject1.Call<string>("next", new object[0]);
			strs.Add(str);
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return strs;
	}

	public enum HashSetMethod
	{
		getDeveloperStringKeys,
		getHighestNumberKeys,
		getLowestNumberKeys,
		getLatestNumberKeys,
		getHighNumberListKeys,
		getLowNumberListKeys,
		getLatestNumberListKeys,
		getAccumulatingNumberKeys,
		getLatestStringKeys,
		getLatestStringListKeys,
		getStringSetKeys,
		getMapKeys
	}

	public enum SyncableMethod
	{
		getDeveloperString,
		getHighestNumber,
		getLowestNumber,
		getLatestNumber,
		getHighNumberList,
		getLowNumberList,
		getLatestNumberList,
		getAccumulatingNumber,
		getLatestString,
		getLatestStringList,
		getStringSet,
		getMap
	}
}