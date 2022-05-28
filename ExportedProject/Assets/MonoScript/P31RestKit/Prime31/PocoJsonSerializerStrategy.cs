using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Prime31.Reflection;

namespace Prime31
{
	public class PocoJsonSerializerStrategy : IJsonSerializerStrategy
	{
		internal CacheResolver cacheResolver;

		private static readonly string[] Iso8601Format = new string[3] { "yyyy-MM-dd\\THH:mm:ss.FFFFFFF\\Z", "yyyy-MM-dd\\THH:mm:ss\\Z", "yyyy-MM-dd\\THH:mm:ssK" };

		public PocoJsonSerializerStrategy()
		{
			cacheResolver = new CacheResolver(buildMap);
		}

		protected virtual void buildMap(global::System.Type type, SafeDictionary<string, CacheResolver.MemberMap> memberMaps)
		{
			PropertyInfo[] properties = type.GetProperties((BindingFlags)52);
			foreach (PropertyInfo val in properties)
			{
				memberMaps.add(((MemberInfo)val).get_Name(), new CacheResolver.MemberMap(val));
			}
			FieldInfo[] fields = type.GetFields((BindingFlags)52);
			foreach (FieldInfo val2 in fields)
			{
				memberMaps.add(((MemberInfo)val2).get_Name(), new CacheResolver.MemberMap(val2));
			}
		}

		public virtual bool serializeNonPrimitiveObject(object input, out object output)
		{
			return trySerializeKnownTypes(input, out output) || trySerializeUnknownTypes(input, out output);
		}

		public virtual object deserializeObject(object value, global::System.Type type)
		{
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Expected O, but got Unknown
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			object result = null;
			if (value is string)
			{
				string text = value as string;
				result = ((string.IsNullOrEmpty(text) || (type != typeof(global::System.DateTime) && (!ReflectionUtils.isNullableType(type) || Nullable.GetUnderlyingType(type) != typeof(global::System.DateTime)))) ? text : ((object)global::System.DateTime.ParseExact(text, Iso8601Format, (IFormatProvider)(object)CultureInfo.get_InvariantCulture(), (DateTimeStyles)80)));
			}
			else if (value is bool)
			{
				result = value;
			}
			else if (value == null)
			{
				result = null;
			}
			else if ((value is long && type == typeof(long)) || (value is double && type == typeof(double)))
			{
				result = value;
			}
			else
			{
				if ((!(value is double) || type == typeof(double)) && (!(value is long) || type == typeof(long)))
				{
					if (value is IDictionary<string, object>)
					{
						IDictionary<string, object> val = (IDictionary<string, object>)value;
						if (ReflectionUtils.isTypeDictionary(type))
						{
							global::System.Type type2 = type.GetGenericArguments()[0];
							global::System.Type type3 = type.GetGenericArguments()[1];
							global::System.Type type4 = typeof(Dictionary<, >).MakeGenericType(new global::System.Type[2] { type2, type3 });
							IDictionary val2 = (IDictionary)CacheResolver.getNewInstance(type4);
							global::System.Collections.Generic.IEnumerator<KeyValuePair<string, object>> enumerator = ((global::System.Collections.Generic.IEnumerable<KeyValuePair<string, object>>)val).GetEnumerator();
							try
							{
								while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
								{
									KeyValuePair<string, object> current = enumerator.get_Current();
									val2.Add((object)current.get_Key(), deserializeObject(current.get_Value(), type3));
								}
							}
							finally
							{
								if (enumerator != null)
								{
									((global::System.IDisposable)enumerator).Dispose();
								}
							}
							result = val2;
						}
						else
						{
							result = CacheResolver.getNewInstance(type);
							SafeDictionary<string, CacheResolver.MemberMap> safeDictionary = cacheResolver.loadMaps(type);
							if (safeDictionary != null)
							{
								global::System.Collections.Generic.IEnumerator<KeyValuePair<string, CacheResolver.MemberMap>> enumerator2 = safeDictionary.GetEnumerator();
								try
								{
									while (((global::System.Collections.IEnumerator)enumerator2).MoveNext())
									{
										KeyValuePair<string, CacheResolver.MemberMap> current2 = enumerator2.get_Current();
										CacheResolver.MemberMap value2 = current2.get_Value();
										if (value2.Setter != null)
										{
											string key = current2.get_Key();
											if (val.ContainsKey(key))
											{
												object value3 = deserializeObject(val.get_Item(key), value2.Type);
												value2.Setter(result, value3);
											}
										}
									}
									return result;
								}
								finally
								{
									if (enumerator2 != null)
									{
										((global::System.IDisposable)enumerator2).Dispose();
									}
								}
							}
							result = value;
						}
					}
					else if (value is global::System.Collections.Generic.IList<object>)
					{
						global::System.Collections.Generic.IList<object> list = (global::System.Collections.Generic.IList<object>)value;
						global::System.Collections.IList list2 = null;
						if (type.get_IsArray())
						{
							list2 = (global::System.Collections.IList)Activator.CreateInstance(type, new object[1] { ((global::System.Collections.Generic.ICollection<object>)list).get_Count() });
							int num = 0;
							global::System.Collections.Generic.IEnumerator<object> enumerator3 = ((global::System.Collections.Generic.IEnumerable<object>)list).GetEnumerator();
							try
							{
								while (((global::System.Collections.IEnumerator)enumerator3).MoveNext())
								{
									object current3 = enumerator3.get_Current();
									list2.set_Item(num++, deserializeObject(current3, type.GetElementType()));
								}
							}
							finally
							{
								if (enumerator3 != null)
								{
									((global::System.IDisposable)enumerator3).Dispose();
								}
							}
						}
						else if (ReflectionUtils.isTypeGenericeCollectionInterface(type) || typeof(global::System.Collections.IList).IsAssignableFrom(type))
						{
							global::System.Type type5 = type.GetGenericArguments()[0];
							global::System.Type type6 = typeof(List<>).MakeGenericType(new global::System.Type[1] { type5 });
							list2 = (global::System.Collections.IList)CacheResolver.getNewInstance(type6);
							global::System.Collections.Generic.IEnumerator<object> enumerator4 = ((global::System.Collections.Generic.IEnumerable<object>)list).GetEnumerator();
							try
							{
								while (((global::System.Collections.IEnumerator)enumerator4).MoveNext())
								{
									object current4 = enumerator4.get_Current();
									list2.Add(deserializeObject(current4, type5));
								}
							}
							finally
							{
								if (enumerator4 != null)
								{
									((global::System.IDisposable)enumerator4).Dispose();
								}
							}
						}
						result = list2;
					}
					return result;
				}
				result = ((value is long && type == typeof(global::System.DateTime)) ? ((object)new global::System.DateTime(1970, 1, 1, 0, 0, 0, (DateTimeKind)1).AddMilliseconds((double)(long)value)) : ((!type.get_IsEnum()) ? ((!typeof(IConvertible).IsAssignableFrom(type)) ? value : Convert.ChangeType(value, type, (IFormatProvider)(object)CultureInfo.get_InvariantCulture())) : global::System.Enum.ToObject(type, value)));
			}
			if (ReflectionUtils.isNullableType(type))
			{
				return ReflectionUtils.toNullableType(result, type);
			}
			return result;
		}

