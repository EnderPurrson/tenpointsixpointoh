using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;

namespace FyberPlugin.LitJson
{
	[DefaultMember("Item")]
	public class JsonData : IJsonWrapper, global::System.Collections.IList, global::System.Collections.ICollection, IDictionary, global::System.Collections.IEnumerable, IOrderedDictionary, IEquatable<JsonData>
	{
		private global::System.Collections.Generic.IList<JsonData> inst_array;

		private bool inst_boolean;

		private double inst_double;

		private int inst_int;

		private long inst_long;

		private IDictionary<string, JsonData> inst_object;

		private string inst_string;

		private string json;

		private JsonType type;

		private global::System.Collections.Generic.IList<KeyValuePair<string, JsonData>> object_list;

		int Count
		{
			get
			{
				return Count;
			}
		}

		bool IsSynchronized
		{
			get
			{
				return EnsureCollection().get_IsSynchronized();
			}
		}

		object SyncRoot
		{
			get
			{
				return EnsureCollection().get_SyncRoot();
			}
		}

		bool IsFixedSize
		{
			get
			{
				return EnsureDictionary().get_IsFixedSize();
			}
		}

		bool IsReadOnly
		{
			get
			{
				return EnsureDictionary().get_IsReadOnly();
			}
		}

		global::System.Collections.ICollection Keys
		{
			get
			{
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				EnsureDictionary();
				global::System.Collections.Generic.IList<string> list = (global::System.Collections.Generic.IList<string>)new List<string>();
				global::System.Collections.Generic.IEnumerator<KeyValuePair<string, JsonData>> enumerator = ((global::System.Collections.Generic.IEnumerable<KeyValuePair<string, JsonData>>)object_list).GetEnumerator();
				try
				{
					while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
					{
						((global::System.Collections.Generic.ICollection<string>)list).Add(enumerator.get_Current().get_Key());
					}
				}
				finally
				{
					if (enumerator != null)
					{
						((global::System.IDisposable)enumerator).Dispose();
					}
				}
				return (global::System.Collections.ICollection)list;
			}
		}

		global::System.Collections.ICollection Values
		{
			get
			{
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				EnsureDictionary();
				global::System.Collections.Generic.IList<JsonData> list = (global::System.Collections.Generic.IList<JsonData>)new List<JsonData>();
				global::System.Collections.Generic.IEnumerator<KeyValuePair<string, JsonData>> enumerator = ((global::System.Collections.Generic.IEnumerable<KeyValuePair<string, JsonData>>)object_list).GetEnumerator();
				try
				{
					while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
					{
						((global::System.Collections.Generic.ICollection<JsonData>)list).Add(enumerator.get_Current().get_Value());
					}
				}
				finally
				{
					if (enumerator != null)
					{
						((global::System.IDisposable)enumerator).Dispose();
					}
				}
				return (global::System.Collections.ICollection)list;
			}
		}

		bool IJsonWrapper.IsArray
		{
			get
			{
				return IsArray;
			}
		}

		bool IJsonWrapper.IsBoolean
		{
			get
			{
				return IsBoolean;
			}
		}

		bool IJsonWrapper.IsDouble
		{
			get
			{
				return IsDouble;
			}
		}

		bool IJsonWrapper.IsInt
		{
			get
			{
				return IsInt;
			}
		}

		bool IJsonWrapper.IsLong
		{
			get
			{
				return IsLong;
			}
		}

		bool IJsonWrapper.IsObject
		{
			get
			{
				return IsObject;
			}
		}

		bool IJsonWrapper.IsString
		{
			get
			{
				return IsString;
			}
		}

		bool IsFixedSize
		{
			get
			{
				return EnsureList().get_IsFixedSize();
			}
		}

		bool IsReadOnly
		{
			get
			{
				return EnsureList().get_IsReadOnly();
			}
		}

		object Item
		{
			get
			{
				return EnsureDictionary().get_Item(key);
			}
			set
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				if (!(key is string))
				{
					throw new ArgumentException("The key has to be a string");
				}
				JsonData value2 = ToJsonData(value);
				this[(string)key] = value2;
			}
		}

