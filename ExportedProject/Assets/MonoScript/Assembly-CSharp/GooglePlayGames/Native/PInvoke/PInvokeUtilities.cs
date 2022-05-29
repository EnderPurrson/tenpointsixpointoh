using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace GooglePlayGames.Native.PInvoke
{
	internal static class PInvokeUtilities
	{
		private readonly static DateTime UnixEpoch;

		static PInvokeUtilities()
		{
			PInvokeUtilities.UnixEpoch = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);
		}

		internal static UIntPtr ArrayToSizeT<T>(T[] array)
		{
			if (array == null)
			{
				return UIntPtr.Zero;
			}
			return new UIntPtr((ulong)((int)array.Length));
		}

		internal static HandleRef CheckNonNull(HandleRef reference)
		{
			if (PInvokeUtilities.IsNull(reference))
			{
				throw new InvalidOperationException();
			}
			return reference;
		}

		internal static DateTime FromMillisSinceUnixEpoch(long millisSinceEpoch)
		{
			DateTime unixEpoch = PInvokeUtilities.UnixEpoch;
			return unixEpoch.Add(TimeSpan.FromMilliseconds((double)millisSinceEpoch));
		}

		internal static bool IsNull(HandleRef reference)
		{
			return PInvokeUtilities.IsNull(HandleRef.ToIntPtr(reference));
		}

		internal static bool IsNull(IntPtr pointer)
		{
			return pointer.Equals(IntPtr.Zero);
		}

		internal static T[] OutParamsToArray<T>(PInvokeUtilities.OutMethod<T> outMethod)
		{
			UIntPtr uIntPtr = outMethod(null, UIntPtr.Zero);
			if (uIntPtr.Equals(UIntPtr.Zero))
			{
				return new T[0];
			}
			T[] tArray = new T[(void*)(checked((IntPtr)uIntPtr.ToUInt64()))];
			outMethod(tArray, uIntPtr);
			return tArray;
		}

		internal static string OutParamsToString(PInvokeUtilities.OutStringMethod outStringMethod)
		{
			UIntPtr uIntPtr = outStringMethod(null, UIntPtr.Zero);
			if (uIntPtr.Equals(UIntPtr.Zero))
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder((int)uIntPtr.ToUInt32());
			outStringMethod(stringBuilder, uIntPtr);
			return stringBuilder.ToString();
		}

		[DebuggerHidden]
		internal static IEnumerable<T> ToEnumerable<T>(UIntPtr size, Func<UIntPtr, T> getElement)
		{
			PInvokeUtilities.u003cToEnumerableu003ec__Iterator7E<T> variable = null;
			return variable;
		}

		internal static IEnumerator<T> ToEnumerator<T>(UIntPtr size, Func<UIntPtr, T> getElement)
		{
			return PInvokeUtilities.ToEnumerable<T>(size, getElement).GetEnumerator();
		}

		internal static long ToMilliseconds(TimeSpan span)
		{
			double totalMilliseconds = span.TotalMilliseconds;
			if (totalMilliseconds > 9.223372036854776E+18)
			{
				return 9223372036854775807L;
			}
			if (totalMilliseconds < -9.223372036854776E+18)
			{
				return -9223372036854775808L;
			}
			return Convert.ToInt64(totalMilliseconds);
		}

		internal delegate UIntPtr OutMethod<T>([In][Out] T[] out_bytes, UIntPtr out_size);

		internal delegate UIntPtr OutStringMethod(StringBuilder out_string, UIntPtr out_size);
	}
}