using Com.Google.Android.Gms.Common;
using Google.Developers;
using System;

namespace Com.Google.Android.Gms.Common.Api
{
	public class GoogleApiClient : JavaObjWrapper
	{
		private const string CLASS_NAME = "com/google/android/gms/common/api/GoogleApiClient";

		public GoogleApiClient(IntPtr ptr) : base(ptr)
		{
		}

		public GoogleApiClient() : base("com.google.android.gms.common.api.GoogleApiClient")
		{
		}

		public ConnectionResult blockingConnect(long arg_long_1, object arg_object_2)
		{
			return base.InvokeCall<ConnectionResult>("blockingConnect", "(JLjava/util/concurrent/TimeUnit;)Lcom/google/android/gms/common/ConnectionResult;", new object[] { arg_long_1, arg_object_2 });
		}

		public ConnectionResult blockingConnect()
		{
			return base.InvokeCall<ConnectionResult>("blockingConnect", "()Lcom/google/android/gms/common/ConnectionResult;", new object[0]);
		}

		public PendingResult<Status> clearDefaultAccountAndReconnect()
		{
			return base.InvokeCall<PendingResult<Status>>("clearDefaultAccountAndReconnect", "()Lcom/google/android/gms/common/api/PendingResult;", new object[0]);
		}

		public void connect()
		{
			base.InvokeCallVoid("connect", "()V", new object[0]);
		}

		public void disconnect()
		{
			base.InvokeCallVoid("disconnect", "()V", new object[0]);
		}

		public void dump(string arg_string_1, object arg_object_2, object arg_object_3, string[] arg_string_4)
		{
			base.InvokeCallVoid("dump", "(Ljava/lang/String;Ljava/io/FileDescriptor;Ljava/io/PrintWriter;[Ljava/lang/String;)V", new object[] { arg_string_1, arg_object_2, arg_object_3, arg_string_4 });
		}

		public ConnectionResult getConnectionResult(object arg_object_1)
		{
			return base.InvokeCall<ConnectionResult>("getConnectionResult", "(Lcom/google/android/gms/common/api/Api;)Lcom/google/android/gms/common/ConnectionResult;", new object[] { arg_object_1 });
		}

		public object getContext()
		{
			return base.InvokeCall<object>("getContext", "()Landroid/content/Context;", new object[0]);
		}

		public object getLooper()
		{
			return base.InvokeCall<object>("getLooper", "()Landroid/os/Looper;", new object[0]);
		}

		public int getSessionId()
		{
			return base.InvokeCall<int>("getSessionId", "()I", new object[0]);
		}

		public bool hasConnectedApi(object arg_object_1)
		{
			return base.InvokeCall<bool>("hasConnectedApi", "(Lcom/google/android/gms/common/api/Api;)Z", new object[] { arg_object_1 });
		}

		public bool isConnected()
		{
			return base.InvokeCall<bool>("isConnected", "()Z", new object[0]);
		}

		public bool isConnecting()
		{
			return base.InvokeCall<bool>("isConnecting", "()Z", new object[0]);
		}

		public bool isConnectionCallbacksRegistered(object arg_object_1)
		{
			return base.InvokeCall<bool>("isConnectionCallbacksRegistered", "(Lcom/google/android/gms/common/api/GoogleApiClient$ConnectionCallbacks;)Z", new object[] { arg_object_1 });
		}

		public bool isConnectionFailedListenerRegistered(object arg_object_1)
		{
			return base.InvokeCall<bool>("isConnectionFailedListenerRegistered", "(Lcom/google/android/gms/common/api/GoogleApiClient$OnConnectionFailedListener;)Z", new object[] { arg_object_1 });
		}

		public void reconnect()
		{
			base.InvokeCallVoid("reconnect", "()V", new object[0]);
		}

		public void registerConnectionCallbacks(object arg_object_1)
		{
			base.InvokeCallVoid("registerConnectionCallbacks", "(Lcom/google/android/gms/common/api/GoogleApiClient$ConnectionCallbacks;)V", new object[] { arg_object_1 });
		}

		public void registerConnectionFailedListener(object arg_object_1)
		{
			base.InvokeCallVoid("registerConnectionFailedListener", "(Lcom/google/android/gms/common/api/GoogleApiClient$OnConnectionFailedListener;)V", new object[] { arg_object_1 });
		}

		public void stopAutoManage(object arg_object_1)
		{
			base.InvokeCallVoid("stopAutoManage", "(Landroid/support/v4/app/FragmentActivity;)V", new object[] { arg_object_1 });
		}

		public void unregisterConnectionCallbacks(object arg_object_1)
		{
			base.InvokeCallVoid("unregisterConnectionCallbacks", "(Lcom/google/android/gms/common/api/GoogleApiClient$ConnectionCallbacks;)V", new object[] { arg_object_1 });
		}

		public void unregisterConnectionFailedListener(object arg_object_1)
		{
			base.InvokeCallVoid("unregisterConnectionFailedListener", "(Lcom/google/android/gms/common/api/GoogleApiClient$OnConnectionFailedListener;)V", new object[] { arg_object_1 });
		}
	}
}