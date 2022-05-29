using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Rilisoft
{
	public static class RiliExtensions
	{
		public static Color ColorFromHex(string hex)
		{
			hex = hex.Replace("0x", string.Empty);
			hex = hex.Replace("#", string.Empty);
			byte num = 255;
			byte num1 = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
			byte num2 = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
			byte num3 = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
			if (hex.Length == 8)
			{
				num = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
			}
			return new Color32(num1, num2, num3, num);
		}

		public static Color ColorFromRGB(int r, int g, int b, int a = 255)
		{
			return new Color((float)(r / 255), (float)(g / 255), (float)(b / 255), (float)(a / 255));
		}

		public static int[] EnumNumbers<T>()
		where T : struct
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException("T must be an enumerated type");
			}
			return Enum.GetValues(typeof(T)).Cast<int>().ToArray<int>();
		}

		public static string[] EnumValues<T>()
		where T : struct
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException("T must be an enumerated type");
			}
			return Enum.GetValues(typeof(T)).Cast<string>().ToArray<string>();
		}

		public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
		{
			IEnumerator<T> enumerator = enumeration.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					action(enumerator.Current);
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
		}

		public static void ForEachEnum<T>(Action<T> action)
		{
			if (action != null)
			{
				IEnumerator enumerator = Enum.GetValues(typeof(T)).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						action((T)enumerator.Current);
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable == null)
					{
					}
					disposable.Dispose();
				}
			}
		}

		public static GameObject GetChildGameObject(this GameObject go, string name, bool includeInactive = false)
		{
			GameObject gameObject;
			Transform transforms = go.transform.GetComponentsInChildren<Transform>(includeInactive).FirstOrDefault<Transform>((Transform t) => t.gameObject.name == name);
			if (transforms == null)
			{
				gameObject = null;
			}
			else
			{
				gameObject = transforms.gameObject;
			}
			return gameObject;
		}

		public static T GetComponentInChildren<T>(this GameObject go, string name, bool includeInactive = false)
		{
			Transform[] componentsInChildren = go.transform.GetComponentsInChildren<Transform>(includeInactive);
			for (int i = 0; i < (int)componentsInChildren.Length; i++)
			{
				Transform transforms = componentsInChildren[i];
				if (transforms.gameObject.name == name)
				{
					return transforms.gameObject.GetComponent<T>();
				}
			}
			return default(T);
		}

		public static T GetComponentInParents<T>(this GameObject go)
		{
			T component = go.GetComponent<T>();
			if (component != null)
			{
				if (!component.Equals(default(T)))
				{
					return component;
				}
			}
			return go.transform.parent.gameObject.GetComponentInParents<T>();
		}

		public static GameObject GetGameObjectInParent(this GameObject go, string name, bool includeInactive = false)
		{
			Transform[] componentsInParent = go.transform.GetComponentsInParent<Transform>(includeInactive);
			for (int i = 0; i < (int)componentsInParent.Length; i++)
			{
				Transform transforms = componentsInParent[i];
				if (transforms.gameObject.name == name)
				{
					return transforms.gameObject;
				}
			}
			return null;
		}

		public static T GetOrAddComponent<T>(this Component child)
		where T : Component
		{
			T component = child.GetComponent<T>();
			if (component == null)
			{
				component = child.gameObject.AddComponent<T>();
			}
			return component;
		}

		public static T GetOrAddComponent<T>(this GameObject child)
		where T : Component
		{
			T component = child.GetComponent<T>();
			if (component == null)
			{
				component = child.gameObject.AddComponent<T>();
			}
			return component;
		}

		public static bool IsNullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		public static string nameNoClone(this UnityEngine.Object obj)
		{
			if (obj == null)
			{
				return null;
			}
			return obj.name.Replace("(Clone)", string.Empty);
		}

		public static IEnumerable<T> Random<T>(this IEnumerable<T> source, int count)
		{
			if (source == null)
			{
				return source;
			}
			List<T> list = source.ToList<T>();
			if (!list.Any<T>())
			{
				return list;
			}
			List<T> ts = new List<T>();
			if (count < 1)
			{
				return ts;
			}
			bool flag = true;
			while (flag)
			{
				int num = UnityEngine.Random.Range(0, list.Count);
				ts.Add(list[num]);
				list.RemoveAt(num);
				if (list.Count != 0 && ts.Count != count)
				{
					continue;
				}
				flag = false;
			}
			return ts;
		}

		public static void SetActiveSafe(this GameObject go, bool state)
		{
			if (go.activeInHierarchy != state)
			{
				go.SetActive(state);
			}
		}

		public static Nullable<T> ToEnum<T>(this string str, Nullable<T> defaultVal = null)
		where T : struct
		{
			Nullable<T> nullable;
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException("T must be an enumerated type");
			}
			if (str.IsNullOrEmpty())
			{
				UnityEngine.Debug.LogError("String is null or empty");
				return defaultVal;
			}
			str = str.ToLower();
			IEnumerator enumerator = Enum.GetValues(typeof(T)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					T current = (T)enumerator.Current;
					if (current.ToString().ToLower() != str)
					{
						continue;
					}
					nullable = new Nullable<T>(current);
					return nullable;
				}
				UnityEngine.Debug.LogErrorFormat("'{0}' does not contain '{1}'", new object[] { typeof(T).Name, str });
				return defaultVal;
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable == null)
				{
				}
				disposable.Dispose();
			}
			return nullable;
		}

		public static string ToHex(this Color32 color)
		{
			string str = string.Concat(color.r.ToString("X2"), color.g.ToString("X2"), color.b.ToString("X2"));
			return str;
		}

		[DebuggerHidden]
		public static IEnumerable<T> WithoutLast<T>(IEnumerable<T> source)
		{
			RiliExtensions.u003cWithoutLastu003ec__Iterator19B<T> variable = null;
			return variable;
		}
	}
}