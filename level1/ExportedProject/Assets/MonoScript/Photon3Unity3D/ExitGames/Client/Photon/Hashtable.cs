using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ExitGames.Client.Photon
{
	[DefaultMember("Item")]
	public class Hashtable : Dictionary<object, object>
	{
		public object this[object key]
		{
			get
			{
				object result = null;
				base.TryGetValue(key, ref result);
				return result;
			}
			set
			{
				base.set_Item(key, value);
			}
		}

		public Hashtable()
		{
		}

		public Hashtable(int x)
			: base(x)
		{
		}

		public global::System.Collections.Generic.IEnumerator<DictionaryEntry> GetEnumerator()
		{
			return new DictionaryEntryEnumerator(((IDictionary)this).GetEnumerator());
		}

		public override string ToString()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			List<string> val = new List<string>();
			Enumerator<object, object> enumerator = base.get_Keys().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.get_Current();
					if (current == null || this[current] == null)
					{
						val.Add(string.Concat(current, (object)"=", this[current]));
						continue;
					}
					val.Add(string.Concat(new object[8]
					{
						"(",
						current.GetType(),
						")",
						current,
						"=(",
						this[current].GetType(),
						")",
						this[current]
					}));
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			return string.Join(", ", val.ToArray());
		}

		public object Clone()
		{
			return new Dictionary<object, object>((IDictionary<object, object>)(object)this);
		}
	}
}
