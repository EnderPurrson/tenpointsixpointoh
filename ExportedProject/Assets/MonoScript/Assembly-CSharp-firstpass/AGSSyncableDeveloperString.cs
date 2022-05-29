using System;
using UnityEngine;

public class AGSSyncableDeveloperString : AGSSyncable
{
	public AGSSyncableDeveloperString(AmazonJavaWrapper javaObject) : base(javaObject)
	{
	}

	public AGSSyncableDeveloperString(AndroidJavaObject javaObject) : base(javaObject)
	{
	}

	public string getCloudValue()
	{
		return this.javaObject.Call<string>("getCloudValue", new object[0]);
	}

	public string getValue()
	{
		return this.javaObject.Call<string>("getValue", new object[0]);
	}

	public bool inConflict()
	{
		return this.javaObject.Call<bool>("inConflict", new object[0]);
	}

	public bool isSet()
	{
		return this.javaObject.Call<bool>("isSet", new object[0]);
	}

	public void markAsResolved()
	{
		this.javaObject.Call("markAsResolved", new object[0]);
	}

	public void setValue(string val)
	{
		this.javaObject.Call("setValue", new object[] { val });
	}
}