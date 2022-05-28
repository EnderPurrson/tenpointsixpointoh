using System;
using System.Collections;
using System.Collections.Generic;

namespace ExitGames.Client.Photon
{
	public class DictionaryEntryEnumerator : global::System.Collections.Generic.IEnumerator<DictionaryEntry>, global::System.IDisposable, global::System.Collections.IEnumerator
	{
		private IDictionaryEnumerator enumerator;

		object Current
		{
			get
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				return (object)(DictionaryEntry)((global::System.Collections.IEnumerator)enumerator).get_Current();
			}
		}

		public DictionaryEntry Current
		{
			get
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				return (DictionaryEntry)((global::System.Collections.IEnumerator)enumerator).get_Current();
			}
		}

		public object Key
		{
			get
			{
				return enumerator.get_Key();
			}
		}

		public object Value
		{
			get
			{
				return enumerator.get_Value();
			}
		}

		public DictionaryEntry Entry
		{
			get
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				return enumerator.get_Entry();
			}
		}

		public DictionaryEntryEnumerator(IDictionaryEnumerator original)
		{
			enumerator = original;
		}

		public bool MoveNext()
		{
			return ((global::System.Collections.IEnumerator)enumerator).MoveNext();
		}

		public void Reset()
		{
			((global::System.Collections.IEnumerator)enumerator).Reset();
		}

		public void Dispose()
		{
			enumerator = null;
		}
	}
}
