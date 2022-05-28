using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DevToDev.Core.Utils;

namespace DevToDev.Core.Serialization
{
	public class ObjectInfo
	{
		private static readonly string TYPE_KEY = "#type";

		private static readonly string COLLECTION_TYPE_KEY = "#collectionType";

		private static readonly string PRIMITIVE_KEY = "#primitive";

		private static readonly string COLLECTION_KEY = "#collection";

		private static readonly string ARRAY_KEY = "#array";

		private static readonly string DICTIONARY_KEY = "#dictionary";

		private static readonly string VALUE_KEY = "#value";

		private static readonly string SERIAL_VERSION_ID_KEY = "#serialVersionId";

		private int serialVersionId;

		private string type;

		private Dictionary<string, object> objectData;

		public int SerialVersionId
		{
			get
			{
				return serialVersionId;
			}
		}

		public string TypeInfo
		{
			get
			{
				return type;
			}
		}

		public ObjectInfo()
		{
			objectData = new Dictionary<string, object>();
		}

		public ObjectInfo(ISaveable saveableObject)
		{
			objectData = new Dictionary<string, object>();
			type = ((object)saveableObject).GetType().get_FullName();
			FieldInfo fieldInfo = ObjectInfoPlatform.GetFieldInfo(saveableObject, "SerialVersionId");
			if (fieldInfo != null)
			{
				serialVersionId = (int)fieldInfo.GetValue((object)default(object));
			}
			saveableObject.GetObjectData(this);
		}

		public object GetValue(string name, global::System.Type type)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			if (name == null)
			{
				throw new ArgumentNullException("name is null");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type is null");
			}
			if (!objectData.ContainsKey(name))
			{
				return null;
			}
			object obj = objectData.get_Item(name);
			if (obj != null)
			{
				return Decode(obj);
			}
			return obj;
		}

