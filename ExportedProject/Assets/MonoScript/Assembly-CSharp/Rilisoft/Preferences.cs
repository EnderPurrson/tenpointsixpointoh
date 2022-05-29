using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Rilisoft
{
	internal abstract class Preferences : ICollection<KeyValuePair<string, string>>, IEnumerable, IDictionary<string, string>, IEnumerable<KeyValuePair<string, string>>
	{
		public abstract int Count
		{
			get;
		}

		public abstract bool IsReadOnly
		{
			get;
		}

		public string this[string key]
		{
			get
			{
				string str;
				if (!this.TryGetValue(key, out str))
				{
					throw new KeyNotFoundException();
				}
				return str;
			}
			set
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				this.AddCore(key, value);
			}
		}

		public abstract ICollection<string> Keys
		{
			get;
		}

		public abstract ICollection<string> Values
		{
			get;
		}

		protected Preferences()
		{
		}

		public void Add(string key, string value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this.AddCore(key, value);
		}

		public void Add(KeyValuePair<string, string> item)
		{
			this.AddCore(item.Key, item.Value);
		}

		protected abstract void AddCore(string key, string value);

		public abstract void Clear();

		public bool Contains(KeyValuePair<string, string> item)
		{
			string str;
			if (item.Key == null)
			{
				throw new ArgumentException("Key is null.", "item");
			}
			if (!this.TryGetValueCore(item.Key, out str))
			{
				return false;
			}
			return EqualityComparer<string>.Default.Equals(item.Value, str);
		}

		public bool ContainsKey(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return this.ContainsKeyCore(key);
		}

		protected abstract bool ContainsKeyCore(string key);

		public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			if (arrayIndex > (int)array.Length)
			{
				throw new ArgumentException("Index larger than largest valid index of array.");
			}
			if ((int)array.Length - arrayIndex < this.Count)
			{
				throw new ArgumentException("Destination array cannot hold the requested elements!");
			}
			this.CopyToCore(array, arrayIndex);
		}

		protected abstract void CopyToCore(KeyValuePair<string, string>[] array, int arrayIndex);

		public abstract IEnumerator<KeyValuePair<string, string>> GetEnumerator();

		public bool Remove(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return this.RemoveCore(key);
		}

		public bool Remove(KeyValuePair<string, string> item)
		{
			if (!this.Contains(item))
			{
				return false;
			}
			return this.Remove(item.Key);
		}

		protected abstract bool RemoveCore(string key);

		public abstract void Save();

		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public bool TryGetValue(string key, out string value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return this.TryGetValueCore(key, out value);
		}

		protected abstract bool TryGetValueCore(string key, out string value);
	}
}