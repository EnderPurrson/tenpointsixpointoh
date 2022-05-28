using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Prime31
{
	public sealed class P31Error
	{
		private bool _containsOnlyMessage;

		public string message
		{
			[CompilerGenerated]
			get
			{
				return _003Cmessage_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003Cmessage_003Ek__BackingField = value;
			}
		}

		public string domain
		{
			[CompilerGenerated]
			get
			{
				return _003Cdomain_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003Cdomain_003Ek__BackingField = value;
			}
		}

		public int code
		{
			[CompilerGenerated]
			get
			{
				return _003Ccode_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003Ccode_003Ek__BackingField = value;
			}
		}

		public Dictionary<string, object> userInfo
		{
			[CompilerGenerated]
			get
			{
				return _003CuserInfo_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003CuserInfo_003Ek__BackingField = value;
			}
		}

		public static P31Error errorFromJson(string json)
		{
			P31Error p31Error = new P31Error();
			if (!json.StartsWith("{"))
			{
				p31Error.message = json;
				p31Error._containsOnlyMessage = true;
				return p31Error;
			}
			Dictionary<string, object> val = Json.decode(json) as Dictionary<string, object>;
			if (val == null)
			{
				p31Error.message = "Unknown error";
			}
			else
			{
				p31Error.message = ((!val.ContainsKey("message")) ? null : val.get_Item("message").ToString());
				p31Error.domain = ((!val.ContainsKey("domain")) ? null : val.get_Item("domain").ToString());
				p31Error.code = ((!val.ContainsKey("code")) ? (-1) : int.Parse(val.get_Item("code").ToString()));
				p31Error.userInfo = ((!val.ContainsKey("userInfo")) ? null : (val.get_Item("userInfo") as Dictionary<string, object>));
			}
			return p31Error;
		}

		public override string ToString()
		{
			if (_containsOnlyMessage)
			{
				return string.Format("[P31Error]: {0}", (object)message);
			}
			try
			{
				string input = Json.encode(this);
				return string.Format("[P31Error]: {0}", (object)JsonFormatter.prettyPrint(input));
			}
			catch (global::System.Exception)
			{
				return string.Format("[P31Error]: {0}", (object)message);
			}
		}
	}
}
