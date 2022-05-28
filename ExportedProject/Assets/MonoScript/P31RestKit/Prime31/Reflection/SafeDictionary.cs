using System.Collections.Generic;
using System.Reflection;

namespace Prime31.Reflection
{
	[DefaultMember("Item")]
	public class SafeDictionary<TKey, TValue>
	{
		private readonly object _padlock = new object();

		private readonly Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

		public TValue this[TKey key]
		{
			get
			{
				return _dictionary.get_Item(key);
			}
		}

		public bool tryGetValue(TKey key, out TValue value)
		{
			return _dictionary.TryGetValue(key, ref value);
		}

		public global::System.Collections.Generic.IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return ((global::System.Collections.Generic.IEnumerable<KeyValuePair<KeyValuePair<TKey, TValue>, _003F>>)_dictionary).GetEnumerator();
		}

		public void add(TKey key, TValue value)
		{
			lock (_padlock)
			{
				if (!_dictionary.ContainsKey(key))
				{
					_dictionary.Add(key, value);
				}
			}
		}
	}
}
