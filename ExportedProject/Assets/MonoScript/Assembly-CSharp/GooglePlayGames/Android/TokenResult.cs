using Com.Google.Android.Gms.Common.Api;
using Google.Developers;
using System;

namespace GooglePlayGames.Android
{
	internal class TokenResult : JavaObjWrapper, Result
	{
		public TokenResult(IntPtr ptr) : base(ptr)
		{
		}

		public string getAccessToken()
		{
			return base.InvokeCall<string>("getAccessToken", "()Ljava/lang/String;", new object[0]);
		}

		public string getEmail()
		{
			return base.InvokeCall<string>("getEmail", "()Ljava/lang/String;", new object[0]);
		}

		public string getIdToken()
		{
			return base.InvokeCall<string>("getIdToken", "()Ljava/lang/String;", new object[0]);
		}

		public Status getStatus()
		{
			IntPtr intPtr = base.InvokeCall<IntPtr>("getStatus", "()Lcom/google/android/gms/common/api/Status;", new object[0]);
			return new Status(intPtr);
		}
	}
}