using System.Reflection;
using System.Runtime.CompilerServices;

namespace FyberPlugin
{
	[Obfuscation(Exclude = true)]
	internal class BannerAdPayload
	{
		public int? Event
		{
			[CompilerGenerated]
			get
			{
				return _003CEvent_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CEvent_003Ek__BackingField = value;
			}
		}

		public string ErrorMessage
		{
			[CompilerGenerated]
			get
			{
				return _003CErrorMessage_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CErrorMessage_003Ek__BackingField = value;
			}
		}
	}
}