		protected virtual object serializeEnum(global::System.Enum p)
		{
			return Convert.ToDouble((object)p, (IFormatProvider)(object)CultureInfo.get_InvariantCulture());
		}

		protected virtual bool trySerializeKnownTypes(object input, out object output)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			bool result = true;
			if (input is global::System.DateTime)
			{
				output = ((global::System.DateTime)input).ToUniversalTime().ToString(Iso8601Format[0], (IFormatProvider)(object)CultureInfo.get_InvariantCulture());
			}
			else if (input is Guid)
			{
				Guid val = (Guid)input;
				output = ((Guid)(ref val)).ToString("D");
			}
			else if (input is Uri)
			{
				output = input.ToString();
			}
			else if (input is global::System.Enum)
			{
				output = serializeEnum((global::System.Enum)input);
			}
			else
			{
				result = false;
				output = null;
			}
			return result;
		}

		protected virtual bool trySerializeUnknownTypes(object input, out object output)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			output = null;
			global::System.Type type = input.GetType();
			if (type.get_FullName() == null)
			{
				return false;
			}
			IDictionary<string, object> val = (IDictionary<string, object>)(object)new JsonObject();
			SafeDictionary<string, CacheResolver.MemberMap> safeDictionary = cacheResolver.loadMaps(type);
			global::System.Collections.Generic.IEnumerator<KeyValuePair<string, CacheResolver.MemberMap>> enumerator = safeDictionary.GetEnumerator();
			try
			{
				while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
				{
					KeyValuePair<string, CacheResolver.MemberMap> current = enumerator.get_Current();
					if (current.get_Value().Getter != null)
					{
						val.Add(current.get_Key(), current.get_Value().Getter(input));
					}
				}
			}
			finally
			{
				if (enumerator != null)
				{
					((global::System.IDisposable)enumerator).Dispose();
				}
			}
			output = val;
			return true;
		}
	}
}
