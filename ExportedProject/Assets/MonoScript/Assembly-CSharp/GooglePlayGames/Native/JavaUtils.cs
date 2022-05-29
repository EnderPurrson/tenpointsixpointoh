using GooglePlayGames.OurUtils;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GooglePlayGames.Native
{
	internal static class JavaUtils
	{
		private static ConstructorInfo IntPtrConstructor;

		static JavaUtils()
		{
			JavaUtils.IntPtrConstructor = typeof(AndroidJavaObject).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(IntPtr) }, null);
		}

		internal static AndroidJavaObject JavaObjectFromPointer(IntPtr jobject)
		{
			if (jobject == IntPtr.Zero)
			{
				return null;
			}
			return (AndroidJavaObject)JavaUtils.IntPtrConstructor.Invoke(new object[] { jobject });
		}

		internal static AndroidJavaObject NullSafeCall(this AndroidJavaObject target, string methodName, params object[] args)
		{
			AndroidJavaObject androidJavaObject;
			try
			{
				androidJavaObject = target.Call<AndroidJavaObject>(methodName, args);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				if (!exception.Message.Contains("null"))
				{
					GooglePlayGames.OurUtils.Logger.w(string.Concat("CallObjectMethod exception: ", exception));
					androidJavaObject = null;
				}
				else
				{
					androidJavaObject = null;
				}
			}
			return androidJavaObject;
		}
	}
}