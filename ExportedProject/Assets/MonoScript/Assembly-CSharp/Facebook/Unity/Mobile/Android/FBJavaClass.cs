using System;
using UnityEngine;

namespace Facebook.Unity.Mobile.Android
{
	internal class FBJavaClass : IAndroidJavaClass
	{
		private const string FacebookJavaClassName = "com.facebook.unity.FB";

		private AndroidJavaClass facebookJavaClass = new AndroidJavaClass("com.facebook.unity.FB");

		public FBJavaClass()
		{
		}

		public T CallStatic<T>(string methodName)
		{
			return this.facebookJavaClass.CallStatic<T>(methodName, new object[0]);
		}

		public void CallStatic(string methodName, params object[] args)
		{
			this.facebookJavaClass.CallStatic(methodName, args);
		}
	}
}