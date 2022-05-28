using System;
using System.Collections;
using System.Collections.Specialized;

namespace FyberPlugin.LitJson
{
	public class JsonMockWrapper : IJsonWrapper, global::System.Collections.IList, global::System.Collections.ICollection, IDictionary, global::System.Collections.IEnumerable, IOrderedDictionary
	{
		bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		object Item
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		int Count
		{
			get
			{
				return 0;
			}
		}

		bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		object SyncRoot
		{
			get
			{
				return null;
			}
		}

		bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		global::System.Collections.ICollection Keys
		{
			get
			{
				return null;
			}
		}

		global::System.Collections.ICollection Values
		{
			get
			{
				return null;
			}
		}

		object Item
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		object Item
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public bool IsArray
		{
			get
			{
				return false;
			}
		}

		public bool IsBoolean
		{
			get
			{
				return false;
			}
		}

		public bool IsDouble
		{
			get
			{
				return false;
			}
		}

		public bool IsInt
		{
			get
			{
				return false;
			}
		}

		public bool IsLong
		{
			get
			{
				return false;
			}
		}

		public bool IsObject
		{
			get
			{
				return false;
			}
		}

		public bool IsString
		{
			get
			{
				return false;
			}
		}

		int global::System.Collections.IList.Add(object value)
		{
			return 0;
		}

		void global::System.Collections.IList.Clear()
		{
		}

		bool global::System.Collections.IList.Contains(object value)
		{
			return false;
		}

		int global::System.Collections.IList.IndexOf(object value)
		{
			return -1;
		}

		void global::System.Collections.IList.Insert(int i, object v)
		{
		}

		void global::System.Collections.IList.Remove(object value)
		{
		}

		void global::System.Collections.IList.RemoveAt(int index)
		{
		}

		void global::System.Collections.ICollection.CopyTo(global::System.Array array, int index)
		{
		}

		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return null;
		}

		void IDictionary.Add(object k, object v)
		{
		}

		void IDictionary.Clear()
		{
		}

		bool IDictionary.Contains(object key)
		{
			return false;
		}

		void IDictionary.Remove(object key)
		{
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return null;
		}

		IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
		{
			return null;
		}

		void IOrderedDictionary.Insert(int i, object k, object v)
		{
		}

		void IOrderedDictionary.RemoveAt(int i)
		{
		}

		public bool GetBoolean()
		{
			return false;
		}

		public double GetDouble()
		{
			return 0.0;
		}

		public int GetInt()
		{
			return 0;
		}

		public JsonType GetJsonType()
		{
			return JsonType.None;
		}

		public long GetLong()
		{
			return 0L;
		}

		public string GetString()
		{
			return string.Empty;
		}

		public void SetBoolean(bool val)
		{
		}

		public void SetDouble(double val)
		{
		}

		public void SetInt(int val)
		{
		}

		public void SetJsonType(JsonType type)
		{
		}

		public void SetLong(long val)
		{
		}

		public void SetString(string val)
		{
		}

		public string ToJson()
		{
			return string.Empty;
		}

		public void ToJson(JsonWriter writer)
		{
		}
	}
}
