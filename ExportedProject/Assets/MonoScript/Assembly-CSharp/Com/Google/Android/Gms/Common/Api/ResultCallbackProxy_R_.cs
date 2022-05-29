using Google.Developers;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Com.Google.Android.Gms.Common.Api
{
	public abstract class ResultCallbackProxy<R> : JavaInterfaceProxy, ResultCallback<R>
	where R : Result
	{
		private const string CLASS_NAME = "com/google/android/gms/common/api/ResultCallback";

		public ResultCallbackProxy() : base("com/google/android/gms/common/api/ResultCallback")
		{
		}

		public void onResult(R arg_Result_1)
		{
			this.OnResult(arg_Result_1);
		}

		public void onResult(AndroidJavaObject arg_Result_1)
		{
			R r;
			IntPtr rawObject = arg_Result_1.GetRawObject();
			ConstructorInfo constructor = typeof(R).GetConstructor(new Type[] { rawObject.GetType() });
			if (constructor == null)
			{
				ConstructorInfo constructorInfo = typeof(R).GetConstructor(new Type[0]);
				r = (R)constructorInfo.Invoke(new object[0]);
				Marshal.PtrToStructure(rawObject, r);
			}
			else
			{
				r = (R)constructor.Invoke(new object[] { rawObject });
			}
			this.OnResult(r);
		}

		public abstract void OnResult(R arg_Result_1);
	}
}