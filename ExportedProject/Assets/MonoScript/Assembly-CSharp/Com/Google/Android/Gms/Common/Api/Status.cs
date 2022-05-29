using Google.Developers;
using System;

namespace Com.Google.Android.Gms.Common.Api
{
	public class Status : JavaObjWrapper, Result
	{
		private const string CLASS_NAME = "com/google/android/gms/common/api/Status";

		public static int CONTENTS_FILE_DESCRIPTOR
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/api/Status", "CONTENTS_FILE_DESCRIPTOR");
			}
		}

		public static object CREATOR
		{
			get
			{
				return JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/common/api/Status", "CREATOR", "Landroid/os/Parcelable$Creator;");
			}
		}

		public static string NULL
		{
			get
			{
				return JavaObjWrapper.GetStaticStringField("com/google/android/gms/common/api/Status", "NULL");
			}
		}

		public static int PARCELABLE_WRITE_RETURN_VALUE
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/api/Status", "PARCELABLE_WRITE_RETURN_VALUE");
			}
		}

		public Status(IntPtr ptr) : base(ptr)
		{
		}

		public Status(int arg_int_1, string arg_string_2, object arg_object_3)
		{
			base.CreateInstance("com/google/android/gms/common/api/Status", new object[] { arg_int_1, arg_string_2, arg_object_3 });
		}

		public Status(int arg_int_1, string arg_string_2)
		{
			base.CreateInstance("com/google/android/gms/common/api/Status", new object[] { arg_int_1, arg_string_2 });
		}

		public Status(int arg_int_1)
		{
			base.CreateInstance("com/google/android/gms/common/api/Status", new object[] { arg_int_1 });
		}

		public int describeContents()
		{
			return base.InvokeCall<int>("describeContents", "()I", new object[0]);
		}

		public bool equals(object arg_object_1)
		{
			return base.InvokeCall<bool>("equals", "(Ljava/lang/Object;)Z", new object[] { arg_object_1 });
		}

		public object getResolution()
		{
			return base.InvokeCall<object>("getResolution", "()Landroid/app/PendingIntent;", new object[0]);
		}

		public Status getStatus()
		{
			return base.InvokeCall<Status>("getStatus", "()Lcom/google/android/gms/common/api/Status;", new object[0]);
		}

		public int getStatusCode()
		{
			return base.InvokeCall<int>("getStatusCode", "()I", new object[0]);
		}

		public string getStatusMessage()
		{
			return base.InvokeCall<string>("getStatusMessage", "()Ljava/lang/String;", new object[0]);
		}

		public int hashCode()
		{
			return base.InvokeCall<int>("hashCode", "()I", new object[0]);
		}

		public bool hasResolution()
		{
			return base.InvokeCall<bool>("hasResolution", "()Z", new object[0]);
		}

		public bool isCanceled()
		{
			return base.InvokeCall<bool>("isCanceled", "()Z", new object[0]);
		}

		public bool isInterrupted()
		{
			return base.InvokeCall<bool>("isInterrupted", "()Z", new object[0]);
		}

		public bool isSuccess()
		{
			return base.InvokeCall<bool>("isSuccess", "()Z", new object[0]);
		}

		public void startResolutionForResult(object arg_object_1, int arg_int_2)
		{
			base.InvokeCallVoid("startResolutionForResult", "(Landroid/app/Activity;I)V", new object[] { arg_object_1, arg_int_2 });
		}

		public string toString()
		{
			return base.InvokeCall<string>("toString", "()Ljava/lang/String;", new object[0]);
		}

		public void writeToParcel(object arg_object_1, int arg_int_2)
		{
			base.InvokeCallVoid("writeToParcel", "(Landroid/os/Parcel;I)V", new object[] { arg_object_1, arg_int_2 });
		}
	}
}