		object Item
		{
			get
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				EnsureDictionary();
				return object_list.get_Item(idx).get_Value();
			}
			set
			{
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				EnsureDictionary();
				JsonData jsonData = ToJsonData(value);
				KeyValuePair<string, JsonData> val = object_list.get_Item(idx);
				inst_object.set_Item(val.get_Key(), jsonData);
				KeyValuePair<string, JsonData> val2 = default(KeyValuePair<string, JsonData>);
				val2._002Ector(val.get_Key(), jsonData);
				object_list.set_Item(idx, val2);
			}
		}

		object Item
		{
			get
			{
				return EnsureList().get_Item(index);
			}
			set
			{
				EnsureList();
				JsonData jsonData2 = (this[index] = ToJsonData(value));
			}
		}

		public int Count
		{
			get
			{
				return EnsureCollection().get_Count();
			}
		}

		public bool IsArray
		{
			get
			{
				return type == JsonType.Array;
			}
		}

		public bool IsBoolean
		{
			get
			{
				return type == JsonType.Boolean;
			}
		}

		public bool IsDouble
		{
			get
			{
				return type == JsonType.Double;
			}
		}

		public bool IsInt
		{
			get
			{
				return type == JsonType.Int;
			}
		}

		public bool IsLong
		{
			get
			{
				return type == JsonType.Long;
			}
		}

		public bool IsObject
		{
			get
			{
				return type == JsonType.Object;
			}
		}

		public bool IsString
		{
			get
			{
				return type == JsonType.String;
			}
		}

		public global::System.Collections.Generic.ICollection<string> Keys
		{
			get
			{
				EnsureDictionary();
				return inst_object.get_Keys();
			}
		}

		public JsonData this[string prop_name]
		{
			get
			{
				EnsureDictionary();
				return inst_object.get_Item(prop_name);
			}
			set
			{
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0079: Unknown result type (might be due to invalid IL or missing references)
				EnsureDictionary();
				KeyValuePair<string, JsonData> val = default(KeyValuePair<string, JsonData>);
				val._002Ector(prop_name, value);
				if (inst_object.ContainsKey(prop_name))
				{
					for (int i = 0; i < ((global::System.Collections.Generic.ICollection<KeyValuePair<string, JsonData>>)object_list).get_Count(); i++)
					{
						if (object_list.get_Item(i).get_Key() == prop_name)
						{
							object_list.set_Item(i, val);
							break;
						}
					}
				}
				else
				{
					((global::System.Collections.Generic.ICollection<KeyValuePair<string, JsonData>>)object_list).Add(val);
				}
				inst_object.set_Item(prop_name, value);
				json = null;
			}
		}

		public JsonData this[int index]
		{
			get
			{
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				EnsureCollection();
				if (type == JsonType.Array)
				{
					return inst_array.get_Item(index);
				}
				return object_list.get_Item(index).get_Value();
			}
			set
			{
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				EnsureCollection();
				if (type == JsonType.Array)
				{
					inst_array.set_Item(index, value);
				}
				else
				{
					KeyValuePair<string, JsonData> val = object_list.get_Item(index);
					KeyValuePair<string, JsonData> val2 = default(KeyValuePair<string, JsonData>);
					val2._002Ector(val.get_Key(), value);
					object_list.set_Item(index, val2);
					inst_object.set_Item(val.get_Key(), value);
				}
				json = null;
			}
		}

		public JsonData()
		{
		}

		public JsonData(bool boolean)
		{
			type = JsonType.Boolean;
			inst_boolean = boolean;
		}

		public JsonData(double number)
		{
			type = JsonType.Double;
			inst_double = number;
		}

		public JsonData(int number)
		{
			type = JsonType.Int;
			inst_int = number;
		}

		public JsonData(long number)
		{
			type = JsonType.Long;
			inst_long = number;
		}

		public JsonData(object obj)
		{
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			if (obj is bool)
			{
				type = JsonType.Boolean;
				inst_boolean = (bool)obj;
				return;
			}
			if (obj is double)
			{
				type = JsonType.Double;
				inst_double = (double)obj;
				return;
			}
			if (obj is int)
			{
				type = JsonType.Int;
				inst_int = (int)obj;
				return;
			}
			if (obj is long)
			{
				type = JsonType.Long;
				inst_long = (long)obj;
				return;
			}
			if (obj is string)
			{
				type = JsonType.String;
				inst_string = (string)obj;
				return;
			}
			throw new ArgumentException("Unable to wrap the given object with JsonData");
		}

		public JsonData(string str)
		{
			type = JsonType.String;
			inst_string = str;
		}

		void global::System.Collections.ICollection.CopyTo(global::System.Array array, int index)
		{
			EnsureCollection().CopyTo(array, index);
		}

		void IDictionary.Add(object key, object value)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			JsonData jsonData = ToJsonData(value);
			EnsureDictionary().Add(key, (object)jsonData);
			KeyValuePair<string, JsonData> val = default(KeyValuePair<string, JsonData>);
			val._002Ector((string)key, jsonData);
			((global::System.Collections.Generic.ICollection<KeyValuePair<string, JsonData>>)object_list).Add(val);
			json = null;
		}

		void IDictionary.Clear()
		{
			EnsureDictionary().Clear();
			((global::System.Collections.Generic.ICollection<KeyValuePair<string, JsonData>>)object_list).Clear();
			json = null;
		}

		bool IDictionary.Contains(object key)
		{
			return EnsureDictionary().Contains(key);
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return ((IOrderedDictionary)this).GetEnumerator();
		}

		void IDictionary.Remove(object key)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			EnsureDictionary().Remove(key);
			for (int i = 0; i < ((global::System.Collections.Generic.ICollection<KeyValuePair<string, JsonData>>)object_list).get_Count(); i++)
			{
				if (object_list.get_Item(i).get_Key() == (string)key)
				{
					object_list.RemoveAt(i);
					break;
				}
			}
			json = null;
		}

		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return ((global::System.Collections.IEnumerable)EnsureCollection()).GetEnumerator();
		}

		bool IJsonWrapper.GetBoolean()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			if (type != JsonType.Boolean)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a boolean");
			}
			return inst_boolean;
		}

		double IJsonWrapper.GetDouble()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			if (type != JsonType.Double)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a double");
			}
			return inst_double;
		}

		int IJsonWrapper.GetInt()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			if (type != JsonType.Int)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold an int");
			}
			return inst_int;
		}

		long IJsonWrapper.GetLong()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			if (type != JsonType.Long)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a long");
			}
			return inst_long;
		}

		string IJsonWrapper.GetString()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			if (type != JsonType.String)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a string");
			}
			return inst_string;
		}

		void IJsonWrapper.SetBoolean(bool val)
		{
			type = JsonType.Boolean;
			inst_boolean = val;
			json = null;
		}

		void IJsonWrapper.SetDouble(double val)
		{
			type = JsonType.Double;
			inst_double = val;
			json = null;
		}

		void IJsonWrapper.SetInt(int val)
		{
			type = JsonType.Int;
			inst_int = val;
			json = null;
		}

		void IJsonWrapper.SetLong(long val)
		{
			type = JsonType.Long;
			inst_long = val;
			json = null;
		}

		void IJsonWrapper.SetString(string val)
		{
			type = JsonType.String;
			inst_string = val;
			json = null;
		}

		string IJsonWrapper.ToJson()
		{
			return ToJson();
		}

		void IJsonWrapper.ToJson(JsonWriter writer)
		{
			ToJson(writer);
		}

		int global::System.Collections.IList.Add(object value)
		{
			return Add(value);
		}

		void global::System.Collections.IList.Clear()
		{
			EnsureList().Clear();
			json = null;
		}

		bool global::System.Collections.IList.Contains(object value)
		{
			return EnsureList().Contains(value);
		}

		int global::System.Collections.IList.IndexOf(object value)
		{
			return EnsureList().IndexOf(value);
		}

		void global::System.Collections.IList.Insert(int index, object value)
		{
			EnsureList().Insert(index, value);
			json = null;
		}

		void global::System.Collections.IList.Remove(object value)
		{
			EnsureList().Remove(value);
			json = null;
		}

		void global::System.Collections.IList.RemoveAt(int index)
		{
			EnsureList().RemoveAt(index);
			json = null;
		}

		IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
		{
			EnsureDictionary();
			return (IDictionaryEnumerator)(object)new OrderedDictionaryEnumerator(((global::System.Collections.Generic.IEnumerable<KeyValuePair<string, JsonData>>)object_list).GetEnumerator());
		}

		void IOrderedDictionary.Insert(int idx, object key, object value)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			string text = (string)key;
			JsonData jsonData2 = (this[text] = ToJsonData(value));
			KeyValuePair<string, JsonData> val = default(KeyValuePair<string, JsonData>);
			val._002Ector(text, jsonData2);
			object_list.Insert(idx, val);
		}

		void IOrderedDictionary.RemoveAt(int idx)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			EnsureDictionary();
			inst_object.Remove(object_list.get_Item(idx).get_Key());
			object_list.RemoveAt(idx);
		}

		private global::System.Collections.ICollection EnsureCollection()
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			if (type == JsonType.Array)
			{
				return (global::System.Collections.ICollection)inst_array;
			}
			if (type == JsonType.Object)
			{
				return (global::System.Collections.ICollection)inst_object;
			}
			throw new InvalidOperationException("The JsonData instance has to be initialized first");
		}

		private IDictionary EnsureDictionary()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected O, but got Unknown
			if (type != JsonType.Object)
			{
				if (type != 0)
				{
					throw new InvalidOperationException("Instance of JsonData is not a dictionary");
				}
				type = JsonType.Object;
				inst_object = (IDictionary<string, JsonData>)(object)new Dictionary<string, JsonData>();
				object_list = (global::System.Collections.Generic.IList<KeyValuePair<string, JsonData>>)new List<KeyValuePair<string, JsonData>>();
				return (IDictionary)inst_object;
			}
			return (IDictionary)inst_object;
		}

		private global::System.Collections.IList EnsureList()
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			if (type == JsonType.Array)
			{
				return (global::System.Collections.IList)inst_array;
			}
			if (type != 0)
			{
				throw new InvalidOperationException("Instance of JsonData is not a list");
			}
			type = JsonType.Array;
			inst_array = (global::System.Collections.Generic.IList<JsonData>)new List<JsonData>();
			return (global::System.Collections.IList)inst_array;
		}

		private JsonData ToJsonData(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is JsonData)
			{
				return (JsonData)obj;
			}
			return new JsonData(obj);
		}

		private static void WriteJson(IJsonWrapper obj, JsonWriter writer)
		{
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			if (obj == null)
			{
				writer.Write(null);
			}
			else if (obj.IsString)
			{
				writer.Write(obj.GetString());
			}
			else if (obj.IsBoolean)
			{
				writer.Write(obj.GetBoolean());
			}
			else if (obj.IsDouble)
			{
				writer.Write(obj.GetDouble());
			}
			else if (obj.IsInt)
			{
				writer.Write(obj.GetInt());
			}
			else if (obj.IsLong)
			{
				writer.Write(obj.GetLong());
			}
			else if (obj.IsArray)
			{
				writer.WriteArrayStart();
				global::System.Collections.IEnumerator enumerator = ((global::System.Collections.IEnumerable)obj).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object current = enumerator.get_Current();
						WriteJson((JsonData)current, writer);
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
				writer.WriteArrayEnd();
			}
			else
			{
				if (!obj.IsObject)
				{
					return;
				}
				writer.WriteObjectStart();
				IDictionaryEnumerator enumerator2 = ((IDictionary)obj).GetEnumerator();
				try
				{
					while (((global::System.Collections.IEnumerator)enumerator2).MoveNext())
					{
						DictionaryEntry val = (DictionaryEntry)((global::System.Collections.IEnumerator)enumerator2).get_Current();
						writer.WritePropertyName((string)((DictionaryEntry)(ref val)).get_Key());
						WriteJson((JsonData)((DictionaryEntry)(ref val)).get_Value(), writer);
					}
				}
				finally
				{
					global::System.IDisposable disposable2;
					if ((disposable2 = enumerator2 as global::System.IDisposable) != null)
					{
						disposable2.Dispose();
					}
				}
				writer.WriteObjectEnd();
			}
		}

		public int Add(object value)
		{
			JsonData jsonData = ToJsonData(value);
			json = null;
			return EnsureList().Add((object)jsonData);
		}

		public void Clear()
		{
			if (IsObject)
			{
				((IDictionary)this).Clear();
			}
			else if (IsArray)
			{
				((global::System.Collections.IList)this).Clear();
			}
		}

		public bool Equals(JsonData x)
		{
			if (x == null)
			{
				return false;
			}
			if (x.type != type)
			{
				return false;
			}
			switch (type)
			{
			case JsonType.None:
				return true;
			case JsonType.Object:
				return ((object)inst_object).Equals((object)x.inst_object);
			case JsonType.Array:
				return ((object)inst_array).Equals((object)x.inst_array);
			case JsonType.String:
				return inst_string.Equals(x.inst_string);
			case JsonType.Int:
				return inst_int.Equals(x.inst_int);
			case JsonType.Long:
				return inst_long.Equals(x.inst_long);
			case JsonType.Double:
				return inst_double.Equals(x.inst_double);
			case JsonType.Boolean:
				return inst_boolean.Equals(x.inst_boolean);
			default:
				return false;
			}
		}

		public JsonType GetJsonType()
		{
			return type;
		}

		public void SetJsonType(JsonType type)
		{
			if (this.type != type)
			{
				switch (type)
				{
				case JsonType.Object:
					inst_object = (IDictionary<string, JsonData>)(object)new Dictionary<string, JsonData>();
					object_list = (global::System.Collections.Generic.IList<KeyValuePair<string, JsonData>>)new List<KeyValuePair<string, JsonData>>();
					break;
				case JsonType.Array:
					inst_array = (global::System.Collections.Generic.IList<JsonData>)new List<JsonData>();
					break;
				case JsonType.String:
					inst_string = null;
					break;
				case JsonType.Int:
					inst_int = 0;
					break;
				case JsonType.Long:
					inst_long = 0L;
					break;
				case JsonType.Double:
					inst_double = 0.0;
					break;
				case JsonType.Boolean:
					inst_boolean = false;
					break;
				}
				this.type = type;
			}
		}

		public string ToJson()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			if (json != null)
			{
				return json;
			}
			StringWriter val = new StringWriter();
			JsonWriter jsonWriter = new JsonWriter((TextWriter)(object)val);
			jsonWriter.Validate = false;
			WriteJson(this, jsonWriter);
			json = ((object)val).ToString();
			return json;
		}

		public void ToJson(JsonWriter writer)
		{
			bool validate = writer.Validate;
			writer.Validate = false;
			WriteJson(this, writer);
			writer.Validate = validate;
		}

		public override string ToString()
		{
			switch (type)
			{
			case JsonType.Array:
				return "JsonData array";
			case JsonType.Boolean:
				return inst_boolean.ToString();
			case JsonType.Double:
				return inst_double.ToString();
			case JsonType.Int:
				return inst_int.ToString();
			case JsonType.Long:
				return inst_long.ToString();
			case JsonType.Object:
				return "JsonData object";
			case JsonType.String:
				return inst_string;
			default:
				return "Uninitialized JsonData";
			}
		}

		public static implicit operator JsonData(bool data)
		{
			return new JsonData(data);
		}

		public static implicit operator JsonData(double data)
		{
			return new JsonData(data);
		}

		public static implicit operator JsonData(int data)
		{
			return new JsonData(data);
		}

		public static implicit operator JsonData(long data)
		{
			return new JsonData(data);
		}

		public static implicit operator JsonData(string data)
		{
			return new JsonData(data);
		}

		public static explicit operator bool(JsonData data)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			if (data.type != JsonType.Boolean)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_boolean;
		}

		public static explicit operator double(JsonData data)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			if (data.type != JsonType.Double)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_double;
		}

		public static explicit operator int(JsonData data)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			if (data.type != JsonType.Int)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold an int");
			}
			return data.inst_int;
		}

		public static explicit operator long(JsonData data)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			if (data.type != JsonType.Long)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold an int");
			}
			return data.inst_long;
		}

		public static explicit operator string(JsonData data)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			if (data.type != JsonType.String)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a string");
			}
			return data.inst_string;
		}
	}
}
