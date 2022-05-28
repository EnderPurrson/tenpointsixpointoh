using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public static class RiliExtensions
	{
		[CompilerGenerated]
		private sealed class _003CGetChildGameObject_003Ec__AnonStorey2FF
		{
			internal string name;

			internal bool _003C_003Em__46B(Transform t)
			{
				return t.gameObject.name == name;
			}
		}

		public static IEnumerable<T> WithoutLast<T>(this IEnumerable<T> source)
		{
			using (IEnumerator<T> e = source.GetEnumerator())
			{
				if (e.MoveNext())
				{
					T value = e.Current;
					while (e.MoveNext())
					{
						yield return value;
						value = e.Current;
					}
				}
			}
		}

		public static string nameNoClone(this UnityEngine.Object obj)
		{
			if (obj == null)
			{
				return null;
			}
			return obj.name.Replace("(Clone)", string.Empty);
		}

		public static bool IsNullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		public static T? ToEnum<T>(this string str, T? defaultVal = null) where T : struct
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException("T must be an enumerated type");
			}
			if (str.IsNullOrEmpty())
			{
				Debug.LogError("String is null or empty");
				return defaultVal;
			}
			str = str.ToLower();
			foreach (T value in Enum.GetValues(typeof(T)))
			{
				if (value.ToString().ToLower() == str)
				{
					return value;
				}
			}
			Debug.LogErrorFormat("'{0}' does not contain '{1}'", typeof(T).Name, str);
			return defaultVal;
		}

		public static string[] EnumValues<T>() where T : struct
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException("T must be an enumerated type");
			}
			return Enum.GetValues(typeof(T)).Cast<string>().ToArray();
		}

		public static int[] EnumNumbers<T>() where T : struct
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException("T must be an enumerated type");
			}
			return Enum.GetValues(typeof(T)).Cast<int>().ToArray();
		}

		public static void ForEachEnum<T>(Action<T> action)
		{
			if (action == null)
			{
				return;
			}
			Array values = Enum.GetValues(typeof(T));
			foreach (object item in values)
			{
				action((T)item);
			}
		}

		public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
		{
			foreach (T item in enumeration)
			{
				action(item);
			}
		}

		public static GameObject GetChildGameObject(this GameObject go, string name, bool includeInactive = false)
		{
			_003CGetChildGameObject_003Ec__AnonStorey2FF _003CGetChildGameObject_003Ec__AnonStorey2FF = new _003CGetChildGameObject_003Ec__AnonStorey2FF();
			_003CGetChildGameObject_003Ec__AnonStorey2FF.name = name;
			Transform transform = go.transform.GetComponentsInChildren<Transform>(includeInactive).FirstOrDefault(_003CGetChildGameObject_003Ec__AnonStorey2FF._003C_003Em__46B);
			return (!(transform != null)) ? null : transform.gameObject;
		}

		public static T GetComponentInChildren<T>(this GameObject go, string name, bool includeInactive = false)
		{
			Transform[] componentsInChildren = go.transform.GetComponentsInChildren<Transform>(includeInactive);
			Transform[] array = componentsInChildren;
			foreach (Transform transform in array)
			{
				if (transform.gameObject.name == name)
				{
					return transform.gameObject.GetComponent<T>();
				}
			}
			return default(T);
		}

		public static GameObject GetGameObjectInParent(this GameObject go, string name, bool includeInactive = false)
		{
			Transform[] componentsInParent = go.transform.GetComponentsInParent<Transform>(includeInactive);
			Transform[] array = componentsInParent;
			foreach (Transform transform in array)
			{
				if (transform.gameObject.name == name)
				{
					return transform.gameObject;
				}
			}
			return null;
		}

		public static T GetComponentInParents<T>(this GameObject go)
		{
			T component = go.GetComponent<T>();
			if (component != null && !component.Equals(default(T)))
			{
				return component;
			}
			Transform parent = go.transform.parent;
			return parent.gameObject.GetComponentInParents<T>();
		}

		public static void SetActiveSafe(this GameObject go, bool state)
		{
			if (go.activeInHierarchy != state)
			{
				go.SetActive(state);
			}
		}

		public static T GetOrAddComponent<T>(this Component child) where T : Component
		{
			T val = child.GetComponent<T>();
			if ((UnityEngine.Object)val == (UnityEngine.Object)default(UnityEngine.Object))
			{
				val = child.gameObject.AddComponent<T>();
			}
			return val;
		}

		public static T GetOrAddComponent<T>(this GameObject child) where T : Component
		{
			T val = child.GetComponent<T>();
			if ((UnityEngine.Object)val == (UnityEngine.Object)default(UnityEngine.Object))
			{
				val = child.gameObject.AddComponent<T>();
			}
			return val;
		}

		public static IEnumerable<T> Random<T>(this IEnumerable<T> source, int count)
		{
			if (source == null)
			{
				return source;
			}
			List<T> list = source.ToList();
			if (!list.Any())
			{
				return list;
			}
			List<T> list2 = new List<T>();
			if (count < 1)
			{
				return list2;
			}
			bool flag = true;
			while (flag)
			{
				int index = UnityEngine.Random.Range(0, list.Count);
				list2.Add(list[index]);
				list.RemoveAt(index);
				if (list.Count == 0 || list2.Count == count)
				{
					flag = false;
				}
			}
			return list2;
		}

		public static Color ColorFromRGB(int r, int g, int b, int a = 255)
		{
			return new Color(r / 255, g / 255, b / 255, a / 255);
		}

		public static Color ColorFromHex(string hex)
		{
			hex = hex.Replace("0x", string.Empty);
			hex = hex.Replace("#", string.Empty);
			byte a = byte.MaxValue;
			byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
			if (hex.Length == 8)
			{
				a = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
			}
			return new Color32(r, g, b, a);
		}

		public static string ToHex(this Color32 color)
		{
			return color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		}
	}
}
