using System;
using System.Collections.Generic;
using UnityEngine;

public class AGSSyncableNumber : AGSSyncableNumberElement
{
	public AGSSyncableNumber(AmazonJavaWrapper javaObject) : base(javaObject)
	{
	}

	public AGSSyncableNumber(AndroidJavaObject javaObject) : base(javaObject)
	{
	}

	public bool IsSet()
	{
		return this.javaObject.Call<bool>("isSet", new object[0]);
	}

	public void Set(long val)
	{
		this.javaObject.Call("set", new object[] { val });
	}

	public void Set(double val)
	{
		this.javaObject.Call("set", new object[] { val });
	}

	public void Set(int val)
	{
		this.javaObject.Call("set", new object[] { val });
	}

	public void Set(string val)
	{
		this.javaObject.Call("set", new object[] { val });
	}

	public void Set(long val, Dictionary<string, string> metadata)
	{
		this.javaObject.Call("set", new object[] { val, base.DictionaryToAndroidHashMap(metadata) });
	}

	public void Set(double val, Dictionary<string, string> metadata)
	{
		this.javaObject.Call("set", new object[] { val, base.DictionaryToAndroidHashMap(metadata) });
	}

	public void Set(int val, Dictionary<string, string> metadata)
	{
		this.javaObject.Call("set", new object[] { val, base.DictionaryToAndroidHashMap(metadata) });
	}

	public void Set(string val, Dictionary<string, string> metadata)
	{
		this.javaObject.Call("set", new object[] { val, base.DictionaryToAndroidHashMap(metadata) });
	}
}