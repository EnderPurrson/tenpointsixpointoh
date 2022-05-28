using System.Reflection;
using System.Runtime.CompilerServices;

namespace FyberPlugin
{
	[Obfuscation(Exclude = true)]
	internal class AdPayload
	{
		public bool AdStarted
		{
			[CompilerGenerated]
			get
			{
				return _003CAdStarted_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CAdStarted_003Ek__BackingField = value;
			}
		}

		public string Error
		{
			[CompilerGenerated]
			get
			{
				return _003CError_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CError_003Ek__BackingField = value;
			}
		}

		public string Status
		{
			[CompilerGenerated]
			get
			{
				return _003CStatus_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CStatus_003Ek__BackingField = value;
			}
		}
	}
}
