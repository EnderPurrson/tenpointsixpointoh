using System;
using UnityEngine;

public class AGSSyncableAccumulatingNumber : AGSSyncable
{
	public AGSSyncableAccumulatingNumber(AmazonJavaWrapper javaObject) : base(javaObject)
	{
	}

	public AGSSyncableAccumulatingNumber(AndroidJavaObject javaObject) : base(javaObject)
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

	public void Decrement(long delta)
	{
		this.javaObject.Call("decrement", new object[] { delta });
	}

	public void Decrement(double delta)
	{
		this.javaObject.Call("decrement", new object[] { delta });
	}

	public void Decrement(int delta)
	{
		this.javaObject.Call("decrement", new object[] { delta });
	}

	public void Decrement(string delta)
	{
		this.javaObject.Call("decrement", new object[] { delta });
	}

	public void Increment(long delta)
	{
		this.javaObject.Call("increment", new object[] { delta });
	}

	public void Increment(double delta)
	{
		this.javaObject.Call("increment", new object[] { delta });
	}

	public void Increment(int delta)
	{
		this.javaObject.Call("increment", new object[] { delta });
	}

	public void Increment(string delta)
	{
		this.javaObject.Call("increment", new object[] { delta });
	}
}