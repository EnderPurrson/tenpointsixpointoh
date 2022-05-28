using System.Reflection;
using System.Runtime.CompilerServices;

namespace FyberPlugin
{
	[Obfuscation(Exclude = true)]
	internal class NativeError
	{
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

		public string Id
		{
			[CompilerGenerated]
			get
			{
				return _003CId_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CId_003Ek__BackingField = value;
			}
		}
	}
}
