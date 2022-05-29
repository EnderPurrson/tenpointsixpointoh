using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SimpleJSON
{
	public class JSONArray : JSONNode, IEnumerable
	{
		private List<JSONNode> m_List = new List<JSONNode>();

		public override IEnumerable<JSONNode> Childs
		{
			get
			{
				JSONArray.u003cu003ec__Iterator84 variable = null;
				return variable;
			}
		}

		public override int Count
		{
			get
			{
				return this.m_List.Count;
			}
		}

		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					return new JSONLazyCreator(this);
				}
				return this.m_List[aIndex];
			}
			set
			{
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					this.m_List.Add(value);
				}
				else
				{
					this.m_List[aIndex] = value;
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
				this.m_List.Add(value);
			}
		}

		public JSONArray()
		{
		}

		public override void Add(string aKey, JSONNode aItem)
		{
			this.m_List.Add(aItem);
		}

		[DebuggerHidden]
		public IEnumerator GetEnumerator()
		{
			JSONArray.u003cGetEnumeratoru003ec__Iterator85 variable = null;
			return variable;
		}

		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= this.m_List.Count)
			{
				return null;
			}
			JSONNode item = this.m_List[aIndex];
			this.m_List.RemoveAt(aIndex);
			return item;
		}

		public override JSONNode Remove(JSONNode aNode)
		{
			this.m_List.Remove(aNode);
			return aNode;
		}

		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write((byte)1);
			aWriter.Write(this.m_List.Count);
			for (int i = 0; i < this.m_List.Count; i++)
			{
				this.m_List[i].Serialize(aWriter);
			}
		}

		public override string ToString()
		{
			string str = "[ ";
			foreach (JSONNode mList in this.m_List)
			{
				if (str.Length > 2)
				{
					str = string.Concat(str, ", ");
				}
				str = string.Concat(str, mList.ToString());
			}
			str = string.Concat(str, " ]");
			return str;
		}

		public override string ToString(string aPrefix)
		{
			string str = "[ ";
			foreach (JSONNode mList in this.m_List)
			{
				if (str.Length > 3)
				{
					str = string.Concat(str, ", ");
				}
				str = string.Concat(str, "\n", aPrefix, "   ");
				str = string.Concat(str, mList.ToString(string.Concat(aPrefix, "   ")));
			}
			str = string.Concat(str, "\n", aPrefix, "]");
			return str;
		}
	}
}