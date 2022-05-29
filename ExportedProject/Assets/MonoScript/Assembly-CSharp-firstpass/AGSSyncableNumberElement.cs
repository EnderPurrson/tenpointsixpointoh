using System;
using UnityEngine;

public class AGSSyncableNumberElement : AGSSyncableElement
{
	public AGSSyncableNumberElement(AmazonJavaWrapper javaObject) : base(javaObject)
	{
	}

	public AGSSyncableNumberElement(AndroidJavaObject javaObject) : base(javaObject)
	{
	}

	public double AsDouble()
	{
		return this.javaObject.Call<double>("asDouble", new object[0]);
	}

	public int AsInt()
	{
		return this.javaObject.Call<int>("asInt", new object[0]);
	}

	public long AsLong()
	{
		return this.javaObject.Call<long>("asLong", new object[0]);
	}

	public string AsString()
	{
		return this.javaObject.Call<string>("asString", new object[0]);
	}
}