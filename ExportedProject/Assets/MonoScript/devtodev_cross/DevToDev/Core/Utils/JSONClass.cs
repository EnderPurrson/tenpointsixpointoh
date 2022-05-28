using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DevToDev.Core.Utils
{
	[DefaultMember("Item")]
	public class JSONClass : JSONNode, global::System.Collections.IEnumerable
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass2
		{
			public JSONNode aNode;

			public bool _003CRemove_003Eb__0(KeyValuePair<string, JSONNode> k)
			{
				return k.get_Value() == aNode;
			}
		}

		private Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>();

		public override JSONNode this[string aKey]
		{
			get
			{
				if (m_Dict.ContainsKey(aKey))
				{
					return m_Dict.get_Item(aKey);
				}
				return new JSONLazyCreator(this, aKey);
			}
			set
			{
				if (m_Dict.ContainsKey(aKey))
				{
					m_Dict.set_Item(aKey, value);
				}
				else
				{
					m_Dict.Add(aKey, value);
				}
			}
		}

		public override JSONNode this[int aIndex]
		{
			get
			{
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				if (aIndex < 0 || aIndex >= m_Dict.get_Count())
				{
					return null;
				}
				return Enumerable.ElementAt<KeyValuePair<string, JSONNode>>((global::System.Collections.Generic.IEnumerable<KeyValuePair<string, JSONNode>>)m_Dict, aIndex).get_Value();
			}
			set
			{
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				if (aIndex >= 0 && aIndex < m_Dict.get_Count())
				{
					string key = Enumerable.ElementAt<KeyValuePair<string, JSONNode>>((global::System.Collections.Generic.IEnumerable<KeyValuePair<string, JSONNode>>)m_Dict, aIndex).get_Key();
					m_Dict.set_Item(key, value);
				}
			}
		}

		public override int Count
		{
			get
			{
				return m_Dict.get_Count();
			}
		}

		public override global::System.Collections.Generic.IEnumerable<JSONNode> Children
		{
			get
			{
				Enumerator<string, JSONNode> enumerator = m_Dict.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, JSONNode> N = enumerator.get_Current();
						KeyValuePair<string, JSONNode> val = N;
						yield return val.get_Value();
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator).Dispose();
				}
			}
		}

		public override void Add(string aKey, JSONNode aItem)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			if (aKey != null)
			{
				if (m_Dict.ContainsKey(aKey))
				{
					m_Dict.set_Item(aKey, aItem);
				}
				else
				{
					m_Dict.Add(aKey, aItem);
				}
			}
			else
			{
				Dictionary<string, JSONNode> dict = m_Dict;
				Guid val = Guid.NewGuid();
				dict.Add(((object)(Guid)(ref val)).ToString(), aItem);
			}
		}

		public override JSONNode Remove(string aKey)
		{
			if (!m_Dict.ContainsKey(aKey))
			{
				return null;
			}
			JSONNode result = m_Dict.get_Item(aKey);
			m_Dict.Remove(aKey);
			return result;
		}

		public override JSONNode Remove(int aIndex)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			if (aIndex < 0 || aIndex >= m_Dict.get_Count())
			{
				return null;
			}
			KeyValuePair<string, JSONNode> val = Enumerable.ElementAt<KeyValuePair<string, JSONNode>>((global::System.Collections.Generic.IEnumerable<KeyValuePair<string, JSONNode>>)m_Dict, aIndex);
			m_Dict.Remove(val.get_Key());
			return val.get_Value();
		}

		public override JSONNode Remove(JSONNode aNode)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			Func<KeyValuePair<string, JSONNode>, bool> val = null;
			_003C_003Ec__DisplayClass2 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass2();
			_003C_003Ec__DisplayClass.aNode = aNode;
			try
			{
				Dictionary<string, JSONNode> dict = m_Dict;
				if (val == null)
				{
					val = _003C_003Ec__DisplayClass._003CRemove_003Eb__0;
				}
				KeyValuePair<string, JSONNode> val2 = Enumerable.First<KeyValuePair<string, JSONNode>>(Enumerable.Where<KeyValuePair<string, JSONNode>>((global::System.Collections.Generic.IEnumerable<KeyValuePair<string, JSONNode>>)dict, val));
				m_Dict.Remove(val2.get_Key());
				return _003C_003Ec__DisplayClass.aNode;
			}
			catch
			{
				return null;
			}
		}

		public global::System.Collections.IEnumerator GetEnumerator()
		{
			Enumerator<string, JSONNode> enumerator = m_Dict.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, JSONNode> N = enumerator.get_Current();
					yield return N;
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		public override string ToString()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			string text = "{";
			Enumerator<string, JSONNode> enumerator = m_Dict.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, JSONNode> current = enumerator.get_Current();
					if (text.get_Length() > 2)
					{
						text += ", ";
					}
					string text2 = text;
					text = string.Concat(new string[5]
					{
						text2,
						"\"",
						JSONNode.Escape(current.get_Key()),
						"\":",
						((object)current.get_Value()).ToString()
					});
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			return text + "}";
		}

		public override string ToString(string aPrefix)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			string text = "{";
			Enumerator<string, JSONNode> enumerator = m_Dict.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, JSONNode> current = enumerator.get_Current();
					if (text.get_Length() > 3)
					{
						text += ",";
					}
					text += aPrefix;
					string text2 = text;
					text = string.Concat(new string[5]
					{
						text2,
						"\"",
						JSONNode.Escape(current.get_Key()),
						"\":",
						current.get_Value().ToString(aPrefix)
					});
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			return text + aPrefix + "}";
		}

		public override string ToJSON(int prefix)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			string text = "{";
			Enumerator<string, JSONNode> enumerator = m_Dict.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, JSONNode> current = enumerator.get_Current();
					if (text.get_Length() > 3)
					{
						text += ",";
					}
					text += string.Format("\"{0}\":{1}", (object)current.get_Key(), (object)current.get_Value().ToJSON(prefix + 1));
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			return text + "}";
		}

		public override void Serialize(BinaryWriter aWriter)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			aWriter.Write((byte)2);
			aWriter.Write(m_Dict.get_Count());
			Enumerator<string, JSONNode> enumerator = m_Dict.get_Keys().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.get_Current();
					aWriter.Write(current);
					m_Dict.get_Item(current).Serialize(aWriter);
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}
	}
}
