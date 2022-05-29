using System;
using System.Collections.Generic;
using UnityEngine;

public class AGSSyncableList : AGSSyncable
{
	public AGSSyncableList(AmazonJavaWrapper javaObject) : base(javaObject)
	{
	}

	public AGSSyncableList(AndroidJavaObject javaObject) : base(javaObject)
	{
	}

	public void Add(string val, Dictionary<string, string> metadata)
	{
		this.javaObject.Call("add", new object[] { val, base.DictionaryToAndroidHashMap(metadata) });
	}

	public void Add(string val)
	{
		this.javaObject.Call("add", new object[] { val });
	}

	public int GetMaxSize()
	{
		return this.javaObject.Call<int>("getMaxSize", new object[0]);
	}

	public bool IsSet()
	{
		return this.javaObject.Call<bool>("isSet", new object[0]);
	}

	public void SetMaxSize(int size)
	{
		this.javaObject.Call("setMaxSize", new object[] { size });
	}
}