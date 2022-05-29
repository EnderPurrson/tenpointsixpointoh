using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;

namespace LitJson
{
	public class JsonData : IEnumerable, IList, IDictionary, ICollection, IOrderedDictionary, IJsonWrapper, IEquatable<JsonData>
	{
		private IList<JsonData> inst_array;

		private bool inst_boolean;

		private double inst_double;

		private int inst_int;

		private long inst_long;

		private IDictionary<string, JsonData> inst_object;

		private string inst_string;

		private string json;

		private JsonType type;

		private IList<KeyValuePair<string, JsonData>> object_list;

		public int Count
		{
			get
			{
				return this.EnsureCollection().Count;
			}
		}

		public bool IsArray
		{
			get
			{
				return this.type == JsonType.Array;
			}
		}

		public bool IsBoolean
		{
			get
			{
				return this.type == JsonType.Boolean;
			}
		}

		public bool IsDouble
		{
			get
			{
				return this.type == JsonType.Double;
			}
		}

		public bool IsInt
		{
			get
			{
				return this.type == JsonType.Int;
			}
		}

		public bool IsLong
		{
			get
			{
				return this.type == JsonType.Long;
			}
		}

		public bool IsObject
		{
			get
			{
				return this.type == JsonType.Object;
			}
		}

		public bool IsString
		{
			get
			{
				return this.type == JsonType.String;
			}
		}

		public JsonData this[string prop_name]
		{
			get
			{
				this.EnsureDictionary();
				return this.inst_object[prop_name];
			}
			set
			{
				this.EnsureDictionary();
				KeyValuePair<string, JsonData> keyValuePair = new KeyValuePair<string, JsonData>(prop_name, value);
				if (!this.inst_object.ContainsKey(prop_name))
				{
					this.object_list.Add(keyValuePair);
				}
				else
				{
					int num = 0;
					while (num < this.object_list.Count)
					{
						if (this.object_list[num].Key != prop_name)
						{
							num++;
						}
						else
						{
							this.object_list[num] = keyValuePair;
							break;
						}
					}
				}
				this.inst_object[prop_name] = value;
				this.json = null;
			}
		}

		public JsonData this[int index]
		{
			get
			{
				this.EnsureCollection();
				if (this.type == JsonType.Array)
				{
					return this.inst_array[index];
				}
				return this.object_list[index].Value;
			}
			set
			{
				this.EnsureCollection();
				if (this.type != JsonType.Array)
				{
					KeyValuePair<string, JsonData> item = this.object_list[index];
					KeyValuePair<string, JsonData> keyValuePair = new KeyValuePair<string, JsonData>(item.Key, value);
					this.object_list[index] = keyValuePair;
					this.inst_object[item.Key] = value;
				}
				else
				{
					this.inst_array[index] = value;
				}
				this.json = null;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				this.EnsureDictionary();
				return this.inst_object.Keys;
			}
		}

		bool LitJson.IJsonWrapper.IsArray
		{
			get
			{
				return this.IsArray;
			}
		}

		bool LitJson.IJsonWrapper.IsBoolean
		{
			get
			{
				return this.IsBoolean;
			}
		}

		bool LitJson.IJsonWrapper.IsDouble
		{
			get
			{
				return this.IsDouble;
			}
		}

		bool LitJson.IJsonWrapper.IsInt
		{
			get
			{
				return this.IsInt;
			}
		}

		bool LitJson.IJsonWrapper.IsLong
		{
			get
			{
				return this.IsLong;
			}
		}

		bool LitJson.IJsonWrapper.IsObject
		{
			get
			{
				return this.IsObject;
			}
		}

		bool LitJson.IJsonWrapper.IsString
		{
			get
			{
				return this.IsString;
			}
		}

		int System.Collections.ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		bool System.Collections.ICollection.IsSynchronized
		{
			get
			{
				return this.EnsureCollection().IsSynchronized;
			}
		}

