using System;

namespace UnityScript.Lang
{
	public static class Extensions
	{
		public static int length
		{
			get
			{
				return a.Length;
			}
		}

		public static int length
		{
			get
			{
				return s.Length;
			}
		}

		public static bool operator ==(char lhs, string rhs)
		{
			bool num = 1 == rhs.Length;
			if (num)
			{
				num = lhs == rhs[0];
			}
			return num;
		}

		public static bool operator ==(string lhs, char rhs)
		{
			return rhs == lhs;
		}

		public static bool operator !=(char lhs, string rhs)
		{
			bool num = 1 != rhs.Length;
			if (!num)
			{
				num = lhs != rhs[0];
			}
			return num;
		}

		public static bool operator !=(string lhs, char rhs)
		{
			return rhs != lhs;
		}

		public static implicit operator bool(Enum e)
		{
			return ((IConvertible)e).ToInt32((IFormatProvider)default(IFormatProvider)) != 0;
		}
	}
}
