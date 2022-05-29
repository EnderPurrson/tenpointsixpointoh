using System;
using System.Collections;
using System.Collections.Specialized;

namespace LitJson
{
	public class JsonMockWrapper : IEnumerable, IList, IDictionary, ICollection, IOrderedDictionary, IJsonWrapper
	{
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

		int System.Collections.ICollection.Count
		{
			get
			{
				return 0;
			}
		}

		bool System.Collections.ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		object System.Collections.ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		bool System.Collections.IDictionary.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		bool System.Collections.IDictionary.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		object System.Collections.IDictionary.this[object key]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		ICollection System.Collections.IDictionary.Keys
		{
			get
			{
				return null;
			}
		}

		ICollection System.Collections.IDictionary.Values
		{
			get
			{
				return null;
			}
		}

		bool System.Collections.IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		bool System.Collections.IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		object System.Collections.IList.this[int index]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		object System.Collections.Specialized.IOrderedDictionary.this[int idx]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public JsonMockWrapper()
		{
		}

		public bool GetBoolean()
		{
			return false;
		}

		public double GetDouble()
		{
			return 0;
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
			return (long)0;
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

		void System.Collections.ICollection.CopyTo(Array array, int index)
		{
		}

		void System.Collections.IDictionary.Add(object k, object v)
		{
		}

		void System.Collections.IDictionary.Clear()
		{
		}

		bool System.Collections.IDictionary.Contains(object key)
		{
			return false;
		}

		IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator()
		{
			return null;
		}

		void System.Collections.IDictionary.Remove(object key)
		{
		}

		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return null;
		}

		int System.Collections.IList.Add(object value)
		{
			return 0;
		}

		void System.Collections.IList.Clear()
		{
		}

		bool System.Collections.IList.Contains(object value)
		{
			return false;
		}

		int System.Collections.IList.IndexOf(object value)
		{
			return -1;
		}

		void System.Collections.IList.Insert(int i, object v)
		{
		}

		void System.Collections.IList.Remove(object value)
		{
		}

		void System.Collections.IList.RemoveAt(int index)
		{
		}

		IDictionaryEnumerator System.Collections.Specialized.IOrderedDictionary.GetEnumerator()
		{
			return null;
		}

		void System.Collections.Specialized.IOrderedDictionary.Insert(int i, object k, object v)
		{
		}

		void System.Collections.Specialized.IOrderedDictionary.RemoveAt(int i)
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