using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Prime31
{
	public static class DeserializationExtensions
	{
		public static List<T> toList<T>(this global::System.Collections.IList self)
		{
			List<T> val = new List<T>();
			global::System.Collections.IEnumerator enumerator = ((global::System.Collections.IEnumerable)self).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Dictionary<string, object> self2 = (Dictionary<string, object>)enumerator.get_Current();
					val.Add(((IDictionary)(object)self2).toClass<T>());
				}
				return val;
			}
			finally
			{
				global::System.IDisposable disposable;
				if ((disposable = enumerator as global::System.IDisposable) != null)
				{
					disposable.Dispose();
				}
			}
		}

		public static T toClass<T>(this IDictionary self)
		{
			object obj = Activator.CreateInstance(typeof(T));
			FieldInfo[] fields = typeof(T).GetFields((BindingFlags)52);
			foreach (FieldInfo val in fields)
			{
				object[] customAttributes = ((MemberInfo)val).GetCustomAttributes(typeof(P31DeserializeableFieldAttribute), true);
				foreach (object obj2 in customAttributes)
				{
					P31DeserializeableFieldAttribute p31DeserializeableFieldAttribute = obj2 as P31DeserializeableFieldAttribute;
					if (!self.Contains((object)p31DeserializeableFieldAttribute.key))
					{
						continue;
					}
					object obj3 = self.get_Item((object)p31DeserializeableFieldAttribute.key);
					if (obj3 is IDictionary)
					{
						MethodInfo val2 = typeof(DeserializationExtensions).GetMethod("toClass").MakeGenericMethod(new global::System.Type[1] { p31DeserializeableFieldAttribute.type });
						object obj4 = ((MethodBase)val2).Invoke((object)null, new object[1] { obj3 });
						val.SetValue(obj, obj4);
						self.Remove((object)p31DeserializeableFieldAttribute.key);
					}
					else if (obj3 is global::System.Collections.IList)
					{
						if (!p31DeserializeableFieldAttribute.isCollection)
						{
							Debug.LogError("found an IList but the field is not a collection: " + p31DeserializeableFieldAttribute.key);
							continue;
						}
						MethodInfo val3 = typeof(DeserializationExtensions).GetMethod("toList").MakeGenericMethod(new global::System.Type[1] { p31DeserializeableFieldAttribute.type });
						object obj5 = ((MethodBase)val3).Invoke((object)null, new object[1] { obj3 });
						val.SetValue(obj, obj5);
						self.Remove((object)p31DeserializeableFieldAttribute.key);
					}
					else
					{
						val.SetValue(obj, Convert.ChangeType(obj3, val.get_FieldType()));
						self.Remove((object)p31DeserializeableFieldAttribute.key);
					}
				}
			}
			return (T)obj;
		}

		public static Dictionary<string, object> toDictionary(this object self)
		{
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			Dictionary<string, object> val = new Dictionary<string, object>();
			FieldInfo[] fields = self.GetType().GetFields((BindingFlags)52);
			foreach (FieldInfo val2 in fields)
			{
				object[] customAttributes = ((MemberInfo)val2).GetCustomAttributes(typeof(P31DeserializeableFieldAttribute), true);
				foreach (object obj in customAttributes)
				{
					P31DeserializeableFieldAttribute p31DeserializeableFieldAttribute = obj as P31DeserializeableFieldAttribute;
					if (p31DeserializeableFieldAttribute.isCollection)
					{
						global::System.Collections.IEnumerable enumerable = val2.GetValue(self) as global::System.Collections.IEnumerable;
						ArrayList val3 = new ArrayList();
						global::System.Collections.IEnumerator enumerator = enumerable.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								object current = enumerator.get_Current();
								val3.Add((object)current.toDictionary());
							}
						}
						finally
						{
							global::System.IDisposable disposable;
							if ((disposable = enumerator as global::System.IDisposable) != null)
							{
								disposable.Dispose();
							}
						}
						val.set_Item(p31DeserializeableFieldAttribute.key, (object)val3);
					}
					else if (p31DeserializeableFieldAttribute.type != null)
					{
						val.set_Item(p31DeserializeableFieldAttribute.key, (object)val2.GetValue(self).toDictionary());
					}
					else
					{
						val.set_Item(p31DeserializeableFieldAttribute.key, val2.GetValue(self));
					}
				}
			}
			return val;
		}

		[Obsolete("Use the toDictionary method to get a proper generic Dictionary returned. Hashtables are obsolute.")]
		public static Hashtable toHashtable(this object self)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			Hashtable val = new Hashtable();
			FieldInfo[] fields = self.GetType().GetFields((BindingFlags)52);
			foreach (FieldInfo val2 in fields)
			{
				object[] customAttributes = ((MemberInfo)val2).GetCustomAttributes(typeof(P31DeserializeableFieldAttribute), true);
				foreach (object obj in customAttributes)
				{
					P31DeserializeableFieldAttribute p31DeserializeableFieldAttribute = obj as P31DeserializeableFieldAttribute;
					if (p31DeserializeableFieldAttribute.isCollection)
					{
						global::System.Collections.IEnumerable enumerable = val2.GetValue(self) as global::System.Collections.IEnumerable;
						ArrayList val3 = new ArrayList();
						global::System.Collections.IEnumerator enumerator = enumerable.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								object current = enumerator.get_Current();
								val3.Add((object)current.toHashtable());
							}
						}
						finally
						{
							global::System.IDisposable disposable;
							if ((disposable = enumerator as global::System.IDisposable) != null)
							{
								disposable.Dispose();
							}
						}
						val.set_Item((object)p31DeserializeableFieldAttribute.key, (object)val3);
					}
					else if (p31DeserializeableFieldAttribute.type != null)
					{
						val.set_Item((object)p31DeserializeableFieldAttribute.key, (object)val2.GetValue(self).toHashtable());
					}
					else
					{
						val.set_Item((object)p31DeserializeableFieldAttribute.key, val2.GetValue(self));
					}
				}
			}
			return val;
		}
	}
}