		object System.Collections.ICollection.SyncRoot
		{
			get
			{
				return this.EnsureCollection().SyncRoot;
			}
		}

		bool System.Collections.IDictionary.IsFixedSize
		{
			get
			{
				return this.EnsureDictionary().IsFixedSize;
			}
		}

		bool System.Collections.IDictionary.IsReadOnly
		{
			get
			{
				return this.EnsureDictionary().IsReadOnly;
			}
		}

		object System.Collections.IDictionary.this[object key]
		{
			get
			{
				return this.EnsureDictionary()[key];
			}
			set
			{
				if (!(key is string))
				{
					throw new ArgumentException("The key has to be a string");
				}
				JsonData jsonData = this.ToJsonData(value);
				this[(string)key] = jsonData;
			}
		}

		ICollection System.Collections.IDictionary.Keys
		{
			get
			{
				this.EnsureDictionary();
				IList<string> strs = new List<string>();
				IEnumerator<KeyValuePair<string, JsonData>> enumerator = this.object_list.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						strs.Add(enumerator.Current.Key);
					}
				}
				finally
				{
					if (enumerator == null)
					{
					}
					enumerator.Dispose();
				}
				return (ICollection)strs;
			}
		}

		ICollection System.Collections.IDictionary.Values
		{
			get
			{
				this.EnsureDictionary();
				IList<JsonData> jsonDatas = new List<JsonData>();
				IEnumerator<KeyValuePair<string, JsonData>> enumerator = this.object_list.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						jsonDatas.Add(enumerator.Current.Value);
					}
				}
				finally
				{
					if (enumerator == null)
					{
					}
					enumerator.Dispose();
				}
				return (ICollection)jsonDatas;
			}
		}

		bool System.Collections.IList.IsFixedSize
		{
			get
			{
				return this.EnsureList().IsFixedSize;
			}
		}

		bool System.Collections.IList.IsReadOnly
		{
			get
			{
				return this.EnsureList().IsReadOnly;
			}
		}

		object System.Collections.IList.this[int index]
		{
			get
			{
				return this.EnsureList()[index];
			}
			set
			{
				this.EnsureList();
				this[index] = this.ToJsonData(value);
			}
		}

		object System.Collections.Specialized.IOrderedDictionary.this[int idx]
		{
			get
			{
				this.EnsureDictionary();
				return this.object_list[idx].Value;
			}
			set
			{
				this.EnsureDictionary();
				JsonData jsonData = this.ToJsonData(value);
				KeyValuePair<string, JsonData> item = this.object_list[idx];
				this.inst_object[item.Key] = jsonData;
				KeyValuePair<string, JsonData> keyValuePair = new KeyValuePair<string, JsonData>(item.Key, jsonData);
				this.object_list[idx] = keyValuePair;
			}
		}

		public JsonData()
		{
		}

		public JsonData(bool boolean)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = boolean;
		}

		public JsonData(double number)
		{
			this.type = JsonType.Double;
			this.inst_double = number;
		}

		public JsonData(int number)
		{
			this.type = JsonType.Int;
			this.inst_int = number;
		}

		public JsonData(long number)
		{
			this.type = JsonType.Long;
			this.inst_long = number;
		}

		public JsonData(object obj)
		{
			if (obj is bool)
			{
				this.type = JsonType.Boolean;
				this.inst_boolean = (bool)obj;
				return;
			}
			if (obj is double)
			{
				this.type = JsonType.Double;
				this.inst_double = (double)obj;
				return;
			}
			if (obj is int)
			{
				this.type = JsonType.Int;
				this.inst_int = (int)obj;
				return;
			}
			if (obj is long)
			{
				this.type = JsonType.Long;
				this.inst_long = (long)obj;
				return;
			}
			if (!(obj is string))
			{
				throw new ArgumentException("Unable to wrap the given object with JsonData");
			}
			this.type = JsonType.String;
			this.inst_string = (string)obj;
		}

		public JsonData(string str)
		{
			this.type = JsonType.String;
			this.inst_string = str;
		}

		public int Add(object value)
		{
			JsonData jsonData = this.ToJsonData(value);
			this.json = null;
			return this.EnsureList().Add(jsonData);
		}

		public void Clear()
		{
			if (this.IsObject)
			{
				((IDictionary)this).Clear();
				return;
			}
			if (!this.IsArray)
			{
				return;
			}
			((IList)this).Clear();
		}

		private ICollection EnsureCollection()
		{
			if (this.type == JsonType.Array)
			{
				return (ICollection)this.inst_array;
			}
			if (this.type != JsonType.Object)
			{
				throw new InvalidOperationException("The JsonData instance has to be initialized first");
			}
			return (ICollection)this.inst_object;
		}

		private IDictionary EnsureDictionary()
		{
			if (this.type == JsonType.Object)
			{
				return (IDictionary)this.inst_object;
			}
			if (this.type != JsonType.None)
			{
				throw new InvalidOperationException("Instance of JsonData is not a dictionary");
			}
			this.type = JsonType.Object;
			this.inst_object = new Dictionary<string, JsonData>();
			this.object_list = new List<KeyValuePair<string, JsonData>>();
			return (IDictionary)this.inst_object;
		}

		private IList EnsureList()
		{
			if (this.type == JsonType.Array)
			{
				return (IList)this.inst_array;
			}
			if (this.type != JsonType.None)
			{
				throw new InvalidOperationException("Instance of JsonData is not a list");
			}
			this.type = JsonType.Array;
			this.inst_array = new List<JsonData>();
			return (IList)this.inst_array;
		}

		public bool Equals(JsonData x)
		{
			if (x == null)
			{
				return false;
			}
			if (x.type != this.type)
			{
				return false;
			}
			switch (this.type)
			{
				case JsonType.None:
				{
					return true;
				}
				case JsonType.Object:
				{
					return this.inst_object.Equals(x.inst_object);
				}
				case JsonType.Array:
				{
					return this.inst_array.Equals(x.inst_array);
				}
				case JsonType.String:
				{
					return this.inst_string.Equals(x.inst_string);
				}
				case JsonType.Int:
				{
					return this.inst_int.Equals(x.inst_int);
				}
				case JsonType.Long:
				{
					return this.inst_long.Equals(x.inst_long);
				}
				case JsonType.Double:
				{
					return this.inst_double.Equals(x.inst_double);
				}
				case JsonType.Boolean:
				{
					return this.inst_boolean.Equals(x.inst_boolean);
				}
			}
			return false;
		}

		public JsonType GetJsonType()
		{
			return this.type;
		}

		bool LitJson.IJsonWrapper.GetBoolean()
		{
			if (this.type != JsonType.Boolean)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a boolean");
			}
			return this.inst_boolean;
		}

		double LitJson.IJsonWrapper.GetDouble()
		{
			if (this.type != JsonType.Double)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a double");
			}
			return this.inst_double;
		}

		int LitJson.IJsonWrapper.GetInt()
		{
			if (this.type != JsonType.Int)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold an int");
			}
			return this.inst_int;
		}

		long LitJson.IJsonWrapper.GetLong()
		{
			if (this.type != JsonType.Long)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a long");
			}
			return this.inst_long;
		}

		string LitJson.IJsonWrapper.GetString()
		{
			if (this.type != JsonType.String)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a string");
			}
			return this.inst_string;
		}

		void LitJson.IJsonWrapper.SetBoolean(bool val)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = val;
			this.json = null;
		}

		void LitJson.IJsonWrapper.SetDouble(double val)
		{
			this.type = JsonType.Double;
			this.inst_double = val;
			this.json = null;
		}

		void LitJson.IJsonWrapper.SetInt(int val)
		{
			this.type = JsonType.Int;
			this.inst_int = val;
			this.json = null;
		}

		void LitJson.IJsonWrapper.SetLong(long val)
		{
			this.type = JsonType.Long;
			this.inst_long = val;
			this.json = null;
		}

		void LitJson.IJsonWrapper.SetString(string val)
		{
			this.type = JsonType.String;
			this.inst_string = val;
			this.json = null;
		}

		string LitJson.IJsonWrapper.ToJson()
		{
			return this.ToJson();
		}

		void LitJson.IJsonWrapper.ToJson(JsonWriter writer)
		{
			this.ToJson(writer);
		}

		public static explicit operator Boolean(JsonData data)
		{
			if (data.type != JsonType.Boolean)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_boolean;
		}

		public static explicit operator Double(JsonData data)
		{
			if (data.type != JsonType.Double)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_double;
		}

		public static explicit operator Int32(JsonData data)
		{
			if (data.type != JsonType.Int)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold an int");
			}
			return data.inst_int;
		}

		public static explicit operator Int64(JsonData data)
		{
			if (data.type != JsonType.Long)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold an int");
			}
			return data.inst_long;
		}

		public static explicit operator String(JsonData data)
		{
			if (data.type != JsonType.String)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a string");
			}
			return data.inst_string;
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

		public void SetJsonType(JsonType type)
		{
			if (this.type == type)
			{
				return;
			}
			switch (type)
			{
				case JsonType.Object:
				{
					this.inst_object = new Dictionary<string, JsonData>();
					this.object_list = new List<KeyValuePair<string, JsonData>>();
					break;
				}
				case JsonType.Array:
				{
					this.inst_array = new List<JsonData>();
					break;
				}
				case JsonType.String:
				{
					this.inst_string = null;
					break;
				}
				case JsonType.Int:
				{
					this.inst_int = 0;
					break;
				}
				case JsonType.Long:
				{
					this.inst_long = (long)0;
					break;
				}
				case JsonType.Double:
				{
					this.inst_double = 0;
					break;
				}
				case JsonType.Boolean:
				{
					this.inst_boolean = false;
					break;
				}
			}
			this.type = type;
		}

		void System.Collections.ICollection.CopyTo(Array array, int index)
		{
			this.EnsureCollection().CopyTo(array, index);
		}

		void System.Collections.IDictionary.Add(object key, object value)
		{
			JsonData jsonData = this.ToJsonData(value);
			this.EnsureDictionary().Add(key, jsonData);
			KeyValuePair<string, JsonData> keyValuePair = new KeyValuePair<string, JsonData>((string)key, jsonData);
			this.object_list.Add(keyValuePair);
			this.json = null;
		}

		void System.Collections.IDictionary.Clear()
		{
			this.EnsureDictionary().Clear();
			this.object_list.Clear();
			this.json = null;
		}

		bool System.Collections.IDictionary.Contains(object key)
		{
			return this.EnsureDictionary().Contains(key);
		}

		IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator()
		{
			return ((IOrderedDictionary)this).GetEnumerator();
		}

		void System.Collections.IDictionary.Remove(object key)
		{
			this.EnsureDictionary().Remove(key);
			int num = 0;
			while (num < this.object_list.Count)
			{
				if (this.object_list[num].Key != (string)key)
				{
					num++;
				}
				else
				{
					this.object_list.RemoveAt(num);
					break;
				}
			}
			this.json = null;
		}

		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.EnsureCollection().GetEnumerator();
		}

		int System.Collections.IList.Add(object value)
		{
			return this.Add(value);
		}

		void System.Collections.IList.Clear()
		{
			this.EnsureList().Clear();
			this.json = null;
		}

		bool System.Collections.IList.Contains(object value)
		{
			return this.EnsureList().Contains(value);
		}

		int System.Collections.IList.IndexOf(object value)
		{
			return this.EnsureList().IndexOf(value);
		}

		void System.Collections.IList.Insert(int index, object value)
		{
			this.EnsureList().Insert(index, value);
			this.json = null;
		}

		void System.Collections.IList.Remove(object value)
		{
			this.EnsureList().Remove(value);
			this.json = null;
		}

		void System.Collections.IList.RemoveAt(int index)
		{
			this.EnsureList().RemoveAt(index);
			this.json = null;
		}

		IDictionaryEnumerator System.Collections.Specialized.IOrderedDictionary.GetEnumerator()
		{
			this.EnsureDictionary();
			return new OrderedDictionaryEnumerator(this.object_list.GetEnumerator());
		}

		void System.Collections.Specialized.IOrderedDictionary.Insert(int idx, object key, object value)
		{
			string str = (string)key;
			JsonData jsonData = this.ToJsonData(value);
			this[str] = jsonData;
			KeyValuePair<string, JsonData> keyValuePair = new KeyValuePair<string, JsonData>(str, jsonData);
			this.object_list.Insert(idx, keyValuePair);
		}

		void System.Collections.Specialized.IOrderedDictionary.RemoveAt(int idx)
		{
			this.EnsureDictionary();
			IDictionary<string, JsonData> instObject = this.inst_object;
			KeyValuePair<string, JsonData> item = this.object_list[idx];
			instObject.Remove(item.Key);
			this.object_list.RemoveAt(idx);
		}

		public string ToJson()
		{
			if (this.json != null)
			{
				return this.json;
			}
			StringWriter stringWriter = new StringWriter();
			JsonWriter jsonWriter = new JsonWriter(stringWriter)
			{
				Validate = false
			};
			JsonData.WriteJson(this, jsonWriter);
			this.json = stringWriter.ToString();
			return this.json;
		}

		public void ToJson(JsonWriter writer)
		{
			bool validate = writer.Validate;
			writer.Validate = false;
			JsonData.WriteJson(this, writer);
			writer.Validate = validate;
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

		public override string ToString()
		{
			switch (this.type)
			{
				case JsonType.Object:
				{
					return "JsonData object";
				}
				case JsonType.Array:
				{
					return "JsonData array";
				}
				case JsonType.String:
				{
					return this.inst_string;
				}
				case JsonType.Int:
				{
					return this.inst_int.ToString();
				}
				case JsonType.Long:
				{
					return this.inst_long.ToString();
				}
				case JsonType.Double:
				{
					return this.inst_double.ToString();
				}
				case JsonType.Boolean:
				{
					return this.inst_boolean.ToString();
				}
			}
			return "Uninitialized JsonData";
		}

		private static void WriteJson(IJsonWrapper obj, JsonWriter writer)
		{
			if (obj == null)
			{
				writer.Write(null);
				return;
			}
			if (obj.IsString)
			{
				writer.Write(obj.GetString());
				return;
			}
			if (obj.IsBoolean)
			{
				writer.Write(obj.GetBoolean());
				return;
			}
			if (obj.IsDouble)
			{
				writer.Write(obj.GetDouble());
				return;
			}
			if (obj.IsInt)
			{
				writer.Write(obj.GetInt());
				return;
			}
			if (obj.IsLong)
			{
				writer.Write(obj.GetLong());
				return;
			}
			if (obj.IsArray)
			{
				writer.WriteArrayStart();
				IEnumerator enumerator = obj.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						JsonData.WriteJson((JsonData)enumerator.Current, writer);
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
				writer.WriteArrayEnd();
				return;
			}
			if (!obj.IsObject)
			{
				return;
			}
			writer.WriteObjectStart();
			IDictionaryEnumerator dictionaryEnumerator = obj.GetEnumerator();
			try
			{
				while (dictionaryEnumerator.MoveNext())
				{
					DictionaryEntry current = (DictionaryEntry)dictionaryEnumerator.Current;
					writer.WritePropertyName((string)current.Key);
					JsonData.WriteJson((JsonData)current.Value, writer);
				}
			}
			finally
			{
				IDisposable disposable1 = dictionaryEnumerator as IDisposable;
				if (disposable1 == null)
				{
				}
				disposable1.Dispose();
			}
			writer.WriteObjectEnd();
		}
	}
}