		private object Encode(object value)
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			if (value is IDictionary)
			{
				Dictionary<string, object> val = new Dictionary<string, object>();
				val.Add(COLLECTION_KEY, (object)DICTIONARY_KEY);
				val.Add(COLLECTION_TYPE_KEY, (object)value.GetType().get_FullName());
				Dictionary<string, object> val2 = new Dictionary<string, object>();
				IDictionaryEnumerator enumerator = ((IDictionary)((value is IDictionary) ? value : null)).GetEnumerator();
				try
				{
					while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
					{
						DictionaryEntry val3 = (DictionaryEntry)((global::System.Collections.IEnumerator)enumerator).get_Current();
						val2.Add(((DictionaryEntry)(ref val3)).get_Key().ToString(), EncodeValue(((DictionaryEntry)(ref val3)).get_Value()));
					}
				}
				finally
				{
					global::System.IDisposable disposable = enumerator as global::System.IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				val.Add(VALUE_KEY, (object)val2);
				return val;
			}
			if (value is global::System.Collections.IList)
			{
				Dictionary<string, object> val4 = new Dictionary<string, object>();
				val4.Add(COLLECTION_KEY, (object)ARRAY_KEY);
				val4.Add(COLLECTION_TYPE_KEY, (object)value.GetType().get_FullName());
				List<object> val5 = new List<object>();
				{
					global::System.Collections.IEnumerator enumerator2 = ((global::System.Collections.IEnumerable)(value as global::System.Collections.IList)).GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							object current = enumerator2.get_Current();
							val5.Add(EncodeValue(current));
						}
					}
					finally
					{
						global::System.IDisposable disposable2 = enumerator2 as global::System.IDisposable;
						if (disposable2 != null)
						{
							disposable2.Dispose();
						}
					}
				}
				val4.Add(VALUE_KEY, (object)val5);
				return val4;
			}
			if (value is ISaveable)
			{
				return new ObjectInfo(value as ISaveable);
			}
			throw new ArgumentException(string.Concat((object)"value does not inherit ISaveable. the type is: ", (object)value.GetType()));
		}

		public ObjectInfo(JSONNode json)
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			objectData = new Dictionary<string, object>();
			serialVersionId = json[SERIAL_VERSION_ID_KEY].AsInt;
			type = json[TYPE_KEY];
			global::System.Collections.IEnumerator enumerator = json.AsObject.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, JSONNode> val = (KeyValuePair<string, JSONNode>)enumerator.get_Current();
					object obj = null;
					obj = ((!(val.get_Value() is JSONClass)) ? ((object)((!(val.get_Value() is JSONArray)) ? val.get_Value().Value : ((string)(object)DecodeList(val.get_Value())))) : ((object)((!(val.get_Value()[TYPE_KEY] != null)) ? ((ObjectInfo)(object)DecodeDictionary(val.get_Value())) : new ObjectInfo(val.get_Value()))));
					objectData.Add(val.get_Key(), obj);
				}
			}
			finally
			{
				global::System.IDisposable disposable = enumerator as global::System.IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}

		private List<object> DecodeList(JSONNode data)
		{
			List<object> val = new List<object>();
			global::System.Collections.IEnumerator enumerator = data.AsArray.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					JSONNode jSONNode = (JSONNode)enumerator.get_Current();
					object obj = null;
					obj = ((!(jSONNode is JSONClass)) ? ((object)((!(jSONNode is JSONArray)) ? jSONNode.Value : ((string)(object)DecodeList(jSONNode)))) : ((object)((!(jSONNode[TYPE_KEY] != null)) ? ((ObjectInfo)(object)DecodeDictionary(jSONNode)) : new ObjectInfo(jSONNode))));
					val.Add(obj);
				}
				return val;
			}
			finally
			{
				global::System.IDisposable disposable = enumerator as global::System.IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}

		private Dictionary<string, object> DecodeDictionary(JSONNode data)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			Dictionary<string, object> val = new Dictionary<string, object>();
			global::System.Collections.IEnumerator enumerator = data.AsObject.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, JSONNode> val2 = (KeyValuePair<string, JSONNode>)enumerator.get_Current();
					object obj = null;
					obj = ((!(val2.get_Value() is JSONClass)) ? ((object)((!(val2.get_Value() is JSONArray)) ? val2.get_Value().Value : ((string)(object)DecodeList(val2.get_Value())))) : ((object)((!(val2.get_Value()[TYPE_KEY] != null)) ? ((ObjectInfo)(object)DecodeDictionary(val2.get_Value())) : new ObjectInfo(val2.get_Value()))));
					val.Add(val2.get_Key(), obj);
				}
				return val;
			}
			finally
			{
				global::System.IDisposable disposable = enumerator as global::System.IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}

		private object DecodePrimitive(Dictionary<string, object> data)
		{
			switch (data.get_Item(PRIMITIVE_KEY) as string)
			{
			case "short":
			{
				short num = default(short);
				if (short.TryParse(data.get_Item(VALUE_KEY) as string, ref num))
				{
					return num;
				}
				return (short)0;
			}
			case "ushort":
			{
				ushort num8 = default(ushort);
				if (ushort.TryParse(data.get_Item(VALUE_KEY) as string, ref num8))
				{
					return num8;
				}
				return (ushort)0;
			}
			case "int":
			{
				int num3 = default(int);
				if (int.TryParse(data.get_Item(VALUE_KEY) as string, ref num3))
				{
					return num3;
				}
				return 0;
			}
			case "byte":
			{
				byte b = default(byte);
				if (byte.TryParse(data.get_Item(VALUE_KEY) as string, ref b))
				{
					return b;
				}
				return (byte)0;
			}
			case "bool":
			{
				bool flag = default(bool);
				if (bool.TryParse(data.get_Item(VALUE_KEY) as string, ref flag))
				{
					return flag;
				}
				return false;
			}
			case "char":
			{
				char c = default(char);
				if (char.TryParse(data.get_Item(VALUE_KEY) as string, ref c))
				{
					return c;
				}
				return '\0';
			}
			case "sbyte":
			{
				sbyte b2 = default(sbyte);
				if (sbyte.TryParse(data.get_Item(VALUE_KEY) as string, ref b2))
				{
					return b2;
				}
				return (sbyte)0;
			}
			case "double":
			{
				double num7 = default(double);
				if (double.TryParse(data.get_Item(VALUE_KEY) as string, ref num7))
				{
					return num7;
				}
				return 0.0;
			}
			case "decimal":
			{
				decimal num4 = default(decimal);
				if (decimal.TryParse(data.get_Item(VALUE_KEY) as string, ref num4))
				{
					return num4;
				}
				return 0m;
			}
			case "#DateTime":
			{
				global::System.DateTime dateTime = default(global::System.DateTime);
				if (global::System.DateTime.TryParse(data.get_Item(VALUE_KEY) as string, ref dateTime))
				{
					return dateTime;
				}
				return default(global::System.DateTime);
			}
			case "uint":
			{
				uint num9 = default(uint);
				if (uint.TryParse(data.get_Item(VALUE_KEY) as string, ref num9))
				{
					return num9;
				}
				return 0u;
			}
			case "long":
			{
				long num6 = default(long);
				if (long.TryParse(data.get_Item(VALUE_KEY) as string, ref num6))
				{
					return num6;
				}
				return 0L;
			}
			case "ulong":
			{
				ulong num5 = default(ulong);
				if (ulong.TryParse(data.get_Item(VALUE_KEY) as string, ref num5))
				{
					return num5;
				}
				return 0uL;
			}
			case "float":
			{
				float num2 = default(float);
				if (float.TryParse(data.get_Item(VALUE_KEY) as string, ref num2))
				{
					return num2;
				}
				return 0f;
			}
			case "string":
				if (!data.ContainsKey(VALUE_KEY))
				{
					return "";
				}
				return data.get_Item(VALUE_KEY);
			default:
				return null;
			}
		}

		private object Decode(object data)
		{
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			if (ObjectInfoPlatform.IsPrimitive(data))
			{
				return data;
			}
			if (data is IDictionary && data is Dictionary<string, object>)
			{
				Dictionary<string, object> val = data as Dictionary<string, object>;
				if (val.ContainsKey(PRIMITIVE_KEY))
				{
					return DecodePrimitive(val);
				}
				if (val.ContainsKey(COLLECTION_KEY))
				{
					string text = val.get_Item(COLLECTION_KEY).ToString();
					string text2 = val.get_Item(COLLECTION_TYPE_KEY).ToString();
					if (text.Equals(ARRAY_KEY))
					{
						if (global::System.Type.GetType(text2) == null)
						{
							return null;
						}
						global::System.Collections.IList list = Activator.CreateInstance(global::System.Type.GetType(text2)) as global::System.Collections.IList;
						global::System.Collections.IList list2 = val.get_Item(VALUE_KEY) as global::System.Collections.IList;
						{
							global::System.Collections.IEnumerator enumerator = ((global::System.Collections.IEnumerable)list2).GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									object current = enumerator.get_Current();
									list.Add(Decode(current));
								}
								return list;
							}
							finally
							{
								global::System.IDisposable disposable2 = enumerator as global::System.IDisposable;
								if (disposable2 != null)
								{
									disposable2.Dispose();
								}
							}
						}
					}
					if (text.Equals(DICTIONARY_KEY))
					{
						if (global::System.Type.GetType(text2) == null)
						{
							return null;
						}
						object obj = Activator.CreateInstance(global::System.Type.GetType(text2));
						IDictionary val2 = (IDictionary)((obj is IDictionary) ? obj : null);
						object obj2 = val.get_Item(VALUE_KEY);
						IDictionary val3 = (IDictionary)((obj2 is IDictionary) ? obj2 : null);
						IDictionaryEnumerator enumerator2 = val3.GetEnumerator();
						try
						{
							while (((global::System.Collections.IEnumerator)enumerator2).MoveNext())
							{
								DictionaryEntry val4 = (DictionaryEntry)((global::System.Collections.IEnumerator)enumerator2).get_Current();
								val2.Add(((DictionaryEntry)(ref val4)).get_Key(), Decode(((DictionaryEntry)(ref val4)).get_Value()));
							}
							return val2;
						}
						finally
						{
							global::System.IDisposable disposable = enumerator2 as global::System.IDisposable;
							if (disposable != null)
							{
								disposable.Dispose();
							}
						}
					}
				}
			}
			if (data is ObjectInfo)
			{
				ObjectInfo objectInfo = data as ObjectInfo;
				if (global::System.Type.GetType(objectInfo.TypeInfo) == null)
				{
					return null;
				}
				return Activator.CreateInstance(global::System.Type.GetType(objectInfo.TypeInfo), new object[1] { objectInfo }) as ISaveable;
			}
			throw new ArgumentException("Something went wrong :" + ((object)data.GetType()).ToString());
		}

		public object SelfValue(global::System.Type type)
		{
			return Decode(this);
		}

		private Dictionary<string, object> EncodeSimpleValue(short value)
		{
			Dictionary<string, object> val = new Dictionary<string, object>();
			val.Add(PRIMITIVE_KEY, (object)"short");
			val.Add(VALUE_KEY, (object)value.ToString());
			return val;
		}

		private Dictionary<string, object> EncodeSimpleValue(ushort value)
		{
			Dictionary<string, object> val = new Dictionary<string, object>();
			val.Add(PRIMITIVE_KEY, (object)"ushort");
			val.Add(VALUE_KEY, (object)value.ToString());
			return val;
		}

		private Dictionary<string, object> EncodeSimpleValue(int value)
		{
			Dictionary<string, object> val = new Dictionary<string, object>();
			val.Add(PRIMITIVE_KEY, (object)"int");
			val.Add(VALUE_KEY, (object)value.ToString());
			return val;
		}

		private Dictionary<string, object> EncodeSimpleValue(byte value)
		{
			Dictionary<string, object> val = new Dictionary<string, object>();
			val.Add(PRIMITIVE_KEY, (object)"byte");
			val.Add(VALUE_KEY, (object)value.ToString());
			return val;
		}

		private Dictionary<string, object> EncodeSimpleValue(bool value)
		{
			Dictionary<string, object> val = new Dictionary<string, object>();
			val.Add(PRIMITIVE_KEY, (object)"bool");
			val.Add(VALUE_KEY, (object)value.ToString());
			return val;
		}

		private Dictionary<string, object> EncodeSimpleValue(char value)
		{
			Dictionary<string, object> val = new Dictionary<string, object>();
			val.Add(PRIMITIVE_KEY, (object)"char");
			val.Add(VALUE_KEY, (object)value.ToString());
			return val;
		}

		private Dictionary<string, object> EncodeSimpleValue(sbyte value)
		{
			Dictionary<string, object> val = new Dictionary<string, object>();
			val.Add(PRIMITIVE_KEY, (object)"sbyte");
			val.Add(VALUE_KEY, (object)value.ToString());
			return val;
		}

		private Dictionary<string, object> EncodeSimpleValue(double value)
		{
			Dictionary<string, object> val = new Dictionary<string, object>();
			val.Add(PRIMITIVE_KEY, (object)"double");
			val.Add(VALUE_KEY, (object)value.ToString());
			return val;
		}

		private Dictionary<string, object> EncodeSimpleValue(decimal value)
		{
			Dictionary<string, object> val = new Dictionary<string, object>();
			val.Add(PRIMITIVE_KEY, (object)"decimal");
			val.Add(VALUE_KEY, (object)value.ToString());
			return val;
		}

		private Dictionary<string, object> EncodeSimpleValue(global::System.DateTime value)
		{
			Dictionary<string, object> val = new Dictionary<string, object>();
			val.Add(PRIMITIVE_KEY, (object)"#DateTime");
			val.Add(VALUE_KEY, (object)((object)value).ToString());
			return val;
		}

		private Dictionary<string, object> EncodeSimpleValue(float value)
		{
			Dictionary<string, object> val = new Dictionary<string, object>();
			val.Add(PRIMITIVE_KEY, (object)"float");
			val.Add(VALUE_KEY, (object)value.ToString());
			return val;
		}

		private Dictionary<string, object> EncodeSimpleValue(uint value)
		{
			Dictionary<string, object> val = new Dictionary<string, object>();
			val.Add(PRIMITIVE_KEY, (object)"uint");
			val.Add(VALUE_KEY, (object)value.ToString());
			return val;
		}

		private Dictionary<string, object> EncodeSimpleValue(long value)
		{
			Dictionary<string, object> val = new Dictionary<string, object>();
			val.Add(PRIMITIVE_KEY, (object)"long");
			val.Add(VALUE_KEY, (object)value.ToString());
			return val;
		}

		private Dictionary<string, object> EncodeSimpleValue(ulong value)
		{
			Dictionary<string, object> val = new Dictionary<string, object>();
			val.Add(PRIMITIVE_KEY, (object)"ulong");
			val.Add(VALUE_KEY, (object)value.ToString());
			return val;
		}

		private Dictionary<string, object> EncodeSimpleValue(string value)
		{
			Dictionary<string, object> val = new Dictionary<string, object>();
			val.Add(PRIMITIVE_KEY, (object)"string");
			val.Add(VALUE_KEY, (object)value);
			return val;
		}

		private Dictionary<string, object> EncodeNullValue()
		{
			Dictionary<string, object> val = new Dictionary<string, object>();
			val.Add(PRIMITIVE_KEY, (object)"null");
			return val;
		}

		private object EncodeValue(object value)
		{
			if (value is bool)
			{
				return EncodeSimpleValue((bool)value);
			}
			if (value is short)
			{
				return EncodeSimpleValue((short)value);
			}
			if (value is ushort)
			{
				return EncodeSimpleValue((ushort)value);
			}
			if (value is int)
			{
				return EncodeSimpleValue((int)value);
			}
			if (value is byte)
			{
				return EncodeSimpleValue((byte)value);
			}
			if (value is char)
			{
				return EncodeSimpleValue((char)value);
			}
			if (value is sbyte)
			{
				return EncodeSimpleValue((sbyte)value);
			}
			if (value is double)
			{
				return EncodeSimpleValue((double)value);
			}
			if (value is decimal)
			{
				return EncodeSimpleValue((decimal)value);
			}
			if (value is global::System.DateTime)
			{
				return EncodeSimpleValue((global::System.DateTime)value);
			}
			if (value is uint)
			{
				return EncodeSimpleValue((uint)value);
			}
			if (value is long)
			{
				return EncodeSimpleValue((long)value);
			}
			if (value is ulong)
			{
				return EncodeSimpleValue((ulong)value);
			}
			if (value is string)
			{
				return EncodeSimpleValue((string)value);
			}
			if (value is float)
			{
				return EncodeSimpleValue((float)value);
			}
			if (value == null)
			{
				return EncodeNullValue();
			}
			return Encode(value);
		}

		public void AddValue(string name, object value)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			if (objectData.ContainsKey(name))
			{
				throw new ArgumentException("value is already serialized.");
			}
			object obj = EncodeValue(value);
			objectData.Add(name, obj);
		}

		private JSONNode GetDictionary(IDictionary dictionary)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			JSONClass jSONClass = new JSONClass();
			IDictionaryEnumerator enumerator = dictionary.GetEnumerator();
			try
			{
				while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
				{
					DictionaryEntry val = (DictionaryEntry)((global::System.Collections.IEnumerator)enumerator).get_Current();
					if (((DictionaryEntry)(ref val)).get_Value() is ObjectInfo)
					{
						jSONClass.Add(((DictionaryEntry)(ref val)).get_Key() as string, (((DictionaryEntry)(ref val)).get_Value() as ObjectInfo).ToJson());
					}
					else if (((DictionaryEntry)(ref val)).get_Value() is IDictionary)
					{
						string aKey = ((DictionaryEntry)(ref val)).get_Key() as string;
						object value = ((DictionaryEntry)(ref val)).get_Value();
						jSONClass.Add(aKey, GetDictionary((IDictionary)((value is IDictionary) ? value : null)));
					}
					else if (((DictionaryEntry)(ref val)).get_Value() is global::System.Collections.IList)
					{
						jSONClass.Add(((DictionaryEntry)(ref val)).get_Key() as string, GetArray(((DictionaryEntry)(ref val)).get_Value() as global::System.Collections.IList));
					}
					else
					{
						jSONClass.Add(((DictionaryEntry)(ref val)).get_Key() as string, new JSONData(((DictionaryEntry)(ref val)).get_Value()));
					}
				}
				return jSONClass;
			}
			finally
			{
				global::System.IDisposable disposable = enumerator as global::System.IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}

		private JSONNode GetArray(global::System.Collections.IList array)
		{
			JSONArray jSONArray = new JSONArray();
			global::System.Collections.IEnumerator enumerator = ((global::System.Collections.IEnumerable)array).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.get_Current();
					if (current is IDictionary)
					{
						jSONArray.Add(GetDictionary((IDictionary)((current is IDictionary) ? current : null)));
					}
					else if (current is global::System.Collections.IList)
					{
						jSONArray.Add(GetArray(current as global::System.Collections.IList));
					}
					else if (current is ObjectInfo)
					{
						jSONArray.Add((current as ObjectInfo).ToJson());
					}
					else
					{
						jSONArray.Add(new JSONData(current));
					}
				}
				return jSONArray;
			}
			finally
			{
				global::System.IDisposable disposable = enumerator as global::System.IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}

		public JSONNode ToJson()
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			JSONClass jSONClass = new JSONClass();
			jSONClass.Add(SERIAL_VERSION_ID_KEY, new JSONData(SerialVersionId));
			jSONClass.Add(TYPE_KEY, new JSONData(TypeInfo));
			Enumerator<string, object> enumerator = objectData.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, object> current = enumerator.get_Current();
					if (current.get_Value() is ObjectInfo)
					{
						jSONClass.Add(current.get_Key(), (current.get_Value() as ObjectInfo).ToJson());
					}
					else if (current.get_Value() is IDictionary)
					{
						string key = current.get_Key();
						object value = current.get_Value();
						jSONClass.Add(key, GetDictionary((IDictionary)((value is IDictionary) ? value : null)));
					}
					else if (current.get_Value() is global::System.Collections.IList)
					{
						jSONClass.Add(current.get_Key(), GetArray(current.get_Value() as global::System.Collections.IList));
					}
					else
					{
						jSONClass.Add(current.get_Key(), new JSONData(current.get_Value()));
					}
				}
				return jSONClass;
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}
	}
}
