using System.Collections.Generic;

namespace System.Runtime.CompilerServices
{
	public class ConditionalWeakTable<TKey, TValue> where TKey : class where TValue : class
	{
		private class Reference
		{
			private int hashCode;

			public WeakReference WeakReference
			{
				[CompilerGenerated]
				get
				{
					return _003CWeakReference_003Ek__BackingField;
				}
				[CompilerGenerated]
				private set
				{
					_003CWeakReference_003Ek__BackingField = value;
				}
			}

			public TKey Value
			{
				get
				{
					return (TKey)WeakReference.get_Target();
				}
			}

			public Reference(TKey obj)
			{
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Expected O, but got Unknown
				hashCode = ((object)obj).GetHashCode();
				WeakReference = new WeakReference((object)obj);
			}

			public override int GetHashCode()
			{
				return hashCode;
			}

			public override bool Equals(object obj)
			{
				Reference reference = obj as Reference;
				if (reference == null)
				{
					return false;
				}
				if (((object)reference).GetHashCode() != ((object)this).GetHashCode())
				{
					return false;
				}
				return reference.WeakReference.get_Target() == WeakReference.get_Target();
			}
		}

		public delegate TValue CreateValueCallback(TKey key);

		private IDictionary<Reference, TValue> data;

		public ConditionalWeakTable()
		{
			data = (IDictionary<Reference, TValue>)(object)new Dictionary<ConditionalWeakTable<Reference, TValue>.Reference, TValue>();
		}

		private unsafe void CleanUp()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<Reference> enumerator = new HashSet<ConditionalWeakTable<Reference, _003F>.Reference>((System.Collections.Generic.IEnumerable<ConditionalWeakTable<Reference, _003F>.Reference>)((IDictionary<ConditionalWeakTable<Reference, TValue>.Reference, TValue>)(object)data).get_Keys()).GetEnumerator();
			try
			{
				while (((Enumerator<ConditionalWeakTable<Reference, _003F>.Reference>*)(&enumerator))->MoveNext())
				{
					Reference current = ((Enumerator<ConditionalWeakTable<Reference, _003F>.Reference>*)(&enumerator))->get_Current();
					if (!current.WeakReference.get_IsAlive())
					{
						((IDictionary<ConditionalWeakTable<Reference, TValue>.Reference, TValue>)(object)data).Remove((ConditionalWeakTable<Reference, TValue>.Reference)(object)current);
					}
				}
			}
			finally
			{
				((System.IDisposable)enumerator).Dispose();
			}
		}

		public TValue GetValue(TKey key, CreateValueCallback createValueCallback)
		{
			CleanUp();
			Reference reference = new Reference(key);
			TValue result = default(TValue);
			if (((IDictionary<ConditionalWeakTable<Reference, TValue>.Reference, TValue>)(object)data).TryGetValue((ConditionalWeakTable<Reference, TValue>.Reference)(object)reference, ref result))
			{
				return result;
			}
			TValue result2;
			((IDictionary<ConditionalWeakTable<Reference, TValue>.Reference, TValue>)(object)data).set_Item((ConditionalWeakTable<Reference, TValue>.Reference)(object)reference, result2 = createValueCallback(key));
			return result2;
		}
	}
}
