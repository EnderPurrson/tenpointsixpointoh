using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native.PInvoke
{
	internal static class Callbacks
	{
		internal enum Type
		{
			Permanent = 0,
			Temporary = 1
		}

		internal delegate void ShowUICallbackInternal(CommonErrorStatus.UIStatus status, IntPtr data);

		[CompilerGenerated]
		private sealed class _003CToIntPtr_003Ec__AnonStorey267<T> where T : BaseReferenceHolder
		{
			internal Func<IntPtr, T> conversionFunction;

			internal Action<T> callback;

			internal void _003C_003Em__122(IntPtr result)
			{
				using (T obj = conversionFunction(result))
				{
					if (callback != null)
					{
						callback(obj);
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CToIntPtr_003Ec__AnonStorey268<T, P> where P : BaseReferenceHolder
		{
			internal Func<IntPtr, P> conversionFunction;

			internal Action<T, P> callback;

			internal void _003C_003Em__123(T param1, IntPtr param2)
			{
				using (P arg = conversionFunction(param2))
				{
					if (callback != null)
					{
						callback(param1, arg);
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CAsOnGameThreadCallback_003Ec__AnonStorey269<T>
		{
			private sealed class _003CAsOnGameThreadCallback_003Ec__AnonStorey26A
			{
				internal T result;

				internal _003CAsOnGameThreadCallback_003Ec__AnonStorey269<T> _003C_003Ef__ref_0024617;

				internal void _003C_003Em__126()
				{
					_003C_003Ef__ref_0024617.toInvokeOnGameThread(result);
				}
			}

			internal Action<T> toInvokeOnGameThread;

			internal void _003C_003Em__124(T result)
			{
				_003CAsOnGameThreadCallback_003Ec__AnonStorey26A _003CAsOnGameThreadCallback_003Ec__AnonStorey26A = new _003CAsOnGameThreadCallback_003Ec__AnonStorey26A();
				_003CAsOnGameThreadCallback_003Ec__AnonStorey26A._003C_003Ef__ref_0024617 = this;
				_003CAsOnGameThreadCallback_003Ec__AnonStorey26A.result = result;
				if (toInvokeOnGameThread != null)
				{
					PlayGamesHelperObject.RunOnGameThread(_003CAsOnGameThreadCallback_003Ec__AnonStorey26A._003C_003Em__126);
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CAsOnGameThreadCallback_003Ec__AnonStorey26B<T1, T2>
		{
			private sealed class _003CAsOnGameThreadCallback_003Ec__AnonStorey26C
			{
				internal T1 result1;

				internal T2 result2;

				internal _003CAsOnGameThreadCallback_003Ec__AnonStorey26B<T1, T2> _003C_003Ef__ref_0024619;

				internal void _003C_003Em__127()
				{
					_003C_003Ef__ref_0024619.toInvokeOnGameThread(result1, result2);
				}
			}

			internal Action<T1, T2> toInvokeOnGameThread;

			internal void _003C_003Em__125(T1 result1, T2 result2)
			{
				_003CAsOnGameThreadCallback_003Ec__AnonStorey26C _003CAsOnGameThreadCallback_003Ec__AnonStorey26C = new _003CAsOnGameThreadCallback_003Ec__AnonStorey26C();
				_003CAsOnGameThreadCallback_003Ec__AnonStorey26C._003C_003Ef__ref_0024619 = this;
				_003CAsOnGameThreadCallback_003Ec__AnonStorey26C.result1 = result1;
				_003CAsOnGameThreadCallback_003Ec__AnonStorey26C.result2 = result2;
				if (toInvokeOnGameThread != null)
				{
					PlayGamesHelperObject.RunOnGameThread(_003CAsOnGameThreadCallback_003Ec__AnonStorey26C._003C_003Em__127);
				}
			}
		}

		internal static readonly Action<CommonErrorStatus.UIStatus> NoopUICallback;

		[CompilerGenerated]
		private static Action<CommonErrorStatus.UIStatus> _003C_003Ef__am_0024cache1;

		static Callbacks()
		{
			if (_003C_003Ef__am_0024cache1 == null)
			{
				_003C_003Ef__am_0024cache1 = _003CNoopUICallback_003Em__121;
			}
			NoopUICallback = _003C_003Ef__am_0024cache1;
		}

		internal static IntPtr ToIntPtr<T>(Action<T> callback, Func<IntPtr, T> conversionFunction) where T : BaseReferenceHolder
		{
			_003CToIntPtr_003Ec__AnonStorey267<T> _003CToIntPtr_003Ec__AnonStorey = new _003CToIntPtr_003Ec__AnonStorey267<T>();
			_003CToIntPtr_003Ec__AnonStorey.conversionFunction = conversionFunction;
			_003CToIntPtr_003Ec__AnonStorey.callback = callback;
			Action<IntPtr> callback2 = _003CToIntPtr_003Ec__AnonStorey._003C_003Em__122;
			return ToIntPtr(callback2);
		}

		internal static IntPtr ToIntPtr<T, P>(Action<T, P> callback, Func<IntPtr, P> conversionFunction) where P : BaseReferenceHolder
		{
			_003CToIntPtr_003Ec__AnonStorey268<T, P> _003CToIntPtr_003Ec__AnonStorey = new _003CToIntPtr_003Ec__AnonStorey268<T, P>();
			_003CToIntPtr_003Ec__AnonStorey.conversionFunction = conversionFunction;
			_003CToIntPtr_003Ec__AnonStorey.callback = callback;
			Action<T, IntPtr> callback2 = _003CToIntPtr_003Ec__AnonStorey._003C_003Em__123;
			return ToIntPtr(callback2);
		}

		internal static IntPtr ToIntPtr(Delegate callback)
		{
			if ((object)callback == null)
			{
				return IntPtr.Zero;
			}
			GCHandle value = GCHandle.Alloc(callback);
			return GCHandle.ToIntPtr(value);
		}

		internal static T IntPtrToTempCallback<T>(IntPtr handle) where T : class
		{
			return IntPtrToCallback<T>(handle, true);
		}

		private static T IntPtrToCallback<T>(IntPtr handle, bool unpinHandle) where T : class
		{
			//Discarded unreachable code: IL_002b, IL_006f
			if (PInvokeUtilities.IsNull(handle))
			{
				return (T)default(T);
			}
			GCHandle gCHandle = GCHandle.FromIntPtr(handle);
			try
			{
				return (T)gCHandle.Target;
			}
			catch (InvalidCastException ex)
			{
				Logger.e("GC Handle pointed to unexpected type: " + gCHandle.Target.ToString() + ". Expected " + typeof(T));
				throw ex;
			}
			finally
			{
				if (unpinHandle)
				{
					gCHandle.Free();
				}
			}
		}

		internal static T IntPtrToPermanentCallback<T>(IntPtr handle) where T : class
		{
			return IntPtrToCallback<T>(handle, false);
		}

		[MonoPInvokeCallback(typeof(ShowUICallbackInternal))]
		internal static void InternalShowUICallback(CommonErrorStatus.UIStatus status, IntPtr data)
		{
			Logger.d("Showing UI Internal callback: " + status);
			Action<CommonErrorStatus.UIStatus> action = IntPtrToTempCallback<Action<CommonErrorStatus.UIStatus>>(data);
			try
			{
				action(status);
			}
			catch (Exception ex)
			{
				Logger.e("Error encountered executing InternalShowAllUICallback. Smothering to avoid passing exception into Native: " + ex);
			}
		}

		internal static void PerformInternalCallback(string callbackName, Type callbackType, IntPtr response, IntPtr userData)
		{
			Logger.d("Entering internal callback for " + callbackName);
			Action<IntPtr> action = ((callbackType != 0) ? IntPtrToTempCallback<Action<IntPtr>>(userData) : IntPtrToPermanentCallback<Action<IntPtr>>(userData));
			if (action == null)
			{
				return;
			}
			try
			{
				action(response);
			}
			catch (Exception ex)
			{
				Logger.e("Error encountered executing " + callbackName + ". Smothering to avoid passing exception into Native: " + ex);
			}
		}

		internal static void PerformInternalCallback<T>(string callbackName, Type callbackType, T param1, IntPtr param2, IntPtr userData)
		{
			//Discarded unreachable code: IL_005f
			Logger.d("Entering internal callback for " + callbackName);
			Action<T, IntPtr> action = null;
			try
			{
				action = ((callbackType != 0) ? IntPtrToTempCallback<Action<T, IntPtr>>(userData) : IntPtrToPermanentCallback<Action<T, IntPtr>>(userData));
			}
			catch (Exception ex)
			{
				Logger.e("Error encountered converting " + callbackName + ". Smothering to avoid passing exception into Native: " + ex);
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
			catch (Exception ex2)
			{
				Logger.e("Error encountered executing " + callbackName + ". Smothering to avoid passing exception into Native: " + ex2);
			}
		}

		internal static Action<T> AsOnGameThreadCallback<T>(Action<T> toInvokeOnGameThread)
		{
			_003CAsOnGameThreadCallback_003Ec__AnonStorey269<T> _003CAsOnGameThreadCallback_003Ec__AnonStorey = new _003CAsOnGameThreadCallback_003Ec__AnonStorey269<T>();
			_003CAsOnGameThreadCallback_003Ec__AnonStorey.toInvokeOnGameThread = toInvokeOnGameThread;
			return _003CAsOnGameThreadCallback_003Ec__AnonStorey._003C_003Em__124;
		}

		internal static Action<T1, T2> AsOnGameThreadCallback<T1, T2>(Action<T1, T2> toInvokeOnGameThread)
		{
			_003CAsOnGameThreadCallback_003Ec__AnonStorey26B<T1, T2> _003CAsOnGameThreadCallback_003Ec__AnonStorey26B = new _003CAsOnGameThreadCallback_003Ec__AnonStorey26B<T1, T2>();
			_003CAsOnGameThreadCallback_003Ec__AnonStorey26B.toInvokeOnGameThread = toInvokeOnGameThread;
			return _003CAsOnGameThreadCallback_003Ec__AnonStorey26B._003C_003Em__125;
		}

		internal static void AsCoroutine(IEnumerator routine)
		{
			PlayGamesHelperObject.RunCoroutine(routine);
		}

		internal static byte[] IntPtrAndSizeToByteArray(IntPtr data, UIntPtr dataLength)
		{
			if (dataLength.ToUInt64() == 0L)
			{
				return null;
			}
			byte[] array = new byte[dataLength.ToUInt32()];
			Marshal.Copy(data, array, 0, (int)dataLength.ToUInt32());
			return array;
		}

		[CompilerGenerated]
		private static void _003CNoopUICallback_003Em__121(CommonErrorStatus.UIStatus status)
		{
			Logger.d("Received UI callback: " + status);
		}
	}
}
