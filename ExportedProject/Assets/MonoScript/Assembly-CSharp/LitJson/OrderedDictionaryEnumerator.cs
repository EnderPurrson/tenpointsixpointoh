using System;
using System.Collections;
using System.Collections.Generic;

namespace LitJson
{
	internal class OrderedDictionaryEnumerator : IEnumerator, IDictionaryEnumerator
	{
		private IEnumerator<KeyValuePair<string, JsonData>> list_enumerator;

		public object Current
		{
			get
			{
				return this.Entry;
			}
		}

		public DictionaryEntry Entry
		{
			get
			{
				KeyValuePair<string, JsonData> current = this.list_enumerator.Current;
				return new DictionaryEntry(current.Key, current.Value);
			}
		}

		public object Key
		{
			get
			{
				return this.list_enumerator.Current.Key;
			}
		}

		public object Value
		{
			get
			{
				return this.list_enumerator.Current.Value;
			}
		}

		public OrderedDictionaryEnumerator(IEnumerator<KeyValuePair<string, JsonData>> enumerator)
		{
			this.list_enumerator = enumerator;
		}

		public bool MoveNext()
		{
			return this.list_enumerator.MoveNext();
		}

		public void Reset()
		{
			this.list_enumerator.Reset();
		}
	}
}