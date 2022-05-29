using System;
using UnityEngine;

public class AmazonJavaWrapper : IDisposable
{
	private AndroidJavaObject jo;

	public AmazonJavaWrapper()
	{
	}

	public AmazonJavaWrapper(AndroidJavaObject o)
	{
		this.setAndroidJavaObject(o);
	}

	public void Call(string method, params object[] args)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJNI.PushLocalFrame((int)args.Length + 1);
			this.jo.Call(method, args);
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
	}

	public ReturnType Call<ReturnType>(string method, params object[] args)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return default(ReturnType);
		}
		AndroidJNI.PushLocalFrame((int)args.Length + 1);
		ReturnType returnType = this.jo.Call<ReturnType>(method, args);
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return returnType;
	}

	public void CallStatic(string method, params object[] args)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJNI.PushLocalFrame((int)args.Length + 1);
			this.jo.CallStatic(method, args);
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
	}

	public ReturnType CallStatic<ReturnType>(string method, params object[] args)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return default(ReturnType);
		}
		AndroidJNI.PushLocalFrame((int)args.Length + 1);
		ReturnType returnType = this.jo.CallStatic<ReturnType>(method, args);
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return returnType;
	}

	public void Dispose()
	{
		if (this.jo != null)
		{
			this.jo.Dispose();
		}
	}

	public FieldType Get<FieldType>(string fieldName)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return default(FieldType);
		}
		return this.jo.Get<FieldType>(fieldName);
	}

	public AndroidJavaObject getJavaObject()
	{
		return this.jo;
	}

	public IntPtr GetRawClass()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			return this.jo.GetRawClass();
		}
		return new IntPtr();
	}

	public IntPtr GetRawObject()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			return this.jo.GetRawObject();
		}
		return new IntPtr();
	}

	public FieldType GetStatic<FieldType>(string fieldName)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return default(FieldType);
		}
		return this.jo.GetStatic<FieldType>(fieldName);
	}

	public void Set<FieldType>(string fieldName, FieldType type)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			this.jo.Set<FieldType>(fieldName, type);
		}
	}

	public void setAndroidJavaObject(AndroidJavaObject o)
	{
		this.jo = o;
	}

	public void SetStatic<FieldType>(string fieldName, FieldType type)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			this.jo.SetStatic<FieldType>(fieldName, type);
		}
	}
}