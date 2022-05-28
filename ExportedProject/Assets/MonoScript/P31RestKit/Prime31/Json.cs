using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Prime31
{
	public class Json
	{
		internal class ObjectDecoder
		{
			[CompilerGenerated]
			private sealed class _003CgetMembersWithSetters_003Ec__AnonStorey0
			{
				internal FieldInfo theInfo;

				internal global::System.Type theFieldType;

				internal void _003C_003Em__0(object ownerObject, object val)
				{
					theInfo.SetValue(ownerObject, Convert.ChangeType(val, theFieldType));
				}
			}

			[CompilerGenerated]
			private sealed class _003CgetMembersWithSetters_003Ec__AnonStorey1
			{
				internal PropertyInfo theInfo;

				internal global::System.Type thePropertyType;

				internal void _003C_003Em__0(object ownerObject, object val)
				{
					theInfo.SetValue(ownerObject, Convert.ChangeType(val, thePropertyType), (object[])default(object[]));
				}
			}

			private Dictionary<string, Action<object, object>> _memberInfo;

			public static object decode<T>(string json, string rootElement = null) where T : new()
			{
				object obj = Json.decode(json);
				if (obj == null)
				{
					return null;
				}
				return new ObjectDecoder().decode<T>(obj, rootElement);
			}

			private object decode<T>(object decodedJsonObject, string rootElement = null) where T : new()
			{
				if (rootElement != null)
				{
					IDictionary val = (IDictionary)((decodedJsonObject is IDictionary) ? decodedJsonObject : null);
					if (val == null)
					{
						Utils.logObject(string.Concat(new object[4] { "A rootElement was requested (", rootElement, ") but the json did not decode to a Dictionary. It decoded to: ", decodedJsonObject }));
						return null;
					}
					if (!val.Contains((object)rootElement))
					{
						Utils.logObject("A rootElement was requested (" + rootElement + ") but does not exist in the decoded Dictionary");
						return null;
					}
					decodedJsonObject = val.get_Item((object)rootElement);
				}
				global::System.Type typeFromHandle = typeof(T);
				global::System.Collections.IList list = null;
				if (typeFromHandle.get_IsGenericType() && typeFromHandle.GetGenericTypeDefinition() == typeof(List<>))
				{
					list = new T() as global::System.Collections.IList;
					typeFromHandle = ((object)list).GetType().GetGenericArguments()[0];
					if (!(decodedJsonObject is global::System.Collections.IList) || !decodedJsonObject.GetType().get_IsGenericType())
					{
						Utils.logObject(string.Concat((object)"A List was required but the json did not decode to a List. It decoded to: ", decodedJsonObject));
						return null;
					}
					global::System.Collections.IEnumerator enumerator = ((global::System.Collections.IEnumerable)(global::System.Collections.IList)decodedJsonObject).GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							Dictionary<string, object> val2 = (Dictionary<string, object>)enumerator.get_Current();
							if (val2 == null)
							{
								Utils.logObject("Aborted populating List because the json did not decode to a List of Dictionaries");
								return list;
							}
							list.Add(createAndPopulateObjectFromDictionary(typeFromHandle, val2));
						}
						return list;
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
				return createAndPopulateObjectFromDictionary(typeFromHandle, decodedJsonObject as Dictionary<string, object>);
			}

			private Dictionary<string, Action<object, object>> getMemberInfoForObject(object obj)
			{
				if (_memberInfo == null)
				{
					_memberInfo = getMembersWithSetters(obj);
				}
				return _memberInfo;
			}

			private static Dictionary<string, Action<object, object>> getMembersWithSetters(object obj)
			{
				Dictionary<string, Action<object, object>> val = new Dictionary<string, Action<object, object>>();
				FieldInfo[] fields = obj.GetType().GetFields((BindingFlags)52);
				foreach (FieldInfo val2 in fields)
				{
					_003CgetMembersWithSetters_003Ec__AnonStorey0 _003CgetMembersWithSetters_003Ec__AnonStorey = new _003CgetMembersWithSetters_003Ec__AnonStorey0();
					if (val2.get_FieldType().get_Namespace().StartsWith("System"))
					{
						_003CgetMembersWithSetters_003Ec__AnonStorey.theInfo = val2;
						_003CgetMembersWithSetters_003Ec__AnonStorey.theFieldType = val2.get_FieldType();
						val.set_Item(((MemberInfo)val2).get_Name(), (Action<object, object>)_003CgetMembersWithSetters_003Ec__AnonStorey._003C_003Em__0);
					}
				}
				PropertyInfo[] properties = obj.GetType().GetProperties((BindingFlags)52);
				foreach (PropertyInfo val3 in properties)
				{
					if (val3.get_PropertyType().get_Namespace().StartsWith("System") && val3.get_CanWrite() && val3.GetSetMethod(true) != null)
					{
						_003CgetMembersWithSetters_003Ec__AnonStorey1 _003CgetMembersWithSetters_003Ec__AnonStorey2 = new _003CgetMembersWithSetters_003Ec__AnonStorey1();
						_003CgetMembersWithSetters_003Ec__AnonStorey2.theInfo = val3;
						_003CgetMembersWithSetters_003Ec__AnonStorey2.thePropertyType = val3.get_PropertyType();
						val.set_Item(((MemberInfo)val3).get_Name(), (Action<object, object>)_003CgetMembersWithSetters_003Ec__AnonStorey2._003C_003Em__0);
					}
				}
				return val;
			}

			public object createAndPopulateObjectFromDictionary(global::System.Type objectType, Dictionary<string, object> dict)
			{
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				object obj = Activator.CreateInstance(objectType);
				Dictionary<string, Action<object, object>> memberInfoForObject = getMemberInfoForObject(obj);
				KeyCollection<string, object> keys = dict.get_Keys();
				Enumerator<string, object> enumerator = keys.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.get_Current();
						if (memberInfoForObject.ContainsKey(current))
						{
							try
							{
								memberInfoForObject.get_Item(current).Invoke(obj, dict.get_Item(current));
							}
							catch (global::System.Exception obj2)
							{
								Utils.logObject(obj2);
							}
						}
					}
					return obj;
				}
				finally
				{
					((global::System.IDisposable)enumerator).Dispose();
				}
			}
		}

		internal class Deserializer
		{
			private enum JsonToken
			{
				None = 0,
				CurlyOpen = 1,
				CurlyClose = 2,
				SquaredOpen = 3,
				SquaredClose = 4,
				Colon = 5,
				Comma = 6,
				String = 7,
				Number = 8,
				True = 9,
				False = 10,
				Null = 11
			}

			private char[] charArray;

			private Deserializer(string json)
			{
				charArray = json.ToCharArray();
			}

			public static object deserialize(string json)
			{
				if (json != null)
				{
					Deserializer deserializer = new Deserializer(json);
					return deserializer.deserialize();
				}
				return null;
			}

			private object deserialize()
			{
				int index = 0;
				return parseValue(charArray, ref index);
			}

			protected object parseValue(char[] json, ref int index)
			{
				switch (lookAhead(json, index))
				{
				case JsonToken.String:
					return parseString(json, ref index);
				case JsonToken.Number:
					return parseNumber(json, ref index);
				case JsonToken.CurlyOpen:
					return parseObject(json, ref index);
				case JsonToken.SquaredOpen:
					return parseArray(json, ref index);
				case JsonToken.True:
					nextToken(json, ref index);
					return bool.Parse("TRUE");
				case JsonToken.False:
					nextToken(json, ref index);
					return bool.Parse("FALSE");
				case JsonToken.Null:
					nextToken(json, ref index);
					return null;
				default:
					return null;
				}
			}

			private IDictionary parseObject(char[] json, ref int index)
			{
				IDictionary val = (IDictionary)(object)new Dictionary<string, object>();
				nextToken(json, ref index);
				bool flag = false;
				while (!flag)
				{
					switch (lookAhead(json, index))
					{
					case JsonToken.None:
						return null;
					case JsonToken.Comma:
						nextToken(json, ref index);
						continue;
					case JsonToken.CurlyClose:
						nextToken(json, ref index);
						return val;
					}
					string text = parseString(json, ref index);
					if (text == null)
					{
						return null;
					}
					JsonToken jsonToken = nextToken(json, ref index);
					if (jsonToken != JsonToken.Colon)
					{
						return null;
					}
					object obj = parseValue(json, ref index);
					val.set_Item((object)text, obj);
				}
				return val;
			}

			private global::System.Collections.IList parseArray(char[] json, ref int index)
			{
				List<object> val = new List<object>();
				nextToken(json, ref index);
				bool flag = false;
				while (!flag)
				{
					switch (lookAhead(json, index))
					{
					case JsonToken.None:
						return null;
					case JsonToken.Comma:
						nextToken(json, ref index);
						continue;
					case JsonToken.SquaredClose:
						break;
					default:
					{
						object obj = parseValue(json, ref index);
						val.Add(obj);
						continue;
					}
					}
					nextToken(json, ref index);
					break;
				}
				return (global::System.Collections.IList)val;
			}

			private string parseString(char[] json, ref int index)
			{
				string text = string.Empty;
				eatWhitespace(json, ref index);
				char c = json[index++];
				bool flag = false;
				while (!flag && index != json.Length)
				{
					c = json[index++];
					switch (c)
					{
					case '"':
						flag = true;
						break;
					case '\\':
					{
						if (index == json.Length)
						{
							break;
						}
						switch (json[index++])
						{
						case '"':
							text = string.Concat((object)text, (object)'"');
							continue;
						case '\\':
							text = string.Concat((object)text, (object)'\\');
							continue;
						case '/':
							text = string.Concat((object)text, (object)'/');
							continue;
						case 'b':
							text = string.Concat((object)text, (object)'\b');
							continue;
						case 'f':
							text = string.Concat((object)text, (object)'\f');
							continue;
						case 'n':
							text = string.Concat((object)text, (object)'\n');
							continue;
						case 'r':
							text = string.Concat((object)text, (object)'\r');
							continue;
						case 't':
							text = string.Concat((object)text, (object)'\t');
							continue;
						case 'u':
							break;
						default:
							continue;
						}
						int num = json.Length - index;
						if (num < 4)
						{
							break;
						}
						char[] array = new char[4];
						global::System.Array.Copy((global::System.Array)json, index, (global::System.Array)array, 0, 4);
						uint num2 = uint.Parse(new string(array), (NumberStyles)515);
						try
						{
							text += char.ConvertFromUtf32((int)num2);
						}
						catch (global::System.Exception)
						{
							char[] array2 = array;
							foreach (char c2 in array2)
							{
								text = string.Concat((object)text, (object)c2);
							}
						}
						index += 4;
						continue;
					}
					default:
						text = string.Concat((object)text, (object)c);
						continue;
					}
					break;
				}
				if (!flag)
				{
					return null;
				}
				return text;
			}

			private object parseNumber(char[] json, ref int index)
			{
				eatWhitespace(json, ref index);
				int lastIndexOfNumber = getLastIndexOfNumber(json, index);
				int num = lastIndexOfNumber - index + 1;
				char[] array = new char[num];
				global::System.Array.Copy((global::System.Array)json, index, (global::System.Array)array, 0, num);
				index = lastIndexOfNumber + 1;
				string text = new string(array);
				long num2 = default(long);
				if (!text.Contains(".") && long.TryParse(text, (NumberStyles)7, (IFormatProvider)(object)CultureInfo.get_InvariantCulture(), ref num2))
				{
					return num2;
				}
				return double.Parse(new string(array), (IFormatProvider)(object)CultureInfo.get_InvariantCulture());
			}

			private int getLastIndexOfNumber(char[] json, int index)
			{
				int i;
				for (i = index; i < json.Length && "0123456789+-.eE".IndexOf(json[i]) != -1; i++)
				{
				}
				return i - 1;
			}

			private void eatWhitespace(char[] json, ref int index)
			{
				while (index < json.Length && " \t\n\r".IndexOf(json[index]) != -1)
				{
					index++;
				}
			}

			private JsonToken lookAhead(char[] json, int index)
			{
				int index2 = index;
				return nextToken(json, ref index2);
			}

			private JsonToken nextToken(char[] json, ref int index)
			{
				eatWhitespace(json, ref index);
				if (index == json.Length)
				{
					return JsonToken.None;
				}
				char c = json[index];
				index++;
				switch (c)
				{
				case '{':
					return JsonToken.CurlyOpen;
				case '}':
					return JsonToken.CurlyClose;
				case '[':
					return JsonToken.SquaredOpen;
				case ']':
					return JsonToken.SquaredClose;
				case ',':
					return JsonToken.Comma;
				case '"':
					return JsonToken.String;
				case '-':
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					return JsonToken.Number;
				case ':':
					return JsonToken.Colon;
				default:
				{
					index--;
					int num = json.Length - index;
					if (num >= 5 && json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
					{
						index += 5;
						return JsonToken.False;
					}
					if (num >= 4 && json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
					{
						index += 4;
						return JsonToken.True;
					}
					if (num >= 4 && json[index] == 'n' && json[index + 1] == 'u' && json[index + 2] == 'l' && json[index + 3] == 'l')
					{
						index += 4;
						return JsonToken.Null;
					}
					return JsonToken.None;
				}
				}
			}
		}

		internal class Serializer
		{
			private StringBuilder _builder;

			private Serializer()
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				_builder = new StringBuilder();
			}

			public static string serialize(object obj)
			{
				Serializer serializer = new Serializer();
				serializer.serializeObject(obj);
				return ((object)serializer._builder).ToString();
			}

			private void serializeObject(object value)
			{
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0087: Expected O, but got Unknown
				//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
				//IL_0101: Unknown result type (might be due to invalid IL or missing references)
				if (value == null)
				{
					_builder.Append("null");
					return;
				}
				if (value is string)
				{
					serializeString((string)value);
					return;
				}
				if (value is global::System.Collections.IList)
				{
					serializeIList((global::System.Collections.IList)value);
					return;
				}
				if (value is Dictionary<string, object>)
				{
					serializeDictionary((Dictionary<string, object>)value);
					return;
				}
				if (value is IDictionary)
				{
					serializeIDictionary((IDictionary)value);
					return;
				}
				if (value is bool)
				{
					_builder.Append(value.ToString().ToLower());
					return;
				}
				if (value.GetType().get_IsPrimitive())
				{
					_builder.Append(value);
					return;
				}
				if (value is global::System.DateTime)
				{
					global::System.DateTime dateTime = new global::System.DateTime(1970, 1, 1, 0, 0, 0, (DateTimeKind)1);
					TimeSpan val = ((global::System.DateTime)value).Subtract(dateTime);
					double totalMilliseconds = ((TimeSpan)(ref val)).get_TotalMilliseconds();
					serializeString(Convert.ToString(totalMilliseconds, (IFormatProvider)(object)CultureInfo.get_InvariantCulture()));
					return;
				}
				try
				{
					serializeClass(value);
				}
				catch (global::System.Exception ex)
				{
					Utils.logObject(string.Format("failed to serialize {0} with error: {1}", value, (object)ex.get_Message()));
				}
			}

			private void serializeIList(global::System.Collections.IList anArray)
			{
				_builder.Append("[");
				bool flag = true;
				for (int i = 0; i < ((global::System.Collections.ICollection)anArray).get_Count(); i++)
				{
					object value = anArray.get_Item(i);
					if (!flag)
					{
						_builder.Append(", ");
					}
					serializeObject(value);
					flag = false;
				}
				_builder.Append("]");
			}

			private void serializeIDictionary(IDictionary dict)
			{
				_builder.Append("{");
				bool flag = true;
				global::System.Collections.IEnumerator enumerator = ((global::System.Collections.IEnumerable)dict.get_Keys()).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object current = enumerator.get_Current();
						if (!flag)
						{
							_builder.Append(", ");
						}
						serializeString(current.ToString());
						_builder.Append(":");
						serializeObject(dict.get_Item(current));
						flag = false;
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
				_builder.Append("}");
			}

			private void serializeDictionary(Dictionary<string, object> dict)
			{
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				_builder.Append("{");
				bool flag = true;
				KeyCollection<string, object> keys = dict.get_Keys();
				Enumerator<string, object> enumerator = keys.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.get_Current();
						if (!flag)
						{
							_builder.Append(", ");
						}
						serializeString(((object)current).ToString());
						_builder.Append(":");
						serializeObject(dict.get_Item(current));
						flag = false;
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator).Dispose();
				}
				_builder.Append("}");
			}

			private void serializeString(string str)
			{
				_builder.Append("\"");
				char[] array = str.ToCharArray();
				foreach (char c in array)
				{
					switch (c)
					{
					case '"':
						_builder.Append("\\\"");
						continue;
					case '\\':
						_builder.Append("\\\\");
						continue;
					case '\b':
						_builder.Append("\\b");
						continue;
					case '\f':
						_builder.Append("\\f");
						continue;
					case '\n':
						_builder.Append("\\n");
						continue;
					case '\r':
						_builder.Append("\\r");
						continue;
					case '\t':
						_builder.Append("\\t");
						continue;
					}
					int num = Convert.ToInt32((object)c, (IFormatProvider)(object)CultureInfo.get_InvariantCulture());
					if (num >= 32 && num <= 126)
					{
						_builder.Append(c);
					}
					else
					{
						_builder.Append("\\u" + Convert.ToString(num, 16).PadLeft(4, '0'));
					}
				}
				_builder.Append("\"");
			}

			private void serializeClass(object value)
			{
				_builder.Append("{");
				bool flag = true;
				FieldInfo[] fields = value.GetType().GetFields((BindingFlags)52);
				foreach (FieldInfo val in fields)
				{
					if (!val.get_IsPrivate() || !((MemberInfo)val).get_Name().Contains("k__BackingField"))
					{
						if (!flag)
						{
							_builder.Append(", ");
						}
						serializeString(((MemberInfo)val).get_Name());
						_builder.Append(":");
						serializeObject(val.GetValue(value));
						flag = false;
					}
				}
				PropertyInfo[] properties = value.GetType().GetProperties((BindingFlags)52);
				foreach (PropertyInfo val2 in properties)
				{
					if (!flag)
					{
						_builder.Append(", ");
					}
					serializeString(((MemberInfo)val2).get_Name());
					_builder.Append(":");
					serializeObject(val2.GetValue(value, (object[])default(object[])));
					flag = false;
				}
				_builder.Append("}");
			}
		}

		public static bool useSimpleJson = true;

		public static object decode(string json)
		{
			if (useSimpleJson)
			{
				return SimpleJson.decode(json);
			}
			object obj = Deserializer.deserialize(json);
			if (obj == null)
			{
				Utils.logObject("Something went wrong deserializing the json. We got a null return. Here is the json we tried to deserialize: " + json);
			}
			return obj;
		}

		public static T decode<T>(string json, string rootElement = null) where T : new()
		{
			if (useSimpleJson)
			{
				return SimpleJson.decode<T>(json, rootElement);
			}
			return (T)ObjectDecoder.decode<T>(json, rootElement);
		}

		public static T decodeObject<T>(object jsonObject, string rootElement = null) where T : new()
		{
			return SimpleJson.decodeObject<T>(jsonObject, rootElement);
		}

		public static string encode(object obj)
		{
			string text = ((!useSimpleJson) ? Serializer.serialize(obj) : SimpleJson.encode(obj));
			if (text == null)
			{
				Utils.logObject("Something went wrong serializing the object. We got a null return. Here is the object we tried to deserialize: ");
				Utils.logObject(obj);
			}
			return text;
		}

		public static object jsonDecode(string json)
		{
			return decode(json);
		}

		public static string jsonEncode(object obj)
		{
			string text = Serializer.serialize(obj);
			if (text == null)
			{
				Utils.logObject("Something went wrong serializing the object. We got a null return. Here is the object we tried to deserialize: ");
				Utils.logObject(obj);
			}
			return text;
		}
	}
}
