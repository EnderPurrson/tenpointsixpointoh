using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.PInvoke
{
	internal static class Callbacks
	{
		internal readonly static Action<CommonErrorStatus.UIStatus> NoopUICallback;

		static Callbacks()
		{
			Callbacks.NoopUICallback = (CommonErrorStatus.UIStatus status) => Logger.d(string.Concat("Received UI callback: ", status));
		}

		internal static void AsCoroutine(IEnumerator routine)
		{
			PlayGamesHelperObject.RunCoroutine(routine);
		}

		internal static Action<T> AsOnGameThreadCallback<T>(Action<T> toInvokeOnGameThread)
		{
			return (T result) => {
				if (toInvokeOnGameThread == null)
				{
					return;
				}
				PlayGamesHelperObject.RunOnGameThread(() => toInvokeOnGameThread(result));
			};
		}

		internal static Action<T1, T2> AsOnGameThreadCallback<T1, T2>(Action<T1, T2> toInvokeOnGameThread)
		{
			return (T1 result1, T2 result2) => {
				if (toInvokeOnGameThread == null)
				{
					return;
				}
				PlayGamesHelperObject.RunOnGameThread(() => toInvokeOnGameThread(result1, result2));
			};
		}

		[MonoPInvokeCallback(typeof(Callbacks.ShowUICallbackInternal))]
		internal static void InternalShowUICallback(CommonErrorStatus.UIStatus status, IntPtr data)
		{
			Logger.d(string.Concat("Showing UI Internal callback: ", status));
			Action<CommonErrorStatus.UIStatus> tempCallback = Callbacks.IntPtrToTempCallback<Action<CommonErrorStatus.UIStatus>>(data);
			try
			{
				tempCallback(status);
			}
			catch (Exception exception)
			{
				Logger.e(string.Concat("Error encountered executing InternalShowAllUICallback. Smothering to avoid passing exception into Native: ", exception));
			}
		}

		internal static byte[] IntPtrAndSizeToByteArray(IntPtr data, UIntPtr dataLength)
		{
			unsafe
			{
				if (dataLength.ToUInt64() == 0)
				{
					return null;
				}
				byte[] numArray = new byte[dataLength.ToUInt32()];
				Marshal.Copy(data, numArray, 0, (int)dataLength.ToUInt32());
				return numArray;
			}
		}

		private static T IntPtrToCallback<T>(IntPtr handle, bool unpinHandle)
		where T : class
		{
			T target;
			if (PInvokeUtilities.IsNull(handle))
			{
				return (T)null;
			}
			GCHandle gCHandle = GCHandle.FromIntPtr(handle);
			try
			{
				try
				{
					target = (T)gCHandle.Target;
				}
				catch (InvalidCastException invalidCastException1)
				{
					InvalidCastException invalidCastException = invalidCastException1;
					Logger.e(string.Concat(new object[] { "GC Handle pointed to unexpected type: ", gCHandle.Target.ToString(), ". Expected ", typeof(T) }));
					throw invalidCastException;
				}
			}
			finally
			{
				if (unpinHandle)
				{
					gCHandle.Free();
				}
			}
			return target;
		}

		internal static T IntPtrToPermanentCallback<T>(IntPtr handle)
		where T : class
		{
			return Callbacks.IntPtrToCallback<T>(handle, false);
		}

		internal static T IntPtrToTempCallback<T>(IntPtr handle)
		where T : class
		{
			return Callbacks.IntPtrToCallback<T>(handle, true);
		}

		internal static void PerformInternalCallback(string callbackName, Callbacks.Type callbackType, IntPtr response, IntPtr userData)
		{
			Logger.d(string.Concat("Entering internal callback for ", callbackName));
			Action<IntPtr> action = (callbackType != Callbacks.Type.Permanent ? Callbacks.IntPtrToTempCallback<Action<IntPtr>>(userData) : Callbacks.IntPtrToPermanentCallback<Action<IntPtr>>(userData));
			if (action == null)
			{
				return;
			}
			try
			{
				action(response);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				Logger.e(string.Concat(new object[] { "Error encountered executing ", callbackName, ". Smothering to avoid passing exception into Native: ", exception }));
			}
		}

		internal static void PerformInternalCallback<T>(string callbackName, Callbacks.Type callbackType, T param1, IntPtr param2, IntPtr userData)
		{
			Logger.d(string.Concat("Entering internal callback for ", callbackName));
			Action<T, IntPtr> action = null;
			try
			{
				action = (callbackType != Callbacks.Type.Permanent ? Callbacks.IntPtrToTempCallback<Action<T, IntPtr>>(userData) : Callbacks.IntPtrToPermanentCallback<Action<T, IntPtr>>(userData));
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				Logger.e(string.Concat(new object[] { "Error encountered converting ", callbackName, ". Smothering to avoid passing exception into Native: ", exception }));
				return;
			}
			Logger.d("Internal Callback converted to action");
			if (action == null)
			{
				return;
			}
			try
			{
				action(param1, param2);
			}
			catch (Exception exception3)
			{
				Exception exception2 = exception3;
				Logger.e(string.Concat(new object[] { "Error encountered executing ", callbackName, ". Smothering to avoid passing exception into Native: ", exception2 }));
			}
		}

		internal static IntPtr ToIntPtr<T>(Action<T> callback, Func<IntPtr, T> conversionFunction)
		where T : BaseReferenceHolder
		{
			return Callbacks.ToIntPtr(new Action<IntPtr>((IntPtr result) => {
				using (T t = conversionFunction(result))
				{
					if (callback != null)
					{
						callback(t);
					}
				}
			}));
		}

		internal static IntPtr ToIntPtr<T, P>(Action<T, P> callback, Func<IntPtr, P> conversionFunction)
		where P : BaseReferenceHolder
		{
			return Callbacks.ToIntPtr(new Action<T, IntPtr>((T param1, IntPtr param2) => {
				using (P p = conversionFunction(param2))
				{
					if (callback != null)
					{
						callback(param1, p);
					}
				}
			}));
		}

		internal static IntPtr ToIntPtr(Delegate callback)
		{
			if (callback == null)
			{
				return IntPtr.Zero;
			}
			return GCHandle.ToIntPtr(GCHandle.Alloc(callback));
		}

		internal delegate void ShowUICallbackInternal(CommonErrorStatus.UIStatus status, IntPtr data);

		internal enum Type
		{
			Permanent,
			Temporary
		}
	}
}