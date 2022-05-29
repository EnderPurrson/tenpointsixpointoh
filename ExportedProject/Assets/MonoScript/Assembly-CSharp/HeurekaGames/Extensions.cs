using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;

namespace HeurekaGames
{
	public static class Extensions
	{
		public static T Add<T>(this Enum type, T value)
		{
			T t;
			try
			{
				t = (T)(object)((int)type | (int)(object)value);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				throw new ArgumentException(string.Format("Could not append value from enumerated type '{0}'.", typeof(T).Name), exception);
			}
			return t;
		}

		public static void CastList<T>(this List<T> targetList)
		{
			targetList = targetList.Cast<T>().ToList<T>();
		}

		public static bool Has<T>(this Enum type, T value)
		{
			bool flag;
			try
			{
				flag = ((int)type & (int)(object)value) == (int)(object)value;
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		public static bool Is<T>(this Enum type, T value)
		{
			bool flag;
			try
			{
				flag = (int)type == (int)(object)value;
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		public static Color ModifiedAlpha(this Color color, float alpha)
		{
			Color color1 = color;
			color1.a = alpha;
			return color1;
		}

		public static float Remap(this float value, float from1, float to1, float from2, float to2)
		{
			return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
		}

		public static T Remove<T>(this Enum type, T value)
		{
			T t;
			try
			{
				t = (T)(object)((int)type & ~(int)(object)value);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				throw new ArgumentException(string.Format("Could not remove value from enumerated type '{0}'.", typeof(T).Name), exception);
			}
			return t;
		}

		public static void SetComponentRecursively<T>(this GameObject gameObject, bool tf)
		where T : Component
		{
			T[] componentsInChildren = gameObject.GetComponentsInChildren<T>();
			for (int i = 0; i < (int)componentsInChildren.Length; i++)
			{
				T t = componentsInChildren[i];
				try
				{
					PropertyInfo property = typeof(T).GetProperty("enabled");
					if (property == null || !property.CanWrite)
					{
						Console.WriteLine("BLABLA");
						Debug.Log("Property does not exist, or cannot write");
					}
					else
					{
						property.SetValue(t, tf, null);
					}
				}
				catch (NullReferenceException nullReferenceException)
				{
					Debug.Log(string.Concat("The property does not exist in MyClass.", nullReferenceException.Message));
				}
			}
		}

		public static string ToCamelCase(this string camelCaseString)
		{
			return Regex.Replace(camelCaseString, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ").Trim();
		}

		public static Vector2 YZ(this Vector3 v)
		{
			return new Vector2(v.x, v.z);
		}

		public static Vector2[] YZ(this Vector3[] v)
		{
			Vector2[] vector2 = new Vector2[(int)v.Length];
			for (int i = 0; i < (int)v.Length; i++)
			{
				vector2[i] = new Vector2(v[i].x, v[i].z);
			}
			return vector2;
		}
	}
}