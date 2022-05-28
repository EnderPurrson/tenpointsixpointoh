using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System
{
	public static class MissingExtensions
	{
		public static bool HasFlag(this System.Enum enumValue, System.Enum flag)
		{
			long num = Convert.ToInt64((object)enumValue);
			long num2 = Convert.ToInt64((object)flag);
			return (num & num2) == num2;
		}

		public static T GetCustomAttribute<T>(this PropertyInfo prop, bool inherit) where T : System.Attribute
		{
			return (T)Enumerable.FirstOrDefault<object>((System.Collections.Generic.IEnumerable<object>)((MemberInfo)prop).GetCustomAttributes(typeof(T), inherit));
		}

		public static T GetCustomAttribute<T>(this PropertyInfo prop) where T : System.Attribute
		{
			return prop.GetCustomAttribute<T>(true);
		}

		public static T GetCustomAttribute<T>(this MemberInfo member, bool inherit) where T : System.Attribute
		{
			return (T)Enumerable.FirstOrDefault<object>((System.Collections.Generic.IEnumerable<object>)member.GetCustomAttributes(typeof(T), inherit));
		}

		public static T GetCustomAttribute<T>(this MemberInfo member) where T : System.Attribute
		{
			return member.GetCustomAttribute<T>(true);
		}

		public static System.Collections.Generic.IEnumerable<TResult> Zip<T1, T2, TResult>(this System.Collections.Generic.IEnumerable<T1> list1, System.Collections.Generic.IEnumerable<T2> list2, Func<T1, T2, TResult> zipper)
		{
			System.Collections.Generic.IEnumerator<T1> e1 = list1.GetEnumerator();
			System.Collections.Generic.IEnumerator<T2> e2 = list2.GetEnumerator();
			while (((System.Collections.IEnumerator)e1).MoveNext() && ((System.Collections.IEnumerator)e2).MoveNext())
			{
				yield return zipper.Invoke(e1.get_Current(), e2.get_Current());
			}
		}
	}
}
