using System;

namespace GooglePlayGames.OurUtils
{
	public static class Misc
	{
		public static bool BuffersAreIdentical(byte[] a, byte[] b)
		{
			if (a == b)
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			if ((int)a.Length != (int)b.Length)
			{
				return false;
			}
			for (int i = 0; i < (int)a.Length; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return true;
		}

		public static T CheckNotNull<T>(T value)
		{
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			return value;
		}

		public static T CheckNotNull<T>(T value, string paramName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(paramName);
			}
			return value;
		}

		public static byte[] GetSubsetBytes(byte[] array, int offset, int length)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (offset < 0 || offset >= (int)array.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (length < 0 || (int)array.Length - offset < length)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			if (offset == 0 && length == (int)array.Length)
			{
				return array;
			}
			byte[] numArray = new byte[length];
			Array.Copy(array, offset, numArray, 0, length);
			return numArray;
		}
	}
}