using System;
using System.Collections.Generic;
using UnityEngine;

public class AGSSyncableString : AGSSyncableStringElement
{
	public AGSSyncableString(AmazonJavaWrapper javaObject) : base(javaObject)
	{
	}

	public AGSSyncableString(AndroidJavaObject javaObject) : base(javaObject)
	{
	}

	public bool IsSet()
	{
		return this.javaObject.Call<bool>("isSet", new object[0]);
	}

	public void Set(string val)
	{
		this.javaObject.Call("set", new object[] { val });
	}

	public void Set(string val, Dictionary<string, string> metadata)
	{
		this.javaObject.Call("set", new object[] { val, base.DictionaryToAndroidHashMap(metadata) });
	}
}