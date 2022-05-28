using System.Collections;
using System.Collections.Generic;

namespace FyberPlugin.LitJson
{
	internal class OrderedDictionaryEnumerator : global::System.Collections.IEnumerator, IDictionaryEnumerator
	{
		private global::System.Collections.Generic.IEnumerator<KeyValuePair<string, JsonData>> list_enumerator;

		public object Current
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return Entry;
			}
		}

		public DictionaryEntry Entry
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				KeyValuePair<string, JsonData> current = list_enumerator.get_Current();
				return new DictionaryEntry((object)current.get_Key(), (object)current.get_Value());
			}
		}

		public object Key
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				return list_enumerator.get_Current().get_Key();
			}
		}

		public object Value
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				return list_enumerator.get_Current().get_Value();
			}
		}

		public OrderedDictionaryEnumerator(global::System.Collections.Generic.IEnumerator<KeyValuePair<string, JsonData>> enumerator)
		{
			list_enumerator = enumerator;
		}

		public bool MoveNext()
		{
			return ((global::System.Collections.IEnumerator)list_enumerator).MoveNext();
		}

		public void Reset()
		{
			((global::System.Collections.IEnumerator)list_enumerator).Reset();
		}
	}
}
