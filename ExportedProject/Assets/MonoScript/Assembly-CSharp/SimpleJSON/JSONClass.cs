using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SimpleJSON
{
	public class JSONClass : JSONNode, IEnumerable
	{
		private Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>();

		public override IEnumerable<JSONNode> Childs
		{
			get
			{
				JSONClass.u003cu003ec__Iterator86 variable = null;
				return variable;
			}
		}

		public override int Count
		{
			get
			{
				return this.m_Dict.Count;
			}
		}

		public override JSONNode this[string aKey]
		{
			get
			{
				if (!this.m_Dict.ContainsKey(aKey))
				{
					return new JSONLazyCreator(this, aKey);
				}
				return this.m_Dict[aKey];
			}
			set
			{
				if (!this.m_Dict.ContainsKey(aKey))
				{
					this.m_Dict.Add(aKey, value);
				}
				else
				{
					this.m_Dict[aKey] = value;
				}
			}
		}

		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= this.m_Dict.Count)
				{
					return null;
				}
				return this.m_Dict.ElementAt<KeyValuePair<string, JSONNode>>(aIndex).Value;
			}
			set
			{
				if (aIndex < 0 || aIndex >= this.m_Dict.Count)
				{
					return;
				}
				string key = this.m_Dict.ElementAt<KeyValuePair<string, JSONNode>>(aIndex).Key;
				this.m_Dict[key] = value;
			}
		}

		public JSONClass()
		{
		}

		public override void Add(string aKey, JSONNode aItem)
		{
			if (string.IsNullOrEmpty(aKey))
			{
				this.m_Dict.Add(Guid.NewGuid().ToString(), aItem);
			}
			else if (!this.m_Dict.ContainsKey(aKey))
			{
				this.m_Dict.Add(aKey, aItem);
			}
			else
			{
				this.m_Dict[aKey] = aItem;
			}
		}

		[DebuggerHidden]
		public IEnumerator GetEnumerator()
		{
			JSONClass.u003cGetEnumeratoru003ec__Iterator87 variable = null;
			return variable;
		}

		public override JSONNode Remove(string aKey)
		{
			if (!this.m_Dict.ContainsKey(aKey))
			{
				return null;
			}
			JSONNode item = this.m_Dict[aKey];
			this.m_Dict.Remove(aKey);
			return item;
		}

		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= this.m_Dict.Count)
			{
				return null;
			}
			KeyValuePair<string, JSONNode> keyValuePair = this.m_Dict.ElementAt<KeyValuePair<string, JSONNode>>(aIndex);
			this.m_Dict.Remove(keyValuePair.Key);
			return keyValuePair.Value;
		}

		public override JSONNode Remove(JSONNode aNode)
		{
			JSONNode jSONNode;
			try
			{
				KeyValuePair<string, JSONNode> keyValuePair = (
					from k in this.m_Dict
					where k.Value == aNode
					select k).First<KeyValuePair<string, JSONNode>>();
				this.m_Dict.Remove(keyValuePair.Key);
				jSONNode = aNode;
			}
			catch
			{
				jSONNode = null;
			}
			return jSONNode;
		}

		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write((byte)2);
			aWriter.Write(this.m_Dict.Count);
			foreach (string key in this.m_Dict.Keys)
			{
				aWriter.Write(key);
				this.m_Dict[key].Serialize(aWriter);
			}
		}

		public override string ToString()
		{
			string str = "{";
			foreach (KeyValuePair<string, JSONNode> mDict in this.m_Dict)
			{
				if (str.Length > 2)
				{
					str = string.Concat(str, ", ");
				}
				string str1 = str;
				str = string.Concat(new string[] { str1, "\"", JSONNode.Escape(mDict.Key), "\":", mDict.Value.ToString() });
			}
			str = string.Concat(str, "}");
			return str;
		}

		public override string ToString(string aPrefix)
		{
			string str = "{ ";
			foreach (KeyValuePair<string, JSONNode> mDict in this.m_Dict)
			{
				if (str.Length > 3)
				{
					str = string.Concat(str, ", ");
				}
				str = string.Concat(str, "\n", aPrefix, "   ");
				string str1 = str;
				str = string.Concat(new string[] { str1, "\"", JSONNode.Escape(mDict.Key), "\" : ", mDict.Value.ToString(string.Concat(aPrefix, "   ")) });
			}
			str = string.Concat(str, "\n", aPrefix, "}");
			return str;
		}
	}
}