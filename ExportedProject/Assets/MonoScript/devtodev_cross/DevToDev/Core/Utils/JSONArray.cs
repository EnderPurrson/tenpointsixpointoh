using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DevToDev.Core.Utils
{
	[DefaultMember("Item")]
	public class JSONArray : JSONNode, global::System.Collections.IEnumerable
	{
		private List<JSONNode> m_List = new List<JSONNode>();

		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= m_List.get_Count())
				{
					return new JSONLazyCreator(this);
				}
				return m_List.get_Item(aIndex);
			}
			set
			{
				if (aIndex < 0 || aIndex >= m_List.get_Count())
				{
					m_List.Add(value);
				}
				else
				{
					m_List.set_Item(aIndex, value);
				}
			}
		}

		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				m_List.Add(value);
			}
		}

		public override int Count
		{
			get
			{
				return m_List.get_Count();
			}
		}

		public override global::System.Collections.Generic.IEnumerable<JSONNode> Children
		{
			get
			{
				Enumerator<JSONNode> enumerator = m_List.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						yield return enumerator.get_Current();
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
			m_List.Add(aItem);
		}

		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= m_List.get_Count())
			{
				return null;
			}
			JSONNode result = m_List.get_Item(aIndex);
			m_List.RemoveAt(aIndex);
			return result;
		}

		public override JSONNode Remove(JSONNode aNode)
		{
			m_List.Remove(aNode);
			return aNode;
		}

		public global::System.Collections.IEnumerator GetEnumerator()
		{
			Enumerator<JSONNode> enumerator = m_List.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					yield return enumerator.get_Current();
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
			string text = "[";
			Enumerator<JSONNode> enumerator = m_List.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					JSONNode current = enumerator.get_Current();
					if (text.get_Length() > 2)
					{
						text += ",";
					}
					text += ((object)current).ToString();
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			return text + "]";
		}

		public override string ToString(string aPrefix)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			string text = "[";
			Enumerator<JSONNode> enumerator = m_List.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					JSONNode current = enumerator.get_Current();
					if (text.get_Length() > 3)
					{
						text += ",";
					}
					text += aPrefix;
					text += current.ToString(aPrefix);
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			return text + aPrefix + "]";
		}

		public override string ToJSON(int prefix)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			string text = "[";
			int num = 0;
			Enumerator<JSONNode> enumerator = m_List.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					JSONNode current = enumerator.get_Current();
					if (num++ > 0)
					{
						text += ",";
					}
					text += current.ToJSON(prefix + 1);
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			return text + "]";
		}

		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write((byte)1);
			aWriter.Write(m_List.get_Count());
			for (int i = 0; i < m_List.get_Count(); i++)
			{
				m_List.get_Item(i).Serialize(aWriter);
			}
		}
	}
}
