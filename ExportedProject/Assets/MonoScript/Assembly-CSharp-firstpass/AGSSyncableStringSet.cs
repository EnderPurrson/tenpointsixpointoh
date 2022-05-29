using System;
using System.Collections.Generic;
using UnityEngine;

public class AGSSyncableStringSet : AGSSyncable
{
	public AGSSyncableStringSet(AmazonJavaWrapper javaObject) : base(javaObject)
	{
	}

	public AGSSyncableStringSet(AndroidJavaObject javaObject) : base(javaObject)
	{
	}

	public void Add(string val)
	{
		this.javaObject.Call("add", new object[] { val });
	}

	public void Add(string val, Dictionary<string, string> metadata)
	{
		this.javaObject.Call("add", new object[] { val, base.DictionaryToAndroidHashMap(metadata) });
	}

	public bool Contains(string val)
	{
		return this.javaObject.Call<bool>("contains", new object[] { val });
	}

	public AGSSyncableStringElement Get(string val)
	{
		return base.GetAGSSyncable<AGSSyncableStringElement>(AGSSyncable.SyncableMethod.getStringSet, val);
	}

	public HashSet<AGSSyncableStringElement> GetValues()
	{
		AndroidJNI.PushLocalFrame(10);
		HashSet<AGSSyncableStringElement> aGSSyncableStringElements = new HashSet<AGSSyncableStringElement>();
		AndroidJavaObject androidJavaObject = this.javaObject.Call<AndroidJavaObject>("getValues", new object[0]);
		if (androidJavaObject == null)
		{
			return aGSSyncableStringElements;
		}
		AndroidJavaObject androidJavaObject1 = androidJavaObject.Call<AndroidJavaObject>("iterator", new object[0]);
		if (androidJavaObject1 == null)
		{
			return aGSSyncableStringElements;
		}
		while (androidJavaObject1.Call<bool>("hasNext", new object[0]))
		{
			AndroidJavaObject androidJavaObject2 = androidJavaObject1.Call<AndroidJavaObject>("next", new object[0]);
			if (androidJavaObject2 == null)
			{
				continue;
			}
			aGSSyncableStringElements.Add(new AGSSyncableStringElement(androidJavaObject2));
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return aGSSyncableStringElements;
	}

	public bool IsSet()
	{
		return this.javaObject.Call<bool>("isSet", new object[0]);
	}
}