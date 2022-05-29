using System;
using UnityEngine;

public class AGSSyncableStringList : AGSSyncableList
{
	public AGSSyncableStringList(AmazonJavaWrapper javaObject) : base(javaObject)
	{
	}

	public AGSSyncableStringList(AndroidJavaObject javaObject) : base(javaObject)
	{
	}

	public AGSSyncableString[] GetValues()
	{
		AndroidJNI.PushLocalFrame(10);
		AndroidJavaObject[] androidJavaObjectArray = this.javaObject.Call<AndroidJavaObject[]>("getValues", new object[0]);
		if (androidJavaObjectArray == null || (int)androidJavaObjectArray.Length == 0)
		{
			return null;
		}
		AGSSyncableString[] aGSSyncableString = new AGSSyncableString[(int)androidJavaObjectArray.Length];
		for (int i = 0; i < (int)androidJavaObjectArray.Length; i++)
		{
			aGSSyncableString[i] = new AGSSyncableString(androidJavaObjectArray[i]);
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return aGSSyncableString;
	}
}