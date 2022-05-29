using System;
using System.Reflection;

namespace SimpleJSON
{
	internal class JSONLazyCreator : JSONNode
	{
		private JSONNode m_Node;

		private string m_Key;

		public override JSONArray AsArray
		{
			get
			{
				JSONArray jSONArrays = new JSONArray();
				this.Set(jSONArrays);
				return jSONArrays;
			}
		}

		public override bool AsBool
		{
			get
			{
				this.Set(new JSONData(false));
				return false;
			}
			set
			{
				this.Set(new JSONData(value));
			}
		}

		public override double AsDouble
		{
			get
			{
				this.Set(new JSONData(0));
				return 0;
			}
			set
			{
				this.Set(new JSONData(value));
			}
		}

		public override float AsFloat
		{
			get
			{
				this.Set(new JSONData(0f));
				return 0f;
			}
			set
			{
				this.Set(new JSONData(value));
			}
		}

		public override int AsInt
		{
			get
			{
				this.Set(new JSONData(0));
				return 0;
			}
			set
			{
				this.Set(new JSONData(value));
			}
		}

		public override JSONClass AsObject
		{
			get
			{
				JSONClass jSONClasses = new JSONClass();
				this.Set(jSONClasses);
				return jSONClasses;
			}
		}

		public override JSONNode this[int aIndex]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				this.Set(new JSONArray()
				{
					value
				});
			}
		}

		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this, aKey);
			}
			set
			{
				this.Set(new JSONClass()
				{
					{ aKey, value }
				});
			}
		}

		public JSONLazyCreator(JSONNode aNode)
		{
			this.m_Node = aNode;
			this.m_Key = null;
		}

		public JSONLazyCreator(JSONNode aNode, string aKey)
		{
			this.m_Node = aNode;
			this.m_Key = aKey;
		}

		public override void Add(JSONNode aItem)
		{
			this.Set(new JSONArray()
			{
				aItem
			});
		}

		public override void Add(string aKey, JSONNode aItem)
		{
			this.Set(new JSONClass()
			{
				{ aKey, aItem }
			});
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return true;
			}
			return object.ReferenceEquals(this, obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static bool operator ==(JSONLazyCreator a, object b)
		{
			if (b == null)
			{
				return true;
			}
			return object.ReferenceEquals(a, b);
		}

		public static bool operator !=(JSONLazyCreator a, object b)
		{
			return !(a == b);
		}

		private void Set(JSONNode aVal)
		{
			if (this.m_Key != null)
			{
				this.m_Node.Add(this.m_Key, aVal);
			}
			else
			{
				this.m_Node.Add(aVal);
			}
			this.m_Node = null;
		}

		public override string ToString()
		{
			return string.Empty;
		}

		public override string ToString(string aPrefix)
		{
			return string.Empty;
		}
	}
}