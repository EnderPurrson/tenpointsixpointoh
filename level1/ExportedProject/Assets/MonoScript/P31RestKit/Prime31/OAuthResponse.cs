using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Prime31
{
	[DefaultMember("Item")]
	public class OAuthResponse
	{
		private Dictionary<string, string> _params;

		public string responseText
		{
			[CompilerGenerated]
			get
			{
				return _003CresponseText_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CresponseText_003Ek__BackingField = value;
			}
		}

		public string this[string ix]
		{
			get
			{
				return _params.get_Item(ix);
			}
		}

		public OAuthResponse(string alltext)
		{
			responseText = alltext;
			_params = new Dictionary<string, string>();
			string[] array = alltext.Split(new char[1] { '&' });
			string[] array2 = array;
			foreach (string text in array2)
			{
				string[] array3 = text.Split(new char[1] { '=' });
				_params.Add(array3[0], array3[1]);
			}
		}
	}
}
