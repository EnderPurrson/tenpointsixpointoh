using System;
using System.Collections.Generic;
using UnityEngine;

public class AGSSyncableNumberList : AGSSyncableList
{
	public AGSSyncableNumberList(AmazonJavaWrapper javaObject) : base(javaObject)
	{
	}

	public AGSSyncableNumberList(AndroidJavaObject javaObject) : base(javaObject)
	{
	}

	public void Add(long val)
	{
		this.javaObject.Call("add", new object[] { val });
	}

	public void Add(double val)
	{
		this.javaObject.Call("add", new object[] { val });
	}

	public void Add(int val)
	{
		this.javaObject.Call("add", new object[] { val });
	}

	public void Add(long val, Dictionary<string, string> metadata)
	{
		this.javaObject.Call("add", new object[] { val, base.DictionaryToAndroidHashMap(metadata) });
	}

	public void Add(double val, Dictionary<string, string> metadata)
	{
		this.javaObject.Call("add", new object[] { val, base.DictionaryToAndroidHashMap(metadata) });
	}

	public void Add(int val, Dictionary<string, string> metadata)
	{
		this.javaObject.Call("add", new object[] { val, base.DictionaryToAndroidHashMap(metadata) });
	}

	public AGSSyncableNumberElement[] GetValues()
	{
		AndroidJNI.PushLocalFrame(10);
		AndroidJavaObject[] androidJavaObjectArray = this.javaObject.Call<AndroidJavaObject[]>("getValues", new object[0]);
		if (androidJavaObjectArray == null || (int)androidJavaObjectArray.Length == 0)
		{
			return null;
		}
		AGSSyncableNumberElement[] aGSSyncableNumber = new AGSSyncableNumberElement[(int)androidJavaObjectArray.Length];
		for (int i = 0; i < (int)androidJavaObjectArray.Length; i++)
		{
			aGSSyncableNumber[i] = new AGSSyncableNumber(androidJavaObjectArray[i]);
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return aGSSyncableNumber;
	}
}